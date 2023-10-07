using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPaanPosition
{
    partial class HisPaanPositionGet : BusinessBase
    {
        internal HisPaanPositionGet()
            : base()
        {

        }

        internal HisPaanPositionGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_PAAN_POSITION> Get(HisPaanPositionFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPaanPositionDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PAAN_POSITION GetById(long id)
        {
            try
            {
                return GetById(id, new HisPaanPositionFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PAAN_POSITION GetById(long id, HisPaanPositionFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPaanPositionDAO.GetById(id, filter.Query());
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
