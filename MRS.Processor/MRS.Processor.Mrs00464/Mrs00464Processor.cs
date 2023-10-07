using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartmentTran;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Treatment.DateTime;

namespace MRS.Processor.Mrs00464
{
    class Mrs00464Processor : AbstractProcessor
    {
        Mrs00464Filter filter = null;
        List<Mrs00464RDO> listRdoTreatment = new List<Mrs00464RDO>();
        List<Mrs00464RDO> listRdoTreatment1 = new List<Mrs00464RDO>();
        List<GROUP_ICD> listRdoGroup = new List<GROUP_ICD>();
        List<INFO_EXAM_EYE> listExamEye = new List<INFO_EXAM_EYE>();

        //List<HIS_TREATMENT> listTreatments = new List<HIS_TREATMENT>();
        List<PATIENT_TYPE_ALTER> listHisPatientTypeAlterTreat = new List<PATIENT_TYPE_ALTER>();
        List<DEPARTMENT_TRAN> listHisDepartmentTranAll = new List<DEPARTMENT_TRAN>();
        List<V_HIS_ROOM> listRooms = new List<V_HIS_ROOM>();
        List<HIS_DEPARTMENT> listDepartments = new List<HIS_DEPARTMENT>();
        List<HIS_ICD> listIcds = new List<HIS_ICD>();
        //List<PATIENT> listHisPatient = new List<PATIENT>();
        List<HIS_DEPARTMENT> KCCDepartments = new List<HIS_DEPARTMENT>();
        List<HIS_DEPARTMENT> CLNDepartment = new List<HIS_DEPARTMENT>();
        List<HIS_PTTT_METHOD> listPtttMethod = new List<HIS_PTTT_METHOD>();
        Dictionary<string, object> dicMini = new Dictionary<string, object>();
        string DEPARTMENT_CODE__OUTPATIENTs = "DEPARTMENT_CODE__OUTPATIENTs";

        string thisReportTypeCode = "";
        public Mrs00464Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00464Filter);
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                
                if (filter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM));
                }
                if (filter.TIME_TO > 0)
                {
                    dicSingleTag.Add("DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO));
                }

                bool exportSuccess = true;
                if (filter.DEPARTMENT_IDs != null)
                {
                    listRdoTreatment = listRdoTreatment.Where(o => filter.DEPARTMENT_IDs.Contains(o.ON_DEPARTMENT_ID)).ToList();
                    listRdoTreatment1 = listRdoTreatment.Where(o => filter.DEPARTMENT_IDs.Contains(o.PREVIOUS_DEPARTMENT_ID)).ToList();
                }

                objectTag.AddObjectData(store, "Treatment", listRdoTreatment
                    .OrderBy(p => p.IN_CODE)
                    .Where(o => o.CLINICAL_DEPARTMENT_ID > 0).ToList());

                objectTag.AddObjectData(store, "Treatment1", listRdoTreatment1
                    .OrderBy(p => p.IN_CODE)
                    .Where(o => o.CLINICAL_DEPARTMENT_ID > 0).ToList());

                objectTag.AddObjectData(store, "Dttt", listRdoTreatment.Where(o => o.ICD_GROUP_CODE == "DTTT").ToList());

                objectTag.AddObjectData(store, "GroupIcd", listRdoGroup);

                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.filter = (Mrs00464Filter)this.reportFilter;

                Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => filter), filter));
                CLNDepartment = new HisDepartmentManager().Get(new HisDepartmentFilterQuery() { IS_CLINICAL = true });
                listRooms = new MOS.MANAGER.HisRoom.HisRoomManager(param).GetView(new HisRoomViewFilterQuery());

                listIcds = new MOS.MANAGER.HisIcd.HisIcdManager(param).Get(new HisIcdFilterQuery());

                List<string> IcdCodeDttts = new List<string>();
                List<string> IcdCodeMos = new List<string>();
                List<string> IcdCodeGls = new List<string>();
                List<string> IcdCodeQus = new List<string>();
                List<HIS_ICD> listIcd = new HisIcdManager().Get(new HisIcdFilterQuery());

                if (!string.IsNullOrWhiteSpace(filter.PREFIX_ICD_CODE__DTTTs))
                {
                    string[] IcdCodes = filter.PREFIX_ICD_CODE__DTTTs.Split(',');
                    IcdCodeDttts = listIcd.Where(o => IcdCodes.ToList().Exists(p => o.ICD_CODE.StartsWith(p))).Select(q => q.ICD_CODE).ToList();
                }
                if (!string.IsNullOrWhiteSpace(filter.PREFIX_ICD_CODE__MOs))
                {
                    string[] IcdCodes = filter.PREFIX_ICD_CODE__MOs.Split(',');
                    IcdCodeMos = listIcd.Where(o => IcdCodes.ToList().Exists(p => o.ICD_CODE.StartsWith(p))).Select(q => q.ICD_CODE).ToList();
                }
                if (!string.IsNullOrWhiteSpace(filter.PREFIX_ICD_CODE__GLs))
                {
                    string[] IcdCodes = filter.PREFIX_ICD_CODE__GLs.Split(',');
                    IcdCodeGls = listIcd.Where(o => IcdCodes.ToList().Exists(p => o.ICD_CODE.StartsWith(p))).Select(q => q.ICD_CODE).ToList();
                }
                if (!string.IsNullOrWhiteSpace(filter.PREFIX_ICD_CODE__QUs))
                {
                    string[] IcdCodes = filter.PREFIX_ICD_CODE__QUs.Split(',');
                    IcdCodeQus = listIcd.Where(o => IcdCodes.ToList().Exists(p => o.ICD_CODE.StartsWith(p))).Select(q => q.ICD_CODE).ToList();
                }
                listRdoTreatment = new ManagerSql().GetTreatment(filter, IcdCodeDttts, IcdCodeMos, IcdCodeQus, IcdCodeGls);
                listHisDepartmentTranAll = new ManagerSql().GetDepartmentTran(filter);
                listHisPatientTypeAlterTreat = new ManagerSql().GetPatientTypeAlter(filter);
                if(filter.RELATIONSHIP_APPOINT==true)
                {
                    listExamEye = new ManagerSql().GetInfoExamEye(filter);
                }    
                listPtttMethod = HisPtttMethodCFG.PTTT_METHODs;
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
                if (IsNotNullOrEmpty(listRdoTreatment))
                {
                    //Khoa kham cap cuu khong dieu tri noi tru, neu dieu tri noi tru thi coi nhu cua khoa tiep theo
                    if (filter.DEPARTMENT_CODE__OUTPATIENTs != null)
                    {
                        List<string> KCCDepartmentCodes = filter.DEPARTMENT_CODE__OUTPATIENTs.Split(',').ToList();
                        KCCDepartments = HisDepartmentCFG.DEPARTMENTs.Where(o => KCCDepartmentCodes.Contains(o.DEPARTMENT_CODE)).ToList();
                        if (KCCDepartments != null)
                        {
                            foreach (var item in listHisDepartmentTranAll.OrderBy(p => p.ID).Where(o => KCCDepartments.Exists(p => p.ID == o.DEPARTMENT_ID) && listHisPatientTypeAlterTreat.Exists(p => p.DEPARTMENT_TRAN_ID == o.ID && (p.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || p.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU))).ToList())
                            {

                                var nextDepartmentTran = NextDepartment(item, listHisDepartmentTranAll);
                                if (nextDepartmentTran.ID > 0 && KCCDepartments.Exists(p => p.ID == nextDepartmentTran.DEPARTMENT_ID))
                                {
                                    //listHisDepartmentTranAll.Remove(nextDepartmentTran);
                                    nextDepartmentTran = NextDepartment(nextDepartmentTran, listHisDepartmentTranAll);
                                }
                                if (nextDepartmentTran.ID > 0)
                                {

                                    var patientTypeAlter = listHisPatientTypeAlterTreat.OrderBy(p => p.LOG_TIME).FirstOrDefault(o => o.TREATMENT_ID == item.TREATMENT_ID && (o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU));
                                    if (patientTypeAlter != null && item.ID == patientTypeAlter.DEPARTMENT_TRAN_ID)
                                    {

                                        var treatment = listRdoTreatment.FirstOrDefault(o => o.ID == item.TREATMENT_ID);
                                        if (treatment != null)
                                        {
                                            if (treatment.CLINICAL_IN_TIME < nextDepartmentTran.DEPARTMENT_IN_TIME)
                                            {
                                                treatment.CLINICAL_IN_TIME = nextDepartmentTran.DEPARTMENT_IN_TIME;
                                            }
                                        }
                                        if (patientTypeAlter.DEPARTMENT_TRAN_ID < nextDepartmentTran.ID)
                                        {
                                            patientTypeAlter.DEPARTMENT_TRAN_ID = nextDepartmentTran.ID;
                                        }
                                        if (patientTypeAlter.LOG_TIME < nextDepartmentTran.DEPARTMENT_IN_TIME)
                                        {
                                            patientTypeAlter.LOG_TIME = nextDepartmentTran.DEPARTMENT_IN_TIME ?? 0;
                                        }
                                    }
                                    listHisDepartmentTranAll.Remove(item);
                                }
                                else
                                {

                                    listHisDepartmentTranAll = listHisDepartmentTranAll.Where(o => o.TREATMENT_ID != item.TREATMENT_ID).ToList();
                                }
                            }
                        }
                    }
                    if (HisDepartmentCFG.HIS_DEPARTMENT_ID__LIST_RESUSCITATION != null && HisDepartmentCFG.HIS_DEPARTMENT_ID__LIST_RESUSCITATION.Count > 0)
                    {
                        List<long> listNoTreatment = HisDepartmentCFG.HIS_DEPARTMENT_ID__LIST_RESUSCITATION;
                        foreach (var item in listHisDepartmentTranAll.OrderBy(p => p.ID).Where(o => listNoTreatment.Contains(o.DEPARTMENT_ID) &&
                        (HasTreatIn(o, listHisPatientTypeAlterTreat)
                        || HasTreatOut(o, listHisPatientTypeAlterTreat))).ToList())
                        {
                            var previousDepartmentTran = listHisDepartmentTranAll.FirstOrDefault(o => o.ID == item.PREVIOUS_ID);
                            var nextDepartmentTran = NextDepartment(item, listHisDepartmentTranAll);
                            if (nextDepartmentTran != null && nextDepartmentTran.ID > 0)
                            {
                                if (previousDepartmentTran != null && previousDepartmentTran.ID > 0)
                                {
                                    nextDepartmentTran.PREVIOUS_ID = previousDepartmentTran.ID;
                                }

                                nextDepartmentTran.DEPARTMENT_IN_TIME = item.DEPARTMENT_IN_TIME;
                                var patientTypeAlter = listHisPatientTypeAlterTreat.OrderBy(p => p.LOG_TIME).FirstOrDefault(o => o.TREATMENT_ID == item.TREATMENT_ID && (o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU));
                                if (patientTypeAlter != null && item.ID == patientTypeAlter.DEPARTMENT_TRAN_ID)
                                {

                                    if (patientTypeAlter.DEPARTMENT_TRAN_ID < nextDepartmentTran.ID)
                                    {
                                        patientTypeAlter.DEPARTMENT_TRAN_ID = nextDepartmentTran.ID;
                                    }
                                }
                            }
                            listHisDepartmentTranAll.Remove(item);


                        }
                    }

                    var listHisDepartmentTran = listHisDepartmentTranAll.Where(o => CLNDepartment.Exists(q => q.ID == o.DEPARTMENT_ID)).ToList();

                    //chi lay cac vao khoa co thoi gian ra khoa sau time_from
                    listHisDepartmentTran = listHisDepartmentTran.Where(o => (NextDepartment(o, listHisDepartmentTranAll).DEPARTMENT_IN_TIME ?? 99999999999999) >= filter.TIME_FROM).ToList();

                    foreach (var treatment in listRdoTreatment)
                    {
                        if (filter.RELATIONSHIP_APPOINT == true)
                        {
                            var listExamEyeSub = listExamEye.Where(o => o.TREATMENT_ID == treatment.ID).OrderBy(p => p.SERVICE_REQ_ID).ToList();
                            if (listExamEyeSub.Count > 0)
                            {
                                treatment.APPOINT_EYESIGHT_GLASS_LEFT = listExamEyeSub[0].PART_EXAM_EYESIGHT_GLASS_LEFT;
                                treatment.APPOINT_EYESIGHT_GLASS_RIGHT = listExamEyeSub[0].PART_EXAM_EYESIGHT_GLASS_RIGHT;
                                treatment.APPOINT_EYESIGHT_LEFT = listExamEyeSub[0].PART_EXAM_EYESIGHT_LEFT;
                                treatment.APPOINT_EYESIGHT_RIGHT = listExamEyeSub[0].PART_EXAM_EYESIGHT_RIGHT;
                                treatment.APPOINT_EYE_TENSION_RIGHT = listExamEyeSub[0].PART_EXAM_EYE_TENSION_RIGHT;
                                treatment.APPOINT_EYE_TENSION_LEFT = listExamEyeSub[0].PART_EXAM_EYE_TENSION_LEFT;
                                if (listExamEyeSub.Count > 1)
                                {
                                    treatment.APPOINT_EYESIGHT_GLASS_LEFT_2 = listExamEyeSub[1].PART_EXAM_EYESIGHT_GLASS_LEFT;
                                    treatment.APPOINT_EYESIGHT_GLASS_RIGHT_2 = listExamEyeSub[1].PART_EXAM_EYESIGHT_GLASS_RIGHT;
                                    treatment.APPOINT_EYESIGHT_LEFT_2 = listExamEyeSub[1].PART_EXAM_EYESIGHT_LEFT;
                                    treatment.APPOINT_EYESIGHT_RIGHT_2 = listExamEyeSub[1].PART_EXAM_EYESIGHT_RIGHT;
                                    treatment.APPOINT_EYE_TENSION_RIGHT_2 = listExamEyeSub[1].PART_EXAM_EYE_TENSION_RIGHT;
                                    treatment.APPOINT_EYE_TENSION_LEFT_2 = listExamEyeSub[1].PART_EXAM_EYE_TENSION_LEFT;
                                    if (listExamEyeSub.Count > 2)
                                    {
                                        treatment.APPOINT_EYESIGHT_GLASS_LEFT_3 = listExamEyeSub[2].PART_EXAM_EYESIGHT_GLASS_LEFT;
                                        treatment.APPOINT_EYESIGHT_GLASS_RIGHT_3 = listExamEyeSub[2].PART_EXAM_EYESIGHT_GLASS_RIGHT;
                                        treatment.APPOINT_EYESIGHT_LEFT_3 = listExamEyeSub[2].PART_EXAM_EYESIGHT_LEFT;
                                        treatment.APPOINT_EYESIGHT_RIGHT_3 = listExamEyeSub[2].PART_EXAM_EYESIGHT_RIGHT;
                                        treatment.APPOINT_EYE_TENSION_RIGHT_3 = listExamEyeSub[2].PART_EXAM_EYE_TENSION_RIGHT;
                                        treatment.APPOINT_EYE_TENSION_LEFT_3 = listExamEyeSub[2].PART_EXAM_EYE_TENSION_LEFT;
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }    
                            
                            
                        }
                        treatment.IN_CODE = treatment.IN_CODE;
                        treatment.STORE_CODE = treatment.STORE_CODE;
                        treatment.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                        treatment.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                        treatment.BIRTHDAY = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.TDL_PATIENT_DOB);
                        treatment.GENDER = treatment.TDL_PATIENT_GENDER_NAME;
                        treatment.ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                        treatment.PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == treatment.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;
                        treatment.IN_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == treatment.IN_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        treatment.RELATIVE = (treatment.RELATIVE_TYPE ?? "") + ": " + (treatment.RELATIVE_NAME ?? "");
                        
                        if (treatment.PTTT_METHOD_IDS != null)
                        {
                            treatment.PTTT_METHOD_NAMES = string.Join(", ", listPtttMethod.Where(o => string.Format(",{0},", treatment.PTTT_METHOD_IDS).Contains(string.Format(",{0},", o.ID))).Select(p => p.PTTT_METHOD_NAME).ToList());
                        }
                        var departmentTranss = listHisDepartmentTran.Where(w => w.TREATMENT_ID == treatment.ID).OrderByDescending(s => s.DEPARTMENT_IN_TIME).ToList();

                        var patientTypeAlterSub = listHisPatientTypeAlterTreat.Where(w => w.TREATMENT_ID == treatment.ID).ToList();

                        if (departmentTranss != null && departmentTranss.Count > 0)
                        {
                            foreach (var departmentTran in departmentTranss)
                            {
                                if (!departmentTran.DEPARTMENT_IN_TIME.HasValue)
                                {
                                    continue;
                                }
                                bool isTreatIn = this.HasTreatIn(departmentTran, patientTypeAlterSub);

                                bool isTreatOut = this.HasTreatOut(departmentTran, patientTypeAlterSub);

                                //BC hiển thị BN nội trú, ngoại trú
                                if (!isTreatIn && !isTreatOut) continue;

                                if (filter.IS_TREAT_IN.HasValue)
                                {
                                    if (filter.IS_TREAT_IN.Value == true)
                                    {
                                        if (!isTreatIn) continue;
                                    }
                                    else
                                    {
                                        if (!isTreatOut) continue;
                                    }
                                }
                                List<string> KCCDepartmentCodes = new List<string>();
                                if (filter.DEPARTMENT_CODE__OUTPATIENTs != null)
                                {
                                    KCCDepartmentCodes = filter.DEPARTMENT_CODE__OUTPATIENTs.Split(',').ToList();
                                }

                                string DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentTran.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                                if (KCCDepartmentCodes.Contains(DEPARTMENT_CODE))
                                {
                                    continue;
                                }

                                var nextDepartmentTran = NextDepartment(departmentTran, listHisDepartmentTranAll);
                                if (nextDepartmentTran.DEPARTMENT_IN_TIME == null) nextDepartmentTran.DEPARTMENT_IN_TIME = treatment.IS_PAUSE == 1 ? treatment.OUT_TIME : 99999999999999;
                                if ((long)nextDepartmentTran.DEPARTMENT_IN_TIME / 100 <= (long)treatment.CLINICAL_IN_TIME / 100)
                                {
                                    continue;
                                }
                                bool previousIsTreatIn = this.HasTreatIn(listHisDepartmentTranAll.FirstOrDefault(o => o.ID == departmentTran.PREVIOUS_ID), listHisPatientTypeAlterTreat);

                                bool previousIsTreatOut = this.HasTreatOut(listHisDepartmentTranAll.FirstOrDefault(o => o.ID == departmentTran.PREVIOUS_ID), listHisPatientTypeAlterTreat);

                                if ((long)departmentTran.DEPARTMENT_IN_TIME / 100 < (long)treatment.CLINICAL_IN_TIME / 100 || !previousIsTreatIn && !previousIsTreatOut)
                                {
                                    departmentTran.DEPARTMENT_IN_TIME = treatment.CLINICAL_IN_TIME;
                                }
                                //Nguoi benh vao dieu tri se lay:
                                //vao khoa tu time_from den time_to
                                if (departmentTran.DEPARTMENT_IN_TIME >= filter.TIME_FROM && departmentTran.DEPARTMENT_IN_TIME < filter.TIME_TO)
                                {

                                    if ((long)departmentTran.DEPARTMENT_IN_TIME / 100 > (long)treatment.CLINICAL_IN_TIME / 100 &&
                                (previousIsTreatIn || previousIsTreatOut))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        //
                                    }
                                }
                                else
                                {
                                    continue;
                                }

                                treatment.IN_TIME = departmentTran.DEPARTMENT_IN_TIME ?? 0;
                                long logTime = 0;
                                long saveTime = 0;
                                this.FindLogTime(departmentTran, patientTypeAlterSub, ref logTime, ref saveTime);
                                treatment.LOG_TIME = logTime;
                                treatment.SAVE_TIME = saveTime;
                                treatment.REQUEST_TIME = departmentTran.REQUEST_TIME ?? 0;
                                treatment.TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(departmentTran.DEPARTMENT_IN_TIME ?? 0);



                                var previousDepartmentTran = PreviousDepartment(departmentTran, listHisDepartmentTranAll);
                                if (previousDepartmentTran != null)
                                {

                                    treatment.PREVIOUS_DEPARTMENT_ID = previousDepartmentTran.DEPARTMENT_ID;
                                    treatment.DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == previousDepartmentTran.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                                    treatment.DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == previousDepartmentTran.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                                }
                                treatment.CREAT_USER_CODE = departmentTran.CREATOR;
                                treatment.IN_ICD = departmentTran.ICD_NAME;
                                treatment.ON_DEPARTMENT_ID = departmentTran.DEPARTMENT_ID;
                                treatment.ON_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentTran.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                                treatment.ON_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentTran.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                                treatment.CLINICAL_DEPARTMENT_ID = departmentTran.DEPARTMENT_ID;
                                treatment.CLINICAL_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentTran.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                                treatment.CLINICAL_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentTran.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                                break;
                            }
                        }

                    }
                }

                patientTypeIdBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                listRdoGroup = listRdoTreatment.GroupBy(o => o.ICD_GROUP_CODE).Select(r => new
                    GROUP_ICD()
                {
                    ICD_GROUP_CODE = r.First().ICD_GROUP_CODE,
                    COUNT_ALL = r.Count(),
                    COUNT_BHYT = r.Count(o => o.TDL_PATIENT_TYPE_ID == patientTypeIdBhyt),
                    COUNT_TP = r.Count(o => o.TDL_PATIENT_TYPE_ID != patientTypeIdBhyt),
                }).ToList();


            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void FindLogTime(DEPARTMENT_TRAN departmentTran, List<PATIENT_TYPE_ALTER> lisPatientTypeAlters, ref long logTime, ref long saveTime)
        {
            try
            {
                if (departmentTran == null)
                {
                    return;
                }
                var patientTypeAlterSub = lisPatientTypeAlters.Where(o => o.DEPARTMENT_TRAN_ID == departmentTran.ID).ToList();
                if (IsNotNullOrEmpty(patientTypeAlterSub))
                {
                    logTime = patientTypeAlterSub.First().LOG_TIME;
                    saveTime = patientTypeAlterSub.First().CREATE_TIME;
                }
                else
                {
                    var patientTypeAlter = lisPatientTypeAlters.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID
                    && o.LOG_TIME <= departmentTran.DEPARTMENT_IN_TIME).ToList();
                    if (IsNotNullOrEmpty(patientTypeAlter))
                    {
                        logTime = patientTypeAlter.OrderBy(o => o.LOG_TIME).Last().LOG_TIME;
                        saveTime = patientTypeAlter.OrderBy(o => o.LOG_TIME).Last().CREATE_TIME;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return;
        }



        //khoa lien ke
        private DEPARTMENT_TRAN NextDepartment(DEPARTMENT_TRAN o, List<DEPARTMENT_TRAN> listHisDepartmentTranAll)
        {

            return listHisDepartmentTranAll.FirstOrDefault(p => p.PREVIOUS_ID == o.ID) ?? new DEPARTMENT_TRAN();

        }

        //khoa truoc
        private DEPARTMENT_TRAN PreviousDepartment(DEPARTMENT_TRAN o, List<DEPARTMENT_TRAN> listHisDepartmentTranAll)
        {

            return listHisDepartmentTranAll.FirstOrDefault(p => p.ID == o.PREVIOUS_ID);

        }


        private bool HasTreatIn(DEPARTMENT_TRAN departmentTran, List<PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            bool result = false;
            try
            {
                if (departmentTran == null)
                    return false;
                if (listPatientTypeAlter == null)
                    return false;
                var patientTypeAlterSub = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID).ToList();

                if (patientTypeAlterSub.Exists(o => o.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU))
                {
                    return false;
                }
                return patientTypeAlterSub.Exists(o => o.DEPARTMENT_TRAN_ID == departmentTran.ID || o.LOG_TIME <= departmentTran.DEPARTMENT_IN_TIME);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private bool HasTreatOut(DEPARTMENT_TRAN departmentTran, List<PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            bool result = false;
            try
            {
                if (departmentTran == null)
                    return false;
                if (listPatientTypeAlter == null)
                    return false;
                var patientTypeAlterSub = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID).ToList();

                if (patientTypeAlterSub.Exists(o => o.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU))
                {
                    return false;
                }
                return patientTypeAlterSub.Exists(o => o.DEPARTMENT_TRAN_ID == departmentTran.ID || o.LOG_TIME <= departmentTran.DEPARTMENT_IN_TIME);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetDicMini(ref Dictionary<string, object> dicMini)
        {
            try
            {
                string jsonNewObjectKey = MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.GetBySheetIndex(this.reportTemplate.REPORT_TEMPLATE_URL, 2, 1, 1);
                if (!string.IsNullOrWhiteSpace(jsonNewObjectKey))
                {
                    if (jsonNewObjectKey.StartsWith("{") && jsonNewObjectKey.EndsWith("}") || jsonNewObjectKey.StartsWith("[") && jsonNewObjectKey.EndsWith("]"))
                    {
                        dicMini = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonNewObjectKey);
                    }
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public long patientTypeIdBhyt = 0;
    }
}
