namespace JobHunter_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class one_mig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vacancis", "isValid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vacancis", "isValid");
        }
    }
}
