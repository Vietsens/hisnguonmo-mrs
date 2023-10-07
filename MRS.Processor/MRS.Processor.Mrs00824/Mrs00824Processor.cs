using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00824
{
    public class Mrs00824Processor : AbstractProcessor
    {
        Mrs00824Filter filter;
        public List<Mrs00824RDO> ListRdo = new List<Mrs00824RDO>();
        public List<Mrs00824RDO> ListRdoResult = new List<Mrs00824RDO>();
        public List<Mrs00824RDO> ListRdoGroup = new List<Mrs00824RDO>();
       
        public Mrs00824Processor(CommonParam param, string reportTypeCode): base(param, reportTypeCode)
        { 
    
        }
       
        public override Type FilterType()
        {
            return typeof(Mrs00824Filter);
        }

        protected override bool GetData()
        {
            filter = (Mrs00824Filter)this.reportFilter;
            bool result = false;
            try
            {
                ListRdo = new ManagerSql().GetListRdo(filter);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }
          
        protected override bool ProcessData()
        {
            bool result = false;
            var listHeinServiceTypeMedi = new List<long>
                {
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU
                };
            try
            {
                if (IsNotNullOrEmpty(ListRdo))
                {
                    var group = ListRdo.GroupBy(x => new {x.IS_EXPEND, x.TDL_HEIN_SERVICE_BHYT_NAME, x.TDL_PATIENT_CODE, x.TDL_INTRUCTION_TIME,x.DEPARTMENT_NAME,x.ROOM_NAME,x.ORIGINAL_PRICE,x.TREATMENT_CODE }).ToList();
                    foreach (var item in group)
                    {
                        if (item.First().ORIGINAL_PRICE==0)
                        {
                            item.First().ORIGINAL_PRICE = 1;
                        }
                        var tyle = item.First().HEIN_LIMIT_PRICE.HasValue ? (item.First().HEIN_LIMIT_PRICE.Value / (item.First().ORIGINAL_PRICE * (1 + item.First().VAT_RATIO))) : (item.First().PRICE / item.First().ORIGINAL_PRICE);
                        Mrs00824RDO rdo = new Mrs00824RDO();
                        rdo.TDL_PATIENT_CODE = item.First().TDL_PATIENT_CODE;
                        rdo.TDL_PATIENT_NAME = item.First().TDL_PATIENT_NAME;
                        rdo.DEPARTMENT_NAME = item.First().DEPARTMENT_NAME;
                        rdo.ROOM_NAME = item.First().ROOM_NAME;
                        rdo.EXAM_ROOM_NAME = item.First().EXAM_ROOM_NAME;
                        rdo.TREATMENT_CODE = item.First().TREATMENT_CODE;
                        rdo.TDL_HEIN_SERVICE_BHYT_CODE = item.First().TDL_HEIN_SERVICE_BHYT_CODE;
                        rdo.TDL_HEIN_SERVICE_BHYT_NAME = item.First().TDL_HEIN_SERVICE_BHYT_NAME;
                        rdo.TDL_HEIN_SERVICE_TYPE_ID = item.First().TDL_HEIN_SERVICE_TYPE_ID;
                        rdo.AMOUNT = item.Sum(x=>x.AMOUNT);

                        if (listHeinServiceTypeMedi.Contains(item.First().TDL_HEIN_SERVICE_TYPE_ID.Value))
                        {
                            rdo.TOTAL_PRICE = Math.Round(item.First().ORIGINAL_PRICE * (1 + item.First().VAT_RATIO), 4) * rdo.AMOUNT;
                        }
                        else  if ((item.First().TDL_HST_BHYT_CODE == "15" /*&& (xml3.TyLeTT == 50 || xml3.TyLeTT == 30)*/)
                            || (item.First().TDL_HST_BHYT_CODE == "13" /*&& (xml3.TyLeTT == 30 || xml3.TyLeTT == 10)*/)
                            || (item.First().TDL_HST_BHYT_CODE == "8" && ((tyle * 100) == 50 || (tyle * 100) == 80))
                            || (item.First().TDL_HST_BHYT_CODE == "18" && ((tyle * 100) == 50 || (tyle * 100) == 80))
                            || HisBranchCFG.HisBranchs.FirstOrDefault(o => item.First().BRANCH_ID == o.ID).HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.COMMUNE)
                        {
                            rdo.TOTAL_PRICE = Math.Round(item.First().ORIGINAL_PRICE * (1 + item.First().VAT_RATIO), 4) * rdo.AMOUNT * tyle;
                        }
                        else
                        {
                            rdo.TOTAL_PRICE = Math.Round(item.First().ORIGINAL_PRICE * (1 + item.First().VAT_RATIO), 4) * rdo.AMOUNT;
                        }
                        rdo.IS_EXPEND = item.First().IS_EXPEND;
                        rdo.PRICE_DV = item.First().ORIGINAL_PRICE * (1 + item.First().VAT_RATIO);
                        rdo.TDL_INTRUCTION_TIME = item.First().TDL_INTRUCTION_TIME;
                        rdo.TDL_HEIN_CARD_NUMBER = item.First().TDL_HEIN_CARD_NUMBER;
                        ListRdoResult.Add(rdo);
                    }

                    var groupByheinService = ListRdo.GroupBy(x => new { x.TDL_HEIN_SERVICE_BHYT_NAME,x.IS_EXPEND, x.ORIGINAL_PRICE, x.TDL_HEIN_SERVICE_TYPE_ID}).ToList();
                    foreach (var item in groupByheinService)
                    {
                        Mrs00824RDO rdo = new Mrs00824RDO();
                        rdo.TDL_HEIN_SERVICE_BHYT_CODE = item.First().TDL_HEIN_SERVICE_BHYT_CODE;
                        rdo.TDL_HEIN_SERVICE_BHYT_NAME = item.First().TDL_HEIN_SERVICE_BHYT_NAME;
                        rdo.TDL_HEIN_SERVICE_TYPE_ID = item.First().TDL_HEIN_SERVICE_TYPE_ID;
                        rdo.AMOUNT = item.Sum(x => x.AMOUNT);
                        rdo.PRICE = item.First().PRICE;
                        rdo.IS_EXPEND = item.First().IS_EXPEND;
                        rdo.PRICE_DV = item.First().ORIGINAL_PRICE * (1 + item.First().VAT_RATIO);
                        ListRdoGroup.Add(rdo);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
            objectTag.AddObjectData(store, "ReportDetail", ListRdoResult);
            objectTag.AddObjectData(store, "XN", ListRdoResult.Where(x=>x.TDL_HEIN_SERVICE_TYPE_ID==IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN).ToList());
            objectTag.AddObjectData(store, "DIIM", ListRdoResult.Where(x=>x.TDL_HEIN_SERVICE_TYPE_ID==IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN||x.TDL_HEIN_SERVICE_TYPE_ID==IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA).ToList());
            objectTag.AddObjectData(store, "PTTT", ListRdoResult.Where(x => x.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TT || x.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT).ToList());
            objectTag.AddObjectData(store, "Material", ListRdoResult.Where(x=>x.TDL_HEIN_SERVICE_TYPE_ID== IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM||x.TDL_HEIN_SERVICE_TYPE_ID== IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM||x.TDL_HEIN_SERVICE_TYPE_ID== IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL).ToList());
            objectTag.AddObjectData(store, "MaterialRatioTT", ListRdoResult.Where(x=>x.TDL_HEIN_SERVICE_TYPE_ID==IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT).ToList());
            objectTag.AddObjectData(store, "ServiceRatio", ListRdoResult.Where(x=>x.TDL_HEIN_SERVICE_TYPE_ID==IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC).ToList());
            objectTag.AddObjectData(store, "Blood", ListRdoResult.Where(x=>x.TDL_HEIN_SERVICE_TYPE_ID==IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM||x.TDL_HEIN_SERVICE_TYPE_ID==IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU).ToList());
            objectTag.AddObjectData(store, "BedDay", ListRdoResult.Where(x => x.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN || x.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L).ToList());
            objectTag.AddObjectData(store, "Bed", ListRdoResult.Where(x => x.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT || x.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT).ToList());
            objectTag.AddObjectData(store,"ReportGroup", ListRdoGroup);
        }
    }
}
