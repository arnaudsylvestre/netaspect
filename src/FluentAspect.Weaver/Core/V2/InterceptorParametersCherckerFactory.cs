using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.V2;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Checkers;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters
{
    public static class InterceptorParametersIlGeneratorFactory
    {
        public static IInterceptorParameterIlGenerator CreateIlGeneratorForInstanceParameter(MethodDefinition method)
        {
            return new InstanceInterceptorParametersIlGenerator();
        }
        public static IInterceptorParameterIlGenerator CreateIlGeneratorForMethodParameter(MethodDefinition method)
        {
            return new MethodInterceptorParametersIlGenerator();
        }
    }

    public class MethodInterceptorParametersIlGenerator : IInterceptorParameterIlGenerator
    {
        public void GenerateIl(List<Instruction> instructions, IlInjectorAvailableVariables info)
        {
            instructions.Add(Instruction.Create(OpCodes.Ldloc, info.CurrentMethodInfo));
        }
    }

    public class InstanceInterceptorParametersIlGenerator : IInterceptorParameterIlGenerator
    {
        public void GenerateIl(List<Instruction> instructions, IlInjectorAvailableVariables info)
        {
            instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
        }
    }

    public interface IInterceptorParameterIlGenerator
    {
        void GenerateIl(List<Instruction> instructions, IlInjectorAvailableVariables info);
    }

    public static class InterceptorParametersCherckerFactory
    {
        public static IInterceptorParameterChecker CreateCheckerForInstanceParameter(MethodDefinition method)
        {
            return new InstanceInterceptorParametersChercker(method);
        }
        public static IInterceptorParameterChecker CreateCheckerForMethodParameter()
        {
            return new MethodInterceptorParametersChercker();
        }
        public static IInterceptorParameterChecker CreateCheckerForResultParameter(MethodDefinition method)
        {
            return new ResultInterceptorParametersChercker(method);
        }
        public static IInterceptorParameterChecker CreateCheckerForExceptionParameter(MethodDefinition method)
        {
            return new ExceptionInterceptorParametersChercker();
        }
        public static IEnumerable<IInterceptorParameterChecker> CreateCheckerForParameterNameParameter(MethodDefinition method)
        {
            foreach (var parameter in methodDefinition.Parameters)
            {
                try
                {
                    ParameterDefinition parameter1 = parameter;
                    engine.AddPossibleParameter(parameter1.Name,
                                        (p, handler) =>
                                        {
                                            Ensure.OfType(p, handler, parameter1);
                                        }, (p, _instructions) =>
                                            {
                                                var moduleDefinition = ((MethodDefinition)parameter.Method).Module;
                                                if (p.ParameterType.IsByRef && !parameter.ParameterType.IsByReference)
                                                {
                                                    _instructions.Add(Instruction.Create(OpCodes.Ldarga, parameter));

                                                }
                                                else if (!p.ParameterType.IsByRef && parameter.ParameterType.IsByReference)
                                                {
                                                    _instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter));
                                                    _instructions.Add(Instruction.Create(OpCodes.Ldobj, moduleDefinition.Import(p.ParameterType)));
                                                }
                                                else
                                                {
                                                    _instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter));

                                                }
                                                if (parameter.ParameterType != moduleDefinition.TypeSystem.Object &&
                                                    p.ParameterType == typeof(Object))
                                                {
                                                    TypeReference reference = parameter.ParameterType;
                                                    if (reference.IsByReference)
                                                    {
                                                        reference = ((MethodDefinition)parameter.Method).GenericParameters.First(t => t.Name == reference.Name.TrimEnd('&'));
                                                        _instructions.Add(Instruction.Create(OpCodes.Ldobj, reference));

                                                    }
                                                    _instructions.Add(Instruction.Create(OpCodes.Box, reference));
                                                }
                                            }
                                            );
                }
                catch (Exception)
                {
                    errorHandler.Errors.Add(string.Format("The parameter {0} is already declared", parameter.Name));
                }


            
            }
        }

            return new ParameterNameInterceptorParametersChercker();
        }

        //public static void AddParameterValue(this ParametersEngine engine, ParameterDefinition parameter)
        //{

        //   engine.AddPossibleParameter("value",
        //                               (p, handler) =>
        //                               {
        //                                  Ensure.NotOut(p, handler);
        //                                  Ensure.OfType(p, handler, parameter.ParameterType.FullName.Replace("/", "+"));
        //                               }, (info, instructions) => { instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter)); }

        //                                   );
        //}

        public static void AddParameterNames(this ParametersEngine engine, MethodDefinition methodDefinition, ErrorHandler errorHandler)
        {
            foreach (var parameter in methodDefinition.Parameters)
            {
                try
                {
                    ParameterDefinition parameter1 = parameter;
                    engine.AddPossibleParameter(parameter1.Name,
                                        (p, handler) =>
                                        {
                                            Ensure.OfType(p, handler, parameter1);
                                        }, (p, _instructions) =>
                                            {
                                                var moduleDefinition = ((MethodDefinition)parameter.Method).Module;
                                                if (p.ParameterType.IsByRef && !parameter.ParameterType.IsByReference)
                                                {
                                                    _instructions.Add(Instruction.Create(OpCodes.Ldarga, parameter));

                                                }
                                                else if (!p.ParameterType.IsByRef && parameter.ParameterType.IsByReference)
                                                {
                                                    _instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter));
                                                    _instructions.Add(Instruction.Create(OpCodes.Ldobj, moduleDefinition.Import(p.ParameterType)));
                                                }
                                                else
                                                {
                                                    _instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter));

                                                }
                                                if (parameter.ParameterType != moduleDefinition.TypeSystem.Object &&
                                                    p.ParameterType == typeof(Object))
                                                {
                                                    TypeReference reference = parameter.ParameterType;
                                                    if (reference.IsByReference)
                                                    {
                                                        reference = ((MethodDefinition)parameter.Method).GenericParameters.First(t => t.Name == reference.Name.TrimEnd('&'));
                                                        _instructions.Add(Instruction.Create(OpCodes.Ldobj, reference));

                                                    }
                                                    _instructions.Add(Instruction.Create(OpCodes.Box, reference));
                                                }
                                            }
                                            );
                }
                catch (Exception)
                {
                    errorHandler.Errors.Add(string.Format("The parameter {0} is already declared", parameter.Name));
                }


            
            }
        }
    }

    public class ExceptionInterceptorParametersChercker : IInterceptorParameterChecker
    {
        public void Check(ParameterInfo parameter, IErrorListener errorListener)
        {
            Ensure.NotReferenced(parameter, errorListener);
            Ensure.OfType(parameter, errorListener, typeof(Exception).FullName);
                                        //}, (info, instructions) => { instructions.Add(Instruction.Create(OpCodes.Ldloc, exception)); }
        }
    }

    public class ResultInterceptorParametersChercker : IInterceptorParameterChecker
    {
        private MethodDefinition methodDefinition;

        public ResultInterceptorParametersChercker(MethodDefinition methodDefinition)
        {
            this.methodDefinition = methodDefinition;
        }

        public void Check(ParameterInfo parameter, IErrorListener errorListener)
        {
            Ensure.ResultOfType(parameter, errorListener, methodDefinition);
            //instructions.Add(Instruction.Create(info.ParameterType.IsByRef ? OpCodes.Ldloca : OpCodes.Ldloc, result))
        }
    }

    public class MethodInterceptorParametersChercker : IInterceptorParameterChecker
    {

        public void Check(ParameterInfo parameter, IErrorListener errorListener)
        {
            Ensure.NotReferenced(parameter, errorListener);
            Ensure.OfType(parameter, errorListener, typeof(MethodInfo).FullName);
        }
    }
}