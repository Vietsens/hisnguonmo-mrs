using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRehaTrainType
{
    partial class HisRehaTrainTypeGet : BusinessBase
    {
        internal HisRehaTrainTypeGet()
            : base()
        {

        }

        internal HisRehaTrainTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_REHA_TRAIN_TYPE> Get(HisRehaTrainTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRehaTrainTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_REHA_TRAIN_TYPE> GetView(HisRehaTrainTypeViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRehaTrainTypeDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REHA_TRAIN_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisRehaTrainTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REHA_TRAIN_TYPE GetById(long id, HisRehaTrainTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRehaTrainTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REHA_TRAIN_TYPE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisRehaTrainTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REHA_TRAIN_TYPE GetByCode(string code, HisRehaTrainTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRehaTrainTypeDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_REHA_TRAIN_TYPE> GetByRehaTrainUnitId(long rehaTrainUnitId)
        {
            try
            {
                HisRehaTrainTypeFilterQuery filter = new HisRehaTrainTypeFilterQuery();
                filter.REHA_TRAIN_UNIT_ID = rehaTrainUnitId;
                return this.Get(filter);
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
