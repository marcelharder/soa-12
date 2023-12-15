import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { JwtHelperService, JWT_OPTIONS } from '@auth0/angular-jwt';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule} from '@angular/platform-browser/animations';

import { NavComponent } from './nav/nav.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { BsDropdownModule} from 'ngx-bootstrap/dropdown';
import { PaginationModule} from 'ngx-bootstrap/pagination';
import { BsDatepickerModule} from 'ngx-bootstrap/datepicker';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { UiSwitchModule } from 'ngx-ui-switch';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { FileUploadModule } from 'ng2-file-upload';

import { HomeComponent } from './home/home.component';
import { ProceduredetailsComponent } from './procedures/proceduredetails/proceduredetails.component';
import { DetailsmainComponent } from './procedures/detailsmain/detailsmain.component';
import { EuroscoredetailsComponent } from './procedures/euroscoredetails/euroscoredetails.component'
import { ToastrModule } from 'ngx-toastr';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { StatisticsComponent } from './statistics/statistics.component';
import { UserlistComponent } from './users/userlist/userlist.component';
import { AboutComponent } from './about/about.component';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { ProcedureMainComponent } from './procedures/procedurelist/procedure-main.component';
import { ProcedureListResolver } from './_resolvers/procedure-list.resolver';
import { ConfigurationComponent } from './configuration/configuration.component';
import { AddEuroScoreDetailsComponent } from './procedures/addprocedure/add-euro-score-details/add-euro-score-details.component';
import { AddprocedureComponent } from './procedures/addprocedure/addprocedure.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { UserProfileComponent } from './users/userprofile/userprofile.component';
import { WorkedInComponent } from './users/userprofile/worked-in/worked-in.component';
import { CareerComponent } from './users/userprofile/career/career.component';
import { PhotoEditorComponent } from './photo-editor/photo-editor.component';
import { ProfileResolver } from './_resolvers/profile.resolver';
import { UserService } from './_services/user.service';
import { RefPhysService } from './_services/refPhys.service';
import { SelectProcedureTypeComponent } from './procedures/addprocedure/select-procedure-type/select-procedure-type.component';
import { CpbComponent } from './procedures/cpb/cpb.component';
import { CabgComponent } from './procedures/cabg/cabg.component';
import { LtxComponent } from './procedures/ltx/ltx.component';
import { AorticComponent } from './procedures/aortic/aortic.component';
import { ValveComponent } from './procedures/valve/valve.component';
import { PostopComponent } from './procedures/postop/postop.component';
import { MininvComponent } from './procedures/mininv/mininv.component';
import { PreviewreportComponent } from './procedures/previewreport/previewreport.component';
import { ExistingconduitComponent } from './procedures/aortic/existingconduit/existingconduit.component';
import { GamecardComponent } from './procedures/aortic/gamecard/gamecard.component';
import { Blok1Component } from './procedures/previewreport/blok1/blok1.component';
import { Blok2Component } from './procedures/previewreport/blok2/blok2.component';
import { Blok3Component } from './procedures/previewreport/blok3/blok3.component';
import { Blok6Component } from './procedures/previewreport/blok6/blok6.component';
import { BlokcabgComponent } from './procedures/previewreport/blokcabg/blokcabg.component';
import { BlokvalveComponent } from './procedures/previewreport/blokvalve/blokvalve.component';
import { EmailRefPhysComponent } from './procedures/previewreport/email-ref-phys/email-ref-phys.component';
import { ReportHeaderComponent } from './procedures/previewreport/report-header/report-header.component';
import { AgeComponent } from './statistics/age/age.component';
import { CasemixComponent } from './statistics/casemix/casemix.component';
import { FiveriskbandsComponent } from './statistics/fiveriskbands/fiveriskbands.component';
import { PermonthComponent } from './statistics/permonth/permonth.component';
import { PeryearComponent } from './statistics/peryear/peryear.component';
import { VladComponent } from './statistics/vlad/vlad.component';
import { EmployeesComponent } from './configuration/employees/employees.component';
import { HospitalsComponent } from './configuration/hospitals/hospitals.component';
import { OpreportComponent } from './configuration/opreport/opreport.component';
import { RefphysComponent } from './configuration/refphys/refphys.component';
import { EditComponent } from './configuration/employees/edit/edit.component';
import { EditdetailsComponent } from './configuration/employees/editdetails/editdetails.component';
import { UploadphotoComponent } from './configuration/employees/uploadphoto/uploadphoto.component';
import { GoogleChartsModule } from 'angular-google-charts';
import { ValvedetailsComponent } from './procedures/valve/valvedetails/valvedetails.component';
import { ValvesinoviComponent } from './procedures/valve/valvesinovi/valvesinovi.component';
import { GraphService } from './_services/graph.service';
import { ProcedureDetailsResolver } from './_resolvers/procedure-details.resolver';
import { ChangesProcedureDetails } from './_guards/changes-procedureDetails.guard';
import { EuroScoreDetailsResolver } from './_resolvers/euroScoreDetails.resolver';
import { CPBDetailsResolver } from './_resolvers/CPB-details.resolver';
import { AorticSurgeryResolver } from './_resolvers/aorticSurgery.resolver';
import { CabgResolver } from './_resolvers/CABG-details.resolver';
import { HospitalResolver } from './_resolvers/Hospital.resolver';
import { ValveResolver } from './_resolvers/Valve.resolver';
import { ValveRepairResolver } from './_resolvers/ValveRepair.resolver';
import { PostResolver } from './_resolvers/PostOp-details.resolver';
import { PreviewReportResolver } from './_resolvers/PreviewReport.resolver';
import { UserDetailsResolver } from './_resolvers/user-details.resolver';
import { UserListResolver } from './_resolvers/user-list.resolver';
import { AiosListResolver } from './_resolvers/aios-list.resolver';
import { AioProcedureResolver } from './_resolvers/aioProcedure.resolver';
import { AioCourseResolver } from './_resolvers/aioCourse.resolver';
import { AioEpaResolver } from './_resolvers/aioEpa.resolver';
import { HospitalListResolver } from './_resolvers/hospitalList.resolver';
import { RefPyhsResolver } from './_resolvers/refPhys.resolver';
import { MinInvResolver } from './_resolvers/MinInv.resolver';
import { LtxResolver } from './_resolvers/Ltx.resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { changesEuroscoreDetails } from './_guards/changes-euroscoreDetails.guard';
import { changesCPBDetails } from './_guards/changes-cpbDetails.guard';
import { changesCABGDetails } from './_guards/changes-cabgDetails.guard';
import { changesPOSTOPDetails } from './_guards/changes-postopDetails.guard';
import { changesPreViewReport } from './_guards/changes-previewReport.guard';
import { changesValveDetails } from './_guards/changes-valveDetails.guard';
import { changesValveRepairDetails } from './_guards/changes-valveRepair.guard';
import { changesMinInv } from './_guards/changes-minInvDetails.guard';
import { changesHospital } from './_guards/changes-Hospital.guard';
import { changesLtxDetails } from './_guards/changes-ltxDetails.guard';
import { changesAorticDetails } from './_guards/changes-aorticDetails.guard';
import { changesDischarge } from './_guards/changes-Discharge.guard';
import { DischargeComponent } from './discharge/discharge.component';
import { dischargeDetailsResolver } from './_resolvers/discharge.resolver';
import { PreViewReportService } from './_services/pre-view-report.service';
import { FinalReportService } from './_services/final-report.service';
import { DischargeService } from './_services/discharge.service';
import { PatientService } from './_services/patient.service';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { DropdownService } from './_services/dropdown.service';
import { AdminGuard } from './_guards/admin.guard';
import { AuthGuard } from './_guards/auth.guard';
import { HasRoleDirective } from './_directives/has-role.directive';
import { AppUserManagementComponent } from './admin/app-user-management/app-user-management.component';
import { RolesModalComponent } from './_modals/roles-modal/roles-modal.component';
import { OnlineUsersComponent } from './users/online-users/online-users.component';
import { UserdetailsComponent } from './users/userdetails/userdetails.component';
import { OnlineUserResolver } from './_resolvers/OnlineUser.resolver ';
import { AddUserComponent } from './users/add-user/add-user.component';
import { AddhospitalComponent } from './admin/addhospital/addhospital.component';
import { EdithospitalComponent } from './admin/edithospital/edithospital.component';
import { ListhospitalsComponent } from './admin/listhospitals/listhospitals.component';
import { RegisterComponent } from './register/register.component';
import { PremiumComponent } from './premium/premium.component';
import { AccordionModule } from 'ngx-bootstrap/accordion';
import { TutorialsComponent } from './tutorials/tutorials.component';
import { DocumentationComponent } from './documentation/documentation.component';
import { TraineesComponent } from './trainees/trainees.component';
import { TraineeListResolver } from './_resolvers/trainee-list.resolver';
import { ChefGuard } from './_guards/chef.guard';
import { TraineeCardComponent } from './trainees/traineeCard/traineeCard.component';
import { TraineeDetailsComponent } from './trainees/traineeDetails/traineeDetails.component';
import { TextInputComponent } from './_forms/text-input/text-input.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { SelectValveComponent } from './procedures/valve/selectValve/selectValve.component';
import { ValvePresentComponent } from './procedures/valve/valve-present/valve-present.component';
import { AddValveComponent } from './procedures/valve/add-valve/add-valve.component';
import { ValveRepairComponent } from './procedures/valveRepair/valveRepair.component';
import { AddValveRepairComponent } from './procedures/valveRepair/addValveRepair/addValveRepair.component';
import { ValveTypeFromHospitalComponent } from './procedures/valveRepair/valveTypeFromHospital/valveTypeFromHospital.component';
import { RepairExistsComponent } from './procedures/valveRepair/repairExists/repairExists.component';
import { EditRepairComponent } from './procedures/valveRepair/editRepair/editRepair.component';
import { ValvesFromOVIComponent } from './procedures/valveRepair/valvesFromOVI/valvesFromOVI.component';
import { OnBlurDirective } from './_directives/OnBlur.directive';
import { HardresetPwdComponent } from './users/hardresetPwd/hardresetPwd.component';
import { InstitutionalReportComponent } from './admin/InstitutionalReport/InstitutionalReport.component';
import { EpaComponent } from './training/epa/epa.component';
import { CmeComponent } from './training/cme/cme.component';
import { DocumentsComponent } from './training/documents/documents.component';
import { TrainingComponent } from './training/training/training.component';
import { CoursesComponent } from './training/courses/courses.component';
import { EpadetailsComponent } from './training/epa/epadetails/epadetails.component';
import { DocumentService } from './_services/document.service';
import { PdfviewerComponent } from './pdfviewer/pdfviewer.component';
import { PdfViewerModule } from 'ng2-pdf-viewer';
import { TrainingService } from './_services/training.service';
import { AddValveTypeComponent } from './configuration/hospitals/add-valveType/add-valveType.component';

@NgModule({
  declarations: [					
    AppComponent,
    NavComponent,
    HomeComponent,
    ProceduredetailsComponent,
    DetailsmainComponent,
    EuroscoredetailsComponent,
    TestErrorsComponent,
    ProcedureMainComponent,
    StatisticsComponent,
    UserlistComponent,
    AboutComponent,
    NotFoundComponent,
    ServerErrorComponent,
    ConfigurationComponent,
    AddEuroScoreDetailsComponent,
    SelectProcedureTypeComponent,
    AddprocedureComponent,
    UserProfileComponent,
    WorkedInComponent,
    CareerComponent,
    PhotoEditorComponent,
    CpbComponent,
    CabgComponent,
    LtxComponent,
    AorticComponent,
    ValveComponent,
    ValveRepairComponent,
    PostopComponent,
    MininvComponent,
    PreviewreportComponent,
    ExistingconduitComponent,
    GamecardComponent,
    Blok1Component,
    Blok2Component,
    Blok3Component,
    Blok6Component,
    BlokcabgComponent,
    BlokvalveComponent,
    EmailRefPhysComponent,
    ReportHeaderComponent,
    AgeComponent,
    CasemixComponent,
    FiveriskbandsComponent,
    PermonthComponent,
    PeryearComponent,
    VladComponent,
    EmployeesComponent,
    HospitalsComponent,
    OpreportComponent,
    RefphysComponent,
    EditComponent,
    EditdetailsComponent,
    UploadphotoComponent,
    ValvedetailsComponent,
    ValvesinoviComponent,
    DischargeComponent,
    HasRoleDirective,
    OnBlurDirective,
    AppUserManagementComponent,
    RolesModalComponent,
    OnlineUsersComponent,
    UserdetailsComponent,
    AddUserComponent,
    AddhospitalComponent,
    EdithospitalComponent,
    ListhospitalsComponent,
    RegisterComponent,
    PremiumComponent,
    TutorialsComponent,
    DocumentationComponent,
    TraineesComponent,
    TraineeCardComponent,
    TraineeDetailsComponent,
    TextInputComponent,
    ResetPasswordComponent,
    ForgotPasswordComponent,
    SelectValveComponent,
    ValvePresentComponent,
    AddValveComponent,
    AddValveRepairComponent,
    ValveTypeFromHospitalComponent,
    RepairExistsComponent,
    EditRepairComponent,
    ValvesFromOVIComponent,
    HardresetPwdComponent,
    InstitutionalReportComponent,
    EpaComponent,
    EpadetailsComponent,
    CmeComponent,
    DocumentsComponent,
    TrainingComponent,
    CoursesComponent,
    PdfviewerComponent,
    AddValveTypeComponent
   ],
  imports: [
    FileUploadModule,
    PdfViewerModule,
    TabsModule.forRoot(),
    UiSwitchModule,
    ButtonsModule,
    PaginationModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    AccordionModule.forRoot(),
    FormsModule,
    ReactiveFormsModule,
    BsDropdownModule.forRoot(),
    BsDatepickerModule.forRoot(),
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    }),
    ModalModule.forRoot(),
    GoogleChartsModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  providers: [
   {provide: JWT_OPTIONS, useValue: JWT_OPTIONS},
   {provide: LocationStrategy, useClass: HashLocationStrategy},
   RefPhysService,
   UserService,
   DropdownService,
   PreViewReportService,
   PatientService,
   FinalReportService,
   GraphService,
   DischargeService,
   JwtHelperService, 
   DocumentService,
   TrainingService,
  //  resolvers
   ProcedureListResolver,
   ProcedureDetailsResolver,
   EuroScoreDetailsResolver,
   CPBDetailsResolver,
   AorticSurgeryResolver,
   CabgResolver,
   HospitalResolver,
   ValveResolver,
   ValveRepairResolver,
   PostResolver,
   PreviewReportResolver,
   UserDetailsResolver,
   UserListResolver,
   TraineeListResolver,
   AiosListResolver,
   AioProcedureResolver,
   AioCourseResolver,
   AioEpaResolver,
   HospitalListResolver,
   ProfileResolver,
   RefPyhsResolver,
   MinInvResolver,
   dischargeDetailsResolver,
   LtxResolver,
   OnlineUserResolver,
   {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
   {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
   // guards
   AdminGuard,
   ChefGuard,
   AuthGuard,
   PreventUnsavedChanges,
   ChangesProcedureDetails,
   changesEuroscoreDetails,
   changesCPBDetails,
   changesCABGDetails,
   changesPOSTOPDetails,
   changesPreViewReport,
   changesValveDetails,
   changesValveRepairDetails,
   changesMinInv,
   changesHospital,
   changesLtxDetails,
   changesAorticDetails,
   changesDischarge
  ],
  bootstrap: [AppComponent]
  
})
export class AppModule { }
