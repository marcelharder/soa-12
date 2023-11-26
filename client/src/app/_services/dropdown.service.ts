import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, JsonpClientBackend } from '@angular/common/http';
import { dropItem } from '../_models/dropItem';
import { environment } from '../../environments/environment';
import { countryItem } from '../_models/countryItem';

@Injectable({
    providedIn: 'root'
})
export class DropdownService {

    baseUrl = environment.apiUrl;
    constructor(private http: HttpClient) { }


    getEmployees(hospital_id: string, job_id: string, activeState: string, liscense_to_kill: string){
        let params = new HttpParams();

          params = params.append('hospital_id', hospital_id);
          params = params.append('job_id', job_id);
          params = params.append('activeState', activeState);
          params = params.append('liscense_to_kill', liscense_to_kill);

        return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/dropEmployee',{ params });
    }
    isProcedureComplete(patientId: number){return this.http.get<string>(this.baseUrl + 'General/isComplete/'+ patientId.toString(),{ responseType: 'text' as 'json' }); }


    //#region <!-- cpb -->
    getVenousCanulation() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/ven_canulation_options'); }
    getArterialCanulation() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/art_canulation_options'); }
    getOcclusionMethod() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/occl_method'); }
    getMyoTech() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/myotech'); }
    getMPT() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/mpt'); }
    getCardioplegTiming() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/cardioplegtiming'); }
    getCardioplegTemp() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/cardioplegtemp'); }
    getDelivery() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/cardioplegdelivery'); }
    getIabpTiming() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/iabp_timing');  }
    getIabpIndication() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/iabp_ind');  }
    getTypeCardioplegia() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/typeCardiopleg'); }
    //#endregion
    //#region <!--  -->

    getGenderOptions() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/gender_options'); }
    getWeightOptions() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/weight_options'); }
    getHeightOptions() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/height_options'); }
    getAgeOptions() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/age_options'); }
    getCreatOptions() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/creat_options'); }

    getTimingOptions() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/timing_options'); }
    getYNOptions() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/optionsYN'); }
    getHours() { return this.http.get<string[]>(this.baseUrl + 'DropDown/dropHours'); }
    getMins() { return this.http.get<string[]>(this.baseUrl + 'DropDown/dropMins'); }
    getUrgentOptions() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/urgent_options'); }
    getEmergentOptions() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/emergent_options'); }
    getInotropeOptions() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/inotrope_options'); }
    getPacemakerOptions() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/pacemaker_options'); }
    getPericardOptions() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/pericard_options'); }
    getPleuraOptions() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/pleura_options'); }

 //#endregion
    //#region <!-- cabg -->
    getCabgQuality() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/cabg_quality'); }
    getCabgDiameter() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/cabg_diameter'); }
    getCabgType() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/cabg_type'); }
    getCabgLocatie() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/cabg_locatie'); }
    getCabgProximal() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/cabg_proximal'); }
    getCabgConduit() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/cabg_conduit'); }
    getCabgProcedure() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/cabg_procedure'); }
    getCabgAngle() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/cabg_angle'); }
    getCabgDroplist1() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/cabg_dropList1'); }
    getCabgRadial() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/cabg_radial'); }
    getCabgLeg() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/cabg_leg'); }
    //#endregion

    getCategory_1_Options() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/addProcedureCategory/' + 1); }
    getCategory_2_Options() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/addProcedureCategory/' + 2); }
    getCategory_3_Options() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/addProcedureCategory/' + 3); }
    getCategory_4_Options() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/addProcedureCategory/' + 4); }
    getCategory_5_Options() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/addProcedureCategory/' + 5); }
    getCategory_6_Options() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/addProcedureCategory/' + 6); }
    getCategory_7_Options() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/addProcedureCategory/' + 7); }
    getCategory_8_Options() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/addProcedureCategory/' + 8); }
    getCategory_9_Options() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/addProcedureCategory/' + 9); }

    getEuroScore_Options(patientId: number) { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/euroScoreOptions/' + patientId); }
  
    // region <!--hospitals -->
    getAvailableHospitals(countryId: string) {return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/availableHospitalOptions/' + countryId);}
    getAllHospitals() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/allHospitals'); }
    getHospitals(userId: number) {return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/hospitalOptions/' + userId);}
   
     //#region <!-- cpb -->
    getMyocardialPreservationTechnique() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/myocardialPreservationTechnique'); }
    getAC() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/art_choice'); }
    getVC() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/ven_choice'); }
    getIABPI() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/iabp_ind'); }
    getIABPT() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/iabp_timing'); }
    getNCCP() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/cpb_nccp'); }
    getAOX() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/cpb_aox'); }
    getCPB_TIMING() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/cpb_timing'); }
    getCPB_TEMP() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/cpb_temp'); }
    getCPB_delivery(i) { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/cpb_delivery'); }
    getIABP_WHEN() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/iabp_when'); }
    getIABP_WHY() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/iabp_why'); }
    //#endregion

    //#region <!-- valve -->
    getAorticProcedure() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/aorticProcedure'); }
    getMitralProcedure() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/mitralProcedure'); }
    getPulmonaryProcedure() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/pulmonaryProcedure'); }
    getTricuspidProcedure() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/tricuspidProcedure'); }
    getAetiology() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/aetiology'); }
    getMitralValveRepair() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/mitralValveRepair'); }
    getTricuspidValveRepair() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/tricuspidValveRepair'); }
    getMitralRingType() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/mitralRingType'); }
    getTricuspidRingType() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/tricuspidRingType'); }
    getMitralRingsInHospital() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/mitralRingTypeInHospital'); }
    getTricuspidRingsInHospital() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/tricuspidRingTypeInHospital'); }
    getValveType() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/valveType'); }
    getImplantPosition() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/implantPosition'); }
    //#endregion

    getWoIntOptions() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/eu_weight_intervention'); }
    getPH() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/eu_pulmonary_hypertension'); }
    getNYHA() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/eu_NYHA'); }
    getLVFunction() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/eu_lv_function'); }

    //#region <!-- postop -->
    getAutoBloodTiming() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/AutoBloodTiming'); }
    getComp_options_01() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/complicationOptions01'); }
    getComp_options_02() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/complicationOptions02'); }
    getComp_options_03() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/complicationOptions03'); }
    getComp_options_04() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/complicationOptions04'); }
    getComp_options_05() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/complicationOptions05'); }
    getComp_options_06() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/complicationOptions06'); }
    getComp_options_07() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/complicationOptions07'); }
    getComp_options_08() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/complicationOptions08'); }
    getComp_options_09() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/complicationOptions09'); }
    //#endregion

    //#region <!-- discharge -->
    getActivitiesDischarge() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/discharge_activities'); }
    getDischargeDiagnosis() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/discharge_diagnosis'); }
    getDischargeDirection() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/discharge_direction'); }
    getMortalityLocation() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/dead_location'); }
    getMortalityCause() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/dead_cause'); }
    getMortality() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/dead'); }
    //#endregion

    //#region <!-- user edit -->
    getAllCountries() { return this.http.get<countryItem[]>(this.baseUrl + 'DropDown/countriesDrops'); }
    getAllCities() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/citiesDrops'); }
    getRoles() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/roles'); }
    getCareerTopics(){return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/career');}
    // tslint:disable-next-line: max-line-length
    //#endregion

     //#region <!-- aneurysm -->
    getAneurysmType() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/aneurysmType'); }
    getDissectionOnset() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/dissectionOnset'); }
    getDissectionType() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/dissectionType'); }
    getPathology() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/pathology'); }
    getOpIndication() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/opIndication'); }
    getOpTechnique() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/optechnique'); }
    getRangeReplacement() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/rangeReplacement'); }
    //#endregion

    //#region <!-- ltx -->
    getLtxIndication() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/dropLtxIndication'); }
    getLtxType() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/dropLtxType'); }

     //endregion

    getConversionDetails() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/conversionDetails'); }
    getStrategy() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/strategy'); }
    getPrimary_incision() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/primaryIncision'); }
    getFollow1() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/follow1'); }
    getFollow2() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/follow2'); }
    getLimaHarvest() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/limaHarvest'); }
    getStabilization() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/stabilization'); }
    getSuture_technique() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/sutureTechnique'); }
    getFollow3() { return this.http.get<dropItem[]>(this.baseUrl + 'DropDown/follow3'); }

    

}
