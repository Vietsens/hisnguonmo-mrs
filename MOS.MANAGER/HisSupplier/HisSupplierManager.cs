using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSupplier
{
    public partial class HisSupplierManager : BusinessBase
    {
        public HisSupplierManager()
            : base()
        {

        }

        public HisSupplierManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_SUPPLIER> Get(HisSupplierFilterQuery filter)
        {
            List<HIS_SUPPLIER> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SUPPLIER> resultData = null;
                if (valid)
                {
                    resultData = new HisSupplierGet(param).Get(filter);
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

        
        public HIS_SUPPLIER GetById(long data)
        {
            HIS_SUPPLIER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SUPPLIER resultData = null;
                if (valid)
                {
                    resultData = new HisSupplierGet(param).GetById(data);
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

        
        public HIS_SUPPLIER GetById(long data, HisSupplierFilterQuery filter)
        {
            HIS_SUPPLIER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SUPPLIER resultData = null;
                if (valid)
                {
                    resultData = new HisSupplierGet(param).GetById(data, filter);
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

        
        public HIS_SUPPLIER GetByCode(string data)
        {
            HIS_SUPPLIER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SUPPLIER resultData = null;
                if (valid)
                {
                    resultData = new HisSupplierGet(param).GetByCode(data);
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

        
        public HIS_SUPPLIER GetByCode(string data, HisSupplierFilterQuery filter)
        {
            HIS_SUPPLIER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SUPPLIER resultData = null;
                if (valid)
                {
                    resultData = new HisSupplierGet(param).GetByCode(data, filter);
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
