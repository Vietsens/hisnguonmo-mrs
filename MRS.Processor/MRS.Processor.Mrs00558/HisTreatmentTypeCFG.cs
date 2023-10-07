using MRS.MANAGER.Base;
using Inventec.Common.Logging;
using Inventec.Core;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisTreatmentType;
using MRS.MANAGER.Config;

namespace MRS.Proccessor.Mrs00558
{
    public class HisTreatmentTypeCFG
    {
        //private const string SDA_TREATMENT_TYPE_CODE__EXAM = "DBCODE.HIS_RS.HIS_TREATMENT_TYPE.TREATMENT_TYPE_CODE.EXAM";
        //private const string SDA_TREATMENT_TYPE_CODE__TREAT_IN = "DBCODE.HIS_RS.HIS_TREATMENT_TYPE.TREATMENT_TYPE_CODE.IN";
        //private const string SDA_TREATMENT_TYPE_CODE__TREAT_OUT = "DBCODE.HIS_RS.HIS_TREATMENT_TYPE.TREATMENT_TYPE_CODE.OUT";
        //private const string MRS__HIS_TREATMENT_TYPE__TREATMENT_IN = "MRS.HIS_TREATMENT_TYPE.TREATMENT_TYPE_CODE.NT";


        //private static long mrsHisTreatmentIn;
        //public static long MRS_TREATMENT_IN
        //{
        //    get
        //    {
        //        if (mrsHisTreatmentIn == 0)
        //        {
        //            mrsHisTreatmentIn = GetId(MRS__HIS_TREATMENT_TYPE__TREATMENT_IN);
        //        }
        //        return mrsHisTreatmentIn;
        //    }
        //    set
        //    {
        //        mrsHisTreatmentIn = value;
        //    }
        //}

        //private static long treatmentTypeIdExam;
        //public static long TREATMENT_TYPE_ID__EXAM
        //{
        //    get
        //    {
        //        if (treatmentTypeIdExam == 0)
        //        {
        //            treatmentTypeIdExam = GetId(SDA_TREATMENT_TYPE_CODE__EXAM);
        //        }
        //        return treatmentTypeIdExam;
        //    }
        //    set
        //    {
        //        treatmentTypeIdExam = value;
        //    }
        //}

        //private static long treatmentTypeIdTreatIn;
        //public static long TREATMENT_TYPE_ID__TREAT_IN
        //{
        //    get
        //    {
        //        if (treatmentTypeIdTreatIn == 0)
        //        {
        //            treatmentTypeIdTreatIn = GetId(SDA_TREATMENT_TYPE_CODE__TREAT_IN);
        //        }
        //        return treatmentTypeIdTreatIn;
        //    }
        //    set
        //    {
        //        treatmentTypeIdTreatIn = value;
        //    }
        //}

        //private static long treatmentTypeIdTreatOut;
        //public static long TREATMENT_TYPE_ID__TREAT_OUT
        //{
        //    get
        //    {
        //        if (treatmentTypeIdTreatOut == 0)
        //        {
        //            treatmentTypeIdTreatOut = GetId(SDA_TREATMENT_TYPE_CODE__TREAT_OUT);
        //        }
        //        return treatmentTypeIdTreatOut;
        //    }
        //    set
        //    {
        //        treatmentTypeIdTreatOut = value;
        //    }
        //}

        private static List<HIS_TREATMENT_TYPE> hisTreatmentTypes;
        public static List<HIS_TREATMENT_TYPE> HisTreatmentTypes
        {
            get
            {
                if (hisTreatmentTypes == null || hisTreatmentTypes.Count == 0)
                {
                    hisTreatmentTypes = GetList();
                }
                return hisTreatmentTypes;
            }
        }

        private static long GetId(string code)
        {
            long result = 0;
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
                HisTreatmentTypeFilterQuery filter = new HisTreatmentTypeFilterQuery();
                //filter.KEY_WORD = value;
                var data = new HisTreatmentTypeManager().Get(filter).FirstOrDefault(o => o.TREATMENT_TYPE_CODE == value);
                if (!(data != null && data.ID > 0)) throw new ArgumentNullException(code + LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => config), config));
                result = data.ID;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = 0;
            }
            return result;
        }

        private static List<HIS_TREATMENT_TYPE> GetList()
        {
            List<HIS_TREATMENT_TYPE> result = null;
            try
            {
                HisTreatmentTypeFilterQuery filter = new HisTreatmentTypeFilterQuery();
                result = new HisTreatmentTypeManager().Get(filter);
                if (result == null) throw new ArgumentNullException();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new List<HIS_TREATMENT_TYPE>();
            }
            return result;
        }
    }
}
