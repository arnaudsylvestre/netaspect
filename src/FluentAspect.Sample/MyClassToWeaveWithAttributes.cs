using System;
using FluentAspect.Sample.AOP;

namespace FluentAspect.Sample
{
    public class MyClassToWeaveWithAttributes
    {
        //public MyClassToWeaveWithAttributes()
        //{

        //}
        private bool thrown;

        [Thrower]
        public MyClassToWeaveWithAttributes(bool thrown)
        {
            this.thrown = thrown;
        }

        public string PropertyGetter
        {
            [GetProperty] get { return "1"; }
        }

        public string CheckWithReturn()
        {
            return "NotWeaved";
        }

        public string CheckWithParameters(string aspectWillReturnThis)
        {
            return "NotWeaved";
        }

        public void CheckWithVoid()
        {
        }

        public string CheckWithGenerics<T>(T arg)
        {
            return arg + "<>" + typeof (T).FullName;
        }

        public void CheckThrow()
        {
            throw new NotImplementedException();
        }

        [CheckBeforeAspect]
        public string CheckBeforeWithAttributes(BeforeParameter parameter)
        {
            return parameter.Value;
        }

        [CheckBeforeAspect]
        private string CheckBeforeWithAttributesPrivate(BeforeParameter parameter)
        {
            return parameter.Value;
        }

        public string CallCheckBeforeWithAttributesPrivate(BeforeParameter parameter)
        {
            return CheckBeforeWithAttributesPrivate(parameter);
        }

        public static string CheckStatic(BeforeParameter parameter)
        {
            return parameter.Value;
        }

        public string CheckNotRenameInAssembly()
        {
            return CheckWithReturn();
        }


        [CheckBeforeAspect]
        private string CheckBeforeWithAttributesProtected(BeforeParameter parameter)
        {
            return parameter.Value;
        }

        public string CallCheckBeforeWithAttributesProtected(BeforeParameter beforeParameter)
        {
            return CheckBeforeWithAttributesProtected(beforeParameter);
        }
    }


    public class GetPropertyAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void After(ref string result)
        {
            result = "3";
        }
    }
}