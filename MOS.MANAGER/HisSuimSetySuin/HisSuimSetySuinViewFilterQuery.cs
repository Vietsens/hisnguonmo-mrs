using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSuimSetySuin
{
    public class HisSuimSetySuinViewFilterQuery : HisSuimSetySuinViewFilter
    {
        public HisSuimSetySuinViewFilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_HIS_SUIM_SETY_SUIN, bool>>> listVHisSuimSetySuinExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SUIM_SETY_SUIN, bool>>>();

        

        internal HisSuimSetySuinSO Query()
        {
            HisSuimSetySuinSO search = new HisSuimSetySuinSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisSuimSetySuinExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listVHisSuimSetySuinExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisSuimSetySuinExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisSuimSetySuinExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisSuimSetySuinExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisSuimSetySuinExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisSuimSetySuinExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisSuimSetySuinExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisSuimSetySuinExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisSuimSetySuinExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listVHisSuimSetySuinExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.SERVICE_ID.HasValue)
                {
                    listVHisSuimSetySuinExpression.Add(o => o.SUIM_SERVICE_TYPE_ID == this.SERVICE_ID.Value);
                }

                search.listVHisSuimSetySuinExpression.AddRange(listVHisSuimSetySuinExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisSuimSetySuinExpression.Clear();
                search.listVHisSuimSetySuinExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
