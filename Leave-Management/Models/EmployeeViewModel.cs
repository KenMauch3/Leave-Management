using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Leave_Management.Models
{
    public class EmployeeViewModel
    {
        public string Id { get; set; }
        [DisplayName("Username")]
        public string UserName { get; set; }
        [DisplayName("Email Address")]
        public string Email { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        [DisplayName("First Name")]
        public string Firstname { get; set; }
        [DisplayName("Last Name")]
        public string Lastname { get; set; }
        [DisplayName("Tax ID Number")]
        public string TaxId { get; set; }
        [DisplayName("Date Of Birth")]
        public DateTime DateOfBirth { get; set; }
        [DisplayName("Display Name")]
        public DateTime DateJoined { get; set; }
    }
}
