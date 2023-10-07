using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentPoison
{
    partial class HisAccidentPoisonGet : BusinessBase
    {
        internal HisAccidentPoisonGet()
            : base()
        {

        }

        internal HisAccidentPoisonGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_ACCIDENT_POISON> Get(HisAccidentPoisonFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentPoisonDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_POISON GetById(long id)
        {
            try
            {
                return GetById(id, new HisAccidentPoisonFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_POISON GetById(long id, HisAccidentPoisonFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentPoisonDAO.GetById(id, filter.Query());
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
