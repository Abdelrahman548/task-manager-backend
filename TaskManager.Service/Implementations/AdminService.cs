﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Entities;
using TaskManager.Repository.Helpers;
using TaskManager.Repository.Interfaces;
using TaskManager.Service.DTOs.Request;
using TaskManager.Service.DTOs.Response;
using TaskManager.Service.Helpers;
using TaskManager.Service.Interfaces;

namespace TaskManager.Service.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork repo;
        private readonly IMapper mapper;

        public AdminService(IUnitOfWork repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }
        public async Task<BaseResult<AdminResponseDto>> Get(Guid adminId)
        {
            var admin = await repo.Admins.GetByIdAsync(adminId);
            if (admin is null) return new BaseResult<AdminResponseDto>() { IsSuccess = false, Errors = ["Not Found"] , StatusCode = MyStatusCode.NotFound};
            var responseDto = mapper.Map<AdminResponseDto>(admin);
            return new BaseResult<AdminResponseDto>() { IsSuccess = true, Data = responseDto, Message = Messages.GET_SUCCESS, StatusCode = MyStatusCode.OK};
        }

        public async Task<BaseResult<PagedList<AdminResponseDto>>> Get(ItemQueryParameters criteria)
        {
            var pageList = await repo.Admins.GetAllAsync<Admin>(criteria);
            var responsePageList = new PagedList<AdminResponseDto>(
                                    pageList.Items.Select(item => mapper.Map<AdminResponseDto>(item)).ToList(),
                                    pageList.Page,
                                    pageList.PageSize,
                                    pageList.TotalCount
                                );

            return new() { IsSuccess = true, Data = responsePageList, Message = Messages.GET_SUCCESS, StatusCode = MyStatusCode.OK};
        }

        public async Task<BaseResult<Guid>> Update(Guid adminId, AdminRequestDto adminDto)
        {
            var admin = await repo.Admins.GetByIdAsync(adminId);
            if (admin is null) return new() { IsSuccess = false, Errors = ["Not Found"], StatusCode = MyStatusCode.NotFound };
            mapper.Map(adminDto, admin);
            await repo.CompeleteAsync();
            return new() { IsSuccess = true, Message = Messages.UPDATE_SUCCESS, Data = admin.ID , StatusCode = MyStatusCode.OK};
        }
    }
}
