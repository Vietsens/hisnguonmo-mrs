using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.Logging;
using SDA.EFMODEL.DataModels;
using MOS.MANAGER.HisReportTypeCat;

namespace MRS.MANAGER.Config
{
    public class HisReportTypeCatCFG
    {
        private const string REPORT_TYPE_CODE__MRS00159 = "MRS.HIS_REPORT_TYPE_CAT.REPORT_TYPE_CODE.MRS00159";//Lấy toàn bộ thông tin của báo cáo MRS00159
        //private const string CATEGORY_CODE__159_XN = "MRS.HIS_SERVICE_TYPE_CAT.CATEGORY_CODE.159-XN";//MRS00159 - Xét nghiệm
        //private const string CATEGORY_CODE__159_CDHA = "MRS.HIS_SERVICE_TYPE_CAT.CATEGORY_CODE.159-CDHA";//MRS00159 - CĐHA, TDCN
        //private const string CATEGORY_CODE__159_Thuoc = "MRS.HIS_SERVICE_TYPE_CAT.CATEGORY_CODE.159-Thuoc";//MRS00159 - Thuốc, DT
        //private const string CATEGORY_CODE__159_M = "MRS.HIS_SERVICE_TYPE_CAT.CATEGORY_CODE.159-M";//MRS00159 - Máu
        //private const string CATEGORY_CODE__159_VTYT = "MRS.HIS_SERVICE_TYPE_CAT.CATEGORY_CODE.159-VTYT";//MRS00159 - VTYT
        //private const string CATEGORY_CODE__159_DVKTC = "MRS.HIS_SERVICE_TYPE_CAT.CATEGORY_CODE.159-DVKTC";//MRS00159 - DVKTC
        //private const string CATEGORY_CODE__159_ThuocK = "MRS.HIS_SERVICE_TYPE_CAT.CATEGORY_CODE.159-ThuocK";//MRS00159 - Thuốc K, thải ghép
        //private const string CATEGORY_CODE__159_TK = "MRS.HIS_SERVICE_TYPE_CAT.CATEGORY_CODE.159-TK";//MRS00159 - Tiền khám
        //private const string CATEGORY_CODE__159_TG = "MRS.HIS_SERVICE_TYPE_CAT.CATEGORY_CODE.159-TG";//MRS00159 - Tiền giường
        //private const string CATEGORY_CODE__159_CPVC = "MRS.HIS_SERVICE_TYPE_CAT.CATEGORY_CODE.159-CPVC";//MRS00159 - CPCV
        //private const string CATEGORY_CODE__159_TC = "MRS.HIS_SERVICE_TYPE_CAT.CATEGORY_CODE.159-TC";//MRS00159 - Tổng cộng
        //private const string CATEGORY_CODE__159_Mien = "MRS.HIS_SERVICE_TYPE_CAT.CATEGORY_CODE.159-Mien";//MRS00159 - Miễn
        //private const string CATEGORY_CODE__159_PTTT = "MRS.HIS_SERVICE_TYPE_CAT.CATEGORY_CODE.159-PTTT";//MRS00159 - PTTT
        //private const string CATECORY_CODE__MRS00172 = "MRS.REPORT_RETY_CAT.CATECORY_CODE.MRS00172";//Nhóm dịch vụ báo cáo MRS00172
        //private const string CATECORY_CODE__MRS00171 = "MRS.REPORT_RETY_CAT.CATECORY_CODE.MRS00171";//Nhóm dịch vụ báo cáo MRS00171
        //private const string CATECORY_CODE__MRS00177 = "MRS.REPORT_RETY_CAT.CATECORY_CODE.MRS00177";//Nhóm dịch vụ báo cáo MRS00177
        //private const string CATECORY_CODE__MRS00179_VATTU = "MRS.HIS_SERVICE_TYPE_CAT.CATEGORY_CODE.179_VATTU";//Nhóm vật tư báo cáo MRS00179
        //private const string CATECORY_CODE__MRS00179_THUOC = "MRS.HIS_SERVICE_TYPE_CAT.CATEGORY_CODE.179_THUOC";//Nhóm thuốc báo cáo MRS00179
        //private const string CATECORY_CODE__MRS00179_MAU = "MRS.HIS_SERVICE_TYPE_CAT.CATEGORY_CODE.179_MAU";//Nhóm máu báo cáo MRS00179
        //private const string CATECORY_CODE__MRS00179_HOACHAT = "MRS.HIS_SERVICE_TYPE_CAT.CATEGORY_CODE.179_HC";//Nhóm hóa chất báo cáo MRS00179


        //private static long CatecoryCodeMrs00179_HOACHAT;//Nhóm hóa chất báo cáo MRS00179
        //public static long CATECORY_CODE_MRS00179_HOACHAT
        //{
        //    get
        //    {
        //        if (CatecoryCodeMrs00179_HOACHAT == 0)
        //        {
        //            CatecoryCodeMrs00179_HOACHAT = GetId(CATECORY_CODE__MRS00179_HOACHAT);
        //        }
        //        return CatecoryCodeMrs00179_HOACHAT;
        //    }
        //    set
        //    {
        //        CatecoryCodeMrs00179_HOACHAT = value;
        //    }
        //}

        //private static long CatecoryCodeMrs00179_MAU;//Nhóm máu báo cáo MRS00179
        //public static long CATECORY_CODE_MRS00179_MAU
        //{
        //    get
        //    {
        //        if (CatecoryCodeMrs00179_MAU == 0)
        //        {
        //            CatecoryCodeMrs00179_MAU = GetId(CATECORY_CODE__MRS00179_MAU);
        //        }
        //        return CatecoryCodeMrs00179_MAU;
        //    }
        //    set
        //    {
        //        CatecoryCodeMrs00179_MAU = value;
        //    }
        //}

        //private static long CatecoryCodeMrs00179_THUOC;//Nhóm thuốc báo cáo MRS00179
        //public static long CATECORY_CODE_MRS00179_THUOC
        //{
        //    get
        //    {
        //        if (CatecoryCodeMrs00179_THUOC == 0)
        //        {
        //            CatecoryCodeMrs00179_THUOC = GetId(CATECORY_CODE__MRS00179_THUOC);
        //        }
        //        return CatecoryCodeMrs00179_THUOC;
        //    }
        //    set
        //    {
        //        CatecoryCodeMrs00179_THUOC = value;
        //    }
        //}


        //private static long CatecoryCodeMrs00179_VATTU;//Nhóm vật tư báo cáo MRS00179
        //public static long CATECORY_CODE_MRS00179_VATTU
        //{
        //    get
        //    {
        //        if (CatecoryCodeMrs00179_VATTU == 0)
        //        {
        //            CatecoryCodeMrs00179_VATTU = GetId(CATECORY_CODE__MRS00179_VATTU);
        //        }
        //        return CatecoryCodeMrs00179_VATTU;
        //    }
        //    set
        //    {
        //        CatecoryCodeMrs00179_VATTU = value;
        //    }
        //}

        //private static long CatecoryCodeMrs00177;
        //public static long CATECORY_CODE_MRS00177
        //{
        //    get
        //    {
        //        if (CatecoryCodeMrs00177 == 0)
        //        {
        //            CatecoryCodeMrs00177 = GetId(CATECORY_CODE__MRS00177);
        //        }
        //        return CatecoryCodeMrs00177;
        //    }
        //    set
        //    {
        //        CatecoryCodeMrs00177 = value;
        //    }
        //}

        //private static long CatecoryCodeMrs00171;
        //public static long CATECORY_CODE_MRS00171
        //{
        //    get
        //    {
        //        if (CatecoryCodeMrs00171 == 0)
        //        {
        //            CatecoryCodeMrs00171 = GetId(CATECORY_CODE__MRS00171);
        //        }
        //        return CatecoryCodeMrs00171;
        //    }
        //    set
        //    {
        //        CatecoryCodeMrs00171 = value;
        //    }
        //}

        //private static long CatecoryCodeMrs00172;
        //public static long CATECORY_CODE_MRS00172
        //{
        //    get
        //    {
        //        if (CatecoryCodeMrs00172 == 0)
        //        {
        //            CatecoryCodeMrs00172 = GetId(CATECORY_CODE__MRS00172);
        //        }
        //        return CatecoryCodeMrs00172;
        //    }
        //    set
        //    {
        //        CatecoryCodeMrs00172 = value;
        //    }
        //}

        //private static long CategoryCode_159_PTTT;
        //public static long CATEGORY_CODE_159_PTTT
        //{
        //    get
        //    {
        //        if (CategoryCode_159_PTTT == 0)
        //        {
        //            CategoryCode_159_PTTT = GetId(CATEGORY_CODE__159_PTTT);
        //        }
        //        return CategoryCode_159_PTTT;
        //    }
        //    set
        //    {
        //        CategoryCode_159_PTTT = value;
        //    }
        //}

        //private static long CategoryCode_159_CDHA;
        //public static long CATEGORY_CODE_159_CDHA
        //{
        //    get
        //    {
        //        if (CategoryCode_159_CDHA == 0)
        //        {
        //            CategoryCode_159_CDHA = GetId(CATEGORY_CODE__159_CDHA);
        //        }
        //        return CategoryCode_159_CDHA;
        //    }
        //    set
        //    {
        //        CategoryCode_159_CDHA = value;
        //    }
        //}

        //private static long CategoryCode_159_Thuoc;
        //public static long CATEGORY_CODE_159_Thuoc
        //{
        //    get
        //    {
        //        if (CategoryCode_159_Thuoc == 0)
        //        {
        //            CategoryCode_159_Thuoc = GetId(CATEGORY_CODE__159_Thuoc);
        //        }
        //        return CategoryCode_159_Thuoc;
        //    }
        //    set
        //    {
        //        CategoryCode_159_Thuoc = value;
        //    }
        //}

        //private static long CategoryCode_159_M;
        //public static long CATEGORY_CODE_159_M
        //{
        //    get
        //    {
        //        if (CategoryCode_159_M == 0)
        //        {
        //            CategoryCode_159_M = GetId(CATEGORY_CODE__159_M);
        //        }
        //        return CategoryCode_159_M;
        //    }
        //    set
        //    {
        //        CategoryCode_159_M = value;
        //    }
        //}

        //private static long CategoryCode_159_VTYT;
        //public static long CATEGORY_CODE_159_VTYT
        //{
        //    get
        //    {
        //        if (CategoryCode_159_VTYT == 0)
        //        {
        //            CategoryCode_159_VTYT = GetId(CATEGORY_CODE__159_VTYT);
        //        }
        //        return CategoryCode_159_VTYT;
        //    }
        //    set
        //    {
        //        CategoryCode_159_VTYT = value;
        //    }
        //}

        //private static long CategoryCode_159_DVKTC;
        //public static long CATEGORY_CODE_159_DVKTC
        //{
        //    get
        //    {
        //        if (CategoryCode_159_DVKTC == 0)
        //        {
        //            CategoryCode_159_DVKTC = GetId(CATEGORY_CODE__159_DVKTC);
        //        }
        //        return CategoryCode_159_DVKTC;
        //    }
        //    set
        //    {
        //        CategoryCode_159_DVKTC = value;
        //    }
        //}

        //private static long CategoryCode_159_ThuocK;
        //public static long CATEGORY_CODE_159_ThuocK
        //{
        //    get
        //    {
        //        if (CategoryCode_159_ThuocK == 0)
        //        {
        //            CategoryCode_159_ThuocK = GetId(CATEGORY_CODE__159_ThuocK);
        //        }
        //        return CategoryCode_159_ThuocK;
        //    }
        //    set
        //    {
        //        CategoryCode_159_ThuocK = value;
        //    }
        //}

        //private static long CategoryCode_159_TK;
        //public static long CATEGORY_CODE_159_TK
        //{
        //    get
        //    {
        //        if (CategoryCode_159_TK == 0)
        //        {
        //            CategoryCode_159_TK = GetId(CATEGORY_CODE__159_TK);
        //        }
        //        return CategoryCode_159_TK;
        //    }
        //    set
        //    {
        //        CategoryCode_159_TK = value;
        //    }
        //}

        //private static long CategoryCode_159_TG;
        //public static long CATEGORY_CODE_159_TG
        //{
        //    get
        //    {
        //        if (CategoryCode_159_TG == 0)
        //        {
        //            CategoryCode_159_TG = GetId(CATEGORY_CODE__159_TG);
        //        }
        //        return CategoryCode_159_TG;
        //    }
        //    set
        //    {
        //        CategoryCode_159_TG = value;
        //    }
        //}

        //private static long CategoryCode_159_CPVC;
        //public static long CATEGORY_CODE_159_CPVC
        //{
        //    get
        //    {
        //        if (CategoryCode_159_CPVC == 0)
        //        {
        //            CategoryCode_159_CPVC = GetId(CATEGORY_CODE__159_CPVC);
        //        }
        //        return CategoryCode_159_CPVC;
        //    }
        //    set
        //    {
        //        CategoryCode_159_CPVC = value;
        //    }
        //}

        //private static long CategoryCode_159_TC;
        //public static long CATEGORY_CODE_159_TC
        //{
        //    get
        //    {
        //        if (CategoryCode_159_TC == 0)
        //        {
        //            CategoryCode_159_TC = GetId(CATEGORY_CODE__159_TC);
        //        }
        //        return CategoryCode_159_TC;
        //    }
        //    set
        //    {
        //        CategoryCode_159_TC = value;
        //    }
        //}

        //private static long CategoryCode_159_Mien;
        //public static long CATEGORY_CODE_159_Mien
        //{
        //    get
        //    {
        //        if (CategoryCode_159_Mien == 0)
        //        {
        //            CategoryCode_159_Mien = GetId(CATEGORY_CODE__159_Mien);
        //        }
        //        return CategoryCode_159_Mien;
        //    }
        //    set
        //    {
        //        CategoryCode_159_Mien = value;
        //    }
        //}

        //private static long CategoryCode_XN;
        //public static long CATEGORY_CODE_XN
        //{
        //    get
        //    {
        //        if (CategoryCode_XN == 0)
        //        {
        //            CategoryCode_XN = GetId(CATEGORY_CODE__159_XN);
        //        }
        //        return CategoryCode_XN;
        //    }
        //    set
        //    {
        //        CategoryCode_XN = value;
        //    }
        //}

        private static string ReportTypeCodeMrs00159;
        public static string REPORT_TYPE_CODE_MRS00159
        {
            get
            {
                if (string.IsNullOrEmpty(ReportTypeCodeMrs00159))
                {
                    ReportTypeCodeMrs00159 = GetCategoryCode(REPORT_TYPE_CODE__MRS00159);
                }
                return ReportTypeCodeMrs00159;
            }
            set
            {
                ReportTypeCodeMrs00159 = value;
            }
        }

        private static long GetId(string code)
        {
            long result = 0;
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                var value = string.IsNullOrEmpty(config.VALUE) ? (string.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
                var filter = new HisReportTypeCatFilterQuery();
                var data = new HisReportTypeCatManager().Get(filter).FirstOrDefault(s => s.CATEGORY_CODE == value);
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

        private static string GetCategoryCode(string code)
        {
            var result = string.Empty;
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                var value = string.IsNullOrEmpty(config.VALUE) ? (string.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
                var filter = new HisReportTypeCatFilterQuery();
                var data = new HisReportTypeCatManager().Get(filter).FirstOrDefault(s => s.CATEGORY_CODE == value);
                if (!(data != null && data.ID > 0)) throw new ArgumentNullException(code + LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => config), config));
                result = data.CATEGORY_CODE;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = string.Empty;
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                ReportTypeCodeMrs00159 = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
