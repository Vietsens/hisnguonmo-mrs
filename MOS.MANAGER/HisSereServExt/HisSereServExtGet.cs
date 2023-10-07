using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServExt
{
    partial class HisSereServExtGet : BusinessBase
    {
        internal HisSereServExtGet()
            : base()
        {

        }

        internal HisSereServExtGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERE_SERV_EXT> Get(HisSereServExtFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServExtDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERE_SERV_EXT GetById(long id)
        {
            try
            {
                return GetById(id, new HisSereServExtFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERE_SERV_EXT GetById(long id, HisSereServExtFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServExtDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERE_SERV_EXT> GetBySereServIds(List<long> sereServIds)
        {
            if (sereServIds != null)
            {
                HisSereServExtFilterQuery filter = new HisSereServExtFilterQuery();
                filter.SERE_SERV_IDs = sereServIds;
                return this.Get(filter);
            }
            return null;
        }

        internal HIS_SERE_SERV_EXT GetBySereServId(long currentSereServId)
        {
            HisSereServExtFilterQuery filter = new HisSereServExtFilterQuery();
            filter.SERE_SERV_ID = currentSereServId;
            List<HIS_SERE_SERV_EXT> sereServExts = this.Get(filter);
            return sereServExts != null ? sereServExts[0] : null;
        }
    }
}
