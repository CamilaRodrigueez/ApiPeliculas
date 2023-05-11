namespace PeliculasWeb.Utilidades
{
    public static class Constants
    {
        public static string URL_BASE_API = "https://localhost:7255/";
        public static string RUTA_CATEGORIAS_API = URL_BASE_API + "api/categorys/";
        public static string RUTA_PELICULAS_API = URL_BASE_API + "api/peliculas/";
        public static string RUTA_USUARIOS_API = URL_BASE_API + "api/usuarios/";

        public static string HTTP_METHOD_POST = "POST";
        public static string HTTP_METHOD_GET = "GET";
        public static string HTTP_METHOD_PATCH = "PATCH";
        public static string HTTP_METHOD_PUT = "PUT";
        public static string HTTP_METHOD_DELETE = "DELETE";
        public static string MEDIA_TYPE_XML = "application/xml";
        public static string MEDIA_TYPE_JSON = "application/json";
        public static string RESPONCE_STATUS = "OK";

        //Faltan otras rutas para buscar y filtrar por categorias
        public static string RUTA_PELICULAS_EN_CATEGORIA_API = URL_BASE_API + "api/peliculas/GetAllPeliculasInCategory/";
     
        public static string RUTA_BUSCAR_PELICULAS_API = URL_BASE_API + "api/peliculas/SearchPelicula?nombre=";


    }
}
