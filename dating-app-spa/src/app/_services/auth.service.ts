import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  private decodedToken: any;
  currentUser: User;
  photoUrl = new BehaviorSubject<string>('../../assets/defaultPhoto.png');
  currentPhotoUrl = this.photoUrl.asObservable();

  constructor(private http: HttpClient) { }

  changeMemberPhoto(photoUrl: string) {
    this.photoUrl.next(photoUrl);
  }

  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model)
      .pipe(
        map((response: any) => {
          const user = response;
          if (user) {
            localStorage.setItem('token', user.token);
            localStorage.setItem('user', JSON.stringify(user.userDetails));
            this.decodedToken = this.jwtHelper.decodeToken(user.token);
            this.currentUser = user.userDetails;
            this.changeMemberPhoto(this.currentUser.photoURL);
          }
        })
      );
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.decodedToken = null;
    this.currentUser = null;
  }

  getTokenFromLocal() {
    const token = localStorage.getItem('token');
    if (token) {
      this.decodedToken = this.jwtHelper.decodeToken(token);
    }

  }

  getUserFromLocal() {
    const user = localStorage.getItem('user');
    if (user) {
      this.currentUser = JSON.parse(user);
      this.changeMemberPhoto(this.currentUser.photoURL);
    }
  }

  updateLocalUser() {
    localStorage.setItem('user', JSON.stringify(this.currentUser));
  }

  userFromDecodedToken() {
    const { nameid } = this.decodedToken;
    return {
      id: nameid[0],
      name: nameid[1]
    };
  }

  register(user: User) {
    return this.http.post(this.baseUrl + 'register', user);
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

}


