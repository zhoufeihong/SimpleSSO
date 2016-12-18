using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FreeBird.Infrastructure.Exceptions
{
    /// <summary>
    /// 当用户试图访问未授权的资源时抛出的异常。
    /// </summary>
    public class AuthorizationException : Exception
    {
        public AuthorizationException()
            : base()
        {
        }

        public AuthorizationException(string message)
            : base(message)
        {
        }

        public AuthorizationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public AuthorizationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
