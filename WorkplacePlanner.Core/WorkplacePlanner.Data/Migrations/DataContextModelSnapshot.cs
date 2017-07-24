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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("WorkplacePlanner.Data.Entities.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<bool>("Active");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("LastUpdatedBy");

                    b.Property<DateTime>("LastUpdatedDate");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

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

                    b.HasIndex("ParentTeamId");

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

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("TeamId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.HasIndex("UserId");

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

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("TeamId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.HasIndex("UserId");

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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole<int>")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("WorkplacePlanner.Data.Entities.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("WorkplacePlanner.Data.Entities.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole<int>")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkplacePlanner.Data.Entities.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
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

            modelBuilder.Entity("WorkplacePlanner.Data.Entities.Team", b =>
                {
                    b.HasOne("WorkplacePlanner.Data.Entities.Team", "ParentTeam")
                        .WithMany("SubTeams")
                        .HasForeignKey("ParentTeamId");
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
                    b.HasOne("WorkplacePlanner.Data.Entities.Team", "Team")
                        .WithMany("Managers")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkplacePlanner.Data.Entities.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkplacePlanner.Data.Entities.TeamMembership", b =>
                {
                    b.HasOne("WorkplacePlanner.Data.Entities.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkplacePlanner.Data.Entities.ApplicationUser", "User")
                        .WithMany("TeamMemberships")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
