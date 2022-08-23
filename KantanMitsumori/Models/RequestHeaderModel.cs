using KantanMitsumori.Models;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Model.Request
{
    public class RequestHeaderModel
    {
        [FromQuery(Name = "IsInputBack")]
        public string? IsInputBack { get; set; }

        [FromQuery(Name = "Sel")]
        public string? Sel { get; set; }

        [FromQuery(Name = "Mode")]
        public string? Mode { get; set; }

        [FromQuery(Name = "PriDisp")]
        public string? PriDisp { get; set; }

        [FromQuery(Name = "leaseFlag")]
        public string? leaseFlag { get; set; }

        [FromQuery(Name = "cot")]
        public string? cot { get; set; }

        [FromQuery(Name = "cna")]
        public string? cna { get; set; }

        [FromQuery(Name = "mem")]
        public string? mem { get; set; }

        [FromQuery(Name = "exh")]
        public string? exh { get; set; }

        [FromQuery(Name = "aan")]
        public string? aan { get; set; }

        [FromQuery(Name = "cor")]
        public string? cor { get; set; }

        [FromQuery(Name = "mak")]
        public string? mak { get; set; }

        [FromQuery(Name = "gra")]
        public string? gra { get; set; }

        [FromQuery(Name = "for")]
        public string? carCase { get; set; }

        [FromQuery(Name = "pla")]
        public string? pla { get; set; }

        [FromQuery(Name = "ins")]
        public string? ins { get; set; }

        [FromQuery(Name = "mil")]
        public string? mil { get; set; }

        [FromQuery(Name = "milUnit")]
        public string? milUnit { get; set; }

        [FromQuery(Name = "vol")]
        public string? vol { get; set; }

        [FromQuery(Name = "volUnit")]
        public string? volUnit { get; set; }

        [FromQuery(Name = "shi")]
        public string? shi { get; set; }

        [FromQuery(Name = "his")]
        public string? his { get; set; }

        [FromQuery(Name = "FuelName")]
        public string? FuelName { get; set; }

        [FromQuery(Name = "DriveName")]
        public string? DriveName { get; set; }

        [FromQuery(Name = "CarDoors")]
        public string? CarDoors { get; set; }

        [FromQuery(Name = "BodyName")]
        public string? BodyName { get; set; }

        [FromQuery(Name = "Capacity")]
        public string? Capacity { get; set; }

        [FromQuery(Name = "equ")]
        public string? equ { get; set; }

        [FromQuery(Name = "col")]
        public string? col { get; set; }

        [FromQuery(Name = "img")]
        public string? img { get; set; }

        [FromQuery(Name = "fex")]
        public string? fex { get; set; }

        [FromQuery(Name = "pri")]
        public string? pri { get; set; }

        [FromQuery(Name = "fee")]
        public string? fee { get; set; }

        [FromQuery(Name = "tra")]
        public string? tra { get; set; }

        [FromQuery(Name = "poi")]
        public string? poi { get; set; }

        [FromQuery(Name = "lim")]
        public string? lim { get; set; }

        [FromQuery(Name = "nonTax")]
        public string? nonTax { get; set; }

        public FormEstMainModel? formEstMainModel { get; set; }
    }
}
