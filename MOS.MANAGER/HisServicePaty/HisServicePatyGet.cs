using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MOS.MANAGER.HisServicePaty
{
    class HisServicePatyGet : GetBase
    {
        internal HisServicePatyGet()
            : base()
        {

        }

        internal HisServicePatyGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERVICE_PATY> Get(HisServicePatyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServicePatyDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        /// <summary>
        /// Lay danh sach chinh sach gia dua vao danh sach service_id truyen vao
        /// </summary>
        /// <param name="serviceIds"></param>
        /// <returns></returns>
        internal List<HIS_SERVICE_PATY> GetByServiceIds(List<long> serviceIds)
        {
            try
            {
                List<HIS_SERVICE_PATY> result = null;
                if (IsNotNullOrEmpty(serviceIds))
                {
                    HisServicePatyFilterQuery filter = new HisServicePatyFilterQuery();
                    filter.SERVICE_IDs = serviceIds;
                    filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                    result = this.Get(filter);
                }
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERVICE_PATY GetApplied(long executeBranchId, long? executeRoomId, long? requestRoomId, long? requestDepartmentId, List<V_HIS_SERVICE_PATY> servicePaties, long instructionTime, long treatmentTime, long serviceId, long patientTypeId)
        {
            return ServicePatyUtil.GetApplied(servicePaties, executeBranchId, executeRoomId, requestRoomId, requestDepartmentId, instructionTime, treatmentTime, serviceId, patientTypeId, null);
        }

        internal List<V_HIS_SERVICE_PATY> GetView(HisServicePatyViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServicePatyDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        /// <summary>
        /// Lay ra du lieu chinh sach gia dang hieu luc tuong ung voi service_id va cac loai doi tuong benh nhan
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="treatmentTime"></param>
        /// <returns></returns>
        internal List<V_HIS_SERVICE_PATY> GetAppliedView(long serviceId, long? treatmentTime)
        {
            List<V_HIS_SERVICE_PATY> result = null;
            try
            {
                //HIS_BRANCH branch = new TokenManager(param).GetBranch();

                long? now = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(DateTime.Now);

                List<V_HIS_SERVICE_PATY> list = new HisServicePatyGet().GetViewActive()
                    .Where(o => (!o.FROM_TIME.HasValue || o.FROM_TIME.Value <= now) && (!o.TO_TIME.HasValue || o.TO_TIME.Value >= now) /*&& o.BRANCH_ID == branch.ID */&& o.SERVICE_ID == serviceId).ToList();

                List<long> patientTypeIds = list.Select(o => o.PATIENT_TYPE_ID).Distinct().ToList();
                if (patientTypeIds != null && patientTypeIds.Count > 0)
                {
                    result = new List<V_HIS_SERVICE_PATY>();
                    foreach (long patientTypeId in patientTypeIds)
                    {
                        List<V_HIS_SERVICE_PATY> subList = list
                            .Where(o => o.PATIENT_TYPE_ID == patientTypeId)
                            .OrderByDescending(o => o.PRIORITY).ToList();
                        result.Add(subList[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
            }
            return result;
        }

        internal HIS_SERVICE_PATY GetById(long id)
        {
            try
            {
                return GetById(id, new HisServicePatyFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_PATY GetById(long id, HisServicePatyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServicePatyDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERVICE_PATY GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisServicePatyViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERVICE_PATY GetViewById(long id, HisServicePatyViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServicePatyDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE_PATY> GetByPatientTypeId(long patientTypeId)
        {
            try
            {
                HisServicePatyFilterQuery filter = new HisServicePatyFilterQuery();
                filter.PATIENT_TYPE_ID = patientTypeId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE_PATY> GetByServiceId(long serviceId)
        {
            try
            {
                HisServicePatyFilterQuery filter = new HisServicePatyFilterQuery();
                filter.SERVICE_ID = serviceId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE_PATY> GetActive()
        {
            HisServicePatyFilterQuery filter = new HisServicePatyFilterQuery();
            filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
            return this.Get(filter);
        }

        internal List<V_HIS_SERVICE_PATY> GetViewActive()
        {
            HisServicePatyViewFilterQuery filter = new HisServicePatyViewFilterQuery();
            filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
            return this.GetView(filter);
        }

        internal List<HIS_SERVICE_PATY> GetByBranchId(long id)
        {
            try
            {
                HisServicePatyFilterQuery filter = new HisServicePatyFilterQuery();
                filter.BRANCH_ID = id;
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
