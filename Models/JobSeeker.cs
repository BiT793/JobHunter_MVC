using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobHunter_MVC.Models
{
    public class JobSeeker
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public Guid Id { get; set; }
        [Required] public string user_id { get; set; }
        [Required] public string first_name { get; set; }
        [Required] public string last_name { get; set; }
        [Required] public string name_pref_job { get; set; }
        [Required] public double my_experience_job { get; set; }
        [Required] public string my_skills { get; set; }
        [Required] public decimal pref_salary { get; set; }
        [Required] public DateTime create_date { get; set; }
        [Required] public DateTime end_data { get; set; }

    }
}