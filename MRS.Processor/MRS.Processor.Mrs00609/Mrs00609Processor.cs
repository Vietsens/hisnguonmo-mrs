using System;
using System.Collections.Generic;
using System.Linq;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTreatment;

using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisHeinServiceType;
using MOS.MANAGER.HisOtherPaySource;

namespace MRS.Processor.Mrs00609
{
    public class Mrs00609Processor : AbstractProcessor
    {
        Mrs00609Filter filter = null;
        private List<Mrs00609RDO> listRdo = new List<Mrs00609RDO>();
        private List<SERE_SERV> listSereServ = new List<SERE_SERV>();
        //private List<HIS_TREATMENT> ListHisTreatment = new List<HIS_TREATMENT>();
        //List<HIS_SERE_SERV> ListHisSereServ = new List<HIS_SERE_SERV>();
        //List<HIS_PATIENT_TYPE_ALTER> ListHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        Dictionary<long, string> dicCate = new Dictionary<long, string>();
        Dictionary<long, HIS_SERVICE> dicPar = new Dictionary<long, HIS_SERVICE>();
        List<HIS_HEIN_SERVICE_TYPE> ListHeinServiceType = new List<HIS_HEIN_SERVICE_TYPE>();
        List<HIS_OTHER_PAY_SOURCE> ListOtherPaySource = new List<HIS_OTHER_PAY_SOURCE>();

        public Mrs00609Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00609Filter);
        }

        protected override bool GetData()
        {
            filter = ((Mrs00609Filter)reportFilter);
            var result = true;
            CommonParam param = new CommonParam();
            try
            {
                // get treatmentFee

                listRdo = new ManagerSql().GetTreatment(filter);
                listSereServ = new ManagerSql().GetSereServ(filter);
                if (filter.IS_TREAT.HasValue)
                {
                    if (filter.IS_TREAT.Value)
                    {
                        listRdo = listRdo.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                    }
                    else
                    {
                        listRdo = listRdo.Where(o => o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                    }
                }
                //danh sách nhóm báo cáo
                GetServiceRetyCate();

                //dịch vụ cha
                GetParService();

                //loại dịch vụ bảo hiểm
                GetHeinServiceType();

                //danh sách nguồn chi trả khác
                GetOtherPaySource();

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
        private void GetParService()
        {
            var listService = new HisServiceManager().Get(new HisServiceFilterQuery());
            if (listService != null)
            {
                var parents = listService.Where(o => listService.Exists(p => p.PARENT_ID == o.ID)).ToList();
                if (parents != null)
                {
                    dicPar = listService.Where(o=>o.PARENT_ID !=null).ToDictionary(o => o.ID, p => parents.FirstOrDefault(q => q.ID == p.PARENT_ID) ?? new HIS_SERVICE());
                }
            }
        }

        private void GetServiceRetyCate()
        {
            HisServiceRetyCatViewFilterQuery serviceRetyCatFilter = new HisServiceRetyCatViewFilterQuery();
            serviceRetyCatFilter.REPORT_TYPE_CODE__EXACT = this.reportType.REPORT_TYPE_CODE;
            dicCate = (new HisServiceRetyCatManager().GetView(serviceRetyCatFilter) ?? new List<V_HIS_SERVICE_RETY_CAT>()).GroupBy(g => g.SERVICE_ID).ToDictionary(o => o.Key, p => p.First().CATEGORY_CODE??"NONE");
        }

        private void GetHeinServiceType()
        {
            ListHeinServiceType = new HisHeinServiceTypeManager().Get(new HisHeinServiceTypeFilterQuery());

        }

        private void GetOtherPaySource()
        {
            ListOtherPaySource = new HisOtherPaySourceManager().Get(new HisOtherPaySourceFilterQuery());

        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                Dictionary<long, string> dicSvtCode = HisServiceTypeCFG.HisServiceTypes.ToDictionary(o => o.ID, p => p.SERVICE_TYPE_CODE);
                Dictionary<long, string> dicHSvtCode = ListHeinServiceType.ToDictionary(o => o.ID, p => p.HEIN_SERVICE_TYPE_CODE);

                foreach (var item in listRdo)
                {
                    var sereServSub = listSereServ.Where(o => o.TDL_TREATMENT_ID == item.TREATMENT_ID).ToList()?? new List<SERE_SERV>();
                    var otherPaySourceIds = sereServSub.Where(o => o.OTHER_PAY_SOURCE_ID.HasValue).Select(o => o.OTHER_PAY_SOURCE_ID.Value).Distinct().ToList();
                    var otherPaySources = (ListOtherPaySource ?? new List<HIS_OTHER_PAY_SOURCE>()).Where(o => otherPaySourceIds.Contains(o.ID)).ToList();
                    if (otherPaySources.Count > 0)
                    { 
                    item.OTHER_PAY_SOURCE_NAMEs = string.Join(", ",otherPaySources.Select(o=>o.OTHER_PAY_SOURCE_NAME).ToList());
                    }
                    item.DOB_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.TDL_PATIENT_DOB??0);
                    item.DOB_YEAR_STR = (item.TDL_PATIENT_DOB??0).ToString().Substring(0, 4);
                    item.AGE = CalcuatorAge(item.TDL_PATIENT_DOB??0);
                    item.TDL_PATIENT_TYPE_ID = item.TDL_PATIENT_TYPE_ID ?? -1;//	Đối tượng bệnh nhân
                    item.TDL_TREATMENT_TYPE_ID = item.TDL_TREATMENT_TYPE_ID ?? -1;//Diện điều trị
                    item.PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID ==item.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;//	Đối tượng bệnh nhân
                    item.TREATMENT_TYPE_NAME = (HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == item.TDL_TREATMENT_TYPE_ID) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_NAME;//	Diện điều trị
                    //item.DOB_YEAR_STR = item.TDL_PATIENT_DOB.ToString().Substring(0, 4); //	Năm sinh
                    item.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.IN_TIME);//Thời gian vào
                    item.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.OUT_TIME ?? 0);//	Thời gian ra

                    item.REQUEST_LOGINNAME = string.Join(";", sereServSub.Select(o=>o.REQUEST_LOGINNAME).Distinct().ToList());//	Tài khoản yêu cầu
                    item.REQUEST_USERNAME = string.Join(";", sereServSub.Select(o => o.REQUEST_USERNAME).Distinct().ToList());//	người yêu cầu
                    item.SERVICE_NAME = string.Join(";", sereServSub.Select(o => o.TDL_SERVICE_NAME).Distinct().ToList());//	Tên dịch vụ
                    item.SERVICE_CODE = string.Join(";", sereServSub.Select(o => o.TDL_SERVICE_CODE).Distinct().ToList()); ;//Mã dịch vụ	
                    item.VIR_TOTAL_PRICE = sereServSub.Sum(o=>o.VIR_TOTAL_PRICE);  //tong chi phí
                    item.VIR_TOTAL_HEIN_PRICE = sereServSub.Sum(o=>o.VIR_TOTAL_HEIN_PRICE) ;  //bao hiem
                    item.VIR_TOTAL_PATIENT_PRICE = sereServSub.Sum(o => o.VIR_TOTAL_PATIENT_PRICE);  //benh nhan
                    item.VIR_TOTAL_PATIENT_PRICE_BHYT = sereServSub.Sum(o=>o.VIR_TOTAL_PATIENT_PRICE_BHYT) ;  //dong chi tra
                    item.TOTAL_OTHER_SOURCE_PRICE =sereServSub.Sum(o=>o.TOTAL_OTHER_SOURCE_PRICE);  //nguon khac

                    item.DIC_CATE_TOTAL_PRICE = sereServSub.GroupBy(o => dicCate.ContainsKey(o.SERVICE_ID)?dicCate[o.SERVICE_ID]?? "NONE":"NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PRICE));
                    item.DIC_CATE_TOTAL_HEIN_PRICE = sereServSub.GroupBy(o =>dicCate.ContainsKey(o.SERVICE_ID)?dicCate[o.SERVICE_ID]?? "NONE":"NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_HEIN_PRICE));
                    item.DIC_CATE_TOTAL_PATIENT_PRICE = sereServSub.GroupBy(o =>dicCate.ContainsKey(o.SERVICE_ID)?dicCate[o.SERVICE_ID]?? "NONE":"NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PATIENT_PRICE));
                    item.DIC_CATE_TOTAL_PATIENT_PRICE_BHYT = sereServSub.GroupBy(o =>dicCate.ContainsKey(o.SERVICE_ID)?dicCate[o.SERVICE_ID]?? "NONE":"NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PATIENT_PRICE_BHYT));
                    item.DIC_CATE_TOTAL_PATIENT_PRICE_SELF = sereServSub.GroupBy(o =>dicCate.ContainsKey(o.SERVICE_ID)?dicCate[o.SERVICE_ID]?? "NONE":"NONE").ToDictionary(p => p.Key, q => q.Sum(su => (su.VIR_TOTAL_PATIENT_PRICE) - (su.VIR_TOTAL_PATIENT_PRICE_BHYT)));
                    item.DIC_CATE_TOTAL_OTHER_SOURCE_PRICE = sereServSub.GroupBy(o => dicCate.ContainsKey(o.SERVICE_ID) ? dicCate[o.SERVICE_ID] ?? "NONE" : "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.TOTAL_OTHER_SOURCE_PRICE));
                    item.DIC_SVT_TOTAL_PRICE = sereServSub.GroupBy(o => dicSvtCode.ContainsKey(o.TDL_SERVICE_TYPE_ID)?dicSvtCode[o.TDL_SERVICE_TYPE_ID]:"NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PRICE));
                    item.DIC_SVT_TOTAL_HEIN_PRICE = sereServSub.GroupBy(o => dicSvtCode.ContainsKey(o.TDL_SERVICE_TYPE_ID)?dicSvtCode[o.TDL_SERVICE_TYPE_ID]:"NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_HEIN_PRICE));
                    item.DIC_SVT_TOTAL_PATIENT_PRICE = sereServSub.GroupBy(o => dicSvtCode.ContainsKey(o.TDL_SERVICE_TYPE_ID)?dicSvtCode[o.TDL_SERVICE_TYPE_ID]:"NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PATIENT_PRICE));
                    item.DIC_SVT_TOTAL_PATIENT_PRICE_BHYT = sereServSub.GroupBy(o => dicSvtCode.ContainsKey(o.TDL_SERVICE_TYPE_ID)?dicSvtCode[o.TDL_SERVICE_TYPE_ID]:"NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PATIENT_PRICE_BHYT));
                    item.DIC_SVT_TOTAL_PATIENT_PRICE_SELF = sereServSub.GroupBy(o => dicSvtCode.ContainsKey(o.TDL_SERVICE_TYPE_ID)?dicSvtCode[o.TDL_SERVICE_TYPE_ID]:"NONE").ToDictionary(p => p.Key, q => q.Sum(su => (su.VIR_TOTAL_PATIENT_PRICE) - (su.VIR_TOTAL_PATIENT_PRICE_BHYT)));
                    item.DIC_SVT_TOTAL_OTHER_SOURCE_PRICE = sereServSub.GroupBy(o => dicSvtCode.ContainsKey(o.TDL_SERVICE_TYPE_ID) ? dicSvtCode[o.TDL_SERVICE_TYPE_ID] : "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.TOTAL_OTHER_SOURCE_PRICE));
                    item.DIC_PAR_TOTAL_PRICE = sereServSub.GroupBy(o => dicPar.ContainsKey(o.SERVICE_ID) ? dicPar[o.SERVICE_ID].SERVICE_CODE ?? "NONE" : "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PRICE));
                    item.DIC_PAR_TOTAL_HEIN_PRICE = sereServSub.GroupBy(o => dicPar.ContainsKey(o.SERVICE_ID) ? dicPar[o.SERVICE_ID].SERVICE_CODE ?? "NONE" : "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_HEIN_PRICE));
                    item.DIC_PAR_TOTAL_PATIENT_PRICE = sereServSub.GroupBy(o => dicPar.ContainsKey(o.SERVICE_ID) ? dicPar[o.SERVICE_ID].SERVICE_CODE ?? "NONE" : "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PATIENT_PRICE));
                    item.DIC_PAR_TOTAL_PATIENT_PRICE_BHYT = sereServSub.GroupBy(o => dicPar.ContainsKey(o.SERVICE_ID) ? dicPar[o.SERVICE_ID].SERVICE_CODE ?? "NONE" : "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PATIENT_PRICE_BHYT));
                    item.DIC_PAR_TOTAL_PATIENT_PRICE_SELF = sereServSub.GroupBy(o => dicPar.ContainsKey(o.SERVICE_ID) ? dicPar[o.SERVICE_ID].SERVICE_CODE ?? "NONE" : "NONE").ToDictionary(p => p.Key, q => q.Sum(su => (su.VIR_TOTAL_PATIENT_PRICE) - (su.VIR_TOTAL_PATIENT_PRICE_BHYT)));
                    item.DIC_PAR_TOTAL_OTHER_SOURCE_PRICE = sereServSub.GroupBy(o => dicPar.ContainsKey(o.SERVICE_ID) ? dicPar[o.SERVICE_ID].SERVICE_CODE ?? "NONE" : "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.TOTAL_OTHER_SOURCE_PRICE));

                    item.DIC_HSVT_TOTAL_PRICE = sereServSub.GroupBy(o => dicHSvtCode.ContainsKey(o.TDL_HEIN_SERVICE_TYPE_ID) ? dicHSvtCode[o.TDL_HEIN_SERVICE_TYPE_ID] : "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PRICE));
                    item.DIC_HSVT_TOTAL_HEIN_PRICE = sereServSub.GroupBy(o => dicHSvtCode.ContainsKey(o.TDL_HEIN_SERVICE_TYPE_ID) ? dicHSvtCode[o.TDL_HEIN_SERVICE_TYPE_ID] : "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_HEIN_PRICE));
                    item.DIC_HSVT_TOTAL_PATIENT_PRICE = sereServSub.GroupBy(o => dicHSvtCode.ContainsKey(o.TDL_HEIN_SERVICE_TYPE_ID) ? dicHSvtCode[o.TDL_HEIN_SERVICE_TYPE_ID] : "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PATIENT_PRICE));
                    item.DIC_HSVT_TOTAL_PATIENT_PRICE_BHYT = sereServSub.GroupBy(o => dicHSvtCode.ContainsKey(o.TDL_HEIN_SERVICE_TYPE_ID) ? dicHSvtCode[o.TDL_HEIN_SERVICE_TYPE_ID] : "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PATIENT_PRICE_BHYT));
                    item.DIC_HSVT_TOTAL_PATIENT_PRICE_SELF = sereServSub.GroupBy(o => dicHSvtCode.ContainsKey(o.TDL_HEIN_SERVICE_TYPE_ID) ? dicHSvtCode[o.TDL_HEIN_SERVICE_TYPE_ID] : "NONE").ToDictionary(p => p.Key, q => q.Sum(su => (su.VIR_TOTAL_PATIENT_PRICE) - (su.VIR_TOTAL_PATIENT_PRICE_BHYT)));
                    item.DIC_HSVT_TOTAL_OTHER_SOURCE_PRICE = sereServSub.GroupBy(o => dicHSvtCode.ContainsKey(o.TDL_HEIN_SERVICE_TYPE_ID) ? dicHSvtCode[o.TDL_HEIN_SERVICE_TYPE_ID] : "NONE").ToDictionary(p => p.Key, q => q.Sum(su => (su.TOTAL_OTHER_SOURCE_PRICE)));
                    if (Continues(item))
                    {
                        item.IS_DELETE = true;
                    }
                }
                listRdo = listRdo.Where(o => o.IS_DELETE != true).ToList();
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private bool Continues(Mrs00609RDO rdo)
        {
            try
            {

                System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<Mrs00609RDO>();
                foreach (var p in pi)
                {
                    var fieldFilter = p.GetValue(filter);
                    if (fieldFilter != null && fieldFilter.ToString() != "" && fieldFilter.ToString() != "0"/* && this.dicDataFilter.ContainsKey(p.Name)*/)
                    {
                        var value = p.GetValue(rdo);
                        if (value == null || !value.ToString().Contains(fieldFilter.ToString()))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private int? CalcuatorAge(long DOB)
        {
            int? AGE = null;
            try
            {
                int? tuoi = RDOCommon.CalculateAge(DOB);
                if (tuoi >= 0)
                {
                    AGE = (tuoi >= 1) ? tuoi : 1;
                }
                return AGE;
            }
            catch (Exception ex)
            {
                return null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {

            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00609Filter)reportFilter).OUT_TIME_FROM));

            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00609Filter)reportFilter).OUT_TIME_TO));

            objectTag.AddObjectData(store, "Report", listRdo);
            objectTag.AddObjectData(store, "ReportQN", listRdo.Where(o => CheckType(o.HEIN_CARD_NUMBER, HeinCardNumberTypeCFG.HeinCardNumber__HeinType__02)).ToList());
            objectTag.AddObjectData(store, "ReportTN", listRdo.Where(o => CheckType(o.HEIN_CARD_NUMBER, HeinCardNumberTypeCFG.HeinCardNumber__HeinType__01)).ToList());
            objectTag.AddObjectData(store, "ReportTH", listRdo.Where(o => (!CheckType(o.HEIN_CARD_NUMBER, HeinCardNumberTypeCFG.HeinCardNumber__HeinType__02)) && (!CheckType(o.HEIN_CARD_NUMBER, HeinCardNumberTypeCFG.HeinCardNumber__HeinType__01))).ToList());


        }

        private bool CheckType(string HeinCardNumber, List<string> types)
        {
            bool result = false;
            try
            {
                if (!String.IsNullOrEmpty(HeinCardNumber))
                {
                    if (IsNotNullOrEmpty(types))
                    {
                        result = types.Exists(o => HeinCardNumber.StartsWith(o));
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

    }
}
