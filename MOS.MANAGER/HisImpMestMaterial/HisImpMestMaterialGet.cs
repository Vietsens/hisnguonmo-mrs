using AutoMapper;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisMaterial;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.MANAGER.HisImpMestMaterial
{
    partial class HisImpMestMaterialGet : GetBase
    {
        internal HisImpMestMaterialGet()
            : base()
        {

        }

        internal HisImpMestMaterialGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_IMP_MEST_MATERIAL> Get(HisImpMestMaterialFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestMaterialDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_IMP_MEST_MATERIAL> GetView(HisImpMestMaterialViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestMaterialDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_IMP_MEST_MATERIAL> GetViewByAggrImpMestId(long aggrExpMestId)
        {
            try
            {
                HisImpMestMaterialViewFilterQuery filter = new HisImpMestMaterialViewFilterQuery();
                filter.AGGR_IMP_MEST_ID = aggrExpMestId;
                return this.GetView(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_IMP_MEST_MATERIAL> GetViewByAggrImpMestIdAndGroupByMaterial(long aggrImpMestId)
        {
            try
            {
                List<V_HIS_IMP_MEST_MATERIAL> result = new List<V_HIS_IMP_MEST_MATERIAL>();
                List<V_HIS_IMP_MEST_MATERIAL> vHisImpMestMaterialDTOs = this.GetViewByAggrImpMestId(aggrImpMestId);
                if (IsNotNullOrEmpty(vHisImpMestMaterialDTOs))
                {
                    result = vHisImpMestMaterialDTOs
                        .GroupBy(o => new { o.MATERIAL_ID, o.MEDI_STOCK_ID })
                        .Select(sl => new V_HIS_IMP_MEST_MATERIAL
                        {
                            MATERIAL_ID = sl.Key.MATERIAL_ID,
                            MEDI_STOCK_ID = sl.Key.MEDI_STOCK_ID,
                            AMOUNT = sl.Sum(x => x.AMOUNT),
                            AGGR_IMP_MEST_ID = sl.First().AGGR_IMP_MEST_ID,
                            IMP_PRICE = sl.First().IMP_PRICE,
                            IMP_VAT_RATIO = sl.First().IMP_VAT_RATIO,
                            BID_NUMBER = sl.First().BID_NUMBER,
                            MATERIAL_TYPE_ID = sl.First().MATERIAL_TYPE_ID,
                            INTERNAL_PRICE = sl.First().INTERNAL_PRICE,
                            MATERIAL_TYPE_CODE = sl.First().MATERIAL_TYPE_CODE,
                            MATERIAL_TYPE_NAME = sl.First().MATERIAL_TYPE_NAME,
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

        /// <summary>
        /// Lay du lieu theo phieu nhap. Neu phieu nhap la phieu tong hop thi lay ra tat ca cac du lieu
        /// thuoc cac phieu con nam trong phieu tong hop do
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        internal List<V_HIS_IMP_MEST_MATERIAL> GetViewAndIncludeChildrenByImpMestId(long expMestId)
        {
            try
            {
                List<V_HIS_IMP_MEST_MATERIAL> result = null;
                HisImpMestGet hisImpMestGet = new HisImpMestGet();
                HIS_IMP_MEST hisImpMest = hisImpMestGet.GetById(expMestId);
                if (hisImpMest != null)
                {
                    //Review lai
                    //List<V_HIS_IMP_MEST_MATERIAL> data = null;
                    //if (hisImpMest.IMP_MEST_TYPE_ID == HisImpMestTypeCFG.IMP_MEST_TYPE_ID__AGGR)
                    //{
                    //    HIS_AGGR_IMP_MEST hisAggrImpMest = new HisAggrImpMestGet().GetByImpMestId(expMestId);
                    //    List<HIS_IMP_MEST> children = hisImpMestGet.GetByAggrImpMestId(hisAggrImpMest.ID);
                    //    if (IsNotNullOrEmpty(children))
                    //    {
                    //        data = this.GetViewByImpMestIds(children.Select(o => o.ID).ToList());
                    //    }
                    //}
                    //else
                    //{
                    //    data = this.GetViewByImpMestId(expMestId);
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
                    //              o.MATERIAL_ID,
                    //              o.MATERIAL_TYPE_CODE,
                    //              o.MATERIAL_TYPE_ID,
                    //              o.MATERIAL_TYPE_NAME,
                    //              o.NATIONAL_NAME,
                    //              o.PRICE,
                    //              o.SERVICE_ID,
                    //              o.SERVICE_UNIT_CODE,
                    //              o.SERVICE_UNIT_ID,
                    //              o.SERVICE_UNIT_NAME,
                    //              o.SUPPLIER_CODE,
                    //              o.SUPPLIER_NAME,
                    //              o.VAT_RATIO,
                    //              o.NUM_ORDER
                    //          } into r
                    //          select new V_HIS_IMP_MEST_MATERIAL
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
                    //              MATERIAL_ID = r.Key.MATERIAL_ID,
                    //              MATERIAL_TYPE_CODE = r.Key.MATERIAL_TYPE_CODE,
                    //              MATERIAL_TYPE_ID = r.Key.MATERIAL_TYPE_ID,
                    //              MATERIAL_TYPE_NAME = r.Key.MATERIAL_TYPE_NAME,
                    //              NATIONAL_NAME = r.Key.NATIONAL_NAME,
                    //              PRICE = r.Key.PRICE,
                    //              SERVICE_ID = r.Key.SERVICE_ID,
                    //              SERVICE_UNIT_CODE = r.Key.SERVICE_UNIT_CODE,
                    //              SERVICE_UNIT_ID = r.Key.SERVICE_UNIT_ID,
                    //              SERVICE_UNIT_NAME = r.Key.SERVICE_UNIT_NAME,
                    //              SUPPLIER_CODE = r.Key.SUPPLIER_CODE,
                    //              SUPPLIER_NAME = r.Key.SUPPLIER_NAME,
                    //              VAT_RATIO = r.Key.VAT_RATIO,
                    //              AMOUNT = r.Sum(o => o.AMOUNT),
                    //              NUM_ORDER = r.Key.NUM_ORDER
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

        internal HIS_IMP_MEST_MATERIAL GetById(long id)
        {
            try
            {
                return GetById(id, new HisImpMestMaterialFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_IMP_MEST_MATERIAL GetById(long id, HisImpMestMaterialFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestMaterialDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_IMP_MEST_MATERIAL GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisImpMestMaterialViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_IMP_MEST_MATERIAL GetViewById(long id, HisImpMestMaterialViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestMaterialDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_IMP_MEST_MATERIAL> GetViewByImpMestId(long id)
        {
            try
            {
                HisImpMestMaterialViewFilterQuery filter = new HisImpMestMaterialViewFilterQuery();
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

        internal List<V_HIS_IMP_MEST_MATERIAL> GetViewByImpMestIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisImpMestMaterialViewFilterQuery filter = new HisImpMestMaterialViewFilterQuery();
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

        internal List<HIS_IMP_MEST_MATERIAL> GetByImpMestId(long id)
        {
            try
            {
                HisImpMestMaterialFilterQuery filter = new HisImpMestMaterialFilterQuery();
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

        internal List<HIS_IMP_MEST_MATERIAL> GetByMaterialId(long id)
        {
            try
            {
                HisImpMestMaterialFilterQuery filter = new HisImpMestMaterialFilterQuery();
                filter.MATERIAL_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_IMP_MEST_MATERIAL> GetByImpMestIds(List<long> impMestIds)
        {
            try
            {
                if (impMestIds != null)
                {
                    HisImpMestMaterialFilterQuery filter = new HisImpMestMaterialFilterQuery();
                    filter.IMP_MEST_IDs = impMestIds;
                    return this.Get(filter);
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
    }
}
