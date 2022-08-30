using KantanMitsumori.Model;
using KantanMitsumori.Model.Response.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.IService
{
    public interface IReportService
    {
        ResponseBase<ReportFileModel> GetArticleSubReport();
        ResponseBase<ReportFileModel> GetMemoSubReport();
    }
}
