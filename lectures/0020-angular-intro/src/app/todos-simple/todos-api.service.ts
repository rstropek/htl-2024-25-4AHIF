import { HttpClient, HttpParams } from '@angular/common/http';
import { computed, inject, Inject, Injectable, signal } from '@angular/core';
import { BASE_URL } from '../app.config';
import { catchError, of, switchMap, tap, finalize, Subject, firstValueFrom } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

export type NewTodo = {
  title: string;
  assignedTo: string;
};

export type Todo = NewTodo & {
  done: boolean;
};

export type IdentifyableTodo = Todo & { id: number };

export type TodoPatch = {
  title?: string;
  assignedTo?: string;
  done?: boolean;
};

export type Filter = {
  assignedTo?: string;
  title?: string;
};

@Injectable()
export class TodosApiService {
  private client = inject(HttpClient);
  private baseUrl = inject(BASE_URL);

  private todos$ = new Subject<Filter>();
  private todosSignal = signal<IdentifyableTodo[]>([]);
  public todos = computed(() => this.todosSignal());

  private todo$ = new Subject<number>();
  private todoSignal = signal<IdentifyableTodo | undefined>(undefined);
  public todo = computed(() => this.todoSignal());

  public error = signal<string | undefined>(undefined);
  public isLoading = signal(false);
  public isWriting = signal(false);

  constructor() {
    this.todos$
      .pipe(
        tap(() => this.isLoading.set(true)),
        tap(() => console.log('Loading todos from server')),
        switchMap((filter) => {
          let params = new HttpParams();
          if (filter.assignedTo) {
            params = params.set('assignedTo', filter.assignedTo);
          }
          if (filter.title) {
            params = params.set('title', filter.title);
          }
          return this.client.get<IdentifyableTodo[]>(this.baseUrl + '/todos', { params });
        }),
        catchError((err) => {
          this.error.set(err);
          return of([]);
        }),
        finalize(() => this.isLoading.set(false)),
        takeUntilDestroyed()
      )
      .subscribe((todos) => this.todosSignal.set(todos));

    this.todo$
      .pipe(
        tap(() => this.isLoading.set(true)),
        tap(() => console.log('Loading todo from server')),
        switchMap((id) => this.client.get<IdentifyableTodo>(this.baseUrl + `/todos/${id}`)),
        catchError((err) => {
          this.error.set(err);
          return of(undefined);
        }),
        finalize(() => this.isLoading.set(false)),
        takeUntilDestroyed()
      )
      .subscribe((todo) => this.todoSignal.set(todo));
  }

  getTodos(title?: string, assignedTo?: string) {
    this.todos$.next({ title, assignedTo });
  }

  getTodo(id: number) {
    this.todo$.next(id);
  }

  addTodo(todo: NewTodo): Promise<IdentifyableTodo> {
    return firstValueFrom(
      this.client.post<IdentifyableTodo>(this.baseUrl + '/todos', todo).pipe(
        tap(() => this.isWriting.set(true)),
        catchError((err) => {
          this.error.set(err);
          return of({} as IdentifyableTodo);
        }),
        finalize(() => this.isWriting.set(false))
      )
    );
  }

  patchTodo(todo: TodoPatch, id: number): Promise<IdentifyableTodo> {
    return firstValueFrom(
      this.client.patch<IdentifyableTodo>(this.baseUrl + `/todos/${id}`, todo).pipe(
        tap(() => this.isWriting.set(true)),
        catchError((err) => {
          this.error.set(err);
          return of({} as IdentifyableTodo);
        }),
        finalize(() => this.isWriting.set(false))
      )
    );
  }

  deleteTodo(id: number): Promise<void> {
    return firstValueFrom(
      this.client.delete<void>(this.baseUrl + `/todos/${id}`).pipe(
        tap(() => this.isWriting.set(true)),
        catchError((err) => {
          this.error.set(err);
          return of(undefined);
        }),
        finalize(() => this.isWriting.set(false))
      )
    );
  }
}
