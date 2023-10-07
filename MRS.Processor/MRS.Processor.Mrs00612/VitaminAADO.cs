using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00612
{
    class VitaminAADO : HIS_VITAMIN_A
    {
        public long? MONTH_OLD { get; set; }

        public VitaminAADO(HIS_VITAMIN_A data)
        {
            try
            {
                this.BRANCH_ID = data.BRANCH_ID;
                this.CASE_TYPE = data.CASE_TYPE;
                this.EXECUTE_TIME = data.EXECUTE_TIME;
                this.IS_ONE_MONTH_BORN = data.IS_ONE_MONTH_BORN;
                this.IS_SICK = data.IS_SICK;

                DateTime? inTime = data.EXECUTE_TIME.HasValue ? Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(data.EXECUTE_TIME.Value) : null;
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
