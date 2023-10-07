using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MRS.Processor.Mrs00398
{
    partial class HisWorkPlaceGet : BusinessBase
    {
        internal HisWorkPlaceGet()
            : base()
        {

        }

        internal HisWorkPlaceGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_WORK_PLACE> Get(HisWorkPlaceFilterQuery filter)
        {
            try
            {
                return new MOS.DAO.Sql.SqlDAO().GetSql<HIS_WORK_PLACE>("SELECT * FROM HIS_WORK_PLACE ");
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
