using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AegisTodoList.Models;

namespace AegisTodoList.Interfaces
{
    public interface ITodoListManager
    {
        IEnumerable<TodoListItem> GetListItems();
        void AddListItem(TodoListItem listItem);
        void DeleteListItem(int listItemID);
        void UpdateListItem(TodoListItem updatedListItem);
    }
}
