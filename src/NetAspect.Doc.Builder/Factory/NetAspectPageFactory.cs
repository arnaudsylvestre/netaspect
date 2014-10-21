namespace NetAspect.Doc.Builder.Model
{
    public class NetAspectPageFactory
    {

        public static Page CreateNetAspectPage()
        {
            return new Page("NetAspect", Templates.Templates.NetAspectPage, "NetAspect", null);
        }
    }
}