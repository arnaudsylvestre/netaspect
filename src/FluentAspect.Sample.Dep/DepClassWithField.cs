namespace NetAspect.Sample.Dep
{
    public class DepClassWithField
    {
        public string Field;

        
        public void TestMethod()
        {
            if (Field == null)
            {
                Field = "";
            }
        }
    }
    public class DepClassWithProperty
    {
        public string Property { get; set; }

    }
}