import { Component, inject, Input, signal } from '@angular/core';
import { BikeRideService, RideDetailsDto, RideDto } from '../bike-ride.service';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-ride-list',
  imports: [RouterModule, FormsModule, DecimalPipe],
  templateUrl: './ride-list.component.html',
  styleUrl: './ride-list.component.css'
})
export class RideListComponent {
  public readonly rides = signal<RideDto[] | null>(null);
  public readonly bikeId = signal<string | null>(null);
  public readonly renamingRideId = signal<number | null>(null);
  public readonly renamingRideTitle = signal<string | null>(null);
  public readonly rideDetails = signal<RideDetailsDto | null>(null);

  private readonly bikeRideService = inject(BikeRideService);

  @Input() public set id(bikeId: string) {
    this.bikeId.set(bikeId);
    this.bikeRideService.getRides(parseInt(bikeId)).then(rides => {
      this.rides.set(rides);
    });
  }

  public async deleteRide(rideId: number) {
    await this.bikeRideService.deleteRide(rideId);
    this.rides.set(await this.bikeRideService.getRides(parseInt(this.bikeId()!)));
  }

  public async showRenameRideModal(rideId: number, title: string) {
    this.renamingRideId.set(rideId);
    this.renamingRideTitle.set(title);
  }

  public async renameRide(rideId: number, newTitle: string) {
    await this.bikeRideService.updateRide(rideId, { title: newTitle });
    this.rides.set(await this.bikeRideService.getRides(parseInt(this.bikeId()!)));
    this.renamingRideId.set(null);
    this.renamingRideTitle.set(null);
  }

  public async getRideDetails(rideId: number) {
    this.rideDetails.set(await this.bikeRideService.getRide(rideId));
  }
}
