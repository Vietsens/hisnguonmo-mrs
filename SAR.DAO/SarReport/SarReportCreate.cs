using SAR.DAO.Base;
using SAR.EFMODEL.DataModels;
using Inventec.Core;
using System.Collections.Generic;

namespace SAR.DAO.SarReport
{
    partial class SarReportCreate : EntityBase
    {
        public SarReportCreate()
            : base()
        {
            bridgeDAO = new BridgeDAO<SAR_REPORT>();
        }

        private BridgeDAO<SAR_REPORT> bridgeDAO;

        public bool Create(SAR_REPORT data)
        {
            return IsNotNull(data) && bridgeDAO.Create(data);
        }

        public bool CreateList(List<SAR_REPORT> listData)
        {
            return IsNotNull(listData) && bridgeDAO.CreateList(listData);
        }
    }
}
