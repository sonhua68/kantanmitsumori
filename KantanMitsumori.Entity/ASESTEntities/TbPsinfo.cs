using System;
using System.Collections.Generic;

namespace KantanMitsumori.Entity.ASESTEntities
{
    public partial class TbPsinfo
    {
        public string Corner { get; set; } = null!;
        public string ExhNum { get; set; } = null!;
        public string? CarFormFull { get; set; }
        public byte? CarVolumeFlag { get; set; }
        public byte? RepairFlag { get; set; }
        public string? CarFormNum { get; set; }
        public string? RuibetsuNum { get; set; }
        public string? ColorNameInterior { get; set; }
        public byte? RecycleFlag { get; set; }
        public int? RecyclingCharge { get; set; }
        public int? SerialNum { get; set; }
        public short? SerialNumBranch { get; set; }
        public short? CarLength { get; set; }
        public short? CarWidth { get; set; }
        public short? CarHeight { get; set; }
        public short? CarCapacity2 { get; set; }
        public string? LaneName { get; set; }
        public string? InspectionDate { get; set; }
        public string? InspectionCode { get; set; }
        public string? CarLocation { get; set; }
        public string? RegDate { get; set; }
    }
}
