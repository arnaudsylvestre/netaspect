using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Calls.Fields
{
    [TestFixture]
    public class BeforeUpdateFieldValueParameterTest 
    {
         [Test]
         public void CheckCallField()
         {
          DoAcceptance.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
                 .ByDefiningAssembly(simpleClassAndWeaver =>
                    {
                       simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
                          .WithParameter<int>("value");
                    })
                    .EnsureErrorHandler(e => e.Warnings.Add("The parameter lineNumber in method BeforeUpdateFieldValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
                  .AndEnsureAssembly((assembly, actual) =>
                      {
                          var caller = actual.CreateCallerObject();
                          actual.CallCallerMethod(caller);
                          Assert.AreEqual(0, actual.Aspect.BeforeUpdateFieldValueLineNumber);
                      })
                  .AndLaunchTest();
         }
    }
}