using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00316
{
    public class Mrs00316RDO
    {

        public string EXECUTE_DEPARTMENT_NAME { get; set; }		//Tên khoa
        public string EXECUTE_DEPARTMENT_CODE { get; set; }		//Mã khoa khoa
        public long EXECUTE_DEPARTMENT_ID { get; set; }		//ID khoa
        public long COUNT_OLD_TREAT { get; set; }		//bệnh nhân cũ
        public long COUNT_NEW_TREAT { get; set; }		//bệnh nhân mới - NTH
        public long COUNT_ORTHER_DEPARTMENT_IN { get; set; }	//khoa khác đến - NTH
        public long COUNT_ORTHER_NEW { get; set; }	//mới khoa khác đến - TT

        public long COUNT_ORTHER_NEW_BHYT { get; set; }	//mới khoa khác đến BHYT - DKTP
        public long COUNT_ORTHER_NEW_VP { get; set; }	//mới khoa khác đến VP - DKTP
        public long COUNT_ORTHER_NEW_BHYT_TREAT_IN { get; set; }	//mới khoa khác đến BHYT - DKTP
        public long COUNT_ORTHER_NEW_VP_TREAT_IN { get; set; }	//mới khoa khác đến VP - DKTP
        public long COUNT_ORTHER_NEW_BHYT_TREAT_OUT { get; set; }	//mới khoa khác đến BHYT - DKTP
        public long COUNT_ORTHER_NEW_VP_TREAT_OUT { get; set; }	//mới khoa khác đến VP - DKTP

        public long COUNT_OUT { get; set; }		//bệnh nhân ra viện - NTH(RV,CC,XV,#) - TT(RV,CC)
        public long COUNT_OUT_NE { get; set; }		//bệnh nhân ra viện - NTH(RV,CC,XV,#) - TT(RV,CC)
        public long COUNT_OUT_XV { get; set; }		//bệnh nhân ra viện - TT(XV)
        public long COUNT_OUT_ORTHER { get; set; }		//bệnh nhân ra viện - TT(khác)
        public long COUNT_SURG { get; set; }		//bệnh nhân PT - DKTP
        public long COUNT_MOV { get; set; }		//Bệnh nhân ra khoa.
        public long COUNT_TRAN_PATI { get; set; }
        //Bệnh nhân chuyển tuyến
        public long COUNT_TE { get; set; }
        //Bệnh nhân TE - NTH

        public long COUNT_BHYT { get; set; }
        //Bệnh nhân BHYT - NTH
        public long COUNT_DV { get; set; }
        //Bệnh nhân VP - NTH
        public long COUNT_CURRENT_TREAT { get; set; }
        //Bệnh nhân còn - NTH
        public long COUNT_BHYT_TREAT_IN { get; set; }
        //Bệnh nhân BHYT - NTH
        public long COUNT_DV_TREAT_IN { get; set; }
        //Bệnh nhân VP - NTH
        public long COUNT_CURRENT_TREAT_TREAT_IN { get; set; }
        //Bệnh nhân còn - NTH 
        public long COUNT_BHYT_TREAT_OUT { get; set; }
        //Bệnh nhân BHYT - NTH
        public long COUNT_DV_TREAT_OUT { get; set; }
        //Bệnh nhân VP - NTH
        public long COUNT_CURRENT_TREAT_TREAT_OUT { get; set; }
        //Bệnh nhân còn - NTH

        public long COUNT_BHYT_SUM { get; set; }
        //Bệnh nhân BHYT - TT
        public long COUNT_FEE_SUM { get; set; }
        //Bệnh nhân VP - TT
        public long COUNT_FREE_SUM { get; set; }
        //Bệnh nhân MP - TT
        public long COUNT_BHYT_NOW { get; set; }
        //Bệnh nhân BHYT hiện tại
        public long COUNT_FEE_NOW { get; set; }
        //Bệnh nhân VP hiện tại
        public long COUNT_FREE_NOW { get; set; }
        //Bệnh nhân Miễn phí hiện tại
        public long COUNT_DIE { get; set; }
        //Bệnh nhân tử vong
        public long NUM_ORDER { get; set; }
        //Sắp xếp khoa

        //Số bệnh nhân khám
        public long COUNT_EXAM_REQUEST { get; set; }

        //số bệnh nhân kết hợp ngoại trú
        public long COUNT_CO_TREATMENT_OUT { get; set; }

        //số bệnh nhân kết hợp nội trú
        public long COUNT_CO_TREATMENT_IN { get; set; }


        public long COUNT_ORTHER_NEW_BHYT_TREAT_LIGHT { get; set; }

        public long COUNT_ORTHER_NEW_VP_TREAT_LIGHT { get; set; }

        public long COUNT_BHYT_TREAT_LIGHT { get; set; }

        public long COUNT_DV_TREAT_LIGHT { get; set; }

        public long COUNT_CURRENT_TREAT_TREAT_LIGHT { get; set; }

        public long COUNT_CO_TREATMENT_LIGHT { get; set; }
    }
}
