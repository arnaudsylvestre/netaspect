using FluentAspect.Weaver.CF.Core.WeaverBuilders;
using FluentAspect.Weaver.Core;
using FluentAspect.Weaver.Core.Configuration.Multi;
using FluentAspect.Weaver.Core.Fluent;

namespace FluentAspect.Weaver.Factory
{
   public static class WeaverCoreFactory
   {
       public static WeaverCore Create()
       {
          return new WeaverCore(
              new MultiConfigurationReader(
                  new MethodAttributeConfigurationReader(), 
                  new ConstructorAttributeConfigurationReader()
               ),
               new MultiWeaverBuilder(new AroundMethodBuilderWeaver(), new AroundConstructorBuilderWeaver()))
           ;

       }
   }
}