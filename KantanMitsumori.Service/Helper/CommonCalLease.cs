using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Model.Request;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Service.Helper
{
    internal class CommonCalLease
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWorkIDE _unitOfWorkIde;
    
        private int carType = 0;
        private string firstReg = "";
        private int contractTime = 0;
        private string leaseSttMonth = "";
        private string leaseExpirationDate = "";
        private string expiresDate = "";
        private int insurExpanded = 0;
        private double consumptionTax = 0;
        private int firstTerm = 0;
        private int afterSecondTerm = 0;
        private int insuranceCompany = 0;
        private int contractPlan = 0;
        private int leasePeriod = 0;
        private double promotion = 0.0;
        private int pricePropertyFee1 = 0;
        private int pricePropertyFee2 = 0;
        private int pricePropertyFee3 = 0;
        private int pricePropertyFee4 = 0;
        private string isShowLogUI = CommonSettings.IsShowLogUI;
        public List<string> _lstWriteLog = new List<string>();
        public readonly RequestInpLeaseCalc _requestInCalc;
        public CommonCalLease(ILogger logger, IUnitOfWorkIDE unitOfWorkIde, List<string> lstWriteLog, RequestInpLeaseCalc requestInCalc)
        {
            _logger = logger;
            _unitOfWorkIde = unitOfWorkIde;
            _lstWriteLog = lstWriteLog;
            _requestInCalc = requestInCalc;
        }
        /// <summary>
        /// 4-1 file doc  lay thue tieu thu  
        /// 消費税を取得する   
        /// </summary>
        /// <returns></returns>
        public double GetConsumptionTax()
        {
            var dt = _unitOfWorkIde.ConsumptionTaxs.GetAll().ToList();
            if (dt.Count > 0)
            {
                consumptionTax = dt.FirstOrDefault()!.ConsumptionTax;

            }
            _logger.LogInformation("4-1 ConsumptionTax:={0}", consumptionTax);
            return consumptionTax;
        }
        /// <summary>
        /// 4-2  file doc   tinh gia xe tu du lieu bao gia
        /// ?見積データから車両価格を算出する    
        /// </summary>
        /// <param name="salesSum"></param>
        /// <param name="taxInsAll"></param>
        /// <param name="taxFreeAll"></param>
        /// <param name="consumptionTax"></param>
        /// <returns></returns>
        public decimal GetPrice(int? salesSum, int? taxInsAll, int? taxFreeAll)
        {
            decimal dPrice = (decimal)((salesSum! - taxInsAll! - taxFreeAll!) / (1 + consumptionTax) + taxInsAll! + taxFreeAll!);
            _logger.LogInformation("SalesSum :={0}", salesSum);
            _logger.LogInformation("TaxInsAll :={0}", taxInsAll);
            _logger.LogInformation("TaxFreeAll :={0}", taxFreeAll);
            _logger.LogInformation("4-2 Price = (SalesSum - TaxInsAll - TaxFreeAll) / (1 + ConsumptionTax) + TaxInsAll + TaxFreeAll :={0}", dPrice);
            return dPrice;
        }

        /// <summary>
        /// 4-3-1 file doc  lay thue xe
        /// 自動車税を取得する   
        /// </summary>
        /// <param name="firstRegistrationDateFrom"></param>
        /// <param name="firstRegistrationDateTo"></param>
        /// <param name="isElectricCar"></param>
        /// <param name="displacementFrom"></param>
        /// <param name="displacementTo"></param>
        /// <returns></returns>
        public decimal GetPriceTax(
            int? firstRegistrationDateFrom, int? firstRegistrationDateTo,
            int? isElectricCar, int? displacementFrom, int? displacementTo)
        {
            decimal monthlyPrice = 0;
            var dt = _unitOfWorkIde.CarTaxs.Query(n => n.CarType == carType &&

            n.FirstRegistrationDateFrom <= firstRegistrationDateFrom &&
            n.FirstRegistrationDateTo >= firstRegistrationDateTo &&
            n.IsElectricCar == isElectricCar &&
            n.DisplacementFrom <= displacementFrom
            && n.DisplacementTo >= displacementTo &&
            n.ElapsedYearsFrom <= 0
            && n.ElapsedYearsTo >= 12);
            monthlyPrice = dt.FirstOrDefault()!.MonthlyPrice;
            _logger.LogInformation("4-3-1 PriceTax :={0}", monthlyPrice);
            return monthlyPrice;
        }
        /// <summary>
        /// * 4-3-2 file doc lay thue gia tang theo dung tich dong co
        /// 検索条件は以下の通り。        
        /// </summary> 
        /// <param name="firstRegistrationDateFrom"></param>
        /// <param name="firstRegistrationDateTo"></param>
        /// <param name="isElectricCar"></param>
        /// <param name="displacementFrom"></param>
        /// <param name="displacementTo"></param>
        /// <returns></returns>
        public decimal GetTaxCollectionIncrease(
            int firstRegistrationDateFrom, int firstRegistrationDateTo,
            int isElectricCar, int displacementFrom, int displacementTo)
        {
            decimal monthlyPrice = 0;
            int elapsedYearsFrom = 0;
            int elapsedYearsTo = 99;
            if (isElectricCar != 1)
            {
                elapsedYearsFrom = 13;
            }
            var dt = _unitOfWorkIde.CarTaxs.Query(n => n.CarType == carType && n.FirstRegistrationDateFrom <= firstRegistrationDateFrom &&
            n.FirstRegistrationDateTo >= firstRegistrationDateTo && n.IsElectricCar == isElectricCar &&
            n.DisplacementFrom <= displacementFrom && n.DisplacementTo >= displacementTo &&
            n.ElapsedYearsFrom <= elapsedYearsFrom && n.ElapsedYearsTo >= elapsedYearsTo);
            monthlyPrice = dt.FirstOrDefault()!.MonthlyPrice;
            _logger.LogInformation("4-3-2 PriceTax :={0}", monthlyPrice);
            return monthlyPrice;
        }
        /// <summary>
        ///  4-3-3  tinh tien thue cua xe trong thoi han 
        ///   期間中自動車計算  期間中自動車税金計算 156 = 3 Year
        /// </summary>
        /// <param name="carType"></param>
        /// <param name="autoTax"></param>
        /// <param name="dispVolUnit"></param>
        /// <param name="chkElectricCar"></param>
        /// <param name="dispVol"></param>
        /// <returns></returns>
        public decimal GetVehicleTaxWithinTheTerm(int carType, int autoTax, string dispVolUnit,
            int electricCar, int dispVol)
        {
         //var d =   _requestInCalc.FirstReg;
         //   decimal vehicleTaxPrice = 0;
         //   int displacementFromAndTo = 0;        
         //   decimal priceMonth = 0;
         //   bool ischeck = dispVolUnit != "cc" && _requestInCalc.ElectricCar == 1;
         //   displacementFromAndTo = ischeck == false ? dispVol : displacementFromAndTo;          
         //   var priceTax = GetPriceTax(Convert.ToInt32(FirstReg), Convert.ToInt32(FirstReg)), electricCar, displacementFromAndTo, displacementFromAndTo);
         //   string endLeaseDate = leaseExpirationDate; //Ngay het han hop dong thue
         //   string endDate = CheckYear(156);// 156 = 13Year
         //   if (endDate == endLeaseDate) //< 13Year 
         //   {
         //       priceMonth = priceTax * contractTime;
         //       _logger.LogInformation("4-3-3 PriceMonth  < 13Year :={0}", priceMonth);
         //   }
         //   else //'> 13Year
         //   {
         //       var taxCollection = GetTaxCollectionIncrease(Convert.ToInt32(firstReg), Convert.ToInt32(firstReg), electricCar, dispVol, dispVol);
         //       priceMonth = (priceTax * GetMonthLease(1) + (taxCollection + GetMonthLease(0)));
         //       _logger.LogInformation("4-3-3 PriceMonth  > 13Year :={0}", priceMonth);

         //   }
         //   vehicleTaxPrice = priceMonth - autoTax;
         //   _logger.LogInformation("4-3-3 PriceVehicleTaxWithinTheTerm(PriceMonth - AutoTax) :={0}", priceMonth);
           return 0;

        }
        /// <summary>
        /// GetPriceinsurance
        /// 4-4  tinh tien bao hiem tu nguyen 1 lan
        /// 期間中自賠責
        /// </summary>      
        /// <returns></returns>
        public decimal GetPriceinsurance()
        {
            decimal priceinsurance = 0;
            decimal insuranceFee = 0;
            bool isFirstTime = true;
            DateTime _expiresDate = DateTime.Parse(expiresDate);
            DateTime _firstReg = DateTime.Parse(firstReg);
            DateTime _leaseSttMonth = DateTime.Parse(leaseSttMonth);
            DateTime _leaseExpirationDate = DateTime.Parse(leaseExpirationDate);
            if (contractPlan != 99)
            {
                insuranceFee = _unitOfWorkIde.LiabilityInsurances.Query(n => n.CarType == carType).FirstOrDefault()!.InsuranceFee;
                var registrationDate = _expiresDate.AddMonths(1);
                var startLeaseDate = _leaseSttMonth;
                var endLeaseDate = _leaseExpirationDate;
                if (_expiresDate == DateTime.MinValue)
                {
                    isFirstTime = false;
                }
                var inspectionCount = InspectionCount(registrationDate, startLeaseDate, endLeaseDate, isFirstTime);
                if ((contractTime % afterSecondTerm == 0 && inspectionCount > 0) & _expiresDate == startLeaseDate)
                {
                    inspectionCount -= 1;
                }
                _logger.LogInformation("InspectionCount :={0}", inspectionCount);
                priceinsurance = inspectionCount * insuranceFee;

            }
            _logger.LogInformation("4-4 PriceInsurance(InspectionCount * InsuranceFee) :={0}", priceinsurance);
            return priceinsurance;
        }
        /// <summary>
        /// 4-5  4-5-1 4-5-2   4-5-3  Thue trong luong
        ///  期間中重量税 - 重量税を取得する（普通） -重量税を取得する（初度登録から13年超） -重量税を取得する（初度登録から18年超）
        /// </summary>
        /// <param name="type"></param>    
        /// <returns></returns>
        public decimal GetWeighTax(int type)
        {
            decimal weighTax = 0;
            double rElapsedYearsFrom = 0;
            double rElapsedYearsTo = 0;
            if (contractPlan != 99)
            {
                if (type == 0) //case 4-5-1
                {
                    rElapsedYearsFrom = 0;
                    rElapsedYearsTo = 12;

                }
                else if (type == 1) // case 4-5-2
                {
                    rElapsedYearsFrom = 13;
                    rElapsedYearsTo = 17;

                }
                else if (type == 2) //case 4-5-3
                {
                    rElapsedYearsFrom = 18;
                    rElapsedYearsTo = 99;
                }
                weighTax = _unitOfWorkIde.WeightTaxs.Query(n => n.CarType == carType && n.ElapsedYearsFrom <= rElapsedYearsFrom
                                                         && n.ElapsedYearsTo >= rElapsedYearsTo).FirstOrDefault()!.WeightTax;

            }
            return weighTax;

        }
        /// <summary>
        ///  4-5-4 tinh thua trong luong 
        ///   重量税計算を行う
        /// </summary>
        /// <returns></returns>
        public decimal getPriceWeighTax()
        {
            decimal priceWeighTax = 0;
            var weighTax = GetWeighTax(0); // case 4-5-1
            var weighTax1 = GetWeighTax(0); // case 4-5-2
            var weighTax2 = GetWeighTax(0); // case 4-5-3
            _logger.LogInformation("4-5-1 WeighTax :={0}", weighTax);
            _logger.LogInformation("4-5-2 WeighTax :={0}", weighTax1);
            _logger.LogInformation("4-5-3 WeighTax :={0}", weighTax2);
            var inspectionCount = 0;
            var inspectionCount1 = 0;
            var inspectionCount2 = 0;
            bool isFirstTime = true;
            DateTime _expiresDate = DateTime.Parse(expiresDate);
            DateTime _firstReg = DateTime.Parse(firstReg);
            DateTime _leaseSttMonth = DateTime.Parse(leaseSttMonth);
            DateTime _leaseExpirationDate = DateTime.Parse(leaseExpirationDate);
            var registrationDate = _expiresDate.AddMonths(1);
            var startLeaseDate = _leaseSttMonth;
            var endLeaseDate = _leaseExpirationDate;
            if (_expiresDate == DateTime.MinValue)
            {
                isFirstTime = false;
            }
            // First Time 
            var sttDate = startLeaseDate;
            var endDate = DateTime.Parse(CheckYear(155));
            if (endDate < sttDate) { endDate = sttDate; };
            //Not over 13 year
            inspectionCount = InspectionCount(registrationDate, startLeaseDate, endDate, isFirstTime);
            if ((contractTime % afterSecondTerm == 0 && inspectionCount > 0) & _expiresDate == startLeaseDate)
            {
                inspectionCount -= 1;
            }
            _logger.LogInformation("InspectionCount Not over 13 year:={0}", inspectionCount);
            //Over 13 year and not over 18 year        
            var endDate1 = DateTime.Parse(CheckYear(215));
            if (endDate <= endDate1)
            {
                inspectionCount1 = InspectionCount(registrationDate, endDate, endDate1, isFirstTime);
            }
            _logger.LogInformation("InspectionCount Not over 13 year:={0}", inspectionCount);

            if (endDate1 <= endLeaseDate)
            {
                inspectionCount2 = InspectionCount(registrationDate, endDate1, endLeaseDate, isFirstTime);
            }
            priceWeighTax = (weighTax * inspectionCount) + (weighTax1 * inspectionCount1) + (weighTax2 * inspectionCount2);
            _logger.LogInformation("4-5-4 PriceWeighTax::={0}", priceWeighTax);
            return priceWeighTax;

        }
        /// <summary>
        ///  4-6 tinh chi phi khuyen mai
        ///  販売促進費計算
        /// </summary>
        /// <param name="salesSum"></param>
        /// <returns></returns>
        public double GetPricePromotional(int salesSum)
        {
            double pricePromotional = 0;
            promotion = _unitOfWorkIde.Promotions.GetAll().FirstOrDefault()!.Promotion;
            pricePromotional = salesSum * promotion * contractTime;
            _logger.LogInformation("4-6 PricePromotional:={0}", pricePromotional);
            if (pricePromotional > 100000)
            {
                pricePromotional = 100000;
            }
            else if (pricePromotional < 15000)
            {
                pricePromotional = 15000;
            }
            _logger.LogInformation("4-6 PricePromotional  > 100000= 100000 and < 15000= 15000::={0}", pricePromotional);
            return pricePromotional;
        }
        /// <summary>
        /// 4-7 tinh chi phi tai san cua Idemitsu
        ///  出光興産手数料、出光クレジット手数料、特販店手数料、SMAS手数料の合計算出
        /// </summary>
        /// <returns></returns>
        public decimal GetPropertyFeeIdemitsu()
        {
            decimal pricePropertyFeeIdemitsu = 0;
            pricePropertyFee1 = GetPropertyFee(1);
            pricePropertyFee2 = GetPropertyFee(2);
            pricePropertyFee3 = GetPropertyFee(3);
            pricePropertyFee4 = GetPropertyFee(carType + 3);

            pricePropertyFeeIdemitsu = pricePropertyFee1 + pricePropertyFee2 + pricePropertyFee3 + pricePropertyFee4;
            _logger.LogInformation("ID =1:={0}", pricePropertyFee1);
            _logger.LogInformation("ID =2:={0}", pricePropertyFee2);
            _logger.LogInformation("ID =3:={0}", pricePropertyFee3);
            _logger.LogInformation("ID =CarType+3:={0}", pricePropertyFee4);
            _logger.LogInformation("4-7 PricePropertyFeeIdemitsu:={0}", pricePropertyFeeIdemitsu);

            return pricePropertyFeeIdemitsu;
        }
        /// <summary>
        ///  4-8 tinh phi bao hanh - Guarantee Fee
        /// 保証料計算
        /// GetGuaranteeFee
        /// </summary>
        /// <returns></returns>
        public decimal GetGuaranteeFee()
        {
            decimal priceGuaranteeCharg = 0;
            var year = 0;
            if (insurExpanded == 0)
            {
                year = 2;
            }
            else if (insurExpanded == 1)
            {
                year = 1;
            }
            priceGuaranteeCharg = _unitOfWorkIde.Guarantees.Query(n => n.Years == year).FirstOrDefault()!.GuaranteeCharge;
            _logger.LogInformation("4-8 PriceGuaranteeFee:={0}", priceGuaranteeCharg);
            return priceGuaranteeCharg;

        }
        /// <summary>
        /// 4-9  lay phi doi ten 
        /// 名義変更費用
        /// </summary>
        /// <returns></returns>
        public decimal GetPriceNameChange()
        {
            decimal priceNameChange = 0;
            priceNameChange = _unitOfWorkIde.NameChanges.GetAll().FirstOrDefault()!.NameChange;
            _logger.LogInformation("4-9 PriceNameChange::={0}", priceNameChange);
            return priceNameChange;
        }
        /// <summary>
        /// 4-10  tinh phi bao tri
        /// メンテナンス料計算
        /// </summary>
        /// <returns></returns>
        public decimal getPriceMantance()
        {
            decimal priceMantance = 0;

            if (contractPlan != 99)
            {
                bool isFirstTime = true;
                DateTime _expiresDate = DateTime.Parse(expiresDate);
                DateTime _firstReg = DateTime.Parse(firstReg);
                DateTime _leaseSttMonth = DateTime.Parse(leaseSttMonth);
                DateTime _leaseExpirationDate = DateTime.Parse(leaseExpirationDate);
                var registrationDate = _expiresDate.AddMonths(1);
                var startLeaseDate = _leaseSttMonth;
                var endLeaseDate = _leaseExpirationDate;
                if (_expiresDate == DateTime.MinValue)
                {
                    isFirstTime = false;
                }
                var inspectionCount = InspectionCount(registrationDate, startLeaseDate, endLeaseDate, isFirstTime);
                if ((contractTime % afterSecondTerm == 0 && inspectionCount > 0) & _expiresDate == startLeaseDate)
                {
                    inspectionCount -= 1;
                }
                _logger.LogInformation("InspectionCount:={0}", inspectionCount);
                var isBeforeFirstInspection = carType == 3 ? 9 : IsBeforeFirstInspection();
                var myMaintenancePrice = _unitOfWorkIde.Maintenances.Query(n => n.CarType == carType
                && n.LeasePeriod == contractTime &&
                n.BeforeFirstInspection == isBeforeFirstInspection
                && n.InspectionCount == inspectionCount).FirstOrDefault()!.MyMaintenancePrice;
                priceMantance = myMaintenancePrice * contractTime;
                _logger.LogInformation("4-10 PriceMaintenance:={0}", priceMantance);
            }
            return priceMantance;
        }
        /// <summary>
        ///  4-11  tinh phi lai suat ap dung
        ///  適用金利率取得する
        /// </summary>
        /// <returns></returns>
        public double GetInterest()
        {
            double interest = 0;
            interest = _unitOfWorkIde.Interests.Query(n => n.LeasePeriodFrom <= contractTime
            && n.LeasePeriodTo >= contractTime).FirstOrDefault()!.Interest;
            _logger.LogInformation("4-11  PriceInterest:={0}", interest);
            return interest;
        }
        #region Fuc
        /// <summary>
        /// CheckYear
        /// </summary>
        /// <param name="imonth"></param>
        /// <param name="firstReg"></param>
        /// <param name="leaseExpirationDate"></param>
        /// <returns></returns>
        public string CheckYear(int imonth)
        {
            string date = "";
            DateTime regDay = DateTime.Parse(firstReg);
            DateTime ExpCurrY = DateTime.Parse(leaseExpirationDate);
            var regCurrY = regDay.AddMonths(imonth);
            if (ExpCurrY > regCurrY)
            {
                date = regCurrY.ToString("yyyyMMdd");
            }
            else
            {
                date = ExpCurrY.ToString("yyyyMMdd");
            }
            return date;
        }
        /// <summary>
        /// getMonthLease
        /// </summary>
        /// <param name="type"></param>
        /// <param name="firstReg"></param>
        /// <param name="leaseExpirationDate"></param>
        /// <param name="leaseSttMonth"></param>
        /// <returns></returns>
        public int GetMonthLease(int type)
        {
            int month = 0;
            DateTime regDay = DateTime.Parse(firstReg);
            var over13Year = regDay.AddMonths(156);
            var expCurrSttY = DateTime.Parse(leaseSttMonth);
            var expCurrLeaseExpY = DateTime.Parse(leaseExpirationDate);
            if (type == 0)
            {
                if (over13Year < expCurrSttY)
                {
                    month = Convert.ToInt32(CommonFunction.DateDiff(IntervalEnum.Months, expCurrSttY, expCurrLeaseExpY));
                }
                else
                {
                    month = Convert.ToInt32(CommonFunction.DateDiff(IntervalEnum.Months, over13Year, expCurrLeaseExpY));
                }
                _logger.LogInformation("getMonthLease  > Over13Year :={0}", month);
            }
            else if (type == 1)
            {
                if (over13Year < expCurrSttY)
                {
                    month = 0;
                }
                else
                {
                    month = Convert.ToInt32(CommonFunction.DateDiff(IntervalEnum.Months, expCurrSttY, over13Year));
                }
                _logger.LogInformation("getMonthLease  < Over13Year :={0}", month);
            }
            return month;
        }
        /// <summary>
        /// InspectionCount
        /// </summary>
        /// <param name="currentExpiresDate"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="firstTerm"></param>
        /// <param name="afterSecondTerm"></param>
        /// <param name="isFirstTimes"></param>
        /// <returns></returns>
        public int InspectionCount(DateTime currentExpiresDate, DateTime startDate, DateTime endDate, bool isFirstTimes)
        {
            int inspectionCount = 0;
            DateTime currentCheckDate = currentExpiresDate;
            if (isFirstTimes)
            {
                currentCheckDate = currentCheckDate.AddMonths(firstTerm);

                if (currentCheckDate > startDate && currentCheckDate < endDate)
                {
                    currentExpiresDate = currentCheckDate;
                    inspectionCount += 1;
                }
            }
            {
                while (currentCheckDate < endDate)
                {
                    if (isFirstTimes)
                    {
                        currentCheckDate = currentCheckDate.AddMonths(afterSecondTerm);
                    }
                    if (currentCheckDate > startDate && currentCheckDate < endDate)
                    {
                        currentExpiresDate = currentCheckDate;
                        inspectionCount += 1;

                    }
                    if (!isFirstTimes)
                    {
                        currentCheckDate = currentCheckDate.AddMonths(afterSecondTerm);
                    }
                }
            }
            return inspectionCount;
        }
        /// <summary>
        /// IsBeforeFirstInspection
        /// </summary>      
        /// <returns></returns>

        public int IsBeforeFirstInspection()
        {
            DateTime startP = DateTime.Parse(firstReg);
            DateTime leaseSttM = DateTime.Parse(leaseSttMonth);
            var currY = startP.AddMonths(firstTerm);
            if (currY <= leaseSttM)
            {
                return 0;
            }
            else
            {
                return 1;
            }

        }

        /// <summary>
        /// GetPropertyFee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetPropertyFee(int id)
        {
            int pricePropertyFee = 0;

            var commissions = _unitOfWorkIde.Commissions.Query(n => n.Id == id).FirstOrDefault();
            if (commissions!.IsMonthly == 1)
            {
                pricePropertyFee = commissions!.Fee * contractTime;
            }
            else
            {
                pricePropertyFee = commissions!.Fee;
            }
            return pricePropertyFee;


        }
        #endregion Fuc
    }


}
