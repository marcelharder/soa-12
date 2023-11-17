import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { previewReport } from '../_models/previewReport';
import { dropItem } from '../_models/dropItem';
import { Suggestion } from '../_models/Suggestion';
import { reportHeader } from '../_models/reportHeader';
import { additionalReportModel } from '../_models/InstitutionalReportModels/additionalReportModel';
import { mainTextModel } from '../_models/InstitutionalReportModels/mainTextModel';

@Injectable()
export class PreViewReportService {

    baseUrl = environment.apiUrl;
    constructor(private http: HttpClient) { }


    isFinalReportPresentable(procedureId:number){return this.http.get<number>(this.baseUrl + 'previewReport/isFinalReportReady/' + procedureId);}

    getReportHeader(procedureId: number) { return this.http.get<reportHeader>(this.baseUrl + 'reportHeader/' + procedureId);}

    getPreView(id: number) { return this.http.get<previewReport>(this.baseUrl + 'previewReport/' + id); }
    
    resetPreView(id: number) { return this.http.get<previewReport>(this.baseUrl + 'previewReport/reset/' + id); }

    savePreView(c: previewReport) { return this.http.post<previewReport>(this.baseUrl + 'previewReport', c); }

    getAllPossibleReports(userId: number) { return this.http.get<dropItem[]>(this.baseUrl + 'getAllIndividualReports/' + userId); }

    getIndividualSuggestions() { return this.http.get<dropItem[]>(this.baseUrl + 'Suggestion'); }

    getSuggestionBySoort(id: number) { return this.http.get<Suggestion>(this.baseUrl + 'Suggestion/' + id); }

    saveSuggestion(sug: Suggestion) { return this.http.put<number>(this.baseUrl + 'Suggestion/personalizedSuggestion', sug); }
    
    savePreViewSuggestion(sug: previewReport) { return this.http.put<number>(this.baseUrl + 'Suggestion', sug); }

    getReportCode(id: number) { return this.http.get<number>(this.baseUrl + 'General/loadReportCode/' + id);}

    


     



}
