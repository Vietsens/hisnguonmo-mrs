using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMest;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Repository;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMestPeriodMate;
using MOS.MANAGER.HisMestPeriodMedi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType;

namespace MRS.Processor.Mrs00531
{
    class Mrs00531Processor : AbstractProcessor
    {
        Mrs00531Filter castFilter = null;

        List<Mrs00531RDO> ListRdo = new List<Mrs00531RDO>();

        List<Mrs00531RDO> listRdoMedicine = new List<Mrs00531RDO>();
        List<Mrs00531RDO> listRdoMaterial = new List<Mrs00531RDO>();

        List<V_HIS_MEDI_STOCK> ListMediStock = new List<V_HIS_MEDI_STOCK>();
        List<V_HIS_MEDI_STOCK> ListCabiMediStock = new List<V_HIS_MEDI_STOCK>();
        V_HIS_MEDI_STOCK CurrentMediStock = new V_HIS_MEDI_STOCK();
        Dictionary<string, long> dicExpMestType = new Dictionary<string, long>();
        Dictionary<string, long> dicImpMestType = new Dictionary<string, long>();
        List<HIS_EXP_MEST> listHisExpMest = new List<HIS_EXP_MEST>();
        public Mrs00531Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00531Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                castFilter = ((Mrs00531Filter)reportFilter);
                ListCabiMediStock = HisMediStockCFG.HisMediStocks.Where(o => o.IS_CABINET == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                if (IsNotNullOrEmpty(castFilter.MEDI_STOCK_IDs))
                {
                    //DS kho
                    HisMediStockViewFilterQuery mediStockFilter = new HisMediStockViewFilterQuery();
                    mediStockFilter.IDs = castFilter.MEDI_STOCK_IDs;
                    ListMediStock = new HisMediStockManager(paramGet).GetView(mediStockFilter);
                    //DS xuất
                    HisExpMestFilterQuery HisExpMestfilter = new HisExpMestFilterQuery();
                    HisExpMestfilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                    HisExpMestfilter.FINISH_TIME_FROM = castFilter.TIME_FROM;
                    HisExpMestfilter.FINISH_TIME_TO = castFilter.TIME_TO;
                    HisExpMestfilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                    listHisExpMest = new HisExpMestManager(paramGet).Get(HisExpMestfilter);
                    result = true;
                }
                else
                {
                    throw new DataMisalignedException("Filter khong truyen len MEDI_STOCK ID");
                }
                //Tao loai nhap xuat
                RDOImpExpMestTypeContext.Define(ref dicImpMestType, ref dicExpMestType);
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
                if (IsNotNullOrEmpty(ListMediStock))
                {
                    CommonParam paramGet = new CommonParam();
                    foreach (var medistock in ListMediStock)
                    {
                        CurrentMediStock = medistock;
                        HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                        impMediFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                        impMediFilter.IMP_TIME_TO = castFilter.TIME_TO;
                        impMediFilter.MEDI_STOCK_ID = medistock.ID;
                        impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                        List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new HisImpMestMedicineManager().GetView(impMediFilter);

                        HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                        expMediFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                        expMediFilter.EXP_TIME_TO = castFilter.TIME_TO;
                        expMediFilter.MEDI_STOCK_ID = medistock.ID;
                        expMediFilter.IS_EXPORT = true;
                        List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new HisExpMestMedicineManager().GetView(expMediFilter);
                        var CkImpMestMedicineId = hisExpMestMedicine.Select(o => o.CK_IMP_MEST_MEDICINE_ID ?? 0).Distinct().ToList() ?? new List<long>();
                        // nhap tuong ung
                        List<V_HIS_IMP_MEST_MEDICINE> listHisImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
                        if (CkImpMestMedicineId != null && CkImpMestMedicineId.Count > 0)
                        {
                            var skip = 0;
                            while (CkImpMestMedicineId.Count - skip > 0)
                            {
                                var Ids = CkImpMestMedicineId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                                HisImpMestMedicineViewFilterQuery HisImpMestMedicinefilter = new HisImpMestMedicineViewFilterQuery();
                                HisImpMestMedicinefilter.IDs = Ids;
                                var listHisImpMestMedicineSub = new HisImpMestMedicineManager(param).GetView(HisImpMestMedicinefilter);
                                if (listHisImpMestMedicineSub == null)
                                    Inventec.Common.Logging.LogSystem.Error("listHisImpMestMedicineSub GetView null");
                                else
                                    listHisImpMestMedicine.AddRange(listHisImpMestMedicineSub);
                            }
                        }
                        HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                        impMateFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                        impMateFilter.IMP_TIME_TO = castFilter.TIME_TO;
                        impMateFilter.MEDI_STOCK_ID = medistock.ID; ////
                        impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                        List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new HisImpMestMaterialManager().GetView(impMateFilter);

                        HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                        expMateFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                        expMateFilter.EXP_TIME_TO = castFilter.TIME_TO;
                        expMateFilter.MEDI_STOCK_ID = medistock.ID;
                        expMateFilter.IS_EXPORT = true;
                        List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new HisExpMestMaterialManager().GetView(expMateFilter);
                        var CkImpMestMaterialId = hisExpMestMaterial.Select(o => o.CK_IMP_MEST_MATERIAL_ID ?? 0).Distinct().ToList() ?? new List<long>();
                        // nhap tuong ung
                        var listHisImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();
                        if (CkImpMestMaterialId != null && CkImpMestMaterialId.Count > 0)
                        {
                            var skip = 0;
                            while (CkImpMestMaterialId.Count - skip > 0)
                            {
                                var Ids = CkImpMestMaterialId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                                HisImpMestMaterialViewFilterQuery HisImpMestMaterialfilter = new HisImpMestMaterialViewFilterQuery();
                                HisImpMestMaterialfilter.IDs = Ids;
                                var listHisImpMestSubMaterial = new HisImpMestMaterialManager(param).GetView(HisImpMestMaterialfilter);
                                if (listHisImpMestSubMaterial == null)
                                    Inventec.Common.Logging.LogSystem.Error("listHisImpMestSubMaterial GetView null");
                                else
                                    listHisImpMestMaterial.AddRange(listHisImpMestSubMaterial);
                            }
                        }
                        if (!paramGet.HasException)
                        {
                            ProcessAmountMedicine(hisImpMestMedicine, hisExpMestMedicine, listHisImpMestMedicine);
                            ProcessAmountMaterial(hisImpMestMaterial, hisExpMestMaterial, listHisImpMestMaterial);
                            ProcessGetPeriod(paramGet);

                            listRdoMedicine = groupById(listRdoMedicine);
                            listRdoMedicine = groupByServiceAndPriceAndSupplier(listRdoMedicine);

                            listRdoMaterial = groupById(listRdoMaterial);
                            listRdoMaterial = groupByServiceAndPriceAndSupplier(listRdoMaterial);

                            if (castFilter.IS_MERGE != null && castFilter.IS_MERGE != false)
                            {
                                #region gop thuoc, vat tu
                                listRdoMedicine = groupByServiceAndPrice(listRdoMedicine);
                                listRdoMaterial = groupByServiceAndPrice(listRdoMaterial);
                                #endregion
                            }
                            ListRdo.AddRange(listRdoMedicine);
                            ListRdo.AddRange(listRdoMaterial);

                            AddInfo(ListRdo);
                        }
                        else
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00531.");
                        }
                    }

                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00531.");
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
                ListMediStock.Clear();
                return false;
            }
            return result;
        }

        private void AddInfo(List<Mrs00531RDO> ListRdo)
        {
            foreach (var item in ListRdo)
            {
                item.END_AMOUNT = item.BEGIN_AMOUNT - item.EXP_TOTAL_AMOUNT + item.IMP_TOTAL_AMOUNT;
                
            }
        }

        //Gop theo id
        private List<Mrs00531RDO> groupById(List<Mrs00531RDO> listRdoMedicine)
        {
            List<Mrs00531RDO> result = new List<Mrs00531RDO>();
            try
            {
                var group = listRdoMedicine.GroupBy(g => g.MEDI_MATE_ID).ToList();
                Decimal sum = 0;
                Mrs00531RDO rdo;
                List<Mrs00531RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00531RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00531RDO();
                    listSub = item.ToList<Mrs00531RDO>();
                    bool hide = true;
                    foreach (var field in pi)
                    {
                        if (field.Name.Contains("_AMOUNT"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s))) ?? new Mrs00531RDO()));
                        }
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<Mrs00531RDO>();
            }
            return result;
        }

        //Gop theo gia, theo dich vu, theo nha cung cap
        private List<Mrs00531RDO> groupByServiceAndPriceAndSupplier(List<Mrs00531RDO> listRdoMedicine)
        {
            List<Mrs00531RDO> result = new List<Mrs00531RDO>();
            try
            {
                var group = listRdoMedicine.GroupBy(g => new { g.SERVICE_ID, g.SUPPLIER_ID, g.IMP_PRICE }).ToList();
                Decimal sum = 0;
                Mrs00531RDO rdo;
                List<Mrs00531RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00531RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00531RDO();
                    listSub = item.ToList<Mrs00531RDO>();
                    bool hide = true;
                    foreach (var field in pi)
                    {
                        if (field.Name.Contains("_AMOUNT"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s))) ?? new Mrs00531RDO()));
                        }
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<Mrs00531RDO>();
            }
            return result;
        }

        //Gop theo gia, theo dich vu
        private List<Mrs00531RDO> groupByServiceAndPrice(List<Mrs00531RDO> listRdoMedicine)
        {
            List<Mrs00531RDO> result = new List<Mrs00531RDO>();
            try
            {
                var group = listRdoMedicine.GroupBy(g => new { g.SERVICE_ID, g.IMP_PRICE }).ToList();
                Decimal sum = 0;
                Mrs00531RDO rdo;
                List<Mrs00531RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00531RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00531RDO();
                    listSub = item.ToList<Mrs00531RDO>();
                    bool hide = true;
                    foreach (var field in pi)
                    {
                        if (field.Name.Contains("_AMOUNT"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s))) ?? new Mrs00531RDO()));
                        }
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<Mrs00531RDO>();
            }
            return result;
        }

        private bool IsMeaningful(object p)
        {
            return (IsNotNull(p) && p.ToString() != "0" && p.ToString() != "");
        }

        //Tính số lượng nhập và xuất thuốc
        private void ProcessAmountMedicine(List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine, List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine, List<V_HIS_IMP_MEST_MEDICINE> hisCkImpMestMedicine)
        {
            try
            {
                listRdoMedicine.Clear();
                ProcessImpAmountMedicine(hisImpMestMedicine);
                ProcessExpAmountMedicine(hisExpMestMedicine, hisCkImpMestMedicine);
                listRdoMedicine = groupById(listRdoMedicine);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listRdoMedicine.Clear();
            }
        }

        private void ProcessImpAmountMedicine(List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine)
        {
            try
            {
                if (IsNotNullOrEmpty(hisImpMestMedicine))
                {
                    var listImpMestMediSub = new List<V_HIS_IMP_MEST_MEDICINE>();
                    foreach (var item in dicImpMestType.Keys)
                    {
                        listImpMestMediSub = hisImpMestMedicine.Where(o => o.IMP_MEST_TYPE_ID == dicImpMestType[item]).ToList();
                        if (!IsNotNullOrEmpty(listImpMestMediSub)) continue;

                        ProcessImpAmountMedicineByImpMestType(listImpMestMediSub, item);
                    }

                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessExpAmountMedicine(List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine, List<V_HIS_IMP_MEST_MEDICINE> hisCkImpMestMedicine)
        {
            try
            {
                if (IsNotNullOrEmpty(hisExpMestMedicine))
                {
                    //Inventec.Common.Logging.LogSystem.Info("1:" + hisExpMestMedicine.Where(o => o.MEDICINE_TYPE_CODE == "ACE001").Sum(p => p.AMOUNT));
                    var listExpMestMediSub = new List<V_HIS_EXP_MEST_MEDICINE>();
                    //Inventec.Common.Logging.LogSystem.Info("1:" + dicExpMestType.Count);
                    foreach (var item in dicExpMestType.Keys)
                    {
                        listExpMestMediSub = hisExpMestMedicine.Where(o => o.EXP_MEST_TYPE_ID == dicExpMestType[item]).ToList();
                        if (!IsNotNullOrEmpty(listExpMestMediSub)) continue;
                        //Inventec.Common.Logging.LogSystem.Info("1:" + item + listExpMestMediSub.Where(o => o.MEDICINE_TYPE_CODE == "ACE001").Sum(p => p.AMOUNT));
                        ProcessExpAmountMedicineByExpMestType(listExpMestMediSub, item, hisCkImpMestMedicine);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessImpAmountMedicineByImpMestType(List<V_HIS_IMP_MEST_MEDICINE> listImpMestMediSub, string fieldName)
        {
            try
            {
                if (IsNotNullOrEmpty(listImpMestMediSub))
                {
                    PropertyInfo p = typeof(Mrs00531RDO).GetProperty(fieldName);
                    if (!IsNotNull(p)) return;

                    var GroupImps = listImpMestMediSub.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        Mrs00531RDO rdo = new Mrs00531RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID;
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        rdo.IMP_VAT = listmediSub.First().IMP_VAT_RATIO;
                        p.SetValue(rdo, listmediSub.Sum(s => s.AMOUNT));
                        rdo.IMP_TOTAL_AMOUNT = listmediSub.Sum(s => s.AMOUNT);
                        listRdoMedicine.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessExpAmountMedicineByExpMestType(List<V_HIS_EXP_MEST_MEDICINE> listExpMestMediSub, string fieldName, List<V_HIS_IMP_MEST_MEDICINE> hisCkImpMestMedicine)
        {
            try
            {
                if (IsNotNullOrEmpty(listExpMestMediSub))
                {
                    PropertyInfo p = typeof(Mrs00531RDO).GetProperty(fieldName);
                    if (!IsNotNull(p)) return;
                    foreach (var exp in listExpMestMediSub)
                    {
                        var expMest = listHisExpMest.FirstOrDefault(o => o.ID == exp.EXP_MEST_ID) ?? new HIS_EXP_MEST();
                        Mrs00531RDO rdo = new Mrs00531RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = exp.MEDICINE_ID ?? 0;
                        rdo.SERVICE_ID = exp.SERVICE_ID;
                        rdo.SERVICE_CODE = exp.MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = exp.MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = exp.SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = exp.SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = exp.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = exp.SUPPLIER_NAME;
                        rdo.NUM_ORDER = exp.NUM_ORDER;
                        rdo.IMP_PRICE = exp.IMP_PRICE;
                        rdo.IMP_VAT = exp.IMP_VAT_RATIO;
                        if (expMest.EXP_MEST_REASON_ID == MRS.Processor.Mrs00531.HisExpMestReasonCFG.HIS_EXP_MEST_REASON___05)
                            rdo.ID__CK_WARD_EXP_AMOUNT = exp.AMOUNT;
                        else
                            p.SetValue(rdo, exp.AMOUNT);

                        rdo.EXP_TOTAL_AMOUNT = exp.AMOUNT;

                        if (fieldName == "ID__CK_EXP_AMOUNT" && this.castFilter.CLN_MEDI_STOCK_CODEs != null)
                        {
                            if (hisCkImpMestMedicine.Exists(o => o.ID == exp.CK_IMP_MEST_MEDICINE_ID && this.castFilter.CLN_MEDI_STOCK_CODEs.Contains(HisMediStockCFG.HisMediStocks.FirstOrDefault(q => q.ID == o.MEDI_STOCK_ID).MEDI_STOCK_CODE)))
                                rdo.ID__CK_CLN_EXP_AMOUNT = exp.AMOUNT;
                            else if (hisCkImpMestMedicine.Exists(o => o.ID == exp.CK_IMP_MEST_MEDICINE_ID && this.ListCabiMediStock.Select(r => r.MEDI_STOCK_CODE).Contains(HisMediStockCFG.HisMediStocks.FirstOrDefault(q => q.ID == o.MEDI_STOCK_ID).MEDI_STOCK_CODE)))
                                rdo.ID__CK_CABI_EXP_AMOUNT = exp.AMOUNT;
                            else if (hisCkImpMestMedicine.Exists(o => o.ID == exp.CK_IMP_MEST_MEDICINE_ID && this.castFilter.UUCLN_MEDI_STOCK_CODEs.Contains(HisMediStockCFG.HisMediStocks.FirstOrDefault(q => q.ID == o.MEDI_STOCK_ID).MEDI_STOCK_CODE)))
                                rdo.ID__CK_UUCLN_EXP_AMOUNT = exp.AMOUNT;
                            else 
                                rdo.ID__CK_UCLN_EXP_AMOUNT = exp.AMOUNT;
                        }
                        //if (rdo.SERVICE_CODE == "ACE001")
                        //Inventec.Common.Logging.LogSystem.Info("sss:" + listmediSub.Sum(x => x.AMOUNT));
                        listRdoMedicine.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        // Tính số lượng nhập và xuất vật tư
        private void ProcessAmountMaterial(List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial, List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial, List<V_HIS_IMP_MEST_MATERIAL> hisCkImpMestMaterial)
        {
            try
            {
                listRdoMaterial.Clear();
                ProcessImpAmountMaterial(hisImpMestMaterial);
                ProcessExpAmountMaterial(hisExpMestMaterial, hisCkImpMestMaterial);
                listRdoMaterial = groupById(listRdoMaterial);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listRdoMaterial.Clear();
            }
        }

        private void ProcessImpAmountMaterial(List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial)
        {
            try
            {
                if (IsNotNullOrEmpty(hisImpMestMaterial))
                {
                    var listImpMestMateSub = new List<V_HIS_IMP_MEST_MATERIAL>();
                    foreach (var item in dicImpMestType.Keys)
                    {
                        listImpMestMateSub = hisImpMestMaterial.Where(o => o.IMP_MEST_TYPE_ID == (long)dicImpMestType[item]).ToList();
                        if (!IsNotNullOrEmpty(listImpMestMateSub)) continue;
                        ProcessImpAmountMateByImpMestType(listImpMestMateSub, item);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessExpAmountMaterial(List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial, List<V_HIS_IMP_MEST_MATERIAL> hisCkImpMestMaterial)
        {
            try
            {
                if (IsNotNullOrEmpty(hisExpMestMaterial))
                {
                    if (IsNotNullOrEmpty(hisExpMestMaterial))
                    {
                        var listExpMestMateSub = new List<V_HIS_EXP_MEST_MATERIAL>();
                        foreach (var item in dicExpMestType.Keys)
                        {
                            listExpMestMateSub = hisExpMestMaterial.Where(o => o.EXP_MEST_TYPE_ID == dicExpMestType[item]).ToList();
                            if (!IsNotNullOrEmpty(listExpMestMateSub)) continue;
                            ProcessExpAmountMateByExpMestType(listExpMestMateSub, item, hisCkImpMestMaterial);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessImpAmountMateByImpMestType(List<V_HIS_IMP_MEST_MATERIAL> listImpMestMateSub, string fieldName)
        {
            try
            {
                if (IsNotNullOrEmpty(listImpMestMateSub))
                {
                    PropertyInfo p = typeof(Mrs00531RDO).GetProperty(string.Format(fieldName));
                    if (!IsNotNull(p)) return;
                    var GroupImps = listImpMestMateSub.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        Mrs00531RDO rdo = new Mrs00531RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID;
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        rdo.IMP_VAT = listmateSub.First().IMP_VAT_RATIO;
                        p.SetValue(rdo, listmateSub.Sum(o => o.AMOUNT));
                        rdo.IMP_TOTAL_AMOUNT = listmateSub.Sum(s => s.AMOUNT);
                        listRdoMaterial.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessExpAmountMateByExpMestType(List<V_HIS_EXP_MEST_MATERIAL> listExpMestMateSub, string fieldName, List<V_HIS_IMP_MEST_MATERIAL> hisCkImpMestMaterial)
        {
            try
            {
                if (IsNotNullOrEmpty(listExpMestMateSub))
                {
                    PropertyInfo p = typeof(Mrs00531RDO).GetProperty(string.Format(fieldName));
                    if (!IsNotNull(p)) return;

                    foreach (var exp in listExpMestMateSub)
                    {
                        Mrs00531RDO rdo = new Mrs00531RDO();
                        var expMest = listHisExpMest.FirstOrDefault(o => o.ID == exp.EXP_MEST_ID) ?? new HIS_EXP_MEST();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = exp.MATERIAL_ID ?? 0;
                        rdo.SERVICE_ID = exp.SERVICE_ID;
                        rdo.SERVICE_CODE = exp.MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = exp.MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = exp.SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = exp.SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = exp.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = exp.SUPPLIER_NAME;
                        rdo.NUM_ORDER = exp.NUM_ORDER;
                        rdo.IMP_PRICE = exp.IMP_PRICE;
                        rdo.IMP_VAT = exp.IMP_VAT_RATIO;

                        if (expMest.EXP_MEST_REASON_ID == MRS.Processor.Mrs00531.HisExpMestReasonCFG.HIS_EXP_MEST_REASON___05)
                            rdo.ID__CK_WARD_EXP_AMOUNT = exp.AMOUNT;
                        else
                            p.SetValue(rdo, exp.AMOUNT);
                        rdo.EXP_TOTAL_AMOUNT = exp.AMOUNT;

                        if (fieldName == "ID__CK_EXP_AMOUNT" && this.castFilter.CLN_MEDI_STOCK_CODEs != null)
                        {
                            if (hisCkImpMestMaterial.Exists(o => o.ID == exp.CK_IMP_MEST_MATERIAL_ID && this.castFilter.CLN_MEDI_STOCK_CODEs.Contains(HisMediStockCFG.HisMediStocks.FirstOrDefault(q => q.ID == o.MEDI_STOCK_ID).MEDI_STOCK_CODE)))
                                rdo.ID__CK_CLN_EXP_AMOUNT = exp.AMOUNT;
                            else if (hisCkImpMestMaterial.Exists(o => o.ID == exp.CK_IMP_MEST_MATERIAL_ID && this.ListCabiMediStock.Select(r => r.MEDI_STOCK_CODE).Contains(HisMediStockCFG.HisMediStocks.FirstOrDefault(q => q.ID == o.MEDI_STOCK_ID).MEDI_STOCK_CODE)))
                                rdo.ID__CK_CABI_EXP_AMOUNT = exp.AMOUNT;
                            else if (hisCkImpMestMaterial.Exists(o => o.ID == exp.CK_IMP_MEST_MATERIAL_ID && this.castFilter.UUCLN_MEDI_STOCK_CODEs.Contains(HisMediStockCFG.HisMediStocks.FirstOrDefault(q => q.ID == o.MEDI_STOCK_ID).MEDI_STOCK_CODE)))
                                rdo.ID__CK_UUCLN_EXP_AMOUNT = exp.AMOUNT;
                            else
                                rdo.ID__CK_UCLN_EXP_AMOUNT = exp.AMOUNT;
                        }
                        listRdoMaterial.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //Lay ky du liệu cũ và gần timeFrom nhất
        private void ProcessGetPeriod(CommonParam paramGet)
        {
            try
            {
                HisMediStockPeriodViewFilterQuery periodFilter = new HisMediStockPeriodViewFilterQuery();
                periodFilter.CREATE_TIME_TO = castFilter.TIME_FROM;
                periodFilter.MEDI_STOCK_ID = CurrentMediStock.ID;
                List<V_HIS_MEDI_STOCK_PERIOD> hisMediStockPeriod = new HisMediStockPeriodManager(paramGet).GetView(periodFilter);
                if (!paramGet.HasException)
                {
                    if (IsNotNullOrEmpty(hisMediStockPeriod))
                    {
                        //Trường hợp có kỳ được chốt gần nhất
                        V_HIS_MEDI_STOCK_PERIOD neighborPeriod = hisMediStockPeriod.OrderByDescending(d => d.CREATE_TIME).ToList()[0];
                        ProcessBeinAmountMedicineByMediStockPeriod(paramGet, neighborPeriod);
                        processBeinAmountMaterialByMediStockPeriod(paramGet, neighborPeriod);
                    }
                    else
                    {
                        // Trường hợp không có kỳ chốt gần nhât - chưa được chốt kỳ nào
                        ProcessBeinAmountMedicineNotMediStockPriod(paramGet);
                        ProcessBeinAmountMaterialNotMediStockPriod(paramGet);
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
                listRdoMaterial.Clear();
                listRdoMedicine.Clear();
            }
        }

        //Tinh so luong ton dau neu co chot ky gan nhat của thuốc
        private void ProcessBeinAmountMedicineByMediStockPeriod(CommonParam paramGet, V_HIS_MEDI_STOCK_PERIOD neighborPeriod)
        {
            try
            {
                List<Mrs00531RDO> listrdo = new List<Mrs00531RDO>();
                HisMestPeriodMediViewFilterQuery periodMediFilter = new HisMestPeriodMediViewFilterQuery();
                periodMediFilter.MEDI_STOCK_PERIOD_ID = neighborPeriod.ID;
                List<V_HIS_MEST_PERIOD_MEDI> hisMestPeriodMedi = new HisMestPeriodMediManager(paramGet).GetView(periodMediFilter);
                if (IsNotNullOrEmpty(hisMestPeriodMedi))
                {
                    foreach (var item in hisMestPeriodMedi)
                    {
                        Mrs00531RDO rdo = new Mrs00531RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = item.MEDICINE_ID;
                        rdo.SERVICE_ID = item.SERVICE_ID;
                        rdo.SERVICE_CODE = item.MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = item.MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = item.SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = item.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = item.SUPPLIER_NAME;
                        rdo.NUM_ORDER = item.NUM_ORDER;
                        rdo.IMP_PRICE = item.IMP_PRICE;
                        rdo.IMP_VAT = item.IMP_VAT_RATIO;
                        rdo.BEGIN_AMOUNT = item.AMOUNT;
                        listrdo.Add(rdo);
                    }
                }

                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                impMediFilter.IMP_TIME_FROM = neighborPeriod.CREATE_TIME + 1;
                impMediFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                impMediFilter.MEDI_STOCK_ID = CurrentMediStock.ID;
                impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new HisImpMestMedicineManager(paramGet).GetView(impMediFilter);
                if (IsNotNullOrEmpty(hisImpMestMedicine))
                {
                    var GroupImps = hisImpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        Mrs00531RDO rdo = new Mrs00531RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID;
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        rdo.IMP_VAT = listmediSub.First().IMP_VAT_RATIO;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => s.AMOUNT);
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                expMediFilter.EXP_TIME_FROM = neighborPeriod.CREATE_TIME + 1;
                expMediFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMediFilter.MEDI_STOCK_ID = CurrentMediStock.ID;
                expMediFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new HisExpMestMedicineManager(paramGet).GetView(expMediFilter);
                if (IsNotNullOrEmpty(hisExpMestMedicine))
                {
                    var GroupExps = hisExpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        Mrs00531RDO rdo = new Mrs00531RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID ?? 0;
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        rdo.IMP_VAT = listmediSub.First().IMP_VAT_RATIO;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => -(s.AMOUNT));
                        listrdo.Add(rdo);
                    }
                }

                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s => new Mrs00531RDO
                {
                    SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                    SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                    MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                    MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                    MEDI_MATE_ID = s.First().MEDI_MATE_ID,
                    SUPPLIER_ID = s.First().SUPPLIER_ID,
                    SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                    SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                    IMP_PRICE = s.First().IMP_PRICE,
                    IMP_VAT = s.First().IMP_VAT,
                    SERVICE_ID = s.First().SERVICE_ID,
                    SERVICE_CODE = s.First().SERVICE_CODE,
                    SERVICE_NAME = s.First().SERVICE_NAME,
                    SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                    NUM_ORDER = s.First().NUM_ORDER,
                    BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT)
                }).ToList();
                listRdoMedicine.AddRange(listrdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //Tính số lượng tồn đầu nếu có chốt kỳ gần nhất của vật tư
        private void processBeinAmountMaterialByMediStockPeriod(CommonParam paramGet, V_HIS_MEDI_STOCK_PERIOD neighborPeriod)
        {
            try
            {
                List<Mrs00531RDO> listrdo = new List<Mrs00531RDO>();
                HisMestPeriodMateViewFilterQuery periodMateFilter = new HisMestPeriodMateViewFilterQuery();
                periodMateFilter.MEDI_STOCK_PERIOD_ID = neighborPeriod.ID;
                List<V_HIS_MEST_PERIOD_MATE> hisMestPeriodMate = new HisMestPeriodMateManager(paramGet).GetView(periodMateFilter);
                if (IsNotNullOrEmpty(hisMestPeriodMate))
                {
                    foreach (var item in hisMestPeriodMate)
                    {
                        Mrs00531RDO rdo = new Mrs00531RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = item.MATERIAL_ID;
                        rdo.SERVICE_ID = item.SERVICE_ID;
                        rdo.SERVICE_CODE = item.MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = item.MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = item.SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = item.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = item.SUPPLIER_NAME;
                        rdo.NUM_ORDER = item.NUM_ORDER;
                        rdo.IMP_PRICE = item.IMP_PRICE;
                        rdo.IMP_VAT = item.IMP_VAT_RATIO;
                        rdo.BEGIN_AMOUNT = item.AMOUNT;
                        listrdo.Add(rdo);
                    }
                }

                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                impMateFilter.IMP_TIME_FROM = neighborPeriod.CREATE_TIME + 1;
                impMateFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                impMateFilter.MEDI_STOCK_ID = CurrentMediStock.ID;
                impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; ;
                List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new HisImpMestMaterialManager(paramGet).GetView(impMateFilter);
                if (IsNotNullOrEmpty(hisImpMestMaterial))
                {
                    var GroupImps = hisImpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        Mrs00531RDO rdo = new Mrs00531RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID;
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        rdo.IMP_VAT = listmateSub.First().IMP_VAT_RATIO;
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => s.AMOUNT);
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                expMateFilter.EXP_TIME_FROM = neighborPeriod.CREATE_TIME + 1;
                expMateFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMateFilter.MEDI_STOCK_ID = CurrentMediStock.ID;
                expMateFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new HisExpMestMaterialManager(paramGet).GetView(expMateFilter);
                if (IsNotNullOrEmpty(hisExpMestMaterial))
                {
                    var GroupExps = hisExpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        Mrs00531RDO rdo = new Mrs00531RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID ?? 0;
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        rdo.IMP_VAT = listmateSub.First().IMP_VAT_RATIO;
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => -(s.AMOUNT));
                        listrdo.Add(rdo);
                    }
                }

                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s => new Mrs00531RDO
                {
                    SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                    SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                    MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                    MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                    MEDI_MATE_ID = s.First().MEDI_MATE_ID,
                    SUPPLIER_ID = s.First().SUPPLIER_ID,
                    SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                    SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                    IMP_PRICE = s.First().IMP_PRICE,
                    IMP_VAT = s.First().IMP_VAT,
                    SERVICE_ID = s.First().SERVICE_ID,
                    SERVICE_CODE = s.First().SERVICE_CODE,
                    SERVICE_NAME = s.First().SERVICE_NAME,
                    SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                    NUM_ORDER = s.First().NUM_ORDER,
                    BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT)
                }).ToList();
                listRdoMaterial.AddRange(listrdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //Tính số lượng tồn đầu nều không có chốt kỳ gần nhất của thuốc
        private void ProcessBeinAmountMedicineNotMediStockPriod(CommonParam paramGet)
        {
            try
            {
                List<Mrs00531RDO> listrdo = new List<Mrs00531RDO>();

                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                impMediFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                impMediFilter.MEDI_STOCK_ID = CurrentMediStock.ID;
                impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new HisImpMestMedicineManager().GetView(impMediFilter);
                if (IsNotNullOrEmpty(hisImpMestMedicine))
                {
                    var GroupImps = hisImpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        Mrs00531RDO rdo = new Mrs00531RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID;
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        rdo.IMP_VAT = listmediSub.First().IMP_VAT_RATIO;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => s.AMOUNT);
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                expMediFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMediFilter.MEDI_STOCK_ID = CurrentMediStock.ID;
                expMediFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new HisExpMestMedicineManager().GetView(expMediFilter);
                if (IsNotNullOrEmpty(hisExpMestMedicine))
                {
                    var GroupExps = hisExpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        Mrs00531RDO rdo = new Mrs00531RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID ?? 0;
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        rdo.IMP_VAT = listmediSub.First().IMP_VAT_RATIO;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => -(s.AMOUNT));
                        listrdo.Add(rdo);
                    }
                }

                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s =>
                    new Mrs00531RDO
                    {
                        SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                        SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                        MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                        MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                        MEDI_MATE_ID = s.First().MEDI_MATE_ID,
                        SUPPLIER_ID = s.First().SUPPLIER_ID,
                        SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                        SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                        IMP_PRICE = s.First().IMP_PRICE,
                        IMP_VAT = s.First().IMP_VAT,
                        SERVICE_ID = s.First().SERVICE_ID,
                        SERVICE_CODE = s.First().SERVICE_CODE,
                        SERVICE_NAME = s.First().SERVICE_NAME,
                        SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                        NUM_ORDER = s.First().NUM_ORDER,
                        BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT)
                    }).ToList();
                listRdoMedicine.AddRange(listrdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        // Tính số lượng tồn đầu nếu không có chốt kỳ gần nhất của vật tư
        private void ProcessBeinAmountMaterialNotMediStockPriod(CommonParam paramGet)
        {
            try
            {
                List<Mrs00531RDO> listrdo = new List<Mrs00531RDO>();

                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                impMateFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                impMateFilter.MEDI_STOCK_ID = CurrentMediStock.ID;
                impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; ;
                List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new HisImpMestMaterialManager(paramGet).GetView(impMateFilter);
                if (IsNotNullOrEmpty(hisImpMestMaterial))
                {
                    var GroupImps = hisImpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        Mrs00531RDO rdo = new Mrs00531RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID;
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        rdo.IMP_VAT = listmateSub.First().IMP_VAT_RATIO;
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => s.AMOUNT);
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                expMateFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMateFilter.MEDI_STOCK_ID = CurrentMediStock.ID;
                expMateFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new HisExpMestMaterialManager(paramGet).GetView(expMateFilter);
                if (IsNotNullOrEmpty(hisExpMestMaterial))
                {
                    var GroupExps = hisExpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        Mrs00531RDO rdo = new Mrs00531RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID ?? 0;
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        rdo.IMP_VAT = listmateSub.First().IMP_VAT_RATIO;
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => -(s.AMOUNT));
                        listrdo.Add(rdo);
                    }
                }

                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s =>
                    new Mrs00531RDO
                    {
                        SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                        SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                        MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                        MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                        MEDI_MATE_ID = s.First().MEDI_MATE_ID,
                        SUPPLIER_ID = s.First().SUPPLIER_ID,
                        SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                        SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                        IMP_PRICE = s.First().IMP_PRICE,
                        IMP_VAT = s.First().IMP_VAT,
                        SERVICE_ID = s.First().SERVICE_ID,
                        SERVICE_CODE = s.First().SERVICE_CODE,
                        SERVICE_NAME = s.First().SERVICE_NAME,
                        SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                        NUM_ORDER = s.First().NUM_ORDER,
                        BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT)
                    }).ToList();
                listRdoMaterial.AddRange(listrdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            #region Cac the Single
            if (castFilter.TIME_FROM > 0)
            {
                dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
            }
            if (castFilter.TIME_TO > 0)
            {
                dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
            }
            #endregion

            ListRdo = ListRdo.OrderBy(o => o.MEDI_STOCK_ID).ThenBy(t1 => t1.SERVICE_TYPE_ID).ThenBy(t3 => t3.SERVICE_NAME).ThenByDescending(t2 => t2.NUM_ORDER).ToList();
            if (ListRdo != null && ListRdo.Count > 0 && ListMediStock != null && ListMediStock.Count > 0)
            {
                var mediStockId = ListRdo.Select(o => o.MEDI_STOCK_ID).Distinct().ToList();
                ListMediStock = ListMediStock.Where(o => mediStockId.Contains(o.ID)).ToList();
            }

            objectTag.AddObjectData(store, "MediStocks", ListMediStock);
            objectTag.AddObjectData(store, "Services", ListRdo);
            objectTag.AddRelationship(store, "MediStocks", "Services", "ID", "MEDI_STOCK_ID");
            objectTag.SetUserFunction(store, "FuncSameTitleCol", new CustomerFuncMergeSameData());
            objectTag.SetUserFunction(store, "FuncRownumber", new RDOCustomerFuncManyRownumberData());
        }
    }

    class CustomerFuncMergeSameData : TFlexCelUserFunction
    {
        long MediStockId;
        int SameType;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length <= 0)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            bool result = false;
            try
            {
                long mediId = Convert.ToInt64(parameters[0]);
                int ServiceId = Convert.ToInt32(parameters[1]);

                if (mediId > 0 && ServiceId > 0)
                {
                    if (SameType == ServiceId && MediStockId == mediId)
                    {
                        return true;
                    }
                    else
                    {
                        MediStockId = mediId;
                        SameType = ServiceId;
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return result;
        }
    }

    class RDOCustomerFuncManyRownumberData : TFlexCelUserFunction
    {
        long Medi_Stock_Id;
        int Service_Type_Id;
        long num_order = 0;
        public RDOCustomerFuncManyRownumberData()
        {
        }
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            try
            {
                long mediId = Convert.ToInt64(parameters[0]);
                int ServiceId = Convert.ToInt32(parameters[1]);

                if (mediId > 0 && ServiceId > 0)
                {
                    if (Service_Type_Id == ServiceId && Medi_Stock_Id == mediId)
                    {
                        num_order = num_order + 1;
                    }
                    else
                    {
                        Medi_Stock_Id = mediId;
                        Service_Type_Id = ServiceId;
                        num_order = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Debug(ex);
            }

            return num_order;
        }
    }
}
