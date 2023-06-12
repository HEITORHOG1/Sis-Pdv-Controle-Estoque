namespace Sis_Pdv_Controle_Estoque_Form.Services.Exceptions
{
    public class ApiException : Exception
    {
        public HttpResponseMessage Response { get; }

        public ApiException(HttpResponseMessage response) : base(response.ReasonPhrase)
        {
            Response = response;
        }
    }
}
