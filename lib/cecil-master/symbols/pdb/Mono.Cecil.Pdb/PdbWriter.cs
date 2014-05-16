//
// PdbWriter.cs
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

using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using ISymbolWriter = Mono.Cecil.Cil.ISymbolWriter;

#if !READ_ONLY

namespace Mono.Cecil.Pdb
{
    public class PdbWriter : ISymbolWriter
    {
        private readonly Dictionary<string, SymDocumentWriter> documents;
        private readonly ModuleDefinition module;
        private readonly SymWriter writer;

        internal PdbWriter(ModuleDefinition module, SymWriter writer)
        {
            this.module = module;
            this.writer = writer;
            documents = new Dictionary<string, SymDocumentWriter>();
        }

        public bool GetDebugHeader(out ImageDebugDirectory directory, out byte[] header)
        {
            header = writer.GetDebugInfo(out directory);
            return true;
        }

        public void Write(MethodBody body)
        {
            MetadataToken method_token = body.Method.MetadataToken;
            var sym_token = new SymbolToken(method_token.ToInt32());

            Collection<Instruction> instructions = CollectInstructions(body);
            if (instructions.Count == 0)
                return;

            int start_offset = 0;
            int end_offset = body.CodeSize;

            writer.OpenMethod(sym_token);
            writer.OpenScope(start_offset);

            DefineSequencePoints(instructions);
            DefineVariables(body, start_offset, end_offset);

            writer.CloseScope(end_offset);
            writer.CloseMethod();
        }

        public void Write(MethodSymbols symbols)
        {
            var sym_token = new SymbolToken(symbols.MethodToken.ToInt32());

            int start_offset = 0;
            int end_offset = symbols.CodeSize;

            writer.OpenMethod(sym_token);
            writer.OpenScope(start_offset);

            DefineSequencePoints(symbols);
            DefineVariables(symbols, start_offset, end_offset);

            writer.CloseScope(end_offset);
            writer.CloseMethod();
        }

        public void Dispose()
        {
            MethodDefinition entry_point = module.EntryPoint;
            if (entry_point != null)
                writer.SetUserEntryPoint(new SymbolToken(entry_point.MetadataToken.ToInt32()));

            writer.Close();
        }

        private Collection<Instruction> CollectInstructions(MethodBody body)
        {
            var collection = new Collection<Instruction>();
            Collection<Instruction> instructions = body.Instructions;

            for (int i = 0; i < instructions.Count; i++)
            {
                Instruction instruction = instructions[i];
                SequencePoint sequence_point = instruction.SequencePoint;
                if (sequence_point == null)
                    continue;

                GetDocument(sequence_point.Document);
                collection.Add(instruction);
            }

            return collection;
        }

        private void DefineVariables(MethodBody body, int start_offset, int end_offset)
        {
            if (!body.HasVariables)
                return;

            var sym_token = new SymbolToken(body.LocalVarToken.ToInt32());

            Collection<VariableDefinition> variables = body.Variables;
            for (int i = 0; i < variables.Count; i++)
            {
                VariableDefinition variable = variables[i];
                CreateLocalVariable(variable, sym_token, start_offset, end_offset);
            }
        }

        private void DefineSequencePoints(Collection<Instruction> instructions)
        {
            for (int i = 0; i < instructions.Count; i++)
            {
                Instruction instruction = instructions[i];
                SequencePoint sequence_point = instruction.SequencePoint;

                writer.DefineSequencePoints(
                    GetDocument(sequence_point.Document),
                    new[] {instruction.Offset},
                    new[] {sequence_point.StartLine},
                    new[] {sequence_point.StartColumn},
                    new[] {sequence_point.EndLine},
                    new[] {sequence_point.EndColumn});
            }
        }

        private void CreateLocalVariable(VariableDefinition variable, SymbolToken local_var_token, int start_offset,
                                         int end_offset)
        {
            writer.DefineLocalVariable2(
                variable.Name,
                0,
                local_var_token,
                SymAddressKind.ILOffset,
                variable.Index,
                0,
                0,
                start_offset,
                end_offset);
        }

        private SymDocumentWriter GetDocument(Document document)
        {
            if (document == null)
                return null;

            SymDocumentWriter doc_writer;
            if (documents.TryGetValue(document.Url, out doc_writer))
                return doc_writer;

            doc_writer = writer.DefineDocument(
                document.Url,
                document.Language.ToGuid(),
                document.LanguageVendor.ToGuid(),
                document.Type.ToGuid());

            documents[document.Url] = doc_writer;
            return doc_writer;
        }

        private void DefineSequencePoints(MethodSymbols symbols)
        {
            Collection<InstructionSymbol> instructions = symbols.instructions;

            for (int i = 0; i < instructions.Count; i++)
            {
                InstructionSymbol instruction = instructions[i];
                SequencePoint sequence_point = instruction.SequencePoint;

                writer.DefineSequencePoints(
                    GetDocument(sequence_point.Document),
                    new[] {instruction.Offset},
                    new[] {sequence_point.StartLine},
                    new[] {sequence_point.StartColumn},
                    new[] {sequence_point.EndLine},
                    new[] {sequence_point.EndColumn});
            }
        }

        private void DefineVariables(MethodSymbols symbols, int start_offset, int end_offset)
        {
            if (!symbols.HasVariables)
                return;

            var sym_token = new SymbolToken(symbols.LocalVarToken.ToInt32());

            Collection<VariableDefinition> variables = symbols.Variables;
            for (int i = 0; i < variables.Count; i++)
            {
                VariableDefinition variable = variables[i];
                CreateLocalVariable(variable, sym_token, start_offset, end_offset);
            }
        }
    }
}

#endif