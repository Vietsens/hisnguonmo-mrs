using SAR.DAO.StagingObject;
using SAR.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SAR.DAO.SarReport
{
    public partial class SarReportDAO : EntityBase
    {
        private SarReportCreate CreateWorker
        {
            get
            {
                return (SarReportCreate)Worker.Get<SarReportCreate>();
            }
        }
        private SarReportUpdate UpdateWorker
        {
            get
            {
                return (SarReportUpdate)Worker.Get<SarReportUpdate>();
            }
        }
        private SarReportGet GetWorker
        {
            get
            {
                return (SarReportGet)Worker.Get<SarReportGet>();
            }
        }
        private SarReportCheck CheckWorker
        {
            get
            {
                return (SarReportCheck)Worker.Get<SarReportCheck>();
            }
        }

        public bool Create(SAR_REPORT data)
        {
            bool result = false;
            try
            {
                result = CreateWorker.Create(data);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        public bool CreateList(List<SAR_REPORT> listData)
        {
            bool result = false;
            try
            {
                result = CreateWorker.CreateList(listData);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        public bool Update(SAR_REPORT data)
        {
            bool result = false;
            try
            {
                result = UpdateWorker.Update(data);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        public bool UpdateList(List<SAR_REPORT> listData)
        {
            bool result = false;
            try
            {
                result = UpdateWorker.UpdateList(listData);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        public List<SAR_REPORT> Get(SarReportSO search, CommonParam param)
        {
            List<SAR_REPORT> result = new List<SAR_REPORT>();
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

        public SAR_REPORT GetById(long id, SarReportSO search)
        {
            SAR_REPORT result = null;
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

        public bool IsUnLock(long id)
        {
            try
            {
                return CheckWorker.IsUnLock(id);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                throw;
            }
        }
    }
}
