using MOS.EFMODEL.DataModels; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00742
{
    public class Mrs00742RDO:HIS_TREATMENT
    {
       

        public Mrs00742RDO()
        {

        }
        public long TOTAL { get; set; }
        public long SICK { get; set; }
        public long HEIN { get; set; }
        public long FEE { get; set; }
        public long FREE { get; set; }
        public long KSK {get; set; }	
        public long CHILD {get; set; }
        public long HAS_MEDI {get; set; }

        public long F_ROOM_1 { get; set; }//F_ROOM: cac phong ket thuc kham tat ca cac cong kham co thuc hien
        public long F_ROOM_2 { get; set; }
        public long F_ROOM_3 { get; set; }
        public long F_ROOM_4 { get; set; }
        public long F_ROOM_5 { get; set; }
        public long F_ROOM_6 { get; set; }
        public long F_ROOM_7 { get; set; }
        public long F_ROOM_8 { get; set; }
        public long F_ROOM_9 { get; set; }
        public long F_ROOM_10 { get; set; }
        public long F_ROOM_11 { get; set; }
        public long F_ROOM_12 { get; set; }
        public long F_ROOM_13 { get; set; }
        public long F_ROOM_14 { get; set; }
        public long F_ROOM_15 { get; set; }
        public long F_ROOM_16 { get; set; }
        public long F_ROOM_17 { get; set; }
        public long F_ROOM_18 { get; set; }
        public long F_ROOM_19 { get; set; }
        public long F_ROOM_20 { get; set; }
        public long F_ROOM_21 { get; set; }
        public long F_ROOM_22 { get; set; }
        public long F_ROOM_23 { get; set; }
        public long F_ROOM_24 { get; set; }
        public long F_ROOM_25 { get; set; }
        public long F_ROOM_26 { get; set; }
        public long F_ROOM_27 { get; set; }
        public long F_ROOM_28 { get; set; }
        public long F_ROOM_29 { get; set; }
        public long F_ROOM_30 { get; set; }
        public long F_ROOM_31 { get; set; }
        public long F_ROOM_32 { get; set; }
        public long F_ROOM_33 { get; set; }
        public long F_ROOM_34 { get; set; }
        public long F_ROOM_35 { get; set; }
        public long F_ROOM_36 { get; set; }
        public long F_ROOM_37 { get; set; }
        public long F_ROOM_38 { get; set; }
        public long F_ROOM_39 { get; set; }
        public long F_ROOM_40 { get; set; }
        public long F_ROOM_41 { get; set; }
        public long F_ROOM_42 { get; set; }
        public long F_ROOM_43 { get; set; }
        public long F_ROOM_44 { get; set; }
        public long F_ROOM_45 { get; set; }
        public long F_ROOM_46 { get; set; }
        public long F_ROOM_47 { get; set; }
        public long F_ROOM_48 { get; set; }
        public long F_ROOM_49 { get; set; }
        public long F_ROOM_50 { get; set; }


        public long CLN_HEIN { get; set; }
        public long CLN_FEE { get; set; }
        public long CLN_FREE { get; set; }	
        public long TRAN_OUT {get; set; }
        public long LEFT_LINE {get; set; }


        public long TRANSFER_OUT { get; set; }

        public string IN_DATE_STR { get; set; }

        public long KSKs { get; set; }

        public long COUNT_BHYT { get; set; }

        public long COUNT_KSK { get; set; }

        public long COUNT_VP { get; set; }

        public long MALE { get; set; }

        public long FEMALE { get; set; }

        //tong theo ho so dieu tri

        public long T_TOTAL { get; set; }
        public long T_SICK { get; set; }
        public long T_HEIN { get; set; }
        public long T_FEE { get; set; }
        public long T_FREE { get; set; }
        public long T_KSK { get; set; }
        public long T_CHILD { get; set; }
        public long T_HAS_MEDI { get; set; }
        public long T_CLN_HEIN { get; set; }
        public long T_CLN_FEE { get; set; }
        public long T_CLN_FREE { get; set; }
        public long T_TRAN_OUT { get; set; }
        public long T_LEFT_LINE { get; set; }

        public int T_KSKs { get; set; }

        public long T_MALE { get; set; }

        public long T_FEMALE { get; set; }

        public long T_ROOM_1 { get; set; }//T_ROOM: cac phong co xu li kham nhap vien hoac ket thuc dieu tri
        public long T_ROOM_2 { get; set; }
        public long T_ROOM_3 { get; set; }
        public long T_ROOM_4 { get; set; }
        public long T_ROOM_5 { get; set; }
        public long T_ROOM_6 { get; set; }
        public long T_ROOM_7 { get; set; }
        public long T_ROOM_8 { get; set; }
        public long T_ROOM_9 { get; set; }
        public long T_ROOM_10 { get; set; }
        public long T_ROOM_11 { get; set; }
        public long T_ROOM_12 { get; set; }
        public long T_ROOM_13 { get; set; }
        public long T_ROOM_14 { get; set; }
        public long T_ROOM_15 { get; set; }
        public long T_ROOM_16 { get; set; }
        public long T_ROOM_17 { get; set; }
        public long T_ROOM_18 { get; set; }
        public long T_ROOM_19 { get; set; }
        public long T_ROOM_20 { get; set; }
        public long T_ROOM_21 { get; set; }
        public long T_ROOM_22 { get; set; }
        public long T_ROOM_23 { get; set; }
        public long T_ROOM_24 { get; set; }
        public long T_ROOM_25 { get; set; }
        public long T_ROOM_26 { get; set; }
        public long T_ROOM_27 { get; set; }
        public long T_ROOM_28 { get; set; }
        public long T_ROOM_29 { get; set; }
        public long T_ROOM_30 { get; set; }
        public long T_ROOM_31 { get; set; }
        public long T_ROOM_32 { get; set; }
        public long T_ROOM_33 { get; set; }
        public long T_ROOM_34 { get; set; }
        public long T_ROOM_35 { get; set; }
        public long T_ROOM_36 { get; set; }
        public long T_ROOM_37 { get; set; }
        public long T_ROOM_38 { get; set; }
        public long T_ROOM_39 { get; set; }
        public long T_ROOM_40 { get; set; }
        public long T_ROOM_41 { get; set; }
        public long T_ROOM_42 { get; set; }
        public long T_ROOM_43 { get; set; }
        public long T_ROOM_44 { get; set; }
        public long T_ROOM_45 { get; set; }
        public long T_ROOM_46 { get; set; }
        public long T_ROOM_47 { get; set; }
        public long T_ROOM_48 { get; set; }
        public long T_ROOM_49 { get; set; }
        public long T_ROOM_50 { get; set; }

        public long CLN_BHYT { get; set; }

        public long CLN_VP { get; set; }
    }
    class Title
    {
        public string ROOM_NAME_1 { get; set; }
        public string ROOM_NAME_2 { get; set; }
        public string ROOM_NAME_3 { get; set; }
        public string ROOM_NAME_4 { get; set; }
        public string ROOM_NAME_5 { get; set; }
        public string ROOM_NAME_6 { get; set; }
        public string ROOM_NAME_7 { get; set; }
        public string ROOM_NAME_8 { get; set; }
        public string ROOM_NAME_9 { get; set; }
        public string ROOM_NAME_10 { get; set; }
        public string ROOM_NAME_11 { get; set; }
        public string ROOM_NAME_12 { get; set; }
        public string ROOM_NAME_13 { get; set; }
        public string ROOM_NAME_14 { get; set; }
        public string ROOM_NAME_15 { get; set; }
        public string ROOM_NAME_16 { get; set; }
        public string ROOM_NAME_17 { get; set; }
        public string ROOM_NAME_18 { get; set; }
        public string ROOM_NAME_19 { get; set; }
        public string ROOM_NAME_20 { get; set; }
        public string ROOM_NAME_21 { get; set; }
        public string ROOM_NAME_22 { get; set; }
        public string ROOM_NAME_23 { get; set; }
        public string ROOM_NAME_24 { get; set; }
        public string ROOM_NAME_25 { get; set; }
        public string ROOM_NAME_26 { get; set; }
        public string ROOM_NAME_27 { get; set; }
        public string ROOM_NAME_28 { get; set; }
        public string ROOM_NAME_29 { get; set; }
        public string ROOM_NAME_30 { get; set; }
        public string ROOM_NAME_31 { get; set; }
        public string ROOM_NAME_32 { get; set; }
        public string ROOM_NAME_33 { get; set; }
        public string ROOM_NAME_34 { get; set; }
        public string ROOM_NAME_35 { get; set; }
        public string ROOM_NAME_36 { get; set; }
        public string ROOM_NAME_37 { get; set; }
        public string ROOM_NAME_38 { get; set; }
        public string ROOM_NAME_39 { get; set; }
        public string ROOM_NAME_40 { get; set; }
        public string ROOM_NAME_41 { get; set; }
        public string ROOM_NAME_42 { get; set; }
        public string ROOM_NAME_43 { get; set; }
        public string ROOM_NAME_44 { get; set; }
        public string ROOM_NAME_45 { get; set; }
        public string ROOM_NAME_46 { get; set; }
        public string ROOM_NAME_47 { get; set; }
        public string ROOM_NAME_48 { get; set; }
        public string ROOM_NAME_49 { get; set; }
        public string ROOM_NAME_50 { get; set; }

    }
}
