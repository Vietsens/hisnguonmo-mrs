using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisImpMest;
using AutoMapper;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisPatientTypeAlter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.SDO;
using Newtonsoft.Json;
using MRS.Proccessor.Mrs00105;

namespace MRS.Processor.Mrs00188
{
    public class Mrs00188Processor : AbstractProcessor
    {
        Mrs00188Filter castFilter = null;
        private List<Mrs00188RDO> listRdoDetail = new List<Mrs00188RDO>();
        CommonParam paramGet = new CommonParam();
        List<Mrs00188RDO> ListRdo = new List<Mrs00188RDO>();
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicineView = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterialView = new List<V_HIS_EXP_MEST_MATERIAL>();

        List<V_HIS_IMP_MEST_MEDICINE> listMobaImpMestMedicineView = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> listMobaImpMestMaterialView = new List<V_HIS_IMP_MEST_MATERIAL>();

        HIS_DEPARTMENT department = new HIS_DEPARTMENT();
        HIS_MEDI_STOCK mediStock = new HIS_MEDI_STOCK();
        List<V_HIS_EXP_MEST> listPrescription = new List<V_HIS_EXP_MEST>();
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>();
        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>();
        Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType = new Dictionary<long, V_HIS_MEDICINE_TYPE>();
        Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType = new Dictionary<long, V_HIS_MATERIAL_TYPE>();
        private string a = "";
        public Mrs00188Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            a = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00188Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                castFilter = (Mrs00188Filter)this.reportFilter;
                GetMedicineTypeMaterialType();
                //get dữ liệu:
                if (castFilter.DEPARTMENT_ID != null)
                {
                    department = new HisDepartmentManager(paramGet).GetById((long)castFilter.DEPARTMENT_ID);
                }

                if (castFilter.MEDI_STOCK_ID != null)
                {
                    mediStock = new HisMediStockManager(paramGet).GetById(castFilter.MEDI_STOCK_ID ?? 0);
                }

                var presFilter = new HisExpMestViewFilterQuery
                {
                    FINISH_TIME_FROM = castFilter.TIME_FROM,
                    FINISH_TIME_TO = castFilter.TIME_TO,
                    MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID,
                    REQ_DEPARTMENT_ID = castFilter.DEPARTMENT_ID,
                    MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs,
                    REQ_DEPARTMENT_IDs = castFilter.DEPARTMENT_IDs
                };

                listPrescription = new HisExpMestManager(paramGet).GetView(presFilter);
                listPrescription = listPrescription.Where(o => o.TDL_TREATMENT_ID.HasValue).ToList();
                if (IsNotNullOrEmpty(listPrescription))
                {
                    var listTreatmentId = listPrescription.GroupBy(o => o.TDL_TREATMENT_ID).Select(p => p.First().TDL_TREATMENT_ID.Value).ToList();
                    listPatientTypeAlter = new HisPatientTypeAlterManager(paramGet).GetViewByTreatmentIds(listTreatmentId).OrderByDescending(o => o.LOG_TIME).ToList();
                    foreach (var p in listPatientTypeAlter) if (!dicPatientTypeAlter.ContainsKey(p.TREATMENT_ID)) dicPatientTypeAlter[p.TREATMENT_ID] = p;

                    if (IsNotNullOrEmpty(castFilter.TREATMENT_TYPE_IDs))
                    {
                        listPrescription = listPrescription.Where(o => castFilter.TREATMENT_TYPE_IDs.Contains(dicPatientTypeAlter[o.TDL_TREATMENT_ID.Value].TREATMENT_TYPE_ID)).ToList();
                    }
                }

                var listExpMestIds = listPrescription.Select(s => s.ID).ToList();
                if (IsNotNullOrEmpty(listExpMestIds))
                {
                    var skip = 0;
                    while (listExpMestIds.Count - skip > 0)
                    {
                        var listIds = listExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        var metyFilterExpMestMaterial = new HisExpMestMaterialViewFilterQuery
                        {
                            EXP_MEST_IDs = listIds,
                            IS_EXPEND = true,
                            PATIENT_TYPE_ID = castFilter.PATIENT_TYPE_ID,
                        };
                        if (castFilter.SERVICE_IDs != null)
                        {
                            metyFilterExpMestMaterial.MATERIAL_TYPE_IDs = dicMaterialType.Values.Where(o => castFilter.SERVICE_IDs.Contains(o.SERVICE_ID)).Select(o => o.ID).ToList();
                        }
                        if (castFilter.MATERIAL_TYPE_IDs != null)
                        {
                            metyFilterExpMestMaterial.MATERIAL_TYPE_IDs = castFilter.MATERIAL_TYPE_IDs;
                        }
                        var expMestMaterialViews = new HisExpMestMaterialManager(paramGet).GetView(metyFilterExpMestMaterial);
                        listExpMestMaterialView.AddRange(expMestMaterialViews);

                        var metyFilterExpMestMedicine = new HisExpMestMedicineViewFilterQuery
                        {
                            EXP_MEST_IDs = listIds,
                            IS_EXPEND = true,
                            PATIENT_TYPE_ID = castFilter.PATIENT_TYPE_ID
                        };
                        if (castFilter.SERVICE_IDs != null)
                        {
                            metyFilterExpMestMedicine.MEDICINE_TYPE_IDs = dicMedicineType.Values.Where(o => castFilter.SERVICE_IDs.Contains(o.SERVICE_ID)).Select(o => o.ID).ToList();
                        }
                        if (castFilter.MEDICINE_TYPE_IDs != null)
                        {
                            metyFilterExpMestMedicine.MEDICINE_TYPE_IDs = castFilter.MEDICINE_TYPE_IDs;
                        }
                        var expMestMedicineViews = new HisExpMestMedicineManager(paramGet).GetView(metyFilterExpMestMedicine);
                        listExpMestMedicineView.AddRange(expMestMedicineViews);
                    }
                }


                var listMobaExpMestIds = listExpMestMedicineView.Where(o=>o.TH_AMOUNT>0).Select(s => s.EXP_MEST_ID??0).Distinct().ToList();
                listMobaExpMestIds.AddRange(listExpMestMaterialView.Where(o => o.TH_AMOUNT > 0).Select(s => s.EXP_MEST_ID ?? 0).Distinct().ToList());
                listMobaExpMestIds = listMobaExpMestIds.Distinct().ToList();
                if (IsNotNullOrEmpty(listMobaExpMestIds))
                {
                    var skip = 0;
                    while (listMobaExpMestIds.Count - skip > 0)
                    {
                        var listIds = listMobaExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        var mobaImpMestFilter = new HisImpMestFilterQuery
                        {
                            MOBA_EXP_MEST_IDs = listIds,
                            IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT
                        };

                        var mobaImpMest = new HisImpMestManager(paramGet).Get(mobaImpMestFilter)??new List<HIS_IMP_MEST>();

                        var metyFilterImpMestMaterial = new HisImpMestMaterialViewFilterQuery
                        {
                            IMP_MEST_IDs = mobaImpMest.Select(o=>o.ID).ToList(),
                        };
                       
                        var impMestMaterialViews = new HisImpMestMaterialManager(paramGet).GetView(metyFilterImpMestMaterial)??new List<V_HIS_IMP_MEST_MATERIAL>();
                        listMobaImpMestMaterialView.AddRange(impMestMaterialViews);

                        var metyFilterImpMestMedicine = new HisImpMestMedicineViewFilterQuery
                        {
                            IMP_MEST_IDs = mobaImpMest.Select(o => o.ID).ToList(),
                        };

                        var impMestMedicineViews = new HisImpMestMedicineManager(paramGet).GetView(metyFilterImpMestMedicine) ?? new List<V_HIS_IMP_MEST_MEDICINE>();
                        listMobaImpMestMedicineView.AddRange(impMestMedicineViews);
                    }
                }
                
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }


        private void GetMedicineTypeMaterialType()
        {
            CommonParam paramGet = new CommonParam();
            var ListMedicineType = new HisMedicineTypeManager(paramGet).GetView(new HisMedicineTypeViewFilterQuery());
            var ListMaterialType = new HisMaterialTypeManager(paramGet).GetView(new HisMaterialTypeViewFilterQuery());
            if (IsNotNullOrEmpty(ListMedicineType))
            {
                foreach (var item in ListMedicineType)
                {
                    dicMedicineType[item.ID] = item;
                }
            }

            if (IsNotNullOrEmpty(ListMaterialType))
            {
                foreach (var item in ListMaterialType)
                {
                    dicMaterialType[item.ID] = item;
                }
            }
        }

        private List<Mrs00105RDO> CreateOtherReport()
        {
            try
            {
                if (castFilter.IS_ADD_INVENTORY == true)
                {
                    CreateReportSDO sdo = new CreateReportSDO();
                    sdo.ReportTypeCode = "MRS00105";
                    sdo.ReportTemplateCode = "MRS0010501";
                    sdo.ListKeyAllow = "Services";
                    Mrs00188Filter filter = new Mrs00188Filter();
                    filter.TIME_FROM = castFilter.TIME_FROM;
                    filter.TIME_TO = castFilter.TIME_TO;
                    filter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                    filter.MEDICINE_TYPE_IDs = listExpMestMedicineView.Select(o => o.MEDICINE_TYPE_ID).Distinct().ToList();
                    filter.MATERIAL_TYPE_IDs = listExpMestMaterialView.Select(o => o.MATERIAL_TYPE_ID).Distinct().ToList();
                    sdo.Filter = JsonConvert.SerializeObject(filter);
                    {
                        ReportResultSDO result = GetDataReport(sdo);
                        {
                            {
                                if (result != null && result.DATA_DETAIL != null && result.DATA_DETAIL.Count > 0)
                                {
                                    return result.DATA_DETAIL["Services"] as List<Mrs00105RDO>;


                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return new List<Mrs00105RDO>();
        }

        ReportResultSDO GetDataReport(CreateReportSDO createReportSDO)
        {
            ReportResultSDO result = null;
            try
            {
                var rs = new MRS.MANAGER.Manager.MrsReportManager(new CommonParam()).CreateData(createReportSDO);
                result = rs as ReportResultSDO;

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                result = null;
            }

            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                ListRdo.Clear();
                foreach (var item in listExpMestMaterialView)
                {
                    var expMest = listPrescription.FirstOrDefault(o => o.ID == item.EXP_MEST_ID) ?? new V_HIS_EXP_MEST();
                    var impMobaMestMaterial = listMobaImpMestMaterialView.Where(o => o.TH_EXP_MEST_MATERIAL_ID == item.ID).ToList();
                    var materialType = dicMaterialType.ContainsKey(item.MATERIAL_TYPE_ID) ? dicMaterialType[item.MATERIAL_TYPE_ID] : new V_HIS_MATERIAL_TYPE();
                    string parentName = "";
                    if (materialType != null && materialType.PARENT_ID != null)
                    {
                        var parentMaterialType = dicMaterialType.ContainsKey(materialType.PARENT_ID ?? 0) ? dicMaterialType[materialType.PARENT_ID ?? 0] : null;
                        if (parentMaterialType != null)
                        {
                            parentName = parentMaterialType.MATERIAL_TYPE_NAME;
                        }
                        else
                        {
                            parentName = "Vật tư";
                        }
                    }
                    else
                    {
                        parentName = "Vật tư";
                    }

                    Mrs00188RDO rdo = new Mrs00188RDO
                    {
                        TYPE_NAME = materialType.IS_CHEMICAL_SUBSTANCE == 1 ? "HÓA CHẤT" : "VẬT TƯ",
                        MEDICINE_ID = item.MATERIAL_ID,
                        CONCENTRA = "",
                        REGISTER_NUMBER = "",
                       MEDICINE_TYPE_NAME = item.MATERIAL_TYPE_NAME,
                       UNIT = item.SERVICE_UNIT_NAME,
                       PRICE = item.PRICE ?? item.IMP_PRICE,
                        AMOUNT = item.AMOUNT,
                        TH_AMOUNT = impMobaMestMaterial.Sum(s => s.AMOUNT),
                        TT_PRICE = (item.PRICE ?? item.IMP_PRICE) * item.AMOUNT,
                        TH_TT_PRICE = impMobaMestMaterial.Sum(s => (s.PRICE ?? s.IMP_PRICE) * s.AMOUNT),
                       EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(expMest.FINISH_TIME ?? 0),
                       EXP_MEST_CODE = expMest.EXP_MEST_CODE,
                       TDL_PATIENT_NAME = expMest.TDL_PATIENT_NAME,
                       TDL_PATIENT_DOB = expMest.TDL_PATIENT_DOB,
                       TDL_PATIENT_ADDRESS = expMest.TDL_PATIENT_ADDRESS,
                        FINISH_TIME = expMest.FINISH_TIME,
                        REQ_DEPARTMENT_CODE = expMest.REQ_DEPARTMENT_CODE,
                        REQ_DEPARTMENT_NAME = expMest.REQ_DEPARTMENT_NAME,
                       MEDICINE_GROUP_NAME = parentName,
                       MEDICINE_TYPE_CODE = item.MATERIAL_TYPE_CODE
                   };
                    listRdoDetail.Add(rdo);
                }

                foreach (var item in listExpMestMedicineView)
                {
                    var expMest = listPrescription.FirstOrDefault(o => o.ID == item.EXP_MEST_ID) ?? new V_HIS_EXP_MEST();
                    var impMobaMestMedicine = listMobaImpMestMedicineView.Where(o => o.TH_EXP_MEST_MEDICINE_ID == item.ID).ToList();
                    var medicineType = dicMedicineType.ContainsKey(item.MEDICINE_TYPE_ID) ? dicMedicineType[item.MEDICINE_TYPE_ID] : new V_HIS_MEDICINE_TYPE();
                    string parentName = "";
                    if (medicineType != null && medicineType.PARENT_ID != null)
                    {
                        var parentMedicineType = dicMedicineType.ContainsKey(medicineType.PARENT_ID ?? 0) ? dicMedicineType[medicineType.PARENT_ID ?? 0] : null;
                        if (parentMedicineType != null)
                        {
                            parentName = parentMedicineType.MEDICINE_TYPE_NAME;
                        }
                        else
                        {
                            parentName = "Thuốc";
                        }
                    }
                    else
                    {
                        parentName = "Thuốc";
                    }
                    Mrs00188RDO rdo = new Mrs00188RDO
                    {
                        TYPE_NAME = "THUỐC",
                        MEDICINE_ID = item.MEDICINE_ID,
                        CONCENTRA = item.CONCENTRA,
                        REGISTER_NUMBER = item.REGISTER_NUMBER,

                        MEDICINE_TYPE_NAME = item.MEDICINE_TYPE_NAME,
                        UNIT = item.SERVICE_UNIT_NAME,
                        PRICE = item.PRICE ?? item.IMP_PRICE,
                        AMOUNT = item.AMOUNT,
                        TH_AMOUNT = impMobaMestMedicine.Sum(s=>s.AMOUNT),
                        TT_PRICE = (item.PRICE ?? item.IMP_PRICE) * item.AMOUNT,
                        TH_TT_PRICE = impMobaMestMedicine.Sum(s => (s.PRICE ?? s.IMP_PRICE) * s.AMOUNT),
                        EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(expMest.FINISH_TIME ?? 0),
                        EXP_MEST_CODE = expMest.EXP_MEST_CODE,
                        TDL_PATIENT_NAME = expMest.TDL_PATIENT_NAME,
                        TDL_PATIENT_DOB = expMest.TDL_PATIENT_DOB,
                        TDL_PATIENT_ADDRESS = expMest.TDL_PATIENT_ADDRESS,
                        FINISH_TIME = expMest.FINISH_TIME,
                        REQ_DEPARTMENT_CODE = expMest.REQ_DEPARTMENT_CODE,
                        REQ_DEPARTMENT_NAME = expMest.REQ_DEPARTMENT_NAME,
                        MEDICINE_GROUP_NAME = parentName,
                        MEDICINE_TYPE_CODE = item.MEDICINE_TYPE_CODE
                    };
                    listRdoDetail.Add(rdo);
                }

                string keyGroup = "{0}_{1}";
                //khi có điều kiện lọc từ template thì đổi sang key từ template
                if (this.dicDataFilter.ContainsKey("KEY_GROUP_EXP") && this.dicDataFilter["KEY_GROUP_EXP"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_GROUP_EXP"].ToString()))
                {
                    keyGroup = this.dicDataFilter["KEY_GROUP_EXP"].ToString();
                }
                var GroupbyMaterialIds = listRdoDetail.GroupBy(o => string.Format(keyGroup, o.TYPE_NAME, o.MEDICINE_ID, o.REQ_DEPARTMENT_CODE)).ToList();
                foreach (var group in GroupbyMaterialIds)
                {
                    List<Mrs00188RDO> materialSub = group.ToList<Mrs00188RDO>();
                    Mrs00188RDO rdo = new Mrs00188RDO
                    {
                        TYPE_NAME = materialSub.First().TYPE_NAME,
                        MEDICINE_ID = materialSub.First().MEDICINE_ID,
                        CONCENTRA = materialSub.First().CONCENTRA,
                        REGISTER_NUMBER = materialSub.First().REGISTER_NUMBER,
                        MEDICINE_TYPE_NAME = materialSub.First().MEDICINE_TYPE_NAME,
                        UNIT = materialSub.First().UNIT,
                        PRICE = materialSub.First().PRICE,
                        AMOUNT = materialSub.Sum(s=>s.AMOUNT),
                        TH_AMOUNT = materialSub.Sum(s => s.TH_AMOUNT),
                        TT_PRICE = materialSub.Sum(s => s.TT_PRICE),
                        TH_TT_PRICE = materialSub.Sum(s => s.TH_TT_PRICE),
                        EXP_TIME_STR = materialSub.First().EXP_TIME_STR,
                        EXP_MEST_CODE = materialSub.First().EXP_MEST_CODE,
                        TDL_PATIENT_NAME = materialSub.First().TDL_PATIENT_NAME,
                        TDL_PATIENT_DOB = materialSub.First().TDL_PATIENT_DOB,
                        TDL_PATIENT_ADDRESS = materialSub.First().TDL_PATIENT_ADDRESS,
                        FINISH_TIME = materialSub.First().FINISH_TIME,
                        REQ_DEPARTMENT_CODE = materialSub.First().REQ_DEPARTMENT_CODE,
                        REQ_DEPARTMENT_NAME = materialSub.First().REQ_DEPARTMENT_NAME,
                        MEDICINE_GROUP_NAME = materialSub.First().MEDICINE_GROUP_NAME,
                        MEDICINE_TYPE_CODE = materialSub.First().MEDICINE_TYPE_CODE,
                    };
                    ListRdo.Add(rdo);
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00188Filter)this.reportFilter).TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00188Filter)this.reportFilter).TIME_TO));
            dicSingleTag.Add("DEPARTMENT_NAME", department.DEPARTMENT_NAME ?? "");
            dicSingleTag.Add("MEDI_STOCK_NAME", mediStock.MEDI_STOCK_NAME ?? "");
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "ReportDetail", listRdoDetail);
            objectTag.AddObjectData(store, "Services", CreateOtherReport());
        }

    }
}