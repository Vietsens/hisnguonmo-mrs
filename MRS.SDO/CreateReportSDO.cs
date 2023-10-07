using System;

namespace MRS.SDO
{
    public class CreateReportSDO
    {
        public string ReportTypeCode { get; set; }
        public string ReportTemplateCode { get; set; }
        public string Loginname { get; set; }
        public string Username { get; set; }
        public string ReportName { get; set; }
        public string Description { get; set; }
        public object Filter { get; set; }
        public string ListKeyAllow { get; set; }

        public long BranchId { get; set; }

        public string REPORTER_IP { get; set; }
    }
}
