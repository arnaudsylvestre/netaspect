using System;
using System.Reflection;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.MethodWeaving.Constructor.Exceptions.OnException
{
    public class CheckMethodWeavingOnExceptionWithExceptionAndFinallyTest :
        NetAspectTest<CheckMethodWeavingOnExceptionWithExceptionAndFinallyTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                   Assert.IsNull(MyAspect.Constructor);
                    try
                    {
                       var classToWeave_L = new ClassToWeave();
                        Assert.Fail();
                    }
                    catch (Exception)
                    {
                        Assert.AreEqual("Weaved", MyAspect.Constructor.Name);
                        Assert.AreEqual("Weaved", MyAspect.FinallyConstructor.Name);
                    }
                };
        }

        public class ClassToWeave
        {
            [MyAspect]
            public ClassToWeave()
            {
                    throw new Exception();
                //return toWeave;
            }
        }

        public class MyAspect : Attribute
        {
           public static ConstructorInfo Constructor;
           public static ConstructorInfo FinallyConstructor;
            public bool NetAspectAttribute = true;

            public void OnExceptionConstructor(ConstructorInfo constructor)
            {
               Constructor = constructor;
            }

            public void OnFinallyConstructor(ConstructorInfo constructor)
            {
               FinallyConstructor = constructor;
            }
        }
    }
}