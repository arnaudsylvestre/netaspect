namespace NetAspect.Doc.Builder.Model
{
    public static class WebsiteFactory
    {
        public static WebSite Create(string baseFolder)
        {
            var webSite = new WebSite();
            webSite.Pages.Add(HomePageFactory.CreateHomePage());
            webSite.Pages.Add(NetAspectPageFactory.CreateNetAspectPage());
            webSite.Pages.Add(GettingStartedPageFactory.CreateGettingStartedPage(baseFolder));
            webSite.Pages.Add(DocumentationPageFactory.CreateDocumentationPage(baseFolder));
            return webSite;
        }
    }
}