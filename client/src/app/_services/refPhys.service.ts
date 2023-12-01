import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { RefPhysModel } from '../_models/RefPhysModel';
import { dropItem } from '../_models/dropItem';
import { EmailModel } from '../_models/EmailModel';
import { SMSModel } from '../_models/SMSModel';

@Injectable()
export class RefPhysService {
    baseUrl = environment.apiUrl;
   
    constructor(private http: HttpClient) { }

    getSpecificRefPhys(id: number) { return this.http.get<RefPhysModel>(this.baseUrl + 'RefPhys/specificRefPhys/' + id); }

    updateRefPhys(c: RefPhysModel) { return this.http.put<RefPhysModel>(this.baseUrl + 'RefPhys/updateRefPhys/', c); }

    addRefPhys() { return this.http.get<RefPhysModel>(this.baseUrl + 'RefPhys/addRefPhys'); }

    deleteRefPhys(id: number) { return this.http.delete<number>(this.baseUrl + 'RefPhys/deleteRefPhys/' + id);}

    getRefPhys(id: number) { return this.http.get<dropItem[]>(this.baseUrl + 'RefPhys/AllRefPhys/' + id); }

    sendEmail(e: EmailModel) // Use the config service to send email about the procedure
    {
        return this.http.post<string>(this.baseUrl + 'General/sendProcedureNotification', e,{ responseType: 'text' as 'json' });
    }
    sendSMS(e: SMSModel) // Use the config service to send email
    {
        return this.http.post<string>(this.baseUrl + 'General/sendSMS', e,{ responseType: 'text' as 'json' });
    }


}
