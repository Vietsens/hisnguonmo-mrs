using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestMaterial
{
    public class HisImpMestMaterialView2FilterQuery : HisImpMestMaterialView2Filter
    {
        public HisImpMestMaterialView2FilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_MATERIAL_2, bool>>> listVHisImpMestMaterial2Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_MATERIAL_2, bool>>>();

        

        internal HisImpMestMaterialSO Query()
        {
            HisImpMestMaterialSO search = new HisImpMestMaterialSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisImpMestMaterial2Expression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisImpMestMaterial2Expression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisImpMestMaterial2Expression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisImpMestMaterial2Expression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisImpMestMaterial2Expression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisImpMestMaterial2Expression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisImpMestMaterial2Expression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisImpMestMaterial2Expression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisImpMestMaterial2Expression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisImpMestMaterial2Expression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.IMP_MEST_ID.HasValue)
                {
                    listVHisImpMestMaterial2Expression.Add(o => o.IMP_MEST_ID == this.IMP_MEST_ID.Value);
                }
                if (this.IMP_MEST_IDs != null)
                {
                    listVHisImpMestMaterial2Expression.Add(o => this.IMP_MEST_IDs.Contains(o.IMP_MEST_ID));
                }
                
                if (this.MATERIAL_TYPE_ID.HasValue)
                {
                    listVHisImpMestMaterial2Expression.Add(o => o.MATERIAL_TYPE_ID == this.MATERIAL_TYPE_ID.Value);
                }
                if (this.MATERIAL_TYPE_IDs != null)
                {
                    listVHisImpMestMaterial2Expression.Add(s => this.MATERIAL_TYPE_IDs.Contains(s.MATERIAL_TYPE_ID));
                }
                if (this.IDs != null)
                {
                    listVHisImpMestMaterial2Expression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.MATERIAL_ID.HasValue)
                {
                    listVHisImpMestMaterial2Expression.Add(o => o.MATERIAL_ID == this.MATERIAL_ID.Value);
                }
                if (this.MATERIAL_IDs != null)
                {
                    listVHisImpMestMaterial2Expression.Add(o => this.MATERIAL_IDs.Contains(o.MATERIAL_ID));
                }
                
                search.listVHisImpMestMaterial2Expression.AddRange(listVHisImpMestMaterial2Expression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisImpMestMaterial2Expression.Clear();
                search.listVHisImpMestMaterial2Expression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
