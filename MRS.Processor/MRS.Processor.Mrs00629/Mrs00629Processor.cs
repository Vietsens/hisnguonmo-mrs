using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
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
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisTranPatiForm;
using MOS.MANAGER.HisTreatmentResult;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisCoTreatment;

namespace MRS.Processor.Mrs00629
{
    public class Mrs00629Processor : AbstractProcessor
    {
        Mrs00629Filter filter = null;
        List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();
        List<HIS_PATIENT> listPatient = new List<HIS_PATIENT>();
        List<HIS_BRANCH> listBranch = new List<HIS_BRANCH>();
        List<CoTreatmentDepartment> ListCoTreatmentDepartment = new List<CoTreatmentDepartment>();
        List<HIS_TRAN_PATI_FORM> listTranPatiForm = new List<HIS_TRAN_PATI_FORM>();
        List<HIS_TREATMENT_RESULT> listTreatmentResult = new List<HIS_TREATMENT_RESULT>();
        List<HIS_DEPARTMENT> listHisDepartmentCln = new List<HIS_DEPARTMENT>();
        List<Mrs00629RDO> ListRdo = new List<Mrs00629RDO>();
        Dictionary<string, int> dicLess6 = new Dictionary<string, int>();
        Dictionary<string, int> dicMore60 = new Dictionary<string, int>();
        Dictionary<string, int> dicTransfer = new Dictionary<string, int>();
        Dictionary<string, int> dicExp = new Dictionary<string, int>();

        //thong tin moi
        int COUNT_ALL = 0;
        Dictionary<string, int> dicCountOfPatientType = new Dictionary<string, int>();
        int COUNT_ORG_OTHER = 0;
        int COUNT_PROVINCE_OTHER = 0;
        int COUNT_LESS6 = 0;
        int COUNT_LESS6BH = 0;
        int COUNT_LESS6VP = 0;
        int COUNT_MORE60 = 0;
        int COUNT_MORE60BH = 0;
        int COUNT_MORE60VP = 0;
        int COUNT_FOREIGNER = 0;
        int COUNT_TRANSFEROUT = 0;
        int COUNT_TRANSFEROUT_EXAM = 0;
        int COUNT_TRANSFEROUT_TREATIN = 0;
        int COUNT_TRANSFEROUT_TREATOUT = 0;
        int COUNT_TREATIN = 0;
        int COUNT_TREATOUT = 0;
        decimal TOTAL_DAY_TREATOUT = 0;
        int COUNT_TREATINVP = 0;
        int COUNT_TREATINBH = 0;
        decimal TOTAL_DAY_TREATIN = 0;
        int COUNT_TREATIN_YHCT = 0;
        int COUNT_TREATIN_LESS6 = 0;
        int COUNT_TREATIN_LESS6BH = 0;
        int COUNT_TREATIN_LESS6VP = 0;
        int COUNT_TREATIN_MORE60 = 0;
        int COUNT_TREATIN_MORE60BH = 0;
        int COUNT_TREATIN_MORE60VP = 0;
        int COUNT_TREATOUT_MORE60 = 0;
        int COUNT_TREATOUT_MORE60BH = 0;
        int COUNT_TREATOUT_MORE60VP = 0;

        public Mrs00629Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00629Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00629Filter)this.reportFilter;
            try
            {
                //hinh thuc chuyen tuyen
                HisTranPatiFormFilterQuery tranPatiFromFilter = new HisTranPatiFormFilterQuery();
                tranPatiFromFilter.IS_ACTIVE = 1;
                listTranPatiForm = new HisTranPatiFormManager().Get(tranPatiFromFilter);
                //ket qua dieu tri
                HisTreatmentResultFilterQuery HisTreatmentResultfilter = new HisTreatmentResultFilterQuery();
                HisTreatmentResultfilter.IS_ACTIVE = 1;
                listTreatmentResult = new HisTreatmentResultManager().Get(HisTreatmentResultfilter);
                //chi nhanh
                HisBranchFilterQuery HisBranchfilter = new HisBranchFilterQuery();
                HisBranchfilter.IS_ACTIVE = 1;
                listBranch = new HisBranchManager().Get(HisBranchfilter);

                HisTreatmentFilterQuery listHisTreatmentfilter = new HisTreatmentFilterQuery();
                listHisTreatmentfilter.OUT_TIME_FROM = filter.OUT_TIME_FROM;
                listHisTreatmentfilter.OUT_TIME_TO = filter.OUT_TIME_TO;
                listHisTreatmentfilter.IS_PAUSE = true;
                listHisTreatment = new HisTreatmentManager().Get(listHisTreatmentfilter);
                List<long> listPatientId = listHisTreatment.Select(o => o.PATIENT_ID).Distinct().ToList();
                if (IsNotNullOrEmpty(listPatientId))
                {
                    var skip = 0;
                    while (listPatientId.Count - skip > 0)
                    {
                        var listIDs = listPatientId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientFilterQuery PatientFilter = new HisPatientFilterQuery()
                        {
                            IDs = listIDs
                        };
                        var ListPatientSub = new HisPatientManager().Get(PatientFilter);
                        listPatient.AddRange(ListPatientSub);
                    }
                }
                ListCoTreatmentDepartment = new ManagerSql().GetCoTreatmentDepartment(this.filter);

                //Danh sach khoa lâm sàng
                listHisDepartmentCln = HisDepartmentCFG.DEPARTMENTs.Where(o => o.IS_CLINICAL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).OrderBy(o => o.NUM_ORDER).ToList();

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        private bool Less6(long IN_TIME, long TDL_PATIENT_DOB)
        {
            return TDL_PATIENT_DOB >= IN_TIME - 60000000000;
        }

        private bool Less15(long IN_TIME, long TDL_PATIENT_DOB)
        {
            return TDL_PATIENT_DOB >= IN_TIME - 150000000000;
        }

        private bool More60(long IN_TIME, long TDL_PATIENT_DOB)
        {
            return TDL_PATIENT_DOB < IN_TIME - 600000000000;
        }

        private string PatientTypeCode(long PatientTypeId)
        {
            string result = "";
            try
            {
                result = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == PatientTypeId) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_CODE ?? "";
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private string TreatmentPatientTypeCode(long TreatmentTypeId, long PatientTypeId)
        {
            string result = "";
            try
            {
                result = ((HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == TreatmentTypeId) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_CODE ?? "") + "_" + ((HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == PatientTypeId) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_CODE ?? "");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private string TreatmentTranPatiFormCode(long TreatmentTypeId, long TranPatiFormId)
        {
            string result = "";
            try
            {
                result = ((HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == TreatmentTypeId) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_CODE ?? "") + "_" + ((listTranPatiForm.FirstOrDefault(o => o.ID == TranPatiFormId) ?? new HIS_TRAN_PATI_FORM()).TRAN_PATI_FORM_CODE ?? "");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }
        private string TreatmentResultCode(long TreatmentTypeId, long TreatmentResultId)
        {
            string result = "";
            try
            {
                result = ((HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == TreatmentTypeId) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_CODE ?? "") + "_" + ((listTreatmentResult.FirstOrDefault(o => o.ID == TreatmentResultId) ?? new HIS_TREATMENT_RESULT()).TREATMENT_RESULT_CODE ?? "");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                long patientTypeIdBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                List<long> departmentYhcts = HisDepartmentCFG.HIS_DEPARTMENT_ID__YHCT ?? new List<long>();
                foreach (var item in listHisTreatment)
                {
                    //Số lượng trẻ em
                    if (Less6(item.IN_TIME, item.TDL_PATIENT_DOB))
                    {
                        string KeyTreatmentPatientType = TreatmentPatientTypeCode((item.TDL_TREATMENT_TYPE_ID ?? 0), (item.TDL_PATIENT_TYPE_ID ?? 0));
                        if (!dicLess6.ContainsKey(KeyTreatmentPatientType))
                        {
                            dicLess6.Add(KeyTreatmentPatientType, 1);
                        }
                        else
                        {
                            dicLess6[KeyTreatmentPatientType] += 1;
                        }
                    }
                    //Số lượng người già
                    if (More60(item.IN_TIME, item.TDL_PATIENT_DOB))
                    {
                        string KeyTreatmentPatientType = TreatmentPatientTypeCode((item.TDL_TREATMENT_TYPE_ID ?? 0), (item.TDL_PATIENT_TYPE_ID ?? 0));
                        if (!dicMore60.ContainsKey(KeyTreatmentPatientType))
                        {
                            dicMore60.Add(KeyTreatmentPatientType, 1);
                        }
                        else
                        {
                            dicMore60[KeyTreatmentPatientType] += 1;
                        }
                    }

                    //Số lượng chuyển viện 
                    string keyTranPati = TreatmentTranPatiFormCode((item.TDL_TREATMENT_TYPE_ID ?? 0), (item.TRAN_PATI_FORM_ID ?? 0));
                    if (item.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                    {
                        if (!dicTransfer.ContainsKey(keyTranPati))
                        {
                            dicTransfer.Add(keyTranPati, 1);
                        }
                        else
                        {
                            dicTransfer[keyTranPati] += 1;
                        }
                    }
                    //Số lượng ra viện 
                    string keyResult = TreatmentResultCode(item.TDL_TREATMENT_TYPE_ID ?? 0, item.TREATMENT_RESULT_ID ?? 0);
                    if (item.TREATMENT_END_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                    {
                        if (!dicExp.ContainsKey(keyResult))
                        {
                            dicExp.Add(keyResult, 1);
                        }
                        else
                        {
                            dicExp[keyResult] += 1;
                        }
                    }
                    //thong tin moi
                    COUNT_ALL += 1;
                    //cac doi tuong
                    string keyPatientType = PatientTypeCode(item.TDL_PATIENT_TYPE_ID ?? 0);
                    if (!dicCountOfPatientType.ContainsKey(keyPatientType))
                    {
                        dicCountOfPatientType.Add(keyPatientType, 1);
                    }
                    else
                    {
                        dicCountOfPatientType[keyPatientType] += 1;
                    }
                    var branch = this.listBranch.FirstOrDefault(o => o.ID == item.BRANCH_ID) ?? new HIS_BRANCH();
                    //khac noi dang ky ban dau
                    if (branch.HEIN_MEDI_ORG_CODE != item.TDL_HEIN_MEDI_ORG_CODE)
                    {
                        COUNT_ORG_OTHER += 1;
                    }
                    //khac tinh noi dang ky ban dau
                    if (!(item.TDL_HEIN_MEDI_ORG_CODE ?? "").StartsWith((branch.HEIN_PROVINCE_CODE ?? "")))
                    {
                        COUNT_PROVINCE_OTHER += 1;
                    }
                    //Số lượng trẻ em
                    if (Less6(item.IN_TIME, item.TDL_PATIENT_DOB))
                    {
                        COUNT_LESS6 += 1;
                        if (item.TDL_PATIENT_TYPE_ID == patientTypeIdBhyt)
                        {
                            COUNT_LESS6BH += 1;
                        }
                        else
                        {
                            COUNT_LESS6VP += 1;
                        }
                    }
                    //Số lượng người già
                    if (More60(item.IN_TIME, item.TDL_PATIENT_DOB))
                    {
                        COUNT_MORE60 += 1;
                        if (item.TDL_PATIENT_TYPE_ID == patientTypeIdBhyt)
                        {
                            COUNT_MORE60BH += 1;
                        }
                        else
                        {
                            COUNT_MORE60VP += 1;
                        }
                    }
                    HIS_PATIENT patient = listPatient.FirstOrDefault(o => o.ID == item.PATIENT_ID) ?? new HIS_PATIENT();
                    if (patient.NATIONAL_CODE != "VN")
                    {
                        COUNT_FOREIGNER += 1;
                    }
                    //chuyển viện
                    if (item.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                    {
                        COUNT_TRANSFEROUT += 1;

                        if (item.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                        {
                            COUNT_TRANSFEROUT_TREATIN += 1;
                        }
                        else if (item.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                        {
                            COUNT_TRANSFEROUT_TREATOUT += 1;
                        }
                        else
                        {
                            COUNT_TRANSFEROUT_EXAM += 1;
                        }
                    }
                    //nội trú
                    if (item.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        COUNT_TREATIN += 1;
                        TOTAL_DAY_TREATIN += item.TREATMENT_DAY_COUNT ?? 0;
                        if (item.TDL_PATIENT_TYPE_ID == patientTypeIdBhyt)
                        {
                            COUNT_TREATINBH += 1;
                        }
                        else
                        {
                            COUNT_TREATINVP += 1;
                        }
                        //Số lượng trẻ em
                        if (Less6(item.IN_TIME, item.TDL_PATIENT_DOB))
                        {
                            COUNT_TREATIN_LESS6 += 1;
                            if (item.TDL_PATIENT_TYPE_ID == patientTypeIdBhyt)
                            {
                                COUNT_TREATIN_LESS6BH += 1;
                            }
                            else
                            {
                                COUNT_TREATIN_LESS6VP += 1;
                            }
                        }
                        //Số lượng người già
                        if (More60(item.IN_TIME, item.TDL_PATIENT_DOB))
                        {
                            COUNT_TREATIN_MORE60 += 1;
                            if (item.TDL_PATIENT_TYPE_ID == patientTypeIdBhyt)
                            {
                                COUNT_TREATIN_MORE60BH += 1;
                            }
                            else
                            {
                                COUNT_TREATIN_MORE60VP += 1;
                            }
                        }
                        //điều trị đông y
                        CoTreatmentDepartment coTreatment = ListCoTreatmentDepartment.FirstOrDefault(o => o.TREATMENT_ID == item.ID && departmentYhcts.Contains(o.DEPARTMENT_ID)) ?? new CoTreatmentDepartment();
                        if (coTreatment != null || departmentYhcts.Contains(item.END_DEPARTMENT_ID ?? 0))
                        {
                            COUNT_TREATIN_YHCT += 1;
                        }
                    }
                    //ngoại trú
                    if (item.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                    {
                        COUNT_TREATOUT += 1;
                        TOTAL_DAY_TREATOUT += item.TREATMENT_DAY_COUNT ?? 0;
                        //Số lượng người già
                        if (More60(item.IN_TIME, item.TDL_PATIENT_DOB))
                        {
                            COUNT_TREATOUT_MORE60 += 1;
                            if (item.TDL_PATIENT_TYPE_ID == patientTypeIdBhyt)
                            {
                                COUNT_TREATOUT_MORE60BH += 1;
                            }
                            else
                            {
                                COUNT_TREATOUT_MORE60VP += 1;
                            }
                        }
                    }
                }

                foreach (var item in listHisDepartmentCln)
                {
                    var treatmentSub = listHisTreatment.Where(o => o.END_DEPARTMENT_ID == item.ID).ToList();
                    Mrs00629RDO rdo = new Mrs00629RDO();
                    rdo.DEPARTMENT_ID = item.ID;
                    rdo.DEPARTMENT_NAME = item.DEPARTMENT_NAME;
                    SetExtendField(treatmentSub, rdo);
                    ListRdo.Add(rdo);
                }



            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
        private void SetExtendField(List<HIS_TREATMENT> treatments, Mrs00629RDO rdo)
        {
            try
            {
                foreach (var r in treatments)
                {

                    if (r.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {

                        if (r.TDL_PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            UpDate(rdo.EXAM_BH, r);
                        }
                        else
                        {
                            UpDate(rdo.EXAM_VP, r);
                        }
                    }
                    else if (r.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        rdo.TOTAL_DATE += Calculation.DayOfTreatment(r.CLINICAL_IN_TIME ?? 0, r.OUT_TIME, r.TREATMENT_END_TYPE_ID, r.TREATMENT_RESULT_ID, r.TDL_PATIENT_TYPE_ID == 1 ? PatientTypeEnum.TYPE.BHYT : PatientTypeEnum.TYPE.THU_PHI) ?? 0;

                        if (r.TDL_PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            UpDate(rdo.IN_BH, r);
                        }
                        else
                        {
                            UpDate(rdo.IN_VP, r);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        private void UpDate(CountInfoTreat IN, HIS_TREATMENT r)
        {
            try
            {
                if (r.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                {
                    IN.COUNT_FEMALE += 1;
                }

                if (Less6(r.IN_TIME, r.TDL_PATIENT_DOB))
                {
                    IN.COUNT_CHILD_LESS6 += 1;
                }
                else if (Less15(r.IN_TIME, r.TDL_PATIENT_DOB))
                {
                    IN.COUNT_CHILD_LESS15 += 1;
                }
                else if (More60(r.IN_TIME, r.TDL_PATIENT_DOB))
                {
                    IN.COUNT_MORE60 += 1;
                }
                if (r.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KHOI)
                {
                    IN.COUNT_KHOI += 1;
                }
                else if (r.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__DO)
                {
                    IN.COUNT_DO += 1;
                }

                else if (r.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KTD)
                {
                    IN.COUNT_KTD += 1;
                }

                else if (r.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG)
                {
                    IN.COUNT_NANG += 1;
                }

                else if (r.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET)
                {
                    IN.COUNT_CHET += 1;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        private void UpDate(CountInfo EXAM, HIS_TREATMENT r)
        {
            try
            {
                if (r.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                {
                    EXAM.COUNT_FEMALE += 1;
                }

                if (Less6(r.IN_TIME, r.TDL_PATIENT_DOB))
                {
                    EXAM.COUNT_CHILD_LESS6 += 1;
                }
                else if (Less15(r.IN_TIME, r.TDL_PATIENT_DOB))
                {
                    EXAM.COUNT_CHILD_LESS15 += 1;
                }
                else if (More60(r.IN_TIME, r.TDL_PATIENT_DOB))
                {
                    EXAM.COUNT_MORE60 += 1;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //private int Age(long IN_TIME, long TDL_PATIENT_DOB)
        //{
        //    return (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(IN_TIME) ?? DateTime.Now).Year - (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(TDL_PATIENT_DOB) ?? DateTime.Now).Year;
        //}

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.OUT_TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.OUT_TIME_TO ?? 0));
            dicSingleTag.Add("DIC_LESS6", dicLess6);
            dicSingleTag.Add("DIC_MORE60", dicMore60);
            dicSingleTag.Add("DIC_TRANSFER", dicTransfer);
            dicSingleTag.Add("DIC_EXP", dicExp);


            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "ReportDetail", listHisTreatment);
            objectTag.SetUserFunction(store, "Element", new RDOElement());
            //thong tin moi
            dicSingleTag.Add("COUNT_ALL", COUNT_ALL);
            dicSingleTag.Add("DIC_PATIENT_TYPE", dicCountOfPatientType);
            dicSingleTag.Add("COUNT_ORG_OTHER", COUNT_ORG_OTHER);
            dicSingleTag.Add("COUNT_PROVINCE_OTHER", COUNT_PROVINCE_OTHER);
            dicSingleTag.Add("COUNT_LESS6", COUNT_LESS6);
            dicSingleTag.Add("COUNT_LESS6BH", COUNT_LESS6BH);
            dicSingleTag.Add("COUNT_LESS6VP", COUNT_LESS6VP);
            dicSingleTag.Add("COUNT_MORE60", COUNT_MORE60);
            dicSingleTag.Add("COUNT_MORE60BH", COUNT_MORE60BH);
            dicSingleTag.Add("COUNT_MORE60VP", COUNT_MORE60VP);
            dicSingleTag.Add("COUNT_FOREIGNER", COUNT_FOREIGNER);
            dicSingleTag.Add("COUNT_TRANSFEROUT", COUNT_TRANSFEROUT);
            dicSingleTag.Add("COUNT_TRANSFEROUT_EXAM", COUNT_TRANSFEROUT_EXAM);
            dicSingleTag.Add("COUNT_TRANSFEROUT_TREATIN", COUNT_TRANSFEROUT_TREATIN);
            dicSingleTag.Add("COUNT_TRANSFEROUT_TREATOUT", COUNT_TRANSFEROUT_TREATOUT);
            dicSingleTag.Add("COUNT_TREATIN", COUNT_TREATIN);
            dicSingleTag.Add("COUNT_TREATOUT", COUNT_TREATOUT);
            dicSingleTag.Add("TOTAL_DAY_TREATOUT", TOTAL_DAY_TREATOUT);
            dicSingleTag.Add("COUNT_TREATINVP", COUNT_TREATINVP);
            dicSingleTag.Add("COUNT_TREATINBH", COUNT_TREATINBH);
            dicSingleTag.Add("TOTAL_DAY_TREATIN", TOTAL_DAY_TREATIN);
            dicSingleTag.Add("COUNT_TREATIN_YHCT", COUNT_TREATIN_YHCT);
            dicSingleTag.Add("COUNT_TREATIN_LESS6", COUNT_TREATIN_LESS6);
            dicSingleTag.Add("COUNT_TREATIN_LESS6BH", COUNT_TREATIN_LESS6BH);
            dicSingleTag.Add("COUNT_TREATIN_LESS6VP", COUNT_TREATIN_LESS6VP);
            dicSingleTag.Add("COUNT_TREATIN_MORE60", COUNT_TREATIN_MORE60);
            dicSingleTag.Add("COUNT_TREATIN_MORE60BH", COUNT_TREATIN_MORE60BH);
            dicSingleTag.Add("COUNT_TREATIN_MORE60VP", COUNT_TREATIN_MORE60VP);
            dicSingleTag.Add("COUNT_TREATOUT_MORE60", COUNT_TREATOUT_MORE60);
            dicSingleTag.Add("COUNT_TREATOUT_MORE60BH", COUNT_TREATOUT_MORE60BH);
            dicSingleTag.Add("COUNT_TREATOUT_MORE60VP", COUNT_TREATOUT_MORE60VP);

        }

    }

}
