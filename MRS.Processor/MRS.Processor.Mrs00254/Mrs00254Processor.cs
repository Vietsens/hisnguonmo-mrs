using MOS.MANAGER.HisTransaction;
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
 
using MOS.MANAGER.HisTreatment; 

namespace MRS.Processor.Mrs00254
{
    public class Mrs00254Processor : AbstractProcessor
    {
        private List<Mrs00254RDO> listRdo = new List<Mrs00254RDO>(); 
        CommonParam paramGet = new CommonParam(); 
        List<V_HIS_TRANSACTION> ListCurrentDeposit = new List<V_HIS_TRANSACTION>(); 
        List<V_HIS_TREATMENT> ListTreatment = new List<V_HIS_TREATMENT>(); 
        Dictionary<long, V_HIS_TREATMENT> dicTreatment = new Dictionary<long, V_HIS_TREATMENT>(); 
        List<Mrs00254RDO> Department = new List<Mrs00254RDO>(); 
        public Mrs00254Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00254Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            try
            {
                HisTransactionViewFilterQuery Dpsfilter = new HisTransactionViewFilterQuery();
                Dpsfilter.TRANSACTION_TIME_FROM = ((Mrs00254Filter)this.reportFilter).TIME_FROM;
                Dpsfilter.TRANSACTION_TIME_TO = ((Mrs00254Filter)this.reportFilter).TIME_TO;
                Dpsfilter.ACCOUNT_BOOK_ID = ((Mrs00254Filter)this.reportFilter).ACCOUNT_BOOK_ID;
                Dpsfilter.TRANSACTION_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU }; 
                ListCurrentDeposit = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).GetView(Dpsfilter); 
                var listTreatmentId = ListCurrentDeposit.Select(o => o.TREATMENT_ID ?? 0).Distinct().ToList();


                if (((Mrs00254Filter)this.reportFilter).CASHIER_LOGINNAME != null)
                {
                    ListCurrentDeposit = ListCurrentDeposit.Where(o => o.CASHIER_LOGINNAME == ((Mrs00254Filter)this.reportFilter).CASHIER_LOGINNAME).ToList();
                }
                ListTreatment = new HisTreatmentManager(paramGet).GetViewByIds(listTreatmentId); 
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    foreach (var item in ListTreatment)
                    {
                        if (!dicTreatment.ContainsKey(item.ID))
                        {
                            dicTreatment.Add(item.ID, item); 
                        }
                    }
                }
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
            var result = true; 
            try
            {
                if (IsNotNullOrEmpty(ListCurrentDeposit))
                {
                    listRdo.Clear(); 
                    ListCurrentDeposit = ListCurrentDeposit.Where(o => o.IS_CANCEL != 1 && o.IS_DELETE != 1).ToList(); 
                    foreach (var r in ListCurrentDeposit)
                    {
                        if (!dicTreatment.ContainsKey(r.TREATMENT_ID ?? 0))
                        {
                            continue; 
                        }
                        string TreatmentEndType = ""; 
                        long departmentid = 0; 
                        string departmentname = ""; 

                        if (dicTreatment[r.TREATMENT_ID ?? 0].TREATMENT_END_TYPE_ID.HasValue)
                        {
                            TreatmentEndType = dicTreatment[r.TREATMENT_ID ?? 0].TREATMENT_END_TYPE_NAME; 
                            departmentid = dicTreatment[r.TREATMENT_ID ?? 0].END_DEPARTMENT_ID ?? 0; 
                            departmentname = dicTreatment[r.TREATMENT_ID ?? 0].END_DEPARTMENT_NAME; 
                        }
                        else
                        {
                            TreatmentEndType = "Chưa ra"; 
                            departmentid = 0; 
                            departmentname = "CHƯA XÁC ĐỊNH KHOA"; 
                        }

                        Mrs00254RDO rdo = new Mrs00254RDO()
                        {
                            TRANSACTION_CODE = r.TRANSACTION_CODE,
                            PATIENT_CODE = r.TDL_PATIENT_CODE,
                            VIR_PATIENT_NAME = r.TDL_PATIENT_NAME,
                            DESCRIPTION = r.DESCRIPTION,
                            AMOUNT = r.AMOUNT,
                            CASHIER_USERNAME = r.CASHIER_USERNAME,
                            CREATE_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.CREATE_TIME ?? 0),
                            TRANSACTION_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.TRANSACTION_DATE),
                            IS_OUT = TreatmentEndType,
                            DEPARTMENT_ID = departmentid,
                            DEPARTMENT_NAME = departmentname,
                            DOB = Inventec.Common.DateTime.Convert.TimeNumberToDateString(dicTreatment[r.TREATMENT_ID ?? 0].TDL_PATIENT_DOB),
                            VIR_ADDRESS = dicTreatment[r.TREATMENT_ID ?? 0].TDL_PATIENT_ADDRESS,
                            GENDER_NAME = dicTreatment[r.TREATMENT_ID ?? 0].TDL_PATIENT_GENDER_NAME
                        }; 

                        listRdo.Add(rdo); 
                    }
                    Department = listRdo.GroupBy(o => o.DEPARTMENT_ID).Select(p => p.First()).ToList(); 
                    result = true; 
                }
            }
            catch (Exception ex)
            {
                result = false; 
                LogSystem.Error(ex); 
            }
            return result; 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                if (((Mrs00254Filter)this.reportFilter).TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00254Filter)this.reportFilter).TIME_FROM)); 
                }
                if (((Mrs00254Filter)this.reportFilter).TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00254Filter)this.reportFilter).TIME_TO)); 
                }
                listRdo = listRdo.OrderBy(o => o.TRANSACTION_DATE_STR).ThenBy(t => t.TRANSACTION_CODE).ToList(); 

                objectTag.AddObjectData(store, "Patient", listRdo); 
                objectTag.AddObjectData(store, "Department", Department); 
                objectTag.AddRelationship(store, "Department", "Patient", "DEPARTMENT_ID", "DEPARTMENT_ID"); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
