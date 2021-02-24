import { Component, Inject, NgModule } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { TodoListItemModel } from '../models/todoListItemModel';
import { TodoListItemComponent } from '../todo-list-item/todo-list-item.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public todoListItems: TodoListItemModel[];
  private readonly controllerPath = 'TodoList';

  newListItemForm = this.formBuilder.group({
    name: '',
    description: '',
    completed: false
  })

  constructor(
    private http: HttpClient, 
    @Inject('BASE_URL') private baseUrl: string,
    private formBuilder: FormBuilder
  ) {
    this.loadListItems();
  }

  loadListItems(): void {
    this.http.get<TodoListItemModel[]>(this.baseUrl + this.controllerPath).subscribe(result => {
      this.todoListItems = result;
    }, error => console.error(error));
  }

  addListItem(): void {
    let listItem : TodoListItemModel = {
      todoListItemId: 0,
      name: this.newListItemForm.get('name').value,
      description: this.newListItemForm.get('description').value,
      completed: false
    };

    this.http.post<TodoListItemModel>(this.baseUrl + this.controllerPath, listItem).subscribe(result =>
      this.loadListItems()
    );
  }
}
