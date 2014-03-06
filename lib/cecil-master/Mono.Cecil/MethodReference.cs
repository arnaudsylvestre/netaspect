//
// MethodReference.cs
//
// Author:
//   Jb Evain (jbevain@gmail.com)
//
// Copyright (c) 2008 - 2011 Jb Evain
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Text;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
    public class MethodReference : MemberReference, IMethodSignature, IGenericParameterProvider, IGenericContext
    {
        internal Collection<GenericParameter> generic_parameters;
        internal ParameterDefinitionCollection parameters;
        private MethodReturnType return_type;

        internal MethodReference()
        {
            return_type = new MethodReturnType(this);
            token = new MetadataToken(TokenType.MemberRef);
        }

        public MethodReference(string name, TypeReference returnType)
            : base(name)
        {
            if (returnType == null)
                throw new ArgumentNullException("returnType");

            return_type = new MethodReturnType(this);
            return_type.ReturnType = returnType;
            token = new MetadataToken(TokenType.MemberRef);
        }

        public MethodReference(string name, TypeReference returnType, TypeReference declaringType)
            : this(name, returnType)
        {
            if (declaringType == null)
                throw new ArgumentNullException("declaringType");

            DeclaringType = declaringType;
        }

        public override string FullName
        {
            get
            {
                var builder = new StringBuilder();
                builder.Append(ReturnType.FullName)
                       .Append(" ")
                       .Append(MemberFullName());
                this.MethodSignatureFullName(builder);
                return builder.ToString();
            }
        }

        public virtual bool IsGenericInstance
        {
            get { return false; }
        }

        internal override bool ContainsGenericParameter
        {
            get
            {
                if (ReturnType.ContainsGenericParameter || base.ContainsGenericParameter)
                    return true;

                Collection<ParameterDefinition> parameters = Parameters;

                for (int i = 0; i < parameters.Count; i++)
                    if (parameters[i].ParameterType.ContainsGenericParameter)
                        return true;

                return false;
            }
        }

        IGenericParameterProvider IGenericContext.Type
        {
            get
            {
                TypeReference declaring_type = DeclaringType;
                var instance = declaring_type as GenericInstanceType;
                if (instance != null)
                    return instance.ElementType;

                return declaring_type;
            }
        }

        IGenericParameterProvider IGenericContext.Method
        {
            get { return this; }
        }

        GenericParameterType IGenericParameterProvider.GenericParameterType
        {
            get { return GenericParameterType.Method; }
        }

        public virtual bool HasGenericParameters
        {
            get { return !generic_parameters.IsNullOrEmpty(); }
        }

        public virtual Collection<GenericParameter> GenericParameters
        {
            get
            {
                if (generic_parameters != null)
                    return generic_parameters;

                return generic_parameters = new GenericParameterCollection(this);
            }
        }

        public virtual bool HasThis { get; set; }

        public virtual bool ExplicitThis { get; set; }

        public virtual MethodCallingConvention CallingConvention { get; set; }

        public virtual bool HasParameters
        {
            get { return !parameters.IsNullOrEmpty(); }
        }

        public virtual Collection<ParameterDefinition> Parameters
        {
            get
            {
                if (parameters == null)
                    parameters = new ParameterDefinitionCollection(this);

                return parameters;
            }
        }

        public TypeReference ReturnType
        {
            get
            {
                MethodReturnType return_type = MethodReturnType;
                return return_type != null ? return_type.ReturnType : null;
            }
            set
            {
                MethodReturnType return_type = MethodReturnType;
                if (return_type != null)
                    return_type.ReturnType = value;
            }
        }

        public virtual MethodReturnType MethodReturnType
        {
            get { return return_type; }
            set { return_type = value; }
        }

        public virtual MethodReference GetElementMethod()
        {
            return this;
        }

        public virtual MethodDefinition Resolve()
        {
            ModuleDefinition module = Module;
            if (module == null)
                throw new NotSupportedException();

            return module.Resolve(this);
        }
    }

    static partial class Mixin
    {
        public static bool IsVarArg(this IMethodSignature self)
        {
            return (self.CallingConvention & MethodCallingConvention.VarArg) != 0;
        }

        public static int GetSentinelPosition(this IMethodSignature self)
        {
            if (!self.HasParameters)
                return -1;

            Collection<ParameterDefinition> parameters = self.Parameters;
            for (int i = 0; i < parameters.Count; i++)
                if (parameters[i].ParameterType.IsSentinel)
                    return i;

            return -1;
        }
    }
}