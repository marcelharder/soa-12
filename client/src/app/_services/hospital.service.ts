import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { dropItem } from '../_models/dropItem';
import { environment } from '../../environments/environment';
import { Hospital } from '../_models/Hospital';
import { Observable } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from './account.service';
import { previewReport } from '../_models/previewReport';
import { additionalReportModel } from '../_models/InstitutionalReportModels/additionalReportModel';
import { mainTextModel } from '../_models/InstitutionalReportModels/mainTextModel';

@Injectable({
  providedIn: 'root'
})
export class HospitalService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient,
    private alertify: ToastrService,
    private auth: AccountService) { }



  getSpecificHospital(id: number) { return this.http.get<Hospital>(this.baseUrl + 'hospital/' + id); }

  getSpecificHospitalFromInventory(id: number){return this.http.get<Hospital>(this.baseUrl + 'hospital/hospitalFromInventory/' + id);}
  
  saveHospital(item: Hospital) { return this.http.put<Hospital>(this.baseUrl + 'hospital', item); }
  IsThisHospitalUsingOVI(id: number) { return this.http.get<boolean>(this.baseUrl + 'hospital/IsThisHospitalImplementingOVI/' + id) }
  getListOfCities(id: string) { return this.http.get<dropItem[]>(this.baseUrl + 'citiesPerCountry/' + id); }
  getAllThisUserWorkedInHospitals(userId: number) { return this.http.get<Hospital[]>(this.baseUrl + 'hospital_worked_in/' + userId); }

  allHospitals(): Observable<Hospital[]> { return this.http.get<Hospital[]>(this.baseUrl + 'Hospital/allFullHospitals'); }
  getHospitalsInCountry(id: string): Observable<Hospital[]> { return this.http.get<Hospital[]>(this.baseUrl + 'Hospital/allFullHospitalsPerCountry/' + id); }
 
  addHospital(country: string, no: number) { return this.http.post<Hospital>(this.baseUrl + 'hospital/' + country + '/' + no, null); }
  deleteHospital(id: string) { return this.http.delete<number>(this.baseUrl + 'hospital/' + id); }

 
  updateAdditionalReports(id: number,which: number, model: Partial<additionalReportModel>)
  {return this.http.put<string>(this.baseUrl + 'hospital/UpdateAdditionalReportItems/' + id + '/' + which, model);}
  
  getAdditionalInstitutionalReport(id: number, which: number)
  {return this.http.get<additionalReportModel>(this.baseUrl + 'hospital/AdditionalReportItems/' + id + '/' + which);}
 
  
  
  
  updateInstitutionalReport(id: number,soort: number, model: Partial<mainTextModel>)
  {return this.http.put<string>(this.baseUrl + 'hospital/InstitutionalReport/' + id + '/' + soort, model);}

  getInstitutionalReport(id: number, soort: number)
  {return this.http.get<mainTextModel>(this.baseUrl + 'hospital/InstitutionalReport/' + id + '/' + soort);}

  createInstitutionalReport(id:number)
  {return this.http.post<string>(this.baseUrl + 'hospital/InstitutionalReport/' + id, null);}

 


}
