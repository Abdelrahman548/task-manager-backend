using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager.Data.Entities;
using TaskManager.Data.Entities.Abstracts;
using TaskManager.Repository.Interfaces;
using TaskManager.Service.DTOs.Request;
using TaskManager.Service.DTOs.Response;
using TaskManager.Service.DTOs.SignUp;
using TaskManager.Service.Helpers;
using TaskManager.Service.Interfaces;

namespace TaskManager.Service.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork repo;
        private readonly IMapper mapper;
        private readonly ITokenService tokenService;
        private readonly IOTPService otpService;

        public AuthService(IUnitOfWork repo, IMapper mapper, ITokenService tokenService, IOTPService otpService)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.tokenService = tokenService;
            this.otpService = otpService;
        }

        public async Task<BaseResult<string>> ForgotPassword(VerifyEmailRequestDto verifyDto)
        {
            Person? user = await repo.Employees.FindAsync(E => E.Username == verifyDto.Email);
            if (user is null)
            {
                user = await repo.Managers.FindAsync(E => E.Username == verifyDto.Email);
                if (user is null)
                {
                    user = await repo.Admins.FindAsync(E => E.Username == verifyDto.Email);
                    if (user is null)
                        return new() { IsSuccess = false, Errors = ["Invalid Email"], StatusCode = MyStatusCode.BadRequest };
                }
            }
            int timeInMinutes = 5;
            string subject = "Your OTP for Email Verfication";
            string otp = otpService.GenerateOTP();
            try
            {
                string body = otpService.GetBodyTemplate(otp, timeInMinutes, "TaskManager App");
                await otpService.SendOTPEmailHtmlBody($"{user.FirstName} {user.LastName}", verifyDto.Email, subject, body);
            }
            catch
            {
                return new() { IsSuccess = false, Errors = ["Something went wrong, please try again later"], StatusCode = MyStatusCode.BadRequest };
            }
            var otpVerify = await repo.OTPVerifications.FindAsync(E => E.Email == verifyDto.Email);
            var expirationTime = DateTime.UtcNow.AddMinutes(timeInMinutes);
            var hashedOtp = HashingManager.HashPassword(otp);
            if (otpVerify is null)
            {
                otpVerify = new OTPVerification() { ID = Guid.NewGuid(), Email = user.Username, HashedOTP =  hashedOtp, ExpirationTime = expirationTime};
                await repo.OTPVerifications.AddAsync(otpVerify);
            }else
            {
                otpVerify.HashedOTP = hashedOtp;
                otpVerify.ExpirationTime = expirationTime;
            }
            await repo.CompeleteAsync();
            return new() { IsSuccess = true, Message = "OTP is sent, Please check your email box", StatusCode = MyStatusCode.OK };
        }
        public async Task<BaseResult<string>> ResetPassword(ResetPasswordRequestDto passwordRequestDto)
        {
            var otpVerify = await repo.OTPVerifications.FindAsync(E => E.Email == passwordRequestDto.Email);
            if(otpVerify is null)
                return new() { IsSuccess = false, Errors = ["Invalid Creadentials"], StatusCode = MyStatusCode.BadRequest };
            if(otpVerify.ExpirationTime < DateTime.UtcNow)
                return new() { IsSuccess = false, Errors = ["Invalid Creadentials"], StatusCode = MyStatusCode.BadRequest };
            if (HashingManager.VerifyPassword(passwordRequestDto.OTPCode, otpVerify.HashedOTP))
            {

                Person? user = await repo.Employees.FindAsync(E => E.Username == passwordRequestDto.Email);
                if (user is null)
                {
                    user = await repo.Managers.FindAsync(E => E.Username == passwordRequestDto.Email);
                    if (user is null)
                    {
                        user = await repo.Admins.FindAsync(E => E.Username == passwordRequestDto.Email);
                        if (user is null)
                            return new() { IsSuccess = false, Errors = ["Invalid Email"], StatusCode = MyStatusCode.BadRequest };
                    }
                }
                user.Password = HashingManager.HashPassword(passwordRequestDto.NewPassword);
                await repo.CompeleteAsync();
                return new() { IsSuccess = true, Message = "Password is reset Successfully", StatusCode = MyStatusCode.OK };
            }
            return new() { IsSuccess = false, Errors = ["Invalid Creadentials"], StatusCode = MyStatusCode.BadRequest };
        }
        public async Task<BaseResult<LoginResponseDto>> Login(LoginRequestDto user)
        {
            var userview = await repo.UsersView.FindAsync(E => E.Username == user.Username);
            if (userview is null)
                return new() { IsSuccess = false, Errors = ["Invalid Email or Password"], StatusCode = MyStatusCode.NotFound };

            var isValidPassword = HashingManager.VerifyPassword(user.Password, userview.Password);
            if (!isValidPassword)
                return new() { IsSuccess = false, Errors = ["Invalid Email or Password"], StatusCode = MyStatusCode.NotFound };

            var accessToken = tokenService.GenerateAccessToken(userview);

            var loginResponse = new LoginResponseDto() { Token = new Token() { AccessToken = accessToken }, Role = userview.Role };

            return new() { IsSuccess = true, Data = loginResponse, StatusCode = MyStatusCode.OK };
        }
        public async Task<BaseResult<string>> VerifyEmail(VerifyEmailRequestDto verifyDto)
        {
            int timeInMinutes = 5;
            string subject = "Your OTP for Email Verfication";
            string otp = otpService.GenerateOTP();
            try
            {
                string body = otpService.GetBodyTemplate(otp, timeInMinutes, "TaskManager App");
                await otpService.SendOTPEmailHtmlBody($"unknown", verifyDto.Email, subject, body);
            }
            catch
            {
                return new() { IsSuccess = false, Errors = ["Something went wrong, please try again later"], StatusCode = MyStatusCode.BadRequest };
            }
            var otpVerify = await repo.OTPVerifications.FindAsync(E => E.Email == verifyDto.Email);
            var expirationTime = DateTime.UtcNow.AddMinutes(timeInMinutes);
            var hashedOtp = HashingManager.HashPassword(otp);
            if (otpVerify is null)
            {
                otpVerify = new OTPVerification() { ID = Guid.NewGuid(), Email = verifyDto.Email, HashedOTP = hashedOtp, ExpirationTime = expirationTime };
                await repo.OTPVerifications.AddAsync(otpVerify);
            }
            else
            {
                otpVerify.HashedOTP = hashedOtp;
                otpVerify.ExpirationTime = expirationTime;
            }
            await repo.CompeleteAsync();
            return new() { IsSuccess = true, Message = "OTP is sent, Please check your email box", StatusCode = MyStatusCode.OK };
        }
        public async Task<BaseResult<string>> SignUp(EmployeeSignUpDto employeeDto)
        {
            var repeated = await IsUsernameRepeated(employeeDto.Username);
            if (repeated)
                return new() { IsSuccess = false, Errors = ["Repeated Username"], StatusCode = MyStatusCode.BadRequest };

            var otpVerify = await repo.OTPVerifications.FindAsync(E => E.Email == employeeDto.Username);
            if(otpVerify is null)
                return new() { IsSuccess = false, Errors = ["The Username is not verfied"], StatusCode = MyStatusCode.BadRequest };

            if(!HashingManager.VerifyPassword(employeeDto.OTPEmailVerifyCode, otpVerify.HashedOTP))
                return new() { IsSuccess = false, Errors = ["Invalid Credintials"], StatusCode = MyStatusCode.BadRequest };
            
            if(otpVerify.ExpirationTime < DateTime.UtcNow)
                return new() { IsSuccess = false, Errors = ["Invalid Credintials"], StatusCode = MyStatusCode.BadRequest };

            var employee = mapper.Map<Employee>(employeeDto);
            
            employee.Username = employeeDto.Username;
            var hashed = HashingManager.HashPassword(employeeDto.Password);
            employee.Password = hashed;
            
            employee.ID = Guid.NewGuid();
            
            var department = await repo.Departments.GetByIdAsync(employeeDto.DepartmentId);
            if (department is null) return new() { IsSuccess = false, Errors = ["Department is Not Found"], StatusCode = MyStatusCode.BadRequest };
            employee.Department = department;

            await repo.Employees.AddAsync(employee);
            await repo.CompeleteAsync();
            return new() { IsSuccess = true, Message = "Registered Successfully", StatusCode = MyStatusCode.Created };
        }
        public async Task<BaseResult<string>> SignUp(ManagerSignUpDto managerDto)
        {
            var repeated = await IsUsernameRepeated(managerDto.Username);
            if (repeated)
                return new() { IsSuccess = false, Errors = ["Repeated Username"], StatusCode = MyStatusCode.BadRequest };
            var otpVerify = await repo.OTPVerifications.FindAsync(E => E.Email == managerDto.Username);
            if (otpVerify is null)
                return new() { IsSuccess = false, Errors = ["The Username is not verfied"], StatusCode = MyStatusCode.BadRequest };

            if (!HashingManager.VerifyPassword(managerDto.OTPEmailVerifyCode, otpVerify.HashedOTP))
                return new() { IsSuccess = false, Errors = ["Invalid Credintials"], StatusCode = MyStatusCode.BadRequest };

            if (otpVerify.ExpirationTime < DateTime.UtcNow)
                return new() { IsSuccess = false, Errors = ["Invalid Credintials"], StatusCode = MyStatusCode.BadRequest };

            var manager = mapper.Map<Manager>(managerDto);

            manager.Username = managerDto.Username;
            var hashed = HashingManager.HashPassword(managerDto.Password);
            manager.Password = hashed;
            manager.ID = Guid.NewGuid();

            var department = await repo.Departments.GetByIdAsync(managerDto.DepartmentId);
            if (department is null) return new() { IsSuccess = false, Errors = ["Department is Not Found"], StatusCode = MyStatusCode.BadRequest };
            manager.Department = department;

            await repo.Managers.AddAsync(manager);
            await repo.CompeleteAsync();
            return new() { IsSuccess = true, Message = "Registered Successfully", StatusCode = MyStatusCode.Created };
        }
        public async Task<BaseResult<string>> SignUp(AdminSignUpDto adminDto)
        {
            var repeated = await IsUsernameRepeated(adminDto.Username);
            if(repeated)
                return new() { IsSuccess = false, Errors = ["Repeated Username"], StatusCode = MyStatusCode.BadRequest };

            var otpVerify = await repo.OTPVerifications.FindAsync(E => E.Email == adminDto.Username);
            if (otpVerify is null)
                return new() { IsSuccess = false, Errors = ["The Username is not verfied"], StatusCode = MyStatusCode.BadRequest };

            if (!HashingManager.VerifyPassword(adminDto.OTPEmailVerifyCode, otpVerify.HashedOTP))
                return new() { IsSuccess = false, Errors = ["Invalid Credintials"], StatusCode = MyStatusCode.BadRequest };

            if (otpVerify.ExpirationTime < DateTime.UtcNow)
                return new() { IsSuccess = false, Errors = ["Invalid Credintials"], StatusCode = MyStatusCode.BadRequest };

            var admin = mapper.Map<Admin>(adminDto);

            admin.Username = adminDto.Username;
            var hashed = HashingManager.HashPassword(adminDto.Password);
            admin.Password = hashed;

            admin.ID = Guid.NewGuid();

            await repo.Admins.AddAsync(admin);
            await repo.CompeleteAsync();
            return new() { IsSuccess = true, Message = "Registered Successfully", StatusCode = MyStatusCode.Created };
        }
        private async Task<bool> IsUsernameRepeated(string username)
        {
            var user = await repo.UsersView.FindAsync(E => E.Username == username);
            if (user is not null) return true;

            return false;
        }
    }
}
