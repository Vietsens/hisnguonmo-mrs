using MRS.MANAGER.Base;
using Inventec.Common.Logging;
using Inventec.Core;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisServiceType;

namespace MRS.MANAGER.Config
{
    public class HisServiceTypeCFG
    {
        //private const string SERVICE_TYPE_CODE__EXAM = "DBCODE.HIS_RS.HIS_SERVICE_TYPE.SERVICE_TYPE_CODE.EXAM";//khám
        //private const string SERVICE_TYPE_CODE__TEST = "DBCODE.HIS_RS.HIS_SERVICE_TYPE.SERVICE_TYPE_CODE.TEST";//Xét nghiệm
        //private const string SERVICE_TYPE_CODE__DIIM = "DBCODE.HIS_RS.HIS_SERVICE_TYPE.SERVICE_TYPE_CODE.DIIM";//chẩn đoán hình ảnh
        //private const string SERVICE_TYPE_CODE__MISU = "DBCODE.HIS_RS.HIS_SERVICE_TYPE.SERVICE_TYPE_CODE.MISU";//Thủ thuật
        //private const string SERVICE_TYPE_CODE__SURG = "DBCODE.HIS_RS.HIS_SERVICE_TYPE.SERVICE_TYPE_CODE.SURG";//Phẫu thuật
        //private const string SERVICE_TYPE_CODE__FUEX = "DBCODE.HIS_RS.HIS_SERVICE_TYPE.SERVICE_TYPE_CODE.FUEX";// thăm dò chức năng
        //private const string SERVICE_TYPE_CODE__ENDO = "DBCODE.HIS_RS.HIS_SERVICE_TYPE.SERVICE_TYPE_CODE.ENDO";//Nội soi
        //private const string SERVICE_TYPE_CODE__SUIM = "DBCODE.HIS_RS.HIS_SERVICE_TYPE.SERVICE_TYPE_CODE.SUIM";//siêu âm
        //private const string SERVICE_TYPE_CODE__BED = "DBCODE.HIS_RS.HIS_SERVICE_TYPE.SERVICE_TYPE_CODE.BED";//giường bệnh
        //private const string SERVICE_TYPE_CODE__OTHER = "DBCODE.HIS_RS.HIS_SERVICE_TYPE.SERVICE_TYPE_CODE.OTHER";//khác
        //private const string SERVICE_TYPE_CODE__MEDI = "DBCODE.HIS_RS.HIS_SERVICE_TYPE.SERVICE_TYPE_CODE.MEDI";//thuốc
        //private const string SERVICE_TYPE_CODE__MATE = "DBCODE.HIS_RS.HIS_SERVICE_TYPE.SERVICE_TYPE_CODE.MATE";//vật tư
        //private const string SERVICE_TYPE_CODE__BLOOD = "DBCODE.HIS_RS.HIS_SERVICE_TYPE.SERVICE_TYPE_CODE.BLOOD";//Máu
        private const string SERVICE_TYPE_CODE__TEST_BLOOD = "MRS.HIS_RS.HIS_SERVICE_TYPE.SERVICE_TYPE_CODE.TES_BLOOD";//xét nghiệm máu
        private const string SERVICE_TYPE_CODE__HEIN_RATIO_5 = "MRS.HIS_SERVICE_TYPE.SERVICE_TYPE_CODE.HEIN_RATIO_5";//Hưởng 5% BHYT
        private const string SERVICE_TYPE_CODE__HEIN_RATIO_20 = "MRS.HIS_SERVICE_TYPE.SERVICE_TYPE_CODE.HEIN_RATIO_20";//Hưởng 20% BHYT
        private const string SERVICE_TYPE_CODE__HEIN_RATIO_40 = "MRS.HIS_SERVICE_TYPE.SERVICE_TYPE_CODE.HEIN_RATIO_40";//Hưởng 40% BHYT
        private const string SERVICE_TYPE_CODE__HEIN_RATIO_0 = "MRS.HIS_SERVICE_TYPE.SERVICE_TYPE_CODE.HEIN_RATIO_0";//Hưởng 100% BHYT
        //private const string SERVICE_TYPE_CODE__CLS = "MRS.HIS_SERVICE_TYPE.SERVICE_TYPE_CODE.CLS";//Các loại dịch vụ là cận lâm sàng

        private static decimal? serviceTypeIdHeinRatio0;
        public static decimal? SERVICE_TYPE_ID__HEIN_RATIO_0
        {
            get
            {
                if (serviceTypeIdHeinRatio0 == null)
                {
                    var config = Loader.dictionaryConfig[SERVICE_TYPE_CODE__HEIN_RATIO_0];
                    if (config == null) throw new ArgumentNullException(SERVICE_TYPE_CODE__HEIN_RATIO_0);
                    var value = string.IsNullOrEmpty(config.VALUE) ? (string.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                    serviceTypeIdHeinRatio0 = decimal.Parse(value);
                }
                return serviceTypeIdHeinRatio0;
            }
            set
            {
                serviceTypeIdHeinRatio0 = value;
            }
        }

        private static decimal? serviceTypeIdHeinRatio40;
        public static decimal? SERVICE_TYPE_ID__HEIN_RATIO_40
        {
            get
            {
                if (serviceTypeIdHeinRatio40 == null)
                {
                    var config = Loader.dictionaryConfig[SERVICE_TYPE_CODE__HEIN_RATIO_40];
                    if (config == null) throw new ArgumentNullException(SERVICE_TYPE_CODE__HEIN_RATIO_40);
                    var value = string.IsNullOrEmpty(config.VALUE) ? (string.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                    serviceTypeIdHeinRatio40 = decimal.Parse(value);
                }
                return serviceTypeIdHeinRatio40;
            }
            set
            {
                serviceTypeIdHeinRatio40 = value;
            }
        }

        private static decimal? serviceTypeIdHeinRatio20;
        public static decimal? SERVICE_TYPE_ID__HEIN_RATIO_20
        {
            get
            {
                if (serviceTypeIdHeinRatio20 == null)
                {
                    var config = Loader.dictionaryConfig[SERVICE_TYPE_CODE__HEIN_RATIO_20];
                    if (config == null) throw new ArgumentNullException(SERVICE_TYPE_CODE__HEIN_RATIO_20);
                    var value = string.IsNullOrEmpty(config.VALUE) ? (string.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                    serviceTypeIdHeinRatio20 = decimal.Parse(value);
                }
                return serviceTypeIdHeinRatio20;
            }
            set
            {
                serviceTypeIdHeinRatio20 = value;
            }
        }

        private static decimal? serviceTypeIdHeinRatio5;
        public static decimal? SERVICE_TYPE_ID__HEIN_RATIO_5
        {
            get
            {
                if (serviceTypeIdHeinRatio5 == null)
                {
                    var config = Loader.dictionaryConfig[SERVICE_TYPE_CODE__HEIN_RATIO_5];
                    if (config == null) throw new ArgumentNullException(SERVICE_TYPE_CODE__HEIN_RATIO_5);
                    var value = string.IsNullOrEmpty(config.VALUE) ? (string.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                    serviceTypeIdHeinRatio5 = decimal.Parse(value);
                }
                return serviceTypeIdHeinRatio5;
            }
            set
            {
                serviceTypeIdHeinRatio5 = value;
            }
        }

        private static List<long> serviceTypeIdTestBlood;
        public static List<long> SERVICE_TYPE_ID__TEST_BLOOD
        {
            get
            {
                if (serviceTypeIdTestBlood == null)
                {
                    serviceTypeIdTestBlood = GetListId(SERVICE_TYPE_CODE__TEST_BLOOD);
                }
                return serviceTypeIdTestBlood;
            }
            set
            {
                serviceTypeIdTestBlood = value;
            }
        }

        //private static List<long> serviceTypeIdCls;
        //public static List<long> SERVICE_TYPE_ID__CLS
        //{
        //    get
        //    {
        //        if (serviceTypeIdCls == null || serviceTypeIdCls.Count == 0)
        //        {
        //            serviceTypeIdCls = GetListId(SERVICE_TYPE_CODE__CLS);
        //        }
        //        return serviceTypeIdCls;
        //    }
        //    set
        //    {
        //        serviceTypeIdCls = value;
        //    }
        //}

        //private static long serviceTypeIdBlood;
        //public static long SERVICE_TYPE_ID__BLOOD
        //{
        //    get
        //    {
        //        if (serviceTypeIdBlood == 0)
        //        {
        //            serviceTypeIdBlood = GetId(SERVICE_TYPE_CODE__BLOOD);
        //        }
        //        return serviceTypeIdBlood;
        //    }
        //    set
        //    {
        //        serviceTypeIdBlood = value;
        //    }
        //}

        //private static long serviceTypeIdExam;
        //public static long SERVICE_TYPE_ID__EXAM
        //{
        //    get
        //    {
        //        if (serviceTypeIdExam == 0)
        //        {
        //            serviceTypeIdExam = GetId(SERVICE_TYPE_CODE__EXAM);
        //        }
        //        return serviceTypeIdExam;
        //    }
        //    set
        //    {
        //        serviceTypeIdExam = value;
        //    }
        //}

        //private static long serviceTypeIdTest;
        //public static long SERVICE_TYPE_ID__TEST
        //{
        //    get
        //    {
        //        if (serviceTypeIdTest == 0)
        //        {
        //            serviceTypeIdTest = GetId(SERVICE_TYPE_CODE__TEST);
        //        }
        //        return serviceTypeIdTest;
        //    }
        //    set
        //    {
        //        serviceTypeIdTest = value;
        //    }
        //}

        //private static long serviceTypeIdDiim;
        //public static long SERVICE_TYPE_ID__DIIM
        //{
        //    get
        //    {
        //        if (serviceTypeIdDiim == 0)
        //        {
        //            serviceTypeIdDiim = GetId(SERVICE_TYPE_CODE__DIIM);
        //        }
        //        return serviceTypeIdDiim;
        //    }
        //    set
        //    {
        //        serviceTypeIdDiim = value;
        //    }
        //}

        //private static long serviceTypeIdMisu;
        //public static long SERVICE_TYPE_ID__MISU
        //{
        //    get
        //    {
        //        if (serviceTypeIdMisu == 0)
        //        {
        //            serviceTypeIdMisu = GetId(SERVICE_TYPE_CODE__MISU);
        //        }
        //        return serviceTypeIdMisu;
        //    }
        //    set
        //    {
        //        serviceTypeIdMisu = value;
        //    }
        //}

        //private static long serviceTypeIdSurg;
        //public static long SERVICE_TYPE_ID__SURG
        //{
        //    get
        //    {
        //        if (serviceTypeIdSurg == 0)
        //        {
        //            serviceTypeIdSurg = GetId(SERVICE_TYPE_CODE__SURG);
        //        }
        //        return serviceTypeIdSurg;
        //    }
        //    set
        //    {
        //        serviceTypeIdSurg = value;
        //    }
        //}

        //private static long serviceTypeIdFuex;
        //public static long SERVICE_TYPE_ID__FUEX
        //{
        //    get
        //    {
        //        if (serviceTypeIdFuex == 0)
        //        {
        //            serviceTypeIdFuex = GetId(SERVICE_TYPE_CODE__FUEX);
        //        }
        //        return serviceTypeIdFuex;
        //    }
        //    set
        //    {
        //        serviceTypeIdFuex = value;
        //    }
        //}

        //private static long serviceTypeIdEndo;
        //public static long SERVICE_TYPE_ID__ENDO
        //{
        //    get
        //    {
        //        if (serviceTypeIdEndo == 0)
        //        {
        //            serviceTypeIdEndo = GetId(SERVICE_TYPE_CODE__ENDO);
        //        }
        //        return serviceTypeIdEndo;
        //    }
        //    set
        //    {
        //        serviceTypeIdEndo = value;
        //    }
        //}

        //private static long serviceTypeIdSuim;
        //public static long SERVICE_TYPE_ID__SUIM
        //{
        //    get
        //    {
        //        if (serviceTypeIdSuim == 0)
        //        {
        //            serviceTypeIdSuim = GetId(SERVICE_TYPE_CODE__SUIM);
        //        }
        //        return serviceTypeIdSuim;
        //    }
        //    set
        //    {
        //        serviceTypeIdSuim = value;
        //    }
        //}

        //private static long serviceTypeIdBed;
        //public static long SERVICE_TYPE_ID__BED
        //{
        //    get
        //    {
        //        if (serviceTypeIdBed == 0)
        //        {
        //            serviceTypeIdBed = GetId(SERVICE_TYPE_CODE__BED);
        //        }
        //        return serviceTypeIdBed;
        //    }
        //    set
        //    {
        //        serviceTypeIdBed = value;
        //    }
        //}

        //private static long serviceTypeIdOther;
        //public static long SERVICE_TYPE_ID__OTHER
        //{
        //    get
        //    {
        //        if (serviceTypeIdOther == 0)
        //        {
        //            serviceTypeIdOther = GetId(SERVICE_TYPE_CODE__OTHER);
        //        }
        //        return serviceTypeIdOther;
        //    }
        //    set
        //    {
        //        serviceTypeIdOther = value;
        //    }
        //}

        //private static long serviceTypeIdMedi;
        //public static long SERVICE_TYPE_ID__MEDI
        //{
        //    get
        //    {
        //        if (serviceTypeIdMedi == 0)
        //        {
        //            serviceTypeIdMedi = GetId(SERVICE_TYPE_CODE__MEDI);
        //        }
        //        return serviceTypeIdMedi;
        //    }
        //    set
        //    {
        //        serviceTypeIdMedi = value;
        //    }
        //}

        //private static long serviceTypeIdMate;
        //public static long SERVICE_TYPE_ID__MATE
        //{
        //    get
        //    {
        //        if (serviceTypeIdMate == 0)
        //        {
        //            serviceTypeIdMate = GetId(SERVICE_TYPE_CODE__MATE);
        //        }
        //        return serviceTypeIdMate;
        //    }
        //    set
        //    {
        //        serviceTypeIdMate = value;
        //    }
        //}

        private static List<HIS_SERVICE_TYPE> hisServiceTypes;
        public static List<HIS_SERVICE_TYPE> HisServiceTypes
        {
            get
            {
                if (hisServiceTypes == null || hisServiceTypes.Count == 0)
                {
                    HisServiceTypeFilterQuery filter = new HisServiceTypeFilterQuery();
                    hisServiceTypes = new HisServiceTypeManager().Get(filter);
                }
                return hisServiceTypes;
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
                HisServiceTypeFilterQuery filter = new HisServiceTypeFilterQuery();
                //filter.SERVICE_RYPE_CODE = value;
                var data = new HisServiceTypeManager().Get(filter).FirstOrDefault(o => o.SERVICE_TYPE_CODE == value);
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
                if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
                var data = HisServiceTypes.Where(o => arr.Contains(o.SERVICE_TYPE_CODE)).ToList();
                if (!(data != null && data.Count > 0)) throw new ArgumentNullException(code + LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => config), config));
                result = data.Select(o => o.ID).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Info("CODE:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => code), code));
                LogSystem.Error(ex);
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                serviceTypeIdHeinRatio0 = null;
                serviceTypeIdHeinRatio40 = null;
                serviceTypeIdHeinRatio20 = null;
                serviceTypeIdHeinRatio5 = null;
                serviceTypeIdTestBlood = null;
                //serviceTypeIdCls = null;
                hisServiceTypes = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
