using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.DataAccess
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<Dbo.Account, Account>();
            CreateMap<Account, Dbo.Account>();

            CreateMap<Dbo.Category, Category>();
            CreateMap<Category, Dbo.Category>();

            CreateMap<Dbo.User, User>();
            CreateMap<User, Dbo.User>();

            CreateMap<Dbo.Bank,Bank>();
            CreateMap<Bank, Dbo.Bank>();
        }
    }
}
