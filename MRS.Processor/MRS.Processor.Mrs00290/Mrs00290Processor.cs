using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using ACS.Filter;
using AutoMapper;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using MRS.SDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisHeinServiceType;
using SDA.MANAGER.Manager;
using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Core.SdaProvince.Get;
using MOS.MANAGER.HisServiceRetyCat;

namespace MRS.Processor.Mrs00290
{
    class Mrs00290Processor : AbstractProcessor
    {
        //List<HIS_SERE_SERV> listSereServ = new List<HIS_SERE_SERV>();
        //List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();
        //List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        //List<HIS_SERVICE_REQ> listServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_SERVICE> listService = new List<HIS_SERVICE>();
        List<Mrs00290RDO> listTreatmentService = new List<Mrs00290RDO>();
        CommonParam paramGet = new CommonParam();
        List<Mrs00290RDO> ListRdo = new List<Mrs00290RDO>();
        List<Mrs00290RDO> ListUser = new List<Mrs00290RDO>();
        List<HIS_ICD> ListICD = new List<HIS_ICD>();
        Mrs00290Filter filter = null;
        //List<Mrs00290RDO> Parent = new List<Mrs00290RDO>();
        Dictionary<string, SDA_PROVINCE> dicSdaProvince = new Dictionary<string, SDA_PROVINCE>();

        long COUNT_TREATMENT = 0;
        List<HIS_HEIN_SERVICE_TYPE> listHeinServiceType = new List<HIS_HEIN_SERVICE_TYPE>();
        public Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicServiceRetyCat = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>();
        public Mrs00290Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00290Filter);
        }

        protected override bool GetData()
        {
            filter = ((Mrs00290Filter)reportFilter);
            bool result = true;
            try
            {
                ListRdo = new ManagerSql().GetSereServDO(filter);

                if (IsNotNullOrEmpty(filter.LOGINNAMEs))
                {
                    ListRdo = ListRdo.Where(o => filter.LOGINNAMEs.Contains(o.TDL_REQUEST_LOGINNAME)).ToList();
                }

                if (IsNotNullOrEmpty(filter.LOGINNAME_DOCTORs))
                {
                    ListRdo = ListRdo.Where(o => (filter.LOGINNAME_DOCTORs.Contains(o.TDL_REQUEST_LOGINNAME) && o.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH) || (filter.LOGINNAME_DOCTORs.Contains(o.EXECUTE_LOGINNAME) && o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)).ToList();
                }

                ListICD = new ManagerSql().GetIcd(filter);
                if (filter.ICD_IDs != null && ListICD != null && ListICD.Count > 0)
                {
                    ListRdo = ListRdo.Where(o => ListICD.Exists(p => p.ICD_CODE == o.ICD_CODE)).ToList();
                }

                //danh sách loại dịch vụ bảo hiểm
                GetHeinServiceType();

                //danh sách dịch vụ
                GetService();

                //danh sách nhóm báo cáo
                GetServiceRetyCat();

                // danh sach tỉnh
                GetProvince();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetProvince()
        {
            //Danh sách huyện
            var listSdaProvince = new SdaProvinceManager(new CommonParam()).Get<List<SDA_PROVINCE>>(new SdaProvinceFilterQuery()) ?? new List<SDA_PROVINCE>();
            dicSdaProvince = listSdaProvince.GroupBy(o => o.PROVINCE_CODE).ToDictionary(p => p.Key, q => q.First());
        }

        private void GetServiceRetyCat()
        {
            HisServiceRetyCatViewFilterQuery serviceRetyCatFilter = new HisServiceRetyCatViewFilterQuery()
            {
                IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE,
                REPORT_TYPE_CODE__EXACT = "MRS00290"
            };
            var serviceRetyCat = new HisServiceRetyCatManager().GetView(serviceRetyCatFilter) ?? new List<V_HIS_SERVICE_RETY_CAT>();
            dicServiceRetyCat = serviceRetyCat.GroupBy(o => o.SERVICE_ID).ToDictionary(p => p.Key, q => q.First());
        }

        private void GetService()
        {
            HisServiceFilterQuery serviceFilter = new HisServiceFilterQuery()
            {
                IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
            };
            listService = new HisServiceManager().Get(serviceFilter);
        }

        private void GetHeinServiceType()
        {
            HisHeinServiceTypeFilterQuery hsvtfilter = new HisHeinServiceTypeFilterQuery();
            this.listHeinServiceType = new HisHeinServiceTypeManager().Get(hsvtfilter);
        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                //ListRdo.Clear();

                if (IsNotNullOrEmpty(ListRdo))
                {
                    ListRdo = ListRdo.OrderBy(o => o.TDL_TREATMENT_ID).ToList();

                    List<long> listWeekStart = new List<long>();
                    List<long> listWeekEnd = new List<long>();
                    List<long> listDate = new List<long>();
                    long dateFrom = filter.TIME_FROM - filter.TIME_FROM % 1000000;
                    long dateTo = filter.TIME_TO - filter.TIME_TO % 1000000;
                    if (Inventec.Common.DateTime.Calculation.DifferenceDate(dateFrom, dateTo) < 8)
                    {
                        DateTime? fromTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(dateFrom);
                        for (int i = 0; i < 7; i++)
                        {
                            listDate.Add(Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(fromTime.Value) ?? 0);
                            fromTime = fromTime.Value.AddDays(1);
                        }
                    }
                    else if (Inventec.Common.DateTime.Calculation.DifferenceDate(dateFrom, dateTo) < 36)
                    {
                        DateTime? fromTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(dateFrom);
                        for (int i = 0; i < 5; i++)
                        {
                            listWeekStart.Add(Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(fromTime.Value) ?? 0);
                            listWeekEnd.Add(Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(fromTime.Value.AddDays(6)) ?? 0);
                            fromTime = fromTime.Value.AddDays(7);
                        }
                    }

                    foreach (var rdo in ListRdo)
                    {
                        if (filter.INPUT_DATA_ID_FEE_TYPE != null)
                        {
                            FilterFeeType(rdo,filter.INPUT_DATA_ID_FEE_TYPE??0);
                        }
                        rdo.REPORT_PERIOD = AddInfoPeriod(rdo,listDate,listWeekStart,listWeekEnd);
                        rdo.INSTRUCTION_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo.TDL_INTRUCTION_TIME);
                        rdo.START_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo.START_TIME ?? 0);
                        if (rdo.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
                        {
                            rdo.REQ_USERNAME = rdo.EXECUTE_USERNAME;
                            rdo.REQ_LOGINNAME = rdo.EXECUTE_LOGINNAME;
                        }
                        else
                        {
                            rdo.REQ_USERNAME = rdo.TDL_REQUEST_USERNAME;
                            rdo.REQ_LOGINNAME = rdo.TDL_REQUEST_LOGINNAME;
                        }

                        var Service = listService.FirstOrDefault(o => o.ID == rdo.SERVICE_ID);
                        if (Service != null)
                        {
                            var ParentService = listService.FirstOrDefault(o => o.ID == Service.PARENT_ID);
                            if (ParentService != null)
                            {
                                rdo.PARENT_SERVICE_CODE = ParentService.SERVICE_CODE;
                                rdo.PARENT_SERVICE_NAME = ParentService.SERVICE_NAME;
                            }
                        }
                        var serviceRetyCat = dicServiceRetyCat.ContainsKey(rdo.SERVICE_ID) ? dicServiceRetyCat[rdo.SERVICE_ID] : null;
                        if (serviceRetyCat != null)
                        {
                            rdo.CATEGORY_CODE = serviceRetyCat.CATEGORY_CODE;
                        }
                        if (rdo.ORIGINAL_PRICE > 0)
                        {
                            rdo.PAY_RATE_RATIO = rdo.HEIN_LIMIT_PRICE.HasValue ? (rdo.HEIN_LIMIT_PRICE.Value / (rdo.ORIGINAL_PRICE * (1 + rdo.VAT_RATIO))) * 100 : (rdo.PRICE / rdo.ORIGINAL_PRICE) * 100;
                        }
                        rdo.ROUTE_TYPE = 3;
                        var branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == rdo.BRANCH_ID);
                        if (branch != null)
                        {
                            rdo.ACCEPT_HEIN_MEDI_ORG_CODE = branch.ACCEPT_HEIN_MEDI_ORG_CODE;
                            rdo.BRANCH_HEIN_PROVINCE_CODE = branch.HEIN_PROVINCE_CODE;
                        }
                        if (!string.IsNullOrWhiteSpace(rdo.JSON_PATIENT_TYPE_ALTER))
                        {
                            try
                            {
                                var patientTypeAlter = Newtonsoft.Json.JsonConvert.DeserializeObject<HIS_PATIENT_TYPE_ALTER>(rdo.JSON_PATIENT_TYPE_ALTER);
                                if (patientTypeAlter != null)
                                {
                                    if ((rdo.ACCEPT_HEIN_MEDI_ORG_CODE ?? "").Contains(rdo.TREA_HEIN_MEDI_ORG_CODE ?? "")
                                            && checkBhytProvinceCode((rdo.TREA_HEIN_MEDI_ORG_CODE ?? ""), rdo.BRANCH_HEIN_PROVINCE_CODE ?? ""))
                                    {
                                        rdo.ROUTE_TYPE = 1;
                                    }
                                    else if (!string.IsNullOrWhiteSpace(patientTypeAlter.RIGHT_ROUTE_TYPE_CODE) || patientTypeAlter.RIGHT_ROUTE_CODE == "DT")
                                    {
                                        rdo.ROUTE_TYPE = 2;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Inventec.Common.Logging.LogSystem.Warn("Loi du lieu doi tuong benh nhan JSON_PATIENT_TYPE_ALTER (HIS_SERE_SERV): " + rdo.JSON_PATIENT_TYPE_ALTER);
                            }
                        }
                        rdo.SERVICE_NAME = rdo.TDL_SERVICE_NAME;
                        var reqRoom = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == rdo.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM();
                        rdo.ROOM_NAME = reqRoom.ROOM_NAME;
                        rdo.REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == rdo.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        rdo.REQUEST_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == rdo.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                        rdo.SERVICE_TYPE_CODE = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == rdo.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE;
                        rdo.HEIN_SERVICE_TYPE_CODE = (this.listHeinServiceType.FirstOrDefault(o => o.ID == rdo.TDL_HEIN_SERVICE_TYPE_ID) ?? new HIS_HEIN_SERVICE_TYPE()).HEIN_SERVICE_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == rdo.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_NAME;
                        rdo.PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == rdo.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;
                        rdo.TDL_PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == rdo.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;
                        rdo.EXECUTE_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == rdo.TDL_EXECUTE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        var executeRoom = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == rdo.TDL_EXECUTE_ROOM_ID) ?? new V_HIS_ROOM();
                        rdo.EXECUTE_ROOM_NAME = executeRoom.ROOM_NAME;
                        if (executeRoom.IS_EXAM == 1 && rdo.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
                        {
                            rdo.EXAM_ROOM_ID = executeRoom.ID;
                            rdo.EXAM_ROOM_CODE = executeRoom.ROOM_CODE;
                            rdo.EXAM_ROOM_NAME = executeRoom.ROOM_NAME;
                        }
                        else if (reqRoom.IS_EXAM == 1 && rdo.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
                        {
                            rdo.EXAM_ROOM_ID = reqRoom.ID;
                            rdo.EXAM_ROOM_CODE = reqRoom.ROOM_CODE;
                            rdo.EXAM_ROOM_NAME = reqRoom.ROOM_NAME;
                        }

                    }
                }
                if (!string.IsNullOrWhiteSpace(filter.PAY_RATE_RATIO))
                {
                    decimal payRatioFilter = -1;
                    if (decimal.TryParse(filter.PAY_RATE_RATIO, out payRatioFilter))
                    {
                        ListRdo = ListRdo.Where(o => o.PAY_RATE_RATIO == payRatioFilter).ToList();
                    }
                }
                if (filter.INPUT_DATA_ID_FEE_TYPE != null)
                {
                    ListRdo = ListRdo.Where(o => o.FEE_TYPE == filter.INPUT_DATA_ID_FEE_TYPE).ToList();
                }
                //Parent = ListRdo.GroupBy(o => o.REQ_USERNAME).Select(p => p.First()).ToList();
                var serviceIds = ListRdo.Select(o => o.SERVICE_ID).Distinct().ToList();
                this.listService = this.listService.Where(o => serviceIds.Contains(o.ID)).ToList();
                if (filter.IS_NOT_TAKE_AMOUNT_SV != true)
                {
                    var groupByTreatment = ListRdo.GroupBy(o => o.TDL_TREATMENT_ID).ToList();
                    foreach (var item in groupByTreatment)
                    {
                        Mrs00290RDO rdo = new Mrs00290RDO();
                        Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00290RDO>(rdo, item.First());
                        for (int i = 0; i < listService.Count; i++)
                        {
                            if (i > 49)
                            {
                                continue;
                            }
                            System.Reflection.PropertyInfo pi = typeof(Mrs00290RDO).GetProperty(string.Format("AMOUNT_{0}", i + 1));
                            pi.SetValue(rdo, item.ToList().Where(o => o.SERVICE_ID == listService[i].ID).Sum(s => s.AMOUNT));
                        }

                        this.listTreatmentService.Add(rdo);
                    }
                }
                COUNT_TREATMENT = ListRdo.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().Count();
                ListUser = ListRdo.GroupBy(o => o.REQ_LOGINNAME).Select(p => p.First()).ToList();
                if (this.dicDataFilter.ContainsKey("KEY_GROUP_SS") && this.dicDataFilter["KEY_GROUP_SS"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_GROUP_SS"].ToString()))
                {
                    GroupByKey(this.dicDataFilter["KEY_GROUP_SS"].ToString());
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

        private void FilterFeeType(Mrs00290RDO rdo, short FeeType)
        {
            if (FeeType == 1)//chọn loại chi phí bảo hiểm
            {
                if (rdo.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    rdo.FEE_TYPE = FeeType;
                    if (rdo.PRIMARY_PATIENT_TYPE_ID != null)
                    {
                        rdo.VIR_TOTAL_PATIENT_PRICE = (rdo.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                        rdo.VIR_TOTAL_PRICE = (rdo.VIR_TOTAL_HEIN_PRICE ?? 0) + (rdo.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                    }
                }
            }
            if (FeeType == 2)//chọn loại chi phí tự trả
            {
                    rdo.FEE_TYPE = FeeType;
                if (rdo.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    if (rdo.PRIMARY_PATIENT_TYPE_ID != null)
                    {
                        rdo.VIR_TOTAL_PATIENT_PRICE = (rdo.VIR_TOTAL_PATIENT_PRICE ?? 0) - (rdo.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                        rdo.VIR_TOTAL_PRICE = (rdo.VIR_TOTAL_PRICE ?? 0) - (rdo.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0) - (rdo.VIR_TOTAL_HEIN_PRICE ?? 0);
                        rdo.VIR_TOTAL_HEIN_PRICE = 0;
                        rdo.VIR_TOTAL_PATIENT_PRICE_BHYT = 0;
                    }
                    else
                    {
                        rdo.FEE_TYPE = null;
                    }
                }
            }
        }

        private string AddInfoPeriod(Mrs00290RDO rdo, List<long> listDate, List<long> listWeekStart, List<long> listWeekEnd)
        {
            string result = "";
            if (listWeekStart.Count == 0 && listDate.Count == 0)
            {
                result= string.Format("{0}/{1}", rdo.REPORT_TIME.ToString().Substring(4, 2), rdo.REPORT_TIME.ToString().Substring(0, 4));
            }
            else if (listWeekStart.Count == 0 && listDate.Count > 0)
            {
                result= string.Format("{0}/{1}", rdo.REPORT_TIME.ToString().Substring(6, 2), rdo.REPORT_TIME.ToString().Substring(4, 2));
            }
            else
            {
                if (rdo.REPORT_TIME < listWeekStart[1])
                {
                    result= string.Format("{0}/{1} - {2}/{3}", listWeekStart[0].ToString().Substring(6, 2), listWeekStart[0].ToString().Substring(4, 2), listWeekEnd[0].ToString().Substring(6, 2), listWeekEnd[0].ToString().Substring(4, 2));
                }
                else if (rdo.REPORT_TIME < listWeekStart[2])
                {
                    result= string.Format("{0}/{1} - {2}/{3}", listWeekStart[1].ToString().Substring(6, 2), listWeekStart[1].ToString().Substring(4, 2), listWeekEnd[1].ToString().Substring(6, 2), listWeekEnd[1].ToString().Substring(4, 2));
                }
                else if (rdo.REPORT_TIME < listWeekStart[3])
                {
                    result= string.Format("{0}/{1} - {2}/{3}", listWeekStart[2].ToString().Substring(6, 2), listWeekStart[2].ToString().Substring(4, 2), listWeekEnd[2].ToString().Substring(6, 2), listWeekEnd[2].ToString().Substring(4, 2));
                }
                else if (rdo.REPORT_TIME < listWeekStart[4])
                {
                    result= string.Format("{0}/{1} - {2}/{3}", listWeekStart[3].ToString().Substring(6, 2), listWeekStart[3].ToString().Substring(4, 2), listWeekEnd[3].ToString().Substring(6, 2), listWeekEnd[3].ToString().Substring(4, 2));
                }
                else
                {
                    result= string.Format("{0}/{1} - {2}/{3}", listWeekStart[4].ToString().Substring(6, 2), listWeekStart[4].ToString().Substring(4, 2), listWeekEnd[4].ToString().Substring(6, 2), listWeekEnd[4].ToString().Substring(4, 2));
                }

            }
            return result;
        }


        private bool checkBhytProvinceCode(string HeinMediOrgCode, string branchHeinProvinceCode)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(HeinMediOrgCode) && HeinMediOrgCode.Length == 5)
                {
                    string provinceCode = HeinMediOrgCode.Substring(0, 2);
                    if (branchHeinProvinceCode.Equals(provinceCode))
                    {
                        result = true;
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
        private void GroupByKey(string key)
        {
            string errorField = "";
            try
            {
                var group = ListRdo.GroupBy(o => string.Format(key, o.SERVICE_ID, o.TDL_SERVICE_TYPE_ID, o.TDL_REQUEST_DEPARTMENT_ID, o.PATIENT_TYPE_ID, o.TDL_TREATMENT_ID, o.VIR_PRICE, o.TDL_EXECUTE_DEPARTMENT_ID, o.PAY_RATE_RATIO, o.TDL_TREATMENT_TYPE_ID ?? 0, o.ICD_CODE, o.ROUTE_TYPE, o.TDL_REQUEST_LOGINNAME, o.TDL_REQUEST_ROOM_ID, o.REPORT_DATE, o.EXECUTE_LOGINNAME, o.REPORT_TIME, o.EXAM_ROOM_CODE, o.TDL_PATIENT_TYPE_ID, o.TREA_HEIN_MEDI_ORG_CODE, o.LAST_DEPARTMENT_CODE, o.INVOICE_NUM_ORDER, o.TDL_EXECUTE_ROOM_ID,o.REPORT_PERIOD)).ToList();
                ListRdo.Clear();
                Inventec.Common.Logging.LogSystem.Info("ListRdo" + ListRdo.Count);
                Decimal sum = 0;
                Mrs00290RDO rdo;
                List<Mrs00290RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00290RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00290RDO();
                    listSub = item.ToList<Mrs00290RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("TOTAL") || field.Name.Contains("AMOUNT") || field.Name.Contains("_AM_"))
                        {
                            //trong V_HIS_SERE_SERV thêm AMOUNT_TEMP
                            sum = listSub.Sum(s => (decimal?)field.GetValue(s) ?? 0);
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    rdo.COUNT_REQ = listSub.Select(o => o.SERVICE_REQ_ID ?? 0).Distinct().Count();
                    rdo.COUNT_TREATMENT = listSub.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().Count();
                    rdo.TREATMENT_DAY_COUNT = listSub.GroupBy(o => o.TDL_TREATMENT_ID ?? 0).Sum(s => s.First().TREATMENT_DAY_COUNT ?? 0);
                    rdo.TREA_HEIN_MEDI_ORG_CODE = string.Join(";", listSub.Select(o => o.TREA_HEIN_MEDI_ORG_CODE ?? "").Distinct().ToList());
                    rdo.DIC_SVT_PRICE = listSub.GroupBy(o => o.SERVICE_TYPE_CODE ?? "NONE").ToDictionary(o => o.Key, p => p.Sum(s => s.VIR_TOTAL_PRICE ?? 0));
                    rdo.DIC_CATE_PRICE = listSub.GroupBy(o => o.CATEGORY_CODE ?? "NONE").ToDictionary(o => o.Key, p => p.Sum(s => s.VIR_TOTAL_PRICE ?? 0));
                    rdo.DIC_PAR_PRICE = listSub.GroupBy(o => o.PARENT_SERVICE_CODE ?? "NONE").ToDictionary(o => o.Key, p => p.Sum(s => s.VIR_TOTAL_PRICE ?? 0));
                    rdo.DIC_HSVT_PRICE = listSub.GroupBy(o => o.HEIN_SERVICE_TYPE_CODE ?? "NONE").ToDictionary(o => o.Key, p => p.Sum(s => s.VIR_TOTAL_PRICE ?? 0));
                    rdo.DIC_SV_PRICE = listSub.GroupBy(o => o.TDL_SERVICE_CODE ?? "NONE").ToDictionary(o => o.Key, p => p.Sum(s => s.VIR_TOTAL_PRICE ?? 0));

                    //so luong
                    rdo.DIC_SVT_AMOUNT = listSub.GroupBy(o => o.SERVICE_TYPE_CODE ?? "NONE").ToDictionary(o => o.Key, p => p.Sum(s => s.AMOUNT));
                    rdo.DIC_CATE_AMOUNT = listSub.GroupBy(o => o.CATEGORY_CODE ?? "NONE").ToDictionary(o => o.Key, p => p.Sum(s => s.AMOUNT));
                    rdo.DIC_PAR_AMOUNT = listSub.GroupBy(o => o.PARENT_SERVICE_CODE ?? "NONE").ToDictionary(o => o.Key, p => p.Sum(s => s.AMOUNT));
                    rdo.DIC_HSVT_AMOUNT = listSub.GroupBy(o => o.HEIN_SERVICE_TYPE_CODE ?? "NONE").ToDictionary(o => o.Key, p => p.Sum(s => s.AMOUNT));
                    rdo.DIC_SV_AMOUNT = listSub.GroupBy(o => o.TDL_SERVICE_CODE ?? "NONE").ToDictionary(o => o.Key, p => p.Sum(s => s.AMOUNT));

                    //so benh nhan
                    rdo.DIC_SVT_TREATMENT = listSub.GroupBy(o => o.SERVICE_TYPE_CODE ?? "NONE").ToDictionary(o => o.Key, p => p.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().Count());
                    rdo.DIC_CATE_TREATMENT = listSub.GroupBy(o => o.CATEGORY_CODE ?? "NONE").ToDictionary(o => o.Key, p => p.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().Count());
                    rdo.DIC_PAR_TREATMENT = listSub.GroupBy(o => o.PARENT_SERVICE_CODE ?? "NONE").ToDictionary(o => o.Key, p => p.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().Count());
                    rdo.DIC_HSVT_TREATMENT = listSub.GroupBy(o => o.HEIN_SERVICE_TYPE_CODE ?? "NONE").ToDictionary(o => o.Key, p => p.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().Count());
                    rdo.DIC_SV_TREATMENT = listSub.GroupBy(o => o.TDL_SERVICE_CODE ?? "NONE").ToDictionary(o => o.Key, p => p.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().Count());

                    //chi tiet ma benh
                    rdo.DIC_SVT_ICD = listSub.GroupBy(o => o.SERVICE_TYPE_CODE ?? "NONE").ToDictionary(o => o.Key, p => string.Join(";",p.Select(o => o.ICD_CODE).Distinct().ToList()));
                    rdo.DIC_CATE_ICD = listSub.GroupBy(o => o.CATEGORY_CODE ?? "NONE").ToDictionary(o => o.Key, p => string.Join(";", p.Select(o => o.ICD_CODE).Distinct().ToList()));
                    rdo.DIC_PAR_ICD = listSub.GroupBy(o => o.PARENT_SERVICE_CODE ?? "NONE").ToDictionary(o => o.Key, p => string.Join(";", p.Select(o => o.ICD_CODE).Distinct().ToList()));
                    rdo.DIC_HSVT_ICD = listSub.GroupBy(o => o.HEIN_SERVICE_TYPE_CODE ?? "NONE").ToDictionary(o => o.Key, p => string.Join(";", p.Select(o => o.ICD_CODE).Distinct().ToList()));
                    rdo.DIC_SV_ICD = listSub.GroupBy(o => o.TDL_SERVICE_CODE ?? "NONE").ToDictionary(o => o.Key, p => string.Join(";", p.Select(o => o.ICD_CODE).Distinct().ToList()));

                    if (!hide) ListRdo.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }

        private Mrs00290RDO IsMeaningful(List<Mrs00290RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00290RDO();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (((Mrs00290Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)((Mrs00290Filter)reportFilter).TIME_FROM));
            }

            if (((Mrs00290Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)((Mrs00290Filter)reportFilter).TIME_TO));
            }

            dicSingleTag.Add("COUNT_TREATMENT", COUNT_TREATMENT);

            objectTag.AddObjectData(store, "Parent", ListUser);
            //objectTag.AddObjectData(store, "Department", ListRdo.GroupBy(o => o.REQUEST_DEPARTMENT_NAME).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddRelationship(store, "Parent", "Report", "REQ_LOGINNAME", "REQ_LOGINNAME");
            // objectTag.AddRelationship(store, "Department", "Report", "REQUEST_DEPARTMENT_NAME", "REQUEST_DEPARTMENT_NAME");
            for (int i = 0; i < listService.Count; i++)
            {
                if (i > 49)
                {
                    continue;
                }
                dicSingleTag.Add(string.Format("SERVICE_NAME_{0}", i + 1), listService[i].SERVICE_NAME);
            }

            objectTag.AddObjectData(store, "TreatmentService", listTreatmentService);
        }

    }

}
