using Inventec.Common.Logging;
using MOS.MANAGER.HisMediStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Config
{
    public class HisMediStockTypeCFG
    {
        private const string MRS_MEDI_STOCK_CODE_NOI_TRU = "MRS.HIS_RS.HIS_MEDI_STOCK.MEDI_STOCK_CODE.NOI_TRU";
        private const string MRS_MEDI_STOCK_CODE_NGOAI_TRU = "MRS.HIS_RS.HIS_MEDI_STOCK.MEDI_STOCK_CODE.NGOAI_TRU";
        private const string MRS_MEDI_STOCK_CODE_TONG = "MRS.HIS_RS.HIS_MEDI_STOCK.MEDI_STOCK_CODE.TONG";
        private const string MRS_MEDI_STOCK_CODE_NTCT = "MRS.HIS_RS.HIS_MEDI_STOCK.MEDI_STOCK_CODE.NTCT";
        private const string MRS_MEDI_STOCK_CODE_NGTCT = "MRS.HIS_RS.HIS_MEDI_STOCK.MEDI_STOCK_CODE.NGTCT";
        private const string MRS_MEDI_STOCK_CODE_TCT = "MRS.HIS_RS.HIS_MEDI_STOCK.MEDI_STOCK_CODE.TCT";

        private static List<long> MediStockNoiTru;
        public static List<long> MEDI_STOCK_ID_NOI_TRU
        {
            get
            {
                if (MediStockNoiTru == null || MediStockNoiTru.Count == 0)
                {
                    MediStockNoiTru = GetIds(MRS_MEDI_STOCK_CODE_NOI_TRU);
                }
                return MediStockNoiTru;
            }
            set
            {
                MediStockNoiTru = value;
            }
        }

        private static List<long> MediStockTong;
        public static List<long> MEDI_STOCK_ID_TONG
        {
            get
            {
                if (MediStockTong == null || MediStockTong.Count == 0)
                {
                    MediStockTong = GetIds(MRS_MEDI_STOCK_CODE_TONG);
                }
                return MediStockTong;
            }
            set
            {
                MediStockTong = value;
            }
        }

        private static List<long> MediStockNgoaiTru;
        public static List<long> MEDI_STOCK_ID_NGOAI_TRU
        {
            get
            {
                if (MediStockNgoaiTru == null || MediStockNgoaiTru.Count == 0)
                {
                    MediStockNgoaiTru = GetIds(MRS_MEDI_STOCK_CODE_NGOAI_TRU);
                }
                return MediStockNgoaiTru;
            }
            set
            {
                MediStockNgoaiTru = value;
            }
        }

        private static List<long> MediStockNoiTruCT;
        public static List<long> MEDI_STOCK_ID_NTCT
        {
            get
            {
                if (MediStockNoiTruCT == null || MediStockNoiTruCT.Count == 0)
                {
                    MediStockNoiTruCT = GetIds(MRS_MEDI_STOCK_CODE_NTCT);
                }
                return MediStockNoiTruCT;
            }
            set
            {
                MediStockNoiTruCT = value;
            }
        }

        private static List<long> MediStockTongCT;
        public static List<long> MEDI_STOCK_ID_TCT
        {
            get
            {
                if (MediStockTongCT == null || MediStockTongCT.Count == 0)
                {
                    MediStockTongCT = GetIds(MRS_MEDI_STOCK_CODE_TCT);
                }
                return MediStockTongCT;
            }
            set
            {
                MediStockTongCT = value;
            }
        }

        private static List<long> MediStockNgoaiTruCT;
        public static List<long> MEDI_STOCK_ID_NGTCT
        {
            get
            {
                if (MediStockNgoaiTruCT == null || MediStockNgoaiTruCT.Count == 0)
                {
                    MediStockNgoaiTruCT = GetIds(MRS_MEDI_STOCK_CODE_NGTCT);
                }
                return MediStockNgoaiTruCT;
            }
            set
            {
                MediStockNgoaiTruCT = value;
            }
        }

        private static List<string> GetCode(string code)
        {
            List<string> result = new List<string>();
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
                result.AddRange(value.Split(new char[] { ',' }));
                if (result == null) throw new ArgumentNullException(code + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => result), result));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new List<string>();
            }
            return result;
        }

        private static List<long> GetIds(string code)
        {
            List<long> result = null;
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
                string[] ar = value.Split(new char[] { ',' });

                var data = HisMediStockCFG.HisMediStocks.Where(o => ar.Contains(o.MEDI_STOCK_CODE)).ToList();
                if (!(data != null && data.Count > 0)) throw new ArgumentNullException(code + LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => config), config));
                result = data.Select(o => o.ID).ToList();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                MediStockNoiTru = null;
                MediStockTong = null;
                MediStockNgoaiTru = null;
                MediStockNoiTruCT = null;
                MediStockTongCT = null;
                MediStockNgoaiTruCT = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
