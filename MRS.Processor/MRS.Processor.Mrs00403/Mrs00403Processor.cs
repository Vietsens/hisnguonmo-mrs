using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
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
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatmentBedRoom;
using MOS.MANAGER.HisBedRoom;

namespace MRS.Processor.Mrs00403
{
    class Mrs00403Processor : AbstractProcessor
    {
        Mrs00403Filter castFilter = null;
        List<Mrs00403RDO> listRdo = new List<Mrs00403RDO>();

        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>();
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlterGroups = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<V_HIS_EXP_MEST> listPrescriptions = new List<V_HIS_EXP_MEST>();
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>();

        Dictionary<long, List<HIS_SERVICE_REQ>> dicServiceReq = new Dictionary<long, List<HIS_SERVICE_REQ>>();
        Dictionary<long, List<HIS_TREATMENT_BED_ROOM>> dicTreatmentBedRoom = new Dictionary<long, List<HIS_TREATMENT_BED_ROOM>>();
        List<HIS_BED_ROOM> listBedRoom = new List<HIS_BED_ROOM>();

        public Mrs00403Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00403Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00403Filter)this.reportFilter;

                var skip = 0;

                HisTreatmentViewFilterQuery treatmentViewFilter = new HisTreatmentViewFilterQuery();
                treatmentViewFilter.IN_TIME_FROM = castFilter.TIME_FROM;
                treatmentViewFilter.IN_TIME_TO = castFilter.TIME_TO;

                listTreatments = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentViewFilter);

                var listTreatmentIds = listTreatments.Select(s => s.ID).ToList();
                while (listTreatmentIds.Count - skip > 0)
                {
                    var listTreatId = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisPatientTypeAlterViewFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery();
                    patientTypeAlterFilter.TREATMENT_IDs = listTreatId;
                    patientTypeAlterFilter.ORDER_DIRECTION = "DESC";
                    patientTypeAlterFilter.ORDER_FIELD = "LOG_TIME";

                    var listPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(patientTypeAlterFilter);

                    listPatientTypeAlters.AddRange(listPatientTypeAlter);

                    //lấy danh sách khám để lấy thông tin phòng khám cuối cùng
                    HisServiceReqFilterQuery serviceReqFilter = new HisServiceReqFilterQuery();
                    serviceReqFilter.TREATMENT_IDs = listTreatId;
                    serviceReqFilter.ORDER_DIRECTION = "DESC";
                    serviceReqFilter.ORDER_FIELD = "LOG_TIME";

                    var listServiceReqSub = new HisServiceReqManager(paramGet).Get(serviceReqFilter);
                    foreach (var item in listServiceReqSub)
                    {
                        if (!dicServiceReq.ContainsKey(item.TREATMENT_ID))
                        {
                            dicServiceReq[item.TREATMENT_ID] = new List<HIS_SERVICE_REQ>();
                        }
                        dicServiceReq[item.TREATMENT_ID].Add(item);
                    }

                    //lấy danh sách buồng bệnh để lấy buồng điều trị cuối cùng
                    HisTreatmentBedRoomFilterQuery treatmentBedRoomFilter = new HisTreatmentBedRoomFilterQuery();
                    treatmentBedRoomFilter.TREATMENT_IDs = listTreatId;
                    treatmentBedRoomFilter.ORDER_DIRECTION = "DESC";
                    treatmentBedRoomFilter.ORDER_FIELD = "LOG_TIME";

                    var listTreatmentBedRoomSub = new HisTreatmentBedRoomManager(paramGet).Get(treatmentBedRoomFilter);
                    foreach (var item in listTreatmentBedRoomSub)
                    {
                        if (!dicTreatmentBedRoom.ContainsKey(item.TREATMENT_ID))
                        {
                            dicTreatmentBedRoom[item.TREATMENT_ID] = new List<HIS_TREATMENT_BED_ROOM>();
                        }
                        dicTreatmentBedRoom[item.TREATMENT_ID].Add(item);
                    }

                }
                foreach (var listPatientTypeAlter_ in listPatientTypeAlters.GroupBy(x => x.TREATMENT_ID))
                {
                    var patientTypeAlter = listPatientTypeAlter_.First();
                    listPatientTypeAlterGroups.Add(patientTypeAlter);
                }

                var listTreatmentIds_ = listPatientTypeAlterGroups.Where(w => w.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).Select(s => s.TREATMENT_ID).ToList();
                listTreatments = listTreatments.Where(w => listTreatmentIds_.Contains(w.ID)).ToList();

                var listBestTreatmentIds = listTreatments.Select(s => s.ID).ToList();

                skip = 0;
                while (listBestTreatmentIds.Count - skip > 0)
                {
                    var listBestTreatmentId = listBestTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisExpMestViewFilterQuery prescriptionFilter = new HisExpMestViewFilterQuery();
                    prescriptionFilter.TDL_TREATMENT_IDs = listBestTreatmentId;
                    prescriptionFilter.EXP_MEST_TYPE_IDs = new List<long>() 
                    {
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT,
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT
                    };
                    var listPrescription = new HisExpMestManager(paramGet).GetView(prescriptionFilter);
                    if (listPrescription != null)
                    {
                        listPrescriptions.AddRange(listPrescription);
                    }
                    listPrescriptions = listPrescriptions.Where(o => o.EXP_MEST_STT_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__REQUEST || o.EXP_MEST_STT_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__EXECUTE).ToList();
                }

                //get bed room
                GetBedRoom();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetBedRoom()
        {
            listBedRoom = new HisBedRoomManager().Get(new HisBedRoomFilterQuery());
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(listTreatments))
                {
                    foreach (var treatment in listTreatments)
                    {
                        var patientTypeAlter = listPatientTypeAlters.FirstOrDefault(w => w.TREATMENT_ID == treatment.ID);
                        if (patientTypeAlter == null)
                        {
                            continue;
                        }
                        var prescription = listPrescriptions.Where(w => w.TDL_TREATMENT_ID == treatment.ID);

                        if (treatment.IS_ACTIVE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && prescription.Count() == 0)
                        {
                            continue;
                        }
                        Mrs00403RDO rdo = new Mrs00403RDO();
                        rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
                        rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                        rdo.AGE = treatment.TDL_PATIENT_DOB;
                        rdo.GENDER = treatment.TDL_PATIENT_GENDER_NAME;
                        rdo.ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                        if (patientTypeAlter.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.HEIN_CARD_NUMBER = patientTypeAlter.HEIN_CARD_NUMBER;
                        }
                        rdo.IN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.IN_TIME);
                        rdo.OUT_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME ?? 0);
                        var lastRoomId = treatment.END_ROOM_ID;
                        
                        rdo.DEPARTMENT_EXAM = treatment.END_ROOM_NAME;
                        if(rdo.DEPARTMENT_EXAM == null)
                        {
                            var bedRoomId = dicTreatmentBedRoom.ContainsKey(treatment.ID)?dicTreatmentBedRoom[treatment.ID].OrderBy(o=>o.ADD_TIME).Select(p=>p.BED_ROOM_ID).LastOrDefault():0;
                            if(bedRoomId>0)
                            {
                                var bedRoom = listBedRoom.FirstOrDefault(o => o.ID == bedRoomId);
                                if (bedRoom != null)
                                {
                                    rdo.DEPARTMENT_EXAM = bedRoom.BED_ROOM_NAME;
                                }
                            }
                        }
                        if (rdo.DEPARTMENT_EXAM == null)
                        {
                            var examRoomId = dicServiceReq.ContainsKey(treatment.ID) ? dicServiceReq[treatment.ID].Where(n=>n.SERVICE_REQ_TYPE_ID==IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH).OrderBy(o => !o.FINISH_TIME.HasValue).ThenBy(p => p.FINISH_TIME).Select(p => p.EXECUTE_ROOM_ID).LastOrDefault() : 0;
                            if (examRoomId > 0)
                            {
                                var examRoom = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == examRoomId);
                                if (examRoom != null)
                                {
                                    rdo.DEPARTMENT_EXAM = examRoom.ROOM_NAME;
                                }
                            }
                        }
                        if (treatment.IS_PAUSE!=IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                        {
                            rdo.NOT_OUT = "X";
                        }
                        if (treatment.IS_ACTIVE==IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE&& treatment.IS_PAUSE==IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                        {
                            rdo.NOT_LOCK_FEE = "X";
                        }
                        if (prescription.Count()>0)
                        {
                            rdo.NOT_GET_MEDICINE = "X";
                        }
                        listRdo.Add(rdo);
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                objectTag.AddObjectData(store, "Report", listRdo);
                //objectTag.AddObjectData(store, "Group", listRdoGroup.OrderBy(s => s.GROUP_DATE).ToList()); 
                //bool exportSuccess = true; 
                //exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group", "Report", "GROUP_DATE", "GROUP_DATE"); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
