using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestMaterial
{
    public partial class HisImpMestMaterialManager : BusinessBase
    {
        public HisImpMestMaterialManager()
            : base()
        {

        }

        public HisImpMestMaterialManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_IMP_MEST_MATERIAL> Get(HisImpMestMaterialFilterQuery filter)
        {
            List<HIS_IMP_MEST_MATERIAL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_IMP_MEST_MATERIAL> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMaterialGet(param).Get(filter);
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

        
        public HIS_IMP_MEST_MATERIAL GetById(long data)
        {
            HIS_IMP_MEST_MATERIAL result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_IMP_MEST_MATERIAL resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMaterialGet(param).GetById(data);
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

        
        public HIS_IMP_MEST_MATERIAL GetById(long data, HisImpMestMaterialFilterQuery filter)
        {
            HIS_IMP_MEST_MATERIAL result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_IMP_MEST_MATERIAL resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMaterialGet(param).GetById(data, filter);
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

        
        public List<HIS_IMP_MEST_MATERIAL> GetByImpMestId(long data)
        {
            List<HIS_IMP_MEST_MATERIAL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_IMP_MEST_MATERIAL> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMaterialGet(param).GetByImpMestId(data);
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

        
        public List<HIS_IMP_MEST_MATERIAL> GetByImpMestIds(List<long> data)
        {
            List<HIS_IMP_MEST_MATERIAL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_IMP_MEST_MATERIAL> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_IMP_MEST_MATERIAL>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisImpMestMaterialGet(param).GetByImpMestIds(Ids));
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

        
        public List<HIS_IMP_MEST_MATERIAL> GetByMaterialId(long data)
        {
            List<HIS_IMP_MEST_MATERIAL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_IMP_MEST_MATERIAL> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMaterialGet(param).GetByMaterialId(data);
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
