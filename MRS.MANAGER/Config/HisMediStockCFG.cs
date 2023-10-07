using MOS.MANAGER.HisMediStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Config
{
    public class HisMediStockCFG
    {
        private static List<MOS.EFMODEL.DataModels.V_HIS_MEDI_STOCK> mediStocks;
        public static List<MOS.EFMODEL.DataModels.V_HIS_MEDI_STOCK> HisMediStocks
        {
            get
            {
                if (mediStocks == null || mediStocks.Count == 0)
                {
                    mediStocks = new List<MOS.EFMODEL.DataModels.V_HIS_MEDI_STOCK>();
                    mediStocks.AddRange(GetAll());
                }
                return mediStocks;
            }
            set
            {
                mediStocks = value;
            }
        }

        private static List<MOS.EFMODEL.DataModels.V_HIS_MEDI_STOCK> GetAll()
        {
            List<MOS.EFMODEL.DataModels.V_HIS_MEDI_STOCK> result = new List<MOS.EFMODEL.DataModels.V_HIS_MEDI_STOCK>();
            try
            {
                HisMediStockViewFilterQuery filter = new HisMediStockViewFilterQuery();
                result = new HisMediStockManager().GetView(filter);
                if (result == null) result = new List<MOS.EFMODEL.DataModels.V_HIS_MEDI_STOCK>();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new List<MOS.EFMODEL.DataModels.V_HIS_MEDI_STOCK>();
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                mediStocks = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
