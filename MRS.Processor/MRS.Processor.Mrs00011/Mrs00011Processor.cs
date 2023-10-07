using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00011
{
    public class Mrs00011Processor : AbstractProcessor
    {
        Mrs00011Filter castFilter = null;
        Dictionary<string, Mrs00011RDO> dicRdo = new Dictionary<string, Mrs00011RDO>(); // key = MATERIAL_TYPE_id & imp_price
        List<Mrs00011RDO> listRdo = new List<Mrs00011RDO>();
        List<V_HIS_TREATMENT_4> listTemp = new List<V_HIS_TREATMENT_4>();
        List<HIS_TRANSACTION> ListTransaction = new List<HIS_TRANSACTION>();
        List<V_HIS_DEPARTMENT_TRAN> listDepartmentTran = new List<V_HIS_DEPARTMENT_TRAN>();
        public Mrs00011Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00011Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00011Filter)this.reportFilter);
                GetTreatment();
                GetDeposit();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetDeposit()
        {
            try
            {
                CommonParam paramGet = new CommonParam();
                var skip = 0;
                var ListTreatmentId = listTemp.Select(o => o.ID).Distinct().ToList();
                while (ListTreatmentId.Count - skip > 0)
                {
                    var listIds = ListTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisTransactionFilterQuery transactionFilter = new HisTransactionFilterQuery();
                    transactionFilter.TREATMENT_IDs = listIds;
                    transactionFilter.IS_CANCEL = false;
                    transactionFilter.TRANSACTION_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU;
                    var ListTransactionSub = new HisTransactionManager(paramGet).Get(transactionFilter);
                    ListTransaction.AddRange(ListTransactionSub ?? new List<HIS_TRANSACTION>());
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void GetTreatment()
        {
            try
            {
                castFilter = ((Mrs00011Filter)this.reportFilter);
                CommonParam getParam = new CommonParam();
              
                HisTreatmentView4FilterQuery viewFilter = new HisTreatmentView4FilterQuery();
                viewFilter.CLINICAL_IN_TIME_FROM = castFilter.TIME_FROM;
                viewFilter.CLINICAL_IN_TIME_TO = castFilter.TIME_TO;
                viewFilter.IS_PAUSE = false;
                listTemp = new HisTreatmentManager(getParam).GetView4(viewFilter);

                if (IsNotNullOrEmpty(listTemp))
                {
                    var skip = 0;
                    while (listTemp.Count > skip)
                    {
                        var listResult = listTemp.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisDepartmentTranViewFilterQuery depaTranFilter = new HisDepartmentTranViewFilterQuery();
                        depaTranFilter.TREATMENT_IDs = listResult.Select(o => o.ID).ToList();
                        depaTranFilter.DEPARTMENT_ID = castFilter.DEPARTMENT_ID;
                        depaTranFilter.IS_RECEIVE = true;
                        var departmentTran = new HisDepartmentTranManager(getParam).GetView(depaTranFilter) ?? new List<V_HIS_DEPARTMENT_TRAN>();
                        listDepartmentTran.AddRange(departmentTran);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(listTemp))
                {
                    foreach (var r in listTemp)
                    {
                        var departmentTran = listDepartmentTran.OrderBy(p => p.DEPARTMENT_IN_TIME).ThenBy(q => q.ID).FirstOrDefault(o => o.TREATMENT_ID == r.ID);
                        var transactions = ListTransaction.Where(p => p.TREATMENT_ID==r.ID).ToList();
                       
                        Mrs00011RDO rdo = new Mrs00011RDO();
                        rdo.PATIENT_CODE = r.TDL_PATIENT_CODE;
                        rdo.VIR_PATIENT_NAME = r.TDL_PATIENT_NAME;
                        rdo.GENDER_NAME = r.TDL_PATIENT_GENDER_NAME;
                        rdo.DOB = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.TDL_PATIENT_DOB);
                        rdo.TREATMENT_CODE = r.TREATMENT_CODE;
                        rdo.VIR_ADDRESS = r.TDL_PATIENT_ADDRESS;
                        rdo.HEIN_CARD_NUM =r.TDL_HEIN_CARD_NUMBER;
                        if (departmentTran != null)
                        {
                            rdo.OPEN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(departmentTran.DEPARTMENT_IN_TIME??0);
                        }
                        rdo.PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o=>o.ID==r.TDL_PATIENT_TYPE_ID)??new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;
                        rdo.ICD_CODE = r.ICD_CODE;
                        rdo.ICD_MAIN_TEXT = r.ICD_NAME;
                        if (transactions != null)
                        {
                            rdo.DEPOSIT_AMOUNT = transactions.Sum(s => s.AMOUNT);
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
                if (this.castFilter.DEPARTMENT_ID != null)
                {
                    dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == this.castFilter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME);
                    dicSingleTag.Add("DEPARTMENT_CODE", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == this.castFilter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE);
                }


                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM ?? 0));
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO ?? 0));
                }
                dicSingleTag.Add("COUNT_TREATMENT", listRdo.Count);
                objectTag.AddObjectData(store, "Report", listRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
