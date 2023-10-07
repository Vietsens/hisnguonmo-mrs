using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServPttt
{
    public class HisSereServPtttViewFilterQuery : HisSereServPtttViewFilter
    {
        public HisSereServPtttViewFilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_HIS_SERE_SERV_PTTT, bool>>> listVHisSereServPtttExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERE_SERV_PTTT, bool>>>();

        

        internal HisSereServPtttSO Query()
        {
            HisSereServPtttSO search = new HisSereServPtttSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisSereServPtttExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listVHisSereServPtttExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisSereServPtttExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisSereServPtttExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisSereServPtttExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisSereServPtttExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisSereServPtttExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisSereServPtttExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisSereServPtttExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisSereServPtttExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.TDL_TREATMENT_IDs != null)
                {
                    listVHisSereServPtttExpression.Add(o => o.TDL_TREATMENT_ID.HasValue && this.TDL_TREATMENT_IDs.Contains(o.TDL_TREATMENT_ID.Value));
                }
                if (this.SERE_SERV_ID.HasValue)
                {
                    listVHisSereServPtttExpression.Add(o => o.SERE_SERV_ID == this.SERE_SERV_ID.Value);
                }
                if (this.SERE_SERV_IDs != null)
                {
                    listVHisSereServPtttExpression.Add(o => this.SERE_SERV_IDs.Contains(o.SERE_SERV_ID));
                }
                if (this.PTTT_GROUP_ID.HasValue)
                {
                    listVHisSereServPtttExpression.Add(o => o.PTTT_GROUP_ID.HasValue && o.PTTT_GROUP_ID == this.PTTT_GROUP_ID.Value);
                }
                if (this.PTTT_GROUP_IDs != null)
                {
                    listVHisSereServPtttExpression.Add(o => o.PTTT_GROUP_ID.HasValue && this.PTTT_GROUP_IDs.Contains(o.PTTT_GROUP_ID.Value));
                }

                search.listVHisSereServPtttExpression.AddRange(listVHisSereServPtttExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisSereServPtttExpression.Clear();
                search.listVHisSereServPtttExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
