using System.Reflection;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Core.Weaver.Detectors.Engine.Selectors
{
    public class Selector<T>
    {
        private readonly MethodInfo _method;
        private SelectorParametersGenerator<T> selectorParametersGenerator;

        public Selector(MethodInfo method, SelectorParametersGenerator<T> selectorParametersGenerator)
        {
            _method = method;
            this.selectorParametersGenerator = selectorParametersGenerator;
        }

        public void Check(ErrorHandler errorHandler)
        {
            if (_method == null)
                return;
            selectorParametersGenerator.Check(_method, errorHandler);
            if (!_method.IsStatic)
                errorHandler.OnError("The selector {0} in the aspect {1} must be static", _method.Name, _method.DeclaringType.FullName);

            if (_method.ReturnType != typeof(bool))
                errorHandler.OnError("The selector {0} in the aspect {1} must return boolean value", _method.Name, _method.DeclaringType.FullName);
        }

        public bool IsCompliant(T member)
        {
            if (_method == null)
                return false;
            var errorHandler = new ErrorHandler();
            Check(errorHandler);
            if (errorHandler.Errors.Count > 0 || errorHandler.Failures.Count > 0)
                return false;
            return (bool)_method.Invoke(null, selectorParametersGenerator.Generate(_method, member));
        }
    }
}