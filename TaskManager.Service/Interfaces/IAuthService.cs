﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Service.DTOs.Request;
using TaskManager.Service.DTOs.Response;
using TaskManager.Service.DTOs.SignUp;
using TaskManager.Service.Helpers;

namespace TaskManager.Service.Interfaces
{
    public interface IAuthService
    {
        Task<BaseResult<LoginResponseDto>> Login(LoginRequestDto user);
        Task<BaseResult<LoginResponseDto>> Refresh(RefreshRequestDto refreshRequest);
        Task<BaseResult<string>> Logout(string UserId);
        Task<BaseResult<string>> SignUp(EmployeeSignUpDto employee);
        Task<BaseResult<string>> SignUp(ManagerSignUpDto manager);
        Task<BaseResult<string>> SignUp(AdminSignUpDto adminDto);
        Task<BaseResult<string>> ForgotPassword(VerifyEmailRequestDto verifyDto);
        Task<BaseResult<string>> ResetPassword(ResetPasswordRequestDto passwordRequestDto);
        Task<BaseResult<string>> VerifyEmail(VerifyEmailRequestDto verifyDto);

    }
}
