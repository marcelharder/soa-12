import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { previewReport } from '../_models/previewReport';

@Injectable()
export class FinalReportService {

    baseUrl = environment.apiUrl;
    constructor(private http: HttpClient) { }

    postReportId(preview: previewReport) { return this.http.post<previewReport>(this.baseUrl + 'PreviewReport', preview); }

    getPdf(id: number, soort: number) { return this.http.get<any>(this.baseUrl + 'getPDF/'+ id + '/' + soort); }


}
