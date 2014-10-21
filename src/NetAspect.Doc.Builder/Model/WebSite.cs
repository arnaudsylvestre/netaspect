using System.Collections.Generic;

namespace NetAspect.Doc.Builder.Model
{
    public class WebSite
   {
      public WebSite()
      {
         Pages = new List<Page>();
      }

      public List<Page> Pages { get; set; }
   }
}
