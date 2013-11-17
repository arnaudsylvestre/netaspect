using System;
using NUnit.Framework;
using SheepAspect.AroundAdvising;
using SheepAspect.Core;
using SheepAspect.Pointcuts;
using SheepAspect.Pointcuts.Impl;
using SheepAspect.Runtime;
using SheepAspect.UnitTest.Target;
using SheepAspect.UnitTest.TestHelper;

namespace SheepAspect.UnitTest.AroundAdviceTests
{
    [TestFixture]
    public class PropertySetAspectTest : AspectTestBase
    {
        protected override Type TargetType()
        {
            return typeof (SomeClass);
        }

        protected override void SetupAspect(AspectDefinition aspect)
        {
            _advice = null;
            var pointcuts = CreatePointcuts<PropertyMethodPointcut>(aspect, "SheepPoint", "Setter & Name:'SomeProperty'");
            aspect.Advise(new AroundAdvice(pointcuts, GetAspectMethod("MockAdvice")));
        }

        private static Action<PropertySetJointPoint> _advice;
        public void MockAdvice(PropertySetJointPoint jp)
        {
            _advice(jp);
        }

        [Assert]
        public void CanProceedAndIntercept()
        {
            _advice = j =>
                          {
                              j.Value = (int)j.Value*10;
                              j.Proceed();
                          };
            Target.SomeProperty = 20;
            Assert.AreEqual(200, Target.SomeProperty);
        }

        [Assert]
        public void CanIgnoreCompletely()
        {
            _advice = j => { };

            Target.SomeProperty = 10;
            Assert.AreEqual(0, Target.SomeProperty);
        }
    }
}