using Newtonsoft.Json;
using PeliculasWeb.Models;
using PeliculasWeb.Repository.IRepository;
using PeliculasWeb.Utilidades;
using System.Net;
using System.Text;

namespace PeliculasWeb.Repository
{
    public class AccountRepository: Repository<UsuarioAccount>, IAccountRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AccountRepository(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<UsuarioLoginResponse> LoguinAsync(string url, UsuarioAccount account)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if (account != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(account), Encoding.UTF8, Constants.MEDIA_TYPE_JSON
                    );
            }
            else
            {
                return new UsuarioLoginResponse();
            }
            var cliente = _httpClientFactory.CreateClient();

            HttpResponseMessage response = await cliente.SendAsync(request);
            //Validar si se actualizo y retorna los datos
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                UsuarioLoginResponse userLogin = JsonConvert.DeserializeObject<UsuarioLoginResponse>(jsonString);

                return userLogin;
            }
            else
            {
                return new UsuarioLoginResponse();
            }
        }

        public async Task<bool> RegisterAsync(string url, UsuarioRegister account)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if (account != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(account), Encoding.UTF8, Constants.MEDIA_TYPE_JSON
                    );
            }
            else
            {
                return false;
            }
            var cliente = _httpClientFactory.CreateClient();

            HttpResponseMessage response = await cliente.SendAsync(request);

            //Validar si se registro y retorna los boleano
            return response.StatusCode == HttpStatusCode.OK ? true : false;
        }
    }
}
