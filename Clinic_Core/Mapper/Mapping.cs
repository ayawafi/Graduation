using AutoMapper;
using Clinic_DbModel.Models;
using Clinic_ModelView;

namespace Clinic_Core.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Appointment, AppointmentModelView>().ReverseMap();
            CreateMap<Blog, BlogModelView>().ReverseMap();
            CreateMap<Clinic, ClinicModelView>().ReverseMap();
            CreateMap<Doctor, DoctorModelView>().ReverseMap();
            CreateMap<Patient, PatientModelView>().ReverseMap();
            CreateMap<Review, ReviewModelView>().ReverseMap();
            CreateMap<Scheduletiming, ScheduletimingModelView>().ReverseMap();
            CreateMap<Socialmediaurl, SocialMediaUrlModelView>().ReverseMap();
            CreateMap<Specialization, SpectalizationModelView>().ReverseMap();



            CreateMap<PatientLoginModelView, Patient>().ReverseMap();
            CreateMap<PatientRegistrationModelView, Patient>().ReverseMap();
            CreateMap<Patient, LoginPatientResponse>().ReverseMap();


            CreateMap<PatientLoginModelView, Doctor>().ReverseMap();
            CreateMap<DoctorRegistrationModelView, Doctor>().ReverseMap();
            CreateMap<Doctor, LoginDoctorResponse>().ReverseMap();




        }
    }
}
