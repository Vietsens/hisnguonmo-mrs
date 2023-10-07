using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEmergencyWtime
{
    public class HisEmergencyWtimeFilterQuery : HisEmergencyWtimeFilter
    {
        public HisEmergencyWtimeFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_EMERGENCY_WTIME, bool>>> listHisEmergencyWtimeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EMERGENCY_WTIME, bool>>>();

        

        internal HisEmergencyWtimeSO Query()
        {
            HisEmergencyWtimeSO search = new HisEmergencyWtimeSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisEmergencyWtimeExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisEmergencyWtimeExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisEmergencyWtimeExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisEmergencyWtimeExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisEmergencyWtimeExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisEmergencyWtimeExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisEmergencyWtimeExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisEmergencyWtimeExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisEmergencyWtimeExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisEmergencyWtimeExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisEmergencyWtimeExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.EMERGENCY_WTIME_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.EMERGENCY_WTIME_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }
                search.listHisEmergencyWtimeExpression.AddRange(listHisEmergencyWtimeExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisEmergencyWtimeExpression.Clear();
                search.listHisEmergencyWtimeExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
