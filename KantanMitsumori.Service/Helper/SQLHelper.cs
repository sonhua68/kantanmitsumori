using KantanMitsumori.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Service.Helper
{

    public class SQLHelper
    {

        [Obsolete("GetListTB_RUIBETSU_N is deprecated, please use GetListTB_RUIBETSU_N instead.")]
        public static string GetListTB_RUIBETSU_N(RequestSelGrd requestSel)
        {
            return @"SELECT DISTINCT 
                    MakerId,
                    ModelName,
                    GradeNameOrd = 
	                    CASE GradeName 
	                    WHEN '' THEN 0 WHEN '不明' THEN 0 ELSE 1 END,
                    GradeName = 
	                    CASE GradeName WHEN '不明' THEN '' ELSE GradeName END, 
                    RegularCaseOrd = 
	                    CASE RegularCase WHEN '' THEN 0 WHEN '不明' THEN 0 ELSE 1 END, 
                    RegularCase = 
	                    CASE RegularCase WHEN '不明' THEN '' ELSE RegularCase END, 
                    DispVolOrd = CASE DispVol WHEN '' THEN 0 WHEN '不明' THEN 0 ELSE 1 END, 
                    DispVol = CASE DispVol WHEN '不明' THEN '' ELSE DispVol END, Shift = '',
                    DriveTypeCodeOrd = CASE upper(DriveTypeCode) WHEN '' THEN 0 WHEN '不明' THEN 0 WHEN '4WD' THEN 1 ELSE 0 END,
                    DriveTypeCode = CASE upper(DriveTypeCode) WHEN '不明' THEN '' WHEN '4WD' THEN '4WD' ELSE ''
                    END FROM TB_RUIBETSU_N WHERE 1 = 1 AND MakerId = @MakerId AND ModelName = @ModelName";
                   ;
        }

        public static string GetListTB_RUIBETSU_NEW(RequestSelGrd requestSel)
        {
            return @"SELECT DISTINCT 
                    MakerId,
                    ModelName,
					MakerName,
                    GradeNameOrd = 
	                    CASE GradeName 
	                    WHEN '' THEN 0 WHEN '不明' THEN 0 ELSE 1 END,
                    GradeName = 
	                    CASE GradeName WHEN '不明' THEN '' ELSE GradeName END, 
                    RegularCaseOrd = 
	                    CASE RegularCase WHEN '' THEN 0 WHEN '不明' THEN 0 ELSE 1 END, 
                    RegularCase = 
	                    CASE RegularCase WHEN '不明' THEN '' ELSE RegularCase END, 
                    DispVolOrd = CASE DispVol WHEN '' THEN 0 WHEN '不明' THEN 0 ELSE 1 END, 
                    DispVol = CASE DispVol WHEN '不明' THEN '' ELSE DispVol END, Shift = '',
                    DriveTypeCodeOrd = CASE upper(DriveTypeCode) WHEN '' THEN 0 WHEN '不明' THEN 0 WHEN '4WD' THEN 1 ELSE 0 END,
                    DriveTypeCode = CASE upper(DriveTypeCode) WHEN '不明' THEN '' WHEN '4WD' THEN '4WD' ELSE ''
                    END FROM TB_RUIBETSU_N WHERE 1 = 1 AND MakerId = '" + requestSel.sesMakID + "' AND ModelName = '" + requestSel.sesCarNM + "'  ";
        }

    }
}

