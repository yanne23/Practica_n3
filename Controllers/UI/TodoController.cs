using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Practica_n3.DTO;
using Practica_n3.Integrations;

namespace Practica_n3.Controllers.UI
{
   
    public class TodoController : Controller
    {
        private readonly ILogger<TodoController> _logger;

        private readonly JsonplaceholderAPIIntegration _jsonplaceholder;

        public TodoController(ILogger<TodoController>logger, JsonplaceholderAPIIntegration jsonplaceholder)
        {
            _logger = logger;
            _jsonplaceholder = jsonplaceholder;
        }

        public async Task<IActionResult> Index()
        {

            List<TodoDTO> todos =await _jsonplaceholder.GetAll();

            //List<TodoDTO> filtro = todos.Where(todo => todo.userId > 6).ToList();

            //List<TodoDTO> filtro = todos.Where(todo => todo.title.Contains("Tarea")).ToList();

            List<TodoDTO> filtro = todos
            .Where(todo => todo.userId == 1)
            .OrderBy(todo => todo.title)
            .ThenByDescending(todo => todo.body)
            .ToList();

            return View(todos);
        }

        public async Task<IActionResult> Details(int id)
        {
            TodoDTO todo = await _jsonplaceholder.GetTodoById(id);

            if (todo == null)
            {
                return NotFound(); // Puedes personalizar esta vista de error.
            }

            return View(todo);
        }

                public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TodoDTO todo)
        {
            if (ModelState.IsValid)
            {
                // Llama a tu servicio de integración o lógica de negocio para crear el post.
                await _jsonplaceholder.CreateTodo(todo);
                return RedirectToAction("Index");
            }

            return View(todo);
        }

        public async Task<IActionResult> Edit(int id)
        {
            TodoDTO todo = await _jsonplaceholder.GetTodoById(id);

            if (todo == null)
            {
                return NotFound(); // Puedes personalizar esta vista de error.
            }

            return View(todo);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TodoDTO todo)
        {
            if (id != todo.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Llama a tu servicio de integración o lógica de negocio para actualizar el post.
                await _jsonplaceholder.UpdateTodo(todo);
                return RedirectToAction("Index");
            }

            return View(todo);
        }

        public async Task<IActionResult> Delete(int id)
            {
                TodoDTO todo = await _jsonplaceholder.GetTodoById(id);

                if (todo == null)
                {
                    return NotFound(); // Puedes personalizar esta vista de error.
                }

                return View(todo);
            }

            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                // Llama a tu servicio de integración o lógica de negocio para eliminar el post.
                await _jsonplaceholder.DeleteTodo(id);
                return RedirectToAction("Index");
            }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}