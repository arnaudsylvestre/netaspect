using System;

namespace FluentAspect.Sample
{
    internal class TryFinallySample
    {
        public string Hello()
        {
            try
            {
                return "Hello";
            }
            catch (Exception e)
            {
                string a = e.Message;
                throw;
            }
            finally
            {
                int b = 2;
            }
        }
    }
}