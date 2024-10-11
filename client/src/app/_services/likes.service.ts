import { Injectable, inject, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Member } from '../_models/member';
import { PaginatedResult } from '../_models/pagination';

@Injectable({
  providedIn: 'root'
})

export class LikesService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  likeIds = signal<number[]>([]);
  paginatedResult = signal<PaginatedResult<Member[]> | null>(null);

  toggleLike(targetId: number) {
    return this.http.post(`${this.baseUrl}/likes/${targetId}`, {})
  }

  getLikes(predicate: string, pageNumber: number, pageSize: number) {
    return this.http.get(`${this.baseUrl}/likes?predicate=${predicate}`)
  }
  getLikeIds() {
    return this.http.get<number[]>(`${this.baseUrl}/likes/list`).subscribe({
      next: ids => this.likeIds.set(ids)
    })
  }
}