using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSaroExro
{
    public class HisSaroExroFilterQuery : HisSaroExroFilter
    {
        public HisSaroExroFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_SARO_EXRO, bool>>> listHisSaroExroExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SARO_EXRO, bool>>>();

        

        internal HisSaroExroSO Query()
        {
            HisSaroExroSO search = new HisSaroExroSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisSaroExroExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisSaroExroExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisSaroExroExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisSaroExroExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisSaroExroExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisSaroExroExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisSaroExroExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisSaroExroExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisSaroExroExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisSaroExroExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisSaroExroExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.EXECUTE_ROOM_ID.HasValue)
                {
                    listHisSaroExroExpression.Add(o => o.EXECUTE_ROOM_ID == this.EXECUTE_ROOM_ID.Value);
                }
                if (this.SAMPLE_ROOM_ID.HasValue)
                {
                    listHisSaroExroExpression.Add(o => o.SAMPLE_ROOM_ID == this.SAMPLE_ROOM_ID.Value);
                }

                search.listHisSaroExroExpression.AddRange(listHisSaroExroExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisSaroExroExpression.Clear();
                search.listHisSaroExroExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
