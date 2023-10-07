using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisServicePaty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Config
{
    public class HisServicePatyCFG
    {
        private static Dictionary<long, List<V_HIS_SERVICE_PATY>> dicServicePaty;
        public static Dictionary<long, List<V_HIS_SERVICE_PATY>> DicServicePaty
        {
            get
            {
                if (dicServicePaty == null || dicServicePaty.Count <= 0)
                {
                    dicServicePaty = GetDic();
                }

                return dicServicePaty;
            }
            set
            {
                dicServicePaty = value;
            }
        }

        private static List<V_HIS_SERVICE_PATY> listServicePaty;
        public static List<V_HIS_SERVICE_PATY> DATAs
        {
            get
            {
                if (listServicePaty == null || listServicePaty.Count <= 0)
                {
                    listServicePaty = GetAll();
                }

                return listServicePaty;
            }
            set
            {
                listServicePaty = value;
            }
        }

        private static Dictionary<long, List<V_HIS_SERVICE_PATY>> GetDic()
        {
            Dictionary<long, List<V_HIS_SERVICE_PATY>> result = null;
            try
            {
                var data = DATAs;
                if (DATAs != null && DATAs.Count > 0)
                {
                    result = new Dictionary<long, List<V_HIS_SERVICE_PATY>>();
                    foreach (var item in DATAs)
                    {
                        if (!result.ContainsKey(item.SERVICE_ID))
                            result[item.SERVICE_ID] = new List<V_HIS_SERVICE_PATY>();

                        result[item.SERVICE_ID].Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private static List<V_HIS_SERVICE_PATY> GetAll()
        {
            List<V_HIS_SERVICE_PATY> result = null;
            try
            {
                HisServicePatyViewFilterQuery filter = new HisServicePatyViewFilterQuery();
                result = new HisServicePatyManager().GetView(filter);
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                listServicePaty = GetAll();
                dicServicePaty = GetDic();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
