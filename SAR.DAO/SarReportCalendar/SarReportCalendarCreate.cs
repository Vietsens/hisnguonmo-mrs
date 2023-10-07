using SAR.DAO.Base;
using SAR.EFMODEL.DataModels;
using Inventec.Core;
using System.Collections.Generic;

namespace SAR.DAO.SarReportCalendar
{
    partial class SarReportCalendarCreate : EntityBase
    {
        public SarReportCalendarCreate()
            : base()
        {
            bridgeDAO = new BridgeDAO<SAR_REPORT_CALENDAR>();
        }

        private BridgeDAO<SAR_REPORT_CALENDAR> bridgeDAO;

        public bool Create(SAR_REPORT_CALENDAR data)
        {
            return IsNotNull(data) && bridgeDAO.Create(data);
        }

        public bool CreateList(List<SAR_REPORT_CALENDAR> listData)
        {
            return IsNotNull(listData) && bridgeDAO.CreateList(listData);
        }
    }
}
