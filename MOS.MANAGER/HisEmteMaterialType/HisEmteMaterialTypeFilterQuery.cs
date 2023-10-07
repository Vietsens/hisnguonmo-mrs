using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEmteMaterialType
{
    public class HisEmteMaterialTypeFilterQuery : HisEmteMaterialTypeFilter
    {
        public HisEmteMaterialTypeFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_EMTE_MATERIAL_TYPE, bool>>> listHisEmteMaterialTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EMTE_MATERIAL_TYPE, bool>>>();

        

        internal HisEmteMaterialTypeSO Query()
        {
            HisEmteMaterialTypeSO search = new HisEmteMaterialTypeSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisEmteMaterialTypeExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisEmteMaterialTypeExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisEmteMaterialTypeExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisEmteMaterialTypeExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisEmteMaterialTypeExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisEmteMaterialTypeExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisEmteMaterialTypeExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisEmteMaterialTypeExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisEmteMaterialTypeExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisEmteMaterialTypeExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.EXP_MEST_TEMPLATE_ID.HasValue)
                {
                    search.listHisEmteMaterialTypeExpression.Add(o => o.EXP_MEST_TEMPLATE_ID == this.EXP_MEST_TEMPLATE_ID.Value);
                }
                
                #endregion

                if (this.MATERIAL_TYPE_ID.HasValue)
                {
                    search.listHisEmteMaterialTypeExpression.Add(o => o.MATERIAL_TYPE_ID == this.MATERIAL_TYPE_ID.Value);
                }

                search.listHisEmteMaterialTypeExpression.AddRange(listHisEmteMaterialTypeExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisEmteMaterialTypeExpression.Clear();
                search.listHisEmteMaterialTypeExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
