import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { catchError } from 'rxjs/operators';

// Did a different approach
@Injectable({providedIn: 'root'})

export class MessagesResolver implements Resolve<Message[]> {
    pageNumber = 1;
    pageSize = 5;
    messageContainer = 'Unread';

    constructor(
        private authService: AuthService,
        private userService: UserService,
        private router: Router,
        private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Message[]> {
        const userId = this.authService.userFromDecodedToken().id;
        return this.userService.getMessages(userId, this.pageNumber, this.pageSize, this.messageContainer)
                    .pipe(
                        catchError(error => {
                            this.alertify.error('Problem retrieving messages');
                            this.router.navigate(['/home']);
                            return of(null);
                        })
                    );
    }

}