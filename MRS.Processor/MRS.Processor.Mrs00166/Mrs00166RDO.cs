using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00166
{
    public class Mrs00166RDO : HIS_TREATMENT
    {
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string TDL_HEIN_SERVICE_BHYT_CODE { get; set; }
        public string TDL_HEIN_SERVICE_BHYT_NAME { get; set; }
        public decimal AMOUNT { get; set; }
        public long SERVICE_ID { get; set; }
        public long TDL_SERVICE_TYPE_ID { get; set; }
        public long? TDL_HEIN_SERVICE_TYPE_ID { get; set; }
        public long PATIENT_TYPE_ID { get; set; }
        public long TREATMENT_ID { get; set; }
        public decimal PRICE { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public string END_DEPARTMENT_NAME { get; set; }
        public string IN_TIME_STR { get; set; }
        public string OUT_TIME_STR { get; set; }
        public string DOB_STR { get; set; }
        public long? TDL_INTRUCTION_TIME { get; set; }
        public string REQUEST_ROOM_NAME { get; set; }
        public string REQUEST_USERNAME { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }
        public string EXECUTE_USERNAME { get; set; }
        public string DIPLOMA { get; set; }
        public string TREATMENT_TYPE_NAME { get; set; }
        public string REQUEST_DEPARTMENT_NAME { set; get; }// khoa chỉ định
        public string REQUEST_DEPARTMENT_CODE { get; set; }
        public string EXECUTE_DEPARTMENT_NAME { get; set; }
        public Mrs00166RDO()
        {

        }

        public Mrs00166RDO(Mrs00166RDO r)
        {
            System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<Mrs00166RDO>();
            foreach (var item in pi)
            {
                item.SetValue(this, (item.GetValue(r)));
            }
        }



        public long? TDL_FINISH_TIME { get; set; }

        public string REQUEST_ROOM_CODE { get; set; }

        public string EXECUTE_ROOM_CODE { get; set; }

        public string REQUEST_LOGINNAME { get; set; }

        public string EXECUTE_LOGINNAME { get; set; }

        public string TREATMENT_TYPE_CODE { get; set; }

        public string EXE_DIPLOMA { get; set; }

        public long? EXECUTE_TIME { get; set; }

        public long REQUEST_ROOM_ID { get; set; }

        public long EXECUTE_ROOM_ID { get; set; }

        public long REQUEST_DEPARTMENT_ID { get; set; }

        public long EXECUTE_DEPARTMENT_ID { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public string SERVICE_TYPE_CODE { get; set; }
        public string PARENT_SERVICE_NAME { get; set; }
        public string PARENT_SERVICE_CODE { get; set; }

        public long? SS_OTHER_PAY_SOURCE_ID { get; set; }
        public string PTTT_GROUP_NAME { get; set; }
        public string PTTT_GROUP_CODE { get; set; }
        public string OTHER_PAY_SOURCE_CODE { get; set; }
        public string OTHER_PAY_SOURCE_NAME { get; set; }

        public long? BEGIN_TIME { get; set; }

        public long? END_TIME { get; set; }
    }
}
