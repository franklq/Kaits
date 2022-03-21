using Kaits.WEB.Entities.DTO;
using Kaits.WEB.Entities.General;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kaits.WEB.Data
{
    public sealed class HttpClientPedido
    {
        public static async Task<ResponseModel> Listar(string IDORDEN, string FECDESDE, string FECHASTA, string IDCLIENTE)
        {
            ResponseModel responseModel = new ResponseModel();
            var client = new RestClient(AuthToken.URL_BASE_API);
            var request = new RestRequest("/api/Pedidos", Method.GET).AddHeader("Authorization", "Bearer " + AuthToken.Token);
            request.AddParameter("IDORDEN", IDORDEN);
            request.AddParameter("FECDESDE", FECDESDE);
            request.AddParameter("FECHASTA", FECHASTA);
            request.AddParameter("IDCLIENTE", IDCLIENTE);
            IRestResponse response = await client.ExecuteAsync(request);
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest)
            {
                var content = response.Content;
                responseModel = JsonConvert.DeserializeObject<ResponseModel>(content);
            }
            else
            {
                responseModel.success = false;
                responseModel.errorMessage = "Error 500: No se pudo conectar con el Servidor.";
            }
            return responseModel;
        }
        public static async Task<ResponseModel> GetPedido(string IDORDEN)
        {
            ResponseModel responseModel = new ResponseModel();
            var client = new RestClient(AuthToken.URL_BASE_API);
            var request = new RestRequest("/api/Pedidos/GetPedido", Method.GET).AddHeader("Authorization", "Bearer " + AuthToken.Token);
            request.AddParameter("IDORDEN", IDORDEN);
            IRestResponse response = await client.ExecuteAsync(request);
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest)
            {
                var content = response.Content;
                responseModel = JsonConvert.DeserializeObject<ResponseModel>(content);
            }
            else
            {
                responseModel.success = false;
                responseModel.errorMessage = "Error 500: No se pudo conectar con el Servidor.";
            }
            return responseModel;
        }
        public static async Task<ResponseModel> Insert(PEDIDO_DTO dto)
        {
            ResponseModel responseModel = new ResponseModel();
            HttpClient _httpClient = new HttpClient();
            String UriGeneral = AuthToken.URL_BASE_API + "/api/Pedidos";
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + AuthToken.Token);            

            string dataJson = JsonConvert.SerializeObject(dto);
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
        public static async Task<ResponseModel> Update(PEDIDO_DTO dto)
        {
            ResponseModel responseModel = new ResponseModel();
            HttpClient _httpClient = new HttpClient();
            String UriGeneral = AuthToken.URL_BASE_API + "/api/Pedidos";
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + AuthToken.Token);

            string dataJson = JsonConvert.SerializeObject(dto);
            StringContent contenidoFromBody = new StringContent(dataJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync(UriGeneral, contenidoFromBody);
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
        public static async Task<ResponseModel> Delete(string IDORDEN, string USUARIO)
        {
            ResponseModel responseModel = new ResponseModel();
            var client = new RestClient(AuthToken.URL_BASE_API);
            var request = new RestRequest("/api/Pedidos", Method.DELETE).AddHeader("Authorization", "Bearer " + AuthToken.Token);
            request.AddParameter("IDORDEN", IDORDEN);
            request.AddParameter("USUARIO", USUARIO);
            IRestResponse response = await client.ExecuteAsync(request);
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest)
            {
                var content = response.Content;
                responseModel = JsonConvert.DeserializeObject<ResponseModel>(content);
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
