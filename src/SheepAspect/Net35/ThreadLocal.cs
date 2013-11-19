using System.Collections;

namespace System.Threading
{
    public class ThreadLocal<T>
        where T : class 
    {
        private readonly Func<T> _func;
        private T value;

        public ThreadLocal(Func<T> func)
        {
            _func = func;
        }

        public T Value
        {
            get { return value ?? (value = _func()); }
        }
    }
}