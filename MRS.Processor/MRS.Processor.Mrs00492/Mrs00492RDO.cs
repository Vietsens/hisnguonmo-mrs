using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using HTC.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00492
{
    public class Mrs00492RDO : HTC_REVENUE
    {

        public Mrs00492RDO(HTC_REVENUE r)
        {
            System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<HTC_REVENUE>();
            foreach (var item in pi)
            {
                item.SetValue(this, (item.GetValue(r)));
            }
        }	
        public  string IN_TIME_STR { get; set; }
        public  string OUT_TIME_STR { get; set; }	
        public  decimal AMOUNT_1 { get; set; }	
        public  decimal AMOUNT_2 { get; set; }	
        public  decimal AMOUNT_3 { get; set; }	
        public  decimal AMOUNT_4 { get; set; }	
        public  decimal AMOUNT_5 { get; set; }	
        public  decimal AMOUNT_6 { get; set; }	
        public  decimal AMOUNT_7 { get; set; }	
        public  decimal AMOUNT_8 { get; set; }	
        public  decimal AMOUNT_9 { get; set; }	
        public  decimal AMOUNT_10 { get; set; }	
        public  decimal AMOUNT_11 { get; set; }	
        public  decimal AMOUNT_12 { get; set; }	
        public  decimal AMOUNT_13 { get; set; }
        public  decimal AMOUNT_14 { get; set; }	
        public  decimal AMOUNT_15 { get; set; }	
        public  decimal AMOUNT_16 { get; set; }
        public  decimal AMOUNT_17 { get; set; }	
        public  decimal AMOUNT_18 { get; set; }	
        public  decimal AMOUNT_19 { get; set; }	
        public  decimal AMOUNT_20 { get; set; }
        public  decimal AMOUNT_21 { get; set; }	
        public  decimal AMOUNT_22 { get; set; }	
        public  decimal AMOUNT_23 { get; set; }	
        public  decimal AMOUNT_24 { get; set; }	
        public  decimal AMOUNT_25 { get; set; }	
        public  decimal AMOUNT_26 { get; set; }	
        public  decimal AMOUNT_27 { get; set; }	
        public  decimal AMOUNT_28 { get; set; }	
        public  decimal AMOUNT_29 { get; set; }	
        public  decimal AMOUNT_30 { get; set; }	
        public  decimal AMOUNT_31 { get; set; }	
        public  decimal AMOUNT_32 { get; set; }	
        public  decimal AMOUNT_33 { get; set; }	
        public  decimal AMOUNT_34 { get; set; }	
        public  decimal AMOUNT_35 { get; set; }	
        public  decimal AMOUNT_36 { get; set; }	
        public  decimal AMOUNT_37 { get; set; }	
        public  decimal AMOUNT_38 { get; set; }	
        public  decimal AMOUNT_39 { get; set; }
    }
}
