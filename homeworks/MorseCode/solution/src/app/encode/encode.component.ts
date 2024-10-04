import { Component, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { LETTER_BREAK, PlayMorseCodeService, SYMBOL_BREAK, WORD_BREAK } from '../play-morse-code.service';
import { EncoderService } from '../encoder.service';

@Component({
  selector: 'app-encode',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './encode.component.html',
  styleUrl: './encode.component.css'
})
export class EncodeComponent {
  private encoder = inject(EncoderService);
  private playService = inject(PlayMorseCodeService);

  public input = signal('HELLO WORLD');
  public morseCode = signal('');
  public canEncode = computed(() => this.encoder.canEncode(this.input()));

  public onEncode() {
    this.morseCode.set(this.encoder.encode(this.input()));
  }

  public onPlay() {
    this.playService.initializeAudioContext();
    const words = this.morseCode().split(' / ');
    this.playSentence(words);
  }

  private async playLetter(letter: string) {
    for (let dashdot of letter) {
      if (dashdot === '-') {
        await this.playService.playDash();
      } else if (dashdot === '.') {
        await this.playService.playDot();
      }

      await this.playService.sleep(SYMBOL_BREAK);
    }
  }

  private async playWord(word: string) {
    for (let letter of word) {
      await this.playLetter(letter);
      await this.playService.sleep(LETTER_BREAK);
    }
  }

  private async playSentence(words: string[]) {
    for (let word of words) {
      await this.playWord(word);
      await this.playService.sleep(WORD_BREAK);
    }
  }
}
