import { AfterViewInit, Component, ElementRef, inject, OnInit, viewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TodoStoreService } from '../todo-store.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-todo-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.css']
})
export class TodoListComponent implements AfterViewInit {
  todoStore = inject(TodoStoreService);

  private doneFilterCtrl = viewChild.required<ElementRef<HTMLInputElement>>('doneFilter');
  private assignedToFilterCtrl = viewChild.required<ElementRef<HTMLInputElement>>('assignedToFilter');

  ngAfterViewInit() {
    this.refresh();
  }

  refresh() {
    this.todoStore.setDoneFilter(this.doneFilterCtrl().nativeElement.value !== 'all');
    this.todoStore.setAssignedToFilter(this.assignedToFilterCtrl().nativeElement.value);

    this.todoStore.getTodos();
  }
}
