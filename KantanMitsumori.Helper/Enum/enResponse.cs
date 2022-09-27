using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Helper.Enum
{
    public enum enResponse
	{
        isSuccess = 0,
        isError = -1,      
    }

    public enum enTypeButton
    {
        isNextGrade = 1,
        isChkModel = 2,
    }
    public enum enSortCar
    {
        isDefault = 0,
        isGradeName = 1,
        isRegularCase = 2,
        isDispVol = 3,
        isDriveTypeCode = 4,
        isDispVolDesc = 5,
        isDriveTypeCodeDesc = 6,
        isRegularCaseDesc = 7,
    }
    public enum enSortCarEst
    {
        isDefault = 0,
        isEstNo = 1,
        isTradeDate = 2,
        isCustKName = 3,     
        isCarName = 4,
        isCustKNameDesc = 5,
        isCarNameDesc = 6,
        isTradeDateDesc = 7,
    }
}
