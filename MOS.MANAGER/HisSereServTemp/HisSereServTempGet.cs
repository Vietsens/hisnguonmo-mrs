using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServTemp
{
    partial class HisSereServTempGet : BusinessBase
    {
        internal HisSereServTempGet()
            : base()
        {

        }

        internal HisSereServTempGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERE_SERV_TEMP> Get(HisSereServTempFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServTempDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERE_SERV_TEMP GetById(long id)
        {
            try
            {
                return GetById(id, new HisSereServTempFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERE_SERV_TEMP GetById(long id, HisSereServTempFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServTempDAO.GetById(id, filter.Query());
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
