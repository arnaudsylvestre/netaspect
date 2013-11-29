using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentAspect.Weaver.Core
{
    public class WeaverCore
    {
        public void Weave(Assembly assembly)
        {
            var typeWalker = WalkerFactory.Create();
            typeWalker.Walk();
        }
    }



    public interface ITypeWalkerHandler
    {
        void OnType(Type t);
    }

    public class TypeWalker
    {
        public void Walk(Assembly assembly, ITypeWalkerHandler typeWalkerHandler)
        {
            foreach (var type in assembly.GetTypes())
            {
                typeWalkerHandler.OnType(type);
            }
        }
    }

    public class MultipleTypeWalkerHandler : ITypeWalkerHandler
    {
        private List<ITypeWalkerHandler> handlers;

        public void OnType(Type t)
        {
            foreach (var handler in handlers)
            {
                handler.OnType(t);
            }
        }
    }

    public class WalkerFactory
    {
        public static TypeWalker Create()
        {


            return new TypeWalker();
        }
    }

    public interface IMethodWalkerHandler
    {
        void OnMethod(MethodInfo m);

    }

    public class MethodWalker
    {
        public void Walk(Type type, IMethodWalkerHandler methodWalkerHandlerHandler)
        {
            foreach (var methodInfo in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
            {
                methodWalkerHandlerHandler.OnMethod(methodInfo);
            }

        }
    }
}