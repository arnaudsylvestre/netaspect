using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Methods;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Core.V2
{
    public interface IIlBeforeInjectorAvailableVariables
    {
        VariableDefinition CurrentMethodInfo { get; }
    }



    public class MethodWeavingBeforeMethodInjector : IIlInjector<IIlBeforeInjectorAvailableVariables>
    {
        public void Check(ErrorHandler errorHandler, IIlBeforeInjectorAvailableVariables availableInformations)
        {
            ParametersEngineFactory.CreateForBeforeMethodWeaving()
        }

        public void Inject(Collection<Instruction> instructions, IIlBeforeInjectorAvailableVariables availableInformations)
        {
            throw new NotImplementedException();
        }
    }


    public class IlBeforeInjectorAvailableVariables : IIlBeforeInjectorAvailableVariables
    {
        private readonly VariableDefinition _result;
        public Collection<Instruction> Instructions = new Collection<Instruction>();
        private MethodDefinition method;
        private VariableDefinition currentMethodInfo;

        public IlBeforeInjectorAvailableVariables(VariableDefinition result)
        {
            _result = result;
        }


        public VariableDefinition CurrentMethodInfo { get
        {
            if (currentMethodInfo == null)
            {
                currentMethodInfo = method.CreateVariable<MethodInfo>();

                Instructions.AppendCallToThisGetType(method.Module);
                Instructions.AppendCallToGetMethod(method.Name, method.Module);
                Instructions.AppendSaveResultTo(currentMethodInfo);
            }
            return currentMethodInfo;
        } }

        public VariableDefinition Result
        {
            get { return _result; }
        }
    }

    public interface IIlInjector<TInfo>
    {
        void Check(ErrorHandler errorHandler, TInfo availableInformations);
        void Inject(Collection<Instruction> instructions, TInfo availableInformations);
    }


    public static class IIlInjectorsExtensions
    {
        public static void Check<TInfo>(this IEnumerable<IIlInjector<TInfo>> injectors, ErrorHandler errorHandler, TInfo info)
        {
            foreach (var ilInjector in injectors)
            {
                ilInjector.Check(errorHandler, info);
            }
        }
        public static void Inject<TInfo>(this IEnumerable<IIlInjector<TInfo>> injectors, Collection<Instruction> instructions, TInfo info)
        {
            foreach (var ilInjector in injectors)
            {
                ilInjector.Inject(instructions, info);
            }
        }
    }

   public class AroundMethodWeaver
   {
       public List<IIlInjector<IIlBeforeInjectorAvailableVariables>> Befores;
       public List<IIlInjector<IIlBeforeInjectorAvailableVariables>> Afters;
       public List<IIlInjector<IIlBeforeInjectorAvailableVariables>> OnExceptions;
       public List<IIlInjector<IIlBeforeInjectorAvailableVariables>> OnFinallys;
       private MethodToWeave method;

       public AroundMethodWeaver(MethodToWeave method)
        {
            this.method = method;
        }

       IlBeforeInjectorAvailableVariables variables;

        public void Weave()
        {
            if (!Befores.Any() && !Afters.Any() && !OnExceptions.Any() && !OnFinallys.Any())
            {
                return;
            }
            method.Method.MethodDefinition.Body.InitLocals = true;
            var beforeInstructions = new Collection<Instruction>();
            var beforeAfter = Instruction.Create(OpCodes.Nop);
            var afterInstructions = new Collection<Instruction>();
            var onExceptionInstructions = new Collection<Instruction>();
            var onFinallyInstructions = new Collection<Instruction>();
            var methodInstructions = new Collection<Instruction>(method.Method.MethodDefinition.Body.Instructions);
            
            var end = InstructionsHelper.FixReturns(method.Method.MethodDefinition, this.variables.Result, methodInstructions, beforeAfter);
            
            foreach (var before in Befores)
            {
                before.Inject(beforeInstructions, variables);
            }
            foreach (var injector in OnExceptions)
            {
                injector.Inject(onExceptionInstructions, variables);
            }
            foreach (var injector in Afters)
            {
                injector.Inject(afterInstructions, variables);
            }

            foreach (var injector in OnFinallys)
            {
                injector.Inject(onFinallyInstructions, variables);
            }
            var allInstructions = new List<Instruction>();
            allInstructions.AddRange(variables.Instructions);
            allInstructions.AddRange(beforeInstructions);
            allInstructions.AddRange(methodInstructions);
            if (end.Count != 0)
            {
               allInstructions.Add(beforeAfter);
               allInstructions.AddRange(afterInstructions);
                var gotoEnd = Instruction.Create(OpCodes.Leave, end.First());
                allInstructions.Add(gotoEnd);
            }


            
            allInstructions.AddRange(onExceptionInstructions);
            allInstructions.AddRange(onFinallyInstructions);
            allInstructions.AddRange(end);

           method.Method.MethodDefinition.Body.Instructions.Clear();
           method.Method.MethodDefinition.Body.Instructions.AddRange(allInstructions);


           if (onExceptionInstructions.Any())
           {
              Instruction lastCatchException = null;
              if (onFinallyInstructions.Any())
                 lastCatchException = onFinallyInstructions.First();
              else
              {
                 if (end.Count != 0)
                    lastCatchException = end.First();
              }

              method.Method.AddTryCatch(methodInstructions.First(), onExceptionInstructions.First(), onExceptionInstructions.First(), lastCatchException);
           }

           if (onFinallyInstructions.Any())
               method.Method.AddTryFinally(methodInstructions.First(), onFinallyInstructions.First(), onFinallyInstructions.First(), end.Count > 0 ? end.First() : null);

        }


        

       public void Check(ErrorHandler errorHandlerP_P)
       {
           VariableDefinition result = method.Method.MethodDefinition.ReturnType == method.Method.MethodDefinition.Module.TypeSystem.Void ? null : new VariableDefinition(method.Method.MethodDefinition.ReturnType);
           variables = new IlBeforeInjectorAvailableVariables(result);
           Befores.Check(errorHandlerP_P, variables);
           Afters.Check(errorHandlerP_P, variables);
           OnExceptions.Check(errorHandlerP_P, variables);
           OnFinallys.Check(errorHandlerP_P, variables);
       }
    }


}