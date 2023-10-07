using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisReportTypeCat
{
    partial class HisReportTypeCatGet : BusinessBase
    {
        internal HisReportTypeCatGet()
            : base()
        {

        }

        internal HisReportTypeCatGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_REPORT_TYPE_CAT> Get(HisReportTypeCatFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisReportTypeCatDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REPORT_TYPE_CAT GetById(long id)
        {
            try
            {
                return GetById(id, new HisReportTypeCatFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REPORT_TYPE_CAT GetById(long id, HisReportTypeCatFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisReportTypeCatDAO.GetById(id, filter.Query());
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
