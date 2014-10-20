using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Checkers
{
    public static class EnsureResult
    {
        public static void OfType(ParameterInfo info, ErrorHandler handler, MethodDefinition method)
        {
            if (MethodMustNotBeVoid(info, handler, method)) return;
            EnsureParameter.IsNotOut(info, handler);
            if (info.ParameterType == typeof(object))
                return;
            if (info.ParameterType.FullName.Replace("&", "") != method.ReturnType.FullName.Replace("/", "+"))
            {
                handler.OnError(
                    ErrorCode.ParameterWithBadTypeBecauseReturnMethod,
                    FileLocation.None,
                    info.Name,
                    info.Member.Name,
                    info.Member.DeclaringType.FullName.Replace("/", "+"),
                    info.ParameterType.FullName,
                    method.ReturnType.FullName,
                    method.Name,
                    method.DeclaringType.FullName.Replace("/", "+"));
            }
        }


        public static void OfTypeNotReferenced(ParameterInfo parameter_P, ErrorHandler errorListener_P, MethodDefinition method_P)
        {
            EnsureResult.OfType(parameter_P, errorListener_P, method_P);
            EnsureParameter.IsNotReferenced(parameter_P, errorListener_P);
        }



        private static bool MethodMustNotBeVoid(ParameterInfo info, ErrorHandler handler, MethodDefinition method)
        {
            if (method.ReturnType != method.Module.TypeSystem.Void)
                return false;
            handler.OnError(
                ErrorCode.MustNotBeVoid,
                FileLocation.None,
                info.Name,
                info.Member.Name,
                info.Member.DeclaringType.FullName.Replace("/", "+"),
                method.Name,
                method.DeclaringType.FullName.Replace("/", "+"));
            return true;
        }

    }
}