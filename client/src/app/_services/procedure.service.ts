import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { Procedure } from '../_models/Procedure';
import { map } from 'rxjs/operators';
import { ProcedureDetails } from '../_models/procedureDetails';
import { CandA } from '../_models/CandA';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ProcedureService {
    baseUrl = environment.apiUrl;

    constructor(private http: HttpClient) { }



    getProcedures(page?, itemsPerPage?): Observable<PaginatedResult<Procedure[]>> {
        const paginatedResult: PaginatedResult<Procedure[]> = new PaginatedResult<Procedure[]>();
        let params = new HttpParams();

        if (page != null && itemsPerPage != null) {
            params = params.append('pageNumber', page);
            params = params.append('pageSize', itemsPerPage);
        }

        return this.http.get<Procedure[]>(this.baseUrl + 'procedure', { observe: 'response', params })
            .pipe(
                map(response => {
                    paginatedResult.result = response.body;
                    if (response.headers.get('Pagination') != null) {
                        paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
                    }
                    return paginatedResult;
                })
            );

    }
    getAioProcedures(id: string,page?, itemsPerPage?): Observable<PaginatedResult<Procedure[]>> {
        const paginatedResult: PaginatedResult<Procedure[]> = new PaginatedResult<Procedure[]>();
        let params = new HttpParams();

        if (page != null && itemsPerPage != null) {
            params = params.append('pageNumber', page);
            params = params.append('aioId', id);
            params = params.append('pageSize', itemsPerPage);
        }

        return this.http.get<Procedure[]>(this.baseUrl + 'procedure/aioProcedures', { observe: 'response', params })
            .pipe(
                map(response => {
                    paginatedResult.result = response.body;
                    if (response.headers.get('Pagination') != null) {
                        paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
                    }
                    return paginatedResult;
                })
            );

    }
    getAssistedProcedures(page?, itemsPerPage?): Observable<PaginatedResult<Procedure[]>> {
        const paginatedResult: PaginatedResult<Procedure[]> = new PaginatedResult<Procedure[]>();
        let params = new HttpParams();

        if (page != null && itemsPerPage != null) {
            params = params.append('pageNumber', page);
            params = params.append('pageSize', itemsPerPage);
        }

        return this.http.get<Procedure[]>(this.baseUrl + 'procedure/assistedProcedures', { observe: 'response', params })
            .pipe(
                map(response => {
                    paginatedResult.result = response.body;
                    if (response.headers.get('Pagination') != null) {
                        paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
                    }
                    return paginatedResult;
                })
            );

    }



    getButtonsAndCaptions(id: number): Observable<CandA> {
        return this.http.get<CandA>(this.baseUrl + 'procedure/loadButtonCapAndActions/' + id); }

    addProcedure(procedureDetails: ProcedureDetails, id: number, patientId: number): Observable<ProcedureDetails> {
        return this.http.post<ProcedureDetails>(this.baseUrl + 'procedure/' + id + '/' + patientId, procedureDetails);
    }

    getReportCode(procedureSoort: number): Observable<number> {
        return this.http.get<number>(this.baseUrl + 'loadReportCode/' + procedureSoort); }

    saveProcedureDetails(id: number, p: ProcedureDetails) { return this.http.put<number>(this.baseUrl + 'procedure/' + id, p); }

    getProcedure(id: number): Observable<ProcedureDetails> { return this.http.get<ProcedureDetails>(this.baseUrl + 'procedure/' + id); }

    deleteProcedure(id: number) { return this.http.delete<number>(this.baseUrl + 'procedure/' + id); }

    getRefPhysEmailHash(id: number){ return this.http.get<string>(
        this.baseUrl + 'procedure/refPhysEmailHash/' + id,{ responseType: 'text' as 'json' }); }

}
