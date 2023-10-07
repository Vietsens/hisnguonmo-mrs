using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisProgram
{
    partial class HisProgramGet : BusinessBase
    {
        internal HisProgramGet()
            : base()
        {

        }

        internal HisProgramGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_PROGRAM> Get(HisProgramFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisProgramDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PROGRAM GetById(long id)
        {
            try
            {
                return GetById(id, new HisProgramFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PROGRAM GetById(long id, HisProgramFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisProgramDAO.GetById(id, filter.Query());
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
