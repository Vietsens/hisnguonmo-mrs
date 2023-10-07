using MRS.MANAGER.Base;
using Inventec.Common.Logging;
using Inventec.Core;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using MOS.MANAGER.HisTreatmentEndType;

namespace MRS.MANAGER.Config
{
    public class HisTreatmentEndTypeCFG
    {
        //private const string SDA_TREATMENT_END_TYPE_CODE__TRAN_PATI = "DBCODE.HIS_RS.HIS_TREATMENT_END_TYPE.TREATMENT_END_TYPE_CODE.TRAN_PATI";
        //private const string SDA_TREATMENT_END_TYPE_CODE__DEATH = "DBCODE.HIS_RS.HIS_TREATMENT_END_TYPE.TREATMENT_END_TYPE_CODE.DEATH";//tử vong
        //private const string SDA_TREATMENT_END_TYPE_CODE__CV__MRS00191 = "MRS.HIS_TREATMENT_END_TYPE.TREATMENT_END_TYPE_CODE.CV__MRS00191";//chuyển viện

        private const string SDA_TREATMENT_END_TYPE_CODE__KHAC = "MRS.HIS_TREATMENT_END_TYPE.TREATMENT_END_TYPE_CODE.KHAC";//khác
        private const string SDA_TREATMENT_END_TYPE_CODE__APPOINT = "MRS.HIS_TREATMENT_END_TYPE.TREATMENT_END_TYPE_CODE.APPOINT";//Hẹn khám
        private const string SDA_TREATMENT_END_TYPE_CODE__XV = "MRS.HIS_TREATMENT_END_TYPE.TREATMENT_END_TYPE_CODE.XV";//xin về
        private const string SDA_TREATMENT_END_TYPE_CODE__CB = "MRS.HIS_TREATMENT_END_TYPE.TREATMENT_END_TYPE_CODE.CB";//đưa về
        private const string SDA_TREATMENT_END_TYPE_CODE__BV = "MRS.HIS_TREATMENT_END_TYPE.TREATMENT_END_TYPE_CODE.BV";//bỏ về
        private const string SDA_TREATMENT_END_TYPE_CODE__CV = "MRS.HIS_TREATMENT_END_TYPE.TREATMENT_END_TYPE_CODE.CV";//chuyển viện
        private const string SDA_TREATMENT_END_TYPE_CODE__KHOI = "MRS.HIS_TREATMENT_END_TYPE.TREATMENT_END_TYPE_CODE.KHOI";//khỏi bệnh
        //private const string SDA_TREATMENT_END_TYPE_CODE__TRAN_DEPARTMENT = "MRS.HIS_TREATMENT_END_TYPE.TREATMENT_END_TYPE_CODE.TRAN_DEPARTMENT";
        private const string SDA_TREATMENT_END_TYPE_CODE__HOME = "MRS.HIS_TREATMENT_END_TYPE.TREATMENT_END_TYPE_CODE.HOME";

        //private static long treatmentEndTypeIdCvMrs00191;
        //public static long TREATMENT_END_TYPE_ID__CV__MRS00191
        //{
        //    get
        //    {
        //        if (treatmentEndTypeIdCvMrs00191 ==0)
        //        {
        //            treatmentEndTypeIdCvMrs00191 = GetId(SDA_TREATMENT_END_TYPE_CODE__CV__MRS00191);
        //        }
        //        return treatmentEndTypeIdCvMrs00191;
        //    }
        //    set
        //    {
        //        treatmentEndTypeIdCvMrs00191 = value;
        //    }
        //}

        private static List<long> treatmentEndTypeIdKhac;
        public static List<long> TREATMENT_END_TYPE_ID__KHAC
        {
            get
            {
                if (treatmentEndTypeIdKhac == null)
                {
                    treatmentEndTypeIdKhac = GetListId(SDA_TREATMENT_END_TYPE_CODE__KHAC);
                }
                return treatmentEndTypeIdKhac;
            }
            set
            {
                treatmentEndTypeIdKhac = value;
            }
        }

        private static List<long> treatmentEndTypeIdAppoint;
        public static List<long> TREATMENT_END_TYPE_ID__APPOINT
        {
            get
            {
                if (treatmentEndTypeIdAppoint == null)
                {
                    treatmentEndTypeIdAppoint = GetListId(SDA_TREATMENT_END_TYPE_CODE__APPOINT);
                }
                return treatmentEndTypeIdAppoint;
            }
            set
            {
                treatmentEndTypeIdAppoint = value;
            }
        }

        private static List<long> treatmentEndTypeIdXv;
        public static List<long> TREATMENT_END_TYPE_ID__XV
        {
            get
            {
                if (treatmentEndTypeIdXv == null)
                {
                    treatmentEndTypeIdXv = GetListId(SDA_TREATMENT_END_TYPE_CODE__XV);
                }
                return treatmentEndTypeIdXv;
            }
            set
            {
                treatmentEndTypeIdXv = value;
            }
        }

        private static List<long> treatmentEndTypeIdCb;
        public static List<long> TREATMENT_END_TYPE_ID__CB
        {
            get
            {
                if (treatmentEndTypeIdCb == null)
                {
                    treatmentEndTypeIdCb = GetListId(SDA_TREATMENT_END_TYPE_CODE__CB);
                }
                return treatmentEndTypeIdCb;
            }
            set
            {
                treatmentEndTypeIdCb = value;
            }
        }

        private static List<long> treatmentEndTypeIdBv;
        public static List<long> TREATMENT_END_TYPE_ID__BV
        {
            get
            {
                if (treatmentEndTypeIdBv == null)
                {
                    treatmentEndTypeIdBv = GetListId(SDA_TREATMENT_END_TYPE_CODE__BV);
                }
                return treatmentEndTypeIdBv;
            }
            set
            {
                treatmentEndTypeIdBv = value;
            }
        }

        private static List<long> treatmentEndTypeIdCv;
        public static List<long> TREATMENT_END_TYPE_ID__CV
        {
            get
            {
                if (treatmentEndTypeIdCv == null)
                {
                    treatmentEndTypeIdCv = GetListId(SDA_TREATMENT_END_TYPE_CODE__CV);
                }
                return treatmentEndTypeIdCv;
            }
            set
            {
                treatmentEndTypeIdCv = value;
            }
        }

        private static List<long> treatmentEndTypeIdKhoi;
        public static List<long> TREATMENT_END_TYPE_ID__KHOI
        {
            get
            {
                if (treatmentEndTypeIdKhoi == null)
                {
                    treatmentEndTypeIdKhoi = GetListId(SDA_TREATMENT_END_TYPE_CODE__KHOI);
                }
                return treatmentEndTypeIdKhoi;
            }
            set
            {
                treatmentEndTypeIdKhoi = value;
            }
        }

        //private static long treatmentEndTypeTranDepartment;
        //public static long TREATMENT_END_TYPE_ID__TRAN_DEPARTMENT
        //{
        //    get
        //    {
        //        if (treatmentEndTypeTranDepartment == 0)
        //        {
        //            treatmentEndTypeTranDepartment = GetId(SDA_TREATMENT_END_TYPE_CODE__TRAN_DEPARTMENT);
        //        }
        //        return treatmentEndTypeTranDepartment;
        //    }
        //    set
        //    {
        //        treatmentEndTypeTranDepartment = value;
        //    }
        //}

        private static long treatmentEndTypeIdHome;
        public static long TREATMENT_END_TYPE_ID__HOME
        {
            get
            {
                if (treatmentEndTypeIdHome == 0)
                {
                    treatmentEndTypeIdHome = GetId(SDA_TREATMENT_END_TYPE_CODE__HOME);
                }
                return treatmentEndTypeIdHome;
            }
            set
            {
                treatmentEndTypeIdHome = value;
            }
        }

        //private static long treatmentEndTypeTranPati;
        //public static long TREATMENT_END_TYPE_ID__TRAN_PATI
        //{
        //    get
        //    {
        //        if (treatmentEndTypeTranPati == 0)
        //        {
        //            treatmentEndTypeTranPati = GetId(SDA_TREATMENT_END_TYPE_CODE__TRAN_PATI);
        //        }
        //        return treatmentEndTypeTranPati;
        //    }
        //    set
        //    {
        //        treatmentEndTypeTranPati = value;
        //    }
        //}

        private static long treatmentEndTypeDeath;
        public static long TREATMENT_END_TYPE_ID__DEATH
        {
            get
            {
                if (treatmentEndTypeDeath == 0)
                {
                    treatmentEndTypeDeath = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET;// GetId(SDA_TREATMENT_END_TYPE_CODE__DEATH);
                }
                return treatmentEndTypeDeath;
            }
            set
            {
                treatmentEndTypeDeath = value;
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
                HisTreatmentEndTypeFilterQuery filter = new HisTreatmentEndTypeFilterQuery();
                //filter.KEY_WORD = value;
                var data = new HisTreatmentEndTypeManager().Get(filter).FirstOrDefault(o => o.TREATMENT_END_TYPE_CODE == value);
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

        private static List<long> GetListId(string code)
        {
            List<long> result = new List<long>();
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                var arr = value.Split(',');
                foreach (var s in arr)
                {
                    if (String.IsNullOrEmpty(s)) throw new ArgumentNullException(code);
                    HisTreatmentEndTypeFilterQuery filter = new HisTreatmentEndTypeFilterQuery();
                    var data = new HisTreatmentEndTypeManager().Get(filter).FirstOrDefault(o => o.TREATMENT_END_TYPE_CODE == s);
                    if (!(data != null && data.ID > 0)) throw new ArgumentNullException(code + LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => config), config));
                    result.Add(data.ID);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                treatmentEndTypeIdHome = 0;
                treatmentEndTypeDeath = 0;
                treatmentEndTypeIdKhac = null;
                treatmentEndTypeIdAppoint = null;
                treatmentEndTypeIdXv = null;
                treatmentEndTypeIdCb = null;
                treatmentEndTypeIdBv = null;
                treatmentEndTypeIdCv = null;
                treatmentEndTypeIdKhoi = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
