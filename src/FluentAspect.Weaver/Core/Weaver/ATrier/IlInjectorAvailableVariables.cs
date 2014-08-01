using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method
{
   public class IlInjectorAvailableVariables
   {
      private readonly VariableDefinition _result;
      private readonly MethodDefinition method;
      private VariableDefinition _exception;
      private VariableDefinition _parameters;
      private VariableDefinition currentMethodInfo;
      private VariableDefinition currentPropertyInfo;
      private VariableDefinition _field;

      public List<Instruction> BeforeInstructions = new List<Instruction>();


      public List<Instruction> calledInstructions = new List<Instruction>();
      public List<Instruction> calledParametersInstructions = new List<Instruction>();
      public List<Instruction> calledParametersObjectInstructions = new List<Instruction>();
      public List<Instruction> recallcalledInstructions = new List<Instruction>();
      public List<Instruction> recallcalledParametersInstructions = new List<Instruction>();
      private List<Instruction> beforeAfter = new List<Instruction>();

      private VariableDefinition _called;
      public Instruction Instruction { get; private set; }
      private Dictionary<string, VariableDefinition> _calledParameters;
      private VariableDefinition _calledParametersObject;

      public List<VariableDefinition> Variables { get; private set; }
      public List<FieldDefinition> Fields { get; private set; }

      public VariableDefinition InterceptorVariable { get; set; }

      public IlInjectorAvailableVariables(VariableDefinition result, MethodDefinition method, Instruction instruction)
      {
         this.Instruction = instruction;
         Variables = new List<VariableDefinition>();
         Fields = new List<FieldDefinition>();
         _result = result;
         this.method = method;
      }


      public VariableDefinition CurrentMethodBase
      {
         get
         {
            if (currentMethodInfo == null)
            {
               currentMethodInfo = new VariableDefinition(method.Module.Import(typeof(MethodBase)));
               Variables.Add(currentMethodInfo);

               BeforeInstructions.Add(Instruction.Create(OpCodes.Call,
                                                   method.Module.Import(
                                                       typeof(MethodBase).GetMethod("GetCurrentMethod",
                                                                                     new Type[] { }))));
               BeforeInstructions.AppendSaveResultTo(currentMethodInfo);
            }
            return currentMethodInfo;
         }
      }

      public VariableDefinition Result
      {
         get { return _result; }
      }

      public VariableDefinition Parameters
      {
         get
         {
            if (_parameters == null)
            {
               _parameters = new VariableDefinition(method.Module.Import(typeof(object[])));
               Variables.Add(_parameters);

               method.FillArgsArrayFromParameters(BeforeInstructions, _parameters);
            }
            return _parameters;
         }
      }

      public VariableDefinition Exception
      {
         get
         {
            if (_exception == null)
            {
               _exception = new VariableDefinition(method.Module.Import(typeof(Exception)));
               Variables.Add(_exception);
            }
            return _exception;
         }
      }

      public VariableDefinition CurrentPropertyInfo
      {
         get
         {
            if (currentPropertyInfo == null)
            {
               currentPropertyInfo = new VariableDefinition(method.Module.Import(typeof(PropertyInfo)));
               Variables.Add(currentPropertyInfo);


               BeforeInstructions.Add(Instruction.Create(OpCodes.Call,
                                                   method.Module.Import(
                                                       typeof(MethodBase).GetMethod("GetCurrentMethod",
                                                                                     new Type[] { }))));
               BeforeInstructions.Add(Instruction.Create(OpCodes.Callvirt,
                                                   method.Module.Import(
                                                       typeof(MemberInfo).GetMethod("get_DeclaringType",
                                                                                     new Type[] { }))));
               BeforeInstructions.AppendCallToGetProperty(method.Name.Replace("get_", "").Replace("set_", ""),
                                                    method.Module);
               BeforeInstructions.AppendSaveResultTo(currentPropertyInfo);
            }
            return currentPropertyInfo;
         }
      }


      public VariableDefinition Field
      {
         get
         {
            if (_field == null)
            {

               _field = new VariableDefinition(method.Module.Import(typeof(FieldInfo)));
               Variables.Add(_field);
            }
            return _field;
         }
      }

      public Dictionary<string, VariableDefinition> CalledParameters { get
      {
         if (_calledParameters == null)
         {
            if (Instruction.IsAnUpdatePropertyCall())
            {
               var methodDefinition_L = ((MethodReference)Instruction.Operand).Resolve();
               var property = methodDefinition_L.GetPropertyForSetter();
               _calledParameters = new Dictionary<string, VariableDefinition>();
               var propertyType_L = property.PropertyType;
               var variableDefinition = new VariableDefinition(propertyType_L);
               Variables.Add(variableDefinition);
               _calledParameters.Add("value", variableDefinition);
               calledInstructions.Add(Instruction.Create(OpCodes.Stloc, variableDefinition));
               recallcalledInstructions.Add(Instruction.Create(OpCodes.Ldloc, variableDefinition));
            }
            else if (Instruction.Operand is MethodReference)
            {
               _calledParameters = new Dictionary<string, VariableDefinition>();
               var calledMethod = Instruction.GetCalledMethod();
               //var fieldType = (Instruction.Operand as FieldReference).Resolve().FieldType;
               //var variableDefinition = new VariableDefinition(fieldType);
               //calledInstructions.Add(Instruction.Create(OpCodes.Stloc, variableDefinition));
               foreach (var parameter in calledMethod.Parameters.Reverse())
               {
                  var variableDefinition = new VariableDefinition(parameter.ParameterType);
                  _calledParameters.Add("called" + parameter.Name, variableDefinition);
                  Variables.Add(variableDefinition);
                  calledParametersInstructions.Add(Instruction.Create(OpCodes.Stloc, variableDefinition));
               }
               foreach (var parameter in calledMethod.Parameters)
               {
                  recallcalledParametersInstructions.Add(Instruction.Create(OpCodes.Ldloc, _calledParameters["called" + parameter.Name]));
               }
            }
            else if (Instruction.IsAnUpdateField())
            {
               _calledParameters = new Dictionary<string, VariableDefinition>();
               var fieldType = (Instruction.Operand as FieldReference).Resolve().FieldType;
               var variableDefinition = new VariableDefinition(fieldType);
               Variables.Add(variableDefinition);
               _calledParameters.Add("value", variableDefinition);
               calledInstructions.Add(Instruction.Create(OpCodes.Stloc, variableDefinition));
               recallcalledInstructions.Add(Instruction.Create(OpCodes.Ldloc, variableDefinition));
            }
                
         }
         return _calledParameters;
      }}

      public VariableDefinition CalledParametersObject
      {
         get
         {
            if (_calledParametersObject == null)
            {
               if (Instruction.Operand is MethodReference)
               {
                  var p = CalledParameters;
                  var calledMethod = Instruction.GetCalledMethod();
                  _calledParametersObject = new VariableDefinition(calledMethod.Module.Import(typeof(object[])));
                  Variables.Add(_calledParametersObject);

                  calledMethod.FillCalledArgsArrayFromParameters(calledParametersObjectInstructions, _calledParametersObject, p);
               }

                    
            }
            return _calledParametersObject;
         }
      }

      public VariableDefinition Called
      {
         get
         {
            if (_called == null)
            {
               var calledParameters = CalledParameters;
               TypeReference declaringType = null;
               var operand = Instruction.Operand as FieldReference;
               if (operand != null && !operand.Resolve().IsStatic)
                  declaringType = operand.DeclaringType;
               var methodReference = Instruction.Operand as MethodReference;
               if (methodReference != null && !methodReference.Resolve().IsStatic)
                  declaringType = methodReference.DeclaringType;

               if (declaringType == null)
                  return null;

               _called = new VariableDefinition(declaringType);
               Variables.Add(_called);

               calledInstructions.Add(Instruction.Create(OpCodes.Stloc, _called));
               calledInstructions.Add(Instruction.Create(OpCodes.Ldloc, _called));
            }
            return _called;
         }
      }
   }
}