import { Component, Input } from '@angular/core';
import { TodoListItemModel } from '../models/todoListItemModel';

@Component({
  selector: 'app-todo-list-item',
  templateUrl: './todo-list-item.component.html',
  styleUrls: ['./todo-list-item.component.css']
})
export class TodoListItemComponent {
  @Input() listItem: TodoListItemModel;
  public editing: boolean; // Are we currently editing this list item?

  constructor() {
    this.editing = false;
  }

  public toggleEditing(): void {
    console.log("toggleEditing()");
    this.editing = !this.editing;
  }

}
