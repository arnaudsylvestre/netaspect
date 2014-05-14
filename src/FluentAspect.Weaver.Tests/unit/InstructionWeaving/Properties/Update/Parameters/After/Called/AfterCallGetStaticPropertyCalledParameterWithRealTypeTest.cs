using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NUnit.Framework;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Update.Parameters.After.Called
{
    public class AfterCallGetStaticPropertyCalledParameterWithRealTypeTest :
        NetAspectTest<AfterCallGetStaticPropertyCalledParameterWithRealTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Add(new ErrorReport.Error()
                {
                    Level = ErrorLevel.Warning,
                    Message =
                    string.Format(
                        "the called parameter in the method AfterSetProperty of the type '{0}' is not available for static member : default value will be passed",
                        typeof(MyAspect).FullName)
                });
        }


        protected override Action CreateEnsure()
        {
            return () =>
            {
                Assert.IsNull(MyAspect.Called);
                ClassCalled called = new ClassCalled();
                var classToWeave_L = new ClassToWeave(called);
                classToWeave_L.Weaved();
                Assert.AreEqual(null, MyAspect.Called);
            };
        }

        public class ClassCalled
        {
            [MyAspect] public static string Property {get;set;}


        }

        public class ClassToWeave
        {
            ClassCalled called;

            public ClassToWeave(ClassCalled called)
            {
                this.called = called;
            }

            public void Weaved()
            {

                ClassCalled.Property = "Dummy";
            }
        }

        public class MyAspect : Attribute
        {
            public static ClassCalled Called;
            public bool NetAspectAttribute = true;

            public void AfterSetProperty(ClassCalled called)
            {
                Called = called;
            }
        }
    }
}