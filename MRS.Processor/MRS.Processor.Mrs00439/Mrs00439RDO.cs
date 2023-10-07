using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00439
{
    public class Mrs00439RDO
    {
        public decimal TOTAL_TREATMENT_EXAM { get;  set;  }
        public decimal TOTAL_TREATMENT_TD { get;  set;  }
        public decimal TOTAL_TREATMENT { get;  set;  }
        public decimal TOTAL_EXAM { get;  set;  }
        public decimal TOTAL_EXAM_LEFT { get;  set;  }
        public decimal TOTAL_EXAM_RIGHT { get;  set;  }

        public decimal TOTAL_TREAT_OUT { get;  set;  }
        public decimal TOTAL_TREAT_OUT_DATE { get;  set;  }

        public decimal TOTAL_TREAT_IN { get;  set;  }
        public decimal TOTAL_TREAT_IN_DATE { get;  set;  }

        public decimal TOTAL_EMERGENCY { get;  set;  }
        public decimal TOTAL_TRAN_PATI { get;  set;  }

        public decimal TOTAL_SURG { get;  set;  }
        public decimal TOTAL_MISU { get;  set;  }

        public decimal TOTAL_BORN { get;  set;  }
        public decimal TOTAL_BORN_NORMAL { get;  set;  }
        public decimal TOTAL_BORN_HARD { get;  set;  }
        public decimal TOTAL_BORN_PTLT { get;  set;  }

        public decimal TOTAL_DEATH { get;  set;  }                            // chết
        public decimal TOTAL_DEATH_BEF_24 { get;  set;  }                     // chết trước 24h
        public decimal TOTAL_DEATH_AFT_24 { get;  set;  }                     // chết sau 24h

        public decimal TOTAL_TEST { get;  set;  }
        public decimal TOTAL_TEST_GP { get;  set;  }
        public decimal TOTAL_TEST_SH { get;  set;  }
        public decimal TOTAL_TEST_HH { get;  set;  }
        public decimal TOTAL_TEST_VS { get;  set;  }

        public decimal TOTAL_DIIM_XQ { get;  set;  }
        public decimal TOTAL_DIIM_SA { get;  set;  }
        public decimal TOTAL_DIIM_NS { get;  set;  }

    }

    public class RDO_TREATMENT_LOG
    {
        public long TREATMENT_ID { get;  set;  }
        public long TREATMENT_TYPE_ID { get;  set;  }
        public long IN_TIME { get;  set;  }
        public long OUT_TIME { get;  set;  }
    }

    public class LIST_KEY
    {
        public long? START_MONTH { get; set; }
        public long TREATMENT { get; set; }                  // số hsđt đk trong khoảng thời gian đã chọn (có sử dụng dịch vụ)
        public long TREATMENT_EXAM { get; set; }             // số hsđt có khám
        public decimal TOTAL_EXAM { get; set; }              // tổng số lượt khám
        public long BHYT_LEFT { get; set; }                  // bhyt đúng tuyến 
        public long BHYT_RIGHT { get; set; }                 // bhyt trái tuyến
        public long OTHER_PROVINCE { get; set; }                //Tỉnh khác

        public long TREATMENT_OUT { get; set; }              // số bn nhập viện ngoại trú
        public long TREATMENT_OUT_DATE { get; set; }         // Số ngày điều trị ngoại trú

        public long TREATMENT_IN { get; set; }               // số bn nhập viện nội trú
        public long TREATMENT_IN_DATE { get; set; }          // số ngày điều trị nội trú


        public long DIFF_DATE_REPORT { get; set; }          //Số ngày báo cáo
        public decimal REALY_BED_AMOUNT { get; set; }          //Số ngày giường thực kê

        public long RESULT_HEAVY { get; set; }              //Số ca nặng hơn

        public long EMERGENCY { get; set; }                  // số ca cấp cứu (bn mới)
        public long TRAN_PATI { get; set; }                  // số ca chuyển viện (bn mới)
        public long TOTAL_TRAN_PATI { get; set; }            // số ca chuyển viện (bao gồm cả bn cũ)
        public long TRAN_PATI_AMBULANCE { get; set; }            // số ca chuyển viện dùng xe của viện

        public decimal TOTAL_SURG { get; set; }                 // số ca pt
        public decimal TOTAL_MISU { get; set; }                 // số ca tt

        public decimal TOTAL_SURG_GROUP_DB { get; set; }                 // số ca pt dặc biệt
        public decimal TOTAL_SURG_GROUP_1 { get; set; }                 // số ca pt loại 1
        public decimal TOTAL_SURG_GROUP_2 { get; set; }                 // số ca pt loại 2
        public decimal TOTAL_SURG_GROUP_3 { get; set; }                 // số ca pt loại 3


        public decimal TOTAL_SURG_MICRO { get; set; }             //Phẫu thuật vi phẫu
        public decimal TOTAL_SURG_ENDO { get; set; }             //Phẫu thuật Nội soi

        //long TOTAL_BORN { get; set; }               // đẻ (tổng số ca trong tháng)
        public decimal TOTAL_BORN_NORMAL { get; set; }       // đẻ thường
        public decimal TOTAL_BORN_HARD { get; set; }         // đẻ khó
        public decimal TOTAL_BORN_PTLT { get; set; }         // phẫu thuật lấy thai

        //long BORN { get; set; }                     // đẻ (bn mới)
        public decimal BORN_NORMAL { get; set; }             // đẻ thường
        public decimal BORN_HARD { get; set; }               // đẻ khó
        public decimal BORN_PTLT { get; set; }               // phẫu thuật lấy thai

        public long TOTAL_DEATH { get; set; }                // chết (tổng số ca trong tháng)
        public long TOTAL_DEATH_BEF_24 { get; set; }         // chết trước 24 giờ sau vào viện
        public long TOTAL_DEATH_AFT_24 { get; set; }         // chết sau 24 giờ sau nhập viện

        public long DEATH { get; set; }                      // chết (bn mới)
        public long DEATH_BEF_24 { get; set; }
        public long DEATH_AFT_24 { get; set; }

        public decimal TOTAL_TEST_SH { get; set; }              // xn sinh hóa (tổng số ca trong tháng)
        public decimal TOTAL_TEST_HH { get; set; }              // xn huyết học
        public decimal TOTAL_TEST_VS { get; set; }              // xn ví sinh
        public decimal TOTAL_TEST_GP { get; set; }              // xn Giải phẫu

        public decimal TEST_SH { get; set; }                    // xn sinh hóa (bn mới)
        public decimal TEST_HH { get; set; }                    // xn huyết học
        public decimal TEST_VS { get; set; }                    // xn vi sinh

        public decimal TOTAL_DIIM_XQ { get; set; }              // xquang (tất cả bn)
        public decimal TOTAL_DIIM_SA { get; set; }              // siêu âm 
        public decimal TOTAL_DIIM_NS { get; set; }              // nội soi
        public decimal TOTAL_DIIM_MRI { get; set; }             //MRI
        public decimal TOTAL_DIIM_CT { get; set; }              //Chụp CT

        public decimal DIIM_XQ { get; set; }                    // xquang (bn mới)
        public decimal DIIM_SA { get; set; }
        public decimal DIIM_NS { get; set; }


        public decimal CLS_NEW_COUNT { get; set; }                    //Tổng số cận lâm sàng mới
        public decimal CLS_NEW_AMOUNT { get; set; }                    //Tổng số lượng sử dụng cận lâm sàng mới

        public long? FINISH_MONTH { get; set; }

        public int COUNT { get; set; }
    }
}
