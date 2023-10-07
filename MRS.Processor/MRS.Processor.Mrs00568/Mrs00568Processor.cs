using Inventec.Core;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00568
{
    class Mrs00568Processor : AbstractProcessor
    {
        Mrs00568Filter castFilter = null;
        long YEAR;
        List<TYT.EFMODEL.DataModels.TYT_NERVES> ListTytNerves = new List<TYT.EFMODEL.DataModels.TYT_NERVES>();
        List<Mrs00568RDO> ListRDO = new List<Mrs00568RDO>();

        public Mrs00568Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00568Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                var param = new CommonParam();
                castFilter = (Mrs00568Filter)this.reportFilter;
                YEAR = (long)castFilter.YEAR / 10000000000;

                var branch = new MOS.MANAGER.HisBranch.HisBranchManager(param).GetById(castFilter.BRANCH_ID);

                var tytNervesFilter = new TYT.MANAGER.Core.TytNerves.Get.TytNervesFilterQuery();
                tytNervesFilter.BRANCH_CODE = branch.BRANCH_CODE;

                ListTytNerves = new TYT.MANAGER.Manager.TytNervesManager(param).Get<List<TYT.EFMODEL.DataModels.TYT_NERVES>>(tytNervesFilter);

                if (param.HasException)
                {
                    result = false;
                    Inventec.Common.Logging.LogSystem.Error("Co loi khi get du lieu Mrs00568");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(ListTytNerves))
                {
                    ListRDO.Clear();
                    foreach (var item in ListTytNerves)
                    {
                        if (String.IsNullOrWhiteSpace(item.MEDICINE_MONITOR))
                            continue;
                        Mrs00568RDO rdo = null;
                        string[] year = item.MEDICINE_MONITOR.Split(',');
                        foreach (var month in year)
                        {
                            if (String.IsNullOrWhiteSpace(month)) continue;

                            var numOfMonth = ProcessNumber(month);
                            if (YEAR == long.Parse(numOfMonth.Substring(0, 4)))
                            {
                                if (rdo == null)
                                {
                                    rdo = new Mrs00568RDO();
                                    Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00568RDO>(rdo, item);
                                }

                                SetMonth(ref rdo, numOfMonth);
                            }
                        }

                        if (IsNotNull(rdo))
                        {
                            ListRDO.Add(rdo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ListRDO.Clear();
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private string ProcessNumber(string num)
        {
            string result = "";
            try
            {
                if (!String.IsNullOrWhiteSpace(num))
                {
                    var a = num.Trim();
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (Char.IsDigit(a[i]))
                        {
                            result += a[i];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void SetMonth(ref Mrs00568RDO rdo, string time)
        {
            try
            {
                if (rdo != null && !String.IsNullOrWhiteSpace(time))
                {
                    var month = time.Substring(4, 2);

                    switch (month)
                    {
                        case "01":
                            rdo.MEDICINE_MONITOR_1 = "X";
                            break;
                        case "02":
                            rdo.MEDICINE_MONITOR_2 = "X";
                            break;
                        case "03":
                            rdo.MEDICINE_MONITOR_3 = "X";
                            break;
                        case "04":
                            rdo.MEDICINE_MONITOR_4 = "X";
                            break;
                        case "05":
                            rdo.MEDICINE_MONITOR_5 = "X";
                            break;
                        case "06":
                            rdo.MEDICINE_MONITOR_6 = "X";
                            break;
                        case "07":
                            rdo.MEDICINE_MONITOR_7 = "X";
                            break;
                        case "08":
                            rdo.MEDICINE_MONITOR_8 = "X";
                            break;
                        case "09":
                            rdo.MEDICINE_MONITOR_9 = "X";
                            break;
                        case "10":
                            rdo.MEDICINE_MONITOR_10 = "X";
                            break;
                        case "11":
                            rdo.MEDICINE_MONITOR_11 = "X";
                            break;
                        case "12":
                            rdo.MEDICINE_MONITOR_12 = "X";
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("YEAR", YEAR);

                objectTag.AddObjectData(store, "Report", ListRDO);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
