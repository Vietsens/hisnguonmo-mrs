using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentVehicle
{
    public class HisAccidentVehicleFilterQuery : HisAccidentVehicleFilter
    {
        public HisAccidentVehicleFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_ACCIDENT_VEHICLE, bool>>> listHisAccidentVehicleExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ACCIDENT_VEHICLE, bool>>>();

        

        internal HisAccidentVehicleSO Query()
        {
            HisAccidentVehicleSO search = new HisAccidentVehicleSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisAccidentVehicleExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisAccidentVehicleExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisAccidentVehicleExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisAccidentVehicleExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisAccidentVehicleExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisAccidentVehicleExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisAccidentVehicleExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisAccidentVehicleExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisAccidentVehicleExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisAccidentVehicleExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisAccidentVehicleExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisAccidentVehicleExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.ACCIDENT_VEHICLE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.ACCIDENT_VEHICLE_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listHisAccidentVehicleExpression.AddRange(listHisAccidentVehicleExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisAccidentVehicleExpression.Clear();
                search.listHisAccidentVehicleExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
