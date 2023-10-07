using HIS.Common.Treatment;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00621
{
    class Mrs00621RDO : V_HIS_TREATMENT
    {
        public long SO_NGAY_DT { get; set; }
        public string IS_FEMALE { get; set; }
        public string IS_BHYT { get; set; }

        public Mrs00621RDO(V_HIS_TREATMENT data)
        {
            try
            {
                if (data != null)
                {
                    Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00621RDO>(this, data);

                    SO_NGAY_DT = HIS.Common.Treatment.Calculation.DayOfTreatment(data.IN_TIME, data.OUT_TIME, data.TREATMENT_END_TYPE_ID, data.TREATMENT_RESULT_ID, data.TDL_HEIN_CARD_NUMBER != null ? PatientTypeEnum.TYPE.BHYT : PatientTypeEnum.TYPE.THU_PHI) ?? 0;

                    if (data.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                    {
                        IS_FEMALE = "x";
                    }

                    if (data.TDL_HEIN_CARD_NUMBER != null)
                    {
                        IS_BHYT = "x";
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
