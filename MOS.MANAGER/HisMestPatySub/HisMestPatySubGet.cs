using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPatySub
{
    partial class HisMestPatySubGet : BusinessBase
    {
        internal HisMestPatySubGet()
            : base()
        {

        }

        internal HisMestPatySubGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEST_PATY_SUB> Get(HisMestPatySubFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPatySubDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_PATY_SUB GetById(long id)
        {
            try
            {
                return GetById(id, new HisMestPatySubFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_PATY_SUB GetById(long id, HisMestPatySubFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPatySubDAO.GetById(id, filter.Query());
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
