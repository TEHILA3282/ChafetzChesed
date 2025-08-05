import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, tap } from 'rxjs';
import { environment } from '../environments/environment';

export interface Message {
  id: number;
  clientID: string;
  messageType: string;
  messageText: string;
  dateSent: Date;
  isRead: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private apiUrl = `${environment.apiUrl}/messages`;
  private messagesSubject = new BehaviorSubject<Message[]>([]);
  messages$ = this.messagesSubject.asObservable();

  constructor(private http: HttpClient) {
    const cached = localStorage.getItem('messages');
    if (cached) {
      this.messagesSubject.next(JSON.parse(cached));
    }
  }

  getMessages(): Observable<Message[]> {
    return this.http.get<Message[]>(this.apiUrl).pipe(
      tap(messages => {
        localStorage.setItem('messages', JSON.stringify(messages));
        this.messagesSubject.next(messages);
      })
    );
  }
}
