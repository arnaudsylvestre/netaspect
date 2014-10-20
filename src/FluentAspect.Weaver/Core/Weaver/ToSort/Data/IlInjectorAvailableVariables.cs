using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using NetAspect.Weaver.Helpers.Mono.Cecil.IL;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Data
{
   public class IlInjectorAvailableVariables
   {
      private readonly VariableDefinition _result;
      private readonly MethodDefinition method;
      private VariableDefinition _called;
      private Dictionary<string, VariableDefinition> _calledParameters;
      private VariableDefinition _calledParametersObject;
      private VariableDefinition _exception;
      private VariableDefinition _field;
      private VariableDefinition _fieldValue;
      private VariableDefinition _parameters;
      private VariableDefinition _resultForInstruction;

      private VariableDefinition currentMethodInfo;
      private VariableDefinition currentPropertyInfo;

       readonly InstructionsToInsert instructionsToInsert;

      public IlInjectorAvailableVariables(VariableDefinition result, MethodDefinition method, Mono.Cecil.Cil.Instruction instruction, InstructionsToInsert instructionsToInsert_P)
      {
         Instruction = instruction;
         instructionsToInsert = instructionsToInsert_P;
         Variables = new List<VariableDefinition>();
         Fields = new List<FieldDefinition>();
         _result = result;
         this.method = method;
      }

      public Mono.Cecil.Cil.Instruction Instruction { get; private set; }

      public List<VariableDefinition> Variables { get; private set; }
      public List<FieldDefinition> Fields { get; private set; }

      public VariableDefinition InterceptorVariable { get; set; }


      public VariableDefinition CurrentMethodBase
      {
         get
         {
            if (currentMethodInfo == null)
            {
               currentMethodInfo = new VariableDefinition(method.Module.Import(typeof (MethodBase)));
               Variables.Add(currentMethodInfo);

               instructionsToInsert.BeforeInstructions.Add(
                  Mono.Cecil.Cil.Instruction.Create(
                     OpCodes.Call,
                     method.Module.Import(
                        typeof (MethodBase).GetMethod(
                           "GetCurrentMethod",
                           new Type[] {}))));
               instructionsToInsert.BeforeInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, currentMethodInfo));
            }
            return currentMethodInfo;
         }
      }

      public VariableDefinition Result
      {
         get { return _result; }
      }

      public VariableDefinition ResultForInstruction
      {
         get
         {
            if (_resultForInstruction == null)
            {
               if (Instruction.Operand is MethodReference)
               {
                  var method = Instruction.Operand as MethodReference;
                  if (method.ReturnType == method.Module.TypeSystem.Void)
                     return null;
                  _resultForInstruction = new VariableDefinition(method.ReturnType);
                  Variables.Add(_resultForInstruction);
                  instructionsToInsert.resultInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, _resultForInstruction));
                  instructionsToInsert.resultInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldloc, _resultForInstruction));
               }
               if (Instruction.IsAGetField())
               {
                  var fieldReference_L = Instruction.Operand as FieldReference;
                  _resultForInstruction = new VariableDefinition(fieldReference_L.FieldType);
                  Variables.Add(_resultForInstruction);
                  instructionsToInsert.resultInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, _resultForInstruction));
                  instructionsToInsert.resultInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldloc, _resultForInstruction));
               }
            }
            return _resultForInstruction;
         }
      }

      public VariableDefinition Parameters
      {
         get
         {
            if (_parameters == null)
            {
               _parameters = new VariableDefinition(method.Module.Import(typeof (object[])));
               Variables.Add(_parameters);

               method.FillArgsArrayFromParameters(instructionsToInsert.BeforeInstructions, _parameters);
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
               _exception = new VariableDefinition(method.Module.Import(typeof (Exception)));
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
               currentPropertyInfo = new VariableDefinition(method.Module.Import(typeof (PropertyInfo)));
               Variables.Add(currentPropertyInfo);


               instructionsToInsert.BeforeInstructions.Add(
                  Mono.Cecil.Cil.Instruction.Create(
                     OpCodes.Call,
                     method.Module.Import(
                        typeof (MethodBase).GetMethod(
                           "GetCurrentMethod",
                           new Type[] {}))));
               instructionsToInsert.BeforeInstructions.Add(
                  Mono.Cecil.Cil.Instruction.Create(
                     OpCodes.Callvirt,
                     method.Module.Import(
                        typeof (MemberInfo).GetMethod(
                           "get_DeclaringType",
                           new Type[] {}))));
               instructionsToInsert.BeforeInstructions.AppendCallToGetProperty(
                  method.Name.Replace("get_", "").Replace("set_", ""),
                  method.Module);
               instructionsToInsert.BeforeInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, currentPropertyInfo));
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
               _field = new VariableDefinition(method.Module.Import(typeof (FieldInfo)));
               Variables.Add(_field);
            }
            return _field;
         }
      }

      public Dictionary<string, VariableDefinition> CalledParameters
      {
         get
         {
            if (_calledParameters == null)
            {
               if (Instruction.IsAnUpdatePropertyCall())
               {
                  MethodDefinition methodDefinition_L = ((MethodReference) Instruction.Operand).Resolve();
                  PropertyDefinition property = methodDefinition_L.GetPropertyForSetter();
                  _calledParameters = new Dictionary<string, VariableDefinition>();
                  TypeReference propertyType_L = property.PropertyType;
                  var variableDefinition = new VariableDefinition(propertyType_L);
                  Variables.Add(variableDefinition);
                  _calledParameters.Add("value", variableDefinition);
                  instructionsToInsert.calledInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, variableDefinition));
                  instructionsToInsert.recallcalledInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldloc, variableDefinition));
               }
               else if (Instruction.Operand is MethodReference)
               {
                  _calledParameters = new Dictionary<string, VariableDefinition>();
                  MethodDefinition calledMethod = Instruction.GetCalledMethod();
                  //var fieldType = (Instruction.Operand as FieldReference).Resolve().FieldType;
                  //var variableDefinition = new VariableDefinition(fieldType);
                  //calledInstructions.Add(Instruction.Create(OpCodes.Stloc, variableDefinition));
                  foreach (ParameterDefinition parameter in calledMethod.Parameters.Reverse())
                  {
                     var variableDefinition = new VariableDefinition(ComputeVariableType(parameter, Instruction));
                     _calledParameters.Add("called" + parameter.Name, variableDefinition);
                     Variables.Add(variableDefinition);
                     instructionsToInsert.calledParametersInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, variableDefinition));
                  }
                  foreach (ParameterDefinition parameter in calledMethod.Parameters)
                  {
                     instructionsToInsert.recallcalledParametersInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldloc, _calledParameters["called" + parameter.Name]));
                  }
               }
               else if (Instruction.IsAnUpdateField())
               {
                  _calledParameters = new Dictionary<string, VariableDefinition>();
                  TypeReference fieldType = (Instruction.Operand as FieldReference).Resolve().FieldType;
                  var variableDefinition = new VariableDefinition(fieldType);
                  Variables.Add(variableDefinition);
                  _calledParameters.Add("value", variableDefinition);
                  instructionsToInsert.calledInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, variableDefinition));
                  instructionsToInsert.recallcalledInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldloc, variableDefinition));
               }
            }
            return _calledParameters;
         }
      }

      public VariableDefinition CalledParametersObject
      {
         get
         {
            if (_calledParametersObject == null)
            {
               if (Instruction.Operand is MethodReference)
               {
                  Dictionary<string, VariableDefinition> p = CalledParameters;
                  MethodDefinition calledMethod = Instruction.GetCalledMethod();
                  _calledParametersObject = new VariableDefinition(calledMethod.Module.Import(typeof (object[])));
                  Variables.Add(_calledParametersObject);

                  calledMethod.FillCalledArgsArrayFromParameters(instructionsToInsert.calledParametersObjectInstructions, _calledParametersObject, p);
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
               Dictionary<string, VariableDefinition> calledParameters = CalledParameters;
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

               instructionsToInsert.calledInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, _called));
               instructionsToInsert.calledInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldloc, _called));
            }
            return _called;
         }
      }

      public VariableDefinition FieldValue
      {
         get
         {
            if (_fieldValue == null)
            {
               //if (Instruction.IsAnUpdateFieldCall())
               {
                  FieldDefinition fieldDefinition = ((FieldReference) Instruction.Operand).Resolve();
                  TypeReference propertyType_L = fieldDefinition.FieldType;
                  _fieldValue = new VariableDefinition(propertyType_L);
                  Variables.Add(_fieldValue);
                  instructionsToInsert.calledInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, _fieldValue));
                  instructionsToInsert.recallcalledInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldloc, _fieldValue));
               }
            }
            return _fieldValue;
         }
      }

      public static TypeReference ComputeVariableType(ParameterDefinition parameter, Mono.Cecil.Cil.Instruction instruction)
      {
         if (instruction.Operand is GenericInstanceMethod && parameter.ParameterType is GenericParameter)
         {
            var method = (GenericInstanceMethod) instruction.Operand;
            var genericParameter = (GenericParameter) parameter.ParameterType;
            Collection<GenericParameter> genericParameters = ((MethodReference) parameter.Method).GenericParameters;
            int index = -1;
            for (int i = 0; i < genericParameters.Count; i++)
            {
               if (genericParameters[i] == genericParameter)
               {
                  index = i;
                  break;
               }
            }
            if (index != -1)
            {
               return method.GenericArguments[index];
            }
         }
         return parameter.ParameterType;
      }
   }
}
