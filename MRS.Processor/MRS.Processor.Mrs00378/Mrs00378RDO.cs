using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00378
{
    public class Mrs00378RDO : HIS_EXP_MEST_MATERIAL
    {
        public Mrs00378RDO()
        {
        }

        public Mrs00378RDO(HIS_EXP_MEST_MATERIAL data)
        {
            if (data != null)
            {
                Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00378RDO>(this, data);
            }
            this.VIR_PRICE = data.PRICE ?? 0;
        }

        public string TREATMENT_CODE { get; set; }

        public string PATIENT_NAME { get; set; }

        public string SERVICE_NAME { get; set; }
        public string SERVICE_CODE { get; set; }
        public string REQ_DEPARTMENT_NAME { get; set; }
        public string CONCENTRA { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }

        public decimal AMOUNT_1 { get; set; }
        public decimal AMOUNT_2 { get; set; }
        public decimal AMOUNT_3 { get; set; }
        public decimal AMOUNT_4 { get; set; }
        public decimal AMOUNT_5 { get; set; }
        public decimal AMOUNT_6 { get; set; }
        public decimal AMOUNT_7 { get; set; }
        public decimal AMOUNT_8 { get; set; }
        public decimal AMOUNT_9 { get; set; }
        public decimal AMOUNT_10 { get; set; }
        public decimal AMOUNT_11 { get; set; }
        public decimal AMOUNT_12 { get; set; }
        public decimal AMOUNT_13 { get; set; }
        public decimal AMOUNT_14 { get; set; }
        public decimal AMOUNT_15 { get; set; }
        public decimal AMOUNT_16 { get; set; }
        public decimal AMOUNT_17 { get; set; }
        public decimal AMOUNT_18 { get; set; }
        public decimal AMOUNT_19 { get; set; }
        public decimal AMOUNT_20 { get; set; }
        public decimal AMOUNT_21 { get; set; }
        public decimal AMOUNT_22 { get; set; }
        public decimal AMOUNT_23 { get; set; }
        public decimal AMOUNT_24 { get; set; }
        public decimal AMOUNT_25 { get; set; }
        public decimal AMOUNT_26 { get; set; }
        public decimal AMOUNT_27 { get; set; }
        public decimal AMOUNT_28 { get; set; }
        public decimal AMOUNT_29 { get; set; }
        public decimal AMOUNT_30 { get; set; }

    }
}
