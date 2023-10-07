using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00648
{
    class Mrs00648RDO : V_HIS_TREATMENT
    {
        public string IS_BHYT { get; set; }
        public string IS_TE { get; set; }
        public string IS_CV { get; set; }
        public string IS_NV { get; set; }
        public HIS_ICD ICD { get; set; }

        public string DEPARTMENT_IN_CODE { get; set; }
        public long DEPARTMENT_IN_ID { get; set; }
        public string DEPARTMENT_IN_NAME { get; set; }

        public Mrs00648RDO(V_HIS_TREATMENT data)
        {
            try
            {
                if (data != null)
                {
                    Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00648RDO>(this, data);

                    if (data.CLINICAL_IN_TIME.HasValue) IS_NV = "X";

                    if (data.IS_ACTIVE == 0 && data.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN) IS_CV = "X";

                    if (data.TDL_PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT) IS_BHYT = "X";

                    this.ICD = new HIS_ICD();

                    try
                    {
                        var inYear = data.IN_TIME.ToString().Substring(0, 4);
                        var dobYear = data.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                        int tuoi = int.Parse(inYear) - int.Parse(dobYear);

                        if (tuoi < 15) IS_TE = "X";
                    }
                    catch (Exception)
                    {
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
