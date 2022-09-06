using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Model.Response.Report
{
    public class ReportFileModel
    {
        public string Name { get; set; } = "result.pdf";
        public byte[] Data { get; set; } = new byte[0];
        public string ContentType { get; set; } = "application/pdf";

        public ReportFileModel(byte[] data)
        {
            Data = data;
        }

    }

}
