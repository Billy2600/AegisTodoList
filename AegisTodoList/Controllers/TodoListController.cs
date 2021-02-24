using AegisTodoList.Interfaces;
using AegisTodoList.Managers;
using AegisTodoList.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AegisTodoList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoListController : ControllerBase
    {
        private ITodoListManager _todoListManager;

        public TodoListController()
        {
            _todoListManager = new TodoListManager();
        }

        [HttpGet]
        public IEnumerable<TodoListItem> Get()
        {
            return _todoListManager.GetTodoListItems();
        }
    }
}
