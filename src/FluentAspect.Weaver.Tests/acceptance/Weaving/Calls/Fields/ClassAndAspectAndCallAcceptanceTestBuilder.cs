using System;
using System.Reflection;
using FluentAspect.Weaver.Tests.Core;
using FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Parameters.Before;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Calls.Fields
{
    [Serializable]
    public class ClassAndAspectAndCallAcceptanceTestBuilder : IAcceptanceTestBuilder<ClassAndAspectAndCall, ClassAndAspectAndCallActual>
    {
        private const string _aspectName = "MyAspectAttribute";
        private const string _typeName = "MyClassToWeave";
        private const string fieldName = "MyFieldToWeave";
        private const string callerTypeName = "CallerType";
        private const string callerMethod = "Caller";

        public ClassAndAspectAndCallActual CreateActual(Assembly assemblyDllP_P)
        {
            return new ClassAndAspectAndCallActual(assemblyDllP_P, callerTypeName, callerMethod)
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
            var callerType = assembly.WithType(callerTypeName);
            callerType.WithMethod(callerMethod)
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
}