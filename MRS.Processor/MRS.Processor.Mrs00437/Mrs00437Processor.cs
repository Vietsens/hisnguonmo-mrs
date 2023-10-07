using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00437
{
    class Mrs00437Processor : AbstractProcessor
    {
        Mrs00437Filter castFilter = null;
        CommonParam paramGet = new CommonParam();

        List<DATA_GET> listDataGet = new List<DATA_GET>();
        List<SERVICE_PTTT_GROUP> listServicePtttGroup = new List<SERVICE_PTTT_GROUP>();

        List<DATA_REPORT> listServiceType = new List<DATA_REPORT>();
        Dictionary<string, DATA_REPORT> dicCategory = new Dictionary<string, DATA_REPORT>();
        Dictionary<string, DATA_REPORT> dicExecuteRoom = new Dictionary<string, DATA_REPORT>();
        Dictionary<string, DATA_REPORT> dicTtGroup = new Dictionary<string, DATA_REPORT>();
        Dictionary<string, DATA_REPORT> dicPtGroup = new Dictionary<string, DATA_REPORT>();
        List<Mrs00437RDO> listRdoOnTime = new List<Mrs00437RDO>();
        List<Mrs00437RDO> listRdoYear = new List<Mrs00437RDO>();

        public List<V_HIS_SERVICE_RETY_CAT> listServiceTRUYEN = null;
        public List<V_HIS_SERVICE_RETY_CAT> listServiceKSK = null;
        public List<V_HIS_SERVICE_RETY_CAT> listServiceCDHA = null;
        public List<V_HIS_SERVICE_RETY_CAT> listServiceKHAC = null;
        public List<V_HIS_SERVICE_RETY_CAT> listServiceSBA = null;
        public List<V_HIS_SERVICE_RETY_CAT> listServiceXQCC = null;
        public List<V_HIS_SERVICE_RETY_CAT> listServices = null;

        public Mrs00437Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00437Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00437Filter)this.reportFilter;
                HisServiceRetyCatViewFilterQuery serviceRetyCatFilter = new HisServiceRetyCatViewFilterQuery();
                serviceRetyCatFilter.REPORT_TYPE_CODE__EXACT = "MRS00437";
                listServices = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(paramGet).GetView(serviceRetyCatFilter);

                listServiceCDHA = listServices.Where(w => w.CATEGORY_CODE == "437CDHA").ToList();          // cđha
                listServiceKHAC = listServices.Where(w => w.CATEGORY_CODE == "437KHAC").ToList();          // dv khác
                listServiceKSK = listServices.Where(w => w.CATEGORY_CODE == "437KSK").ToList();            // khám sức khỏe
                listServiceTRUYEN = listServices.Where(w => w.CATEGORY_CODE == "437TRUYEN").ToList();      // truyền
                listServiceSBA = listServices.Where(w => w.CATEGORY_CODE == "437SBA").ToList();            // sao bệnh án
                listServiceXQCC = listServices.Where(w => w.CATEGORY_CODE == "437XQCC").ToList();          // xq cận chóp


                //nếu không lũy kế thì chỉ lấy dữ liệu trong khoảng thời gian báo cáo
                //nếu có lũy kế thì lấy cả dữ liệu của năm
                // thời gian bắt đầu của năm
                var FIRST_DAY_OF_YEAR = (long)Convert.ToInt64(this.castFilter.TIME_TO.ToString().Substring(0, 4) + "0101000000");
                if (castFilter.IS_NOT_ACCUMULATE == true)
                {
                }
                else
                {
                    this.castFilter.TIME_FROM = FIRST_DAY_OF_YEAR;
                }
                listDataGet = new ManagerSql().GetData(this.castFilter);
                listServicePtttGroup = new ManagerSql().GetServicePtttGroup();


            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (castFilter.IS_NOT_ACCUMULATE == true)
                {

                }
                else
                {
                    listRdoYear = ProcessDataRdo(listDataGet);
                }

                listRdoOnTime = ProcessDataRdo(listDataGet.Where(o => o.REPORT_TIME > this.castFilter.TIME_FROM).ToList());

                ProcessServiceType(listDataGet);


            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessServiceType(List<DATA_GET> listDataGet)
        {
            try
            {


                listServiceType.Add(new DATA_REPORT()
                {
                    CODE = "01",
                    NAME = "Khám bệnh",
                    SERVICE_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH }
                });

                listServiceType.Add(new DATA_REPORT()
                {
                    CODE = "02",
                    NAME = "Ngày giường nội trú",
                    SERVICE_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G }
                });

                listServiceType.Add(new DATA_REPORT()
                {
                    CODE = "03",
                    NAME = "Xét nghiệm",
                    SERVICE_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN }
                });

                listServiceType.Add(new DATA_REPORT()
                {
                    CODE = "04",
                    NAME = "Chẩn đoán hình ảnh",
                    SERVICE_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA }
                });

                listServiceType.Add(new DATA_REPORT()
                {
                    CODE = "05",
                    NAME = "Siêu âm",
                    SERVICE_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA }
                });

                listServiceType.Add(new DATA_REPORT()
                {
                    CODE = "06",
                    NAME = "Thăm dò chức năng",
                    SERVICE_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN }
                });

                listServiceType.Add(new DATA_REPORT()
                {
                    CODE = "07",
                    NAME = "Nội soi",
                    SERVICE_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS }
                });

                listServiceType.Add(new DATA_REPORT()
                {
                    CODE = "08",
                    NAME = " Phẫu thuật, thủ thuật",
                    SERVICE_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT }
                });

                listServiceType.Add(new DATA_REPORT()
                {
                    CODE = "09",
                    NAME = "Thuốc , máu, dịch truyền",
                    SERVICE_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU }
                });

                listServiceType.Add(new DATA_REPORT()
                {
                    CODE = "10",
                    NAME = "Vật tư tiêu hao thay thế",
                    SERVICE_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT }
                });

                listServiceType.Add(new DATA_REPORT()
                {
                    CODE = "11",
                    NAME = "Suất ăn",
                    SERVICE_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__AN }
                });

                listServiceType.Add(new DATA_REPORT()
                {
                    CODE = "12",
                    NAME = "Thu khác",
                    SERVICE_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC }
                });



                foreach (var item in listServiceType)
                {

                    var dataGetSub = listDataGet.Where(o => item.SERVICE_TYPE_IDs.Contains(o.TDL_SERVICE_TYPE_ID)).ToList();
                    caculatorSum(dataGetSub, item);

                    if (item.CODE == "03" || item.CODE == "04" || item.CODE == "05" || item.CODE == "06" || item.CODE == "07")
                    {
                        var groupByRoom = dataGetSub.GroupBy(g => g.TDL_EXECUTE_ROOM_ID).ToList();
                        foreach (var group in groupByRoom)
                        {

                            var executeRoom = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == group.First().TDL_EXECUTE_ROOM_ID)??new V_HIS_ROOM();
                            if (!dicExecuteRoom.ContainsKey(executeRoom.ROOM_CODE))
                            {

                                dicExecuteRoom[item.CODE + "_" + executeRoom.ROOM_CODE] = new DATA_REPORT();
                                dicExecuteRoom[item.CODE + "_" + executeRoom.ROOM_CODE].NAME = executeRoom.ROOM_NAME;
                                dicExecuteRoom[item.CODE + "_" + executeRoom.ROOM_CODE].CODE = executeRoom.ROOM_CODE;
                                dicExecuteRoom[item.CODE + "_" + executeRoom.ROOM_CODE].PARENT_CODE = item.CODE;
                            }
                            caculatorSum(group.ToList<DATA_GET>(), dicExecuteRoom[executeRoom.ROOM_CODE]);

                            if (item.CODE == "03")
                            {
                                foreach (var value in group)
                                {
                                    var serviceRetyCat = listServices.FirstOrDefault(o => o.SERVICE_ID == value.SERVICE_ID) ?? new V_HIS_SERVICE_RETY_CAT() { CATEGORY_CODE = "KHAC", CATEGORY_NAME = "Xét nghiệm khác" };
                                    if (!dicCategory.ContainsKey(executeRoom.ROOM_CODE+"_"+serviceRetyCat.CATEGORY_CODE))
                                    {

                                        dicCategory[executeRoom.ROOM_CODE + "_" + serviceRetyCat.CATEGORY_CODE] = new DATA_REPORT();
                                        dicCategory[executeRoom.ROOM_CODE + "_" + serviceRetyCat.CATEGORY_CODE].NAME = serviceRetyCat.CATEGORY_NAME;
                                        dicCategory[executeRoom.ROOM_CODE + "_" + serviceRetyCat.CATEGORY_CODE].CODE = serviceRetyCat.CATEGORY_CODE;
                                        dicCategory[executeRoom.ROOM_CODE + "_" + serviceRetyCat.CATEGORY_CODE].PARENT_CODE = executeRoom.ROOM_CODE;
                                    }
                                    caculatorSum(new List<DATA_GET>() { value }, dicCategory[executeRoom.ROOM_CODE + "_" + serviceRetyCat.CATEGORY_CODE]);
                                }
                            }
                        }
                    }

                    if (item.CODE == "08")
                    {
                        foreach (var sv in dataGetSub)
                        {
                            var ptttGroup = listServicePtttGroup.FirstOrDefault(o => o.SERVICE_ID == sv.SERVICE_ID) ?? new SERVICE_PTTT_GROUP() { PTTT_GROUP_CODE = "KHAC", PTTT_GROUP_NAME = "PTTT khác" };
                            if (sv.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT)
                            {
                                if (!dicPtGroup.ContainsKey(ptttGroup.PTTT_GROUP_CODE))
                                {

                                    dicPtGroup[ptttGroup.PTTT_GROUP_CODE] = new DATA_REPORT();
                                    dicPtGroup[ptttGroup.PTTT_GROUP_CODE].NAME = ptttGroup.PTTT_GROUP_NAME;
                                    dicPtGroup[ptttGroup.PTTT_GROUP_CODE].CODE = ptttGroup.PTTT_GROUP_CODE;
                                    dicPtGroup[ptttGroup.PTTT_GROUP_CODE].PARENT_CODE = item.CODE;
                                }
                                caculatorSum(new List<DATA_GET>() { sv }, dicPtGroup[ptttGroup.PTTT_GROUP_CODE]);
                            }
                            else
                            {
                                if (!dicTtGroup.ContainsKey(ptttGroup.PTTT_GROUP_CODE))
                                {

                                    dicTtGroup[ptttGroup.PTTT_GROUP_CODE] = new DATA_REPORT();
                                    dicTtGroup[ptttGroup.PTTT_GROUP_CODE].NAME = ptttGroup.PTTT_GROUP_NAME;
                                    dicTtGroup[ptttGroup.PTTT_GROUP_CODE].CODE = ptttGroup.PTTT_GROUP_CODE;
                                    dicTtGroup[ptttGroup.PTTT_GROUP_CODE].PARENT_CODE = item.CODE;
                                }
                                caculatorSum(new List<DATA_GET>() { sv }, dicTtGroup[ptttGroup.PTTT_GROUP_CODE]);
                            }
                        }
                        
                    }
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void caculatorSum(List<DATA_GET> dataGetSub, DATA_REPORT item)
        {
            long patientTypeIdBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
            item.AMOUNT += dataGetSub.Sum(s => s.AMOUNT ?? 0);
            item.TOTAL_PRICE += dataGetSub.Sum(s => s.VIR_TOTAL_PRICE ?? 0);
            item.AMOUNT_NOCARD += dataGetSub.Where(o => o.PATIENT_TYPE_ID != patientTypeIdBhyt).Sum(s => s.AMOUNT ?? 0);
            item.TOTAL_PRICE_NOCARD += dataGetSub.Where(o => o.PATIENT_TYPE_ID != patientTypeIdBhyt).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
            item.AMOUNT_HASCARD += dataGetSub.Where(o => o.PATIENT_TYPE_ID == patientTypeIdBhyt).Sum(s => s.AMOUNT ?? 0);
            item.TOTAL_PRICE_HASCARD += dataGetSub.Where(o => o.PATIENT_TYPE_ID == patientTypeIdBhyt).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
            item.TOTAL_HEIN_PRICE_HASCARD += dataGetSub.Where(o => o.PATIENT_TYPE_ID == patientTypeIdBhyt).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
            item.TOTAL_PATIENT_PRICE_HASCARD += dataGetSub.Where(o => o.PATIENT_TYPE_ID == patientTypeIdBhyt).Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
        }


        private List<Mrs00437RDO> ProcessDataRdo(List<DATA_GET> listData)
        {
            List<Mrs00437RDO> result = new List<Mrs00437RDO>();
            try
            {
                long patientTypeIdBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                var rdo = new Mrs00437RDO();
                // khám

                rdo.TOTAL_TREATMENT_EXAM = listData.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().Count();
                rdo.TREATMENT_EXAM_HEIN = listData.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && o.PATIENT_TYPE_ID == patientTypeIdBhyt).Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().Count();

                rdo.TREATMENT_EXAM_HEIN_HN = listData.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && o.PATIENT_TYPE_ID == patientTypeIdBhyt && o.HEIN_CARD_NUMBER != null && o.HEIN_CARD_NUMBER.StartsWith("HN")).Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().Count();

                rdo.TREATMENT_EXAM_HEIN_CN = listData.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && o.PATIENT_TYPE_ID == patientTypeIdBhyt && o.HEIN_CARD_NUMBER != null && o.HEIN_CARD_NUMBER.StartsWith("CN")).Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().Count();

                rdo.TREATMENT_EXAM_FEE = listData.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && o.PATIENT_TYPE_ID != patientTypeIdBhyt).Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().Count();

                // nội trú
                rdo.TREATMENT_TREAT_IN = listData.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().Count();

                rdo.TREATMENT_TREAT_IN_BHYT = listData.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && o.PATIENT_TYPE_ID == patientTypeIdBhyt).Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().Count();

                rdo.TREATMANT_TREAT_IN_BHYT_HN = listData.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && o.PATIENT_TYPE_ID == patientTypeIdBhyt && o.HEIN_CARD_NUMBER != null && o.HEIN_CARD_NUMBER.StartsWith("HN")).Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().Count();

                rdo.TREATMENT_TREAT_IN_BHYT_CN = listData.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && o.PATIENT_TYPE_ID == patientTypeIdBhyt && o.HEIN_CARD_NUMBER != null && o.HEIN_CARD_NUMBER.StartsWith("CN")).Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().Count();
                rdo.TREATMENT_TREAT_IN_FEE = listData.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && o.PATIENT_TYPE_ID != patientTypeIdBhyt).Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().Count();
                // tiền dịch vụ + viện phí ........
                rdo.BHYT_CHITRA = listData.Sum(su => su.VIR_TOTAL_HEIN_PRICE) ?? 0;
                rdo.BN_CHITRA = listData.Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                if (IsNotNullOrEmpty(listServiceXQCC))
                {
                    var xqccId = listServiceXQCC.Select(o => o.SERVICE_ID).ToList();
                    rdo.XQ_CANCHOP = listData.Where(w => xqccId.Contains(w.SERVICE_ID ?? 0)).Sum(su => su.VIR_TOTAL_PRICE ?? 0);
                    rdo.XQ_CANCHOP_BHYT_CHITRA = listData.Where(w => w.VIR_TOTAL_HEIN_PRICE != null && xqccId.Contains(w.SERVICE_ID ?? 0)).Sum(su => su.VIR_TOTAL_HEIN_PRICE) ?? 0;
                }
                rdo.CDHA_CHIPHI = listData.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA).Sum(su => su.VIR_TOTAL_PRICE ?? 0);
                rdo.CDHA_BHYT_CHITRA = listData.Where(w => w.VIR_TOTAL_HEIN_PRICE != null && w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA).Sum(su => su.VIR_TOTAL_HEIN_PRICE) ?? 0;

                rdo.TOTAL_HEIN_PRICE = rdo.BHYT_CHITRA - rdo.CDHA_BHYT_CHITRA + rdo.XQ_CANCHOP_BHYT_CHITRA;
                rdo.TOTAL_PATIENT_PRICE = rdo.BN_CHITRA - (rdo.CDHA_CHIPHI - rdo.CDHA_BHYT_CHITRA) + (rdo.XQ_CANCHOP - rdo.XQ_CANCHOP_BHYT_CHITRA);
                rdo.TOTAL_PRICE = rdo.TOTAL_HEIN_PRICE + rdo.TOTAL_PATIENT_PRICE;
                // chi tiết tiền dịch vụ
                if (IsNotNullOrEmpty(listServiceKSK))
                {
                    var kskId = listServiceKSK.Select(o => o.SERVICE_ID).ToList();
                    rdo.TOTAL_EXAM = listData.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH
                    && !kskId.Contains(w.SERVICE_ID ?? 0)).Sum(su => su.VIR_TOTAL_PRICE ?? 0);

                    rdo.TOTAL_KSK = listData.Where(w => kskId.Contains(w.SERVICE_ID ?? 0)).Sum(su => su.VIR_TOTAL_PRICE ?? 0);
                }
                else
                    rdo.TOTAL_EXAM = listData.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Sum(su => su.VIR_TOTAL_PRICE ?? 0);

                rdo.TOTAL_BED = listData.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G).Sum(su => su.VIR_TOTAL_PRICE ?? 0);

                rdo.TOTAL_TEST = listData.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).Sum(su => su.VIR_TOTAL_PRICE ?? 0);

                if (IsNotNullOrEmpty(listServiceCDHA))
                {
                    var cdhaId = listServiceCDHA.Select(o => o.SERVICE_ID).ToList();
                    rdo.TOTAL_DIIM_XQSANS = listData.Where(w => cdhaId.Contains(w.SERVICE_ID ?? 0)).Sum(su => su.VIR_TOTAL_PRICE ?? 0);
                }

                rdo.TOTAL_SURG_MISU = listData.Where(w => (w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT || w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT)).Sum(su => su.VIR_TOTAL_PRICE ?? 0);

                if (IsNotNullOrEmpty(listServiceKHAC))
                {
                    var khacId = listServiceKHAC.Select(s => s.SERVICE_ID).ToList();
                    rdo.TOTAL_OTHER = listData.Where(w => khacId.Contains(w.SERVICE_ID ?? 0)).Sum(su => su.VIR_TOTAL_PRICE ?? 0);
                }

                rdo.TOTAL_MEDICINE = listData.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM).Sum(su => su.VIR_TOTAL_PRICE ?? 0);

                rdo.TOTAL_MATERIAL = listData.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM).Sum(su => su.VIR_TOTAL_PRICE ?? 0);

                rdo.TOTAL_BLOOD = listData.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU).Sum(su => su.VIR_TOTAL_PRICE ?? 0);

                if (IsNotNullOrEmpty(listServiceTRUYEN))
                {
                    var truyenId = listServiceTRUYEN.Select(o => o.SERVICE_ID).ToList();
                    rdo.TOTAL_TRUYEN = listData.Where(w => truyenId.Contains(w.SERVICE_ID ?? 0)).Sum(su => su.VIR_TOTAL_PRICE ?? 0);
                }

                if (IsNotNullOrEmpty(listServiceSBA))
                {
                    var sbaId = listServiceSBA.Select(o => o.SERVICE_ID).ToList();
                    rdo.TOTAL_COPY = listData.Where(w => sbaId.Contains(w.SERVICE_ID ?? 0)).Sum(su => su.VIR_TOTAL_PRICE ?? 0);
                }

                rdo.TOTAL_EXEMPTION = listData.Sum(su => su.EXEMPTION ?? 0);
                result.Add(rdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                objectTag.AddObjectData(store, "Report", listRdoOnTime);
                objectTag.AddObjectData(store, "YearReport", listRdoYear);
                objectTag.AddObjectData(store, "ServiceTypes", listServiceType);
                objectTag.AddObjectData(store, "ExecuteRooms", dicExecuteRoom.Values.ToList());
                objectTag.AddObjectData(store, "Categorys", dicCategory.Values.ToList());
                objectTag.AddObjectData(store, "PtGroups", dicPtGroup.Values.ToList());
                objectTag.AddObjectData(store, "TtGroups", dicTtGroup.Values.ToList());
                objectTag.AddRelationship(store, "ServiceTypes", "ExecuteRooms", "CODE", "PARENT_CODE");
                objectTag.AddRelationship(store, "ExecuteRooms", "Categorys", "CODE", "PARENT_CODE");
                objectTag.AddRelationship(store, "ServiceTypes", "PtGroups", "CODE", "PARENT_CODE");
                objectTag.AddRelationship(store, "ServiceTypes", "TtGroups", "CODE", "PARENT_CODE");
                listDataGet.Clear();
                listDataGet = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
