using System;
using System.Collections.Generic;
using Mono.Cecil;

namespace FluentAspect.Weaver.Tests.Core.Model
{
    public class NetAspectClass : INetAspectType
    {
        List<NetAspectMethod> methods = new List<NetAspectMethod>();
        List<NetAspectField> fields = new List<NetAspectField>();

        public string Name { get; set; }

        public NetAspectClass(string name, ModuleDefinition module)
        {
            Name = name;
            Module = module;
        }

        public bool IsAbstract { get; set; }

        public ModuleDefinition Module { get; private set; }

        public void Add(NetAspectMethod method)
        {
            methods.Add(method);
        }
        public void Add(NetAspectField field)
        {
            fields.Add(field);
        }

        private TypeDefinition definition;

       public Type BaseType { get; set; }

       public TypeDefinition TypeDefinition
        {
            get { 
                if (definition == null)
                {
                    definition = Generate();
                }
                return definition;
            }
        }

       public IEnumerable<NetAspectField> Fields { get { return fields; } }

       public MethodReference DefaultConstructor
        {
            get {
                foreach (var method in TypeDefinition.Methods)
                {
                    if (method.Name == ".ctor" && method.Parameters.Count == 0)
                        return method;
                }
                throw new Exception(); }
        }

        public TypeDefinition Generate()
        {
            TypeAttributes attributes = TypeAttributes.Class | TypeAttributes.Public;
            if (IsAbstract)
                attributes = attributes | TypeAttributes.Abstract;
            TypeDefinition def = new TypeDefinition("A", Name, attributes);
           var baseType = BaseType;
           if (baseType == null)
              baseType = typeof (object);
           def.BaseType = Module.Import(baseType);
           foreach (var field in fields)
           {
              def.Fields.Add(field.Field);
           }
            foreach (var method in methods)
            {
                def.Methods.Add(method.MethodDefinition);
            }
            return def;
        }
    }
}