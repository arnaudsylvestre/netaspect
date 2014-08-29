using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Constructors.Parameters.Before.FileName
{
    public class BeforeCallConstructorFileNameParameterWithBadTypeTest :
        NetAspectTest<BeforeCallConstructorFileNameParameterWithBadTypeTest.ClassToWeave>
    {
        protected override Action<List<ErrorReport.Error>> CreateErrorHandlerProvider()
        {
            return
                errorHandler =>
                errorHandler.Add(new ErrorReport.Error()
                {
                    Level = ErrorLevel.Error,
                    Message =
                    string.Format(
                        "the fileName parameter in the method BeforeCallConstructor of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.String",
                        typeof(MyAspect).FullName)
                });
        }

        
        public class ClassToWeave
        {
			[MyAspect]
            public ClassToWeave()
            {
            }

            public static ClassToWeave Create()
            {
                return new ClassToWeave();
            }
        }

        public class MyAspect : Attribute
        {
            public static int FileName;
            public bool NetAspectAttribute = true;

            public void BeforeCallConstructor(int fileName)
            {
                FileName = fileName;
            }
        }
    }
}