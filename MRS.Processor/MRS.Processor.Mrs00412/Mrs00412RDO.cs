using MOS.EFMODEL.DataModels; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00412
{
    public class Mrs00412RDO
    {
        public long DEPARTMENT_ID { get;  set;  }
        public string DEPARTMENT_NAME { get;  set;  }

        public long EXECUTE_ROOM_ID { get;  set;  }
        public string EXECUTE_ROOM_NAME { get;  set;  }

        public decimal TOTAL { get;  set;  }
        public decimal FEE { get;  set;  }
        public decimal HEIN { get;  set;  }
        public decimal HEIN_CB { get;  set;  }
        public decimal HEIN_ND { get;  set;  }
        public decimal HEIN_TE { get;  set;  }
        public decimal HEIN_TE__6T { get;  set;  }
        public decimal FEMALE { get;  set;  }
        public decimal OLDER_THAN_60 { get;  set;  }

        public string PROVINCE_CODE { get;  set;  }
        public string DISTRICT_CODE { get;  set;  }
        //00
        public decimal DISTRICT_NAME_1 { get;  set;  }
        public decimal DISTRICT_NAME_2 { get;  set;  }
        public decimal DISTRICT_NAME_3 { get;  set;  }
        public decimal DISTRICT_NAME_4 { get;  set;  }
        public decimal DISTRICT_NAME_5 { get;  set;  }
        public decimal DISTRICT_NAME_6 { get;  set;  }
        public decimal DISTRICT_NAME_7 { get;  set;  }
        public decimal DISTRICT_NAME_8 { get;  set;  }
        public decimal DISTRICT_NAME_9 { get;  set;  }
        public decimal DISTRICT_NAME_10 { get;  set;  }
        //10
        public decimal DISTRICT_NAME_11 { get;  set;  }
        public decimal DISTRICT_NAME_12 { get;  set;  }
        public decimal DISTRICT_NAME_13 { get;  set;  }
        public decimal DISTRICT_NAME_14 { get;  set;  }
        public decimal DISTRICT_NAME_15 { get;  set;  }
        public decimal DISTRICT_NAME_16 { get;  set;  }
        public decimal DISTRICT_NAME_17 { get;  set;  }
        public decimal DISTRICT_NAME_18 { get;  set;  }
        public decimal DISTRICT_NAME_19 { get;  set;  }
        public decimal DISTRICT_NAME_20 { get;  set;  }
        //20
        public decimal DISTRICT_NAME_21 { get;  set;  }
        public decimal DISTRICT_NAME_22 { get;  set;  }
        public decimal DISTRICT_NAME_23 { get;  set;  }
        public decimal DISTRICT_NAME_24 { get;  set;  }
        public decimal DISTRICT_NAME_25 { get;  set;  }
        public decimal DISTRICT_NAME_26 { get;  set;  }
        public decimal DISTRICT_NAME_27 { get;  set;  }
        public decimal DISTRICT_NAME_28 { get;  set;  }
        public decimal DISTRICT_NAME_29 { get;  set;  }
        public decimal DISTRICT_NAME_30 { get;  set;  }
        //30
        public decimal DISTRICT_NAME_31 { get;  set;  }
        public decimal DISTRICT_NAME_32 { get;  set;  }
        public decimal DISTRICT_NAME_33 { get;  set;  }
        public decimal DISTRICT_NAME_34 { get;  set;  }
        public decimal DISTRICT_NAME_35 { get;  set;  }
        public decimal DISTRICT_NAME_36 { get;  set;  }
        public decimal DISTRICT_NAME_37 { get;  set;  }
        public decimal DISTRICT_NAME_38 { get;  set;  }
        public decimal DISTRICT_NAME_39 { get;  set;  }
        public decimal DISTRICT_NAME_40 { get;  set;  }
        //40
        public decimal DISTRICT_NAME_41 { get;  set;  }
        public decimal DISTRICT_NAME_42 { get;  set;  }
        public decimal DISTRICT_NAME_43 { get;  set;  }
        public decimal DISTRICT_NAME_44 { get;  set;  }
        public decimal DISTRICT_NAME_45 { get;  set;  }
        public decimal DISTRICT_NAME_46 { get;  set;  }
        public decimal DISTRICT_NAME_47 { get;  set;  }
        public decimal DISTRICT_NAME_48 { get;  set;  }
        public decimal DISTRICT_NAME_49 { get;  set;  }
        public decimal DISTRICT_NAME_50 { get;  set;  }
        //50
        public decimal DISTRICT_NAME_51 { get;  set;  }
        public decimal DISTRICT_NAME_52 { get;  set;  }
        public decimal DISTRICT_NAME_53 { get;  set;  }
        public decimal DISTRICT_NAME_54 { get;  set;  }
        public decimal DISTRICT_NAME_55 { get;  set;  }
        public decimal DISTRICT_NAME_56 { get;  set;  }
        public decimal DISTRICT_NAME_57 { get;  set;  }
        public decimal DISTRICT_NAME_58 { get;  set;  }
        public decimal DISTRICT_NAME_59 { get;  set;  }
        public decimal DISTRICT_NAME_60 { get;  set;  }
        //60
        public decimal DISTRICT_NAME_61 { get;  set;  }
        public decimal DISTRICT_NAME_62 { get;  set;  }
        public decimal DISTRICT_NAME_63 { get;  set;  }
        public decimal DISTRICT_NAME_64 { get;  set;  }
        public decimal DISTRICT_NAME_65 { get;  set;  }
        public decimal DISTRICT_NAME_66 { get;  set;  }
        public decimal DISTRICT_NAME_67 { get;  set;  }
        public decimal DISTRICT_NAME_68 { get;  set;  }
        public decimal DISTRICT_NAME_69 { get;  set;  }
        public decimal DISTRICT_NAME_70 { get;  set;  }
        //70
        public decimal DISTRICT_NAME_71 { get;  set;  }
        public decimal DISTRICT_NAME_72 { get;  set;  }
        public decimal DISTRICT_NAME_73 { get;  set;  }
        public decimal DISTRICT_NAME_74 { get;  set;  }
        public decimal DISTRICT_NAME_75 { get;  set;  }
        public decimal DISTRICT_NAME_76 { get;  set;  }
        public decimal DISTRICT_NAME_77 { get;  set;  }
        public decimal DISTRICT_NAME_78 { get;  set;  }
        public decimal DISTRICT_NAME_79 { get;  set;  }
        public decimal DISTRICT_NAME_80 { get;  set;  }
        //80
        public decimal DISTRICT_NAME_81 { get;  set;  }
        public decimal DISTRICT_NAME_82 { get;  set;  }
        public decimal DISTRICT_NAME_83 { get;  set;  }
        public decimal DISTRICT_NAME_84 { get;  set;  }
        public decimal DISTRICT_NAME_85 { get;  set;  }
        public decimal DISTRICT_NAME_86 { get;  set;  }
        public decimal DISTRICT_NAME_87 { get;  set;  }
        public decimal DISTRICT_NAME_88 { get;  set;  }
        public decimal DISTRICT_NAME_89 { get;  set;  }
        public decimal DISTRICT_NAME_90 { get;  set;  }
        //90
        public decimal DISTRICT_NAME_91 { get;  set;  }
        public decimal DISTRICT_NAME_92 { get;  set;  }
        public decimal DISTRICT_NAME_93 { get;  set;  }
        public decimal DISTRICT_NAME_94 { get;  set;  }
        public decimal DISTRICT_NAME_95 { get;  set;  }
        public decimal DISTRICT_NAME_96 { get;  set;  }
        public decimal DISTRICT_NAME_97 { get;  set;  }
        public decimal DISTRICT_NAME_98 { get;  set;  }
        public decimal DISTRICT_NAME_99 { get;  set;  }
        public decimal DISTRICT_NAME_100 { get;  set;  }

        public decimal OTHER_PROVINCE { get;  set;  }
        public decimal TRAN_OUT { get;  set;  }
        public decimal IN_CLINICAL { get;  set;  }

        public decimal HEIN_OLDER_THAN_60 { get; set; }

        public decimal TE__6T { get; set; }

        public decimal TE__16T { get; set; }

        public decimal HEIN_FEMALE { get; set; }
    }

    public class DISTRICT
    {
        public string DISTRICT_CODE { get;  set;  }
        public string DISTRICT_NAME { get;  set;  }
        public string DISTRICT_TAG { get;  set;  }
    }
}
