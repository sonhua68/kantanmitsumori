namespace KantanMitsumori.Entity.ASESTEntities
{
    public partial class TbSy
    {
        public string? Corner { get; set; } = null!;
        public short? Aacount { get; set; }
        public string Aadate { get; set; } = null!;
        public byte BidFlag { get; set; }
        public string AsmemberNum { get; set; } = null!;
        public string ExhPos { get; set; } = null!;
        public string BidPos { get; set; } = null!;
        public string RealBidPos { get; set; } = null!;
        public string Aaplace { get; set; } = null!;
        public string Aaname { get; set; } = null!;
        public int BidCharge { get; set; }
        public string AaplaceEng { get; set; } = null!;
        public string AanameEng { get; set; } = null!;
        public short CarPic { get; set; }
        public string CarPicExtension { get; set; } = null!;
        public string OhpExtension { get; set; } = null!;
        public short Aadigit { get; set; }
        public short Aaparam { get; set; }
        public short CornerType { get; set; }
        public short CornerAttr { get; set; }
        public short RealType { get; set; }
    }
}
