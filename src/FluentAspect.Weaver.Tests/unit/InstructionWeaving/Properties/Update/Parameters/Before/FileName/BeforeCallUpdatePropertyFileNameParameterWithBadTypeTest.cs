using System;
using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.unit.InstructionWeaving.Properties.Update.Parameters.Before.FileName
{
    public class BeforeCallUpdatePropertyFileNameParameterWithBadTypeTest :
        NetAspectTest<BeforeCallUpdatePropertyFileNameParameterWithBadTypeTest.ClassToWeave>
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
                        "the fileName parameter in the method BeforeSetProperty of the type '{0}' is declared with the type 'System.Int32' but it is expected to be System.String",
                        typeof(MyAspect).FullName)
                });
        }

        public class ClassToWeave
        {
            [MyAspect] public string Property {get;set;}

            public void Weaved()
            {
                Property = "Dummy";
            }
        }

        public class MyAspect : Attribute
        {
            public static int FileName;
            public bool NetAspectAttribute = true;

            public void BeforeSetProperty(int fileName)
            {
                FileName = fileName;
            }
        }
    }
}