using System;
using System.Reflection;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.MethodWeaving.Method.Exceptions.OnException
{
    public class CheckMethodWeavingOnExceptionWithExceptionAndFinallyTest :
        NetAspectTest<CheckMethodWeavingOnExceptionWithExceptionAndFinallyTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.Method);
                    var classToWeave_L = new ClassToWeave();
                    try
                    {
                        classToWeave_L.Weaved(classToWeave_L);
                        Assert.Fail();
                    }
                    catch (Exception)
                    {
                        Assert.AreEqual("Weaved", MyAspect.Method.Name);
                        Assert.AreEqual("Weaved", MyAspect.FinallyMethod.Name);
                    }
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave Weaved(ClassToWeave toWeave)
            {
                if (toWeave != null)
                    throw new Exception();
                return toWeave;
            }
        }

        public class MyAspect : Attribute
        {
            public static MethodInfo Method;
            public static MethodInfo FinallyMethod;
            public bool NetAspectAttribute = true;

            public void OnException(MethodInfo method)
            {
                Method = method;
            }

            public void OnFinally(MethodInfo method)
            {
                FinallyMethod = method;
            }
        }
    }
}