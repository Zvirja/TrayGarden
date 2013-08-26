﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.Helpers.ThreadSwitcher;
using TrayGarden.Resources;
using TrayGarden.RuntimeSettings;
using TrayGarden.TypesHatcher;
using TrayGarden.UI.Common.Commands;
using TrayGarden.Helpers;

namespace TrayGarden.UI.WindowWithReturn
{
  /// <summary>
  /// ViewModel for Window with back
  /// </summary>
  public class WindowWithBackVM : INotifyPropertyChanged, IDisposable
  {

    public delegate bool WindowSizePozitionInfoRetriever(
        out double top, out double left, out double width, out double height, out bool maximized);



    protected int TimesEnterBulkUpdate { get; set; }

    //protected static bool CommandsEnabledByDefault = true;

    protected delegate void GoAheadWithBackInvokable(WindowStepState newState);

    protected static event GoAheadWithBackInvokable GoAheadTargets;

    public static void GoAheadWithBackIfPossible(WindowStepState newState)
    {
      if (GoAheadTargets != null)
        GoAheadTargets(newState);
    }

    protected static double GetDoubleValueOrZero(string str)
    {
      double result;
      return double.TryParse(str, out result) ? result : 0;
    }


    protected ISettingsBox SelfSettingsBox { get; set; }

    protected RelayCommand _backCommand;
    protected ObservableCollection<ActionCommandVM> _helpActions;
    protected string _copyrightTitle;
    private Stack<WindowStepState> _steps;


    protected virtual string BackToTitleInternal { get; set; }

    protected virtual bool CanBack
    {
      get { return _backCommand.CanExecute(null); }
      set { _backCommand.CanExecuteMaster = value; }
    }

    protected virtual Stack<WindowStepState> Steps
    {
      get { return _steps; }
      set { _steps = value; }
    }

    protected virtual WindowStepState CurrentState
    {
      get { return Steps.Count > 0 ? Steps.Peek() : WindowStepState.EmptyState; }
    }


    public WindowWithBackVM()
    {

      _backCommand = new RelayCommand(BackExecute, false);
      _helpActions = new ObservableCollection<ActionCommandVM>();
      _helpActions.CollectionChanged += HelpActions_CollectionChanged;
      _copyrightTitle = "Zvirja Inc (c)";
      _steps = new Stack<WindowStepState>();
      SelfSettingsBox =
          HatcherGuide<IRuntimeSettingsManager>.Instance.SystemSettings.GetSubBox("WindowWithBackVMBase");

      GoAheadTargets += GoAheadWithBack;

    }



    public event PropertyChangedEventHandler PropertyChanged;

    public virtual string GlobalTitle
    {
      get { return CurrentState.GlobalTitle; }
    }

    public virtual string Header
    {
      get { return CurrentState.Header; }
    }

    public virtual object ContentVM
    {
      get { return CurrentState.ContentVM; }
      set
      {
        if (CurrentState.ContentVM == value) return;
        CurrentState.ContentVM = value;
        OnPropertyChanged("ContentVM");
      }
    }

    [UsedImplicitly]
    public virtual string CopyrightTitle
    {
      get { return _copyrightTitle; }
      set
      {
        if (value == _copyrightTitle) return;
        _copyrightTitle = value;
        OnPropertyChanged("CopyrightTitle");
      }
    }

    //--

    public virtual string BackToTitle
    {
      get { return "Back to " + BackToTitleInternal; }
      set
      {
        if (value == BackToTitleInternal) return;
        BackToTitleInternal = value;
        OnPropertyChanged("BackToTitle");
      }
    }


    [UsedImplicitly]
    public virtual RelayCommand BackCommand
    {
      get { return _backCommand; }
    }

    //--


    [UsedImplicitly]
    public virtual string ExtraActionTitle
    {
      get { return CurrentState.SuperAction.Title; }
    }

    [UsedImplicitly]
    public virtual ICommand ExtraActionCommand
    {
      get { return CurrentState.SuperAction.Command; }
    }

    //---
    /// <summary>
    /// For binding. Returns aggregated collection. Don't use it to assign actions.
    /// </summary>
    public IEnumerable<ActionCommandVM> HelpActions
    {
      get
      {
        var aggregated = new List<ActionCommandVM>(_helpActions);
        Assert.IsNotNull(CurrentState.StateSpecificHelpActions, "StateSpecificHelpActions can't be null");
        aggregated.AddRange(CurrentState.StateSpecificHelpActions);
        return aggregated;
      }
    }


    #region Size and position
    public WindowSizePozitionInfoRetriever SizePozitionProvider { get; set; }

    public double Top
    {
      get { return GetDoubleValueOrZero(SelfSettingsBox.GetString("WindowTop", null)); }
      protected set { SelfSettingsBox.SetString("WindowTop", value.ToString(CultureInfo.InvariantCulture)); }
    }

    public double Left
    {
      get { return GetDoubleValueOrZero(SelfSettingsBox.GetString("WindowLeft", null)); }
      protected set { SelfSettingsBox.SetString("WindowLeft", value.ToString(CultureInfo.InvariantCulture)); }
    }

    public double Width
    {
      get { return GetDoubleValueOrZero(SelfSettingsBox.GetString("WindowWidth", null)); }
      protected set { SelfSettingsBox.SetString("WindowWidth", value.ToString(CultureInfo.InvariantCulture)); }
    }

    public double Height
    {
      get { return GetDoubleValueOrZero(SelfSettingsBox.GetString("WindowHeight", null)); }
      protected set { SelfSettingsBox.SetString("WindowHeight", value.ToString(CultureInfo.InvariantCulture)); }
    }
    public bool Maximized
    {
      get { return SelfSettingsBox.GetBool("WindowMaximized", false); }
      protected set { SelfSettingsBox.SetBool("WindowMaximized", value); }
    }

    public bool SizePropertiesAreValid
    {
      get { return SelfSettingsBox.GetBool("WindowPropertiesAreValid", false); }
      protected set { SelfSettingsBox.SetBool("WindowPropertiesAreValid", value); }
    }

    #endregion

    /// <summary>
    /// Get actions, which are related to whole VM, not to current state.
    /// </summary>
    public ObservableCollection<ActionCommandVM> SelfHelpActions
    {
      get { return _helpActions; }
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }


    public virtual void ReplaceInitialState(WindowStepState newHomeState)
    {
      ClearStepsStackWithDisposing();
      Steps.Push(newHomeState);
      CanBack = false;
      NotifyPublicVisibleChanged();
    }

    public virtual void PrepareToShow()
    {
      StackRawSwitcher<BulkUpdateState>.Enter(BulkUpdateState.Enabled);
      TimesEnterBulkUpdate++;
      _helpActions.Clear();
      foreach (ActionCommandVM helpAction in GetHelpActions())
      {
        _helpActions.Add(helpAction);
      }
    }


    public virtual void Dispose()
    {
      if (TimesEnterBulkUpdate == 1)
      {
        StackRawSwitcher<BulkUpdateState>.Exit();
        HatcherGuide<IRuntimeSettingsManager>.Instance.SaveNow(true);
        TimesEnterBulkUpdate--;
      }
      else
      {
        Log.Warn(
            "Invalid value of TimesEnterBulkUpdate setting: {0}. Should be 1".FormatWith(TimesEnterBulkUpdate +
                                                                                         1), this);
      }
      GoAheadTargets -= GoAheadWithBack;
      ClearStepsStackWithDisposing();
    }

    protected virtual void ClearStepsStackWithDisposing()
    {
      while (Steps.Count > 0)
      {
        WindowStepState currentStep = Steps.Pop();
        var currentStepContentVM = currentStep.ContentVM as IDisposable;
        if (currentStepContentVM != null)
          currentStepContentVM.Dispose();
      }
    }


    protected virtual void BackExecute(object o)
    {
      Assert.IsTrue(Steps.Count > 0, "Steps stack is corrupted. Can't be less than 1");
      var contentVMtoDestroy = ContentVM as IDisposable;
      if (contentVMtoDestroy != null)
        contentVMtoDestroy.Dispose();
      Steps.Pop();
      CanBack = Steps.Count > 1;
      if (CanBack)
      {
        WindowStepState windowStepState = Steps.Where((state, index) => index == 1).FirstOrDefault();
        if (windowStepState != null)
          BackToTitle = windowStepState.ShortName;
        else
          BackToTitle = "hell :)";
      }
      NotifyPublicVisibleChanged();
    }


    protected virtual void GoAheadWithBack(WindowStepState newState)
    {
      Assert.IsNotNull(newState, "New state cannot be null");
      BackToTitleInternal = CurrentState.ShortName;
      Steps.Push(newState);
      CanBack = true;
      NotifyPublicVisibleChanged();
    }

    protected virtual void NotifyPublicVisibleChanged()
    {
      OnPropertyChanged("GlobalTitle");
      OnPropertyChanged("Header");
      OnPropertyChanged("ContentVM");
      OnPropertyChanged("BackToTitle");
      OnPropertyChanged("ExtraActionTitle");
      OnPropertyChanged("ExtraActionCommand");
      OnPropertyChanged("HelpActions");
    }


    protected virtual void HelpActions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
      OnPropertyChanged("HelpActions");
    }


    protected virtual List<ActionCommandVM> GetHelpActions()
    {
      return new List<ActionCommandVM>()
                {
                    new ActionCommandVM(new RelayCommand(o => Application.Current.Shutdown(), true), "Close app"),
                    new ActionCommandVM(new RelayCommand(SavePositionAndSize, true), "Save P&S")
                };
    }

    protected virtual void SavePositionAndSize(object o)
    {
      if (SizePozitionProvider == null)
      {
        HatcherGuide<IUIManager>.Instance.OKMessageBox("Tray Garden -- Save position and size",
                                                       "Unable to save position and size. Provider is empty.");
        Log.Warn("SizePozitionProvider of WindowWithBackVMBase is empty. Something is wrong", this);
        return;
      }
      double top;
      double left;
      double width;
      double height;
      bool maximized;
      if (SizePozitionProvider(out top, out left, out width, out height, out maximized))
      {
        Top = top;
        Left = left;
        Width = width;
        Height = height;
        Maximized = maximized;
        SizePropertiesAreValid = true;
      }
    }
  }
}
