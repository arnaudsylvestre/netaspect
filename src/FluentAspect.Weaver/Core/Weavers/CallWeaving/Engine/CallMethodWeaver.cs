using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Weavers.Helpers;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Core.Weavers.Calls
{
   public class MethodPoint
   {
      public MethodDefinition Method { get; set; }
      public Instruction Instruction { get; set; }

      public override int GetHashCode()
      {
         unchecked
         {
            return ((Method != null ? Method.GetHashCode() : 0) * 397) ^ (Instruction != null ? Instruction.GetHashCode() : 0);
         }
      }

      protected bool Equals(MethodPoint other)
      {
         return Equals(Method, other.Method) && Equals(Instruction, other.Instruction);
      }

      /// <summary>
      /// Détermine si l'objet <see cref="T:System.Object"/> spécifié est égal à l'objet <see cref="T:System.Object"/> actuel.
      /// </summary>
      /// <returns>
      /// true si l'objet <see cref="T:System.Object"/> spécifié est égal à l'objet <see cref="T:System.Object"/> en cours ; sinon, false.
      /// </returns>
      /// <param name="obj"><see cref="T:System.Object"/> à comparer avec le <see cref="T:System.Object"/> actuel. 
      ///                 </param><exception cref="T:System.NullReferenceException">Le paramètre <paramref name="obj"/> est null.
      ///                 </exception>
      public override bool Equals(object obj)
      {
         if (ReferenceEquals(null, obj)) return false;
         if (ReferenceEquals(this, obj)) return true;
         if (obj.GetType() != this.GetType()) return false;
         return Equals((MethodPoint) obj);
      }
   }

    public class CallMethodWeaver : IWeaveable
    {
        private CallToWeave toWeave;


        public CallMethodWeaver(MethodPoint point,
                                IEnumerable<CallWeavingConfiguration> interceptorTypes)
        {
            toWeave = new CallToWeave
                {
                    MethodToWeave = point.Method,
                    Instruction                    = point.Instruction,
                    Interceptors                    = interceptorTypes
                };
        }

        public void Weave(ErrorHandler errorP_P)
        {
            var reference = toWeave.Instruction.Operand as MethodReference;

            SequencePoint point_L = toWeave.Instruction.GetLastSequencePoint();

            List<Instruction> instructions = new List<Instruction>();
            var variablesForParameters = new List<KeyValue>();
            foreach (var parameterDefinition_L in reference.Parameters.Reverse())
            {
               var variableDefinition_L = toWeave.MethodToWeave.CreateVariable(parameterDefinition_L.ParameterType);
               instructions.Add(Instruction.Create(OpCodes.Stloc, variableDefinition_L));
               variablesForParameters.Add(new KeyValue()
                   {
                       Variable = variableDefinition_L,
                       ParameterName                       = parameterDefinition_L.Name,
                   });
            }
           instructions.AddRange(CreateBeforeInstructions(toWeave.MethodToWeave.Module, point_L, variablesForParameters, reference));

            foreach (var variablesForParameter in ((IEnumerable<KeyValue>)variablesForParameters).Reverse())
            {
                instructions.Add(Instruction.Create(OpCodes.Ldloc, variablesForParameter.Variable));
            }

            var afterInstructions = CreateAfterInstructions(toWeave.MethodToWeave.Module, point_L, variablesForParameters, reference).ToList();
            toWeave.MethodToWeave.InsertAfter(toWeave.Instruction, afterInstructions);
            toWeave.MethodToWeave.InsertBefore(toWeave.Instruction, instructions);

        }

        public void Check(ErrorHandler errorHandler)
        {
            foreach (var netAspectAttribute in toWeave.Interceptors)
            {
                CheckParameters(netAspectAttribute.BeforeInterceptor.GetParameters(), errorHandler);
            }
        }

        private void CheckParameters(IEnumerable<ParameterInfo> getParameters, ErrorHandler errorHandler)
        {
            var errors = new Dictionary<string, Action<ParameterInfo, ErrorHandler>>();
            errors.Add("linenumber", (info, handler) =>
            {
                EnsureSequencePoint(errorHandler, info);
            });
            errors.Add("columnnumber", (info, handler) =>
            {
                EnsureSequencePoint(errorHandler, info);
            });
            errors.Add("filename", (info, handler) =>
            {
                EnsureSequencePoint(errorHandler, info);
            });
            errors.Add("filepath", (info, handler) =>
            {
                EnsureSequencePoint(errorHandler, info);
            });

            foreach (var parameterInfo in getParameters)
            {
                errors[parameterInfo.Name.ToLower()](parameterInfo, errorHandler);
            }
        }

        private void EnsureSequencePoint(ErrorHandler errorHandler, ParameterInfo info)
        {
            if (toWeave.Instruction.GetLastSequencePoint() == null)
                errorHandler.Warnings.Add(string.Format("The parameter {0} in method {1} of type {2} will have the default value because there is no debugging information",
                    info.Name, (info.Member).Name, (info.Member.DeclaringType).FullName));
        }

        class KeyValue
        {
            public VariableDefinition Variable;
            public string ParameterName;
        }

        public bool CanWeave()
        {
            return true;
        }

        private IEnumerable<Instruction> CreateAfterInstructions(ModuleDefinition module, SequencePoint instructionP_P, List<KeyValue> variableParameters, MethodReference reference)
        {
            var instructions = new List<Instruction>();
            foreach (var interceptorType in toWeave.Interceptors)
            {
                MethodInfo afterCallMethod = interceptorType.AfterInterceptor.Method;
                if (afterCallMethod != null)
                {
                    var parameters = new Dictionary<string, Action<ParameterInfo>>();
                    FillParameters(instructionP_P, parameters, instructions, variableParameters, reference);


                    foreach (ParameterInfo parameterInfo_L in afterCallMethod.GetParameters())
                    {
                        parameters[parameterInfo_L.Name.ToLower()](parameterInfo_L);
                    }

                    instructions.Add(Instruction.Create(OpCodes.Call, module.Import(afterCallMethod)));
                }
            }
            return instructions;
        }

        private void FillParameters(SequencePoint instructionP_P, Dictionary<string, Action<ParameterInfo>> parameters, List<Instruction> instructions, List<KeyValue> variablesForParameters, MethodReference reference_P)
        {
           foreach (var parameterDefinition_L in reference_P.Parameters.Reverse())
           {
              ParameterDefinition definition_L = parameterDefinition_L;
               var variable =
                   (from v in variablesForParameters where v.ParameterName == definition_L.Name select v.Variable).First
                       ();
               parameters.Add((parameterDefinition_L.Name + "Called").ToLower(), p => instructions.Add(Instruction.Create(OpCodes.Ldloc, variable)));
           }

           parameters.Add("linenumber", p => instructions.Add(Create(instructionP_P, i => i.StartLine)));
            parameters.Add("columnnumber", p => instructions.Add(Create(instructionP_P, i => i.StartColumn)));
            parameters.Add("filename", p => instructions.Add(Create(instructionP_P, i => Path.GetFileName(i.Document.Url))));
            parameters.Add("filepath", p => instructions.Add(Create(instructionP_P, i => i.Document.Url)));
            parameters.Add("caller", p => instructions.Add(Instruction.Create(OpCodes.Ldarg_0)));

            foreach (ParameterDefinition parameter_L in toWeave.MethodToWeave.Parameters)
            {
                ParameterDefinition parameter1_L = parameter_L;
                parameters.Add((parameter1_L.Name + "Caller").ToLower(), p => instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter1_L)));
            }

        }

        private static Instruction Create(SequencePoint instructionP_P, Func<SequencePoint, int> provider)
        {
            return Instruction.Create(OpCodes.Ldc_I4,
                                      instructionP_P == null
                                          ? 0
                                          : provider(instructionP_P));
        }

        private static Instruction Create(SequencePoint instructionP_P, Func<SequencePoint, string> provider)
        {
            return Instruction.Create(OpCodes.Ldstr,
                                      instructionP_P == null
                                          ? null
                                          : provider(instructionP_P));
        }

        private IEnumerable<Instruction> CreateBeforeInstructions(ModuleDefinition module, SequencePoint pointL, List<KeyValue> variableParameters, MethodReference reference)
        {
            var instructions = new List<Instruction>();
            foreach (var interceptorType in toWeave.Interceptors)
            {
                if (interceptorType.BeforeInterceptor.Method != null)
                {
                    var parameters = new Dictionary<string, Action<ParameterInfo>>();
                    FillParameters(pointL, parameters, instructions, variableParameters, reference);
                    instructions.Add(Instruction.Create(OpCodes.Call,
                                                        module.Import(
                                                            interceptorType.BeforeInterceptor
                                                                           .Method)));
                }
            }
            return instructions;
        }
    }
}