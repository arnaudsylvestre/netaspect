using System.IO;

namespace FluentAspect.Weaver.Helpers
{
    public static class StreamExtensions
    {
        public static void CopyStream(this Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        } 
    }
}