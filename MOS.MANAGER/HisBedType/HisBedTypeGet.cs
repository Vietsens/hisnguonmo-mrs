using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBedType
{
    partial class HisBedTypeGet : BusinessBase
    {
        internal HisBedTypeGet()
            : base()
        {

        }

        internal HisBedTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_BED_TYPE> Get(HisBedTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBedTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BED_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisBedTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BED_TYPE GetById(long id, HisBedTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBedTypeDAO.GetById(id, filter.Query());
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
