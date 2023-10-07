using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRehaTrainUnit
{
    class HisRehaTrainUnitGet : GetBase
    {
        internal HisRehaTrainUnitGet()
            : base()
        {

        }

        internal HisRehaTrainUnitGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_REHA_TRAIN_UNIT> Get(HisRehaTrainUnitFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRehaTrainUnitDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REHA_TRAIN_UNIT GetById(long id)
        {
            try
            {
                return GetById(id, new HisRehaTrainUnitFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REHA_TRAIN_UNIT GetById(long id, HisRehaTrainUnitFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRehaTrainUnitDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REHA_TRAIN_UNIT GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisRehaTrainUnitFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REHA_TRAIN_UNIT GetByCode(string code, HisRehaTrainUnitFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRehaTrainUnitDAO.GetByCode(code, filter.Query());
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
