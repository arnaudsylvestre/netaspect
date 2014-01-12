using System;
using FluentAspect.Weaver.Tests.Core;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Parameters.Before
{
    [TestFixture]
   public abstract class InstanceParameterTest
    {
        private Func<MethodWeavingAspectDefiner, MethodDefinitionDefiner> interceptor;
        private string methodName;

        public InstanceParameterTest(Func<MethodWeavingAspectDefiner, MethodDefinitionDefiner> interceptor, string method)
        {
            this.interceptor = interceptor;
            methodName = method;
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
                    .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("impossible to ref the parameter 'instance' in the method {0} of the type 'A.MyAspectAttribute'", methodName)))
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
                    .EnsureErrorHandler(errorHandler => errorHandler.Errors.Add(string.Format("the instance parameter in the method {0} of the type 'A.MyAspectAttribute' is declared with the type 'System.Int32' but it is expected to be System.Object or A.MyClassToWeave", methodName)))
                    .AndLaunchTest();
        }

        [Test]
        public void CheckInstanceWithObjectType()
        {
            DoAcceptance.Test(methodName)
                    .ByDefiningAssembly(assembly =>
                    {
                        var aspect = assembly.WithMethodWeavingAspect("MyAspectAttribute");
                        interceptor(aspect).WithParameter<object>("instance");
                        var method = assembly.WithType("MyClassToWeave").WithMethod("MyMethodToWeave");
                        method.AddAspect(aspect);
                    })
                    .AndEnsureAssembly((assemblyP, context, helper) =>
                    {
                        var o = assemblyP.CreateObject("MyClassToWeave");
                        o.CallMethod("MyMethodToWeave");

                        var netAspectAttribute = helper.GetNetAspectAttribute("MyAspectAttribute");

                        Assert.AreEqual(o, netAspectAttribute.GetInstance(context));
                    })
                    .AndLaunchTest();
        }

        public class AssemblyBuilder
        {
            public static SimpleClassAndWeaverWithBefore CreateSimpleClassAndWeaver(AssemblyDefinitionDefiner assembly)
            {
                var type = assembly.WithType("MyClassToWeave");
                var aspect = assembly.WithMethodWeavingAspect("MyAspectAttribute");
                var method = type.WithMethod("MyMethodToWeave");
                method.AddAspect(aspect);
                return new SimpleClassAndWeaverWithBefore()
                    {
                        ClassToWeave = type,
                        Aspect = aspect,     
                        MethodToWeave = method,
                    };
            }

            public class SimpleClassAndWeaverWithBefore
            {
                public TypeDefinitionDefiner ClassToWeave { get; set; }

                public MethodWeavingAspectDefiner Aspect { get; set; }

                public MethodDefinitionDefiner MethodToWeave { get; set; }

                public MethodDefinitionDefiner BeforeInterceptor
                {
                    get { return Aspect.AddBefore(); }
                }

                public MethodDefinitionDefiner AfterInterceptor
                {
                    get { return Aspect.AddAfter(); }
                }

                public MethodDefinitionDefiner OnExceptionInterceptor
                {
                    get { return Aspect.AddOnException(); }
                }
            }

            
        }

        [Test]
        public void CheckInstanceWithRealType()
        {
            DoAcceptance.Test(methodName)
                    .ByDefiningAssembly(assembly =>
                    {
                        var simpleClassAndWeaver = AssemblyBuilder.CreateSimpleClassAndWeaver(assembly);
                        simpleClassAndWeaver.BeforeInterceptor.WithParameter("instance", simpleClassAndWeaver.ClassToWeave.Type);
                        simpleClassAndWeaver.MethodToWeave.WhichRaiseException();
                    })
                    .AndEnsureAssembly((assemblyP, context, helper) =>
                    {
                            var o = assemblyP.CreateObject("MyClassToWeave");
                        try
                        {
                            o.CallMethod("MyMethodToWeave");
                            Assert.Fail();

                        }
                        catch (Exception)
                        {
                            var netAspectAttribute = helper.GetNetAspectAttribute("MyAspectAttribute");
                            Assert.AreEqual(o, netAspectAttribute.GetInstance(context));
                            
                        }

                    })
                    .AndLaunchTest();
        }
   }

}