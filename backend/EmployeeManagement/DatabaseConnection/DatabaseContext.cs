using System;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.DatabaseConnection
{
	public class DatabaseContext: DbContext
	{
		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
		}

        public DbSet<EmployeeRegisterModel> employee { get; set; }

    }
}
