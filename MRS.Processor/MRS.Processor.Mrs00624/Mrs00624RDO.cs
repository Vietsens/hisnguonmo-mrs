using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00624
{
    internal class Mrs00624RDO : V_HIS_TRANSACTION
    {
        public decimal BILL_AMOUNT { get; set; }
        public decimal DEPOSIT_AMOUNT { get; set; }
        public decimal REPAY_AMOUNT { get; set; }
        public string TRANSACTION_TIME_STR { get; set; }
        public string CANCEL_TIME_STR { get; set; }
        public string CANCEL_DEPARTMENT_NAME { get; set; }
        public string SAMPLE_DAY { get; set; }
        public string TDL_HEIN_CARD_NUMBER { get; set; }

        internal Mrs00624RDO(Mrs00624RDO tran, List<HIS_DEPARTMENT_TRAN> listHisDepartmentTran, Mrs00624Filter filter)
        {
            Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00624RDO>(this, tran);
            var cancelDepartment = listHisDepartmentTran.OrderByDescending(o => o.DEPARTMENT_IN_TIME ?? 0).FirstOrDefault(p => p.TREATMENT_ID == tran.TREATMENT_ID && p.DEPARTMENT_IN_TIME <= tran.CANCEL_TIME);
            if (cancelDepartment != null)
            {
                this.CANCEL_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o=>o.ID==cancelDepartment.DEPARTMENT_ID)??new HIS_DEPARTMENT()).DEPARTMENT_NAME;
            }
            if (tran.TRANSACTION_TIME < (filter.TIME_FROM??0))
            {
                this.SAMPLE_DAY = "";
            }
            else
            {
                this.SAMPLE_DAY = "x";
            }

            if (tran.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT)
            {
                this.BILL_AMOUNT += tran.AMOUNT;
            }
            else if (tran.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU)
            {
                this.DEPOSIT_AMOUNT += tran.AMOUNT;
            }
            else if (tran.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU)
            {
                this.REPAY_AMOUNT += tran.AMOUNT;
            }

            this.TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(tran.TRANSACTION_TIME);
            this.CANCEL_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(tran.CANCEL_TIME ?? 0);
        }
    }
}
