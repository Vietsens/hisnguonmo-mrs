using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.MANAGER.HisDepartmentTran
{
    partial class HisDepartmentTranGet : BusinessBase
    {
        internal HisDepartmentTranGet()
            : base()
        {

        }

        internal HisDepartmentTranGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_DEPARTMENT_TRAN> Get(HisDepartmentTranFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDepartmentTranDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_DEPARTMENT_TRAN> GetView(HisDepartmentTranViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDepartmentTranDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DEPARTMENT_TRAN GetById(long id)
        {
            try
            {
                return GetById(id, new HisDepartmentTranFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DEPARTMENT_TRAN GetById(long id, HisDepartmentTranFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDepartmentTranDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_DEPARTMENT_TRAN GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisDepartmentTranViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_DEPARTMENT_TRAN> GetViewByTreatmentIds(List<long> treatmentIds)
        {
            if (IsNotNullOrEmpty(treatmentIds))
            {
                HisDepartmentTranViewFilterQuery filter = new HisDepartmentTranViewFilterQuery();
                filter.TREATMENT_IDs = treatmentIds;
                return this.GetView(filter);
            }
            return null;
        }

        /// <summary>
        /// Ban ghi chuyen khoa moi nhat theo treatment_id va truoc thoi gian "log_time" truyen vao
        /// </summary>
        /// <param name="treatmentId"></param>
        /// <returns></returns>
        internal HIS_DEPARTMENT_TRAN GetLastByTreatmentId(long treatmentId, long? beforeLogTime)
        {
            try
            {
                HisDepartmentTranFilterQuery filter = new HisDepartmentTranFilterQuery();
                filter.TREATMENT_ID = treatmentId;
                filter.DEPARTMENT_IN_TIME_TO = beforeLogTime;
                List<HIS_DEPARTMENT_TRAN> dts = this.Get(filter);
                return dts.OrderByDescending(o => o.CREATE_TIME).ThenByDescending(o => o.DEPARTMENT_IN_TIME).ThenByDescending(o => o.ID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        /// <summary>
        /// Ban ghi chuyen khoa moi nhat theo treatment_id va truoc thoi gian "log_time" truyen vao
        /// </summary>
        /// <param name="treatmentId"></param>
        /// <returns></returns>
        internal V_HIS_DEPARTMENT_TRAN GetViewLastByTreatmentId(long treatmentId, long? beforeLogTime)
        {
            try
            {
                HisDepartmentTranViewFilterQuery filter = new HisDepartmentTranViewFilterQuery();
                filter.TREATMENT_ID = treatmentId;
                filter.DEPARTMENT_IN_TIME_TO = beforeLogTime;
                List<V_HIS_DEPARTMENT_TRAN> vHisDepartmentTrans = this.GetView(filter);
                return vHisDepartmentTrans.OrderByDescending(o => o.DEPARTMENT_IN_TIME).ThenByDescending(o => o.ID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        /// <summary>
        /// Ban ghi chuyen khoa moi nhat theo treatment_id
        /// </summary>
        /// <param name="treatmentId"></param>
        /// <returns></returns>
        internal HIS_DEPARTMENT_TRAN GetLastByTreatmentId(long treatmentId)
        {
            return this.GetLastByTreatmentId(treatmentId, null);
        }

        /// <summary>
        /// Ban ghi chuyen khoa moi nhat theo treatment_id
        /// </summary>
        /// <param name="treatmentId"></param>
        /// <returns></returns>
        internal V_HIS_DEPARTMENT_TRAN GetViewLastByTreatmentId(long treatmentId)
        {
            return this.GetViewLastByTreatmentId(treatmentId, null);
        }

        /// <summary>
        /// Ban ghi chuyen khoa moi nhat theo treatment_id trong danh sach departmentTran truyen vao
        /// </summary>
        /// <param name="treatmentId"></param>
        /// <returns></returns>
        internal V_HIS_DEPARTMENT_TRAN GetViewLastByTreatmentId(List<V_HIS_DEPARTMENT_TRAN> departmentTrans, long treatmentId)
        {
            try
            {
                if (IsNotNullOrEmpty(departmentTrans))
                {
                    return departmentTrans.Where(o => o.TREATMENT_ID == treatmentId).OrderByDescending(o => o.DEPARTMENT_IN_TIME).ThenByDescending(o => o.ID).FirstOrDefault();
                }
                return null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        /// <summary>
        /// Ban ghi nhap khoa dau tien theo treatment_id
        /// </summary>
        /// <param name="treatmentId"></param>
        /// <returns></returns>
        internal V_HIS_DEPARTMENT_TRAN GetViewFirstByTreatmentId(long treatmentId)
        {
            try
            {
                HisDepartmentTranViewFilterQuery filter = new HisDepartmentTranViewFilterQuery();
                filter.TREATMENT_ID = treatmentId;
                List<V_HIS_DEPARTMENT_TRAN> vHisDepartmentTrans = this.GetView(filter);
                if (IsNotNullOrEmpty(vHisDepartmentTrans))
                {
                    return vHisDepartmentTrans
                        .OrderBy(o => o.DEPARTMENT_IN_TIME)
                        .ThenBy(o => o.ID).FirstOrDefault();
                }
                return null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        /// <summary>
        /// Ban ghi nhap khoa dau tien theo treatment_id
        /// </summary>
        /// <param name="treatmentId"></param>
        /// <returns></returns>
        internal HIS_DEPARTMENT_TRAN GetFirstByTreatmentId(long treatmentId)
        {
            try
            {
                HisDepartmentTranFilterQuery filter = new HisDepartmentTranFilterQuery();
                filter.TREATMENT_ID = treatmentId;
                List<HIS_DEPARTMENT_TRAN> hisDepartmentTrans = this.Get(filter);
                if (IsNotNullOrEmpty(hisDepartmentTrans))
                {
                    return hisDepartmentTrans
                        .OrderBy(o => o.DEPARTMENT_IN_TIME)
                        .ThenBy(o => o.ID).FirstOrDefault();
                }
                return null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_DEPARTMENT_TRAN GetViewById(long id, HisDepartmentTranViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDepartmentTranDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_DEPARTMENT_TRAN> GetViewByDepartmentId(long departmentId)
        {
            try
            {
                HisDepartmentTranViewFilterQuery filter = new HisDepartmentTranViewFilterQuery();
                filter.DEPARTMENT_ID = departmentId;
                return this.GetView(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_DEPARTMENT_TRAN> GetViewByTreatmentId(long treatmentId)
        {
            try
            {
                HisDepartmentTranViewFilterQuery filter = new HisDepartmentTranViewFilterQuery();
                filter.TREATMENT_ID = treatmentId;
                return this.GetView(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_DEPARTMENT_TRAN> GetByTreatmentId(long treatmentId)
        {
            try
            {
                HisDepartmentTranFilterQuery filter = new HisDepartmentTranFilterQuery();
                filter.TREATMENT_ID = treatmentId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_DEPARTMENT_TRAN> GetByDepartmentId(long departmentId)
        {
            try
            {
                HisDepartmentTranFilterQuery filter = new HisDepartmentTranFilterQuery();
                filter.DEPARTMENT_ID = departmentId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
