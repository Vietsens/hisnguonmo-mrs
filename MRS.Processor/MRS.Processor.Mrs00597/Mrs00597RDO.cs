using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using Inventec.Common.Repository;

namespace MRS.Processor.Mrs00597
{

    public class Mrs00597RDO:V_HIS_DEPARTMENT_TRAN
    {
        public HIS_TREATMENT HIS_TREATMENT { get; set; }
        public int? AGE_MALE { get; set; }//
        public int? AGE_FEMALE { get; set; }//
        public string DEPARTMENT_IN_TIME_STR { get; set; }//
        public string HEIN_CARD_NUMBER { get; set; }
        public string IS_BHYT { get; set; }
        public string FROM_DEPARTMENT_CODE { get; set; }
        public string FROM_DEPARTMENT_NAME { get; set; }
        public string TOTAL_DAYS_IN_TREATMENT { get; set; }
        public string IN_DATE { get; set; }
        public Mrs00597RDO(V_HIS_DEPARTMENT_TRAN r,HIS_TREATMENT treatment)
        {
            this.HIS_TREATMENT = treatment??new HIS_TREATMENT();
            PropertyInfo[] p = Properties.Get<V_HIS_DEPARTMENT_TRAN>();
            foreach (var item in p)
            {
                item.SetValue(this, item.GetValue(r));
            }
            SetExtensionField(this);
        }

        private void SetExtensionField(Mrs00597RDO r)
        {
            this.HEIN_CARD_NUMBER = this.HIS_TREATMENT.TDL_HEIN_CARD_NUMBER;
            this.DEPARTMENT_IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(this.DEPARTMENT_IN_TIME ?? 0);
            CalcuatorAge(HIS_TREATMENT);
        }
        private void CalcuatorAge(HIS_TREATMENT r)
        {
            try
            {
                int? tuoi = MRS.MANAGER.Core.MrsReport.RDO.RDOCommon.CalculateAge(r.TDL_PATIENT_DOB);
                if (tuoi >= 0)
                {
                    if (r.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        this.AGE_MALE = (tuoi >= 1) ? tuoi : 1;
                    }
                    else
                    {
                        this.AGE_FEMALE = (tuoi >= 1) ? tuoi : 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
