using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMilitaryRank
{
    partial class HisMilitaryRankGet : BusinessBase
    {
        internal HisMilitaryRankGet()
            : base()
        {

        }

        internal HisMilitaryRankGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MILITARY_RANK> Get(HisMilitaryRankFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMilitaryRankDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MILITARY_RANK GetById(long id)
        {
            try
            {
                return GetById(id, new HisMilitaryRankFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MILITARY_RANK GetById(long id, HisMilitaryRankFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMilitaryRankDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MILITARY_RANK GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisMilitaryRankFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MILITARY_RANK GetByCode(string code, HisMilitaryRankFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMilitaryRankDAO.GetByCode(code, filter.Query());
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
