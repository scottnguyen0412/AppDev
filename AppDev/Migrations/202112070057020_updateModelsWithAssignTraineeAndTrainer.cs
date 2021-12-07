namespace AppDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateModelsWithAssignTraineeAndTrainer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssignTraineeToCourses",
                c => new
                    {
                        TraineeId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TraineeId, t.CourseId })
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Trainees", t => t.TraineeId, cascadeDelete: true)
                .Index(t => t.TraineeId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.AssignTrainerToCourses",
                c => new
                    {
                        TrainerId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TrainerId, t.CourseId })
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Trainners", t => t.TrainerId, cascadeDelete: true)
                .Index(t => t.TrainerId)
                .Index(t => t.CourseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssignTrainerToCourses", "TrainerId", "dbo.Trainners");
            DropForeignKey("dbo.AssignTrainerToCourses", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.AssignTraineeToCourses", "TraineeId", "dbo.Trainees");
            DropForeignKey("dbo.AssignTraineeToCourses", "CourseId", "dbo.Courses");
            DropIndex("dbo.AssignTrainerToCourses", new[] { "CourseId" });
            DropIndex("dbo.AssignTrainerToCourses", new[] { "TrainerId" });
            DropIndex("dbo.AssignTraineeToCourses", new[] { "CourseId" });
            DropIndex("dbo.AssignTraineeToCourses", new[] { "TraineeId" });
            DropTable("dbo.AssignTrainerToCourses");
            DropTable("dbo.AssignTraineeToCourses");
        }
    }
}
