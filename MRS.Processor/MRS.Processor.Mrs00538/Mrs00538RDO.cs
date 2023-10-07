using MRS.Processor.Mrs00538;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using MRS.MANAGER.Config;

namespace MRS.Proccessor.Mrs00538
{
    public class Mrs00538RDO : V_HIS_TREATMENT_4
    {
       
        public long COUNT_TREATTING { get; set; }// Số BN đang điều trị
        public string DATE { get; set; } //Ngày 
        public long TREATMENT_TYPE_ID { get; set; } // Diện điều trị

        public GROUP_PRICE TreatIn { get; set; }
        public GROUP_PRICE TreatOut { get; set; }

        public GROUP_PRICE AllTreatmentType { get; set; }

        private List<HIS_SERE_SERV> listSereServSub;

        public Mrs00538RDO(V_HIS_TREATMENT_4 r, List<HIS_SERE_SERV> listHisSereServ, List<HIS_PATIENT_TYPE_ALTER> lastPatientTypeAlter)
        {

            this.listSereServSub = listHisSereServ.Where(o => o.TDL_TREATMENT_ID == r.ID).ToList();
            this.TREATMENT_TYPE_ID = (lastPatientTypeAlter.FirstOrDefault(o => o.TREATMENT_ID == r.ID) ?? new HIS_PATIENT_TYPE_ALTER()).TREATMENT_TYPE_ID;
            System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<MOS.EFMODEL.DataModels.V_HIS_TREATMENT_4>();
            foreach (var item in pi)
            {
                item.SetValue(this, (item.GetValue(r)));
            }

            this.ChargeKey();
        }

        private void ChargeKey()
        {
            this.AllTreatmentType = new GROUP_PRICE();
            this.AllTreatmentType.COUNT_OUT = 1;
            this.AllTreatmentType.SUM_TOTAL_PRICE = this.listSereServSub.Sum(o => o.VIR_TOTAL_PRICE ?? 0);
            this.AllTreatmentType.SUM_TOTAL_HEIN_PRICE = this.listSereServSub.Sum(o => o.VIR_TOTAL_HEIN_PRICE ?? 0);
        }

        public Mrs00538RDO()
        {

        }
    }
}
