using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAspect.Weaver.Core.Errors;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Errors
{
    public class ErrorsTest : AcceptanceTest
    {
        protected override void EnsureErrorHandler(ErrorHandler errorHandler)
        {
            var builder = new StringBuilder();
            Dump(errorHandler, builder);
            Console.Write(builder);

            EnsureEquals(new List<string>
               {
                  "The aspect FluentAspect.Sample.MethodWeaving.Problems.Warnings.EmptyAspectAttribute doesn't have a Before/After/OnException method",
"The aspect FluentAspect.Sample.AOP.CheckWithVoidInterceptorAttribute doesn't have a Before/After/OnException method",
"The aspect FluentAspect.Sample.AOP.CheckNotRenameInAssemblyAttribute doesn't have a Before/After/OnException method",
               }, errorHandler.Warnings);

            EnsureEquals(new List<string>
            {
            }, errorHandler.Failures);
            EnsureEquals(new List<string>
                {
"The type FluentAspect.Sample.AOP.SelectorWithNoDefaultConstructorAttribute must have a default constructor",
"Wrong type for parameter methodName, expected System.String",
"impossible to ref the parameter 'method' in the method After of the type 'FluentAspect.Sample.MethodWeaving.Problems.Errors.ToCheckMethodReferencedInAfterAspectAttribute'",
"the result parameter in the method After of the type 'FluentAspect.Sample.MethodWeaving.Problems.Errors.ToCheckResultInVoidAspectAttribute' is declared with the type 'System.Int32&' but it is expected to be System.Void because the return type of the method Check in the type FluentAspect.Sample.MethodWeaving.Problems.Errors.ToCheckResultInVoid",
"impossible to ref the parameter 'instance' in the method After of the type 'FluentAspect.Sample.MethodWeaving.Problems.Errors.ToCheckInstanceReferencedInAfterAspectAttribute'",
"the method parameter in the method After of the type 'FluentAspect.Sample.MethodWeaving.Problems.Errors.ToCheckMethodWithWrongTypeInAfterAspectAttribute' is declared with the type 'System.Int32' but it is expected to be System.Reflection.MethodInfo",
"A method declared in interface can not be weaved : InterfaceToWeaveWithAttributes.CheckBeforeWithAttributes",
"impossible to ref the parameter 'parameters' in the method After of the type 'FluentAspect.Sample.AOP.CheckWithParametersReferencedAttribute'",
"the instance parameter in the method After of the type 'FluentAspect.Sample.MethodWeaving.Problems.Errors.ToCheckInstanceWithWrongTypeInAfterAspectAttribute' is declared with the type 'System.Int32' but it is expected to be System.Object or FluentAspect.Sample.MethodWeaving.Problems.Errors.ToCheckInstanceWithWrongTypeInAfter",
"An abstract method can not be weaved : AbstractClassToWeaveWithAttributes.CheckBeforeWithAttributes",
"impossible to ref the parameter 'parameters' in the method OnException of the type 'FluentAspect.Sample.MethodWeaving.Problems.Errors.ToCheckParametersReferencedOnExceptionAspectAttribute'",
"impossible to ref the parameter 'parameters' in the method Before of the type 'FluentAspect.Sample.MethodWeaving.Problems.Errors.ToCheckParametersReferencedInBeforeAspectAttribute'",
"impossible to ref the parameter 'parameters' in the method After of the type 'FluentAspect.Sample.MethodWeaving.Problems.Errors.ToCheckParametersReferencedInAfterAspectAttribute'",
"Wrong parameter type for lineNumber in method AfterCall of type FluentAspect.Sample.CheckAfterCallParameterTypeOnCallAfterAttribute",
"Wrong parameter type for columnNumber in method AfterCall of type FluentAspect.Sample.CheckAfterCallParameterTypeOnCallAfterAttribute",
"Wrong parameter type for fileName in method AfterCall of type FluentAspect.Sample.CheckAfterCallParameterTypeOnCallAfterAttribute",
"Wrong parameter type for filePath in method AfterCall of type FluentAspect.Sample.CheckAfterCallParameterTypeOnCallAfterAttribute",


                }, errorHandler.Errors);
        }

       private static void EnsureEquals(List<string> expected_P, List<string> actual)
       {
          Assert.AreEqual(
             (from e in expected_P orderby e select e).ToList(),
             (from e in actual orderby e select e).ToList());
       }

       public static void Dump(ErrorHandler errorHandler, StringBuilder builder)
        {
            Dump("Warnings", errorHandler.Warnings, builder);
            Dump("Errors", errorHandler.Errors, builder);
            Dump("Failures", errorHandler.Failures, builder);

        }

        private static void Dump(string format, IEnumerable<string> warnings, StringBuilder builder)
        {
            if (!warnings.Any())
                return;
            builder.AppendFormat("{0} :\n", format);
            foreach (var error in warnings)
            {
                builder.AppendFormat("\"{0}\",\n", error);
            }
        }

        protected override Action Execute()
        {
            return () => { };
        }
    }
}