using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager.Data.Entities;
using TaskManager.Data.Entities.Abstracts;
using TaskManager.Repository.Interfaces;
using TaskManager.Service.DTOs.Request;
using TaskManager.Service.DTOs.Response;
using TaskManager.Service.DTOs.SignIn;
using TaskManager.Service.Helpers;
using TaskManager.Service.Interfaces;

namespace TaskManager.Service.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork repo;
        private readonly JwtOptions jwtOptions;
        private readonly IMapper mapper;

        public AuthService(IUnitOfWork repo, JwtOptions jwtOptions, IMapper mapper)
        {
            this.repo = repo;
            this.jwtOptions = jwtOptions;
            this.mapper = mapper;
        }
        public async Task<BaseResult<LoginResponseDto>> Login(LoginRequestDto user)
        {
            var employee = await repo.Employees.FindAsync(E => E.Username == user.Username);
            if (employee is not null)
            {
                var result = PasswordManager.VerifyPassword(user.Password, employee.Password);
                if (result)
                {
                    var accessToken = GenerateToken(employee, "Employee");
                    var loginResponse = new LoginResponseDto() { Token = new Token() { AccessToken = accessToken } };
                    return new() { IsSuccess = true, Data = loginResponse , StatusCode = MyStatusCode.OK};
                }
            }
            var manager = await repo.Managers.FindAsync(M => M.Username == user.Username);
            if (manager is not null)
            {
                var result = PasswordManager.VerifyPassword(user.Password, manager.Password);
                if (result)
                {
                    var accessToken = GenerateToken(manager, "Manager");
                    var loginResponse = new LoginResponseDto() { Token = new Token() { AccessToken = accessToken } };
                    return new() { IsSuccess = true, Data = loginResponse, StatusCode = MyStatusCode.OK };
                }
            }
            var admin = await repo.Admins.FindAsync(E => E.Username == user.Username);
            if (admin is not null)
            {
                var result = PasswordManager.VerifyPassword(user.Password, admin.Password);
                if (result)
                {
                    var accessToken = GenerateToken(admin, "Admin");
                    var loginResponse = new LoginResponseDto() { Token = new Token() { AccessToken = accessToken } };
                    return new() { IsSuccess = true, Data = loginResponse, StatusCode = MyStatusCode.OK };
                }
            }
            return new() { IsSuccess = false, Errors = ["Invalid Email or Password" ], StatusCode = MyStatusCode.NotFound };
        }

        public async Task<BaseResult<string>> SignIn(EmployeeSignInDto employeeDto)
        {
            var oldEmployee = await repo.Employees.FindAsync(E => E.Username == employeeDto.Username);
            if(oldEmployee is not null) return new() { IsSuccess = false, Errors = ["Repaeated Username"], StatusCode = MyStatusCode.BadRequest };
            var oldManager = await repo.Managers.FindAsync(E => E.Username == employeeDto.Username);
            if (oldManager is not null) return new() { IsSuccess = false, Errors = ["Repaeated Username"], StatusCode = MyStatusCode.BadRequest };
            var oldAdmin = await repo.Admins.FindAsync(E => E.Username == employeeDto.Username);
            if (oldAdmin is not null) return new() { IsSuccess = false, Errors = ["Repaeated Username"], StatusCode = MyStatusCode.BadRequest };

            var employee = mapper.Map<Employee>(employeeDto);
            
            employee.Username = employeeDto.Username;
            var hashed = PasswordManager.HashPassword(employeeDto.Password);
            employee.Password = hashed;
            
            employee.ID = Guid.NewGuid();
            
            var department = await repo.Departments.GetByIdAsync(employeeDto.DepartmentId);
            if (department is null) return new() { IsSuccess = false, Errors = ["Department is Not Found"], StatusCode = MyStatusCode.BadRequest };
            employee.Department = department;

            await repo.Employees.AddAsync(employee);
            await repo.CompeleteAsync();
            return new() { IsSuccess = true, Message = "Registed Successfully", StatusCode = MyStatusCode.Created };
        }
        public async Task<BaseResult<string>> SignIn(ManagerSignInDto managerDto)
        {
            var oldManager = await repo.Managers.FindAsync(E => E.Username == managerDto.Username);
            if (oldManager is not null) return new() { IsSuccess = false, Errors = ["Repaeated Username"], StatusCode = MyStatusCode.BadRequest };
            var oldEmployee = await repo.Employees.FindAsync(E => E.Username == managerDto.Username);
            if (oldEmployee is not null) return new() { IsSuccess = false, Errors = ["Repaeated Username"], StatusCode = MyStatusCode.BadRequest };
            var oldAdmin = await repo.Admins.FindAsync(E => E.Username == managerDto.Username);
            if (oldAdmin is not null) return new() { IsSuccess = false, Errors = ["Repaeated Username"], StatusCode = MyStatusCode.BadRequest };

            var manager = mapper.Map<Manager>(managerDto);

            manager.Username = managerDto.Username;
            var hashed = PasswordManager.HashPassword(managerDto.Password);
            manager.Password = hashed;
            manager.ID = Guid.NewGuid();

            var department = await repo.Departments.GetByIdAsync(managerDto.DepartmentId);
            if (department is null) return new() { IsSuccess = false, Errors = ["Department is Not Found"], StatusCode = MyStatusCode.BadRequest };
            manager.Department = department;

            await repo.Managers.AddAsync(manager);
            await repo.CompeleteAsync();
            return new() { IsSuccess = true, Message = "Registed Successfully", StatusCode = MyStatusCode.Created };
        }
        public async Task<BaseResult<string>> SignIn(AdminSignInDto adminDto)
        {
            var oldEmployee = await repo.Employees.FindAsync(E => E.Username == adminDto.Username);
            if (oldEmployee is not null) return new() { IsSuccess = false, Errors = ["Repaeated Username"], StatusCode = MyStatusCode.BadRequest };
            var oldManager = await repo.Managers.FindAsync(E => E.Username == adminDto.Username);
            if (oldManager is not null) return new() { IsSuccess = false, Errors = ["Repaeated Username"], StatusCode = MyStatusCode.BadRequest };
            var oldAdmin = await repo.Admins.FindAsync(E => E.Username == adminDto.Username);
            if (oldAdmin is not null) return new() { IsSuccess = false, Errors = ["Repaeated Username"], StatusCode = MyStatusCode.BadRequest };

            var admin = mapper.Map<Admin>(adminDto);

            admin.Username = adminDto.Username;
            var hashed = PasswordManager.HashPassword(adminDto.Password);
            admin.Password = hashed;

            admin.ID = Guid.NewGuid();

            await repo.Admins.AddAsync(admin);
            await repo.CompeleteAsync();
            return new() { IsSuccess = true, Message = "Registed Successfully", StatusCode = MyStatusCode.Created };
        }

        private string GenerateToken(Person person, string role)
        {
            // Fill Token with Info
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                                    SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.NameIdentifier, person.ID.ToString()),
                    new(ClaimTypes.Name, person.Username),
                    new(ClaimTypes.Role, role),
                })

            };

            // Generate Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);
            return accessToken;
        }
    }
}
