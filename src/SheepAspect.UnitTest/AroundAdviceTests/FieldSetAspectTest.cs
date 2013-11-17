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
    public class FieldSetAspectTest : AspectTestBase
    {
        protected override Type TargetType()
        {
            return typeof (SomeClass);
        }

        protected override void SetupAspect(AspectDefinition aspect)
        {
            _advice = null;
            var pointcut = CreatePointcuts<SetFieldPointcut>(aspect, "SheepPoint", "Field: (Name: '_some*')");
            aspect.Advise(new AroundAdvice(pointcut, GetAspectMethod("MockAdvice")));
        }

        private static Action<SetFieldJointPoint> _advice;
        public void MockAdvice(SetFieldJointPoint jp)
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
            Target.SetField(20);
            Assert.AreEqual(200, Target.GetField());
        }

        [Assert]
        public void CanIgnoreCompletely()
        {
            _advice = j => { };

            Target.SetField(10);
            Assert.AreEqual(0, Target.GetField());
        }
    }
}