import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HttpClientModule } from '@angular/common/http';

import { MessageService, Message } from '../../services/message.service';


@Component({
  selector: 'app-messages-box',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  templateUrl: './messages-box.html',
  styleUrls: ['./messages-box.scss']
})
export class MessagesBoxComponent implements OnInit {
  messages: Message[] = [];

  constructor(private messageService: MessageService) {}

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages(): void {
    this.messageService.getMessages().subscribe({
      next: (data: Message[]) => this.messages = data,
      error: (err: any) => console.error('שגיאה בטעינת הודעות:', err)
    });
  }
}
