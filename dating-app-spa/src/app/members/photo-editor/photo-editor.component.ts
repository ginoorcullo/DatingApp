import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { Photo } from 'src/app/_models/photo';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos: Photo[];
  @Output() getMemberPhotoChange = new EventEmitter<string>();

  currentMain: Photo;
  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  response: string;
  baseUrl = environment.apiUrl;

  constructor(private authService: AuthService, private userService: UserService, private alertify: AlertifyService) {
    this.initializeUploader();
  }

  ngOnInit(): void {
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/' + this.authService.userFromDecodedToken().id + '/photos',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: Photo = JSON.parse(response);
        const photo = {
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          description: res.description,
          isMain: res.isMain
        }
        this.photos.push(photo);
      }
    };
  }

  setPhotoToMain(photo: Photo) {
    const userId = this.authService.userFromDecodedToken().id;
    this.userService.setMainPhoto(userId, photo.id).subscribe(next => {
      this.alertify.success('new main photo updated.');
      this.currentMain = this.photos.filter(p => p.isMain === true)[0];
      this.currentMain.isMain = false;
      photo.isMain = true;
      this.authService.changeMemberPhoto(photo.url);
      this.authService.currentUser.photoURL = photo.url;
      this.authService.updateLocalUser();
    }, error => {
      this.alertify.error(error);
    });
  }

  deletePhoto(photo: Photo) {
    this.alertify.confirm('Are you sure you want to delete this photo?', () => {
      const userId = this.authService.userFromDecodedToken().id;
      this.userService.deletePhoto(userId, photo.id).subscribe(() => {
        this.photos.splice(this.photos.findIndex(p => p.id === photo.id), 1);
        this.alertify.success('successfully deleted photo.');
      }, error => {
        this.alertify.error(error);
      });
    });

  }

}
