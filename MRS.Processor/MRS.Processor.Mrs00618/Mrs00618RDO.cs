using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using HIS.Common.Treatment;

namespace MRS.Processor.Mrs00618
{
    public class Mrs00618RDO
    {

        public string ROUTE_TYPE_CODE { get; set; }
        public string ROUTE_TYPE_NAME { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public decimal AMOUNT { get; set; }
        public long DAY { get; set; }
        public decimal VIR_TOTAL_PRICE { get; set; }
        public decimal BED { get; set; }
        public decimal BED_AMOUNT { get; set; }
        public decimal MEDI { get; set; }
        public decimal TEST { get; set; }
        public decimal CDHA { get; set; }
        public decimal SERV { get; set; }
        public decimal TOTAL_PATIENT_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE { get; set; }
        public decimal TOTAL_PATIENT_PRICE_SELF { get; set; }
        public decimal TOTAL_PATIENT_PRICE_BHYT { get; set; }

        public Mrs00618RDO(HIS_TREATMENT t, List<HIS_SERE_SERV> ss,  HIS_BRANCH branch)
        {
            if (t.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
            {
                if (t.TDL_HEIN_MEDI_ORG_CODE == branch.HEIN_MEDI_ORG_CODE)
                {
                    this.ROUTE_TYPE_CODE = "00001";
                    this.ROUTE_TYPE_NAME = "BN BHYT DKKCBBĐ";
                }
                else
                {

                    this.ROUTE_TYPE_CODE = "00002";
                    this.ROUTE_TYPE_NAME = "BN BHYT Khác";
                }
            }
            else
            {

                this.ROUTE_TYPE_CODE = string.Format("{0:00000}",3+t.TDL_PATIENT_TYPE_ID);
                this.ROUTE_TYPE_NAME = string.Format("BN {0}", (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == t.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME);
            }

            if (t.LAST_DEPARTMENT_ID != null)
            {
                this.DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == t.LAST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                this.DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == t.LAST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
            }
            this.AMOUNT = 1;
            if (t.OUT_TIME > 0)
            {
                this.DAY = HIS.Common.Treatment.Calculation.DayOfTreatment(t.IN_TIME, t.OUT_TIME, t.TREATMENT_END_TYPE_ID, t.TREATMENT_RESULT_ID, t.TDL_HEIN_CARD_NUMBER != null ? PatientTypeEnum.TYPE.BHYT : PatientTypeEnum.TYPE.THU_PHI) ?? 0;
            }
            foreach (var item in ss)
            {
                this.VIR_TOTAL_PRICE += item.VIR_TOTAL_PRICE ?? 0;
                this.TOTAL_HEIN_PRICE += item.VIR_TOTAL_HEIN_PRICE ?? 0;
                this.TOTAL_PATIENT_PRICE += item.VIR_TOTAL_PATIENT_PRICE ?? 0;
                this.TOTAL_PATIENT_PRICE_BHYT += item.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                this.TOTAL_PATIENT_PRICE_SELF += (item.VIR_TOTAL_PATIENT_PRICE ?? 0) - (item.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G)
                {
                    this.BED += item.VIR_TOTAL_PRICE ?? 0;
                    this.BED_AMOUNT += item.AMOUNT;
                }
                else if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC || item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT)
                {
                    this.MEDI += item.VIR_TOTAL_PRICE ?? 0;
                }
                else if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN)
                {
                    this.TEST += item.VIR_TOTAL_PRICE ?? 0;
                }
                else if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA || item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA || item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS || item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN)
                {
                    this.CDHA += item.VIR_TOTAL_PRICE ?? 0;
                }
                else
                {
                    this.SERV += item.VIR_TOTAL_PRICE ?? 0;
                }
            }

        }

        public Mrs00618RDO()
        {
            // TODO: Complete member initialization
        }

    }
}
