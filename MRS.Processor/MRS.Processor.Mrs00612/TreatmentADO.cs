using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00612
{
    class TreatmentADO : HIS_TREATMENT
    {
        public long? MONTH_OLD { get; set; }

        public TreatmentADO(HIS_TREATMENT data)
        {
            try
            {
                this.ID = data.ID;
                this.BRANCH_ID = data.BRANCH_ID;

                DateTime? inTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(data.IN_TIME);
                DateTime? dob = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(data.TDL_PATIENT_DOB);

                if (inTime.HasValue && dob.HasValue)
                {
                    var diff = new DateDifference(inTime.Value, dob.Value);
                    diff.DateDiff();
                    MONTH_OLD = diff.Years * 12 + diff.Months;
                    if ((diff.Weeks * 7 + diff.Days) > 0)
                    {
                        MONTH_OLD += 1;
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
