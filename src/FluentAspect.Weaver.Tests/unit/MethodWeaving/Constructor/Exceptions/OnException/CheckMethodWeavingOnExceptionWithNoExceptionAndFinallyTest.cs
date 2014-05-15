using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Exceptions.OnException
{
    public class CheckMethodWeavingOnExceptionWithNoExceptionAndFinallyTest :
        NetAspectTest<CheckMethodWeavingOnExceptionWithNoExceptionAndFinallyTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Method);
                    try
                    {
                        var classToWeave_L = new ClassToWeave(null);
                        Assert.Fail();
                    }
                    catch (Exception)
                    {
                        Assert.IsNull(MyAspect.Method);
                        Assert.AreEqual(".ctor", MyAspect.FinallyMethod.Name);
                    }
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave(ClassToWeave toWeave)
            {
            }
        }

        public class MyAspect : Attribute
        {
            public static MethodBase Method;
            public static MethodBase FinallyMethod;
            public bool NetAspectAttribute = true;

            public void OnExceptionConstructor(MethodBase constructor)
            {
                Method = constructor;
            }

            public void OnFinallyConstructor(MethodBase constructor)
            {
                FinallyMethod = constructor;
            }
        }
    }
}