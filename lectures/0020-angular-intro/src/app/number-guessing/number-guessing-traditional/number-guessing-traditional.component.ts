import { Component, computed, ElementRef, model, signal, ViewChild, viewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-number-guessing-traditional',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './number-guessing-traditional.component.html',
  styleUrl: './number-guessing-traditional.component.css',
})
export class NumberGuessingTraditionalComponent {
  randomNumber = 0;
  userGuess: number | undefined = undefined;
  feedback = '';
  numberOfGuesses = 0;
  guesses: { guess: number; result: '⬆️' | '⬇️' | '👍' }[] = [];

  get found() {
    const guesses = this.guesses;
    return guesses.length > 0 && guesses[guesses.length - 1].result === '👍';
  }
  
  @ViewChild('userGuessInput') userGuessInput: ElementRef | undefined;

  private static generateRandomNumber() {
    return Math.floor(Math.random() * 100) + 1;
  }

  constructor() {
    this.resetGame();
  }

  makeGuess() {
    if (!this.userGuess) {
      return;
    }

    this.numberOfGuesses++;
    if (this.userGuess === this.randomNumber) {
      this.guesses.push({ guess: this.userGuess, result: '👍' });
      this.feedback = `Found. It took you ${this.numberOfGuesses} guesses.`;
    } else {
      if (this.userGuess < this.randomNumber) {
        this.guesses.push({ guess: this.userGuess, result: '⬆️' });
        this.feedback = 'Higher';
      } else {
        this.guesses.push({ guess: this.userGuess, result: '⬇️' });
        this.feedback = 'Lower';
      }

      const element = this.userGuessInput?.nativeElement as HTMLInputElement;
      element.select();
      element.focus();
    }
  }

  resetGame() {
    this.randomNumber = NumberGuessingTraditionalComponent.generateRandomNumber();
    delete this.userGuess;
    this.feedback = '';
    this.numberOfGuesses = 0;
    this.guesses = [];
  }
}
