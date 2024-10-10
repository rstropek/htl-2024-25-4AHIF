import { AfterViewInit, Component, ElementRef, inject, signal, viewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { OpenAIService } from '../open-ai.service';
import { MarkdownModule } from 'ngx-markdown';

@Component({
  selector: 'app-answer-question',
  standalone: true,
  imports: [FormsModule, MarkdownModule],
  templateUrl: './answer-question.component.html',
  styleUrl: './answer-question.component.css'
})
export class AnswerQuestionComponent implements AfterViewInit {
  question = signal('');
  answer = signal('');
  message = signal('');

  openAiService = inject(OpenAIService);

  questionInput = viewChild.required<ElementRef<HTMLInputElement>>('questionInput');

  ngAfterViewInit() {
    this.questionInput().nativeElement.focus();
  }

  async submit() {
    this.message.set('Thinking 🧠 ...');
    const response = await this.openAiService.answerQuestion(this.question());
    this.answer.set(response.choices[0].message.content);
    this.message.set('');
  }
}
