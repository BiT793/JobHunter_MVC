using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobHunter_MVC.Models
{
    public class Vacanci
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public Guid Id { get; set; }
        [Required] public string user_id { get; set; }
        [Required] public string name_company { get; set; }
        [Required] public string name_job { get; set; }
        [Required] public double experience_job { get; set; }
        [Required] public string skills { get; set; }
        [Required] public decimal salary { get; set; }
        [Required] public DateTime create_data { get; set; }
        [Required] public bool isValid { get; set; }
        public string list_summary { get; set; }
    }
}