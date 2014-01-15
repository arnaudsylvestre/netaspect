using System;
using System.Reflection;
using FluentAspect.Weaver.Tests.Core;
using FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Parameters.Before;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Calls.Fields
{
    [TestFixture]
    public class BeforeCallFieldTest 
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
                  .AndEnsureAssembly((assembly, actual) =>
                      {
                          var caller = actual.CreateCallerObject();
                          actual.CallCallerMethod(caller);
                          Assert.AreEqual(caller, actual.Aspect.BeforeCaller);
                      })
                  .AndLaunchTest();
         }
    }

    public class ClassAndAspectAndCallActual
    {
        public NetAspectAttribute Aspect { get; set; }
        private string callerClassName;
        private string callerMethodName;
        private Assembly assembly;

        public ClassAndAspectAndCallActual(Assembly assembly_P, string callerClassName, string callerMethodName)
        {
            assembly = assembly_P;
            this.callerClassName = callerClassName;
            this.callerMethodName = callerMethodName;
        }

        public object CreateCallerObject(params object[] parameters)
        {
            return assembly.CreateObject(callerClassName, parameters);
        }

        public void CallCallerMethod(object o_P, params object[] parameters)
        {
            o_P.CallMethod(callerMethodName, parameters);
        }
    }

    [Serializable]
    public class ClassAndAspectAndCallAcceptanceTestBuilder : IAcceptanceTestBuilder<ClassAndAspectAndCall, ClassAndAspectAndCallActual>
    {
        private const string _aspectName = "MyAspectAttribute";
        private const string _typeName = "MyClassToWeave";
        private const string fieldName = "MyFieldToWeave";

        public ClassAndAspectAndCallActual CreateActual(Assembly assemblyDllP_P, DoAcceptanceHelper helper)
        {
            return new ClassAndAspectAndCallActual(assemblyDllP_P, _typeName, fieldName)
            {
                Aspect = new NetAspectAttribute(assemblyDllP_P, _aspectName)
            };
        }

        public ClassAndAspectAndCall CreateSample(AssemblyDefinitionDefiner assembly)
        {

            var type = assembly.WithType(_typeName);
            var aspect = assembly.WithCallFieldWeavingAspect(_aspectName);
            var field = type.WithField<int>(fieldName);
            field.AddAspect(aspect);
            var callerType = assembly.WithType("CallerType");
            callerType.WithMethod("Caller")
                .WhichInstantiateAnObject("toCall", type)
                .AndInitializeField("toCall", fieldName, 3)
                .AndReturn();
            return new ClassAndAspectAndCall()
            {
                Class = type,
                Aspect = aspect,
                FieldToWeave = field,
                CallerType = callerType,
            };
        }
    }

    public class ClassAndAspectAndCall
    {
        public TypeDefinitionDefiner Class { get; set; }

        public CallFieldWeavingAspectDefiner Aspect { get; set; }

        public FieldDefinitionDefiner FieldToWeave { get; set; }

        public TypeDefinitionDefiner CallerType { get; set; }
    }
}