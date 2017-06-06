using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using WorkplacePlanner.Data;

namespace WorkplacePlanner.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WorkplacePlanner.Data.Entities.CalendarEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comments")
                        .HasMaxLength(500);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("Date");

                    b.Property<int>("LastUpdatedBy");

                    b.Property<DateTime>("LastUpdatedDate");

                    b.Property<int>("TeamMembershipId");

                    b.Property<int>("UsageTypeId");

                    b.HasKey("Id");

                    b.HasIndex("TeamMembershipId");

                    b.HasIndex("UsageTypeId");

                    b.ToTable("CalendarEntries");
                });

            modelBuilder.Entity("WorkplacePlanner.Data.Entities.GlobalDefaultUsageType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime?>("EndDate");

                    b.Property<int>("LastUpdatedBy");

                    b.Property<DateTime>("LastUpdatedDate");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("UsageTypeId");

                    b.HasKey("Id");

                    b.HasIndex("UsageTypeId");

                    b.ToTable("GlobalDefaultUsageTypes");
                });

            modelBuilder.Entity("WorkplacePlanner.Data.Entities.Holiday", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("Date");

                    b.Property<int>("LastUpdatedBy");

                    b.Property<DateTime>("LastUpdatedDate");

                    b.Property<string>("Reason");

                    b.HasKey("Id");

                    b.ToTable("Holidays");
                });

            modelBuilder.Entity("WorkplacePlanner.Data.Entities.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("LastUpdatedBy");

                    b.Property<DateTime>("LastUpdatedDate");

                    b.HasKey("Id");

                    b.ToTable("People");
                });

            modelBuilder.Entity("WorkplacePlanner.Data.Entities.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("LastUpdatedBy");

                    b.Property<DateTime>("LastUpdatedDate");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("WorkplacePlanner.Data.Entities.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("DeskCount");

                    b.Property<bool>("EmailNotificationEnabled");

                    b.Property<int>("LastUpdatedBy");

                    b.Property<DateTime>("LastUpdatedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int?>("ParentTeamId");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("WorkplacePlanner.Data.Entities.TeamDefaultUsageType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime?>("EndDate");

                    b.Property<int>("LastUpdatedBy");

                    b.Property<DateTime>("LastUpdatedDate");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("TeamId");

                    b.Property<int>("UsageTypeId");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.HasIndex("UsageTypeId");

                    b.ToTable("TeamDefaultUsageTypes");
                });

            modelBuilder.Entity("WorkplacePlanner.Data.Entities.TeamManager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime?>("EndDate");

                    b.Property<int>("LastUpdatedBy");

                    b.Property<DateTime>("LastUpdatedDate");

                    b.Property<int>("PersonId");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("TeamId");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.HasIndex("TeamId");

                    b.ToTable("TeamManagers");
                });

            modelBuilder.Entity("WorkplacePlanner.Data.Entities.TeamMembership", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime?>("EndDate");

                    b.Property<int>("LastUpdatedBy");

                    b.Property<DateTime>("LastUpdatedDate");

                    b.Property<int>("PersonId");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("TeamId");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.HasIndex("TeamId");

                    b.ToTable("TeamMemberships");
                });

            modelBuilder.Entity("WorkplacePlanner.Data.Entities.UsageType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Abbreviation")
                        .IsRequired()
                        .HasMaxLength(3);

                    b.Property<bool>("Active");

                    b.Property<string>("ColorCode")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("LastUpdatedBy");

                    b.Property<DateTime>("LastUpdatedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<bool>("Selectable");

                    b.HasKey("Id");

                    b.ToTable("UsageTypes");
                });

            modelBuilder.Entity("WorkplacePlanner.Data.Entities.CalendarEntry", b =>
                {
                    b.HasOne("WorkplacePlanner.Data.Entities.TeamMembership", "TeamMembership")
                        .WithMany("CalendarEntries")
                        .HasForeignKey("TeamMembershipId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkplacePlanner.Data.Entities.UsageType", "UsageType")
                        .WithMany()
                        .HasForeignKey("UsageTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkplacePlanner.Data.Entities.GlobalDefaultUsageType", b =>
                {
                    b.HasOne("WorkplacePlanner.Data.Entities.UsageType", "UsageType")
                        .WithMany()
                        .HasForeignKey("UsageTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkplacePlanner.Data.Entities.TeamDefaultUsageType", b =>
                {
                    b.HasOne("WorkplacePlanner.Data.Entities.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkplacePlanner.Data.Entities.UsageType", "UsageType")
                        .WithMany()
                        .HasForeignKey("UsageTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkplacePlanner.Data.Entities.TeamManager", b =>
                {
                    b.HasOne("WorkplacePlanner.Data.Entities.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkplacePlanner.Data.Entities.Team", "Team")
                        .WithMany("Managers")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkplacePlanner.Data.Entities.TeamMembership", b =>
                {
                    b.HasOne("WorkplacePlanner.Data.Entities.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkplacePlanner.Data.Entities.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
