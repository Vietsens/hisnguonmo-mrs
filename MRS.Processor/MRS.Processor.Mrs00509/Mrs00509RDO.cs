using MRS.Processor.Mrs00509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Proccessor.Mrs00509
{
    public class Mrs00509RDO
    {
        public long TYPE { get; set; }
        public long MEDI_MATE_TYPE_ID { get; set; }
        public long SERVICE_TYPE_ID { get; set; }

        public long MEDI_MATE_ID { get; set; }
        public long SERVICE_ID { get; set; }
        public string PACKAGE_NUMBER { get; set; }
        public string EXPIRED_DATE_STR { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public string IMP_DEPARTMENT_NAME { get; set; }


        public decimal? VAT_RATIO { get; set; }
        public string VAT_RATIO_STR { get; set; }
        public decimal IMP_PRICE { get; set; }
        public string CONCENTRA { get; set; }
        public string MEDICINE_TYPE_PROPRIETARY_NAME { get; set; }
        public string NATIONAL_NAME { get; set; }
        public string HEIN_SERVICE_CODE { get; set; }
        public string HEIN_SERVICE_NAME { get; set; }
        public string MANUFACTURER_NAME { get; set; }

        public long? SUPPLIER_ID { get; set; }
        public string SUPPLIER_CODE { get; set; }
        public string SUPPLIER_NAME { get; set; }

        //Phần tách nhóm
        public decimal MAIN_IMP_AMOUNT { get; set; }

        public decimal MAIN_EXP_AMOUNT { get; set; }

        public decimal MAIN_BEGIN_AMOUNT { get; set; }

        public decimal MAIN_END_AMOUNT { get; set; }

        public decimal EXTRA_IMP_AMOUNT { get; set; }

        public decimal EXTRA_EXP_AMOUNT { get; set; }

        public decimal EXTRA_EXP_AMOUNT_DV { get; set; }

        public decimal EXTRA_EXP_AMOUNT_BHYT { get; set; }

        public decimal EXTRA_BEGIN_AMOUNT { get; set; }

        public decimal EXTRA_END_AMOUNT { get; set; }

        public Mrs00509RDO()
        {

        }
    }
}
