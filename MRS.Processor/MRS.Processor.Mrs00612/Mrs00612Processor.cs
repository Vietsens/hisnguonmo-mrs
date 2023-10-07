using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMedicine;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00612
{
    class Mrs00612Processor : AbstractProcessor
    {
        Mrs00612Filter castFilter = null;
        List<Mrs00612RDO> ListRdo = new List<Mrs00612RDO>();
        List<VitaminAADO> ListVitaminA = new List<VitaminAADO>();
        List<HIS_MEDICINE_TYPE> ListTypeVitaminA = new List<HIS_MEDICINE_TYPE>();
        List<HIS_BRANCH> ListBranch = new List<HIS_BRANCH>();
        List<TreatmentADO> ListTreatment = new List<TreatmentADO>();
        List<HIS_BABY> ListBaby = new List<HIS_BABY>();
        Dictionary<long, List<HIS_MEDI_STOCK>> DicMedistock = new Dictionary<long, List<HIS_MEDI_STOCK>>();//kho theo khoa

        Dictionary<long, HIS_MEDI_STOCK_PERIOD> dicMediStockPeriod = new Dictionary<long, HIS_MEDI_STOCK_PERIOD>();

        List<HIS_MEDICINE> Medicines = new List<HIS_MEDICINE>();
        Dictionary<long, List<V_HIS_MEST_PERIOD_MEDI>> dicMestPeriodMedi = new Dictionary<long, List<V_HIS_MEST_PERIOD_MEDI>>(); // DS thuoc chot ki
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicineBefore = new List<V_HIS_IMP_MEST_MEDICINE>(); // DS thuoc nhap truoc ki
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicineBefore = new List<V_HIS_EXP_MEST_MEDICINE>(); // DS thuoc xuat truoc ki

        //Cac list nhap xuat trong ki
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicineOn = new List<V_HIS_IMP_MEST_MEDICINE>(); // DS thuoc nhap trong ki
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicineOn = new List<V_HIS_EXP_MEST_MEDICINE>(); // DS thuoc xuat trong ki

        public Mrs00612Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00612Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00612Filter)this.reportFilter;

                MOS.MANAGER.HisMedicineType.HisMedicineTypeFilterQuery filterMedicine = new MOS.MANAGER.HisMedicineType.HisMedicineTypeFilterQuery();
                filterMedicine.IS_ACTIVE = 1;
                var listTypeVitaminA = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager().Get(filterMedicine);
                if (listTypeVitaminA != null && listTypeVitaminA.Count > 0)
                {
                    ListTypeVitaminA = listTypeVitaminA.Where(o => o.IS_VITAMIN_A.HasValue && o.IS_VITAMIN_A == 1).ToList();
                }

                ListBranch = MRS.MANAGER.Config.HisBranchCFG.HisBranchs.Where(o => o.IS_ACTIVE == 1).ToList();

                MOS.MANAGER.HisVitaminA.HisVitaminAFilterQuery filter = new MOS.MANAGER.HisVitaminA.HisVitaminAFilterQuery();
                filter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                filter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                var lstVitamin = new MOS.MANAGER.HisVitaminA.HisVitaminAManager(new CommonParam()).Get(filter);
                if (lstVitamin != null && lstVitamin.Count > 0)
                {
                    ListVitaminA = new List<VitaminAADO>();
                    foreach (var item in lstVitamin)
                    {
                        ListVitaminA.Add(new VitaminAADO(item));
                    }
                }

                //MOS.MANAGER.HisTreatment.HisTreatmentFilterQuery treatmentFilter = new MOS.MANAGER.HisTreatment.HisTreatmentFilterQuery();
                //treatmentFilter.IN_TIME_FROM = castFilter.TIME_FROM;
                //treatmentFilter.IN_TIME_TO = castFilter.TIME_TO;
                //var treatments = new MOS.MANAGER.HisTreatment.HisTreatmentManager().Get(treatmentFilter);
                //if (IsNotNullOrEmpty(treatments))
                //{
                //    foreach (var item in treatments)
                //    {
                //        ListTreatment.Add(new TreatmentADO(item));
                //    }

                //    var skip = 0;
                //    while (treatments.Count - skip > 0)
                //    {
                //        var tmIds = treatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                //        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                //        string sql = "SELECT * FROM HIS_BABY B WHERE BORN_TIME BETWEEN {0} AND {1} AND TREATMENT_ID IN ({2})";
                //        var babys = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_BABY>(String.Format(sql, this.castFilter.TIME_FROM, this.castFilter.TIME_TO, string.Join(",", tmIds.Select(s => s.ID).ToList())));
                //        if (babys != null)
                //        {
                //            ListBaby.AddRange(babys);
                //        }
                //    }
                //}

                List<HIS_MEDI_STOCK> lstMediStock = new List<HIS_MEDI_STOCK>();
                if (IsNotNullOrEmpty(ListBranch))
                {
                    foreach (var branch in ListBranch)
                    {
                        string sql = "SELECT * FROM HIS_MEDI_STOCK MS WHERE EXISTS (SELECT 1 FROM V_HIS_ROOM WHERE MS.ROOM_ID = ID AND BRANCH_ID = {0})";

                        var mediStocks = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_MEDI_STOCK>(String.Format(sql, branch.ID));
                        if (mediStocks != null && mediStocks.Count > 0)
                        {
                            if (!DicMedistock.ContainsKey(branch.ID)) DicMedistock[branch.ID] = new List<HIS_MEDI_STOCK>();

                            DicMedistock[branch.ID].AddRange(mediStocks);
                            lstMediStock.AddRange(mediStocks);
                        }
                    }
                }

                lstMediStock = lstMediStock.Distinct().ToList();
                //get du lieu nhap xuat ton
                ProcessMedicineData(lstMediStock);
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessMedicineData(List<HIS_MEDI_STOCK> lstMediStock)
        {
            try
            {
                if (IsNotNullOrEmpty(lstMediStock))
                {
                    CommonParam paramGet = new CommonParam();
                    List<long> mediStockIds = new List<long>();
                    mediStockIds = lstMediStock.Select(s => s.ID).ToList();

                    var skip = 0;
                    while (mediStockIds.Count - skip > 0)
                    {
                        var limit = mediStockIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        //Danh sach chot ki gan nhat cua kho
                        MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodFilterQuery periodFilter = new MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodFilterQuery();
                        periodFilter.CREATE_TIME_TO = castFilter.TIME_FROM;
                        periodFilter.ORDER_DIRECTION = "DESC";
                        periodFilter.ORDER_FIELD = "CREATE_TIME";
                        periodFilter.MEDI_STOCK_IDs = limit;
                        var listPeriod = new MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodManager(paramGet).Get(periodFilter);
                        if (IsNotNullOrEmpty(listPeriod))
                        {
                            foreach (var item in listPeriod)
                            {
                                if (dicMediStockPeriod.ContainsKey(item.MEDI_STOCK_ID)) continue;
                                dicMediStockPeriod[item.MEDI_STOCK_ID] = item;
                            }
                        }

                        GetMestMediMate(limit, castFilter.TIME_FROM, castFilter.TIME_TO, ref listImpMestMedicineOn, ref listExpMestMedicineOn);
                    }

                    foreach (var item in dicMediStockPeriod)
                    {
                        GetMestMediMate(new List<long> { item.Key }, item.Value.CREATE_TIME, castFilter.TIME_FROM, ref listImpMestMedicineBefore, ref listExpMestMedicineBefore);


                        MOS.MANAGER.HisMestPeriodMedi.HisMestPeriodMediViewFilterQuery mestPeriodMediFilter = new MOS.MANAGER.HisMestPeriodMedi.HisMestPeriodMediViewFilterQuery();
                        mestPeriodMediFilter.MEDI_STOCK_PERIOD_ID = item.Value.ID;
                        var listPeriod = new MOS.MANAGER.HisMestPeriodMedi.HisMestPeriodMediManager(paramGet).GetView(mestPeriodMediFilter);
                        if (IsNotNullOrEmpty(listPeriod))
                        {
                            if (!dicMestPeriodMedi.ContainsKey(item.Key))
                                dicMestPeriodMedi[item.Key] = new List<V_HIS_MEST_PERIOD_MEDI>();

                            dicMestPeriodMedi[item.Key].AddRange(listPeriod);
                        }
                    }

                    var listMediStockNoPeriod = lstMediStock.Where(o => !dicMediStockPeriod.ContainsKey(o.ID)).Select(o => o.ID).ToList();
                    if (IsNotNullOrEmpty(listMediStockNoPeriod))
                    {
                        GetMestMediMate(listMediStockNoPeriod, null, castFilter.TIME_FROM, ref listImpMestMedicineBefore, ref listExpMestMedicineBefore);
                    }


                    List<long> medicineIds = new List<long>();
                    if (dicMestPeriodMedi != null)
                    {
                        foreach (var item in dicMestPeriodMedi)
                        {
                            medicineIds.AddRange(item.Value.Select(s => s.MEDICINE_ID).ToList());
                        }
                    }
                    if (listImpMestMedicineBefore != null)
                    {
                        medicineIds.AddRange(listImpMestMedicineBefore.Select(o => o.MEDICINE_ID).ToList());
                    }
                    if (listExpMestMedicineBefore != null)
                    {
                        medicineIds.AddRange(listExpMestMedicineBefore.Select(o => o.MEDICINE_ID ?? 0).ToList());
                    }
                    if (listImpMestMedicineOn != null)
                    {
                        medicineIds.AddRange(listImpMestMedicineOn.Select(o => o.MEDICINE_ID).ToList());
                    }
                    if (listExpMestMedicineOn != null)
                    {
                        medicineIds.AddRange(listExpMestMedicineOn.Select(o => o.MEDICINE_ID ?? 0).ToList());
                    }
                    medicineIds = medicineIds.Distinct().ToList();

                    if (medicineIds != null && medicineIds.Count > 0)
                    {
                        skip = 0;
                        while (medicineIds.Count - skip > 0)
                        {
                            var limit = medicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisMedicineFilterQuery Medicinefilter = new HisMedicineFilterQuery();
                            Medicinefilter.IDs = limit;
                            var MedicineSub = new HisMedicineManager().Get(Medicinefilter);
                            Medicines.AddRange(MedicineSub);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void GetMestMediMate(List<long> listMediStockId, long? timeFrom, long? timeTo, ref List<V_HIS_IMP_MEST_MEDICINE> impMestMedicines, ref List<V_HIS_EXP_MEST_MEDICINE> expMestMedicines)
        {
            //Nhap thuoc, vat tu
            CommonParam paramGet = new CommonParam();
            HisImpMestFilterQuery impMestFilter = new HisImpMestFilterQuery();
            impMestFilter.MEDI_STOCK_IDs = listMediStockId.Count > 0 ? listMediStockId : null;
            impMestFilter.IMP_TIME_FROM = timeFrom;
            impMestFilter.IMP_TIME_TO = timeTo;
            impMestFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
            var listImpMest = new HisImpMestManager(paramGet).Get(impMestFilter) ?? new List<HIS_IMP_MEST>();
            var listImpMestId = listImpMest.Select(o => o.ID).Distinct().ToList();
            if (listImpMestId != null && listImpMestId.Count > 0)
            {
                var skip = 0;
                while (listImpMestId.Count - skip > 0)
                {
                    var limit = listImpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisImpMestMedicineViewFilterQuery ImpMestMedicinefilter = new HisImpMestMedicineViewFilterQuery();
                    ImpMestMedicinefilter.IMP_MEST_IDs = limit;
                    ImpMestMedicinefilter.MEDICINE_TYPE_IDs = ListTypeVitaminA.Select(s => s.ID).ToList();
                    var ImpMestMedicineSub = new HisImpMestMedicineManager().GetView(ImpMestMedicinefilter);
                    impMestMedicines.AddRange(ImpMestMedicineSub);
                }
            }

            //Xuat thuoc, vat tu
            HisExpMestMedicineViewFilterQuery ExpMestMedicinefilter = new HisExpMestMedicineViewFilterQuery();
            ExpMestMedicinefilter.MEDI_STOCK_IDs = listMediStockId.Count > 0 ? listMediStockId : null;
            ExpMestMedicinefilter.EXP_TIME_FROM = timeFrom;
            ExpMestMedicinefilter.EXP_TIME_TO = timeTo;
            ExpMestMedicinefilter.IS_EXPORT = true;
            ExpMestMedicinefilter.MEDICINE_TYPE_IDs = ListTypeVitaminA.Select(s => s.ID).ToList();
            var ExpMestMedicineSub = new HisExpMestMedicineManager().GetView(ExpMestMedicinefilter);
            expMestMedicines.AddRange(ExpMestMedicineSub);
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(ListBranch) && IsNotNullOrEmpty(ListVitaminA))
                {
                    ListRdo.Clear();

                    foreach (var branch in ListBranch)
                    {
                        var lstVtmA = ListVitaminA.Where(o => o.BRANCH_ID == branch.ID).ToList();
                        List<long> listMediStockIds = new List<long>();
                        if (DicMedistock.ContainsKey(branch.ID))
                        {
                            listMediStockIds = DicMedistock[branch.ID].Select(s => s.ID).ToList();
                        }

                        decimal? beginAmount = null;
                        decimal? inAmountl = null;
                        decimal? outAmount = null;
                        decimal? endAmount = null;

                        ProcessAmountVitaminA(listMediStockIds, ref beginAmount, ref inAmountl, ref outAmount, ref endAmount);

                        var lstTreatment = ListTreatment.Where(o => o.BRANCH_ID == branch.ID).ToList();

                        var rdo = new Mrs00612RDO(branch, lstVtmA, lstTreatment, ListBaby, beginAmount, inAmountl, outAmount, endAmount);

                        ListRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessAmountVitaminA(List<long> listMediStockIds, ref decimal? beginAmount, ref decimal? inAmount, ref decimal? outAmount, ref decimal? endAmount)
        {
            try
            {
                if (IsNotNullOrEmpty(listMediStockIds))
                {
                    List<V_HIS_MEST_PERIOD_MEDI> periodMedi = new List<V_HIS_MEST_PERIOD_MEDI>();
                    List<V_HIS_IMP_MEST_MEDICINE> lstImpMestBefore = listImpMestMedicineBefore.Where(o => listMediStockIds.Contains(o.MEDI_STOCK_ID)).ToList();
                    List<V_HIS_EXP_MEST_MEDICINE> lstExpMestBefore = listExpMestMedicineBefore.Where(o => listMediStockIds.Contains(o.MEDI_STOCK_ID)).ToList();
                    List<V_HIS_IMP_MEST_MEDICINE> lstImpMestOn = listImpMestMedicineOn.Where(o => listMediStockIds.Contains(o.MEDI_STOCK_ID)).ToList();
                    List<V_HIS_EXP_MEST_MEDICINE> lstExpMestOn = listExpMestMedicineOn.Where(o => listMediStockIds.Contains(o.MEDI_STOCK_ID)).ToList();

                    foreach (var item in listMediStockIds)
                    {
                        if (dicMestPeriodMedi.ContainsKey(item)) periodMedi = dicMestPeriodMedi[item];
                    }

                    if (IsNotNullOrEmpty(lstImpMestBefore) || IsNotNullOrEmpty(lstExpMestBefore) || IsNotNullOrEmpty(periodMedi))
                    {
                        if (!beginAmount.HasValue) beginAmount = 0;

                        if (IsNotNullOrEmpty(periodMedi)) beginAmount += periodMedi.Sum(s => s.AMOUNT);
                        if (IsNotNullOrEmpty(lstImpMestBefore)) beginAmount += lstImpMestBefore.Sum(s => s.AMOUNT);
                        if (IsNotNullOrEmpty(lstExpMestBefore)) beginAmount -= lstExpMestBefore.Sum(s => s.AMOUNT);
                    }

                    if (IsNotNullOrEmpty(lstImpMestOn))
                    {
                        if (!inAmount.HasValue) inAmount = 0;

                        inAmount += lstImpMestOn.Sum(s => s.AMOUNT);
                    }

                    if (IsNotNullOrEmpty(lstExpMestOn))
                    {
                        if (!outAmount.HasValue) outAmount = 0;

                        outAmount += lstExpMestOn.Sum(s => s.AMOUNT);
                    }

                    if (beginAmount.HasValue || inAmount.HasValue || outAmount.HasValue)
                    {
                        if (!endAmount.HasValue) endAmount = 0;

                        endAmount = (beginAmount ?? 0) + (inAmount ?? 0) - (outAmount ?? 0);
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
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO ?? 0));

                objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
