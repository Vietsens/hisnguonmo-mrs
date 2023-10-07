using MOS.MANAGER.HisBranch;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00073
{
    public class Mrs00073RDO : V_HIS_HEIN_APPROVAL
    {
        public short? IS_DUNG_TRAI { get; set; }
        public string DUNG_TRAI { get; set; }

        public long? DOB_MALE { get; set; }
        public long? DOB_FEMALE { get; set; }
        public long TOTAL_DATE { get; set; }

        public string PATIENT_NAME { get; set; }
        public string MEDIORG_NAME { get; set; }
        public string ICD_CODE_MAIN { get; set; }
        public string OPEN_TIME { get; set; }
        public string CLOSE_TIME { get; set; }
        public string BRANCH_NAME { get; set; }

        public decimal TOTAL_PRICE { get; set; }
        public decimal TEST_PRICE { get; set; }
        public decimal DIIM_PRICE { get; set; }
        public decimal MEDICINE_PRICE { get; set; }
        public decimal BLOOD_PRICE { get; set; }
        public decimal SURGMISU_PRICE { get; set; }
        public decimal MATERIAL_PRICE { get; set; }
        public decimal SERVICE_PRICE_RATIO { get; set; }
        public decimal MEDICINE_PRICE_RATIO { get; set; }
        public decimal MATERIAL_PRICE_RATIO { get; set; }
        public decimal BED_PRICE { get; set; }
        public decimal EXAM_PRICE { get; set; }
        public decimal TRAN_PRICE { get; set; }
        public decimal TOTAL_PATIENT_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE_NDS { get; set; }

        public decimal TOTAL_COUNT { get; set; }

        //TheoThoi gian vao
        public string IN_TIME_STR { get; set; }
        public long? MAIN_DAY { get; set; }

        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public Mrs00073RDO()
        {

        }
        public Mrs00073RDO(V_HIS_HEIN_APPROVAL heinApproval, Mrs00073Filter castFilter)
        {
            if (heinApproval != null)
            {
                Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00073RDO>(this, heinApproval);
                //this.TREATMENT_CODE = heinApproval.TREATMENT_CODE; 
                //this.PATIENT_NAME = heinApproval.TDL_PATIENT_NAME; 
                //this.MEDIORG_NAME = heinApproval.HEIN_MEDI_ORG_NAME; 
                this.BRANCH_NAME = (HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID) ?? new HIS_BRANCH()).BRANCH_NAME;
                if (heinApproval.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
                {
                   
                    this.IS_DUNG_TRAI = 1;
                    this.DUNG_TRAI = "Đúng tuyến";
                    if (castFilter.SPLIT_RIGHT_ROUTE == true)
                    {
                        if (heinApproval.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.EMERGENCY)
                        {
                            this.IS_DUNG_TRAI = 2;
                            this.DUNG_TRAI = "Cấp cứu";
                        }
                        else if (heinApproval.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.OVER)
                        {
                            this.IS_DUNG_TRAI = 3;
                            this.DUNG_TRAI = "Thông tuyến";
                        }
                    }
                }
                else
                {
                    this.IS_DUNG_TRAI = 0;
                    this.DUNG_TRAI = "Trái tuyến";
                }
            }
        }

        public Mrs00073RDO(List<Mrs00073RDO> Data)
        {
            try
            {
                TOTAL_COUNT = Data.Sum(s => s.TOTAL_COUNT);
                TOTAL_PRICE = Data.Sum(s => s.TOTAL_PRICE);
                TEST_PRICE = Data.Sum(s => s.TEST_PRICE);
                DIIM_PRICE = Data.Sum(s => s.DIIM_PRICE);
                MEDICINE_PRICE = Data.Sum(s => s.MEDICINE_PRICE);
                BLOOD_PRICE = Data.Sum(s => s.BLOOD_PRICE);
                MATERIAL_PRICE = Data.Sum(s => s.MATERIAL_PRICE);
                SURGMISU_PRICE = Data.Sum(s => s.SURGMISU_PRICE);
                SERVICE_PRICE_RATIO = Data.Sum(s => s.SERVICE_PRICE_RATIO);
                MEDICINE_PRICE_RATIO = Data.Sum(s => s.MEDICINE_PRICE_RATIO);
                MATERIAL_PRICE_RATIO = Data.Sum(s => s.MATERIAL_PRICE_RATIO);
                BED_PRICE = Data.Sum(s => s.BED_PRICE);
                EXAM_PRICE = Data.Sum(s => s.EXAM_PRICE);
                TRAN_PRICE = Data.Sum(s => s.TRAN_PRICE);
                TT_PRICE = Data.Sum(s => s.TT_PRICE);
                TOTAL_PATIENT_PRICE = Data.Sum(s => s.TOTAL_PATIENT_PRICE);
                TOTAL_HEIN_PRICE = Data.Sum(s => s.TOTAL_HEIN_PRICE);
                TOTAL_HEIN_PRICE_NDS = Data.Sum(s => s.TOTAL_HEIN_PRICE_NDS);
                BRANCH_NAME = Data.First().BRANCH_NAME;
                TOTAL_OTHER_SOURCE_PRICE += Data.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public string SERVICE_CODE { get; set; }

        public string SERVICE_NAME { get; set; }

        public string EXECUTE_TIME_STR { get; set; }

        public decimal PRICE { get; set; }

        public decimal AMOUNT { get; set; }

        public decimal TT_PRICE { get; set; }
    }
}
