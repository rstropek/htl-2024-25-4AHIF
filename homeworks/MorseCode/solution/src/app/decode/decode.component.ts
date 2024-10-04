import { Component, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DecoderService } from '../decoder.service';

@Component({
  selector: 'app-decode',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './decode.component.html',
  styleUrl: './decode.component.css'
})
export class DecodeComponent {
  private decoder = inject(DecoderService);

  public input = signal('...- .. . .-.. / . .-. ..-. --- .-.. --.');
  public plainText = signal('');
  public errorMessage = signal('');
  public canDecode = computed(() => this.decoder.canDecode(this.input()));

  public onDecode() {
    try {
      this.plainText.set(this.decoder.decode(this.input()));
      this.errorMessage.set('');
    } catch (ex: any) {
      this.errorMessage.set(ex);
    }
  }
}
