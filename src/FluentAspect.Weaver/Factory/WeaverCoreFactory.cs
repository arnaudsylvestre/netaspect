using FluentAspect.Weaver.Core;
using FluentAspect.Weaver.Core.Configuration.Attributes;
using FluentAspect.Weaver.Core.Configuration.Multi;
using FluentAspect.Weaver.Core.Configuration.Selector;
using FluentAspect.Weaver.Core.V2;
using FluentAspect.Weaver.Core.WeaverBuilders;

namespace FluentAspect.Weaver.Factory
{
    public static class WeaverCoreFactory
    {
        public static WeaverCore2 CreateV2()
        {
            return new WeaverCore2(new WeavingModelComputer());
        }

        public static WeaverCore Create()
        {
            return new WeaverCore(
                new MultiConfigurationReader(
                    new MethodAttributeConfigurationReader(),
                    new ConstructorAttributeConfigurationReader(),
                    new SelectorConfigurationReader(),
                    new FieldAttributeConfigurationReader(),
                    new PropertyAttributeConfigurationReader(),
                    new ParameterAttributeConfigurationReader(),
                    new EventAttributeConfigurationReader()
                    ),
                new MultiWeaverBuilder(new AroundMethodBuilderWeaver(),
                     new AroundConstructorBuilderWeaver(),
                     new CallMethodBuilderWeaver(),
                     new CallFieldBuilderWeaver(),
                     new CallEventBuilderWeaver(),
                     new AroundPropertyGetMethodBuilderWeaver(),
                     new AroundPropertySetMethodBuilderWeaver(),
                     new AroundMethodParameterBuilderWeaver()))
                ;
        }
    }
}