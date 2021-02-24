using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AegisTodoList.Interfaces;
using AegisTodoList.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace AegisTodoList.Managers
{
    public class TodoListManager : ITodoListManager
    {
        private readonly string _connectionString;

        public TodoListManager(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("TodoDatabase");
        }

        public IEnumerable<TodoListItemModel> GetListItems()
        {
            var todoListItems = new List<TodoListItemModel>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"SELECT todo_list_item_id, name, description, completed FROM todo_list;";

                using (var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        todoListItems.Add(new TodoListItemModel()
                        {
                            TodoListItemId = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            Completed = reader.GetInt32(3) == 1 ? true : false // SQLite doesn't have boolean type
                        });
                    }
                }
            }

            return todoListItems;
        }

        public void AddListItem(TodoListItemModel listItem)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO todo_list (name, description, completed) VALUES($name, $description, $complete);";

                command.Parameters.AddWithValue("$name", listItem.Name == null ? "" : listItem.Name);
                command.Parameters.AddWithValue("$description", listItem.Description);
                command.Parameters.AddWithValue("$complete", listItem.Completed ? 1 : 0);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteListItem(int listItemID)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"DELETE FROM todo_list WHERE todo_list_item_id = $id";

                command.Parameters.AddWithValue("$id", listItemID);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateListItem(TodoListItemModel updatedListItem)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"UPDATE todo_list SET name = $name, description = $description, completed = $complete
                    WHERE todo_list_item_id = $id";

                command.Parameters.AddWithValue("$id", updatedListItem.TodoListItemId);
                command.Parameters.AddWithValue("$name", updatedListItem.Name);
                command.Parameters.AddWithValue("$description", updatedListItem.Description);
                command.Parameters.AddWithValue("$complete", updatedListItem.Completed ? 1 : 0);

                command.ExecuteNonQuery();
            }
        }
    }
}
