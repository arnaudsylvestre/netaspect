using System.Collections.Generic;

namespace NetAspect.Weaver.Core.Errors
{
    public enum ErrorCode
    {
        
    }

    public class FileLocation
    {
        public string FilePath { get; set; }
        public int Column { get; set; }
        public int Line { get; set; }
    }        

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