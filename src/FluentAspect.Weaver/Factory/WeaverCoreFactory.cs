using FluentAspect.Weaver.Core;
using FluentAspect.Weaver.Core.Configuration.Attributes;
using FluentAspect.Weaver.Core.Configuration.Multi;
using FluentAspect.Weaver.Core.Configuration.Selector;
using FluentAspect.Weaver.Core.WeaverBuilders;

namespace FluentAspect.Weaver.Factory
{
    public static class WeaverCoreFactory
    {
        public static WeaverCore Create()
        {
            return new WeaverCore(
                new MultiConfigurationReader(
                    new MethodAttributeConfigurationReader(),
                    new ConstructorAttributeConfigurationReader(),
                    new SelectorConfigurationReader(),
                    new FieldAttributeConfigurationReader(),
                    new PropertyAttributeConfigurationReader()
                    ),
                new MultiWeaverBuilder(new AroundMethodBuilderWeaver(),
                     new AroundConstructorBuilderWeaver(),
                     new CallMethodBuilderWeaver(),
                     new CallFieldBuilderWeaver(),
                     new AroundPropertyGetMethodBuilderWeaver(),
                     new AroundPropertySetMethodBuilderWeaver()))
                ;
        }
    }
}