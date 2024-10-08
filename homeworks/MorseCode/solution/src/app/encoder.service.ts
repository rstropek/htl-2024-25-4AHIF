import { inject, Injectable } from '@angular/core';
import { ALPHABET } from './app.config';

const A = 'A'.charCodeAt(0);
const textToConvertRegex = /^[A-Z ]+$/;

@Injectable({
  providedIn: 'root'
})
export class EncoderService {
  private alphabet = inject(ALPHABET);

  public canEncode(textToConvert: string): boolean {
    // This sample uses RegEx, other solutions are possible
    return textToConvertRegex.test(textToConvert);
  }

  public encode(textToConvert: string): string {
    let morseString = '';
    for (const letterToConvert of textToConvert.trim()) {
      if (letterToConvert >= 'A' && letterToConvert <= 'Z') {
        if (morseString !== '' && !morseString.endsWith(' ')) {
          morseString += ' ';
        }

        morseString += this.alphabet[letterToConvert.charCodeAt(0) - A];
      } else if (letterToConvert === ' ') {
        if (!morseString.endsWith(' / ')) {
          morseString += ' / ';
        }
      } else {
        throw new Error(`Invalid text, contains unknown character ${letterToConvert}`)
      }
    }

    return morseString;
  }
}
