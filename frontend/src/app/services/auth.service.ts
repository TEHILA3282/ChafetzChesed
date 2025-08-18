import { Injectable, NgZone } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, BehaviorSubject } from 'rxjs';
import { environment } from '../environments/environment';
import { Router } from '@angular/router';
import { AccountAction } from './account-actions.service';
import { MessageService, Message } from './message.service';
import { UserSummary } from './account-actions.service';
import { DepositType } from './deposit-type.service';
import { LoanTypeService, LoanType } from './loan-type.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = environment.apiUrl;
  private tokenSubject = new BehaviorSubject<string | null>(null);

  private accountActions: AccountAction[] = [];
  private messages: Message[] = [];
  private userSummarySubject = new BehaviorSubject<UserSummary | null>(null);
  private depositTypes: DepositType[] = [];

  private loanTypes: LoanType[] = [];
  private loanTypesSubject = new BehaviorSubject<LoanType[]>([]);
  loanTypes$ = this.loanTypesSubject.asObservable();

  constructor(
    private http: HttpClient,
    private router: Router,
    private zone: NgZone,
    private messageService: MessageService,
    private loanTypeService: LoanTypeService
  ) {
    const savedToken = localStorage.getItem('token');
    if (savedToken) this.tokenSubject.next(savedToken);
  }

  login(credentials: { emailOrId: string, password: string, institutionId: number }): Observable<any> {
    return this.http.post(`${this.apiUrl}/auth/login`, {
      identifier: credentials.emailOrId,
      password: credentials.password,
      institutionId: credentials.institutionId
    }).pipe(
      tap((res: any) => {
        localStorage.setItem('token', res.token);
        localStorage.setItem('user', JSON.stringify(res.user));
        if (res.user?.firstName) {
          localStorage.setItem('userName', res.user.firstName);
        }
        this.tokenSubject.next(res.token);

        if (res.user?.registrationStatus !== 'מאושר') {
          console.warn('חשבון לא מאושר – לא טוענים מידע נוסף');
          return;
        }

        setTimeout(() => {
          this.http.get<AccountAction[]>(`${this.apiUrl}/users/account-actions`).subscribe({
            next: actions => this.setAccountActions(actions),
            error: err => console.warn('שגיאה בטעינת פעולות:', err)
          });

          this.messageService.getMessages().subscribe({
            next: messages => this.setMessages(messages),
            error: err => console.warn('שגיאה בטעינת הודעות:', err)
          });

          this.http.get<DepositType[]>(`${this.apiUrl}/DepositTypes`).subscribe({
            next: (types) => { this.depositTypes = types; },
            error: err => console.warn('שגיאה בטעינת סוגי הפקדות:', err)
          });

          this.loanTypeService.getLoanTypes().subscribe({
            next: (types) => { 
              this.loanTypes = types;
              this.loanTypesSubject.next(types); 
            },
            error: err => console.warn('שגיאה בטעינת סוגי הלוואות:', err)
          });

          this.http.get<UserSummary>(`${this.apiUrl}/users/account-summary`).subscribe({
            next: summary => this.setUserSummary(summary),
            error: err => console.warn('שגיאה בטעינת סיכום כספי:', err)
          });
        }, 0);
      })
    );
  }

  fetchUserFromServer(userId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/auth/get-user/${userId}`);
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    localStorage.removeItem('userName');
    this.tokenSubject.next(null);
    this.accountActions = [];
    this.messages = [];
    this.userSummarySubject.next(null);
    this.loanTypes = [];
    this.loanTypesSubject.next([]); 
    this.zone.run(() => {
      this.router.navigate(['/home']);
    });
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getTokenObservable(): Observable<string | null> {
    return this.tokenSubject.asObservable();
  }

  getCurrentUser(): any {
    const userJson = localStorage.getItem('user');
    return userJson ? JSON.parse(userJson) : null;
  }

  setAccountActions(actions: AccountAction[]) {
    this.accountActions = actions;
  }
  getAccountActions(): AccountAction[] {
    return this.accountActions;
  }

  setMessages(messages: Message[]) {
    this.messages = messages;
  }
  getMessages(): Message[] {
    return this.messages;
  }

  setUserSummary(summary: UserSummary) {
    this.userSummarySubject.next(summary);
  }
  getUserSummary(): Observable<UserSummary | null> {
    return this.userSummarySubject.asObservable();
  }

  getDepositTypes(): DepositType[] {
    return this.depositTypes;
  }

  getLoanTypes(): LoanType[] {
    return this.loanTypes;
  }
}
