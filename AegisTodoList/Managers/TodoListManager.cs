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
        private static List<TodoListItemModel> _TodoListItemModels;
        private static int lastId;

        public TodoListManager()
        {
            if(_TodoListItemModels == null) _TodoListItemModels = new List<TodoListItemModel>();
        }

        public IEnumerable<TodoListItemModel> GetListItems()
        {
            return _TodoListItemModels;
        }

        public void AddListItem(TodoListItemModel listItem)
        {
            listItem.TodoListItemId = ++lastId;
            _TodoListItemModels.Add(listItem);
        }

        public void DeleteListItem(int listItemID)
        {
            _TodoListItemModels.RemoveAll(x => x.TodoListItemId == listItemID);
        }

        public void UpdateListItem(TodoListItemModel updatedListItem)
        {
            var oldListItem = _TodoListItemModels.Where(x => x.TodoListItemId == updatedListItem.TodoListItemId).FirstOrDefault();
            var index = _TodoListItemModels.IndexOf(oldListItem);

            if(oldListItem != null && index != -1)
            {
                _TodoListItemModels[index] = updatedListItem; 
            }
        }
    }
}
