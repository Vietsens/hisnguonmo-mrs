using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestMaterial
{
    public class HisImpMestMaterialFilterQuery : HisImpMestMaterialFilter
    {
        public HisImpMestMaterialFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_MATERIAL, bool>>> listHisImpMestMaterialExpression = new List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_MATERIAL, bool>>>();



        internal HisImpMestMaterialSO Query()
        {
            HisImpMestMaterialSO search = new HisImpMestMaterialSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisImpMestMaterialExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisImpMestMaterialExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisImpMestMaterialExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisImpMestMaterialExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisImpMestMaterialExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisImpMestMaterialExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisImpMestMaterialExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisImpMestMaterialExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisImpMestMaterialExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisImpMestMaterialExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.MATERIAL_ID.HasValue)
                {
                    search.listHisImpMestMaterialExpression.Add(o => o.MATERIAL_ID == this.MATERIAL_ID.Value);
                }
                if (this.IMP_MEST_ID.HasValue)
                {
                    search.listHisImpMestMaterialExpression.Add(o => o.IMP_MEST_ID == this.IMP_MEST_ID.Value);
                }
                if (this.IMP_MEST_IDs != null)
                {
                    search.listHisImpMestMaterialExpression.Add(o => this.IMP_MEST_IDs.Contains(o.IMP_MEST_ID));
                }
                if (this.IDs != null)
                {
                    search.listHisImpMestMaterialExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.MATERIAL_ID.HasValue)
                {
                    listHisImpMestMaterialExpression.Add(o => o.MATERIAL_ID == this.MATERIAL_ID.Value);
                }
                if (this.MATERIAL_IDs != null)
                {
                    listHisImpMestMaterialExpression.Add(o => this.MATERIAL_IDs.Contains(o.MATERIAL_ID));
                }
                if (this.IMP_MEST_ID__NOT__EQUAL.HasValue)
                {
                    listHisImpMestMaterialExpression.Add(o => o.IMP_MEST_ID == this.IMP_MEST_ID__NOT__EQUAL.Value);
                }

                search.listHisImpMestMaterialExpression.AddRange(listHisImpMestMaterialExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisImpMestMaterialExpression.Clear();
                search.listHisImpMestMaterialExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
