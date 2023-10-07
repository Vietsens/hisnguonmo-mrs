using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBed
{
    partial class HisBedGet : BusinessBase
    {
        internal HisBedGet()
            : base()
        {

        }

        internal HisBedGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_BED> Get(HisBedFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBedDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BED GetById(long id)
        {
            try
            {
                return GetById(id, new HisBedFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BED GetById(long id, HisBedFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBedDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_BED> GetByHisBedTypeId(long id)
        {
            try
            {
                HisBedFilterQuery filter = new HisBedFilterQuery();
                filter.BED_TYPE_ID = id;
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
