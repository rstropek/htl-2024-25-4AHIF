import { AfterViewInit, Component, inject } from '@angular/core';
import { TodosApiService } from '../todos-api.service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-todos-list',
  standalone: true,
  imports: [RouterModule],
  providers: [TodosApiService],
  templateUrl: './todos-list.component.html',
  styleUrl: './todos-list.component.css'
})
export class TodosListComponent implements AfterViewInit {
  client = inject(TodosApiService);
  
  ngAfterViewInit(): void {
    this.client.getTodos(/* Try filters here */);
  }
}
