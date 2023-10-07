using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisDepartment;
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
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisMaterialType;

namespace MRS.Processor.Mrs00156
{
    class Mrs00156Processor : AbstractProcessor
    {
        List<HIS_DEPARTMENT> _department = new List<HIS_DEPARTMENT>();
        List<V_HIS_MEDICINE_TYPE_NEW> _medicineTypesNews = new List<V_HIS_MEDICINE_TYPE_NEW>();
        List<V_HIS_MATERIAL_TYPE_NEW> _materialTypesNews = new List<V_HIS_MATERIAL_TYPE_NEW>();
        List<VSarReportMrs00156RDO> _listSarReportMrs00156Rdos = new List<VSarReportMrs00156RDO>();
        List<VSarReportMrs00156RDO> _listSarReportMrs00156RdoMates = new List<VSarReportMrs00156RDO>();
        Mrs00156Filter CastFilter;

        public Mrs00156Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00156Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                CastFilter = (Mrs00156Filter)this.reportFilter;
                var paramGet = new CommonParam();
                var metyFilterDepartment = new HisDepartmentFilterQuery
                {
                    IDs = CastFilter.DEPARTMENT_IDs
                };
                _department = new MOS.MANAGER.HisDepartment.HisDepartmentManager(paramGet).Get(metyFilterDepartment);
                //-------------------------------------------------------------------------------------------------- V_HIS_IMP_MEST
                var metyFilterExpMest = new HisExpMestViewFilterQuery
                {
                    EXP_MEST_TYPE_ID = CastFilter.EXP_MEST_TYPE_ID,//loại xuất
                    EXP_MEST_STT_ID = CastFilter.EXP_MEST_STT_ID//trạng thái xuất
                };
                if (CastFilter.DEPARTMENT_IDs.Count > 0)
                    metyFilterExpMest.REQ_DEPARTMENT_IDs = CastFilter.DEPARTMENT_IDs; //Khoa yêu cầu xuất
                if (CastFilter.EXP_MEST_STT_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE)//nếu trạng thái đã xuất lấy theo EXP_TIME(ngày xuất)
                {
                    metyFilterExpMest.FINISH_DATE_FROM = CastFilter.DATE_FROM;
                    metyFilterExpMest.FINISH_DATE_TO = CastFilter.DATE_TO;
                }
                else//Nếu trạng thái không phải đã xuất lấy theo CREATE_TIME (ngày tạo)
                {
                    metyFilterExpMest.CREATE_TIME_FROM = CastFilter.DATE_FROM;
                    metyFilterExpMest.CREATE_TIME_TO = CastFilter.DATE_TO;
                }
                if (CastFilter.REQ_ROOM_IDs != null && CastFilter.REQ_ROOM_IDs.Count != 0)//Không bắt buộc nhập
                {
                    metyFilterExpMest.REQ_ROOM_IDs = CastFilter.REQ_ROOM_IDs; //phòng yêu cầu xuất
                }
                if (CastFilter.MEDI_STOCK_IDs != null && CastFilter.MEDI_STOCK_IDs.Count > 0)
                {
                    metyFilterExpMest.MEDI_STOCK_IDs = CastFilter.MEDI_STOCK_IDs;
                }
                var listExpMestViews = new MOS.MANAGER.HisExpMest.HisExpMestManager(paramGet).GetView(metyFilterExpMest);
                //-------------------------------------------------------------------------------------------------- V_HIS_EXP_MEST_MEDICINE
                var listExpMestIds = listExpMestViews.Select(s => s.ID).ToList();
                var listExpMestMedicineViews = new List<V_HIS_EXP_MEST_MEDICINE>();
                var listExpMestMaterialViews = new List<V_HIS_EXP_MEST_MATERIAL>();
                var skip = 0;
                while (listExpMestIds.Count - skip > 0)
                {
                    var listIds = listExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisExpMestMedicineViewFilterQuery metyFilterExpMestMedicine = new HisExpMestMedicineViewFilterQuery
                    {
                        EXP_MEST_IDs = listIds,
                        EXP_MEST_STT_ID = CastFilter.EXP_MEST_STT_ID,
                        IS_EXPORT = true
                    };

                    var expMestMedicineViews = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(metyFilterExpMestMedicine);
                    listExpMestMedicineViews.AddRange(expMestMedicineViews);

                    HisExpMestMaterialViewFilterQuery metyFilterExpMestMaterial = new HisExpMestMaterialViewFilterQuery
                    {
                        EXP_MEST_IDs = listIds,
                        EXP_MEST_STT_ID = CastFilter.EXP_MEST_STT_ID,
                        IS_EXPORT = true
                    };

                    var expMestMaterialViews = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(paramGet).GetView(metyFilterExpMestMaterial);
                    listExpMestMaterialViews.AddRange(expMestMaterialViews);
                }
                //--------------------------------------------------------------------------------------------------
                var metyFilterMedicineType = new HisMedicineTypeViewFilterQuery();
                var listMedicineTypeViews = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(paramGet).GetView(metyFilterMedicineType);
                //--------------------------------------------------------------------------------------------------
                var metyFilterMaterialType = new HisMaterialTypeViewFilterQuery();
                var listMaterialTypeViews = new HisMaterialTypeManager(paramGet).GetView(metyFilterMaterialType);

                ProcessFilterData(listExpMestMedicineViews, listMedicineTypeViews);

                ProcessFilterData(listExpMestMaterialViews, listMaterialTypeViews);

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

            dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_FROM));
            dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TO));

            objectTag.AddObjectData(store, "Department", _department.Where(p=>_medicineTypesNews.Exists(q=>q.PARENT_ID ==p.ID)).ToList());

            objectTag.AddObjectData(store, "MedicineType", _medicineTypesNews);

            objectTag.AddObjectData(store, "Report", _listSarReportMrs00156Rdos);
            objectTag.AddRelationship(store, "Department", "MedicineType", "ID", "PARENT_ID");

            objectTag.AddRelationship(store, "MedicineType", "Report", new string[] { "V_HIS_MEDICINE_TYPE.ID", "PARENT_ID" }, new string[] { "PARENT_ID", "DEPARTMENT_ID" });

            objectTag.AddObjectData(store, "DepartmentMate", _department.Where(p => _materialTypesNews.Exists(q => q.PARENT_ID == p.ID)).ToList());

            objectTag.AddObjectData(store, "MaterialType", _materialTypesNews);

            objectTag.AddObjectData(store, "ReportMate", _listSarReportMrs00156RdoMates);
            objectTag.AddRelationship(store, "Department", "MaterialType", "ID", "PARENT_ID");

            objectTag.AddRelationship(store, "MaterialType", "ReportMate", new string[] { "V_HIS_MATERIAL_TYPE.ID", "PARENT_ID" }, new string[] { "PARENT_ID", "DEPARTMENT_ID" });

        }


        private void ProcessFilterData(List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicineViews, List<V_HIS_MEDICINE_TYPE> listMedicineTypeViews)
        {
            try
            {
                LogSystem.Info("Bat dau xu ly du lieu MRS00156 ===============================================================");
                var listExpMestMedicineNews = new List<V_HIS_EXP_MEST_MEDICINE_NEW>();
                foreach (var listExpMestMedicineView in listExpMestMedicineViews)
                {
                    long? medicineTypeId = listExpMestMedicineView.MEDICINE_TYPE_ID;
                    long parentId = 0;
                    while (medicineTypeId != null)
                    {
                        var medicineTypeView = listMedicineTypeViews.First(s => s.ID == medicineTypeId);
                        medicineTypeId = medicineTypeView.PARENT_ID;
                        if (medicineTypeId == null)
                            parentId = medicineTypeView.ID;
                    }
                    var expMestMedicineNew = new V_HIS_EXP_MEST_MEDICINE_NEW
                    {
                        PARENT_ID = parentId,
                        V_HIS_EXP_MEST_MEDICINE = listExpMestMedicineView
                    };
                    listExpMestMedicineNews.Add(expMestMedicineNew);
                }
                var listExpMestMedicineNewGroupByReqDepartments = listExpMestMedicineNews.GroupBy(s => s.V_HIS_EXP_MEST_MEDICINE.REQ_DEPARTMENT_ID).ToList();
                foreach (var listExpMestMedicineNewGroupByReqDepartment in listExpMestMedicineNewGroupByReqDepartments)
                {
                    var listExpMestMedicineNewGroupByParents = listExpMestMedicineNewGroupByReqDepartment.GroupBy(s => s.PARENT_ID).ToList();
                    foreach (var listExpMestMedicineNewGroupByParent in listExpMestMedicineNewGroupByParents)
                    {
                        var hh = listMedicineTypeViews.First(s => s.ID == listExpMestMedicineNewGroupByParent.Key);
                        var mm = new V_HIS_MEDICINE_TYPE_NEW
                        {
                            PARENT_ID = listExpMestMedicineNewGroupByReqDepartment.First().V_HIS_EXP_MEST_MEDICINE.REQ_DEPARTMENT_ID,
                            V_HIS_MEDICINE_TYPE = hh
                        };
                        _medicineTypesNews.Add(mm);

                        var listExpMestMedicineNewGroupByMedicines = listExpMestMedicineNewGroupByParent.GroupBy(s => s.V_HIS_EXP_MEST_MEDICINE.MEDICINE_TYPE_ID).ToList();
                        foreach (var listExpMestMedicineNewGroupByMedicine in listExpMestMedicineNewGroupByMedicines)
                        {
                            var listExpMestMedicineNewGroupByNationals = listExpMestMedicineNewGroupByMedicine.GroupBy(s => s.V_HIS_EXP_MEST_MEDICINE.NATIONAL_NAME).ToList();
                            foreach (var listExpMestMedicineNewGroupByNational in listExpMestMedicineNewGroupByNationals)
                            {
                                var listExpMestMedicineNewGroupByPrices = listExpMestMedicineNewGroupByNational.GroupBy(s => s.V_HIS_EXP_MEST_MEDICINE.PRICE).ToList();
                                foreach (var listExpMestMedicineNewGroupByPrice in listExpMestMedicineNewGroupByPrices)
                                {
                                    var price = listExpMestMedicineNewGroupByPrice.First().V_HIS_EXP_MEST_MEDICINE.PRICE;
                                    var amount = listExpMestMedicineNewGroupByPrice.Sum(s => s.V_HIS_EXP_MEST_MEDICINE.AMOUNT);
                                    decimal totalPrice = 0;
                                    if (price.HasValue)
                                        totalPrice = (decimal)price * amount;
                                    var rdo = new VSarReportMrs00156RDO
                                    {
                                        PARENT_ID = listExpMestMedicineNewGroupByParent.Key,
                                        DEPARTMENT_ID = listExpMestMedicineNewGroupByReqDepartment.Key,
                                        MEDICINE_NAME = listExpMestMedicineNewGroupByPrice.First().V_HIS_EXP_MEST_MEDICINE.MEDICINE_TYPE_NAME,
                                        SERVICE_UNIT_NAME = listExpMestMedicineNewGroupByPrice.First().V_HIS_EXP_MEST_MEDICINE.SERVICE_UNIT_NAME,
                                        NATIONAL_NAME = listExpMestMedicineNewGroupByPrice.First().V_HIS_EXP_MEST_MEDICINE.NATIONAL_NAME,
                                        PRICE = price,
                                        AMOUNT = amount,
                                        TOTAL_PRICE = totalPrice
                                    };
                                    _listSarReportMrs00156Rdos.Add(rdo);
                                }
                            }
                        }
                    }
                }
                LogSystem.Info("Ket thuc xu ly du lieu MRS00156 ===============================================================");
            }
            catch (Exception ex)
            {
                LogSystem.Info("Loi trong qua trinh xu ly du lieu ===============================================================");
                LogSystem.Error(ex);
            }
        }


        private void ProcessFilterData(List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterialViews, List<V_HIS_MATERIAL_TYPE> listMaterialTypeViews)
        {
            try
            {
                LogSystem.Info("Bat dau xu ly du lieu MRS00156 ===============================================================");
                var listExpMestMaterialNews = new List<V_HIS_EXP_MEST_MATERIAL_NEW>();
                foreach (var listExpMestMaterialView in listExpMestMaterialViews)
                {
                    long? materialTypeId = listExpMestMaterialView.MATERIAL_TYPE_ID;
                    long parentId = 0;
                    while (materialTypeId != null)
                    {
                        var materialTypeView = listMaterialTypeViews.First(s => s.ID == materialTypeId);
                        materialTypeId = materialTypeView.PARENT_ID;
                        if (materialTypeId == null)
                            parentId = materialTypeView.ID;
                    }
                    var expMestMaterialNew = new V_HIS_EXP_MEST_MATERIAL_NEW
                    {
                        PARENT_ID = parentId,
                        V_HIS_EXP_MEST_MATERIAL = listExpMestMaterialView
                    };
                    listExpMestMaterialNews.Add(expMestMaterialNew);
                }
                var listExpMestMaterialNewGroupByReqDepartments = listExpMestMaterialNews.GroupBy(s => s.V_HIS_EXP_MEST_MATERIAL.REQ_DEPARTMENT_ID).ToList();
                foreach (var listExpMestMaterialNewGroupByReqDepartment in listExpMestMaterialNewGroupByReqDepartments)
                {
                    var listExpMestMaterialNewGroupByParents = listExpMestMaterialNewGroupByReqDepartment.GroupBy(s => s.PARENT_ID).ToList();
                    foreach (var listExpMestMaterialNewGroupByParent in listExpMestMaterialNewGroupByParents)
                    {
                        var hh = listMaterialTypeViews.First(s => s.ID == listExpMestMaterialNewGroupByParent.Key);
                        var mm = new V_HIS_MATERIAL_TYPE_NEW
                        {
                            PARENT_ID = listExpMestMaterialNewGroupByReqDepartment.First().V_HIS_EXP_MEST_MATERIAL.REQ_DEPARTMENT_ID,
                            V_HIS_MATERIAL_TYPE = hh
                        };
                        _materialTypesNews.Add(mm);

                        var listExpMestMaterialNewGroupByMaterials = listExpMestMaterialNewGroupByParent.GroupBy(s => s.V_HIS_EXP_MEST_MATERIAL.MATERIAL_TYPE_ID).ToList();
                        foreach (var listExpMestMaterialNewGroupByMaterial in listExpMestMaterialNewGroupByMaterials)
                        {
                            var listExpMestMaterialNewGroupByNationals = listExpMestMaterialNewGroupByMaterial.GroupBy(s => s.V_HIS_EXP_MEST_MATERIAL.NATIONAL_NAME).ToList();
                            foreach (var listExpMestMaterialNewGroupByNational in listExpMestMaterialNewGroupByNationals)
                            {
                                var listExpMestMaterialNewGroupByPrices = listExpMestMaterialNewGroupByNational.GroupBy(s => s.V_HIS_EXP_MEST_MATERIAL.PRICE).ToList();
                                foreach (var listExpMestMaterialNewGroupByPrice in listExpMestMaterialNewGroupByPrices)
                                {
                                    var price = listExpMestMaterialNewGroupByPrice.First().V_HIS_EXP_MEST_MATERIAL.PRICE;
                                    var amount = listExpMestMaterialNewGroupByPrice.Sum(s => s.V_HIS_EXP_MEST_MATERIAL.AMOUNT);
                                    decimal totalPrice = 0;
                                    if (price.HasValue)
                                        totalPrice = (decimal)price * amount;
                                    var rdo = new VSarReportMrs00156RDO
                                    {
                                        PARENT_ID = listExpMestMaterialNewGroupByParent.Key,
                                        DEPARTMENT_ID = listExpMestMaterialNewGroupByReqDepartment.Key,
                                        MEDICINE_NAME = listExpMestMaterialNewGroupByPrice.First().V_HIS_EXP_MEST_MATERIAL.MATERIAL_TYPE_NAME,
                                        SERVICE_UNIT_NAME = listExpMestMaterialNewGroupByPrice.First().V_HIS_EXP_MEST_MATERIAL.SERVICE_UNIT_NAME,
                                        NATIONAL_NAME = listExpMestMaterialNewGroupByPrice.First().V_HIS_EXP_MEST_MATERIAL.NATIONAL_NAME,
                                        PRICE = price,
                                        AMOUNT = amount,
                                        TOTAL_PRICE = totalPrice
                                    };
                                    _listSarReportMrs00156RdoMates.Add(rdo);
                                }
                            }
                        }
                    }
                }
                LogSystem.Info("Ket thuc xu ly du lieu MRS00156 ===============================================================");
            }
            catch (Exception ex)
            {
                LogSystem.Info("Loi trong qua trinh xu ly du lieu ===============================================================");
                LogSystem.Error(ex);
            }
        }
    }
}
