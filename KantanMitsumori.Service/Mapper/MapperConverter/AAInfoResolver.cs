using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Model.Response.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    public class AAInfoResolver : IValueResolver<TEstimate, EstimateReportModel, string>
    {     
        public string Resolve(TEstimate source, EstimateReportModel destination, string destMember, ResolutionContext context)
        {
            try
            {
                var estSubEntity = context.Items["estSubEntity"] as TEstimateSub;
                var sysEntity = context.Items["sysEntity"] as TbSy;
                if (estSubEntity == null || sysEntity == null)
                    return "";
                if (estSubEntity.Mode.HasValue && estSubEntity.Mode.Value == 1)
                {
                    if (sysEntity.CornerType == 1)
                        return $"お問合せ番号{sysEntity.CornerType}00-{sysEntity.Aacount:00000}-{estSubEntity.Aano:00000}";
                    else
                        return $"お問合せ番号{sysEntity.CornerType}00-{estSubEntity.Corner}{estSubEntity.Aano}";
                }
                return "";
            }
            catch
            {
                return "";
            }
        }
    }
}
