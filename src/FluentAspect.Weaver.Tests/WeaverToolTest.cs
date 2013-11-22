using System.Diagnostics;
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
          const string asm = "FluentAspect.Sample.exe";
          var weaver_L = new WeaverTool(asm);
          weaver_L.Weave();
       }

       [Test]
       public void CheckSimpleWeave2()
       {
          MyClassToWeave classe = new MyClassToWeave();
          Assert.AreEqual("Weaved", classe.MustRaiseExceptionAfterWeave());

       }

        [Test]
        public void CheckSimpleWeaveCF()
        {
           const string asm = "FluentAspect.Sample.CF.exe";
           var weaver_L = new WeaverTool(asm);
           weaver_L.Weave();
        }
    }
}