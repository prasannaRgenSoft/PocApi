using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestCoreWebApi.Model
{
    public class EmployeeModel
    {
        public int  ?Id { get; set; }
        
        public string   EmpName { get; set; }
        public string   EmpLastName { get; set; }
        [Required]
        public bool     ?Gender { get; set; }
        public string   City { get; set; }
        public string   Designation { get; set; }
        [Required]
        public decimal  ?Salary { get; set; }
     


    }



    public class Employee : DbContext
    {
      
        public Employee(DbContextOptions<Employee> options)
            : base(options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer(@"Data Source=203.193.138.52,6148;Initial Catalog=POC_UAT;User ID=ttsh_crio;Password=Newuser@123;");
            //optionsBuilder.UseSqlServer(@"Data Source=WSD001\SQLEXPRESS;Initial Catalog=Practice;User ID=sa;Password=Newuser@123;");
        }

       
        public  DbSet<EmployeeModel> empset { get; set; }
    }

}
