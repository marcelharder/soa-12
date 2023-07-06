using System;
namespace api.Entities
{
    public class Class_CPB
    {
        public int Id { get; set; }
        public int PROCEDURE_ID { get; set; }
        public int CROSS_CLAMP_TIME { get; set; }
        public int PERFUSION_TIME { get; set; }
        public Nullable<int> LOWEST_CORE_TEMP { get; set; }
        public string CARDIOPLEGIA { get; set; }
        public string CARDIOPLEGIA_TYPE { get; set; }
        public string INFUSION_MODE_ANTE { get; set; }
        public Nullable<int> INFUSION_MODE_RETRO { get; set; }
        public Nullable<int> INFUSION_DOSE_INT { get; set; }
        public Nullable<int> INFUSION_DOSE_CONT { get; set; }
        public Nullable<int> CARDIOPLEGIA_TEMP_WARM { get; set; }
        public Nullable<int> CARDIOPLEGIA_TEMP_COLD { get; set; }
        public string IABP { get; set; }
        public string IABP_OPTIONS { get; set; }
        public string IABP_IND { get; set; }
        public Nullable<int> PACING_HARV { get; set; }
        public Nullable<int> PACING_ATRIAL { get; set; }
        public Nullable<int> PACING_VENTRICULAR { get; set; }
        public Nullable<int> CARDIOVERSION { get; set; }
        public string VAD { get; set; }
        public Nullable<int> LVAD { get; set; }
        public Nullable<int> RVAD { get; set; }
        public string BVAD { get; set; }
        public string TAH { get; set; }
        public Nullable<int> INOTROPES { get; set; }
        public Nullable<int> Antiarrhythmics { get; set; }
        public Nullable<int> SKIN_INCISION_START_TIME { get; set; }
        public Nullable<int> SKIN_INCISION_STOP_TIME { get; set; }
        public string opcab_attempt { get; set; }
        public string cpb_used { get; set; }
        public string a1 { get; set; }
        public string a2 { get; set; }
        public string a3 { get; set; }
        public string a4 { get; set; }
        public string v1 { get; set; }
        public string v2 { get; set; }
        public string v3 { get; set; }
        public string v4 { get; set; }
        public string aoOCCL { get; set; }
        public Nullable<int> long_isch { get; set; }
        public string cardiopl_timing { get; set; }
        public string cardiopl_temp { get; set; }
        public string cns_protect { get; set; }
        public Nullable<int> cns_time_1 { get; set; }
        public Nullable<int> cns_time_2 { get; set; }
        public Nullable<int> cns_time_3 { get; set; }
        public string deep_hypo { get; set; }
        public string deep_hypo_rcp { get; set; }
        public string acp_circ { get; set; }
        public string other_cns_protect { get; set; }
        public string nonCMProtect { get; set; }
        public Nullable<short> nonCMProtect_type { get; set; }
        public Nullable<System.DateTime> IABP_DATE { get; set; }
        public string myoplasty { get; set; }
        public Nullable<int> cpb_start_hr { get; set; }
        public Nullable<int> cpb_start_min { get; set; }
        public Nullable<int> cpb_stop_hr { get; set; }
        public Nullable<int> cpb_stop_min { get; set; }
        public Nullable<int> clamp_start_hr { get; set; }
        public Nullable<int> clamp_start_min { get; set; }
        public Nullable<int> clamp_stop_hr { get; set; }
        public Nullable<int> clamp_stop_min { get; set; }
        public string other_cardiac_support { get; set; }
        public string cardiac_support { get; set; }
    }
}