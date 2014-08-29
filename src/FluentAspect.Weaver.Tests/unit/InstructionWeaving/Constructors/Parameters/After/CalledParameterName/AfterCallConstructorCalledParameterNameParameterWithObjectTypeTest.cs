using System;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.After.CalledParameterName
{
    public class AfterCallConstructorCalledParameterNameParameterWithObjectTypeTest :
        NetAspectTest<AfterCallConstructorCalledParameterNameParameterWithObjectTypeTest.ClassToWeave>
    {
        //protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        //{
        //    return
        //        errorHandler =>
        //        errorHandler.Errors.Add(
        //            string.Format(
        //                "the calledParam1 parameter in the method AfterCallConstructor of the type '{0}' is declared with the type 'System.Object' but it is expected to be System.Int32",
        //                typeof(MyAspect).FullName));
        //}

        protected override Action CreateEnsure()
        {
            return () =>
            {
                Assert.AreEqual(null, MyAspect.ParameterName);
                var classToWeave_L = ClassToWeave.Create();
                Assert.AreEqual(12, MyAspect.ParameterName);
            };
        }


        public class ClassToWeave
        {
			[MyAspect]
            public ClassToWeave(int param1)
            {
            }

            public static ClassToWeave Create()
            {
                return new ClassToWeave(12);
            }
        }

        public class MyAspect : Attribute
        {
            public static object ParameterName;
            public bool NetAspectAttribute = true;

            public void AfterCallConstructor(object calledParam1)
            {
                ParameterName = calledParam1;
            }
        }
    }
}