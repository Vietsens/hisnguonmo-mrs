using SAR.DAO.Base;
using SAR.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAR.DAO.SarReport
{
    partial class SarReportUpdate : EntityBase
    {
        public SarReportUpdate()
            : base()
        {
            bridgeDAO = new BridgeDAO<SAR_REPORT>();
        }

        private BridgeDAO<SAR_REPORT> bridgeDAO;

        public bool Update(SAR_REPORT data)
        {
            return IsNotNull(data) && IsGreaterThanZero(data.ID) && bridgeDAO.Update(data);
        }

        public bool UpdateList(List<SAR_REPORT> listData)
        {
            return IsNotNull(listData) && bridgeDAO.UpdateList(listData);
        }
    }
}
