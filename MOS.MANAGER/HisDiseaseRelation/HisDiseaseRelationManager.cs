using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDiseaseRelation
{
    public partial class HisDiseaseRelationManager : BusinessBase
    {
        public HisDiseaseRelationManager()
            : base()
        {

        }

        public HisDiseaseRelationManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_DISEASE_RELATION> Get(HisDiseaseRelationFilterQuery filter)
        {
             List<HIS_DISEASE_RELATION> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_DISEASE_RELATION> resultData = null;
                if (valid)
                {
                    resultData = new HisDiseaseRelationGet(param).Get(filter);
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

        
        public  HIS_DISEASE_RELATION GetById(long data)
        {
             HIS_DISEASE_RELATION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_DISEASE_RELATION resultData = null;
                if (valid)
                {
                    resultData = new HisDiseaseRelationGet(param).GetById(data);
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

        
        public  HIS_DISEASE_RELATION GetById(long data, HisDiseaseRelationFilterQuery filter)
        {
             HIS_DISEASE_RELATION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_DISEASE_RELATION resultData = null;
                if (valid)
                {
                    resultData = new HisDiseaseRelationGet(param).GetById(data, filter);
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

        
        public  HIS_DISEASE_RELATION GetByCode(string data)
        {
             HIS_DISEASE_RELATION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_DISEASE_RELATION resultData = null;
                if (valid)
                {
                    resultData = new HisDiseaseRelationGet(param).GetByCode(data);
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

        
        public  HIS_DISEASE_RELATION GetByCode(string data, HisDiseaseRelationFilterQuery filter)
        {
             HIS_DISEASE_RELATION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_DISEASE_RELATION resultData = null;
                if (valid)
                {
                    resultData = new HisDiseaseRelationGet(param).GetByCode(data, filter);
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
