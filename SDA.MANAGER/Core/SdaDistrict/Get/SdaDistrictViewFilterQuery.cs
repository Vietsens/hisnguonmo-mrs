using SDA.DAO.StagingObject;
using SDA.EFMODEL.DataModels;
using SDA.Filter;
using SDA.MANAGER.Base;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaDistrict.Get
{
    public class SdaDistrictViewFilterQuery : SdaDistrictViewFilter
    {
        public SdaDistrictViewFilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_SDA_DISTRICT, bool>>> listExpression = new List<System.Linq.Expressions.Expression<Func<V_SDA_DISTRICT, bool>>>();

        internal OrderProcessorBase OrderProcess = new OrderProcessorBase();

        internal SdaDistrictSO Query()
        {
            SdaDistrictSO search = new SdaDistrictSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null && this.IDs.Count > 0)
                {
                    listExpression.Add(o => this.IDs.Contains(o.ID));
                }

                if (this.PROVINCE_ID.HasValue)
                {
                    listExpression.Add(o => o.PROVINCE_ID == this.PROVINCE_ID);
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.DISTRICT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.DISTRICT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.INITIAL_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.SEARCH_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.PROVINCE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.PROVINCE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.ID_PATH.ToLower().Contains(this.KEY_WORD) ||
                        o.CODE_PATH.ToLower().Contains(this.KEY_WORD)
                        );
                }
                #endregion

                search.listVSdaDistrictExpression.AddRange(listExpression);
                search.OrderField = OrderProcess.GetOrderField<V_SDA_DISTRICT>(ORDER_FIELD);
                search.OrderDirection = OrderProcess.GetOrderDirection(ORDER_DIRECTION);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVSdaDistrictExpression.Clear();
                search.listVSdaDistrictExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
