using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBornResult
{
    partial class HisBornResultGet : BusinessBase
    {
        internal HisBornResultGet()
            : base()
        {

        }

        internal HisBornResultGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_BORN_RESULT> Get(HisBornResultFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBornResultDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BORN_RESULT GetById(long id)
        {
            try
            {
                return GetById(id, new HisBornResultFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BORN_RESULT GetById(long id, HisBornResultFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBornResultDAO.GetById(id, filter.Query());
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
