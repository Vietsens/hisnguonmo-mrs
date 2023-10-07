using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServTemp
{
    public class HisSereServTempFilterQuery : HisSereServTempFilter
    {
        public HisSereServTempFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_TEMP, bool>>> listHisSereServTempExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_TEMP, bool>>>();

        

        internal HisSereServTempSO Query()
        {
            HisSereServTempSO search = new HisSereServTempSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisSereServTempExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisSereServTempExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisSereServTempExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisSereServTempExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisSereServTempExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisSereServTempExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisSereServTempExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisSereServTempExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisSereServTempExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisSereServTempExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisSereServTempExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisSereServTempExpression.Add(o =>
                        o.CONCLUDE.ToLower().Contains(this.KEY_WORD) ||
                        o.SERE_SERV_TEMP_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SERE_SERV_TEMP_NAME.ToLower().Contains(this.KEY_WORD));
                }

                search.listHisSereServTempExpression.AddRange(listHisSereServTempExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisSereServTempExpression.Clear();
                search.listHisSereServTempExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
