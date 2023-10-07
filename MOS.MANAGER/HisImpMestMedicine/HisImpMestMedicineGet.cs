using AutoMapper;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisMedicine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.MANAGER.HisImpMestMedicine
{
    partial class HisImpMestMedicineGet : GetBase
    {
        internal HisImpMestMedicineGet()
            : base()
        {

        }

        internal HisImpMestMedicineGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_IMP_MEST_MEDICINE> Get(HisImpMestMedicineFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestMedicineDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_IMP_MEST_MEDICINE> GetView(HisImpMestMedicineViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestMedicineDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_IMP_MEST_MEDICINE> GetViewByAggrImpMestId(long aggrImpMestId)
        {
            try
            {
                HisImpMestMedicineViewFilterQuery filter = new HisImpMestMedicineViewFilterQuery();
                filter.AGGR_IMP_MEST_ID = aggrImpMestId;
                return this.GetView(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_IMP_MEST_MEDICINE> GetViewByAggrImpMestIdAndGroupByMedicine(long aggrImpMestId)
        {
            try
            {
                List<V_HIS_IMP_MEST_MEDICINE> result = new List<V_HIS_IMP_MEST_MEDICINE>();
                List<V_HIS_IMP_MEST_MEDICINE> vHisImpMestMedicineDTOs = this.GetViewByAggrImpMestId(aggrImpMestId);
                if (IsNotNullOrEmpty(vHisImpMestMedicineDTOs))
                {
                    result = vHisImpMestMedicineDTOs
                        .GroupBy(o => new { o.MEDICINE_ID, o.MEDI_STOCK_ID })
                        .Select(sl => new V_HIS_IMP_MEST_MEDICINE
                        {
                            MEDICINE_ID = sl.Key.MEDICINE_ID,
                            MEDI_STOCK_ID = sl.Key.MEDI_STOCK_ID,
                            AMOUNT = sl.Sum(x => x.AMOUNT),
                            AGGR_IMP_MEST_ID = sl.First().AGGR_IMP_MEST_ID,
                            IMP_PRICE = sl.First().IMP_PRICE,
                            IMP_VAT_RATIO = sl.First().IMP_VAT_RATIO,
                            BID_NUMBER = sl.First().BID_NUMBER,
                            PACKAGE_NUMBER = sl.First().PACKAGE_NUMBER,
                            EXPIRED_DATE = sl.First().EXPIRED_DATE,
                            REGISTER_NUMBER = sl.First().REGISTER_NUMBER,
                            MEDICINE_TYPE_ID = sl.First().MEDICINE_TYPE_ID,
                            INTERNAL_PRICE = sl.First().INTERNAL_PRICE,
                            MEDICINE_TYPE_CODE = sl.First().MEDICINE_TYPE_CODE,
                            MEDICINE_TYPE_NAME = sl.First().MEDICINE_TYPE_NAME,
                            SERVICE_ID = sl.First().SERVICE_ID,
                            NATIONAL_NAME = sl.First().NATIONAL_NAME,
                            MANUFACTURER_ID = sl.First().MANUFACTURER_ID,
                            MANUFACTURER_CODE = sl.First().MANUFACTURER_CODE,
                            MANUFACTURER_NAME = sl.First().MANUFACTURER_NAME,
                            SERVICE_UNIT_ID = sl.First().SERVICE_UNIT_ID,
                            SERVICE_UNIT_CODE = sl.First().SERVICE_UNIT_CODE,
                            SERVICE_UNIT_NAME = sl.First().SERVICE_UNIT_NAME,
                            SUPPLIER_CODE = sl.First().SUPPLIER_CODE,
                            SUPPLIER_NAME = sl.First().SUPPLIER_NAME,
                        })
                        .ToList();
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

        internal HIS_IMP_MEST_MEDICINE GetById(long id)
        {
            try
            {
                return GetById(id, new HisImpMestMedicineFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_IMP_MEST_MEDICINE GetById(long id, HisImpMestMedicineFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestMedicineDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_IMP_MEST_MEDICINE GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisImpMestMedicineViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_IMP_MEST_MEDICINE GetViewById(long id, HisImpMestMedicineViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestMedicineDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_IMP_MEST_MEDICINE> GetViewByImpMestId(long id)
        {
            try
            {
                HisImpMestMedicineViewFilterQuery filter = new HisImpMestMedicineViewFilterQuery();
                filter.IMP_MEST_ID = id;
                return this.GetView(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        /// <summary>
        /// Lay du lieu theo phieu nhap. Neu phieu nhap la phieu tong hop thi lay ra tat ca cac du lieu
        /// thuoc cac phieu con nam trong phieu tong hop do
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        internal List<V_HIS_IMP_MEST_MEDICINE> GetViewAndIncludeChildrenByImpMestId(long impMestId)
        {
            try
            {
                List<V_HIS_IMP_MEST_MEDICINE> result = null;
                HisImpMestGet hisImpMestGet = new HisImpMestGet();
                HIS_IMP_MEST hisImpMest = hisImpMestGet.GetById(impMestId);
                if (hisImpMest != null)
                {
                    //Review lai
                    //List<V_HIS_IMP_MEST_MEDICINE> data = null;
                    //if (hisImpMest.IMP_MEST_TYPE_ID == HisImpMestTypeCFG.IMP_MEST_TYPE_ID__AGGR)
                    //{
                    //    HIS_AGGR_IMP_MEST hisAggrImpMest = new HisAggrImpMestGet().GetByImpMestId(impMestId);
                    //    List<HIS_IMP_MEST> children = hisImpMestGet.GetByAggrImpMestId(hisAggrImpMest.ID);
                    //    if (IsNotNullOrEmpty(children))
                    //    {
                    //        data = this.GetViewByImpMestIds(children.Select(o => o.ID).ToList());
                    //    }
                    //}
                    //else
                    //{
                    //    data = this.GetViewByImpMestId(impMestId);
                    //}
                    ////group lai 
                    //result = (from o in data
                    //          group o by new
                    //          {
                    //              o.BID_NUMBER,
                    //              o.EXPIRED_DATE,
                    //              o.IMP_PRICE,
                    //              o.IMP_VAT_RATIO,
                    //              o.INTERNAL_PRICE,
                    //              o.IS_ACTIVE,
                    //              o.MANUFACTURER_CODE,
                    //              o.MANUFACTURER_ID,
                    //              o.MANUFACTURER_NAME,
                    //              o.MEDICINE_ID,
                    //              o.MEDICINE_TYPE_CODE,
                    //              o.MEDICINE_TYPE_ID,
                    //              o.MEDICINE_TYPE_NAME,
                    //              o.NATIONAL_NAME,
                    //              o.PACKAGE_NUMBER,
                    //              o.PRICE,
                    //              o.REGISTER_NUMBER,
                    //              o.SERVICE_ID,
                    //              o.SERVICE_UNIT_CODE,
                    //              o.SERVICE_UNIT_ID,
                    //              o.SERVICE_UNIT_NAME,
                    //              o.SUPPLIER_CODE,
                    //              o.SUPPLIER_NAME,
                    //              o.VAT_RATIO,
                    //              o.NUM_ORDER,
                    //              o.IS_ADDICTIVE,
                    //              o.IS_NEUROLOGICAL
                    //          } into r
                    //          select new V_HIS_IMP_MEST_MEDICINE
                    //          {
                    //              BID_NUMBER = r.Key.BID_NUMBER,
                    //              EXPIRED_DATE = r.Key.EXPIRED_DATE,
                    //              IMP_PRICE = r.Key.IMP_PRICE,
                    //              IMP_VAT_RATIO = r.Key.IMP_VAT_RATIO,
                    //              INTERNAL_PRICE = r.Key.INTERNAL_PRICE,
                    //              IS_ACTIVE = r.Key.IS_ACTIVE,
                    //              MANUFACTURER_CODE = r.Key.MANUFACTURER_CODE,
                    //              MANUFACTURER_ID = r.Key.MANUFACTURER_ID,
                    //              MANUFACTURER_NAME = r.Key.MANUFACTURER_NAME,
                    //              MEDICINE_ID = r.Key.MEDICINE_ID,
                    //              MEDICINE_TYPE_CODE = r.Key.MEDICINE_TYPE_CODE,
                    //              MEDICINE_TYPE_ID = r.Key.MEDICINE_TYPE_ID,
                    //              MEDICINE_TYPE_NAME = r.Key.MEDICINE_TYPE_NAME,
                    //              NATIONAL_NAME = r.Key.NATIONAL_NAME,
                    //              PACKAGE_NUMBER = r.Key.PACKAGE_NUMBER,
                    //              PRICE = r.Key.PRICE,
                    //              REGISTER_NUMBER = r.Key.REGISTER_NUMBER,
                    //              SERVICE_ID = r.Key.SERVICE_ID,
                    //              SERVICE_UNIT_CODE = r.Key.SERVICE_UNIT_CODE,
                    //              SERVICE_UNIT_ID = r.Key.SERVICE_UNIT_ID,
                    //              SERVICE_UNIT_NAME = r.Key.SERVICE_UNIT_NAME,
                    //              SUPPLIER_CODE = r.Key.SUPPLIER_CODE,
                    //              SUPPLIER_NAME = r.Key.SUPPLIER_NAME,
                    //              VAT_RATIO = r.Key.VAT_RATIO,
                    //              NUM_ORDER = r.Key.NUM_ORDER,
                    //              IS_NEUROLOGICAL = r.Key.IS_NEUROLOGICAL,
                    //              IS_ADDICTIVE = r.Key.IS_ADDICTIVE,
                    //              AMOUNT = r.Sum(o => o.AMOUNT)
                    //          }).ToList();
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

        internal List<V_HIS_IMP_MEST_MEDICINE> GetViewByImpMestIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisImpMestMedicineViewFilterQuery filter = new HisImpMestMedicineViewFilterQuery();
                    filter.IMP_MEST_IDs = ids;
                    return this.GetView(filter);
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

        internal List<HIS_IMP_MEST_MEDICINE> GetByImpMestId(long id)
        {
            try
            {
                HisImpMestMedicineFilterQuery filter = new HisImpMestMedicineFilterQuery();
                filter.IMP_MEST_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_IMP_MEST_MEDICINE> GetByMedicineId(long id)
        {
            try
            {
                HisImpMestMedicineFilterQuery filter = new HisImpMestMedicineFilterQuery();
                filter.MEDICINE_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_IMP_MEST_MEDICINE> GetByImpMestIds(List<long> ids)
        {
            HisImpMestMedicineFilterQuery filter = new HisImpMestMedicineFilterQuery();
            filter.IMP_MEST_IDs = ids;
            return this.Get(filter);
        }

        internal List<HIS_IMP_MEST_MEDICINE> GetByIds(List<long> ids)
        {
            HisImpMestMedicineFilterQuery filter = new HisImpMestMedicineFilterQuery();
            filter.IDs = ids;
            return this.Get(filter);
        }
    }
}
