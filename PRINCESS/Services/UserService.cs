using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PRINCESS.model;
using PRINCESS.Helpers;
using PRINCESS.controller;

/**
 *  authors:
 *          @Gabriel Vega
 *          
 *  The user service contains a method for authenticating user credentials and returning a JWT token,
 *  and a method for getting all users in the application.
 *  
 *  public User Authenticate(string username, string password)
 *  Connects to the database in order to search for the incoming user. If the user exists, generates a JWT token for that user.
 *  If no user exists, returns null.
 *  
 *  parameters:
 *      username - A string representing the username.
 *      password - A string representing the password.
 *  
 *  return:
 *      null - if no user is found.
 *      user.WithoutPassword - if a user is found.
 *      
 *  public IEnumerable<User> GetAll()
 *  Returns all the users in the applications without passwords.
 **/

namespace PRINCESS.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
    }
    public class UserService : IUserService
    {

        private List<User> _users = new List<User>();

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public User Authenticate(string username, string password)
        {

            PrincessDb.connect();
            var u = PrincessDb.CheckUser(username, password);
            

            if(u != null)
            {
                _users.Add(u);
            }
            else
            {
                PrincessDb.disconnect();
            }

            var user = _users.SingleOrDefault(x => x.Email == username && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Usertype),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user.WithoutPassword();
        }

        public IEnumerable<User> GetAll()
        {
            return _users.WithoutPasswords();
        }
    }
}
