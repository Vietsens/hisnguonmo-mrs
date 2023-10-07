using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestBlood
{
    public partial class HisExpMestBloodManager : BusinessBase
    {
        public HisExpMestBloodManager()
            : base()
        {

        }

        public HisExpMestBloodManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_EXP_MEST_BLOOD> Get(HisExpMestBloodFilterQuery filter)
        {
            List<HIS_EXP_MEST_BLOOD> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EXP_MEST_BLOOD> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestBloodGet(param).Get(filter);
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

        
        public HIS_EXP_MEST_BLOOD GetById(long data)
        {
            HIS_EXP_MEST_BLOOD result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EXP_MEST_BLOOD resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestBloodGet(param).GetById(data);
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

        
        public HIS_EXP_MEST_BLOOD GetById(long data, HisExpMestBloodFilterQuery filter)
        {
            HIS_EXP_MEST_BLOOD result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EXP_MEST_BLOOD resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestBloodGet(param).GetById(data, filter);
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

        
        public List<HIS_EXP_MEST_BLOOD> GetByExpMestId(long data)
        {
            List<HIS_EXP_MEST_BLOOD> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_EXP_MEST_BLOOD> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestBloodGet(param).GetByExpMestId(data);
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

        
        public List<HIS_EXP_MEST_BLOOD> GetExportedByExpMestId(long data)
        {
            List<HIS_EXP_MEST_BLOOD> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_EXP_MEST_BLOOD> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestBloodGet(param).GetExportedByExpMestId(data);
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

        
        public List<HIS_EXP_MEST_BLOOD> GetUnexportByExpMestId(long data)
        {
            List<HIS_EXP_MEST_BLOOD> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_EXP_MEST_BLOOD> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestBloodGet(param).GetUnexportByExpMestId(data);
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

        
        public List<HIS_EXP_MEST_BLOOD> GetByBloodId(long data)
        {
            List<HIS_EXP_MEST_BLOOD> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_EXP_MEST_BLOOD> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestBloodGet(param).GetByBloodId(data);
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
