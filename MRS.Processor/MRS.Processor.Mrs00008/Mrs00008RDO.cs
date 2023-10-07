using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMediStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00008
{
    class Mrs00008RDO
    {
        public string MEDICINE_TYPE_CODE { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string PATIENT_CODE { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
        public decimal AMOUNT { get; set; }
        public string EXP_MEST_CODE { get; set; }
        public string MEDI_STOCK_NAME { get; set; }
        public string MEDI_STOCK_CODE { get; set; }

        public Mrs00008RDO(V_HIS_EXP_MEST_MEDICINE expMestMedicine)
        {
            try
            {
                MEDICINE_TYPE_CODE = expMestMedicine.MEDICINE_TYPE_CODE;
                MEDICINE_TYPE_NAME = expMestMedicine.MEDICINE_TYPE_NAME;
                SERVICE_UNIT_NAME = expMestMedicine.SERVICE_UNIT_NAME;
                AMOUNT = expMestMedicine.AMOUNT;
                EXP_MEST_CODE = expMestMedicine.EXP_MEST_CODE;
                SetExtendField(expMestMedicine);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void SetExtendField(V_HIS_EXP_MEST_MEDICINE data)
        {
            try
            {
                var patient = new HisExpMestManager().GetById(data.EXP_MEST_ID ?? 0);
                if (patient != null)
                {
                    PATIENT_CODE = patient.TDL_PATIENT_CODE;
                    VIR_PATIENT_NAME = patient.TDL_PATIENT_NAME;
                }

                HIS_MEDI_STOCK mediStock = new HisMediStockManager().GetById(data.MEDI_STOCK_ID);
                if (mediStock != null)
                {
                    MEDI_STOCK_CODE = mediStock.MEDI_STOCK_NAME;
                    MEDI_STOCK_NAME = mediStock.MEDI_STOCK_NAME;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
    }
}
