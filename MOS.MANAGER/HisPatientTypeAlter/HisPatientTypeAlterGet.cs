using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.MANAGER.HisPatientTypeAlter
{
    partial class HisPatientTypeAlterGet : GetBase
    {
        internal HisPatientTypeAlterGet()
            : base()
        {

        }

        internal HisPatientTypeAlterGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_PATIENT_TYPE_ALTER> Get(HisPatientTypeAlterFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientTypeAlterDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_PATIENT_TYPE_ALTER> GetByPatientTypeId(long patientTypeId)
        {
            try
            {
                HisPatientTypeAlterFilterQuery filter = new HisPatientTypeAlterFilterQuery();
                filter.PATIENT_TYPE_ID = patientTypeId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_PATIENT_TYPE_ALTER> GetViewByPatientTypeIdAndTreatmentId(long patientTypeId, long treatmentId)
        {
            try
            {
                HisPatientTypeAlterViewFilterQuery filter = new HisPatientTypeAlterViewFilterQuery();
                filter.PATIENT_TYPE_ID = patientTypeId;
                filter.TREATMENT_ID = treatmentId;
                return this.GetView(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_PATIENT_TYPE_ALTER> GetByPatientTypeIdAndTreatmentId(long patientTypeId, long treatmentId)
        {
            try
            {
                HisPatientTypeAlterFilterQuery filter = new HisPatientTypeAlterFilterQuery();
                filter.PATIENT_TYPE_ID = patientTypeId;
                filter.TREATMENT_ID = treatmentId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_PATIENT_TYPE_ALTER> GetView(HisPatientTypeAlterViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientTypeAlterDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PATIENT_TYPE_ALTER GetById(long id)
        {
            try
            {
                return GetById(id, new HisPatientTypeAlterFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PATIENT_TYPE_ALTER GetApplied(long treatmentId)
        {
            return this.GetApplied(treatmentId, null);
        }

        /// <summary>
        /// Lay du lieu thay doi dien doi tuong gan nhat cua benh nhan thuoc chuoi dieu tri phu hop voi thoi gian y lenh
        /// - Du lieu can thuoc treatment truyen vao
        /// - Thoi gian log_time <= instruction_time
        /// </summary>
        /// <param name="treatmentId"></param>
        /// <param name="instructionTime"></param>
        /// <returns></returns>
        internal HIS_PATIENT_TYPE_ALTER GetApplied(long treatmentId, long? instructionTime)
        {
            try
            {
                HisPatientTypeAlterFilterQuery filter = new HisPatientTypeAlterFilterQuery();
                filter.TREATMENT_ID = treatmentId;
                filter.LOG_TIME_TO = instructionTime;
                List<HIS_PATIENT_TYPE_ALTER> hisPatientTypeAlters = this.Get(filter);
                if (IsNotNullOrEmpty(hisPatientTypeAlters))
                {
                    return hisPatientTypeAlters
                        .OrderByDescending(o => o.LOG_TIME)
                        .ThenByDescending(o => o.ID)
                        .FirstOrDefault();
                }
                return null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        /// <summary>
        /// Lay du lieu thay doi dien doi tuong gan nhat cua benh nhan thuoc chuoi dieu tri phu hop voi thoi gian y lenh
        /// - Du lieu can thuoc treatment truyen vao
        /// - Thoi gian log_time <= instruction_time
        /// </summary>
        /// <param name="treatmentId"></param>
        /// <param name="instructionTime"></param>
        /// <returns></returns>
        internal V_HIS_PATIENT_TYPE_ALTER GetViewApplied(long treatmentId, long? instructionTime)
        {
            try
            {
                HisPatientTypeAlterViewFilterQuery filter = new HisPatientTypeAlterViewFilterQuery();
                filter.TREATMENT_ID = treatmentId;
                filter.LOG_TIME_TO = instructionTime;
                List<V_HIS_PATIENT_TYPE_ALTER> hisPatientTypeAlters = this.GetView(filter);
                if (IsNotNullOrEmpty(hisPatientTypeAlters))
                {
                    return hisPatientTypeAlters
                        .OrderByDescending(o => o.LOG_TIME)
                        .ThenByDescending(o => o.ID)
                        .FirstOrDefault();
                }
                return null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        /// <summary>
        /// Lay du lieu thay doi dien doi tuong gan nhat cua benh nhan thuoc chuoi dieu tri phu hop voi thoi gian y lenh
        /// - Du lieu can thuoc treatment truyen vao
        /// - Thoi gian log_time <= instruction_time
        /// </summary>
        /// <param name="hisPatientTypeAlterDTOs"></param>
        /// <param name="instructionTime"></param>
        /// <returns></returns>
        internal HIS_PATIENT_TYPE_ALTER GetApplied(long treatmentId, long instructionTime, List<HIS_PATIENT_TYPE_ALTER> hisPatientTypeAlters)
        {
            try
            {
                HIS_PATIENT_TYPE_ALTER result = null;
                if (hisPatientTypeAlters != null)
                {
                    result = hisPatientTypeAlters
                        .Where(o => o.TREATMENT_ID == treatmentId && o.LOG_TIME <= instructionTime)
                        .OrderByDescending(o => o.LOG_TIME).FirstOrDefault();
                }
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_PATIENT_TYPE_ALTER> GetViewByTreatmentId(long treatmentId)
        {
            HisPatientTypeAlterViewFilterQuery filter = new HisPatientTypeAlterViewFilterQuery();
            filter.TREATMENT_ID = treatmentId;
            return this.GetView(filter);
        }

        internal List<V_HIS_PATIENT_TYPE_ALTER> GetViewByTreatmentIds(List<long> treatmentIds)
        {
            if (IsNotNullOrEmpty(treatmentIds))
            {
                HisPatientTypeAlterViewFilterQuery filter = new HisPatientTypeAlterViewFilterQuery();
                filter.TREATMENT_IDs = treatmentIds;
                return this.GetView(filter);
            }
            return null;
        }

        internal V_HIS_PATIENT_TYPE_ALTER GetViewLastByTreatmentId(long treatmentId)
        {
            try
            {
                return this.GetViewLastByTreatmentId(treatmentId, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_PATIENT_TYPE_ALTER GetViewLastByTreatmentId(long treatmentId, long? logTime)
        {
            try
            {
                HisPatientTypeAlterViewFilterQuery filter = new HisPatientTypeAlterViewFilterQuery();
                filter.TREATMENT_ID = treatmentId;
                filter.LOG_TIME_TO = logTime;
                filter.ORDER_FIELD = "LOG_TIME";
                filter.ORDER_DIRECTION = "DESC";
                List<V_HIS_PATIENT_TYPE_ALTER> list = this.GetView(filter);
                return IsNotNullOrEmpty(list) ? list[0] : null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PATIENT_TYPE_ALTER GetLastByTreatmentId(long treatmentId)
        {
            try
            {
                return this.GetLastByTreatmentId(treatmentId, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PATIENT_TYPE_ALTER GetLastByTreatmentId(long treatmentId, long? logTime)
        {
            try
            {
                HisPatientTypeAlterFilterQuery filter = new HisPatientTypeAlterFilterQuery();
                filter.TREATMENT_ID = treatmentId;
                filter.LOG_TIME_TO = logTime;
                filter.ORDER_FIELD = "LOG_TIME";
                filter.ORDER_DIRECTION = "DESC";
                List<HIS_PATIENT_TYPE_ALTER> list = this.Get(filter);
                return IsNotNullOrEmpty(list) ? list[0] : null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PATIENT_TYPE_ALTER GetById(long id, HisPatientTypeAlterFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientTypeAlterDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_PATIENT_TYPE_ALTER GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisPatientTypeAlterViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_PATIENT_TYPE_ALTER GetViewById(long id, HisPatientTypeAlterViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientTypeAlterDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_PATIENT_TYPE_ALTER> GetByPatientId(long id)
        {
            try
            {
                HisPatientTypeAlterFilterQuery filter = new HisPatientTypeAlterFilterQuery();
                filter.TDL_PATIENT_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_PATIENT_TYPE_ALTER> GetByTreatmentTypeId(long id)
        {
            try
            {
                HisPatientTypeAlterFilterQuery filter = new HisPatientTypeAlterFilterQuery();
                filter.TREATMENT_TYPE_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_PATIENT_TYPE_ALTER> GetViewByPatientId(long id)
        {
            try
            {
                HisPatientTypeAlterViewFilterQuery filter = new HisPatientTypeAlterViewFilterQuery();
                filter.TDL_PATIENT_ID = id;
                return this.GetView(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_PATIENT_TYPE_ALTER> GetByHeinCardNumber(string heinCardNumber)
        {
            try
            {
                HisPatientTypeAlterFilterQuery filter = new HisPatientTypeAlterFilterQuery();
                filter.HEIN_CARD_NUMBER__EXACT = heinCardNumber;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_PATIENT_TYPE_ALTER> GetByTreatmentId(long treatmentId)
        {
            try
            {
                HisPatientTypeAlterFilterQuery filter = new HisPatientTypeAlterFilterQuery();
                filter.TREATMENT_ID = treatmentId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_PATIENT_TYPE_ALTER> GetByTreatmentIds(List<long> treatmentIds)
        {
            try
            {
                if (IsNotNullOrEmpty(treatmentIds))
                {
                    HisPatientTypeAlterFilterQuery filter = new HisPatientTypeAlterFilterQuery();
                    filter.TREATMENT_IDs = treatmentIds;
                    return this.Get(filter);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        /// <summary>
        /// Lay danh sach HIS_PATIENT_TYPE_ALTER va distinct can cu vao cac cac thuoc tinh dien doi tuong de phuc vu duyet BHYT.
        /// </summary>
        /// <param name="treatmentId"></param>
        /// <returns></returns>
        internal List<HIS_PATIENT_TYPE_ALTER> GetDistinct(long treatmentId)
        {
            try
            {
                //Luu y: su dung new HisPatyAlterBhytGet() de ko truyen vao param cua client gui len, anh huong den phan trang
                List<HIS_PATIENT_TYPE_ALTER> list = this.GetByTreatmentId(treatmentId);
                return this.GetDistinct(list);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        /// <summary>
        /// Lay danh sach HIS_PATIENT_TYPE_ALTER va distinct can cu vao cac cac thuoc tinh dien doi tuong de phuc vu duyet BHYT.
        /// </summary>
        /// <param name="treatmentId"></param>
        /// <returns></returns>
        internal List<HIS_PATIENT_TYPE_ALTER> GetDistinct(List<HIS_PATIENT_TYPE_ALTER> list)
        {
            try
            {
                List<HIS_PATIENT_TYPE_ALTER> result = null;
                if (IsNotNullOrEmpty(list))
                {
                    var tmp = list.Select(o => new
                    {
                        //DOB = o.DOB,
                        //FIRST_NAME = o.FIRST_NAME,
                        //GENDER_CODE = o.GENDER_CODE,
                        //GENDER_ID = o.GENDER_ID,
                        //GENDER_NAME = o.GENDER_NAME,
                        //cac truong nay co the null ==> can check, vi null se bi loi khi distinct
                        HAS_BIRTH_CERTIFICATE = o.HAS_BIRTH_CERTIFICATE == null ? "" : o.HAS_BIRTH_CERTIFICATE,
                        HEIN_CARD_NUMBER = o.HEIN_CARD_NUMBER,
                        HEIN_MEDI_ORG_CODE = o.HEIN_MEDI_ORG_CODE,
                        //LAST_NAME = o.LAST_NAME,
                        LEVEL_CODE = o.LEVEL_CODE == null ? "" : o.LEVEL_CODE,
                        LIVE_AREA_CODE = o.LIVE_AREA_CODE == null ? "" : o.LIVE_AREA_CODE,
                        //PATIENT_CODE = o.PATIENT_CODE,
                        RIGHT_ROUTE_CODE = o.RIGHT_ROUTE_CODE,
                        RIGHT_ROUTE_TYPE_CODE = o.RIGHT_ROUTE_TYPE_CODE == null ? "" : o.RIGHT_ROUTE_TYPE_CODE,
                        TDL_PATIENT_ID = o.TDL_PATIENT_ID,
                        JOIN_5_YEAR = o.JOIN_5_YEAR == null ? "" : o.JOIN_5_YEAR,
                        PAID_6_MONTH = o.PAID_6_MONTH == null ? "" : o.PAID_6_MONTH,
                        ADDRESS = o.ADDRESS == null ? "" : o.ADDRESS,
                        //VIR_ADDRESS = o.VIR_ADDRESS == null ? "" : o.VIR_ADDRESS,
                        //VIR_PATIENT_NAME = o.VIR_PATIENT_NAME,
                        HEIN_CARD_FROM_TIME = o.HEIN_CARD_FROM_TIME,
                        HEIN_CARD_TO_TIME = o.HEIN_CARD_TO_TIME,
                        FREE_CO_PAID_TIME = o.FREE_CO_PAID_TIME
                    }).Distinct().ToList();

                    if (tmp != null && tmp.Count > 0)
                    {
                        result = new List<HIS_PATIENT_TYPE_ALTER>();
                        foreach (var t in tmp)
                        {
                            HIS_PATIENT_TYPE_ALTER bhyt = new HIS_PATIENT_TYPE_ALTER();
                            //bhyt.DOB = t.DOB;
                            //bhyt.FIRST_NAME = t.FIRST_NAME;
                            //bhyt.GENDER_CODE = t.GENDER_CODE;
                            //bhyt.GENDER_ID = t.GENDER_ID;
                            //bhyt.GENDER_NAME = t.GENDER_NAME;
                            bhyt.HAS_BIRTH_CERTIFICATE = t.HAS_BIRTH_CERTIFICATE;
                            bhyt.HEIN_CARD_NUMBER = t.HEIN_CARD_NUMBER;
                            bhyt.HEIN_MEDI_ORG_CODE = t.HEIN_MEDI_ORG_CODE;
                            //bhyt.LAST_NAME = t.LAST_NAME;
                            bhyt.LEVEL_CODE = t.LEVEL_CODE;
                            bhyt.LIVE_AREA_CODE = t.LIVE_AREA_CODE;
                            //bhyt.PATIENT_CODE = t.PATIENT_CODE;
                            bhyt.RIGHT_ROUTE_CODE = t.RIGHT_ROUTE_CODE;
                            bhyt.RIGHT_ROUTE_TYPE_CODE = t.RIGHT_ROUTE_TYPE_CODE;
                            bhyt.TDL_PATIENT_ID = t.TDL_PATIENT_ID;
                            bhyt.JOIN_5_YEAR = t.JOIN_5_YEAR;
                            bhyt.PAID_6_MONTH = t.PAID_6_MONTH;
                            bhyt.ADDRESS = t.ADDRESS;
                            //bhyt.VIR_ADDRESS = t.VIR_ADDRESS;
                            //bhyt.VIR_PATIENT_NAME = t.VIR_PATIENT_NAME;
                            bhyt.HEIN_CARD_FROM_TIME = t.HEIN_CARD_FROM_TIME;
                            bhyt.HEIN_CARD_TO_TIME = t.HEIN_CARD_TO_TIME;
                            bhyt.FREE_CO_PAID_TIME = t.FREE_CO_PAID_TIME;
                            result.Add(bhyt);
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
