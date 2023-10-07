using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBlood
{
    public class HisBloodFilterQuery : HisBloodFilter
    {
        public HisBloodFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_BLOOD, bool>>> listHisBloodExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BLOOD, bool>>>();

        

        internal HisBloodSO Query()
        {
            HisBloodSO search = new HisBloodSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisBloodExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisBloodExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisBloodExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisBloodExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisBloodExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisBloodExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisBloodExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisBloodExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisBloodExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisBloodExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisBloodExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.BLOOD_TYPE_ID.HasValue)
                {
                    listHisBloodExpression.Add(o => o.BLOOD_TYPE_ID == this.BLOOD_TYPE_ID.Value);
                }
                if (this.BLOOD_TYPE_IDs != null)
                {
                    listHisBloodExpression.Add(o => this.BLOOD_TYPE_IDs.Contains(o.BLOOD_TYPE_ID));
                }
                if (this.MEDI_STOCK_ID.HasValue)
                {
                    listHisBloodExpression.Add(o => o.MEDI_STOCK_ID == this.MEDI_STOCK_ID.Value);
                }
                if (this.MEDI_STOCK_IDs != null)
                {
                    listHisBloodExpression.Add(o => o.MEDI_STOCK_ID.HasValue && this.MEDI_STOCK_IDs.Contains(o.MEDI_STOCK_ID.Value));
                }
                if (this.BLOOD_RH_ID.HasValue)
                {
                    listHisBloodExpression.Add(o => o.BLOOD_RH_ID.HasValue && o.BLOOD_RH_ID.Value == this.BLOOD_RH_ID.Value);
                }
                if (this.BLOOD_RH_IDs != null)
                {
                    listHisBloodExpression.Add(o => o.BLOOD_RH_ID.HasValue && this.BLOOD_RH_IDs.Contains(o.BLOOD_RH_ID.Value));
                }
                if (this.BLOOD_ABO_ID.HasValue)
                {
                    listHisBloodExpression.Add(o => o.BLOOD_ABO_ID == this.BLOOD_ABO_ID.Value);
                }
                if (this.BLOOD_ABO_IDs != null)
                {
                    listHisBloodExpression.Add(o => this.BLOOD_ABO_IDs.Contains(o.BLOOD_ABO_ID));
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisBloodExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.GIVE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.GIVE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.PACKAGE_NUMBER.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listHisBloodExpression.AddRange(listHisBloodExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisBloodExpression.Clear();
                search.listHisBloodExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
