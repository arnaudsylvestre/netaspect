namespace NetAspect.Sample.Dep
{
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