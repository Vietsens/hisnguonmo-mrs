using MRS.Processor.Mrs00579;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using MRS.MANAGER.Config;

namespace MRS.Proccessor.Mrs00579
{
    public class Mrs00579RDO
    {
        public long ID { get; set; }
        public long? INTRUCTION_TIME { get; set; }	
        public string INTRUCTION_TIME_STR { get; set; }	
        public string TREATMENT_CODE { get; set; }	
        public string PATIENT_NAME { get; set; }	
        public long? DOB { get; set; }	
        public string DOB_STR { get; set; }	
        public string GENDER_NAME { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }	
        public string ADDRESS { get; set; }	
        public string ICD_NAME { get; set; }	
        public string REQUEST_ROOM_NAME { get; set; }	
        public string SERVICE_NAME { get; set; }	
        public Decimal? AMOUNT { get; set; }
        public Decimal? PRICE { get; set; }
        public Decimal? TOTAL_PRICE { get; set; }
        public long? VACCINE_ID { get; set; }// id vắcxin ( thuốc)
        public long? VACINATION_ORDER { set; get; }// số lần tiêm
        public string SERVICE_UNIT_NAME { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public string NATIONAL_NAME { get; set; }
        
    }
}
