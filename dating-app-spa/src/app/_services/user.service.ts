import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { PaginatedResult } from '../_models/pagination';
import { map, subscribeOn } from 'rxjs/operators';
import { Message } from '../_models/message';
import { JsonPipe } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl = environment.apiUrl + 'users/';

  constructor(private http: HttpClient) { }

  getUsers(page?, itemsPerPage?, userParams?, likesParams?): Observable<PaginatedResult<User[]>> {
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();

    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (userParams != null) {
      params = params.append('minAge', userParams.minAge);
      params = params.append('maxAge', userParams.maxAge);
      params = params.append('gender', userParams.gender);
      params = params.append('orderBy', userParams.orderBy);
    }

    if (likesParams === 'Likers') {
      params = params.append('Likers', 'true');
    }

    if (likesParams === 'Likees') {
      params = params.append('Likees', 'true');
    }

    const result = this.http.get<User[]>(this.baseUrl + 'get-all', { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
    return result;
  }

  getUser(id: number): Observable<User> {
    return this.http.get<User>(this.baseUrl + id);
  }

  updateUser(id: number, user: User) {
    return this.http.put(this.baseUrl + id, user);
  }

  setMainPhoto(userId: number, photoId: number) {
    return this.http.post(this.baseUrl + userId + '/photos/' + photoId + '/setMain', {});
  }

  deletePhoto(userId: number, photoId: number) {
    return this.http.delete(this.baseUrl + userId + '/photos/' + photoId);
  }

  sendLike(id: number, recipientId: number) {
    return this.http.post(this.baseUrl + id + '/like/' + recipientId, {});
  }

  getMessages(userId: number, page?, itemsPaerPage?, messageContainer?) {
    const paginatedResult: PaginatedResult<Message[]> = new PaginatedResult<Message[]>();
    let params = new HttpParams();

    params = params.append('messageContainer', messageContainer);
    if (page !== null && itemsPaerPage !== null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPaerPage);
    }

    return this.http.get<Message[]>(this.baseUrl + userId + '/messages/', { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') !== null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }

          return paginatedResult;
        })
      );
  }

  getMessageThread(id: number, recipientId: number) {
    return this.http.get<Message[]>(this.baseUrl + id + '/messages/thread/' + recipientId);
  }

  sendMessage(id: number, message: Message) {
    return this.http.post(this.baseUrl + id + '/messages', message);
  }

  deleteMessage(id: number, userId: number) {
    return this.http.delete(this.baseUrl + userId + '/messages/' + id, {});
  }

  markAsRead(id: number, userId: number) {
    debugger;
    this.http.post(this.baseUrl + userId + '/messages/' + id + '/read', {})
      .subscribe();
  }
}
