using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Entities;
using TaskManager.Service.DTOs.Request;
using TaskManager.Service.DTOs.Response;

namespace TaskManager.Service.Profiles
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<AdminRequestDto, Admin>();
            CreateMap<Admin, AdminResponseDto>();
        }
    }
}
