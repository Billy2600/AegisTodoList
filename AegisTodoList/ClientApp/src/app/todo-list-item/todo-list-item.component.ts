import { Component, Input, Inject } from '@angular/core';
import { TodoListItemModel } from '../models/todoListItemModel';
import { FormBuilder, FormGroup } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-todo-list-item',
  templateUrl: './todo-list-item.component.html',
  styleUrls: ['./todo-list-item.component.css']
})
export class TodoListItemComponent {
  @Input() listItem: TodoListItemModel;
  public editing: boolean; // Are we currently editing this list items
  editListItemForm: FormGroup;

  constructor(
    private http: HttpClient, 
    @Inject('BASE_URL') private baseUrl: string,
    private formBuilder: FormBuilder
  ) {
    this.editing = false;
  }

  public toggleEditing(): void {
    this.editing = !this.editing;
    if(this.editing) {
      this.editListItemForm = this.formBuilder.group({
        name: this.listItem.name,
        description: this.listItem.description,
        completed: this.listItem.completed
      })
    }
  }

  editListItem(): void {
    let listItem : TodoListItemModel = {
      todoListItemId: this.listItem.todoListItemId,
      name: this.editListItemForm.get('name').value,
      description: this.editListItemForm.get('description').value,
      completed: this.editListItemForm.get('completed').value
    };
    this.listItem = listItem;

    console.log(this.listItem.todoListItemId);

    this.http.put<TodoListItemModel>(this.baseUrl + 'TodoList/' + this.listItem.todoListItemId, listItem).subscribe();

    this.toggleEditing();
  }

}
