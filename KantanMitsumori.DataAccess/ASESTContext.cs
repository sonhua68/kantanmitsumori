using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KantanMitsumori.Entity.ASESTEntities
{
    public partial class ASESTContext : DbContext
    {
        public ASESTContext()
        {
        }

        public ASESTContext(DbContextOptions<ASESTContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AsopCarname> AsopCarnames { get; set; } = null!;
        public virtual DbSet<AsopMaker> AsopMakers { get; set; } = null!;
        public virtual DbSet<DmtMaker> DmtMakers { get; set; } = null!;
        public virtual DbSet<MAquisitionTax> MAquisitionTaxes { get; set; } = null!;
        public virtual DbSet<MCar> MCars { get; set; } = null!;
        public virtual DbSet<MCarTax> MCarTaxes { get; set; } = null!;
        public virtual DbSet<MMaker> MMakers { get; set; } = null!;
        public virtual DbSet<MModel> MModels { get; set; } = null!;
        public virtual DbSet<MSelfInsurance> MSelfInsurances { get; set; } = null!;
        public virtual DbSet<MToffice> MToffices { get; set; } = null!;
        public virtual DbSet<MUser> MUsers { get; set; } = null!;
        public virtual DbSet<MUserDef> MUserDefs { get; set; } = null!;
        public virtual DbSet<MWeightTax> MWeightTaxes { get; set; } = null!;
        public virtual DbSet<TEstimate> TEstimates { get; set; } = null!;
        public virtual DbSet<TEstimateIde> TEstimateIdes { get; set; } = null!;
        public virtual DbSet<TEstimateItc> TEstimateItcs { get; set; } = null!;
        public virtual DbSet<TEstimateSub> TEstimateSubs { get; set; } = null!;
        public virtual DbSet<TTaxRatioDef> TTaxRatioDefs { get; set; } = null!;
        public virtual DbSet<TUseLog> TUseLogs { get; set; } = null!;
        public virtual DbSet<TUseLogItc> TUseLogItcs { get; set; } = null!;
        public virtual DbSet<TbPsinfo> TbPsinfos { get; set; } = null!;
        public virtual DbSet<TbRuibetsuN> TbRuibetsuNs { get; set; } = null!;
        public virtual DbSet<TbSy> TbSys { get; set; } = null!;
        public virtual DbSet<TbSysExh> TbSysExhs { get; set; } = null!;
        public virtual DbSet<WAsMember> WAsMembers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AsopCarname>(entity =>
            {
                entity.HasKey(e => new { e.MekerCode, e.CarmodelCode });

                entity.ToTable("ASOP_Carname");

                entity.Property(e => e.MekerCode).HasColumnName("meker_code");

                entity.Property(e => e.CarmodelCode).HasColumnName("carmodel_code");

                entity.Property(e => e.CarmodelName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("carmodel_name");
            });

            modelBuilder.Entity<AsopMaker>(entity =>
            {
                entity.HasKey(e => e.MakerCode);

                entity.ToTable("ASOP_Maker");

                entity.Property(e => e.MakerCode)
                    .ValueGeneratedNever()
                    .HasColumnName("maker_code");

                entity.Property(e => e.MakerName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("maker_name");
            });

            modelBuilder.Entity<DmtMaker>(entity =>
            {
                entity.HasKey(e => e.MakerCode);

                entity.ToTable("DMT_MAKER");

                entity.Property(e => e.MakerCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.InsertDate)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MakerName)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<MAquisitionTax>(entity =>
            {
                entity.HasKey(e => e.AquisitionTaxId);

                entity.ToTable("m_AquisitionTax");

                entity.Property(e => e.AquisitionTaxId).ValueGeneratedNever();

                entity.Property(e => e.CarType).HasColumnName("CAR_TYPE");

                entity.Property(e => e.Dflag).HasColumnName("DFlag");

                entity.Property(e => e.PassedDisp)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PASSED_DISP");

                entity.Property(e => e.PassedYearFrom).HasColumnName("PASSED_YEAR_FROM");

                entity.Property(e => e.PassedYearTo).HasColumnName("PASSED_YEAR_TO");

                entity.Property(e => e.Rdate)
                    .HasColumnType("datetime")
                    .HasColumnName("RDate");

                entity.Property(e => e.RemainRate).HasColumnName("REMAIN_RATE");

                entity.Property(e => e.Udate)
                    .HasColumnType("datetime")
                    .HasColumnName("UDate");
            });

            modelBuilder.Entity<MCar>(entity =>
            {
                entity.HasKey(e => e.CarId);

                entity.ToTable("m_Car");

                entity.Property(e => e.CarId).ValueGeneratedNever();

                entity.Property(e => e.CaseId).HasColumnName("CaseID");

                entity.Property(e => e.ClassNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Dflag).HasColumnName("DFlag");

                entity.Property(e => e.DispVol)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DriveTypeCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.GradeId).HasColumnName("GradeID");

                entity.Property(e => e.GradeName)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Mission)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NewPrice)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PublicCase)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Rdate)
                    .HasColumnType("datetime")
                    .HasColumnName("RDate");

                entity.Property(e => e.RegularCase)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SalesFinish)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.SalesStart)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.SetNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Udate)
                    .HasColumnType("datetime")
                    .HasColumnName("UDate");

                entity.Property(e => e.Weight)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MCarTax>(entity =>
            {
                entity.HasKey(e => e.CarTaxId);

                entity.ToTable("m_CarTax");

                entity.Property(e => e.CarTaxId).ValueGeneratedNever();

                entity.Property(e => e.Dflag).HasColumnName("DFlag");

                entity.Property(e => e.ExaustDisp)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EXAUST_DISP");

                entity.Property(e => e.ExaustFrom).HasColumnName("EXAUST_FROM");

                entity.Property(e => e.ExaustTo).HasColumnName("EXAUST_TO");

                entity.Property(e => e.Rdate)
                    .HasColumnType("datetime")
                    .HasColumnName("RDate");

                entity.Property(e => e.Udate)
                    .HasColumnType("datetime")
                    .HasColumnName("UDate");

                entity.Property(e => e.YearAmount).HasColumnName("YEAR_AMOUNT");
            });

            modelBuilder.Entity<MMaker>(entity =>
            {
                entity.HasKey(e => e.MakerId);

                entity.ToTable("m_Maker");

                entity.Property(e => e.MakerId).ValueGeneratedNever();

                entity.Property(e => e.Dflag).HasColumnName("DFlag");

                entity.Property(e => e.MakerName)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Rdate)
                    .HasColumnType("datetime")
                    .HasColumnName("RDate");

                entity.Property(e => e.Udate)
                    .HasColumnType("datetime")
                    .HasColumnName("UDate");
            });

            modelBuilder.Entity<MModel>(entity =>
            {
                entity.HasKey(e => new { e.MakerId, e.ModelId });

                entity.ToTable("m_Model");

                entity.Property(e => e.Dflag).HasColumnName("DFlag");

                entity.Property(e => e.ModelName)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Rdate)
                    .HasColumnType("datetime")
                    .HasColumnName("RDate");

                entity.Property(e => e.Udate)
                    .HasColumnType("datetime")
                    .HasColumnName("UDate");
            });

            modelBuilder.Entity<MSelfInsurance>(entity =>
            {
                entity.HasKey(e => e.SelfInsuranceId);

                entity.ToTable("m_SelfInsurance");

                entity.Property(e => e.SelfInsuranceId).ValueGeneratedNever();

                entity.Property(e => e.CarType).HasColumnName("CAR_TYPE");

                entity.Property(e => e.Dflag).HasColumnName("DFlag");

                entity.Property(e => e.Rdate)
                    .HasColumnType("datetime")
                    .HasColumnName("RDate");

                entity.Property(e => e.RemainInspection).HasColumnName("REMAIN_INSPECTION");

                entity.Property(e => e.SelfInsurance).HasColumnName("SELF_INSURANCE");

                entity.Property(e => e.Udate)
                    .HasColumnType("datetime")
                    .HasColumnName("UDate");
            });

            modelBuilder.Entity<MToffice>(entity =>
            {
                entity.HasKey(e => e.TofficeId);

                entity.ToTable("mTOffice");

                entity.Property(e => e.TofficeId)
                    .ValueGeneratedNever()
                    .HasColumnName("TOfficeID");

                entity.Property(e => e.PlaceNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TofficeCode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("TOfficeCode");

                entity.Property(e => e.TofficeName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("TOfficeName");
            });

            modelBuilder.Entity<MUser>(entity =>
            {
                entity.HasKey(e => e.UserNo);

                entity.ToTable("m_User");

                entity.Property(e => e.UserNo)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Dflag).HasColumnName("DFlag");

                entity.Property(e => e.Rdate)
                    .HasColumnType("datetime")
                    .HasColumnName("RDate");

                entity.Property(e => e.Udate)
                    .HasColumnType("datetime")
                    .HasColumnName("UDate");

                entity.Property(e => e.UserAdr)
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.Property(e => e.UserNm)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserTel)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<MUserDef>(entity =>
            {
                entity.HasKey(e => e.UserNo);

                entity.ToTable("m_UserDef");

                entity.Property(e => e.UserNo)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DamageInsMonth)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Dflag).HasColumnName("DFlag");

                entity.Property(e => e.EstTanName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MemberUrl)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MemberURL");

                entity.Property(e => e.Rdate)
                    .HasColumnType("datetime")
                    .HasColumnName("RDate");

                entity.Property(e => e.SekininName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShopAdr)
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.Property(e => e.ShopNm)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ShopTel)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TaxFreeSet1K)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.TaxFreeSet1Title)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TaxFreeSet2Title)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TaxSet1Title)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TaxSet2Title)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TaxSet3Title)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Udate)
                    .HasColumnType("datetime")
                    .HasColumnName("UDate");
            });

            modelBuilder.Entity<MWeightTax>(entity =>
            {
                entity.HasKey(e => e.WeightTaxId);

                entity.ToTable("m_WeightTax");

                entity.Property(e => e.WeightTaxId).ValueGeneratedNever();

                entity.Property(e => e.CarType).HasColumnName("CAR_TYPE");

                entity.Property(e => e.CarWeightFrom).HasColumnName("CAR_WEIGHT_FROM");

                entity.Property(e => e.CarWeightTo).HasColumnName("CAR_WEIGHT_TO");

                entity.Property(e => e.Dflag).HasColumnName("DFlag");

                entity.Property(e => e.Rdate)
                    .HasColumnType("datetime")
                    .HasColumnName("RDate");

                entity.Property(e => e.Udate)
                    .HasColumnType("datetime")
                    .HasColumnName("UDate");

                entity.Property(e => e.WeightTax).HasColumnName("WEIGHT_TAX");
            });

            modelBuilder.Entity<TEstimate>(entity =>
            {
                entity.HasKey(e => new { e.EstNo, e.EstSubNo });

                entity.ToTable("t_Estimate");

                entity.Property(e => e.EstNo)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.EstSubNo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.BodyColor)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.BodyName)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.BonusFirst)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.BonusSecond)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessHis)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.CallKbn)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CarImgPath)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CarImgPath1)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CarImgPath2)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CarImgPath3)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CarImgPath4)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CarImgPath5)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CarImgPath6)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CarImgPath7)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CarImgPath8)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Case)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ChassisNo)
                    .HasMaxLength(42)
                    .IsUnicode(false);

                entity.Property(e => e.CheckCarYm)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.CustKname)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("CustKName");

                entity.Property(e => e.Dflag).HasColumnName("DFlag");

                entity.Property(e => e.DispVol)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DriveName)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Equipment)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.EstInpKbn)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.EstTanName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EstUserNo)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FirstPayMonth)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.FirstRegYm)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.FuelName)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.GradeName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastPayMonth)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.LeaseFlag)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MakerName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Mission)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ModelName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OptionName1)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.OptionName10)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.OptionName11)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.OptionName12)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.OptionName2)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.OptionName3)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.OptionName4)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.OptionName5)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.OptionName6)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.OptionName7)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.OptionName8)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.OptionName9)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Rdate)
                    .HasColumnType("datetime")
                    .HasColumnName("RDate");

                entity.Property(e => e.SekininName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShopAdr)
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.Property(e => e.ShopNm)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ShopTel)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TradeDate).HasColumnType("datetime");

                entity.Property(e => e.TradeInBodyColor)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.TradeInCarName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TradeInChassisNo)
                    .HasMaxLength(42)
                    .IsUnicode(false);

                entity.Property(e => e.TradeInCheckCarYm)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.TradeInFirstRegYm)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TradeInRegNo)
                    .HasMaxLength(22)
                    .IsUnicode(false);

                entity.Property(e => e.Udate)
                    .HasColumnType("datetime")
                    .HasColumnName("UDate");
            });

            modelBuilder.Entity<TEstimateIde>(entity =>
            {
                entity.HasKey(e => new { e.EstNo, e.EstSubNo });

                entity.ToTable("t_Estimate_Ide");

                entity.Property(e => e.EstNo)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.EstSubNo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ContractPlanId).HasColumnName("ContractPlanID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.EstUserNo)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FirstRegistration)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.InspectionExpirationDate)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.InsuranceCompanyId).HasColumnName("InsuranceCompanyID");

                entity.Property(e => e.LeaseExpirationDate)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.LeaseStartMonth)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Smasfee).HasColumnName("SMASFee");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateUser).HasMaxLength(20);
            });

            modelBuilder.Entity<TEstimateItc>(entity =>
            {
                entity.HasKey(e => new { e.EstNo, e.EstSubNo });

                entity.ToTable("t_EstimateItc");

                entity.Property(e => e.EstNo)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.EstSubNo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.BodyColor)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.BonusFirst)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.BonusSecond)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessHis)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.CallKbn)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CarImgPath)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Case)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ChassisNo)
                    .HasMaxLength(42)
                    .IsUnicode(false);

                entity.Property(e => e.CheckCarYm)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.CustKname)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("CustKName");

                entity.Property(e => e.Dflag).HasColumnName("DFlag");

                entity.Property(e => e.DispVol)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Equipment)
                    .HasMaxLength(56)
                    .IsUnicode(false);

                entity.Property(e => e.EstInpKbn)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.EstTanName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EstUserNo)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FirstPayMonth)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.FirstRegYm)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.GradeName)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.LastPayMonth)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.MakerName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Mission)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ModelName)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.OptionName1)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.OptionName2)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.OptionName3)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.OptionName4)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.OptionName5)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.OptionName6)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Rdate)
                    .HasColumnType("datetime")
                    .HasColumnName("RDate");

                entity.Property(e => e.ShopAdr)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ShopNm)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ShopTel)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TradeDate).HasColumnType("datetime");

                entity.Property(e => e.TradeInBodyColor)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.TradeInCarName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TradeInChassisNo)
                    .HasMaxLength(42)
                    .IsUnicode(false);

                entity.Property(e => e.TradeInCheckCarYm)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.TradeInFirstRegYm)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TradeInRegNo)
                    .HasMaxLength(22)
                    .IsUnicode(false);

                entity.Property(e => e.Udate)
                    .HasColumnType("datetime")
                    .HasColumnName("UDate");
            });

            modelBuilder.Entity<TEstimateSub>(entity =>
            {
                entity.HasKey(e => new { e.EstNo, e.EstSubNo });

                entity.ToTable("t_EstimateSub");

                entity.Property(e => e.EstNo)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.EstSubNo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Aacount).HasColumnName("AACount");

                entity.Property(e => e.Aahyk)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("AAHyk");

                entity.Property(e => e.Aano)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("AANo");

                entity.Property(e => e.Aaplace)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("AAPlace");

                entity.Property(e => e.Aaprice).HasColumnName("AAPrice");

                entity.Property(e => e.Aatime)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("AATime");

                entity.Property(e => e.Aayear)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("AAYear");

                entity.Property(e => e.AutoTaxMonth)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Corner)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.CustMemo)
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.Property(e => e.DamageInsMonth)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Dflag).HasColumnName("DFlag");

                entity.Property(e => e.DispVolUnit)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.EstUserNo)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MilUnit)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Notes)
                    .HasMaxLength(600)
                    .IsUnicode(false);

                entity.Property(e => e.Rdate)
                    .HasColumnType("datetime")
                    .HasColumnName("RDate");

                entity.Property(e => e.SonotaTitle)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TaxFreeSet1Title)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TaxFreeSet2Title)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TaxSet1Title)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TaxSet2Title)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TaxSet3Title)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TradeInMilUnit)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TradeInUm).HasColumnName("TradeInUM");

                entity.Property(e => e.Udate)
                    .HasColumnType("datetime")
                    .HasColumnName("UDate");
            });

            modelBuilder.Entity<TTaxRatioDef>(entity =>
            {
                entity.HasKey(e => e.UserNo);

                entity.ToTable("t_TaxRatioDef");

                entity.Property(e => e.UserNo)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TaxRatioId).HasColumnName("TaxRatioID");
            });

            modelBuilder.Entity<TUseLog>(entity =>
            {
                entity.HasKey(e => e.LoginNo);

                entity.ToTable("t_UseLog");

                entity.Property(e => e.LoginNo).ValueGeneratedNever();

                entity.Property(e => e.Dflag).HasColumnName("DFlag");

                entity.Property(e => e.LoginDateTime).HasColumnType("datetime");

                entity.Property(e => e.Rdate)
                    .HasColumnType("datetime")
                    .HasColumnName("RDate");

                entity.Property(e => e.RefName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Udate)
                    .HasColumnType("datetime")
                    .HasColumnName("UDate");

                entity.Property(e => e.UserNm)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserNo)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<TUseLogItc>(entity =>
            {
                entity.HasKey(e => e.LoginNo);

                entity.ToTable("t_UseLogItc");

                entity.Property(e => e.LoginNo).ValueGeneratedNever();

                entity.Property(e => e.Dflag).HasColumnName("DFlag");

                entity.Property(e => e.LoginDateTime).HasColumnType("datetime");

                entity.Property(e => e.Rdate)
                    .HasColumnType("datetime")
                    .HasColumnName("RDate");

                entity.Property(e => e.RefName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Udate)
                    .HasColumnType("datetime")
                    .HasColumnName("UDate");

                entity.Property(e => e.UserNm)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserNo)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<TbPsinfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TB_PSINFO");

                entity.Property(e => e.CarFormFull)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CarFormNum)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CarLocation)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.ColorNameInterior)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Corner)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ExhNum)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.InspectionCode)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.InspectionDate)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.LaneName)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.RegDate)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.RuibetsuNum)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<TbRuibetsuN>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TB_RUIBETSU_N");

                entity.Property(e => e.ClassNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DispVol)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DriveTypeCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.FlgOptAbg)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("FlgOptABG");

                entity.Property(e => e.FlgOptAbs)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("FlgOptABS");

                entity.Property(e => e.FlgOptAw)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("FlgOptAW");

                entity.Property(e => e.FlgOptNav)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.FlgOptPs)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("FlgOptPS");

                entity.Property(e => e.FlgOptPw)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("FlgOptPW");

                entity.Property(e => e.FlgOptSht)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("FlgOptSHT");

                entity.Property(e => e.FlgOptSrf)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("FlgOptSRF");

                entity.Property(e => e.FlgOptTv)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("FlgOptTV");

                entity.Property(e => e.FuelType)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.GradeName)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Made)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.MakerId).HasColumnName("MakerID");

                entity.Property(e => e.MakerName)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Mission)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ModelId).HasColumnName("ModelID");

                entity.Property(e => e.ModelName)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.RegularCase)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SetNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ShiftId).HasColumnName("ShiftID");
            });

            modelBuilder.Entity<TbSy>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TB_SYS");

                entity.Property(e => e.Aacount).HasColumnName("AACount");

                entity.Property(e => e.Aadate)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("AADate")
                    .IsFixedLength();

                entity.Property(e => e.Aadigit).HasColumnName("AADigit");

                entity.Property(e => e.Aaname)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("AAName");

                entity.Property(e => e.AanameEng)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("AAName_Eng");

                entity.Property(e => e.Aaparam).HasColumnName("AAParam");

                entity.Property(e => e.Aaplace)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("AAPlace");

                entity.Property(e => e.AaplaceEng)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("AAPlace_Eng");

                entity.Property(e => e.AsmemberNum)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ASMemberNum")
                    .IsFixedLength();

                entity.Property(e => e.BidPos)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.CarPicExtension)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Corner)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ExhPos)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.OhpExtension)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.RealBidPos)
                    .HasMaxLength(8)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TbSysExh>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TB_SYS_EXH");

                entity.Property(e => e.Corner)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ExhFrom)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ExhTo)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<WAsMember>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("w_AsMember");

                entity.Property(e => e.UserAdr)
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.Property(e => e.UserNm)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserNo)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UserTel)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
