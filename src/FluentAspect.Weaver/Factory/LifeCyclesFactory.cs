using System.Collections.Generic;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Engine.Lifecycle;

namespace NetAspect.Weaver.Factory
{
    public static class LifeCyclesFactory
    {
        public static Dictionary<LifeCycle, ILifeCycleHandler> CreateLifeCycles()
        {
            return new Dictionary<LifeCycle, ILifeCycleHandler>
               {
                   {LifeCycle.Transient, new TransientLifeCycleHandler()},
                   {LifeCycle.PerInstance, new PerInstanceLifeCycleHandler()},
                   {LifeCycle.PerType, new PerInstanceLifeCycleHandler {Static = true}},
               };
        } 
    }
}