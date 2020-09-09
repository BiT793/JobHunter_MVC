using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobHunter_MVC.Models
{
    public class Account
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public Guid Id { get; set; }

        [Required] public DateTime create_date { get; set; }
        [Required] public string login { get; set; }
        [Required] public string password { get; set; }
        [Required] public string email { get; set; }
        [Required] public int access_level { get; set; }
        [Required] public string company { get; set; }
    }
}