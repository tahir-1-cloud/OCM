using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace OCMDomain.Repository.Edmx
{
    public partial class OCMContext : IdentityDbContext<Users, IdentityRole, string>
    {
        public OCMContext()
        {
        }

        public OCMContext(DbContextOptions<OCMContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRole { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaim { get; set; }
        public virtual DbSet<AspNetUser> AspNetUser { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaim { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogin { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRole { get; set; }
        public virtual DbSet<AssetTble> AssetTbles { get; set; }
        public virtual DbSet<AssignTeacherTble> AssignTeacherTbles { get; set; }
        public virtual DbSet<BatchTble> BatchTbles { get; set; }
        public virtual DbSet<BranchTble> BranchTbles { get; set; }
        public virtual DbSet<ChangePasswordTble> ChangePasswordTbles { get; set; }
        public virtual DbSet<CheckUserTble> CheckUserTbles { get; set; }
        public virtual DbSet<ContactUsTble> ContactUsTbles { get; set; }
        public virtual DbSet<CourseMaterialTble> CourseMaterialTbles { get; set; }
        public virtual DbSet<CourseOutlineTble> CourseOutlineTbles { get; set; }
        public virtual DbSet<CourseQuotaTble> CourseQuotaTbles { get; set; }
        public virtual DbSet<CourseScheduleTble> CourseScheduleTbles { get; set; }
        public virtual DbSet<CourseTimeLineTble> CourseTimeLineTbles { get; set; }
        public virtual DbSet<DetailAccount> DetailAccounts { get; set; }
        public virtual DbSet<EmployContractTble> EmployContractTbles { get; set; }
        public virtual DbSet<EmployTble> EmployTbles { get; set; }
        public virtual DbSet<ExistingUserTble> ExistingUserTbles { get; set; }
        public virtual DbSet<FaqsTble> FaqsTbles { get; set; }
        public virtual DbSet<FeeTble> FeeTbles { get; set; }
        public virtual DbSet<ForgotPaswordTble> ForgotPaswordTbles { get; set; }
        public virtual DbSet<InstallmentPlanTble> InstallmentPlanTbles { get; set; }
        public virtual DbSet<InstitutionTble> InstitutionTbles { get; set; }
        public virtual DbSet<LabTble> LabTbles { get; set; }
        public virtual DbSet<OnlineCourseTble> OnlineCourseTbles { get; set; }
        public virtual DbSet<PhysicalCourseTble> PhysicalCourseTbles { get; set; }
        public virtual DbSet<Receivable> Receivables { get; set; }
        public virtual DbSet<RegisterNewUserTble> RegisterNewUserTbles { get; set; }
        public virtual DbSet<ResetPassword> ResetPasswords { get; set; }
        public virtual DbSet<StudentRegistrationTble> StudentRegistrationTbles { get; set; }
        public virtual DbSet<SubscriptionTble> SubscriptionTbles { get; set; }
        public virtual DbSet<UpdateUserTble> UpdateUserTbles { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-DF3JMOD\\SQLEXPRESS;Database=OCM;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<IdentityUserLogin<string>>();
            modelBuilder.Ignore<IdentityUserToken<string>>();
            modelBuilder.Ignore<IdentityUser<string>>();
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.Property(e => e.ConcurrencyStamp)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.NormalizedName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.Property(e => e.Cnic).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.ProfileImg)
                    .HasColumnType("image")
                    .HasColumnName("profile_img");

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AssetTble>(entity =>
            {
                entity.HasKey(e => e.AssetId);

                entity.ToTable("Asset_tble");

                entity.Property(e => e.AssetName).HasMaxLength(50);

                entity.Property(e => e.SelectLab).HasMaxLength(50);
            });

            modelBuilder.Entity<AssignTeacherTble>(entity =>
            {
                entity.HasKey(e => e.AssignId);

                entity.ToTable("AssignTeacher_tble");

                entity.Property(e => e.AssignBy).HasMaxLength(50);

                entity.Property(e => e.AssignDate).HasColumnType("date");

                entity.Property(e => e.TeacherName).HasMaxLength(50);
            });

            modelBuilder.Entity<BatchTble>(entity =>
            {
                entity.HasKey(e => e.BatchId);

                entity.ToTable("Batch_tble");

                entity.Property(e => e.BtachName).HasMaxLength(50);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<BranchTble>(entity =>
            {
                entity.HasKey(e => e.BranchId);

                entity.ToTable("Branch_tble");

                entity.Property(e => e.AddedBy).HasMaxLength(50);

                entity.Property(e => e.BranchEmail).HasMaxLength(50);

                entity.Property(e => e.BranchName).HasMaxLength(50);

                entity.Property(e => e.BranchPhone).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.InstituteName).HasMaxLength(50);

                entity.Property(e => e.NoofCourses).HasMaxLength(50);

                entity.Property(e => e.NoofStudents).HasMaxLength(50);

                entity.Property(e => e.Owner).HasMaxLength(50);

                entity.Property(e => e.OwnerEmail).HasMaxLength(50);

                entity.Property(e => e.OwnerMobile).HasMaxLength(50);
            });

            modelBuilder.Entity<ChangePasswordTble>(entity =>
            {
                entity.HasKey(e => e.ChangePasswordId);

                entity.ToTable("ChangePassword_tble");

                entity.Property(e => e.ConfirmNewPassword)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.CurrentPassword).HasMaxLength(50);

                entity.Property(e => e.NewPassword).HasMaxLength(50);
            });

            modelBuilder.Entity<CheckUserTble>(entity =>
            {
                entity.ToTable("CheckUser_tble");
            });

            modelBuilder.Entity<ContactUsTble>(entity =>
            {
                entity.HasKey(e => e.ContacUsId);

                entity.ToTable("ContactUs_tble");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CourseMaterialTble>(entity =>
            {
                entity.HasKey(e => e.CourseId);

                entity.ToTable("CourseMaterial_tble");

                entity.Property(e => e.UploadDate).HasColumnType("date");
            });

            modelBuilder.Entity<CourseOutlineTble>(entity =>
            {
                entity.ToTable("CourseOutline_tble");
            });

            modelBuilder.Entity<CourseQuotaTble>(entity =>
            {
                entity.HasKey(e => e.CourseQuotaId);

                entity.ToTable("CourseQuota_tble");

                entity.Property(e => e.CourseQuotaId).HasColumnName("CourseQuotaID");

                entity.Property(e => e.BatchName).HasMaxLength(50);

                entity.Property(e => e.CourseName).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.NoofStudents).HasMaxLength(50);

                entity.Property(e => e.RemainingSeats).HasMaxLength(50);
            });

            modelBuilder.Entity<CourseScheduleTble>(entity =>
            {
                entity.HasKey(e => e.TimeTableId);

                entity.ToTable("CourseSchedule_tble");
            });

            modelBuilder.Entity<CourseTimeLineTble>(entity =>
            {
                entity.ToTable("CourseTimeLine_tble");
            });

            modelBuilder.Entity<DetailAccount>(entity =>
            {
                entity.Property(e => e.DetailAccountId).HasColumnName("DetailAccountID");

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<EmployContractTble>(entity =>
            {
                entity.HasKey(e => e.ConId);

                entity.ToTable("EmployContract_tble");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Cnic).HasMaxLength(50);

                entity.Property(e => e.ContractExpireDate).HasColumnType("datetime");

                entity.Property(e => e.ContractType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Enum).HasColumnName("ENum");

                entity.Property(e => e.FullName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.JoiningDate).HasColumnType("datetime");

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.ProbationEndDate).HasColumnType("datetime");

                entity.Property(e => e.ProbationPeriod)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProbationStartDate).HasColumnType("datetime");

                entity.Property(e => e.SalaryAmount).HasMaxLength(50);

                entity.Property(e => e.SalaryType).HasMaxLength(50);
            });

            modelBuilder.Entity<EmployTble>(entity =>
            {
                entity.HasKey(e => e.EmpId);

                entity.ToTable("Employ_tble");

                entity.Property(e => e.DrivingLicence).HasMaxLength(50);

                entity.Property(e => e.EmpCnic).HasMaxLength(50);

                entity.Property(e => e.EmpDegree).HasColumnType("image");

                entity.Property(e => e.EmpDegreeYear).HasColumnType("date");

                entity.Property(e => e.EmpDob).HasColumnType("date");

                entity.Property(e => e.EmpEmail).HasMaxLength(50);

                entity.Property(e => e.EmpExperience).HasMaxLength(50);

                entity.Property(e => e.EmpExperienceLetter).HasColumnType("image");

                entity.Property(e => e.EmpMaritalStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmpMobileNum).HasMaxLength(50);

                entity.Property(e => e.EmpNumber).HasMaxLength(50);

                entity.Property(e => e.EmpQualification).HasMaxLength(50);

                entity.Property(e => e.EmpSpecializedDegree).HasMaxLength(50);

                entity.Property(e => e.EmpType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RefAddress).HasMaxLength(50);

                entity.Property(e => e.RefEmail).HasMaxLength(50);

                entity.Property(e => e.RefMobileNum).HasMaxLength(50);

                entity.Property(e => e.RefName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ExistingUserTble>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("ExistingUser_tble");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FaqsTble>(entity =>
            {
                entity.HasKey(e => e.FaqId);

                entity.ToTable("Faqs_tble");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<FeeTble>(entity =>
            {
                entity.HasKey(e => e.FeeId);

                entity.ToTable("Fee_tble");

                entity.Property(e => e.AddedBy).HasMaxLength(50);

                entity.Property(e => e.CourseName).HasMaxLength(50);

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.FeeAmount).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PerCreditHour).HasMaxLength(50);
            });

            modelBuilder.Entity<ForgotPaswordTble>(entity =>
            {
                entity.HasKey(e => e.ForgotId);

                entity.ToTable("ForgotPasword_tble");

                entity.Property(e => e.Email).HasMaxLength(50);
            });

            modelBuilder.Entity<InstallmentPlanTble>(entity =>
            {
                entity.ToTable("InstallmentPlan-tble");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.InstallmentDate).HasColumnName("installmentDate");
            });

            modelBuilder.Entity<InstitutionTble>(entity =>
            {
                entity.HasKey(e => e.InstituteId);

                entity.ToTable("Institution_tble");

                entity.Property(e => e.Aolevel).HasColumnName("AOLevel");

                entity.Property(e => e.Board).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Logo).HasColumnType("image");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.NoofCourses).HasMaxLength(50);

                entity.Property(e => e.NoofStudents).HasMaxLength(50);

                entity.Property(e => e.OwnerName).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.RegistrationNo).HasMaxLength(50);

                entity.Property(e => e.StartedYear).HasMaxLength(50);
            });

            modelBuilder.Entity<LabTble>(entity =>
            {
                entity.HasKey(e => e.LabId);

                entity.ToTable("Lab_tble");

                entity.Property(e => e.AssistantName).HasMaxLength(50);

                entity.Property(e => e.Department).HasMaxLength(50);

                entity.Property(e => e.LabName).HasMaxLength(50);
            });

            modelBuilder.Entity<OnlineCourseTble>(entity =>
            {
                entity.HasKey(e => e.OnlineCourseId);

                entity.ToTable("OnlineCourse_tble");

                entity.Property(e => e.CourseType).HasMaxLength(50);

                entity.Property(e => e.CreditHours).HasMaxLength(50);

                entity.Property(e => e.Logo).HasColumnType("image");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PhysicalCourseTble>(entity =>
            {
                entity.HasKey(e => e.CourseId);

                entity.ToTable("PhysicalCourse_tble");

                entity.Property(e => e.BatchName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CourseCode).HasMaxLength(50);

                entity.Property(e => e.CourseName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CoursePhoto)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.FeePerCredit).HasMaxLength(50);
            });

            modelBuilder.Entity<Receivable>(entity =>
            {
                entity.HasKey(e => e.RecvId);

                entity.Property(e => e.RecvId).HasColumnName("RecvID");

                entity.Property(e => e.DetailAccountId).HasColumnName("DetailAccountID");

                entity.HasOne(d => d.DetailAccount)
                    .WithMany(p => p.Receivables)
                    .HasForeignKey(d => d.DetailAccountId)
                    .HasConstraintName("FK_Receivables_DetailAccounts");
            });

            modelBuilder.Entity<RegisterNewUserTble>(entity =>
            {
                entity.HasKey(e => e.RegisterNewUserId);

                entity.ToTable("RegisterNewUser_Tble");

                entity.Property(e => e.RegisterNewUserId).HasColumnName("RegisterNewUserID");

                entity.Property(e => e.ConfirmPassword).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.SelectRole)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ResetPassword>(entity =>
            {
                entity.ToTable("ResetPassword");

                entity.Property(e => e.ConfirmPassword).HasMaxLength(250);

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.Password).HasMaxLength(250);
            });

            modelBuilder.Entity<StudentRegistrationTble>(entity =>
            {
                entity.HasKey(e => e.StudentId);

                entity.ToTable("StudentRegistration_tble");

                entity.Property(e => e.Dob).HasColumnType("date");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.Gender)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.StdChallanForm).HasColumnType("image");

                entity.Property(e => e.StdCnic).HasMaxLength(50);

                entity.Property(e => e.Username)
                    .HasMaxLength(256)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<SubscriptionTble>(entity =>
            {
                entity.HasKey(e => e.SubscriptionId);

                entity.ToTable("Subscription_tble");

                entity.Property(e => e.SubscriptionEmail).HasMaxLength(50);
            });

            modelBuilder.Entity<UpdateUserTble>(entity =>
            {
                entity.HasKey(e => e.UpdateUserId);

                entity.ToTable("UpdateUser_tble");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.Property(e => e.VoucherId).HasColumnName("VoucherID");

                entity.Property(e => e.AccountCode).HasMaxLength(50);

                entity.Property(e => e.AccountName).HasMaxLength(50);

                entity.Property(e => e.Amount).HasMaxLength(50);

                entity.Property(e => e.ClosingBalance).HasMaxLength(50);

                entity.Property(e => e.Credit).HasMaxLength(50);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Debit).HasMaxLength(50);

                entity.Property(e => e.DetailAccountId).HasColumnName("DetailAccountID");

                entity.Property(e => e.VoucherNumber).HasMaxLength(50);

                entity.HasOne(d => d.DetailAccount)
                    .WithMany(p => p.Vouchers)
                    .HasForeignKey(d => d.DetailAccountId)
                    .HasConstraintName("FK_Vouchers_DetailAccounts");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
