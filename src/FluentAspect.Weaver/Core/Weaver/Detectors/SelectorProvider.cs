using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Detectors.Engine.Selectors;

namespace NetAspect.Weaver.Core.Weaver.Detectors
{
    public delegate Selector<T> SelectorProvider<T>(NetAspectDefinition aspect);
}