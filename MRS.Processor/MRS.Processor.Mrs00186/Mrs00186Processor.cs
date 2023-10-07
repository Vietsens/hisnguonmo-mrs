using MOS.MANAGER.HisService;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisMestPeriodMedi;
using MOS.MANAGER.HisMestPeriodMate;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using AutoMapper;
using FlexCel.Report;
using Inventec.Common.Repository;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.SDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType;

namespace MRS.Processor.Mrs00186
{
    class Mrs00186Processor : AbstractProcessor
    {
        Mrs00186Filter castFilter = null;

        List<VSarReportMrs00186RDO> ListRdo = new List<VSarReportMrs00186RDO>();

        List<VSarReportMrs00186RDO> listRdoMedicine = new List<VSarReportMrs00186RDO>();
        List<VSarReportMrs00186RDO> listRdoMaterial = new List<VSarReportMrs00186RDO>();
        List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine3 = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine3 = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial3 = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial3 = new List<V_HIS_EXP_MEST_MATERIAL>();


        List<V_HIS_MEDI_STOCK> ListMediStock = new List<V_HIS_MEDI_STOCK>();
        List<V_HIS_SERVICE_RETY_CAT> ListServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();
        List<V_HIS_SERVICE_RETY_CAT> ListServiceRetyCat1 = new List<V_HIS_SERVICE_RETY_CAT>();

        Dictionary<string, long> dicExpMestType = new Dictionary<string, long>();
        Dictionary<string, long> dicImpMestType = new Dictionary<string, long>();
        V_HIS_MEDI_STOCK CurrentMediStock = new V_HIS_MEDI_STOCK();
        V_HIS_SERVICE_RETY_CAT CurrentServiceRetyCat = new V_HIS_SERVICE_RETY_CAT();



        public Mrs00186Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00186Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                CommonParam paramGet = new CommonParam();
                castFilter = (Mrs00186Filter)this.reportFilter;


                if (IsNotNullOrEmpty(castFilter.REPORT_TYPE_CAT_IDs))
                {

                    HisServiceRetyCatViewFilterQuery serviceRetyCatFilter = new HisServiceRetyCatViewFilterQuery();
                    ListServiceRetyCat = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(paramGet).GetView(serviceRetyCatFilter);
                    ListServiceRetyCat1 = ListServiceRetyCat.Where(o => castFilter.REPORT_TYPE_CAT_IDs.Contains(o.REPORT_TYPE_CAT_ID)).ToList();
                    ListServiceRetyCat = ListServiceRetyCat.Where(o => castFilter.REPORT_TYPE_CAT_IDs.Contains(o.REPORT_TYPE_CAT_ID)).GroupBy(p => p.REPORT_TYPE_CAT_ID).Select(g => g.First()).ToList();

                }
                //Tao loai nhap xuat
                RDOImpExpMestTypeContext.Define(ref dicImpMestType, ref dicExpMestType);

                ProcessListServiceRetyCat();
                result = true;


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
            return true;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
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

            ListRdo = ListRdo.OrderBy(o => o.REPORT_TYPE_CAT_ID).ThenBy(t1 => t1.SERVICE_TYPE_ID).ThenByDescending(t2 => t2.NUM_ORDER).ThenBy(t3 => t3.SERVICE_NAME).ToList();


            objectTag.AddObjectData(store, "MediStocks", ListServiceRetyCat);
            objectTag.AddObjectData(store, "Services", ListRdo);
            objectTag.AddRelationship(store, "MediStocks", "Services", "REPORT_TYPE_CAT_ID", "REPORT_TYPE_CAT_ID");
            objectTag.SetUserFunction(store, "FuncSameTitleCol", new CustomerFuncMergeSameData());
            objectTag.SetUserFunction(store, "FuncRownumber", new RDOCustomerFuncManyRownumberData());

        }

        private void ProcessListServiceRetyCat()
        {
            try
            {
                if (IsNotNullOrEmpty(ListServiceRetyCat))
                {
                    CommonParam paramGet = new CommonParam(); //CastFilter = (Mrs00157Filter)this.reportFilter; 
                    HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                    impMediFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                    impMediFilter.IMP_TIME_TO = castFilter.TIME_TO;
                    impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                    impMediFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                    //impMediFilter.SERVICE_IDs = ListServiceRetyCat.Where(o=>o.REPORT_TYPE_CAT_ID==serviceRetyCat.REPORT_TYPE_CAT_ID).Select(p=>p.SERVICE_ID).ToList(); 
                    List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager().GetView(impMediFilter);

                    HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                    expMediFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                    expMediFilter.EXP_TIME_TO = castFilter.TIME_TO;
                    expMediFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                    //expMediFilter.MEDICINE_IDs = ListServiceRetyCat.Where(o => o.REPORT_TYPE_CAT_ID == serviceRetyCat.REPORT_TYPE_CAT_ID).Select(p => p.SERVICE_ID).ToList(); 
                    expMediFilter.IS_EXPORT = true;
                    List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager().GetView(expMediFilter);

                    HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                    impMateFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                    impMateFilter.IMP_TIME_TO = castFilter.TIME_TO;
                    impMediFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                    //impMediFilter.SERVICE_IDs = ListServiceRetyCat.Where(o => o.REPORT_TYPE_CAT_ID == serviceRetyCat.REPORT_TYPE_CAT_ID).Select(p => p.SERVICE_ID).ToList(); 
                    impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                    List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager().GetView(impMateFilter);

                    HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                    expMateFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                    expMateFilter.EXP_TIME_TO = castFilter.TIME_TO;
                    expMediFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                    //expMediFilter.MEDICINE_IDs = ListServiceRetyCat.Where(o => o.REPORT_TYPE_CAT_ID == serviceRetyCat.REPORT_TYPE_CAT_ID).Select(p => p.SERVICE_ID).ToList(); 
                    expMediFilter.IS_EXPORT = true;
                    List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager().GetView(expMateFilter);

                    foreach (var serviceRetyCat in ListServiceRetyCat)
                    {
                        hisImpMestMedicine3.Clear();
                        hisExpMestMedicine3.Clear();
                        hisImpMestMaterial3.Clear();
                        hisExpMestMaterial3.Clear();

                        List<string> listServiceCodes = ListServiceRetyCat1.Where(o => o.REPORT_TYPE_CAT_ID == serviceRetyCat.REPORT_TYPE_CAT_ID).Select(o => o.SERVICE_CODE).ToList();
                        CurrentServiceRetyCat = serviceRetyCat;
                       
                        for (int i = 0; i < listServiceCodes.Count; i++)
                        {
                            var hisImpMestMedicine2 = hisImpMestMedicine.Where(o => o.MEDICINE_TYPE_CODE == listServiceCodes[i]).ToList(); //Where(o => o. == ListServiceRetyCat1.Select(p => p.SERVICE_ID).First()).ToList(); 
                            hisImpMestMedicine3.AddRange(hisImpMestMedicine2);
                        }

                       
                        for (int i = 0; i < listServiceCodes.Count; i++)
                        {
                            var hisExpMestMedicine2 = hisExpMestMedicine.Where(o => o.MEDICINE_TYPE_CODE == listServiceCodes[i]).ToList(); //Where(o => o. == ListServiceRetyCat1.Select(p => p.SERVICE_ID).First()).ToList(); 
                            hisExpMestMedicine3.AddRange(hisExpMestMedicine2);
                        }


                        for (int i = 0; i < listServiceCodes.Count; i++)
                        {
                            var hisImpMestMaterial2 = hisImpMestMaterial.Where(o => o.MATERIAL_TYPE_CODE == listServiceCodes[i]).ToList(); //Where(o => o. == ListServiceRetyCat1.Select(p => p.SERVICE_ID).First()).ToList(); 
                            hisImpMestMaterial3.AddRange(hisImpMestMaterial2);
                        }


                      
                        for (int i = 0; i < listServiceCodes.Count; i++)
                        {
                            var hisExpMestMaterial2 = hisExpMestMaterial.Where(o => o.MATERIAL_TYPE_CODE == listServiceCodes[i]).ToList(); //Where(o => o. == ListServiceRetyCat1.Select(p => p.SERVICE_ID).First()).ToList(); 
                            hisExpMestMaterial3.AddRange(hisExpMestMaterial2);
                        }

                        if (!paramGet.HasException)
                        {
                            ProcessAmountMedicine(hisImpMestMedicine3, hisExpMestMedicine3);
                            ProcessAmountMaterial(hisImpMestMaterial3, hisExpMestMaterial3);
                            ProcessGetPeriod(paramGet);
                            listRdoMedicine = groupById(listRdoMedicine);

                            listRdoMedicine = groupByServiceAndPriceAndSupplier(listRdoMedicine);

                            listRdoMaterial = groupById(listRdoMaterial);

                            listRdoMaterial = groupByServiceAndPriceAndSupplier(listRdoMaterial);
                            ListRdo.AddRange(listRdoMedicine);
                            ListRdo.AddRange(listRdoMaterial);
                        }
                        else
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00186.");
                        }
                    }
                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00186.");
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
                ListMediStock.Clear();
            }
        }


        //Tính số lượng nhập và xuất thuốc
        private void ProcessAmountMedicine(List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine, List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine)
        {
            try
            {
                listRdoMedicine.Clear();
                ProcessImpAmountMedicine(hisImpMestMedicine);
                ProcessExpAmountMedicine(hisExpMestMedicine);
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

        private void ProcessExpAmountMedicine(List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine)
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
                        ProcessExpAmountMedicineByExpMestType(listExpMestMediSub, item);
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
                    PropertyInfo p = typeof(VSarReportMrs00186RDO).GetProperty(string.Format("{0}_IMP_AMOUNT", fieldName));
                    if (!IsNotNull(p)) return;

                    var GroupImps = listImpMestMediSub.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        VSarReportMrs00186RDO rdo = new VSarReportMrs00186RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.REPORT_TYPE_CAT_ID = CurrentServiceRetyCat.REPORT_TYPE_CAT_ID;
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID;
                        rdo.SERVICE_ID = listmediSub.First().MEDICINE_TYPE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        p.SetValue(rdo, listmediSub.Sum(s => s.AMOUNT));
                        listRdoMedicine.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessExpAmountMedicineByExpMestType(List<V_HIS_EXP_MEST_MEDICINE> listExpMestMediSub, string fieldName)
        {
            try
            {
                if (IsNotNullOrEmpty(listExpMestMediSub))
                {
                    PropertyInfo p = typeof(VSarReportMrs00186RDO).GetProperty(string.Format("{0}_IMP_AMOUNT", fieldName));
                    if (!IsNotNull(p)) return;

                    var GroupImps = listExpMestMediSub.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        VSarReportMrs00186RDO rdo = new VSarReportMrs00186RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.REPORT_TYPE_CAT_ID = CurrentServiceRetyCat.REPORT_TYPE_CAT_ID;
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID ?? 0;
                        rdo.SERVICE_ID = listmediSub.First().MEDICINE_TYPE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        p.SetValue(rdo, listmediSub.Sum(s => s.AMOUNT));
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
        private void ProcessAmountMaterial(List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial, List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial)
        {
            try
            {
                listRdoMaterial.Clear();
                ProcessImpAmountMaterial(hisImpMestMaterial);
                ProcessExpAmountMaterial(hisExpMestMaterial);
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

        private void ProcessExpAmountMaterial(List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial)
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
                            ProcessExpAmountMateByExpMestType(listExpMestMateSub, item);
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
                    PropertyInfo p = typeof(VSarReportMrs00186RDO).GetProperty(string.Format("{0}_IMP_AMOUNT", fieldName));
                    if (!IsNotNull(p)) return;
                    var GroupImps = listImpMestMateSub.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        VSarReportMrs00186RDO rdo = new VSarReportMrs00186RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.REPORT_TYPE_CAT_ID = CurrentServiceRetyCat.REPORT_TYPE_CAT_ID;
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID;
                        rdo.SERVICE_ID = listmateSub.First().MATERIAL_TYPE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        p.SetValue(rdo, listmateSub.Sum(o => o.AMOUNT));
                        listRdoMaterial.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessExpAmountMateByExpMestType(List<V_HIS_EXP_MEST_MATERIAL> listExpMestMateSub, string fieldName)
        {
            try
            {
                if (IsNotNullOrEmpty(listExpMestMateSub))
                {
                    PropertyInfo p = typeof(VSarReportMrs00186RDO).GetProperty(string.Format("{0}_EXP_AMOUNT", fieldName));
                    if (!IsNotNull(p)) return;

                    var GroupImps = listExpMestMateSub.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        VSarReportMrs00186RDO rdo = new VSarReportMrs00186RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.REPORT_TYPE_CAT_ID = CurrentServiceRetyCat.REPORT_TYPE_CAT_ID;
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID ?? 0;
                        rdo.SERVICE_ID = listmateSub.First().MATERIAL_TYPE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        p.SetValue(rdo, listmateSub.Sum(o => o.AMOUNT));
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
                List<V_HIS_MEDI_STOCK_PERIOD> hisMediStockPeriod = new MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodManager(paramGet).GetView(periodFilter);
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
                List<VSarReportMrs00186RDO> listrdo = new List<VSarReportMrs00186RDO>();
                HisMestPeriodMediViewFilterQuery periodMediFilter = new HisMestPeriodMediViewFilterQuery();
                periodMediFilter.MEDI_STOCK_PERIOD_ID = neighborPeriod.ID;
                List<V_HIS_MEST_PERIOD_MEDI> hisMestPeriodMedi = new MOS.MANAGER.HisMestPeriodMedi.HisMestPeriodMediManager(paramGet).GetView(periodMediFilter);
                if (IsNotNullOrEmpty(hisMestPeriodMedi))
                {
                    foreach (var item in hisMestPeriodMedi)
                    {
                        VSarReportMrs00186RDO rdo = new VSarReportMrs00186RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.REPORT_TYPE_CAT_ID = CurrentServiceRetyCat.REPORT_TYPE_CAT_ID;
                        rdo.MEDI_MATE_ID = item.MEDICINE_ID;
                        rdo.SERVICE_ID = item.MEDICINE_TYPE_ID;
                        rdo.SERVICE_CODE = item.MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = item.MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = item.SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = item.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = item.SUPPLIER_NAME;
                        rdo.NUM_ORDER = item.NUM_ORDER;
                        rdo.IMP_PRICE = item.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = item.AMOUNT;
                        listrdo.Add(rdo);
                    }
                }
                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                impMediFilter.IMP_TIME_FROM = neighborPeriod.CREATE_TIME + 1;
                impMediFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                impMediFilter.MEDI_STOCK_ID = CurrentMediStock.ID;
                impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(impMediFilter);
                if (IsNotNullOrEmpty(hisImpMestMedicine))
                {
                    var GroupImps = hisImpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        VSarReportMrs00186RDO rdo = new VSarReportMrs00186RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.REPORT_TYPE_CAT_ID = CurrentServiceRetyCat.REPORT_TYPE_CAT_ID;
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID;
                        rdo.SERVICE_ID = listmediSub.First().MEDICINE_TYPE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => s.AMOUNT);
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                expMediFilter.EXP_TIME_FROM = neighborPeriod.CREATE_TIME + 1;
                expMediFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMediFilter.MEDI_STOCK_ID = CurrentMediStock.ID;
                expMediFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(expMediFilter);
                if (IsNotNullOrEmpty(hisExpMestMedicine))
                {
                    var GroupExps = hisExpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        VSarReportMrs00186RDO rdo = new VSarReportMrs00186RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.REPORT_TYPE_CAT_ID = CurrentServiceRetyCat.REPORT_TYPE_CAT_ID;
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID ?? 0;
                        rdo.SERVICE_ID = listmediSub.First().MEDICINE_TYPE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => -(s.AMOUNT));
                        listrdo.Add(rdo);
                    }
                }
                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s => new VSarReportMrs00186RDO { SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME, REPORT_TYPE_CAT_ID = s.First().REPORT_TYPE_CAT_ID, SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID, MEDI_STOCK_ID = s.First().MEDI_STOCK_ID, MEDI_MATE_ID = s.First().MEDI_MATE_ID, SUPPLIER_ID = s.First().SUPPLIER_ID, SUPPLIER_CODE = s.First().SUPPLIER_CODE, SUPPLIER_NAME = s.First().SUPPLIER_NAME, IMP_PRICE = s.First().IMP_PRICE, SERVICE_ID = s.First().SERVICE_ID, SERVICE_CODE = s.First().SERVICE_CODE, SERVICE_NAME = s.First().SERVICE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, NUM_ORDER = s.First().NUM_ORDER, BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT) }).ToList();
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
                List<VSarReportMrs00186RDO> listrdo = new List<VSarReportMrs00186RDO>();
                HisMestPeriodMateViewFilterQuery periodMateFilter = new HisMestPeriodMateViewFilterQuery();
                periodMateFilter.MEDI_STOCK_PERIOD_ID = neighborPeriod.ID;
                List<V_HIS_MEST_PERIOD_MATE> hisMestPeriodMate = new MOS.MANAGER.HisMestPeriodMate.HisMestPeriodMateManager(paramGet).GetView(periodMateFilter);
                if (IsNotNullOrEmpty(hisMestPeriodMate))
                {
                    foreach (var item in hisMestPeriodMate)
                    {
                        VSarReportMrs00186RDO rdo = new VSarReportMrs00186RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.REPORT_TYPE_CAT_ID = CurrentServiceRetyCat.REPORT_TYPE_CAT_ID;
                        rdo.MEDI_MATE_ID = item.MATERIAL_ID;
                        rdo.SERVICE_ID = item.MATERIAL_TYPE_ID;
                        rdo.SERVICE_CODE = item.MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = item.MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = item.SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = item.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = item.SUPPLIER_NAME;
                        rdo.NUM_ORDER = item.NUM_ORDER;
                        rdo.IMP_PRICE = item.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = item.AMOUNT;
                        listrdo.Add(rdo);
                    }
                }
                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                impMateFilter.IMP_TIME_FROM = neighborPeriod.CREATE_TIME + 1;
                impMateFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                impMateFilter.MEDI_STOCK_ID = CurrentMediStock.ID;
                impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(impMateFilter);
                if (IsNotNullOrEmpty(hisImpMestMaterial))
                {
                    var GroupImps = hisImpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        VSarReportMrs00186RDO rdo = new VSarReportMrs00186RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.REPORT_TYPE_CAT_ID = CurrentServiceRetyCat.REPORT_TYPE_CAT_ID;
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID;
                        rdo.SERVICE_ID = listmateSub.First().MATERIAL_TYPE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => s.AMOUNT);
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                expMateFilter.EXP_TIME_FROM = neighborPeriod.CREATE_TIME + 1;
                expMateFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMateFilter.MEDI_STOCK_ID = CurrentMediStock.ID;
                expMateFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(paramGet).GetView(expMateFilter);
                if (IsNotNullOrEmpty(hisExpMestMaterial))
                {
                    var GroupExps = hisExpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        VSarReportMrs00186RDO rdo = new VSarReportMrs00186RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.REPORT_TYPE_CAT_ID = CurrentServiceRetyCat.REPORT_TYPE_CAT_ID;
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID ?? 0;
                        rdo.SERVICE_ID = listmateSub.First().MATERIAL_TYPE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => -(s.AMOUNT));
                        listrdo.Add(rdo);
                    }
                }
                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s => new VSarReportMrs00186RDO { SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME, REPORT_TYPE_CAT_ID = s.First().REPORT_TYPE_CAT_ID, SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID, MEDI_STOCK_ID = s.First().MEDI_STOCK_ID, MEDI_MATE_ID = s.First().MEDI_MATE_ID, SUPPLIER_ID = s.First().SUPPLIER_ID, SUPPLIER_CODE = s.First().SUPPLIER_CODE, SUPPLIER_NAME = s.First().SUPPLIER_NAME, IMP_PRICE = s.First().IMP_PRICE, SERVICE_ID = s.First().SERVICE_ID, SERVICE_CODE = s.First().SERVICE_CODE, SERVICE_NAME = s.First().SERVICE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, NUM_ORDER = s.First().NUM_ORDER, BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT) }).ToList();
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
                List<VSarReportMrs00186RDO> listrdo = new List<VSarReportMrs00186RDO>();

                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                impMediFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                impMediFilter.MEDI_STOCK_ID = CurrentMediStock.ID;
                impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager().GetView(impMediFilter);
                if (IsNotNullOrEmpty(hisImpMestMedicine))
                {
                    var GroupImps = hisImpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        VSarReportMrs00186RDO rdo = new VSarReportMrs00186RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.REPORT_TYPE_CAT_ID = CurrentServiceRetyCat.REPORT_TYPE_CAT_ID;
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID;
                        rdo.SERVICE_ID = listmediSub.First().MEDICINE_TYPE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => s.AMOUNT);
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                expMediFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMediFilter.MEDI_STOCK_ID = CurrentMediStock.ID;
                expMediFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager().GetView(expMediFilter);
                if (IsNotNullOrEmpty(hisExpMestMedicine))
                {
                    var GroupExps = hisExpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        VSarReportMrs00186RDO rdo = new VSarReportMrs00186RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.REPORT_TYPE_CAT_ID = CurrentServiceRetyCat.REPORT_TYPE_CAT_ID;
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID ?? 0;
                        rdo.SERVICE_ID = listmediSub.First().MEDICINE_TYPE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => -(s.AMOUNT));
                        listrdo.Add(rdo);
                    }
                }
                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s => new VSarReportMrs00186RDO { SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME, REPORT_TYPE_CAT_ID = s.First().REPORT_TYPE_CAT_ID, SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID, MEDI_STOCK_ID = s.First().MEDI_STOCK_ID, MEDI_MATE_ID = s.First().MEDI_MATE_ID, SUPPLIER_ID = s.First().SUPPLIER_ID, SUPPLIER_CODE = s.First().SUPPLIER_CODE, SUPPLIER_NAME = s.First().SUPPLIER_NAME, IMP_PRICE = s.First().IMP_PRICE, SERVICE_ID = s.First().SERVICE_ID, SERVICE_CODE = s.First().SERVICE_CODE, SERVICE_NAME = s.First().SERVICE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, NUM_ORDER = s.First().NUM_ORDER, BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT) }).ToList();
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
                List<VSarReportMrs00186RDO> listrdo = new List<VSarReportMrs00186RDO>();

                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                impMateFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                impMateFilter.MEDI_STOCK_ID = CurrentMediStock.ID;
                impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(impMateFilter);
                if (IsNotNullOrEmpty(hisImpMestMaterial))
                {
                    var GroupImps = hisImpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        VSarReportMrs00186RDO rdo = new VSarReportMrs00186RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.REPORT_TYPE_CAT_ID = CurrentServiceRetyCat.REPORT_TYPE_CAT_ID;
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID;
                        rdo.SERVICE_ID = listmateSub.First().MATERIAL_TYPE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => s.AMOUNT);
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                expMateFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMateFilter.MEDI_STOCK_ID = CurrentMediStock.ID;
                expMateFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(paramGet).GetView(expMateFilter);
                if (IsNotNullOrEmpty(hisExpMestMaterial))
                {
                    var GroupExps = hisExpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        VSarReportMrs00186RDO rdo = new VSarReportMrs00186RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.REPORT_TYPE_CAT_ID = CurrentServiceRetyCat.REPORT_TYPE_CAT_ID;
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID ?? 0;
                        rdo.SERVICE_ID = listmateSub.First().MATERIAL_TYPE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => -(s.AMOUNT));
                        listrdo.Add(rdo);
                    }
                }
                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s => new VSarReportMrs00186RDO { SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME, REPORT_TYPE_CAT_ID = s.First().REPORT_TYPE_CAT_ID, SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID, MEDI_STOCK_ID = s.First().MEDI_STOCK_ID, MEDI_MATE_ID = s.First().MEDI_MATE_ID, SUPPLIER_ID = s.First().SUPPLIER_ID, SUPPLIER_CODE = s.First().SUPPLIER_CODE, SUPPLIER_NAME = s.First().SUPPLIER_NAME, IMP_PRICE = s.First().IMP_PRICE, SERVICE_ID = s.First().SERVICE_ID, SERVICE_CODE = s.First().SERVICE_CODE, SERVICE_NAME = s.First().SERVICE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, NUM_ORDER = s.First().NUM_ORDER, BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT) }).ToList();
                listRdoMaterial.AddRange(listrdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private Mrs00186Filter ProcessCastFilterQuery(Mrs00186Filter Mrs00186Filter)
        {
            Mrs00186Filter castFilter = null;
            try
            {
                if (Mrs00186Filter == null) throw new ArgumentNullException("Input param Mrs00186Filter is null");

                Mapper.CreateMap<Mrs00186Filter, Mrs00186Filter>();
                castFilter = Mapper.Map<Mrs00186Filter, Mrs00186Filter>(Mrs00186Filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return castFilter;
        }
        //Gop theo id
        private List<VSarReportMrs00186RDO> groupById(List<VSarReportMrs00186RDO> listRdoMedicine)
        {
            List<VSarReportMrs00186RDO> result = new List<VSarReportMrs00186RDO>();
            try
            {
                var group = listRdoMedicine.GroupBy(g => g.MEDI_MATE_ID).ToList();
                Decimal sum = 0;
                VSarReportMrs00186RDO rdo;
                List<VSarReportMrs00186RDO> listSub;
                PropertyInfo[] pi = Properties.Get<VSarReportMrs00186RDO>();
                foreach (var item in group)
                {
                    rdo = new VSarReportMrs00186RDO();
                    listSub = item.ToList<VSarReportMrs00186RDO>();

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
                            field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s)))));
                        }
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<VSarReportMrs00186RDO>();
            }
            return result;
        }
        //Gop theo gia, theo dich vu, theo nha cung cap
        private List<VSarReportMrs00186RDO> groupByServiceAndPriceAndSupplier(List<VSarReportMrs00186RDO> listRdoMedicine)
        {
            List<VSarReportMrs00186RDO> result = new List<VSarReportMrs00186RDO>();
            try
            {
                var group = listRdoMedicine.GroupBy(g => new { g.SERVICE_ID, g.SUPPLIER_ID, g.IMP_PRICE }).ToList();
                Decimal sum = 0;
                VSarReportMrs00186RDO rdo;
                List<VSarReportMrs00186RDO> listSub;
                PropertyInfo[] pi = Properties.Get<VSarReportMrs00186RDO>();
                foreach (var item in group)
                {
                    rdo = new VSarReportMrs00186RDO();
                    listSub = item.ToList<VSarReportMrs00186RDO>();

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
                            field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s)))));
                        }
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<VSarReportMrs00186RDO>();
            }
            return result;
        }

        //Gop theo gia, theo dich vu
        private List<VSarReportMrs00186RDO> groupByServiceAndPrice(List<VSarReportMrs00186RDO> listRdoMedicine)
        {
            List<VSarReportMrs00186RDO> result = new List<VSarReportMrs00186RDO>();
            try
            {
                var group = listRdoMedicine.GroupBy(g => new { g.SERVICE_ID, g.IMP_PRICE }).ToList();
                Decimal sum = 0;
                VSarReportMrs00186RDO rdo;
                List<VSarReportMrs00186RDO> listSub;
                PropertyInfo[] pi = Properties.Get<VSarReportMrs00186RDO>();
                foreach (var item in group)
                {
                    rdo = new VSarReportMrs00186RDO();
                    listSub = item.ToList<VSarReportMrs00186RDO>();

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
                            field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s)))));
                        }
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<VSarReportMrs00186RDO>();
            }
            return result;
        }
        //co gia tri
        private bool IsMeaningful(object p)
        {
            return (IsNotNull(p) && p.ToString() != "0" && p.ToString() != "");
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