using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisImpMest;
using ACS.Filter;
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
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMediStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACS.MANAGER.Manager;
using ACS.MANAGER.Core.AcsUser.Get;
using ACS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTransaction;

namespace MRS.Processor.Mrs00304
{
    class Mrs00304Processor : AbstractProcessor
    {
        List<Mrs00304RDO> listRdo = new List<Mrs00304RDO>();
        List<Mrs00304RDO> ListExpRdo = new List<Mrs00304RDO>();
        List<V_HIS_EXP_MEST_MEDICINE> listMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> listMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<V_HIS_EXP_MEST> listSaleExpMest = new List<V_HIS_EXP_MEST>();
        List<HIS_EXP_MEST> listPrescription = new List<HIS_EXP_MEST>();
        List<HIS_SERVICE_REQ> listServiceReq = new List<HIS_SERVICE_REQ>();

        List<HIS_TRANSACTION> ListTransaction = new List<HIS_TRANSACTION>();

        CommonParam paramGet = new CommonParam();
        public Mrs00304Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00304Filter);
        }

        protected override bool GetData()///
        {
            var filter = ((Mrs00304Filter)reportFilter);
            bool result = true;
            try
            {
                HisExpMestMedicineViewFilterQuery medicinefilter = new HisExpMestMedicineViewFilterQuery();
                medicinefilter.EXP_TIME_FROM = filter.TIME_FROM;
                medicinefilter.EXP_TIME_TO = filter.TIME_TO;
                //medicinefilter.CREATOR = filter.CREATOR_LOGINNAME;
                medicinefilter.EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN;
                medicinefilter.MEDI_STOCK_IDs = filter.MEDI_STOCK_BUSINESS_IDs;
                medicinefilter.IS_EXPORT = true;
                listMedicine = new HisExpMestMedicineManager(paramGet).GetView(medicinefilter);
                if (IsNotNullOrEmpty(listMedicine) && !string.IsNullOrWhiteSpace(filter.LOGINNAME_SALE))
                {
                    listMedicine = listMedicine.Where(o => o.EXP_LOGINNAME == filter.LOGINNAME_SALE).ToList();
                }
                if (IsNotNullOrEmpty(listMedicine) && !string.IsNullOrWhiteSpace(filter.LOGINNAME))
                {
                    listMedicine = listMedicine.Where(o => o.EXP_LOGINNAME == filter.LOGINNAME).ToList();
                }

                HisExpMestMaterialViewFilterQuery materialfilter = new HisExpMestMaterialViewFilterQuery();
                materialfilter.EXP_TIME_FROM = filter.TIME_FROM;
                materialfilter.EXP_TIME_TO = filter.TIME_TO;
                //materialfilter.CREATOR = filter.LOGINNAME_SALE;
                materialfilter.EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN;
                materialfilter.MEDI_STOCK_IDs = filter.MEDI_STOCK_BUSINESS_IDs;
                materialfilter.IS_EXPORT = true;
                listMaterial = new HisExpMestMaterialManager(paramGet).GetView(materialfilter);
                if (IsNotNullOrEmpty(listMaterial) && !string.IsNullOrWhiteSpace(filter.LOGINNAME_SALE))
                {
                    listMaterial = listMaterial.Where(o => o.EXP_LOGINNAME == filter.LOGINNAME_SALE).ToList();
                }
                if (IsNotNullOrEmpty(listMaterial) && !string.IsNullOrWhiteSpace(filter.LOGINNAME))
                {
                    listMaterial = listMaterial.Where(o => o.EXP_LOGINNAME == filter.LOGINNAME).ToList();
                }

                List<long> lstExpMestId = new List<long>();
                if (IsNotNullOrEmpty(listMedicine))
                {
                    lstExpMestId.AddRange(listMedicine.Select(s => s.EXP_MEST_ID ?? 0).Distinct().ToList());
                }
                if (IsNotNullOrEmpty(listMaterial))
                {
                    lstExpMestId.AddRange(listMaterial.Select(s => s.EXP_MEST_ID ?? 0).Distinct().ToList());
                }

                lstExpMestId = lstExpMestId.Distinct().ToList();
                if (IsNotNullOrEmpty(lstExpMestId))
                {
                    var skip = 0;
                    while (lstExpMestId.Count - skip > 0)
                    {
                        var listIDs = lstExpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisExpMestViewFilterQuery expMestFilter = new HisExpMestViewFilterQuery();
                        expMestFilter.IDs = listIDs;
                        expMestFilter.CREATOR = filter.CREATOR_LOGINNAME;
                        
                        var expMest = new MOS.MANAGER.HisExpMest.HisExpMestManager(paramGet).GetView(expMestFilter);
                        if (!string.IsNullOrEmpty(filter.CASHIER_LOGINNAME))
                        {
                            expMest = expMest.Where(P => filter.CASHIER_LOGINNAME.Contains(P.CASHIER_LOGINNAME)).ToList();
                        }
                        if (IsNotNullOrEmpty(expMest))
                        {
                            listSaleExpMest.AddRange(expMest);
                        }
                    }
                }
                listMedicine = listMedicine.Where(o => listSaleExpMest.Exists(p => p.ID == o.EXP_MEST_ID)).ToList();
                listMaterial = listMaterial.Where(o => listSaleExpMest.Exists(p => p.ID == o.EXP_MEST_ID)).ToList();
                if (IsNotNullOrEmpty(listSaleExpMest))
                {
                    List<long> prescriptionId = new List<long>();
                    prescriptionId = listSaleExpMest.Select(o => o.PRESCRIPTION_ID ?? 0).Distinct().ToList();
                    if (IsNotNullOrEmpty(prescriptionId))
                    {
                        var skip = 0;
                        while (prescriptionId.Count - skip > 0)
                        {
                            var listIDs = prescriptionId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                            HisServiceReqFilterQuery reqFilter = new HisServiceReqFilterQuery();
                            reqFilter.IDs = listIDs;
                            var req = new MOS.MANAGER.HisServiceReq.HisServiceReqManager(paramGet).Get(reqFilter);
                            if (IsNotNullOrEmpty(req))
                            {
                                listServiceReq.AddRange(req);
                            }

                            HisExpMestFilterQuery expMestFilter = new HisExpMestFilterQuery();
                            expMestFilter.SERVICE_REQ_IDs = listIDs;
                            var expMest = new MOS.MANAGER.HisExpMest.HisExpMestManager(paramGet).Get(expMestFilter);
                            if (IsNotNullOrEmpty(expMest))
                            {
                                listPrescription.AddRange(expMest);
                            }
                        }
                    }

                    List<long> billIds = new List<long>();
                    billIds = listSaleExpMest.Select(o => o.BILL_ID ?? 0).Distinct().ToList();
                    if (IsNotNullOrEmpty(billIds))
                    {
                        var skip = 0;
                        while (billIds.Count - skip > 0)
                        {
                            var listIDs = billIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                            HisTransactionFilterQuery tranFilter = new HisTransactionFilterQuery();
                            tranFilter.IDs = listIDs;
                            var trans = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).Get(tranFilter);
                            if (IsNotNullOrEmpty(trans))
                            {
                                ListTransaction.AddRange(trans);
                            }
                        }
                    }
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
            var result = true;
            try
            {
                listRdo.Clear();
                Dictionary<long, V_HIS_EXP_MEST> dicExpMest = listSaleExpMest.ToDictionary(o => o.ID);
                Dictionary<long, HIS_EXP_MEST> dicPres = listPrescription.ToDictionary(o => o.SERVICE_REQ_ID ?? 0);
                Dictionary<long, HIS_SERVICE_REQ> dicReq = listServiceReq.ToDictionary(o => o.ID);
                Dictionary<long, HIS_TRANSACTION> dicTran = ListTransaction.ToDictionary(o => o.ID);

                if (IsNotNullOrEmpty(listMedicine))
                {
                    foreach (var Medicine in listMedicine)
                    {
                        Mrs00304RDO rdo = new Mrs00304RDO();
                        if (dicExpMest.ContainsKey(Medicine.EXP_MEST_ID ?? 0))
                        {
                            Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00304RDO>(rdo, dicExpMest[Medicine.EXP_MEST_ID ?? 0]);
                            // ten ng chi dinh la ten bac si ke don trong phong kham, tranh th ng ban sua don lam thay doi ten bac si
                            if (dicReq.ContainsKey(dicExpMest[Medicine.EXP_MEST_ID ?? 0].PRESCRIPTION_ID ?? 0))
                            {
                                rdo.REQ_USERNAME = dicReq[dicExpMest[Medicine.EXP_MEST_ID ?? 0].PRESCRIPTION_ID ?? 0].REQUEST_USERNAME;
                            }

                            if (dicPres.ContainsKey(dicExpMest[Medicine.EXP_MEST_ID ?? 0].PRESCRIPTION_ID ?? 0))
                            {
                                //rdo.REQ_USERNAME = dicPres[dicExpMest[Medicine.EXP_MEST_ID].PRESCRIPTION_ID ?? 0].REQ_USERNAME;
                                rdo.PRESCRIPTION_CODE = dicPres[dicExpMest[Medicine.EXP_MEST_ID ?? 0].PRESCRIPTION_ID ?? 0].EXP_MEST_CODE;
                                rdo.CLIENT_NAME = dicPres[dicExpMest[Medicine.EXP_MEST_ID ?? 0].PRESCRIPTION_ID ?? 0].TDL_PATIENT_NAME;
                            }
                            else
                            {
                                rdo.CLIENT_NAME = dicExpMest[Medicine.EXP_MEST_ID ?? 0].TDL_PATIENT_NAME;
                            }

                            if (dicTran.ContainsKey(dicExpMest[Medicine.EXP_MEST_ID ?? 0].BILL_ID ?? 0))
                            {
                                rdo.TRANSACTION_CODE = dicTran[dicExpMest[Medicine.EXP_MEST_ID ?? 0].BILL_ID ?? 0].TRANSACTION_CODE;
                                rdo.TRANSACTION_NUM_ORDER = dicTran[dicExpMest[Medicine.EXP_MEST_ID ?? 0].BILL_ID ?? 0].NUM_ORDER;
                            }
                        }
                        rdo.EXP_TIME = Medicine.EXP_TIME ?? 99999999999999;
                        rdo.EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(Medicine.EXP_TIME ?? 0);
                        rdo.EXP_MEST_CODE = Medicine.EXP_MEST_CODE;
                        rdo.PRICE = Medicine.PRICE.HasValue ? Medicine.PRICE.Value * (1 + (Medicine.VAT_RATIO ?? 0)) : Medicine.IMP_PRICE * (1 + Medicine.IMP_VAT_RATIO) * (1 + (Medicine.VAT_RATIO ?? 0));
                        rdo.VIR_TOTAL_PRICE_ALL = rdo.PRICE * Medicine.AMOUNT;
                        rdo.VIR_TOTAL_PRICE = (Medicine.PRICE.HasValue ? Medicine.PRICE.Value : Medicine.IMP_PRICE * (1 + Medicine.IMP_VAT_RATIO)) * Medicine.AMOUNT;
                        rdo.VAT_RATIO = Medicine.VAT_RATIO ?? 0;
                        rdo.VIR_TOTAL_VAT_RATIO = rdo.VIR_TOTAL_PRICE * rdo.VAT_RATIO;
                        listRdo.Add(rdo);
                    }
                }

                if (IsNotNullOrEmpty(listMaterial))
                {
                    foreach (var Material in listMaterial)
                    {
                        //var SaleExpMest = listSaleExpMest.Where(o => o.ID == Material.EXP_MEST_ID).ToList(); 
                        //var prescription = new List<V_HIS_EXP_MEST>(); 
                        //if (IsNotNullOrEmpty(SaleExpMest))
                        //{
                        //    prescription = listPrescription.Where(o => o.ID == SaleExpMest.First().PRESCRIPTION_ID).ToList(); 
                        //}
                        Mrs00304RDO rdo = new Mrs00304RDO();
                        if (dicExpMest.ContainsKey(Material.EXP_MEST_ID ?? 0))
                        {
                            Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00304RDO>(rdo, dicExpMest[Material.EXP_MEST_ID ?? 0]);
                            if (dicReq.ContainsKey(dicExpMest[Material.EXP_MEST_ID ?? 0].PRESCRIPTION_ID ?? 0))
                            {
                                rdo.REQ_USERNAME = dicReq[dicExpMest[Material.EXP_MEST_ID ?? 0].PRESCRIPTION_ID ?? 0].REQUEST_USERNAME;
                            }

                            if (dicPres.ContainsKey(dicExpMest[Material.EXP_MEST_ID ?? 0].PRESCRIPTION_ID ?? 0))
                            {
                                //rdo.REQ_USERNAME = dicPres[dicExpMest[Material.EXP_MEST_ID].PRESCRIPTION_ID ?? 0].REQ_USERNAME;
                                rdo.PRESCRIPTION_CODE = dicPres[dicExpMest[Material.EXP_MEST_ID ?? 0].PRESCRIPTION_ID ?? 0].EXP_MEST_CODE;
                                rdo.CLIENT_NAME = dicPres[dicExpMest[Material.EXP_MEST_ID ?? 0].PRESCRIPTION_ID ?? 0].TDL_PATIENT_NAME;
                            }
                            else
                            {
                                rdo.CLIENT_NAME = dicExpMest[Material.EXP_MEST_ID ?? 0].TDL_PATIENT_NAME;
                            }

                            if (dicTran.ContainsKey(dicExpMest[Material.EXP_MEST_ID ?? 0].BILL_ID ?? 0))
                            {
                                rdo.TRANSACTION_CODE = dicTran[dicExpMest[Material.EXP_MEST_ID ?? 0].BILL_ID ?? 0].TRANSACTION_CODE;
                                rdo.TRANSACTION_NUM_ORDER = dicTran[dicExpMest[Material.EXP_MEST_ID ?? 0].BILL_ID ?? 0].NUM_ORDER;
                            }
                        }
                        rdo.EXP_TIME = Material.EXP_TIME ?? 99999999999999;
                        rdo.EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)Material.EXP_TIME);
                        rdo.EXP_MEST_CODE = Material.EXP_MEST_CODE;
                        rdo.PRICE = Material.PRICE.HasValue ? Material.PRICE.Value * (1 + (Material.VAT_RATIO ?? 0)) : Material.IMP_PRICE * (1 + Material.IMP_VAT_RATIO) * (1 + (Material.VAT_RATIO ?? 0));
                        rdo.VIR_TOTAL_PRICE_ALL = rdo.PRICE * Material.AMOUNT;
                        rdo.VIR_TOTAL_PRICE = (Material.PRICE.HasValue ? Material.PRICE.Value: Material.IMP_PRICE * (1 + Material.IMP_VAT_RATIO))*Material.AMOUNT;
                        rdo.VAT_RATIO = Material.VAT_RATIO ?? Material.IMP_VAT_RATIO;
                        rdo.VIR_TOTAL_VAT_RATIO = rdo.VIR_TOTAL_PRICE * rdo.VAT_RATIO;
                        listRdo.Add(rdo);
                    }
                }

                var groupbyExpMestCode = listRdo.GroupBy(o => new { o.EXP_MEST_CODE, o.VAT_RATIO }).ToList();
                listRdo.Clear();
                foreach (var group in groupbyExpMestCode)
                {
                    List<Mrs00304RDO> p = group.ToList<Mrs00304RDO>();
                    Mrs00304RDO rdo = new Mrs00304RDO();
                    rdo = p.First();
                    rdo.EXP_TIME = p.First().EXP_TIME;
                    rdo.EXP_TIME_STR = p.First().EXP_TIME_STR;
                    rdo.EXP_MEST_CODE = p.First().EXP_MEST_CODE;
                    rdo.PRESCRIPTION_CODE = p.First().PRESCRIPTION_CODE;
                    rdo.CLIENT_NAME = p.First().CLIENT_NAME;
                    rdo.REQ_USERNAME = p.First().REQ_USERNAME;
                    rdo.VIR_TOTAL_PRICE = p.Sum(o => o.VIR_TOTAL_PRICE);
                    rdo.VAT_RATIO = p.First().VAT_RATIO;
                    rdo.VIR_TOTAL_VAT_RATIO = p.Sum(o => o.VIR_TOTAL_VAT_RATIO);
                    rdo.VIR_TOTAL_PRICE_ALL = p.Sum(o => o.VIR_TOTAL_PRICE_ALL);

                    rdo.TRANSACTION_CODE = p.First().TRANSACTION_CODE;
                    rdo.TRANSACTION_NUM_ORDER = p.First().TRANSACTION_NUM_ORDER;
                    listRdo.Add(rdo);
                }
                listRdo = listRdo.OrderBy(o => o.EXP_TIME).ThenBy(o => o.ID).ToList();

            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (((Mrs00304Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00304Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00304Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00304Filter)reportFilter).TIME_TO));

            }
            if (IsNotNullOrEmpty(((Mrs00304Filter)reportFilter).LOGINNAME_SALE))
            {
                dicSingleTag.Add("SALE_USERNAME", new AcsUserManager(paramGet).Get<List<ACS_USER>>(new AcsUserFilterQuery()).Where(o => o.LOGINNAME == ((Mrs00304Filter)reportFilter).LOGINNAME_SALE).ToList().First().USERNAME);
            }
            if (IsNotNullOrEmpty(((Mrs00304Filter)reportFilter).LOGINNAME))
            {
                dicSingleTag.Add("SALE_USERNAME", new AcsUserManager(paramGet).Get<List<ACS_USER>>(new AcsUserFilterQuery()).Where(o => o.LOGINNAME == ((Mrs00304Filter)reportFilter).LOGINNAME).ToList().First().USERNAME);
            }
            if (IsNotNullOrEmpty(((Mrs00304Filter)reportFilter).MEDI_STOCK_BUSINESS_IDs))
            {
                dicSingleTag.Add("MEDI_STOCK_NAME", String.Join(", ", new HisMediStockManager().Get(new HisMediStockFilterQuery()).Where(o => ((Mrs00304Filter)reportFilter).MEDI_STOCK_BUSINESS_IDs.Contains(o.ID)).Select(p => p.MEDI_STOCK_NAME).ToArray()));
            }

            objectTag.AddObjectData(store, "Report", listRdo);
        }
    }
}
