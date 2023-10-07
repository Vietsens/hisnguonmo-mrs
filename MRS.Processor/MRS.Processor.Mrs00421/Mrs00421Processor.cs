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
using MRS.Processor.Mrs00421;
using System;
using System.Collections.Generic;
using System.Linq;
using MRS.MANAGER.Config;
using MRS.MANAGER.Base;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMaterialType;

namespace MRS.Processor.Mrs00421
{
    public class Mrs00421Processor : AbstractProcessor
    {
        private List<Mrs00421RDO> listRdo = new List<Mrs00421RDO>();
        CommonParam paramGet = new CommonParam();
        List<Mrs00421RDO> ListRdo = new List<Mrs00421RDO>();
        List<Mrs00421RDO> ListRdoTreat = new List<Mrs00421RDO>();
        List<HIS_MEDI_STOCK> listMedistock = new List<HIS_MEDI_STOCK>();
        List<V_HIS_EXP_MEST> listHisExpMest = new List<V_HIS_EXP_MEST>();
        List<HIS_EXP_MEST_TYPE> listHisExpMestType = new List<HIS_EXP_MEST_TYPE>();
        List<V_HIS_EXP_MEST_MEDICINE> listHisExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> listHisExpMestMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();
        Mrs00421Filter filter = new Mrs00421Filter();
        string DATE_STR = "";

        public Mrs00421Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00421Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                filter = ((Mrs00421Filter)this.reportFilter);
                List<string> listDate = new List<string>();
                listDate = ConvertToListStringTime(((Mrs00421Filter)this.reportFilter).TIME_FROM, ((Mrs00421Filter)this.reportFilter).TIME_TO);
                DATE_STR = String.Join(" ", listDate);
                //get dữ liệu:
                HisMediStockFilterQuery hisMediStockFilter = new HisMediStockFilterQuery();
                hisMediStockFilter.IDs = ((Mrs00421Filter)this.reportFilter).MEDI_STOCK_IDs;
                listMedistock = new HisMediStockManager(paramGet).Get(hisMediStockFilter);
                List<long> EXP_MEST_TYPE_IDs = new List<long>();
                EXP_MEST_TYPE_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP);
                EXP_MEST_TYPE_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK);
                EXP_MEST_TYPE_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT);
                EXP_MEST_TYPE_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT);
                var metyFilterHisExpMest = new HisExpMestViewFilterQuery
                {
                    FINISH_TIME_FROM = ((Mrs00421Filter)this.reportFilter).TIME_FROM,
                    FINISH_TIME_TO = ((Mrs00421Filter)this.reportFilter).TIME_TO,
                    MEDI_STOCK_IDs = ((Mrs00421Filter)this.reportFilter).MEDI_STOCK_IDs
                };
                if (((Mrs00421Filter)this.reportFilter).IS_AGGR_OR_CHMS_EXP_MEST == true)
                {
                    metyFilterHisExpMest.EXP_MEST_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT };
                }
                if (((Mrs00421Filter)this.reportFilter).IS_AGGR_OR_CHMS_EXP_MEST == false)
                {
                    metyFilterHisExpMest.EXP_MEST_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK };
                }
                if (((Mrs00421Filter)this.reportFilter).IS_AGGR_OR_CHMS_EXP_MEST == null)
                {
                    metyFilterHisExpMest.EXP_MEST_TYPE_IDs = EXP_MEST_TYPE_IDs;
                }
                if (((Mrs00421Filter)this.reportFilter).EXP_MEST_TYPE_IDs != null)
                {
                    metyFilterHisExpMest.EXP_MEST_TYPE_IDs = ((Mrs00421Filter)this.reportFilter).EXP_MEST_TYPE_IDs;
                }
                metyFilterHisExpMest.REQ_DEPARTMENT_ID = ((Mrs00421Filter)this.reportFilter).REQ_DEPARTMENT_ID;
                metyFilterHisExpMest.REQ_DEPARTMENT_IDs = ((Mrs00421Filter)this.reportFilter).REQ_DEPARTMENT_IDs;
                listHisExpMest = new HisExpMestManager(paramGet).GetView(metyFilterHisExpMest);
                List<long> listHisExpMestId = listHisExpMest.Select(o => o.ID).ToList();
                int start = 0;
                int count = listHisExpMestId.Count;
                while (count > 0)
                {
                    int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                    var listHisExpMestIdLimit = listHisExpMestId.Skip(start).Take(limit).ToList();
                    HisExpMestMedicineViewFilterQuery medifilter = new HisExpMestMedicineViewFilterQuery();
                    medifilter.EXP_MEST_IDs = listHisExpMestIdLimit;
                    medifilter.IS_EXPORT = true;
                    var listHisExpMestMedicineLimit = new HisExpMestMedicineManager(paramGet).GetView(medifilter);
                    listHisExpMestMedicine.AddRange(listHisExpMestMedicineLimit);
                    HisExpMestMaterialViewFilterQuery matefilter = new HisExpMestMaterialViewFilterQuery();
                    matefilter.EXP_MEST_IDs = listHisExpMestIdLimit;
                    matefilter.IS_EXPORT = true;
                    var listHisExpMestMaterialLimit = new HisExpMestMaterialManager(paramGet).GetView(matefilter);
                    listHisExpMestMaterial.AddRange(listHisExpMestMaterialLimit);
                    start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                }
                if (((Mrs00421Filter)this.reportFilter).PATIENT_TYPE_ID != null)
                {
                    listHisExpMestMedicine = listHisExpMestMedicine.Where(o => o.PATIENT_TYPE_ID == ((Mrs00421Filter)this.reportFilter).PATIENT_TYPE_ID).ToList();
                    listHisExpMestMaterial = listHisExpMestMaterial.Where(o => o.PATIENT_TYPE_ID == ((Mrs00421Filter)this.reportFilter).PATIENT_TYPE_ID).ToList();
                }
                // get mobaImpMest
                HisImpMestViewFilterQuery hisImpMestViewFilter = new HisImpMestViewFilterQuery();
                hisImpMestViewFilter.MEDI_STOCK_IDs = ((Mrs00421Filter)this.reportFilter).MEDI_STOCK_IDs;
                List<long> impMestTypeIds = new List<long>();
                impMestTypeIds.Add(IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__THT);
                impMestTypeIds.Add(IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BTL);
                impMestTypeIds.Add(IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DMTL);
                impMestTypeIds.Add(IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL);
                impMestTypeIds.Add(IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DONKTL);
                impMestTypeIds.Add(IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL);
                impMestTypeIds.Add(IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HPTL);
                hisImpMestViewFilter.IMP_MEST_TYPE_IDs = impMestTypeIds;
                hisImpMestViewFilter.IMP_TIME_FROM = ((Mrs00421Filter)this.reportFilter).TIME_FROM;
                hisImpMestViewFilter.REQ_DEPARTMENT_ID = filter.REQ_DEPARTMENT_ID;
                hisImpMestViewFilter.IMP_TIME_TO = ((Mrs00421Filter)this.reportFilter).TIME_TO;
                List<V_HIS_IMP_MEST> impMests = new MOS.MANAGER.HisImpMest.HisImpMestManager(paramGet).GetView(hisImpMestViewFilter);
                LogSystem.Info(impMests.Count.ToString());
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
                    if (filter.REQ_DEPARTMENT_ID != null && listImpMestMedicineLimit != null)
                    {
                        listImpMestMedicineLimit = listImpMestMedicineLimit.Where(x => x.REQ_DEPARTMENT_ID == filter.REQ_DEPARTMENT_ID).ToList();
                    }
                    listImpMestMedicine.AddRange(listImpMestMedicineLimit);
                    HisImpMestMaterialViewFilterQuery impMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery();
                    impMestMaterialViewFilter.IMP_MEST_IDs = listHisImpMestIdLimit;
                    var listImpMestMaterialLimit = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(impMestMaterialViewFilter);
                    if (filter.REQ_DEPARTMENT_ID != null && listImpMestMedicineLimit != null)
                    {
                        listImpMestMaterialLimit = listImpMestMaterialLimit.Where(x => x.REQ_DEPARTMENT_ID == filter.REQ_DEPARTMENT_ID).ToList();
                    }
                    listImpMestMaterial.AddRange(listImpMestMaterialLimit);
                    startImpMest += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    countImpMest -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                }
                //listImpMestMedicine = new ManagerSql().GetMobaImpMedicine(filter);
                //listImpMestMaterial = new ManagerSql().GetMobaImpMaterial(filter);
                listTreatment = new ManagerSql().GetTreatment(((Mrs00421Filter)this.reportFilter));
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

                    var GroupbyMedicineTypeIDs = listHisExpMestMedicine.GroupBy(o => new { o.MEDICINE_TYPE_ID, o.IMP_PRICE, o.IMP_VAT_RATIO }).ToList();

                    foreach (var group in GroupbyMedicineTypeIDs)
                    {
                        var Sub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        Mrs00421RDO rdo = new Mrs00421RDO();
                        var firstSubItem = Sub.First();
                        foreach (var item in Sub)
                        {
                            var impMest = listImpMestMedicine.Where(p => p.MEDICINE_TYPE_ID == item.MEDICINE_TYPE_ID && p.TH_EXP_MEST_MEDICINE_ID == item.ID).ToList() ?? new List<V_HIS_IMP_MEST_MEDICINE>();
                            long dayInMonth = GetDayInMonth(item.EXP_TIME ?? 0);
                            if (dayInMonth == 1)
                            {
                                rdo.Day1_Amount += item.AMOUNT;
                                rdo.Day1_Amount_TL +=0;
                            }
                            else if (dayInMonth == 2)
                            {
                                rdo.Day2_Amount += item.AMOUNT;
                                rdo.Day2_Amount_TL +=0;
                            }
                            else if (dayInMonth == 3)
                            {
                                rdo.Day3_Amount += item.AMOUNT;
                                rdo.Day3_Amount_TL +=0;
                            }
                            else if (dayInMonth == 4)
                            {
                                rdo.Day4_Amount += item.AMOUNT;
                                rdo.Day4_Amount_TL +=0;
                            }
                            else if (dayInMonth == 5)
                            {
                                rdo.Day5_Amount += item.AMOUNT;
                                rdo.Day5_Amount_TL +=0;
                            }
                            else if (dayInMonth == 6)
                            {
                                rdo.Day6_Amount += item.AMOUNT;
                                rdo.Day6_Amount_TL +=0;
                            }
                            else if (dayInMonth == 7)
                            {
                                rdo.Day7_Amount += item.AMOUNT;
                                rdo.Day7_Amount_TL +=0;
                            }
                            else if (dayInMonth == 8)
                            {
                                rdo.Day8_Amount += item.AMOUNT;
                                rdo.Day8_Amount_TL +=0;
                            }
                            else if (dayInMonth == 9)
                            {
                                rdo.Day9_Amount += item.AMOUNT;
                                rdo.Day9_Amount_TL +=0;
                            }
                            else if (dayInMonth == 10)
                            {
                                rdo.Day10_Amount += item.AMOUNT;
                                rdo.Day10_Amount_TL +=0;
                            }
                            else if (dayInMonth == 11)
                            {
                                rdo.Day11_Amount += item.AMOUNT;
                                rdo.Day11_Amount_TL +=0;
                            }
                            else if (dayInMonth == 12)
                            {
                                rdo.Day12_Amount += item.AMOUNT;
                                rdo.Day12_Amount_TL +=0;
                            }
                            else if (dayInMonth == 13)
                            {
                                rdo.Day13_Amount += item.AMOUNT;
                                rdo.Day13_Amount_TL +=0;
                            }
                            else if (dayInMonth == 14)
                            {
                                rdo.Day14_Amount += item.AMOUNT;
                                rdo.Day14_Amount_TL +=0;
                            }
                            else if (dayInMonth == 15)
                            {
                                rdo.Day15_Amount += item.AMOUNT;
                                rdo.Day15_Amount_TL +=0;
                            }
                            else if (dayInMonth == 16)
                            {
                                rdo.Day16_Amount += item.AMOUNT;
                                rdo.Day16_Amount_TL +=0;
                            }
                            else if (dayInMonth == 17)
                            {
                                rdo.Day17_Amount += item.AMOUNT;
                                rdo.Day17_Amount_TL +=0;
                            }
                            else if (dayInMonth == 18)
                            {
                                rdo.Day18_Amount += item.AMOUNT;
                                rdo.Day18_Amount_TL +=0;
                            }
                            else if (dayInMonth == 19)
                            {
                                rdo.Day19_Amount += item.AMOUNT;
                                rdo.Day19_Amount_TL +=0;
                            }
                            else if (dayInMonth == 20)
                            {
                                rdo.Day20_Amount += item.AMOUNT;
                                rdo.Day20_Amount_TL +=0;
                            }
                            else if (dayInMonth == 21)
                            {
                                rdo.Day21_Amount += item.AMOUNT;
                                rdo.Day21_Amount_TL +=0;
                            }
                            else if (dayInMonth == 22)
                            {
                                rdo.Day22_Amount += item.AMOUNT;
                                rdo.Day22_Amount_TL +=0;
                            }
                            else if (dayInMonth == 23)
                            {
                                rdo.Day23_Amount += item.AMOUNT;
                                rdo.Day23_Amount_TL +=0;
                            }
                            else if (dayInMonth == 24)
                            {
                                rdo.Day24_Amount += item.AMOUNT;
                                rdo.Day24_Amount_TL +=0;
                            }
                            else if (dayInMonth == 25)
                            {
                                rdo.Day25_Amount += item.AMOUNT;
                                rdo.Day25_Amount_TL +=0;
                            }
                            else if (dayInMonth == 26)
                            {
                                rdo.Day26_Amount += item.AMOUNT;
                                rdo.Day26_Amount_TL +=0;
                            }
                            else if (dayInMonth == 27)
                            {
                                rdo.Day27_Amount += item.AMOUNT;
                                rdo.Day27_Amount_TL +=0;
                            }
                            else if (dayInMonth == 28)
                            {
                                rdo.Day28_Amount += item.AMOUNT;
                                rdo.Day28_Amount_TL +=0;
                            }
                            else if (dayInMonth == 29)
                            {
                                rdo.Day29_Amount += item.AMOUNT;
                                rdo.Day29_Amount_TL +=0;
                            }
                            else if (dayInMonth == 30)
                            {
                                rdo.Day30_Amount += item.AMOUNT;
                                rdo.Day30_Amount_TL +=0;
                            }
                            else if (dayInMonth == 31)
                            {
                                rdo.Day31_Amount += item.AMOUNT;
                                rdo.Day31_Amount_TL +=0;
                            }
                            else
                            {
                                Inventec.Common.Logging.LogSystem.Warn("Khong xac dinh duoc ngay thuc xuat");
                            }
                        }
                        rdo.NAME = "Thuốc";
                        rdo.MEDI_MATE_TYPE_NAME = firstSubItem.MEDICINE_TYPE_NAME;
                        rdo.MEDI_MATE_TYPE_CODE = firstSubItem.MEDICINE_TYPE_CODE;
                        rdo.SERVICE_UNIT_NAME = firstSubItem.SERVICE_UNIT_NAME;
                        rdo.PRICE = firstSubItem.IMP_PRICE * (1 + firstSubItem.IMP_VAT_RATIO);
                        rdo.AMOUNT_EXP_SUM = Sub.Sum(o => o.AMOUNT);
                        rdo.VIR_TOTAL_PRICE = rdo.PRICE * rdo.AMOUNT_EXP_SUM;
                        rdo.AMOUNT_STR = ConvertToStringListAmount(((Mrs00421Filter)this.reportFilter).TIME_FROM, ((Mrs00421Filter)this.reportFilter).TIME_TO, Sub);
                        //var checkMedicines = listImpMestMedicine.Where(o => Sub.Exists(p => p.ID == o.TH_EXP_MEST_MEDICINE_ID)).ToList();
                        //rdo.AMOUNT_MOBA_SUM = checkMedicines.Sum(o => o.AMOUNT);
                        
                        ListRdo.Add(rdo);
                        if (Sub.Exists(p => treatmentTypeId(p.TDL_TREATMENT_ID ?? 0) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU))
                        {
                            Mrs00421RDO rdoTreat = new Mrs00421RDO();
                            Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00421RDO>(rdoTreat, rdo);
                            rdoTreat.TREATMENT_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU;
                            rdoTreat.TREATMENT_TYPE_NAME = (HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_NAME;
                            rdoTreat.AMOUNT_EXP_SUM = Sub.Where(p => treatmentTypeId(p.TDL_TREATMENT_ID ?? 0) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(o => o.AMOUNT);
                            //rdoTreat.AMOUNT_MOBA_SUM = checkMedicines.Where(o => Sub.Where(q => treatmentTypeId(q.TDL_TREATMENT_ID ?? 0) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList().Exists(p => p.ID == o.TH_EXP_MEST_MEDICINE_ID)).Sum(o => o.AMOUNT);
                            rdoTreat.VIR_TOTAL_PRICE = rdoTreat.PRICE * rdoTreat.AMOUNT_EXP_SUM;
                            ListRdoTreat.Add(rdoTreat);

                        }

                        if (Sub.Exists(p => treatmentTypeId(p.TDL_TREATMENT_ID ?? 0) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU))
                        {
                            Mrs00421RDO rdoTreat = new Mrs00421RDO();
                            Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00421RDO>(rdoTreat, rdo);
                            rdoTreat.TREATMENT_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU;
                            rdoTreat.TREATMENT_TYPE_NAME = (HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_NAME;
                            rdoTreat.AMOUNT_EXP_SUM = Sub.Where(p => treatmentTypeId(p.TDL_TREATMENT_ID ?? 0) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).Sum(o => o.AMOUNT);
                            //rdoTreat.AMOUNT_MOBA_SUM = checkMedicines.Where(o => Sub.Where(q => treatmentTypeId(q.TDL_TREATMENT_ID ?? 0) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).ToList().Exists(p => p.ID == o.TH_EXP_MEST_MEDICINE_ID)).Sum(o => o.AMOUNT);
                            rdoTreat.VIR_TOTAL_PRICE = rdoTreat.PRICE * rdoTreat.AMOUNT_EXP_SUM;
                            ListRdoTreat.Add(rdoTreat);
                        }
                        if (Sub.Exists(p => treatmentTypeId(p.TDL_TREATMENT_ID ?? 0) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM))
                        {
                            Mrs00421RDO rdoTreat = new Mrs00421RDO();
                            Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00421RDO>(rdoTreat, rdo);
                            rdoTreat.TREATMENT_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM;
                            rdoTreat.TREATMENT_TYPE_NAME = (HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_NAME;
                            rdoTreat.AMOUNT_EXP_SUM = Sub.Where(p => treatmentTypeId(p.TDL_TREATMENT_ID ?? 0) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).Sum(o => o.AMOUNT);
                            //rdoTreat.AMOUNT_MOBA_SUM = checkMedicines.Where(o => Sub.Where(q => treatmentTypeId(q.TDL_TREATMENT_ID ?? 0) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).ToList().Exists(p => p.ID == o.TH_EXP_MEST_MEDICINE_ID)).Sum(o => o.AMOUNT);
                            rdoTreat.VIR_TOTAL_PRICE = rdoTreat.PRICE * rdoTreat.AMOUNT_EXP_SUM;
                            ListRdoTreat.Add(rdoTreat);
                        }
                    }
                    //var medicineOnlyImp = listImpMestMedicine.Where(o => !listHisExpMestMedicine.Exists(p => p.ID == o.TH_EXP_MEST_MEDICINE_ID)).ToList();
                    var medicineOnlyImp = listImpMestMedicine.Where(o => o.TH_EXP_MEST_MEDICINE_ID != null).ToList();
                    if (IsNotNullOrEmpty(medicineOnlyImp))
                    {

                        var Gr = medicineOnlyImp.GroupBy(o => new { o.MEDICINE_TYPE_ID,o.IMP_PRICE,o.IMP_VAT_RATIO }).ToList();

                        foreach (var group in Gr)
                        {
                            var Sub = group.ToList<V_HIS_IMP_MEST_MEDICINE>();
                            Mrs00421RDO rdo = new Mrs00421RDO();
                            var firstSubItem = Sub.First();
                            foreach (var item in Sub)
                            {
                                long dayInMonth = GetDayInMonth(item.IMP_TIME ?? 0);
                                if (dayInMonth == 1)
                                {
                                    rdo.Day1_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 2)
                                {
                                    rdo.Day2_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 3)
                                {
                                    rdo.Day3_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 4)
                                {
                                    rdo.Day4_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 5)
                                {
                                    rdo.Day5_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 6)
                                {
                                    rdo.Day6_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 7)
                                {
                                    rdo.Day7_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 8)
                                {
                                    rdo.Day8_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 9)
                                {
                                    rdo.Day9_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 10)
                                {
                                    rdo.Day10_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 11)
                                {
                                    rdo.Day11_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 12)
                                {
                                    rdo.Day12_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 13)
                                {
                                    rdo.Day13_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 14)
                                {
                                    rdo.Day14_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 15)
                                {
                                    rdo.Day15_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 16)
                                {
                                    rdo.Day16_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 17)
                                {
                                    rdo.Day17_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 18)
                                {
                                    rdo.Day18_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 19)
                                {
                                    rdo.Day19_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 20)
                                {
                                    rdo.Day20_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 21)
                                {
                                    rdo.Day21_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 22)
                                {
                                    rdo.Day22_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 23)
                                {
                                    rdo.Day23_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 24)
                                {
                                    rdo.Day24_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 25)
                                {
                                    rdo.Day25_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 26)
                                {
                                    rdo.Day26_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 27)
                                {
                                    rdo.Day27_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 28)
                                {
                                    rdo.Day28_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 29)
                                {
                                    rdo.Day29_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 30)
                                {
                                    rdo.Day30_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 31)
                                {
                                    rdo.Day31_Amount_TL += item.AMOUNT;
                                }
                                else
                                {
                                    Inventec.Common.Logging.LogSystem.Warn("Khong xac dinh duoc ngay thuc xuat");
                                }
                            }
                            rdo.NAME = "Thuốc";
                            rdo.AMOUNT_MOBA_SUM = Sub.Sum(o => o.AMOUNT);
                            rdo.MEDI_MATE_TYPE_NAME = firstSubItem.MEDICINE_TYPE_NAME;
                            rdo.MEDI_MATE_TYPE_CODE = firstSubItem.MEDICINE_TYPE_CODE;
                            rdo.SERVICE_UNIT_NAME = firstSubItem.SERVICE_UNIT_NAME;
                            rdo.PRICE = firstSubItem.IMP_PRICE * ( 1+firstSubItem.IMP_VAT_RATIO);
                            ListRdo.Add(rdo);
                            Mrs00421RDO rdoTreat = new Mrs00421RDO();
                            Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00421RDO>(rdoTreat, rdo);
                            rdoTreat.TREATMENT_TYPE_ID = 0;
                            rdoTreat.TREATMENT_TYPE_NAME = "";
                            rdoTreat.AMOUNT_MOBA_SUM = Sub.Sum(o => o.AMOUNT);
                            ListRdoTreat.Add(rdoTreat);
                        }
                    }
                }
                Inventec.Common.Logging.LogSystem.Info("listHisExpMestMaterial" + listHisExpMestMaterial.Count);
                if (IsNotNullOrEmpty(listHisExpMestMaterial))
                {

                    var groupByMaterialTypeIds = listHisExpMestMaterial.GroupBy(o => new { o.MATERIAL_TYPE_ID, o.IMP_PRICE, o.IMP_VAT_RATIO }).ToList();
                    foreach (var group in groupByMaterialTypeIds)
                    {
                        var subItems = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        Mrs00421RDO rdo = new Mrs00421RDO();
                        var firstSubItem = subItems.First();
                        rdo.MEDI_MATE_TYPE_NAME = firstSubItem.MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = firstSubItem.SERVICE_UNIT_NAME;
                        rdo.MEDI_MATE_TYPE_CODE = firstSubItem.MATERIAL_TYPE_CODE;
                        foreach (var item in subItems)
                        {
                            var impMest = listImpMestMaterial.Where(p => p.MATERIAL_TYPE_ID == item.MATERIAL_TYPE_ID && p.TH_EXP_MEST_MATERIAL_ID == item.ID) ?? new List<V_HIS_IMP_MEST_MATERIAL>();
                            long dayInMonth = GetDayInMonth(item.EXP_TIME ?? 0);
                            if (dayInMonth == 1)
                            {
                                rdo.Day1_Amount += item.AMOUNT;
                                rdo.Day1_Amount_TL +=0;
                            }
                            else if (dayInMonth == 2)
                            {
                                rdo.Day2_Amount += item.AMOUNT;
                                rdo.Day2_Amount_TL +=0;
                            }
                            else if (dayInMonth == 3)
                            {
                                rdo.Day3_Amount += item.AMOUNT;
                                rdo.Day3_Amount_TL +=0;
                            }
                            else if (dayInMonth == 4)
                            {
                                rdo.Day4_Amount += item.AMOUNT;
                                rdo.Day4_Amount_TL +=0;
                            }
                            else if (dayInMonth == 5)
                            {
                                rdo.Day5_Amount += item.AMOUNT;
                                rdo.Day5_Amount_TL +=0;
                            }
                            else if (dayInMonth == 6)
                            {
                                rdo.Day6_Amount += item.AMOUNT;
                                rdo.Day6_Amount_TL +=0;
                            }
                            else if (dayInMonth == 7)
                            {
                                rdo.Day7_Amount += item.AMOUNT;
                                rdo.Day7_Amount_TL +=0;
                            }
                            else if (dayInMonth == 8)
                            {
                                rdo.Day8_Amount += item.AMOUNT;
                                rdo.Day8_Amount_TL +=0;
                            }
                            else if (dayInMonth == 9)
                            {
                                rdo.Day9_Amount += item.AMOUNT;
                                rdo.Day9_Amount_TL +=0;
                            }
                            else if (dayInMonth == 10)
                            {
                                rdo.Day10_Amount += item.AMOUNT;
                                rdo.Day10_Amount_TL +=0;
                            }
                            else if (dayInMonth == 11)
                            {
                                rdo.Day11_Amount += item.AMOUNT;
                                rdo.Day11_Amount_TL +=0;
                            }
                            else if (dayInMonth == 12)
                            {
                                rdo.Day12_Amount += item.AMOUNT;
                                rdo.Day12_Amount_TL +=0;
                            }
                            else if (dayInMonth == 13)
                            {
                                rdo.Day13_Amount += item.AMOUNT;
                                rdo.Day13_Amount_TL +=0;
                            }
                            else if (dayInMonth == 14)
                            {
                                rdo.Day14_Amount += item.AMOUNT;
                                rdo.Day14_Amount_TL +=0;
                            }
                            else if (dayInMonth == 15)
                            {
                                rdo.Day15_Amount += item.AMOUNT;
                                rdo.Day15_Amount_TL +=0;
                            }
                            else if (dayInMonth == 16)
                            {
                                rdo.Day16_Amount += item.AMOUNT;
                                rdo.Day16_Amount_TL +=0;
                            }
                            else if (dayInMonth == 17)
                            {
                                rdo.Day17_Amount += item.AMOUNT;
                                rdo.Day17_Amount_TL +=0;
                            }
                            else if (dayInMonth == 18)
                            {
                                rdo.Day18_Amount += item.AMOUNT;
                                rdo.Day18_Amount_TL +=0;
                            }
                            else if (dayInMonth == 19)
                            {
                                rdo.Day19_Amount += item.AMOUNT;
                                rdo.Day19_Amount_TL +=0;
                            }
                            else if (dayInMonth == 20)
                            {
                                rdo.Day20_Amount += item.AMOUNT;
                                rdo.Day20_Amount_TL +=0;
                            }
                            else if (dayInMonth == 21)
                            {
                                rdo.Day21_Amount += item.AMOUNT;
                                rdo.Day21_Amount_TL +=0;
                            }
                            else if (dayInMonth == 22)
                            {
                                rdo.Day22_Amount += item.AMOUNT;
                                rdo.Day22_Amount_TL +=0;
                            }
                            else if (dayInMonth == 23)
                            {
                                rdo.Day23_Amount += item.AMOUNT;
                                rdo.Day23_Amount_TL +=0;
                            }
                            else if (dayInMonth == 24)
                            {
                                rdo.Day24_Amount += item.AMOUNT;
                                rdo.Day24_Amount_TL +=0;
                            }
                            else if (dayInMonth == 25)
                            {
                                rdo.Day25_Amount += item.AMOUNT;
                                rdo.Day25_Amount_TL +=0;
                            }
                            else if (dayInMonth == 26)
                            {
                                rdo.Day26_Amount += item.AMOUNT;
                                rdo.Day26_Amount_TL +=0;
                            }
                            else if (dayInMonth == 27)
                            {
                                rdo.Day27_Amount += item.AMOUNT;
                                rdo.Day27_Amount_TL +=0;
                            }
                            else if (dayInMonth == 28)
                            {
                                rdo.Day28_Amount += item.AMOUNT;
                                rdo.Day28_Amount_TL +=0;
                            }
                            else if (dayInMonth == 29)
                            {
                                rdo.Day29_Amount += item.AMOUNT;
                                rdo.Day29_Amount_TL +=0;
                            }
                            else if (dayInMonth == 30)
                            {
                                rdo.Day30_Amount += item.AMOUNT;
                                rdo.Day30_Amount_TL +=0;
                            }
                            else if (dayInMonth == 31)
                            {
                                rdo.Day31_Amount += item.AMOUNT;
                                rdo.Day31_Amount_TL +=0;
                            }
                            else
                            {
                                Inventec.Common.Logging.LogSystem.Warn("Khong xac dinh duoc ngay thuc xuat");
                            }
                        }
                        rdo.NAME = "Vật tư";
                        rdo.PRICE = firstSubItem.IMP_PRICE * (1 + firstSubItem.IMP_VAT_RATIO);
                        rdo.AMOUNT_EXP_SUM = subItems.Sum(o => o.AMOUNT);
                        rdo.VIR_TOTAL_PRICE = rdo.PRICE * rdo.AMOUNT_EXP_SUM;
                        rdo.AMOUNT_STR = ConvertToStringListAmount(((Mrs00421Filter)this.reportFilter).TIME_FROM, ((Mrs00421Filter)this.reportFilter).TIME_TO, subItems);
                        //var checkMaterialTypes = listImpMestMaterial.Where(o => subItems.Exists(p => p.ID == o.TH_EXP_MEST_MATERIAL_ID)).ToList();
                        
                        //rdo.AMOUNT_MOBA_SUM = checkMaterialTypes.Sum(o => o.AMOUNT);
                        
                        ListRdo.Add(rdo);
                        if (subItems.Exists(p => treatmentTypeId(p.TDL_TREATMENT_ID ?? 0) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU))
                        {
                            Mrs00421RDO rdoTreat = new Mrs00421RDO();
                            Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00421RDO>(rdoTreat, rdo);
                            rdoTreat.TREATMENT_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU;
                            rdoTreat.TREATMENT_TYPE_NAME = (HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_NAME;
                            rdoTreat.AMOUNT_EXP_SUM = subItems.Where(p => treatmentTypeId(p.TDL_TREATMENT_ID ?? 0) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(o => o.AMOUNT);
                            //rdoTreat.AMOUNT_MOBA_SUM = checkMaterialTypes.Where(o => subItems.Where(q => treatmentTypeId(q.TDL_TREATMENT_ID ?? 0) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList().Exists(p => p.ID == o.TH_EXP_MEST_MATERIAL_ID)).Sum(o => o.AMOUNT);
                            rdoTreat.VIR_TOTAL_PRICE = rdoTreat.PRICE * rdoTreat.AMOUNT_EXP_SUM;
                            ListRdoTreat.Add(rdoTreat);

                        }

                        if (subItems.Exists(p => treatmentTypeId(p.TDL_TREATMENT_ID ?? 0) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU))
                        {
                            Mrs00421RDO rdoTreat = new Mrs00421RDO();
                            Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00421RDO>(rdoTreat, rdo);
                            rdoTreat.TREATMENT_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU;
                            rdoTreat.TREATMENT_TYPE_NAME = (HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_NAME;
                            rdoTreat.AMOUNT_EXP_SUM = subItems.Where(p => treatmentTypeId(p.TDL_TREATMENT_ID ?? 0) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).Sum(o => o.AMOUNT);
                            //rdoTreat.AMOUNT_MOBA_SUM = checkMaterialTypes.Where(o => subItems.Where(q => treatmentTypeId(q.TDL_TREATMENT_ID ?? 0) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).ToList().Exists(p => p.ID == o.TH_EXP_MEST_MATERIAL_ID)).Sum(o => o.AMOUNT);
                            rdoTreat.VIR_TOTAL_PRICE = rdoTreat.PRICE * rdoTreat.AMOUNT_EXP_SUM;
                            ListRdoTreat.Add(rdoTreat);
                        }
                        if (subItems.Exists(p => treatmentTypeId(p.TDL_TREATMENT_ID ?? 0) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM))
                        {
                            Mrs00421RDO rdoTreat = new Mrs00421RDO();
                            Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00421RDO>(rdoTreat, rdo);
                            rdoTreat.TREATMENT_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM;
                            rdoTreat.TREATMENT_TYPE_NAME = (HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_NAME;
                            //rdoTreat.AMOUNT_MOBA_SUM = checkMaterialTypes.Where(o => subItems.Where(q => treatmentTypeId(q.TDL_TREATMENT_ID ?? 0) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).ToList().Exists(p => p.ID == o.TH_EXP_MEST_MATERIAL_ID)).Sum(o => o.AMOUNT);
                            rdoTreat.AMOUNT_EXP_SUM = subItems.Where(p => treatmentTypeId(p.TDL_TREATMENT_ID ?? 0) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).Sum(o => o.AMOUNT);
                            rdoTreat.VIR_TOTAL_PRICE = rdoTreat.PRICE * rdoTreat.AMOUNT_EXP_SUM;
                            ListRdoTreat.Add(rdoTreat);
                        }
                    }
                    //var materialOnlyImp = listImpMestMaterial.Where(o => !listHisExpMestMaterial.Exists(p => p.ID == o.TH_EXP_MEST_MATERIAL_ID)).ToList();
                    var materialOnlyImp = listImpMestMaterial.Where(o => o.TH_EXP_MEST_MATERIAL_ID != null).ToList();
                    if (IsNotNullOrEmpty(materialOnlyImp))
                    {

                        var Gr = materialOnlyImp.GroupBy(o => new { o.MATERIAL_TYPE_ID, o.IMP_PRICE, o.IMP_VAT_RATIO }).ToList();

                        foreach (var group in Gr)
                        {
                            var Sub = group.ToList<V_HIS_IMP_MEST_MATERIAL>();
                            Mrs00421RDO rdo = new Mrs00421RDO();
                            var firstSubItem = Sub.First();
                            foreach (var item in Sub)
                            {
                                long dayInMonth = GetDayInMonth(item.IMP_TIME ?? 0);
                                if (dayInMonth == 1)
                                {
                                    rdo.Day1_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 2)
                                {
                                    rdo.Day2_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 3)
                                {
                                    rdo.Day3_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 4)
                                {
                                    rdo.Day4_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 5)
                                {
                                    rdo.Day5_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 6)
                                {
                                    rdo.Day6_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 7)
                                {
                                    rdo.Day7_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 8)
                                {
                                    rdo.Day8_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 9)
                                {
                                    rdo.Day9_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 10)
                                {
                                    rdo.Day10_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 11)
                                {
                                    rdo.Day11_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 12)
                                {
                                    rdo.Day12_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 13)
                                {
                                    rdo.Day13_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 14)
                                {
                                    rdo.Day14_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 15)
                                {
                                    rdo.Day15_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 16)
                                {
                                    rdo.Day16_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 17)
                                {
                                    rdo.Day17_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 18)
                                {
                                    rdo.Day18_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 19)
                                {
                                    rdo.Day19_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 20)
                                {
                                    rdo.Day20_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 21)
                                {
                                    rdo.Day21_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 22)
                                {
                                    rdo.Day22_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 23)
                                {
                                    rdo.Day23_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 24)
                                {
                                    rdo.Day24_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 25)
                                {
                                    rdo.Day25_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 26)
                                {
                                    rdo.Day26_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 27)
                                {
                                    rdo.Day27_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 28)
                                {
                                    rdo.Day28_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 29)
                                {
                                    rdo.Day29_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 30)
                                {
                                    rdo.Day30_Amount_TL += item.AMOUNT;
                                }
                                else if (dayInMonth == 31)
                                {
                                    rdo.Day31_Amount_TL += item.AMOUNT;
                                }
                                else
                                {
                                    Inventec.Common.Logging.LogSystem.Warn("Khong xac dinh duoc ngay thuc xuat");
                                }
                            }
                            rdo.NAME = "Vật tư";
                            rdo.AMOUNT_MOBA_SUM = Sub.Sum(o => o.AMOUNT);
                            rdo.MEDI_MATE_TYPE_NAME = firstSubItem.MATERIAL_TYPE_NAME;
                            rdo.MEDI_MATE_TYPE_CODE = firstSubItem.MATERIAL_TYPE_CODE;
                            rdo.SERVICE_UNIT_NAME = firstSubItem.SERVICE_UNIT_NAME;
                            rdo.PRICE = firstSubItem.IMP_PRICE * (1 + firstSubItem.IMP_VAT_RATIO);
                            ListRdo.Add(rdo);
                            
                            Mrs00421RDO rdoTreat = new Mrs00421RDO();
                            Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00421RDO>(rdoTreat, rdo);
                            rdoTreat.TREATMENT_TYPE_ID = 0;
                            rdoTreat.TREATMENT_TYPE_NAME = "";
                            rdoTreat.AMOUNT_MOBA_SUM = Sub.Sum(o => o.AMOUNT);
                            ListRdoTreat.Add(rdoTreat);
                        }
                    }
                }
                GroupListRdo(ListRdo);
                AddInfoMedicineLine(ListRdoTreat);
                AddInfoMedicineLine(ListRdo);
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
            return result;
        }

        private void GroupListRdo(List<Mrs00421RDO> listRdo)
        {
            if (listRdo != null)
            {
                ListRdo = listRdo.GroupBy(p => new { p.NAME, p.MEDI_MATE_TYPE_CODE, p.PRICE }).Select(q => new Mrs00421RDO
                {
                    NAME = q.First().NAME,
                    MEDI_MATE_TYPE_CODE = q.First().MEDI_MATE_TYPE_CODE,
                    MEDI_MATE_TYPE_NAME = q.First().MEDI_MATE_TYPE_NAME,
                    SERVICE_UNIT_NAME = q.First().SERVICE_UNIT_NAME,
                    Day1_Amount = q.Sum(p => p.Day1_Amount),
                    Day2_Amount = q.Sum(p => p.Day2_Amount),
                    Day3_Amount = q.Sum(p => p.Day3_Amount),
                    Day4_Amount = q.Sum(p => p.Day4_Amount),
                    Day5_Amount = q.Sum(p => p.Day5_Amount),
                    Day6_Amount = q.Sum(p => p.Day6_Amount),
                    Day7_Amount = q.Sum(p => p.Day7_Amount),
                    Day8_Amount = q.Sum(p => p.Day8_Amount),
                    Day9_Amount = q.Sum(p => p.Day9_Amount),
                    Day10_Amount = q.Sum(p => p.Day10_Amount),
                    Day11_Amount = q.Sum(p => p.Day11_Amount),
                    Day12_Amount = q.Sum(p => p.Day12_Amount),
                    Day13_Amount = q.Sum(p => p.Day13_Amount),
                    Day14_Amount = q.Sum(p => p.Day14_Amount),
                    Day15_Amount = q.Sum(p => p.Day15_Amount),
                    Day16_Amount = q.Sum(p => p.Day16_Amount),
                    Day17_Amount = q.Sum(p => p.Day17_Amount),
                    Day18_Amount = q.Sum(p => p.Day18_Amount),
                    Day19_Amount = q.Sum(p => p.Day19_Amount),
                    Day20_Amount = q.Sum(p => p.Day20_Amount),
                    Day21_Amount = q.Sum(p => p.Day21_Amount),
                    Day22_Amount = q.Sum(p => p.Day22_Amount),
                    Day23_Amount = q.Sum(p => p.Day23_Amount),
                    Day24_Amount = q.Sum(p => p.Day24_Amount),
                    Day25_Amount = q.Sum(p => p.Day25_Amount),
                    Day26_Amount = q.Sum(p => p.Day26_Amount),
                    Day27_Amount = q.Sum(p => p.Day27_Amount),
                    Day28_Amount = q.Sum(p => p.Day28_Amount),
                    Day29_Amount = q.Sum(p => p.Day29_Amount),
                    Day30_Amount = q.Sum(p => p.Day30_Amount),
                    Day31_Amount = q.Sum(p => p.Day31_Amount),
                    Day1_Amount_TL = q.Sum(p => p.Day1_Amount_TL),
                    Day2_Amount_TL = q.Sum(p => p.Day2_Amount_TL),
                    Day3_Amount_TL = q.Sum(p => p.Day3_Amount_TL),
                    Day4_Amount_TL = q.Sum(p => p.Day4_Amount_TL),
                    Day5_Amount_TL = q.Sum(p => p.Day5_Amount_TL),
                    Day6_Amount_TL = q.Sum(p => p.Day6_Amount_TL),
                    Day7_Amount_TL = q.Sum(p => p.Day7_Amount_TL),
                    Day8_Amount_TL = q.Sum(p => p.Day8_Amount_TL),
                    Day9_Amount_TL = q.Sum(p => p.Day9_Amount_TL),
                    Day10_Amount_TL = q.Sum(p => p.Day10_Amount_TL),
                    Day11_Amount_TL = q.Sum(p => p.Day11_Amount_TL),
                    Day12_Amount_TL = q.Sum(p => p.Day12_Amount_TL),
                    Day13_Amount_TL = q.Sum(p => p.Day13_Amount_TL),
                    Day14_Amount_TL = q.Sum(p => p.Day14_Amount_TL),
                    Day15_Amount_TL = q.Sum(p => p.Day15_Amount_TL),
                    Day16_Amount_TL = q.Sum(p => p.Day16_Amount_TL),
                    Day17_Amount_TL = q.Sum(p => p.Day17_Amount_TL),
                    Day18_Amount_TL = q.Sum(p => p.Day18_Amount_TL),
                    Day19_Amount_TL = q.Sum(p => p.Day19_Amount_TL),
                    Day20_Amount_TL = q.Sum(p => p.Day20_Amount_TL),
                    Day21_Amount_TL = q.Sum(p => p.Day21_Amount_TL),
                    Day22_Amount_TL = q.Sum(p => p.Day22_Amount_TL),
                    Day23_Amount_TL = q.Sum(p => p.Day23_Amount_TL),
                    Day24_Amount_TL = q.Sum(p => p.Day24_Amount_TL),
                    Day25_Amount_TL = q.Sum(p => p.Day25_Amount_TL),
                    Day26_Amount_TL = q.Sum(p => p.Day26_Amount_TL),
                    Day27_Amount_TL = q.Sum(p => p.Day27_Amount_TL),
                    Day28_Amount_TL = q.Sum(p => p.Day28_Amount_TL),
                    Day29_Amount_TL = q.Sum(p => p.Day29_Amount_TL),
                    Day30_Amount_TL = q.Sum(p => p.Day30_Amount_TL),
                    Day31_Amount_TL = q.Sum(p => p.Day31_Amount_TL),
                    AMOUNT_EXP_SUM = q.Sum(p => p.AMOUNT_EXP_SUM),
                    VIR_TOTAL_PRICE = q.Sum(p => p.VIR_TOTAL_PRICE),
                    AMOUNT_MOBA_SUM = q.Sum(p => p.AMOUNT_MOBA_SUM),
                    PRICE = q.First().PRICE,
                    
                }).ToList();
            }
        }

        private void AddInfoMedicineLine(List<Mrs00421RDO> ListRdo)
        {
            var ListMedicineType = new HisMedicineTypeManager(paramGet).GetView(new HisMedicineTypeViewFilterQuery());
            foreach (var item in ListRdo)
            {
                var medicineType = ListMedicineType.FirstOrDefault(o => o.MEDICINE_TYPE_CODE == item.MEDI_MATE_TYPE_CODE);
                if (medicineType != null && medicineType.MEDICINE_LINE_ID != null)
                {
                    item.MEDI_MATE_LINE_NAME = medicineType.MEDICINE_LINE_NAME;
                }
                else
                {
                    item.MEDI_MATE_LINE_NAME = "Dòng thuốc khác";
                }
            }
        }


        private long treatmentTypeId(long treatmentId)
        {
            return (listTreatment.FirstOrDefault(o => o.ID == treatmentId) ?? new HIS_TREATMENT()).TDL_TREATMENT_TYPE_ID ?? 0;
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

        private string ConvertToStringListAmount(long p1, long p2, List<V_HIS_EXP_MEST_MEDICINE> Sub)
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

        private string ConvertToStringListAmount(long p1, long p2, List<V_HIS_EXP_MEST_MATERIAL> Sub)
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

        private string ConvertToStringAmount(DateTime IndexTime, List<V_HIS_EXP_MEST_MEDICINE> Sub)
        {
            string result = "";
            try
            {
                Decimal IndexAmount = Sub.Where(p => ((DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(p.EXP_TIME ?? 0)).Date == IndexTime).Sum(o => o.AMOUNT);
                result = string.Format("{0:000000000000}", Convert.ToInt64(IndexAmount));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private string ConvertToStringAmount(DateTime IndexTime, List<V_HIS_EXP_MEST_MATERIAL> Sub)
        {
            string result = "";
            try
            {
                Decimal IndexAmount = Sub.Where(p => ((DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(p.EXP_TIME ?? 0)).Date == IndexTime).Sum(o => o.AMOUNT);
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
            dicSingleTag.Add("EXP_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00421Filter)this.reportFilter).TIME_FROM));
            dicSingleTag.Add("EXP_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00421Filter)this.reportFilter).TIME_TO));
            dicSingleTag.Add("DATE_STR", DATE_STR);
            dicSingleTag.Add("MEDI_STOCK_NAMEs", String.Join(", ", listMedistock.Select(o => o.MEDI_STOCK_NAME).ToList()));

            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "Type", ListRdoTreat.GroupBy(o => o.NAME).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "ReportTreat", ListRdoTreat);
            objectTag.AddObjectData(store, "ReportTreatParent", ListRdoTreat.GroupBy(o => new { o.TREATMENT_TYPE_ID, o.NAME }).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "Type", "ReportTreatParent", "NAME", "NAME");
            objectTag.AddRelationship(store, "ReportTreatParent", "ReportTreat", new string[] { "TREATMENT_TYPE_ID", "NAME" }, new string[] { "TREATMENT_TYPE_ID", "NAME" });

            objectTag.AddObjectData(store, "Total", ListRdo);
        }
    }
}
