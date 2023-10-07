using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisServiceType;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using ACS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisServiceReqStt;
using ACS.MANAGER.Manager;
using ACS.MANAGER.Core.AcsUser.Get;

namespace MRS.Processor.Mrs00422
{
    class Mrs00422Processor : AbstractProcessor
    {
        Mrs00422Filter filter = null;
        List<Mrs00422RDO> ListRdo = new List<Mrs00422RDO>();
        CommonParam paramGet = new CommonParam();
        List<Mrs00422RDO> ListRdoServiceExecutor = new List<Mrs00422RDO>();
        List<Mrs00422RDO> ListRdoService = new List<Mrs00422RDO>();
        List<Mrs00422RDO> ParentServiceExecutor = new List<Mrs00422RDO>();
        List<Mrs00422RDO> MediumServiceExecutor = new List<Mrs00422RDO>();
        List<Mrs00422RDO> ParentService = new List<Mrs00422RDO>();
        List<HIS_SERVICE_TYPE> listServiceType = new List<HIS_SERVICE_TYPE>();
        List<HIS_SERVICE_REQ_STT> listHisServiceReqStt = new List<HIS_SERVICE_REQ_STT>();
        List<HIS_SERVICE> listHisService = new List<HIS_SERVICE>();

        List<Mrs00422ServiceDoctorReq> ListServiceDoctorReq = new List<Mrs00422ServiceDoctorReq>();
        List<ACS_USER> acsUsers = new List<ACS_USER>();

        public Mrs00422Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
        public override Type FilterType()
        {
            return typeof(Mrs00422Filter);
        }


        protected override bool GetData()
        {
            filter = ((Mrs00422Filter)reportFilter);
            bool result = true;
            try
            {
                HisServiceReqSttFilterQuery HisServiceReqSttfilter = new HisServiceReqSttFilterQuery();
                listHisServiceReqStt = new HisServiceReqSttManager().Get(HisServiceReqSttfilter);
                HisServiceFilterQuery HisServicefilter = new HisServiceFilterQuery();
                listHisService = new HisServiceManager().Get(HisServicefilter);
                ListRdo = new ManagerSql().GetSereServDO(filter);

                acsUsers = new ManagerSql().GetAcsUser(filter);
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
            var result = true;
            try
            {

                ListRdoServiceExecutor.Clear();

                if (IsNotNullOrEmpty(ListRdo))
                {
                    long PATIENT_TYPE_ID__BHYT = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                    long PATIENT_TYPE_ID__FEE = HisPatientTypeCFG.PATIENT_TYPE_ID__FEE;
                    long PATIENT_TYPE_ID__KSK = HisPatientTypeCFG.PATIENT_TYPE_ID__KSK;
                    long PATIENT_TYPE_ID__KSKHD = HisPatientTypeCFG.PATIENT_TYPE_ID__KSKHD;
                    long PATIENT_TYPE_ID__IS_FREE = HisPatientTypeCFG.PATIENT_TYPE_ID__IS_FREE;
                    List<HIS_SERVICE_TYPE> HisServiceTypes = HisServiceTypeCFG.HisServiceTypes;

                    foreach (var r in ListRdo)
                    {
                        var serviceReqStt = listHisServiceReqStt.FirstOrDefault(o => o.ID == r.SERVICE_REQ_STT_ID) ?? new HIS_SERVICE_REQ_STT();
                        var executeRoom = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == r.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM();
                        var serviceType = HisServiceTypes.FirstOrDefault(o => o.ID == r.SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE();
                        r.CHILD = Age(r.IN_TIME, r.TDL_PATIENT_DOB ?? 0) <= 15 ? r.AMOUNT : 0;
                        r.BHYT = r.TDL_PATIENT_TYPE_ID == PATIENT_TYPE_ID__BHYT ? r.AMOUNT : 0;
                        r.FEE = r.TDL_PATIENT_TYPE_ID == PATIENT_TYPE_ID__FEE ? r.AMOUNT : 0;
                        r.FREE = r.TDL_PATIENT_TYPE_ID == PATIENT_TYPE_ID__IS_FREE ? r.AMOUNT : 0;
                        r.KSK = r.TDL_PATIENT_TYPE_ID == PATIENT_TYPE_ID__KSK ? r.AMOUNT : 0;
                        r.KSK_HD = r.TDL_PATIENT_TYPE_ID == PATIENT_TYPE_ID__KSKHD ? r.AMOUNT : 0;
                        r.SERVICE_TYPE_CODE = serviceType.SERVICE_TYPE_CODE;
                        r.SERVICE_TYPE_NAME = serviceType.SERVICE_TYPE_NAME;
                        if (r.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                        {
                            r.IS_MALE = 1;
                        }
                        else
                            r.IS_FEMALE = 1;
                        r.EXECUTE_ROOM_NAME = executeRoom.ROOM_NAME;
                        r.SERVICE_REQ_STT_NAME = serviceReqStt.SERVICE_REQ_STT_NAME;
                        if (r.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
                        {
                            r.REQUEST_LOGINNAME = r.EXECUTE_LOGINNAME;
                            r.REQUEST_USERNAME = r.EXECUTE_USERNAME;
                        }
                    }

                    ListRdo = ListRdo.OrderBy(o => o.SERVICE_CODE).ToList();

                    ListRdoServiceExecutor = ListRdo.GroupBy(o => new { o.SERVICE_CODE, o.EXECUTE_LOGINNAME }).
                        Select(p => new Mrs00422RDO
                        {
                            SERVICE_CODE = p.First().SERVICE_CODE,
                            SERVICE_NAME = p.First().SERVICE_NAME,
                            PRICE = p.First().PRICE,
                            AMOUNT = p.Sum(s => s.AMOUNT),
                            EXECUTE_LOGINNAME = p.First().EXECUTE_LOGINNAME,
                            EXECUTE_USERNAME = p.First().EXECUTE_USERNAME,
                            CHILD = p.Sum(s => s.CHILD),
                            BHYT = p.Sum(s => s.BHYT),
                            FEE = p.Sum(s => s.FEE),
                            FREE = p.Sum(s => s.FREE),
                            KSK = p.Sum(s => s.KSK),
                            KSK_HD = p.Sum(s => s.KSK_HD),
                            SERVICE_TYPE_ID = p.First().SERVICE_TYPE_ID,
                            IS_MALE = p.Sum(s => s.IS_MALE),
                            IS_FEMALE = p.Sum(s => s.IS_FEMALE),
                            SERVICE_TYPE_CODE = p.First().SERVICE_TYPE_CODE,
                            SERVICE_TYPE_NAME = p.First().SERVICE_TYPE_NAME
                        }).ToList();
                    ParentServiceExecutor = ListRdoServiceExecutor.GroupBy(o => o.SERVICE_TYPE_ID).Select(p => p.First()).ToList();
                    MediumServiceExecutor = ListRdoServiceExecutor.GroupBy(o => new { o.EXECUTE_LOGINNAME, o.SERVICE_TYPE_ID }).Select(p => p.First()).ToList();

                    ListRdoService.Clear();
                    ListRdoService = ListRdo.GroupBy(o => o.SERVICE_CODE).
                         Select(p => new Mrs00422RDO
                         {
                             SERVICE_CODE = p.First().SERVICE_CODE,
                             SERVICE_NAME = p.First().SERVICE_NAME,
                             PRICE = p.First().PRICE,
                             AMOUNT = p.Sum(s => s.AMOUNT),
                             CHILD = p.Sum(s => s.CHILD),
                             BHYT = p.Sum(s => s.BHYT),
                             FEE = p.Sum(s => s.FEE),
                             FREE = p.Sum(s => s.FREE),
                             KSK = p.Sum(s => s.KSK),
                             KSK_HD = p.Sum(s => s.KSK_HD),
                             SERVICE_TYPE_ID = p.First().SERVICE_TYPE_ID,
                             IS_MALE = p.Sum(s => s.IS_MALE),
                             IS_FEMALE = p.Sum(s => s.IS_FEMALE),
                             SERVICE_TYPE_CODE = p.First().SERVICE_TYPE_CODE,
                             SERVICE_TYPE_NAME = p.First().SERVICE_TYPE_NAME
                         }).ToList();
                    ParentService = ListRdoService.GroupBy(o => o.SERVICE_TYPE_ID).Select(p => p.First()).ToList();

                    var groupByServiceAndPrice = ListRdo.GroupBy(o => new { o.SERVICE_ID, o.PRICE }).ToList();
                    foreach (var group in groupByServiceAndPrice)
                    {
                        Mrs00422ServiceDoctorReq rdo = new Mrs00422ServiceDoctorReq();
                        rdo.SERVICE_CODE = group.First().SERVICE_CODE;
                        rdo.SERVICE_NAME = group.First().SERVICE_NAME;
                        rdo.PRICE = group.Key.PRICE;
                        var service = listHisService.FirstOrDefault(o => o.ID == group.Key.SERVICE_ID);
                        if (service != null)
                        {
                            var serviceType = HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == service.SERVICE_TYPE_ID);
                            if (serviceType != null)
                            {
                                rdo.PARENT_NUM_ORDER = serviceType.ID;
                                var parent = listHisService.FirstOrDefault(o => o.ID == service.PARENT_ID);
                                if (parent != null)
                                {
                                    rdo.PARENT_SERVICE_CODE = parent.SERVICE_CODE;
                                    rdo.PARENT_SERVICE_NAME = parent.SERVICE_NAME;
                                }
                                else
                                {
                                    rdo.PARENT_SERVICE_CODE = serviceType.SERVICE_TYPE_CODE;
                                    rdo.PARENT_SERVICE_NAME = serviceType.SERVICE_TYPE_NAME;
                                }
                            }
                        }
                        rdo.AMOUNT = group.ToList<Mrs00422RDO>().Where(o => o.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Sum(s => s.AMOUNT);
                        rdo.AMOUNT_FROM_REA = group.ToList<Mrs00422RDO>().Where(o => o.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.REQUEST_ROOM_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD && o.SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Sum(s => s.AMOUNT);
                        rdo.DIC_REQUEST_LOGINNAME_AMOUNT = group.ToList<Mrs00422RDO>().Where(o => o.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && (o.REQUEST_ROOM_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD || o.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)).GroupBy(g => g.REQUEST_LOGINNAME??"NONE").ToDictionary(p => p.Key, p => p.Sum(s => s.AMOUNT));
                        if (rdo.PARENT_NUM_ORDER > 0)
                        {
                            ListServiceDoctorReq.Add(rdo);
                        }
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


        private int Age(long IN_TIME, long TDL_PATIENT_DOB)
        {
            return (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(IN_TIME) ?? DateTime.Now).Year - (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(TDL_PATIENT_DOB) ?? DateTime.Now).Year;
        }



        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (((Mrs00422Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)((Mrs00422Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00422Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)((Mrs00422Filter)reportFilter).TIME_TO));
            }
            Dictionary<string, string> dicUserName = ListRdo.GroupBy(g => g.REQUEST_LOGINNAME ?? "NONE").ToDictionary(p => p.Key, p => p.First().REQUEST_USERNAME);
            foreach (var item in dicUserName)
            {
                if (!dicSingleTag.ContainsKey(item.Key))
                {
                    dicSingleTag.Add(item.Key, item.Value);
                }
                else
                {
                    dicSingleTag[item.Key] = item.Value;
                }
            }
            objectTag.AddObjectData(store, "ReportExecutor", ListRdo);
            objectTag.AddObjectData(store, "ReportServiceExecutor", ListRdoServiceExecutor);
            objectTag.AddObjectData(store, "ParentServiceExecutor", ParentServiceExecutor);
            objectTag.AddObjectData(store, "MediumServiceExecutor", MediumServiceExecutor);

            string[] masterKeyFields = { "SERVICE_TYPE_ID", "EXECUTE_LOGINNAME" };
            objectTag.AddRelationship(store, "MediumServiceExecutor", "ReportServiceExecutor", masterKeyFields, masterKeyFields);
            objectTag.AddRelationship(store, "ParentServiceExecutor", "MediumServiceExecutor", "SERVICE_TYPE_ID", "SERVICE_TYPE_ID");

            objectTag.AddRelationship(store, "ReportServiceExecutor", "ReportExecutor", "SERVICE_CODE", "SERVICE_CODE");

            objectTag.AddObjectData(store, "ReportService", ListRdoService);
            objectTag.AddObjectData(store, "ParentService", ParentService);
            objectTag.AddRelationship(store, "ParentService", "ReportService", "SERVICE_TYPE_ID", "SERVICE_TYPE_ID");
            objectTag.AddObjectData(store, "ServiceDoctorReqs", ListServiceDoctorReq);
            objectTag.AddObjectData(store, "TypeDoctorReqs", ListServiceDoctorReq.GroupBy(o => o.PARENT_SERVICE_CODE).Select(p => p.First()).OrderBy(q => q.PARENT_NUM_ORDER).ToList());
            objectTag.AddRelationship(store, "TypeDoctorReqs", "ServiceDoctorReqs", "PARENT_SERVICE_CODE", "PARENT_SERVICE_CODE");
            ListRdo.Clear();
            ListRdo = null;
            objectTag.AddObjectData(store, "AcsUsers", acsUsers.Where(o=> dicUserName.ContainsKey(o.LOGINNAME)).ToList());
        }
    }

}
