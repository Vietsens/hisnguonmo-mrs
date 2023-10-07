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
using MOS.MANAGER.HisReportTypeCat;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisMedicineType;
using Inventec.Common.Logging;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisMedicine;

namespace MRS.Processor.Mrs00179
{
    class Mrs00179Processor : AbstractProcessor
    {
        Mrs00179Filter castFilter = null;

        List<VSarReportMrs00179RDO> ListRdo = new List<VSarReportMrs00179RDO>();

        List<VSarReportMrs00179RDO> listRdoMedicine = new List<VSarReportMrs00179RDO>();
        List<VSarReportMrs00179RDO> listRdoMaterial = new List<VSarReportMrs00179RDO>();

        List<V_HIS_MEDI_STOCK> ListMediStock = new List<V_HIS_MEDI_STOCK>();
        List<HIS_REPORT_TYPE_CAT> listReportTypeCat = new List<HIS_REPORT_TYPE_CAT>();
        List<V_HIS_SERVICE_RETY_CAT> ListServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();

        List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<V_HIS_EXP_MEST> hisExpMest = new List<V_HIS_EXP_MEST>();

        List<V_HIS_MEDICINE_TYPE> hisMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        List<HIS_MEDICINE> Medicines = new List<HIS_MEDICINE>();
        List<HIS_MATERIAL> Materials = new List<HIS_MATERIAL>();

        Dictionary<string, long> dicExpMestType = new Dictionary<string, long>();
        Dictionary<string, long> dicImpMestType = new Dictionary<string, long>();

        public Mrs00179Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00179Filter);
        }
        protected override bool GetData()
        {
            bool result = false;
            try
            {
                castFilter = (Mrs00179Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                HisReportTypeCatFilterQuery reportTypeCatfilter = new HisReportTypeCatFilterQuery();
                reportTypeCatfilter.REPORT_TYPE_CODE__EXACT = this.ReportTypeCode;
                reportTypeCatfilter.IDs = this.castFilter.REPORT_TYPE_CAT_IDs;
                listReportTypeCat = new HisReportTypeCatManager().Get(reportTypeCatfilter);
                if (IsNotNullOrEmpty(listReportTypeCat))
                {
                    var skip = 0;
                    while (listReportTypeCat.Count - skip > 0)
                    {
                        var listIDs = listReportTypeCat.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisServiceRetyCatViewFilterQuery serviceRetyCatFilter = new HisServiceRetyCatViewFilterQuery();
                        serviceRetyCatFilter.REPORT_TYPE_CAT_IDs = listIDs.Select(o => o.ID).ToList();
                        var ListServiceRetyCatSub = new HisServiceRetyCatManager(paramGet).GetView(serviceRetyCatFilter);
                        if (IsNotNullOrEmpty(ListServiceRetyCatSub))
                            ListServiceRetyCat.AddRange(ListServiceRetyCatSub);
                    }

                }

                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                impMediFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                impMediFilter.IMP_TIME_TO = castFilter.TIME_TO;
                impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                impMediFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                hisImpMestMedicine = new HisImpMestMedicineManager().GetView(impMediFilter);
                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                expMediFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                expMediFilter.EXP_TIME_TO = castFilter.TIME_TO;
                expMediFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMediFilter.IS_EXPORT = true;
                hisExpMestMedicine = new HisExpMestMedicineManager().GetView(expMediFilter);

                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                impMateFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                impMateFilter.IMP_TIME_TO = castFilter.TIME_TO;
                impMateFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                hisImpMestMaterial = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager().GetView(impMateFilter);

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                expMateFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                expMateFilter.EXP_TIME_TO = castFilter.TIME_TO;
                expMateFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMateFilter.IS_EXPORT = true;
                hisExpMestMaterial = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager().GetView(expMateFilter);
                GetExpMest(hisExpMestMedicine, hisExpMestMaterial);
                hisMedicineType = new HisMedicineTypeManager().GetView(new HisMedicineTypeViewFilterQuery());
                //Tao loai nhap xuat
                RDOImpExpMestTypeContext.Define(ref dicImpMestType, ref dicExpMestType);
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetExpMest(List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine, List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial)
        {
            try
            {
                HisExpMestViewFilterQuery expFilter = new HisExpMestViewFilterQuery();
                expFilter.FINISH_TIME_FROM = castFilter.TIME_FROM;
                expFilter.FINISH_TIME_TO = castFilter.TIME_TO;
                expFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                hisExpMest = new HisExpMestManager().GetView(expFilter);
                var expMestId = new List<long>();
                if (hisExpMestMedicine != null)
                {
                    expMestId.AddRange(hisExpMestMedicine.Select(o => o.EXP_MEST_ID ?? 0).ToList());
                }
                if (hisExpMestMaterial != null)
                {
                    expMestId.AddRange(hisExpMestMaterial.Select(o => o.EXP_MEST_ID ?? 0).ToList());
                }
                expMestId = expMestId.Where(o => hisExpMest != null && hisExpMest.Exists(p => p.ID == o)).Distinct().ToList();
                if (expMestId.Count>0)
                {
                    var skip = 0;
                    while (expMestId.Count - skip > 0)
                    {
                        var listIDs = expMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisExpMestViewFilterQuery exFilter = new HisExpMestViewFilterQuery();
                        exFilter.IDs = listIDs;
                        var hisExpMestSub = new HisExpMestManager().GetView(exFilter);
                        if (IsNotNullOrEmpty(hisExpMestSub))
                            hisExpMest.AddRange(hisExpMestSub);
                    }

                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                ProcessAmountMedicine(hisImpMestMedicine, hisExpMestMedicine);

                ProcessAmountMaterial(hisImpMestMaterial, hisExpMestMaterial);

                ProcessGetPeriod();

                listRdoMedicine = groupById(listRdoMedicine);
                GetMediMate(listRdoMedicine);
                listRdoMedicine = groupByServiceAndPrice(listRdoMedicine);

                listRdoMaterial = groupById(listRdoMaterial);

                GetMediMate(listRdoMaterial);
                listRdoMaterial = groupByServiceAndPrice(listRdoMaterial);

                ListRdo.AddRange(listRdoMedicine);
                ListRdo.AddRange(listRdoMaterial);
                AddInfoGroup(ListRdo);
                AddInfoMedicine(ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
                result = false;
            }
            return result;
        }

        private void AddInfoMedicine(List<VSarReportMrs00179RDO> ListRdo)
        {
            foreach (var item in ListRdo)
            {
                if (item.SERVICE_TYPE_ID == 1)
                {
                    var medicineType = hisMedicineType.FirstOrDefault(o => o.SERVICE_ID == item.SERVICE_ID);
                    if (medicineType != null)
                    {
                        item.REGISTER_NUMBER = medicineType.REGISTER_NUMBER;//Số đăng ký
                    }
                    var medicine = Medicines.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                    if (medicine != null)
                    {
                        item.PACKAGE_NUMBER = medicine.PACKAGE_NUMBER;//Số lô
                        item.BID_PACKAGE_CODE = medicine.TDL_BID_PACKAGE_CODE;//Mã gói thầu
                        item.MEDICINE_REGISTER_NUMBER = medicine.MEDICINE_REGISTER_NUMBER;//Số đăng ký
                        item.BID_GROUP_CODE = medicine.TDL_BID_GROUP_CODE;//Mã nhóm thầu
                        item.BID_NUM_ORDER = medicine.TDL_BID_NUM_ORDER;//Số thứ tự thầu
                    }
                }
                if (item.SERVICE_TYPE_ID == 2)
                {
                    var material = Materials.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                    if (material != null)
                    {
                        item.PACKAGE_NUMBER = material.PACKAGE_NUMBER;//Số lô
                        item.BID_PACKAGE_CODE = material.TDL_BID_PACKAGE_CODE;//Mã gói thầu
                        //
                        item.BID_GROUP_CODE = material.TDL_BID_GROUP_CODE;//Mã nhóm thầu
                        item.BID_NUM_ORDER = material.TDL_BID_NUM_ORDER;//Số thứ tự thầu
                    }
                }
            }
        }

        private void GetMediMate(List<VSarReportMrs00179RDO> list)
        {
            try
            {
                List<long> medicineIds = new List<long>();
                List<long> materialIds = new List<long>();
                if (list != null)
                {
                    medicineIds.AddRange(list.Where(p => p.SERVICE_TYPE_ID == 1).Select(o => o.MEDI_MATE_ID).ToList());
                    medicineIds = medicineIds.Distinct().ToList();
                    materialIds.AddRange(list.Where(p => p.SERVICE_TYPE_ID == 2).Select(o => o.MEDI_MATE_ID).ToList());
                    materialIds = materialIds.Distinct().ToList();
                    if (medicineIds.Count > 0)
                    {
                        var skip = 0;
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
                    if (materialIds.Count > 0)
                    {
                        var skip = 0;
                        while (materialIds.Count - skip > 0)
                        {
                            var limit = materialIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisMaterialFilterQuery Materialfilter = new HisMaterialFilterQuery();
                            Materialfilter.IDs = limit;
                            var MaterialSub = new HisMaterialManager().Get(Materialfilter);
                            Materials.AddRange(MaterialSub);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

            }
        }


        private void AddInfoGroup(List<VSarReportMrs00179RDO> ListRdo)
        {
            foreach (var item in ListRdo)
            {
                var medicineType = hisMedicineType.FirstOrDefault(o => o.SERVICE_ID == item.SERVICE_ID);
                if (item.SERVICE_TYPE_ID == 1)
                {
                    if (medicineType != null && medicineType.MEDICINE_LINE_ID != null)
                    {
                        item.MEDICINE_LINE_ID = medicineType.MEDICINE_LINE_ID;
                        item.MEDICINE_LINE_CODE = medicineType.MEDICINE_LINE_CODE;
                        item.MEDICINE_LINE_NAME = medicineType.MEDICINE_LINE_NAME;
                    }
                    else
                    {
                        item.MEDICINE_LINE_ID = 0;
                        item.MEDICINE_LINE_CODE = "DTK";
                        item.MEDICINE_LINE_NAME = "Dòng thuốc khác";
                    }
                    if (medicineType != null && medicineType.MEDICINE_GROUP_ID != null)
                    {
                        item.MEDICINE_GROUP_ID = medicineType.MEDICINE_GROUP_ID;
                        item.MEDICINE_GROUP_CODE = medicineType.MEDICINE_GROUP_CODE;
                        item.MEDICINE_GROUP_NAME = medicineType.MEDICINE_GROUP_NAME;
                    }
                    else
                    {
                        item.MEDICINE_GROUP_ID = 0;
                        item.MEDICINE_GROUP_CODE = "NTK";
                        item.MEDICINE_GROUP_NAME = "Nhóm thuốc khác";
                    }
                    if (medicineType != null && medicineType.PARENT_ID != null)
                    {
                        var parentMedicineType = hisMedicineType.FirstOrDefault(o => o.ID == medicineType.PARENT_ID);
                        if (parentMedicineType != null)
                        {
                            item.PARENT_MEDICINE_TYPE_ID = parentMedicineType.ID;
                            item.PARENT_MEDICINE_TYPE_CODE = parentMedicineType.MEDICINE_TYPE_CODE;
                            item.PARENT_MEDICINE_TYPE_NAME = parentMedicineType.MEDICINE_TYPE_NAME;
                        }
                        else
                        {
                            item.PARENT_MEDICINE_TYPE_ID = 0;
                            item.PARENT_MEDICINE_TYPE_CODE = "NTK";
                            item.PARENT_MEDICINE_TYPE_NAME = "Nhóm thuốc khác";
                        }
                    }
                    else
                    {
                        item.PARENT_MEDICINE_TYPE_ID = 0;
                        item.PARENT_MEDICINE_TYPE_CODE = "NTK";
                        item.PARENT_MEDICINE_TYPE_NAME = "Nhóm thuốc khác";
                    }
                }
                if (item.SERVICE_TYPE_ID == 2)
                {
                    item.MEDICINE_LINE_CODE = "DVT";
                    item.MEDICINE_LINE_NAME = "Vật tư";
                    item.MEDICINE_GROUP_CODE = "DVT";
                    item.MEDICINE_GROUP_NAME = "Vật tư";
                    item.PARENT_MEDICINE_TYPE_CODE = "DVT";
                    item.PARENT_MEDICINE_TYPE_NAME = "Vật tư";
                }
            }
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
            dicSingleTag.Add("MEDI_STOCK_NAME", (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == castFilter.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME);
            #endregion
            if (castFilter.IS_MEDICINE == false && castFilter.IS_MATERIAL == true)
            {
                ListRdo = ListRdo.Where(o => o.SERVICE_TYPE_ID == 2).ToList();
            }
            if (castFilter.IS_MEDICINE == true && castFilter.IS_MATERIAL == false)
            {
                ListRdo = ListRdo.Where(o => o.SERVICE_TYPE_ID == 1).ToList();
            }
            ListRdo = ListRdo.OrderBy(o => o.MEDI_STOCK_ID).ThenBy(t1 => t1.SERVICE_TYPE_ID).ThenByDescending(t2 => t2.NUM_ORDER).ThenBy(t3 => t3.SERVICE_NAME).ToList();

            objectTag.AddObjectData(store, "MediStocks", ListRdo.GroupBy(o => o.REPORT_TYPE_CAT_ID).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "Services", ListRdo);
            objectTag.AddRelationship(store, "MediStocks", "Services", "REPORT_TYPE_CAT_ID", "REPORT_TYPE_CAT_ID");

            objectTag.AddObjectData(store, "GrandParent", ListRdo.OrderBy(q => q.PARENT_MEDICINE_TYPE_NAME).GroupBy(o => o.PARENT_MEDICINE_TYPE_ID).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "Parent", ListRdo.GroupBy(o => new { o.MEDICINE_LINE_ID, o.PARENT_MEDICINE_TYPE_ID }).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "GrandParent", "Parent", "PARENT_MEDICINE_TYPE_ID", "PARENT_MEDICINE_TYPE_ID");

            objectTag.AddRelationship(store, "Parent", "Services", new string[] { "MEDICINE_LINE_ID", "PARENT_MEDICINE_TYPE_ID" }, new string[] { "MEDICINE_LINE_ID", "PARENT_MEDICINE_TYPE_ID" });

            objectTag.SetUserFunction(store, "Element", new RDOElement());
            objectTag.SetUserFunction(store, "FuncSameTitleCol", new CustomerFuncMergeSameData());
            objectTag.SetUserFunction(store, "FuncRownumber", new RDOCustomerFuncManyRownumberData());

        }



        //Tính số lượng nhập và xuất thuốc
        private void ProcessAmountMedicine(List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine, List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine)
        {
            try
            {
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
                    ////Inventec.Common.Logging.LogSystem.Info("1:" + hisExpMestMedicine.Where(o => o.MEDICINE_TYPE_CODE == "ACE001").Sum(p => p.AMOUNT));
                    var listExpMestMediSub = new List<V_HIS_EXP_MEST_MEDICINE>();
                    ////Inventec.Common.Logging.LogSystem.Info("1:" + dicExpMestType.Count);
                    foreach (var item in dicExpMestType.Keys)
                    {
                        listExpMestMediSub = hisExpMestMedicine.Where(o => o.EXP_MEST_TYPE_ID == dicExpMestType[item]).ToList();
                        if (!IsNotNullOrEmpty(listExpMestMediSub)) continue;
                        ////Inventec.Common.Logging.LogSystem.Info("1:" + item + listExpMestMediSub.Where(o => o.MEDICINE_TYPE_CODE == "ACE001").Sum(p => p.AMOUNT));
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
                    PropertyInfo p = typeof(VSarReportMrs00179RDO).GetProperty(fieldName);
                    if (!IsNotNull(p)) return;

                    var GroupImps = listImpMestMediSub.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        VSarReportMrs00179RDO rdo = new VSarReportMrs00179RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        var serviceRetyCat = ListServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == listmediSub.First().SERVICE_ID);
                        if (serviceRetyCat != null)
                        {
                            rdo.REPORT_TYPE_CAT_ID = serviceRetyCat.REPORT_TYPE_CAT_ID;
                            rdo.CATEGORY_NAME = serviceRetyCat.CATEGORY_NAME;
                        }
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID;
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE * (1 + listmediSub.First().IMP_VAT_RATIO);
                        rdo.CONCENTRA = listmediSub.First().CONCENTRA;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        rdo.ACTIVE_INGR_BHYT_CODE = listmediSub.First().ACTIVE_INGR_BHYT_CODE;
                        rdo.ACTIVE_INGR_BHYT_NAME = listmediSub.First().ACTIVE_INGR_BHYT_NAME;
                        p.SetValue(rdo, listmediSub.Sum(s => s.AMOUNT));
                        listRdoMedicine.Add(rdo);
                    }
                    //Inventec.Common.Logging.LogSystem.Info("_1listRdoMedicine" + listRdoMedicine.Count);
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
                    PropertyInfo p = typeof(VSarReportMrs00179RDO).GetProperty(fieldName);
                    if (!IsNotNull(p)) return;
                    var GroupImps = listExpMestMediSub.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        VSarReportMrs00179RDO rdo = new VSarReportMrs00179RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        var serviceRetyCat = ListServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == listmediSub.First().SERVICE_ID);
                        if (serviceRetyCat != null)
                        {
                            rdo.REPORT_TYPE_CAT_ID = serviceRetyCat.REPORT_TYPE_CAT_ID; rdo.CATEGORY_NAME = serviceRetyCat.CATEGORY_NAME;
                        }
                        var list1 = listmediSub.Where(o => ExpMestReasonCode(o.EXP_MEST_ID ?? 0)!="").ToList();
                        if (list1.Count > 0)
                        {
                            rdo.DIC_REASON = listmediSub.GroupBy(o => ExpMestReasonCode(o.EXP_MEST_ID ?? 0)).ToDictionary(q => q.Key, q => q.Sum(s => s.AMOUNT));
                            Inventec.Common.Logging.LogSystem.Info("rdo.DIC_REASON" + string.Join(",", rdo.DIC_REASON.Keys));
                        }
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID ?? 0;
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE * (1 + listmediSub.First().IMP_VAT_RATIO);
                        rdo.CONCENTRA = listmediSub.First().CONCENTRA;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        rdo.ACTIVE_INGR_BHYT_CODE = listmediSub.First().ACTIVE_INGR_BHYT_CODE;
                        rdo.ACTIVE_INGR_BHYT_NAME = listmediSub.First().ACTIVE_INGR_BHYT_NAME;
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

        private string ExpMestReasonCode(long expMestId)
        {
            string result = "";
            try
            {
                result = (hisExpMest.FirstOrDefault(o => o.ID == expMestId) ?? new V_HIS_EXP_MEST()).EXP_MEST_REASON_CODE ?? "";
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        //Tính số lượng nhập và xuất vật tư
        private void ProcessAmountMaterial(List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial, List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial)
        {
            try
            {
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
                    var listImpMestMediSub = new List<V_HIS_IMP_MEST_MATERIAL>();
                    foreach (var item in dicImpMestType.Keys)
                    {
                        listImpMestMediSub = hisImpMestMaterial.Where(o => o.IMP_MEST_TYPE_ID == dicImpMestType[item]).ToList();
                        if (!IsNotNullOrEmpty(listImpMestMediSub)) continue;
                        ProcessImpAmountMaterialByImpMestType(listImpMestMediSub, item);
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
                    ////Inventec.Common.Logging.LogSystem.Info("1:" + hisExpMestMaterial.Where(o => o.MATERIAL_TYPE_CODE == "ACE001").Sum(p => p.AMOUNT));
                    var listExpMestMediSub = new List<V_HIS_EXP_MEST_MATERIAL>();
                    ////Inventec.Common.Logging.LogSystem.Info("1:" + dicExpMestType.Count);
                    foreach (var item in dicExpMestType.Keys)
                    {
                        listExpMestMediSub = hisExpMestMaterial.Where(o => o.EXP_MEST_TYPE_ID == dicExpMestType[item]).ToList();
                        if (!IsNotNullOrEmpty(listExpMestMediSub)) continue;
                        ////Inventec.Common.Logging.LogSystem.Info("1:" + item + listExpMestMediSub.Where(o => o.MATERIAL_TYPE_CODE == "ACE001").Sum(p => p.AMOUNT));
                        ProcessExpAmountMaterialByExpMestType(listExpMestMediSub, item);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessImpAmountMaterialByImpMestType(List<V_HIS_IMP_MEST_MATERIAL> listImpMestMediSub, string fieldName)
        {
            try
            {
                if (IsNotNullOrEmpty(listImpMestMediSub))
                {
                    PropertyInfo p = typeof(VSarReportMrs00179RDO).GetProperty(fieldName);
                    if (!IsNotNull(p)) return;

                    var GroupImps = listImpMestMediSub.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listmediSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        VSarReportMrs00179RDO rdo = new VSarReportMrs00179RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        var serviceRetyCat = ListServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == listmediSub.First().SERVICE_ID);
                        if (serviceRetyCat != null)
                        {
                            rdo.REPORT_TYPE_CAT_ID = serviceRetyCat.REPORT_TYPE_CAT_ID;
                            rdo.CATEGORY_NAME = serviceRetyCat.CATEGORY_NAME;
                        }
                        rdo.MEDI_MATE_ID = listmediSub.First().MATERIAL_ID;
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE * (1 + listmediSub.First().IMP_VAT_RATIO);
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        p.SetValue(rdo, listmediSub.Sum(s => s.AMOUNT));
                        listRdoMaterial.Add(rdo);
                    }
                    //Inventec.Common.Logging.LogSystem.Info("_1listRdoMaterial" + listRdoMaterial.Count);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessExpAmountMaterialByExpMestType(List<V_HIS_EXP_MEST_MATERIAL> listExpMestMediSub, string fieldName)
        {
            try
            {
                if (IsNotNullOrEmpty(listExpMestMediSub))
                {
                    PropertyInfo p = typeof(VSarReportMrs00179RDO).GetProperty(fieldName);
                    if (!IsNotNull(p)) return;
                    var GroupImps = listExpMestMediSub.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listmediSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        VSarReportMrs00179RDO rdo = new VSarReportMrs00179RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        var serviceRetyCat = ListServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == listmediSub.First().SERVICE_ID);
                        if (serviceRetyCat != null)
                        {
                            rdo.REPORT_TYPE_CAT_ID = serviceRetyCat.REPORT_TYPE_CAT_ID; rdo.CATEGORY_NAME = serviceRetyCat.CATEGORY_NAME;
                        }
                        var list1 = listmediSub.Where(o => ExpMestReasonCode(o.EXP_MEST_ID ?? 0) != "").ToList();
                        if (list1.Count > 0)
                        {
                            rdo.DIC_REASON = listmediSub.GroupBy(o => ExpMestReasonCode(o.EXP_MEST_ID ?? 0)).ToDictionary(q => q.Key, q => q.Sum(s => s.AMOUNT));
                            Inventec.Common.Logging.LogSystem.Info("rdo.DIC_REASON" + string.Join(",", rdo.DIC_REASON.Keys));
                        }
                        rdo.MEDI_MATE_ID = listmediSub.First().MATERIAL_ID ?? 0;
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE * (1 + listmediSub.First().IMP_VAT_RATIO);
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        p.SetValue(rdo, listmediSub.Sum(s => s.AMOUNT));
                        listRdoMaterial.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //Gop theo id
        private List<VSarReportMrs00179RDO> groupById(List<VSarReportMrs00179RDO> listRdoMedicine)
        {
            List<VSarReportMrs00179RDO> result = new List<VSarReportMrs00179RDO>();
            try
            {
                var group = listRdoMedicine.GroupBy(g => g.MEDI_MATE_ID).ToList();
                Decimal sum = 0;
                VSarReportMrs00179RDO rdo;
                List<VSarReportMrs00179RDO> listSub;
                PropertyInfo[] pi = Properties.Get<VSarReportMrs00179RDO>();
                foreach (var item in group)
                {
                    rdo = new VSarReportMrs00179RDO();
                    listSub = item.ToList<VSarReportMrs00179RDO>();
                    bool hide = true;
                    foreach (var sub in listSub)
                    {
                        if (sub.DIC_REASON.Count == 0) continue;
                        foreach (var r in sub.DIC_REASON)
                        {
                            if (!rdo.DIC_REASON.ContainsKey(r.Key))
                                rdo.DIC_REASON.Add(r.Key, r.Value);
                            else
                                rdo.DIC_REASON[r.Key] += r.Value;
                        }
                    }
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
                            field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s))) ?? new VSarReportMrs00179RDO()));
                        }
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<VSarReportMrs00179RDO>();
            }
            return result;
        }

        //Gop theo gia, theo dich vu, theo nha cung cap
        private List<VSarReportMrs00179RDO> groupByServiceAndPriceAndSupplier(List<VSarReportMrs00179RDO> listRdoMedicine)
        {
            List<VSarReportMrs00179RDO> result = new List<VSarReportMrs00179RDO>();
            try
            {
                var group = listRdoMedicine.GroupBy(g => new { g.SERVICE_ID, g.SUPPLIER_ID, g.IMP_PRICE }).ToList();
                Decimal sum = 0;
                VSarReportMrs00179RDO rdo;
                List<VSarReportMrs00179RDO> listSub;
                PropertyInfo[] pi = Properties.Get<VSarReportMrs00179RDO>();
                foreach (var item in group)
                {
                    rdo = new VSarReportMrs00179RDO();
                    listSub = item.ToList<VSarReportMrs00179RDO>();
                    bool hide = true;
                    foreach (var sub in listSub)
                    {
                        if (sub.DIC_REASON.Count == 0) continue;
                        foreach (var r in sub.DIC_REASON)
                        {
                            if (!rdo.DIC_REASON.ContainsKey(r.Key))
                                rdo.DIC_REASON.Add(r.Key, r.Value);
                            else
                                rdo.DIC_REASON[r.Key] += r.Value;
                        }
                    }
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
                            field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s))) ?? new VSarReportMrs00179RDO()));
                        }
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<VSarReportMrs00179RDO>();
            }
            return result;
        }

        //Gop theo gia, theo dich vu
        private List<VSarReportMrs00179RDO> groupByServiceAndPrice(List<VSarReportMrs00179RDO> listRdoMedicine)
        {
            List<VSarReportMrs00179RDO> result = new List<VSarReportMrs00179RDO>();
            try
            {
                var group = listRdoMedicine.GroupBy(g => new { g.SERVICE_ID, g.IMP_PRICE }).ToList();
                Decimal sum = 0;
                VSarReportMrs00179RDO rdo;
                List<VSarReportMrs00179RDO> listSub;
                PropertyInfo[] pi = Properties.Get<VSarReportMrs00179RDO>();
                foreach (var item in group)
                {
                    rdo = new VSarReportMrs00179RDO();
                    listSub = item.ToList<VSarReportMrs00179RDO>();
                    bool hide = true;
                    foreach (var sub in listSub)
                    {
                        if (sub.DIC_REASON.Count == 0) continue;
                        foreach (var r in sub.DIC_REASON)
                        {
                            if (!rdo.DIC_REASON.ContainsKey(r.Key))
                                rdo.DIC_REASON.Add(r.Key, r.Value);
                            else
                                rdo.DIC_REASON[r.Key] += r.Value;
                        }
                    }
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
                            field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s))) ?? new VSarReportMrs00179RDO()));
                        }
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<VSarReportMrs00179RDO>();
            }
            return result;
        }

        private bool IsMeaningful(object p)
        {
            return (IsNotNull(p) && p.ToString() != "0" && p.ToString() != "");
        }

        //Lay ky du liệu cũ và gần timeFrom nhất
        private void ProcessGetPeriod()
        {
            try
            {
                CommonParam param = new CommonParam();
                HisMediStockPeriodViewFilterQuery periodFilter = new HisMediStockPeriodViewFilterQuery();
                periodFilter.CREATE_TIME_TO = castFilter.TIME_FROM;
                periodFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                List<V_HIS_MEDI_STOCK_PERIOD> hisMediStockPeriod = new MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodManager(param).GetView(periodFilter);

                if (IsNotNullOrEmpty(hisMediStockPeriod))
                {
                    //Trường hợp có kỳ được chốt gần nhất
                    V_HIS_MEDI_STOCK_PERIOD neighborPeriod = hisMediStockPeriod.OrderByDescending(d => d.CREATE_TIME).ToList()[0];
                    ProcessBeinAmountMedicineByMediStockPeriod(param, neighborPeriod);
                    processBeinAmountMaterialByMediStockPeriod(param, neighborPeriod);
                }
                else
                {
                    // Trường hợp không có kỳ chốt gần nhât - chưa được chốt kỳ nào
                    ProcessBeinAmountMedicineNotMediStockPriod(param);
                    ProcessBeinAmountMaterialNotMediStockPriod(param);
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
                List<VSarReportMrs00179RDO> listrdo = new List<VSarReportMrs00179RDO>();
                HisMestPeriodMediViewFilterQuery periodMediFilter = new HisMestPeriodMediViewFilterQuery();
                periodMediFilter.MEDI_STOCK_PERIOD_ID = neighborPeriod.ID;
                List<V_HIS_MEST_PERIOD_MEDI> hisMestPeriodMedi = new MOS.MANAGER.HisMestPeriodMedi.HisMestPeriodMediManager(paramGet).GetView(periodMediFilter);
                if (IsNotNullOrEmpty(hisMestPeriodMedi))
                {
                    foreach (var item in hisMestPeriodMedi)
                    {
                        VSarReportMrs00179RDO rdo = new VSarReportMrs00179RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        var serviceRetyCat = ListServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == item.SERVICE_ID);
                        if (serviceRetyCat != null)
                        {
                            rdo.REPORT_TYPE_CAT_ID = serviceRetyCat.REPORT_TYPE_CAT_ID; rdo.CATEGORY_NAME = serviceRetyCat.CATEGORY_NAME;
                        }
                        rdo.MEDI_MATE_ID = item.MEDICINE_ID;
                        rdo.SERVICE_ID = item.SERVICE_ID;
                        rdo.SERVICE_CODE = item.MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = item.MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = item.SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = item.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = item.SUPPLIER_NAME;
                        rdo.NUM_ORDER = item.NUM_ORDER;
                        rdo.IMP_PRICE = item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.EXPIRED_DATE ?? 0);
                        rdo.BEGIN_AMOUNT = item.AMOUNT;
                        listrdo.Add(rdo);
                    }
                }
                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                impMediFilter.IMP_TIME_FROM = neighborPeriod.CREATE_TIME + 1;
                impMediFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                impMediFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(impMediFilter);
                if (IsNotNullOrEmpty(hisImpMestMedicine))
                {
                    var GroupImps = hisImpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        VSarReportMrs00179RDO rdo = new VSarReportMrs00179RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        var serviceRetyCat = ListServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == listmediSub.First().SERVICE_ID);
                        if (serviceRetyCat != null)
                        {
                            rdo.REPORT_TYPE_CAT_ID = serviceRetyCat.REPORT_TYPE_CAT_ID; rdo.CATEGORY_NAME = serviceRetyCat.CATEGORY_NAME;
                        }
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID;
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE * (1 + listmediSub.First().IMP_VAT_RATIO);
                        rdo.CONCENTRA = listmediSub.First().CONCENTRA;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        rdo.ACTIVE_INGR_BHYT_CODE = listmediSub.First().ACTIVE_INGR_BHYT_CODE;
                        rdo.ACTIVE_INGR_BHYT_NAME = listmediSub.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => s.AMOUNT);
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                expMediFilter.EXP_TIME_FROM = neighborPeriod.CREATE_TIME + 1;
                expMediFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMediFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMediFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(expMediFilter);
                if (IsNotNullOrEmpty(hisExpMestMedicine))
                {
                    var GroupExps = hisExpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        VSarReportMrs00179RDO rdo = new VSarReportMrs00179RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        var serviceRetyCat = ListServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == listmediSub.First().SERVICE_ID);
                        if (serviceRetyCat != null)
                        {
                            rdo.REPORT_TYPE_CAT_ID = serviceRetyCat.REPORT_TYPE_CAT_ID; rdo.CATEGORY_NAME = serviceRetyCat.CATEGORY_NAME;
                        }
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID ?? 0;
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE * (1 + listmediSub.First().IMP_VAT_RATIO);
                        rdo.CONCENTRA = listmediSub.First().CONCENTRA;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        rdo.ACTIVE_INGR_BHYT_CODE = listmediSub.First().ACTIVE_INGR_BHYT_CODE;
                        rdo.ACTIVE_INGR_BHYT_NAME = listmediSub.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => -(s.AMOUNT));
                        listrdo.Add(rdo);
                    }
                }
                listrdo = groupById(listrdo);
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
                List<VSarReportMrs00179RDO> listrdo = new List<VSarReportMrs00179RDO>();
                HisMestPeriodMateViewFilterQuery periodMateFilter = new HisMestPeriodMateViewFilterQuery();
                periodMateFilter.MEDI_STOCK_PERIOD_ID = neighborPeriod.ID;
                List<V_HIS_MEST_PERIOD_MATE> hisMestPeriodMate = new MOS.MANAGER.HisMestPeriodMate.HisMestPeriodMateManager(paramGet).GetView(periodMateFilter);
                if (IsNotNullOrEmpty(hisMestPeriodMate))
                {
                    foreach (var item in hisMestPeriodMate)
                    {
                        VSarReportMrs00179RDO rdo = new VSarReportMrs00179RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        var serviceRetyCat = ListServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == item.SERVICE_ID);
                        if (serviceRetyCat != null)
                        {
                            rdo.REPORT_TYPE_CAT_ID = serviceRetyCat.REPORT_TYPE_CAT_ID; rdo.CATEGORY_NAME = serviceRetyCat.CATEGORY_NAME;
                        }
                        rdo.MEDI_MATE_ID = item.MATERIAL_ID;
                        rdo.SERVICE_ID = item.SERVICE_ID;
                        rdo.SERVICE_CODE = item.MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = item.MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = item.SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = item.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = item.SUPPLIER_NAME;
                        rdo.NUM_ORDER = item.NUM_ORDER;
                        rdo.IMP_PRICE = item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.EXPIRED_DATE ?? 0);
                        rdo.BEGIN_AMOUNT = item.AMOUNT;
                        listrdo.Add(rdo);
                    }
                }
                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                impMateFilter.IMP_TIME_FROM = neighborPeriod.CREATE_TIME + 1;
                impMateFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                impMateFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(impMateFilter);
                if (IsNotNullOrEmpty(hisImpMestMaterial))
                {
                    var GroupImps = hisImpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        VSarReportMrs00179RDO rdo = new VSarReportMrs00179RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        var serviceRetyCat = ListServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == listmateSub.First().SERVICE_ID);
                        if (serviceRetyCat != null)
                        {
                            rdo.REPORT_TYPE_CAT_ID = serviceRetyCat.REPORT_TYPE_CAT_ID; rdo.CATEGORY_NAME = serviceRetyCat.CATEGORY_NAME;
                        }
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID;
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE * (1 + listmateSub.First().IMP_VAT_RATIO);
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmateSub.First().EXPIRED_DATE ?? 0);
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => s.AMOUNT);
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                expMateFilter.EXP_TIME_FROM = neighborPeriod.CREATE_TIME + 1;
                expMateFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMateFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMateFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(paramGet).GetView(expMateFilter);
                if (IsNotNullOrEmpty(hisExpMestMaterial))
                {
                    var GroupExps = hisExpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        VSarReportMrs00179RDO rdo = new VSarReportMrs00179RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        var serviceRetyCat = ListServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == listmateSub.First().SERVICE_ID);
                        if (serviceRetyCat != null)
                        {
                            rdo.REPORT_TYPE_CAT_ID = serviceRetyCat.REPORT_TYPE_CAT_ID; rdo.CATEGORY_NAME = serviceRetyCat.CATEGORY_NAME;
                        }
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID ?? 0;
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE * (1 + listmateSub.First().IMP_VAT_RATIO);
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmateSub.First().EXPIRED_DATE ?? 0);
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => -(s.AMOUNT));
                        listrdo.Add(rdo);
                    }
                }
                listrdo = groupById(listrdo);
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
                List<VSarReportMrs00179RDO> listrdo = new List<VSarReportMrs00179RDO>();

                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                impMediFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                impMediFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager().GetView(impMediFilter);
                if (IsNotNullOrEmpty(hisImpMestMedicine))
                {
                    var GroupImps = hisImpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        VSarReportMrs00179RDO rdo = new VSarReportMrs00179RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        var serviceRetyCat = ListServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == listmediSub.First().SERVICE_ID);
                        if (serviceRetyCat != null)
                        {
                            rdo.REPORT_TYPE_CAT_ID = serviceRetyCat.REPORT_TYPE_CAT_ID; rdo.CATEGORY_NAME = serviceRetyCat.CATEGORY_NAME;
                        }
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID;
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE * (1 + listmediSub.First().IMP_VAT_RATIO);
                        rdo.CONCENTRA = listmediSub.First().CONCENTRA;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        rdo.ACTIVE_INGR_BHYT_CODE = listmediSub.First().ACTIVE_INGR_BHYT_CODE;
                        rdo.ACTIVE_INGR_BHYT_NAME = listmediSub.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => s.AMOUNT);
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                expMediFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMediFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMediFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager().GetView(expMediFilter);
                if (IsNotNullOrEmpty(hisExpMestMedicine))
                {
                    var GroupExps = hisExpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        VSarReportMrs00179RDO rdo = new VSarReportMrs00179RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        var serviceRetyCat = ListServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == listmediSub.First().SERVICE_ID);
                        if (serviceRetyCat != null)
                        {
                            rdo.REPORT_TYPE_CAT_ID = serviceRetyCat.REPORT_TYPE_CAT_ID; rdo.CATEGORY_NAME = serviceRetyCat.CATEGORY_NAME;
                        }
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID ?? 0;
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE * (1 + listmediSub.First().IMP_VAT_RATIO);
                        rdo.CONCENTRA = listmediSub.First().CONCENTRA;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        rdo.ACTIVE_INGR_BHYT_CODE = listmediSub.First().ACTIVE_INGR_BHYT_CODE;
                        rdo.ACTIVE_INGR_BHYT_NAME = listmediSub.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => -(s.AMOUNT));
                        listrdo.Add(rdo);
                    }
                }
                listrdo = groupById(listrdo);
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
                List<VSarReportMrs00179RDO> listrdo = new List<VSarReportMrs00179RDO>();

                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                impMateFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                impMateFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(impMateFilter);
                if (IsNotNullOrEmpty(hisImpMestMaterial))
                {
                    var GroupImps = hisImpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        VSarReportMrs00179RDO rdo = new VSarReportMrs00179RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        var serviceRetyCat = ListServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == listmateSub.First().SERVICE_ID);
                        if (serviceRetyCat != null)
                        {
                            rdo.REPORT_TYPE_CAT_ID = serviceRetyCat.REPORT_TYPE_CAT_ID; rdo.CATEGORY_NAME = serviceRetyCat.CATEGORY_NAME;
                        }
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID;
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE * (1 + listmateSub.First().IMP_VAT_RATIO);
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmateSub.First().EXPIRED_DATE ?? 0);
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => s.AMOUNT);
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                expMateFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMateFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMateFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(paramGet).GetView(expMateFilter);
                if (IsNotNullOrEmpty(hisExpMestMaterial))
                {
                    var GroupExps = hisExpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        VSarReportMrs00179RDO rdo = new VSarReportMrs00179RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        var serviceRetyCat = ListServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == listmateSub.First().SERVICE_ID);
                        if (serviceRetyCat != null)
                        {
                            rdo.REPORT_TYPE_CAT_ID = serviceRetyCat.REPORT_TYPE_CAT_ID; rdo.CATEGORY_NAME = serviceRetyCat.CATEGORY_NAME;
                        }
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID ?? 0;
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE * (1 + listmateSub.First().IMP_VAT_RATIO);
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmateSub.First().EXPIRED_DATE ?? 0);
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => -(s.AMOUNT));
                        listrdo.Add(rdo);
                    }
                }
                listrdo = groupById(listrdo);
                listRdoMaterial.AddRange(listrdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
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