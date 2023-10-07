using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServMaty
{
    partial class HisSereServMatyGet : BusinessBase
    {
        internal HisSereServMatyGet()
            : base()
        {

        }

        internal HisSereServMatyGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERE_SERV_MATY> Get(HisSereServMatyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServMatyDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERE_SERV_MATY GetById(long id)
        {
            try
            {
                return GetById(id, new HisSereServMatyFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERE_SERV_MATY GetById(long id, HisSereServMatyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServMatyDAO.GetById(id, filter.Query());
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
