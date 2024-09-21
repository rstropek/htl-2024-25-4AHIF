import { Component, computed, ElementRef, model, signal, viewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';

// Learn more about signals at https://angular.dev/guide/signals

@Component({
  selector: 'app-number-guessing-signals',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './number-guessing-signals.component.html',
  styleUrl: './number-guessing-signals.component.css',
})
export class NumberGuessingSignalsComponent {
  randomNumber = signal<number>(0);
  userGuess = model<number | undefined>();
  feedback = signal<string>('');
  numberOfGuesses = signal<number>(0);
  guesses = signal<{ guess: number; result: '⬆️' | '⬇️' | '👍' }[]>([]);

  found = computed(() => {
    const guesses = this.guesses();
    return guesses.length > 0 && guesses[guesses.length - 1].result === '👍';
  });
  
  userGuessInput = viewChild<ElementRef>('userGuessInput');

  private static generateRandomNumber() {
    return Math.floor(Math.random() * 100) + 1;
  }

  constructor() {
    this.resetGame();
  }

  makeGuess() {
    this.numberOfGuesses.update((n) => n + 1);
    const guess = this.userGuess()!;
    if (guess === this.randomNumber()) {
      this.guesses.update((g) => [...g, { guess, result: '👍' }]);
      this.feedback.set(`Found. It took you ${this.numberOfGuesses()} guesses.`);
    } else {
      if (guess < this.randomNumber()) {
        this.guesses.update((g) => [...g, { guess, result: '⬆️' }]);
        this.feedback.set('Higher');
      } else {
        this.guesses.update((g) => [...g, { guess, result: '⬇️' }]);
        this.feedback.set('Lower');
      }

      const element = this.userGuessInput()?.nativeElement as HTMLInputElement;
      element.select();
      element.focus();
    }
  }

  resetGame() {
    this.randomNumber.set(NumberGuessingSignalsComponent.generateRandomNumber());
    this.userGuess.set(undefined);
    this.feedback.set('');
    this.numberOfGuesses.set(0);
    this.guesses.update(_ => []);
  }
}
