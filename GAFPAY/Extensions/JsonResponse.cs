using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GAFPAY.Extensions
{
    public class JsonResponse
    {
        public static object ErrorResponse(string error)
        {
            return new {Success = false, ErrorMessage = error};
        }

        public static object SuccessResponse()
        {
            return new {Success = true};
        }

        public static object SuccessResponse(string Message)
        {
            return  new {Success = true, Message = Message};
        }
    }
}