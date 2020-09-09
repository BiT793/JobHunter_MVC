using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobHunter_MVC.Models
{
    public class Add
    {
        public class AddVacanci
        {
            [Required] public string name_company { get; set; }
            [Required] public string name_job { get; set; }
            [Required] public double experience_job { get; set; }
            [Required] public string skills { get; set; }
            [Required] public decimal salary { get; set; }
            [Required] public DateTime create_data { get; set; }
            [Required] public bool isValid { get; set; }
        }

        public class AddJob
        {
            [Required] public string first_name { get; set; }
            [Required] public string last_name { get; set; }
            [Required] public string name_pref_job { get; set; }
            [Required] public double my_experience_job { get; set; }
            [Required] public string my_skills { get; set; }
            [Required] public decimal pref_salary { get; set; }
            [Required] public DateTime create_date { get; set; }
            [Required] public DateTime end_data { get; set; }
        }
        public class AddAccount
        {
            [Required] public string login { get; set; }
            [Required] public string password { get; set; }
            [Required] public string email { get; set; }
            [Required] public int access_level { get; set; }
            [Required] public string company { get; set; }
            [Required] public DateTime create_date { get; set; }
        }
    }
}