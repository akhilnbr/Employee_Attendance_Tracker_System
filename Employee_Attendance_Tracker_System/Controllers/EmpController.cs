using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Employee_Attendence_Tracker.Models;
using System.Data.Entity;
using System.Net.Mail;
using System.Net;
using Employee_Attendence_Tracker;

namespace Employee_Attendence_Tracker.Controllers
{
    public class EmpController : Controller
    {
        // GET: Emp

        private DbModel db;

        public EmpController()
        {
            db = new DbModel();
        }
        public ActionResult RegisterEmp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterEmp(Employee employee)
        {
            foreach(ModelState modelstate in ViewData.ModelState.Values)
            {
                foreach(ModelError error in  modelstate.Errors)
                {
                    var list = error.ErrorMessage;
                    db.Employees.Add(employee);
                    db.SaveChanges();
                    

                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Employee employee, LogDetails log)
         {
            
            if (employee.Email.Equals("admin") && employee.Password.Equals("admin"))
            {
                return RedirectToAction("Admin","Emp");
            }


            employee = db.Employees.Where(m =>
                m.Email.Equals(employee.Email) && m.Password.Equals(employee.Password)).FirstOrDefault();
            if (employee != null)
            {


                TempData["EmployeeDash"] = employee.FirstName + " " + employee.LastName;


                TempData["Email"] = employee.Email;

                log.employee = employee.FirstName + employee.LastName;
                log.date = DateTime.Now.Date.ToShortDateString();
                log.login = DateTime.Now.ToShortTimeString();
                log.email = employee.Email;

                db.Logs.Add(log);
                db.SaveChanges();

                // ViewBag.details = Session["FirstName"].ToString();
                // EmployeeDash(ViewBag.details);

                return RedirectToAction("EmployeeDash", "Emp", employee);
            }
            else
            {
                ViewBag.message = "Login Failed";



                return View();

            }

            

        }
        [HttpGet]
        public ActionResult Admin()
        {

            string date = DateTime.Now.ToShortDateString();
            try
            {

                List<LogDetails> model = (from a in db.Logs.ToList()
                    where a.date == date
                    group a by new { a.email, a.date, a.employee }
                    into g

                    let firstLogin = g.OrderBy(x => x.login).First()
                    let lastLogin = g.OrderBy(x => x.logout).Last()
                    select new Employee_Attendence_Tracker.Models.LogDetails()
                    {
                        employee = g.Key.employee,
                        date = g.Key.date,

                        login = firstLogin.login,
                        logout = lastLogin.logout
                    }).ToList();
                return View(model);
            }
            catch
            {
                ViewBag.adminalert = "Sorry something went wrong";
            }





            return View();

        }
        [HttpPost]
        public ActionResult Admin(string selected )
        {
             selected = Request["date"].ToString();
            List<LogDetails> model = (from a in db.Logs.ToList()
                                      where a.date == selected
                                      group a by new { a.email, a.date, a.employee }
                into g

                                      let firstLogin = g.OrderBy(x => x.login).First()
                                      let lastLogin = g.OrderBy(x => x.logout).Last()
                                      select new Employee_Attendence_Tracker.Models.LogDetails()
                                      {
                                          employee = g.Key.employee,
                                          date = g.Key.date,

                                          login = firstLogin.login,
                                          logout = lastLogin.logout
                                      }).ToList();
            return View(model);

        }

        public enum Command
        {
            signout
        }

        public enum date
        {
            selectdate
        }





        [HttpGet]
        public ActionResult EmployeeDash()
        {
           

            var emp = TempData["Email"];

            if (emp != null)
            {
                string Date = DateTime.Today.ToShortDateString();
                string inTime = (from x in db.Logs where x.date == Date && x.email==emp select x.login).Min();
                string outTime = (from x in db.Logs where x.date == Date && x.email==emp select x.logout).Max();

               List<string> sList = new List<string>();
               sList.Add(emp.ToString());
               sList.Add(inTime);
               sList.Add(outTime);
               sList.ToList();
              
               
                

                //var detailss = from x in db.Logs select (x.login == inTime && x.logout == outTime);

                //var detailss = from x in db.Logs (select (x.login == inTime && x.logout == outTime)
               // var details = db.Logs.Where(m => m.email == emp && m.login == inTime && m.logout==outTime).ToList();
              
                return View(sList.ToList());

            }

            

            return View();



        }

        [HttpPost]
        public ActionResult EmployeeDash( Command command, LogDetails log)
        {


            if (command==Command.signout)

            {
                TempData["login"] = log.login;
                
                string str = DateTime.Now.ToShortTimeString();   
                int id = (from x in db.Logs select x.id).Max();
                var update =db.Logs.Single(u => u.id == id);
                update.logout = str;
                db.SaveChanges();
                return RedirectToAction("Login", "Emp");
                 

           
            }

            return View();
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPass pass )
        {
            var getdetails = db.Employees.Where(x => x.Email.Equals(pass.Email) && x.Phone.Equals(pass.phone)).SingleOrDefault();
            if(getdetails!=null)
            {
                try
                {

                   
                        var sendmail = new MailAddress("mcakhil@gmail.com", "Akhil");
                        var receiver = new MailAddress(pass.Email, "Buddy");
                    var password = "";//please type your email here
                    var subject = "testmail";
                    var body = "Please follow the link  to reset the password \"http://localhost:52601/Emp/ConfirmPassword\" ";




                    var smptp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(sendmail.Address, password)
                    };
                    using (var mess = new MailMessage(sendmail, receiver)
                    {
                        Subject = subject,
                        Body = body,
                       
                       
                    })
                    {
                        smptp.Send(mess);
                    }

                    ViewBag.Status = "Please check your mail";
                    return View();

                }
                catch
                {
                    ViewBag.Status = "Problem while sending email, Please check details.";
                }


               



            }
            else
            {
                ViewBag.Status = "Sorry it seems you are not a registered user";
            }
            return View();

        }
        [HttpGet]
        public ActionResult ConfirmPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ConfirmPassword(ForgotPass pass)
        {
            var a = TempData["Email"].ToString();
            var update = db.Employees.Single(x => x.Email == a);
            var passwrd= (pass.Password).ToString();
            update.Password = pass.Password;
            //db.Employees.Add(update);
            db.Entry(update).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Login", "Emp");

        }




    }

}