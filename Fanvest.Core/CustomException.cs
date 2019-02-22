using System;
using System.Runtime.Serialization;
namespace Fanvest.Core
{
    [Serializable]
    public partial class CustomException : Exception
    {
        public CustomException()
        {

        }

        public CustomException(string message)
            : base(message)
        {

        }

        public CustomException(string messageFormat, params object[] args)
            : base(string.Format(messageFormat, args))
        {

        }

        protected CustomException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        public CustomException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}