using Kaits.WEB.Entities.General;
using Kaits.WEB.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kaits.WEB.Data
{
    public class HttpClientAuth
    {

        public static async Task<ResponseModel> GetTokenClient(string JWT_AUTH_KEY, string JWT_AUTH_TOKEN)
        {
            ResponseModel responseModel = new ResponseModel();
            HttpClient _httpClient = new HttpClient();
            String UriGeneral = AuthToken.URL_BASE_API + "/api/Auth";

            Auth auth = new Auth();
            auth.UniqueID = JWT_AUTH_KEY;
            auth.Token = JWT_AUTH_TOKEN;

            string dataJson = JsonConvert.SerializeObject(auth);
            StringContent contenidoFromBody = new StringContent(dataJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(UriGeneral, contenidoFromBody);
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest)
            {
                var data = await response.Content.ReadAsStringAsync();
                responseModel = JsonConvert.DeserializeObject<ResponseModel>(data);                
            }
            else
            {
                responseModel.success = false;
                responseModel.errorMessage = "Error 500: No se pudo conectar con el Servidor.";
            }
            return responseModel;
        }

        public static async Task<ResponseModel> LoginUsuario(string LOGIN, string PASS)
        {
            ResponseModel responseModel = new ResponseModel();
            HttpClient _httpClient = new HttpClient();
            String UriGeneral = AuthToken.URL_BASE_API + "/api/UsuarioLogin/";
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + AuthToken.Token);
            
            Usuario usuario = new Usuario();
            usuario.LOGIN = LOGIN;
            usuario.PASS = PASS;

            string dataJson = JsonConvert.SerializeObject(usuario);
            StringContent contenidoFromBody = new StringContent(dataJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(UriGeneral, contenidoFromBody);
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest)
            {
                var data = await response.Content.ReadAsStringAsync();
                responseModel = JsonConvert.DeserializeObject<ResponseModel>(data);
            }
            else
            {
                responseModel.success = false;
                responseModel.errorMessage = "Error 500: No se pudo conectar con el Servidor.";
            }
            return responseModel;
        }
    }
}
