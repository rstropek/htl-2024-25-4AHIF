import { computed, Injectable, signal } from '@angular/core';

/** Master data of new todo item (not done by default) */
export type NewTodo = {
  title: string;
  assignedTo: string;
};

/** Master data of existing todo item */
export type Todo = NewTodo & {
  id: number;
  done: boolean;
};

/** Filter fields */
export type GetTodosFilter = {
  onlyUnfinished?: boolean;
  assignedTo?: string;
};

export type TodoState = {
  isLoading: boolean;
  filteredTodos: Todo[];
  assignees: string[];
  error?: string;
  filter: GetTodosFilter;
};

@Injectable({
  providedIn: 'root',
})
export class TodoStoreService {
  // Helper for generating primary keys
  private nextId = 1;

  // Store
  private todoStore: Todo[] = [
    { id: this.nextId++, title: 'Buy milk', assignedTo: 'John', done: false },
    { id: this.nextId++, title: 'Clean the room', assignedTo: 'John', done: false },
    { id: this.nextId++, title: 'Call mom', assignedTo: 'Jane', done: false },
    { id: this.nextId++, title: 'Buy a present', assignedTo: 'Jane', done: true },
  ];

  // This signal holds the state of the todo store
  private todoState = signal<TodoState>({
    isLoading: false, // Single loading indicator; could be extended for more complex scenarios
    filteredTodos: [],
    assignees: ['John', 'Jane'],
    filter: {},
  });

  // #region Selectors
  isLoading = computed(() => this.todoState().isLoading);
  todos = computed(() => this.todoState().filteredTodos);
  assignees = computed(() => this.todoState().assignees);
  error = computed(() => this.todoState().error);
  // #endregion

  // #region Private actions
  private setLoading(isLoading: boolean) {
    this.todoState.update((state) => ({ ...state, isLoading }));
  }

  private setFilteredTodos(todos: Todo[]) {
    this.todoState.update((state) => ({ ...state, filteredTodos: todos }));
  }

  private setError(error: string) {
    this.todoState.update((state) => ({ ...state, error }));
  }

  private clearError() {
    this.todoState.update((state) => ({ ...state, error: undefined }));
  }
  // #endregion

  // #region Public actions
  async reload() {
    this.setLoading(true);
    this.clearError();

    // Simulate loading
    await this.delay(1000);

    try {
      if (Math.random() < 0.1) {
        // Simulate error
        this.setError('Failed to fetch todos');
        return;
      }

      const filter = this.todoState().filter;
      let todos = this.todoStore;
      if (filter) {
        todos = todos.filter((todo) => (!filter.onlyUnfinished || !todo.done) && (!filter.assignedTo || todo.assignedTo === filter.assignedTo));
      }

      // Data binding of component will be triggered by setting the signal
      this.setFilteredTodos(todos);
    } finally {
      this.setLoading(false);
    }
  }

  async addTodo(todo: NewTodo): Promise<void> {
    this.setLoading(true);

    // Simulate adding
    await this.delay(1000);

    this.todoStore = [...this.todoStore, { ...todo, id: this.nextId++, done: false }];

    this.setLoading(false);
  }

  setDoneFilter(onlyDone: boolean) {
    this.todoState.update((state) => ({ ...state, filter: { ...state.filter, onlyUnfinished: onlyDone } }));
    this.reload();
  }

  setAssignedToFilter(assignedTo: string) {
    this.todoState.update((state) => ({ ...state, filter: { ...state.filter, assignedTo } }));
    this.reload();
  }

  async updateTodo(todo: Todo): Promise<void> {
    this.setLoading(true);

    // Simulate adding
    await this.delay(1000);

    this.todoStore = this.todoStore.map((t) => (t.id === todo.id ? todo : t));

    this.setLoading(false);
  }
  // #endregion

  // #region Helpers
  private delay(ms: number): Promise<void> {
    return new Promise((resolve) => setTimeout(resolve, ms));
  }
  // #endregion
}
