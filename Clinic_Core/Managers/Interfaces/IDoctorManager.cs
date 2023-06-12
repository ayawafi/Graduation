using Clinic_DbModel.Models;
using Clinic_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_Core.Managers.Interfaces
{
    public interface IDoctorManager
    {
        Task<ResponseApi> SignUp(DoctorRegistrationModelView DoctorReg);
        Task<ResponseApi> SignIn(PatientLoginModelView DoctorLogin);
        ResponseApi GetAllPatients();
        ResponseApi GetTopDoctors();
        ResponseApi GetTopDoctorsBySpecificNumber(int Number);
        ResponseApi CompleteDoctorProfile(string DoctorId, CompleteDoctorVM doctor);
        ResponseApi UpdateDoctorProfile(string DoctorId, UpdateDoctorVM doctor);
        ResponseApi SearchDoctors(string gender, int SpecialtyId);
        ResponseApi Search(string clinicAddress, string clinicName);
        ResponseApi GetMyPatientAppointment(string userId);
        ResponseApi GetMyPatient(string userId);
        ResponseApi ChangePassword(string userId, ChangePasswordViewModel changePasswordVM);
        ResponseApi ResetPassword(string email, ResetPasswordVM resetPasswordVM);
        ResponseApi ConfirmationCode(int confirmationCode, string email);
        Task<ResponseApi> SendEmailResetPassword(string email);
        ResponseApi GetDoctorProfileById(int DoctorId);

    }
}
