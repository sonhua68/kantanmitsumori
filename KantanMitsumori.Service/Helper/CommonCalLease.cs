using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Infrastructure.Base;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Service.Helper
{
    internal class CommonCalLease
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWorkIDE _unitOfWorkIde;
        public CommonCalLease(ILogger logger, IUnitOfWorkIDE unitOfWorkIde)
        {
            _logger = logger;
            _unitOfWorkIde = unitOfWorkIde;
        }
        /// <summary>
        /// 4-1 file doc  lay thue tieu thu  
        /// 消費税を取得する   
        /// </summary>
        /// <returns></returns>
        public double GetConsumptionTax()
        {
            double ConsumptionTax = 0.0;
            var dt = _unitOfWorkIde.ConsumptionTaxs.GetAll().ToList();
            if (dt.Count > 0)
            {
                ConsumptionTax = dt.FirstOrDefault()!.ConsumptionTax;
            }
            _logger.LogInformation("4-1 ConsumptionTax:={0}", ConsumptionTax);
            return ConsumptionTax;
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
        public decimal GetPrice(int salesSum, int taxInsAll, int taxFreeAll, decimal consumptionTax)
        {
            decimal dPrice = (salesSum - taxInsAll - taxFreeAll) / (1 + consumptionTax) + taxInsAll + taxFreeAll;
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
        /// <param name="carType"></param>
        /// <param name="firstRegistrationDateFrom"></param>
        /// <param name="firstRegistrationDateTo"></param>
        /// <param name="isElectricCar"></param>
        /// <param name="displacementFrom"></param>
        /// <param name="displacementTo"></param>
        /// <returns></returns>
        public decimal GetPriceTax(int carType,
            int firstRegistrationDateFrom, int firstRegistrationDateTo,
            int isElectricCar, int displacementFrom, int displacementTo)
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
            monthlyPrice = dt.Result.FirstOrDefault()!.MonthlyPrice;
            _logger.LogInformation("4-3-1 PriceTax :={0}", monthlyPrice);
            return monthlyPrice;
        }
        /// <summary>
        /// * 4-3-2 file doc lay thue gia tang theo dung tich dong co
        /// 検索条件は以下の通り。        
        /// </summary>
        /// <param name="carType"></param>
        /// <param name="firstRegistrationDateFrom"></param>
        /// <param name="firstRegistrationDateTo"></param>
        /// <param name="isElectricCar"></param>
        /// <param name="displacementFrom"></param>
        /// <param name="displacementTo"></param>
        /// <returns></returns>
        public decimal GetTaxCollectionIncrease(int carType,
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
            var dt = _unitOfWorkIde.CarTaxs.Query(n => n.CarType == carType &&
       n.FirstRegistrationDateFrom <= firstRegistrationDateFrom &&
            n.FirstRegistrationDateTo >= firstRegistrationDateTo &&
       n.IsElectricCar == isElectricCar &&
       n.DisplacementFrom <= displacementFrom
       && n.DisplacementTo >= displacementTo &&
       n.ElapsedYearsFrom <= elapsedYearsFrom
       && n.ElapsedYearsTo >= elapsedYearsTo);
            monthlyPrice = dt.Result.FirstOrDefault()!.MonthlyPrice;
            _logger.LogInformation("4-3-2 PriceTax :={0}", monthlyPrice);
            return monthlyPrice;
        }
        /// <summary>
        ///  4-3-3  tinh tien thue cua xe trong thoi han 
        ///   期間中自動車計算  期間中自動車税金計算 156 = 3 Year
        /// </summary>
        /// <param name="autoTax"></param>
        /// <returns></returns>
        public decimal GetVehicleTaxWithinTheTerm(int carType, int autoTax, int contractTime, string leaseExpirationDate, string dispVolUnit,
            bool chkElectricCar, int dispVol, int firstRegform, int firstRegTo)
        {
            decimal vehicleTaxPrice = 0;
            int displacementFromAndTo = 0;
            int isElectricCar = 0;
            decimal priceMonth = 0;
            int month = 0;
            int month1 = 0;
            if (chkElectricCar)
            {
                isElectricCar = 1;
            }
            if (dispVolUnit != "cc")
            {
                displacementFromAndTo = 0;
            }
            else
            {
                displacementFromAndTo = dispVol;
            }
            var priceTax = GetPriceTax(carType, firstRegform, firstRegTo, isElectricCar, displacementFromAndTo, displacementFromAndTo);
            string endLeaseDate = leaseExpirationDate; //Ngay het han hop dong thue
            string endDate = CheckYear(156, firstRegTo.ToString(), leaseExpirationDate);// 156 = 13Year
            if (endDate == endLeaseDate)
            {
                priceMonth = priceTax * contractTime;
                _logger.LogInformation("4-3-3 PriceMonth  < 13Year :={0}", priceMonth);
            }
            else //'> 13Year
            {
                var taxCollection = GetTaxCollectionIncrease(carType, firstRegform, firstRegTo, isElectricCar, dispVol, dispVol);
                priceMonth = priceTax * contractTime;

            }



            return vehicleTaxPrice;


        }
        #region Fuc
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imonth"></param>
        /// <param name="firstReg"></param>
        /// <param name="leaseExpirationDate"></param>
        /// <returns></returns>
        public string CheckYear(int imonth, string firstReg, string leaseExpirationDate)
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
        public int getMonthLease(int type, string firstReg, string leaseExpirationDate, string leaseSttMonth)
        {
            int month = 0;



            return month;
        }


        #endregion Fuc
    }


}
