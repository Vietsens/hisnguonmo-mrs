using MRS.Processor.Mrs00532;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using MRS.MANAGER.Config;

namespace MRS.Proccessor.Mrs00532
{
    public class Mrs00532RDO : V_HIS_TREATMENT_4
    {
        private List<long> HeinServiceTypeId_Medi = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM,
            IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM,
            IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL,
            IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT,
            IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM,
            IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM,
            IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL,
            IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT
        };
        private List<long> HeinServiceTypeId_Cls = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA,
            IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN,
            IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN
        };
        public long COUNT_TREATTING { get; set; }
        public string DATE { get; set; }
        public long TREATMENT_TYPE_ID { get; set; }
        public GROUP_PRICE Exam { get; set; }
        public GROUP_PRICE TreatIn { get; set; }
        public GROUP_PRICE TreatOut { get; set; }
        public GROUP_PRICE AllTreatmentType { get; set; }
        private List<HIS_SERE_SERV> listSereServSub;

        public Mrs00532RDO(V_HIS_TREATMENT_4 r, List<HIS_SERE_SERV> listHisSereServ, List<HIS_PATIENT_TYPE_ALTER> lastPatientTypeAlter)
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
            this.AllTreatmentType.COUNT_FEE_LOCK = 1;
            this.AllTreatmentType.SUM_TOTAL_PRICE = this.listSereServSub.Sum(o => o.VIR_TOTAL_PRICE ?? 0);
            this.AllTreatmentType.SUM_MEDI_PRICE = this.listSereServSub.Where(p => this.HeinServiceTypeId_Medi.Contains(p.TDL_HEIN_SERVICE_TYPE_ID ?? 0)).Sum(o => o.VIR_TOTAL_PRICE ?? 0);
            this.AllTreatmentType.SUM_CLS_PRICE = this.listSereServSub.Where(p => this.HeinServiceTypeId_Cls.Contains(p.TDL_HEIN_SERVICE_TYPE_ID ?? 0)).Sum(o => o.VIR_TOTAL_PRICE ?? 0);
        }

        public Mrs00532RDO()
        {

        }
    }
}
