// Create date 2022/06/2022 By Hoai Phong
using GrapeCity.DataVisualization.TypeScript;
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

        public double consumptionTax = 0;
        public double promotion = 0.0;
        public int pricePropertyFee1 = 0;
        public int pricePropertyFee2 = 0;
        public int pricePropertyFee3 = 0;
        public int pricePropertyFee4 = 0;
        public int logCreditFee = 0;
        //private string isShowLogUI = CommonSettings.IsShowLogUI;
        public List<string> _lstWriteLog = new List<string>();
        private RequestInpLeaseCalc _requestInCalc;
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
        public double GetPrice(int? salesSum, int? taxInsAll, int? taxFreeAll)
        {
            double dPrice = (double)((salesSum! - taxInsAll! - taxFreeAll!) / (1 + consumptionTax) + taxInsAll! + taxFreeAll!);
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
        public double GetPriceTax(
            int? firstRegistrationDateFrom, int? firstRegistrationDateTo,
            int? isElectricCar, int? displacementFrom, int? displacementTo)
        {
            double monthlyPrice = 0;
            var dt = _unitOfWorkIde.CarTaxs.GetSingleOrDefault(n => n.CarType == _requestInCalc.CarType &&
            n.FirstRegistrationDateFrom <= firstRegistrationDateFrom &&
            n.FirstRegistrationDateTo >= firstRegistrationDateTo &&
            n.IsElectricCar == isElectricCar &&
            n.DisplacementFrom <= displacementFrom
            && n.DisplacementTo >= displacementTo &&
            n.ElapsedYearsFrom <= 0
            && n.ElapsedYearsTo >= 12);
            if (dt != null)
            {
                monthlyPrice = dt.MonthlyPrice;
            }
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
        public double GetTaxCollectionIncrease(
            int firstRegistrationDateFrom, int firstRegistrationDateTo,
            int displacementFrom, int displacementTo)
        {
            double monthlyPrice = 0;
            int elapsedYearsFrom = 0;
            int elapsedYearsTo = 99;
            if (_requestInCalc.ElectricCar != 1)
            {
                elapsedYearsFrom = 13;
            }
            var dt = _unitOfWorkIde.CarTaxs.GetSingleOrDefault(n => n.CarType == _requestInCalc.CarType && n.FirstRegistrationDateFrom <= firstRegistrationDateFrom &&
            n.FirstRegistrationDateTo >= firstRegistrationDateTo && n.IsElectricCar == _requestInCalc.ElectricCar &&
            n.DisplacementFrom <= displacementFrom && n.DisplacementTo >= displacementTo &&
            n.ElapsedYearsFrom <= elapsedYearsFrom && n.ElapsedYearsTo >= elapsedYearsTo);
            if (dt != null)
            {
                monthlyPrice = dt.MonthlyPrice; monthlyPrice = dt.MonthlyPrice;
            }
            _logger.LogInformation("4-3-2 PriceTax :={0}", monthlyPrice);
            return monthlyPrice;
        }
        /// <summary>
        ///  4-3-3  tinh tien thue cua xe trong thoi han 
        ///   期間中自動車計算  期間中自動車税金計算 156 = 3 Year
        /// </summary>
        /// <returns></returns>
        public double GetVehicleTaxWithinTheTerm(int autoTax, string dispVolUnit,
           int dispVol)
        {
            var firstReg = Convert.ToInt32(_requestInCalc.FirstReg);
            double vehicleTaxPrice = 0;
            int displacementFromAndTo = 0;
            double priceMonth = 0;
            bool ischeck = dispVolUnit != "cc" && _requestInCalc.ElectricCar == 1;
            displacementFromAndTo = ischeck == false ? dispVol : displacementFromAndTo;
            var priceTax = GetPriceTax(firstReg, firstReg, _requestInCalc.ElectricCar, displacementFromAndTo, displacementFromAndTo);
            var diffMonth = _requestInCalc.CalcDiffMonth;
            if (diffMonth == 0) //< 13Year 
            {
                priceMonth = priceTax * _requestInCalc.ContractTimes;
                _logger.LogInformation("4-3-3 PriceMonth  < 13Year :={0}", priceMonth);
            }
            else
            {
                var taxCollection = GetTaxCollectionIncrease(firstReg, firstReg, dispVol, dispVol);
                var monthsOver13Year = _requestInCalc.ContractTimes - diffMonth;
                _logger.LogInformation("getMonthLease  < Over13Year :={0}", diffMonth);
                _logger.LogInformation("getMonthLease  > Over13Year: :={0}", _requestInCalc.ContractTimes - diffMonth);
                priceMonth = (priceTax * diffMonth + (taxCollection * monthsOver13Year));
                _logger.LogInformation("4-3-3 PriceMonth  > 13Year :={0}", priceMonth);
            }
            vehicleTaxPrice = (double)(priceMonth - autoTax);
            _logger.LogInformation("4-3-3 PriceVehicleTaxWithinTheTerm(PriceMonth - AutoTax) :={0}", vehicleTaxPrice);
            return vehicleTaxPrice;

        }
        /// <summary>
        /// GetPriceinsurance
        /// 4-4  tinh tien bao hiem tu nguyen 1 lan
        /// 期間中自賠責
        /// </summary>      
        /// <returns></returns>
        public double GetPriceinsurance()
        {
            double priceInsurance = 0;
            double insuranceFee = 0;
            DateTime _expiresDate = DateTime.Parse(CommonFunction.ConvertDate(_requestInCalc.ExpiresDate!));
            DateTime _firstReg = DateTime.Parse(CommonFunction.ConvertDate(_requestInCalc.FirstReg!));
            DateTime _leaseSttMonth = DateTime.Parse(CommonFunction.ConvertDate(_requestInCalc.LeaseSttMonth!));
            DateTime _leaseExpirationDate = DateTime.Parse(CommonFunction.ConvertDate(_requestInCalc.LeaseExpirationDate!));
            if (_requestInCalc.ContractPlan != 99)
            {
                var dt = _unitOfWorkIde.LiabilityInsurances.GetSingleOrDefault(n => n.CarType == _requestInCalc.CarType);
                if (dt != null)
                {
                    insuranceFee = dt.InsuranceFee;
                }
                var registrationDate = _expiresDate.AddMonths(1);
                var startLeaseDate = _leaseSttMonth;
                var endLeaseDate = _leaseExpirationDate;
                bool isFirstTime = string.IsNullOrEmpty(_requestInCalc.ExpiresDate) == true ? true : false;
                var inspectionCount = InspectionCount(ref registrationDate, startLeaseDate, endLeaseDate, isFirstTime);
                if ((_requestInCalc.ContractTimes % _requestInCalc.AfterSecondTerm == 0 && inspectionCount > 0) & _expiresDate == startLeaseDate)
                {
                    inspectionCount -= 1;
                }
                _logger.LogInformation("InspectionCount :={0}", inspectionCount);
                _logger.LogInformation("InsuranceFee :={0}", insuranceFee);
                priceInsurance = inspectionCount * insuranceFee;

            }
            _logger.LogInformation("4-4 PriceInsurance(InspectionCount * InsuranceFee) :={0}", priceInsurance);
            return priceInsurance;
        }
        /// <summary>
        /// 4-5  4-5-1 4-5-2   4-5-3  Thue trong luong
        ///  期間中重量税 - 重量税を取得する（普通） -重量税を取得する（初度登録から13年超） -重量税を取得する（初度登録から18年超）
        /// </summary>
        /// <param name="type"></param>    
        /// <returns></returns>
        public double GetWeighTax(int type)
        {
            double weighTax = 0;
            double rElapsedYearsFrom = 0;
            double rElapsedYearsTo = 0;
            if (_requestInCalc.ContractPlan != 99)
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
                var dt = _unitOfWorkIde.WeightTaxs.GetSingleOrDefault(n => n.CarType == _requestInCalc.CarType && n.ElapsedYearsFrom <= rElapsedYearsFrom
                                                       && n.ElapsedYearsTo >= rElapsedYearsTo);
                if (dt != null)
                {
                    weighTax = dt.WeightTax;
                }
            }
            return weighTax;

        }
        /// <summary>
        ///  4-5-4 tinh thua trong luong 
        ///   重量税計算を行う
        /// </summary>
        /// <returns></returns>
        public double GetPriceWeighTax()
        {
            double priceWeighTax = 0;
            var weighTax = GetWeighTax(0); // case 4-5-1
            var weighTax1 = GetWeighTax(1); // case 4-5-2
            var weighTax2 = GetWeighTax(2); // case 4-5-3
            _logger.LogInformation("4-5-1 WeighTax :={0}", weighTax);
            _logger.LogInformation("4-5-2 WeighTax :={0}", weighTax1);
            _logger.LogInformation("4-5-3 WeighTax :={0}", weighTax2);
            var inspectionCount = 0;
            var inspectionCount1 = 0;
            var inspectionCount2 = 0;
            DateTime _expiresDate = DateTime.Parse(CommonFunction.ConvertDate(_requestInCalc.ExpiresDate!));
            DateTime _firstReg = DateTime.Parse(CommonFunction.ConvertDate(_requestInCalc.FirstReg!));
            DateTime _leaseSttMonth = DateTime.Parse(CommonFunction.ConvertDate(_requestInCalc.LeaseSttMonth!));
            DateTime _leaseExpirationDate = DateTime.Parse(CommonFunction.ConvertDate(_requestInCalc.LeaseExpirationDate!));
            var registrationDate = _expiresDate.AddMonths(1);
            var startLeaseDate = _leaseSttMonth;
            var endLeaseDate = _leaseExpirationDate;
            bool isFirstTime = (string.IsNullOrEmpty(_requestInCalc.ExpiresDate)) ? true : false;
            // First Time 
            var sttDate = startLeaseDate;
            var endDate = CheckYear(155);
            if (endDate < sttDate) { endDate = sttDate; };
            //Not over 13 year
            inspectionCount = InspectionCount(ref registrationDate, startLeaseDate, endDate, isFirstTime);
            if ((_requestInCalc.ContractTimes % _requestInCalc.AfterSecondTerm == 0 && inspectionCount > 0) & _expiresDate == startLeaseDate)
            {
                inspectionCount -= 1;
            }
            _logger.LogInformation("InspectionCount Not over 13 year:={0}", inspectionCount);
            //Over 13 year and not over 18 year        
            var endDate1 = CheckYear(215);
            if (endDate <= endDate1)
            {
                inspectionCount1 = InspectionCount(ref registrationDate, endDate, endDate1, isFirstTime);
            }
            _logger.LogInformation("InspectionCount Not over 13 year:={0}", inspectionCount);

            if (endDate1 <= endLeaseDate)
            {
                inspectionCount2 = InspectionCount(ref registrationDate, endDate1, endLeaseDate, isFirstTime);
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
            pricePromotional = salesSum * promotion * _requestInCalc.ContractTimes;
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
        public double GetPropertyFeeIdemitsu()
        {
            double pricePropertyFeeIdemitsu = 0;
            pricePropertyFee1 = GetPropertyFee(1);
            pricePropertyFee2 = GetPropertyFee(2);
            pricePropertyFee3 = GetPropertyFee(3);
            pricePropertyFee4 = GetPropertyFee(_requestInCalc.CarType + 3);
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
        public double GetGuaranteeFee()
        {
            double priceGuaranteeCharg = 0;
            var year = 0;
            if (_requestInCalc.InsurExpanded == 0)
            {
                year = 2;
            }
            else if (_requestInCalc.InsurExpanded == 1)
            {
                year = 1;
            }          
            var dt = _unitOfWorkIde.Guarantees.GetSingleOrDefault(n => n.Years == year);
            if(dt != null)
            {
                priceGuaranteeCharg = dt.GuaranteeCharge;
            }
            _logger.LogInformation("4-8 PriceGuaranteeFee:={0}", priceGuaranteeCharg);
            return priceGuaranteeCharg;

        }
        /// <summary>
        /// 4-9  lay phi doi ten 
        /// 名義変更費用
        /// </summary>
        /// <returns></returns>
        public double GetPriceNameChange()
        {
            double priceNameChange = 0;
            priceNameChange = _unitOfWorkIde.NameChanges.GetAll().FirstOrDefault()!.NameChange;
            _logger.LogInformation("4-9 PriceNameChange::={0}", priceNameChange);
            return priceNameChange;
        }
        /// <summary>
        /// 4-10  tinh phi bao tri
        /// メンテナンス料計算
        /// </summary>
        /// <returns></returns>
        public double GetPriceMantance()
        {
            double priceMantance = 0;

            if (_requestInCalc.ContractPlan != 99)
            {
                DateTime _expiresDate = DateTime.Parse(CommonFunction.ConvertDate(_requestInCalc.ExpiresDate!));
                DateTime _firstReg = DateTime.Parse(CommonFunction.ConvertDate(_requestInCalc.FirstReg!));
                DateTime _leaseSttMonth = DateTime.Parse(CommonFunction.ConvertDate(_requestInCalc.LeaseSttMonth!));
                DateTime _leaseExpirationDate = DateTime.Parse(CommonFunction.ConvertDate(_requestInCalc.LeaseExpirationDate!));
                var registrationDate = _expiresDate.AddMonths(1);
                var startLeaseDate = _leaseSttMonth;
                var endLeaseDate = _leaseExpirationDate;
                bool isFirstTime = string.IsNullOrEmpty(_requestInCalc.ExpiresDate!) ? true : false;
                var inspectionCount = InspectionCount(ref registrationDate, startLeaseDate, endLeaseDate, isFirstTime);
                if ((_requestInCalc.ContractTimes % _requestInCalc.AfterSecondTerm == 0 && inspectionCount > 0) & _expiresDate == startLeaseDate)
                {
                    inspectionCount -= 1;
                }
                _logger.LogInformation("InspectionCount:={0}", inspectionCount);
                var isBeforeFirstInspection = _requestInCalc.CarType == 3 ? 9 : IsBeforeFirstInspection();
                var dt = _unitOfWorkIde.Maintenances.GetSingleOrDefault(n => n.CarType == _requestInCalc.CarType
                && n.LeasePeriod == _requestInCalc.ContractTimes && n.BeforeFirstInspection == isBeforeFirstInspection
                && n.InspectionCount == inspectionCount);
                if (dt != null)
                {
                    priceMantance = dt.MyMaintenancePrice * _requestInCalc.ContractTimes;
                }
                _logger.LogInformation("4-10 PriceMaintenance:={0}", priceMantance);
            }
            return priceMantance;
        }
        /// <summary>
        /// 
        ///  4-11  tinh phi lai suat ap dung
        ///  適用金利率取得する
        /// </summary>
        /// <returns></returns>
        public double GetInterest()
        {
            double interest = 0;
            var dt = _unitOfWorkIde.Interests.GetSingleOrDefault(n => n.LeasePeriodFrom <= _requestInCalc.ContractTimes
               && n.LeasePeriodTo >= _requestInCalc.ContractTimes);
            if (dt != null)
            {
                interest = dt.Interest;
            }
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
        public DateTime CheckYear(int imonth)
        {
            DateTime regDay = DateTime.Parse(CommonFunction.ConvertDate(_requestInCalc.LeaseSttMonth!));
            DateTime ExpCurrY = DateTime.Parse(CommonFunction.ConvertDate(_requestInCalc.LeaseExpirationDate!));
            var regCurrY = regDay.AddMonths(imonth);
            return ExpCurrY > regCurrY ? regCurrY : ExpCurrY;
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
        public int InspectionCount(ref DateTime currentExpiresDate, DateTime startDate, DateTime endDate, bool isFirstTimes)
        {
            int inspectionCount = 0;
            DateTime currentCheckDate = currentExpiresDate;
            if (isFirstTimes)
            {
                currentCheckDate = currentCheckDate.AddMonths(_requestInCalc.FirstTerm);

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
                        currentCheckDate = currentCheckDate.AddMonths(_requestInCalc.AfterSecondTerm);
                    }
                    if (currentCheckDate > startDate && currentCheckDate < endDate)
                    {
                        currentExpiresDate = currentCheckDate;
                        inspectionCount += 1;

                    }
                    if (!isFirstTimes)
                    {
                        currentCheckDate = currentCheckDate.AddMonths(_requestInCalc.AfterSecondTerm);
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
            DateTime startP = DateTime.Parse(CommonFunction.ConvertDate(_requestInCalc.FirstReg!));
            DateTime leaseSttM = DateTime.Parse(CommonFunction.ConvertDate(_requestInCalc.LeaseSttMonth!));
            var currY = startP.AddMonths(_requestInCalc.FirstTerm);
            return (currY <= leaseSttM) ? 0 : 1;
        }
        /// <summary>
        /// GetPropertyFee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetPropertyFee(int id)
        {
            int pricePropertyFee = 0;
            var commissions = _unitOfWorkIde.Commissions.GetSingleOrDefault(n => n.Id == id);
            if (commissions != null)
            {
                if (commissions.IsMonthly == 1)
                {
                    pricePropertyFee = commissions.Fee * _requestInCalc.ContractTimes;
                    logCreditFee = commissions.Fee;
                }
                else
                {
                    pricePropertyFee = commissions.Fee;
                }
            }
            return pricePropertyFee;

        }

        #endregion Fuc
    }
}
