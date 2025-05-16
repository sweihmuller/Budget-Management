using AutoMapper;
using Budget_Management.Models;

namespace Budget_Management.Services
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<Account, CreationAccountViewModel>();
        }
    }
}
