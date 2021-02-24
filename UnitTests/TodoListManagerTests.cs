using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;
using Moq;
using AegisTodoList.Managers;
using AutoFixture;
using System.Linq;
using AegisTodoList.Models;

namespace UnitTests
{
    [TestClass]
    public class TodoListManagerTests
    {
        private Fixture _fixture;
        private SqliteConnection _connection;
        private Mock<IConfiguration> _mockConfiguration;

        const string _connectionString = "Data Source=InMemorySample;Mode=Memory;Cache=Shared";

        public TodoListManagerTests()
        {
            _fixture = new Fixture();
        }

        [TestInitialize]
        public void Init()
        {
            _connection = new SqliteConnection(_connectionString);
            _connection.Open();
            var command = _connection.CreateCommand();
            command.CommandText = @"CREATE TABLE IF NOT EXISTS todo_list (
                    todo_list_item_id INTEGER PRIMARY KEY,
                    name VARCHAR(255) NOT NULL,
                    description VARCHAR(1000) NULL,
                    completed INTEGER NULL
                    );";

            command.ExecuteNonQuery();

            var mockConfSection = new Mock<IConfigurationSection>();
            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "TodoDatabase")]).Returns(_connectionString);

            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(a => a.GetSection(It.Is<string>(s => s == "ConnectionStrings"))).Returns(mockConfSection.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DROP TABLE todo_list;";

                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        [TestMethod]
        public void GetListItems_Success()
        {
            // Arrange
            var todoListManager = new TodoListManager(_mockConfiguration.Object);
            AddListItems(1);

            // Act
            var result = todoListManager.GetListItems().ToList();

            // Assert
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void GetListItems_NoResults()
        {
            // Arrange
            var todoListManager = new TodoListManager(_mockConfiguration.Object);

            // Act
            var result = todoListManager.GetListItems().ToList();

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void AddListItem_Success()
        {
            // Arrange
            var testListItem = new TodoListItemModel()
            {
                Name = _fixture.Create<string>(),
                Description = _fixture.Create<string>(),
                Completed = _fixture.Create<bool>()
            };

            var todoListManager = new TodoListManager(_mockConfiguration.Object);

            // Act 
            todoListManager.AddListItem(testListItem);

            // Assert
            var insertedItem = todoListManager.GetListItems().First();
            Assert.IsNotNull(insertedItem);
            Assert.AreEqual(testListItem.Name, insertedItem.Name);
            Assert.AreEqual(testListItem.Description, insertedItem.Description);
            Assert.AreEqual(testListItem.Completed, insertedItem.Completed);
        }

        [TestMethod]
        public void AddListItem_NullItem()
        {
            // Arrange
            var todoListManager = new TodoListManager(_mockConfiguration.Object);

            // Act 
            todoListManager.AddListItem(null);

            // Assert
            Assert.AreEqual(0, todoListManager.GetListItems().ToList().Count());
        }

        [TestMethod]
        public void DeleteListItem_Success()
        {
            // Arrange
            var todoListManager = new TodoListManager(_mockConfiguration.Object);
            AddListItems(3);

            // Act 
            todoListManager.DeleteListItem(2);

            // Assert
            Assert.AreEqual(2, todoListManager.GetListItems().ToList().Count());
        }

        [TestMethod]
        public void DeleteListItem_InvalidId()
        {
            // Arrange
            var todoListManager = new TodoListManager(_mockConfiguration.Object);
            AddListItems(3);

            // Act 
            todoListManager.DeleteListItem(5);

            // Assert
            Assert.AreEqual(3, todoListManager.GetListItems().ToList().Count());
        }

        [TestMethod]
        public void UpdateListItem_Success()
        {
            // Arrange
            var testListItem = new TodoListItemModel()
            {
                TodoListItemId = 1,
                Name = _fixture.Create<string>(),
                Description = _fixture.Create<string>(),
                Completed = _fixture.Create<bool>()
            };
            AddListItems(1);

            var todoListManager = new TodoListManager(_mockConfiguration.Object);

            // Act 
            todoListManager.UpdateListItem(testListItem);

            // Assert
            var updatedItem = todoListManager.GetListItems().First();
            Assert.IsNotNull(updatedItem);
            Assert.AreEqual(testListItem.Name, updatedItem.Name);
            Assert.AreEqual(testListItem.Description, updatedItem.Description);
            Assert.AreEqual(testListItem.Completed, updatedItem.Completed);
        }

        [TestMethod]
        public void UpdateListItem_InvalidId()
        {
            // Arrange
            var testListItem = new TodoListItemModel()
            {
                TodoListItemId = 2,
                Name = _fixture.Create<string>(),
                Description = _fixture.Create<string>(),
                Completed = _fixture.Create<bool>()
            };
            AddListItems(1);

            var todoListManager = new TodoListManager(_mockConfiguration.Object);

            // Act 
            todoListManager.UpdateListItem(testListItem);

            // Assert
            var updatedItem = todoListManager.GetListItems().First();
            Assert.AreNotEqual(testListItem.Name, updatedItem.Name);
            Assert.AreNotEqual(testListItem.Description, updatedItem.Description);
            Assert.AreNotEqual(testListItem.Completed, updatedItem.Completed);
        }

        private void AddListItems(int count)
        {
            for(int i = 0; i < count; i++)
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"INSERT INTO todo_list (name, description, completed) VALUES($name, $description, $complete);";

                    command.Parameters.AddWithValue("$name", _fixture.Create<string>());
                    command.Parameters.AddWithValue("$description", _fixture.Create<string>());
                    command.Parameters.AddWithValue("$complete", _fixture.Create<bool>() ? 1 : 0);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
