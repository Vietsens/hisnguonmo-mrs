using MOS.MANAGER.HisImpMest;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisMaterialType;
using Inventec.Common.Logging;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisImpMestType;

namespace MRS.Processor.Mrs00005
{
    class Mrs00005Processor : AbstractProcessor
    {
        Mrs00005Filter castFilter = null;
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<HIS_IMP_MEST> listImpMest = new List<HIS_IMP_MEST>();
        List<HIS_MATERIAL> listMaterial = new List<HIS_MATERIAL>();
        List<HIS_MEDICINE> listMedicine = new List<HIS_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> listMaterialFiltered = new List<V_HIS_IMP_MEST_MATERIAL>();
        //List<V_HIS_IMP_MEST_MEDICINE> listMedicineFiltered = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> listChemicalSubstanceFiltered = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<HIS_MATERIAL_TYPE> listMaterialType = new List<HIS_MATERIAL_TYPE>();
        List<Mrs00005RDO> ListRdo = new List<Mrs00005RDO>();
        Dictionary<string, Mrs00005RDO> dicMediMate = new Dictionary<string, Mrs00005RDO>();
        List<HIS_IMP_MEST_TYPE> listImpMestType = new List<HIS_IMP_MEST_TYPE>();
        List<HIS_MEDICAL_CONTRACT> listMedicalContract = new List<HIS_MEDICAL_CONTRACT>();

        bool takeMedicineData = true;
        bool takeMaterialData = true;
        bool takeChemicalSubstanceData = true;
        const string THUOC = "THUOC";
        const string VATTU = "VATTU";
        const string HOACHAT = "HOACHAT";
        string KeyGroupImp = "{0}_{1}_{2}_{3}_{4}";

        public Mrs00005Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00005Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                this.castFilter = (Mrs00005Filter)this.reportFilter;
                if (castFilter.IS_MEDICINE != true && (castFilter.IS_MATERIAL == true || castFilter.IS_CHEMICAL_SUBSTANCE == true))
                {
                    takeMedicineData = false;
                }
                if (castFilter.IS_MATERIAL != true && (castFilter.IS_MEDICINE == true || castFilter.IS_CHEMICAL_SUBSTANCE == true))
                {
                    takeMaterialData = false;
                }

                if (castFilter.IS_CHEMICAL_SUBSTANCE != true && (castFilter.IS_MEDICINE == true || castFilter.IS_MATERIAL == true))
                {
                    takeChemicalSubstanceData = false;
                }

                CommonParam getParam = new CommonParam();
                if (castFilter.IMP_TIME_FROM != null || castFilter.IMP_TIME_TO != null)
                {
                    if (takeMedicineData)
                    {
                        HisImpMestMedicineViewFilterQuery medicineFilter = new HisImpMestMedicineViewFilterQuery();
                        medicineFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                        medicineFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                        medicineFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                        medicineFilter.IMP_TIME_FROM = castFilter.IMP_TIME_FROM;
                        medicineFilter.IMP_TIME_TO = castFilter.IMP_TIME_TO;
                        medicineFilter.IMP_MEST_TYPE_ID = castFilter.IMP_MEST_TYPE_ID;
                        listImpMestMedicine = new HisImpMestMedicineManager(getParam).GetView(medicineFilter);
                    }
                    if (takeMaterialData || takeChemicalSubstanceData)
                    {
                        HisImpMestMaterialViewFilterQuery materialFilter = new HisImpMestMaterialViewFilterQuery();
                        materialFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                        materialFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                        materialFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                        materialFilter.IMP_TIME_FROM = castFilter.IMP_TIME_FROM;
                        materialFilter.IMP_TIME_TO = castFilter.IMP_TIME_TO;
                        materialFilter.IMP_MEST_TYPE_ID = castFilter.IMP_MEST_TYPE_ID;
                        listImpMestMaterial = new HisImpMestMaterialManager(getParam).GetView(materialFilter);
                    }
                }
                GetMediMate(listImpMestMedicine, listImpMestMaterial, ref listMedicine, ref listMaterial);
                GetImpMest(listImpMestMedicine, listImpMestMaterial, ref listImpMest);
                if (castFilter.EXPIRED_TIME_FROM != null || castFilter.EXPIRED_TIME_TO != null)
                {
                    if (takeMedicineData)
                    {
                        listImpMestMedicine = new HisManagerSql().GetMedi(castFilter);
                    }
                    if (takeMaterialData || takeChemicalSubstanceData)
                    {
                        listImpMestMaterial = new HisManagerSql().GetMate(castFilter);
                    }
                }
                listMaterialType = new HisMaterialTypeManager().Get(new HisMaterialTypeFilterQuery()) ?? new List<HIS_MATERIAL_TYPE>();
                if (takeMaterialData)
                {
                    listMaterialFiltered = listImpMestMaterial.Where(o => listMaterialType.Exists(p => p.IS_CHEMICAL_SUBSTANCE != 1 && p.ID == o.MATERIAL_TYPE_ID)).ToList();
                }
                if (takeChemicalSubstanceData)
                {
                    listChemicalSubstanceFiltered = listImpMestMaterial.Where(o => listMaterialType.Exists(p => p.IS_CHEMICAL_SUBSTANCE == 1 && p.ID == o.MATERIAL_TYPE_ID)).ToList();
                }
                GetImpMestType();
                GetMedicalContract();

                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetMedicalContract()
        {
            listMedicalContract = new HisManagerSql().GetMedicalContract(); 
        }

        private void GetImpMestType()
        {
            listImpMestType = new HisImpMestTypeManager().Get(new HisImpMestTypeFilterQuery());
        }
        private void GetMediMate(List<V_HIS_IMP_MEST_MEDICINE> _listImpMestMedicine, List<V_HIS_IMP_MEST_MATERIAL> _listImpMestMaterial, ref List<HIS_MEDICINE> _listMedicine, ref  List<HIS_MATERIAL> _listMaterial)
        {
            try
            {
                List<long> medicineIds = new List<long>();
                if (_listImpMestMedicine != null)
                {
                    medicineIds.AddRange(_listImpMestMedicine.Select(o => o.MEDICINE_ID).ToList());
                }

                medicineIds = medicineIds.Distinct().ToList();

                if (medicineIds != null && medicineIds.Count > 0)
                {
                    var skip = 0;
                    while (medicineIds.Count - skip > 0)
                    {
                        var limit = medicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisMedicineFilterQuery Medicinefilter = new HisMedicineFilterQuery();
                        Medicinefilter.IDs = limit;
                        var MedicineSub = new HisMedicineManager().Get(Medicinefilter);
                        _listMedicine.AddRange(MedicineSub);
                    }
                }

                List<long> materialIds = new List<long>();
                if (_listImpMestMaterial != null)
                {
                    materialIds.AddRange(_listImpMestMaterial.Select(o => o.MATERIAL_ID).ToList());
                }

                materialIds = materialIds.Distinct().ToList();

                if (materialIds != null && materialIds.Count > 0)
                {
                    var skip = 0;
                    while (materialIds.Count - skip > 0)
                    {
                        var limit = materialIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisMaterialFilterQuery Materialfilter = new HisMaterialFilterQuery();
                        Materialfilter.IDs = limit;
                        var MaterialSub = new HisMaterialManager().Get(Materialfilter);
                        _listMaterial.AddRange(MaterialSub);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

            }
        }
        private void GetImpMest(List<V_HIS_IMP_MEST_MEDICINE> _listImpMestMedicine, List<V_HIS_IMP_MEST_MATERIAL> _listImpMestMaterial, ref List<HIS_IMP_MEST> _listImpMest)
        {
            try
            {
                List<long> impMestIds = new List<long>();
                if (_listImpMestMedicine != null)
                {
                    impMestIds.AddRange(_listImpMestMedicine.Select(o => o.IMP_MEST_ID).ToList());
                }
                if (_listImpMestMaterial != null)
                {
                    impMestIds.AddRange(_listImpMestMaterial.Select(o => o.IMP_MEST_ID).ToList());
                }

                impMestIds = impMestIds.Distinct().ToList();

                if (impMestIds != null && impMestIds.Count > 0)
                {
                    var skip = 0;
                    while (impMestIds.Count - skip > 0)
                    {
                        var limit = impMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisImpMestFilterQuery impMestfilter = new HisImpMestFilterQuery();
                        impMestfilter.IDs = limit;
                        var listImpMestSub = new HisImpMestManager().Get(impMestfilter);
                        _listImpMest.AddRange(listImpMestSub);
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
                List<Mrs00005RDO> listRdoTemp = new List<Mrs00005RDO>();
                foreach (var imm in listImpMestMedicine)
                {
                    Mrs00005RDO rdo = new Mrs00005RDO(imm, listMedicine, listImpMest);
                    rdo.TYPE = THUOC;
                    listRdoTemp.Add(rdo);
                }
                foreach (var imm in listChemicalSubstanceFiltered)
                {
                    Mrs00005RDO rdo = new Mrs00005RDO(imm, listMaterial, listImpMest);
                    rdo.TYPE = HOACHAT;
                    listRdoTemp.Add(rdo);
                }
                foreach (var imm in listMaterialFiltered)
                {
                    Mrs00005RDO rdo = new Mrs00005RDO(imm, listMaterial, listImpMest);
                    rdo.TYPE = VATTU;
                    listRdoTemp.Add(rdo);
                }

                ListRdo = listRdoTemp.OrderBy(o => o.IMP_TIME).ToList();
                GroupByKey();

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GroupByKey()
        {
            try
            {
                if (this.dicDataFilter.ContainsKey("KEY_GROUP_IMP") && this.dicDataFilter["KEY_GROUP_IMP"] != null)
                {
                    KeyGroupImp = this.dicDataFilter["KEY_GROUP_IMP"].ToString();
                }
                foreach (var item in ListRdo)
                {
                    
                    if (item.V_HIS_IMP_MEST_MEDICINE != null && item.V_HIS_IMP_MEST_MEDICINE.ID > 0)
                    {
                        ProcessGroupByMedicine(item);

                    }
                    if (item.V_HIS_IMP_MEST_MATERIAL != null && item.V_HIS_IMP_MEST_MATERIAL.ID > 0)
                    {

                        if (item.TYPE == HOACHAT)
                        {
                            ProcessGroupByChemicalSubstance(item);
                        }
                        else if (item.TYPE == VATTU)
                        {
                            ProcessGroupByMaterial(item);
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessGroupByMedicine(Mrs00005RDO item)
        {
            string valueGroup = "";
            valueGroup = string.Format(KeyGroupImp, THUOC, item.V_HIS_IMP_MEST_MEDICINE.MEDICINE_TYPE_ID, item.V_HIS_IMP_MEST_MEDICINE.PRICE ?? 0, item.V_HIS_IMP_MEST_MEDICINE.VAT_RATIO,item.IMP_MEST_ID);
            if (!dicMediMate.ContainsKey(valueGroup))
            {
                Mrs00005RDO rdo = new Mrs00005RDO(item.V_HIS_IMP_MEST_MEDICINE, listMedicine, listImpMest);
                var impMestType = listImpMestType.FirstOrDefault(o => o.ID == item.IMP_MEST_TYPE_ID);
                if (impMestType != null)
                {
                    rdo.IMP_MEST_TYPE_NAME = impMestType.IMP_MEST_TYPE_NAME;
                    rdo.IMP_MEST_TYPE_CODE = impMestType.IMP_MEST_TYPE_CODE;
                }
                var medicalContract = listMedicalContract.FirstOrDefault(o => o.ID == item.MEDICAL_CONTRACT_ID);
                if (medicalContract != null)
                {
                    rdo.MEDICAL_CONTRACT_CODE = medicalContract.MEDICAL_CONTRACT_CODE;
                    rdo.MEDICAL_CONTRACT_NAME = medicalContract.MEDICAL_CONTRACT_NAME;
                }
                dicMediMate[valueGroup] = rdo;
            }
            else
            {
                dicMediMate[valueGroup].AMOUNT += item.AMOUNT;
                dicMediMate[valueGroup].TOTAL_PRICE += item.TOTAL_PRICE;
                dicMediMate[valueGroup].TOTAL_VAT += item.TOTAL_VAT;

            }
        }

        private void ProcessGroupByMaterial(Mrs00005RDO item)
        {
            string valueGroup = "";
            valueGroup = string.Format(KeyGroupImp, VATTU, item.V_HIS_IMP_MEST_MATERIAL.MATERIAL_TYPE_ID, item.V_HIS_IMP_MEST_MATERIAL.PRICE ?? 0, item.V_HIS_IMP_MEST_MATERIAL.VAT_RATIO, item.IMP_MEST_ID);
            if (!dicMediMate.ContainsKey(valueGroup))
            {
                Mrs00005RDO rdo = new Mrs00005RDO(item.V_HIS_IMP_MEST_MATERIAL, listMaterial, listImpMest);
                var impMestType = listImpMestType.FirstOrDefault(o => o.ID == item.IMP_MEST_TYPE_ID);
                if (impMestType != null)
                {
                    rdo.IMP_MEST_TYPE_NAME = impMestType.IMP_MEST_TYPE_NAME;
                    rdo.IMP_MEST_TYPE_CODE = impMestType.IMP_MEST_TYPE_CODE;
                }
                var medicalContract = listMedicalContract.FirstOrDefault(o => o.ID == item.MEDICAL_CONTRACT_ID);
                if (medicalContract != null)
                {
                    rdo.MEDICAL_CONTRACT_CODE = medicalContract.MEDICAL_CONTRACT_CODE;
                    rdo.MEDICAL_CONTRACT_NAME = medicalContract.MEDICAL_CONTRACT_NAME;
                }
                dicMediMate[valueGroup] = rdo;
            }
            else
            {
                dicMediMate[valueGroup].AMOUNT += item.AMOUNT;
                dicMediMate[valueGroup].TOTAL_PRICE += item.TOTAL_PRICE;
                dicMediMate[valueGroup].TOTAL_VAT += item.TOTAL_VAT;

            }
        }

        private void ProcessGroupByChemicalSubstance(Mrs00005RDO item)
        {
            string valueGroup = "";
            valueGroup = string.Format(KeyGroupImp, HOACHAT, item.V_HIS_IMP_MEST_MATERIAL.MATERIAL_TYPE_ID, item.V_HIS_IMP_MEST_MATERIAL.PRICE ?? 0, item.V_HIS_IMP_MEST_MATERIAL.VAT_RATIO, item.IMP_MEST_ID);
            if (!dicMediMate.ContainsKey(valueGroup))
            {
                Mrs00005RDO rdo = new Mrs00005RDO(item.V_HIS_IMP_MEST_MATERIAL, listMaterial, listImpMest);
                var impMestType = listImpMestType.FirstOrDefault(o => o.ID == item.IMP_MEST_TYPE_ID);
                if (impMestType != null)
                {
                    rdo.IMP_MEST_TYPE_NAME = impMestType.IMP_MEST_TYPE_NAME;
                    rdo.IMP_MEST_TYPE_CODE = impMestType.IMP_MEST_TYPE_CODE;
                }
                var medicalContract = listMedicalContract.FirstOrDefault(o => o.ID == item.MEDICAL_CONTRACT_ID);
                if (medicalContract != null)
                {
                    rdo.MEDICAL_CONTRACT_CODE = medicalContract.MEDICAL_CONTRACT_CODE;
                    rdo.MEDICAL_CONTRACT_NAME = medicalContract.MEDICAL_CONTRACT_NAME;
                }
                dicMediMate[valueGroup] = rdo;
            }
            else
            {
                dicMediMate[valueGroup].AMOUNT += item.AMOUNT;
                dicMediMate[valueGroup].TOTAL_PRICE += item.TOTAL_PRICE;
                dicMediMate[valueGroup].TOTAL_VAT += item.TOTAL_VAT;

            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.IMP_TIME_FROM ?? 0));
                dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.IMP_TIME_TO ?? 0));
                dicSingleTag.Add("EXPIRED_DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.EXPIRED_TIME_FROM ?? 0));
                dicSingleTag.Add("EXPIRED_DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.EXPIRED_TIME_TO ?? 0));
                dicSingleTag.Add("MEDI_STOCK_NAME", (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == castFilter.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME ?? string.Join(" - ", (HisMediStockCFG.HisMediStocks.Where(o => (castFilter.MEDI_STOCK_IDs ?? new List<long>()).Contains(o.ID)) ?? new List<V_HIS_MEDI_STOCK>()).Select(o => o.MEDI_STOCK_NAME).ToList()));
                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "ImpMest", ListRdo.GroupBy(o => o.IMP_MEST_CODE).Select(p => p.First()).ToList());
                objectTag.AddRelationship(store, "ImpMest", "Report", "IMP_MEST_ID", "IMP_MEST_ID");
                objectTag.AddObjectData(store, "MediMate", dicMediMate.Values.ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
