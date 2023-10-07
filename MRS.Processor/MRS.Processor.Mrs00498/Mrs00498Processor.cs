using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisDepartment;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTreatment;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MRS.Processor.Mrs00498
{
    class Mrs00498Processor : AbstractProcessor
    {
        Mrs00498Filter castFilter = new Mrs00498Filter();
        List<Mrs00498RDO> listRdo = new List<Mrs00498RDO>();
        List<Mrs00498RDO> listRdoParent = new List<Mrs00498RDO>();

        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>();
        List<V_HIS_TREATMENT_FEE> listTreatmentFees = new List<V_HIS_TREATMENT_FEE>();
        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>();

        List<HIS_DEPARTMENT> listDepartments = new List<HIS_DEPARTMENT>();

        //string DEPA__EXAM_CODE = "MRS.MRS_495.DEPARTMENT_CODE_EXAM";
        //string DEPA__CLIN_IN_CODE = "MRS.MRS_495.DEPARTMENT_CODE_CLINICAL_IN";
        //string DEPA__CLIN_OUT_CODE = "MRS.MRS_495.DEPARTMENT_CODE_CLINICAL_OUT";

        string LIMIT_PRICE__EXAM = "MRS.MRS_495.LIMIT_PRICE_EXAM";
        string LIMIT_PRICE__CLIN_IN = "MRS.MRS_495.LIMIT_PRICE_CLINICAL_IN";
        string LIMIT_PRICE__CLIN_OUT = "MRS.MRS_495.LIMIT_PRICE_CLINICAL_OUT";

        string CAPD__ICD_CODE = "MRS.MRS_495.ICD_CODE_CAPD";

        List<string> ICD_CODE__CAPDs = new List<string>();           // mã bệnh capd

        //List<long> DEPARTMENT_ID__EXAMs = new List<long>();
        //List<long> DEPARTMENT_ID__INs = new List<long>();
        //List<long> DEPARTMENT_ID__OUTs = new List<long>();

        /// <summary>
        /// mức trần theo từng loại điều trị
        /// </summary>
        public decimal LIMIT_PRICE_EXAM = 0;
        public decimal LIMIT_PRICE_CLIN_IN = 0;
        public decimal LIMIT_PRICE_CLIN_OUT = 0;
        ///// <summary>
        ///// 0: không có loại điều trị
        ///// 1: capd
        ///// 2: khám
        ///// 3: nội trú
        ///// 4: ngoại trú
        ///// </summary>
        //public long TREATMENT_TYPE_ID = 0;


        public Mrs00498Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00498Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00498Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay bao cao Mrs00498: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                // config
                listDepartments = new MOS.MANAGER.HisDepartment.HisDepartmentManager(param).Get(new HisDepartmentFilterQuery()) ?? new List<HIS_DEPARTMENT>();
                if (IsNotNullOrEmpty(listDepartments))
                {
                    //DEPARTMENT_ID__EXAMs = GetListConfig(DEPA__EXAM_CODE);
                    //DEPARTMENT_ID__INs = GetListConfig(DEPA__CLIN_IN_CODE);
                    //DEPARTMENT_ID__OUTs = GetListConfig(DEPA__CLIN_OUT_CODE);

                    var config = Loader.dictionaryConfig[CAPD__ICD_CODE];
                    if (config == null) throw new ArgumentNullException(CAPD__ICD_CODE);
                    ICD_CODE__CAPDs = (String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE).Split(',').ToList();

                    //tiền các mức trần
                    LIMIT_PRICE_EXAM = GetConfig(LIMIT_PRICE__EXAM);
                    LIMIT_PRICE_CLIN_IN = GetConfig(LIMIT_PRICE__CLIN_IN);
                    LIMIT_PRICE_CLIN_OUT = GetConfig(LIMIT_PRICE__CLIN_OUT);

                    //lọc danh sách khoa theo điều kiện lọc
                    if (castFilter.DEPARTMENT_ID != null)
                    {
                        listDepartments = listDepartments.Where(w => w.ID == castFilter.DEPARTMENT_ID).ToList();
                    }

                    //if (castFilter.IS_CAPD)
                    //{
                    //    //TREATMENT_TYPE_ID = 1;
                    //    LIMIT_PRICE = GetConfig(LIMIT_PRICE__EXAM);
                    //    if (castFilter.DEPARTMENT_ID != null && castFilter.DEPARTMENT_ID != 0)
                    //        listDepartments = listDepartments.Where(w => w.ID == castFilter.DEPARTMENT_ID).ToList();
                    //    else
                    //        listDepartments = listDepartments.Where(w => DEPARTMENT_ID__EXAMs.Contains(w.ID)).ToList();
                    //}
                    //else
                    //{
                    //    if (DEPARTMENT_ID__EXAMs != null && DEPARTMENT_ID__EXAMs.Where(w => w == castFilter.DEPARTMENT_ID).Count() > 0)
                    //    {
                    //        TREATMENT_TYPE_ID = 2;
                    //        LIMIT_PRICE = GetConfig(LIMIT_PRICE__EXAM);
                    //        listDepartments = listDepartments.Where(w => DEPARTMENT_ID__EXAMs.Contains(w.ID)).ToList();
                    //    }
                    //    else if (DEPARTMENT_ID__INs != null && DEPARTMENT_ID__INs.Where(w => w == castFilter.DEPARTMENT_ID).Count() > 0)
                    //    {
                    //        TREATMENT_TYPE_ID = 3;
                    //        LIMIT_PRICE = GetConfig(LIMIT_PRICE__CLIN_IN);
                    //        listDepartments = listDepartments.Where(w => DEPARTMENT_ID__INs.Contains(w.ID)).ToList();
                    //    }
                    //    else if (DEPARTMENT_ID__OUTs != null && DEPARTMENT_ID__OUTs.Where(w => w == castFilter.DEPARTMENT_ID).Count() > 0)
                    //    {
                    //        TREATMENT_TYPE_ID = 4;
                    //        LIMIT_PRICE = GetConfig(LIMIT_PRICE__CLIN_OUT);
                    //        listDepartments = listDepartments.Where(w => DEPARTMENT_ID__OUTs.Contains(w.ID)).ToList();
                    //    }
                    //    else
                    //        listDepartments.Clear();
                    //}
                }

                //HisHeinApprovalFilterQuery heinApprovalFilter = new HisHeinApprovalFilterQueryQuery(); 
                //heinApprovalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM; 
                //heinApprovalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO; 
                //var listHeinApprovals = new MOS.MANAGER.HisHeinApproval.HisHeinApprovalManager(param).Get(heinApprovalFilter).Data.Data; 

                var skip = 0;
                //while (listHeinApprovals.Count - skip > 0)
                //{
                //    var listIDs = listHeinApprovals.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                //    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                //    HisTreatmentViewFilterQueryQuery treatmentViewFilter = new HisTreatmentViewFilterQueryQuery(); 
                //    treatmentViewFilter.IDs = listIDs.Select(s => s.TREATMENT_ID).ToList(); 
                //    treatmentViewFilter.END_DEPARTMENT_IDs = listDepartments.Select(s => s.ID).ToList(); 
                //    listTreatments.AddRange(new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentViewFilter).Data.Data ?? new List<V_HIS_TREATMENT>()); 
                //}

                HisTreatmentViewFilterQuery treatmentViewFilter = new HisTreatmentViewFilterQuery();
                treatmentViewFilter.OUT_TIME_FROM = castFilter.TIME_FROM;
                treatmentViewFilter.OUT_TIME_TO = castFilter.TIME_TO;
                treatmentViewFilter.END_DEPARTMENT_IDs = listDepartments.Select(s => s.ID).ToList();
                listTreatments.AddRange(new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentViewFilter) ?? new List<V_HIS_TREATMENT>());

                // chỉ lấy bn có thẻ bhyt
                listTreatments = listTreatments.Where(w => w.TDL_HEIN_CARD_NUMBER != null).ToList();

                //nếu lọc bệnh nhân CAPD thì chỉ lấy bệnh nhân mắc bệnh CAPD
                if (castFilter.IS_CAPD == true)
                {
                    listTreatments = listTreatments.Where(w => ICD_CODE__CAPDs.Contains(w.ICD_CODE)).ToList();
                }

                //lọc theo diện điều trị
                if (castFilter.TREATMENT_TYPE_IDs != null)
                {
                    listTreatments = listTreatments.Where(w => castFilter.TREATMENT_TYPE_IDs.Contains(w.TDL_TREATMENT_TYPE_ID ?? 0)).ToList();
                }


                skip = 0;
                while (listTreatments.Count - skip > 0)
                {
                    var listIDs = listTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisTreatmentFeeViewFilterQuery treatmentFeeViewFilter = new HisTreatmentFeeViewFilterQuery();
                    treatmentFeeViewFilter.IDs = listIDs.Select(s => s.ID).ToList();
                    listTreatmentFees.AddRange(new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetFeeView(treatmentFeeViewFilter));

                    HisSereServViewFilterQuery sereServViewFilter = new HisSereServViewFilterQuery();
                    sereServViewFilter.TREATMENT_IDs = listIDs.Select(s => s.ID).ToList();
                    sereServViewFilter.HAS_EXECUTE = true;
                    //sereServViewFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE; 
                    listSereServs.AddRange(new MOS.MANAGER.HisSereServ.HisSereServManager(param).GetView(sereServViewFilter));
                }

                listSereServs = listSereServs.Where(w => w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected decimal GetConfig(string key)
        {
            var config = Loader.dictionaryConfig[key];
            if (config == null) throw new ArgumentNullException(key);
            string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;

            decimal result = 0;
            try
            {
                result = Convert.ToDecimal(value);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected List<long> GetListConfig(string key)
        {
            var config = Loader.dictionaryConfig[key];
            if (config == null) throw new ArgumentNullException(key);
            string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;

            var departmentCode = value.Split(',').ToList();

            var result = listDepartments.Where(w => departmentCode.Contains(w.DEPARTMENT_CODE)).Select(s => s.ID).ToList() ?? new List<long>();

            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                listTreatments = listTreatments.Distinct().ToList();
                foreach (var treatment in listTreatments)
                {
                    var rdo = new Mrs00498RDO();
                    rdo.TREATMENT = treatment;
                    rdo.HEIN_CARD_NUMBER = treatment.TDL_HEIN_CARD_NUMBER;
                    rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                    rdo.DOB = treatment.TDL_PATIENT_DOB;

                    var treatmentFee = listTreatmentFees.FirstOrDefault(o => o.ID == treatment.ID);
                    if (treatmentFee != null)
                    {
                        rdo.TOTAL_PRICE = treatmentFee.TOTAL_PRICE ?? 0;
                        rdo.TOTAL_HEIN_PRICE = treatmentFee.TOTAL_HEIN_PRICE ?? 0;
                        rdo.TOTAL_PATIENT_PRICE = treatmentFee.TOTAL_PATIENT_PRICE ?? 0;

                        decimal LIMIT_PRICE = 0;
                        if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                            LIMIT_PRICE = LIMIT_PRICE_EXAM;
                        else if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                            LIMIT_PRICE = LIMIT_PRICE_CLIN_OUT;
                        else LIMIT_PRICE = LIMIT_PRICE_CLIN_IN;

                        if (treatmentFee.TOTAL_PRICE > LIMIT_PRICE)
                        {
                            rdo.PARENT_ID = 1;
                            rdo.PARENT_NAME = "Chi phí điều trị trên " + LIMIT_PRICE + " đồng";
                        }
                        else
                        {
                            rdo.PARENT_ID = 2;
                            rdo.PARENT_NAME = "Chi phí điều trị dưới " + LIMIT_PRICE + " đồng";
                        }
                    }

                    if (rdo.TOTAL_PATIENT_PRICE != 0)
                    {
                        var sereServ = listSereServs.Where(w => w.TDL_TREATMENT_ID == treatment.ID).ToList();
                        if (IsNotNullOrEmpty(sereServ))
                        {
                            rdo.EXAM_PRICE = sereServ.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Sum(su => su.VIR_TOTAL_PRICE ?? 0);
                            rdo.TEST_PRICE = sereServ.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).Sum(su => su.VIR_TOTAL_PRICE ?? 0);
                            rdo.DIIM_PRICE = sereServ.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA).Sum(su => su.VIR_TOTAL_PRICE ?? 0);
                            rdo.MISU_PRICE = sereServ.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).Sum(su => su.VIR_TOTAL_PRICE ?? 0);
                            rdo.FUEX_PRICE = sereServ.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN).Sum(su => su.VIR_TOTAL_PRICE ?? 0);
                            rdo.MEDI_PRICE = sereServ.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).Sum(su => su.VIR_TOTAL_PRICE ?? 0);
                            rdo.MATE_PRICE = sereServ.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).Sum(su => su.VIR_TOTAL_PRICE ?? 0);
                            rdo.BED_PRICE = sereServ.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G).Sum(su => su.VIR_TOTAL_PRICE ?? 0);
                            rdo.ENDO_PRICE = sereServ.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS).Sum(su => su.VIR_TOTAL_PRICE ?? 0);
                            rdo.SUIM_PRICE = sereServ.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA).Sum(su => su.VIR_TOTAL_PRICE ?? 0);
                            rdo.SURG_PRICE = sereServ.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT).Sum(su => su.VIR_TOTAL_PRICE ?? 0);
                            rdo.OTHER_PRICE = sereServ.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC).Sum(su => su.VIR_TOTAL_PRICE ?? 0);
                            rdo.PHCN_PRICE = sereServ.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PHCN).Sum(su => su.VIR_TOTAL_PRICE ?? 0);
                            rdo.BLOOD_PRICE = sereServ.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU).Sum(su => su.VIR_TOTAL_PRICE ?? 0);
                            rdo.GPBL_PRICE = sereServ.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__GPBL).Sum(su => su.VIR_TOTAL_PRICE ?? 0);
                        }

                        listRdo.Add(rdo);
                    }
                }

                listRdoParent = listRdo.GroupBy(s => s.PARENT_ID).Select(s => new Mrs00498RDO()
                {
                    PARENT_ID = s.First().PARENT_ID,
                    PARENT_NAME = s.First().PARENT_NAME
                }).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error("Lỗi xảy ra tại ProcessData: " + ex);
                result = false;
            }
            return result;
        }



        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", castFilter.TIME_FROM);
                dicSingleTag.Add("TIME_TO", castFilter.TIME_TO);

                dicSingleTag.Add("DEPARTMENT_NAME", String.Join(", ", listDepartments.Select(s => s.DEPARTMENT_NAME.ToUpper())));
                dicSingleTag.Add("TREATMENT_TYPE", castFilter.TREATMENT_TYPE_IDs != null ? string.Join(",", HisTreatmentTypeCFG.HisTreatmentTypes.Where(o => castFilter.TREATMENT_TYPE_IDs.Contains(o.ID)).ToList()) : "");
                dicSingleTag.Add("LIMIT_PRICE_EXAM", LIMIT_PRICE_EXAM);
                dicSingleTag.Add("LIMIT_PRICE_CLIN_OUT", LIMIT_PRICE_CLIN_OUT);
                dicSingleTag.Add("LIMIT_PRICE_CLIN_IN", LIMIT_PRICE_CLIN_IN);

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Rdo", listRdo.OrderBy(s => s.PATIENT_NAME).ToList());
                objectTag.AddObjectData(store, "RdoParent", listRdoParent.OrderBy(s => s.PARENT_ID).ToList());
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "RdoParent", "Rdo", "PARENT_ID", "PARENT_ID");
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
