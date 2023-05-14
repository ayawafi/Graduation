using AutoMapper;
using Clinic_Core.Managers.Interfaces;
using Clinic_DbModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Core.Managers.Services
{
    public class SpecializationManager : ISpecializationManager
    {
        private clinic_dbContext _dbContext;
        private IMapper _mapper;
        public SpecializationManager(clinic_dbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public List<Specialization> GetAllSpecialties()
        {
            var result = _dbContext.Specializations.ToList();

            return result;
        }
    }
}
