using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PRINCESS.model;
using PRINCESS.Services;

/**
 *  authors:
 *          @Gabriel Vega
 *
 *  A controller class for handling users who wants to login.
 *  
 *  public IActionResult Authenticate([FromBody]AuthenticateModel model)
 *  Authenticates the incoming user credential.
 *  parameters:
 *      model - A structure type that encapsulates the user credentials.
 *  returns:
 *      BadRequest with a message "Username or password is incorrect.", if no user is found in the database.
 *      
 *      Ok-response with a cookie.
 *      
 *  public IActionResult GetAll()
 *  Retrieves all the users in the user-table.
 *  return:
 *      Ok-response the all the users.
 *  
 *  public void logout()
 *  Deletes the cookies and disconnects from the database.
 *  
 **/

namespace PRINCESS.controller
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: User/authenticate
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticateModel model)
        {
            model.Username = model.Username.ToLower();
            var user = _userService.Authenticate(model.Username, model.Password);
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            CookieOptions options = new CookieOptions
            {
                HttpOnly = false
            };
            Response.Cookies.Append("Token " + user.Name, user.Token, options);
            var response = Ok(user);
            response.ContentTypes.Add(user.Token);

            return Ok(response);
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        //GET: User/logout
        [Authorize]
        [HttpGet("logout")]
        public void Logout()
        {
            string TokenName = "Token " + User.Identity.Name;
            
            if(Request.Cookies[TokenName] != null){
                Response.Cookies.Delete(TokenName);
            }
            PrincessDb.disconnect();
        }


    }
}
