using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Samples.Methods
{
   public class LogSampleTest :
      NetAspectTest<LogSampleTest.ClassToWeave>
   {
      protected override Action CreateEnsure()
      {
         return () =>
             {
                 var defaultLogger = new DefaultLogger();
                 LogAttribute.Logger = defaultLogger;

                 var classToWeave = new ClassToWeave();
                 classToWeave.Divide(12, 3);
                 Assert.AreEqual(2, defaultLogger.infos.Count);
                 Assert.AreEqual(0, defaultLogger.errors.Count);
                 try
                 {
                     classToWeave.Divide(12, 0);
                 }
                 catch (Exception)
                 {
                 }
                 Assert.AreEqual(3, defaultLogger.infos.Count);
                 Assert.AreEqual(1, defaultLogger.errors.Count);
             };
      }

      public class DefaultLogger : LogAttribute.ILogger
      {
          public List<string> infos = new List<string>();
          public List<string> errors = new List<string>();

          public void Info(string text)
          {
              infos.Add(text);
          }

          public void Error(string text)
          {
              errors.Add(text);
          }
       }

      public class ClassToWeave
      {
          [Log] 
         public int Divide(int value, int divideBy)
          {
              return value/divideBy;
          }
      }

      public class LogAttribute : Attribute
      {
          public interface ILogger
          {
              void Info(string text);
              void Error(string text);
          }

          public static ILogger Logger;

         public bool NetAspectAttribute = true;

          private static string ComputeLog(string eventName, MethodBase method)
          {
              return string.Format("{0} {1} ", eventName, method.Name);
          }

          public void BeforeMethod(MethodBase method)
          {
              Logger.Info(ComputeLog("On enter :", method));
          }

          public void AfterMethod(MethodBase method)
          {
              Logger.Info(ComputeLog("On exit :", method));
          }

          public void OnExceptionMethod(MethodBase method, Exception exception)
          {
              Logger.Error(ComputeLog(exception.Message, method));
          }
      }
   }
}
