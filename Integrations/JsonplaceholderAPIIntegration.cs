using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;
using Practica_n3.DTO;

namespace Practica_n3.Integrations
{
    public class JsonplaceholderAPIIntegration
    {
        private readonly ILogger<JsonplaceholderAPIIntegration> _logger;
        private const string API_URL="https://jsonplaceholder.typicode.com/posts";
        private readonly HttpClient httpClient;

        public JsonplaceholderAPIIntegration(ILogger<JsonplaceholderAPIIntegration> logger){
            _logger = logger;
            httpClient = new HttpClient();
        }

        public async Task<List<TodoDTO>> GetAll(){
            string requestUrl = $"{API_URL}";
            List<TodoDTO> listado = new List<TodoDTO>();
            try{
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    listado =  await response.Content.ReadFromJsonAsync<List<TodoDTO>>() ?? new List<TodoDTO>();
                }
            }catch(Exception ex){
                _logger.LogDebug($"Error al llamar a la API: {ex.Message}");
            }
            return listado;
        }
    }
}