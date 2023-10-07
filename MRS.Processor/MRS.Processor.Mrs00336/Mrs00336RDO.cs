using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00336
{
    public class Mrs00336RDO : V_HIS_TRANSACTION
    {
        public string PATIENT_CODE { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
        //public string TRANSACTION_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        //public string CREATE_TIME { get; set; }
        public string BILL_TIME { get; set; }
        //public decimal AMOUNT { get; set; }
        //public string CASHIER_USERNAME { get; set; }
        public bool IS_RP_OTHERDATE { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public string PATIENT_TYPE_CODE { get; set; }
        public string TDL_HEIN_CARD_NUMBER { get; set; }
        public Mrs00336RDO()
        {

        }
        public Mrs00336RDO(V_HIS_TRANSACTION transaction, List<HIS_TRANSACTION> depositBillSub, List<HIS_TRANSACTION> billSub, V_HIS_TREATMENT treatment)
        {
            try
            {
                if (transaction != null)
                {
                    Inventec.Common.Mapper.DataObjectMapper.Map<V_HIS_TRANSACTION>(this, transaction);

                    this.TRANSACTION_CODE = transaction.TRANSACTION_CODE;
                    //this.CREATE_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.CREATE_TIME.ToString());
                    this.BILL_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(this.TRANSACTION_TIME.ToString());
                    this.TRANSACTION_TYPE_NAME = transaction.TRANSACTION_TYPE_NAME;
                    //this.AMOUNT = r.AMOUNT;
                    this.CASHIER_USERNAME = transaction.CASHIER_USERNAME;
                    this.PATIENT_CODE = transaction.TDL_PATIENT_CODE;
                    this.VIR_PATIENT_NAME = transaction.TDL_PATIENT_NAME;
                    this.NUM_ORDER = transaction.NUM_ORDER;
                    this.EINVOICE_NUM_ORDER = transaction.EINVOICE_NUM_ORDER;
                    if (treatment != null)
                    {
                        this.DEPARTMENT_NAME = treatment.END_DEPARTMENT_NAME;
                        //this.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                        this.TDL_HEIN_CARD_NUMBER = treatment.TDL_HEIN_CARD_NUMBER;
                        this.PATIENT_TYPE_CODE = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == treatment.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_CODE;
                        this.PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == treatment.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;
                    }
                    if (depositBillSub != null && depositBillSub.Count > 0)
                    {
                        this.IS_RP_OTHERDATE = depositBillSub.Exists(o => o.TRANSACTION_TIME < this.TRANSACTION_DATE && o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU);
                        
                    }
                    if (billSub != null && billSub.Count > 0)
                    {
                        this.BILL_AMOUNT = billSub.Sum(p => p.AMOUNT);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }



        public decimal BILL_AMOUNT { get; set; }
    }
}
