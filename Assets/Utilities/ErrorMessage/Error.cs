using System;

namespace MError
{
    public class Error
    {
        private string _errorMsg;
        private object _errorTarget;
        private Type _targetType;

        public Error(string errorMsg, object target)
        {
            _errorMsg = errorMsg;
            _errorTarget = target;
        }

        public string What()
        {
            return _errorMsg;
        }

        public object ErrorSource()
        {
            return _errorTarget;
        }
    }
}