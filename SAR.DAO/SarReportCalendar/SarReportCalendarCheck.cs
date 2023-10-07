using SAR.DAO.Base;
using SAR.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Linq;

namespace SAR.DAO.SarReportCalendar
{
    partial class SarReportCalendarCheck : EntityBase
    {
        public SarReportCalendarCheck()
            : base()
        {
            bridgeDAO = new BridgeDAO<SAR_REPORT_CALENDAR>();
        }

        private BridgeDAO<SAR_REPORT_CALENDAR> bridgeDAO;

        public bool IsUnLock(long id)
        {
            return bridgeDAO.IsUnLock(id);
        }
    }
}
