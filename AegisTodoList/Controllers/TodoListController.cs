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

        public TodoListController(ITodoListManager todoListManager)
        {
            _todoListManager = todoListManager;
        }

        [HttpGet]
        public IEnumerable<TodoListItemModel> Get()
        {
            return _todoListManager.GetListItems();
        }

        [HttpPost]
        public void Add(TodoListItemModel listItem)
        {
            _todoListManager.AddListItem(listItem);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _todoListManager.DeleteListItem(id);
        }

        [HttpPut("{id}")]
        public void Update(int id, TodoListItemModel listItem)
        {
            listItem.TodoListItemId = id; // Parameter ID takes precedence over the one in the object
            _todoListManager.UpdateListItem(listItem);
        }
    }
}
