using FluentAspect.Weaver.Tests.acceptance;
using FluentAspect.Weaver.Tests.acceptance.Weaving.Calls.Fields;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.Before
{
    [TestFixture]
    public class BeforeCallMethodValueParameterTest
    {
        [Test]
        public void CheckCallMethodWithValue()
        {
            DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
                   .ByDefiningAssembly(simpleClassAndWeaver =>
                   {
                       simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
                          .WithParameter<int>("value");
                   })
                //.EnsureErrorHandler(e => e.Warnings.Add("The parameter lineNumber in method BeforeCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
                    .AndEnsureAssembly((assembly, actual) =>
                    {
                        var caller = actual.CreateCallerObject();
                        actual.CallCallerMethod(caller);
                        Assert.AreEqual(3, actual.Aspect.BeforeCallMethodValueValue);
                    })
                    .AndLaunchTest();
        }
        [Test]
        public void CheckCallMethodWithValueWithWrongType()
        {
            DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
                   .ByDefiningAssembly(simpleClassAndWeaver =>
                   {
                       simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
                          .WithParameter<string>("value");
                   })
                //.EnsureErrorHandler(e => e.Warnings.Add("The parameter lineNumber in method BeforeCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
                    .AndEnsureAssembly((assembly, actual) =>
                    {
                        var caller = actual.CreateCallerObject();
                        actual.CallCallerMethod(caller);
                        Assert.AreEqual(3, actual.Aspect.BeforeCallMethodValueValue);
                    })
                    .AndLaunchTest();
        }
        [Test]
        public void CheckCallMethodWithValueReferenced()
        {
            DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
                   .ByDefiningAssembly(simpleClassAndWeaver =>
                   {
                       simpleClassAndWeaver.Aspect.AddBeforeFieldAccess()
                          .WithReferencedParameter<string>("value");
                   })
                //.EnsureErrorHandler(e => e.Warnings.Add("The parameter lineNumber in method BeforeCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
                    .AndEnsureAssembly((assembly, actual) =>
                    {
                        var caller = actual.CreateCallerObject();
                        actual.CallCallerMethod(caller);
                        Assert.AreEqual(3, actual.Aspect.BeforeCallMethodValueValue);
                    })
                    .AndLaunchTest();
        }
    }
}