using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
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
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisMedicineType;

namespace MRS.Processor.Mrs00459
{
    class Mrs00459Processor : AbstractProcessor
    {
        Mrs00459Filter castFilter = null;
        List<Mrs00459RDO> listRdo = new List<Mrs00459RDO>();
        List<Mrs00459RDO> listRdoGroup = new List<Mrs00459RDO>();
        //List<Mrs00459RDO> listRdoGroupFinishTime = new List<Mrs00459RDO>(); 
        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>();
        List<V_HIS_ROOM> listRooms = new List<V_HIS_ROOM>();
        List<V_HIS_PATIENT_TYPE_ALTER> lisPatientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<HIS_DEPARTMENT> listDepartments = new List<HIS_DEPARTMENT>();
        List<V_HIS_DEPARTMENT_TRAN> listDepartmentTrans = new List<V_HIS_DEPARTMENT_TRAN>();

        string thisReportTypeCode = "";
        public Mrs00459Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00459Filter);
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM.HasValue)
                {
                    dicSingleTag.Add("DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM.Value));
                }
                else if (castFilter.OUT_TIME_FROM > 0)
                {
                    dicSingleTag.Add("DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.OUT_TIME_FROM.Value));
                }

                if (castFilter.TIME_TO.HasValue)
                {
                    dicSingleTag.Add("DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO.Value));
                }
                else if (castFilter.OUT_TIME_TO.HasValue)
                {
                    dicSingleTag.Add("DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.OUT_TIME_TO.Value));
                }

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Report", listRdo);
                objectTag.AddObjectData(store, "listRdoGroup", listRdoGroup);
                //objectTag.AddObjectData(store, "listRdoGroupFinishTime", listRdoGroupFinishTime); 
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "listRdoGroup", "Report", "GROUP_ID", "GROUP_ID");
                //exportSuccess = exportSuccess && objectTag.AddRelationship(store, "listRdoGroupRoom", "listRdoGroupFinishTime", new string[] { "DEPARTMENT_CODE", "ROOM_CODE" }, new string[] { "DEPARTMENT_CODE", "ROOM_CODE" }); 
                //exportSuccess = exportSuccess && objectTag.AddRelationship(store, "listRdoGroupFinishTime", "Report", new string[] { "DEPARTMENT_CODE", "ROOM_CODE", "FINISH_TIME" }, new string[] { "DEPARTMENT_CODE", "ROOM_CODE", "FINISH_TIME" }); 
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
                this.castFilter = (Mrs00459Filter)this.reportFilter;

                HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                if(castFilter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    treatmentFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM??castFilter.OUT_TIME_FROM;
                    treatmentFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO??castFilter.OUT_TIME_TO;
                    treatmentFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                }
                else if (castFilter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    treatmentFilter.OUT_TIME_FROM = castFilter.TIME_FROM??castFilter.OUT_TIME_FROM;
                    treatmentFilter.OUT_TIME_TO = castFilter.TIME_TO??castFilter.OUT_TIME_TO;
                    treatmentFilter.IS_PAUSE = true;
                }
                else
                {
                    treatmentFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                    treatmentFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                    treatmentFilter.OUT_TIME_FROM = castFilter.OUT_TIME_FROM;
                    treatmentFilter.OUT_TIME_TO = castFilter.OUT_TIME_TO;
                    //treatmentFilter.IS_PAUSE = false;
                }

                treatmentFilter.END_DEPARTMENT_IDs = castFilter.DEPARTMENT_IDs;
                listTreatments = new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentFilter);

                if (castFilter.IS_TREAT72H_AND_HAS_BIO == true)
                {
                    //lọc chỉ lấy bệnh nhân điều trị từ 72 tiếng và có sử dụng kháng sinh
                    listTreatments = FilterTrea72hAndHasBio(listTreatments);
                }

                var skip = 0;
                if (IsNotNullOrEmpty(listTreatments))
                {
                    while (listTreatments.Count - skip > 0)
                    {
                        var listIds = listTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientTypeAlterViewFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery();
                        patientTypeAlterFilter.TREATMENT_IDs = listIds.Select(s => s.ID).ToList();
                        lisPatientTypeAlters.AddRange(new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).GetView(patientTypeAlterFilter));
                    }
                }

                //
                skip = 0;
                if (IsNotNullOrEmpty(listTreatments))
                {
                    while (listTreatments.Count - skip > 0)
                    {
                        var listIds = listTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisDepartmentTranViewFilterQuery departmentTranFilter = new HisDepartmentTranViewFilterQuery();
                        departmentTranFilter.TREATMENT_IDs = listIds.Select(s => s.ID).ToList();
                        listDepartmentTrans.AddRange(new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(param).GetView(departmentTranFilter));
                    }
                }


                HisRoomViewFilterQuery roomFilter = new HisRoomViewFilterQuery();
                listRooms.AddRange(new MOS.MANAGER.HisRoom.HisRoomManager(param).GetView(roomFilter));

                skip = 0;
                if (IsNotNullOrEmpty(listRooms))
                {
                    while (listRooms.Count - skip > 0)
                    {
                        var listIds = listRooms.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery();
                        departmentFilter.IDs = listIds.Select(s => s.DEPARTMENT_ID).ToList();
                        listDepartments.AddRange(new MOS.MANAGER.HisDepartment.HisDepartmentManager(param).Get(departmentFilter));
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

        private List<V_HIS_TREATMENT> FilterTrea72hAndHasBio(List<V_HIS_TREATMENT> _listTreatments)
        {
            List<V_HIS_TREATMENT> result = new List<V_HIS_TREATMENT>();

            //lọc thời gian điều trị lớn hơn 72 h
            foreach (var item in _listTreatments)
            {
                if (item.CLINICAL_IN_TIME == null)
                {
                    continue;
                }
                if (item.OUT_TIME == null)
                {
                    continue;
                }
                if (ToTalDay(item.CLINICAL_IN_TIME, item.OUT_TIME) >= 3)
                {
                    result.Add(item);
                }
                
            }

            //lọc sử dụng kháng sinh
            var treaIds = result.Select(o => o.ID).ToList();
            if (treaIds.Count > 0)
            {
                var listSereServ = new List<HIS_SERE_SERV>();
                var skip = 0;
                if (IsNotNullOrEmpty(treaIds))
                {
                    while (treaIds.Count - skip > 0)
                    {
                        var listIds = treaIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServFilterQuery exmmFilter = new HisSereServFilterQuery();
                        exmmFilter.TREATMENT_IDs = listIds.ToList();
                        exmmFilter.HAS_EXECUTE = true;
                        exmmFilter.TDL_SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC;
                        listSereServ.AddRange(new HisSereServManager().Get(exmmFilter));
                    }
                    var listMety = new HisMedicineTypeManager().Get(new HisMedicineTypeFilterQuery())?? new List<HIS_MEDICINE_TYPE>();
                    var svBios = listMety.Where(o => o.MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__KS).Select(p => p.SERVICE_ID).Distinct().ToList();

                    listSereServ = listSereServ.Where(o => svBios.Contains(o.SERVICE_ID)).ToList();
                }
                treaIds = listSereServ.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                result = result.Where(o => treaIds.Contains(o.ID)).ToList();
            }
            return result;
        }

        private double ToTalDay(long? ClinicalInTime, long? OutTime)
        {
            double result = 0;
            try
            {
                if (ClinicalInTime != null && OutTime != null)
                {
                    System.DateTime? dateBefore = System.DateTime.ParseExact(ClinicalInTime.ToString(), "yyyyMMddHHmmss",
                                          System.Globalization.CultureInfo.InvariantCulture);
                    System.DateTime? dateAfter = System.DateTime.ParseExact(OutTime.ToString(), "yyyyMMddHHmmss",
                                       System.Globalization.CultureInfo.InvariantCulture);
                    if (dateBefore != null && dateAfter != null)
                    {
                        TimeSpan difference = dateAfter.Value - dateBefore.Value;
                        result = (double)difference.TotalDays;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Debug(ex);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();

                //lay phan tu cuoi cung luc doi doi tuong trong Patient_type_alter. ==> lay doi tuong BN la ng/tru va n/tru
                var listPatientTypeAlterGroupTreatment = lisPatientTypeAlters.GroupBy(s => s.TREATMENT_ID).ToList();
                foreach (var patientTypeAlterGroupTreatment in listPatientTypeAlterGroupTreatment)
                {
                    Mrs00459RDO rdo = new Mrs00459RDO();
                    var patientTypeAlters = patientTypeAlterGroupTreatment.OrderByDescending(s => s.LOG_TIME).Where(s => s.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || s.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).ToList();
                    var patientTypeAlter = patientTypeAlters.FirstOrDefault();

                    if (IsNotNull(patientTypeAlter) && patientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        rdo.GROUP_ID = 1;
                        rdo.TREATMENT_TYPE_NAME = patientTypeAlter.TREATMENT_TYPE_NAME;
                        if (patientTypeAlters.Last().EXECUTE_ROOM_ID != null)
                        {
                            var room = listRooms.FirstOrDefault(s => s.ID == patientTypeAlters.Last().EXECUTE_ROOM_ID);
                            if (room != null)
                            {
                                rdo.DEPARTMENT_NAME = room.DEPARTMENT_NAME ;
                            }
                        }

                        //lay treatment noi tru
                        var listtreatment = listTreatments.Where(s => s.ID == patientTypeAlter.TREATMENT_ID).ToList();
                        foreach (var treatment in listtreatment)
                        {
                            if (treatment.TREATMENT_END_TYPE_ID==IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                            {
                                rdo.HOSPITAL_TRAN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.OUT_TIME??0);
                            }
                            rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                            rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
                            rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                            rdo.PATIENT_NATIONAL_NAME = treatment.TDL_PATIENT_NATIONAL_NAME;
                            rdo.PATIENT_ETHNIC_NAME = treatment.TDL_PATIENT_ETHNIC_NAME;
                            rdo.PATIENT_CAREER_NAME = treatment.TDL_PATIENT_CAREER_NAME;

                            //Nam
                            if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                            {
                                rdo.MALE = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.TDL_PATIENT_DOB);
                            }

                            //Nu
                            if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                            {
                                rdo.FEMALE = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.TDL_PATIENT_DOB);
                            }

                            rdo.ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                            rdo.OUT_ICD = treatment.ICD_NAME;
                            rdo.OUT_ICD_CODE = treatment.ICD_CODE;
                            rdo.IN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.CLINICAL_IN_TIME.Value);
                            rdo.OUT_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME.Value);
                            rdo.TREATMENT_RESULT_NAME = treatment.TREATMENT_RESULT_NAME;
                            rdo.END_DEPARTMENT_NAME = treatment.END_DEPARTMENT_NAME;
                            rdo.CLINICAL_IN_TIME_NUM = treatment.CLINICAL_IN_TIME;
                            rdo.OUT_TIME_NUM = treatment.OUT_TIME;
                            //lay chan doan vao vien va ra vien.
                            var listDepartmentTran = listDepartmentTrans.Where(s => s.TREATMENT_ID == treatment.ID).OrderByDescending(s => s.DEPARTMENT_IN_TIME).ToList();

                            if (IsNotNullOrEmpty(listDepartmentTran))
                            {

                                rdo.IN_ICD = listDepartmentTran.Last().ICD_NAME;
                                rdo.IN_ICD_CODE = listDepartmentTran.Last().ICD_CODE;
                                //var departmentTranOut = departmentTran.Where(s => s.IN_OUT == IMSys.DbConfig.HIS_RS.HIS_DEPARTMENT_TRAN.IN_OUT__OUT).ToList(); 
                                //if (IsNotNullOrEmpty(departmentTranOut))
                                //{
                                //    rdo.OUT_ICD = departmentTranOut.First().ICD_NAME; 
                                //}
                            }
                            rdo.TREATMENT_DAY_COUNT = treatment.TREATMENT_DAY_COUNT;
                            listRdo.Add(rdo);
                        }
                    }

                    if (IsNotNull(patientTypeAlter) && patientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                    {
                        rdo.GROUP_ID = 2;
                        rdo.TREATMENT_TYPE_NAME = patientTypeAlter.TREATMENT_TYPE_NAME;
                        if (patientTypeAlters.Last().EXECUTE_ROOM_ID != null)
                        {
                            var listRoom = listRooms.Where(s => s.ID == patientTypeAlters.Last().EXECUTE_ROOM_ID).ToList();
                            rdo.DEPARTMENT_NAME = IsNotNull(listRoom) ? listRoom.First().DEPARTMENT_NAME : "";
                        }

                        //lay treatment ngoai tru
                        var listtreatment = listTreatments.Where(s => s.ID == patientTypeAlter.TREATMENT_ID).ToList();
                        foreach (var treatment in listtreatment)
                        {
                            if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                            {
                                rdo.HOSPITAL_TRAN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.OUT_TIME ?? 0);
                            }
                            rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                            rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
                            rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                            rdo.PATIENT_NATIONAL_NAME = treatment.TDL_PATIENT_NATIONAL_NAME;
                            rdo.PATIENT_ETHNIC_NAME = treatment.TDL_PATIENT_ETHNIC_NAME;
                            rdo.PATIENT_CAREER_NAME = treatment.TDL_PATIENT_CAREER_NAME;

                            //Nam
                            if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                            {
                                rdo.MALE = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.TDL_PATIENT_DOB);
                            }

                            //Nu
                            if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                            {
                                rdo.FEMALE = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.TDL_PATIENT_DOB);
                            }

                            rdo.ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                            rdo.OUT_ICD = treatment.ICD_NAME;
                            rdo.OUT_ICD_CODE = treatment.ICD_CODE;
                            rdo.IN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.CLINICAL_IN_TIME ?? 0);
                            rdo.OUT_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME ?? 0);
                            rdo.TREATMENT_RESULT_NAME = treatment.TREATMENT_RESULT_NAME;
                            rdo.END_DEPARTMENT_NAME = treatment.END_DEPARTMENT_NAME;
                            rdo.CLINICAL_IN_TIME_NUM = treatment.CLINICAL_IN_TIME;
                            rdo.OUT_TIME_NUM = treatment.OUT_TIME;
                            //lay chan doan vao vien va ra vien.
                            var listDepartmentTran = listDepartmentTrans.Where(s => s.TREATMENT_ID == treatment.ID).OrderByDescending(s => s.DEPARTMENT_IN_TIME).ToList();

                            if (IsNotNullOrEmpty(listDepartmentTran))
                            {

                                rdo.IN_ICD = listDepartmentTran.Last().ICD_NAME;
                                rdo.IN_ICD_CODE = listDepartmentTran.Last().ICD_CODE;
                                //var departmentTranOut = departmentTran.Where(s => s.IN_OUT == IMSys.DbConfig.HIS_RS.HIS_DEPARTMENT_TRAN.IN_OUT__OUT).ToList(); 
                                //if (IsNotNullOrEmpty(departmentTranOut))
                                //{
                                //    rdo.OUT_ICD = departmentTranOut.First().ICD_NAME; 
                                //}
                            }
                            rdo.TREATMENT_DAY_COUNT = treatment.TREATMENT_DAY_COUNT;
                            listRdo.Add(rdo);
                        }
                    }
                }

                if (IsNotNullOrEmpty(listRdo))
                {
                    listRdo = listRdo.OrderBy(s => s.GROUP_ID).ToList();

                    listRdoGroup = listRdo.GroupBy(s => s.GROUP_ID).Select(s => new Mrs00459RDO
                    {
                        GROUP_ID = s.First().GROUP_ID,
                        TREATMENT_TYPE_NAME = s.First().TREATMENT_TYPE_NAME,
                    }).ToList();
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
