using FluentAspect.Weaver.Tests.acceptance;
using FluentAspect.Weaver.Tests.acceptance.Weaving.Calls.Fields;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Update.Parameters.After
{
    [TestFixture]
    public class AfterUpdatePropertyValueParameterTest
    {
        [Test]
        public void CheckUpdatePropertyWithValue()
        {
            DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
                   .ByDefiningAssembly(simpleClassAndWeaver =>
                   {
                       simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
                          .WithParameter<int>("value");
                   })
                //.EnsureErrorHandler(e => e.Warnings.Add("The parameter lineNumber in method AfterUpdatePropertyValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
                    .AndEnsureAssembly((assembly, actual) =>
                    {
                        var caller = actual.CreateCallerObject();
                        actual.CallCallerMethod(caller);
                        Assert.AreEqual(3, actual.Aspect.AfterUpdatePropertyValueValue);
                    })
                    .AndLaunchTest();
        }
        [Test]
        public void CheckUpdatePropertyWithValueWithWrongType()
        {
            DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
                   .ByDefiningAssembly(simpleClassAndWeaver =>
                   {
                       simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
                          .WithParameter<string>("value");
                   })
                //.EnsureErrorHandler(e => e.Warnings.Add("The parameter lineNumber in method AfterUpdatePropertyValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
                    .AndEnsureAssembly((assembly, actual) =>
                    {
                        var caller = actual.CreateCallerObject();
                        actual.CallCallerMethod(caller);
                        Assert.AreEqual(3, actual.Aspect.AfterUpdatePropertyValueValue);
                    })
                    .AndLaunchTest();
        }
        [Test]
        public void CheckUpdatePropertyWithValueReferenced()
        {
            DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
                   .ByDefiningAssembly(simpleClassAndWeaver =>
                   {
                       simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
                          .WithReferencedParameter<string>("value");
                   })
                //.EnsureErrorHandler(e => e.Warnings.Add("The parameter lineNumber in method AfterUpdatePropertyValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
                    .AndEnsureAssembly((assembly, actual) =>
                    {
                        var caller = actual.CreateCallerObject();
                        actual.CallCallerMethod(caller);
                        Assert.AreEqual(3, actual.Aspect.AfterUpdatePropertyValueValue);
                    })
                    .AndLaunchTest();
        }
    }
}