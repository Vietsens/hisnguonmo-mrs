using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMestType;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisMediStock;
using MRS.Processor.Mrs00637;
using System;
using System.Collections.Generic;
using System.Linq;
using MRS.MANAGER.Config;
using MRS.MANAGER.Base;

namespace MRS.Processor.Mrs00637
{
    public class Mrs00637Processor : AbstractProcessor
    {
        private List<Mrs00637RDO> listRdo = new List<Mrs00637RDO>();
        CommonParam paramGet = new CommonParam();
        List<Mrs00637RDO> ListRdo = new List<Mrs00637RDO>();
        List<HIS_MEDI_STOCK> listMedistock = new List<HIS_MEDI_STOCK>();
        List<HIS_EXP_MEST_TYPE> listHisExpMestType = new List<HIS_EXP_MEST_TYPE>();
        List<DetailRDOMedicine> listHisExpMestMedicine = new List<DetailRDOMedicine>();
        List<DetailRDOMaterial> listHisExpMestMaterial = new List<DetailRDOMaterial>();
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();

        string DATE_STR = "";

        public Mrs00637Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00637Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                List<string> listDate = new List<string>();
                listDate = ConvertToListStringTime(((Mrs00637Filter)this.reportFilter).TIME_FROM, ((Mrs00637Filter)this.reportFilter).TIME_TO);
                DATE_STR = String.Join(" ", listDate);
                //get dữ liệu:
                HisMediStockFilterQuery hisMediStockFilter = new HisMediStockFilterQuery();
                hisMediStockFilter.IDs = ((Mrs00637Filter)this.reportFilter).MEDI_STOCK_IDs;
                listMedistock = new HisMediStockManager(paramGet).Get(hisMediStockFilter);

                listHisExpMestMedicine = new ManagerSql().GetMedicine((Mrs00637Filter)this.reportFilter);
                listHisExpMestMaterial = new ManagerSql().GetMaterial((Mrs00637Filter)this.reportFilter);

                // get mobaImpMest
                HisImpMestViewFilterQuery hisImpMestViewFilter = new HisImpMestViewFilterQuery();
                hisImpMestViewFilter.MEDI_STOCK_IDs = ((Mrs00637Filter)this.reportFilter).MEDI_STOCK_IDs;
                List<long> impMestTypeIds = new List<long>();
                impMestTypeIds.Add(IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__THT);
                hisImpMestViewFilter.IMP_MEST_TYPE_IDs = impMestTypeIds;
                hisImpMestViewFilter.IMP_TIME_FROM = ((Mrs00637Filter)this.reportFilter).TIME_FROM;
                hisImpMestViewFilter.IMP_TIME_TO = ((Mrs00637Filter)this.reportFilter).TIME_TO;
                List<V_HIS_IMP_MEST> impMests = new MOS.MANAGER.HisImpMest.HisImpMestManager(paramGet).GetView(hisImpMestViewFilter);
                List<long> impMestIds = impMests.Select(o => o.ID).ToList();

                int startImpMest = 0;
                int countImpMest = impMestIds.Count;
                while (countImpMest > 0)
                {
                    int limit = (countImpMest <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? countImpMest : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);

                    var listHisImpMestIdLimit = impMestIds.Skip(startImpMest).Take(limit).ToList();
                    HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery();
                    impMestMedicineViewFilter.IMP_MEST_IDs = listHisImpMestIdLimit;
                    var listImpMestMedicineLimit = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(impMestMedicineViewFilter);
                    listImpMestMedicine.AddRange(listImpMestMedicineLimit);
                    HisImpMestMaterialViewFilterQuery impMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery();
                    impMestMaterialViewFilter.IMP_MEST_IDs = listHisImpMestIdLimit;
                    var listImpMestMaterialLimit = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(impMestMaterialViewFilter);
                    listImpMestMaterial.AddRange(listImpMestMaterialLimit);
                    startImpMest += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    countImpMest -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                }

                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private List<string> ConvertToListStringTime(long p1, long p2)
        {
            List<string> result = new List<string>();
            try
            {
                DateTime StartTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(p1);
                DateTime FinishTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(p2);
                DateTime IndexTime = StartTime.Date;
                while (IndexTime < FinishTime)
                {
                    result.Add(Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(IndexTime));
                    var daysInMonth = DateTime.DaysInMonth(IndexTime.Year, IndexTime.Month);
                        IndexTime = IndexTime.AddDays(1);
                    //if (IndexTime.Day != daysInMonth)
                    //else
                    //{
                    //    IndexTime.AddDays(1);
                    //    IndexTime = IndexTime.AddMonths(1);
                    //}
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = new List<string>();
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                ListRdo.Clear();
                Inventec.Common.Logging.LogSystem.Info("listHisExpMestMedicine" + listHisExpMestMedicine.Count);

                if (IsNotNullOrEmpty(listHisExpMestMedicine))
                {
                    var GroupbyMedicineTypeIDs = listHisExpMestMedicine.GroupBy(o => o.MEDICINE_TYPE_ID).ToList();

                    foreach (var group in GroupbyMedicineTypeIDs)
                    {
                        var Sub = group.ToList<DetailRDOMedicine>();
                        Mrs00637RDO rdo = new Mrs00637RDO();
                        var firstSubItem = Sub.First();
                        foreach (var item in Sub)
                        {
                            long dayInMonth = GetDayInMonth(item.INTRUCTION_TIME);
                            if (dayInMonth == 1)
                            {
                                rdo.Day1_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 2)
                            {
                                rdo.Day2_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 3)
                            {
                                rdo.Day3_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 4)
                            {
                                rdo.Day4_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 5)
                            {
                                rdo.Day5_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 6)
                            {
                                rdo.Day6_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 7)
                            {
                                rdo.Day7_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 8)
                            {
                                rdo.Day8_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 9)
                            {
                                rdo.Day9_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 10)
                            {
                                rdo.Day10_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 11)
                            {
                                rdo.Day11_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 12)
                            {
                                rdo.Day12_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 13)
                            {
                                rdo.Day13_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 14)
                            {
                                rdo.Day14_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 15)
                            {
                                rdo.Day15_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 16)
                            {
                                rdo.Day16_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 17)
                            {
                                rdo.Day17_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 18)
                            {
                                rdo.Day18_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 19)
                            {
                                rdo.Day19_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 20)
                            {
                                rdo.Day20_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 21)
                            {
                                rdo.Day21_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 22)
                            {
                                rdo.Day22_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 23)
                            {
                                rdo.Day23_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 24)
                            {
                                rdo.Day24_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 25)
                            {
                                rdo.Day25_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 26)
                            {
                                rdo.Day26_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 27)
                            {
                                rdo.Day27_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 28)
                            {
                                rdo.Day28_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 29)
                            {
                                rdo.Day29_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 30)
                            {
                                rdo.Day30_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 31)
                            {
                                rdo.Day31_Amount += item.AMOUNT;
                            }
                            else
                            {
                                Inventec.Common.Logging.LogSystem.Warn("Khong xac dinh duoc ngay thuc xuat");
                            }
                        }

                        var checkMedicines = listImpMestMedicine.Where(o => o.MEDICINE_TYPE_ID == firstSubItem.MEDICINE_TYPE_ID).ToList();
                        rdo.AMOUNT_MOBA_SUM = checkMedicines.Sum(o => o.AMOUNT);
                        rdo.MEDI_MATE_TYPE_NAME = firstSubItem.MEDICINE_TYPE_NAME;
                        rdo.MEDI_MATE_TYPE_CODE = firstSubItem.MEDICINE_TYPE_CODE;
                        rdo.SERVICE_UNIT_NAME = firstSubItem.SERVICE_UNIT_NAME;
                        rdo.PRICE = firstSubItem.PRICE ?? 0;
                        rdo.AMOUNT_EXP_SUM = Sub.Sum(o => o.AMOUNT);
                        rdo.VIR_TOTAL_PRICE = rdo.PRICE * rdo.AMOUNT_EXP_SUM;
                        rdo.AMOUNT_STR = ConvertToStringListAmount(((Mrs00637Filter)this.reportFilter).TIME_FROM, ((Mrs00637Filter)this.reportFilter).TIME_TO, Sub);
                        ListRdo.Add(rdo);
                    }
                }
                Inventec.Common.Logging.LogSystem.Info("listHisExpMestMaterial" + listHisExpMestMaterial.Count);
                if (IsNotNullOrEmpty(listHisExpMestMaterial))
                {
                    var groupByMaterialTypeIds = listHisExpMestMaterial.GroupBy(o => o.MATERIAL_TYPE_ID).ToList();
                    foreach (var group in groupByMaterialTypeIds)
                    {
                        var subItems = group.ToList<DetailRDOMaterial>();
                        Mrs00637RDO rdo = new Mrs00637RDO();
                        var firstSubItem = subItems.First();
                        rdo.MEDI_MATE_TYPE_NAME = firstSubItem.MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = firstSubItem.SERVICE_UNIT_NAME;
                        rdo.MEDI_MATE_TYPE_CODE = firstSubItem.MATERIAL_TYPE_CODE;
                        foreach (var item in subItems)
                        {
                            long dayInMonth = GetDayInMonth(item.INTRUCTION_TIME);
                            if (dayInMonth == 1)
                            {
                                rdo.Day1_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 2)
                            {
                                rdo.Day2_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 3)
                            {
                                rdo.Day3_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 4)
                            {
                                rdo.Day4_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 5)
                            {
                                rdo.Day5_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 6)
                            {
                                rdo.Day6_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 7)
                            {
                                rdo.Day7_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 8)
                            {
                                rdo.Day8_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 9)
                            {
                                rdo.Day9_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 10)
                            {
                                rdo.Day10_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 11)
                            {
                                rdo.Day11_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 12)
                            {
                                rdo.Day12_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 13)
                            {
                                rdo.Day13_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 14)
                            {
                                rdo.Day14_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 15)
                            {
                                rdo.Day15_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 16)
                            {
                                rdo.Day16_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 17)
                            {
                                rdo.Day17_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 18)
                            {
                                rdo.Day18_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 19)
                            {
                                rdo.Day19_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 20)
                            {
                                rdo.Day20_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 21)
                            {
                                rdo.Day21_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 22)
                            {
                                rdo.Day22_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 23)
                            {
                                rdo.Day23_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 24)
                            {
                                rdo.Day24_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 25)
                            {
                                rdo.Day25_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 26)
                            {
                                rdo.Day26_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 27)
                            {
                                rdo.Day27_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 28)
                            {
                                rdo.Day28_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 29)
                            {
                                rdo.Day29_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 30)
                            {
                                rdo.Day30_Amount += item.AMOUNT;
                            }
                            else if (dayInMonth == 31)
                            {
                                rdo.Day31_Amount += item.AMOUNT;
                            }
                            else
                            {
                                Inventec.Common.Logging.LogSystem.Warn("Khong xac dinh duoc ngay thuc xuat");
                            }
                        }
                        var checkMaterialTypes = listImpMestMaterial.Where(o => o.MATERIAL_TYPE_ID == firstSubItem.MATERIAL_TYPE_ID).ToList();
                        rdo.AMOUNT_MOBA_SUM = checkMaterialTypes.Sum(o => o.AMOUNT);
                        rdo.PRICE = firstSubItem.PRICE ?? 0;
                        rdo.AMOUNT_EXP_SUM = subItems.Sum(o => o.AMOUNT);
                        rdo.VIR_TOTAL_PRICE = rdo.PRICE * rdo.AMOUNT_EXP_SUM;
                        rdo.AMOUNT_STR = ConvertToStringListAmount(((Mrs00637Filter)this.reportFilter).TIME_FROM, ((Mrs00637Filter)this.reportFilter).TIME_TO, subItems);
                        ListRdo.Add(rdo);
                    }
                }

                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
            return result;
        }

        private long GetDayInMonth(long time)
        {
            long result = 0;
            try
            {
                if (time > 0)
                {
                    //20171208235959
                    string timeStr = time.ToString();
                    string day = timeStr.Substring(6, 2);
                    result = Inventec.Common.TypeConvert.Parse.ToInt64(day);
                }
            }
            catch (Exception ex)
            {
                result = 0;
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return result;
        }

        private string ConvertToStringListAmount(long p1, long p2, List<DetailRDOMedicine> Sub)
        {
            string result = "";
            try
            {
                DateTime StartTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(p1);
                DateTime FinishTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(p2);
                DateTime IndexTime = StartTime.Date;
                while (IndexTime < FinishTime)
                {
                    result += " " + ConvertToStringAmount(IndexTime, Sub);
                    var daysInMonth = DateTime.DaysInMonth(IndexTime.Year, IndexTime.Month);
                        IndexTime = IndexTime.AddDays(1);
                    //if (IndexTime.Day != daysInMonth)
                    //else
                    //{
                    //    IndexTime.AddDays(1);
                    //    IndexTime = IndexTime.AddMonths(1);
                    //}
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private string ConvertToStringListAmount(long p1, long p2, List<DetailRDOMaterial> Sub)
        {
            string result = "";
            try
            {
                DateTime StartTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(p1);
                DateTime FinishTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(p2);
                DateTime IndexTime = StartTime.Date;
                while (IndexTime < FinishTime)
                {
                    result += " " + ConvertToStringAmount(IndexTime, Sub);
                    var daysInMonth = DateTime.DaysInMonth(IndexTime.Year, IndexTime.Month);
                    if (IndexTime.Day != daysInMonth)
                        IndexTime = IndexTime.AddDays(1);
                    else
                    {
                        IndexTime.AddDays(1);
                        IndexTime = IndexTime.AddMonths(1);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private string ConvertToStringAmount(DateTime IndexTime, List<DetailRDOMedicine> Sub)
        {
            string result = "";
            try
            {
                Decimal IndexAmount = Sub.Where(p => ((DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(p.INTRUCTION_TIME)).Date == IndexTime).Sum(o => o.AMOUNT);
                result = string.Format("{0:000000000000}", Convert.ToInt64(IndexAmount));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private string ConvertToStringAmount(DateTime IndexTime, List<DetailRDOMaterial> Sub)
        {
            string result = "";
            try
            {
                Decimal IndexAmount = Sub.Where(p => ((DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(p.INTRUCTION_TIME)).Date == IndexTime).Sum(o => o.AMOUNT);
                result = string.Format("{0:000000000000}", Convert.ToInt64(IndexAmount));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("EXP_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00637Filter)this.reportFilter).TIME_FROM));
            dicSingleTag.Add("EXP_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00637Filter)this.reportFilter).TIME_TO));
            dicSingleTag.Add("DATE_STR", DATE_STR);
            dicSingleTag.Add("MEDI_STOCK_NAMEs", String.Join(", ", listMedistock.Select(o => o.MEDI_STOCK_NAME).ToList()));

            objectTag.AddObjectData(store, "Report", ListRdo);
        }
    }
}