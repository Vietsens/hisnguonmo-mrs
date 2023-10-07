using SAR.DAO.Base;
using SAR.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Linq;

namespace SAR.DAO.SarReport
{
    partial class SarReportCheck : EntityBase
    {
        public SarReportCheck()
            : base()
        {
            bridgeDAO = new BridgeDAO<SAR_REPORT>();
        }

        private BridgeDAO<SAR_REPORT> bridgeDAO;

        public bool IsUnLock(long id)
        {
            return bridgeDAO.IsUnLock(id);
        }
    }
}
