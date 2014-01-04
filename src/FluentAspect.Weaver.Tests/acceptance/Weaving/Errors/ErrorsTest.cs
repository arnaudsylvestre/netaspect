using System;
using System.Collections.Generic;
using FluentAspect.Weaver.Core.Errors;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Errors
{
    public class ErrorsTest : AcceptanceTest
    {
        protected override void EnsureErrorHandler(ErrorHandler errorHandler)
        {
            Dump(errorHandler);

            Assert.AreEqual(new List<string>
            {
                "The aspect FluentAspect.Sample.AOP.CheckWithVoidInterceptorAttribute doesn't have a Before/After/OnException method",
"The aspect FluentAspect.Sample.AOP.CheckNotRenameInAssemblyAttribute doesn't have a Before/After/OnException method",
"The aspect FluentAspect.Sample.MethodWeaving.Problems.Warnings.EmptyAspectAttribute doesn't have a Before/After/OnException method",           
            }, errorHandler.Warnings);
            Assert.AreEqual(new List<string>
            {
            }, errorHandler.Failures);
            Assert.AreEqual(new List<string>
                {
                   "A method declared in interface can not be weaved : InterfaceToWeaveWithAttributes.CheckBeforeWithAttributes",
"impossible to ref the parameter 'parameters' in the method After of the type 'FluentAspect.Sample.AOP.CheckWithParametersReferencedAttribute'",
"An abstract method can not be weaved : AbstractClassToWeaveWithAttributes.CheckBeforeWithAttributes",
"impossible to ref the parameter 'method' in the method After of the type 'FluentAspect.Sample.MethodWeaving.Parameters.ToCheckMethodReferencedInAfterAspectAttribute'",
"the method parameter in the method After of the type 'FluentAspect.Sample.MethodWeaving.Parameters.ToCheckMethodWithWrongTypeInAfterAspectAttribute' is declared with the type 'System.Int32' but it is expected to be System.Reflection.MethodInfo",
"impossible to ref the parameter 'parameters' in the method OnException of the type 'FluentAspect.Sample.MethodWeaving.Parameters.ToCheckParametersReferencedOnExceptionAspectAttribute'",
"impossible to ref the parameter 'parameters' in the method Before of the type 'FluentAspect.Sample.MethodWeaving.Parameters.ToCheckParametersReferencedInBeforeAspectAttribute'",
"impossible to ref the parameter 'parameters' in the method After of the type 'FluentAspect.Sample.MethodWeaving.Parameters.ToCheckParametersReferencedInAfterAspectAttribute'",
"impossible to ref the parameter 'instance' in the method After of the type 'FluentAspect.Sample.MethodWeaving.Parameters.ToCheckInstanceReferencedInAfterAspectAttribute'",
"the instance parameter in the method After of the type 'FluentAspect.Sample.MethodWeaving.Parameters.ToCheckInstanceWithWrongTypeInAfterAspectAttribute' is declared with the type 'System.Int32' but it is expected to be System.Object or FluentAspect.Sample.MethodWeaving.Parameters.ToCheckInstanceWithWrongTypeInAfter",
"the result parameter in the method After of the type 'FluentAspect.Sample.MethodWeaving.Parameters.ToCheckResultInVoidAspectAttribute' is declared with the type 'System.Int32&' but it is expected to be System.Void because the return type of the method Check in the type FluentAspect.Sample.MethodWeaving.Parameters.ToCheckResultInVoid",
                }, errorHandler.Errors);
        }

        private static void Dump(ErrorHandler errorHandler)
        {
            Dump("Warnings", errorHandler.Warnings);
            Dump("Errors", errorHandler.Errors);
            Dump("Failures", errorHandler.Failures);

        }

        private static void Dump(string format, IEnumerable<string> warnings)
        {
            Console.WriteLine("{0} :", format);
            foreach (var error in warnings)
            {
                Console.WriteLine("\"{0}\",", error);
            }
        }

        protected override Action Execute()
        {
            return () => { };
        }
    }
}