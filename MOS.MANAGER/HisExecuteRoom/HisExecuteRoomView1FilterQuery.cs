using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExecuteRoom
{
    public class HisExecuteRoomView1FilterQuery : HisExecuteRoomView1Filter
    {
        public HisExecuteRoomView1FilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_HIS_EXECUTE_ROOM_1, bool>>> listVHisExecuteRoom1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXECUTE_ROOM_1, bool>>>();

        

        internal HisExecuteRoomSO Query()
        {
            HisExecuteRoomSO search = new HisExecuteRoomSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listVHisExecuteRoom1Expression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisExecuteRoom1Expression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listVHisExecuteRoom1Expression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listVHisExecuteRoom1Expression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listVHisExecuteRoom1Expression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listVHisExecuteRoom1Expression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listVHisExecuteRoom1Expression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listVHisExecuteRoom1Expression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listVHisExecuteRoom1Expression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listVHisExecuteRoom1Expression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisExecuteRoom1Expression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.EXECUTE_ROOM_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.EXECUTE_ROOM_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }
                if (this.IS_EXAM.HasValue && this.IS_EXAM.Value)
                {
                    search.listVHisExecuteRoom1Expression.Add(o => o.IS_EXAM == ManagerConstant.IS_TRUE);
                }
                if (this.IS_EXAM.HasValue && !this.IS_EXAM.Value)
                {
                    search.listVHisExecuteRoom1Expression.Add(o => o.IS_EXAM != ManagerConstant.IS_TRUE);
                }
                if (this.BRANCH_ID.HasValue)
                {
                    search.listVHisExecuteRoom1Expression.Add(o => o.BRANCH_ID == this.BRANCH_ID.Value);
                }
                if (this.BRANCH_IDs != null)
                {
                    listVHisExecuteRoom1Expression.Add(o => this.BRANCH_IDs.Contains(o.BRANCH_ID));
                }

                search.listVHisExecuteRoom1Expression.AddRange(listVHisExecuteRoom1Expression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisExecuteRoom1Expression.Clear();
                search.listVHisExecuteRoom1Expression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
