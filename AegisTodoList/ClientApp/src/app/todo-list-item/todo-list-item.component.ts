import { Component, Input } from '@angular/core';
import { TodoListItemModel } from '../models/todoListItemModel';

@Component({
  selector: 'app-todo-list-item',
  templateUrl: './todo-list-item.component.html',
  styleUrls: ['./todo-list-item.component.css']
})
export class TodoListItemComponent {
  @Input() listItem: TodoListItemModel;

  constructor() { }

}
