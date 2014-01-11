using System;
using FluentAspect.Weaver.Tests.Core;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Parameters.Before
{
    [TestFixture]
   public abstract class InstanceParameterTest
    {
        private Func<MethodWeavingAspectDefiner, MethodDefinitionDefiner> interceptor;

        public InstanceParameterTest(Func<MethodWeavingAspectDefiner, MethodDefinitionDefiner> interceptor)
        {
            this.interceptor = interceptor;
        }

        [Test]
        public void CheckInstanceReferenced()
        {
            DoAcceptance.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                        var aspect = assembly.WithMethodWeavingAspect("MyAspectAttribute");
                        interceptor(aspect).WithReferencedParameter<object>("instance");
                        var method = assembly.WithType("MyClassToWeave").WithMethod("MyMethodToWeave");
                        method.AddAspect(aspect);
                    })
                    .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add("impossible to ref the parameter 'instance' in the method After of the type 'A.MyAspectAttribute'"))
                    .AndLaunchTest();
        }

        [Test]
        public void CheckInstanceBadType()
        {
            DoAcceptance.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                        var aspect = assembly.WithMethodWeavingAspect("MyAspectAttribute");
                        interceptor(aspect).WithParameter<int>("instance");
                        var method = assembly.WithType("MyClassToWeave").WithMethod("MyMethodToWeave");
                        method.AddAspect(aspect);
                    })
                    .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add("the instance parameter in the method After of the type 'A.MyAspectAttribute' is declared with the type 'System.Int32' but it is expected to be System.Object or A.MyClassToWeave"))
                    .AndLaunchTest();
        }

        [Test]
        public void CheckInstanceWithObjectType()
        {
            DoAcceptance.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                        var aspect = assembly.WithMethodWeavingAspect("MyAspectAttribute");
                        interceptor(aspect).WithParameter<object>("instance");
                        var method = assembly.WithType("MyClassToWeave").WithMethod("MyMethodToWeave");
                        method.AddAspect(aspect);
                    })
                    .AndEnsureAssembly((assemblyP, helper) =>
                    {
                        var o = assemblyP.CreateObject("MyClassToWeave");
                        o.CallMethod("MyMethodToWeave");

                        var netAspectAttribute = helper.GetNetAspectAttribute("MyAspectAttribute");

                        Assert.AreEqual(o, netAspectAttribute.BeforeInstance);
                    })
                    .AndLaunchTest();
        }

        [Test]
        public void CheckInstanceWithRealType()
        {
            DoAcceptance.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                        var type = assembly.WithType("MyClassToWeave");
                        var aspect = assembly.WithMethodWeavingAspect("MyAspectAttribute");
                        interceptor(aspect).WithParameter("instance", type.Type);
                        var method = type.WithMethod("MyMethodToWeave");
                        method.AddAspect(aspect);
                    })
                    .AndEnsureAssembly((assemblyP, helper) =>
                    {
                        var o = assemblyP.CreateObject("MyClassToWeave");
                        o.CallMethod("MyMethodToWeave");

                        var netAspectAttribute = helper.GetNetAspectAttribute("MyAspectAttribute");

                        Assert.AreEqual(o, netAspectAttribute.BeforeInstance);
                    })
                    .AndLaunchTest();
        }
   }

}