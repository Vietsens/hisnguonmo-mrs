using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00623
{
    public class Mrs00623RDO
    {
        public Mrs00623RDO()
        { }
        public Mrs00623RDO(HIS_SERE_SERV r, HIS_SERVICE_REQ serviceReq,HIS_SERVICE service)
        {
            this.PATIENT_TYPE_ID = r.PATIENT_TYPE_ID;
            this.VIR_PRICE = r.VIR_PRICE??0;
            this.SERVICE_ID = r.SERVICE_ID;
            this.SERVICE_NAME = r.TDL_SERVICE_NAME;
            if (r.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
            {
                this.AMOUNT_BHYT = NumberOfFilm(service) ?? r.AMOUNT;
                this.PRICE_BHYT = r.VIR_PRICE ?? 0;
                this.VIR_TOTAL_PRICE_BHYT = r.VIR_TOTAL_PRICE ?? 0;
            }
            else if (r.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
            {
                this.AMOUNT_TP = NumberOfFilm(service) ?? r.AMOUNT;
                this.PRICE_TP = r.VIR_PRICE ?? 0;
                this.VIR_TOTAL_PRICE_TP = r.VIR_TOTAL_PRICE ?? 0;
            }
            else if (r.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__IS_FREE)
            {
                this.AMOUNT_MP = NumberOfFilm(service) ?? r.AMOUNT;
                this.PRICE_MP = r.VIR_PRICE ?? 0;
                this.VIR_TOTAL_PRICE_MP = r.VIR_TOTAL_PRICE ?? 0;
            }
            else
            {
                this.AMOUNT_OTHER = NumberOfFilm(service) ?? r.AMOUNT;
            }
            if (Age(serviceReq.INTRUCTION_TIME, serviceReq.TDL_PATIENT_DOB) < 6)
            {
                this.AMOUNT_TE = NumberOfFilm(service) ?? r.AMOUNT;
            }
            this.AMOUNT = NumberOfFilm(service) ?? r.AMOUNT;
            this.VIR_TOTAL_PRICE = r.VIR_TOTAL_PRICE ?? 0;
        }
        public long PATIENT_TYPE_ID { get; set; }
        public decimal VIR_PRICE { get; set; }
        public long SERVICE_ID { get; set; }
        public string SERVICE_NAME { get; set; }
        public decimal PRICE_BHYT { get; set; }
        public decimal PRICE_TP { get; set; }
        public decimal PRICE_MP { get; set; }
        public decimal AMOUNT_BHYT { get; set; }
        public decimal AMOUNT_TP { get; set; }
        public decimal AMOUNT_MP { get; set; }
        public decimal AMOUNT_OTHER { get; set; }
        public decimal AMOUNT_TE { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal VIR_TOTAL_PRICE_BHYT { get; set; }
        public decimal VIR_TOTAL_PRICE_TP { get; set; }
        public decimal VIR_TOTAL_PRICE_MP { get; set; }
        public decimal VIR_TOTAL_PRICE { get; set; }

        private int Age(long INTRUCTION_TIME, long TDL_PATIENT_DOB)
        {
            return (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(INTRUCTION_TIME) ?? DateTime.Now).Year - (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(TDL_PATIENT_DOB) ?? DateTime.Now).Year;
        }

        private decimal? NumberOfFilm(HIS_SERVICE service)
        {
            if (service.NUMBER_OF_FILM > 0)
            {
                return service.NUMBER_OF_FILM;
            }
            else
            {
                return null;
            }
        }

    }
}
