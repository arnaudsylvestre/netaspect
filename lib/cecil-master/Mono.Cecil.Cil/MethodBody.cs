//
// MethodBody.cs
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
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
    public sealed class MethodBody : IVariableDefinitionProvider
    {
        internal readonly MethodDefinition method;

        internal int code_size;
        internal Collection<ExceptionHandler> exceptions;
        internal bool init_locals;

        internal Collection<Instruction> instructions;
        internal MetadataToken local_var_token;
        internal int max_stack_size;
        internal ParameterDefinition this_parameter;
        internal Collection<VariableDefinition> variables;

        public MethodBody(MethodDefinition method)
        {
            this.method = method;
        }

        public MethodDefinition Method
        {
            get { return method; }
        }

        public int MaxStackSize
        {
            get { return max_stack_size; }
            set { max_stack_size = value; }
        }

        public int CodeSize
        {
            get { return code_size; }
        }

        public bool InitLocals
        {
            get { return init_locals; }
            set { init_locals = value; }
        }

        public MetadataToken LocalVarToken
        {
            get { return local_var_token; }
            set { local_var_token = value; }
        }

        public Collection<Instruction> Instructions
        {
            get { return instructions ?? (instructions = new InstructionCollection()); }
        }

        public bool HasExceptionHandlers
        {
            get { return !exceptions.IsNullOrEmpty(); }
        }

        public Collection<ExceptionHandler> ExceptionHandlers
        {
            get { return exceptions ?? (exceptions = new Collection<ExceptionHandler>()); }
        }

        public Scope Scope { get; set; }

        public ParameterDefinition ThisParameter
        {
            get
            {
                if (method == null || method.DeclaringType == null)
                    throw new NotSupportedException();

                if (!method.HasThis)
                    return null;

                if (this_parameter != null)
                    return this_parameter;

                TypeDefinition declaring_type = method.DeclaringType;
                TypeReference type = declaring_type.IsValueType || declaring_type.IsPrimitive
                                         ? new PointerType(declaring_type)
                                         : declaring_type as TypeReference;

                return this_parameter = new ParameterDefinition(type, method);
            }
        }

        public bool HasVariables
        {
            get { return !variables.IsNullOrEmpty(); }
        }

        public Collection<VariableDefinition> Variables
        {
            get { return variables ?? (variables = new VariableDefinitionCollection()); }
        }

        public ILProcessor GetILProcessor()
        {
            return new ILProcessor(this);
        }
    }

    public interface IVariableDefinitionProvider
    {
        bool HasVariables { get; }
        Collection<VariableDefinition> Variables { get; }
    }

    internal class VariableDefinitionCollection : Collection<VariableDefinition>
    {
        internal VariableDefinitionCollection()
        {
        }

        internal VariableDefinitionCollection(int capacity)
            : base(capacity)
        {
        }

        protected override void OnAdd(VariableDefinition item, int index)
        {
            item.index = index;
        }

        protected override void OnInsert(VariableDefinition item, int index)
        {
            item.index = index;

            for (int i = index; i < size; i++)
                items[i].index = i + 1;
        }

        protected override void OnSet(VariableDefinition item, int index)
        {
            item.index = index;
        }

        protected override void OnRemove(VariableDefinition item, int index)
        {
            item.index = -1;

            for (int i = index + 1; i < size; i++)
                items[i].index = i - 1;
        }
    }

    internal class InstructionCollection : Collection<Instruction>
    {
        internal InstructionCollection()
        {
        }

        internal InstructionCollection(int capacity)
            : base(capacity)
        {
        }

        protected override void OnAdd(Instruction item, int index)
        {
            if (index == 0)
                return;

            Instruction previous = items[index - 1];
            previous.next = item;
            item.previous = previous;
        }

        protected override void OnInsert(Instruction item, int index)
        {
            if (size == 0)
                return;

            Instruction current = items[index];
            if (current == null)
            {
                Instruction last = items[index - 1];
                last.next = item;
                item.previous = last;
                return;
            }

            Instruction previous = current.previous;
            if (previous != null)
            {
                previous.next = item;
                item.previous = previous;
            }

            current.previous = item;
            item.next = current;
        }

        protected override void OnSet(Instruction item, int index)
        {
            Instruction current = items[index];

            item.previous = current.previous;
            item.next = current.next;

            current.previous = null;
            current.next = null;
        }

        protected override void OnRemove(Instruction item, int index)
        {
            Instruction previous = item.previous;
            if (previous != null)
                previous.next = item.next;

            Instruction next = item.next;
            if (next != null)
                next.previous = item.previous;

            item.previous = null;
            item.next = null;
        }
    }
}