using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMaterialBean
{
    public partial class HisMaterialBeanManager : BusinessBase
    {
        public HisMaterialBeanManager()
            : base()
        {

        }

        public HisMaterialBeanManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_MATERIAL_BEAN> Get(HisMaterialBeanFilterQuery filter)
        {
            List<HIS_MATERIAL_BEAN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MATERIAL_BEAN> resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialBeanGet(param).Get(filter);
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

        
        public HIS_MATERIAL_BEAN GetById(long data)
        {
            HIS_MATERIAL_BEAN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MATERIAL_BEAN resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialBeanGet(param).GetById(data);
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

        
        public HIS_MATERIAL_BEAN GetById(long data, HisMaterialBeanFilterQuery filter)
        {
            HIS_MATERIAL_BEAN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MATERIAL_BEAN resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialBeanGet(param).GetById(data, filter);
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

        
        public List<HIS_MATERIAL_BEAN> GetByMediStockId(long data)
        {
            List<HIS_MATERIAL_BEAN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MATERIAL_BEAN> resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialBeanGet(param).GetByMediStockId(data);
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

        
        public List<HIS_MATERIAL_BEAN> GetByMaterialId(long data)
        {
            List<HIS_MATERIAL_BEAN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MATERIAL_BEAN> resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialBeanGet(param).GetByMaterialId(data);
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

        
        public List<HIS_MATERIAL_BEAN> GetByIds(List<long> data)
        {
            List<HIS_MATERIAL_BEAN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MATERIAL_BEAN> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_MATERIAL_BEAN>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisMaterialBeanGet(param).GetByIds(Ids));
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
