using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KantanMitsumori.Entity.IDEEnitities
{
    public partial class IDEContext : DbContext
    {
        public IDEContext()
        {
        }

        public IDEContext(DbContextOptions<IDEContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MtIdeCarTax> MtIdeCarTaxes { get; set; } = null!;
        public virtual DbSet<MtIdeCartype> MtIdeCartypes { get; set; } = null!;
        public virtual DbSet<MtIdeCommission> MtIdeCommissions { get; set; } = null!;
        public virtual DbSet<MtIdeConsumptionTax> MtIdeConsumptionTaxes { get; set; } = null!;
        public virtual DbSet<MtIdeContractPlan> MtIdeContractPlans { get; set; } = null!;
        public virtual DbSet<MtIdeFeeAdjustment> MtIdeFeeAdjustments { get; set; } = null!;
        public virtual DbSet<MtIdeGuarantee> MtIdeGuarantees { get; set; } = null!;
        public virtual DbSet<MtIdeInspection> MtIdeInspections { get; set; } = null!;
        public virtual DbSet<MtIdeInterest> MtIdeInterests { get; set; } = null!;
        public virtual DbSet<MtIdeLeaseFeeLowerLimit> MtIdeLeaseFeeLowerLimits { get; set; } = null!;
        public virtual DbSet<MtIdeLeaseTarget> MtIdeLeaseTargets { get; set; } = null!;
        public virtual DbSet<MtIdeLiabilityInsurance> MtIdeLiabilityInsurances { get; set; } = null!;
        public virtual DbSet<MtIdeMaintenance> MtIdeMaintenances { get; set; } = null!;
        public virtual DbSet<MtIdeNameChange> MtIdeNameChanges { get; set; } = null!;
        public virtual DbSet<MtIdePromotion> MtIdePromotions { get; set; } = null!;
        public virtual DbSet<MtIdeUnitPrice> MtIdeUnitPrices { get; set; } = null!;
        public virtual DbSet<MtIdeVoluntaryInsurance> MtIdeVoluntaryInsurances { get; set; } = null!;
        public virtual DbSet<MtIdeWeightTax> MtIdeWeightTaxes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MtIdeCarTax>(entity =>
            {
                entity.HasKey(e => new { e.CarType, e.FirstRegistrationDateFrom, e.FirstRegistrationDateTo, e.ElapsedYearsFrom, e.ElapsedYearsTo, e.IsElectricCar, e.DisplacementFrom, e.DisplacementTo });

                entity.ToTable("MT_IDE_CAR_TAX");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser).HasMaxLength(20);
            });

            modelBuilder.Entity<MtIdeCartype>(entity =>
            {
                entity.HasKey(e => e.CarType);

                entity.ToTable("MT_IDE_CARTYPE");

                entity.Property(e => e.CarType).ValueGeneratedNever();

                entity.Property(e => e.CarTypeName).HasMaxLength(20);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser).HasMaxLength(20);
            });

            modelBuilder.Entity<MtIdeCommission>(entity =>
            {
                entity.ToTable("MT_IDE_COMMISSION");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Idname)
                    .HasMaxLength(20)
                    .HasColumnName("IDName");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser).HasMaxLength(20);
            });

            modelBuilder.Entity<MtIdeConsumptionTax>(entity =>
            {
                entity.HasKey(e => e.ConsumptionTax);

                entity.ToTable("MT_IDE_CONSUMPTION_TAX");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser).HasMaxLength(20);
            });

            modelBuilder.Entity<MtIdeContractPlan>(entity =>
            {
                entity.ToTable("MT_IDE_CONTRACT_PLAN");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.PlanName).HasMaxLength(20);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser).HasMaxLength(20);
            });

            modelBuilder.Entity<MtIdeFeeAdjustment>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("MT_IDE_FEE_ADJUSTMENT");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser).HasMaxLength(20);
            });

            modelBuilder.Entity<MtIdeGuarantee>(entity =>
            {
                entity.HasKey(e => e.Years);

                entity.ToTable("MT_IDE_GUARANTEE");

                entity.Property(e => e.Years).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser).HasMaxLength(20);
            });

            modelBuilder.Entity<MtIdeInspection>(entity =>
            {
                entity.HasKey(e => e.CarType);

                entity.ToTable("MT_IDE_INSPECTION");

                entity.Property(e => e.CarType).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.FirstTerm).HasMaxLength(20);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser).HasMaxLength(20);
            });

            modelBuilder.Entity<MtIdeInterest>(entity =>
            {
                entity.HasKey(e => new { e.LeasePeriodFrom, e.LeasePeriodTo });

                entity.ToTable("MT_IDE_INTEREST");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser).HasMaxLength(20);
            });

            modelBuilder.Entity<MtIdeLeaseFeeLowerLimit>(entity =>
            {
                entity.HasKey(e => e.LeaseFeeLowerLimit);

                entity.ToTable("MT_IDE_LEASE_FEE_LOWER_LIMIT");

                entity.Property(e => e.LeaseFeeLowerLimit).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser).HasMaxLength(20);
            });

            modelBuilder.Entity<MtIdeLeaseTarget>(entity =>
            {
                entity.ToTable("MT_IDE_LEASE_TARGET");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.TargetName).HasMaxLength(20);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser).HasMaxLength(20);
            });

            modelBuilder.Entity<MtIdeLiabilityInsurance>(entity =>
            {
                entity.HasKey(e => e.CarType);

                entity.ToTable("MT_IDE_LIABILITY_INSURANCE");

                entity.Property(e => e.CarType).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser).HasMaxLength(20);
            });

            modelBuilder.Entity<MtIdeMaintenance>(entity =>
            {
                entity.HasKey(e => new { e.CarType, e.LeasePeriod, e.BeforeFirstInspection, e.InspectionCount })
                    .HasName("PK_MT_IDE_MENTENANCE");

                entity.ToTable("MT_IDE_MAINTENANCE");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser).HasMaxLength(20);
            });

            modelBuilder.Entity<MtIdeNameChange>(entity =>
            {
                entity.HasKey(e => e.NameChange);

                entity.ToTable("MT_IDE_NAME_CHANGE");

                entity.Property(e => e.NameChange).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser).HasMaxLength(20);
            });

            modelBuilder.Entity<MtIdePromotion>(entity =>
            {
                entity.HasKey(e => e.Promotion);

                entity.ToTable("MT_IDE_PROMOTION");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser).HasMaxLength(20);
            });

            modelBuilder.Entity<MtIdeUnitPrice>(entity =>
            {
                entity.HasKey(e => e.UnitPrice);

                entity.ToTable("MT_IDE_UNIT_PRICE");

                entity.Property(e => e.UnitPrice).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser).HasMaxLength(20);
            });

            modelBuilder.Entity<MtIdeVoluntaryInsurance>(entity =>
            {
                entity.ToTable("MT_IDE_VOLUNTARY_INSURANCE");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CompanyName).HasMaxLength(20);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser).HasMaxLength(20);
            });

            modelBuilder.Entity<MtIdeWeightTax>(entity =>
            {
                entity.HasKey(e => new { e.CarType, e.ElapsedYearsFrom, e.ElapsedYearsTo });

                entity.ToTable("MT_IDE_WEIGHT_TAX");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser).HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
