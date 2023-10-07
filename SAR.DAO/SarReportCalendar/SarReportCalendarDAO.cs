using SAR.DAO.StagingObject;
using SAR.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SAR.DAO.SarReportCalendar
{
    public partial class SarReportCalendarDAO : EntityBase
    {
        private SarReportCalendarCreate CreateWorker
        {
            get
            {
                return (SarReportCalendarCreate)Worker.Get<SarReportCalendarCreate>();
            }
        }
        private SarReportCalendarUpdate UpdateWorker
        {
            get
            {
                return (SarReportCalendarUpdate)Worker.Get<SarReportCalendarUpdate>();
            }
        }
        private SarReportCalendarDelete DeleteWorker
        {
            get
            {
                return (SarReportCalendarDelete)Worker.Get<SarReportCalendarDelete>();
            }
        }
        private SarReportCalendarTruncate TruncateWorker
        {
            get
            {
                return (SarReportCalendarTruncate)Worker.Get<SarReportCalendarTruncate>();
            }
        }
        private SarReportCalendarGet GetWorker
        {
            get
            {
                return (SarReportCalendarGet)Worker.Get<SarReportCalendarGet>();
            }
        }
        private SarReportCalendarCheck CheckWorker
        {
            get
            {
                return (SarReportCalendarCheck)Worker.Get<SarReportCalendarCheck>();
            }
        }

        public bool Create(SAR_REPORT_CALENDAR data)
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

        public bool CreateList(List<SAR_REPORT_CALENDAR> listData)
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

        public bool Update(SAR_REPORT_CALENDAR data)
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

        public bool UpdateList(List<SAR_REPORT_CALENDAR> listData)
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

        public bool Delete(SAR_REPORT_CALENDAR data)
        {
            bool result = false;
            try
            {
                result = DeleteWorker.Delete(data);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        public bool DeleteList(List<SAR_REPORT_CALENDAR> listData)
        {
            bool result = false;

            try
            {
                result = DeleteWorker.DeleteList(listData);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        public bool Truncate(SAR_REPORT_CALENDAR data)
        {
            bool result = false;
            try
            {
                result = TruncateWorker.Truncate(data);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        public bool TruncateList(List<SAR_REPORT_CALENDAR> listData)
        {
            bool result = false;
            try
            {
                result = TruncateWorker.TruncateList(listData);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        public List<SAR_REPORT_CALENDAR> Get(SarReportCalendarSO search, CommonParam param)
        {
            List<SAR_REPORT_CALENDAR> result = new List<SAR_REPORT_CALENDAR>();
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

        public SAR_REPORT_CALENDAR GetById(long id, SarReportCalendarSO search)
        {
            SAR_REPORT_CALENDAR result = null;
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
