import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { BikeRideService, NewBikeDto } from '../bike-ride.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';

type EtrtoSize = {
  rim_diam: number;
  tire: number;
  ETRTO: string;
  circumference_mm: number;
};

@Component({
  selector: 'app-register-bike',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './register-bike.component.html',
  styleUrl: './register-bike.component.css'
})
export class RegisterBikeComponent {
  public readonly title = signal<string>('');
  public readonly serialNumberBikeComputer = signal<string>('');
  public readonly sizeMethod = signal<'etrto' | 'manual'>('etrto');
  public readonly manualDiameter = signal<number | null>(null);
  public readonly selectedEtrtoSize = signal<string>('');
  public readonly isFormValid = computed(() => {
    return this.title() !== '' && this.serialNumberBikeComputer() !== '' &&
      (this.sizeMethod() === 'etrto' ? this.selectedEtrtoSize() !== '' : this.manualDiameter() !== null);
  });

  public readonly etrtoSizes: EtrtoSize[] = [
      { "rim_diam": 26, "tire": 1.0, "ETRTO": "25-559", "circumference_mm": 1913 },
      { "rim_diam": 26, "tire": 1.25, "ETRTO": "32-559", "circumference_mm": 1950 },
      { "rim_diam": 26, "tire": 1.4, "ETRTO": "37-559", "circumference_mm": 2005 },
      { "rim_diam": 26, "tire": 1.5, "ETRTO": "40-559", "circumference_mm": 2010 },
      { "rim_diam": 26, "tire": 1.75, "ETRTO": "47-559", "circumference_mm": 2023 },
      { "rim_diam": 26, "tire": 1.95, "ETRTO": "50-559", "circumference_mm": 2050 },
      { "rim_diam": 26, "tire": 2.0, "ETRTO": "52-559", "circumference_mm": 2055 },
      { "rim_diam": 26, "tire": 2.1, "ETRTO": "54-559", "circumference_mm": 2068 },
      { "rim_diam": 26, "tire": 2.125, "ETRTO": "57-559", "circumference_mm": 2070 },
      { "rim_diam": 26, "tire": 2.35, "ETRTO": "58-559", "circumference_mm": 2083 },
      { "rim_diam": 26, "tire": 3.0, "ETRTO": "75-559", "circumference_mm": 2170 },
      { "rim_diam": 27.5, "tire": 1.5, "ETRTO": "40-584", "circumference_mm": 2079 },
      { "rim_diam": 27.5, "tire": 1.95, "ETRTO": "50-584", "circumference_mm": 2090 },
      { "rim_diam": 27.5, "tire": 2.1, "ETRTO": "54-584", "circumference_mm": 2148 },
      { "rim_diam": 27.5, "tire": 2.25, "ETRTO": "57-584", "circumference_mm": 2182 },
      { "rim_diam": 27.5, "tire": 2.3, "ETRTO": "58-584", "circumference_mm": 2199 },
      { "rim_diam": 27.5, "tire": 2.35, "ETRTO": "60-584", "circumference_mm": 2207 },
      { "rim_diam": 27.5, "tire": 2.4, "ETRTO": "61-584", "circumference_mm": 2213 },
      { "rim_diam": 27.5, "tire": 2.5, "ETRTO": "64-584", "circumference_mm": 2231 },
      { "rim_diam": 27.5, "tire": 2.6, "ETRTO": "66-584", "circumference_mm": 2247 },
      { "rim_diam": 27.5, "tire": 2.8, "ETRTO": "71-584", "circumference_mm": 2279 },
      { "rim_diam": 29, "tire": 2.1, "ETRTO": "54-622", "circumference_mm": 2286 },
      { "rim_diam": 29, "tire": 2.2, "ETRTO": "56-622", "circumference_mm": 2302 },
      { "rim_diam": 29, "tire": 2.25, "ETRTO": "57-622", "circumference_mm": 2310 },
      { "rim_diam": 29, "tire": 2.3, "ETRTO": "58-622", "circumference_mm": 2326 },
      { "rim_diam": 29, "tire": 2.35, "ETRTO": "60-622", "circumference_mm": 2326 },
      { "rim_diam": 29, "tire": 2.4, "ETRTO": "61-622", "circumference_mm": 2333 },
      { "rim_diam": 29, "tire": 2.5, "ETRTO": "64-622", "circumference_mm": 2350 },
      { "rim_diam": 29, "tire": 2.6, "ETRTO": "66-622", "circumference_mm": 2366 }
    ];

  // Service
  private readonly bikeRideService = inject(BikeRideService);
  private readonly router = inject(Router);

  // Error handling
  public readonly errorMessage = signal<string | null>(null);
  public readonly isSubmitting = signal(false);

  // Display text for ETRTO size in the dropdown
  getEtrtoDisplayText(size: EtrtoSize): string {
    return `${size.ETRTO} (${size.rim_diam}" × ${size.tire}" - ${size.circumference_mm} mm)`;
  }

  // Method to switch size input method
  toggleSizeMethod(method: 'etrto' | 'manual'): void {
    this.sizeMethod.set(method);

    // Reset relevant fields
    if (method === 'etrto') {
      this.manualDiameter.set(null);
    } else {
      this.selectedEtrtoSize.set('');
    }
  }

  async submitForm(): Promise<void> {
    if (!this.isFormValid()) {
      this.errorMessage.set('Please fill in all required fields correctly.');
      return;
    }

    try {
      this.isSubmitting.set(true);
      this.errorMessage.set(null);

      const newBike: NewBikeDto = {
        title: this.title(),
        serialNumberBikeComputer: this.serialNumberBikeComputer(),
      };

      // Set the correct sizing property based on selection method
      if (this.sizeMethod() === 'etrto') {
        newBike.etrtoDesignation = this.selectedEtrtoSize();
        newBike.diameter_mm = undefined;
      } else {
        newBike.diameter_mm = this.manualDiameter() as number;
        newBike.etrtoDesignation = undefined;
      }

      // Call the service to register the bike
      const result = await this.bikeRideService.registerBike(newBike);

      // Navigate back to bike list on success
      this.router.navigate(['/bike-list']);
    } catch (error) {
      console.error('Error registering bike:', error);
      this.errorMessage.set('Failed to register bike. Please try again.');
    } finally {
      this.isSubmitting.set(false);
    }
  }
}
