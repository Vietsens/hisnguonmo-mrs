using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisWorkPlace
{
    public class HisWorkPlaceFilterQuery : HisWorkPlaceFilter
    {
        public HisWorkPlaceFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_WORK_PLACE, bool>>> listHisWorkPlaceExpression = new List<System.Linq.Expressions.Expression<Func<HIS_WORK_PLACE, bool>>>();

        

        internal HisWorkPlaceSO Query()
        {
            HisWorkPlaceSO search = new HisWorkPlaceSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisWorkPlaceExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisWorkPlaceExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisWorkPlaceExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisWorkPlaceExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisWorkPlaceExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisWorkPlaceExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisWorkPlaceExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisWorkPlaceExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisWorkPlaceExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisWorkPlaceExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisWorkPlaceExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.CONTACT_MOBILE.ToLower().Contains(this.KEY_WORD) ||
                        o.CONTACT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.DIRECTOR_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.PHONE.ToLower().Contains(this.KEY_WORD) ||
                        o.TAX_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.WORK_PLACE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.WORK_PLACE_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listHisWorkPlaceExpression.AddRange(listHisWorkPlaceExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisWorkPlaceExpression.Clear();
                search.listHisWorkPlaceExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
