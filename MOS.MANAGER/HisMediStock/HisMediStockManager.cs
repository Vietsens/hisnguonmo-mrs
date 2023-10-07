using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediStock
{
    public partial class HisMediStockManager : BusinessBase
    {
        public HisMediStockManager()
            : base()
        {

        }

        public HisMediStockManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_MEDI_STOCK> Get(HisMediStockFilterQuery filter)
        {
            List<HIS_MEDI_STOCK> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEDI_STOCK> resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockGet(param).Get(filter);
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

        
        public HIS_MEDI_STOCK GetById(long data)
        {
            HIS_MEDI_STOCK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEDI_STOCK resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockGet(param).GetById(data);
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

        
        public HIS_MEDI_STOCK GetById(long data, HisMediStockFilterQuery filter)
        {
            HIS_MEDI_STOCK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEDI_STOCK resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockGet(param).GetById(data, filter);
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

        
        public HIS_MEDI_STOCK GetByCode(string data)
        {
            HIS_MEDI_STOCK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEDI_STOCK resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockGet(param).GetByCode(data);
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

        
        public HIS_MEDI_STOCK GetByCode(string data, HisMediStockFilterQuery filter)
        {
            HIS_MEDI_STOCK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEDI_STOCK resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockGet(param).GetByCode(data, filter);
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

        
        public HIS_MEDI_STOCK GetByRoomId(long data)
        {
            HIS_MEDI_STOCK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEDI_STOCK resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockGet(param).GetByRoomId(data);
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

        
        public List<HIS_MEDI_STOCK> GetActive()
        {
            List<HIS_MEDI_STOCK> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                List<HIS_MEDI_STOCK> resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockGet(param).GetActive();
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

        
        public List<HIS_MEDI_STOCK> GetByRoomIds(List<long> roomIds)
        {
            List<HIS_MEDI_STOCK> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(roomIds);
                List<HIS_MEDI_STOCK> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_MEDI_STOCK>();
                    var skip = 0;
                    while (roomIds.Count - skip > 0)
                    {
                        var Ids = roomIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisMediStockGet(param).GetByRoomIds(Ids));
                    }
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

        
        public List<HIS_MEDI_STOCK> GetByParentId(long data)
        {
            List<HIS_MEDI_STOCK> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEDI_STOCK> resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockGet(param).GetByParentId(data);
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
