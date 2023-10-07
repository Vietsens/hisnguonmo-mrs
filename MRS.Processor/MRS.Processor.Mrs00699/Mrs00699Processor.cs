using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBaby;
using MOS.MANAGER.HisEmployee;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MOS.MANAGER.HisPatientTypeAlter;
using ACS.EFMODEL.DataModels;
using ACS.MANAGER.Manager;
using ACS.MANAGER.Core.AcsUser.Get;
using MOS.MANAGER.HisWorkPlace;
using MOS.MANAGER.HisPatientType;
using Inventec.Common.Logging;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00699
{
    class Mrs00699Processor : AbstractProcessor
    {
        List<Mrs00699RDO> ListRdoRv = new List<Mrs00699RDO>();//ra viện
        List<Mrs00699RDO> ListRdoCs = new List<Mrs00699RDO>();//chứng sinh
        List<Mrs00699RDO> ListRdoNo = new List<Mrs00699RDO>();//nghỉ ốm

        List<V_HIS_TREATMENT> ListTreatment = new List<V_HIS_TREATMENT>();
        List<HIS_PATIENT> ListPatient = new List<HIS_PATIENT>();
        Dictionary<long, HIS_PATIENT_TYPE_ALTER> dicLastPatientType = new Dictionary<long, HIS_PATIENT_TYPE_ALTER>();

        List<V_HIS_BABY> ListBaby = new List<V_HIS_BABY>();
        List<HIS_TREATMENT> ListTreatmentBaby = new List<HIS_TREATMENT>();
        List<HIS_PATIENT> ListPatientBaby = new List<HIS_PATIENT>();
        Dictionary<long, HIS_PATIENT_TYPE_ALTER> dicLastPatientTypeBaby = new Dictionary<long, HIS_PATIENT_TYPE_ALTER>();

        List<HIS_EMPLOYEE> ListEmployee = new List<HIS_EMPLOYEE>();
        List<ACS_USER> ListAcsUser = new List<ACS_USER>();
        List<HIS_WORK_PLACE> workPlaces = new List<HIS_WORK_PLACE>();
        List<HIS_PATIENT_TYPE> ListPatientType = new List<HIS_PATIENT_TYPE>();
        Mrs00699Filter castFilter = null;
        public Mrs00699Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00699Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00699Filter)this.reportFilter;
                ListPatientType = new HisPatientTypeManager().Get(new HisPatientTypeFilterQuery());
                workPlaces = new HisWorkPlaceManager().Get(new HisWorkPlaceFilterQuery() { });
                ListTreatment = new ManagerSql().GetTreatmentView(castFilter);
                if (castFilter.IS_VIENPHI != null && castFilter.IS_VIENPHI == true)
                {
                    ListPatientType = ListPatientType.Where(x => x.PATIENT_TYPE_NAME.ToLower().Contains("viện phí")).ToList();
                    var patientTypeVPIds = ListPatientType.Select(x => x.ID).ToList();
                    ListTreatment = ListTreatment.Where(x => patientTypeVPIds.Contains(x.TDL_PATIENT_TYPE_ID ?? 0)).ToList();
                }
                
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    List<HIS_PATIENT_TYPE_ALTER> ListPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
                    List<long> lstPatientIds = ListTreatment.Select(s => s.PATIENT_ID).Distinct().ToList();

                    int skip = 0;
                    while (lstPatientIds.Count - skip > 0)
                    {
                        var listIds = lstPatientIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientFilterQuery patientFilter = new HisPatientFilterQuery();
                        patientFilter.IDs = listIds;
                        var patients = new HisPatientManager().Get(patientFilter);
                        if (IsNotNullOrEmpty(patients))
                        {
                            ListPatient.AddRange(patients);
                        }
                    }

                    List<long> lstTreatmentIds = ListTreatment.Select(s => s.ID).ToList();

                    skip = 0;
                    while (lstTreatmentIds.Count - skip > 0)
                    {
                        var listIds = lstTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientTypeAlterFilterQuery alterFilter = new HisPatientTypeAlterFilterQuery();
                        alterFilter.TREATMENT_IDs = listIds;
                        var patientTypeAlters = new HisPatientTypeAlterManager().Get(alterFilter);
                        if (IsNotNullOrEmpty(patientTypeAlters))
                        {
                            ListPatientTypeAlter.AddRange(patientTypeAlters);
                        }
                    }

                    if (IsNotNullOrEmpty(ListPatientTypeAlter))
                    {
                        //xếp giảm dần theo thời gian tạo để lấy diện đtrị cuối cùng
                        ListPatientTypeAlter = ListPatientTypeAlter.OrderByDescending(o => o.LOG_TIME).ToList();
                        foreach (var item in ListPatientTypeAlter)
                        {
                            if (!dicLastPatientType.ContainsKey(item.TREATMENT_ID))
                            dicLastPatientType[item.TREATMENT_ID] = item;
                        }
                    }
                }
                // lấy danh sách em bé 
                ListBaby = new ManagerSql().GetListBaby(castFilter);
                var treatmentBabyIds = ListBaby.Select(x => x.TREATMENT_ID).ToList();
                if (treatmentBabyIds!=null&& treatmentBabyIds.Count>0)
                {
                    List<HIS_PATIENT_TYPE_ALTER> ListPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
                    var skip = 0;
                    while (treatmentBabyIds.Count-skip>0)
                    {
                        var limit = treatmentBabyIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisTreatmentFilterQuery treatBabyFilter = new HisTreatmentFilterQuery();
                        treatBabyFilter.IDs = limit;
                        var treatmentBabys = new HisTreatmentManager().Get(treatBabyFilter);
                        if (castFilter.PATIENT_TYPE_IDs != null)
                        {
                            treatmentBabys = treatmentBabys.Where(o => castFilter.PATIENT_TYPE_IDs.Contains(o.TDL_PATIENT_TYPE_ID ?? 0)).ToList();
                        }
                        ListTreatmentBaby.AddRange(treatmentBabys);
                    }
                    List<long> lstPatientIds = ListTreatmentBaby.Select(s => s.PATIENT_ID).Distinct().ToList();
                    skip = 0;
                    while (lstPatientIds.Count - skip > 0)
                    {
                        var listIds = lstPatientIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientFilterQuery patientFilter = new HisPatientFilterQuery();
                        patientFilter.IDs = listIds;
                        var patients = new HisPatientManager().Get(patientFilter);
                        if (IsNotNullOrEmpty(patients))
                        {
                            ListPatientBaby.AddRange(patients);
                        }
                    }

                    List<long> lstTreatmentIds = ListTreatmentBaby.Select(s => s.ID).ToList();

                    skip = 0;
                    while (lstTreatmentIds.Count - skip > 0)
                    {
                        var listIds = lstTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientTypeAlterFilterQuery alterFilter = new HisPatientTypeAlterFilterQuery();
                        alterFilter.TREATMENT_IDs = listIds;
                        var patientTypeAlters = new HisPatientTypeAlterManager().Get(alterFilter);
                        if (IsNotNullOrEmpty(patientTypeAlters))
                        {
                            ListPatientTypeAlter.AddRange(patientTypeAlters);
                        }
                    }

                    if (IsNotNullOrEmpty(ListPatientTypeAlter))
                    {
                        //xếp giảm dần theo thời gian tạo để lấy diện đtrị cuối cùng
                        ListPatientTypeAlter = ListPatientTypeAlter.OrderByDescending(o => o.LOG_TIME).ToList();
                        foreach (var item in ListPatientTypeAlter)
                        {
                            if (!dicLastPatientType.ContainsKey(item.TREATMENT_ID))
                            dicLastPatientType[item.TREATMENT_ID] = item;
                        }
                    }
                }
                //
                ListEmployee = new HisEmployeeManager().Get(new HisEmployeeFilterQuery());
                ListAcsUser = new AcsUserManager(new CommonParam()).Get<List<ACS_USER>>(new AcsUserFilterQuery());
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                CreateThreadData();
            }
            catch (Exception ex)
            {
                ListRdoRv.Clear();
                ListRdoCs.Clear();
                ListRdoNo.Clear();
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void CreateThreadData()
        {
            Thread raVien = new Thread(ProcessRaVien);
            Thread chungSinh = new Thread(ProcessChungSinh);
            Thread nghiOm = new Thread(ProcessNghiOm);
            try
            {
                raVien.Start();
                chungSinh.Start();
                nghiOm.Start();

                raVien.Join();
                chungSinh.Join();
                nghiOm.Join();
            }
            catch (Exception ex)
            {
                raVien.Abort();
                chungSinh.Abort();
                nghiOm.Abort();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessNghiOm()
        {
            try
            {
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    var treatmentNghiOm = ListTreatment.Where(o => o.TREATMENT_END_TYPE_EXT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE_EXT.ID__NGHI_OM).ToList();
                    foreach (var treatment in treatmentNghiOm)
                    {
                        var patient = ListPatient.FirstOrDefault(o => o.ID == treatment.PATIENT_ID);
                        if (!IsNotNull(patient)) continue;
                        Mrs00699RDO rdo = new Mrs00699RDO();

                        //thong tin chung
                        rdo.TDL_HEIN_CARD_NUMBER = treatment.TDL_HEIN_CARD_NUMBER;
                        rdo.SICK_HEIN_CARD_NUMBER = treatment.SICK_HEIN_CARD_NUMBER;
                        rdo.TDL_SOCIAL_INSURANCE_NUMBER = treatment.TDL_SOCIAL_INSURANCE_NUMBER;
                        rdo.END_DEPARTMENT_HEAD_LOGINNAME = treatment.END_DEPARTMENT_HEAD_LOGINNAME;
                        rdo.END_DEPARTMENT_HEAD_USERNAME = treatment.END_DEPARTMENT_HEAD_USERNAME;
                        if (!string.IsNullOrWhiteSpace(treatment.END_DEPARTMENT_HEAD_LOGINNAME))
                        {
                            var headLoginname = ListEmployee.FirstOrDefault(o => o.LOGINNAME == treatment.END_DEPARTMENT_HEAD_LOGINNAME);
                            rdo.END_DEPARTMENT_HEAD_DIPLOMA = headLoginname != null ? headLoginname.DIPLOMA : "";
                        }
                        rdo.TDL_PATIENT_ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                        rdo.MA_CT = treatment.STORE_CODE;
                        rdo.GHI_CHU = "";
                        var branch = MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == treatment.BRANCH_ID);
                        rdo.MA_BV = branch != null ? branch.HEIN_MEDI_ORG_CODE : "";
                        rdo.MA_CSKCB = branch != null ? branch.HEIN_MEDI_ORG_CODE : "";

                        var department = MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.END_DEPARTMENT_ID);
                        if (department != null)
                        {
                            rdo.MA_KHOA = department != null ? department.BHYT_CODE : "";
                            rdo.TEN_TRUONGKHOA = department.HEAD_USERNAME;

                            var headLoginname = ListEmployee.FirstOrDefault(o => o.LOGINNAME == department.HEAD_LOGINNAME);
                            rdo.MA_TRUONGKHOA = headLoginname != null ? headLoginname.DIPLOMA : "";
                        }

                        rdo.HO_TEN = patient.VIR_PATIENT_NAME;
                        rdo.DAN_TOC = patient.ETHNIC_NAME;
                        rdo.DAN_TOC_CODE = patient.ETHNIC_CODE;
                        if (patient.IS_HAS_NOT_DAY_DOB == 1)
                        {
                            if (patient.DOB.ToString().Length>=4)
                            {
                                rdo.NGAY_SINH = patient.DOB.ToString().Substring(0, 4);
                            }
                        }
                        else
                        {
                            rdo.NGAY_SINH = Inventec.Common.DateTime.Convert.TimeNumberToDateString(patient.DOB);
                        }
                        if (patient.GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                        {
                            rdo.GIOI_TINH = GIOI_TINH.NU;
                        }
                        else if (patient.GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                        {
                            rdo.GIOI_TINH = GIOI_TINH.NAM;
                        }
                        else
                        {
                            rdo.GIOI_TINH = GIOI_TINH.KHONG_XAC_DINH;
                        }
                        if (!String.IsNullOrWhiteSpace(patient.CMND_NUMBER))
                        {
                            rdo.CMND = patient.CMND_NUMBER;
                            rdo.NGAY_CAP_CMND = Inventec.Common.DateTime.Convert.TimeNumberToDateString(patient.CMND_DATE ?? 0);
                            rdo.NOI_CAP_CMND = patient.CMND_PLACE;
                        }
                        else if (!String.IsNullOrWhiteSpace(patient.CCCD_NUMBER))
                        {
                            rdo.CMND = patient.CCCD_NUMBER;
                            rdo.NGAY_CAP_CMND = Inventec.Common.DateTime.Convert.TimeNumberToDateString(patient.CCCD_DATE ?? 0);
                            rdo.NOI_CAP_CMND = patient.CCCD_PLACE;
                        }
                        else if (!String.IsNullOrWhiteSpace(patient.RELATIVE_CMND_NUMBER))
                        {
                            rdo.CMND = patient.RELATIVE_CMND_NUMBER;
                            rdo.NGAY_CAP_CMND = "";
                            rdo.NOI_CAP_CMND = "";
                        }
                        rdo.NGHE_NGHIEP = patient.CAREER_NAME;
                        rdo.NGAY_VAO = Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(treatment.IN_TIME);
                        rdo.NGAY_RA = Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(treatment.OUT_TIME ?? 0);
                        rdo.TUOI_THAI = "";//chua có
                        rdo.NGAY_CT = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME ?? 0);
                        rdo.HO_TEN_CHA = treatment.TDL_PATIENT_FATHER_NAME;
                        rdo.HO_TEN_ME = treatment.TDL_PATIENT_MOTHER_NAME;
                        rdo.PP_DIEUTRI = treatment.TREATMENT_METHOD;
                        string icd = treatment.ICD_NAME;
                        if (!string.IsNullOrWhiteSpace(treatment.ICD_TEXT))
                        {
                            icd += ";" + treatment.ICD_TEXT.Trim(';');
                        }
                        //if (treatment.ICD_NAME != null && treatment.ICD_CODE != null)
                        //{
                        //    string[] icdSubCode = treatment.ICD_CODE.Split(';');
                        //    string[] icdSubName = treatment.ICD_NAME.Split(';');
                        //    if (icdSubCode.Length > 0 && icdSubCode.Length == icdSubName.Length)
                        //    {
                        //        for (int i = 1; i < icdSubCode.Length; i++)
                        //        {
                        //            rdo.ICD_FULL += icdSubCode[i] + " - " + icdSubName[i] + ";";
                        //        }
                        //    }

                        //}
                        if ( treatment.ICD_CODE!=null||treatment.ICD_NAME!=null)
                        {
                            rdo.ICD_FULL += treatment.ICD_CODE + " - " + treatment.ICD_NAME + ";";
                        }
                        rdo.CHAN_DOAN = icd;
                        rdo.LOI_DAN_BS = treatment.ADVISE;
                        rdo.MA_DVI = "";//chưa có
                        rdo.TEN_DVI = patient.WORK_PLACE;
                        var workPlac = workPlaces.FirstOrDefault(o => o.ID == patient.WORK_PLACE_ID);
                        if (workPlac != null)
                        {
                            rdo.MA_DVI = workPlac.WORK_PLACE_CODE;
                            rdo.TEN_DVI = workPlac.WORK_PLACE_NAME;
                        }
                        rdo.LOAI_QUAN_HE = treatment.TDL_PATIENT_RELATIVE_TYPE;
                        rdo.NGUOI_DAI_DIEN = treatment.TDL_PATIENT_RELATIVE_NAME;
                        rdo.SO_KCB = treatment.TREATMENT_CODE;
                        rdo.TEN_BSY = treatment.DOCTOR_USERNAME;
                        var employee = ListEmployee.FirstOrDefault(o => o.LOGINNAME == treatment.DOCTOR_LOGINNAME);
                        rdo.MA_BS = employee != null ? employee.DIPLOMA : "";
                        rdo.SO_SERI = (treatment.SICK_NUM_ORDER ?? 0).ToString();//để trống tự sinh
                        rdo.MAU_SO = treatment.EXTRA_END_CODE_SEED_CODE;//chưa có
                        //bổ sung sau
                        rdo.SO = "";
                        rdo.QUYEN_SO = "";

                        //thong tin rieng
                        rdo.TEN_DVI_NO = treatment.TDL_PATIENT_WORK_PLACE;
                        rdo.TEN_DVI_NO_NAME = treatment.TDL_PATIENT_WORK_PLACE_NAME;
                        rdo.TU_NGAY = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.SICK_LEAVE_FROM ?? 0);
                        rdo.DEN_NGAY = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.SICK_LEAVE_TO ?? 0);
                        rdo.SO_NGAY = (long)(treatment.SICK_LEAVE_DAY ?? 0);
                        if (dicLastPatientType.ContainsKey(treatment.ID) && !String.IsNullOrWhiteSpace(dicLastPatientType[treatment.ID].HEIN_CARD_NUMBER) && dicLastPatientType[treatment.ID].HEIN_CARD_NUMBER!=null)
                        {
                            if (dicLastPatientType[treatment.ID].HEIN_CARD_NUMBER.Length>=15)
                            {
                                rdo.MA_SOBHXH = dicLastPatientType[treatment.ID].HEIN_CARD_NUMBER.Substring(5, 10);
                            }
                            rdo.MA_THE = dicLastPatientType[treatment.ID].HEIN_CARD_NUMBER;
                            rdo.DIA_CHI = dicLastPatientType[treatment.ID].ADDRESS;
                        }
                        else if (!String.IsNullOrWhiteSpace(treatment.TDL_HEIN_CARD_NUMBER))
                        {
                            if (treatment.TDL_HEIN_CARD_NUMBER.Length>=15)
                            {
                                rdo.MA_SOBHXH = treatment.TDL_HEIN_CARD_NUMBER.Substring(5, 10);
                            }
                           
                            rdo.MA_THE = treatment.TDL_HEIN_CARD_NUMBER;
                            rdo.DIA_CHI = treatment.TDL_PATIENT_ADDRESS;
                        }
                        //else if (!String.IsNullOrWhiteSpace(treatment.SICK_HEIN_CARD_NUMBER))
                        //{
                        //    if (treatment.SICK_HEIN_CARD_NUMBER.Length>=15)
                        //    {
                        //        rdo.MA_SOBHXH = treatment.SICK_HEIN_CARD_NUMBER.Substring(5, 10);
                        //    }
                        //    rdo.MA_THE = treatment.SICK_HEIN_CARD_NUMBER;
                        //    rdo.DIA_CHI = treatment.TDL_PATIENT_ADDRESS;
                        //}
                        else if (castFilter.IS_BHXH == true)
                        {
                            if (!String.IsNullOrWhiteSpace(treatment.SICK_HEIN_CARD_NUMBER))
                            {
                                if (treatment.SICK_HEIN_CARD_NUMBER.Length >0)
                                {
                                    rdo.MA_SOBHXH = treatment.SICK_HEIN_CARD_NUMBER;
                                }
                                rdo.MA_THE = treatment.SICK_HEIN_CARD_NUMBER;
                                rdo.DIA_CHI = treatment.TDL_PATIENT_ADDRESS;
                            }
                        }
                        else
                        {
                            if (!String.IsNullOrWhiteSpace(treatment.SICK_HEIN_CARD_NUMBER))
                            {
                                if (treatment.SICK_HEIN_CARD_NUMBER.Length >= 15)
                                {
                                    rdo.MA_SOBHXH = treatment.SICK_HEIN_CARD_NUMBER.Substring(5, 10);
                                }
                                rdo.MA_THE = treatment.SICK_HEIN_CARD_NUMBER;
                                rdo.DIA_CHI = treatment.TDL_PATIENT_ADDRESS;
                            }
                        }
                        
                        
                        ListRdoNo.Add(rdo);
                    }
                }
                //if (!String.IsNullOrWhiteSpace(treatment.SICK_HEIN_CARD_NUMBER))
                //{
                //    if (treatment.SICK_HEIN_CARD_NUMBER.Length >= 15)
                //    {
                //        rdo.MA_SOBHXH = treatment.SICK_HEIN_CARD_NUMBER.Substring(5, 10);
                //    }
                //    rdo.MA_THE = treatment.SICK_HEIN_CARD_NUMBER;
                //    rdo.DIA_CHI = treatment.TDL_PATIENT_ADDRESS;
                //}
                
                if ( castFilter.IS_DICHVU == true)
                    {
                        ListRdoNo = ListRdoNo.ToList();
                    
                    }
                else
                {
                    
                        ListRdoNo = ListRdoNo.Where(o => !String.IsNullOrWhiteSpace(o.MA_SOBHXH)).ToList();                                         
                }

                //ListRdoNo = ListRdoNo.Where(o => !String.IsNullOrWhiteSpace(o.MA_SOBHXH)).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessChungSinh()
        {
            try
            {
                if (IsNotNullOrEmpty(ListTreatmentBaby))
                {
                    //var treatmentBaby = ListTreatment.Where(o => ListBaby.Select(s => s.TREATMENT_ID).Contains(o.ID)).ToList();
                    LogSystem.Info(ListTreatmentBaby.Count.ToString());
                    if (ListTreatmentBaby != null)
                    {
                        foreach (var treatment in ListTreatmentBaby)
                        {
                            Mrs00699RDO rdo = new Mrs00699RDO();
                            rdo.TDL_HEIN_CARD_NUMBER = treatment.TDL_HEIN_CARD_NUMBER;
                            rdo.SICK_HEIN_CARD_NUMBER = treatment.SICK_HEIN_CARD_NUMBER;
                            rdo.TDL_SOCIAL_INSURANCE_NUMBER = treatment.TDL_SOCIAL_INSURANCE_NUMBER;
                            rdo.END_DEPARTMENT_HEAD_LOGINNAME = treatment.END_DEPARTMENT_HEAD_LOGINNAME;
                            rdo.END_DEPARTMENT_HEAD_USERNAME = treatment.END_DEPARTMENT_HEAD_USERNAME;
                            if (!string.IsNullOrWhiteSpace(treatment.END_DEPARTMENT_HEAD_LOGINNAME))
                            {
                                var headLoginname = ListEmployee.FirstOrDefault(o => o.LOGINNAME == treatment.END_DEPARTMENT_HEAD_LOGINNAME);
                                rdo.END_DEPARTMENT_HEAD_DIPLOMA = headLoginname != null ? headLoginname.DIPLOMA : "";
                            }
                            rdo.TDL_PATIENT_ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                            var patient = ListPatientBaby.FirstOrDefault(o => o.ID == treatment.PATIENT_ID);
                            if (patient != null)
                            {
                                AddInforPatient(rdo, treatment, patient);
                            }

                            var branch = MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == treatment.BRANCH_ID);
                            if (branch != null)
                            {
                                AddInforBranch(rdo, branch);
                            }

                            var department = MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.END_DEPARTMENT_ID);
                            if (department != null)
                            {
                                AddInforDepartment(rdo, department);
                            }

                            var employee = ListEmployee.FirstOrDefault(o => o.LOGINNAME == treatment.DOCTOR_LOGINNAME);
                            if (employee != null)
                            {
                                AddInforEmployee(rdo, employee);
                            }
                            rdo.MA_CT = treatment.STORE_CODE;
                            rdo.GHI_CHU = "";
                            rdo.NGAY_VAO = Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(treatment.IN_TIME);
                            rdo.NGAY_RA = Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(treatment.OUT_TIME ?? 0);
                            rdo.TUOI_THAI = "";//chua có
                            rdo.NGAY_CT = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME ?? 0);
                            
                            rdo.PP_DIEUTRI = treatment.TREATMENT_METHOD;
                            string icd = treatment.ICD_NAME;
                            if (!string.IsNullOrWhiteSpace(treatment.ICD_TEXT))
                            {
                                icd += ";" + treatment.ICD_TEXT.Trim(';');
                            }
                            if (treatment.ICD_CODE != null || treatment.ICD_NAME != null)
                            {
                                rdo.ICD_FULL += treatment.ICD_CODE + " - " + treatment.ICD_NAME + ";";
                            }
                            rdo.CHAN_DOAN = icd;
                            rdo.LOI_DAN_BS = treatment.ADVISE;
                            rdo.MA_DVI = "";//chưa có
                            
                            rdo.SO_KCB = treatment.TREATMENT_CODE;
                            rdo.TEN_BSY = treatment.DOCTOR_USERNAME;
                            
                            
                            rdo.SO_SERI = "";//để trống tự sinh
                            rdo.MAU_SO = "";//chưa có
                            if (dicLastPatientType.ContainsKey(treatment.ID) && !string.IsNullOrWhiteSpace(dicLastPatientType[treatment.ID].HEIN_CARD_NUMBER))
                            {
                                rdo.MA_SOBHXH_ME = dicLastPatientType[treatment.ID].HEIN_CARD_NUMBER.Substring(5, 10);
                            }
                            else if (!string.IsNullOrWhiteSpace(treatment.TDL_HEIN_CARD_NUMBER))
                            {
                                rdo.MA_SOBHXH_ME = treatment.TDL_HEIN_CARD_NUMBER.Substring(5, 10);
                            }
                            rdo.HO_TEN_ME = treatment.TDL_PATIENT_NAME;
                            if (dicLastPatientType.ContainsKey(treatment.ID) && !string.IsNullOrWhiteSpace(dicLastPatientType[treatment.ID].HEIN_CARD_NUMBER))
                            {
                                rdo.MA_SOBHXH = dicLastPatientType[treatment.ID].HEIN_CARD_NUMBER.Substring(5, 10);
                                rdo.MA_THE = dicLastPatientType[treatment.ID].HEIN_CARD_NUMBER;
                                rdo.DIA_CHI = dicLastPatientType[treatment.ID].ADDRESS;
                            }
                            else if (!string.IsNullOrWhiteSpace(treatment.TDL_HEIN_CARD_NUMBER))
                            {
                                rdo.MA_SOBHXH = treatment.TDL_HEIN_CARD_NUMBER.Substring(5, 10);
                                rdo.MA_THE = treatment.TDL_HEIN_CARD_NUMBER;
                                rdo.DIA_CHI = treatment.TDL_PATIENT_ADDRESS;
                            }
                            var lstBaby = ListBaby.Where(o => o.TREATMENT_ID == treatment.ID).ToList();
                            if (lstBaby != null)
                            {
                                rdo.SO_CON = lstBaby.Count();
                                if (branch != null)
                                {
                                    rdo.NOI_SINH_CON = branch.BRANCH_NAME;
                                }
                                foreach (var baby in lstBaby)
                                {
                                    AddInforBaby(rdo, treatment, baby, lstBaby);
                                    ListRdoCs.Add(rdo);
                                }

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

        private void AddInforPatient(Mrs00699RDO rdo, HIS_TREATMENT treatment, HIS_PATIENT patient)
        {
            rdo.HO_TEN = patient.VIR_PATIENT_NAME;
            rdo.DAN_TOC = patient.ETHNIC_NAME;
            rdo.DAN_TOC_CODE = patient.ETHNIC_CODE; //+ " - " + patient.ETHNIC_NAME;
            if (patient.IS_HAS_NOT_DAY_DOB == 1)
            {
                rdo.NGAY_SINH = patient.DOB.ToString().Substring(0, 4);
            }
            else
            {
                rdo.NGAY_SINH = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.TDL_PATIENT_DOB);
            }
            if (patient.GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
            {
                rdo.GIOI_TINH = GIOI_TINH.NU;
            }
            else if (patient.GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
            {
                rdo.GIOI_TINH = GIOI_TINH.NAM;
            }

            else
            {
                rdo.GIOI_TINH = GIOI_TINH.KHONG_XAC_DINH;
            }
            if (!String.IsNullOrWhiteSpace(patient.CMND_NUMBER))
            {
                rdo.CMND = patient.CMND_NUMBER;
                rdo.NGAY_CAP_CMND = Inventec.Common.DateTime.Convert.TimeNumberToDateString(patient.CMND_DATE ?? 0);
                rdo.NOI_CAP_CMND = patient.CMND_PLACE;
            }
            else if (!String.IsNullOrWhiteSpace(patient.CCCD_NUMBER))
            {
                rdo.CMND = patient.CCCD_NUMBER;
                rdo.NGAY_CAP_CMND = Inventec.Common.DateTime.Convert.TimeNumberToDateString(patient.CCCD_DATE ?? 0);
                rdo.NOI_CAP_CMND = patient.CCCD_PLACE;
            }
            else if (!String.IsNullOrWhiteSpace(patient.RELATIVE_CMND_NUMBER))
            {
                rdo.CMND = patient.RELATIVE_CMND_NUMBER;
                rdo.NGAY_CAP_CMND = "";
                rdo.NOI_CAP_CMND = "";
            }
            rdo.NGHE_NGHIEP = patient.CAREER_NAME;
            rdo.HO_TEN_CHA = treatment.TDL_PATIENT_FATHER_NAME;
            rdo.HO_TEN_ME = treatment.TDL_PATIENT_MOTHER_NAME;
            rdo.TEN_DVI = patient.WORK_PLACE;
            var workPlac = workPlaces.FirstOrDefault(o => o.ID == patient.WORK_PLACE_ID);
            if (workPlac != null)
            {
                rdo.MA_DVI = workPlac.WORK_PLACE_CODE;
                rdo.TEN_DVI = workPlac.WORK_PLACE_NAME;
            }
            rdo.LOAI_QUAN_HE = treatment.TDL_PATIENT_RELATIVE_TYPE;
            rdo.NGUOI_DAI_DIEN = treatment.TDL_PATIENT_RELATIVE_NAME;
        }


        private void AddInforBranch(Mrs00699RDO rdo, HIS_BRANCH branch)
        {
            rdo.MA_BV = branch.HEIN_MEDI_ORG_CODE;
            rdo.MA_CSKCB = branch.HEIN_MEDI_ORG_CODE;
        }

        private void AddInforDepartment(Mrs00699RDO rdo, HIS_DEPARTMENT department)
        {
            rdo.MA_KHOA = department.BHYT_CODE;
            rdo.TEN_TRUONGKHOA = department.HEAD_USERNAME;
            var headLoginname = ListEmployee.FirstOrDefault(o => o.LOGINNAME == department.HEAD_LOGINNAME);
            if (headLoginname != null)
            {
                rdo.MA_TRUONGKHOA = headLoginname.DIPLOMA;
            }
        }

        private void AddInforEmployee(Mrs00699RDO rdo, HIS_EMPLOYEE employee)
        {
            rdo.MA_BS = employee.DIPLOMA;
        }

        private void AddInforBaby(Mrs00699RDO rdo, HIS_TREATMENT treatment, V_HIS_BABY baby, List<V_HIS_BABY> listBaby)
        {
            if (baby.BIRTH_CERT_NUM != null)
            {
                if (baby.BIRTH_CERT_NUM.ToString().Length == 1)
                {
                    rdo.SO_CT = "00000" + baby.BIRTH_CERT_NUM.ToString();
                }
                else if (baby.BIRTH_CERT_NUM.ToString().Length == 2)
                {
                    rdo.SO_CT = "0000" + baby.BIRTH_CERT_NUM.ToString();
                }
                else if (baby.BIRTH_CERT_NUM.ToString().Length == 3)
                {
                    rdo.SO_CT = "000" + baby.BIRTH_CERT_NUM.ToString();
                }
                else if (baby.BIRTH_CERT_NUM.ToString().Length == 4)
                {
                    rdo.SO_CT = "00" + baby.BIRTH_CERT_NUM.ToString();
                }
                else if (baby.BIRTH_CERT_NUM.ToString().Length == 5)
                {
                    rdo.SO_CT = "0" + baby.BIRTH_CERT_NUM.ToString();
                }
                else if (baby.BIRTH_CERT_NUM.ToString().Length == 6)
                {
                    rdo.SO_CT = baby.BIRTH_CERT_NUM.ToString();
                }

            }
            else
            {
                rdo.SO_CT = "";
            }
            
            //bổ sung sau
            rdo.SO = baby.BIRTH_CERT_BOOK_CODE;
            rdo.QUYEN_SO = baby.BIRTH_CERT_BOOK_NAME;

            //thong tin rieng

            rdo.HO_TEN_CHA = baby.FATHER_NAME;
            if (baby.BORN_TIME != null)
            {
                rdo.NGAY_SINHCON = Inventec.Common.DateTime.Convert.TimeNumberToDateString(baby.BORN_TIME ?? 0);
                rdo.NGAY_GIO_SINH_CON = Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(baby.BORN_TIME ?? 0);
            }
            rdo.TEN_CON = baby.BABY_NAME;
            
            //if (baby.GENDER_ID != null)
            //{
            //    if (baby.GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
            //    {
            //        rdo.GIOI_TINH_CON = GIOI_TINH.NU;
            //    }
            //    else if (baby.GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
            //    {
            //        rdo.GIOI_TINH_CON = GIOI_TINH.NAM;
            //    }
            //}
            //else
            //{
            //    rdo.GIOI_TINH_CON = GIOI_TINH.KHONG_XAC_DINH;
            //}
            rdo.GIOI_TINH_CON = baby.GENDER_NAME;
            rdo.CAN_NANG_CON = (baby.WEIGHT ?? 0);
            rdo.TINH_TRANG_CON = baby.BORN_RESULT_NAME;
            rdo.NGUOI_DO_DE = baby.MIDWIFE;
            
            var user = ListAcsUser.FirstOrDefault(o => o.LOGINNAME == baby.CREATOR);
            if (user != null)
            {
                rdo.NGUOI_GHI_PHIEU = user.USERNAME;
            }
            
            if (baby.BORN_TYPE_CODE == "MO")
            {
                rdo.SINHCON_PHAUTHUAT = 1;
            }

            if (baby.WEEK_COUNT != null && baby.WEEK_COUNT < 32)
            {
                rdo.SINHCON_DUOI32TUAN = 1;
            }
            
        }

        private void ProcessRaVien()
        {
            try
            {
                if (IsNotNullOrEmpty(ListTreatment))
                {

                    foreach (var treatment in ListTreatment)
                    {
                        var patient = ListPatient.FirstOrDefault(o => o.ID == treatment.PATIENT_ID);
                        if (!IsNotNull(patient)) continue;

                        Mrs00699RDO rdo = new Mrs00699RDO();
                        //thong tin chung
                        rdo.TDL_HEIN_CARD_NUMBER = treatment.TDL_HEIN_CARD_NUMBER;
                        rdo.SICK_HEIN_CARD_NUMBER = treatment.SICK_HEIN_CARD_NUMBER;
                        rdo.TDL_SOCIAL_INSURANCE_NUMBER = treatment.TDL_SOCIAL_INSURANCE_NUMBER;
                        rdo.END_DEPARTMENT_HEAD_LOGINNAME = treatment.END_DEPARTMENT_HEAD_LOGINNAME;
                        rdo.END_DEPARTMENT_HEAD_USERNAME = treatment.END_DEPARTMENT_HEAD_USERNAME;
                        if (!string.IsNullOrWhiteSpace(treatment.END_DEPARTMENT_HEAD_LOGINNAME))
                        {
                            var headLoginname = ListEmployee.FirstOrDefault(o => o.LOGINNAME == treatment.END_DEPARTMENT_HEAD_LOGINNAME);
                            rdo.END_DEPARTMENT_HEAD_DIPLOMA = headLoginname != null ? headLoginname.DIPLOMA : "";
                        }
                        rdo.TDL_PATIENT_ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                        rdo.MA_CT = treatment.END_CODE;
                        rdo.GHI_CHU = "";
                        var branch = MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == treatment.BRANCH_ID);
                        rdo.MA_BV = branch != null ? branch.HEIN_MEDI_ORG_CODE : "";
                        rdo.MA_CSKCB = branch != null ? branch.HEIN_MEDI_ORG_CODE : "";


                        var department = MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.END_DEPARTMENT_ID);
                        if (department != null)
                        {
                            rdo.MA_KHOA = department != null ? department.BHYT_CODE : "";
                            rdo.TEN_TRUONGKHOA = department.HEAD_USERNAME;
                            var headLoginname = ListEmployee.FirstOrDefault(o => o.LOGINNAME == department.HEAD_LOGINNAME);
                            rdo.MA_TRUONGKHOA = headLoginname != null ? headLoginname.DIPLOMA : "";
                        }

                        rdo.HO_TEN = patient.VIR_PATIENT_NAME;
                        rdo.DAN_TOC = patient.ETHNIC_NAME;
                        rdo.DAN_TOC_CODE = patient.ETHNIC_CODE;// +" - " + patient.ETHNIC_NAME;
                        if (patient.IS_HAS_NOT_DAY_DOB == 1)
                        {
                            rdo.NGAY_SINH = patient.DOB.ToString().Substring(0, 4);
                        }
                        else
                        {
                            rdo.NGAY_SINH = Inventec.Common.DateTime.Convert.TimeNumberToDateString(patient.DOB).ToString();
                        }
                        if (patient.GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                        {
                            rdo.GIOI_TINH = GIOI_TINH.NU;
                        }
                        else if (patient.GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                        {
                            rdo.GIOI_TINH = GIOI_TINH.NAM;
                        }
                        else
                        {
                            rdo.GIOI_TINH = GIOI_TINH.KHONG_XAC_DINH;
                        }
                        if (!String.IsNullOrWhiteSpace(patient.CMND_NUMBER))
                        {
                            rdo.CMND = patient.CMND_NUMBER;
                            rdo.NGAY_CAP_CMND = Inventec.Common.DateTime.Convert.TimeNumberToDateString(patient.CMND_DATE ?? 0);
                            rdo.NOI_CAP_CMND = patient.CMND_PLACE;
                        }
                        else if (!String.IsNullOrWhiteSpace(patient.CCCD_NUMBER))
                        {
                            rdo.CMND = patient.CCCD_NUMBER;
                            rdo.NGAY_CAP_CMND = Inventec.Common.DateTime.Convert.TimeNumberToDateString(patient.CCCD_DATE ?? 0);
                            rdo.NOI_CAP_CMND = patient.CCCD_PLACE;
                        }
                        else if (!String.IsNullOrWhiteSpace(patient.RELATIVE_CMND_NUMBER))
                        {
                            rdo.CMND = patient.RELATIVE_CMND_NUMBER;
                            rdo.NGAY_CAP_CMND = "";
                            rdo.NOI_CAP_CMND = "";
                        }
                        rdo.NGHE_NGHIEP = patient.CAREER_NAME;
                        rdo.NGAY_VAO = Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(treatment.IN_TIME);
                        rdo.NGAY_RA = Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(treatment.OUT_TIME ?? 0);
                        rdo.TUOI_THAI = "";//chua có
                        rdo.NGAY_CT = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME ?? 0);
                        rdo.HO_TEN_CHA = treatment.TDL_PATIENT_FATHER_NAME;
                        rdo.HO_TEN_ME = treatment.TDL_PATIENT_MOTHER_NAME;
                        rdo.PP_DIEUTRI = treatment.TREATMENT_METHOD;
                        string icd = treatment.ICD_NAME;
                        if (!string.IsNullOrWhiteSpace(treatment.ICD_TEXT))
                        {
                            icd += ";" + treatment.ICD_TEXT.Trim(';');
                        }
                        if (treatment.ICD_CODE != null || treatment.ICD_NAME != null)
                        {
                            rdo.ICD_FULL += treatment.ICD_CODE + " - " + treatment.ICD_NAME + ";";
                        }
                        rdo.CHAN_DOAN = icd;
                        rdo.LOI_DAN_BS = treatment.ADVISE;
                        rdo.MA_DVI = "";//chưa có
                        rdo.TEN_DVI = patient.WORK_PLACE;
                        var workPlac = workPlaces.FirstOrDefault(o => o.ID == patient.WORK_PLACE_ID);
                        if (workPlac != null)
                        {
                            rdo.MA_DVI = workPlac.WORK_PLACE_CODE;
                            rdo.TEN_DVI = workPlac.WORK_PLACE_NAME;
                        }
                        rdo.LOAI_QUAN_HE = treatment.TDL_PATIENT_RELATIVE_TYPE;
                        rdo.NGUOI_DAI_DIEN = treatment.TDL_PATIENT_RELATIVE_NAME;
                        rdo.SO_KCB = treatment.TREATMENT_CODE;
                        rdo.TEN_BSY = treatment.DOCTOR_USERNAME;
                        var employee = ListEmployee.FirstOrDefault(o => o.LOGINNAME == treatment.DOCTOR_LOGINNAME);
                        rdo.MA_BS = employee != null ? employee.DIPLOMA : "";
                        rdo.SO_SERI = "";//để trống tự sinh
                        rdo.MAU_SO = "";//chưa có
                        //bổ sung sau
                        rdo.SO = "";
                        rdo.QUYEN_SO = "";

                        //thong tin rieng
                        int age = Inventec.Common.DateTime.Calculation.Age(treatment.TDL_PATIENT_DOB, treatment.IN_TIME);
                        if (age > 0 && age < 7 && String.IsNullOrWhiteSpace(treatment.TDL_HEIN_CARD_NUMBER))
                        {
                            rdo.TEKT = "1";
                        }

                        if (dicLastPatientType.ContainsKey(treatment.ID) && !String.IsNullOrWhiteSpace(dicLastPatientType[treatment.ID].HEIN_CARD_NUMBER))
                        {
                            rdo.MA_SOBHXH = dicLastPatientType[treatment.ID].HEIN_CARD_NUMBER.Substring(5, 10);
                            rdo.MA_THE = dicLastPatientType[treatment.ID].HEIN_CARD_NUMBER;
                            rdo.DIA_CHI = dicLastPatientType[treatment.ID].ADDRESS;
                        }
                        else if (!String.IsNullOrWhiteSpace(treatment.TDL_HEIN_CARD_NUMBER))
                        {
                            rdo.MA_SOBHXH = treatment.TDL_HEIN_CARD_NUMBER.Substring(5, 10);
                            rdo.MA_THE = treatment.TDL_HEIN_CARD_NUMBER;
                            rdo.DIA_CHI = treatment.TDL_PATIENT_ADDRESS;
                        }

                        ListRdoRv.Add(rdo);
                    }
                }
                if (castFilter.IS_DICHVU == true)
                {
                    ListRdoRv = ListRdoRv.ToList();

                }
                else
                {

                    ListRdoRv = ListRdoRv.Where(o => !String.IsNullOrWhiteSpace(o.MA_SOBHXH) || o.TEKT == "1").ToList();
                }
                //ListRdoRv = ListRdoRv.Where(o => !String.IsNullOrWhiteSpace(o.MA_SOBHXH) || o.TEKT == "1").ToList();
                
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM.HasValue)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));
                }

                if (castFilter.TIME_TO.HasValue)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));
                }

                objectTag.AddObjectData(store, "ReportRV", ListRdoRv);
                objectTag.AddObjectData(store, "ReportCS", ListRdoCs);
                objectTag.AddObjectData(store, "ReportNO", ListRdoNo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
