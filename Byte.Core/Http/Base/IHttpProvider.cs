using System.Collections.Generic;

namespace Byte.Core.Http.Base
{
    public interface IHttpProvider
    {
        HttpResponseParameter Excute(HttpRequestParameter requestParameter, string contentType, string accept);
    }
}
