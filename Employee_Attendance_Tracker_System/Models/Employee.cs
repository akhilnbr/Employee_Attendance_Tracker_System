using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Employee_Attendence_Tracker.Models
{
    [Table("EmployeeTable")]
    public class Employee
    {
         [Key]
        public int Id { get; set; }
       
        public string FirstName { get; set; }
    
        public string LastName { get; set; }
      
        public string Gender { get; set; }
     
        public string Phone { get; set; }
       
        public string Designation { get; set; }
       
        public string Email { get; set; }
       
        //[Range(8,10,ErrorMessage ="please type a password between 8 and 10")]
        public string Password { get; set; }
       

        [NotMapped]
       
        public string ConfirmPassword { get; set; }
        [NotMapped]
       
        public string ConfirmEmail{ get; set; }
      
    }
    [Table("login_details")]
    public class LogDetails
    {
        [Key]
        public int id { get; set; }
        public string employee { get; set; }
        public string date { get; set; }
        public string login { get; set; }
        public string logout { get; set; }
        public string email { get; set; }
        



    }
   
    public class ForgotPass
    {
        public string phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
   

}
