using Mono.Cecil;

namespace FluentAspect.Weaver.Tests.Core.Model
{
    public static class NetAspectAssemblyExtensions
    {
        public static NetAspectClass AddClass(this NetAspectAssembly assembly, string name)
        {
            var c = new NetAspectClass(name, assembly.Assembly.MainModule);
            assembly.Add(c);
            return c;
        }
        public static NetAspectAspect AddAspect(this NetAspectAssembly assembly, string name)
        {
            var c = new NetAspectClass(name, assembly.Assembly.MainModule);
            assembly.Add(c);
            return new NetAspectAspect(c);
        }
        public static NetAspectMethod AddMethod(this NetAspectClass classe, string methodName)
        {
            var c = new NetAspectMethod(methodName, classe.Module.TypeSystem.Void, classe.Module);
            classe.Add(c);
            return c;
        }
        public static NetAspectMethod AddConstructor(this NetAspectClass classe)
        {
            var c = new NetAspectMethod(".ctor", classe.Module.TypeSystem.Void, classe.Module);
            classe.Add(c);
            return c;
        }


        public static NetAspectParameter WithReferencedParameter<T>(this NetAspectMethod method, string parameterName)
        {
            var netAspectParameter = new NetAspectParameter(parameterName, new ByReferenceType(method.ModuleDefinition.Import(typeof(T))), false);
            method.Add(netAspectParameter);
            return netAspectParameter;
        }
    }
}