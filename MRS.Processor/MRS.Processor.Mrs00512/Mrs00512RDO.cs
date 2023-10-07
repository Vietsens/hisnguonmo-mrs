using MRS.Processor.Mrs00512;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using MRS.MANAGER.Config;

namespace MRS.Proccessor.Mrs00512
{
    public class Mrs00512RDO : V_HIS_SERVICE_RETY_CAT
    {
        public decimal AMOUNT_REQUEST_BHYT { get; set; }
        public decimal AMOUNT_REQUEST_ND { get; set; }
        public decimal AMOUNT_REQUEST_SUM { get; set; }

        public decimal AMOUNT_FINISH_BHYT { get; set; }
        public decimal AMOUNT_FINISH_ND { get; set; }
        public decimal AMOUNT_FINISH_SUM { get; set; }

        public decimal AMOUNT_DIFF_BHYT { get; set; }
        public decimal AMOUNT_DIFF_ND { get; set; }
        public decimal AMOUNT_DIFF_SUM { get; set; }

        public Mrs00512RDO(V_HIS_SERVICE_RETY_CAT r, List<HIS_SERE_SERV> listSereServ, List<HIS_SERVICE_REQ> listServiceReq)
        {
            PropertyInfo[] p = typeof(V_HIS_SERVICE_RETY_CAT).GetProperties();
            foreach (var item in p)
            {
                item.SetValue(this, item.GetValue(r));
            }
            SetExtendField(this, listSereServ, listServiceReq);

        }
        private void SetExtendField(Mrs00512RDO data, List<HIS_SERE_SERV> listSereServ, List<HIS_SERVICE_REQ> listServiceReq)
        {
            this.AMOUNT_REQUEST_BHYT = listSereServ.Where(o => o.SERVICE_ID == data.SERVICE_ID&&o.PATIENT_TYPE_ID==HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(p => p.AMOUNT);
            this.AMOUNT_REQUEST_ND = listSereServ.Where(o => o.SERVICE_ID == data.SERVICE_ID && o.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(p => p.AMOUNT);
            this.AMOUNT_REQUEST_SUM = listSereServ.Where(o => o.SERVICE_ID == data.SERVICE_ID).Sum(p => p.AMOUNT);

            this.AMOUNT_FINISH_BHYT = listSereServ.Where(o => o.SERVICE_ID == data.SERVICE_ID && o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && listServiceReq.Where(p=>p.FINISH_TIME!=null&&p.ID==o.SERVICE_REQ_ID).ToList().Count>0).Sum(p => p.AMOUNT);
            this.AMOUNT_FINISH_ND = listSereServ.Where(o => o.SERVICE_ID == data.SERVICE_ID && o.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && listServiceReq.Where(p => p.FINISH_TIME != null && p.ID == o.SERVICE_REQ_ID).ToList().Count > 0).Sum(p => p.AMOUNT);
            this.AMOUNT_FINISH_SUM = listSereServ.Where(o => o.SERVICE_ID == data.SERVICE_ID && listServiceReq.Where(p => p.FINISH_TIME != null && p.ID == o.SERVICE_REQ_ID).ToList().Count > 0).Sum(p => p.AMOUNT);

            this.AMOUNT_DIFF_BHYT = listSereServ.Where(o => o.SERVICE_ID == data.SERVICE_ID && o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && listServiceReq.Where(p => p.FINISH_TIME == null && p.ID == o.SERVICE_REQ_ID).ToList().Count > 0).Sum(p => p.AMOUNT);
            this.AMOUNT_DIFF_ND = listSereServ.Where(o => o.SERVICE_ID == data.SERVICE_ID && o.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && listServiceReq.Where(p => p.FINISH_TIME == null && p.ID == o.SERVICE_REQ_ID).ToList().Count > 0).Sum(p => p.AMOUNT);
            this.AMOUNT_DIFF_SUM = listSereServ.Where(o => o.SERVICE_ID == data.SERVICE_ID && listServiceReq.Where(p => p.FINISH_TIME == null && p.ID == o.SERVICE_REQ_ID).ToList().Count > 0).Sum(p => p.AMOUNT);
        }
        public Mrs00512RDO()
        {

        }
    }
}
