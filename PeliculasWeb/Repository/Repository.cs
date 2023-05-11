using Newtonsoft.Json;
using PeliculasWeb.Repository.IRepository;
using PeliculasWeb.Utilidades;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json.Serialization;

namespace PeliculasWeb.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        //Inyección de Dependecias se debe importar el IHttpClientFactory

        private readonly IHttpClientFactory _httpClientFactory;
        public Repository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory; 
        }
        public async Task<bool> CreateAsync(string url, T itemCrear, string token="")
        {
            var peticion = new HttpRequestMessage(HttpMethod.Post, url);

            if (itemCrear != null)
            {
                peticion.Content = new StringContent(
                    JsonConvert.SerializeObject(itemCrear), Encoding.UTF8, Constants.MEDIA_TYPE_JSON

                    );
            }
            else
            {
                return false;
            }

            var cliente = _httpClientFactory.CreateClient();
            //Aquí valida el token
            if (token != null && token.Length != 0)
            {
                cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await cliente.SendAsync(peticion);
            //Validar si se creo y retorna boleano
            return response.StatusCode == HttpStatusCode.Created ? true : false;
        }

        public async Task<bool> DeleteAsync(string url, int id, string token="")
        {
            var peticion = new HttpRequestMessage(HttpMethod.Delete, url + id);

          
            var cliente = _httpClientFactory.CreateClient();

            //Aquí valida el token
            if (token != null && token.Length != 0)
            {
                cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await cliente.SendAsync(peticion);
            //Validar si se elimino y retorna boleano
            return response.StatusCode == HttpStatusCode.NoContent ? true : false;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string url)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url);


            var cliente = _httpClientFactory.CreateClient();

            HttpResponseMessage response = await cliente.SendAsync(peticion);
            //Validar si se actualizo y retorna los datos
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            else
            {
                return null;
            }

        }


        public async Task<T> GetAsync(string url, int id)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url + id);


            var cliente = _httpClientFactory.CreateClient();

            HttpResponseMessage response = await cliente.SendAsync(peticion);
            //Validar si se actualizo y retorna los datos
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            else
            {
                return null;
            }
        }

       
        public async Task<bool> UpdateAsync(string url, T itemUpdate, string token="")
        {
           var peticion = new HttpRequestMessage(HttpMethod.Patch, url);

            if (itemUpdate != null)
            {
                peticion.Content = new StringContent(
                    JsonConvert.SerializeObject(itemUpdate), Encoding.UTF8, Constants.MEDIA_TYPE_JSON

                    );
            }
            else
            {
                return false;
            }
            var cliente = _httpClientFactory.CreateClient();

            //Aquí valida el token

            if (token != null && token.Length!=0)
            {
                cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",token);
            }
            HttpResponseMessage response = await cliente.SendAsync(peticion);
            //Validar si se actualizo y retorna boleano
            return response.StatusCode == HttpStatusCode.NoContent ? true : false;
        }


        #region Methods Flitro
        public async Task<IEnumerable<T>> GetAllPeliculasInCategory(string url, int categoriaId)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url+categoriaId);


            var cliente = _httpClientFactory.CreateClient();

            HttpResponseMessage response = await cliente.SendAsync(peticion);
            //Validar si se encontraron y retorna los datos
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<T>> SearchPelicula(string url,string nombre)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url+nombre);


            var cliente = _httpClientFactory.CreateClient();

            HttpResponseMessage response = await cliente.SendAsync(peticion);
            //Validar si se actualizo y retorna los datos
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            else
            {
                return null;
            }
        } 
        #endregion


    }
}
