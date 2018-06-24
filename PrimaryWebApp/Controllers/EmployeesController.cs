using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PrimaryWebApp.Controllers
{
    public class EmployeesController : ApiController
    {
        // Reference to the data manager
        private Manager m = new Manager();

        // GET: api/Employees
        [Authorize(Roles = "Employee")]
        [AuthorizeClaim(ClaimType = "OU", ClaimValue = "Human Resources")]
        public IHttpActionResult Get()
        {
            return Ok(m.EmployeeGetAll());
        }

        // GET: api/Employees/5
        [Authorize(Roles = "Employee")]
        [AuthorizeClaim(ClaimType = "OU", ClaimValue = "Human Resources")]
        [AuthorizeClaim(ClaimType = "OU", ClaimValue = "Toronto")]
        public IHttpActionResult Get(int? id)
        {
            // Attempt to get the matching object
            var o = m.EmployeeGetByIdWithDetails(id.GetValueOrDefault());

            if (o == null)
            {
                return NotFound();
            }
            else
            {
                // Pass the object to the view
                return Ok(o);
            }
        }

        // POST: api/Employees
        [Authorize(Roles = "Employee")]
        [AuthorizeClaim(ClaimType = "Task", ClaimValue = "Modify Employee")]
        public IHttpActionResult Post([FromBody]EmployeeAdd newItem)
        {
            // Ensure that the URI is clean (and does not have an id parameter)
            if (Request.GetRouteData().Values["id"] != null) { return BadRequest("Invalid request URI"); }

            // Ensure that a "newItem" is in the entity body
            if (newItem == null) { return BadRequest("Must send an entity body with the request"); }

            // Ensure that we can use the incoming data
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // Attempt to add the new object
            var addedItem = m.EmployeeAdd(newItem);

            // Continue?
            if (addedItem == null) { return BadRequest("Cannot add the object"); }

            // HTTP 201 with the new object in the entity body
            // Notice how to create the URI for the Location header
            var uri = Url.Link("DefaultApi", new { id = addedItem.EmployeeId });

            return Created(uri, addedItem);
        }

        // PUT: api/Employees/5/SetSupervisor
        [Authorize(Roles = "Employee")]
        [AuthorizeClaim(ClaimType = "Task", ClaimValue = "Modify Employee")]
        [Route("api/employees/{id}/setsupervisor")]
        public void PutSetSupervisor(int id, [FromBody]EmployeeSupervisor item)
        {
            // Attention - Employee set supervisor - command pattern

            // Ensure that an "item" is in the entity body
            if (item == null) { return; }

            // Ensure that the id value in the URI matches the id value in the entity body
            if (id != item.EmployeeId) { return; }

            // Ensure that we can use the incoming data
            if (ModelState.IsValid)
            {
                // Attempt to update the item
                m.EmployeeSetSupervisor(item);
            }
            else
            {
                return;
            }
        }

        // PUT: api/Employees/5/SetDirectReports
        [Route("api/employees/{id}/setdirectreports")]
        public void PutSetDirectReports(int id, [FromBody]EmployeeDirectReports item)
        {
            // Attention - Employee set direct-reports - command pattern

            // Ensure that an "item" is in the entity body
            if (item == null) { return; }

            // Ensure that the id value in the URI matches the id value in the entity body
            if (id != item.EmployeeId) { return; }

            // Ensure that we can use the incoming data
            if (ModelState.IsValid)
            {
                // Attempt to update the item
                m.EmployeeSetDirectReports(item);
            }
            else
            {
                return;
            }
        }

        /*
        // PUT: api/Employees/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Employees/5
        public void Delete(int id)
        {
        }
        */
    }
}
