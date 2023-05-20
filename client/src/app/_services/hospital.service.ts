import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { dropItem } from '../_models/dropItem';
import { environment } from '../../environments/environment';
import { Hospital } from '../_models/Hospital';
import { Observable } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from './account.service';

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
}
