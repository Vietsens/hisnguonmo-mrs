using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPtttTable
{
    partial class HisPtttTableGet : BusinessBase
    {
        internal HisPtttTableGet()
            : base()
        {

        }

        internal HisPtttTableGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_PTTT_TABLE> Get(HisPtttTableFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPtttTableDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PTTT_TABLE GetById(long id)
        {
            try
            {
                return GetById(id, new HisPtttTableFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PTTT_TABLE GetById(long id, HisPtttTableFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPtttTableDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PTTT_TABLE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisPtttTableFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PTTT_TABLE GetByCode(string code, HisPtttTableFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPtttTableDAO.GetByCode(code, filter.Query());
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
