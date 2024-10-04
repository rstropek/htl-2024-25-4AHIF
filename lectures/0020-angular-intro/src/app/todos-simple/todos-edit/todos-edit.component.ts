import { Component, effect, inject, input, model, untracked } from '@angular/core';
import { TodosApiService } from '../todos-api.service';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { filter } from 'rxjs';

@Component({
  selector: 'app-todos-edit',
  standalone: true,
  imports: [FormsModule],
  providers: [TodosApiService],
  templateUrl: './todos-edit.component.html',
  styleUrl: './todos-edit.component.css',
})
export class TodosEditComponent {
  client = inject(TodosApiService);
  router = inject(Router);

  id = input<number>();
  title = model('');
  assignedTo = model('');
  done = model(false);

  constructor() {
    toObservable(this.id)
      .pipe(
        filter((id) => !!id),
        takeUntilDestroyed()
      )
      .subscribe((id) => this.client.getTodo(id!));

    toObservable(this.client.todo)
      .pipe(filter((todo) => !!todo))
      .subscribe((todo) => {
        this.title.set(todo.title);
        this.assignedTo.set(todo.assignedTo);
        this.done.set(todo.done);
      });
  }

  async save() {
    let op: Promise<any>;
    if (!this.id()) {
      op = this.client.addTodo({
        title: this.title(),
        assignedTo: this.assignedTo(),
      });
    } else {
      op = this.client.patchTodo(
        {
          title: this.title(),
          assignedTo: this.assignedTo(),
          done: this.done(),
        },
        this.id()!
      );
    }

    await op;

    this.router.navigateByUrl('/todo-list-simple');
  }

  cancel() {
    this.router.navigateByUrl('/todo-list-simple');
  }
}
