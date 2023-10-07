using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediStockMaty
{
    public partial class HisMediStockMatyManager : BusinessBase
    {
        public HisMediStockMatyManager()
            : base()
        {

        }

        public HisMediStockMatyManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_MEDI_STOCK_MATY> Get(HisMediStockMatyFilterQuery filter)
        {
            List<HIS_MEDI_STOCK_MATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEDI_STOCK_MATY> resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockMatyGet(param).Get(filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public HIS_MEDI_STOCK_MATY GetById(long data)
        {
            HIS_MEDI_STOCK_MATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEDI_STOCK_MATY resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockMatyGet(param).GetById(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public HIS_MEDI_STOCK_MATY GetById(long data, HisMediStockMatyFilterQuery filter)
        {
            HIS_MEDI_STOCK_MATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEDI_STOCK_MATY resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockMatyGet(param).GetById(data, filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public List<HIS_MEDI_STOCK_MATY> GetByMediStockId(long data)
        {
            List<HIS_MEDI_STOCK_MATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEDI_STOCK_MATY> resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockMatyGet(param).GetByMediStockId(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public List<HIS_MEDI_STOCK_MATY> GetByMaterialTypeId(long data)
        {
            List<HIS_MEDI_STOCK_MATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEDI_STOCK_MATY> resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockMatyGet(param).GetByMaterialTypeId(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }
    }
}
