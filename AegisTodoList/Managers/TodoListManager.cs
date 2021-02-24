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
        // Temporary psuedo database stuff
        private static List<TodoListItem> _todoListItems;
        private static int lastId;

        public TodoListManager()
        {
            if(_todoListItems == null) _todoListItems = new List<TodoListItem>();
        }

        public IEnumerable<TodoListItem> GetListItems()
        {
            return _todoListItems;
        }

        public void AddListItem(TodoListItem listItem)
        {
            listItem.TodoListItemId = ++lastId;
            _todoListItems.Add(listItem);
        }

        public void DeleteListItem(int listItemID)
        {
            _todoListItems.RemoveAll(x => x.TodoListItemId == listItemID);
        }

        public void UpdateListItem(TodoListItem updatedListItem)
        {
            var oldListItem = _todoListItems.Where(x => x.TodoListItemId == updatedListItem.TodoListItemId).FirstOrDefault();
            var index = _todoListItems.IndexOf(oldListItem);

            if(oldListItem != null && index != -1)
            {
                _todoListItems[index] = updatedListItem; 
            }
        }
    }
}
