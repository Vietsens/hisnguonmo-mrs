using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineTypeTut
{
    partial class HisMedicineTypeTutGet : BusinessBase
    {
        internal HisMedicineTypeTutGet()
            : base()
        {

        }

        internal HisMedicineTypeTutGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEDICINE_TYPE_TUT> Get(HisMedicineTypeTutFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineTypeTutDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDICINE_TYPE_TUT GetById(long id)
        {
            try
            {
                return GetById(id, new HisMedicineTypeTutFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDICINE_TYPE_TUT GetById(long id, HisMedicineTypeTutFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineTypeTutDAO.GetById(id, filter.Query());
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
