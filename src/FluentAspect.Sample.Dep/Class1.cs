namespace FluentAspect.Sample.Dep
{
    public class DepClassWithField
    {
        public string Field;
    }

    public class DepClassWhichCallField
    {
        public string CallField(string fieldValue)
        {
            var c = new DepClassWithField()
                {
                    Field = fieldValue
                };
            return c.Field;
        }
    }
}