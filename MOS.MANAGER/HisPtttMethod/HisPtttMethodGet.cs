using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPtttMethod
{
    partial class HisPtttMethodGet : BusinessBase
    {
        internal HisPtttMethodGet()
            : base()
        {

        }

        internal HisPtttMethodGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_PTTT_METHOD> Get(HisPtttMethodFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPtttMethodDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PTTT_METHOD GetById(long id)
        {
            try
            {
                return GetById(id, new HisPtttMethodFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PTTT_METHOD GetById(long id, HisPtttMethodFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPtttMethodDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PTTT_METHOD GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisPtttMethodFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PTTT_METHOD GetByCode(string code, HisPtttMethodFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPtttMethodDAO.GetByCode(code, filter.Query());
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
