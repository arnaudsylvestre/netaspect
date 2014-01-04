using System;

namespace FluentAspect.Weaver.Core.Weavers.Helpers
{
    public class TryCatch
    {
        private readonly Action _onCatch;
        private readonly Action onTryContent;

        public TryCatch(Action onTryContent, Action onCatch)
        {
            this.onTryContent = onTryContent;
            _onCatch = onCatch;
        }

        public Action OnTryContent
        {
            get { return onTryContent; }
        }

        public Action OnCatch
        {
            get { return _onCatch; }
        }
    }
}