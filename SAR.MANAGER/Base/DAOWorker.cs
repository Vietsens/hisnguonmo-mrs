using Inventec.Common.Repository;

namespace SAR.MANAGER.Base
{
    static class DAOWorker
    {
        internal static SAR.DAO.SarReport.SarReportDAO SarReportDAO { get { return (SAR.DAO.SarReport.SarReportDAO)Worker.Get<SAR.DAO.SarReport.SarReportDAO>(); } }
        internal static SAR.DAO.SarReportStt.SarReportSttDAO SarReportSttDAO { get { return (SAR.DAO.SarReportStt.SarReportSttDAO)Worker.Get<SAR.DAO.SarReportStt.SarReportSttDAO>(); } }
        internal static SAR.DAO.SarReportTemplate.SarReportTemplateDAO SarReportTemplateDAO { get { return (SAR.DAO.SarReportTemplate.SarReportTemplateDAO)Worker.Get<SAR.DAO.SarReportTemplate.SarReportTemplateDAO>(); } }
        internal static SAR.DAO.SarReportType.SarReportTypeDAO SarReportTypeDAO { get { return (SAR.DAO.SarReportType.SarReportTypeDAO)Worker.Get<SAR.DAO.SarReportType.SarReportTypeDAO>(); } }
    }
}
