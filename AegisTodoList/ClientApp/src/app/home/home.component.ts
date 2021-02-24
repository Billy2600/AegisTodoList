import { Component, Inject, NgModule } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { TodoListItem } from './todoListItem';
import { interval } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public todoListItems: TodoListItem[];
  private readonly controllerPath = 'TodoList';

  newListItemForm = this.formBuilder.group({
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
    this.http.get<TodoListItem[]>(this.baseUrl + this.controllerPath).subscribe(result => {
      this.todoListItems = result;
    }, error => console.error(error));
  }

  addListItem(): void {
    let listItem : TodoListItem = {
      todoListItemID: 0,
      description: this.newListItemForm.get('description').value,
      completed: this.newListItemForm.get('completed').value
    };

    this.http.post<TodoListItem>(this.baseUrl + this.controllerPath, listItem).subscribe(result =>
      this.loadListItems()
    );
  }
}
