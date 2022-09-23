using KantanMitsumori.Helper.CommonFuncs;
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

        public static string GetListSerEst(RequestSerEst requestSerEst)
        {
            string sesMode = string.IsNullOrEmpty(requestSerEst.sesMode) ? "0" : requestSerEst.sesMode;
            string SQL = "SELECT est.EstNo + '-' + est.EstSubNo as EstNo,convert(char,TradeDate,111) as TradeDate,ISNULL(CustKName, '') AS CustKName," +
                "isnull(MakerName,'') + ' ' + isnull(ModelName,'')  + ' ' + isnull(GradeName,'') + '<br />' + isnull(ChassisNo,'') AS CarName " +
                "from t_Estimate est left join t_EstimateSub sub on est.EstNo = sub.EstNo AND est.EstSubNo = sub.EstSubNo " +
                " where est.EstUserNo ='" + requestSerEst.EstUserNo + "' and sub.Mode = '" + sesMode + "'";
            if (!string.IsNullOrEmpty(requestSerEst.EstNo))
            {
                SQL += " and est.EstNo like '" + requestSerEst.EstNo + "%'";
            }
            if (!string.IsNullOrEmpty(requestSerEst.EstNo) && !string.IsNullOrEmpty(requestSerEst.EstSubNo))
            {
                SQL += " and est.EstSubNo= '" + requestSerEst.EstSubNo + "'";
            }
            string date = requestSerEst.ddlToSelectY + "/" + CommonFunction.DateFormat(requestSerEst.ddlToSelectM!)+ "/" + requestSerEst.ddlToSelectD;
            var newDate = DateTime.Parse(date);
            newDate = newDate.AddDays(1);
            string toY = newDate.Year.ToString();
            string toM = newDate.Month.ToString();
            string toD = newDate.Day.ToString();
            string toDate = toY + "/" + CommonFunction.DateFormat(toM!) + "/" + toD;
            string formDate = requestSerEst.ddlFromSelectY + "/" + CommonFunction.DateFormat(requestSerEst.ddlFromSelectM!) + "/" + requestSerEst.ddlFromSelectD;
            //
            SQL += " and est.RDate >= '" + formDate + "'";
            SQL += " and est.RDate < '" + toDate + "'"; 
            if (!string.IsNullOrEmpty(requestSerEst.CustKanaName))
            {
                SQL += " and CustKName like '%" + requestSerEst.CustKanaName + "%'";
            }
            if (!string.IsNullOrEmpty(requestSerEst.ddlMaker))
            {
                SQL += " and MakerName = '" + requestSerEst.ddlMaker + "'";
            }
            if (!string.IsNullOrEmpty(requestSerEst.ddlModel))
            {
                SQL += " and ModelName = '" + requestSerEst.ddlModel + "'";
            }
            if (!string.IsNullOrEmpty(requestSerEst.ChassisNo))
            {
                SQL += " and ChassisNo like '%" + requestSerEst.ChassisNo + "%'";
            }
            SQL += " and est.DFlag = 0";
            return SQL;

        }

    }
}

