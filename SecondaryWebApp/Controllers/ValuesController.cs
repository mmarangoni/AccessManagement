using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace SecondaryWebApp.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        [Authorize(Roles = "Employee")]
        [AuthorizeClaim(ClaimType = "OU", ClaimValue = "Human Resources")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Authorize(Roles = "Employee")]
        [AuthorizeClaim(ClaimType = "OU", ClaimValue = "Sales")]

        public string Get(int id)
        {
            var user = User as ClaimsPrincipal;

            return string.Format("The authenticated user is {0}", user.Identity.Name);
        }

        // POST api/values
        [Authorize(Roles = "Employee")]
        [AuthorizeClaim(ClaimType = "OU", ClaimValue = "IT")]
        [AuthorizeClaim(ClaimType = "Task", ClaimValue = "Modify Employee")]
        public string Post([FromBody]string value)
        {
            return "Add successful";
        }

        // PUT api/values/5
        [Authorize(Roles = "Employee")]
        [AuthorizeClaim(ClaimType = "OU", ClaimValue = "Human Resources")]
        [AuthorizeClaim(ClaimType = "Task", ClaimValue = "Modify Employee")]
        public string Put(int id, [FromBody]string value)
        {
            return "Edit successful";
        }

        [Authorize(Roles = "Employee")]
        [AuthorizeClaim(ClaimType = "OU", ClaimValue = "IT")]
        [AuthorizeClaim(ClaimType = "OU", ClaimValue = "Toronto")]
        [AuthorizeClaim(ClaimType = "Task", ClaimValue = "Modify Employee")]
        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
