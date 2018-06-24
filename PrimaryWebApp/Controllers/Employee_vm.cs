using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// added...
using System.ComponentModel.DataAnnotations;

namespace PrimaryWebApp.Controllers
{
    // Attention - Employee view model classes, plain, and with reports-to and direct-reports info

    public class EmployeeAdd
    {
        public EmployeeAdd()
        {
            BirthDate = DateTime.Now.AddYears(-30);
            HireDate = DateTime.Now.AddYears(-5);
            ReportsToId = 0;
        }

        // We are including the relevant data annotations from the design model class
        // However, to improve data quality, we are adding "Required" to some properties

        [Required, StringLength(20)]
        public string LastName { get; set; }

        [Required, StringLength(20)]
        public string FirstName { get; set; }

        [StringLength(30)]
        public string Title { get; set; }

        // Attention - Allow a "new employee" object to include, or not, the "ReportsToId" value
        // Self-referencing to-one property
        public int? ReportsToId { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? HireDate { get; set; }

        [Required, StringLength(70)]
        public string Address { get; set; }

        [Required, StringLength(40)]
        public string City { get; set; }

        [Required, StringLength(40)]
        public string State { get; set; }

        [Required, StringLength(40)]
        public string Country { get; set; }

        [Required, StringLength(10)]
        public string PostalCode { get; set; }

        [Required, StringLength(24)]
        public string Phone { get; set; }

        [Required, StringLength(24)]
        public string Fax { get; set; }

        [Required, StringLength(60)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public class EmployeeBase : EmployeeAdd
    {
        [Key]
        public int EmployeeId { get; set; }
    }

    public class EmployeeWithDetails : EmployeeBase
    {
        public EmployeeWithDetails()
        {
            DirectReports = new List<EmployeeBase>();
        }

        // Attention - Employee with self-referencing object(s) details

        // Self-referencing to-one property
        public virtual EmployeeBase ReportsTo { get; set; }

        // Self-referencing to-many property
        public IEnumerable<EmployeeBase> DirectReports { get; set; }
    }

    public class EmployeeSupervisor
    {
        // Attention - For an employee, set the supervisor identifier

        [Key]
        public int EmployeeId { get; set; }

        // Manager/supervisor identifier
        public int ReportsToId { get; set; }
    }

    public class EmployeeDirectReports
    {
        // Attention - For an employee, set the identifiers of the direct-reports
        public EmployeeDirectReports()
        {
            EmployeeIds = new List<int>();
        }

        [Key]
        public int EmployeeId { get; set; }

        // Collection of identifiers for the direct-reports (other employees who report to this employee)
        public IEnumerable<int> EmployeeIds { get; set; }
    }

}
