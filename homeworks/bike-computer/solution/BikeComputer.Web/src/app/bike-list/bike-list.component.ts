import { Component, inject, OnInit, signal, Signal } from '@angular/core';
import { BikeRideService, GetBikesResultDto } from '../bike-ride.service';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-bike-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './bike-list.component.html',
  styleUrl: './bike-list.component.css'
})
export class BikeListComponent implements OnInit {
  public readonly bikes = signal<GetBikesResultDto[] | null>(null);

  private readonly bikeRideService = inject(BikeRideService);
  private readonly router = inject(Router);

  async ngOnInit() {
    this.bikes.set(await this.bikeRideService.getBikes());
  }

  public registerBike() {
    this.router.navigate(['/register-bike']);
  }
}
