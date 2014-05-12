using System;
using System.Collections.Generic;
using NUnit.Framework;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Fields.Getter.Parameters.After.CallerParameters
{
    public class AfterCallGetFieldCallerParametersParameterWithObjectTypeTest :
        NetAspectTest<AfterCallGetFieldCallerParametersParameterWithObjectTypeTest.ClassToWeave>
    {
        protected override Action CreateEnsure()
        {
            return () =>
                {
                    Assert.IsNull(MyAspect.CallerParameters);
                    var classToWeave_L = new ClassToWeave();
                    classToWeave_L.Weaved(1, 2);
                    Assert.AreEqual(new object[]
                        {
                            1, 2
                        }, MyAspect.CallerParameters);
                };
        }

        public class ClassToWeave
        {
            [MyAspect] public string Field;

            public string Weaved(int i, int i1)
            {
                return Field;
            }
        }

        public class MyAspect : Attribute
        {
            public bool NetAspectAttribute = true;
            public static object CallerParameters;


            public void AfterGetField(object callerParameters)
            {
                CallerParameters = callerParameters;
            }
        }
    }
}