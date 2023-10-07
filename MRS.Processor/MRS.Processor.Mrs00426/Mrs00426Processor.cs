using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisDepartment;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisImpMest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00426
{
    class Mrs00426Processor : AbstractProcessor
    {
        Mrs00426Filter castFilter = null;
        List<Mrs00426RDO> listRdo = new List<Mrs00426RDO>();
        List<Mrs00426RDO> listRdoGroupName = new List<Mrs00426RDO>();
        List<Mrs00426RDO> listRdoIntructionTime = new List<Mrs00426RDO>();
        List<Mrs00426RDO> listRdoTreatmentCode = new List<Mrs00426RDO>();

        List<HIS_DEPARTMENT> listDepartments = new List<HIS_DEPARTMENT>();
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST> listMobaImMests = new List<V_HIS_IMP_MEST>();
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST> listPrescriptions = new List<V_HIS_EXP_MEST>();
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterials = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterials = new List<V_HIS_IMP_MEST_MATERIAL>();

        public Mrs00426Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00426Filter);
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                if (IsNotNullOrEmpty(listDepartments))
                {
                    dicSingleTag.Add("DEPARTMENT_NAME", listDepartments.First().DEPARTMENT_NAME);
                }

                bool exportSuccess = true;

                objectTag.AddObjectData(store, "Report", listRdo);
                objectTag.AddObjectData(store, "listRdoGroupName", listRdoGroupName);
                objectTag.AddObjectData(store, "listRdoIntructionTime", listRdoIntructionTime);
                objectTag.AddObjectData(store, "listRdoTreatmentCode", listRdoTreatmentCode);
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "listRdoGroupName", "listRdoIntructionTime", "GROUP_MANE", "GROUP_MANE");
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "listRdoIntructionTime", "listRdoTreatmentCode", "INTRUCTION_TIME", "INTRUCTION_TIME");
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "listRdoTreatmentCode", "Report", "TREATMENT_CODE", "TREATMENT_CODE");

                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00426Filter)this.reportFilter;

                HisExpMestViewFilterQuery presCriptionFillter = new HisExpMestViewFilterQuery();
                presCriptionFillter.TDL_INTRUCTION_DATE_FROM = castFilter.TIME_FROM;
                presCriptionFillter.TDL_INTRUCTION_DATE_TO = castFilter.TIME_TO;
                presCriptionFillter.REQ_DEPARTMENT_ID = castFilter.DEPARTMENT_ID;
                presCriptionFillter.EXP_MEST_TYPE_IDs = new List<long>()
                {
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT,
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT
                };
                listPrescriptions = new HisExpMestManager(param).GetView(presCriptionFillter);

                var skip = 0;
                while (listPrescriptions.Count() - skip > 0)
                {
                    var listIds = listPrescriptions.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisExpMestMedicineViewFilterQuery expMestMedicineFillter = new HisExpMestMedicineViewFilterQuery();
                    expMestMedicineFillter.EXP_MEST_IDs = listIds.Select(s => s.ID).ToList();
                    expMestMedicineFillter.IS_EXPORT = true;
                    var listExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineFillter);
                    listExpMestMedicines.AddRange(listExpMestMedicine);

                    HisExpMestMaterialViewFilterQuery expMestMaterialFillter = new HisExpMestMaterialViewFilterQuery();
                    expMestMaterialFillter.EXP_MEST_IDs = listIds.Select(s => s.ID).ToList();
                    expMestMaterialFillter.IS_EXPORT = true;
                    var listExpMestMaterial = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(param).GetView(expMestMaterialFillter);
                    listExpMestMaterials.AddRange(listExpMestMaterial);

                    HisDepartmentFilterQuery departmentFillter = new HisDepartmentFilterQuery();
                    departmentFillter.IDs = listIds.Select(s => s.REQ_DEPARTMENT_ID).ToList();
                    var listDepartment = new MOS.MANAGER.HisDepartment.HisDepartmentManager(param).Get(departmentFillter);
                    listDepartments.AddRange(listDepartment);
                }

                skip = 0;
                while (listExpMestMedicines.Count() - skip > 0)
                {
                    var listIds = listExpMestMedicines.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisImpMestViewFilterQuery mobaImMestFillter = new HisImpMestViewFilterQuery();
                    mobaImMestFillter.CREATE_TIME_TO = castFilter.TIME_FROM;
                    mobaImMestFillter.CREATE_TIME_TO = castFilter.TIME_TO;
                    mobaImMestFillter.MOBA_EXP_MEST_IDs = listIds.Select(s => s.EXP_MEST_ID ?? 0).ToList();
                    var listMobaImMest = new HisImpMestManager(param).GetView(mobaImMestFillter);
                    listMobaImMests.AddRange(listMobaImMest);
                }

                skip = 0;
                while (listMobaImMests.Count() - skip > 0)
                {
                    var listIds = listMobaImMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisImpMestMedicineViewFilterQuery ImpMestMedicineFillter = new HisImpMestMedicineViewFilterQuery();
                    ImpMestMedicineFillter.IMP_MEST_IDs = listIds.Select(s => s.ID).ToList();
                    var listImpMestMedicine = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(param).GetView(ImpMestMedicineFillter);
                    listImpMestMedicines.AddRange(listImpMestMedicine);
                }

                skip = 0;
                while (listExpMestMaterials.Count() - skip > 0)
                {
                    var listIds = listExpMestMaterials.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisImpMestViewFilterQuery mobaImMestFillter = new HisImpMestViewFilterQuery();
                    mobaImMestFillter.CREATE_TIME_FROM = castFilter.TIME_FROM;
                    mobaImMestFillter.CREATE_TIME_TO = castFilter.TIME_TO;
                    mobaImMestFillter.MOBA_EXP_MEST_IDs = listIds.Select(s => s.EXP_MEST_ID ?? 0).ToList();
                    var listMobaImMest = new HisImpMestManager(param).GetView(mobaImMestFillter);
                    listMobaImMests.AddRange(listMobaImMest);
                }

                skip = 0;
                while (listMobaImMests.Count() - skip > 0)
                {
                    var listIds = listMobaImMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisImpMestMaterialViewFilterQuery ImpMestMaterialFillter = new HisImpMestMaterialViewFilterQuery();
                    ImpMestMaterialFillter.IMP_MEST_IDs = listIds.Select(s => s.ID).ToList();
                    var listImpMestMaterial = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(param).GetView(ImpMestMaterialFillter);
                    listImpMestMaterials.AddRange(listImpMestMaterials);
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
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();

                foreach (var presCription in listPrescriptions)
                {
                    Mrs00426RDO rdo = new Mrs00426RDO();
                    rdo.PATIENT_NAME = presCription.TDL_PATIENT_NAME;
                    rdo.TREATMENT_CODE = presCription.TDL_TREATMENT_CODE;
                    rdo.INTRUCTION_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(presCription.TDL_INTRUCTION_TIME.Value);
                    //thuoc
                    var listExpMestMediciness = listExpMestMedicines.Where(s => s.EXP_MEST_ID == presCription.ID);
                    foreach (var listExpMestMedicine in listExpMestMediciness)
                    {
                        rdo.GROUP_MANE = "THUỐC";
                        rdo.SERVICE_TYPE_NAME = listExpMestMedicine.MEDICINE_TYPE_NAME;  // ten thuoc
                        rdo.SERVICE_TYPE_CODE = listExpMestMedicine.MEDICINE_TYPE_CODE;   // ma thuoc
                        rdo.PRICE = listExpMestMedicine.PRICE;                            // don gia xuat
                        var EXP_AMOUNT = listExpMestMedicine.AMOUNT;                  // tong xuat
                        decimal? TOTAL_EXP_PRICE = EXP_AMOUNT * listExpMestMedicine.PRICE;

                        var listMobaImMest = listMobaImMests.Where(s => s.MOBA_EXP_MEST_ID == listExpMestMedicine.EXP_MEST_ID).Select(s => s.ID).ToList();
                        var listImpMestMedicine = listImpMestMedicines.Where(s => listMobaImMest.Contains(s.IMP_MEST_ID) && s.MEDICINE_ID == listExpMestMedicine.MEDICINE_ID).ToList();

                        decimal IMP_AMOUNT = 0;
                        decimal? TOTAL_IMP_PRICE = 0;
                        foreach (var ImpMestMedicine in listImpMestMedicine)
                        {
                            IMP_AMOUNT = ImpMestMedicine.AMOUNT;
                            TOTAL_IMP_PRICE = IMP_AMOUNT * listExpMestMedicine.PRICE;
                        }

                        rdo.TOTAL_AMOUNT = EXP_AMOUNT - IMP_AMOUNT;
                        rdo.TOTAL_PRICE = TOTAL_EXP_PRICE - TOTAL_IMP_PRICE;
                        listRdo.Add(rdo);
                    }

                    var listExpMestMaterialss = listExpMestMaterials.Where(s => s.EXP_MEST_ID == presCription.ID);
                    foreach (var listExpMestMaterial in listExpMestMaterialss)
                    {
                        rdo.GROUP_MANE = "VẬT TƯ";
                        rdo.SERVICE_TYPE_NAME = listExpMestMaterial.MATERIAL_TYPE_NAME;  // ten thuoc
                        rdo.SERVICE_TYPE_CODE = listExpMestMaterial.MATERIAL_TYPE_CODE;   // ma thuoc
                        rdo.PRICE = listExpMestMaterial.PRICE;                            // don gia xuat
                        var EXP_AMOUNT = listExpMestMaterial.AMOUNT;                  // tong xuat
                        decimal? TOTAL_EXP_PRICE = EXP_AMOUNT * listExpMestMaterial.PRICE;

                        var listMobaImMest = listMobaImMests.Where(s => s.MOBA_EXP_MEST_ID == listExpMestMaterial.EXP_MEST_ID).Select(s => s.ID).ToList();
                        var listImpMestMaterial = listImpMestMaterials.Where(s => listMobaImMest.Contains(s.IMP_MEST_ID) && s.MATERIAL_ID == listExpMestMaterial.MATERIAL_ID).ToList();

                        decimal IMP_AMOUNT = 0;
                        decimal? TOTAL_IMP_PRICE = 0;
                        foreach (var ImpMestMaterial in listImpMestMaterial)
                        {
                            IMP_AMOUNT = ImpMestMaterial.AMOUNT;
                            TOTAL_IMP_PRICE = IMP_AMOUNT * listExpMestMaterial.PRICE;
                        }

                        rdo.TOTAL_AMOUNT = EXP_AMOUNT - IMP_AMOUNT;
                        rdo.TOTAL_PRICE = TOTAL_EXP_PRICE - TOTAL_IMP_PRICE;
                        listRdo.Add(rdo);
                    }
                }

                listRdo = listRdo.GroupBy(g => new { g.GROUP_MANE, g.INTRUCTION_TIME, g.TREATMENT_CODE, g.SERVICE_TYPE_CODE, g.PRICE }).Select(s => new Mrs00426RDO
                {
                    GROUP_MANE = s.First().GROUP_MANE,
                    INTRUCTION_TIME = s.First().INTRUCTION_TIME,
                    TREATMENT_CODE = s.First().TREATMENT_CODE,
                    PATIENT_NAME = s.First().PATIENT_NAME,
                    SERVICE_TYPE_CODE = s.First().SERVICE_TYPE_CODE,
                    SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                    TOTAL_AMOUNT = s.Sum(a => a.TOTAL_AMOUNT),
                    TOTAL_PRICE = s.Sum(a => a.TOTAL_PRICE),
                    PRICE = s.First().PRICE
                }).ToList();

                listRdoGroupName = listRdo.GroupBy(g => g.GROUP_MANE).Select(s => new Mrs00426RDO
                {
                    GROUP_MANE = s.First().GROUP_MANE
                }).ToList();

                listRdoIntructionTime = listRdo.GroupBy(g => new { g.GROUP_MANE, g.INTRUCTION_TIME }).Select(s => new Mrs00426RDO
                {
                    GROUP_MANE = s.First().GROUP_MANE,
                    INTRUCTION_TIME = s.First().INTRUCTION_TIME
                }).ToList();

                listRdoTreatmentCode = listRdo.GroupBy(g => new { g.GROUP_MANE, g.INTRUCTION_TIME, g.TREATMENT_CODE, g.PATIENT_NAME }).Select(s => new Mrs00426RDO
                {
                    GROUP_MANE = s.First().GROUP_MANE,
                    INTRUCTION_TIME = s.First().INTRUCTION_TIME,
                    TREATMENT_CODE = s.First().TREATMENT_CODE,
                    PATIENT_NAME = s.First().PATIENT_NAME
                }).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
    }
}
