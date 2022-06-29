using System;
using System.Collections.Generic;

namespace KantanMitsumori.DataAccess
{
    public partial class MToffice
    {
        public short TofficeId { get; set; }
        public string TofficeCode { get; set; } = null!;
        public string TofficeName { get; set; } = null!;
        public string? PlaceNumber { get; set; }
        public byte Rflag { get; set; }
    }
}
