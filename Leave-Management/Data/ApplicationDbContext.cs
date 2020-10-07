using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Leave_Management.Models;

namespace Leave_Management.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<LeaveHistory> LeaveHistories { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveAllocation> LeaveAllocations { get; set; }
        public DbSet<Leave_Management.Models.LeaveTypeViewModel> DetailsLeaveTypeViewModel { get; set; }
        public DbSet<Leave_Management.Models.EmployeeViewModel> EmployeeViewModel { get; set; }
        public DbSet<Leave_Management.Models.LeaveAllocationViewModel> LeaveAllocationViewModel { get; set; }
        public DbSet<Leave_Management.Models.EditLeaveAllocationViewModel> EditLeaveAllocationViewModel { get; set; }
    }
}
