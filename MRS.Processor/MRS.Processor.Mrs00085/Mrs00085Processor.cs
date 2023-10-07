using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisImpMestType;
using MOS.MANAGER.HisExpMestType;
using MOS.MANAGER.HisMaterialType;

using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO;

using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMestPeriodMety;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisMestPeriodMate;
using MOS.MANAGER.HisTreatment;

namespace MRS.Processor.Mrs00085
{
    public class Mrs00085Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam();
        Dictionary<long, Mrs00085RDO> dicMaterialType = new Dictionary<long, Mrs00085RDO>();
        HIS_DEPARTMENT department = new HIS_DEPARTMENT();
        HIS_MEDI_STOCK mediStock = new HIS_MEDI_STOCK();
        private string a = "";
        Mrs00085Filter filter = null;

        List<V_HIS_MATERIAL_TYPE> listMaterialType = new List<V_HIS_MATERIAL_TYPE>();

        List<Mrs00085RDO> ListRdo = new List<Mrs00085RDO>();

        public Mrs00085Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            a = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00085Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                filter = (Mrs00085Filter)this.reportFilter;

                List<HIS_EXP_MEST_TYPE> expMestTypes = new MOS.MANAGER.HisExpMestType.HisExpMestTypeManager().Get(new HisExpMestTypeFilterQuery());
                List<HIS_IMP_MEST_TYPE> impMestTypes = new MOS.MANAGER.HisImpMestType.HisImpMestTypeManager().Get(new HisImpMestTypeFilterQuery());
                listMaterialType = new HisMaterialTypeManager().GetView(new HisMaterialTypeViewFilterQuery());
                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                impMateFilter.IMP_TIME_FROM = filter.TIME_FROM;
                impMateFilter.IMP_TIME_TO = filter.TIME_TO;
                impMateFilter.MEDI_STOCK_ID = filter.MEDI_STOCK_ID;
                impMateFilter.MEDI_STOCK_IDs = filter.MEDI_STOCK_IDs;
                impMateFilter.MATERIAL_TYPE_ID = filter.MATERIAL_TYPE_ID;
                impMateFilter.MATERIAL_TYPE_IDs = filter.MATERIAL_TYPE_IDs;
                impMateFilter.MATERIAL_IDs = filter.MATERIAL_IDs;
                impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                impMateFilter.ORDER_FIELD = "IMP_TIME";
                impMateFilter.ORDER_DIRECTION = "ASC";
                List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterials = new HisImpMestMaterialManager().GetView(impMateFilter);

                List<long> impMestIds = hisImpMestMaterials.Select(o => o.IMP_MEST_ID).ToList();
                var aggrImpId = hisImpMestMaterials.Select(o => o.AGGR_IMP_MEST_ID ?? 0).ToList();
                if (IsNotNullOrEmpty(aggrImpId))
                    impMestIds.AddRange(aggrImpId);

                //
                List<V_HIS_IMP_MEST> ImpMest = new List<V_HIS_IMP_MEST>();
                if (IsNotNullOrEmpty(impMestIds))
                {
                    impMestIds = impMestIds.Distinct().ToList();
                    var skip = 0;
                    while (impMestIds.Count - skip > 0)
                    {
                        var listIDs = impMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisImpMestViewFilterQuery ImpFilter = new HisImpMestViewFilterQuery()
                        {
                            IDs = listIDs
                        };
                        var x = new HisImpMestManager(paramGet).GetView(ImpFilter);
                        ImpMest.AddRange(x);
                    }
                }

                //
                List<long> sourceMestIds = ImpMest.Where(o => o.CHMS_EXP_MEST_ID.HasValue).Select(s => s.CHMS_EXP_MEST_ID.Value).ToList();
                List<V_HIS_EXP_MEST> sourceMest = new List<V_HIS_EXP_MEST>();
                if (IsNotNullOrEmpty(sourceMestIds))
                {
                    var skip = 0;
                    while (sourceMestIds.Count - skip > 0)
                    {
                        var listIDs = sourceMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisExpMestViewFilterQuery sourceFilter = new HisExpMestViewFilterQuery()
                        {
                            IDs = listIDs,
                        };
                        var x = new HisExpMestManager(paramGet).GetView(sourceFilter);
                        sourceMest.AddRange(x);
                    }
                }

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                expMateFilter.EXP_TIME_FROM = filter.TIME_FROM;
                expMateFilter.EXP_TIME_TO = filter.TIME_TO;
                expMateFilter.MEDI_STOCK_ID = filter.MEDI_STOCK_ID;
                expMateFilter.MEDI_STOCK_IDs = filter.MEDI_STOCK_IDs;
                expMateFilter.MATERIAL_TYPE_ID = filter.MATERIAL_TYPE_ID;
                expMateFilter.MATERIAL_TYPE_IDs = filter.MATERIAL_TYPE_IDs;
                expMateFilter.MATERIAL_IDs = filter.MATERIAL_IDs;
                expMateFilter.IS_EXPORT = true;
                expMateFilter.ORDER_FIELD = "EXP_TIME";
                expMateFilter.ORDER_DIRECTION = "ASC";
                List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new HisExpMestMaterialManager().GetView(expMateFilter);

                List<long> hisExpMestMaterialIds = hisExpMestMaterial.Select(o => o.EXP_MEST_ID ?? 0).ToList();
                var aggrExpMest = hisExpMestMaterial.Select(o => o.AGGR_EXP_MEST_ID ?? 0).ToList();
                if (IsNotNullOrEmpty(aggrExpMest))
                    hisExpMestMaterialIds.AddRange(aggrExpMest);


                List<V_HIS_EXP_MEST> hisExpMest = new List<V_HIS_EXP_MEST>();
                if (IsNotNullOrEmpty(hisExpMestMaterialIds))
                {
                    hisExpMestMaterialIds = hisExpMestMaterialIds.Distinct().ToList();
                    var skip = 0;
                    while (hisExpMestMaterialIds.Count - skip > 0)
                    {
                        var listIDs = hisExpMestMaterialIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisExpMestViewFilterQuery ExpFilter = new HisExpMestViewFilterQuery()
                        {
                            IDs = listIDs
                        };
                        var x = new HisExpMestManager(paramGet).GetView(ExpFilter);
                        hisExpMest.AddRange(x);
                    }
                }

                //
                List<long> destMestIds = hisExpMest.Where(p => p.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK).Select(o => o.ID).ToList();
                List<V_HIS_IMP_MEST> destMest = new List<V_HIS_IMP_MEST>();
                if (IsNotNullOrEmpty(destMestIds))
                {
                    var skip = 0;
                    while (sourceMestIds.Count - skip > 0)
                    {
                        var listIDs = destMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisImpMestViewFilterQuery destFilter = new HisImpMestViewFilterQuery()
                        {
                            CHMS_EXP_MEST_IDs = listIDs,
                        };
                        var x = new HisImpMestManager(paramGet).GetView(destFilter);
                        destMest.AddRange(x);
                    }
                }
                //tinh ton dau truoc
                if (filter.MEDI_STOCK_IDs != null)
                {
                    foreach (var item in filter.MEDI_STOCK_IDs)
                    {

                        ProcessGetPeriod(paramGet, item);
                    }
                }
                else if (filter.MEDI_STOCK_ID != null)
                {

                    ProcessGetPeriod(paramGet, filter.MEDI_STOCK_ID ?? 0);
                }
                else
                {
                    foreach (var item in HisMediStockCFG.HisMediStocks.Select(o => o.ID).ToList())
                    {

                        ProcessGetPeriod(paramGet, item);
                    }
                }

                if (IsNotNullOrEmpty(hisImpMestMaterials))
                {
                    var listSub = (from r in hisImpMestMaterials select new Mrs00085RDO(r, ImpMest, sourceMest, impMestTypes)).ToList();
                    var groupByCode = listSub.GroupBy(o => new { o.MATERIAL_TYPE_ID, o.IMP_MEST_CODE }).ToList();

                    foreach (var group in groupByCode)
                    {
                        List<Mrs00085RDO> p = group.ToList<Mrs00085RDO>();
                        Mrs00085RDO rdo = new Mrs00085RDO()
                        {
                            
                            AGGR_IMP_MEST_ID = p.First().AGGR_IMP_MEST_ID,
                            MATERIAL_TYPE_ID = p.First().MATERIAL_TYPE_ID,
                            MATERIAL_ID = p.First().MATERIAL_ID,
                            MATERIAL_TYPE_CODE = p.First().MATERIAL_TYPE_CODE,
                            MATERIAL_TYPE_NAME = p.First().MATERIAL_TYPE_NAME,
                            EXP_MEST_CODE = p.First().EXP_MEST_CODE,
                            DESCRIPTION = p.First().DESCRIPTION,
                            EXECUTE_DATE_STR = p.First().EXECUTE_DATE_STR,
                            EXECUTE_TIME = p.First().EXECUTE_TIME,
                            IMP_MEST_CODE = p.First().IMP_MEST_CODE,
                            IMP_MEST_SUB_CODE = p.First().IMP_MEST_SUB_CODE,
                            IMP_MEST_TYPE_CODE = p.First().IMP_MEST_TYPE_CODE,
                            IMP_MEST_TYPE_NAME = p.First().IMP_MEST_TYPE_NAME,
                            EXP_MEST_TYPE_NAME = p.First().EXP_MEST_TYPE_NAME,
                            PACKAGE_NUMBER = string.Join(", ", p.Select(q => q.PACKAGE_NUMBER).Distinct().ToList()),
                            EXPIRED_DATE = p.First().EXPIRED_DATE,
                            EXPIRED_DATE_STR = p.First().EXPIRED_DATE_STR,
                            IMP_AMOUNT = p.Sum(q => q.IMP_AMOUNT) == 0 ? null : p.Sum(q => q.IMP_AMOUNT),
                            REQ_DEPARTMENT_NAME = p.First().REQ_DEPARTMENT_NAME,
                            REQUEST_DEPARTMENT_NAME = p.First().REQUEST_DEPARTMENT_NAME,
                            REQ_ROOM_NAME = p.First().REQ_ROOM_NAME,
                            MEDI_STOCK_NAME = p.First().MEDI_STOCK_NAME,
                            EXP_MEDI_STOCK_CODE = p.First().EXP_MEDI_STOCK_CODE,
                            EXP_MEDI_STOCK_NAME = p.First().EXP_MEDI_STOCK_NAME,
                            IMP_MEDI_STOCK_CODE = p.First().IMP_MEDI_STOCK_CODE,
                            IMP_MEDI_STOCK_NAME = p.First().IMP_MEDI_STOCK_NAME,
                            CLIENT_NAME = p.First().CLIENT_NAME,
                            VIR_PATIENT_NAME = p.First().VIR_PATIENT_NAME,
                            VIR_PATIENT_ADDRESS = p.First().VIR_PATIENT_ADDRESS,
                            TREATMENT_CODE = p.First().TREATMENT_CODE,
                            TREATMENT_ID = p.First().TREATMENT_ID,
                            SUPPLIER_NAME = p.First().SUPPLIER_NAME,
                            SECOND_MEST_CODE = p.First().SECOND_MEST_CODE,
                            IMP_PRICE = p.First().IMP_PRICE,
                            IMP_VAT_RATIO = p.First().IMP_VAT_RATIO,
                            CHMS_TYPE_ID = p.First().CHMS_TYPE_ID,
                            MOBA_EXP_MEST_ID = p.First().MOBA_EXP_MEST_ID,

                        };
                        ListRdo.Add(rdo);
                        if (!dicMaterialType.ContainsKey(rdo.MATERIAL_TYPE_ID))
                        {
                            dicMaterialType[rdo.MATERIAL_TYPE_ID] = new Mrs00085RDO();
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].MATERIAL_TYPE_ID = rdo.MATERIAL_TYPE_ID;
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].MATERIAL_TYPE_CODE = rdo.MATERIAL_TYPE_CODE;
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].MATERIAL_TYPE_NAME = rdo.MATERIAL_TYPE_NAME;
                            V_HIS_MATERIAL_TYPE medicineType = listMaterialType.FirstOrDefault(o => rdo.MATERIAL_TYPE_ID == o.ID) ?? new V_HIS_MATERIAL_TYPE();
                            if (medicineType != null)
                            {
                                dicMaterialType[rdo.MATERIAL_TYPE_ID].NATIONAL_NAME = medicineType.NATIONAL_NAME;
                                dicMaterialType[rdo.MATERIAL_TYPE_ID].MANUFACTURER_NAME = medicineType.MANUFACTURER_NAME;
                                //dicMaterialType[rdo.MATERIAL_TYPE_ID].ACTIVE_INGR_BHYT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME;
                                dicMaterialType[rdo.MATERIAL_TYPE_ID].RECORDING_TRANSACTION = medicineType.RECORDING_TRANSACTION;
                                dicMaterialType[rdo.MATERIAL_TYPE_ID].SERVICE_UNIT_NAME = medicineType.SERVICE_UNIT_NAME;
                            }
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].END_AMOUNT = rdo.IMP_AMOUNT ?? 0;
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].END_TOTAL_PRICE = (rdo.IMP_AMOUNT ?? 0) * rdo.IMP_PRICE * (1 + rdo.IMP_VAT_RATIO);
                        }
                        else
                        {
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].END_AMOUNT += rdo.IMP_AMOUNT ?? 0;
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].END_TOTAL_PRICE += (rdo.IMP_AMOUNT ?? 0) * rdo.IMP_PRICE * (1 + rdo.IMP_VAT_RATIO);
                        }
                    }
                }

                if (IsNotNullOrEmpty(hisExpMestMaterial))
                {
                    var listSub = (from r in hisExpMestMaterial select new Mrs00085RDO(r, hisExpMest, destMest, expMestTypes)).ToList();
                    var groupByCode = listSub.GroupBy(o => new { o.MATERIAL_TYPE_ID, o.EXP_MEST_CODE }).ToList();

                    foreach (var group in groupByCode)
                    {
                        List<Mrs00085RDO> p = group.ToList<Mrs00085RDO>();
                        Mrs00085RDO rdo = new Mrs00085RDO()
                        {
                            
                            MATERIAL_ID = p.First().MATERIAL_ID,
                            MATERIAL_TYPE_ID = p.First().MATERIAL_TYPE_ID,
                            TDL_PATIENT_TYPE_CODE = p.First().TDL_PATIENT_TYPE_CODE,
                            TDL_PATIENT_TYPE_NAME = p.First().TDL_PATIENT_TYPE_NAME,
                            MATERIAL_TYPE_CODE = p.First().MATERIAL_TYPE_CODE,
                            MATERIAL_TYPE_NAME = p.First().MATERIAL_TYPE_NAME,
                            EXP_MEST_CODE = p.First().EXP_MEST_CODE,
                            DESCRIPTION = p.First().DESCRIPTION,
                            EXECUTE_DATE_STR = p.First().EXECUTE_DATE_STR,
                            EXECUTE_TIME = p.First().EXECUTE_TIME,
                            IMP_MEST_CODE = p.First().IMP_MEST_CODE,
                            IMP_MEST_SUB_CODE = p.First().IMP_MEST_SUB_CODE,
                            IMP_MEST_TYPE_CODE = p.First().IMP_MEST_TYPE_CODE,
                            IMP_MEST_TYPE_NAME = p.First().IMP_MEST_TYPE_NAME,
                            EXP_MEST_TYPE_NAME = p.First().EXP_MEST_TYPE_NAME,
                            PACKAGE_NUMBER = string.Join(", ", p.Select(q => q.PACKAGE_NUMBER).Distinct().ToList()),
                            EXPIRED_DATE = p.First().EXPIRED_DATE,
                            EXPIRED_DATE_STR = p.First().EXPIRED_DATE_STR,
                            EXP_AMOUNT = p.Sum(q => q.EXP_AMOUNT) == 0 ? null : p.Sum(q => q.EXP_AMOUNT),
                            REQ_DEPARTMENT_NAME = p.First().REQ_DEPARTMENT_NAME,
                            REQUEST_DEPARTMENT_NAME = p.First().REQUEST_DEPARTMENT_NAME,
                            REQ_ROOM_NAME = p.First().REQ_ROOM_NAME,
                            MEDI_STOCK_NAME = p.First().MEDI_STOCK_NAME,
                            EXP_MEDI_STOCK_CODE = p.First().EXP_MEDI_STOCK_CODE,
                            EXP_MEDI_STOCK_NAME = p.First().EXP_MEDI_STOCK_NAME,
                            IMP_MEDI_STOCK_CODE = p.First().IMP_MEDI_STOCK_CODE,
                            IMP_MEDI_STOCK_NAME = p.First().IMP_MEDI_STOCK_NAME,
                            CLIENT_NAME = p.First().CLIENT_NAME,
                            VIR_PATIENT_NAME = p.First().VIR_PATIENT_NAME,
                            VIR_PATIENT_ADDRESS = p.First().VIR_PATIENT_ADDRESS,
                            TREATMENT_CODE = p.First().TREATMENT_CODE,
                            TREATMENT_ID = p.First().TREATMENT_ID,
                            SECOND_MEST_CODE = p.First().SECOND_MEST_CODE,
                            IMP_PRICE = p.First().IMP_PRICE,
                            IMP_VAT_RATIO = p.First().IMP_VAT_RATIO,
                            CHMS_TYPE_ID = p.First().CHMS_TYPE_ID,
                            DESCRIPTION_DETAIL = p.First().DESCRIPTION_DETAIL,
                            EXP_MEST_REASON_NAME = p.First().EXP_MEST_REASON_NAME,
                            RECEIVING_PLACE = p.First().RECEIVING_PLACE,
                        };
                        ListRdo.Add(rdo);
                        if (!dicMaterialType.ContainsKey(rdo.MATERIAL_TYPE_ID))
                        {
                            dicMaterialType[rdo.MATERIAL_TYPE_ID] = new Mrs00085RDO();
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].MATERIAL_TYPE_ID = rdo.MATERIAL_TYPE_ID;
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].MATERIAL_TYPE_CODE = rdo.MATERIAL_TYPE_CODE;
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].MATERIAL_TYPE_NAME = rdo.MATERIAL_TYPE_NAME;
                            V_HIS_MATERIAL_TYPE medicineType = listMaterialType.FirstOrDefault(o => rdo.MATERIAL_TYPE_ID == o.ID) ?? new V_HIS_MATERIAL_TYPE();
                            if (medicineType != null)
                            {
                                dicMaterialType[rdo.MATERIAL_TYPE_ID].NATIONAL_NAME = medicineType.NATIONAL_NAME;
                                dicMaterialType[rdo.MATERIAL_TYPE_ID].MANUFACTURER_NAME = medicineType.MANUFACTURER_NAME;
                                //dicMaterialType[rdo.MATERIAL_TYPE_ID].ACTIVE_INGR_BHYT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME;
                                dicMaterialType[rdo.MATERIAL_TYPE_ID].RECORDING_TRANSACTION = medicineType.RECORDING_TRANSACTION;
                                dicMaterialType[rdo.MATERIAL_TYPE_ID].SERVICE_UNIT_NAME = medicineType.SERVICE_UNIT_NAME;
                            }
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].END_AMOUNT = -rdo.EXP_AMOUNT ?? 0;
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].END_TOTAL_PRICE -= (rdo.EXP_AMOUNT ?? 0) * rdo.IMP_PRICE * (1 + rdo.IMP_VAT_RATIO);
                        }
                        else
                        {
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].END_AMOUNT -= (rdo.EXP_AMOUNT ?? 0);
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].END_TOTAL_PRICE -= (rdo.EXP_AMOUNT ?? 0) * rdo.IMP_PRICE * (1 + rdo.IMP_VAT_RATIO);
                        }

                    }
                }

                if (!IsNotNullOrEmpty(hisImpMestMaterials) && !IsNotNullOrEmpty(hisExpMestMaterial) && IsNotNullOrEmpty(listMaterialType) && filter.IS_NOT_SHOW_ALL != true)
                {
                    var listSub = (from r in listMaterialType select new Mrs00085RDO(r, filter)).ToList();
                    var groupByCode = listSub.GroupBy(o => new { o.MATERIAL_TYPE_ID }).ToList();

                    foreach (var group in groupByCode)
                    {
                        List<Mrs00085RDO> p = group.ToList<Mrs00085RDO>();
                        Mrs00085RDO rdo = new Mrs00085RDO()
                        {
                            MATERIAL_ID = p.First().MATERIAL_ID,
                            MATERIAL_TYPE_ID = p.First().MATERIAL_TYPE_ID,
                            MATERIAL_TYPE_CODE = p.First().MATERIAL_TYPE_CODE,
                            MATERIAL_TYPE_NAME = p.First().MATERIAL_TYPE_NAME,
                            MANUFACTURER_NAME = p.First().MANUFACTURER_NAME,
                            ACTIVE_INGR_BHYT_NAME = p.First().ACTIVE_INGR_BHYT_NAME,
                            RECORDING_TRANSACTION = p.First().RECORDING_TRANSACTION,
                            SERVICE_UNIT_NAME = p.First().SERVICE_UNIT_NAME,
                            EXPIRED_DATE = p.First().EXPIRED_DATE,
                            EXPIRED_DATE_STR = p.First().EXPIRED_DATE_STR,
                            EXP_AMOUNT = p.Sum(q => q.EXP_AMOUNT) == 0 ? 0 : p.Sum(q => q.EXP_AMOUNT),

                            IMP_PRICE = p.First().IMP_PRICE,
                            IMP_VAT_RATIO = p.First().IMP_VAT_RATIO
                        };
                        ListRdo.Add(rdo);
                        if (!dicMaterialType.ContainsKey(rdo.MATERIAL_TYPE_ID))
                        {
                            dicMaterialType[rdo.MATERIAL_TYPE_ID] = new Mrs00085RDO();
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].MATERIAL_TYPE_ID = rdo.MATERIAL_TYPE_ID;
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].MATERIAL_TYPE_CODE = rdo.MATERIAL_TYPE_CODE;
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].MATERIAL_TYPE_NAME = rdo.MATERIAL_TYPE_NAME;
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].NATIONAL_NAME = rdo.NATIONAL_NAME;
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].MANUFACTURER_NAME = rdo.MANUFACTURER_NAME;
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].ACTIVE_INGR_BHYT_NAME = rdo.ACTIVE_INGR_BHYT_NAME;
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].RECORDING_TRANSACTION = rdo.RECORDING_TRANSACTION;
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].SERVICE_UNIT_NAME = rdo.SERVICE_UNIT_NAME;
                            //V_HIS_MEDICINE_TYPE medicineType = listMedicineType.FirstOrDefault(o => rdo.MEDICINE_TYPE_ID == o.ID) ?? new V_HIS_MEDICINE_TYPE();
                            //if (medicineType != null)
                            //{
                            //    dicMedicineType[rdo.MEDICINE_TYPE_ID].NATIONAL_NAME = medicineType.NATIONAL_NAME;
                            //    dicMedicineType[rdo.MEDICINE_TYPE_ID].MANUFACTURER_NAME = medicineType.MANUFACTURER_NAME;
                            //    dicMedicineType[rdo.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME;
                            //    dicMedicineType[rdo.MEDICINE_TYPE_ID].RECORDING_TRANSACTION = medicineType.RECORDING_TRANSACTION;
                            //    dicMedicineType[rdo.MEDICINE_TYPE_ID].SERVICE_UNIT_NAME = medicineType.SERVICE_UNIT_NAME;
                            //}
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].END_AMOUNT = -rdo.EXP_AMOUNT ?? 0;
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].END_TOTAL_PRICE -= (rdo.EXP_AMOUNT ?? 0) * rdo.IMP_PRICE * (1 + rdo.IMP_VAT_RATIO);
                        }
                        else
                        {
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].END_AMOUNT -= (rdo.EXP_AMOUNT ?? 0);
                            dicMaterialType[rdo.MATERIAL_TYPE_ID].END_TOTAL_PRICE -= (rdo.EXP_AMOUNT ?? 0) * rdo.IMP_PRICE * (1 + rdo.IMP_VAT_RATIO);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                ProcessBeginAndEndAmount();
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();

            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (((Mrs00085Filter)this.reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00085Filter)this.reportFilter).TIME_FROM));
            }
            if (((Mrs00085Filter)this.reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00085Filter)this.reportFilter).TIME_TO));
            }
            var medicineType = dicMaterialType.Values.FirstOrDefault() ?? new Mrs00085RDO();
            dicSingleTag.Add("BEGIN_AMOUNT", medicineType.BEGIN_AMOUNT);
            dicSingleTag.Add("END_AMOUNT", medicineType.END_AMOUNT);
            dicSingleTag.Add("BEGIN_TOTAL_PRICE", medicineType.BEGIN_TOTAL_PRICE);
            dicSingleTag.Add("END_TOTAL_PRICE", medicineType.END_TOTAL_PRICE);
            dicSingleTag.Add("MATERIAL_TYPE_CODE", medicineType.MATERIAL_TYPE_CODE);
            dicSingleTag.Add("MATERIAL_TYPE_NAME", medicineType.MATERIAL_TYPE_NAME);
            dicSingleTag.Add("SERVICE_UNIT_NAME", medicineType.SERVICE_UNIT_NAME);
            dicSingleTag.Add("NATIONAL_NAME", medicineType.NATIONAL_NAME);
            dicSingleTag.Add("MANUFACTURER_NAME", medicineType.MANUFACTURER_NAME);
            dicSingleTag.Add("ACTIVE_INGR_BHYT_NAME", medicineType.ACTIVE_INGR_BHYT_NAME);
            dicSingleTag.Add("RECORDING_TRANSACTION", medicineType.RECORDING_TRANSACTION);

            HIS_MEDI_STOCK mediStock = new MOS.MANAGER.HisMediStock.HisMediStockManager().GetById(((Mrs00085Filter)this.reportFilter).MEDI_STOCK_ID??0);
            if (IsNotNull(mediStock))
            {
                dicSingleTag.Add("MEDI_STOCK_CODE", mediStock.MEDI_STOCK_CODE);
                dicSingleTag.Add("MEDI_STOCK_NAME", mediStock.MEDI_STOCK_NAME);
            }
            dicSingleTag.Add("MEDI_BEGIN_AMOUNT", medicineType.BEGIN_AMOUNT);
            dicSingleTag.Add("MEDI_END_AMOUNT", medicineType.END_AMOUNT);
            objectTag.AddObjectData(store, "Report", ListRdo);

            objectTag.AddObjectData(store, "Parent", dicMaterialType.Values.Where(o => ListRdo.Exists(p => p.MATERIAL_TYPE_ID == o.MATERIAL_TYPE_ID)).ToList());

            objectTag.AddRelationship(store, "Parent", "Report", "MATERIAL_TYPE_ID", "MATERIAL_TYPE_ID");

            objectTag.AddObjectData(store, "ListMaterial", new List<Mrs00085RDO>());
        }

        private void ProcessBeginAndEndAmount()
        {
            try
            {
                ListRdo = ListRdo.OrderBy(o => o.MATERIAL_TYPE_NAME).ThenBy(o => o.EXECUTE_TIME).ToList();

                foreach (var item in dicMaterialType)
                {
                    decimal previousEndAmount = 0;
                    decimal previousEndTotalPrice = 0;
                    decimal BeginAmount = 0;
                    decimal BeginTotalPrice = 0;
                    var listSub = ListRdo.Where(o => o.MATERIAL_TYPE_ID == item.Key).ToList();
                    if (listSub.Count > 0)
                    {
                        previousEndAmount = item.Value.BEGIN_AMOUNT;
                        previousEndTotalPrice = item.Value.BEGIN_TOTAL_PRICE;
                        BeginAmount = item.Value.BEGIN_AMOUNT;
                        BeginTotalPrice = item.Value.BEGIN_TOTAL_PRICE;
                        foreach (var rdo in listSub)
                        {

                            rdo.CalculateAmount(previousEndAmount);
                            rdo.CalculateTotalPrice(previousEndTotalPrice);
                            previousEndAmount = rdo.END_AMOUNT;
                            previousEndTotalPrice = rdo.END_TOTAL_PRICE;
                        }
                    }
                    else
                    {
                        ListRdo.Add(item.Value);
                    }
                }
                ListRdo = ListRdo.OrderBy(o => o.MATERIAL_TYPE_NAME).ThenBy(o => o.EXECUTE_TIME).ToList();


            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        // Lay kỳ chốt gần nhất với thời gian từ của báo cáo
        private void ProcessGetPeriod(CommonParam paramGet, long mediStockId)
        {
            try
            {
                HisMediStockPeriodViewFilterQuery periodFilter = new HisMediStockPeriodViewFilterQuery();
                periodFilter.TO_TIME_TO = ((Mrs00085Filter)this.reportFilter).TIME_FROM;
                periodFilter.MEDI_STOCK_ID = mediStockId;
                List<V_HIS_MEDI_STOCK_PERIOD> hisMateStockPeriod = new HisMediStockPeriodManager(paramGet).GetView(periodFilter);
                if (!paramGet.HasException)
                {
                    if (IsNotNullOrEmpty(hisMateStockPeriod))
                    {
                        //Trường hợp có kỳ được chốt gần nhất
                        V_HIS_MEDI_STOCK_PERIOD neighborPeriod = hisMateStockPeriod.OrderByDescending(d => d.TO_TIME).ToList()[0];
                        if (neighborPeriod != null)
                        {
                            ProcessBeinAmountMaterialByMateStockPeriod(paramGet, neighborPeriod, mediStockId);
                        }
                    }
                    else
                    {
                        // Trường hợp không có kỳ chốt gần nhât - chưa được chốt kỳ nào
                        ProcessBeinAmountMaterialNotMateStockPriod(paramGet, mediStockId);
                    }

                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu.");
                    }
                }
                else
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu.");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        // Tính số lượng đầu kỳ có kỳ dữ liệu gần nhất
        private void ProcessBeinAmountMaterialByMateStockPeriod(CommonParam paramGet, V_HIS_MEDI_STOCK_PERIOD neighborPeriod, long mediStockId)
        {
            try
            {
                List<Mrs00085RDO> listrdo = new List<Mrs00085RDO>();
                HisMestPeriodMateViewFilterQuery periodMetyFilter = new HisMestPeriodMateViewFilterQuery();
                periodMetyFilter.MEDI_STOCK_PERIOD_ID = neighborPeriod.ID;
                periodMetyFilter.MATERIAL_TYPE_ID = ((Mrs00085Filter)this.reportFilter).MATERIAL_TYPE_ID;
                periodMetyFilter.MATERIAL_TYPE_IDs = ((Mrs00085Filter)this.reportFilter).MATERIAL_TYPE_IDs;
                List<V_HIS_MEST_PERIOD_MATE> hisMestPeriodMate = new HisMestPeriodMateManager(paramGet).GetView(periodMetyFilter) ?? new List<V_HIS_MEST_PERIOD_MATE>();

                ProcessMaterialBefore(hisMestPeriodMate);

                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                impMateFilter.IMP_TIME_FROM = neighborPeriod.TO_TIME + 1;
                impMateFilter.IMP_TIME_TO = ((Mrs00085Filter)this.reportFilter).TIME_FROM - 1;
                impMateFilter.MEDI_STOCK_ID = mediStockId;
                impMateFilter.MATERIAL_TYPE_ID = ((Mrs00085Filter)this.reportFilter).MATERIAL_TYPE_ID;
                impMateFilter.MATERIAL_TYPE_IDs = ((Mrs00085Filter)this.reportFilter).MATERIAL_TYPE_IDs;
                impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new HisImpMestMaterialManager(paramGet).GetView(impMateFilter) ?? new List<V_HIS_IMP_MEST_MATERIAL>();


                ProcessMaterialBefore(hisImpMestMaterial);
                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                expMateFilter.EXP_TIME_FROM = neighborPeriod.TO_TIME + 1;
                expMateFilter.EXP_TIME_TO = ((Mrs00085Filter)this.reportFilter).TIME_FROM - 1;
                expMateFilter.MEDI_STOCK_ID = mediStockId;
                expMateFilter.MATERIAL_TYPE_ID = ((Mrs00085Filter)this.reportFilter).MATERIAL_TYPE_ID;
                expMateFilter.MATERIAL_TYPE_IDs = ((Mrs00085Filter)this.reportFilter).MATERIAL_TYPE_IDs;
                expMateFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new HisExpMestMaterialManager(paramGet).GetView(expMateFilter) ?? new List<V_HIS_EXP_MEST_MATERIAL>();
                ProcessMaterialBefore(hisExpMestMaterial);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessMaterialBefore(List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial)
        {
            try
            {
                foreach (var item in hisExpMestMaterial)
                {
                    if (!dicMaterialType.ContainsKey(item.MATERIAL_TYPE_ID))
                    {
                        dicMaterialType[item.MATERIAL_TYPE_ID] = new Mrs00085RDO();
                        dicMaterialType[item.MATERIAL_TYPE_ID].MATERIAL_TYPE_ID = item.MATERIAL_TYPE_ID;
                        dicMaterialType[item.MATERIAL_TYPE_ID].MATERIAL_TYPE_CODE = item.MATERIAL_TYPE_CODE;
                        dicMaterialType[item.MATERIAL_TYPE_ID].MATERIAL_TYPE_NAME = item.MATERIAL_TYPE_NAME;
                        V_HIS_MATERIAL_TYPE medicineType = listMaterialType.FirstOrDefault(o => item.MATERIAL_TYPE_ID == o.ID) ?? new V_HIS_MATERIAL_TYPE();
                        if (medicineType != null)
                        {
                            dicMaterialType[item.MATERIAL_TYPE_ID].NATIONAL_NAME = medicineType.NATIONAL_NAME;
                            dicMaterialType[item.MATERIAL_TYPE_ID].MANUFACTURER_NAME = medicineType.MANUFACTURER_NAME;
                            //dicMaterialType[item.MATERIAL_TYPE_ID].ACTIVE_INGR_BHYT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME;
                            dicMaterialType[item.MATERIAL_TYPE_ID].RECORDING_TRANSACTION = medicineType.RECORDING_TRANSACTION;
                            dicMaterialType[item.MATERIAL_TYPE_ID].SERVICE_UNIT_NAME = medicineType.SERVICE_UNIT_NAME;
                        }
                        dicMaterialType[item.MATERIAL_TYPE_ID].BEGIN_AMOUNT = -item.AMOUNT;
                        dicMaterialType[item.MATERIAL_TYPE_ID].END_AMOUNT = -item.AMOUNT;
                        dicMaterialType[item.MATERIAL_TYPE_ID].BEGIN_TOTAL_PRICE -= item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                        dicMaterialType[item.MATERIAL_TYPE_ID].END_TOTAL_PRICE -= item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                    }
                    else
                    {
                        dicMaterialType[item.MATERIAL_TYPE_ID].BEGIN_AMOUNT -= item.AMOUNT;
                        dicMaterialType[item.MATERIAL_TYPE_ID].END_AMOUNT -= item.AMOUNT;
                        dicMaterialType[item.MATERIAL_TYPE_ID].BEGIN_TOTAL_PRICE -= item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                        dicMaterialType[item.MATERIAL_TYPE_ID].END_TOTAL_PRICE -= item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessMaterialBefore(List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial)
        {
            try
            {
                foreach (var item in hisImpMestMaterial)
                {
                    if (!dicMaterialType.ContainsKey(item.MATERIAL_TYPE_ID))
                    {
                        dicMaterialType[item.MATERIAL_TYPE_ID] = new Mrs00085RDO();
                        dicMaterialType[item.MATERIAL_TYPE_ID].MATERIAL_TYPE_ID = item.MATERIAL_TYPE_ID;
                        dicMaterialType[item.MATERIAL_TYPE_ID].MATERIAL_TYPE_CODE = item.MATERIAL_TYPE_CODE;
                        dicMaterialType[item.MATERIAL_TYPE_ID].MATERIAL_TYPE_NAME = item.MATERIAL_TYPE_NAME;
                        V_HIS_MATERIAL_TYPE medicineType = listMaterialType.FirstOrDefault(o => item.MATERIAL_TYPE_ID == o.ID) ?? new V_HIS_MATERIAL_TYPE();
                        if (medicineType != null)
                        {
                            dicMaterialType[item.MATERIAL_TYPE_ID].NATIONAL_NAME = medicineType.NATIONAL_NAME;
                            dicMaterialType[item.MATERIAL_TYPE_ID].MANUFACTURER_NAME = medicineType.MANUFACTURER_NAME;
                            //dicMaterialType[item.MATERIAL_TYPE_ID].ACTIVE_INGR_BHYT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME;
                            dicMaterialType[item.MATERIAL_TYPE_ID].RECORDING_TRANSACTION = medicineType.RECORDING_TRANSACTION;
                            dicMaterialType[item.MATERIAL_TYPE_ID].SERVICE_UNIT_NAME = medicineType.SERVICE_UNIT_NAME;
                        }
                        dicMaterialType[item.MATERIAL_TYPE_ID].BEGIN_AMOUNT = item.AMOUNT;
                        dicMaterialType[item.MATERIAL_TYPE_ID].END_AMOUNT = item.AMOUNT;
                        dicMaterialType[item.MATERIAL_TYPE_ID].BEGIN_TOTAL_PRICE += item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                        dicMaterialType[item.MATERIAL_TYPE_ID].END_TOTAL_PRICE += item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                    }
                    else
                    {
                        dicMaterialType[item.MATERIAL_TYPE_ID].BEGIN_AMOUNT += item.AMOUNT;
                        dicMaterialType[item.MATERIAL_TYPE_ID].END_AMOUNT += item.AMOUNT;
                        dicMaterialType[item.MATERIAL_TYPE_ID].BEGIN_TOTAL_PRICE += item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                        dicMaterialType[item.MATERIAL_TYPE_ID].END_TOTAL_PRICE += item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessMaterialBefore(List<V_HIS_MEST_PERIOD_MATE> hisMestPeriodMate)
        {
            try
            {
                foreach (var item in hisMestPeriodMate)
                {
                    if (!dicMaterialType.ContainsKey(item.MATERIAL_TYPE_ID))
                    {
                        dicMaterialType[item.MATERIAL_TYPE_ID] = new Mrs00085RDO();
                        dicMaterialType[item.MATERIAL_TYPE_ID].MATERIAL_TYPE_ID = item.MATERIAL_TYPE_ID;
                        dicMaterialType[item.MATERIAL_TYPE_ID].MATERIAL_TYPE_CODE = item.MATERIAL_TYPE_CODE;
                        dicMaterialType[item.MATERIAL_TYPE_ID].MATERIAL_TYPE_NAME = item.MATERIAL_TYPE_NAME;
                        dicMaterialType[item.MATERIAL_TYPE_ID].SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        V_HIS_MATERIAL_TYPE medicineType = listMaterialType.FirstOrDefault(o => item.MATERIAL_TYPE_ID == o.ID) ?? new V_HIS_MATERIAL_TYPE();
                        if (medicineType != null)
                        {
                            dicMaterialType[item.MATERIAL_TYPE_ID].NATIONAL_NAME = medicineType.NATIONAL_NAME;
                            dicMaterialType[item.MATERIAL_TYPE_ID].MANUFACTURER_NAME = medicineType.MANUFACTURER_NAME;
                            //dicMaterialType[item.MATERIAL_TYPE_ID].ACTIVE_INGR_BHYT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME;
                            dicMaterialType[item.MATERIAL_TYPE_ID].RECORDING_TRANSACTION = medicineType.RECORDING_TRANSACTION;
                            dicMaterialType[item.MATERIAL_TYPE_ID].SERVICE_UNIT_NAME = medicineType.SERVICE_UNIT_NAME;
                        }
                        dicMaterialType[item.MATERIAL_TYPE_ID].BEGIN_AMOUNT = item.AMOUNT;
                        dicMaterialType[item.MATERIAL_TYPE_ID].END_AMOUNT = item.AMOUNT;
                        dicMaterialType[item.MATERIAL_TYPE_ID].BEGIN_TOTAL_PRICE += item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                        dicMaterialType[item.MATERIAL_TYPE_ID].END_TOTAL_PRICE += item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                    }
                    else
                    {
                        dicMaterialType[item.MATERIAL_TYPE_ID].BEGIN_AMOUNT += item.AMOUNT;
                        dicMaterialType[item.MATERIAL_TYPE_ID].END_AMOUNT += item.AMOUNT;
                        dicMaterialType[item.MATERIAL_TYPE_ID].BEGIN_TOTAL_PRICE += item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                        dicMaterialType[item.MATERIAL_TYPE_ID].END_TOTAL_PRICE += item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //Tính số lượng đầu kỳ không có kỳ dữ liệu gần nhất
        private void ProcessBeinAmountMaterialNotMateStockPriod(CommonParam paramGet, long mediStockId)
        {
            try
            {
                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                impMateFilter.IMP_TIME_TO = ((Mrs00085Filter)this.reportFilter).TIME_FROM - 1;
                impMateFilter.MEDI_STOCK_ID = mediStockId;
                impMateFilter.MATERIAL_TYPE_ID = ((Mrs00085Filter)this.reportFilter).MATERIAL_TYPE_ID;
                impMateFilter.MATERIAL_TYPE_IDs = ((Mrs00085Filter)this.reportFilter).MATERIAL_TYPE_IDs;
                impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new HisImpMestMaterialManager().GetView(impMateFilter);

                ProcessMaterialBefore(hisImpMestMaterial);
                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                expMateFilter.EXP_TIME_TO = ((Mrs00085Filter)this.reportFilter).TIME_FROM - 1;
                expMateFilter.MEDI_STOCK_ID = mediStockId;
                expMateFilter.MATERIAL_TYPE_ID = ((Mrs00085Filter)this.reportFilter).MATERIAL_TYPE_ID;
                expMateFilter.MATERIAL_TYPE_IDs = ((Mrs00085Filter)this.reportFilter).MATERIAL_TYPE_IDs;
                expMateFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new HisExpMestMaterialManager().GetView(expMateFilter);
                ProcessMaterialBefore(hisExpMestMaterial);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
