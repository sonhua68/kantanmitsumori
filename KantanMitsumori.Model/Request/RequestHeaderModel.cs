using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Model.Request
{
    public class RequestHeaderModel
    {
        [FromForm(Name = "Mode")]
        public string Mode { get; set; }

        [FromForm(Name = "PriDisp")]
        public string PriDisp { get; set; }

        [FromForm(Name = "leaseFlag")]
        public string leaseFlag { get; set; }

        [FromForm(Name = "cot")]
        public string cot { get; set; }

        [FromForm(Name = "cna")]
        public string cna { get; set; }

        [FromForm(Name = "mem")]
        public string mem { get; set; }

        [FromForm(Name = "exh")]
        public string exh { get; set; }

        [FromForm(Name = "aan")]
        public string aan { get; set; }

        [FromForm(Name = "cor")]
        public string cor { get; set; }

        [FromForm(Name = "mak")]
        public string mak { get; set; }

        [FromForm(Name = "gra")]
        public string gra { get; set; }

        [FromForm(Name = "for")]
        public string carCase { get; set; }

        [FromForm(Name = "pla")]
        public string pla { get; set; }

        [FromForm(Name = "ins")]
        public string ins { get; set; }

        [FromForm(Name = "mil")]
        public string mil { get; set; }

        [FromForm(Name = "milUnit")]
        public string milUnit { get; set; }

        [FromForm(Name = "vol")]
        public string vol { get; set; }

        [FromForm(Name = "volUnit")]
        public string volUnit { get; set; }

        [FromForm(Name = "shi")]
        public string shi { get; set; }

        [FromForm(Name = "his")]
        public string his { get; set; }

        [FromForm(Name = "FuelName")]
        public string FuelName { get; set; }

        [FromForm(Name = "DriveName")]
        public string DriveName { get; set; }

        [FromForm(Name = "CarDoors")]
        public string CarDoors { get; set; }

        [FromForm(Name = "BodyName")]
        public string BodyName { get; set; }

        [FromForm(Name = "Capacity")]
        public string Capacity { get; set; }

        [FromForm(Name = "equ")]
        public string equ { get; set; }

        [FromForm(Name = "col")]
        public string col { get; set; }

        [FromForm(Name = "img")]
        public string img { get; set; }

        [FromForm(Name = "fex")]
        public string fex { get; set; }

        [FromForm(Name = "pri")]
        public string pri { get; set; }

        [FromForm(Name = "fee")]
        public string fee { get; set; }

        [FromForm(Name = "tra")]
        public string tra { get; set; }

        [FromForm(Name = "poi")]
        public string poi { get; set; }

        [FromForm(Name = "lim")]
        public string lim { get; set; }

        [FromForm(Name = "nonTax")]
        public string nonTax { get; set; }
    }
}
