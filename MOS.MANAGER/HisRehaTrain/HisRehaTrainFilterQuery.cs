using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRehaTrain
{
    public class HisRehaTrainFilterQuery : HisRehaTrainFilter
    {
        public HisRehaTrainFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_REHA_TRAIN, bool>>> listHisRehaTrainExpression = new List<System.Linq.Expressions.Expression<Func<HIS_REHA_TRAIN, bool>>>();

        

        internal HisRehaTrainSO Query()
        {
            HisRehaTrainSO search = new HisRehaTrainSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisRehaTrainExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisRehaTrainExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisRehaTrainExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisRehaTrainExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisRehaTrainExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisRehaTrainExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisRehaTrainExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisRehaTrainExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisRehaTrainExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisRehaTrainExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.SERE_SERV_REHA_ID.HasValue)
                {
                    listHisRehaTrainExpression.Add(o => o.SERE_SERV_REHA_ID == this.SERE_SERV_REHA_ID.Value);
                }
                if (this.SERE_SERV_REHA_IDs != null)
                {
                    search.listHisRehaTrainExpression.Add(o => this.SERE_SERV_REHA_IDs.Contains(o.SERE_SERV_REHA_ID));
                }

                search.listHisRehaTrainExpression.AddRange(listHisRehaTrainExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisRehaTrainExpression.Clear();
                search.listHisRehaTrainExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
