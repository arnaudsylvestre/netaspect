using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests
{
    [TestFixture]
    public class WeaverToolTest
    {
       [Test]
       public void CheckSimpleWeave()
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
    }
}