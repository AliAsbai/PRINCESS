using System.ComponentModel.DataAnnotations;

/**
 *  authors:
 *          @Gabriel Vega
 *
 *  The authenticate model defines the parameters for incoming request to the /users/authenticate route of the Api,
 *  because it is set as the parameters to the Authenticate method of the UsersController.
 **/

namespace PRINCESS.model
{
    public class AuthenticateModel
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
