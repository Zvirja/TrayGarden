﻿#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace ClipboardChangerPlant.RequestHandling.PipelineModel.Pipeline
{
  [UsedImplicitly]
  public class PostExecuteHandler : Processor
  {
    #region Public Methods and Operators

    public override void Process(ProcessorArgs args)
    {
      RequestHandler resolvedHandler = args.ResolvedHandler;
      if (resolvedHandler == null)
      {
        throw new ArgumentException("args.ResolvedHandler");
      }

      if (!resolvedHandler.PostExecute(args.ResultUrl, args.ClipboardEvent))
      {
        this.HandleErrorAndAbortPipeline(args, this.NotFoundTrayIcon);
      }
    }

    #endregion
  }
}