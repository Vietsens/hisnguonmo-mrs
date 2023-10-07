using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServPttt
{
    public class HisSereServPtttFilterQuery : HisSereServPtttFilter
    {
        public HisSereServPtttFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_PTTT, bool>>> listHisSereServPtttExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_PTTT, bool>>>();

        internal HisSereServPtttSO Query()
        {
            HisSereServPtttSO search = new HisSereServPtttSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisSereServPtttExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisSereServPtttExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisSereServPtttExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisSereServPtttExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisSereServPtttExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisSereServPtttExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisSereServPtttExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisSereServPtttExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisSereServPtttExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisSereServPtttExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisSereServPtttExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.BLOOD_ID.HasValue)
                {
                    listHisSereServPtttExpression.Add(o => o.BLOOD_ABO_ID.HasValue && o.BLOOD_ABO_ID.Value == this.BLOOD_ID.Value);
                }
                if (this.BLOOD_RH_ID.HasValue)
                {
                    listHisSereServPtttExpression.Add(o => o.BLOOD_RH_ID.HasValue && o.BLOOD_RH_ID.Value == this.BLOOD_RH_ID.Value);
                }
                if (this.EMOTIONLESS_METHOD_ID.HasValue)
                {
                    listHisSereServPtttExpression.Add(o => o.EMOTIONLESS_METHOD_ID.HasValue && o.EMOTIONLESS_METHOD_ID.Value == this.EMOTIONLESS_METHOD_ID.Value);
                }
                if (this.PTTT_CATASTROPHE_ID.HasValue)
                {
                    listHisSereServPtttExpression.Add(o => o.PTTT_CATASTROPHE_ID.HasValue && o.PTTT_CATASTROPHE_ID.Value == this.PTTT_CATASTROPHE_ID.Value);
                }
                if (this.PTTT_CONDITION_ID.HasValue)
                {
                    listHisSereServPtttExpression.Add(o => o.PTTT_CONDITION_ID.HasValue && o.PTTT_CONDITION_ID.Value == this.PTTT_CONDITION_ID.Value);
                }
                if (this.PTTT_GROUP_ID.HasValue)
                {
                    listHisSereServPtttExpression.Add(o => o.PTTT_GROUP_ID.HasValue && o.PTTT_GROUP_ID.Value == this.PTTT_GROUP_ID.Value);
                }
                if (this.PTTT_GROUP_IDs != null)
                {
                    listHisSereServPtttExpression.Add(o => o.PTTT_GROUP_ID.HasValue && this.PTTT_GROUP_IDs.Contains(o.PTTT_GROUP_ID.Value));
                }
                if (this.PTTT_METHOD_ID.HasValue)
                {
                    listHisSereServPtttExpression.Add(o => o.PTTT_METHOD_ID.HasValue && o.PTTT_METHOD_ID.Value == this.PTTT_METHOD_ID.Value);
                }
                if (this.SERE_SERV_IDs != null)
                {
                    listHisSereServPtttExpression.Add(o => this.SERE_SERV_IDs.Contains(o.SERE_SERV_ID));
                }
                
                search.listHisSereServPtttExpression.AddRange(listHisSereServPtttExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisSereServPtttExpression.Clear();
                search.listHisSereServPtttExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
