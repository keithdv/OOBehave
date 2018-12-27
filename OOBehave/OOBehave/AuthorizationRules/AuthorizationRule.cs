using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.AuthorizationRules
{

    public interface IAuthorizationRule
    {
    }


    public abstract class AuthorizationRule : IAuthorizationRule
    {
        /// <summary>
        /// Do NOT add dependencies to an Authorization Rule
        /// They are shared accross instances of objects
        /// Add the dependencies to the Execute methods
        /// </summary>
        public AuthorizationRule()
        {

        }

    }
}
