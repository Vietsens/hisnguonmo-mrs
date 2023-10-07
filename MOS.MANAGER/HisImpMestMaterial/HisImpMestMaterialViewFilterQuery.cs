using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestMaterial
{
    public class HisImpMestMaterialViewFilterQuery : HisImpMestMaterialViewFilter
    {
        public HisImpMestMaterialViewFilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_MATERIAL, bool>>> listVHisImpMestMaterialExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_MATERIAL, bool>>>();

        

        internal HisImpMestMaterialSO Query()
        {
            HisImpMestMaterialSO search = new HisImpMestMaterialSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisImpMestMaterialExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisImpMestMaterialExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisImpMestMaterialExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisImpMestMaterialExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisImpMestMaterialExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisImpMestMaterialExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisImpMestMaterialExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisImpMestMaterialExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisImpMestMaterialExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisImpMestMaterialExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.MEDI_STOCK_ID.HasValue)
                {
                    listVHisImpMestMaterialExpression.Add(o => o.MEDI_STOCK_ID == this.MEDI_STOCK_ID.Value);
                }
                if (this.IMP_TIME_FROM.HasValue)
                {
                    listVHisImpMestMaterialExpression.Add(o => o.IMP_TIME >= this.IMP_TIME_FROM.Value);
                }
                if (this.IMP_TIME_TO.HasValue)
                {
                    listVHisImpMestMaterialExpression.Add(o => o.IMP_TIME <= this.IMP_TIME_TO.Value);
                }
                if (this.MEDI_STOCK_ID.HasValue)
                {
                    listVHisImpMestMaterialExpression.Add(o => o.MEDI_STOCK_ID == this.MEDI_STOCK_ID.Value);
                }
                if (this.IMP_MEST_ID.HasValue)
                {
                    listVHisImpMestMaterialExpression.Add(o => o.IMP_MEST_ID == this.IMP_MEST_ID.Value);
                }
                if (this.IMP_MEST_IDs != null)
                {
                    listVHisImpMestMaterialExpression.Add(o => this.IMP_MEST_IDs.Contains(o.IMP_MEST_ID));
                }
                if (this.MEDI_STOCK_PERIOD_ID.HasValue)
                {
                    listVHisImpMestMaterialExpression.Add(o => o.MEDI_STOCK_PERIOD_ID == this.MEDI_STOCK_PERIOD_ID.Value);
                }
                if (this.HAS_MEDI_STOCK_PERIOD.HasValue)
                {
                    if (this.HAS_MEDI_STOCK_PERIOD.Value)
                    {
                        listVHisImpMestMaterialExpression.Add(o => o.MEDI_STOCK_PERIOD_ID != null);
                    }
                    else
                    {
                        listVHisImpMestMaterialExpression.Add(o => o.MEDI_STOCK_PERIOD_ID == null);
                    }
                }
                if (this.MATERIAL_TYPE_ID.HasValue)
                {
                    listVHisImpMestMaterialExpression.Add(o => o.MATERIAL_TYPE_ID == this.MATERIAL_TYPE_ID.Value);
                }
                if (this.MATERIAL_TYPE_IDs != null)
                {
                    listVHisImpMestMaterialExpression.Add(s => this.MATERIAL_TYPE_IDs.Contains(s.MATERIAL_TYPE_ID));
                }
                if (this.IMP_MEST_STT_ID.HasValue)
                {
                    listVHisImpMestMaterialExpression.Add(o => o.IMP_MEST_STT_ID == this.IMP_MEST_STT_ID.Value);
                }
                if (this.IMP_MEST_TYPE_ID.HasValue)
                {
                    listVHisImpMestMaterialExpression.Add(o => o.IMP_MEST_TYPE_ID == this.IMP_MEST_TYPE_ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisImpMestMaterialExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.AGGR_IMP_MEST_ID.HasValue)
                {
                    listVHisImpMestMaterialExpression.Add(o => o.AGGR_IMP_MEST_ID.HasValue && o.AGGR_IMP_MEST_ID.Value == this.AGGR_IMP_MEST_ID.Value);
                }
                if (this.MATERIAL_ID.HasValue)
                {
                    listVHisImpMestMaterialExpression.Add(o => o.MATERIAL_ID == this.MATERIAL_ID.Value);
                }
                if (this.MATERIAL_IDs != null)
                {
                    listVHisImpMestMaterialExpression.Add(o => this.MATERIAL_IDs.Contains(o.MATERIAL_ID));
                }
                if (this.IMP_MEST_STT_IDs != null)
                {
                    listVHisImpMestMaterialExpression.Add(o => this.IMP_MEST_STT_IDs.Contains(o.IMP_MEST_STT_ID));
                }
                if (this.MEDI_STOCK_IDs != null)
                {
                    listVHisImpMestMaterialExpression.Add(s => this.MEDI_STOCK_IDs.Contains(s.MEDI_STOCK_ID));
                }
                
                search.listVHisImpMestMaterialExpression.AddRange(listVHisImpMestMaterialExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisImpMestMaterialExpression.Clear();
                search.listVHisImpMestMaterialExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
