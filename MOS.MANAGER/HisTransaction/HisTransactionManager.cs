using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTransaction
{
    public partial class HisTransactionManager : BusinessBase
    {
        public HisTransactionManager()
            : base()
        {

        }

        public HisTransactionManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_TRANSACTION> Get(HisTransactionFilterQuery filter)
        {
            List<HIS_TRANSACTION> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_TRANSACTION> resultData = null;
                if (valid)
                {
                    resultData = new HisTransactionGet(param).Get(filter);
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

        
        public HIS_TRANSACTION GetById(long data)
        {
            HIS_TRANSACTION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_TRANSACTION resultData = null;
                if (valid)
                {
                    resultData = new HisTransactionGet(param).GetById(data);
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

        
        public HIS_TRANSACTION GetById(long data, HisTransactionFilterQuery filter)
        {
            HIS_TRANSACTION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_TRANSACTION resultData = null;
                if (valid)
                {
                    resultData = new HisTransactionGet(param).GetById(data, filter);
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

        
        public List<HIS_TRANSACTION> GetTotal(HisTransactionFilterQuery filter)
        {
            List<HIS_TRANSACTION> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_TRANSACTION> resultData = null;
                if (valid)
                {
                    resultData = new HisTransactionGet(param).GetTotal(filter);
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

        
        public List<HIS_TRANSACTION> GetByPayFormId(long filter)
        {
            List<HIS_TRANSACTION> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_TRANSACTION> resultData = null;
                if (valid)
                {
                    resultData = new HisTransactionGet(param).GetByPayFormId(filter);
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

        
        public List<HIS_TRANSACTION> GetByTransactionTypeId(long filter)
        {
            List<HIS_TRANSACTION> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_TRANSACTION> resultData = null;
                if (valid)
                {
                    resultData = new HisTransactionGet(param).GetByTransactionTypeId(filter);
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

        
        public List<HIS_TRANSACTION> GetByTreatmentId(long filter)
        {
            List<HIS_TRANSACTION> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_TRANSACTION> resultData = null;
                if (valid)
                {
                    resultData = new HisTransactionGet(param).GetByTreatmentId(filter);
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

        
        public List<HIS_TRANSACTION> GetByAccountBookId(long filter)
        {
            List<HIS_TRANSACTION> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_TRANSACTION> resultData = null;
                if (valid)
                {
                    resultData = new HisTransactionGet(param).GetByAccountBookId(filter);
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

        
        public List<HIS_TRANSACTION> GetByCashierRoomId(long filter)
        {
            List<HIS_TRANSACTION> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_TRANSACTION> resultData = null;
                if (valid)
                {
                    resultData = new HisTransactionGet(param).GetByCashierRoomId(filter);
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

        
        public List<HIS_TRANSACTION> GetByCashoutId(long filter)
        {
            List<HIS_TRANSACTION> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_TRANSACTION> resultData = null;
                if (valid)
                {
                    resultData = new HisTransactionGet(param).GetByCashoutId(filter);
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

        
        public List<HIS_TRANSACTION> GetByIds(List<long> filter)
        {
            List<HIS_TRANSACTION> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_TRANSACTION> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_TRANSACTION>();
                    var skip = 0;
                    while (filter.Count - skip > 0)
                    {
                        var Ids = filter.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisTransactionGet(param).GetByIds(Ids));
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
    }
}
