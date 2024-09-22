import { AfterViewInit, Component, contentChild, ElementRef, inject, model, OnInit, signal, viewChild } from '@angular/core';
import { FormControl, FormControlName, FormsModule } from '@angular/forms';
import { Todo, TodoStoreService } from '../todo-store.service';
import { CommonModule } from '@angular/common';
import { FormInput, FormTodo, TodoFormComponent } from '../todo-form/todo-form.component';

@Component({
  selector: 'app-todo-list',
  standalone: true,
  imports: [CommonModule, FormsModule, TodoFormComponent],
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.css'],
})
export class TodoListComponent implements AfterViewInit {
  todoStore = inject(TodoStoreService);
  todoFormInput = signal<FormInput | undefined>(undefined);
  addDialog = viewChild.required<ElementRef<HTMLDialogElement>>('addDialog');
  doneFilter = viewChild.required<ElementRef<HTMLSelectElement>>('doneFilter');
  assignedToFilter = viewChild.required<ElementRef<HTMLSelectElement>>('assignedToFilter');

  ngAfterViewInit() {
    this.refresh();
  }

  readonly onlyUnfinishedOptionValue = 'onlyUnfinished';

  changeDoneFilter() {
    this.todoStore.setDoneFilter(this.doneFilter().nativeElement.value === this.onlyUnfinishedOptionValue);
    this.refresh();
  }

  changeAssignedToFilter() {
    this.todoStore.setAssignedToFilter(this.assignedToFilter().nativeElement.value);
    this.refresh();
  }

  refresh() {
    this.todoStore.reload();
  }

  add() {
    this.todoFormInput.set({
      title: '',
      assignedTo: '',
      assignees: this.todoStore.assignees(),
    });
    this.addDialog().nativeElement.showModal();
  }

  edit(todo: Todo) {
    this.todoFormInput.set({
      id: todo.id,
      title: todo.title,
      assignedTo: todo.assignedTo,
      assignees: this.todoStore.assignees(),
    });
    this.addDialog().nativeElement.showModal();
  }

  formCancled() {
    this.addDialog().nativeElement.close();
  }

  formSaved(formTodo: FormTodo) {
    this.addDialog().nativeElement.close();

    if (!formTodo.id) {
      this.todoStore.addTodo(formTodo);
    } else {
      const todo = this.todoStore.todos().find((t) => t.id === formTodo.id);
      if (!todo) {
        return;
      }
      todo.title = formTodo.title;
      todo.assignedTo = formTodo.assignedTo;
      this.todoStore.updateTodo(todo);
    }
    
    this.todoStore.reload();
  }

  toggleTodo(todo: Todo) {
    todo.done = !todo.done;
    this.todoStore.updateTodo(todo);
    this.todoStore.reload();
  }
}
