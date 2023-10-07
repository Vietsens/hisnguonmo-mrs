using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00277
{
    public class Mrs00277RDO : V_HIS_HEIN_APPROVAL
    {
        public int IS_DUNG_TRAI { get; set; }
        public string DUNG_TRAI { get; set; }

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
        public decimal TRAN_PRICE { get; set; }
        public decimal TOTAL_PATIENT_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE_NDS { get; set; }
        public decimal EXAM_PRICE { get; set; }

        public decimal TOTAL_COUNT { get; set; }
        public decimal TOTAL_DATE { get; set; }
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }
        public decimal TT_PRICE { get;  set; }

        public Mrs00277RDO() { }

        public Mrs00277RDO(V_HIS_HEIN_APPROVAL data)
        {
            try
            {
                if (data != null)
                {
                    Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00277RDO>(this, data);
                    if (data.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
                    {
                        if (data.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.EMERGENCY)
                        {
                            this.IS_DUNG_TRAI = 2;
                            this.DUNG_TRAI = "Đúng tuyến cấp cứu";
                        }
                        else
                        {
                            this.IS_DUNG_TRAI = 1;
                            this.DUNG_TRAI = "Đúng tuyến";
                        }
                    }
                    else
                    {
                        this.IS_DUNG_TRAI = 3;
                        this.DUNG_TRAI = "Trái tuyến";
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
