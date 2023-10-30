using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Practica_n3.DTO;

namespace Practica_n3.Integrations
{
    public class JsonplaceholderAPIIntegration
    {
        private readonly ILogger<JsonplaceholderAPIIntegration> _logger;
        private const string API_URL = "https://jsonplaceholder.typicode.com/posts";
        private readonly HttpClient httpClient;

        public JsonplaceholderAPIIntegration(ILogger<JsonplaceholderAPIIntegration> logger)
        {
            _logger = logger;
            httpClient = new HttpClient();
        }

        public async Task<List<TodoDTO>> GetAll()
        {
            string requestUrl = $"{API_URL}";
            List<TodoDTO> listado = new List<TodoDTO>();
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    listado = await response.Content.ReadFromJsonAsync<List<TodoDTO>>() ?? new List<TodoDTO>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al llamar a la API: {ex.Message}");
            }
            return listado;
        }

        public async Task<TodoDTO> GetTodoById(int id)
        {
            string requestUrl = $"{API_URL}/{id}";
            TodoDTO todo = null;
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    todo = await response.Content.ReadFromJsonAsync<TodoDTO>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al llamar a la API: {ex.Message}");
            }
            return todo;
        }

        public async Task CreateTodo(TodoDTO todo)
        {
            string requestUrl = $"{API_URL}";
            try
            {
                HttpResponseMessage response = await httpClient.PostAsJsonAsync(requestUrl, todo);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error al crear el elemento: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al llamar a la API: {ex.Message}");
            }
        }

        public async Task UpdateTodo(TodoDTO todo)
        {
            string requestUrl = $"{API_URL}/{todo.id}";
            try
            {
                HttpResponseMessage response = await httpClient.PutAsJsonAsync(requestUrl, todo);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error al actualizar el elemento: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al llamar a la API: {ex.Message}");
            }
        }

        public async Task DeleteTodo(int id)
        {
            string requestUrl = $"{API_URL}/{id}";
            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error al eliminar el elemento: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al llamar a la API: {ex.Message}");
            }
        }
    }
}