using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests
{
    [TestFixture]
    public class WeaverToolTest
    {
        //public void Sample()
        //{
        //    object[] args = new object[0];
        //    Around.Call(this, "CheckWith", args, new CheckThrowInterceptor());
        //}

       [Test]
        public void CheckWithReturn()
       {
          var res = WeaveAndCheck("CheckWithReturn", new object[0]);
          Assert.AreEqual("Weaved", res);
       }
       [Test]
       public void CheckWithParameters()
       {
           var res = WeaveAndCheck("CheckWithParameters", new object[] { "Weaved with parameters" });
           Assert.AreEqual("Weaved with parameters", res);
       }
       [Test]
       public void CheckWithVoid()
       {
           WeaveAndCheck("CheckWithVoid", new object[] {});
       }

       [Test]
       public void CheckWithGenerics()
       {
           var res = WeaveAndCheck<string>("CheckWithGenerics", new object[] { "Weaved" });
           Assert.AreEqual("Weaved", res);
       }

       [Test, ExpectedException(typeof(NotSupportedException))]
       public void CheckThrow()
       {
           try
           {
               WeaveAndCheck("CheckThrow", new object[] { });
           }
           catch (TargetInvocationException e)
           {
               throw e.InnerException;
           }
           
       }

       [Test]
       public void CheckBefore()
       {
           var res = WeaveAndCheck("CheckBefore", new object[] { "not before" });
           Assert.AreEqual("Value set in before", res);
       }

       [Test]
       public void CheckNotRenameInAssembly()
       {
           var res = WeaveAndCheck("CheckNotRenameInAssembly", new object[] {  });
           Assert.AreEqual("Waved", res);
       }

       private static object WeaveAndCheck(string checkwithreturn_L, object[] parameters)
       {
          const string asm = "FluentAspect.Sample.exe";
          const string dst = "FluentAspect.Sample.Weaved.exe";
          var weaver_L = new WeaverTool(asm, dst);
          weaver_L.Weave();
          var myClassToWeaveType = (from t in Assembly.LoadFrom(dst).GetTypes() where t.Name == "MyClassToWeave" select t).First();
          var instance_L = Activator.CreateInstance(myClassToWeaveType);
          var res = myClassToWeaveType.GetMethod(checkwithreturn_L).Invoke(instance_L, parameters);
          return res;
       }

       private static object WeaveAndCheck<T>(string checkwithreturn_L, object[] parameters)
       {
           const string asm = "FluentAspect.Sample.exe";
           const string dst = "FluentAspect.Sample.Weaved.exe";
           var weaver_L = new WeaverTool(asm, dst);
           weaver_L.Weave();
           var myClassToWeaveType = (from t in Assembly.LoadFrom(dst).GetTypes() where t.Name == "MyClassToWeave" select t).First();
           var instance_L = Activator.CreateInstance(myClassToWeaveType);
           var res = myClassToWeaveType.GetMethod(checkwithreturn_L).MakeGenericMethod(typeof(T)).Invoke(instance_L, parameters);
           return res;
       }
    }
}