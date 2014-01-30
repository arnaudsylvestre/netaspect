using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine
{
    public class JoinPoint
    {
        public MethodDefinition Method { get; set; }
        public Instruction InstructionStart { get; set; }
        public Instruction InstructionEnd { get; set; }


        protected bool Equals(JoinPoint other)
        {
            return Equals(Method, other.Method) && Equals(InstructionStart, other.InstructionStart) && Equals(InstructionEnd, other.InstructionEnd);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((JoinPoint) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Method != null ? Method.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (InstructionStart != null ? InstructionStart.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (InstructionEnd != null ? InstructionEnd.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}