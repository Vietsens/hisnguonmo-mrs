using MRS.MANAGER.Base;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Linq;
using System.Collections.Generic;
using MOS.MANAGER.HisMedicineLine;

namespace MRS.MANAGER.Config
{
    public class HisMedicineLineCFG
    {
        
       
        private static List<MOS.EFMODEL.DataModels.HIS_MEDICINE_LINE> medicineLines;
        public static List<MOS.EFMODEL.DataModels.HIS_MEDICINE_LINE> MEDICINE_LINEs
        {
            get
            {
                if (medicineLines == null)
                {
                    medicineLines = GetAll();
                }
                return medicineLines;
            }
            set
            {
                medicineLines = value;
            }
        }

        private static List<MOS.EFMODEL.DataModels.HIS_MEDICINE_LINE> GetAll()
        {
            List<MOS.EFMODEL.DataModels.HIS_MEDICINE_LINE> result = null;
            try
            {
                HisMedicineLineFilterQuery filter = new HisMedicineLineFilterQuery();
                result = new HisMedicineLineManager().Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

       
    }
}
