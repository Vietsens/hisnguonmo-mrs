using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPtttGroup
{
    partial class HisPtttGroupGet : BusinessBase
    {
        internal HisPtttGroupGet()
            : base()
        {

        }

        internal HisPtttGroupGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_PTTT_GROUP> Get(HisPtttGroupFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPtttGroupDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PTTT_GROUP GetById(long id)
        {
            try
            {
                return GetById(id, new HisPtttGroupFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PTTT_GROUP GetById(long id, HisPtttGroupFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPtttGroupDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PTTT_GROUP GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisPtttGroupFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PTTT_GROUP GetByCode(string code, HisPtttGroupFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPtttGroupDAO.GetByCode(code, filter.Query());
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
