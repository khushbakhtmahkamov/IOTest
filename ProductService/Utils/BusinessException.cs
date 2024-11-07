using System;

namespace ProductService.Utils
{
    public class BusinessException : Exception
    {
        public BusinessExceptionCode BusinessExceptionCode { get; }
        public BusinessException(BusinessExceptionCode code, string message) : base(message) {
            BusinessExceptionCode = code;
        }
    }
}
