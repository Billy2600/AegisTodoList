import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TodoListItem } from './todoListItem';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public todoListItems: TodoListItem[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<TodoListItem[]>(baseUrl + 'TodoList').subscribe(result => {
      this.todoListItems = result;
    }, error => console.error(error));
  }
}