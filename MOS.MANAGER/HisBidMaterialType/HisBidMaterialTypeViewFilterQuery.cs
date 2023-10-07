using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBidMaterialType
{
    public class HisBidMaterialTypeViewFilterQuery : HisBidMaterialTypeViewFilter
    {
        public HisBidMaterialTypeViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_BID_MATERIAL_TYPE, bool>>> listVHisBidMaterialTypeExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_BID_MATERIAL_TYPE, bool>>>();

        

        internal HisBidMaterialTypeSO Query()
        {
            HisBidMaterialTypeSO search = new HisBidMaterialTypeSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisBidMaterialTypeExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listVHisBidMaterialTypeExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisBidMaterialTypeExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisBidMaterialTypeExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisBidMaterialTypeExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisBidMaterialTypeExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisBidMaterialTypeExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisBidMaterialTypeExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisBidMaterialTypeExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisBidMaterialTypeExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listVHisBidMaterialTypeExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.BID_ID.HasValue)
                {
                    listVHisBidMaterialTypeExpression.Add(o => o.BID_ID == this.BID_ID.Value);
                }
                if (this.BID_IDs != null)
                {
                    listVHisBidMaterialTypeExpression.Add(o => this.BID_IDs.Contains(o.BID_ID));
                }
                if (this.MATERIAL_TYPE_ID.HasValue)
                {
                    listVHisBidMaterialTypeExpression.Add(o => o.MATERIAL_TYPE_ID == this.MATERIAL_TYPE_ID.Value);
                }
                if (this.MATERIAL_TYPE_IDs != null)
                {
                    listVHisBidMaterialTypeExpression.Add(o => this.MATERIAL_TYPE_IDs.Contains(o.MATERIAL_TYPE_ID.Value));
                }

                search.listVHisBidMaterialTypeExpression.AddRange(listVHisBidMaterialTypeExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisBidMaterialTypeExpression.Clear();
                search.listVHisBidMaterialTypeExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
