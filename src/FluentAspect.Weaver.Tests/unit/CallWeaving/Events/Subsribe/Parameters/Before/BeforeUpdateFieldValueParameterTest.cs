using FluentAspect.Weaver.Tests.acceptance;
using FluentAspect.Weaver.Tests.acceptance.Weaving.Calls.Fields;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Update.Parameters.Before
{
    [TestFixture]
    public class BeforeSubscribeEventValueParameterTest
    {
        [Test]
        public void CheckSubscribeEventWithValue()
        {
            DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
                   .ByDefiningAssembly(simpleClassAndWeaver =>
                   {
                       simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
                          .WithParameter<int>("value");
                   })
                //.EnsureErrorHandler(e => e.Warnings.Add("The parameter lineNumber in method BeforeSubscribeEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
                    .AndEnsureAssembly((assembly, actual) =>
                    {
                        var caller = actual.CreateCallerObject();
                        actual.CallCallerMethod(caller);
                        Assert.AreEqual(3, actual.Aspect.BeforeSubscribeEventValueValue);
                    })
                    .AndLaunchTest();
        }
        [Test]
        public void CheckSubscribeEventWithValueWithWrongType()
        {
            DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
                   .ByDefiningAssembly(simpleClassAndWeaver =>
                   {
                       simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
                          .WithParameter<string>("value");
                   })
                //.EnsureErrorHandler(e => e.Warnings.Add("The parameter lineNumber in method BeforeSubscribeEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
                    .AndEnsureAssembly((assembly, actual) =>
                    {
                        var caller = actual.CreateCallerObject();
                        actual.CallCallerMethod(caller);
                        Assert.AreEqual(3, actual.Aspect.BeforeSubscribeEventValueValue);
                    })
                    .AndLaunchTest();
        }
        [Test]
        public void CheckSubscribeEventWithValueReferenced()
        {
            DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
                   .ByDefiningAssembly(simpleClassAndWeaver =>
                   {
                       simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
                          .WithReferencedParameter<string>("value");
                   })
                //.EnsureErrorHandler(e => e.Warnings.Add("The parameter lineNumber in method BeforeSubscribeEventValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
                    .AndEnsureAssembly((assembly, actual) =>
                    {
                        var caller = actual.CreateCallerObject();
                        actual.CallCallerMethod(caller);
                        Assert.AreEqual(3, actual.Aspect.BeforeSubscribeEventValueValue);
                    })
                    .AndLaunchTest();
        }
    }
}