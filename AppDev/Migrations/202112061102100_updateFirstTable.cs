namespace AppDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateFirstTable : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Trainees", name: "StaffId", newName: "TraineeId");
            RenameIndex(table: "dbo.Trainees", name: "IX_StaffId", newName: "IX_TraineeId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Trainees", name: "IX_TraineeId", newName: "IX_StaffId");
            RenameColumn(table: "dbo.Trainees", name: "TraineeId", newName: "StaffId");
        }
    }
}
