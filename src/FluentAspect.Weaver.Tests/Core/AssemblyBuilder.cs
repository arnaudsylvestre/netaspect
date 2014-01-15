using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Tests.acceptance;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NUnit.Framework;
using FieldAttributes = Mono.Cecil.FieldAttributes;
using MethodAttributes = Mono.Cecil.MethodAttributes;
using ParameterAttributes = Mono.Cecil.ParameterAttributes;
using TypeAttributes = Mono.Cecil.TypeAttributes;

namespace FluentAspect.Weaver.Tests.Core
{
    public class AssemblyBuilder
    {
        public static AssemblyDefinitionDefiner Create()
        {
            return Create("Temp");
        }
        public static AssemblyDefinitionDefiner Create(string name)
        {
            return new AssemblyDefinitionDefiner(AssemblyDefinition.CreateAssembly(new AssemblyNameDefinition(name, new Version("1.0")), name, ModuleKind.Dll));
        }
    }

    public class TypeDefinitionDefiner
    {
        private TypeDefinition type;

        public TypeDefinitionDefiner(TypeDefinition type)
        {
            this.type = type;
        }

        public TypeDefinition Type
        {
            get { return type; }
        }

        public MethodDefinitionDefiner WithMethod(string methodName)
        {
            var methodDefinition = new MethodDefinition(methodName, MethodAttributes.Public, type.Module.TypeSystem.Void);
           //methodDefinition.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            type.Methods.Add(methodDefinition);
            return new MethodDefinitionDefiner(methodDefinition);
        }

        public FieldDefinitionDefiner WithField<T>(string fieldName)
        {
            var fieldDefinition = new FieldDefinition(fieldName, FieldAttributes.Public, type.Module.Import(typeof (T)));
            type.Fields.Add(fieldDefinition);
            return new FieldDefinitionDefiner(fieldDefinition);
        }
    }

    public class FieldDefinitionDefiner
    {
        private readonly FieldDefinition _fieldDefinition;

        public FieldDefinitionDefiner(FieldDefinition fieldDefinition)
        {
            _fieldDefinition = fieldDefinition;
        }

        public void AddAspect(CallFieldWeavingAspectDefiner aspect)
        {
            _fieldDefinition.CustomAttributes.Add(new CustomAttribute(aspect.Constructor));
        }
    }

    public class AssemblyDefinitionDefiner
    {
        public const string DefaultNamespace = "A";

        private readonly AssemblyDefinition _assemblyDefinition;

        public AssemblyDefinitionDefiner(AssemblyDefinition assemblyDefinition)
        {
            _assemblyDefinition = assemblyDefinition;
        }

        public TypeDefinitionDefiner WithType(string typeName)
        {
           var typeDefinition = CreateType(typeName, _assemblyDefinition.MainModule.TypeSystem.Object);
           var typeDefinitionDefiner_L = new TypeDefinitionDefiner(typeDefinition);
           var constructorInfo_L = typeof (object).GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)[0];

           MethodWeavingAspectDefiner.AddEmptyConstructor(typeDefinition, typeDefinition.Module.Import(constructorInfo_L), new List<Instruction>());
           return typeDefinitionDefiner_L;
        }

        private TypeDefinition CreateType(string typeName, TypeReference parent)
        {
           var typeDefinition = new TypeDefinition(DefaultNamespace, typeName, TypeAttributes.Public/* | TypeAttributes.AutoClass*/, parent);
            _assemblyDefinition.MainModule.Types.Add(typeDefinition);
            return typeDefinition;
        }

        public CallWeavingAspectDefiner WithCallWeavingAspect(string typeName)
        {
           return new CallWeavingAspectDefiner(CreateType(typeName, _assemblyDefinition.MainModule.Import(typeof(Attribute))));
        }

        public MethodWeavingAspectDefiner WithMethodWeavingAspect(string typeName)
        {
            return new MethodWeavingAspectDefiner(CreateType(typeName, _assemblyDefinition.MainModule.Import(typeof(Attribute))));
        }

        public void Save(string filename)
        {
            _assemblyDefinition.Write(filename, new WriterParameters()
               {
                  WriteSymbols = true
               });
        }

        public CallFieldWeavingAspectDefiner WithCallFieldWeavingAspect(string aspectName)
        {
            return new CallFieldWeavingAspectDefiner(CreateType(aspectName, _assemblyDefinition.MainModule.Import(typeof(Attribute))));
        }
    }

    public class CallFieldWeavingAspectDefiner
    {
        private readonly TypeDefinition _typeDefinition;

        public CallFieldWeavingAspectDefiner(TypeDefinition typeDefinition)
        {
            _typeDefinition = typeDefinition;
            typeDefinition.BaseType = typeDefinition.Module.Import(typeof(Attribute));
            var fieldDefinition_L = new FieldDefinition("NetAspectAttributeKind", FieldAttributes.Public, typeDefinition.Module.TypeSystem.String);
            typeDefinition.Fields.Add(fieldDefinition_L);

            var instructions_L = new List<Instruction>();
            instructions_L.Add(Instruction.Create(OpCodes.Ldarg_0));
            instructions_L.Add(Instruction.Create(OpCodes.Ldstr, "CallWeaving"));
            instructions_L.Add(Instruction.Create(OpCodes.Stfld, fieldDefinition_L));

            var constructorInfo_L = typeof(Attribute).GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)[0];
            DefaultConstructor = MethodWeavingAspectDefiner.AddEmptyConstructor(typeDefinition, typeDefinition.Module.Import(constructorInfo_L), instructions_L);
        }

        protected MethodDefinition DefaultConstructor { get; set; }

        public MethodDefinition Constructor
        {
            get { return DefaultConstructor; }
        }

        public MethodDefinitionDefiner AddBeforeFieldAccess()
        {
           return MethodWeavingAspectDefiner.CreateInterceptor(_typeDefinition, "BeforeFieldAccess", true);
        }

       public MethodDefinitionDefiner AddAfterFieldAccess()
        {
          return MethodWeavingAspectDefiner.CreateInterceptor(_typeDefinition, "AfterFieldAccess", true);
        }

    }

    public class MethodWeavingAspectDefiner
    {
        private readonly TypeDefinition typeDefinition;
       private MethodDefinition DefaultConstructor;

        public TypeDefinition TypeDefinition
        {
            get { return typeDefinition; }
        }

        public MethodWeavingAspectDefiner(TypeDefinition typeDefinition)
        {
            this.typeDefinition = typeDefinition;
            typeDefinition.BaseType = typeDefinition.Module.Import(typeof (Attribute));
           var fieldDefinition_L = new FieldDefinition("NetAspectAttributeKind", FieldAttributes.Public, typeDefinition.Module.TypeSystem.String);
           typeDefinition.Fields.Add(fieldDefinition_L);

           var instructions_L = new List<Instruction>();
           instructions_L.Add(Instruction.Create(OpCodes.Ldarg_0));
           instructions_L.Add(Instruction.Create(OpCodes.Ldstr, "MethodWeaving"));
           instructions_L.Add(Instruction.Create(OpCodes.Stfld, fieldDefinition_L));

           var constructorInfo_L = typeof (Attribute).GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)[0];
           DefaultConstructor = AddEmptyConstructor(typeDefinition, typeDefinition.Module.Import(constructorInfo_L), instructions_L);
        }

        public static MethodDefinition AddEmptyConstructor(TypeDefinition type, MethodReference baseEmptyConstructor, IEnumerable<Instruction> instructions)
        {
           var methodAttributes = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
           var method = new MethodDefinition(".ctor", methodAttributes, type.Module.TypeSystem.Void);
           foreach (var instruction in instructions)
           {
              method.Body.Instructions.Add(instruction);
           }
           method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
           method.Body.Instructions.Add(Instruction.Create(OpCodes.Call, baseEmptyConstructor));
           method.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
           type.Methods.Add(method);
           return method;
        }

        public MethodReference Constructor
        {
           get { return DefaultConstructor; }
        }

       public object GetBeforeInstance(DoAcceptanceHelper helper)
       {
          return helper.GetNetAspectAttribute(typeDefinition.Name);
       }

       public MethodDefinitionDefiner AddBefore()
        {
           return CreateInterceptor(typeDefinition, "Before", false);
        }

        public MethodDefinitionDefiner AddAfter()
        {
           return CreateInterceptor(typeDefinition, "After", false);
        }

        public MethodDefinitionDefiner AddOnException()
        {
           return CreateInterceptor(typeDefinition, "OnException", false);
        }

        public static MethodDefinitionDefiner CreateInterceptor(TypeDefinition typeDefinition, string before, bool isStatic)
        {
            var firstOrDefault = (from e in typeDefinition.Methods where e.Name == before select e).FirstOrDefault();
            if (firstOrDefault != null)
                return new MethodDefinitionDefiner(firstOrDefault);

           var methodAttributes_L = MethodAttributes.Public;
           if (isStatic)
              methodAttributes_L |= MethodAttributes.Static;
           var definition = new MethodDefinition(before, methodAttributes_L, typeDefinition.Module.Import(typeof (void)));
            typeDefinition.Methods.Add(definition);
            definition.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            return new MethodDefinitionDefiner(definition);
        }
    }

    public class CallWeavingAspectDefiner
    {
        private TypeDefinition typeDefinition;

        public CallWeavingAspectDefiner(TypeDefinition typeDefinition)
        {
            this.typeDefinition = typeDefinition;
        }

        public MethodDefinitionDefiner AddBeforeCall()
        {
            var definition = new MethodDefinition("BeforeCall", MethodAttributes.Public, typeDefinition.Module.Import(typeof (object)));
            typeDefinition.Methods.Add(definition);
            return new MethodDefinitionDefiner(definition);
        }
    }

    public class MethodDefinitionDefiner
    {
        private readonly MethodDefinition _definition;

        public MethodDefinitionDefiner(MethodDefinition definition)
        {
            _definition = definition;
        }

       public MethodDefinition Definition
       {
          get { return _definition; }
       }

       public MethodDefinitionDefiner WithParameter<T>(string name)
        {
            var parameterType = _definition.Module.Import(typeof(T));
            var parameterDefinition = new ParameterDefinition(name, ParameterAttributes.None, parameterType);
            _definition.Parameters.Add(parameterDefinition);
            var fieldDefinition = new FieldDefinition(_definition.Name + name, FieldAttributes.Public | FieldAttributes.Static, parameterType);
            _definition.DeclaringType.Fields.Add(fieldDefinition);

            _definition.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Ldarg_0));
            _definition.Body.Instructions.Insert(1, Instruction.Create(OpCodes.Ldarg, parameterDefinition));
            _definition.Body.Instructions.Insert(2, Instruction.Create(OpCodes.Stfld, fieldDefinition));

            return this;
        }

        public MethodDefinitionDefiner WithReferencedParameter<T>(string name)
        {
            var parameterType = _definition.Module.Import(typeof(T));
            var parameterDefinition = new ParameterDefinition(name, ParameterAttributes.None, new ByReferenceType(parameterType));
            _definition.Parameters.Add(parameterDefinition);
            var fieldDefinition = new FieldDefinition(_definition.Name + name, FieldAttributes.Public | FieldAttributes.Static, parameterType);
            _definition.DeclaringType.Fields.Add(fieldDefinition);

            //_definition.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Ldarg_0));
            _definition.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Ldarg, parameterDefinition));
            _definition.Body.Instructions.Insert(1, Instruction.Create(OpCodes.Ldind_Ref));
            _definition.Body.Instructions.Insert(2, Instruction.Create(OpCodes.Stsfld, fieldDefinition));

            return this;
        }

        public MethodDefinitionDefiner WhichRaiseException()
        {
            var index = _definition.Body.Instructions.FindIndex(instruction => instruction.OpCode == OpCodes.Ret);

            _definition.Body.Instructions.Insert(index, Instruction.Create(OpCodes.Newobj, _definition.Module.Import(typeof(Exception).GetConstructor(new Type[0]))));
            _definition.Body.Instructions.Insert(index + 1, Instruction.Create(OpCodes.Throw));
            return this;
        }

        public void AddAspect(MethodWeavingAspectDefiner aspect)
        {
            _definition.CustomAttributes.Add(new CustomAttribute(aspect.Constructor));
        }

        public MethodDefinitionDefiner WithParameter(string name, TypeDefinition parameterType)
        {
            var parameterDefinition = new ParameterDefinition(name, ParameterAttributes.None, parameterType);
            _definition.Parameters.Add(parameterDefinition);
            var fieldDefinition = new FieldDefinition(_definition.Name + name, FieldAttributes.Public | FieldAttributes.Static, parameterType);
            _definition.DeclaringType.Fields.Add(fieldDefinition);

            _definition.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Ldarg_0));
            _definition.Body.Instructions.Insert(1, Instruction.Create(OpCodes.Ldarg, parameterDefinition));
            _definition.Body.Instructions.Insert(2, Instruction.Create(OpCodes.Stfld, fieldDefinition));

            return this;
        }

        public MethodDefinitionDefiner WhichInstantiateAnObject(string tocall, TypeDefinitionDefiner type)
        {
            var variableDefinition = new VariableDefinition(tocall, type.Type);
            _definition.Body.Variables.Add(variableDefinition);
            _definition.Body.InitLocals = true;
            _definition.Body.Instructions.Add(Instruction.Create(OpCodes.Newobj, type.Type.GetConstructor()));
            _definition.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, variableDefinition));
            return this;
        }

        public MethodDefinitionDefiner AndInitializeField(string variableName, string fieldName, int value)
        {
            var variable = _definition.Find(variableName);
            var field = ((TypeDefinition) variable.VariableType).FindField(fieldName);
            _definition.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, variable));
            _definition.Body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, value));
            _definition.Body.Instructions.Add(Instruction.Create(OpCodes.Stfld, field));
            return this;
        }

        public MethodDefinitionDefiner AndReturn()
        {
            _definition.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            return this;
        }
    }

    public static class TypeDefinitionExtensions
    {
        public static MethodReference GetConstructor(this TypeDefinition type)
        {
            return (from m in type.Methods where m.Name == ".ctor" && m.Parameters.Count == 0 select m).First();
        }
        public static FieldReference FindField(this TypeDefinition type, string fieldName)
        {
            return (from m in type.Fields where m.Name == fieldName select m).First();
        }
        public static VariableDefinition Find(this MethodDefinition method, string variableName)
        {
            return (from m in method.Body.Variables where m.Name == variableName select m).First();
        }
    }

    public static class InstcurionsExtensions
    {
        ///<summary>Finds the index of the first item matching an expression in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="predicate">The expression to test the items against.</param>
        ///<returns>The index of the first matching item, or -1 if no items match.</returns>
        public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (predicate == null) throw new ArgumentNullException("predicate");

            int retVal = 0;
            foreach (var item in items)
            {
                if (predicate(item)) return retVal;
                retVal++;
            }
            return -1;
        }
        ///<summary>Finds the index of the first occurence of an item in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="item">The item to find.</param>
        ///<returns>The index of the first matching item, or -1 if the item was not found.</returns>
        public static int IndexOf<T>(this IEnumerable<T> items, T item) { return items.FindIndex(i => EqualityComparer<T>.Default.Equals(item, i)); }
    }
}