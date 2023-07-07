using System;

namespace api.DTOs
{
    public class CPBForReturnDTO
    {
        public virtual int id { get; set; }
        public virtual int procedure_id { get; set; }
        public virtual int cross_clamp_time { get; set; }
        public virtual int perfusion_time { get; set; }
        public virtual int lowest_core_temp { get; set; }
        public virtual string cardioplegia { get; set; }
        public virtual string cardioplegia_type { get; set; }
        public virtual string group1 { get; set; }
        public virtual string infusion_mode_ante { get; set; }
        public virtual int infusion_mode_retro { get; set; }
        public virtual int infusion_dose_int { get; set; }
        public virtual int infusion_dose_cont { get; set; }
        public virtual string cardioplegia_temp_warm { get; set; }
        public virtual string cardioplegia_temp_cold { get; set; }
        public virtual string iabp { get; set; }
        public virtual string iabp_options { get; set; }
        public virtual string iabp_ind { get; set; }
        public virtual int pacing_harv { get; set; }
        public virtual int pacing_atrial { get; set; }
        public virtual int pacing_ventricular { get; set; }
        public virtual int cardioversion { get; set; }
        public virtual string vad { get; set; }
        public virtual int lvad { get; set; }
        public virtual int rvad { get; set; }
        public virtual string bvad { get; set; }
        public virtual string tah { get; set; }
        public virtual int inotropes { get; set; }
        public virtual int antiarrhythmics { get; set; }
        public virtual int skin_incision_start_time { get; set; }
        public virtual int skin_incision_stop_time { get; set; }
        public virtual string opcab_attempt { get; set; }
        public virtual string cpb_used { get; set; }
        public virtual string a1 { get; set; }
        public virtual string a2 { get; set; }
        public virtual string a3 { get; set; }
        public virtual string a4 { get; set; }
        public virtual string v1 { get; set; }
        public virtual string v2 { get; set; }
        public virtual string v3 { get; set; }
        public virtual string v4 { get; set; }
        public virtual string aooccl { get; set; }
        public virtual int long_isch { get; set; }
        public virtual string cardiopl_timing { get; set; }
        public virtual string cardiopl_temp { get; set; }
        public virtual string cns_protect { get; set; }
        public virtual int cns_time_1 { get; set; }
        public virtual int cns_time_2 { get; set; }
        public virtual int cns_time_3 { get; set; }
        public virtual string deep_hypo { get; set; }
        public virtual string deep_hypo_rcp { get; set; }
        public virtual string acp_circ { get; set; }
        public virtual string other_cns_protect { get; set; }
        public virtual string noncmprotect { get; set; }
        public virtual int noncmprotect_type { get; set; }
        public virtual DateTime iabp_date { get; set; }
        public virtual string myoplasty { get; set; }
        public virtual int cpb_start_hr { get; set; }
        public virtual int cpb_start_min { get; set; }
        public virtual int cpb_stop_hr { get; set; }
        public virtual int cpb_stop_min { get; set; }
        public virtual int clamp_start_hr { get; set; }
        public virtual int clamp_start_min { get; set; }
        public virtual int clamp_stop_hr { get; set; }
        public virtual int clamp_stop_min { get; set; }
        public virtual string other_cardiac_support { get; set; }
        public virtual string cardiac_support { get; set; }
    }
}
