using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AegisTodoList.Models;

namespace AegisTodoList.Interfaces
{
    public interface ITodoListManager
    {
        IEnumerable<TodoListItemModel> GetListItems();
        void AddListItem(TodoListItemModel listItem);
        void DeleteListItem(int listItemID);
        void UpdateListItem(TodoListItemModel updatedListItem);
    }
}
