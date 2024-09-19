import { NgClass } from '@angular/common';
import { Component, computed, signal } from '@angular/core';

// Note the use of TypeScript union types here.
// https://www.typescriptlang.org/docs/handbook/2/everyday-types.html#union-types
type Player = 'X' | 'O';
type SquareValue = Player | ' ';

@Component({
  selector: 'app-tic-tac-toe',
  standalone: true,
  imports: [NgClass],
  templateUrl: './tic-tac-toe.component.html',
  styleUrl: './tic-tac-toe.component.css',
})
export class TicTacToeComponent {
  // Note the use of signals here. Learn more at https://angular.dev/guide/signals

  /** Contains values of TTT board */
  board = signal<SquareValue[]>(Array(9).fill(' '));

  currentPlayer = signal<Player>('X');

  /** If there is a winner, this contains the three indexes of the winning squares */
  winningLine = signal<number[] | undefined>(undefined);

  /** Indicates whether there is a winner */
  winner = computed<SquareValue | undefined>(() => this.winningLine() ? this.board()[this.winningLine()![0]] : undefined);

  makeMove(index: number) {
    // No moves if we already have a winner
    if (this.winner()) {
      return;
    }

    // Change the board
    this.board.update((board) => [...board.slice(0, index), this.currentPlayer(), ...board.slice(index + 1)]);

    // Find the winning line
    const winningLine = this.getWinner();
    if (winningLine) {
      this.winningLine.set(winningLine);
    } else {
      // If no winner, switch to next player
      this.currentPlayer.update((cp) => (cp === 'X' ? 'O' : 'X'));
    }
  }

  getWinner(): number[] | undefined {
    const winningCombinations = [
      // Horizontal
      [0, 1, 2],
      [3, 4, 5],
      [6, 7, 8],

      // Vertical
      [0, 3, 6],
      [1, 4, 7],
      [2, 5, 8],

      // Diagonals
      [0, 4, 8],
      [2, 4, 6],
    ];

    // Check if there is a winning line
    const winning = winningCombinations.filter((combination) => {
      const [a, b, c] = combination;
      return this.board()[a] !== ' ' && this.board()[a] === this.board()[b] && this.board()[a] === this.board()[c];
    });

    if (winning.length > 0) {
      return winning[0];
    }

    return;
  }

  resetGame() {
    this.board.set(Array<SquareValue>(9).fill(' '));
    this.winningLine.set(undefined);
    this.currentPlayer.set('X');
  }
}
