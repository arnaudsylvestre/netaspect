using System;
using System.Reflection;
using NetAspect.Weaver.Core.Assemblies;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Helpers;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Session;
using NetAspect.Weaver.Core.Weaver.ToSort.Engine;

namespace NetAspect.Weaver.Core.Weaver
{
    public class WeaverEngine
    {
        private readonly IAssemblyPoolFactory assemblyPoolFactory;
        private readonly ErrorInfoComputer errorInfoComputer;
        private readonly MethodWeaver methodWeaver;
        private readonly WeavingSessionComputer _weavingSessionComputer;

        public WeaverEngine(WeavingSessionComputer _weavingSessionComputer, IAssemblyPoolFactory assemblyPoolFactory,
                            ErrorInfoComputer errorInfoComputer, MethodWeaver methodWeaver)
        {
            this._weavingSessionComputer = _weavingSessionComputer;
            this.assemblyPoolFactory = assemblyPoolFactory;
            this.errorInfoComputer = errorInfoComputer;
            this.methodWeaver = methodWeaver;
        }

        public ErrorReport Weave(string assemblyFilePath, Func<string, string> newAssemblyNameProvider)
        {
            return Weave(Assembly.LoadFrom(assemblyFilePath).GetTypes(), newAssemblyNameProvider, null);
        }

        public ErrorReport Weave(Type[] types, Func<string, string> newAssemblyNameProvider, Type[] filter)
        {
            var errorHandler = new ErrorHandler();
            var assemblyPool = assemblyPoolFactory.Create();

            var session = _weavingSessionComputer.ComputeWeavingSession(types, filter, assemblyPool, errorHandler);
            foreach (var weavingModel in session.weavingModels)
                methodWeaver.Weave(weavingModel.Key, weavingModel.Value, errorHandler);

            assemblyPool.Save(errorHandler, newAssemblyNameProvider);
            return ConvertErrorReport(errorHandler, errorInfoComputer);
        }

        private ErrorReport ConvertErrorReport(ErrorHandler errorHandler, ErrorInfoComputer computer)
        {
            return errorHandler.ConvertToErrorReport(computer);
        }

        public interface IAssemblyPoolFactory
        {
            AssemblyPool Create();
        }
    }
}