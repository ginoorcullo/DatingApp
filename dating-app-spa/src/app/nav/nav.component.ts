import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';
import { User } from '../_models/user';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};
  loggedInModel: any = {};
  photoUrl: string;

  constructor(
    private authService: AuthService,
    private alertify: AlertifyService,
    private router: Router) { }

  ngOnInit(): void {
    this.loggedInModel = this.authService.userFromDecodedToken();
    this.authService.currentPhotoUrl.subscribe(currPhotoUrl => this.photoUrl = currPhotoUrl);
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      this.loggedInModel = this.authService.userFromDecodedToken();
      this.authService.currentPhotoUrl.subscribe(currPhotoUrl => this.photoUrl = currPhotoUrl);
      this.alertify.success('logged in successfully');
    }, error => {
      this.alertify.error(error);
    }, () => {
      this.router.navigate(['/members']);
    });
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  logOut() {
    this.authService.logout();
    this.model = {};
    this.alertify.message('logged out');
    this.router.navigate(['/home']);
  }

}
