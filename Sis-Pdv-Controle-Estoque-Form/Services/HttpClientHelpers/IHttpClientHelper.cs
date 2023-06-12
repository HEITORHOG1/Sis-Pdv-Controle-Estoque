using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque_Form.Services.HttpClientHelpers
{
    public interface IHttpClientHelper
    {
        Task<T> SendRequest<T>(string url, HttpMethod method, object content = null);
    }
}
