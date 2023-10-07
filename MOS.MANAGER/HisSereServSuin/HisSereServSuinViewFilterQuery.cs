using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServSuin
{
    public class HisSereServSuinViewFilterQuery : HisSereServSuinViewFilter
    {
        public HisSereServSuinViewFilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_HIS_SERE_SERV_SUIN, bool>>> listVHisSereServSuinExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERE_SERV_SUIN, bool>>>();

        

        internal HisSereServSuinSO Query()
        {
            HisSereServSuinSO search = new HisSereServSuinSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisSereServSuinExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listVHisSereServSuinExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisSereServSuinExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisSereServSuinExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisSereServSuinExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisSereServSuinExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisSereServSuinExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisSereServSuinExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisSereServSuinExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisSereServSuinExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listVHisSereServSuinExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.SERE_SERV_ID.HasValue)
                {
                    listVHisSereServSuinExpression.Add(o => o.SERE_SERV_ID == this.SERE_SERV_ID.Value);
                }
                if (this.SERE_SERV_IDs != null)
                {
                    listVHisSereServSuinExpression.Add(o => this.SERE_SERV_IDs.Contains(o.SERE_SERV_ID));
                }
                if (this.SUIN_INDEX_IDs != null)
                {
                    listVHisSereServSuinExpression.Add(o => this.SUIN_INDEX_IDs.Contains(o.SUIM_INDEX_ID));
                }
                
                search.listVHisSereServSuinExpression.AddRange(listVHisSereServSuinExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisSereServSuinExpression.Clear();
                search.listVHisSereServSuinExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
