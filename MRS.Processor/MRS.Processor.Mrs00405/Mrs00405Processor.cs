using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisPatientTypeAlter;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceReq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisTreatment;

namespace MRS.Processor.Mrs00405
{
    class Mrs00405Processor : AbstractProcessor
    {
        Mrs00405Filter castFilter = null;

        List<Mrs00405RDO> ListRdo = new List<Mrs00405RDO>();

        List<V_HIS_TREATMENT_FEE> listTreatments = new List<V_HIS_TREATMENT_FEE>();
        List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters = new List<HIS_PATIENT_TYPE_ALTER>();
        List<V_HIS_DEPARTMENT_TRAN> listDepartmentTran = new List<V_HIS_DEPARTMENT_TRAN>();
        List<HIS_SERVICE_REQ> listExamServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_SERE_SERV> listSerServ = new List<HIS_SERE_SERV>();
        Dictionary<long, V_HIS_DEPARTMENT_TRAN> dicDepartmentTran = new Dictionary<long, V_HIS_DEPARTMENT_TRAN>();
        Dictionary<long, HIS_PATIENT_TYPE_ALTER> dicCurrentPatyAlter = new Dictionary<long, HIS_PATIENT_TYPE_ALTER>();
        public Mrs00405Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00405Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                //HSDT theo thoi gian vao
                this.castFilter = (Mrs00405Filter)this.reportFilter;
                HisTreatmentFeeViewFilterQuery treatmentViewFilter = new HisTreatmentFeeViewFilterQuery();
                treatmentViewFilter.IN_TIME_FROM = castFilter.TIME_FROM;
                treatmentViewFilter.IN_TIME_TO = castFilter.TIME_TO;
                var treatments = new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetFeeView(treatmentViewFilter);
                listTreatments = treatments.Where(o => o.IS_PAUSE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                if (castFilter.IS_ALL == true)
                {

                    listTreatments = treatments;
                }
                if (this.castFilter.BRANCH_ID != null)
                {
                    listTreatments = listTreatments.Where(o => o.BRANCH_ID == this.castFilter.BRANCH_ID).ToList();
                }

                var treatmentIds = listTreatments.Select(o => o.ID).Distinct().ToList();

                //YC-DV
                if (IsNotNullOrEmpty(treatmentIds))
                {
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var IdSubs = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var sereservfilter = new HisSereServFilterQuery()
                        {
                            TREATMENT_IDs = IdSubs,
                            HAS_EXECUTE = true,
                            IS_EXPEND = false
                        };

                        var sereServSub = new HisSereServManager(param).Get(sereservfilter);
                        if (sereServSub != null)
                        {
                            listSerServ.AddRange(sereServSub);
                        }
                    }
                }
                listTreatments = listTreatments.Where(o => listSerServ.Exists(p => p.TDL_TREATMENT_ID == o.ID)).ToList();

                //chuyen doi tuong
                if (IsNotNullOrEmpty(listTreatments))
                {
                    var skip = 0;
                    while (listTreatments.Count - skip > 0)
                    {
                        var listIds = listTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisPatientTypeAlterFilterQuery patientTypeAlterViewFilter = new HisPatientTypeAlterFilterQuery();
                        patientTypeAlterViewFilter.TREATMENT_IDs = listIds.Select(s => s.ID).ToList();
                        listPatientTypeAlters.AddRange(new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).Get(patientTypeAlterViewFilter));
                    }

                }

                //chuyen khoa
                if (IsNotNullOrEmpty(listTreatments))
                {
                    var skip = 0;
                    while (listTreatments.Count - skip > 0)
                    {
                        var listIds = listTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisDepartmentTranViewFilterQuery DepartmentTranFilter = new HisDepartmentTranViewFilterQuery();
                        DepartmentTranFilter.TREATMENT_IDs = listIds.Select(s => s.ID).ToList();
                        listDepartmentTran.AddRange(new HisDepartmentTranManager(param).GetView(DepartmentTranFilter));
                    }
                }

                //yeu cau kham
                if (IsNotNullOrEmpty(listTreatments))
                {
                    var skip = 0;
                    while (listTreatments.Count - skip > 0)
                    {
                        var listIds = listTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisServiceReqFilterQuery ExamServiceReqFilter = new HisServiceReqFilterQuery();
                        ExamServiceReqFilter.HAS_EXECUTE = true;
                        ExamServiceReqFilter.TREATMENT_IDs = listIds.Select(s => s.ID).ToList();
                        ExamServiceReqFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                        listExamServiceReq.AddRange(new HisServiceReqManager(param).Get(ExamServiceReqFilter));
                    }
                }
                result = true;
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
                GetPatientTypeAlter();
                GetDepartmentTran();
                var examRoomFilter = new HisExecuteRoomFilterQuery();
                examRoomFilter.IS_EXAM = true;
                var listExamRoom = new HisExecuteRoomManager(param).Get(examRoomFilter);

                if (!IsNotNullOrEmpty(listTreatments)) return true;

                foreach (var treatment in listTreatments)
                {
                    HIS_SERVICE_REQ ExamServiceReq = listExamServiceReq.OrderBy(p => p.INTRUCTION_TIME).LastOrDefault(o => o.TREATMENT_ID == treatment.ID) ?? new HIS_SERVICE_REQ();
                    List<HIS_SERE_SERV> sereServSub = listSerServ.Where(o => o.TDL_TREATMENT_ID == treatment.ID).ToList();
                    if (this.castFilter.HAS_ZERO_TOTAL_PRICE!=true)
                    {
                        if (sereServSub.Sum(s => s.VIR_TOTAL_PRICE) == 0)
                        {
                            continue;
                        }
                    }
                    if (!dicCurrentPatyAlter.ContainsKey(treatment.ID))
                    {
                        continue;
                    }
                   
                    if (IsNotNullOrEmpty(this.castFilter.TREATMENT_TYPE_IDs))
                    {
                        if (!this.castFilter.TREATMENT_TYPE_IDs.Contains(dicCurrentPatyAlter[treatment.ID].TREATMENT_TYPE_ID))
                        {
                            continue;
                        }
                    }
                    if (IsNotNullOrEmpty(this.castFilter.PATIENT_TYPE_ID_WITH_CARDs))
                    {
                        if (!this.castFilter.PATIENT_TYPE_ID_WITH_CARDs.Contains(dicCurrentPatyAlter[treatment.ID].PATIENT_TYPE_ID))
                        {
                            continue;
                        }
                    }
                    if (IsNotNullOrEmpty(this.castFilter.PATIENT_TYPE_ID_WITH_SERVICEs))
                    {
                        if (!sereServSub.Exists(o => this.castFilter.PATIENT_TYPE_ID_WITH_SERVICEs.Contains(o.PATIENT_TYPE_ID)))
                        {
                            continue;
                        }
                    }
                    var rdo = new Mrs00405RDO();
                    rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
                    rdo.IS_PAUSE = treatment.IS_PAUSE;
                    rdo.IS_ACTIVE = treatment.IS_ACTIVE;
                    rdo.IS_LOCK_HEIN = treatment.IS_LOCK_HEIN;
                    rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                    rdo.DOB = treatment.TDL_PATIENT_DOB;
                    rdo.GENDER_NAME = treatment.TDL_PATIENT_GENDER_NAME;
                    rdo.ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                    var patientTypeAlter = listPatientTypeAlters.Where(s => s.TREATMENT_ID == treatment.ID).OrderByDescending(o => o.LOG_TIME).ToList();
                    rdo.HEIN_CARD_NUMBER = patientTypeAlter.Count > 0 ? patientTypeAlter.First().HEIN_CARD_NUMBER : "";

                    rdo.IN_TIME = treatment.IN_TIME;
                    if (dicCurrentPatyAlter[treatment.ID].TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                    {
                        var sub = listExamRoom.Where(o => o.ROOM_ID == ExamServiceReq.EXECUTE_ROOM_ID).ToList();
                        rdo.EXECUTE_ROOM_NAME = sub.Count > 0 ? sub.First().EXECUTE_ROOM_NAME : "";
                    }
                    else rdo.EXECUTE_ROOM_NAME = dicDepartmentTran.ContainsKey(treatment.ID) ? dicDepartmentTran[treatment.ID].DEPARTMENT_NAME : "";

                    ListRdo.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
        private void GetPatientTypeAlter()
        {
            try
            {
                if (IsNotNullOrEmpty(this.listPatientTypeAlters))
                {
                    var Groups = listPatientTypeAlters.GroupBy(o => o.TREATMENT_ID).ToList();
                    foreach (var group in Groups)
                    {
                        var listGroup = group.ToList<HIS_PATIENT_TYPE_ALTER>().OrderBy(o => o.LOG_TIME).ToList();
                        if (!dicCurrentPatyAlter.ContainsKey(listGroup.First().TREATMENT_ID)) dicCurrentPatyAlter[listGroup.First().TREATMENT_ID] = new HIS_PATIENT_TYPE_ALTER();
                        dicCurrentPatyAlter[listGroup.First().TREATMENT_ID] = listGroup.Last();
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void GetDepartmentTran()
        {
            try
            {
                if (IsNotNullOrEmpty(this.listDepartmentTran))
                {
                    var Groups = listDepartmentTran.GroupBy(o => o.TREATMENT_ID).ToList();
                    foreach (var group in Groups)
                    {
                        var listGroup = group.ToList<V_HIS_DEPARTMENT_TRAN>().OrderBy(o => o.DEPARTMENT_IN_TIME).ToList();
                        if (!dicDepartmentTran.ContainsKey(listGroup.First().TREATMENT_ID)) dicDepartmentTran[listGroup.First().TREATMENT_ID] = new V_HIS_DEPARTMENT_TRAN();
                        dicDepartmentTran[listGroup.First().TREATMENT_ID] = listGroup.Last();
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                bool exportSuccess = true;
                Inventec.Common.Logging.LogSystem.Info(ListRdo.Count.ToString());

                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Rdo", ListRdo.OrderBy(s => s.PATIENT_NAME).Where(o => o.IS_PAUSE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList());
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "RdoPause", ListRdo.OrderBy(s => s.PATIENT_NAME).Where(o => o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList());
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "RdoNotActive", ListRdo.OrderBy(s => s.PATIENT_NAME).Where(o => o.IS_ACTIVE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.IS_LOCK_HEIN != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList());
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "RdoLockHein", ListRdo.OrderBy(s => s.PATIENT_NAME).Where(o => o.IS_LOCK_HEIN == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList());
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
