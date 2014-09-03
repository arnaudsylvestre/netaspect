using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Selectors;

namespace NetAspect.Weaver.Core.Weaver.Detectors
{
   public delegate Selector<T> SelectorProvider<T>(NetAspectDefinition aspect);
}
