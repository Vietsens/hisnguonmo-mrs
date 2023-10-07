using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00105
{
    public class Mrs00105Filter
    {
        public bool? IS_MERGER_IMP_PRICE;
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public bool? IS_MERGE { get; set; }
        public bool? IS_MERGER { get; set; }
        public bool? IS_MEDICINE { get; set; }
        public bool? IS_MATERIAL { get; set; }
        public bool? IS_CHEMICAL_SUBSTANCE { get; set; }
        public bool? IS_BLOOD { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public List<long> PREVIOUS_MEDI_STOCK_IDs { get; set; }
        public List<long> EXP_MEDI_STOCK_IDs { get; set; }
        public bool? IS_MERGE_BID_NUMBER { set; get; }
        public bool? IS_MERGER_EXPIRED_DATE { get; set; }

        public bool? IS_MERGER_SUPPLIER { get; set; }

        public bool? IS_MERGER_MEDI_STOCK { get; set; }

        public bool? IS_MERGE_STOCK { get; set; }

        public long? DEPARTMENT_ID { get; set; }

        public long? BRANCH_ID { get; set; }

        public List<long> DEPARTMENT_IDs { get; set; }

        public bool? IS_CABINET { get; set; }

        public bool? IS_MERGE_PACKAGE_NUMBER { get; set; }//tách thuốc, vật tư theo lô

        public string TEST_PACKAGE_NUMBERs { get; set; }//tách thuốc, vật tư theo lô

        public long? MONTH { get; set; }

        public bool? IS_DETAIL { get; set; }

        public bool? TAKE_INPUT_END_AMOUNT { get; set; }

        public bool? TAKE_ANTICIPATE_INFO { get; set; }

        public bool? TAKE_IMP_EXP_MEST { get; set; }

        public List<long> IMP_SOURCE_IDs { get; set; }

        public List<long> MEDICINE_GROUP_IDs { get; set; }

        public List<long> MEDICINE_LINE_IDs { get; set; }

        //public string MEDI_STOCK_CODE__NGTs { get; set; }

        public string MEDI_STOCK_CODE__NTs { get; set; }

        //public string MEDI_STOCK_CODE__CLSs { get; set; }

        public bool? IS_MEDICINE_GROUP { get; set; }
        public string MEDICINE_TYPE_CODEs { get; set; }
        public string BLOOD_TYPE_CODEs { get; set; }
        public string MATERIAL_TYPE_CODEs { get; set; }

        public bool? ADD_EXP_AMOUNT_BHYT { get; set; }

        public string MEDI_STOCK_CODE__CHMSs { get; set; }//   danh sách các kho xuất của phiếu nhập chuyển kho

        public string MEDI_STOCK_CODE__IMPs { get; set; }//   danh sách các kho nhập của phiếu xuất chuyển kho
                                                         //select distinct ms.medi_stock_code from
                                                         //his_exp_mest im
                                                         //join his_medi_stock ms on ms.id=im.imp_medi_stock_id
                                                         //where im.imp_medi_stock_id is not null
        public List<long> EXACT_PARENT_SERVICE_IDs { get; set; }

        public string KEY_GROUP_INV { get; set; }// key group

        public string KEY_GROUP_INV_FINAL { get; set; }// key group cuối cùng

        public string REQ_DEPARTMENT_CODE__CHMSs { get; set; }

        public string REQ_DEPARTMENT_CODE__IMPs { get; set; }

        public bool? IS_NOT_ROUND_IMP_PRICE { get; set; }// Không làm tròn giá nhập

        public short? NATIONAL_NAME { set; get; }//null:all; 1:VN; 0: Nước ngoài

        public bool? IS_MERGE_CODE { get; set; }

        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public bool IS_MERGE_PRICE_EXPIRED_DATE_PACKAGE_NUMBER { set; get; }

        public bool IS_MERGER_PRICE { set; get; }

        public bool? IS_MERGE_MEDI_MATE_TYPE_ID { get; set; }

        public List<long> INPUT_DATA_ID_MEDICINE_LINEs { get; set; }//dòng thuốc: 1. Thuốc thường, 2. Chế phẩm yhct, 3. Vị thuốc yhct

        public List<long> MEDICINE_TYPE_IDs { get; set; }
        public List<long> BLOOD_TYPE_IDs { get; set; }
        public List<long> MATERIAL_TYPE_IDs { get; set; }

        public long? MEDI_STOCK_ID { get; set; }

        public List<RoleUserADO> EXECUTE_ROLE_GROUP { get; set; }

        public List<long> QUOTA_SERVICE_TYPE_IDs { get; set; }

        public short? INPUT_DATA_ID_IS_BHYT { get; set; }//Là bảo hiểm: 1. Bảo hiểm, 2. viện phí

        public List<long> BID_IDs { get; set; }

        public long? BID_ID { get; set; }

        public bool? ADD_IMP_AMOUNT_BHYT { get; set; }// thêm số lượng nhập bảo hiểm dịch vụ

        public bool? ADD_IMP_LOCAL { get; set; }// thêm key nhập xuất nội bộ trong kỳ

        public bool? IS_ONLLY_GROUP_SERVICE { get; set; }// thêm key nhập xuất nội bộ trong kỳ


        public List<long> MEDI_STOCK_CABINET_IDs { get; set; }

        public long? MEDI_STOCK_CABINET_ID { get; set; }//tủ trực

        public bool? IS_GROUP { get; set; }

        public bool? IS_ADD_BID_IMP_INFO { get; set; }//thêm dữ liệu nhập của các thầu

        public bool? IS_ONLY_IN_BID { get; set; }//chỉ lấy dữ liệu trong thầu

        public bool? ADD_BID_DETAIL_NO_IMP { get; set; }

        public string PARENT_SERVICE_CODEs { get; set; }

        public string KEY_ORDER { get; set; }

        public string KEY_GROUP_BID_DETAIL { get; set; }

        public long? BID_CREATE_TIME_FROM { get; set; }//THỜI GIAN TẠO THẦU TỪ
        public long? BID_CREATE_TIME_TO { get; set; }//THỜI GIAN TẠO THẦU ĐẾN

        public List<long> HEIN_SERVICE_TYPE_IDs { get; set; }// nhóm bảo hiểm

        public string MEDI_STOCK_STR_CODEs { get; set; }

        public bool? IS_IMP_DEPA_BHYT { get; set; }

        public bool? IS_IMP_DEPA_VP { get; set; }

        public bool? IS_IMP_DEPA_HP { get; set; }

        public bool? IS_IMP_DEPA_TL { get; set; }
    }
}
