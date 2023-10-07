using AutoMapper;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMedicineType;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00419
{
    class Mrs00419Processor : AbstractProcessor
    {
        List<Mrs00419RDO> ListRdo = new List<Mrs00419RDO>();

        List<V_HIS_MEDICINE> listMedicines = new List<V_HIS_MEDICINE>();
        List<V_HIS_MATERIAL> listMaterials = new List<V_HIS_MATERIAL>();

        List<HIS_IMP_MEST> listImpMests = new List<HIS_IMP_MEST>();
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterials = new List<V_HIS_IMP_MEST_MATERIAL>();

        List<V_HIS_EXP_MEST> listExpMests = new List<V_HIS_EXP_MEST>();
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterials = new List<V_HIS_EXP_MEST_MATERIAL>();

        List<V_HIS_EXP_MEST> listPrescriptions = new List<V_HIS_EXP_MEST>();
        List<V_HIS_IMP_MEST> listMobaImpMests = new List<V_HIS_IMP_MEST>();

        public string SERVICE_TYPE_ID { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public string SERVICE_TYPE_CODE { get; set; }
        public string CONCENTRA { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string NATIONAL_NAME { get; set; }
        public string DEPARTMENT_NAME = "";
        public string DEPARTMENT_CODE = "";

        List<long> PRES_EXP_MEST_TYPE_IDs = new List<long>
        {
            //IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DNGT,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT
        };

        public Mrs00419Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00419Filter);
        }

        protected override bool GetData()
        {
            var filter = ((Mrs00419Filter)reportFilter);
            bool result = true;
            try
            {
                HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery();
                departmentFilter.ID = filter.DEPARTMENT_ID;
                var listDepartments = new MOS.MANAGER.HisDepartment.HisDepartmentManager(param).Get(departmentFilter);
                if (IsNotNullOrEmpty(listDepartments))
                {
                    DEPARTMENT_NAME = listDepartments.First().DEPARTMENT_NAME;
                    DEPARTMENT_CODE = listDepartments.First().DEPARTMENT_CODE;
                }

                #region thuốc
                if (IsNotNull(filter.MEDICINE_TYPE_ID))
                {
                    HisMedicineTypeViewFilterQuery medicineTypeViewFilter = new HisMedicineTypeViewFilterQuery();
                    medicineTypeViewFilter.ID = filter.MEDICINE_TYPE_ID;
                    var listMedicineTypes = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(param).GetView(medicineTypeViewFilter);
                    if (IsNotNullOrEmpty(listMedicineTypes))
                    {
                        SERVICE_TYPE_ID = listMedicineTypes.First().ID.ToString();
                        SERVICE_TYPE_NAME = listMedicineTypes.First().MEDICINE_TYPE_NAME;
                        SERVICE_TYPE_CODE = listMedicineTypes.First().MEDICINE_TYPE_CODE;
                        CONCENTRA = listMedicineTypes.First().CONCENTRA;
                        SERVICE_UNIT_NAME = listMedicineTypes.First().SERVICE_UNIT_NAME;
                        NATIONAL_NAME = listMedicineTypes.First().NATIONAL_NAME;
                    }

                    // xuất thuốc cho bn
                    HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
                    expMestMedicineViewFilter.EXP_TIME_FROM = filter.TIME_FROM;
                    expMestMedicineViewFilter.EXP_TIME_TO = filter.TIME_TO;
                    expMestMedicineViewFilter.REQ_DEPARTMENT_ID = filter.DEPARTMENT_ID;
                    expMestMedicineViewFilter.MEDICINE_TYPE_ID = filter.MEDICINE_TYPE_ID;
                    expMestMedicineViewFilter.EXP_MEST_TYPE_IDs = PRES_EXP_MEST_TYPE_IDs;
                    expMestMedicineViewFilter.IS_EXPORT = true;
                    listExpMestMedicines = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineViewFilter);

                    var skip = 0;
                    while (listExpMestMedicines.Count - skip > 0)
                    {
                        var listIds = listExpMestMedicines.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisExpMestViewFilterQuery prescriptionViewFilter = new HisExpMestViewFilterQuery();
                        prescriptionViewFilter.IDs = listIds.Select(s => s.EXP_MEST_ID ?? 0).ToList();
                        prescriptionViewFilter.REQ_DEPARTMENT_ID = filter.DEPARTMENT_ID;
                        listPrescriptions.AddRange(new HisExpMestManager(param).GetView(prescriptionViewFilter));

                        HisMedicineViewFilterQuery medicineViewFilter = new HisMedicineViewFilterQuery();
                        medicineViewFilter.IDs = listIds.Select(s => s.MEDICINE_ID ?? 0).ToList();
                        listMedicines.AddRange(new MOS.MANAGER.HisMedicine.HisMedicineManager(param).GetView(medicineViewFilter));
                    }

                    // thuốc bệnh nhân trả
                    HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery();
                    impMestMedicineViewFilter.IMP_TIME_FROM = filter.TIME_FROM;
                    impMestMedicineViewFilter.IMP_TIME_TO = filter.TIME_TO;
                    impMestMedicineViewFilter.MEDICINE_TYPE_ID = filter.MEDICINE_TYPE_ID;
                    impMestMedicineViewFilter.IMP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__THT;
                    listImpMestMedicines = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(param).GetView(impMestMedicineViewFilter);

                    skip = 0;
                    while (listImpMestMedicines.Count - skip > 0)
                    {
                        var listIds = listImpMestMedicines.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisImpMestViewFilterQuery mobaImpMestViewFilter = new HisImpMestViewFilterQuery();
                        mobaImpMestViewFilter.IDs = listIds.Select(s => s.IMP_MEST_ID).ToList();
                        mobaImpMestViewFilter.REQ_DEPARTMENT_ID = filter.DEPARTMENT_ID;
                        listMobaImpMests.AddRange(new HisImpMestManager(param).GetView(mobaImpMestViewFilter));

                        HisMedicineViewFilterQuery medicineViewFilter = new HisMedicineViewFilterQuery();
                        medicineViewFilter.IDs = listIds.Select(s => s.MEDICINE_ID).ToList();
                        listMedicines.AddRange(new MOS.MANAGER.HisMedicine.HisMedicineManager(param).GetView(medicineViewFilter));
                    }

                    listImpMestMedicines = listImpMestMedicines.Where(w => listMobaImpMests.Select(s => s.ID).Contains(w.IMP_MEST_ID)).ToList();
                    listMedicines.Distinct();

                    if (IsNotNullOrEmpty(listMobaImpMests))
                    {
                        skip = 0;
                        while (listMobaImpMests.Count - skip > 0)
                        {
                            var listIds = listMobaImpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                            HisExpMestViewFilterQuery expMestViewFilter = new HisExpMestViewFilterQuery();
                            expMestViewFilter.IDs = listIds.Select(s => s.MOBA_EXP_MEST_ID.Value).ToList();
                            expMestViewFilter.EXP_MEST_TYPE_IDs = PRES_EXP_MEST_TYPE_IDs;
                            listExpMests.AddRange(new HisExpMestManager(param).GetView(expMestViewFilter));
                        }
                    }

                }
                #endregion
                #region vật tư
                else if (IsNotNull(filter.MATERIAL_TYPE_ID))
                {
                    HisMaterialTypeViewFilterQuery materialTypeViewFilter = new HisMaterialTypeViewFilterQuery();
                    materialTypeViewFilter.ID = filter.MATERIAL_TYPE_ID;
                    var listMaterialTypes = new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager(param).GetView(materialTypeViewFilter);

                    if (IsNotNullOrEmpty(listMaterialTypes))
                    {
                        SERVICE_TYPE_ID = listMaterialTypes.First().ID.ToString();
                        SERVICE_TYPE_NAME = listMaterialTypes.First().MATERIAL_TYPE_NAME;
                        SERVICE_TYPE_CODE = listMaterialTypes.First().MATERIAL_TYPE_CODE;
                        CONCENTRA = listMaterialTypes.First().CONCENTRA;
                        SERVICE_UNIT_NAME = listMaterialTypes.First().SERVICE_UNIT_NAME;
                        NATIONAL_NAME = listMaterialTypes.First().NATIONAL_NAME;
                    }

                    // xuất vật tư cho bn
                    HisExpMestMaterialViewFilterQuery expMestMaterialViewFilter = new HisExpMestMaterialViewFilterQuery();
                    expMestMaterialViewFilter.EXP_TIME_FROM = filter.TIME_FROM;
                    expMestMaterialViewFilter.EXP_TIME_TO = filter.TIME_TO;
                    //expMestMaterialViewFilter.REQ_DEPARTMENT_ID = filter.DEPARTMENT_ID; 
                    expMestMaterialViewFilter.MATERIAL_TYPE_ID = filter.MATERIAL_TYPE_ID;
                    expMestMaterialViewFilter.IS_EXPORT = true;
                    //expMestMaterialViewFilter.EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK; 
                    listExpMestMaterials = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(param).GetView(expMestMaterialViewFilter);

                    var skip = 0;
                    while (listExpMestMaterials.Count - skip > 0)
                    {
                        var listIds = listExpMestMaterials.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisExpMestViewFilterQuery prescriptionViewFilter = new HisExpMestViewFilterQuery();
                        prescriptionViewFilter.IDs = listIds.Select(s => s.EXP_MEST_ID ?? 0).ToList();
                        prescriptionViewFilter.REQ_DEPARTMENT_ID = filter.DEPARTMENT_ID;
                        listPrescriptions.AddRange(new HisExpMestManager(param).GetView(prescriptionViewFilter));

                        HisMaterialViewFilterQuery materialViewFilter = new HisMaterialViewFilterQuery();
                        materialViewFilter.IDs = listIds.Select(s => s.MATERIAL_ID ?? 0).ToList();
                        listMaterials.AddRange(new MOS.MANAGER.HisMaterial.HisMaterialManager(param).GetView(materialViewFilter));
                    }

                    listExpMestMaterials = listExpMestMaterials.Where(w => listPrescriptions.Select(s => s.ID).Contains(w.EXP_MEST_ID ?? 0)).ToList();

                    // thuốc bệnh nhân trả
                    HisImpMestMaterialViewFilterQuery impMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery();
                    impMestMaterialViewFilter.IMP_TIME_FROM = filter.TIME_FROM;
                    impMestMaterialViewFilter.IMP_TIME_TO = filter.TIME_TO;
                    impMestMaterialViewFilter.MATERIAL_TYPE_ID = filter.MATERIAL_TYPE_ID;
                    impMestMaterialViewFilter.IMP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__THT;
                    listImpMestMaterials = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(param).GetView(impMestMaterialViewFilter);

                    skip = 0;
                    while (listImpMestMaterials.Count - skip > 0)
                    {
                        var listIds = listImpMestMaterials.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisImpMestViewFilterQuery mobaImpMestViewFilter = new HisImpMestViewFilterQuery();
                        mobaImpMestViewFilter.IDs = listIds.Select(s => s.IMP_MEST_ID).ToList();
                        mobaImpMestViewFilter.REQ_DEPARTMENT_ID = filter.DEPARTMENT_ID;
                        listMobaImpMests.AddRange(new HisImpMestManager(param).GetView(mobaImpMestViewFilter));

                        HisMedicineViewFilterQuery medicineViewFilter = new HisMedicineViewFilterQuery();
                        medicineViewFilter.IDs = listIds.Select(s => s.MATERIAL_ID).ToList();
                        listMedicines.AddRange(new MOS.MANAGER.HisMedicine.HisMedicineManager(param).GetView(medicineViewFilter));
                    }

                    listImpMestMaterials = listImpMestMaterials.Where(w => listMobaImpMests.Select(s => s.ID).Contains(w.IMP_MEST_ID)).ToList();
                    listMaterials.Distinct();

                    if (IsNotNullOrEmpty(listMobaImpMests))
                    {
                        skip = 0;
                        while (listMobaImpMests.Count - skip > 0)
                        {
                            var listIds = listMobaImpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                            HisExpMestViewFilterQuery expMestViewFilter = new HisExpMestViewFilterQuery();
                            expMestViewFilter.IDs = listIds.Select(s => s.MOBA_EXP_MEST_ID.Value).ToList();
                            expMestViewFilter.EXP_MEST_TYPE_IDs = PRES_EXP_MEST_TYPE_IDs;
                            listExpMests.AddRange(new HisExpMestManager(param).GetView(expMestViewFilter));
                        }
                    }

                }
                #endregion
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
                var filter = ((Mrs00419Filter)reportFilter);

                #region thuốc
                if (IsNotNull(filter.MEDICINE_TYPE_ID))
                {
                    foreach (var medi in listExpMestMedicines)
                    {
                        var pres = listPrescriptions.Where(w => w.ID == medi.EXP_MEST_ID).ToList();
                        if (IsNotNullOrEmpty(pres))
                        {
                            var medicine = listMedicines.Where(s => s.ID == medi.MEDICINE_ID).ToList();
                            if (IsNotNullOrEmpty(medicine))
                            {
                                var rdo = new Mrs00419RDO();
                                rdo.IMP_EXP_TIME = pres.First().FINISH_TIME;
                                rdo.EXPIRED_DATE = medicine.First().EXPIRED_DATE;
                                rdo.IMP_EXP_MEST_CODE = pres.First().EXP_MEST_CODE;
                                rdo.PACKAGE_NUMBER = medicine.First().PACKAGE_NUMBER;
                                rdo.TREATMENT_CODE = pres.First().TDL_TREATMENT_CODE;
                                rdo.PATIENT_NAME = pres.First().TDL_PATIENT_NAME;
                                rdo.EXP_AMOUNT = medi.AMOUNT;

                                ListRdo.Add(rdo);
                            }
                        }
                    }

                    foreach (var medi in listImpMestMedicines)
                    {
                        var moba = listMobaImpMests.Where(w => w.ID == medi.IMP_MEST_ID).ToList();
                        if (IsNotNullOrEmpty(moba))
                        {
                            var expMest = listExpMests.Where(w => w.ID == moba.First().ID && PRES_EXP_MEST_TYPE_IDs.Contains(w.EXP_MEST_TYPE_ID)).ToList();
                            if (IsNotNullOrEmpty(expMest))
                            {
                                if (expMest.First().REQ_DEPARTMENT_ID == filter.DEPARTMENT_ID)
                                {
                                    var medicine = listMedicines.Where(s => s.ID == medi.MEDICINE_ID).ToList();
                                    if (IsNotNullOrEmpty(medicine))
                                    {
                                        var rdo = new Mrs00419RDO();
                                        rdo.IMP_EXP_TIME = moba.First().IMP_TIME;
                                        rdo.EXPIRED_DATE = medicine.First().EXPIRED_DATE;
                                        rdo.IMP_EXP_MEST_CODE = moba.First().IMP_MEST_CODE;
                                        rdo.PACKAGE_NUMBER = medicine.First().PACKAGE_NUMBER;
                                        rdo.TREATMENT_CODE = expMest.First().TDL_TREATMENT_CODE;
                                        rdo.PATIENT_NAME = expMest.First().TDL_PATIENT_NAME;
                                        rdo.IMP_AMOUNT = medi.AMOUNT;

                                        ListRdo.Add(rdo);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                #region vật tư
                else if (IsNotNull(filter.MATERIAL_TYPE_ID))
                {
                    foreach (var mate in listExpMestMaterials)
                    {
                        var pres = listPrescriptions.Where(w => w.ID == mate.EXP_MEST_ID).ToList();
                        if (IsNotNullOrEmpty(pres))
                        {
                            var material = listMaterials.Where(s => s.ID == mate.MATERIAL_ID).ToList();
                            if (IsNotNullOrEmpty(material))
                            {
                                var rdo = new Mrs00419RDO();
                                rdo.IMP_EXP_TIME = pres.First().FINISH_TIME;
                                rdo.EXPIRED_DATE = material.First().EXPIRED_DATE;
                                rdo.IMP_EXP_MEST_CODE = pres.First().EXP_MEST_CODE;
                                rdo.PACKAGE_NUMBER = material.First().PACKAGE_NUMBER;
                                rdo.TREATMENT_CODE = pres.First().TDL_TREATMENT_CODE;
                                rdo.PATIENT_NAME = pres.First().TDL_PATIENT_NAME;
                                rdo.EXP_AMOUNT = mate.AMOUNT;

                                ListRdo.Add(rdo);
                            }
                        }
                    }

                    foreach (var mate in listImpMestMaterials)
                    {
                        var moba = listMobaImpMests.Where(w => w.ID == mate.IMP_MEST_ID).ToList();
                        if (IsNotNullOrEmpty(moba))
                        {
                            var expMest = listExpMests.Where(w => w.ID == moba.First().MOBA_EXP_MEST_ID && PRES_EXP_MEST_TYPE_IDs.Contains(w.EXP_MEST_TYPE_ID)).ToList();
                            if (IsNotNullOrEmpty(expMest))
                            {
                                if (expMest.First().REQ_DEPARTMENT_ID == filter.DEPARTMENT_ID)
                                {
                                    var material = listMaterials.Where(s => s.ID == mate.MATERIAL_ID).ToList();
                                    if (IsNotNullOrEmpty(material))
                                    {
                                        var rdo = new Mrs00419RDO();
                                        rdo.IMP_EXP_TIME = moba.First().IMP_TIME;
                                        rdo.EXPIRED_DATE = material.First().EXPIRED_DATE;
                                        rdo.IMP_EXP_MEST_CODE = moba.First().IMP_MEST_CODE;
                                        rdo.PACKAGE_NUMBER = material.First().PACKAGE_NUMBER;
                                        rdo.TREATMENT_CODE = expMest.First().TDL_TREATMENT_CODE;
                                        rdo.PATIENT_NAME = expMest.First().TDL_PATIENT_NAME;
                                        rdo.IMP_AMOUNT = mate.AMOUNT;

                                        ListRdo.Add(rdo);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                if (((Mrs00419Filter)reportFilter).TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00419Filter)reportFilter).TIME_FROM));
                }
                if (((Mrs00419Filter)reportFilter).TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00419Filter)reportFilter).TIME_TO));
                }

                dicSingleTag.Add("SERVICE_TYPE_ID", SERVICE_TYPE_ID);
                dicSingleTag.Add("SERVICE_TYPE_NAME", SERVICE_TYPE_NAME);
                dicSingleTag.Add("SERVICE_TYPE_CODE", SERVICE_TYPE_CODE);
                dicSingleTag.Add("CONCENTRA", CONCENTRA);
                dicSingleTag.Add("SERVICE_UNIT_NAME", SERVICE_UNIT_NAME);
                dicSingleTag.Add("NATIONAL_NAME", NATIONAL_NAME);
                dicSingleTag.Add("DEPARTMENT_NAME", DEPARTMENT_NAME);
                dicSingleTag.Add("DEPARTMENT_CODE", DEPARTMENT_CODE);

                objectTag.AddObjectData(store, "Rdo", ListRdo.OrderBy(o => o.IMP_EXP_TIME).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
