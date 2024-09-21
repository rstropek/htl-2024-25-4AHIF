import { Component } from '@angular/core';
import { NumberGuessingSignalsComponent } from './number-guessing-signals/number-guessing-signals.component';
import { NumberGuessingTraditionalComponent } from './number-guessing-traditional/number-guessing-traditional.component';

@Component({
  selector: 'app-number-guessing',
  standalone: true,
  imports: [NumberGuessingSignalsComponent, NumberGuessingTraditionalComponent],
  templateUrl: './number-guessing.component.html',
  styleUrls: ['./number-guessing.component.css'],
})
export class NumberGuessingComponent {
}
