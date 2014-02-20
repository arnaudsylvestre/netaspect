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
        public static void CreateIlGeneratorForInstanceParameter(this ParametersIlGenerator ilGeneratoir, MethodDefinition method)
        {
            ilGeneratoir.Add("instance", new InstanceInterceptorParametersIlGenerator());
        }
        public static void CreateIlGeneratorForMethodParameter(this ParametersIlGenerator ilGeneratoir)
        {
            ilGeneratoir.Add("method", new MethodInterceptorParametersIlGenerator());
        }
        public static void CreateIlGeneratorForParameterNameParameter(this ParametersIlGenerator ilGeneratoir, MethodDefinition method)
        {
            foreach (var parameterDefinition in method.Parameters)
            {
                ilGeneratoir.Add(parameterDefinition.Name.ToLower(), new ParameterNameInterceptorParametersIlGenerator(parameterDefinition));
                
            }

        }

        //engine.AddPossibleParameter(parameter1.Name,
        //                                (p, handler) =>
        //                                {
        //                                    Ensure.OfType(p, handler, parameter1);
        //                                }, (p, _instructions) =>
        //                                    {
        //                                        var moduleDefinition = ((MethodDefinition)parameter.Method).Module;
        //                                        if (p.ParameterType.IsByRef && !parameter.ParameterType.IsByReference)
        //                                        {
        //                                            _instructions.Add(Instruction.Create(OpCodes.Ldarga, parameter));

        //                                        }
        //                                        else if (!p.ParameterType.IsByRef && parameter.ParameterType.IsByReference)
        //                                        {
        //                                            _instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter));
        //                                            _instructions.Add(Instruction.Create(OpCodes.Ldobj, moduleDefinition.Import(p.ParameterType)));
        //                                        }
        //                                        else
        //                                        {
        //                                            _instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter));

        //                                        }
        //                                        if (parameter.ParameterType != moduleDefinition.TypeSystem.Object &&
        //                                            p.ParameterType == typeof(Object))
        //                                        {
        //                                            TypeReference reference = parameter.ParameterType;
        //                                            if (reference.IsByReference)
        //                                            {
        //                                                reference = ((MethodDefinition)parameter.Method).GenericParameters.First(t => t.Name == reference.Name.TrimEnd('&'));
        //                                                _instructions.Add(Instruction.Create(OpCodes.Ldobj, reference));

        //                                            }
        //                                            _instructions.Add(Instruction.Create(OpCodes.Box, reference));
        //                                        }
        //                                    }
        //                                    );
    }

    public class ParameterNameInterceptorParametersIlGenerator : IInterceptorParameterIlGenerator
    {
        private ParameterDefinition parameter;

        public ParameterNameInterceptorParametersIlGenerator(ParameterDefinition parameter)
        {
            this.parameter = parameter;
        }

        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, IlInjectorAvailableVariables info)
        {
            var moduleDefinition = ((MethodDefinition)parameter.Method).Module;
            if (parameterInfo.ParameterType.IsByRef && !parameter.ParameterType.IsByReference)
            {
                instructions.Add(Instruction.Create(OpCodes.Ldarga, parameter));

            }
            else if (!parameterInfo.ParameterType.IsByRef && parameter.ParameterType.IsByReference)
            {
                instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter));
                instructions.Add(Instruction.Create(OpCodes.Ldobj, moduleDefinition.Import(parameterInfo.ParameterType)));
            }
            else
            {
                instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter));

            }
            if (parameter.ParameterType != moduleDefinition.TypeSystem.Object &&
                parameterInfo.ParameterType == typeof(Object))
            {
                TypeReference reference = parameter.ParameterType;
                if (reference.IsByReference)
                {
                    reference = ((MethodDefinition)parameter.Method).GenericParameters.First(t => t.Name == reference.Name.TrimEnd('&'));
                    instructions.Add(Instruction.Create(OpCodes.Ldobj, reference));

                }
                instructions.Add(Instruction.Create(OpCodes.Box, reference));
            }
        }
    }

    public class MethodInterceptorParametersIlGenerator : IInterceptorParameterIlGenerator
    {
        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, IlInjectorAvailableVariables info)
        {
            instructions.Add(Instruction.Create(OpCodes.Ldloc, info.CurrentMethodInfo));
        }
    }

    public class InstanceInterceptorParametersIlGenerator : IInterceptorParameterIlGenerator
    {
        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, IlInjectorAvailableVariables info)
        {
            instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
        }
    }

    public interface IInterceptorParameterIlGenerator
    {
        void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, IlInjectorAvailableVariables info);
    }

    public class InterceptorParametersChecker
    {
        public string ParameterName { get; set; }
        public IInterceptorParameterChecker Checker { get; set; }
    }


    public static class InterceptorParametersCherckerFactory
    {
        public static void CreateCheckerForInstanceParameter(this ParametersChecker checkers, MethodDefinition method)
        {
            checkers.Add(new InterceptorParametersChecker()
                {
                    ParameterName = "instance",
                    Checker = new InstanceInterceptorParametersChercker(method),
                });
        }
        public static void CreateCheckerForMethodParameter(this ParametersChecker checkers)
        {

            checkers.Add(new InterceptorParametersChecker()
            {
                ParameterName = "method",
                Checker = new MethodInterceptorParametersChercker(),
            });
        }
        public static void CreateCheckerForResultParameter(this ParametersChecker checkers, MethodDefinition method)
        {

            checkers.Add(new InterceptorParametersChecker()
            {
                ParameterName = "method",
                Checker = new ResultInterceptorParametersChercker(method),
            });
        }
        public static void CreateCheckerForExceptionParameter(this ParametersChecker checkers)
        {
            checkers.Add(new InterceptorParametersChecker()
            {
                ParameterName = "exception",
                Checker = new ExceptionInterceptorParametersChercker(),
            });
        }
        public static void CreateCheckerForParameterNameParameter(this ParametersChecker checkers, MethodDefinition methodDefinition)
        {
            checkers.AddRange(methodDefinition.Parameters.Select(parameter => new InterceptorParametersChecker()
                {
                    ParameterName = parameter.Name, Checker = new ParameterNameInterceptorParametersChercker(parameter),
                }));
        }

        //    return new ParameterNameInterceptorParametersChercker();
        //}

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

        
    }

    public class ParameterNameInterceptorParametersChercker : IInterceptorParameterChecker
    {
        private readonly ParameterDefinition _parameterName;

        public ParameterNameInterceptorParametersChercker(ParameterDefinition parameterName)
        {
            _parameterName = parameterName;
        }

        public void Check(ParameterInfo parameter, IErrorListener errorListener)
        {

            Ensure.OfType(parameter, errorListener, _parameterName);
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