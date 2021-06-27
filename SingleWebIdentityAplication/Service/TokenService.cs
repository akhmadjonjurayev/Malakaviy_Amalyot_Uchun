using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SingleWebIdentityAplication.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SingleWebIdentityAplication.Service
{
    public interface ITokenService
    {
       Task<ResponseData<string>> SignInToken(SignInViewModel model);
       Task<ResponseData<string>> LogInToken(LoginViewModel login);
       Task<ResponseData<List<Product>>> GetAllProduct();
       Task<ResponseData<string>> AddRangeProduct(IEnumerable<Product> products);
       Task<ResponseData<Product>> GetSingleProduct(int ProductId);
       Task<ResponseData<List<UsersModel>>> GetAllPassword();
        Task<ResponseData<string>> AddUsersPassword(UsersModel user);
    }
    public class TokenService : ITokenService
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signIn;
        
        public TokenService(IConfiguration config,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signIn,
            ApplicationDbContext db)
        {
            _db = db;
            _config = config;
            _userManager = userManager;
            _signIn = signIn;
        }
        public async Task<ResponseData<string>> SignInToken(SignInViewModel model)
        {
            if (model.UserName == default || model.Password == default)
                return new ResponseData<string>() { IsSuccess = false, Message = "error-invalid-data" };
            var user = new IdentityUser
            {
                UserName = model.UserName,
                Email = model.EmailAddress
            };
            var signIn =await _userManager.CreateAsync(user);
            if(signIn.Succeeded)
            {
                var result = await _userManager.AddPasswordAsync(user, model.Password);
                if(result.Succeeded)
                {
                    return new ResponseData<string>() { IsSuccess = true, Message = "success-add-data", Data = GenerateToken(model) };
                }
            }
            return new ResponseData<string>() { IsSuccess = false, Message = signIn.Errors.ToString() };
        }
        public string GenerateToken(IModel model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["JWT:Key"]);
            var tokenDiscriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimsType.UserName,model.UserName),
                    new Claim(ClaimsType.UserRole, RolesBase.User)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDiscriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<ResponseData<string>> LogInToken(LoginViewModel login)
        {
            if (login.Password == default || login.UserName == default)
                return new ResponseData<string>() { IsSuccess = false, Message = "error-invalid-data" };
            var user = await _userManager.FindByNameAsync(login.UserName);
            if (user == null)
                return new ResponseData<string>() { IsSuccess = false, Message = "error-not-found-data" };
            var isSuccess = await _signIn.CheckPasswordSignInAsync(user, login.Password, false);
            if (isSuccess.Succeeded)
            {
                return new ResponseData<string>() { IsSuccess = true, Message = "success", Data = GenerateToken(login) };
            }
            return new ResponseData<string>() { IsSuccess = false, Message = "error-not-found-data" };
        }

        public async Task<ResponseData<List<Product>>> GetAllProduct()
        {
            return new ResponseData<List<Product>>() { IsSuccess = true, Data = await _db.Products.Take(4).AsNoTracking().ToListAsync() };
        }

        public async Task<ResponseData<string>> AddRangeProduct(IEnumerable<Product> products)
        {
            if(products.Any())
            {
                _db.ChangeTracker.AutoDetectChangesEnabled = false;
                await _db.Products.AddRangeAsync(products);
                _db.ChangeTracker.AutoDetectChangesEnabled = true;
                await _db.SaveChangesAsync();
                return new ResponseData<string>() { IsSuccess = true, Message = "success-add-data" };
            }
            return new ResponseData<string>() { IsSuccess = false, Message = "error-invalid-data" };
        }

        public async Task<ResponseData<Product>> GetSingleProduct(int ProductId)
        {
            var product = await _db.Products.Where(p => p.ProductId == ProductId).FirstOrDefaultAsync();
            if(product != null)
            {
                return new ResponseData<Product>() { IsSuccess = true, Message = "success-find-data", Data = product };
            }
            return new ResponseData<Product>() { IsSuccess = false, Message = "error-not-found-data" };
        }

        public Task<ResponseData<List<UsersModel>>> GetAllPassword()
        {
            return Task.FromResult(new ResponseData<List<UsersModel>>() { IsSuccess = true, Data = _db.UsersPassword.ToList() });
        }

        public async Task<ResponseData<string>> AddUsersPassword(UsersModel user)
        {
            try
            {
                await _db.UsersPassword.AddAsync(user);
                await _db.SaveChangesAsync();
                return new ResponseData<string>() { IsSuccess = true, Message = "success-add-data" };
            }
            catch(Exception ex)
            {
                return new ResponseData<string>() { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
