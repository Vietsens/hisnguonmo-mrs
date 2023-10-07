using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPtttCondition
{
    partial class HisPtttConditionGet : BusinessBase
    {
        internal HisPtttConditionGet()
            : base()
        {

        }

        internal HisPtttConditionGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_PTTT_CONDITION> Get(HisPtttConditionFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPtttConditionDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PTTT_CONDITION GetById(long id)
        {
            try
            {
                return GetById(id, new HisPtttConditionFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PTTT_CONDITION GetById(long id, HisPtttConditionFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPtttConditionDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PTTT_CONDITION GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisPtttConditionFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PTTT_CONDITION GetByCode(string code, HisPtttConditionFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPtttConditionDAO.GetByCode(code, filter.Query());
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
