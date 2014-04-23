using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Call;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method
{
   public class IlInjectorAvailableVariablesForInstruction : IlInstructionInjectorAvailableVariables
   {
      private IlInjectorAvailableVariables methodVariables;

      public List<Instruction> calledInstructions = new List<Instruction>();
      public List<Instruction> calledParametersInstructions = new List<Instruction>();
      public List<Instruction> calledParametersObjectInstructions = new List<Instruction>();
      public List<Instruction> recallcalledInstructions = new List<Instruction>();
      public List<Instruction> recallcalledParametersInstructions = new List<Instruction>();
      private List<Instruction> beforeAfter = new List<Instruction>();

      private VariableDefinition _called;
      private Instruction instruction;
      private Dictionary<string, VariableDefinition> _calledParameters;
      private VariableDefinition _calledParametersObject;

      public List<VariableDefinition> Variables { get; private set; }
      public List<FieldDefinition> Fields { get; private set; }

      public IlInjectorAvailableVariablesForInstruction(IlInjectorAvailableVariables methodVariables, Instruction instruction)
      {
         this.methodVariables = methodVariables;
         this.instruction = instruction;
         Variables = new List<VariableDefinition>();
         Fields = new List<FieldDefinition>();
      }


      public VariableDefinition CurrentMethodBase
      {
         get { return methodVariables.CurrentMethodBase; }
      }

      public VariableDefinition Result
      {
         get { return methodVariables.Result; }
      }

      public VariableDefinition Parameters
      {
         get { return methodVariables.Parameters; }
      }

      public VariableDefinition Exception
      {
         get { return methodVariables.Exception; }
      }

      public VariableDefinition CurrentPropertyInfo
      {

         get { return methodVariables.CurrentPropertyInfo; }
      }

      public Dictionary<string, VariableDefinition> CalledParameters { get
      {
         if (_calledParameters == null)
         {
            if (instruction.IsAnUpdatePropertyCall())
            {
               var methodDefinition_L = ((MethodReference)instruction.Operand).Resolve();
               var property = methodDefinition_L.GetPropertyForSetter();
               _calledParameters = new Dictionary<string, VariableDefinition>();
               var propertyType_L = property.PropertyType;
               var variableDefinition = new VariableDefinition(propertyType_L);
               Variables.Add(variableDefinition);
               _calledParameters.Add("value", variableDefinition);
               calledInstructions.Add(Instruction.Create(OpCodes.Stloc, variableDefinition));
               recallcalledInstructions.Add(Instruction.Create(OpCodes.Ldloc, variableDefinition));
            }
            else if (instruction.Operand is MethodReference)
            {
               _calledParameters = new Dictionary<string, VariableDefinition>();
               var calledMethod = instruction.GetCalledMethod();
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
            else if (instruction.IsAnUpdateField())
            {
               _calledParameters = new Dictionary<string, VariableDefinition>();
               var fieldType = (instruction.Operand as FieldReference).Resolve().FieldType;
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
               if (instruction.Operand is MethodReference)
               {
                  var p = CalledParameters;
                  var calledMethod = instruction.GetCalledMethod();
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
               var operand = instruction.Operand as FieldReference;
               if (operand != null && !operand.Resolve().IsStatic)
                  declaringType = operand.DeclaringType;
               var methodReference = instruction.Operand as MethodReference;
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
      public VariableDefinition Field
      {
         get { return methodVariables.Field; }
      }
   }
}