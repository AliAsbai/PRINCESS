using System.Collections.Generic;
using System.Linq;
using PRINCESS.model;

/**
 *  authors:
 *          @Gabriel Vega
 *          
 *  A static class for returning users without passwords.
 **/

namespace PRINCESS.Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<User> WithoutPasswords(this IEnumerable<User> users)
        {
            return users.Select(x => x.WithoutPassword());
        }

        public static User WithoutPassword(this User user)
        {
            user.Password = null;
            return user;
        }
    }
}
