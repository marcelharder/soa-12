import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { AddPatient } from '../_models/AddPatient';
import { Patient } from '../_models/Patient';
import { PatientAfterAdding } from '../_models/patientAfterAdding';

@Injectable({
    providedIn: 'root'
})
export class PatientService {
    baseUrl = environment.apiUrl;
    constructor(private http: HttpClient) { }

    getPatientDetails(mrn: string) { return this.http.get<Patient>(this.baseUrl + 'patient/patientFromMRN/' + mrn); }

    getPatientFromId(id: number) { return this.http.get<Patient>(this.baseUrl + 'patient/' + id); }

    getPatientFromProcedureId(id: number) { return this.http.get<Patient>(this.baseUrl + 'patient/patientFromProcedureId/' + id ); }

    getPatientinDatabase(mrn: string) { return this.http.get<string>(this.baseUrl + 'General/patient_in_database/' + mrn); }

    addPatientToDatabase(p: AddPatient, id: number) { return this.http.post<PatientAfterAdding>(this.baseUrl + 'patient/' + id, p); }

    updatePatient(p: Patient, id: number) { return this.http.put<Patient>(this.baseUrl + 'patient/' + id, p); }

    getProceduresFromPatientId(id: number) { return this.http.get<number[]>(this.baseUrl + 'General/proceduresFromPatientId/' + id); }
    
    getProceduresFromMRN(mrn: string) { return this.http.get<number[]>(this.baseUrl + 'General/proceduresFromMRN/' + mrn); }

    operatedElsewhere(patientId: number, userId: number) { return this.http.get<number>(this.baseUrl + 'operatedElseWhere/' + patientId + '/' + userId); }
}
