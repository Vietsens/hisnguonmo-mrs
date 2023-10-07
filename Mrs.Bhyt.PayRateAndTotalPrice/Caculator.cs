using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mrs.Bhyt.PayRateAndTotalPrice
{
    public static class Caculator
    {
           
        public static decimal PayRate(HIS_SERE_SERV SereServ)
        {
            decimal result = 0;
            try
            {
                result = SereServ.HEIN_LIMIT_PRICE.HasValue ? (SereServ.HEIN_LIMIT_PRICE.Value / (SereServ.ORIGINAL_PRICE * (1 + SereServ.VAT_RATIO))) : (SereServ.PRICE / SereServ.ORIGINAL_PRICE);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Debug(ex);
                result = 0;
            }
            return result;
        }

        public static decimal TotalPrice(object sese, HIS_BRANCH branch)
        {
            return TotalPrice("4", sese, branch, false);
        }

        public static decimal TotalPrice(object sese, HIS_BRANCH branch, bool IsMaterialOriginalPriceEqualPrice)
        {
            return TotalPrice("4", sese, branch, IsMaterialOriginalPriceEqualPrice);
        }

        public static decimal TotalPrice(string NumDigits, object sese, HIS_BRANCH branch)
        {
            return TotalPrice(NumDigits, sese, branch, false);
        }

        public static decimal TotalPrice(string NumDigits, object sese, HIS_BRANCH branch,bool IsMaterialOriginalPriceEqualPrice)
        {
                var SereServ = new HIS_SERE_SERV();
            decimal result = 0;
            try
            {

                if(sese is HIS_SERE_SERV)
                {
                    SereServ = (HIS_SERE_SERV)sese;
                    if ((sese as HIS_SERE_SERV).IS_NO_EXECUTE == 1) return 0;
                    if ((sese as HIS_SERE_SERV).IS_EXPEND == 1) return 0;
                    if ((sese as HIS_SERE_SERV).VIR_TOTAL_HEIN_PRICE == 0) return 0;
                }
                else if (sese is V_HIS_SERE_SERV_3)
                {
                    if ((sese as V_HIS_SERE_SERV_3).IS_NO_EXECUTE == 1) return 0;
                    if ((sese as V_HIS_SERE_SERV_3).IS_EXPEND == 1) return 0;
                    if ((sese as V_HIS_SERE_SERV_3).VIR_TOTAL_HEIN_PRICE == 0) return 0;
                    SereServ.ID = (sese as V_HIS_SERE_SERV_3).ID;
                    SereServ.HEIN_LIMIT_PRICE = (sese as V_HIS_SERE_SERV_3).HEIN_LIMIT_PRICE;
                    SereServ.ORIGINAL_PRICE = (sese as V_HIS_SERE_SERV_3).ORIGINAL_PRICE;
                    SereServ.VAT_RATIO = (sese as V_HIS_SERE_SERV_3).VAT_RATIO;
                    SereServ.PRICE = (sese as V_HIS_SERE_SERV_3).PRICE;
                    SereServ.TDL_HEIN_SERVICE_TYPE_ID = (sese as V_HIS_SERE_SERV_3).TDL_HEIN_SERVICE_TYPE_ID;
                    SereServ.TDL_HST_BHYT_CODE = (sese as V_HIS_SERE_SERV_3).TDL_HST_BHYT_CODE;
                    SereServ.AMOUNT = (sese as V_HIS_SERE_SERV_3).AMOUNT;
                    SereServ.VIR_TOTAL_PRICE = (sese as V_HIS_SERE_SERV_3).VIR_TOTAL_PRICE;
                }
                
                var listHeinServiceTypeMate = new List<long>
                {
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT
                };
                if (IsMaterialOriginalPriceEqualPrice == true && listHeinServiceTypeMate.Contains(SereServ.TDL_HEIN_SERVICE_TYPE_ID ?? 0))
                {
                    SereServ.ORIGINAL_PRICE = SereServ.PRICE;
                }
                var tyle = SereServ.HEIN_LIMIT_PRICE.HasValue ? (SereServ.HEIN_LIMIT_PRICE.Value / (SereServ.ORIGINAL_PRICE * (1 + SereServ.VAT_RATIO))) : (SereServ.PRICE / SereServ.ORIGINAL_PRICE);

                var listHeinServiceTypeMedi = new List<long>
                {
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU
                };
                if (listHeinServiceTypeMedi.Contains(SereServ.TDL_HEIN_SERVICE_TYPE_ID.Value))
                {
                    result = Math.Round(SereServ.ORIGINAL_PRICE * (1 + SereServ.VAT_RATIO), 4, MidpointRounding.AwayFromZero) * SereServ.AMOUNT;
                }
                else if ((SereServ.TDL_HST_BHYT_CODE == "15" /*&& (xml3.TyLeTT == 50 || xml3.TyLeTT == 30)*/)
                    || (SereServ.TDL_HST_BHYT_CODE == "14" /*&& (xml3.TyLeTT == 50 || xml3.TyLeTT == 30)*/)
                    || (SereServ.TDL_HST_BHYT_CODE == "13" /*&& (xml3.TyLeTT == 30 || xml3.TyLeTT == 10)*/)
                    || (SereServ.TDL_HST_BHYT_CODE == "8" && ((tyle * 100) == 50 || (tyle * 100) == 80))
                    || (SereServ.TDL_HST_BHYT_CODE == "18" && ((tyle * 100) == 50 || (tyle * 100) == 80))
                    || branch.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.COMMUNE)
                {
                    result = Math.Round(SereServ.ORIGINAL_PRICE * (1 + SereServ.VAT_RATIO), 4, MidpointRounding.AwayFromZero) * SereServ.AMOUNT * tyle;
                }
                else
                {
                    result = Math.Round(SereServ.ORIGINAL_PRICE * (1 + SereServ.VAT_RATIO), 4, MidpointRounding.AwayFromZero) * SereServ.AMOUNT;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Debug(ex);
                result = 0;
            }
            if (result != (SereServ.VIR_TOTAL_PRICE ?? 0))
            {
                Inventec.Common.Logging.LogSystem.Info(";ss.id:" + SereServ.ID + ";ss.HEIN_LIMIT_PRICE:" + SereServ.HEIN_LIMIT_PRICE + ";ss.ORIGINAL_PRICE:" + SereServ.ORIGINAL_PRICE + ";ss.PRICE:" + SereServ.PRICE + ";ss.AMOUNT:" + SereServ.AMOUNT + ";ss.PRICE:" + SereServ.PRICE + ";ss.VIR_TOTAL_PRICE:" + SereServ.VIR_TOTAL_PRICE);
            }
            int NumDigit = 4;
            if (int.TryParse(NumDigits, out NumDigit))
            {
                return Math.Round(result, NumDigit, MidpointRounding.AwayFromZero);
            }
            else
            {
                return result;
            };
        }
    }
}
