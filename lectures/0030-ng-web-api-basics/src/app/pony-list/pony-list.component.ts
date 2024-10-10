import { JsonPipe } from '@angular/common';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';

type Pony = {
  "id": number,
  "name": string,
  "alias": string,
};

type PonyResponse = {
  "data": Pony[]
}

@Component({
  selector: 'app-pony-list',
  standalone: true,
  imports: [JsonPipe],
  templateUrl: './pony-list.component.html',
  styleUrl: './pony-list.component.css'
})
export class PonyListComponent implements OnInit {
  ponies = signal<Pony[] | undefined>(undefined);

  private readonly httpClient = inject(HttpClient);

  async ngOnInit() {
    const ponyResponse = await this.getPonies(5, 6);
    this.ponies.set(ponyResponse.data);
  }

  getPonies(offset?: number, limit?: number): Promise<PonyResponse> {
    let params = new HttpParams();
    if (offset) { params = params.set('offset', offset); }
    if (limit) { params = params.set('limit', limit); }
    return firstValueFrom(this.httpClient.get<PonyResponse>(
      `https://ponyapi.net/v1/character/all?${params}`));
  }
}
