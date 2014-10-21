namespace NetAspect.Doc.Builder.Model
{
    public class HomePageFactory
    {
        public static Page CreateHomePage()
        {
            return new Page("Home", Templates.Templates.HomePage, "index", null);
        }
    }
}