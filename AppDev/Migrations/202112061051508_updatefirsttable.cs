namespace AppDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatefirsttable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Category", c => c.String());
            AddColumn("dbo.Trainees", "StaffId", c => c.String(maxLength: 128));
            AddColumn("dbo.Trainners", "TrainerId", c => c.String(maxLength: 128));
            AddColumn("dbo.TrainningStaffs", "StaffId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Courses", "CategoryId");
            CreateIndex("dbo.Trainees", "StaffId");
            CreateIndex("dbo.Trainners", "TrainerId");
            CreateIndex("dbo.TrainningStaffs", "StaffId");
            AddForeignKey("dbo.Courses", "CategoryId", "dbo.CourseCategories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Trainees", "StaffId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Trainners", "TrainerId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.TrainningStaffs", "StaffId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Courses", "CourseCategory");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "CourseCategory", c => c.String());
            DropForeignKey("dbo.TrainningStaffs", "StaffId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Trainners", "TrainerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Trainees", "StaffId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Courses", "CategoryId", "dbo.CourseCategories");
            DropIndex("dbo.TrainningStaffs", new[] { "StaffId" });
            DropIndex("dbo.Trainners", new[] { "TrainerId" });
            DropIndex("dbo.Trainees", new[] { "StaffId" });
            DropIndex("dbo.Courses", new[] { "CategoryId" });
            DropColumn("dbo.TrainningStaffs", "StaffId");
            DropColumn("dbo.Trainners", "TrainerId");
            DropColumn("dbo.Trainees", "StaffId");
            DropColumn("dbo.Courses", "Category");
        }
    }
}
