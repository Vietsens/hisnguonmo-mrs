using Inventec.Common.Repository;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00611
{

   public class Mrs00611RDO : V_HIS_VITAMIN_A
    {
        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; } 
        public string TREATMENT_CODE { get; set; } 
        public string BIRTH_DAY { get; set; } 
        public long DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string ROOM_CODE { get; set; }
        public string ROOM_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string FINISH_TIME { get; set; }
        public string DOB_STR { get; set; }
        public string GENDER_STR { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public long BORN_TIME { get; set; }
        public string RELATIVE_NAME { get; set; }
        public string RELATIVE_ADDRESS { get; set; }
        public string RELATIVE_CMND_NUMBER { get; set; }
        public string RELATIVE_MOBILE { get; set; }
        public string RELATIVE_PHONE { get; set; }
        public double? DESCRIPTION { get; set; }

        public Mrs00611RDO(V_HIS_VITAMIN_A data) 
        {
            try
            {
                PropertyInfo[] pi = Properties.Get<V_HIS_VITAMIN_A>();
                foreach (var item in pi)
                {
                    item.SetValue(this, item.GetValue(data));
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public Mrs00611RDO() { }
    }
}
