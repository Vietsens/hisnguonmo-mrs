using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00125
{
    class Mrs00125RDO
    {
        public string VIR_PATIENT_NAME { get; set; }
        public string PATIENT_CODE { get; set; }
        public string DOB { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string ICD_NAME { get; set; }
        public string ICD_CODE { get; set; }
        public long? ICD_ID { get; set; }
        public long SERVICE_ID { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal PRICE { get; set; }
        public decimal? VIR_TOTAL_PATIENT_PRICE { get; set; }
        public decimal? VIR_TOTAL_HEIN_PRICE { get; set; }
        public decimal? VIR_PATIENT_PRICE { get; set; }
        public string VIR_ADDRESS { get; set; }
        public string SERVICE_CODE { get; set; }
        public V_HIS_SERE_SERV V_HIS_SERE_SERV { get; set; }
        public V_HIS_TREATMENT V_HIS_TREATMENT { get; set; }
        public V_HIS_SERVICE_REQ HIS_SERVICE_REQ { get; set; }

        public string REQUEST_ROOM_NAME { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }
        public string TRANSACTION_NUM_ORDER { get; set; }

        public Mrs00125RDO(V_HIS_SERE_SERV sereServs, V_HIS_TREATMENT treatment, List<V_HIS_SERVICE_REQ> serviceReqs, List<HIS_TRANSACTION> Transactions)
        {
            this.V_HIS_SERE_SERV = sereServs;
            this.V_HIS_TREATMENT = treatment;

            VIR_PATIENT_NAME = treatment.TDL_PATIENT_NAME;
            PATIENT_CODE = treatment.TDL_PATIENT_CODE;
            DOB = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.TDL_PATIENT_DOB);
            TREATMENT_CODE = sereServs.TDL_TREATMENT_CODE;
            SERVICE_ID = sereServs.SERVICE_ID;
            AMOUNT = sereServs.AMOUNT;
            PRICE = sereServs.PRICE;
            VIR_TOTAL_PATIENT_PRICE = sereServs.VIR_TOTAL_PATIENT_PRICE;
            VIR_TOTAL_HEIN_PRICE = sereServs.VIR_TOTAL_HEIN_PRICE;
            VIR_PATIENT_PRICE = sereServs.VIR_PATIENT_PRICE;
            VIR_ADDRESS = treatment.TDL_PATIENT_ADDRESS;
            SERVICE_CODE = string.Format("{0}-{1}", sereServs.TDL_SERVICE_CODE, sereServs.TDL_SERVICE_NAME);

            if (serviceReqs != null)
            {
                this.HIS_SERVICE_REQ = serviceReqs.Count > 0 ? serviceReqs.First() : new V_HIS_SERVICE_REQ();
                ICD_NAME = HIS_SERVICE_REQ.ICD_NAME;
                ICD_CODE = string.Format("{0}-{1}", HIS_SERVICE_REQ.ICD_CODE, ICD_NAME);
                REQUEST_ROOM_NAME = string.Join(",", serviceReqs.Select(s => s.REQUEST_ROOM_NAME).Distinct().ToList());
                EXECUTE_ROOM_NAME = string.Join(",", serviceReqs.Select(s => s.EXECUTE_ROOM_NAME).Distinct().ToList());
            }

            if (Transactions != null)
            {
                Transactions = Transactions.OrderBy(o => o.NUM_ORDER).ToList();
                TRANSACTION_NUM_ORDER = string.Join(",", Transactions.Select(s => s.NUM_ORDER).Distinct().ToList());
            }
        }
    }
}
