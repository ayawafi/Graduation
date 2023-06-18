using Clinic_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Core.Managers.Interfaces
{
    public interface IReviewManager
    {
        ResponseApi AddReview(string userId, int doctorId, ReviewModelView reviewVM);
        ResponseApi GetAllReviewsForDoctor(int doctorId);
    }
}
