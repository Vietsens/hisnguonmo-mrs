using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRemuneration
{
    partial class HisRemunerationGet : BusinessBase
    {
        internal HisRemunerationGet()
            : base()
        {

        }

        internal HisRemunerationGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_REMUNERATION> Get(HisRemunerationFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRemunerationDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REMUNERATION GetById(long id)
        {
            try
            {
                return GetById(id, new HisRemunerationFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REMUNERATION GetById(long id, HisRemunerationFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRemunerationDAO.GetById(id, filter.Query());
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
