using SAR.DAO.StagingObject;
using SAR.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SAR.DAO.SarReportTemplate
{
    public partial class SarReportTemplateDAO : EntityBase
    {
        private SarReportTemplateGet GetWorker
        {
            get
            {
                return (SarReportTemplateGet)Worker.Get<SarReportTemplateGet>();
            }
        }

        public List<SAR_REPORT_TEMPLATE> Get(SarReportTemplateSO search, CommonParam param)
        {
            List<SAR_REPORT_TEMPLATE> result = new List<SAR_REPORT_TEMPLATE>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public SAR_REPORT_TEMPLATE GetById(long id, SarReportTemplateSO search)
        {
            SAR_REPORT_TEMPLATE result = null;
            try
            {
                result = GetWorker.GetById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
