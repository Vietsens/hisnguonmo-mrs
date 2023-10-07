using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExamServiceTemp
{
    partial class HisExamServiceTempGet : BusinessBase
    {
        internal HisExamServiceTempGet()
            : base()
        {

        }

        internal HisExamServiceTempGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EXAM_SERVICE_TEMP> Get(HisExamServiceTempFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExamServiceTempDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXAM_SERVICE_TEMP GetById(long id)
        {
            try
            {
                return GetById(id, new HisExamServiceTempFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXAM_SERVICE_TEMP GetById(long id, HisExamServiceTempFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExamServiceTempDAO.GetById(id, filter.Query());
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
