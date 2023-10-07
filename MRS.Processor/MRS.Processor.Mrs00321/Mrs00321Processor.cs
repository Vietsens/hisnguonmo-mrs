using FlexCel.Report;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisTransaction;

namespace MRS.Processor.Mrs00321
{
    class Mrs00321Processor : AbstractProcessor
    {
        Mrs00321Filter castFilter = null;
        List<Mrs00321RDO> listRdo = new List<Mrs00321RDO>();
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>();    // Ds Thuốc xuất
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterials = new List<V_HIS_EXP_MEST_MATERIAL>();    // Ds vật tư xuất
        List<V_HIS_EXP_MEST> listSaleExpMests = new List<V_HIS_EXP_MEST>();                // phiếu xuất bán
        List<HIS_MEDI_STOCK> listMediStocks = new List<HIS_MEDI_STOCK>();

        List<Mrs00321RDO> listRdoCashier = new List<Mrs00321RDO>();

        public Mrs00321Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00321Filter);
        }

        //get dữ liệu từ data base
        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00321Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();

                HisMediStockFilterQuery mediStockFilter = new HisMediStockFilterQuery();
                if (IsNotNullOrEmpty(castFilter.MEDI_STOCK_BUSINESS_IDs))
                    mediStockFilter.IDs = castFilter.MEDI_STOCK_BUSINESS_IDs;
                listMediStocks = new MOS.MANAGER.HisMediStock.HisMediStockManager(param).Get(mediStockFilter);

                if (IsNotNullOrEmpty(listMediStocks))
                {
                    List<long> expMestIds = new List<long>();

                    HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
                    expMestMedicineViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                    expMestMedicineViewFilter.EXP_TIME_TO = castFilter.TIME_TO;
                    expMestMedicineViewFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_BUSINESS_IDs;
                    expMestMedicineViewFilter.EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN;
                    expMestMedicineViewFilter.IS_EXPORT = true;
                    listExpMestMedicines.AddRange(new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineViewFilter));

                    if (IsNotNullOrEmpty(listExpMestMedicines))
                        expMestIds.AddRange(listExpMestMedicines.Select(o => o.EXP_MEST_ID ?? 0).Distinct().ToList());

                    HisExpMestMaterialViewFilterQuery expMestMaterialViewFilter = new HisExpMestMaterialViewFilterQuery();
                    expMestMaterialViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                    expMestMaterialViewFilter.EXP_TIME_TO = castFilter.TIME_TO;
                    expMestMaterialViewFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_BUSINESS_IDs;
                    expMestMaterialViewFilter.EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN;
                    expMestMaterialViewFilter.IS_EXPORT = true;
                    listExpMestMaterials.AddRange(new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(param).GetView(expMestMaterialViewFilter));

                    if (IsNotNullOrEmpty(listExpMestMaterials))
                        expMestIds.AddRange(listExpMestMaterials.Select(o => o.EXP_MEST_ID ?? 0).Distinct().ToList());

                    if (IsNotNullOrEmpty(expMestIds))
                    {
                        expMestIds = expMestIds.Distinct().ToList();
                        var skip = 0;
                        while (expMestIds.Count - skip > 0)
                        {
                            var listIds = expMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                            var expMestFilter = new HisExpMestViewFilterQuery
                            {
                                IDs = listIds
                            };
                            var listExpMestView = new MOS.MANAGER.HisExpMest.HisExpMestManager(param).GetView(expMestFilter);
                            listSaleExpMests.AddRange(listExpMestView);
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
            bool result = false;
            try
            {
                if (IsNotNullOrEmpty(listSaleExpMests))
                {
                    foreach (var sale in listSaleExpMests)
                    {
                        var medicines = listExpMestMedicines.Where(s => s.EXP_MEST_ID == sale.ID).ToList();
                        if (IsNotNullOrEmpty(medicines))
                        {
                            foreach (var medi in medicines)
                            {
                                var rdo = new Mrs00321RDO();
                                rdo.REQ_USERNAME = sale.REQ_USERNAME;
                                rdo.EXE_USERNAME = medi.EXP_USERNAME;
                                rdo.EXP_MEST_ID = sale.ID;
                                rdo.AMOUNT = medi.AMOUNT;
                                rdo.VAT_RATIO = medi.VAT_RATIO ?? 0;
                                rdo.PRICE = medi.PRICE ?? 0;
                                rdo.IMP_PRICE = medi.IMP_PRICE;
                                rdo.TOTAL_PRICE = (1 + medi.VAT_RATIO ?? 0) * medi.AMOUNT * medi.PRICE ?? medi.IMP_PRICE;
                                rdo.TOTAL_IMP_PRICE = (1 + medi.IMP_VAT_RATIO) * medi.AMOUNT * medi.IMP_PRICE;
                                rdo.CASHIER_USERNAME = sale.CASHIER_USERNAME ?? sale.REQ_USERNAME;

                                listRdo.Add(rdo);
                            }
                        }

                        var materials = listExpMestMaterials.Where(s => s.EXP_MEST_ID == sale.ID).ToList();
                        if (IsNotNullOrEmpty(materials))
                        {
                            foreach (var mate in materials)
                            {
                                var rdo = new Mrs00321RDO();
                                rdo.REQ_USERNAME = sale.REQ_USERNAME;
                                rdo.EXE_USERNAME = mate.EXP_USERNAME;
                                rdo.EXP_MEST_ID = sale.ID;
                                rdo.AMOUNT = mate.AMOUNT;
                                rdo.VAT_RATIO = mate.VAT_RATIO ?? 0;
                                rdo.PRICE = mate.PRICE ?? 0;
                                rdo.IMP_PRICE = mate.IMP_PRICE;
                                rdo.TOTAL_PRICE = (1 + mate.VAT_RATIO ?? 0) * mate.AMOUNT * mate.PRICE ?? mate.IMP_PRICE;
                                rdo.TOTAL_IMP_PRICE = (1 + mate.IMP_VAT_RATIO) * mate.AMOUNT * mate.IMP_PRICE;
                                rdo.CASHIER_USERNAME = sale.CASHIER_USERNAME ?? sale.REQ_USERNAME;

                                listRdo.Add(rdo);
                            }
                        }
                    }

                    if (IsNotNullOrEmpty(listRdo))
                    {
                        listRdoCashier = listRdo.GroupBy(g => g.CASHIER_USERNAME).Select(s => new Mrs00321RDO
                        {
                            CASHIER_USERNAME = s.First().CASHIER_USERNAME,
                            BILL_AMOUNT = s.Select(sl => sl.EXP_MEST_ID).Distinct().Count(),
                            TOTAL_PRICE = s.Sum(su => su.TOTAL_PRICE),
                            TOTAL_IMP_PRICE = s.Sum(su => su.TOTAL_IMP_PRICE),
                            AMOUNT = s.Sum(su => su.AMOUNT)
                        }).ToList();

                        listRdo = listRdo.GroupBy(g => g.REQ_USERNAME).Select(s => new Mrs00321RDO
                        {
                            REQ_USERNAME = s.First().REQ_USERNAME,
                            BILL_AMOUNT = s.Select(sl => sl.EXP_MEST_ID).Distinct().Count(),
                            TOTAL_PRICE = s.Sum(su => su.TOTAL_PRICE),
                            TOTAL_IMP_PRICE = s.Sum(su => su.TOTAL_IMP_PRICE),
                            AMOUNT = s.Sum(su => su.AMOUNT)
                        }).ToList();
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        // xuất ra báo cáo
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                if (listMediStocks != null && listMediStocks.Count > 0)
                    dicSingleTag.Add("MEDI_STOCK_NAMEs", String.Join(", ", listMediStocks.Select(s => s.MEDI_STOCK_NAME).ToArray()));

                objectTag.AddObjectData(store, "Report", listRdo.OrderBy(s => s.REQ_USERNAME).ToList());
                objectTag.AddObjectData(store, "ReportCashier", listRdoCashier.OrderBy(s => s.CASHIER_USERNAME).ToList());
                //objectTag.AddRelationship(store, "Parent", "Report", "IMP_MEST_CODE", "IMP_MEST_CODE"); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
