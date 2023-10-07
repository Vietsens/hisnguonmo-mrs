using HIS.Common.Treatment;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisTreatmentResult;
using MOS.MANAGER.HisTreatmentType;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00817
{
    public class Mrs00817Processor : AbstractProcessor
    {
        public Mrs00817Filter filter;
        public List<Mrs00817RDO> listRdo = new List<Mrs00817RDO>();
        public List<HIS_BRANCH> listBranch = new List<HIS_BRANCH>();
        public List<V_HIS_HEIN_APPROVAL> listHeinApporval = new List<V_HIS_HEIN_APPROVAL>();
        public List<V_HIS_SERE_SERV_3> listSereServ = new List<V_HIS_SERE_SERV_3>();
        public List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();
        public List<HIS_TREATMENT_TYPE> listTreatmentType = new List<HIS_TREATMENT_TYPE>();
        public List<HIS_TREATMENT_RESULT> listTreatmentResult = new List<HIS_TREATMENT_RESULT>();
        public List<HIS_PATIENT_TYPE> listPatientType = new List<HIS_PATIENT_TYPE>();
        public List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();
        public Mrs00817Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }
        public override Type FilterType()
        {
            return typeof(Mrs00817Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
        protected override bool GetData()
        {
            bool result = false;
            filter = (Mrs00817Filter)this.reportFilter;
            try
            {
                listBranch = new HisBranchManager().Get(new HisBranchFilterQuery());
                HisHeinApprovalViewFilterQuery heinApprovalFilter = new HisHeinApprovalViewFilterQuery();
                heinApprovalFilter.BRANCH_ID = filter.BRANCH_ID;
                heinApprovalFilter.EXECUTE_TIME_FROM = filter.TIME_FROM;
                heinApprovalFilter.EXECUTE_TIME_TO = filter.TIME_TO;
                heinApprovalFilter.ORDER_FIELD = "EXECUTE_TIME";
                heinApprovalFilter.ORDER_DIRECTION = "ASC";
                listHeinApporval = new HisHeinApprovalManager().GetView(heinApprovalFilter);
                var skip = 0;
                var heinApprovalIds = listHeinApporval.Select(x => x.ID).Distinct().ToList();
                HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                treatmentFilter.IN_TIME_FROM = filter.TIME_FROM;
                treatmentFilter.IN_TIME_TO = filter.TIME_TO;
                listTreatment = new HisTreatmentManager().Get(treatmentFilter);
                if (filter.BRANCH_ID!=null)
                {
                  listTreatment=  listTreatment.Where(x => x.BRANCH_ID == filter.BRANCH_ID).ToList();
                }
                if (filter.BRANCH_IDs!=null)
                {
                    listTreatment = listTreatment.Where(x => filter.BRANCH_IDs.Contains(x.BRANCH_ID)).ToList();   
                }
                if (filter.PATIENT_TYPE_IDs!=null)
                {
                    listTreatment = listTreatment.Where(x => filter.PATIENT_TYPE_IDs.Contains(x.TDL_PATIENT_TYPE_ID ?? 0)).ToList();
                }
                var IdTreatments = listTreatment.Select(x => x.ID).Distinct().ToList();
                while (IdTreatments.Count - skip > 0)
                {
                    var listIds = IdTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisSereServView3FilterQuery sereServFilter = new HisSereServView3FilterQuery();
                    sereServFilter.TREATMENT_IDs = listIds;
                    var sereServ = new HisSereServManager().GetView3(sereServFilter);
                    listSereServ.AddRange(sereServ);
                }
                //var treatmentIds = listHeinApporval.Select(x => x.TREATMENT_ID).Distinct().ToList();
                //skip = 0;
                //while (treatmentIds.Count - skip > 0)
                //{
                //    var listIds = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                //    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                //    HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                //    treatmentFilter.IDs = listIds;
                //    var treatments = new HisTreatmentManager().Get(treatmentFilter);
                //    listTreatment.AddRange(treatments);
                //}
                listTreatmentType = new HisTreatmentTypeManager().Get(new HisTreatmentTypeFilterQuery());
                //listTreatmentResult = new HisTreatmentResultManager().Get(new HisTreatmentResultFilterQuery());
                listPatientType = new HisPatientTypeManager().Get(new HisPatientTypeFilterQuery());
                listDepartment = new HisDepartmentManager().Get(new HisDepartmentFilterQuery());
                result = true;
            }
            catch (Exception ex)
            {
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
                if (IsNotNullOrEmpty(listTreatment))
                {
                    foreach (var item in listTreatment)
                    {
                        Mrs00817RDO rdo = new Mrs00817RDO();
                        rdo.BRANCH_ID = item.BRANCH_ID;
                        rdo.TREATMENT_ID = item.ID;
                        rdo.PATIENT_CODE = item.TDL_PATIENT_CODE;
                        rdo.PATIENT_NAME = item.TDL_PATIENT_NAME;
                        rdo.DOB = item.TDL_PATIENT_DOB/100000;
                        rdo.GENDER_CODE = item.TDL_PATIENT_GENDER_ID.ToString();
                        rdo.GENDER_NAME = item.TDL_PATIENT_GENDER_NAME;
                        rdo.VIR_ADDRESS = item.TDL_PATIENT_ADDRESS;
                        var department = listDepartment.Where(x => x.ID == item.END_DEPARTMENT_ID).FirstOrDefault();
                        if (item.END_DEPARTMENT_ID!=null)
                        {
                           department = listDepartment.Where(x => x.ID == item.END_DEPARTMENT_ID).FirstOrDefault();
                        }
                        else if (item.LAST_DEPARTMENT_ID != null)
                        {
                            department = listDepartment.Where(x => x.ID == item.LAST_DEPARTMENT_ID).FirstOrDefault();
                        }
                        else
                        {
                            department = listDepartment.Where(x => x.ID == item.IN_DEPARTMENT_ID).FirstOrDefault();
                        }
                        if (department!=null)
                        {
                            rdo.DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                            rdo.DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                        }
                        var treatmentType = listTreatmentType.Where(x => x.ID == item.TDL_TREATMENT_TYPE_ID).FirstOrDefault();
                        if (treatmentType!=null)
                        {
                            rdo.TREATMENT_TYPE_CODE = treatmentType.TREATMENT_TYPE_CODE;
                            rdo.TREATMENT_TYPE_NAME = treatmentType.TREATMENT_TYPE_NAME;
                        }
                        rdo.TREATMENT_CODE = item.TREATMENT_CODE;
                        rdo.HEIN_CARD_NUMBER = item.TDL_HEIN_CARD_NUMBER;
                        rdo.HEIN_MEDI_ORG_CODE = item.TDL_HEIN_MEDI_ORG_CODE;
                        //var treatment = listTreatment.Where(x => x.ID == item.ID).FirstOrDefault();
                        //if (treatment != null)
                        //{
                            var patientType = listPatientType.Where(x => x.ID == item.TDL_PATIENT_TYPE_ID).FirstOrDefault();
                            rdo.PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                            rdo.PATIENT_TYPE_NAME =patientType.PATIENT_TYPE_NAME;
                            rdo.IN_TIME = item.IN_TIME;
                            rdo.OUT_TIME = item.OUT_TIME;
                            rdo.CLINICAL_IN_TIME = item.CLINICAL_IN_TIME ?? 0;
                            rdo.ICD_CODE_MAIN = item.ICD_CODE;
                            rdo.ICD_CODE_EXTRA = item.ICD_SUB_CODE;
                            if (item.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                            {

                                if (item.OUT_TIME.HasValue)
                                {
                                    if (item.TREATMENT_DAY_COUNT.HasValue)
                                    {
                                        rdo.TOTAL_DAY = Convert.ToInt64(item.TREATMENT_DAY_COUNT.Value);
                                    }
                                    else
                                    {
                                        rdo.TOTAL_DAY = Calculation.DayOfTreatment(item.CLINICAL_IN_TIME.HasValue ? item.CLINICAL_IN_TIME : item.IN_TIME, item.OUT_TIME, item.TREATMENT_END_TYPE_ID, item.TREATMENT_RESULT_ID, PatientTypeEnum.TYPE.BHYT);
                                    }
                                }
                            }
                            else
                            {
                                if (item.OUT_TIME.HasValue && item.CLINICAL_IN_TIME.HasValue)
                                {

                                    if (item.TREATMENT_DAY_COUNT.HasValue)
                                    {
                                        rdo.TOTAL_DAY = Convert.ToInt64(item.TREATMENT_DAY_COUNT.Value);
                                    }
                                    else
                                    {
                                        rdo.TOTAL_DAY = Calculation.DayOfTreatment(item.CLINICAL_IN_TIME.HasValue ? item.CLINICAL_IN_TIME : item.IN_TIME, item.OUT_TIME, item.TREATMENT_END_TYPE_ID, item.TREATMENT_RESULT_ID, PatientTypeEnum.TYPE.BHYT);
                                    }
                                    if (rdo.TOTAL_DAY == 0)
                                    {
                                        rdo.TOTAL_DAY = null;
                                    }
                                }

                            }
                        //}
                        var sereServ = listSereServ.Where(x => x.TDL_TREATMENT_ID == item.ID).ToList();
                        if (sereServ != null)
                        {
                            ProcessTotalPrice(rdo, sereServ);
                        }
                        listRdo.Add(rdo);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
        private void ProcessTotalPrice(Mrs00817RDO rdo, List<V_HIS_SERE_SERV_3> hisSereServs)
        {
            try
            {
                foreach (var sereServ in hisSereServs)
                {
                    if (!sereServ.VIR_TOTAL_HEIN_PRICE.HasValue || sereServ.VIR_TOTAL_HEIN_PRICE.Value <= 0)
                        continue;

                    var TotalPriceTreatment = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits, sereServ, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == rdo.BRANCH_ID) ?? new HIS_BRANCH());

                    rdo.TOTAL_PRICE += TotalPriceTreatment;
                    rdo.TOTAL_PATIENT_PRICE += TotalPriceTreatment - (sereServ.VIR_TOTAL_HEIN_PRICE ?? 0);
                    rdo.TOTAL_HEIN_PRICE += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                    if (checkBhytNsd(rdo))
                    {
                        rdo.TOTAL_HEIN_PRICE_NDS = rdo.TOTAL_HEIN_PRICE;
                        rdo.TOTAL_HEIN_PRICE = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        private bool checkBhytNsd(Mrs00817RDO rdo)
        {
            bool result = false;
            try
            {
                if (MRS.MANAGER.Config.ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Other.Contains(rdo.ICD_CODE_MAIN))
                {
                    result = true;
                }
                else if (!String.IsNullOrEmpty(rdo.ICD_CODE_MAIN))
                {
                    if (rdo.HEIN_CARD_NUMBER.Substring(0, 2).Equals("TE") && MRS.MANAGER.Config.ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Te.Contains(rdo.ICD_CODE_MAIN.Substring(0, 3)))
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            objectTag.AddObjectData(store, "Report", listRdo);
        }
    }
}
