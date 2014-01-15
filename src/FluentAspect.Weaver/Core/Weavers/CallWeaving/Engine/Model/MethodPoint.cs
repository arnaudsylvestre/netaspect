using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine
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

        private bool Equals(MethodPoint other)
        {
            return Equals(Method, other.Method) && Equals(Instruction, other.Instruction);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MethodPoint) obj);
        }
    }
}