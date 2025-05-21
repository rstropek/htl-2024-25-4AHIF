import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { BASE_URL } from './app.config';

// Bike API types
export type NewBikeDto = {
  title: string;
  serialNumberBikeComputer: string;
  etrtoDesignation?: string;
  circumference_mm?: number;
}

export type NewBikeResultDto = {
  id: number;
  title: string;
  serialNumberBikeComputer: string;
  circumference_mm: number;
}

export type GetBikesResultDto = {
  id: number;
  title: string;
  serialNumberBikeComputer: string;
  hasNewRides: boolean;
}

// Ride API types
export type RideDto = {
  id: number;
  title: string;
  uploadedAt: string | Date; // Can be string from API or Date after conversion
}

export type RideDetailsDto = {
  id: number;
  title: string;
  uploadedAt: string | Date; // Can be string from API or Date after conversion
  rideDuration_s: number;
  rideDistance_m: number;
  avgSpeed_kmh: number;
  numberOfStops: number;
  totalStopTime_s: number;
}

export type UpdateRideDto = {
  title: string;
}

@Injectable({
  providedIn: 'root'
})
export class BikeRideService {
  private httpClient = inject(HttpClient);
  private apiBaseUrl = inject(BASE_URL);

  // Bike APIs
  registerBike(newBike: NewBikeDto): Promise<NewBikeResultDto> {
    return firstValueFrom(this.httpClient.post<NewBikeResultDto>(
      `${this.apiBaseUrl}/bikes`, newBike));
  }

  getBikes(): Promise<GetBikesResultDto[]> {
    return firstValueFrom(this.httpClient.get<GetBikesResultDto[]>(
      `${this.apiBaseUrl}/bikes`));
  }

  // Ride APIs
  async getRides(): Promise<RideDto[]> {
    const rides = await firstValueFrom(this.httpClient.get<RideDto[]>(
      `${this.apiBaseUrl}/rides`));
    // Convert ISO string dates to Date objects
    return rides.map(ride => ({
      ...ride,
      uploadedAt: new Date(ride.uploadedAt as string)
    }));
  }

  async getRide(id: number): Promise<RideDetailsDto> {
    const ride = await firstValueFrom(this.httpClient.get<RideDetailsDto>(
      `${this.apiBaseUrl}/rides/${id}`));
    // Convert ISO string date to Date object
    return {
      ...ride,
      uploadedAt: new Date(ride.uploadedAt as string)
    };
  }

  deleteRide(id: number): Promise<void> {
    return firstValueFrom(this.httpClient.delete<void>(
      `${this.apiBaseUrl}/rides/${id}`));
  }

  updateRide(id: number, updateRide: UpdateRideDto): Promise<void> {
    return firstValueFrom(this.httpClient.patch<void>(
      `${this.apiBaseUrl}/rides/${id}`, updateRide));
  }
}
