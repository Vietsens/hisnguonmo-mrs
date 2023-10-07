using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAnticipate
{
    public class HisAnticipateViewFilterQuery : HisAnticipateViewFilter
    {
        public HisAnticipateViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_ANTICIPATE, bool>>> listVHisAnticipateExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_ANTICIPATE, bool>>>();

        

        internal HisAnticipateSO Query()
        {
            HisAnticipateSO search = new HisAnticipateSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisAnticipateExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisAnticipateExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisAnticipateExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisAnticipateExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisAnticipateExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisAnticipateExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisAnticipateExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisAnticipateExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisAnticipateExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisAnticipateExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisAnticipateExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.ANTICIPATE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.DESCRIPTION.ToLower().Contains(this.KEY_WORD) ||
                        o.REQUEST_USERNAME.ToLower().Contains(this.KEY_WORD) ||
                        o.REQUEST_LOGINNAME.ToLower().Contains(this.KEY_WORD) ||
                        o.REQUEST_DEPARTMENT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.REQUEST_DEPARTMENT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.REQUEST_ROOM_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.REQUEST_ROOM_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.REQUEST_ROOM_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.USE_TIME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listVHisAnticipateExpression.AddRange(listVHisAnticipateExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisAnticipateExpression.Clear();
                search.listVHisAnticipateExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
