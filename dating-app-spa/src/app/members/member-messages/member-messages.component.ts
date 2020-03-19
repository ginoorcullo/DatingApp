import { Component, OnInit, Input } from '@angular/core';
import { Message } from 'src/app/_models/message';
import { UserService } from 'src/app/_services/user.service';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  @Input() recipientId: number;
  messages: Message[];
  newMessage: any = {};

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private alertify: AlertifyService) { }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {    
    const currentUserId = +this.authService.userFromDecodedToken().id;
    this.userService.getMessageThread(this.authService.userFromDecodedToken().id, this.recipientId)
      .pipe(
        tap(messages => {          
          for (let i = 0; i < messages.length; i++) {
            if (messages[i].isRead === false && messages[i].recipientId === currentUserId) {
              this.userService.markAsRead(messages[i].id, currentUserId);
            }
          }
        })
      )
      .subscribe(result => {
        this.messages = result;
      }, error => {
        this.alertify.error(error);
      });
  }

  sendMessage() {
    this.newMessage.recipientId = this.recipientId;
    this.userService.sendMessage(this.authService.userFromDecodedToken().id, this.newMessage)
      .subscribe((message: Message) => {
        this.messages.unshift(message);
        this.newMessage = {};
      }, error => {
        this.alertify.error(error);
      });
  }

}
