using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Helpers;
using NetAspect.Weaver.Tests.Helpers;

namespace NetAspect.Weaver.Tests.unit
{
    public static class RunWeavingTest
    {
       private static AppDomainIsolatedTestRunner CreateAppRunner(Assembly assembly)
       {
          return AppDomain.CreateDomain("For test", null,
                                        new AppDomainSetup
                                        {
                                           ApplicationBase = Path.GetDirectoryName(assembly.GetAssemblyPath()),
                                           ShadowCopyFiles = "true"
                                        })
                          .CreateInstanceFromAndUnwrap(
                              new Uri(typeof(AppDomainIsolatedTestRunner).Assembly.CodeBase).LocalPath,
                              typeof(AppDomainIsolatedTestRunner).FullName)
                 as AppDomainIsolatedTestRunner;
       }


        public static void For<T, U>(Type t, Action<List<ErrorReport.Error>> resultProvider, Action ensureAssembly)
        {
            Assembly assembly = typeof(T).Assembly;
            var assemblyFile = assembly.GetAssemblyPath() + ".Test";
            if (File.Exists(assemblyFile))
                File.Delete(assemblyFile);
            Assembly otherAssembly = typeof(U).Assembly;
            AppDomainIsolatedTestRunner runner = CreateAppRunner(assembly);

            var errorHandler = new List<ErrorReport.Error>();
            resultProvider(errorHandler);
            string dll = assembly.GetAssemblyPath();
            var otherDll = otherAssembly.GetAssemblyPath();
            Console.Write(runner.RunFromType(dll, typeof(T).FullName, errorHandler, otherDll, typeof(U).FullName));

            runner = CreateAppRunner(assembly);

            runner.Ensure(assemblyFile, t.FullName);
           
        }
    }
}