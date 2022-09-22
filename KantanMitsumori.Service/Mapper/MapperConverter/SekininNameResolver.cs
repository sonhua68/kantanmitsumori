using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    public class SekininNameResolver : IValueResolver<TEstimate, EstimateReportModel, string>
    {
        public string Resolve(TEstimate source, EstimateReportModel destination, string destMember, ResolutionContext context)
        {
            try
            {
                if (string.IsNullOrEmpty(source.SekininName))
                    return "責任者 :";
                if (source.SekininName.Length > 25)
                    return $"責任者 : {source.SekininName}".Substring(0, 25);
                return $"責任者 : {source.SekininName}";
            }
            catch
            {
                return "";
            }
        }
        private string GetEstimateTanto(TEstimate estEntity)
        {
            var tanto = "";
            var sekinin = "";
            var tel = "";
            if (!string.IsNullOrWhiteSpace(estEntity.EstTanName))
                tanto = $"担当 : {estEntity.EstTanName}　　";
            if (!string.IsNullOrWhiteSpace(estEntity.SekininName))
                tanto = $"責任者 : {estEntity.SekininName}　　";
            if (!string.IsNullOrWhiteSpace(estEntity.ShopTel))
                tanto = $"TEL : {estEntity.ShopTel}";
            if(tanto.Length + sekinin.Length + tel.Length > 42)
                return $"{tanto}{sekinin}\r\n{tel}";
            return $"{tanto}{sekinin}{tel}";
        }
    }
}
