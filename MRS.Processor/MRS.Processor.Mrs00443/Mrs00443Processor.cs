using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisBranch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisInvoice;
using MOS.MANAGER.HisInvoiceDetail;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using ACS.EFMODEL.DataModels;
using ACS.Filter;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisExpMest;
using ACS.MANAGER.Manager;
using ACS.MANAGER.Core.AcsUser.Get;

namespace MRS.Processor.Mrs00443
{
    public class Mrs00443Processor : AbstractProcessor
    {
        private List<Mrs00443RDO> ListRdo = new List<Mrs00443RDO>();
        private List<V_HIS_EXP_MEST> ListPrescription = new List<V_HIS_EXP_MEST>();
        private List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();
        private List<HIS_TREATMENT> ListTreatmentCome = new List<HIS_TREATMENT>();
        List<ACS_USER> listAcsUser = new List<ACS_USER>();
        private List<V_HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<HIS_SERVICE_REQ> ListServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_DEPARTMENT> listDepartments = new List<HIS_DEPARTMENT>();
        List<HIS_BRANCH> branch = new List<HIS_BRANCH>();

        CommonParam paramGet = new CommonParam();
        public Mrs00443Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00443Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                //get dữ liệu:

                HisBranchFilterQuery branchFilter = new HisBranchFilterQuery();
                branchFilter.ID = ((Mrs00443Filter)this.reportFilter).BRANCH_ID;
                branch = new MOS.MANAGER.HisBranch.HisBranchManager(paramGet).Get(branchFilter);
                //Khoa theo co so
                HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery();
                departmentFilter.BRANCH_ID = ((Mrs00443Filter)this.reportFilter).BRANCH_ID;
                listDepartments = new MOS.MANAGER.HisDepartment.HisDepartmentManager(paramGet).Get(departmentFilter);
                var listDepartmentIds = listDepartments.Select(s => s.ID).ToList();

                //Phong cap cuu
                HisExecuteRoomFilterQuery roomFilter = new HisExecuteRoomFilterQuery();
                roomFilter.IS_EMERGENCY = true;
                var listRoomEmes = new MOS.MANAGER.HisExecuteRoom.HisExecuteRoomManager(paramGet).Get(roomFilter);
                var listRoomEmeIds = listRoomEmes.Where(w => w.ROOM_ID != null).Select(s => s.ROOM_ID).ToList();

                AcsUserFilterQuery AUfilter = new AcsUserFilterQuery();
                listAcsUser = new AcsUserManager(paramGet).Get<List<ACS_USER>>(AUfilter);
                HisExpMestViewFilterQuery PrescriptionViewFilter = new HisExpMestViewFilterQuery()
                {
                    CREATE_TIME_FROM = ((Mrs00443Filter)this.reportFilter).TIME_FROM,
                    CREATE_TIME_TO = ((Mrs00443Filter)this.reportFilter).TIME_TO,
                    EXP_MEST_TYPE_IDs = new List<long>() 
                    { 
                        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,
                        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT,
                        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT
                    }
                };
                ListPrescription = new HisExpMestManager(paramGet).GetView(PrescriptionViewFilter);
                List<long> ListTreatmentId = ListPrescription.GroupBy(o => o.TDL_TREATMENT_ID.Value).Select(p => p.First()).Select(q => q.TDL_TREATMENT_ID.Value).ToList();
                if (IsNotNullOrEmpty(ListTreatmentId))
                {
                    var skip = 0;
                    while (ListTreatmentId.Count - skip > 0)
                    {
                        var listIDs = ListTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisTreatmentFilterQuery ListTreatmentfilter = new HisTreatmentFilterQuery()
                        {
                            IDs = listIDs,
                            IS_OUT = true
                        };
                        var ListreatmentSub = new HisTreatmentManager(paramGet).Get(ListTreatmentfilter);
                        ListTreatment.AddRange(ListreatmentSub);
                    }
                    ListTreatmentId = ListTreatment.GroupBy(p => p.TREATMENT_CODE).Select(o => o.First().ID).ToList();
                }

                if (IsNotNullOrEmpty(ListTreatmentId))
                {
                    var skip = 0;
                    while (ListTreatmentId.Count - skip > 0)
                    {
                        var listIDs = ListTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientTypeAlterViewFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery()
                        {
                            TREATMENT_IDs = listIDs,
                            //TREATMENT_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM
                        };
                        var LisPatientTypeAlterSub = new HisPatientTypeAlterManager(paramGet).GetView(patientTypeAlterFilter);
                        LisPatientTypeAlter.AddRange(LisPatientTypeAlterSub);
                    }
                }

                List<long> ListTreatmentId2 = LisPatientTypeAlter.Where(p => p.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || p.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).Select(o => o.TREATMENT_ID).ToList();
                ListPrescription = ListPrescription.Where(o => !ListTreatmentId2.Contains(o.TDL_TREATMENT_ID.Value) && listDepartmentIds.Contains(o.REQ_DEPARTMENT_ID) && !listRoomEmeIds.Contains(o.REQ_ROOM_ID)).ToList();
                //Danh sách các bệnh nhân đã đến:

                HisTreatmentFilterQuery ListTreatmentComefilter = new HisTreatmentFilterQuery()
                {
                    IN_TIME_FROM = ((Mrs00443Filter)this.reportFilter).TIME_FROM,
                    IN_TIME_TO = ((Mrs00443Filter)this.reportFilter).TIME_TO
                };

                ListTreatmentCome = new HisTreatmentManager(paramGet).Get(ListTreatmentComefilter);
                List<long> ListTreatmentComeId = ListTreatmentCome.Select(o => o.ID).ToList();
                //Danh sách các dịch vụ với các bn đó:
                if (IsNotNullOrEmpty(ListTreatmentComeId))
                {
                    var skip = 0;
                    while (ListTreatmentComeId.Count - skip > 0)
                    {
                        var listIDs = ListTreatmentComeId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var HisServiceReqfilter = new HisServiceReqFilterQuery()
                        {
                            TREATMENT_IDs = listIDs
                        };
                        var listServiceReqSub = new HisServiceReqManager(paramGet).Get(HisServiceReqfilter);
                        ListServiceReq.AddRange(listServiceReqSub);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Info("????????????????????????????????????????3");
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                ListRdo.Clear();
                if (IsNotNullOrEmpty(ListPrescription))
                {
                    var groupByCreater = ListPrescription.GroupBy(o => new { o.CREATOR, o.REQ_ROOM_ID }).ToList();
                    foreach (var group in groupByCreater)
                    {
                        List<V_HIS_EXP_MEST> listSub = group.ToList<V_HIS_EXP_MEST>();
                        Mrs00443RDO rdo = new Mrs00443RDO()
                        {
                            LOGINNAME = listSub.First().CREATOR,
                            USERNAME = listAcsUser.Where(o => o.LOGINNAME == listSub.First().CREATOR).Select(p => p.USERNAME).First(),
                            ROOMNAME = listSub.First().REQ_ROOM_NAME,
                            AMOUNT = listSub.Count,
                            AMOUNT_EXAM_SERVICE = ListServiceReq
                            .Where(o => o.EXECUTE_LOGINNAME == listSub.First().CREATOR && o.REQUEST_ROOM_ID == listSub.First().REQ_ROOM_ID
                                     && o.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH
                                     && o.FINISH_TIME != null)
                                     .ToList().Count,
                            AMOUNT_IN_TREAT = ListTreatmentCome
                            .Where(o => (o.OUT_TIME != null
                                     || o.CLINICAL_IN_TIME != null)
                                     && ListServiceReq.Where(p => p.EXECUTE_LOGINNAME == listSub.First().CREATOR
                                         && p.REQUEST_ROOM_ID == listSub.First().REQ_ROOM_ID
                                                && p.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH
                                     && p.FINISH_TIME != null).Select(p => p.TREATMENT_ID).ToList().Contains(o.ID))
                                     .ToList().Count,
                            //TOTAL_PRICE = listSub.Select(o => o.).Sum()
                        };
                        ListRdo.Add(rdo);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00443Filter)this.reportFilter).TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00443Filter)this.reportFilter).TIME_TO));
            dicSingleTag.Add("BRANCH", branch.First().BRANCH_NAME);
            objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(o => o.LOGINNAME).ToList());
        }
    }
}
