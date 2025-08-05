import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { MessageService, Message } from '../../services/message.service';
import { AuthService } from '../../services/auth.service';
import { Subscription, filter } from 'rxjs';

@Component({
  selector: 'app-messages-box',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  templateUrl: './messages-box.html',
  styleUrls: ['./messages-box.scss']
})
export class MessagesBoxComponent implements OnInit, OnDestroy {
  messages: Message[] = [];
  private tokenSub: Subscription | undefined;

  constructor(
    private messageService: MessageService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.tokenSub = this.authService.getTokenObservable()
      .pipe(filter(token => !!token))
      .subscribe(() => this.loadMessages());

    if (this.authService.getToken()) {
      this.loadMessages();
    }
  }

  loadMessages(): void {
    this.messageService.getMessages().subscribe({
      next: (data: Message[]) => this.messages = data,
      error: (err: any) => console.error('שגיאה בטעינת הודעות:', err)
    });
  }

  ngOnDestroy(): void {
    this.tokenSub?.unsubscribe();
  }
}
