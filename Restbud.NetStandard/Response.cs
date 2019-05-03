using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Restbud.NetStandard
{
    public class Response<T,J> where T:class where J : HttpResponseMessage, new()
    {
        public T Content { get; set; }
        public J HttpResponseMessage { get; set; }
    }
}
