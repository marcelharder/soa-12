import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { environment } from '../../environments/environment';
import { AccountService } from '../_services/account.service';

@Component({
    selector: 'app-photo-editor',
    templateUrl: './photo-editor.component.html',
    styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
    @Input() userId: number;
    @Input() refId: number;
    @Input() hospitalId: number;
    @Input() documentId: number;
    @Output() getMemberPhotoChange = new EventEmitter<string>();
    uploader: FileUploader;
    token ='';
    hasBaseDropZoneOver = false;
    baseUrl = environment.apiUrl;

    constructor(
        private alertify: ToastrService,
        private account: AccountService
    ) { }

    ngOnInit() {
    this.account.currentUser$.subscribe((next)=>{this.token = next.Token});
     this.initializeUploader();
    }

    initializeUploader() {
        let test = '';

       if (this.userId !== 0) {
            test = this.baseUrl + 'users/addUserPhoto/' + this.userId
        }
        else {
            if (this.hospitalId !== 0) {
                test = this.baseUrl + 'hospital/addHospitalPhoto/' + this.hospitalId
            }
            else {
                if (this.refId !== 0) {
                    test = this.baseUrl + 'RefPhys/addPhoto/' + this.refId
                }
                else {
                    if (this.documentId !== 0) {
                        test = this.baseUrl + 'training/addPhoto/' + this.documentId
                    } 
                }
            }

        }

        this.uploader = new FileUploader({
            url: test,
            authToken: 'Bearer ' + this.token,
            isHTML5: true,
            allowedFileType: ['image'],
            removeAfterUpload: true,
            autoUpload: true,
            maxFileSize: 10 * 1024 * 1024
        });

        this.uploader.onAfterAddingFile = file => {
            file.withCredentials = false;
            console.log(file);
            this.alertify.success('Photo uploaded ...');
        };

        this.uploader.onSuccessItem = (item, response, status, headers) => {
            if (response) {
                const res: any = JSON.parse(response);

                if (this.hospitalId !== 0) { this.getMemberPhotoChange.emit(res.ImageUrl); }
                if (this.userId !== 0) { this.getMemberPhotoChange.emit(res.PhotoUrl); }
                if (this.refId !== 0) { this.getMemberPhotoChange.emit(res.image); }
                if (this.documentId !== 0) { this.getMemberPhotoChange.emit(res.document_url); }
            }
        };
    }
}
