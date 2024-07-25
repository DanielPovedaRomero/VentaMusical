using Newtonsoft.Json;
using System.Net;

namespace VentaMusical.Models.ViewModels
{
    public class RespuestaModel
    {
        [JsonProperty(PropertyName = "codigo")]
        public HttpStatusCode Codigo { get; set; }

        [JsonProperty(PropertyName = "mensaje")]
        public string Mensaje { get; set; }

        [JsonProperty(PropertyName = "resultado")]
        public object Resultado { get; set; }
    }
}