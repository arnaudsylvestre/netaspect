﻿using FluentAspect.Weaver.Tests.acceptance;
using FluentAspect.Weaver.Tests.acceptance.Weaving.Calls.Fields;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.CallWeaving.Fields.Getter.Parameters.After
{
    [TestFixture]
    public class AfterCallMethodValueParameterTest
    {
        [Test]
        public void CheckCallMethodWithValue()
        {
            DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
                   .ByDefiningAssembly(simpleClassAndWeaver =>
                   {
                       simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
                          .WithParameter<int>("value");
                   })
                //.EnsureErrorHandler(e => e.Warnings.Add("The parameter lineNumber in method AfterCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
                    .AndEnsureAssembly((assembly, actual) =>
                    {
                        var caller = actual.CreateCallerObject();
                        actual.CallCallerMethod(caller);
                        Assert.AreEqual(3, actual.Aspect.AfterCallMethodValueValue);
                    })
                    .AndLaunchTest();
        }
        [Test]
        public void CheckCallMethodWithValueWithWrongType()
        {
            DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
                   .ByDefiningAssembly(simpleClassAndWeaver =>
                   {
                       simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
                          .WithParameter<string>("value");
                   })
                //.EnsureErrorHandler(e => e.Warnings.Add("The parameter lineNumber in method AfterCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
                    .AndEnsureAssembly((assembly, actual) =>
                    {
                        var caller = actual.CreateCallerObject();
                        actual.CallCallerMethod(caller);
                        Assert.AreEqual(3, actual.Aspect.AfterCallMethodValueValue);
                    })
                    .AndLaunchTest();
        }
        [Test]
        public void CheckCallMethodWithValueReferenced()
        {
            DoUnit.Test(new ClassAndAspectAndCallAcceptanceTestBuilder())
                   .ByDefiningAssembly(simpleClassAndWeaver =>
                   {
                       simpleClassAndWeaver.Aspect.AddAfterFieldAccess()
                          .WithReferencedParameter<string>("value");
                   })
                //.EnsureErrorHandler(e => e.Warnings.Add("The parameter lineNumber in method AfterCallMethodValue of type A.MyAspectAttribute will have the default value because there is no debugging information"))
                    .AndEnsureAssembly((assembly, actual) =>
                    {
                        var caller = actual.CreateCallerObject();
                        actual.CallCallerMethod(caller);
                        Assert.AreEqual(3, actual.Aspect.AfterCallMethodValueValue);
                    })
                    .AndLaunchTest();
        }
    }
}