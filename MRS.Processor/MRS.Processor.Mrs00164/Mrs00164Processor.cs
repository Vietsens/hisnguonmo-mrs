using MOS.MANAGER.HisService;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisDepartmentTran;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Inventec.Common.DateTime;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.SDO;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using MRS.MANAGER.Core.MrsReport;

namespace MRS.Processor.Mrs00164
{
    internal class Mrs00164Processor : AbstractProcessor
    {
        string DEPARTMENT_NAME = "";
        List<VSarReportMrs00164RDO> _listEmergencys = new List<VSarReportMrs00164RDO>();
        List<VSarReportMrs00164RDO> _listPatients = new List<VSarReportMrs00164RDO>();
        List<VSarReportMrs00164RDO> _listSarReportMrs00164Rdos = new List<VSarReportMrs00164RDO>();

        List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        Dictionary<long, HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, HIS_SERVICE_REQ>();
        Mrs00164Filter CastFilter;
        private List<string> listDate = new List<string>();
        private Dictionary<string, object> dicSingleData = new Dictionary<string, object>();
        public Mrs00164Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00164Filter);
        }


        protected override bool GetData()
        {
            var result = false;
            try
            {
                var paramGet = new CommonParam();
                CastFilter = (Mrs00164Filter)this.reportFilter;

                var metyFilterDepartmentTran = new HisDepartmentTranFilterQuery()
                {
                    DEPARTMENT_IN_TIME_FROM = CastFilter.DATE_FROM,
                    DEPARTMENT_IN_TIME_TO = CastFilter.DATE_TO,
                    DEPARTMENT_ID = CastFilter.DEPARTMENT_ID
                };
                var listDepartmentTrans = new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(paramGet).Get(metyFilterDepartmentTran);

                // DEPARTMENT_NAME
                var metyFilterDepartment = new HisDepartmentFilterQuery
                {
                    ID = CastFilter.DEPARTMENT_ID
                };
                var departmentName = new MOS.MANAGER.HisDepartment.HisDepartmentManager(paramGet).Get(metyFilterDepartment);
                if (departmentName != null)
                {
                    DEPARTMENT_NAME = departmentName.First().DEPARTMENT_NAME;
                }
                // <~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ EMERGENCY
                var ListTreatment = new List<HIS_TREATMENT>();
                var listTreatmentIds = listDepartmentTrans.GroupBy(o => o.TREATMENT_ID).Select(s => s.First().TREATMENT_ID).ToList();
                var skip = 0;
                while (listTreatmentIds.Count() - skip > 0)
                {
                    var listIds = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterTreatment = new HisTreatmentFilterQuery()
                    {
                        IDs = listIds
                    };
                    var listTreatmentSub = new HisTreatmentManager(paramGet).Get(metyFilterTreatment);
                    ListTreatment.AddRange(listTreatmentSub);
                }
                ListTreatment = ListTreatment.Where(o => o.IS_EMERGENCY == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                // <~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ SERE_SERV
                var listServiceReq = new List<HIS_SERVICE_REQ>();
                var metyFilterServiceReq = new HisServiceReqFilterQuery()
                {
                    REQUEST_DEPARTMENT_ID = CastFilter.DEPARTMENT_ID,
                    SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT,
                    FINISH_TIME_FROM = CastFilter.DATE_FROM,
                    FINISH_TIME_TO = CastFilter.DATE_TO
                };
                listServiceReq = new HisServiceReqManager(paramGet).Get(metyFilterServiceReq);
                dicServiceReq = listServiceReq.GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.First());
                var listServiceReqId = dicServiceReq.Keys.Distinct().ToList();
                if (IsNotNullOrEmpty(listServiceReqId)) ListSereServ = GetSereServ(listServiceReqId);


                ProcessFilterData(listDepartmentTrans, ListTreatment, ListSereServ);

                result = true;

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
        private List<HIS_SERE_SERV> GetSereServ(List<long> listServiceReqId)
        {
            List<HIS_SERE_SERV> result = new List<HIS_SERE_SERV>();
            try
            {
                var skip = 0;
                while (listServiceReqId.Count - skip > 0)
                {
                    var listIDs = listServiceReqId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisSereServFilterQuery filterSereServ = new HisSereServFilterQuery();
                    filterSereServ.SERVICE_REQ_IDs = listIDs;
                    var listSereServSub = new HisSereServManager().Get(filterSereServ);
                    result.AddRange(listSereServSub);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<HIS_SERE_SERV>();
            }
            return result;
        }
        protected override bool ProcessData()
        {
            return true;
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("DEPARTMENT_ID", CastFilter.DEPARTMENT_ID.ToString() + " - " + DEPARTMENT_NAME);
            dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_FROM));
            dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TO));

            
            for (int i = 0; i < 31; i++)
            {
                dicSingleTag.Add(string.Format("DATE_{0}",i), listDate[i]);
            }
            objectTag.AddObjectData(store, "DepartmentTran", _listPatients);
            objectTag.AddObjectData(store, "Emergency", _listEmergencys);
            objectTag.AddObjectData(store, "Service", _listSarReportMrs00164Rdos);

        }

      

        // <~~~~~~~~~~~~~~~~~~~~~~~ HERE
        List<V_HIS_DEPARTMENT_TRAN_NEWS> listDepartmentTranNews = new List<V_HIS_DEPARTMENT_TRAN_NEWS>();
        List<V_HIS_EMERGENCY_NEWS> listEmergencyNews = new List<V_HIS_EMERGENCY_NEWS>();
        // <~~~~~~~~~~~~~~~~~~~~~~~ HERE
        private void ProcessFilterData(List<HIS_DEPARTMENT_TRAN> listDepartmentTrans, List<HIS_TREATMENT> ListTreatment, List<HIS_SERE_SERV> listSereServViews)
        {
            try
            {
                LogSystem.Info("Bat dau xu ly du lieu MRS00164 ===============================================================");
                var timeFrom = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(CastFilter.DATE_FROM);
                var timeTo = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(CastFilter.DATE_TO);
                var timex = timeFrom;
                for (int i = 0; i < 31; i++)
                {
                    var systemDateTimeToDateString = Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(timex);
                    listDate.Add(systemDateTimeToDateString);
                    timex = timex + new TimeSpan(1, 0, 0, 0);
                }

                //AddDate();

                // <~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ LIST PATIENT
                foreach (var listDepartmentTran in listDepartmentTrans)
                {
                    var dateToString = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listDepartmentTran.DEPARTMENT_IN_TIME ?? 0);
                    var departmentTranNew = new V_HIS_DEPARTMENT_TRAN_NEWS
                    {
                        DATE_STRING = dateToString,
                    };
                    listDepartmentTranNews.Add(departmentTranNew);
                }
                long[] listNum = new long[31];
                long TOTAL = 0;
                for (int i = 0; i < listDate.Count(); i++)
                {
                    listNum[i] = 0;
                    try
                    {
                        listNum[i] = listDepartmentTranNews.Where(s => s.DATE_STRING == listDate[i]).Count();
                    }
                    catch { };
                    TOTAL = TOTAL + listNum[i];
                }
                var rdo = new VSarReportMrs00164RDO
                {
                    OBJ_NAME = "Patient",
                    DATE_0 = listNum[0] > 0 ? listNum[0].ToString() : "",
                    DATE_1 = listNum[1] > 0 ? listNum[1].ToString() : "",
                    DATE_2 = listNum[2] > 0 ? listNum[2].ToString() : "",
                    DATE_3 = listNum[3] > 0 ? listNum[3].ToString() : "",
                    DATE_4 = listNum[4] > 0 ? listNum[4].ToString() : "",
                    DATE_5 = listNum[5] > 0 ? listNum[5].ToString() : "",
                    DATE_6 = listNum[6] > 0 ? listNum[6].ToString() : "",
                    DATE_7 = listNum[7] > 0 ? listNum[7].ToString() : "",
                    DATE_8 = listNum[8] > 0 ? listNum[8].ToString() : "",
                    DATE_9 = listNum[9] > 0 ? listNum[9].ToString() : "",
                    DATE_10 = listNum[10] > 0 ? listNum[10].ToString() : "",
                    DATE_11 = listNum[11] > 0 ? listNum[11].ToString() : "",
                    DATE_12 = listNum[12] > 0 ? listNum[12].ToString() : "",
                    DATE_13 = listNum[13] > 0 ? listNum[13].ToString() : "",
                    DATE_14 = listNum[14] > 0 ? listNum[14].ToString() : "",
                    DATE_15 = listNum[15] > 0 ? listNum[15].ToString() : "",
                    DATE_16 = listNum[16] > 0 ? listNum[16].ToString() : "",
                    DATE_17 = listNum[17] > 0 ? listNum[17].ToString() : "",
                    DATE_18 = listNum[18] > 0 ? listNum[18].ToString() : "",
                    DATE_19 = listNum[19] > 0 ? listNum[19].ToString() : "",
                    DATE_20 = listNum[20] > 0 ? listNum[20].ToString() : "",
                    DATE_21 = listNum[21] > 0 ? listNum[21].ToString() : "",
                    DATE_22 = listNum[22] > 0 ? listNum[22].ToString() : "",
                    DATE_23 = listNum[23] > 0 ? listNum[23].ToString() : "",
                    DATE_24 = listNum[24] > 0 ? listNum[24].ToString() : "",
                    DATE_25 = listNum[25] > 0 ? listNum[25].ToString() : "",
                    DATE_26 = listNum[26] > 0 ? listNum[26].ToString() : "",
                    DATE_27 = listNum[27] > 0 ? listNum[27].ToString() : "",
                    DATE_28 = listNum[28] > 0 ? listNum[28].ToString() : "",
                    DATE_29 = listNum[29] > 0 ? listNum[29].ToString() : "",
                    DATE_30 = listNum[30] > 0 ? listNum[30].ToString() : "",
                    TOTAL = TOTAL.ToString()
                };
                _listPatients.Add(rdo);
                // <~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ EMERGANCY
                foreach (var listEmergency in ListTreatment)
                {
                    var dateToString = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listEmergency.IN_TIME);
                    var emergency = new V_HIS_EMERGENCY_NEWS
                    {
                        DATE_STRING = dateToString,
                    };
                    listEmergencyNews.Add(emergency);
                }
                long[] listNum2 = new long[31];
                TOTAL = 0;
                for (int i = 0; i < listDate.Count(); i++)
                {
                    listNum2[i] = 0;
                    try
                    {
                        listNum2[i] = listEmergencyNews.Where(s => s.DATE_STRING == listDate[i]).Count();
                    }
                    catch { }
                    TOTAL = TOTAL + listNum2[i];
                }
                var rod = new VSarReportMrs00164RDO
                {
                    OBJ_ID = 0,
                    OBJ_NAME = "EMERGANCY",
                    DATE_0 = listNum2[0] > 0 ? listNum2[0].ToString() : "",
                    DATE_1 = listNum2[1] > 0 ? listNum2[1].ToString() : "",
                    DATE_2 = listNum2[2] > 0 ? listNum2[2].ToString() : "",
                    DATE_3 = listNum2[3] > 0 ? listNum2[3].ToString() : "",
                    DATE_4 = listNum2[4] > 0 ? listNum2[4].ToString() : "",
                    DATE_5 = listNum2[5] > 0 ? listNum2[5].ToString() : "",
                    DATE_6 = listNum2[6] > 0 ? listNum2[6].ToString() : "",
                    DATE_7 = listNum2[7] > 0 ? listNum2[7].ToString() : "",
                    DATE_8 = listNum2[8] > 0 ? listNum2[8].ToString() : "",
                    DATE_9 = listNum2[9] > 0 ? listNum2[9].ToString() : "",
                    DATE_10 = listNum2[10] > 0 ? listNum2[10].ToString() : "",
                    DATE_11 = listNum2[11] > 0 ? listNum2[11].ToString() : "",
                    DATE_12 = listNum2[12] > 0 ? listNum2[12].ToString() : "",
                    DATE_13 = listNum2[13] > 0 ? listNum2[13].ToString() : "",
                    DATE_14 = listNum2[14] > 0 ? listNum2[14].ToString() : "",
                    DATE_15 = listNum2[15] > 0 ? listNum2[15].ToString() : "",
                    DATE_16 = listNum2[16] > 0 ? listNum2[16].ToString() : "",
                    DATE_17 = listNum2[17] > 0 ? listNum2[17].ToString() : "",
                    DATE_18 = listNum2[18] > 0 ? listNum2[18].ToString() : "",
                    DATE_19 = listNum2[19] > 0 ? listNum2[19].ToString() : "",
                    DATE_20 = listNum2[20] > 0 ? listNum2[20].ToString() : "",
                    DATE_21 = listNum2[21] > 0 ? listNum2[21].ToString() : "",
                    DATE_22 = listNum2[22] > 0 ? listNum2[22].ToString() : "",
                    DATE_23 = listNum2[23] > 0 ? listNum2[23].ToString() : "",
                    DATE_24 = listNum2[24] > 0 ? listNum2[24].ToString() : "",
                    DATE_25 = listNum2[25] > 0 ? listNum2[25].ToString() : "",
                    DATE_26 = listNum2[26] > 0 ? listNum2[26].ToString() : "",
                    DATE_27 = listNum2[27] > 0 ? listNum2[27].ToString() : "",
                    DATE_28 = listNum2[28] > 0 ? listNum2[28].ToString() : "",
                    DATE_29 = listNum2[29] > 0 ? listNum2[29].ToString() : "",
                    DATE_30 = listNum2[30] > 0 ? listNum2[30].ToString() : "",
                    TOTAL = TOTAL.ToString()
                };
                _listEmergencys.Add(rod);
                // <~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ FIND PARENT OF MISU SERVICE
                var listSereServNews = new List<V_HIS_SERE_SERV_NEWS>();
                foreach (var ss in listSereServViews)
                {
                    var req = dicServiceReq.ContainsKey(ss.SERVICE_REQ_ID??0) ? dicServiceReq[ss.SERVICE_REQ_ID??0] : new HIS_SERVICE_REQ();
                    var sereServNew = new V_HIS_SERE_SERV_NEWS
                    {
                        FINISH_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(req.FINISH_TIME.Value),
                        AMOUNT = ss.AMOUNT,
                        SERE_SERV = ss
                    };
                    listSereServNews.Add(sereServNew);
                }
                // <~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ MISU_SERVICE
                var listSereServGroupByIds = listSereServNews.GroupBy(s => s.SERE_SERV.SERVICE_ID).ToList();
                foreach (var listSereServGroupById in listSereServGroupByIds)
                {
                    long[] listNum3 = new long[31];
                    TOTAL = 0;
                    for (int i = 0; i < listDate.Count(); i++)
                    {
                        listNum3[i] = 0;
                        try
                        {
                            var x = listSereServGroupById.Where(s => s.FINISH_TIME == listDate[i]);
                            listNum3[i] = (long)x.Sum(s => s.AMOUNT);
                        }
                        catch { }
                        TOTAL = TOTAL + listNum3[i];
                    }
                    var dro = new VSarReportMrs00164RDO
                    {
                        OBJ_ID = listSereServGroupById.First().SERE_SERV.SERVICE_ID,
                        OBJ_NAME = listSereServGroupById.First().SERE_SERV.TDL_SERVICE_NAME,
                        DATE_0 = listNum3[0] > 0 ? listNum3[0].ToString() : "",
                        DATE_1 = listNum3[1] > 0 ? listNum3[1].ToString() : "",
                        DATE_2 = listNum3[2] > 0 ? listNum3[2].ToString() : "",
                        DATE_3 = listNum3[3] > 0 ? listNum3[3].ToString() : "",
                        DATE_4 = listNum3[4] > 0 ? listNum3[4].ToString() : "",
                        DATE_5 = listNum3[5] > 0 ? listNum3[5].ToString() : "",
                        DATE_6 = listNum3[6] > 0 ? listNum3[6].ToString() : "",
                        DATE_7 = listNum3[7] > 0 ? listNum3[7].ToString() : "",
                        DATE_8 = listNum3[8] > 0 ? listNum3[8].ToString() : "",
                        DATE_9 = listNum3[9] > 0 ? listNum3[9].ToString() : "",
                        DATE_10 = listNum3[10] > 0 ? listNum3[10].ToString() : "",
                        DATE_11 = listNum3[11] > 0 ? listNum3[11].ToString() : "",
                        DATE_12 = listNum3[12] > 0 ? listNum3[12].ToString() : "",
                        DATE_13 = listNum3[13] > 0 ? listNum3[13].ToString() : "",
                        DATE_14 = listNum3[14] > 0 ? listNum3[14].ToString() : "",
                        DATE_15 = listNum3[15] > 0 ? listNum3[15].ToString() : "",
                        DATE_16 = listNum3[16] > 0 ? listNum3[16].ToString() : "",
                        DATE_17 = listNum3[17] > 0 ? listNum3[17].ToString() : "",
                        DATE_18 = listNum3[18] > 0 ? listNum3[18].ToString() : "",
                        DATE_19 = listNum3[19] > 0 ? listNum3[19].ToString() : "",
                        DATE_20 = listNum3[20] > 0 ? listNum3[20].ToString() : "",
                        DATE_21 = listNum3[21] > 0 ? listNum3[21].ToString() : "",
                        DATE_22 = listNum3[22] > 0 ? listNum3[22].ToString() : "",
                        DATE_23 = listNum3[23] > 0 ? listNum3[23].ToString() : "",
                        DATE_24 = listNum3[24] > 0 ? listNum3[24].ToString() : "",
                        DATE_25 = listNum3[25] > 0 ? listNum3[25].ToString() : "",
                        DATE_26 = listNum3[26] > 0 ? listNum3[26].ToString() : "",
                        DATE_27 = listNum3[27] > 0 ? listNum3[27].ToString() : "",
                        DATE_28 = listNum3[28] > 0 ? listNum3[28].ToString() : "",
                        DATE_29 = listNum3[29] > 0 ? listNum3[29].ToString() : "",
                        DATE_30 = listNum3[30] > 0 ? listNum3[30].ToString() : "",
                        TOTAL = TOTAL.ToString()
                    };
                    _listSarReportMrs00164Rdos.Add(dro);
                }
                // <~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                LogSystem.Info("Ket thuc xu ly du lieu MRS00164 ===============================================================");
            }
            catch (Exception ex)
            {
                LogSystem.Info("Loi trong qua trinh xu ly du lieu ===============================================================");
                LogSystem.Error(ex);
            }
        }



        class V_HIS_DEPARTMENT_TRAN_NEWS
        {
            public string DATE_STRING { get; set; }
        }
        class V_HIS_EMERGENCY_NEWS
        {
            public string DATE_STRING { get; set; }
        }
        class V_HIS_SERE_SERV_NEWS
        {
            public string FINISH_TIME { get; set; }
            public decimal AMOUNT { get; set; }
            public HIS_SERE_SERV SERE_SERV { get; set; }
        }
    }
}
