using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Detectors.Selectors;

namespace NetAspect.Weaver.Core.Weaver.Detectors.Helpers
{
    public delegate Selector<T> SelectorProvider<T>(NetAspectDefinition aspect);
}