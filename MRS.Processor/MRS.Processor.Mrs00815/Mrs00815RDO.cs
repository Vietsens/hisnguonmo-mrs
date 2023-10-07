using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00815
{
    public class Mrs00815RDO
    {
        public long MATE_MEDI_BEAN_ID { get; set; }
        public long MATE_MEDI_TYPE_ID { get; set; }
        public string MATE_MEDI_TYPE_CODE { get; set; }
        public string MATE_MEDI_TYPE_NAME { get; set; }
        public string ACTIVE_INGR_BHYT_NAME { get; set; }
        public string ACTIVE_INGR_BHYT_CODE{set;get;}
        public string SERVICE_UNIT_NAME { get; set; }
        public decimal? IMP_PRICE { get; set; }
        public string PACKAGE_NUMBER { get; set; }
        public string EXPIRED_DATE_STR { get; set; }
        public string MEDI_STOCK_AMOUNT { get; set; }
        public decimal TOTAL_AMOUNT { get; set; }
        public long? EXPIRED_DATE { get; set; }
        public long? MEDI_STOCK_ID { get; set; }
        public string MEDI_STOCK_NAME { get; set; }
        public long TYPE { set; get; }//1: thuốc, 2:vật tư,3 hóa chất
        public string TYPE_NAME { set; get; }
        public long? NUM_ORDER { get; set; }
        public string MATE_MEDI_PARENT_NAME { get; set; }

        public decimal A_MEDI_STOCK_1 { get; set; }//C_ROOM: số lượng thuốc theo khoa của tủ trực
        public decimal A_MEDI_STOCK_2 { get; set; }
        public decimal A_MEDI_STOCK_3 { get; set; }
        public decimal A_MEDI_STOCK_4 { get; set; }
        public decimal A_MEDI_STOCK_5 { get; set; }
        public decimal A_MEDI_STOCK_6 { get; set; }
        public decimal A_MEDI_STOCK_7 { get; set; }
        public decimal A_MEDI_STOCK_8 { get; set; }
        public decimal A_MEDI_STOCK_9 { get; set; }
        public decimal A_MEDI_STOCK_10 { get; set; }
        public decimal A_MEDI_STOCK_11 { get; set; }
        public decimal A_MEDI_STOCK_12 { get; set; }
        public decimal A_MEDI_STOCK_13 { get; set; }
        public decimal A_MEDI_STOCK_14 { get; set; }
        public decimal A_MEDI_STOCK_15 { get; set; }
        public decimal A_MEDI_STOCK_16 { get; set; }
        public decimal A_MEDI_STOCK_17 { get; set; }
        public decimal A_MEDI_STOCK_18 { get; set; }
        public decimal A_MEDI_STOCK_19 { get; set; }
        public decimal A_MEDI_STOCK_20 { get; set; }
        public decimal A_MEDI_STOCK_21 { get; set; }
        public decimal A_MEDI_STOCK_22 { get; set; }
        public decimal A_MEDI_STOCK_23 { get; set; }
        public decimal A_MEDI_STOCK_24 { get; set; }
        public decimal A_MEDI_STOCK_25 { get; set; }
        public decimal A_MEDI_STOCK_26 { get; set; }
        public decimal A_MEDI_STOCK_27 { get; set; }
        public decimal A_MEDI_STOCK_28 { get; set; }
        public decimal A_MEDI_STOCK_29 { get; set; }
        public decimal A_MEDI_STOCK_30 { get; set; }
        public decimal A_MEDI_STOCK_31 { get; set; }
        public decimal A_MEDI_STOCK_32 { get; set; }
        public decimal A_MEDI_STOCK_33 { get; set; }
        public decimal A_MEDI_STOCK_34 { get; set; }
        public decimal A_MEDI_STOCK_35 { get; set; }
        public decimal A_MEDI_STOCK_36 { get; set; }
        public decimal A_MEDI_STOCK_37 { get; set; }
        public decimal A_MEDI_STOCK_38 { get; set; }
        public decimal A_MEDI_STOCK_39 { get; set; }
        public decimal A_MEDI_STOCK_40 { get; set; }
        public decimal A_MEDI_STOCK_41 { get; set; }
        public decimal A_MEDI_STOCK_42 { get; set; }
        public decimal A_MEDI_STOCK_43 { get; set; }
        public decimal A_MEDI_STOCK_44 { get; set; }
        public decimal A_MEDI_STOCK_45 { get; set; }
        public decimal A_MEDI_STOCK_46 { get; set; }
        public decimal A_MEDI_STOCK_47 { get; set; }
        public decimal A_MEDI_STOCK_48 { get; set; }
        public decimal A_MEDI_STOCK_49 { get; set; }
        public decimal A_MEDI_STOCK_50 { get; set; }
    

       
    }
    public class Title
    {
        public string MEDI_STOCK_NAME_1 { get; set; }
        public string MEDI_STOCK_NAME_2 { get; set; }
        public string MEDI_STOCK_NAME_3 { get; set; }
        public string MEDI_STOCK_NAME_4 { get; set; }
        public string MEDI_STOCK_NAME_5 { get; set; }
        public string MEDI_STOCK_NAME_6 { get; set; }
        public string MEDI_STOCK_NAME_7 { get; set; }
        public string MEDI_STOCK_NAME_8 { get; set; }
        public string MEDI_STOCK_NAME_9 { get; set; }
        public string MEDI_STOCK_NAME_10 { get; set; }
        public string MEDI_STOCK_NAME_11 { get; set; }
        public string MEDI_STOCK_NAME_12 { get; set; }
        public string MEDI_STOCK_NAME_13 { get; set; }
        public string MEDI_STOCK_NAME_14 { get; set; }
        public string MEDI_STOCK_NAME_15 { get; set; }
        public string MEDI_STOCK_NAME_16 { get; set; }
        public string MEDI_STOCK_NAME_17 { get; set; }
        public string MEDI_STOCK_NAME_18 { get; set; }
        public string MEDI_STOCK_NAME_19 { get; set; }
        public string MEDI_STOCK_NAME_20 { get; set; }
        public string MEDI_STOCK_NAME_21 { get; set; }
        public string MEDI_STOCK_NAME_22 { get; set; }
        public string MEDI_STOCK_NAME_23 { get; set; }
        public string MEDI_STOCK_NAME_24 { get; set; }
        public string MEDI_STOCK_NAME_25 { get; set; }
        public string MEDI_STOCK_NAME_26 { get; set; }
        public string MEDI_STOCK_NAME_27 { get; set; }
        public string MEDI_STOCK_NAME_28 { get; set; }
        public string MEDI_STOCK_NAME_29 { get; set; }
        public string MEDI_STOCK_NAME_30 { get; set; }
        public string MEDI_STOCK_NAME_31 { get; set; }
        public string MEDI_STOCK_NAME_32 { get; set; }
        public string MEDI_STOCK_NAME_33 { get; set; }
        public string MEDI_STOCK_NAME_34 { get; set; }
        public string MEDI_STOCK_NAME_35 { get; set; }
        public string MEDI_STOCK_NAME_36 { get; set; }
        public string MEDI_STOCK_NAME_37 { get; set; }
        public string MEDI_STOCK_NAME_38 { get; set; }
        public string MEDI_STOCK_NAME_39 { get; set; }
        public string MEDI_STOCK_NAME_40 { get; set; }
        public string MEDI_STOCK_NAME_41 { get; set; }
        public string MEDI_STOCK_NAME_42 { get; set; }
        public string MEDI_STOCK_NAME_43 { get; set; }
        public string MEDI_STOCK_NAME_44 { get; set; }
        public string MEDI_STOCK_NAME_45 { get; set; }
        public string MEDI_STOCK_NAME_46 { get; set; }
        public string MEDI_STOCK_NAME_47 { get; set; }
        public string MEDI_STOCK_NAME_48 { get; set; }
        public string MEDI_STOCK_NAME_49 { get; set; }
        public string MEDI_STOCK_NAME_50 { get; set; }
    }
    public class AMOUNT {
        public decimal A_MEDI_STOCK_1 { get; set; }//C_ROOM: số lượng thuốc theo khoa của tủ trực
        public decimal A_MEDI_STOCK_2 { get; set; }
        public decimal A_MEDI_STOCK_3 { get; set; }
        public decimal A_MEDI_STOCK_4 { get; set; }
        public decimal A_MEDI_STOCK_5 { get; set; }
        public decimal A_MEDI_STOCK_6 { get; set; }
        public decimal A_MEDI_STOCK_7 { get; set; }
        public decimal A_MEDI_STOCK_8 { get; set; }
        public decimal A_MEDI_STOCK_9 { get; set; }
        public decimal A_MEDI_STOCK_10 { get; set; }
        public decimal A_MEDI_STOCK_11 { get; set; }
        public decimal A_MEDI_STOCK_12 { get; set; }
        public decimal A_MEDI_STOCK_13 { get; set; }
        public decimal A_MEDI_STOCK_14 { get; set; }
        public decimal A_MEDI_STOCK_15 { get; set; }
        public decimal A_MEDI_STOCK_16 { get; set; }
        public decimal A_MEDI_STOCK_17 { get; set; }
        public decimal A_MEDI_STOCK_18 { get; set; }
        public decimal A_MEDI_STOCK_19 { get; set; }
        public decimal A_MEDI_STOCK_20 { get; set; }
        public decimal A_MEDI_STOCK_21 { get; set; }
        public decimal A_MEDI_STOCK_22 { get; set; }
        public decimal A_MEDI_STOCK_23 { get; set; }
        public decimal A_MEDI_STOCK_24 { get; set; }
        public decimal A_MEDI_STOCK_25 { get; set; }
        public decimal A_MEDI_STOCK_26 { get; set; }
        public decimal A_MEDI_STOCK_27 { get; set; }
        public decimal A_MEDI_STOCK_28 { get; set; }
        public decimal A_MEDI_STOCK_29 { get; set; }
        public decimal A_MEDI_STOCK_30 { get; set; }
        public decimal A_MEDI_STOCK_31 { get; set; }
        public decimal A_MEDI_STOCK_32 { get; set; }
        public decimal A_MEDI_STOCK_33 { get; set; }
        public decimal A_MEDI_STOCK_34 { get; set; }
        public decimal A_MEDI_STOCK_35 { get; set; }
        public decimal A_MEDI_STOCK_36 { get; set; }
        public decimal A_MEDI_STOCK_37 { get; set; }
        public decimal A_MEDI_STOCK_38 { get; set; }
        public decimal A_MEDI_STOCK_39 { get; set; }
        public decimal A_MEDI_STOCK_40 { get; set; }
        public decimal A_MEDI_STOCK_41 { get; set; }
        public decimal A_MEDI_STOCK_42 { get; set; }
        public decimal A_MEDI_STOCK_43 { get; set; }
        public decimal A_MEDI_STOCK_44 { get; set; }
        public decimal A_MEDI_STOCK_45 { get; set; }
        public decimal A_MEDI_STOCK_46 { get; set; }
        public decimal A_MEDI_STOCK_47 { get; set; }
        public decimal A_MEDI_STOCK_48 { get; set; }
        public decimal A_MEDI_STOCK_49 { get; set; }
        public decimal A_MEDI_STOCK_50 { get; set; }
    
    }
}
