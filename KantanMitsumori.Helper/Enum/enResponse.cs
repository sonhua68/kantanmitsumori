namespace KantanMitsumori.Helper.Enum
{
    public enum enResponse
    {
        isSuccess = 0,
        isError = -1,
        isLogicError = -2,
    }

    public enum enTypeButton
    {
        isNextGrade = 1,
        isChkModel = 2,
    }
    public enum enSortCar
    {
        isDefault = 0,
        isGradeName = 10,
        isGradeNameDesc = 11,
        isRegularCase = 20,
        isRegularCaseDesc = 21,
        isDispVol = 30,
        isDispVolDesc = 31,
        isDriveTypeCode = 40,
        isDriveTypeCodeDesc = 41,
    }
    public enum enSortCarEst
    {
        isDefault = 0,
        isEstNo = 10,
        isEstNoDesc = 11,
        isTradeDate = 20,
        isTradeDateDesc = 21,
        isCustKName = 30,
        isCustKNameDesc = 31,
        isCarName = 40,
        isCarNameDesc = 41,

    }
}
