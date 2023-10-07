using LIS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00659
{
    class Mrs00659RDO : LIS_SAMPLE
    {
        public string SERVICE_NAME { get; set; }

        public string SAMPLE_TIME_STR { get; set; }//lay mau
        public string APPROVAL_TIME_STR { get; set; }//nhan mau
        public string APPOINTMENT_TIME_STR { get; set; }//tra ket qua
        public string RESULT_TIME_STR { get; set; }//nhan ket qua
        public string RESULT_USERNAME { get; set; }
        public string DISAPPROVAL_TIME_STR { get; set; }//tu choi APPROVAL_TIME
        public string DISAPPROVAL_USERNAME { get; set; }
        public string DISAPPROVAL_LOGINNAME { get; set; }
        public string CANCEL_TIME_STR { get; set; }//du kien huy mau intruction_time + 7
        public string TDL_PATIENT_NAME { get; set; }
        public string TDL_PATIENT_GENDER_NAME { get; set; }

        public Mrs00659RDO(LIS_SAMPLE data, bool sameDay)
        {
            if (data != null)
            {
                Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00659RDO>(this, data);

                if (data.SAMPLE_TIME.HasValue)
                {
                    this.SAMPLE_TIME_STR = ProcessTime(data.SAMPLE_TIME.Value, sameDay);
                }

                if (data.APPROVAL_TIME.HasValue)
                {
                    if (!String.IsNullOrWhiteSpace(data.REJECT_REASON))
                    {
                        this.APPROVAL_LOGINNAME = "";
                        this.APPROVAL_USERNAME = "";
                        this.DISAPPROVAL_TIME_STR = ProcessTime(data.APPROVAL_TIME.Value, sameDay);
                        this.DISAPPROVAL_LOGINNAME = data.APPROVAL_LOGINNAME;
                        this.DISAPPROVAL_USERNAME = data.APPROVAL_USERNAME;
                    }
                    else
                    {
                        this.APPROVAL_TIME_STR = ProcessTime(data.APPROVAL_TIME.Value, sameDay);
                    }
                }

                if (data.APPOINTMENT_TIME.HasValue)
                {
                    this.APPOINTMENT_TIME_STR = ProcessTime(data.APPOINTMENT_TIME.Value, sameDay);
                }

                if (data.RESULT_TIME.HasValue)
                {
                    this.RESULT_TIME_STR = ProcessTime(data.RESULT_TIME.Value, sameDay);
                }
            }
        }

        private string ProcessTime(long time, bool sameDay)
        {
            string result = "";
            try
            {
                if (time > 0)
                {
                    if (sameDay)
                    {
                        string temp = time.ToString();
                        result = new StringBuilder().Append(temp.Substring(8, 2)).Append(":").Append(temp.Substring(10, 2)).Append(":").Append(temp.Substring(12, 2)).ToString();
                    }
                    else
                    {
                        result = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(time);
                    }
                }
            }
            catch (Exception ex)
            {
                result = "";
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}
