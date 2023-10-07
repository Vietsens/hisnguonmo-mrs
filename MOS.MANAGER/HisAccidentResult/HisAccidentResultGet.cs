using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentResult
{
    partial class HisAccidentResultGet : BusinessBase
    {
        internal HisAccidentResultGet()
            : base()
        {

        }

        internal HisAccidentResultGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_ACCIDENT_RESULT> Get(HisAccidentResultFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentResultDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_RESULT GetById(long id)
        {
            try
            {
                return GetById(id, new HisAccidentResultFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_RESULT GetById(long id, HisAccidentResultFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentResultDAO.GetById(id, filter.Query());
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
