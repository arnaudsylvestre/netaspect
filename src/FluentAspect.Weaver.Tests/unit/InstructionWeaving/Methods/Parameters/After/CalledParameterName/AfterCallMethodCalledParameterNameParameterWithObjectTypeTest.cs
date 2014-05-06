using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Methods.Parameters.After.CalledParameterName
{
    public class AfterCallMethodCalledParameterNameParameterWithObjectTypeTest :
        NetAspectTest<AfterCallMethodCalledParameterNameParameterWithObjectTypeTest.ClassToWeave>
    {
        //protected override Action<ErrorHandler> CreateErrorHandlerProvider()
        //{
        //    return
        //        errorHandler =>
        //        errorHandler.Errors.Add(
        //            string.Format(
        //                "the calledParam1 parameter in the method AfterCallMethod of the type '{0}' is declared with the type 'System.Object' but it is expected to be System.Int32",
        //                typeof(MyAspect).FullName));
        //}

        protected override Action CreateEnsure()
        {
            return () =>
            {
                Assert.AreEqual(null, MyAspect.ParameterName);
                var classToWeave_L = new ClassToWeave();
                classToWeave_L.Weaved();
                Assert.AreEqual(12, MyAspect.ParameterName);
            };
        }


        public class ClassToWeave
        {
            [MyAspect]
            public string Method(int param1)
            {
                return "Hello";
            }

            public string Weaved()
            {
                return Method(12);
            }
        }

        public class MyAspect : Attribute
        {
            public static object ParameterName;
            public bool NetAspectAttribute = true;

            public void AfterCallMethod(object calledParam1)
            {
                ParameterName = calledParam1;
            }
        }
    }
}