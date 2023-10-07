using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using Inventec.Common.Repository;
using MRS.MANAGER.Core.MrsReport.RDO;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00348
{
    public class Mrs00348RDO : V_HIS_TREATMENT_4
    {
        public string IS_BHYT { get; set; }// có phải là bhyt 

        public int? AGE_STR { get; set; }

        public string INTRUCTION_TIME { get; set; }
        public string LAST_DEPARTMENT_NAME { get; set; }
        public string EXECUTE_ROOM { get; set; }

        public Mrs00348RDO(V_HIS_TREATMENT_4 data)
        {
            PropertyInfo[] pi = Properties.Get<V_HIS_TREATMENT_4>();
            foreach (var item in pi)
            {
                item.SetValue(this, item.GetValue(data));
            }
            SetExtendField(data);
        }

        private void SetExtendField(V_HIS_TREATMENT_4 data)
        {
            try
            {
                int? tuoi = RDOCommon.CalculateAge(data.TDL_PATIENT_DOB);
                if (tuoi >= 0)
                {
                    this.AGE_STR = (tuoi >= 1) ? tuoi : 1;
                }
                this.LAST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == data.LAST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public Mrs00348RDO() { }
    }
}
