using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestMetyDepa
{
    partial class HisMestMetyDepaGet : BusinessBase
    {
        internal HisMestMetyDepaGet()
            : base()
        {

        }

        internal HisMestMetyDepaGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEST_METY_DEPA> Get(HisMestMetyDepaFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestMetyDepaDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_METY_DEPA GetById(long id)
        {
            try
            {
                return GetById(id, new HisMestMetyDepaFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_METY_DEPA GetById(long id, HisMestMetyDepaFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestMetyDepaDAO.GetById(id, filter.Query());
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
