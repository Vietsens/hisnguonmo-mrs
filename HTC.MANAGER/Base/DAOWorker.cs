using Inventec.Common.Repository;

namespace HTC.MANAGER.Base
{
    static class DAOWorker
    {
        internal static HTC.DAO.HtcExpense.HtcExpenseDAO HtcExpenseDAO { get { return (HTC.DAO.HtcExpense.HtcExpenseDAO)Worker.Get<HTC.DAO.HtcExpense.HtcExpenseDAO>(); } }
        internal static HTC.DAO.HtcExpenseType.HtcExpenseTypeDAO HtcExpenseTypeDAO { get { return (HTC.DAO.HtcExpenseType.HtcExpenseTypeDAO)Worker.Get<HTC.DAO.HtcExpenseType.HtcExpenseTypeDAO>(); } }
        internal static HTC.DAO.HtcPeriod.HtcPeriodDAO HtcPeriodDAO { get { return (HTC.DAO.HtcPeriod.HtcPeriodDAO)Worker.Get<HTC.DAO.HtcPeriod.HtcPeriodDAO>(); } }
        internal static HTC.DAO.HtcPeriodDepartment.HtcPeriodDepartmentDAO HtcPeriodDepartmentDAO { get { return (HTC.DAO.HtcPeriodDepartment.HtcPeriodDepartmentDAO)Worker.Get<HTC.DAO.HtcPeriodDepartment.HtcPeriodDepartmentDAO>(); } }
        internal static HTC.DAO.HtcRevenue.HtcRevenueDAO HtcRevenueDAO { get { return (HTC.DAO.HtcRevenue.HtcRevenueDAO)Worker.Get<HTC.DAO.HtcRevenue.HtcRevenueDAO>(); } }

    }
}
