using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Config
{
    public class HeinCardNumberTypeCFG
    {
        private const string MRS__HEIN_CARD_NUMBER__HEIN_TYPE__01 = "MRS.HEIN_CARD_NUMBER.HEIN_TYPE.TYPE_01";//
        private const string MRS__HEIN_CARD_NUMBER__HEIN_TYPE__02 = "MRS.HEIN_CARD_NUMBER.HEIN_TYPE.TYPE_02";
        private const string MRS__HEIN_CARD_NUMBER__HEIN_TYPE__BILL_FOOD = "MRS.HEIN_CARD_NUMBER.HEIN_TYPE.TYPE_BILL_FOOD";

        private static List<string> heinCardNumber__HeinType__01;
        public static List<string> HeinCardNumber__HeinType__01
        {
            get
            {
                if (heinCardNumber__HeinType__01 == null)
                {
                    heinCardNumber__HeinType__01 = GetTypes(MRS__HEIN_CARD_NUMBER__HEIN_TYPE__01);
                }
                return heinCardNumber__HeinType__01;
            }
        }

        private static List<string> heinCardNumber__HeinType__02;
        public static List<string> HeinCardNumber__HeinType__02
        {
            get
            {
                if (heinCardNumber__HeinType__02 == null)
                {
                    heinCardNumber__HeinType__02 = GetTypes(MRS__HEIN_CARD_NUMBER__HEIN_TYPE__02);
                }
                return heinCardNumber__HeinType__02;
            }
        }

        private static List<string> heinCardNumber__HeinType__Bill_Food;
        public static List<string> HeinCardNumber__HeinType__Bill_Food
        {
            get
            {
                if (heinCardNumber__HeinType__Bill_Food == null || heinCardNumber__HeinType__Bill_Food.Count == 0)
                {
                    heinCardNumber__HeinType__Bill_Food = GetTypes(MRS__HEIN_CARD_NUMBER__HEIN_TYPE__BILL_FOOD);
                }
                return heinCardNumber__HeinType__Bill_Food;
            }
        }

        private static List<string> heinCardNumber__HeinType__All;
        public static List<string> HeinCardNumber__HeinType__All
        {
            get
            {
                if (heinCardNumber__HeinType__All == null)
                {
                    heinCardNumber__HeinType__All = new List<string>();
                    heinCardNumber__HeinType__All.AddRange(HeinCardNumber__HeinType__01);
                    heinCardNumber__HeinType__All.AddRange(HeinCardNumber__HeinType__02);
                }
                return heinCardNumber__HeinType__All;
            }
        }

        private static List<string> GetTypes(string code)
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

        public static void Refresh()
        {
            try
            {
                heinCardNumber__HeinType__01 = null;
                heinCardNumber__HeinType__02 = null;
                heinCardNumber__HeinType__Bill_Food = null;
                heinCardNumber__HeinType__All = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
