using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSaroExro
{
    public class HisSaroExroViewFilterQuery : HisSaroExroViewFilter
    {
        public HisSaroExroViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_SARO_EXRO, bool>>> listVHisSaroExroExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SARO_EXRO, bool>>>();

        

        internal HisSaroExroSO Query()
        {
            HisSaroExroSO search = new HisSaroExroSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisSaroExroExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listVHisSaroExroExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisSaroExroExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisSaroExroExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisSaroExroExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisSaroExroExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisSaroExroExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisSaroExroExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisSaroExroExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisSaroExroExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listVHisSaroExroExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.EXECUTE_ROOM_ID.HasValue)
                {
                    listVHisSaroExroExpression.Add(o => o.EXECUTE_ROOM_ID == this.EXECUTE_ROOM_ID.Value);
                }
                if (this.SAMPLE_ROOM_ID.HasValue)
                {
                    listVHisSaroExroExpression.Add(o => o.SAMPLE_ROOM_ID == this.SAMPLE_ROOM_ID.Value);
                }

                search.listVHisSaroExroExpression.AddRange(listVHisSaroExroExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisSaroExroExpression.Clear();
                search.listVHisSaroExroExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
