using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AegisTodoList.Models
{
    public class TodoListItem
    {
        public int TodoListItemId { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
    }
}
