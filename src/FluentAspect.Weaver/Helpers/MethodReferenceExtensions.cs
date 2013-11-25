using System;
using Mono.Cecil;

namespace FluentAspect.Weaver.Helpers
{
   public static class MethodReferenceExtensions
   {
      public static MethodReference MakeGeneric(this MethodReference
                                                   method,
                                                params TypeReference[] args)
      {
         if (args.Length == 0)
            return method;

         if (method.GenericParameters.Count != args.Length)
            throw new ArgumentException("Invalid number of generic type arguments supplied");

         GenericInstanceMethod genericTypeRef = new GenericInstanceMethod(method);
         foreach (TypeReference arg in args)
            genericTypeRef.GenericArguments.Add(arg);

         return genericTypeRef;
      }
   }
}
