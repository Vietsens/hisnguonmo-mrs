using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTreatmentBedRoom
{
    public class HisTreatmentBedRoomFilterQuery : HisTreatmentBedRoomFilter
    {
        public HisTreatmentBedRoomFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_TREATMENT_BED_ROOM, bool>>> listHisTreatmentBedRoomExpression = new List<System.Linq.Expressions.Expression<Func<HIS_TREATMENT_BED_ROOM, bool>>>();

        

        internal HisTreatmentBedRoomSO Query()
        {
            HisTreatmentBedRoomSO search = new HisTreatmentBedRoomSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisTreatmentBedRoomExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisTreatmentBedRoomExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisTreatmentBedRoomExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisTreatmentBedRoomExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisTreatmentBedRoomExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisTreatmentBedRoomExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisTreatmentBedRoomExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisTreatmentBedRoomExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisTreatmentBedRoomExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisTreatmentBedRoomExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.TREATMENT_ID.HasValue)
                {
                    listHisTreatmentBedRoomExpression.Add(o => o.TREATMENT_ID == this.TREATMENT_ID.Value);
                }
                if (this.TREATMENT_IDs != null)
                {
                    listHisTreatmentBedRoomExpression.Add(o => this.TREATMENT_IDs.Contains(o.TREATMENT_ID));
                }
                if (this.BED_ROOM_ID.HasValue)
                {
                    listHisTreatmentBedRoomExpression.Add(o => o.BED_ROOM_ID == this.BED_ROOM_ID.Value);
                }
                if (this.IS_IN_ROOM.HasValue && this.IS_IN_ROOM.Value)
                {
                    listHisTreatmentBedRoomExpression.Add(o => !o.REMOVE_TIME.HasValue);
                }
                if (this.IS_IN_ROOM.HasValue && !this.IS_IN_ROOM.Value)
                {
                    listHisTreatmentBedRoomExpression.Add(o => o.REMOVE_TIME.HasValue);
                }
                if (this.BED_ID.HasValue)
                {
                    listHisTreatmentBedRoomExpression.Add(o => o.BED_ID == this.BED_ID.Value);
                }
                if (this.BED_IDs != null)
                {
                    listHisTreatmentBedRoomExpression.Add(o => o.BED_ID.HasValue && this.BED_IDs.Contains(o.BED_ID.Value));
                }
                
                search.listHisTreatmentBedRoomExpression.AddRange(listHisTreatmentBedRoomExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisTreatmentBedRoomExpression.Clear();
                search.listHisTreatmentBedRoomExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
