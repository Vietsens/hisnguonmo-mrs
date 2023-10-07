using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MRS.Processor.Mrs00105
{
    class HisMediStockPeriodGet : GetBase
    {
        internal HisMediStockPeriodGet()
            : base()
        {

        }

        internal HisMediStockPeriodGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEDI_STOCK_PERIOD> Get(HisMediStockPeriodFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediStockPeriodDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        /// <summary>
        /// Lay ky kiem ke gan nhat tuong ung voi kho
        /// - Ky gan nhat duoc xac dinh la ky ma ko phai la "previous" cua 1 ky nao khac
        /// - Neu co hon 1 ky thoa man dieu kien tren thi lay ky co ID lon nhat
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        internal HIS_MEDI_STOCK_PERIOD GetTheLast(long mediStockId)
        {
            try
            {
                HIS_MEDI_STOCK_PERIOD result = null;
                HisMediStockPeriodFilterQuery filter = new HisMediStockPeriodFilterQuery();
                filter.MEDI_STOCK_ID = mediStockId;
                List<HIS_MEDI_STOCK_PERIOD> mediStockPeriodDTOs = this.Get(filter);
                if (mediStockPeriodDTOs != null && mediStockPeriodDTOs.Count > 0)
                {
                    List<long> previousIds = mediStockPeriodDTOs.Where(o => o.PREVIOUS_ID.HasValue).Select(o => o.PREVIOUS_ID.Value).ToList();
                    List<HIS_MEDI_STOCK_PERIOD> theLasts = mediStockPeriodDTOs.Where(o => (previousIds != null && !previousIds.Contains(o.ID)) || previousIds == null).ToList();
                    if (theLasts != null && theLasts.Count > 0)
                    {
                        result = theLasts.OrderByDescending(o => o.ID).First();
                    }
                    else
                    {
                        LogSystem.Warn("Can kiem tra lai du lieu cua CSDL. Du lieu khong hop le. Cac ky du lieu deu co ID duoc khoa ngoai den PREVIOUS_ID cua 1 ky du lieu khac." + LogUtil.TraceData("mediStockPeriodDTOs: ", mediStockPeriodDTOs));
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        
        internal HIS_MEDI_STOCK_PERIOD GetById(long id, HisMediStockPeriodFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediStockPeriodDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        
    }
}
