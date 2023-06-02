using api.DTOs;
using api.Entities;
using AutoMapper;

namespace api.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Class_Patient, FullPatientDTO>();
            CreateMap<FullPatientDTO, Class_Patient>()
            .ForMember(dest => dest.PatientId, opt => opt.Ignore())
            .ForMember(dest => dest.extra_cardiac_arteriopathy, opt => opt.NullSubstitute("0"))
            .ForMember(dest => dest.previous_cardiac_surgery, opt => opt.NullSubstitute("0"))
            .ForMember(dest => dest.IsPreviousIntervention, opt => opt.NullSubstitute("0"))
            .ForMember(dest => dest.copd, opt => opt.NullSubstitute("0"))
            .ForMember(dest => dest.active_endocarditis, opt => opt.NullSubstitute("0"))
            .ForMember(dest => dest.CCS, opt => opt.NullSubstitute("0"))
            .ForMember(dest => dest.LVEF, opt => opt.NullSubstitute("0"))
            .ForMember(dest => dest.recent_mi, opt => opt.NullSubstitute("0"))
            .ForMember(dest => dest.NOPM, opt => opt.NullSubstitute("0"))
            .ForMember(dest => dest.surgery_on_thoracic_aorta, opt => opt.NullSubstitute("0"));

            CreateMap<Class_Patient, PatientForReturnDTO>();


            CreateMap<Class_Procedure, ProcedureListDTO>();
            CreateMap<Class_Procedure, ProcedureDTO>();
            CreateMap<ProcedureDTO, Class_Procedure>()
            .ForMember(dest => dest.Sequence, opt => opt.Ignore());



            CreateMap<AppUser, UserForReturnDto>();
            CreateMap<UserForUpdateDto, AppUser>();
           
            CreateMap<Class_Hospital, HospitalForReturnDTO>();
            CreateMap<Class_Valve_Code, valveDTO>();

            CreateMap<HospitalForReturnDTO, Class_Hospital>()
            .ForMember(dest => dest.RegExpr, opt => opt.Ignore())
            .ForMember(dest => dest.SampleMrn, opt => opt.Ignore())
            .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
 
           
            CreateMap<Class_Employee, EmployeeForReturnDTO>();
            CreateMap<EmployeeForUpdateDTO, Class_Employee>().ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Class_LTX, LtxForReturnDTO>();
            CreateMap<LtxForReturnDTO, Class_LTX>().ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Class_Aortic_Surgery, AoSurgeryForReturnDTO>();
            CreateMap<AoSurgeryForReturnDTO, Class_Aortic_Surgery>().ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Class_CPB, CPBForReturnDTO>();
            CreateMap<CPBForReturnDTO, Class_CPB>().ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Class_CABG, CabgDetailsDTO>();
            CreateMap<CabgDetailsDTO, Class_CABG>().ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Class_PostOp, PostOpDetailsDTO>();
            CreateMap<PostOpDetailsDTO, Class_PostOp>().ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Class_Valve, ValveForReturnDTO>();
            CreateMap<ValveForReturnDTO, Class_Valve>().ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<PreviewForReturnDTO, Class_Preview_Operative_report>().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<PreviewForReturnDTO, Class_privacy_model>();

            CreateMap<Class_minInv, MinInvForReturn>();
            CreateMap<MinInvForReturn, Class_minInv>().ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Class_Ref_Phys, refphysForReturn>();
            CreateMap<refphysForUpdate, Class_Ref_Phys>().ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Class_Suggestion, Class_Preview_Operative_report>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.regel_1, opt => opt.MapFrom(src => src.regel_1_a + src.regel_1_b + src.regel_1_c))
            .ForMember(dest => dest.regel_2, opt => opt.MapFrom(src => src.regel_2_a + src.regel_2_b + src.regel_2_c))
            .ForMember(dest => dest.regel_3, opt => opt.MapFrom(src => src.regel_3_a + src.regel_3_b + src.regel_3_c))
            .ForMember(dest => dest.regel_4, opt => opt.MapFrom(src => src.regel_4_a + src.regel_4_b + src.regel_4_c))
            .ForMember(dest => dest.regel_5, opt => opt.MapFrom(src => src.regel_5_a + src.regel_5_b + src.regel_5_c))
            .ForMember(dest => dest.regel_6, opt => opt.MapFrom(src => src.regel_6_a + src.regel_6_b + src.regel_6_c))
            .ForMember(dest => dest.regel_7, opt => opt.MapFrom(src => src.regel_7_a + src.regel_7_b + src.regel_7_c))
            .ForMember(dest => dest.regel_8, opt => opt.MapFrom(src => src.regel_8_a + src.regel_8_b + src.regel_8_c))
            .ForMember(dest => dest.regel_9, opt => opt.MapFrom(src => src.regel_9_a + src.regel_9_b + src.regel_9_c))
            .ForMember(dest => dest.regel_10, opt => opt.MapFrom(src => src.regel_10_a + src.regel_10_b + src.regel_10_c))
            .ForMember(dest => dest.regel_11, opt => opt.MapFrom(src => src.regel_11_a + src.regel_11_b + src.regel_11_c))
            .ForMember(dest => dest.regel_12, opt => opt.MapFrom(src => src.regel_12_a + src.regel_12_b + src.regel_12_c))
            .ForMember(dest => dest.regel_13, opt => opt.MapFrom(src => src.regel_13_a + src.regel_13_b + src.regel_13_c))
            .ForMember(dest => dest.regel_14, opt => opt.MapFrom(src => src.regel_14_a + src.regel_14_b + src.regel_14_c));

            CreateMap<Class_Epa, EpaDetailsDto>();
            CreateMap<EpaDetailsDto, Class_Epa>().ForMember(dest => dest.EpaId, opt => opt.Ignore());

            CreateMap<Class_Course, CourseDetailsDto>();
            CreateMap<CourseDetailsDto, Class_Course>().ForMember(dest => dest.CourseId, opt => opt.Ignore());

            
        }
    }
}