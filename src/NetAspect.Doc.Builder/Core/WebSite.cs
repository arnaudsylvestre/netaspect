using System.Collections.Generic;
using System.Linq;

namespace NetAspect.Doc.Builder.Model
{
    public class WebSite
   {
      public WebSite()
      {
         Pages = new List<Page>();
      }

      public List<Page> Pages { get; set; }
      public IEnumerable<Page> LinkPages { get { return Pages.Skip(1); }}
   }
}
