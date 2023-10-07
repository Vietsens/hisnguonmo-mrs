using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEkip
{
    partial class HisEkipGet : BusinessBase
    {
        internal HisEkipGet()
            : base()
        {

        }

        internal HisEkipGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EKIP> Get(HisEkipFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEkipDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EKIP GetById(long id)
        {
            try
            {
                return GetById(id, new HisEkipFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EKIP GetById(long id, HisEkipFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEkipDAO.GetById(id, filter.Query());
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
