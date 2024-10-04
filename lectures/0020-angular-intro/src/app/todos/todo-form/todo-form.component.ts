import { Component, effect, input, model, output, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';

export type FormTodo = {
  id?: number;
  title: string;
  assignedTo: string;
};

export type FormInput = FormTodo & {
  assignees: string[];
};

@Component({
  selector: 'app-todo-form',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './todo-form.component.html',
  styleUrl: './todo-form.component.css',
})
export class TodoFormComponent {
  todo = input<FormInput | undefined>();
  saved = output<FormTodo>();
  canceled = output<void>();

  id = signal<number | undefined>(undefined);
  title = model<string>();
  assignedTo = model<string>();
  assignees = signal<string[]>([]);

  constructor() {
    toObservable(this.todo).subscribe((input) => {
      this.id.set(input?.id);
      this.title.set(input?.title ?? '');
      this.assignedTo.set(input?.assignedTo ?? '');
      this.assignees.set(input?.assignees ?? []);
    });
  }

  save() {
    this.saved.emit({
      id: this.id(),
      title: this.title()!,
      assignedTo: this.assignedTo()!,
    });
  }

  cancel() {
    this.canceled.emit();
  }
}
