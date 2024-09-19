import { computed, Injectable, input, model, signal } from '@angular/core';

export type Todo = {
  title: string;
  assignedTo: string;
  done: boolean;
};

export type GetTodosFilter = {
  done?: boolean;
  assignedTo?: string;
};

export type TodoState = {
  isLoading: boolean;
  todos: Todo[];
  assignees: string[];
  error?: string;
  filter: GetTodosFilter;
};

@Injectable({
  providedIn: 'root',
})
export class TodoStoreService {
  private todoState = signal<TodoState>({
    isLoading: false,
    todos: [],
    assignees: [],
    filter: {},
  });

  // Selectors
  isLoading = computed(() => this.todoState().isLoading);
  todos = computed(() => this.todoState().todos);
  assignees = computed(() => this.todoState().assignees);
  error = computed(() => this.todoState().error);

  private setLoading(isLoading: boolean) {
    this.todoState.update((state) => ({ ...state, isLoading }));
  }

  private setTodos(todos: Todo[]) {
    this.todoState.update((state) => ({ ...state, todos }));
  }

  private clearTodos() {
    this.setTodos([]);
  }

  private setAssignees(assignees: string[]) {
    this.todoState.update((state) => ({ ...state, assignees }));
  }

  private setError(error: string) {
    this.todoState.update((state) => ({ ...state, error }));
  }

  private clearError() {  
    this.todoState.update((state) => ({ ...state, error: undefined }));
  }

  async getTodos() {
    this.setLoading(true);
    this.clearTodos();
    this.clearError();

    // Simulate loading
    await this.delay(1000);

    if (Math.random() < 0.1) {
      // Simulate error
      this.setError('Failed to fetch todos');
      this.setLoading(false);
      return;
    }

    let todos: Todo[] = [
      { title: 'Buy milk', assignedTo: 'John', done: false },
      { title: 'Clean the room', assignedTo: 'John', done: false },
      { title: 'Call mom', assignedTo: 'Jane', done: false },
      { title: 'Buy a present', assignedTo: 'Jane', done: true },
    ];

    this.setAssignees([...new Set(todos.map((todo) => todo.assignedTo))]);

    const filter = this.todoState().filter;
    if (filter) {
      todos = todos.filter((todo) => (!filter.done || !todo.done) && (!filter.assignedTo || todo.assignedTo === filter.assignedTo));
    }

    this.setTodos(todos);

    this.setLoading(false);
  }

  async addTodo(todo: Todo) {
    this.setLoading(true);

    // Simulate adding
    await this.delay(2000);

    if (Math.random() < 0.1) {
      // Simulate error
      this.setError('Failed to add todo');
      this.setLoading(false);
      return;
    }

    this.setTodos([...this.todos(), todo]);

    this.setLoading(false);
  }

  setDoneFilter(onlyDone: boolean) {
    this.todoState.update((state) => ({ ...state, filter: { ...state.filter, done: onlyDone } }));
    this.getTodos();
  }

  setAssignedToFilter(assignedTo: string) {
    this.todoState.update((state) => ({ ...state, filter: { ...state.filter, assignedTo } }));
    this.getTodos();
  }

  private delay(ms: number): Promise<void> {
    return new Promise((resolve) => setTimeout(resolve, ms));
  }
}
