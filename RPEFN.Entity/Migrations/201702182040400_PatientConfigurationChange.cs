namespace RPEFN.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PatientConfigurationChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Patients", "LastName", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Patients", "LastName", c => c.String());
        }
    }
}
