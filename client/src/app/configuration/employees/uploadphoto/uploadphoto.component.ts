import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-uploadphoto',
  templateUrl: './uploadphoto.component.html',
  styleUrls: ['./uploadphoto.component.css']
})
export class UploadphotoComponent implements OnInit,OnDestroy {
  @Input() targetUrl: string;
  @Output() getMemberPhotoChange = new EventEmitter<string>();
  token = '';
  uploader: FileUploader;

  constructor(private alertify: ToastrService, private account:AccountService) {}
  
  ngOnDestroy(): void {
    this.targetUrl = "";
  }
  ngOnInit() {
    this.account.currentUser$.subscribe((next) => { this.token = next.Token });
    this.initializeUploader();
  }
  

  initializeUploader() {
     this.uploader = new FileUploader({
         // url: this.targetUrl,
          authToken: 'Bearer ' + this.token,
          isHTML5: true,
          allowedFileType: ['image','application/pdf'],
          removeAfterUpload: true,
          autoUpload: true,
          maxFileSize: 10 * 1024 * 1024
      });

      this.uploader.onAfterAddingFile = file => {
          file.url = this.targetUrl;
          file.withCredentials = false;
          console.log(file);
          this.alertify.success('Photo uploaded ...');
      };

      const parseResponse = (response) => {
        const res = JSON.parse(response);
        const image = res.image || res.PhotoUrl || res.Image || res.ImageUrl;
        if (image) {
          this.getMemberPhotoChange.emit(image);
        }
      };
      
      this.uploader.onSuccessItem = (item, response, status, headers) => {
        if (response) {
          debugger;
           parseResponse(response);
        }
      };

     
  }
}

