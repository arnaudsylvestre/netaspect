using System.Collections.Generic;

namespace NetAspect.Weaver.Core.Errors
{
   public class NetAspectError
    {
        public NetAspectError(ErrorCode code, List<object> parameters, FileLocation location)
        {
            Code = code;
            Parameters = parameters;
            Location = location;
        }

        public ErrorCode Code { get; set; }
        public List<object> Parameters { get; set; }
        public FileLocation Location { get;  private set; }
    }
}