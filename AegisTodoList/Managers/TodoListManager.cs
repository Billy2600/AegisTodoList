using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AegisTodoList.Interfaces;
using AegisTodoList.Models;

namespace AegisTodoList.Managers
{
    public class TodoListManager : ITodoListManager
    {
        public IEnumerable<TodoListItem> GetTodoListItems()
        {
            return new List<TodoListItem>()
            {
                new TodoListItem() { Completed = false, Description = "Test Item" },
                new TodoListItem() { Completed = false, Description = "Test Item" }
            };
        }
    }
}
