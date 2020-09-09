using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace JobHunter_MVC.Models
{
    public class JHContext : DbContext
    {
        public JHContext() : base(@"Server=DESKTOP-TLG3S9J\SQLEXPRESS;Database=JH_Db;Trusted_Connection=True;")
        {
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Vacanci> Vacancis { get; set; }
        public DbSet<JobSeeker> JobSeekers { get; set; }
    }
}