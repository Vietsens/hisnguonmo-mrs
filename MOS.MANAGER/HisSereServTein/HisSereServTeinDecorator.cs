using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using MOS.MANAGER.Config;
using System;
using System.Linq;
using System.Collections.Generic;
using Inventec.Core;
using MOS.MANAGER.HisSereServ;

namespace MOS.MANAGER.HisSereServTein
{
    class HisSereServTeinDecorator
    {
        /// <summary>
        /// Bo sung thong tin result_code (duoi nguong, binh thuong, vuot nguong) va description (nguong trung binh)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="age"></param>
        /// <param name="genderId"></param>
        public static void Decorator(HIS_SERE_SERV_TEIN data, long dob, long genderId)
        {
            if (HisTestIndexRangeCFG.ACTIVE_DATA != null && data != null
                && (string.IsNullOrWhiteSpace(data.RESULT_CODE) || string.IsNullOrWhiteSpace(data.DESCRIPTION)))
            {
                long age = Inventec.Common.DateTime.Calculation.Age(dob);

                var query = HisTestIndexRangeCFG.ACTIVE_DATA.Where(o => o.TEST_INDEX_ID == data.TEST_INDEX_ID
                        && ((o.AGE_FROM.HasValue && o.AGE_FROM.Value <= age) || !o.AGE_FROM.HasValue)
                        && ((o.AGE_TO.HasValue && o.AGE_TO.Value >= age) || !o.AGE_TO.HasValue));

                if (genderId == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                {
                    query = query.Where(o => o.IS_MALE == ManagerConstant.IS_TRUE);
                }
                else if (genderId == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                {
                    query = query.Where(o => o.IS_FEMALE == ManagerConstant.IS_TRUE);
                }
                HIS_TEST_INDEX_RANGE range = query.FirstOrDefault();
                if (range != null)
                {
                    if (string.IsNullOrWhiteSpace(data.RESULT_CODE) && data.VALUE != null)
                    {
                        double? value = GetValue(data.VALUE);
                        double? maxValue = GetValue(range.MAX_VALUE);
                        double? minValue = GetValue(range.MIN_VALUE);
                        if (value.HasValue && maxValue.HasValue && value.Value > maxValue.Value)
                        {
                            data.RESULT_CODE = HisSereServTeinCFG.RESULT_CODE__HIGHER;
                        }
                        else if (value.HasValue && minValue.HasValue && value.Value < minValue.Value)
                        {
                            data.RESULT_CODE = HisSereServTeinCFG.RESULT_CODE__LOWER;
                        }
                        else if (value.HasValue && minValue.HasValue && maxValue.HasValue && value.Value >= minValue.Value && value.Value <= maxValue.Value)
                        {
                            data.RESULT_CODE = HisSereServTeinCFG.RESULT_CODE__NORMAL;
                        }
                    }

                    if (string.IsNullOrWhiteSpace(data.DESCRIPTION) && (range.MIN_VALUE != null || range.MAX_VALUE != null))
                    {
                        string minVal = range.MIN_VALUE != null ? range.MIN_VALUE : "";
                        string maxVal = range.MAX_VALUE != null ? range.MAX_VALUE : "";
                        data.DESCRIPTION = string.Format("({0} - {1})", minVal, maxVal);
                    }
                }
            }
        }

        private static double? GetValue(string value)
        {
            try
            {
                return double.Parse(value);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
        }
    }
}
