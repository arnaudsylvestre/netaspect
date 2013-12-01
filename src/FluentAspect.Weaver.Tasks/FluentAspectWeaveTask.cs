using System;
using System.Diagnostics;
using System.IO;
using FluentAspect.Weaver.Factory;
using Microsoft.Build.Utilities;

namespace FluentAspect.Weaver.Tasks
{
    public class FluentAspectWeaveTask : Task
    {
       public override bool Execute()
        {
            try
            {                
                var weaverCore = WeaverCoreFactory.Create();

            }
            catch (Exception e)
            {
                Log.LogError(e.ToString());
                return false;
            }
            return true;
        }
    }
}
