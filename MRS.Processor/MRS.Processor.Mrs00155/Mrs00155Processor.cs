using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestType;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisImpMestMedicine;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FlexCel.Report;
using Inventec.Common.DateTime;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.SDO;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMaterialType;

namespace MRS.Processor.Mrs00155
{
    internal class Mrs00155Processor : AbstractProcessor
    {
        List<VSarReportMrs00155RDO> _listSarReportMrs00155Rdos = new List<VSarReportMrs00155RDO>();
        Mrs00155Filter CastFilter;
        private string MEDI_STOCK_NAME;
        private string IMP_MEST_TYPE_NAME;

        Dictionary<string, ImpMestTypeRDO> dicImpMestTypeRdo = new Dictionary<string, ImpMestTypeRDO>();

        public Mrs00155Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00155Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                CastFilter = (Mrs00155Filter)this.reportFilter;
                if (CastFilter.IS_MEDICINE != true && CastFilter.IS_MATERIAL != true /*&& CastFilter.IS_CHEMICAL_SUBSTANCE != true*/)
                {
                    CastFilter.IS_MEDICINE = true;
                    CastFilter.IS_MATERIAL = true;
                    //CastFilter.IS_CHEMICAL_SUBSTANCE = true;
                }
                if (CastFilter.INPUT_DATA_ID_IMT_TYPEs != null)
                {
                    CastFilter.IMP_MEST_TYPE_IDs = new List<long>();
                    CastFilter.IMP_MEST_TYPE_IDs.AddRange(CastFilter.INPUT_DATA_ID_IMT_TYPEs);
                    if (CastFilter.IMP_MEST_TYPE_IDs.Contains(19/*bổ sung cơ số*/) && !CastFilter.IMP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK))
                    {
                        CastFilter.IMP_MEST_TYPE_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK);
                    }
                }
                var paramGet = new CommonParam();

                var mediStockViews = new MOS.MANAGER.HisMediStock.HisMediStockManager(paramGet).GetById(CastFilter.MEDI_STOCK_ID??0)??new HIS_MEDI_STOCK();
                MEDI_STOCK_NAME = mediStockViews.MEDI_STOCK_NAME;
                //--------------------------------------------------------------------------------------------------
                var metyFilterImpMestType = new HisImpMestTypeFilterQuery
                {
                    ID = CastFilter.IMP_MEST_TYPE_ID
                };
                var impMestType = new MOS.MANAGER.HisImpMestType.HisImpMestTypeManager(paramGet).Get(metyFilterImpMestType);
                if (impMestType != null && impMestType.Count == 1)
                {
                    IMP_MEST_TYPE_NAME = impMestType.First().IMP_MEST_TYPE_NAME;
                }
                //--------------------------------------------------------------------------------------------------V_HIS_IMP_MEST
                var metyFilterImpMest = new HisImpMestViewFilterQuery
                {
                    IMP_TIME_FROM = CastFilter.DATE_FROM,
                    IMP_TIME_TO = CastFilter.DATE_TO,
                    MEDI_STOCK_ID = CastFilter.MEDI_STOCK_ID,
                    MEDI_STOCK_IDs = CastFilter.MEDI_STOCK_IDs,
                    IMP_MEST_TYPE_ID = CastFilter.IMP_MEST_TYPE_ID,
                    IMP_MEST_TYPE_IDs = CastFilter.IMP_MEST_TYPE_IDs
                };
                var listImpMestViews = new MOS.MANAGER.HisImpMest.HisImpMestManager(paramGet).GetView(metyFilterImpMest);
                if (CastFilter.IMP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK))
                {
                    if (!CastFilter.INPUT_DATA_ID_IMT_TYPEs.Contains(19/*bổ sung cơ số*/))
                    {
                        listImpMestViews = listImpMestViews.Where(o => o.CHMS_TYPE_ID == null).ToList();
                    }
                    if (!CastFilter.INPUT_DATA_ID_IMT_TYPEs.Contains(IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK))
                    {
                        listImpMestViews = listImpMestViews.Where(o => o.IMP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK || o.CHMS_TYPE_ID != null).ToList();
                    }
                }
                ////Loai thuoc, vat tu
                //GetMedicineTypeMaterialType();

                var listImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();
                //var listImpMestChemicalSubstance = new List<V_HIS_IMP_MEST_MATERIAL>();
                if (CastFilter.IS_MATERIAL == true /*|| CastFilter.IS_CHEMICAL_SUBSTANCE == true*/)
                {
                    //--------------------------------------------------------------------------------------------------Select
                    var listImpMestIds = listImpMestViews.Select(s => s.ID).ToList();
                    var skip = 0;
                    while (listImpMestIds.Count - skip > 0)
                    {
                        var listIds = listImpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var metyFilterImpMestMaterial = new HisImpMestMaterialViewFilterQuery
                        {
                            IMP_MEST_IDs = listIds,
                        };
                        var impMestMaterialViews = new HisImpMestMaterialManager(paramGet).GetView(metyFilterImpMestMaterial);
                        listImpMestMaterial.AddRange(impMestMaterialViews);
                    }
                }
                var listImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
                if (CastFilter.IS_MEDICINE == true)
                {
                    //--------------------------------------------------------------------------------------------------Select
                    var listImpMestIds = listImpMestViews.Select(s => s.ID).ToList();
                    var skip = 0;
                    while (listImpMestIds.Count - skip > 0)
                    {
                        var listIds = listImpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var metyFilterImpMestMedicine = new HisImpMestMedicineViewFilterQuery
                        {
                            IMP_MEST_IDs = listIds,
                        };
                        var impMestMedicineViews = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(metyFilterImpMestMedicine);
                        listImpMestMedicine.AddRange(impMestMedicineViews);
                    }
                }

                //--------------------------------------------------------------------------------------------------

                ProcessFilterData(listImpMestMedicine);

                //--------------------------------------------------------------------------------------------------

                ProcessFilterData(listImpMestMaterial);

                //--------------------------------------------------------------------------------------------------

                ProcessGroup(listImpMestMedicine, listImpMestViews);

                //--------------------------------------------------------------------------------------------------

                ProcessGroup(listImpMestMaterial, listImpMestViews);

                //thêm thông tin số phiếu cho mỗi loại
                AddCountImpMest(listImpMestViews);

                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void AddCountImpMest(List<V_HIS_IMP_MEST> listImpMestViews)
        {
            string KeyGroupImp = "{0}_{1}_{2}";
            if (this.dicDataFilter.ContainsKey("KEY_GROUP_IMP") && this.dicDataFilter["KEY_GROUP_IMP"] != null)
            {
                KeyGroupImp = this.dicDataFilter["KEY_GROUP_IMP"].ToString();
            }
            //thêm thông tin số phiếu từng loại nhập
            foreach (var item in dicImpMestTypeRdo.Keys)
            {
                var impMest = listImpMestViews.Where(o => string.Format(KeyGroupImp, o.IMP_MEST_TYPE_ID, o.MEDI_STOCK_ID, o.CHMS_MEDI_STOCK_ID ?? 0) == item).ToList();
                dicImpMestTypeRdo[item].COUNT_IMP_MEST = impMest.Count();
            }
        }


        //private void GetMedicineTypeMaterialType()
        //{
        //    CommonParam paramGet = new CommonParam();
        //    var ListMedicineType = new HisMedicineTypeManager(paramGet).GetView(new HisMedicineTypeViewFilterQuery());
        //    var ListMaterialType = new HisMaterialTypeManager(paramGet).GetView(new HisMaterialTypeViewFilterQuery());
        //    if (IsNotNullOrEmpty(ListMedicineType))
        //    {
        //        foreach (var item in ListMedicineType)
        //        {
        //            dicMedicineType[item.ID] = item;
        //        }
        //    }

        //    if (IsNotNullOrEmpty(ListMaterialType))
        //    {
        //        foreach (var item in ListMaterialType)
        //        {
        //            dicMaterialType[item.ID] = item;
        //        }
        //    }

        //}

        protected override bool ProcessData()
        {
            return true;
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_FROM));
            dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TO));
            dicSingleTag.Add("MEDI_STOCK_NAME", MEDI_STOCK_NAME);
            dicSingleTag.Add("IMP_MEST_TYPE_NAME", IMP_MEST_TYPE_NAME);
            dicSingleTag.Add("TOTAL_ALL_PRICE", _listSarReportMrs00155Rdos.Sum(s => s.TOTAL_PRICE));
            dicSingleTag.Add("TOTAL_ALL_PRICE_TO_STRING", Inventec.Common.String.Convert.CurrencyToVneseString(_listSarReportMrs00155Rdos.Sum(s => s.TOTAL_PRICE).ToString()));

            objectTag.AddObjectData(store, "Report", _listSarReportMrs00155Rdos);
            objectTag.AddObjectData(store, "ImpMestTypeRdo", dicImpMestTypeRdo.Values.OrderBy(o=>o.IMP_MEST_TYPE_CODE).ToList());
            objectTag.SetUserFunction(store, "FuncSameTitleRow", new CustomerFuncMergeSameDataExpTime(_listSarReportMrs00155Rdos));

        }


        private void ProcessFilterData(List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicineViews)
        {
            try
            {
                LogSystem.Info("Bat dau xu ly du lieu MRS00155 ===============================================================");
                var listMedicineGroupByNames = listImpMestMedicineViews.GroupBy(s => s.MEDICINE_TYPE_NAME).ToList();
                foreach (var listMedicineGroupByName in listMedicineGroupByNames.OrderBy(s => s.Key))
                {
                    var listMedicineGroupByPrices = listMedicineGroupByName.GroupBy(s => s.IMP_PRICE * (1 + s.IMP_VAT_RATIO)).ToList();
                    foreach (var listImpMestMedicinePrice in listMedicineGroupByPrices.OrderBy(s => s.Key))
                    {
                        var listMedicineGroupByNationals = listImpMestMedicinePrice.GroupBy(s => s.NATIONAL_NAME).ToList();
                        foreach (var listMedicineGroupByNational in listMedicineGroupByNationals.OrderBy(s => s.Key))
                        {
                            var price = listMedicineGroupByNational.First().IMP_PRICE * (1 + listMedicineGroupByNational.First().IMP_VAT_RATIO);
                            var amount = listMedicineGroupByNational.Sum(s => s.AMOUNT);
                            var rdo = new VSarReportMrs00155RDO
                            {
                                SERVICE_TYPE = "THUOC",
                                MEDICINE_TYPE_NAME = listMedicineGroupByNational.First().MEDICINE_TYPE_NAME,
                                SERVICE_UNIT_NAME = listMedicineGroupByNational.First().SERVICE_UNIT_NAME,
                                NATIONAL_NAME = listMedicineGroupByNational.First().NATIONAL_NAME,
                                PRICE = price,
                                AMOUNT = amount,
                                TOTAL_PRICE = price * amount
                            };
                            _listSarReportMrs00155Rdos.Add(rdo);
                        }
                    }
                }
                LogSystem.Info("Ket thuc xu ly du lieu MRS00155 ===============================================================");
            }
            catch (Exception ex)
            {
                LogSystem.Info("Loi trong qua trinh xu ly du lieu ===============================================================");
                LogSystem.Error(ex);
            }
        }


        private void ProcessFilterData(List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterialViews)
        {
            try
            {
                LogSystem.Info("Bat dau xu ly du lieu MRS00155 ===============================================================");
                var listMedicineGroupByNames = listImpMestMaterialViews.GroupBy(s => s.MATERIAL_TYPE_NAME).ToList();
                foreach (var listMedicineGroupByName in listMedicineGroupByNames.OrderBy(s => s.Key))
                {
                    var listMedicineGroupByPrices = listMedicineGroupByName.GroupBy(s => s.IMP_PRICE * (1 + s.IMP_VAT_RATIO)).ToList();
                    foreach (var listImpMestMedicinePrice in listMedicineGroupByPrices.OrderBy(s => s.Key))
                    {
                        var listMedicineGroupByNationals = listImpMestMedicinePrice.GroupBy(s => s.NATIONAL_NAME).ToList();
                        foreach (var listMedicineGroupByNational in listMedicineGroupByNationals.OrderBy(s => s.Key))
                        {
                            var price = listMedicineGroupByNational.First().IMP_PRICE * (1 + listMedicineGroupByNational.First().IMP_VAT_RATIO);
                            var amount = listMedicineGroupByNational.Sum(s => s.AMOUNT);
                            var rdo = new VSarReportMrs00155RDO
                            {
                                SERVICE_TYPE = "VATTU",
                                MEDICINE_TYPE_NAME = listMedicineGroupByNational.First().MATERIAL_TYPE_NAME,
                                SERVICE_UNIT_NAME = listMedicineGroupByNational.First().SERVICE_UNIT_NAME,
                                NATIONAL_NAME = listMedicineGroupByNational.First().NATIONAL_NAME,
                                PRICE = price,
                                AMOUNT = amount,
                                TOTAL_PRICE = price * amount
                            };
                            _listSarReportMrs00155Rdos.Add(rdo);
                        }
                    }
                }
                LogSystem.Info("Ket thuc xu ly du lieu MRS00155 ===============================================================");
            }
            catch (Exception ex)
            {
                LogSystem.Info("Loi trong qua trinh xu ly du lieu ===============================================================");
                LogSystem.Error(ex);
            }
        }


        private void ProcessGroup(List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicineViews, List<V_HIS_IMP_MEST> listImpMestViews)
        {
            try
            {
                string KeyGroupImp = "{0}_{1}_{2}";
                if (this.dicDataFilter.ContainsKey("KEY_GROUP_IMP") && this.dicDataFilter["KEY_GROUP_IMP"] != null)
                {
                    KeyGroupImp = this.dicDataFilter["KEY_GROUP_IMP"].ToString();
                }
                foreach (var item in listImpMestMedicineViews)
                {
                    var impMest = listImpMestViews.FirstOrDefault(o => o.ID == item.IMP_MEST_ID) ?? new V_HIS_IMP_MEST();
                    string Key = string.Format(KeyGroupImp, item.IMP_MEST_TYPE_ID, item.MEDI_STOCK_ID, impMest.CHMS_MEDI_STOCK_ID ?? 0);
                    if (!dicImpMestTypeRdo.ContainsKey(Key))
                    {
                        ImpMestTypeRDO rdo = new ImpMestTypeRDO();
                        rdo.IMP_MEST_TYPE_CODE = impMest.IMP_MEST_TYPE_CODE;
                        rdo.IMP_MEST_TYPE_NAME = impMest.IMP_MEST_TYPE_NAME;
                        rdo.MEDI_STOCK_CODE = impMest.MEDI_STOCK_CODE;
                        rdo.MEDI_STOCK_NAME = impMest.MEDI_STOCK_NAME;
                        var chmsMediStock = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == impMest.CHMS_MEDI_STOCK_ID);
                        if (chmsMediStock != null)
                        {
                            rdo.CHMS_MEDI_STOCK_CODE = chmsMediStock.MEDI_STOCK_CODE;
                            rdo.CHMS_MEDI_STOCK_NAME = chmsMediStock.MEDI_STOCK_NAME;
                        }
                        dicImpMestTypeRdo[Key] = rdo;
                    }
                    dicImpMestTypeRdo[Key].AMOUNT += item.AMOUNT;

                    dicImpMestTypeRdo[Key].TOTAL_PRICE += item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                }

                
            }
            catch (Exception ex)
            {
                LogSystem.Info("Loi trong qua trinh xu ly du lieu ===============================================================");
                LogSystem.Error(ex);
            }
        }

        private void ProcessGroup(List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterialViews, List<V_HIS_IMP_MEST> listImpMestViews)
        {
            try
            {
                string KeyGroupImp = "{0}_{1}_{2}";
                if (this.dicDataFilter.ContainsKey("KEY_GROUP_IMP") && this.dicDataFilter["KEY_GROUP_IMP"] != null)
                {
                    KeyGroupImp = this.dicDataFilter["KEY_GROUP_IMP"].ToString();
                }
                foreach (var item in listImpMestMaterialViews)
                {
                    var impMest = listImpMestViews.FirstOrDefault(o => o.ID == item.IMP_MEST_ID) ?? new V_HIS_IMP_MEST();
                    string Key = string.Format(KeyGroupImp, item.IMP_MEST_TYPE_ID, item.MEDI_STOCK_ID, impMest.CHMS_MEDI_STOCK_ID ?? 0);
                    if (!dicImpMestTypeRdo.ContainsKey(Key))
                    {
                        ImpMestTypeRDO rdo = new ImpMestTypeRDO();
                        rdo.IMP_MEST_TYPE_CODE = impMest.IMP_MEST_TYPE_CODE;
                        rdo.IMP_MEST_TYPE_NAME = impMest.IMP_MEST_TYPE_NAME;
                        rdo.MEDI_STOCK_CODE = impMest.MEDI_STOCK_CODE;
                        rdo.MEDI_STOCK_NAME = impMest.MEDI_STOCK_NAME;
                        var chmsMediStock = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == impMest.CHMS_MEDI_STOCK_ID);
                        if (chmsMediStock != null)
                        {
                            rdo.CHMS_MEDI_STOCK_CODE = chmsMediStock.MEDI_STOCK_CODE;
                            rdo.CHMS_MEDI_STOCK_NAME = chmsMediStock.MEDI_STOCK_NAME;
                        }
                        dicImpMestTypeRdo[Key] = rdo;
                    }
                    dicImpMestTypeRdo[Key].AMOUNT += item.AMOUNT;

                    dicImpMestTypeRdo[Key].TOTAL_PRICE += item.AMOUNT * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                }

            }
            catch (Exception ex)
            {
                LogSystem.Info("Loi trong qua trinh xu ly du lieu ===============================================================");
                LogSystem.Error(ex);
            }
        }

        class CustomerFuncMergeSameDataExpTime : TFlexCelUserFunction
        {
            List<VSarReportMrs00155RDO> sereServRdos;
            int SameType;
            public CustomerFuncMergeSameDataExpTime(List<VSarReportMrs00155RDO> sereServRdos)
            {
                this.sereServRdos = sereServRdos;
            }
            public override object Evaluate(object[] parameters)
            {
                bool result = false;
                try
                {
                    if (parameters == null || parameters.Length < 1 || sereServRdos == null || sereServRdos.Count == 0)
                        throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

                    string currentValue = "";
                    string nextValue = "";

                    int currentIdx = (int)parameters[0];

                    currentValue = sereServRdos[currentIdx].MEDICINE_TYPE_NAME;
                    if (currentIdx + 1 < sereServRdos.Count)
                    {
                        nextValue = sereServRdos[currentIdx + 1].MEDICINE_TYPE_NAME;

                        if (!String.IsNullOrEmpty((currentValue))
                        && !String.IsNullOrEmpty((nextValue))
                        && currentValue.Equals((nextValue)))
                        {
                            result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                }

                return result;
            }
        }
    }
}
