using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMediStock;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisPatientTypeAlter;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00488
{
    public class Mrs00488Processor : AbstractProcessor
    {
        Mrs00488Filter castFilter = null;
        Dictionary<string, Mrs00488RDO> dicRdo = new Dictionary<string, Mrs00488RDO>();
        List<Mrs00488RDO> listRdo = new List<Mrs00488RDO>();
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<HIS_IMP_MEST> ListMobaImpMest = new List<HIS_IMP_MEST>();
        Dictionary<long, HIS_MEDICINE_TYPE> dicMedicineType = new Dictionary<long, HIS_MEDICINE_TYPE>();
        List<V_HIS_IMP_MEST_MEDICINE> ListMobaImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();

        public static Dictionary<string, HIS_CONFIG> dicConfigMediStock = new Dictionary<string, HIS_CONFIG>();
        string WardsMediStocks = "MRS.HIS_RS.HIS_MEDI_STOCK.MEDI_STOCK_CODE.WARDS";
        List<V_HIS_MEDI_STOCK> wardsMediStocks = new List<V_HIS_MEDI_STOCK>();

        public Mrs00488Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00488Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = (Mrs00488Filter)this.reportFilter;
                dicConfigMediStock = MRS.MANAGER.Config.Loader.dictionaryConfig;
                if (dicConfigMediStock.ContainsKey(WardsMediStocks))
                {
                    string WardsMediStockString = dicConfigMediStock[WardsMediStocks].VALUE ?? "";
                    Inventec.Common.Logging.LogSystem.Info("dicConfigMediStock[WardsMediStocks].VALUE" + dicConfigMediStock[WardsMediStocks].VALUE);
                    List<string> listWardMediStockCode = WardsMediStockString.Split(',').ToList();
                    List<V_HIS_MEDI_STOCK> mediStocks = HisMediStockCFG.HisMediStocks;
                    wardsMediStocks = IsNotNullOrEmpty(mediStocks) ? mediStocks.Where(o => listWardMediStockCode.Contains(o.MEDI_STOCK_CODE)).ToList() : new List<V_HIS_MEDI_STOCK>();
                }
                //thuoc da xuat
                GetExpMestMedicine();

                GetWardsMedicine();
                listExpMestMedicine = listExpMestMedicine.Where(o => o.ACTIVE_INGR_BHYT_NAME != null).ToList();

                var expMestIds = listExpMestMedicine.Select(o => o.EXP_MEST_ID ?? 0).Distinct().ToList();
                //thuoc da thu hoi
                GetMobaImpMest(expMestIds);
                //loai thuoc
                var medicineTypeIds = listExpMestMedicine.Select(o => o.MEDICINE_TYPE_ID).Distinct().ToList();
                GetDicMedicineType(medicineTypeIds);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetDicMedicineType(List<long> medicineTypeIds)
        {

            if (IsNotNullOrEmpty(medicineTypeIds))
            {
                List<HIS_MEDICINE_TYPE> listMedicineType = new List<HIS_MEDICINE_TYPE>();
                var skip = 0;
                while (medicineTypeIds.Count - skip > 0)
                {
                    var listIDs = medicineTypeIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisMedicineTypeFilterQuery tyFilter = new HisMedicineTypeFilterQuery();
                    tyFilter.IDs = listIDs;
                    var medicineTypesub = new HisMedicineTypeManager().Get(tyFilter);
                    listMedicineType.AddRange(medicineTypesub);
                }
                dicMedicineType = listMedicineType.GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.First());
            }
        }

        private void GetWardsMedicine()
        {
            if (wardsMediStocks != null && wardsMediStocks.Count > 0)
            {
                HisExpMestViewFilterQuery ChmsExpMestfilter = new HisExpMestViewFilterQuery();
                ChmsExpMestfilter.IMP_MEDI_STOCK_IDs = wardsMediStocks.Select(o => o.ID).ToList();
                ChmsExpMestfilter.FINISH_DATE_FROM = castFilter.TIME_FROM;
                ChmsExpMestfilter.FINISH_DATE_TO = castFilter.TIME_TO;
                ChmsExpMestfilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                ChmsExpMestfilter.EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK;
                var ChmsExpMest = new HisExpMestManager().GetView(ChmsExpMestfilter);
                var chmsExpMestId = ChmsExpMest.Select(o => o.ID).Distinct().ToList();
                if (chmsExpMestId != null && chmsExpMestId.Count > 0)
                {
                    var skip = 0;
                    while (chmsExpMestId.Count - skip > 0)
                    {
                        var limit = chmsExpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisExpMestMedicineViewFilterQuery wardsExpMestMedcinefilter = new HisExpMestMedicineViewFilterQuery();
                        wardsExpMestMedcinefilter.EXP_MEST_IDs = limit;
                        wardsExpMestMedcinefilter.IS_EXPORT = true;
                        var chmsExpMestMedicineSub = new HisExpMestMedicineManager().GetView(wardsExpMestMedcinefilter);
                        listExpMestMedicine.AddRange(chmsExpMestMedicineSub);
                    }
                }
            }
        }

        private void GetMobaImpMest(List<long> expMestIds)
        {
            if (expMestIds != null && expMestIds.Count > 0)
            {
                var skip = 0;
                while (expMestIds.Count - skip > 0)
                {
                    var limit = expMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisImpMestFilterQuery mobaImpMestfilter = new HisImpMestFilterQuery();
                    mobaImpMestfilter.MOBA_EXP_MEST_IDs = limit;
                    var mobaImpMestSub = new HisImpMestManager().Get(mobaImpMestfilter);
                    ListMobaImpMest.AddRange(mobaImpMestSub);
                }
            }
            var mobaImpMestId = ListMobaImpMest.Select(o => o.ID).Distinct().ToList();

            if (mobaImpMestId != null && mobaImpMestId.Count > 0)
            {
                var skip = 0;
                while (mobaImpMestId.Count - skip > 0)
                {
                    var limit = mobaImpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisImpMestMedicineViewFilterQuery mobaImpMestMedcinefilter = new HisImpMestMedicineViewFilterQuery();
                    mobaImpMestMedcinefilter.IMP_MEST_IDs = limit;
                    var mobaImpMestMedicineSub = new HisImpMestMedicineManager().GetView(mobaImpMestMedcinefilter);
                    ListMobaImpMestMedicine.AddRange(mobaImpMestMedicineSub);
                }
            }
        }

        private void GetExpMestMedicine()
        {
            HisExpMestMedicineViewFilterQuery filterExp = new HisExpMestMedicineViewFilterQuery();
            filterExp.EXP_TIME_FROM = castFilter.TIME_FROM;
            filterExp.EXP_TIME_TO = castFilter.TIME_TO;
            filterExp.IS_EXPORT = true;
            filterExp.EXP_MEST_TYPE_IDs = new List<long>()
            {
                IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,
                IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT,
                IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP
            };
            listExpMestMedicine = new HisExpMestMedicineManager().GetView(filterExp);
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                processorExportData();
                processorMobaData();
                listRdo.AddRange(dicRdo.Values.OrderBy(o => o.ACTIVE_INGR_BHYT_NAME).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void processorMobaData()
        {
            string key = "";
            foreach (var item in ListMobaImpMestMedicine)
            {
                key = item.MEDICINE_TYPE_ID + "_" + item.IMP_PRICE;
                if (!dicRdo.ContainsKey(key))
                    continue;
                else dicRdo[key].AMOUNT -= item.AMOUNT;
            }
        }

        private void processorExportData()
        {
            string key = "";
            foreach (var item in listExpMestMedicine)
            {
                key = item.MEDICINE_TYPE_ID + "_" + item.IMP_PRICE;
                if (!dicRdo.ContainsKey(key))
                    dicRdo[key] = new Mrs00488RDO(item, dicMedicineType);
                else dicRdo[key].AMOUNT += item.AMOUNT;
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            if (this.castFilter.TIME_FROM > 0)
                dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(this.castFilter.TIME_FROM ?? 0));
            if (this.castFilter.TIME_TO > 0)
                dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(this.castFilter.TIME_TO ?? 0));

            objectTag.AddObjectData(store, "Report", listRdo);
        }
    }
}
