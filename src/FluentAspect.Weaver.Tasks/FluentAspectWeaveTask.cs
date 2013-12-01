using System;
using System.Diagnostics;
using System.IO;
using FluentAspect.Weaver.Factory;
using Microsoft.Build.Framework;
using Microsoft.Build.Tasks;
using Microsoft.Build.Utilities;

namespace FluentAspect.Weaver.Tasks
{
    public class FluentAspectWeaveTask : Task
    {
        [Required]
        public string AssemblyFile { get; set; }

       public override bool Execute()
        {
            try
            {                
                var weaverCore = WeaverCoreFactory.Create();
                weaverCore.Weave(AssemblyFile);
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
