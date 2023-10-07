using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisImpMestMedicine;
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
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.SDO;
using MRS.MANAGER.Core.MrsReport;

namespace MRS.Processor.Mrs00157
{
    internal class Mrs00157Processor : AbstractProcessor
    {
        List<VSarReportMrs00157RDO> _listSarReportMrs00157Rdos = new List<VSarReportMrs00157RDO>();
        List<VSarReportMrs00157RDO> _listSarReportMrs00157Rdos1 = new List<VSarReportMrs00157RDO>();
        List<V_HIS_MEDICINE_TYPE> listParent = new List<V_HIS_MEDICINE_TYPE>();
        Mrs00157Filter CastFilter;
        private string MEDI_STOCK_NAME;

        public Mrs00157Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00157Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                var paramGet = new CommonParam();
                CastFilter = (Mrs00157Filter)this.reportFilter;

                var medistockFilter = new HisMediStockViewFilterQuery
                {
                    IDs = CastFilter.MEDI_STOCK_IDs, //new List<long> { 22 }
                };
                var listMedistockView = new MOS.MANAGER.HisMediStock.HisMediStockManager(paramGet).GetView(medistockFilter);
                MEDI_STOCK_NAME = string.Join(", ", listMedistockView.Select(s => s.MEDI_STOCK_NAME));
                //-------------------------------------------------------------------------------------------------- V_HIS_INVOICE
                var metyFilterImpMest = new HisImpMestViewFilterQuery
                {
                    CREATE_TIME_FROM = CastFilter.IMP_TIME_FROM,
                    CREATE_TIME_TO = CastFilter.IMP_TIME_TO,
                    MEDI_STOCK_IDs = CastFilter.MEDI_STOCK_IDs //new List<long> { 22 }
                };
                var listImpMestViews = new MOS.MANAGER.HisImpMest.HisImpMestManager(paramGet).GetView(metyFilterImpMest);
                //--------------------------------------------------------------------------------------------------
                var listImpMestIds = listImpMestViews.Select(s => s.ID).ToList();
                var listHisTmpDateailView = new List<V_HIS_IMP_MEST_MEDICINE>();
                var kip = 0;
                while (listImpMestIds.Count - kip > 0)
                {
                    var listIds = listImpMestIds.Skip(kip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    kip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyExpMestIds = new HisImpMestMedicineViewFilterQuery
                    {
                        IMP_MEST_IDs = listIds,
                    };
                    var mestMedicineDetailsViews = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(param).GetView(metyExpMestIds);
                    listHisTmpDateailView.AddRange(mestMedicineDetailsViews);
                }

                //--------------------------------------------------------------------------------------------------
                var metyFilterMedicineType = new HisMedicineTypeViewFilterQuery();
                var listMedicineTypeViews = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(paramGet).GetView(metyFilterMedicineType);

                ProcessFilterData(listHisTmpDateailView, listMedicineTypeViews);

                result = true;

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
            return true;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            dicSingleTag.Add("IMP_TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.IMP_TIME_FROM));
            dicSingleTag.Add("IMP_TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.IMP_TIME_TO));
            dicSingleTag.Add("MEDISTOCK_IDS", MEDI_STOCK_NAME);

            objectTag.AddObjectData(store, "Parent", listParent);
            objectTag.AddObjectData(store, "Report", _listSarReportMrs00157Rdos);
            objectTag.AddRelationship(store, "Parent", "Report", "ID", "PARENT_ID");

            objectTag.AddObjectData(store, "Report1", _listSarReportMrs00157Rdos1);
            objectTag.AddRelationship(store, "Parent", "Report1", "ID", "PARENT_ID");
        }

        private void ProcessFilterData(List<V_HIS_IMP_MEST_MEDICINE> listHisTmpDetailView, List<V_HIS_MEDICINE_TYPE> listExpMestViews)
        {
            try
            {
                LogSystem.Info("Bat dau xu ly du lieu MRS00157 ===============================================================");
                var listMedicine = new List<V_HIS_IMP_MEST_MEDICINE_NEW>();

                // var listMedicineType = new List<V_HIS_MEDICINE_TYPE>(); 
                foreach (var listMestMedicineDetailView in listHisTmpDetailView)
                {
                    long? Imp_Mest_Id = listMestMedicineDetailView.MEDICINE_TYPE_ID;
                    long parentId = 0;
                    while (Imp_Mest_Id != null)
                    {
                        var mestMedicineTypeView = listExpMestViews.First(s => s.ID == Imp_Mest_Id);
                        Imp_Mest_Id = mestMedicineTypeView.PARENT_ID;
                        if (Imp_Mest_Id == null)
                        {
                            parentId = mestMedicineTypeView.ID;
                        }
                    }
                    var expMedicineNew = new V_HIS_IMP_MEST_MEDICINE_NEW
                    {
                        PARENT_ID = parentId,
                        V_HIS_IMP_MEST_MEDICINE = listMestMedicineDetailView
                    };
                    listMedicine.Add(expMedicineNew);
                }
                var listMestMedicineNewDepartments = listMedicine.GroupBy(s => s.PARENT_ID).ToList();
                foreach (var listMestMedicineNewGroupByDepartments in listMestMedicineNewDepartments)
                {
                    var medicineParent = listExpMestViews.FirstOrDefault(s => listMestMedicineNewGroupByDepartments.First().PARENT_ID == s.ID);
                    listParent.Add(medicineParent);
                    var groupByMestMedicineNewDepartments = listMestMedicineNewGroupByDepartments.GroupBy(s => s.V_HIS_IMP_MEST_MEDICINE.IMP_PRICE).ToList();
                    foreach (var groupByMestMedicine in groupByMestMedicineNewDepartments)
                    {
                        var listGroupByMestMedicine = groupByMestMedicine.GroupBy(s => s.V_HIS_IMP_MEST_MEDICINE.VAT_RATIO).ToList();
                        foreach (var listMestMedicine in listGroupByMestMedicine)
                        {
                            var price = listMestMedicine.First().V_HIS_IMP_MEST_MEDICINE.PRICE;
                            var Imp_Number_Medicine = listMestMedicine.First().V_HIS_IMP_MEST_MEDICINE.AMOUNT;
                            var Imp_CK = listMestMedicine.First().V_HIS_IMP_MEST_MEDICINE.VAT_RATIO;
                            decimal? TotalPrice = 0;
                            if (price != null && Imp_Number_Medicine != null && Imp_CK != null)
                            {
                                TotalPrice = (decimal)price * Imp_Number_Medicine * (1 - Imp_CK);
                            }
                            var rdo = new VSarReportMrs00157RDO
                            {
                                PARENT_ID = listMestMedicineNewGroupByDepartments.First().PARENT_ID,
                                IMP_NAME_MEDICINE = listMestMedicine.First().V_HIS_IMP_MEST_MEDICINE.MEDICINE_TYPE_NAME,
                                IMP_UNIT = listMestMedicine.First().V_HIS_IMP_MEST_MEDICINE.SERVICE_UNIT_NAME,
                                IMP_NUMBER_MEDICINE = listMestMedicine.First().V_HIS_IMP_MEST_MEDICINE.AMOUNT,
                                IMP_CK = listMestMedicine.First().V_HIS_IMP_MEST_MEDICINE.VAT_RATIO,
                                IMP_PRICE_MEDICINE = listMestMedicine.First().V_HIS_IMP_MEST_MEDICINE.IMP_PRICE,
                                IMP_TOTAL_PRICE = TotalPrice
                            };

                            _listSarReportMrs00157Rdos.Add(rdo);
                        }
                    }

                    var groupByMestMedicineNew = listMestMedicineNewGroupByDepartments.GroupBy(s => new { s.V_HIS_IMP_MEST_MEDICINE.MEDICINE_TYPE_ID, s.V_HIS_IMP_MEST_MEDICINE.IMP_PRICE }).ToList();
                    foreach (var listMestMedicine in groupByMestMedicineNew)
                    {
                        var price = listMestMedicine.First().V_HIS_IMP_MEST_MEDICINE.PRICE;
                        var Imp_Number_Medicine = listMestMedicine.Sum(p => p.V_HIS_IMP_MEST_MEDICINE.AMOUNT);
                        var Imp_CK = listMestMedicine.First().V_HIS_IMP_MEST_MEDICINE.VAT_RATIO;
                        decimal? TotalPrice = 0;
                        if (price != null && Imp_Number_Medicine != null && Imp_CK != null)
                        {
                            TotalPrice = (decimal)price * Imp_Number_Medicine * (1 - Imp_CK);
                        }
                        var rdo = new VSarReportMrs00157RDO
                        {
                            PARENT_ID = listMestMedicineNewGroupByDepartments.First().PARENT_ID,
                            IMP_NAME_MEDICINE = listMestMedicine.First().V_HIS_IMP_MEST_MEDICINE.MEDICINE_TYPE_NAME,
                            IMP_UNIT = listMestMedicine.First().V_HIS_IMP_MEST_MEDICINE.SERVICE_UNIT_NAME,
                            IMP_NUMBER_MEDICINE = listMestMedicine.First().V_HIS_IMP_MEST_MEDICINE.AMOUNT,
                            IMP_CK = listMestMedicine.First().V_HIS_IMP_MEST_MEDICINE.VAT_RATIO,
                            IMP_PRICE_MEDICINE = listMestMedicine.First().V_HIS_IMP_MEST_MEDICINE.IMP_PRICE,
                            IMP_TOTAL_PRICE = TotalPrice
                        };

                        _listSarReportMrs00157Rdos1.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
    }
}
