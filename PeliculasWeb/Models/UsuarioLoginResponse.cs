using Newtonsoft.Json;
using System.Net;

namespace PeliculasWeb.Models
{
    public class UsuarioLoginResponse
    {

        public HttpStatusCode statusCode { get; set; }
        public bool isSuccess { get; set; }
        public List<string> ErrorMessages { get; set; }
        public Result result { get; set; }

    }

    public class Result
    {
       public _Usuario Usuario { get; set; }
       public string token { get; set; }
    }

    public class _Usuario
    {
        public string id { get; set; }

        public string userName { get; set; }

        public string nombre { get; set; }

    }
}
