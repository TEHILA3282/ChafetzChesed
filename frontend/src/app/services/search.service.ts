import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface SearchResult {
  title: string;
  route: string;
  type: string;
}

@Injectable({ providedIn: 'root' })
export class SearchService {
  constructor(private http: HttpClient) {}

  search(term: string): Observable<SearchResult[]> {
    return this.http.get<SearchResult[]>(`/api/search/${term}`);
  }
}
