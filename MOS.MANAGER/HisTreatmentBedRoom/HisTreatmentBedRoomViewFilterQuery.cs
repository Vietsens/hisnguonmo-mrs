using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTreatmentBedRoom
{
    public class HisTreatmentBedRoomViewFilterQuery : HisTreatmentBedRoomViewFilter
    {
        public HisTreatmentBedRoomViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT_BED_ROOM, bool>>> listVHisTreatmentBedRoomExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT_BED_ROOM, bool>>>();

        

        internal HisTreatmentBedRoomSO Query()
        {
            HisTreatmentBedRoomSO search = new HisTreatmentBedRoomSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisTreatmentBedRoomExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listVHisTreatmentBedRoomExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisTreatmentBedRoomExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisTreatmentBedRoomExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisTreatmentBedRoomExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisTreatmentBedRoomExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisTreatmentBedRoomExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisTreatmentBedRoomExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisTreatmentBedRoomExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisTreatmentBedRoomExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.TREATMENT_ID.HasValue)
                {
                    listVHisTreatmentBedRoomExpression.Add(o => o.TREATMENT_ID == this.TREATMENT_ID.Value);
                }
                if (this.TREATMENT_IDs != null)
                {
                    listVHisTreatmentBedRoomExpression.Add(o => this.TREATMENT_IDs.Contains(o.TREATMENT_ID));
                }
                if (this.BED_ROOM_ID.HasValue)
                {
                    listVHisTreatmentBedRoomExpression.Add(o => o.BED_ROOM_ID == this.BED_ROOM_ID.Value);
                }
                if (this.ADD_TIME_TO.HasValue)
                {
                    listVHisTreatmentBedRoomExpression.Add(o => o.ADD_TIME <= this.ADD_TIME_TO.Value);
                }
                if (this.REMOVE_TIME_FROM.HasValue)
                {
                    listVHisTreatmentBedRoomExpression.Add(o => !o.REMOVE_TIME.HasValue || o.REMOVE_TIME.Value >= this.REMOVE_TIME_FROM.Value);
                }
                if (this.IS_IN_ROOM.HasValue && this.IS_IN_ROOM.Value)
                {
                    listVHisTreatmentBedRoomExpression.Add(o => !o.REMOVE_TIME.HasValue);
                }
                if (this.IS_IN_ROOM.HasValue && !this.IS_IN_ROOM.Value)
                {
                    listVHisTreatmentBedRoomExpression.Add(o => o.REMOVE_TIME.HasValue);
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.Trim().ToLower();
                    listVHisTreatmentBedRoomExpression.Add(o => 
                        o.ADD_LOGINNAME.ToLower().Contains(this.KEY_WORD) ||
                        o.ADD_USERNAME.ToLower().Contains(this.KEY_WORD) ||
                        o.BED_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.BED_ROOM_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.BED_ROOM_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.TDL_PATIENT_FIRST_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.TDL_PATIENT_GENDER_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.TDL_PATIENT_LAST_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.TDL_PATIENT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.REMOVE_LOGINNAME.ToLower().Contains(this.KEY_WORD) ||
                        o.REMOVE_USERNAME.ToLower().Contains(this.KEY_WORD) ||
                        o.TREATMENT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.TDL_PATIENT_ADDRESS.ToLower().Contains(this.KEY_WORD) ||
                        o.TDL_PATIENT_NAME.ToLower().Contains(this.KEY_WORD));
                }
                if (!String.IsNullOrEmpty(this.KEYWORD__PATIENT_NAME__TREATMENT_CODE__BED_NAME))
                {
                    this.KEYWORD__PATIENT_NAME__TREATMENT_CODE__BED_NAME = this.KEYWORD__PATIENT_NAME__TREATMENT_CODE__BED_NAME.Trim().ToLower();
                    listVHisTreatmentBedRoomExpression.Add(o =>
                        o.BED_NAME.ToLower().Contains(this.KEYWORD__PATIENT_NAME__TREATMENT_CODE__BED_NAME) ||
                        o.TREATMENT_CODE.ToLower().Contains(this.KEYWORD__PATIENT_NAME__TREATMENT_CODE__BED_NAME) ||
                        o.TDL_PATIENT_NAME.ToLower().Contains(this.KEYWORD__PATIENT_NAME__TREATMENT_CODE__BED_NAME));
                }

                search.listVHisTreatmentBedRoomExpression.AddRange(listVHisTreatmentBedRoomExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisTreatmentBedRoomExpression.Clear();
                search.listVHisTreatmentBedRoomExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
