using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using JobHunter_MVC.Models;
using Microsoft.Ajax.Utilities;
using static JobHunter_MVC.Models.AccountLoginRegister;
using static JobHunter_MVC.Models.Add;

namespace JobHunter_MVC.Controllers
{
    public class AccountAuthController : Controller
    {
        JHContext db = new JHContext();

        public ActionResult Login()
        {
            return View(new UserLogin());
        }

        [HttpPost]
        public ActionResult Login(UserLogin user)
        {
            if (ModelState.IsValid)
            {


                bool validLogin = db.Accounts.Any(x => x.login == user.login);

                if (!validLogin)
                {
                    
                    return RedirectToAction("Login");
                }

                string password = db.Accounts.Where(x => x.login == user.login)
                                             .Select(x => x.password)
                                             .Single();


                string passwordMatches =  user.password.GetHashCode().ToString();

                if (passwordMatches!=password)
                {
                    
                    return RedirectToAction("Login"); 
                }
                var ConfirmUser = db.Accounts.FirstOrDefault(x => x.login == user.login && x.password == passwordMatches);
                if (ConfirmUser != null)
                {
                    //var ticet = new FormsAuthenticationTicket(1, ConfirmUser.login, DateTime.Now, DateTime.Now.AddDays(1), true, ConfirmUser.access_level.ToString());
                    string authId = Guid.NewGuid().ToString();
                    Session["AuthID"] = authId;
                   
                    if (Request.Cookies["AuthID"] == null)
                    {
                        HttpCookie cookie = new HttpCookie("AuthID");
                        cookie["Id"] = ConfirmUser.Id.ToString();
                        cookie["Login"] = ConfirmUser.login;
                        cookie["Role"] = ConfirmUser.access_level.ToString();
                        cookie.Expires = DateTime.Now.AddHours(2);
                        Response.Cookies.Add(cookie);
                    }
                    return RedirectToAction("Private");
                }
                return RedirectToAction("Login"); 
            }
            return RedirectToAction("Login");
        }
        public ActionResult Private()
        {
            try
            {
                if (Request.Cookies["ASP.NET_SessionId"].Value == Session.SessionID)
                {
                    //HttpCookie cookie = Request.Cookies["AuthID"];
                    //int role;
                    //if (cookie != null)
                    //{
                        //role = Convert.ToInt32(cookie["Role"]);


                        //switch (role)
                        //{
                        //    case 0:
                        //        return RedirectToAction("ShowAdmin");
                        //        break;
                        //    case 1:
                        //        return RedirectToAction("ShowOwner");
                        //        break;
                        //    case 2:
                        //        return RedirectToAction("ShowJobHunter");
                        //        break;
                        //    case 3:
                        //        return RedirectToAction("ShowRecruit");
                        //        break;
                        //    default:
                        //        return RedirectToAction("Login");
                        //        break;
                        //}
                    //}
                    return RedirectToAction("ShowAdmin");
                    //return View(); ПОМЕНЯТЬ
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            catch
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult LogOff()
        {
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                HttpCookie cookie = new HttpCookie("ASP.NET_SessionId");
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            if (Request.Cookies["AuthID"] != null)
            {
                HttpCookie cookie = new HttpCookie("AuthID");
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        public ActionResult Register()
        {
            return View(new UserRegister());
        }
        [HttpPost]
        public ActionResult Register(UserRegister user)
        {
            if (ModelState.IsValid)
            {
                bool ValidUser = db.Accounts.Any(x => x.login == user.login);
                if (!ValidUser)
                {
                    string password = user.password.GetHashCode().ToString();
                    db.Accounts.Add(new Account { login = user.login, email = user.Email, password = password, access_level = Convert.ToInt32(user.access_level), company = user.company, create_date=DateTime.Now });
                    db.SaveChanges();

                    bool ConfirmUser = db.Accounts.Any(x => x.login == user.login);
                    if (ConfirmUser)
                    {
                        
                        return RedirectToAction("Login");
                    }
                    else return RedirectToAction("Register"); 
                }
                else ModelState.AddModelError("", "Пользователь с таким логином уже существует");
            }
            return RedirectToAction("Register");
        }
        public ActionResult ShowAdmin()
        {
            HttpCookie cookie = Request.Cookies["AuthID"];
            string login;
            string id;
            int role;
            IEnumerable<Account> accounts;
            IEnumerable<JobSeeker> jobSeekers;
            IEnumerable<Vacanci> vacancis;

            login = cookie["Login"];
            id = cookie["Id"];
            role = Convert.ToInt32(cookie["Role"]);

            if (cookie != null)
            {
                ViewBag.Login = login;
                ViewBag.Access = role;

                switch (role)
                {
                    case 0:
                        {
                            accounts = db.Accounts.ToList();
                            jobSeekers = db.JobSeekers.ToList();
                            vacancis = db.Vacancis.ToList();
                            ViewBag.Accounts = accounts;
                            ViewBag.JobSeekers = jobSeekers;
                            ViewBag.Vacancis = vacancis;
                            break;
                        }
                    case 1:
                        {
                            jobSeekers = db.JobSeekers.ToList();
                            vacancis = db.Vacancis.Where(x => x.user_id == id).ToList().ToList();
                            ViewBag.JobSeekers = jobSeekers;
                            ViewBag.Vacancis = vacancis;
                            break;
                        }
                    case 2:
                        {
                            jobSeekers = db.JobSeekers.ToList();
                            vacancis = db.Vacancis.ToList();
                            ViewBag.JobSeekers = jobSeekers;
                            ViewBag.Vacancis = vacancis;
                            break;
                        }
                    case 3:
                        {
                            jobSeekers = db.JobSeekers.Where(x => x.user_id == id).ToList();
                            vacancis = db.Vacancis.ToList();
                            ViewBag.JobSeekers = jobSeekers;
                            ViewBag.Vacancis = vacancis;
                            break;
                        }
                        
                    default:
                        return RedirectToAction("Login");
                        break;
                }
 
                return View();
            }
            return RedirectToAction("Login");
        }
        public ActionResult ShowOwner()
        {
            HttpCookie cookie = Request.Cookies["AuthID"];
            string login;
            string id;
            if (cookie != null)
            {
                login = cookie["Login"];
                id = cookie["Id"];
                
                ViewBag.Login = login;
                return View();
            }
            return RedirectToAction("Login");
        }
        public ActionResult ShowRecruit()
        {
            ////HttpCookie cookie = Request.Cookies["AuthID"];
            ////string login;
            ////if (cookie != null)
            ////{
            ////    login = cookie["Login"];
               
            ////    ViewBag.Login = login;
            ////    return View();
            ////}
            return RedirectToAction("Login");
        }
        public ActionResult ShowJobHunter()
        {
            HttpCookie cookie = Request.Cookies["AuthID"];
            string login;
            string id;
            if (cookie != null)
            {
                login = cookie["Login"];
                id= cookie["Id"];
                IEnumerable<JobSeeker> jobSeekers = db.JobSeekers.Where(x => x.user_id == id).ToList();
                IEnumerable<Vacanci> vacancis = db.Vacancis.ToList();
                ViewBag.JobSeekers = jobSeekers;
                ViewBag.Vacancis = vacancis;
                ViewBag.Login = login;
                return View();
            }
            return RedirectToAction("Login");
        }
        
        public ActionResult Delete(string id)
        {
            var del =db.Accounts.FirstOrDefault(x=>x.Id.ToString()==id);
            if (del != null)
            {
                db.Accounts.Remove(del);
            }
            else
            {
                var dell = db.JobSeekers.FirstOrDefault(x => x.Id.ToString() == id);
                if (dell != null)
                {
                    db.JobSeekers.Remove(dell);
                }
                else
                {
                    var delll = db.Vacancis.FirstOrDefault(x => x.Id.ToString() == id);
                    if (delll != null)
                    {
                        db.Vacancis.Remove(delll);
                    }
                    else return RedirectToAction("Private");
                }
               
            }
            db.SaveChanges();
            return RedirectToAction("Private");
        }
        public ActionResult AddVacanci()
        {
            HttpCookie cookie = Request.Cookies["AuthID"];

            int role = Convert.ToInt32(cookie["Role"]);

            ViewBag.Access = role;

            return View();
        }
        [HttpPost]
        public ActionResult AddVacanci(AddVacanci vacan)
        {
            HttpCookie cookie = Request.Cookies["AuthID"];
            if (cookie != null)
            {
                if (ModelState.IsValid)
                {

                    db.Vacancis.Add(new Vacanci {user_id= cookie["Id"], name_company =vacan.name_company, name_job=vacan.name_job,skills=vacan.skills,salary=vacan.salary,experience_job=vacan.experience_job, create_data=DateTime.Now});
                    db.SaveChanges();
                    return RedirectToAction("Private");
                }
            }
            return RedirectToAction("AddVacanci");
        }
        public ActionResult AddJob()
        {
            return View(new JobSeeker());
        }
        [HttpPost]
        public ActionResult AddJob(AddJob job)
        {
            HttpCookie cookie = Request.Cookies["AuthID"];
            if (cookie != null)
            {
                if (ModelState.IsValid)
                {

                    db.JobSeekers.Add(new JobSeeker { user_id = cookie["Id"], first_name =job.first_name, last_name = job.last_name, my_skills = job.my_skills, my_experience_job = job.my_experience_job, name_pref_job = job.name_pref_job, pref_salary=job.pref_salary, create_date=job.create_date, end_data=job.end_data });
                    db.SaveChanges();
                    return RedirectToAction("Private");
                }
            }
            return RedirectToAction("AddVacanci");
        }
        public ActionResult Edit(string id)
        {
            var edit = db.Accounts.FirstOrDefault(x => x.Id.ToString() == id);
            if (edit != null)
            {
                ViewBag.Edit = 1;
            }
            else
            {
                var editt = db.JobSeekers.FirstOrDefault(x => x.Id.ToString() == id);
                if (editt != null)
                {
                    ViewBag.Edit = 2;
                }
                else
                {
                    var edittt = db.Vacancis.FirstOrDefault(x => x.Id.ToString() == id);
                    if (edittt != null)
                    {
                        ViewBag.Edit = 3;
                    }
                    else return RedirectToAction("Private");
                }

            }

            int role;
            if (Request.Cookies["EditID"] == null)
            {
                HttpCookie cookie = new HttpCookie("EditID");
                cookie["Id"] = id;
                cookie.Expires = DateTime.Now.AddMinutes(20);
                Response.Cookies.Add(cookie);
            }
            HttpCookie cooki = Request.Cookies["AuthID"];
            if (cooki != null)
            {
                role = Convert.ToInt32(cooki["Role"]);
                ViewBag.Access = role;
            }

                return View();
        }
        [HttpPost]
        public ActionResult EditV(AddVacanci vac)
        {
            if (ModelState.IsValid)
            {
                HttpCookie cookie = Request.Cookies["EditID"];
                if (cookie != null)
                {
                    string id = cookie["Id"];
                    var confVac = db.Vacancis.FirstOrDefault(x => x.Id.ToString() == id);
                    if (confVac != null)
                    {
                        confVac.name_company = vac.name_company;
                        confVac.name_job = vac.name_job;
                        confVac.experience_job = vac.experience_job;
                        confVac.skills = vac.skills;
                        confVac.salary = vac.salary;
                        db.SaveChanges();

                        if (Request.Cookies["EditID"] != null)
                        {
                            HttpCookie cookii = new HttpCookie("EditID");
                            cookii.Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies.Add(cookii);
                        }
                    }
                    return RedirectToAction("Private");
                }
            }
            return RedirectToAction("Edit");
        }
        [HttpPost]
        public ActionResult EditJ(AddJob job)
        {
            if (ModelState.IsValid)
            {
                HttpCookie cookie = Request.Cookies["EditID"];
                if (cookie != null)
                {
                    string id = cookie["Id"];
                    var confJob = db.JobSeekers.FirstOrDefault(x => x.Id.ToString() == id);
                    if (confJob != null)
                    {
                        confJob.first_name = job.first_name;
                        confJob.last_name = job.last_name;
                        confJob.name_pref_job = job.name_pref_job;
                        confJob.my_experience_job = job.my_experience_job;
                        confJob.my_skills = job.my_skills;
                        confJob.pref_salary = job.pref_salary;
                        db.SaveChanges();

                        if (Request.Cookies["EditID"] != null)
                        {
                            HttpCookie cookii = new HttpCookie("EditID");
                            cookii.Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies.Add(cookii);
                        }
                    }
                    return RedirectToAction("Private");
                }
            }
            
            return RedirectToAction("Edit");
        }
        [HttpPost]
        public ActionResult EditA(UserRegister user)
        {
            if (ModelState.IsValid)
            {
                HttpCookie cookie = Request.Cookies["EditID"];
                if (cookie != null)
                {
                    string id = cookie["Id"];
                    var confUser = db.Accounts.FirstOrDefault(x => x.Id.ToString() == id);
                    if (confUser != null)
                    {
                        confUser.login = "";
                        db.SaveChanges();
                        bool validLogin = db.Accounts.Any(x => x.login == user.login);
                        if (!validLogin)
                        {
                            confUser.login = user.login;
                            confUser.email = user.Email;
                            confUser.access_level = Convert.ToInt32(user.access_level);
                            confUser.company = user.company;
                            confUser.password = user.password;
                            db.SaveChanges();
                            if (Request.Cookies["EditID"] != null)
                            {
                                HttpCookie cookii = new HttpCookie("EditID");
                                cookii.Expires = DateTime.Now.AddDays(-1);
                                Response.Cookies.Add(cookii);
                            }
                        }
                        return RedirectToAction("Private");
                    }
                }
            }
            return RedirectToAction("Edit");
        }
        public ActionResult ShowProfile(string id)
        {
            IEnumerable<JobSeeker> jobSeekers = db.JobSeekers.Where(x => x.Id.ToString() == id).ToList();
            ViewBag.Job = jobSeekers;
            return View();
        }
    }
}