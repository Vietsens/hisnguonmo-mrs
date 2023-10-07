using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisImpMestType;
using MOS.MANAGER.HisExpMestType;
using MOS.MANAGER.HisMedicineType;

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
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMestPeriodMety;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisMestPeriodMedi;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceReq;

namespace MRS.Processor.Mrs00067
{
    public class Mrs00067Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam();
        Dictionary<long, Mrs00067RDO> dicMedicineType = new Dictionary<long, Mrs00067RDO>();
        HIS_DEPARTMENT department = new HIS_DEPARTMENT();
        HIS_MEDI_STOCK mediStock = new HIS_MEDI_STOCK();
        private string a = "";
        Mrs00067Filter filter = null;

        List<V_HIS_MEDICINE_TYPE> listMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        List<HIS_SERE_SERV> listSereServ = new List<HIS_SERE_SERV>();

        List<Mrs00067RDO> ListRdo = new List<Mrs00067RDO>();

        public Mrs00067Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            a = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00067Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                filter = (Mrs00067Filter)this.reportFilter;

                List<HIS_EXP_MEST_TYPE> expMestTypes = new MOS.MANAGER.HisExpMestType.HisExpMestTypeManager().Get(new HisExpMestTypeFilterQuery());
                List<HIS_IMP_MEST_TYPE> impMestTypes = new MOS.MANAGER.HisImpMestType.HisImpMestTypeManager().Get(new HisImpMestTypeFilterQuery());
                listMedicineType = new HisMedicineTypeManager().GetView(new HisMedicineTypeViewFilterQuery());
                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                


                impMediFilter.IMP_TIME_FROM = filter.TIME_FROM;
                impMediFilter.IMP_TIME_TO = filter.TIME_TO;
                impMediFilter.MEDI_STOCK_ID = filter.MEDI_STOCK_ID;
                impMediFilter.MEDI_STOCK_IDs = filter.MEDI_STOCK_IDs;

                if (filter.MEDICINE_TYPE_CODEs != null)
                {

                    filter.MEDICINE_TYPE_ID = null;
                    // filter.MEDICINE_TYPE_IDs = listMedicineType.Where(o => filter.MEDICINE_TYPE_CODEs.Contains(o.MEDICINE_TYPE_CODE)).Select(a => a.ID).ToList();
                    var TYPE_ID = listMedicineType.Where(o => filter.MEDICINE_TYPE_CODEs.Contains(o.MEDICINE_TYPE_CODE)).Select(a => a.ID).ToList();
                    filter.MEDICINE_TYPE_IDs = TYPE_ID;

                }
                impMediFilter.MEDICINE_TYPE_IDs = filter.MEDICINE_TYPE_IDs;
                impMediFilter.MEDICINE_TYPE_ID = filter.MEDICINE_TYPE_ID;
                impMediFilter.MEDICINE_IDs = filter.MEDICINE_IDs;
                impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                impMediFilter.ORDER_FIELD = "IMP_TIME";
                impMediFilter.ORDER_DIRECTION = "ASC";
                List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicines = new HisImpMestMedicineManager().GetView(impMediFilter);




                List<long> impMestIds = hisImpMestMedicines.Select(o => o.IMP_MEST_ID).ToList();



                var aggrImpId = hisImpMestMedicines.Select(o => o.AGGR_IMP_MEST_ID ?? 0).ToList();
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

                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                expMediFilter.EXP_TIME_FROM = filter.TIME_FROM;
                expMediFilter.EXP_TIME_TO = filter.TIME_TO;
                expMediFilter.MEDI_STOCK_ID = filter.MEDI_STOCK_ID;
                expMediFilter.MEDI_STOCK_IDs = filter.MEDI_STOCK_IDs;





                if (filter.MEDICINE_TYPE_CODEs != null)
                {

                    filter.MEDICINE_TYPE_ID = null;
                    var TYPE_ID = listMedicineType.Where(o => filter.MEDICINE_TYPE_CODEs.Contains(o.MEDICINE_TYPE_CODE)).Select(a => a.ID).ToList();
                    filter.MEDICINE_TYPE_IDs = TYPE_ID;

                }

                expMediFilter.MEDICINE_TYPE_ID = filter.MEDICINE_TYPE_ID;


                expMediFilter.MEDICINE_TYPE_IDs = filter.MEDICINE_TYPE_IDs;
                expMediFilter.MEDICINE_IDs = filter.MEDICINE_IDs;
                expMediFilter.IS_EXPORT = true;
                expMediFilter.ORDER_FIELD = "EXP_TIME";
                expMediFilter.ORDER_DIRECTION = "ASC";
                List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new HisExpMestMedicineManager().GetView(expMediFilter);


                List<long> hisExpMestMedicineIds = hisExpMestMedicine.Select(o => o.EXP_MEST_ID ?? 0).ToList();
                
                var aggrExpMest = hisExpMestMedicine.Select(o => o.AGGR_EXP_MEST_ID ?? 0).ToList();
                if (IsNotNullOrEmpty(aggrExpMest))
                    hisExpMestMedicineIds.AddRange(aggrExpMest);
                
                

              


                List<V_HIS_EXP_MEST> hisExpMest = new List<V_HIS_EXP_MEST>();
                if (IsNotNullOrEmpty(hisExpMestMedicineIds))
                {
                    hisExpMestMedicineIds = hisExpMestMedicineIds.Distinct().ToList();
                    var skip = 0;
                    while (hisExpMestMedicineIds.Count - skip > 0)
                    {
                        var listIDs = hisExpMestMedicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
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

                if (IsNotNullOrEmpty(hisImpMestMedicines))
                {
                    var listSub = (from r in hisImpMestMedicines select new Mrs00067RDO(r, ImpMest, sourceMest, impMestTypes)).ToList();
                    var groupByCode = listSub.GroupBy(o => new { o.PACKAGE_NUMBER, o.MEDICINE_TYPE_ID, o.IMP_MEST_CODE, o.EXECUTE_DATE_STR }).ToList();

                    foreach (var group in groupByCode)
                    {
                        List<Mrs00067RDO> p = group.ToList<Mrs00067RDO>();
                        Mrs00067RDO rdo = new Mrs00067RDO()
                        {
                            AGGR_ID = p.First().AGGR_ID,
                            MEDICINE_TYPE_ID = p.First().MEDICINE_TYPE_ID,
                            MEDICINE_ID = p.First().MEDICINE_ID,
                            MEDICINE_TYPE_CODE = p.First().MEDICINE_TYPE_CODE,
                            MEDICINE_TYPE_NAME = p.First().MEDICINE_TYPE_NAME,
                            CONCENTRA = p.First().CONCENTRA,
                            EXP_MEST_CODE = p.First().EXP_MEST_CODE,
                            DESCRIPTION = p.First().DESCRIPTION,
                            EXECUTE_DATE_STR = p.First().EXECUTE_DATE_STR,
                            EXECUTE_TIME = p.First().EXECUTE_TIME,
                            DOCUMENT_NUMBER = p.First().DOCUMENT_NUMBER,
                            IMP_TIME_STR = p.First().IMP_TIME_STR,
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
                            MOBA_EXP_MEST_ID = p.First().MOBA_EXP_MEST_ID,
                            CHMS_TYPE_ID = p.First().CHMS_TYPE_ID,
                            IMP_VAT_RATIO = p.First().IMP_VAT_RATIO,
                            TDL_PATIENT_CODE = p.First().TDL_PATIENT_CODE,
                            DESCRIPTION_DETAIL = p.First().DESCRIPTION_DETAIL,
                            EXP_MEST_REASON_NAME = p.First().EXP_MEST_REASON_NAME,
                            RECEIVING_PLACE = p.First().RECEIVING_PLACE,
                            //  TDL_PATIENT_NAME = p.First().TDL_PATIENT_NAME
                        };
                        ListRdo.Add(rdo);
                        if (!dicMedicineType.ContainsKey(rdo.MEDICINE_TYPE_ID))
                        {
                            dicMedicineType[rdo.MEDICINE_TYPE_ID] = new Mrs00067RDO();
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].MEDICINE_TYPE_ID = rdo.MEDICINE_TYPE_ID;
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].MEDICINE_TYPE_CODE = rdo.MEDICINE_TYPE_CODE;
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].MEDICINE_TYPE_NAME = rdo.MEDICINE_TYPE_NAME;
                            V_HIS_MEDICINE_TYPE medicineType = listMedicineType.FirstOrDefault(o => rdo.MEDICINE_TYPE_ID == o.ID) ?? new V_HIS_MEDICINE_TYPE();
                            if (medicineType != null)
                            {
                                dicMedicineType[rdo.MEDICINE_TYPE_ID].NATIONAL_NAME = medicineType.NATIONAL_NAME;
                                dicMedicineType[rdo.MEDICINE_TYPE_ID].MANUFACTURER_NAME = medicineType.MANUFACTURER_NAME;
                                dicMedicineType[rdo.MEDICINE_TYPE_ID].CONCENTRA = medicineType.CONCENTRA;
                                dicMedicineType[rdo.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME;
                                dicMedicineType[rdo.MEDICINE_TYPE_ID].RECORDING_TRANSACTION = medicineType.RECORDING_TRANSACTION;
                                dicMedicineType[rdo.MEDICINE_TYPE_ID].SERVICE_UNIT_NAME = medicineType.SERVICE_UNIT_NAME;
                            }
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].END_AMOUNT = rdo.IMP_AMOUNT ?? 0;
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].END_TOTAL_PRICE = (rdo.IMP_AMOUNT ?? 0) * rdo.IMP_PRICE * (1 + rdo.IMP_VAT_RATIO);
                        }
                        else
                        {
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].END_AMOUNT += rdo.IMP_AMOUNT ?? 0;
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].END_TOTAL_PRICE += (rdo.IMP_AMOUNT ?? 0) * rdo.IMP_PRICE * (1 + rdo.IMP_VAT_RATIO);
                        }
                    }
                }

                if (IsNotNullOrEmpty(hisExpMestMedicine))
                {
                    var listSub = (from r in hisExpMestMedicine select new Mrs00067RDO(r, hisExpMest, destMest, expMestTypes)).ToList();
                    var groupByCode = listSub.GroupBy(o => new { o.PACKAGE_NUMBER, o.MEDICINE_TYPE_ID, o.EXP_MEST_CODE, o.EXECUTE_DATE_STR }).ToList();

                    foreach (var group in groupByCode)
                    {
                        List<Mrs00067RDO> p = group.ToList<Mrs00067RDO>();
                        Mrs00067RDO rdo = new Mrs00067RDO()
                        {
                            AGGR_ID = p.First().AGGR_ID,
                            MEDICINE_ID = p.First().MEDICINE_ID,
                            MEDICINE_TYPE_ID = p.First().MEDICINE_TYPE_ID,
                            TDL_PATIENT_TYPE_CODE = p.First().TDL_PATIENT_TYPE_CODE,
                            TDL_PATIENT_TYPE_NAME = p.First().TDL_PATIENT_TYPE_NAME,
                            MEDICINE_TYPE_CODE = p.First().MEDICINE_TYPE_CODE,
                            MEDICINE_TYPE_NAME = p.First().MEDICINE_TYPE_NAME,
                            CONCENTRA = p.First().CONCENTRA,
                            EXP_MEST_CODE = p.First().EXP_MEST_CODE,
                            DESCRIPTION = p.First().DESCRIPTION,
                            EXECUTE_DATE_STR = p.First().EXECUTE_DATE_STR,
                            EXECUTE_TIME = p.First().EXECUTE_TIME,
                            IMP_MEST_CODE = p.First().IMP_MEST_CODE,
                            DOCUMENT_NUMBER = p.First().DOCUMENT_NUMBER,
                            IMP_TIME_STR = p.First().IMP_TIME_STR,
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
                            CHMS_TYPE_ID = p.First().CHMS_TYPE_ID,
                            IMP_VAT_RATIO = p.First().IMP_VAT_RATIO,
                            TDL_PATIENT_CODE = p.First().TDL_PATIENT_CODE,
                            // TDL_PATIENT_NAME = p.First().TDL_PATIENT_NAME
                            SERVICE_REQ_ID = p.First().SERVICE_REQ_ID,
                            HEIN_MEDI_ORG_CODE = p.First().HEIN_MEDI_ORG_CODE,
                            MEDICINE_GROUP_CODE = p.First().MEDICINE_GROUP_CODE,
                            MEDICINE_GROUP_NAME = p.First().MEDICINE_GROUP_NAME,
                            DESCRIPTION_DETAIL = p.First().DESCRIPTION_DETAIL,
                            EXP_MEST_REASON_NAME = p.First().EXP_MEST_REASON_NAME,
                            RECEIVING_PLACE = p.First().RECEIVING_PLACE,
                            PRESCRIPTION_ID = p.First().PRESCRIPTION_ID,
                        };
                        ListRdo.Add(rdo);
                        if (!dicMedicineType.ContainsKey(rdo.MEDICINE_TYPE_ID))
                        {
                            dicMedicineType[rdo.MEDICINE_TYPE_ID] = new Mrs00067RDO();
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].MEDICINE_TYPE_ID = rdo.MEDICINE_TYPE_ID;
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].MEDICINE_TYPE_CODE = rdo.MEDICINE_TYPE_CODE;
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].MEDICINE_TYPE_NAME = rdo.MEDICINE_TYPE_NAME;
                            V_HIS_MEDICINE_TYPE medicineType = listMedicineType.FirstOrDefault(o => rdo.MEDICINE_TYPE_ID == o.ID) ?? new V_HIS_MEDICINE_TYPE();
                            if (medicineType != null)
                            {
                                dicMedicineType[rdo.MEDICINE_TYPE_ID].NATIONAL_NAME = medicineType.NATIONAL_NAME;
                                dicMedicineType[rdo.MEDICINE_TYPE_ID].MANUFACTURER_NAME = medicineType.MANUFACTURER_NAME;
                                dicMedicineType[rdo.MEDICINE_TYPE_ID].CONCENTRA = medicineType.CONCENTRA;
                                dicMedicineType[rdo.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME;
                                dicMedicineType[rdo.MEDICINE_TYPE_ID].RECORDING_TRANSACTION = medicineType.RECORDING_TRANSACTION;
                                dicMedicineType[rdo.MEDICINE_TYPE_ID].SERVICE_UNIT_NAME = medicineType.SERVICE_UNIT_NAME;
                            }
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].END_AMOUNT = -rdo.EXP_AMOUNT ?? 0;
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].END_TOTAL_PRICE -= (rdo.EXP_AMOUNT ?? 0) * rdo.IMP_PRICE * (1 + rdo.IMP_VAT_RATIO);
                        }
                        else
                        {
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].END_AMOUNT -= (rdo.EXP_AMOUNT ?? 0);
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].END_TOTAL_PRICE -= (rdo.EXP_AMOUNT ?? 0) * rdo.IMP_PRICE * (1 + rdo.IMP_VAT_RATIO);
                        }

                    }
                }

                if (!IsNotNullOrEmpty(hisImpMestMedicines) && !IsNotNullOrEmpty(hisExpMestMedicine) && IsNotNullOrEmpty(listMedicineType) && filter.IS_NOT_SHOW_ALL != true)
                {
                    var listSub = (from r in listMedicineType select new Mrs00067RDO(r, filter)).ToList();
                    var groupByCode = listSub.GroupBy(o => new { o.MEDICINE_TYPE_ID }).ToList();

                    foreach (var group in groupByCode)
                    {
                        List<Mrs00067RDO> p = group.ToList<Mrs00067RDO>();
                        Mrs00067RDO rdo = new Mrs00067RDO()
                        {
                            MEDICINE_ID = p.First().MEDICINE_ID,
                            MEDICINE_TYPE_ID = p.First().MEDICINE_TYPE_ID,
                            MEDICINE_TYPE_CODE = p.First().MEDICINE_TYPE_CODE,
                            MEDICINE_TYPE_NAME = p.First().MEDICINE_TYPE_NAME,
                            MANUFACTURER_NAME = p.First().MANUFACTURER_NAME,
                            CONCENTRA = p.First().CONCENTRA,
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
                        if (!dicMedicineType.ContainsKey(rdo.MEDICINE_TYPE_ID))
                        {
                            dicMedicineType[rdo.MEDICINE_TYPE_ID] = new Mrs00067RDO();
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].MEDICINE_TYPE_ID = rdo.MEDICINE_TYPE_ID;
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].MEDICINE_TYPE_CODE = rdo.MEDICINE_TYPE_CODE;
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].MEDICINE_TYPE_NAME = rdo.MEDICINE_TYPE_NAME;
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].NATIONAL_NAME = rdo.NATIONAL_NAME;
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].MANUFACTURER_NAME = rdo.MANUFACTURER_NAME;
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].CONCENTRA = rdo.CONCENTRA;
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_NAME = rdo.ACTIVE_INGR_BHYT_NAME;
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].RECORDING_TRANSACTION = rdo.RECORDING_TRANSACTION;
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].SERVICE_UNIT_NAME = rdo.SERVICE_UNIT_NAME;
                            //V_HIS_MEDICINE_TYPE medicineType = listMedicineType.FirstOrDefault(o => rdo.MEDICINE_TYPE_ID == o.ID) ?? new V_HIS_MEDICINE_TYPE();
                            //if (medicineType != null)
                            //{
                            //    dicMedicineType[rdo.MEDICINE_TYPE_ID].NATIONAL_NAME = medicineType.NATIONAL_NAME;
                            //    dicMedicineType[rdo.MEDICINE_TYPE_ID].MANUFACTURER_NAME = medicineType.MANUFACTURER_NAME;
                            //    dicMedicineType[rdo.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME;
                            //    dicMedicineType[rdo.MEDICINE_TYPE_ID].RECORDING_TRANSACTION = medicineType.RECORDING_TRANSACTION;
                            //    dicMedicineType[rdo.MEDICINE_TYPE_ID].SERVICE_UNIT_NAME = medicineType.SERVICE_UNIT_NAME;
                            //}
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].END_AMOUNT = -rdo.EXP_AMOUNT ?? 0;
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].END_TOTAL_PRICE -= (rdo.EXP_AMOUNT ?? 0) * rdo.IMP_PRICE * (1 + rdo.IMP_VAT_RATIO);
                        }
                        else
                        {
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].END_AMOUNT -= (rdo.EXP_AMOUNT ?? 0);
                            dicMedicineType[rdo.MEDICINE_TYPE_ID].END_TOTAL_PRICE -= (rdo.EXP_AMOUNT ?? 0) * rdo.IMP_PRICE * (1 + rdo.IMP_VAT_RATIO);
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
            if (((Mrs00067Filter)this.reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00067Filter)this.reportFilter).TIME_FROM));
            }
            if (((Mrs00067Filter)this.reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00067Filter)this.reportFilter).TIME_TO));
            }
            var medicineType = dicMedicineType.Values.FirstOrDefault() ?? new Mrs00067RDO();
            dicSingleTag.Add("BEGIN_AMOUNT", medicineType.BEGIN_AMOUNT);
            dicSingleTag.Add("END_AMOUNT", medicineType.END_AMOUNT);
            dicSingleTag.Add("BEGIN_TOTAL_PRICE", medicineType.BEGIN_TOTAL_PRICE);
            dicSingleTag.Add("END_TOTAL_PRICE", medicineType.END_TOTAL_PRICE);
            dicSingleTag.Add("MEDICINE_TYPE_CODE", medicineType.MEDICINE_TYPE_CODE);
            dicSingleTag.Add("MEDICINE_TYPE_NAME", medicineType.MEDICINE_TYPE_NAME);
            dicSingleTag.Add("SERVICE_UNIT_NAME", medicineType.SERVICE_UNIT_NAME);
            dicSingleTag.Add("NATIONAL_NAME", medicineType.NATIONAL_NAME);
            dicSingleTag.Add("MANUFACTURER_NAME", medicineType.MANUFACTURER_NAME);
            dicSingleTag.Add("CONCENTRA", medicineType.CONCENTRA);
            dicSingleTag.Add("ACTIVE_INGR_BHYT_NAME", medicineType.ACTIVE_INGR_BHYT_NAME);
            dicSingleTag.Add("RECORDING_TRANSACTION", medicineType.RECORDING_TRANSACTION);

            HIS_MEDI_STOCK mediStock = new MOS.MANAGER.HisMediStock.HisMediStockManager().GetById(((Mrs00067Filter)this.reportFilter).MEDI_STOCK_ID??0);
            if (IsNotNull(mediStock))
            {
                dicSingleTag.Add("MEDI_STOCK_CODE", mediStock.MEDI_STOCK_CODE);
                dicSingleTag.Add("MEDI_STOCK_NAME", mediStock.MEDI_STOCK_NAME);
            }
            dicSingleTag.Add("MEDI_BEGIN_AMOUNT", medicineType.BEGIN_AMOUNT);
            dicSingleTag.Add("MEDI_END_AMOUNT", medicineType.END_AMOUNT);

            objectTag.AddObjectData(store, "Report", ListRdo);

            objectTag.AddObjectData(store, "Parent", dicMedicineType.Values.Where(o=>o.BEGIN_AMOUNT>0||o.END_AMOUNT>0||ListRdo.Exists(p=>p.MEDICINE_TYPE_ID==o.MEDICINE_TYPE_ID&&(p.EXP_AMOUNT>0||p.IMP_AMOUNT>0))).ToList());

            objectTag.AddRelationship(store, "Parent", "Report", "MEDICINE_TYPE_ID", "MEDICINE_TYPE_ID");

            objectTag.AddObjectData(store, "ListMedicine", new List<Mrs00067RDO>());

            List<CHECK_AMOUNT> listCheck = new List<CHECK_AMOUNT>();
            List<CHECK_AMOUNT> listCheckData = new List<CHECK_AMOUNT>();
            if (((Mrs00067Filter)this.reportFilter).MEDICINE_TYPE_ID != null)
            {
                listCheck = new ManagerSql().GetAmountChecked(((Mrs00067Filter)this.reportFilter));

                if (listCheck != null)
                {
                    var group = listCheck.GroupBy(p => new { p.MEDICINE_TYPE_ID, p.MEDI_STOCK_ID, p.PACKAGE_NUMBER, p.EXPIRED_DATE }).ToList();
                    foreach (var item in group)
                    {
                        CHECK_AMOUNT check = new CHECK_AMOUNT();
                        List<CHECK_AMOUNT> listSub = item.ToList<CHECK_AMOUNT>();
                        check.PACKAGE_NUMBER = listSub[0].PACKAGE_NUMBER;
                        check.EXPIRED_DATE = listSub[0].EXPIRED_DATE;
                        check.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listSub[0].EXPIRED_DATE ?? 0);
                        check.END_AMOUNT = listSub.Sum(P => P.IMP_AMOUNT) - listSub.Sum(P => P.EXP_AMOUNT);
                        listCheckData.Add(check);
                    }
                }
                listCheckData = listCheckData.Where(p => p.END_AMOUNT > 0).OrderBy(p => p.PACKAGE_NUMBER).ToList();
                if (listCheckData != null)
                {
                    dicSingleTag.Add("PACKAGE_NUMBER_REMAIN", string.Join(", ", listCheckData.Where(p => p.END_AMOUNT > 0).Select(p => p.PACKAGE_NUMBER).Distinct().ToList()));
                    dicSingleTag.Add("EXPIRED_DATE_REMAIN", string.Join(", ", listCheckData.Where(p => p.END_AMOUNT > 0).Select(p => p.EXPIRED_DATE_STR).Distinct().ToList()));
                }
            }
        }

        private void ProcessBeginAndEndAmount()
        {
            try
            {
                ListRdo = ListRdo.OrderBy(o => o.MEDICINE_TYPE_NAME).ThenBy(o => o.EXECUTE_TIME).ToList();

                foreach (var item in dicMedicineType)
                {
                    decimal previousEndAmount = 0;
                    decimal previousEndTotalPrice = 0;
                    decimal BeginAmount = 0;
                    decimal BeginTotalPrice = 0;
                    var listSub = ListRdo.Where(o => o.MEDICINE_TYPE_ID == item.Key).ToList();
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
                ListRdo = ListRdo.OrderBy(o => o.MEDICINE_TYPE_NAME).ThenBy(o => o.EXECUTE_TIME).ToList();


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
                periodFilter.TO_TIME_TO = ((Mrs00067Filter)this.reportFilter).TIME_FROM;
                periodFilter.MEDI_STOCK_ID = mediStockId;
                List<V_HIS_MEDI_STOCK_PERIOD> hisMediStockPeriod = new HisMediStockPeriodManager(paramGet).GetView(periodFilter);
                if (!paramGet.HasException)
                {
                    if (IsNotNullOrEmpty(hisMediStockPeriod))
                    {
                        //Trường hợp có kỳ được chốt gần nhất
                        V_HIS_MEDI_STOCK_PERIOD neighborPeriod = hisMediStockPeriod.OrderByDescending(d => d.TO_TIME).ToList()[0];
                        if (neighborPeriod != null)
                        {
                            ProcessBeinAmountMedicineByMediStockPeriod(paramGet, neighborPeriod,mediStockId);
                        }
                    }
                    else
                    {
                        // Trường hợp không có kỳ chốt gần nhât - chưa được chốt kỳ nào
                        ProcessBeinAmountMedicineNotMediStockPriod(paramGet, mediStockId);
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
        private void ProcessBeinAmountMedicineByMediStockPeriod(CommonParam paramGet, V_HIS_MEDI_STOCK_PERIOD neighborPeriod,long mediStockId)
        {
            try
            {
                List<Mrs00067RDO> listrdo = new List<Mrs00067RDO>();
                HisMestPeriodMediViewFilterQuery periodMetyFilter = new HisMestPeriodMediViewFilterQuery();
                periodMetyFilter.MEDI_STOCK_PERIOD_ID = neighborPeriod.ID;
                periodMetyFilter.MEDICINE_TYPE_ID = ((Mrs00067Filter)this.reportFilter).MEDICINE_TYPE_ID;
                periodMetyFilter.MEDICINE_TYPE_IDs = ((Mrs00067Filter)this.reportFilter).MEDICINE_TYPE_IDs;
                List<V_HIS_MEST_PERIOD_MEDI> hisMestPeriodMedi = new HisMestPeriodMediManager(paramGet).GetView(periodMetyFilter) ?? new List<V_HIS_MEST_PERIOD_MEDI>();

                ProcessMedicineBefore(hisMestPeriodMedi);

                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                impMediFilter.IMP_TIME_FROM = neighborPeriod.TO_TIME + 1;
                impMediFilter.IMP_TIME_TO = ((Mrs00067Filter)this.reportFilter).TIME_FROM - 1;
                impMediFilter.MEDI_STOCK_ID = mediStockId;
                impMediFilter.MEDICINE_TYPE_ID = ((Mrs00067Filter)this.reportFilter).MEDICINE_TYPE_ID;
                impMediFilter.MEDICINE_TYPE_IDs = ((Mrs00067Filter)this.reportFilter).MEDICINE_TYPE_IDs;
                impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new HisImpMestMedicineManager(paramGet).GetView(impMediFilter) ?? new List<V_HIS_IMP_MEST_MEDICINE>();


                ProcessMedicineBefore(hisImpMestMedicine);
                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                expMediFilter.EXP_TIME_FROM = neighborPeriod.TO_TIME + 1;
                expMediFilter.EXP_TIME_TO = ((Mrs00067Filter)this.reportFilter).TIME_FROM - 1;
                expMediFilter.MEDI_STOCK_ID = mediStockId;
                expMediFilter.MEDICINE_TYPE_ID = ((Mrs00067Filter)this.reportFilter).MEDICINE_TYPE_ID;
                expMediFilter.MEDICINE_TYPE_IDs = ((Mrs00067Filter)this.reportFilter).MEDICINE_TYPE_IDs;
                expMediFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new HisExpMestMedicineManager(paramGet).GetView(expMediFilter) ?? new List<V_HIS_EXP_MEST_MEDICINE>();
                ProcessMedicineBefore(hisExpMestMedicine);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessMedicineBefore(List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine)
        {
            try
            {
                foreach (var item in hisExpMestMedicine)
                {
                    if (!dicMedicineType.ContainsKey(item.MEDICINE_TYPE_ID))
                    {
                        dicMedicineType[item.MEDICINE_TYPE_ID] = new Mrs00067RDO();
                        dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_TYPE_ID = item.MEDICINE_TYPE_ID;
                        dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_TYPE_CODE = item.MEDICINE_TYPE_CODE;
                        dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_TYPE_NAME = item.MEDICINE_TYPE_NAME;
                        V_HIS_MEDICINE_TYPE medicineType = listMedicineType.FirstOrDefault(o => item.MEDICINE_TYPE_ID == o.ID) ?? new V_HIS_MEDICINE_TYPE();
                        if (medicineType != null)
                        {
                            dicMedicineType[item.MEDICINE_TYPE_ID].NATIONAL_NAME = medicineType.NATIONAL_NAME;
                            dicMedicineType[item.MEDICINE_TYPE_ID].MANUFACTURER_NAME = medicineType.MANUFACTURER_NAME;
                            dicMedicineType[item.MEDICINE_TYPE_ID].CONCENTRA = medicineType.CONCENTRA;
                            dicMedicineType[item.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME;
                            dicMedicineType[item.MEDICINE_TYPE_ID].RECORDING_TRANSACTION = medicineType.RECORDING_TRANSACTION;
                            dicMedicineType[item.MEDICINE_TYPE_ID].SERVICE_UNIT_NAME = medicineType.SERVICE_UNIT_NAME;
                        }
                        dicMedicineType[item.MEDICINE_TYPE_ID].BEGIN_AMOUNT = -item.AMOUNT;
                        dicMedicineType[item.MEDICINE_TYPE_ID].END_AMOUNT = -item.AMOUNT;
                        dicMedicineType[item.MEDICINE_TYPE_ID].BEGIN_TOTAL_PRICE -= item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                        dicMedicineType[item.MEDICINE_TYPE_ID].END_TOTAL_PRICE -= item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                    }
                    else
                    {
                        dicMedicineType[item.MEDICINE_TYPE_ID].BEGIN_AMOUNT -= item.AMOUNT;
                        dicMedicineType[item.MEDICINE_TYPE_ID].END_AMOUNT -= item.AMOUNT;
                        dicMedicineType[item.MEDICINE_TYPE_ID].BEGIN_TOTAL_PRICE -= item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                        dicMedicineType[item.MEDICINE_TYPE_ID].END_TOTAL_PRICE -= item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessMedicineBefore(List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine)
        {
            try
            {
                foreach (var item in hisImpMestMedicine)
                {
                    if (!dicMedicineType.ContainsKey(item.MEDICINE_TYPE_ID))
                    {
                        dicMedicineType[item.MEDICINE_TYPE_ID] = new Mrs00067RDO();
                        dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_TYPE_ID = item.MEDICINE_TYPE_ID;
                        dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_TYPE_CODE = item.MEDICINE_TYPE_CODE;
                        dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_TYPE_NAME = item.MEDICINE_TYPE_NAME;
                        V_HIS_MEDICINE_TYPE medicineType = listMedicineType.FirstOrDefault(o => item.MEDICINE_TYPE_ID == o.ID) ?? new V_HIS_MEDICINE_TYPE();
                        if (medicineType != null)
                        {
                            dicMedicineType[item.MEDICINE_TYPE_ID].NATIONAL_NAME = medicineType.NATIONAL_NAME;
                            dicMedicineType[item.MEDICINE_TYPE_ID].MANUFACTURER_NAME = medicineType.MANUFACTURER_NAME;
                            dicMedicineType[item.MEDICINE_TYPE_ID].CONCENTRA = medicineType.CONCENTRA;
                            dicMedicineType[item.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME;
                            dicMedicineType[item.MEDICINE_TYPE_ID].RECORDING_TRANSACTION = medicineType.RECORDING_TRANSACTION;
                            dicMedicineType[item.MEDICINE_TYPE_ID].SERVICE_UNIT_NAME = medicineType.SERVICE_UNIT_NAME;
                        }
                        dicMedicineType[item.MEDICINE_TYPE_ID].BEGIN_AMOUNT = item.AMOUNT;
                        dicMedicineType[item.MEDICINE_TYPE_ID].END_AMOUNT = item.AMOUNT;
                        dicMedicineType[item.MEDICINE_TYPE_ID].BEGIN_TOTAL_PRICE += item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                        dicMedicineType[item.MEDICINE_TYPE_ID].END_TOTAL_PRICE += item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                    }
                    else
                    {
                        dicMedicineType[item.MEDICINE_TYPE_ID].BEGIN_AMOUNT += item.AMOUNT;
                        dicMedicineType[item.MEDICINE_TYPE_ID].END_AMOUNT += item.AMOUNT;
                        dicMedicineType[item.MEDICINE_TYPE_ID].BEGIN_TOTAL_PRICE += item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                        dicMedicineType[item.MEDICINE_TYPE_ID].END_TOTAL_PRICE += item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessMedicineBefore(List<V_HIS_MEST_PERIOD_MEDI> hisMestPeriodMedi)
        {
            try
            {
                foreach (var item in hisMestPeriodMedi)
                {
                    if (!dicMedicineType.ContainsKey(item.MEDICINE_TYPE_ID))
                    {
                        dicMedicineType[item.MEDICINE_TYPE_ID] = new Mrs00067RDO();
                        dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_TYPE_ID = item.MEDICINE_TYPE_ID;
                        dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_TYPE_CODE = item.MEDICINE_TYPE_CODE;
                        dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_TYPE_NAME = item.MEDICINE_TYPE_NAME;
                        dicMedicineType[item.MEDICINE_TYPE_ID].SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        V_HIS_MEDICINE_TYPE medicineType = listMedicineType.FirstOrDefault(o => item.MEDICINE_TYPE_ID == o.ID) ?? new V_HIS_MEDICINE_TYPE();
                        if (medicineType != null)
                        {
                            dicMedicineType[item.MEDICINE_TYPE_ID].NATIONAL_NAME = medicineType.NATIONAL_NAME;
                            dicMedicineType[item.MEDICINE_TYPE_ID].MANUFACTURER_NAME = medicineType.MANUFACTURER_NAME;
                            dicMedicineType[item.MEDICINE_TYPE_ID].CONCENTRA = medicineType.CONCENTRA;
                            dicMedicineType[item.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME;
                            dicMedicineType[item.MEDICINE_TYPE_ID].RECORDING_TRANSACTION = medicineType.RECORDING_TRANSACTION;
                            dicMedicineType[item.MEDICINE_TYPE_ID].SERVICE_UNIT_NAME = medicineType.SERVICE_UNIT_NAME;
                        }
                        dicMedicineType[item.MEDICINE_TYPE_ID].BEGIN_AMOUNT = item.AMOUNT;
                        dicMedicineType[item.MEDICINE_TYPE_ID].END_AMOUNT = item.AMOUNT;
                        dicMedicineType[item.MEDICINE_TYPE_ID].BEGIN_TOTAL_PRICE += item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                        dicMedicineType[item.MEDICINE_TYPE_ID].END_TOTAL_PRICE += item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                    }
                    else
                    {
                        dicMedicineType[item.MEDICINE_TYPE_ID].BEGIN_AMOUNT += item.AMOUNT;
                        dicMedicineType[item.MEDICINE_TYPE_ID].END_AMOUNT += item.AMOUNT;
                        dicMedicineType[item.MEDICINE_TYPE_ID].BEGIN_TOTAL_PRICE += item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                        dicMedicineType[item.MEDICINE_TYPE_ID].END_TOTAL_PRICE += item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //Tính số lượng đầu kỳ không có kỳ dữ liệu gần nhất
        private void ProcessBeinAmountMedicineNotMediStockPriod(CommonParam paramGet,long mediStockId)
        {
            try
            {
                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                impMediFilter.IMP_TIME_TO = ((Mrs00067Filter)this.reportFilter).TIME_FROM - 1;
                impMediFilter.MEDI_STOCK_ID = mediStockId;
                impMediFilter.MEDICINE_TYPE_ID = ((Mrs00067Filter)this.reportFilter).MEDICINE_TYPE_ID;
                impMediFilter.MEDICINE_TYPE_IDs = ((Mrs00067Filter)this.reportFilter).MEDICINE_TYPE_IDs;
                impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new HisImpMestMedicineManager().GetView(impMediFilter);

                ProcessMedicineBefore(hisImpMestMedicine);
                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                expMediFilter.EXP_TIME_TO = ((Mrs00067Filter)this.reportFilter).TIME_FROM - 1;
                expMediFilter.MEDI_STOCK_ID = mediStockId;
                expMediFilter.MEDICINE_TYPE_ID = ((Mrs00067Filter)this.reportFilter).MEDICINE_TYPE_ID;
                expMediFilter.MEDICINE_TYPE_IDs = ((Mrs00067Filter)this.reportFilter).MEDICINE_TYPE_IDs;
                expMediFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new HisExpMestMedicineManager().GetView(expMediFilter);
                ProcessMedicineBefore(hisExpMestMedicine);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
