using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMestPeriodMedi;
using MOS.MANAGER.HisMestPeriodMate;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisBid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Inventec.Common.DateTime;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MRS.MANAGER.Config;
using MRS.SDO;
using MOS.Filter;
using MOS.EFMODEL.DataModels;
using FlexCel.Report;

using System.Reflection;
using Inventec.Common.Repository;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisBidMedicineType;
using MRS.MANAGER.Base;
using MOS.MANAGER.HisTreatment;

namespace MRS.Processor.Mrs00185
{
    class Mrs00185Processor : AbstractProcessor
    {
        Mrs00185Filter castFilter = null;

        List<VSarReportMrs00185RDO> ListRdo = new List<VSarReportMrs00185RDO>();

        List<VSarReportMrs00185RDO> listRdoMedicine = new List<VSarReportMrs00185RDO>();
        List<VSarReportMrs00185RDO> listRdoBidToPatient = new List<VSarReportMrs00185RDO>();
        List<VSarReportMrs00185RDO> listRdoMaterial = new List<VSarReportMrs00185RDO>();
        List<V_HIS_MEDI_STOCK> ListMediStock = new List<V_HIS_MEDI_STOCK>();
        List<V_HIS_MEDICINE> ListMedicine = new List<V_HIS_MEDICINE>();
        List<V_HIS_MEDICINE_TYPE> ListMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_BID_MEDICINE_TYPE> ListBidMedicineType = new List<V_HIS_BID_MEDICINE_TYPE>();
        List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();

        Dictionary<string, long> dicExpMestType = new Dictionary<string, long>();
        Dictionary<string, long> dicImpMestType = new Dictionary<string, long>();

        Dictionary<long, HIS_BID> dicHisBid = new Dictionary<long, HIS_BID>();

        V_HIS_MEDI_STOCK CurrentMediStock = new V_HIS_MEDI_STOCK();


        public Mrs00185Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00185Filter);
        }
        protected override bool GetData()
        {
            bool result = false;
            try
            {
                CommonParam paramGet = new CommonParam();
                castFilter = (Mrs00185Filter)this.reportFilter;
                //Danh sach kho
                HisMediStockViewFilterQuery mediStockFilter = new HisMediStockViewFilterQuery();
                mediStockFilter.IDs = castFilter.CURRENTBRANCH_MEDI_STOCK_IDs;
                ListMediStock = new MOS.MANAGER.HisMediStock.HisMediStockManager(paramGet).GetView(mediStockFilter);
                //Danh sach lo thuoc
                HisMedicineViewFilterQuery medicineFilter = new HisMedicineViewFilterQuery();
                ListMedicine = new HisMedicineManager(paramGet).GetView(medicineFilter);
                //Danh sach loai thuoc
                HisMedicineTypeViewFilterQuery medicineTypeFilter = new HisMedicineTypeViewFilterQuery();
                ListMedicineType = new HisMedicineTypeManager(paramGet).GetView(medicineTypeFilter);
                //Danh sach thau
                HisBidMedicineTypeViewFilterQuery bidMedicineTypeFilter = new HisBidMedicineTypeViewFilterQuery();
                bidMedicineTypeFilter.BID_IDs = castFilter.BID_IDs;
                ListBidMedicineType = new HisBidMedicineTypeManager(paramGet).GetView(bidMedicineTypeFilter);
                //Tao loai nhap xuat
                RDOImpExpMestTypeContext.Define(ref dicImpMestType, ref dicExpMestType);
                ProcessListMediStock();

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
                dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
            }
            if (castFilter.TIME_TO > 0)
            {
                dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
            }
            #endregion

            ListRdo = ListRdo.OrderBy(o => o.MEDI_STOCK_ID).ThenBy(t1 => t1.SERVICE_TYPE_ID).ThenByDescending(t2 => t2.NUM_ORDER).ThenBy(t3 => t3.SERVICE_NAME).ToList();
            var listMediStockId = ListRdo.Select(s => s.MEDI_STOCK_ID).Distinct().ToList();
            if (IsNotNullOrEmpty(listMediStockId))
            {
                ListMediStock = ListMediStock.Where(o => listMediStockId.Contains(o.ID)).ToList();
            }
            else
            {
                ListMediStock.Clear();
            }


            objectTag.AddObjectData(store, "BidToPatient", listRdoBidToPatient);
            objectTag.AddObjectData(store, "MediStocks", ListMediStock);
            objectTag.AddObjectData(store, "Services", ListRdo);
            objectTag.AddRelationship(store, "MediStocks", "Services", "ID", "MEDI_STOCK_ID");
            objectTag.SetUserFunction(store, "FuncSameTitleCol", new CustomerFuncMergeSameData());
            objectTag.SetUserFunction(store, "FuncRownumber", new RDOCustomerFuncManyRownumberData());

        }

        private void ProcessListMediStock()
        {
            try
            {
                if (IsNotNullOrEmpty(ListMediStock))
                {
                    CommonParam paramGet = new CommonParam();

                    HisBidFilterQuery bidFilter = new HisBidFilterQuery();
                    dicHisBid = new HisBidManager(paramGet).Get(bidFilter).ToDictionary(o => o.ID);

                    foreach (var medistock in ListMediStock)
                    {
                        CurrentMediStock = medistock;
                        HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                        impMediFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                        impMediFilter.IMP_TIME_TO = castFilter.TIME_TO;
                        impMediFilter.MEDI_STOCK_ID = medistock.ID;
                        impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                        List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager().GetView(impMediFilter);

                        HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                        expMediFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                        expMediFilter.EXP_TIME_TO = castFilter.TIME_TO;
                        expMediFilter.MEDI_STOCK_ID = medistock.ID;
                        expMediFilter.IS_EXPORT = true;
                        List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager().GetView(expMediFilter);

                        HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                        impMateFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                        impMateFilter.IMP_TIME_TO = castFilter.TIME_TO;
                        impMateFilter.MEDI_STOCK_ID = medistock.ID;
                        impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                        List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager().GetView(impMateFilter);

                        HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                        expMateFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                        expMateFilter.EXP_TIME_TO = castFilter.TIME_TO;
                        expMateFilter.MEDI_STOCK_ID = medistock.ID;
                        expMateFilter.IS_EXPORT = true;
                        List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager().GetView(expMateFilter);
                        //Danh sach benh nhan nhan thuoc
                        var treatmentIds = hisExpMestMedicine.Where(o => o.TDL_TREATMENT_ID.HasValue).Select(p => p.TDL_TREATMENT_ID.Value).Distinct().ToList();
                        if (treatmentIds.Count > 0)
                        {
                            var skip = 0;
                            while (treatmentIds.Count - skip > 0)
                            {
                                var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                                HisTreatmentFilterQuery Treatmentfilter = new HisTreatmentFilterQuery();
                                Treatmentfilter.IDs = limit;
                                var TreatmentSub = new HisTreatmentManager().Get(Treatmentfilter);
                                ListTreatment.AddRange(TreatmentSub);
                            }
                        }
                        if (!paramGet.HasException)
                        {
                            ProcessAmountMedicine(hisImpMestMedicine, hisExpMestMedicine);
                            ProcessAmountMaterial(hisImpMestMaterial, hisExpMestMaterial);
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
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00185.");
                        }
                    }
                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00185.");
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
                    Inventec.Common.Logging.LogSystem.Info("1:" + dicExpMestType.Count);
                    foreach (var item in dicExpMestType.Keys)
                    {
                        listExpMestMediSub = hisExpMestMedicine.Where(o => o.EXP_MEST_TYPE_ID == dicExpMestType[item]).ToList();
                        if (!IsNotNullOrEmpty(listExpMestMediSub)) continue;
                        Inventec.Common.Logging.LogSystem.Info("2:");
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
                    PropertyInfo p = typeof(VSarReportMrs00185RDO).GetProperty(fieldName);
                    if (!IsNotNull(p)) return;

                    var GroupImps = listImpMestMediSub.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        VSarReportMrs00185RDO rdo = new VSarReportMrs00185RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
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
                        rdo.BID_ID = listmediSub.First().BID_ID;
                        rdo.BID_NUMBER = listmediSub.First().BID_NUMBER;
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
                    PropertyInfo p = typeof(VSarReportMrs00185RDO).GetProperty(fieldName);
                    if (!IsNotNull(p)) return;

                    var GroupImps = listExpMestMediSub.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        VSarReportMrs00185RDO rdo = new VSarReportMrs00185RDO();
                        V_HIS_MEDICINE medicine = ListMedicine.FirstOrDefault(o => o.ID == listmediSub.First().MEDICINE_ID) ?? new V_HIS_MEDICINE();
                        V_HIS_MEDICINE_TYPE medicineType = ListMedicineType.FirstOrDefault(o => o.ID == listmediSub.First().MEDICINE_TYPE_ID) ?? new V_HIS_MEDICINE_TYPE();
                        List<V_HIS_BID_MEDICINE_TYPE> bidMedicineType = (ListBidMedicineType ?? new List<V_HIS_BID_MEDICINE_TYPE>()).Where(o => o.BID_ID == listmediSub.First().BID_ID && o.MEDICINE_TYPE_ID == listmediSub.First().MEDICINE_TYPE_ID).ToList();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
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
                        rdo.BID_ID = listmediSub.First().BID_ID;
                        rdo.BID_NUMBER = listmediSub.First().BID_NUMBER;
                        p.SetValue(rdo, listmediSub.Sum(s => s.AMOUNT));

                        rdo.ACTIVE_INGR_BHYT_NAME = listmediSub.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.CONCENTRA = listmediSub.First().CONCENTRA;
                        rdo.MEDICINE_USE_FORM_NAME = medicineType.MEDICINE_USE_FORM_NAME;
                        rdo.MANUFACTURER_NAME = listmediSub.First().MANUFACTURER_NAME;
                        rdo.NATIONAL_NAME = listmediSub.First().NATIONAL_NAME;
                        rdo.PACKING_TYPE_NAME = medicineType.PACKING_TYPE_NAME;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        rdo.REGISTER_NUMBER = listmediSub.First().REGISTER_NUMBER;
                        rdo.BID_NAME = listmediSub.First().BID_NAME;
                        rdo.BID_AMOUNT = bidMedicineType.Sum(s => s.AMOUNT);
                        rdo.BHYT_AMOUNT_BH = listmediSub.Where(o => o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && ListTreatment.Exists(q => q.ID == o.TDL_TREATMENT_ID && q.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)).Sum(s => s.AMOUNT);
                        rdo.BHYT_AMOUNT_VP = listmediSub.Where(o => o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && ListTreatment.Exists(q => q.ID == o.TDL_TREATMENT_ID && q.TDL_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)).Sum(s => s.AMOUNT);
                        rdo.VP_AMOUNT = listmediSub.Where(o => o.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(s => s.AMOUNT);
                        listRdoMedicine.Add(rdo);
                        if (rdo.BID_AMOUNT > 0)
                        {
                            listRdoBidToPatient.Add(rdo);
                        }
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
                    PropertyInfo p = typeof(VSarReportMrs00185RDO).GetProperty(fieldName);
                    if (!IsNotNull(p)) return;
                    var GroupImps = listImpMestMateSub.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        VSarReportMrs00185RDO rdo = new VSarReportMrs00185RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
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
                        rdo.BID_ID = listmateSub.First().BID_ID;
                        rdo.BID_NUMBER = listmateSub.First().BID_NUMBER;
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
                    PropertyInfo p = typeof(VSarReportMrs00185RDO).GetProperty(fieldName);
                    if (!IsNotNull(p)) return;

                    var GroupImps = listExpMestMateSub.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        VSarReportMrs00185RDO rdo = new VSarReportMrs00185RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
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
                        rdo.BID_ID = listmateSub.First().BID_ID;
                        rdo.BID_NUMBER = listmateSub.First().BID_NUMBER;
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
                List<VSarReportMrs00185RDO> listrdo = new List<VSarReportMrs00185RDO>();
                HisMestPeriodMediViewFilterQuery periodMediFilter = new HisMestPeriodMediViewFilterQuery();
                periodMediFilter.MEDI_STOCK_PERIOD_ID = neighborPeriod.ID;
                List<V_HIS_MEST_PERIOD_MEDI> hisMestPeriodMedi = new MOS.MANAGER.HisMestPeriodMedi.HisMestPeriodMediManager(paramGet).GetView(periodMediFilter);
                if (IsNotNullOrEmpty(hisMestPeriodMedi))
                {
                    foreach (var item in hisMestPeriodMedi)
                    {
                        VSarReportMrs00185RDO rdo = new VSarReportMrs00185RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
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
                        if (item.BID_ID.HasValue)
                        {
                            rdo.BID_ID = item.BID_ID;
                            rdo.BID_NUMBER = dicHisBid.ContainsKey(item.BID_ID.Value) ? dicHisBid[item.BID_ID.Value].BID_NUMBER : "";
                        }
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
                        VSarReportMrs00185RDO rdo = new VSarReportMrs00185RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
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
                        rdo.BID_ID = listmediSub.First().BID_ID;
                        rdo.BID_NUMBER = listmediSub.First().BID_NUMBER;
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
                        VSarReportMrs00185RDO rdo = new VSarReportMrs00185RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
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
                        rdo.BID_ID = listmediSub.First().BID_ID;
                        rdo.BID_NUMBER = listmediSub.First().BID_NUMBER;
                        listrdo.Add(rdo);
                    }
                }
                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s => new VSarReportMrs00185RDO { SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME, SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID, MEDI_STOCK_ID = s.First().MEDI_STOCK_ID, MEDI_MATE_ID = s.First().MEDI_MATE_ID, SUPPLIER_ID = s.First().SUPPLIER_ID, SUPPLIER_CODE = s.First().SUPPLIER_CODE, SUPPLIER_NAME = s.First().SUPPLIER_NAME, IMP_PRICE = s.First().IMP_PRICE, BID_ID = s.First().BID_ID, BID_NUMBER = s.First().BID_NUMBER, SERVICE_ID = s.First().SERVICE_ID, SERVICE_CODE = s.First().SERVICE_CODE, SERVICE_NAME = s.First().SERVICE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, NUM_ORDER = s.First().NUM_ORDER, BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT) }).ToList();
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
                List<VSarReportMrs00185RDO> listrdo = new List<VSarReportMrs00185RDO>();
                HisMestPeriodMateViewFilterQuery periodMateFilter = new HisMestPeriodMateViewFilterQuery();
                periodMateFilter.MEDI_STOCK_PERIOD_ID = neighborPeriod.ID;
                List<V_HIS_MEST_PERIOD_MATE> hisMestPeriodMate = new MOS.MANAGER.HisMestPeriodMate.HisMestPeriodMateManager(paramGet).GetView(periodMateFilter);
                if (IsNotNullOrEmpty(hisMestPeriodMate))
                {
                    foreach (var item in hisMestPeriodMate)
                    {
                        VSarReportMrs00185RDO rdo = new VSarReportMrs00185RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
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
                        if (item.BID_ID.HasValue)
                        {
                            rdo.BID_ID = item.BID_ID;
                            rdo.BID_NUMBER = dicHisBid.ContainsKey(item.BID_ID.Value) ? dicHisBid[item.BID_ID.Value].BID_NUMBER : "";
                        }
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
                        VSarReportMrs00185RDO rdo = new VSarReportMrs00185RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
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
                        rdo.BID_ID = listmateSub.First().BID_ID;
                        rdo.BID_NUMBER = listmateSub.First().BID_NUMBER;
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
                        VSarReportMrs00185RDO rdo = new VSarReportMrs00185RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
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
                        rdo.BID_ID = listmateSub.First().BID_ID;
                        rdo.BID_NUMBER = listmateSub.First().BID_NUMBER;
                        listrdo.Add(rdo);
                    }
                }
                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s => new VSarReportMrs00185RDO { SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME, SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID, MEDI_STOCK_ID = s.First().MEDI_STOCK_ID, MEDI_MATE_ID = s.First().MEDI_MATE_ID, SUPPLIER_ID = s.First().SUPPLIER_ID, SUPPLIER_CODE = s.First().SUPPLIER_CODE, SUPPLIER_NAME = s.First().SUPPLIER_NAME, IMP_PRICE = s.First().IMP_PRICE, BID_ID = s.First().BID_ID, BID_NUMBER = s.First().BID_NUMBER, SERVICE_ID = s.First().SERVICE_ID, SERVICE_CODE = s.First().SERVICE_CODE, SERVICE_NAME = s.First().SERVICE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, NUM_ORDER = s.First().NUM_ORDER, BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT) }).ToList();
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
                List<VSarReportMrs00185RDO> listrdo = new List<VSarReportMrs00185RDO>();

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
                        VSarReportMrs00185RDO rdo = new VSarReportMrs00185RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
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
                        rdo.BID_ID = listmediSub.First().BID_ID;
                        rdo.BID_NUMBER = listmediSub.First().BID_NUMBER;
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                expMediFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMediFilter.MEDI_STOCK_IDs = new List<long>(){CurrentMediStock.ID};
                expMediFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new ManagerSql().GetView(expMediFilter);
                if (IsNotNullOrEmpty(hisExpMestMedicine))
                {
                    var GroupExps = hisExpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        VSarReportMrs00185RDO rdo = new VSarReportMrs00185RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
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
                        rdo.BID_ID = listmediSub.First().BID_ID;
                        rdo.BID_NUMBER = listmediSub.First().BID_NUMBER;
                        listrdo.Add(rdo);
                    }
                }
                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s => new VSarReportMrs00185RDO { SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME, SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID, MEDI_STOCK_ID = s.First().MEDI_STOCK_ID, MEDI_MATE_ID = s.First().MEDI_MATE_ID, SUPPLIER_ID = s.First().SUPPLIER_ID, SUPPLIER_CODE = s.First().SUPPLIER_CODE, SUPPLIER_NAME = s.First().SUPPLIER_NAME, IMP_PRICE = s.First().IMP_PRICE, BID_ID = s.First().BID_ID, BID_NUMBER = s.First().BID_NUMBER, SERVICE_ID = s.First().SERVICE_ID, SERVICE_CODE = s.First().SERVICE_CODE, SERVICE_NAME = s.First().SERVICE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, NUM_ORDER = s.First().NUM_ORDER, BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT) }).ToList();
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
                List<VSarReportMrs00185RDO> listrdo = new List<VSarReportMrs00185RDO>();

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
                        VSarReportMrs00185RDO rdo = new VSarReportMrs00185RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
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
                        rdo.BID_ID = listmateSub.First().BID_ID;
                        rdo.BID_NUMBER = listmateSub.First().BID_NUMBER;
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                expMateFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMateFilter.MEDI_STOCK_IDs = new List<long>(){CurrentMediStock.ID};
                expMateFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new ManagerSql().GetView(expMateFilter);
                if (IsNotNullOrEmpty(hisExpMestMaterial))
                {
                    var GroupExps = hisExpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        VSarReportMrs00185RDO rdo = new VSarReportMrs00185RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
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
                        rdo.BID_ID = listmateSub.First().BID_ID;
                        rdo.BID_NUMBER = listmateSub.First().BID_NUMBER;
                        listrdo.Add(rdo);
                    }
                }
                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s => new VSarReportMrs00185RDO { SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME, SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID, MEDI_STOCK_ID = s.First().MEDI_STOCK_ID, MEDI_MATE_ID = s.First().MEDI_MATE_ID, SUPPLIER_ID = s.First().SUPPLIER_ID, SUPPLIER_CODE = s.First().SUPPLIER_CODE, SUPPLIER_NAME = s.First().SUPPLIER_NAME, IMP_PRICE = s.First().IMP_PRICE, BID_ID = s.First().BID_ID, BID_NUMBER = s.First().BID_NUMBER, SERVICE_ID = s.First().SERVICE_ID, SERVICE_CODE = s.First().SERVICE_CODE, SERVICE_NAME = s.First().SERVICE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, NUM_ORDER = s.First().NUM_ORDER, BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT) }).ToList();
                listRdoMaterial.AddRange(listrdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        //Gop theo id
        private List<VSarReportMrs00185RDO> groupById(List<VSarReportMrs00185RDO> listRdoMedicine)
        {
            List<VSarReportMrs00185RDO> result = new List<VSarReportMrs00185RDO>();
            try
            {
                var group = listRdoMedicine.GroupBy(g => g.MEDI_MATE_ID).ToList();
                Decimal sum = 0;
                VSarReportMrs00185RDO rdo;
                List<VSarReportMrs00185RDO> listSub;
                PropertyInfo[] pi = Properties.Get<VSarReportMrs00185RDO>();
                foreach (var item in group)
                {
                    rdo = new VSarReportMrs00185RDO();
                    listSub = item.ToList<VSarReportMrs00185RDO>();

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
                            field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s))) ?? new VSarReportMrs00185RDO()));
                        }
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<VSarReportMrs00185RDO>();
            }
            return result;
        }
        //Gop theo gia, theo dich vu, theo nha cung cap
        private List<VSarReportMrs00185RDO> groupByServiceAndPriceAndSupplier(List<VSarReportMrs00185RDO> listRdoMedicine)
        {
            List<VSarReportMrs00185RDO> result = new List<VSarReportMrs00185RDO>();
            try
            {
                var group = listRdoMedicine.GroupBy(g => new { g.SERVICE_ID, g.SUPPLIER_ID, g.IMP_PRICE }).ToList();
                Decimal sum = 0;
                VSarReportMrs00185RDO rdo;
                List<VSarReportMrs00185RDO> listSub;
                PropertyInfo[] pi = Properties.Get<VSarReportMrs00185RDO>();
                foreach (var item in group)
                {
                    rdo = new VSarReportMrs00185RDO();
                    listSub = item.ToList<VSarReportMrs00185RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        if (field.Name.Contains("_AMOUNT"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum > 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s))) ?? new VSarReportMrs00185RDO()));
                        }
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<VSarReportMrs00185RDO>();
            }
            return result;
        }

        //Gop theo gia, theo dich vu
        private List<VSarReportMrs00185RDO> groupByServiceAndPrice(List<VSarReportMrs00185RDO> listRdoMedicine)
        {
            List<VSarReportMrs00185RDO> result = new List<VSarReportMrs00185RDO>();
            try
            {
                var group = listRdoMedicine.GroupBy(g => new { g.SERVICE_ID, g.IMP_PRICE }).ToList();
                Decimal sum = 0;
                VSarReportMrs00185RDO rdo;
                List<VSarReportMrs00185RDO> listSub;
                PropertyInfo[] pi = Properties.Get<VSarReportMrs00185RDO>();
                foreach (var item in group)
                {
                    rdo = new VSarReportMrs00185RDO();
                    listSub = item.ToList<VSarReportMrs00185RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        if (field.Name.Contains("_AMOUNT"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum > 0) hide = false;
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
                return new List<VSarReportMrs00185RDO>();
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
                long mediId = System.Convert.ToInt64(parameters[0]);
                int ServiceId = System.Convert.ToInt32(parameters[1]);

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
                long mediId = System.Convert.ToInt64(parameters[0]);
                int ServiceId = System.Convert.ToInt32(parameters[1]);

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
