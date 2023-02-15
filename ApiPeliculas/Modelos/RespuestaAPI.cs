using System.Net;

namespace ApiPeliculas.Modelos
{
    public class RespuestaAPI
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }
        public RespuestaAPI()
        {
            ErrorMessages = new List<string>();
        }
       
    }
}
