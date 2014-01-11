using FluentAspect.Weaver.Tests.Core;
using FluentAspect.Weaver.Tests.unit;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Parameters.Before.Instance
{
    [TestFixture]
   public class InstanceParameterBeforeTest
   {
        [Test]
        public void CheckInstanceParameterOnBeforeWithReferenced()
        {
            DoAcceptance.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                        var aspect = assembly.WithMethodWeavingAspect("MyAspectAttribute");
                        aspect.AddBefore().WithReferencedParameter<object>("instance");
                        var method = assembly.WithType("MyClassToWeave").WithMethod("MyMethodToWeave");
                        method.AddAspect(aspect);
                    })
                    .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add("impossible to ref the parameter 'instance' in the method Before of the type 'A.MyAspectAttribute'"))
                    .AndLaunchTest();
        }

        [Test]
        public void CheckInstanceParameterOnBeforeWithBadType()
        {
            DoAcceptance.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                        var aspect = assembly.WithMethodWeavingAspect("MyAspectAttribute");
                        aspect.AddBefore().WithParameter<int>("instance");
                        var method = assembly.WithType("MyClassToWeave").WithMethod("MyMethodToWeave");
                        method.AddAspect(aspect);
                    })
                    .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add("the instance parameter in the method Before of the type 'A.MyAspectAttribute' is declared with the type 'System.Int32' but it is expected to be System.Object or A.MyClassToWeave"))
                    .AndLaunchTest();
        }

        [Test]
        public void CheckInstanceParameterOnBeforeWithObjectType()
        {
            DoAcceptance.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                        var aspect = assembly.WithMethodWeavingAspect("MyAspectAttribute");
                        aspect.AddBefore().WithParameter<object>("instance");
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
        public void CheckInstanceParameterOnBeforeWithRealType()
        {
            DoAcceptance.Test()
                    .ByDefiningAssembly(assembly =>
                    {
                        var type = assembly.WithType("MyClassToWeave");
                        var aspect = assembly.WithMethodWeavingAspect("MyAspectAttribute");
                        aspect.AddBefore().WithParameter("instance", type.Type);
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