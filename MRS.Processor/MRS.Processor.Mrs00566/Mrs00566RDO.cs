using MOS.EFMODEL.DataModels;
using System;

namespace MRS.Processor.Mrs00566
{
    class Mrs00566RDO
    {
        public string MEDICINE_TYPE_NAME { get; set; }
        public string MEDICINE_TYPE_CODE { get; set; }
        public string PACKAGE_NUMBER { get; set; }
        public string UNIT { get; set; }
        public Decimal PRICE { get; set; }
        public Decimal AMOUNT { get; set; }
        public Decimal TT_PRICE { get; set; }

    }
}
