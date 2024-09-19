import { Component, model, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-number-guessing',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './number-guessing.component.html',
  styleUrls: ['./number-guessing.component.css'],
})
export class NumberGuessingComponent {
  randomNumber = signal<number>(this.generateRandomNumber());
  userGuess = model<number | undefined>();
  feedback = signal<string>('');
  numberOfGuesses = signal<number>(0);

  generateRandomNumber() {
    return Math.floor(Math.random() * 100) + 1;
  }

  makeGuess() {
    if (!this.userGuess()) {
      this.feedback.set('Please enter a guess.');
      return;
    }

    this.numberOfGuesses.update((n) => n + 1);
    const guess = this.userGuess()!;
    if (guess === this.randomNumber()) {
      this.feedback.set(`Found. It took you ${this.numberOfGuesses()} guesses.`);
    } else {
      if (guess < this.randomNumber()) {
        this.feedback.set('Higher');
      } else {
        this.feedback.set('Lower');
      }
    }
  }

  resetGame() {
    this.randomNumber.set(this.generateRandomNumber());
    this.userGuess.set(undefined);
    this.feedback.set('');
    this.numberOfGuesses.set(0);
  }
}
