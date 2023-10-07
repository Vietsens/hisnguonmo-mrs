using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisBlood;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestBlood;
using MOS.MANAGER.HisBloodType;
using MOS.MANAGER.HisBid;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00356
{
    class Mrs00356Processor : AbstractProcessor
    {
        Mrs00356Filter castFilter = null;
        List<Mrs00356RDO> ListRdo = new List<Mrs00356RDO>();
        List<Mrs00356RDO> ListRdoGroup = new List<Mrs00356RDO>();
        List<Mrs00356RDO> ListRdoParent = new List<Mrs00356RDO>();

        List<V_HIS_IMP_MEST> listImpMests = new List<V_HIS_IMP_MEST>();
        List<V_HIS_IMP_MEST> listManuImpMests = new List<V_HIS_IMP_MEST>();
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterials = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<V_HIS_IMP_MEST_BLOOD> listImpMestBloods = new List<V_HIS_IMP_MEST_BLOOD>();

        List<V_HIS_MEDICINE_TYPE> listMedicineTypes = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MATERIAL_TYPE> listMaterialTypes = new List<V_HIS_MATERIAL_TYPE>();
        List<V_HIS_BLOOD_TYPE> listBloodTypes = new List<V_HIS_BLOOD_TYPE>();

        List<V_HIS_MEDICINE> listMedicines = new List<V_HIS_MEDICINE>();
        List<V_HIS_MATERIAL> listMaterials = new List<V_HIS_MATERIAL>();
        List<V_HIS_BLOOD> listBloods = new List<V_HIS_BLOOD>();

        public string ListMediStock = "";

        public const long GROUP_MEDI = 1;
        public const long GROUP_MATE = 2;
        public const long GROUP_BLOD = 3;

        public Mrs00356Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00356Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                this.castFilter = (Mrs00356Filter)this.reportFilter;

                // v_his_imp_mest
                HisImpMestViewFilterQuery impMestViewFilter = new HisImpMestViewFilterQuery();
                impMestViewFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                impMestViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                impMestViewFilter.IMP_TIME_TO = castFilter.TIME_TO;
                impMestViewFilter.IMP_MEST_STT_IDs = new List<long> { IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT };
                impMestViewFilter.IMP_MEST_TYPE_IDs = new List<long> { IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC };
                listManuImpMests = new MOS.MANAGER.HisImpMest.HisImpMestManager(param).GetView(impMestViewFilter);

                if (IsNotNullOrEmpty(listManuImpMests))
                {
                    var skip = 0;

                    // v_his_imp_mét_medicine
                    if (castFilter.IS_MEDICINE)
                    {
                        while (listManuImpMests.Count - skip > 0)
                        {
                            var listIDs = listManuImpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery()
                            {
                                IMP_MEST_IDs = listIDs.Select(s => s.ID).ToList()
                            };
                            var listImpMestMedicine = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(param).GetView(impMestMedicineViewFilter);
                            listImpMestMedicines.AddRange(listImpMestMedicine);
                        }
                        skip = 0;
                        while (listImpMestMedicines.Count - skip > 0)
                        {
                            var listIDs = listImpMestMedicines.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisMedicineViewFilterQuery medicineViewFilter = new HisMedicineViewFilterQuery();
                            medicineViewFilter.IDs = listIDs.Select(s => s.MEDICINE_ID).Distinct().ToList();
                            var listMedicine = new MOS.MANAGER.HisMedicine.HisMedicineManager(param).GetView(medicineViewFilter);
                            listMedicines.AddRange(listMedicine);
                        }

                        HisMedicineTypeViewFilterQuery medicinetypeViewFilter = new HisMedicineTypeViewFilterQuery();
                        listMedicineTypes = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(param).GetView(medicinetypeViewFilter);
                    }
                    // v_his_imp_mét_material
                    skip = 0;
                    if (castFilter.IS_MATERIAL)
                    {
                        while (listManuImpMests.Count - skip > 0)
                        {
                            var listIDs = listManuImpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var impMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery()
                            {
                                IMP_MEST_IDs = listIDs.Select(s => s.ID).ToList()
                            };
                            var listImpMestMaterial = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(param).GetView(impMestMaterialViewFilter);
                            listImpMestMaterials.AddRange(listImpMestMaterial);
                        }
                        skip = 0;
                        while (listImpMestMaterials.Count - skip > 0)
                        {
                            var listIDs = listImpMestMaterials.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisMaterialViewFilterQuery materialViewFilter = new HisMaterialViewFilterQuery();
                            materialViewFilter.IDs = listIDs.Select(s => s.MATERIAL_ID).Distinct().ToList();
                            var listMaterial = new MOS.MANAGER.HisMaterial.HisMaterialManager(param).GetView(materialViewFilter);
                            listMaterials.AddRange(listMaterial);
                        }

                        HisMaterialTypeViewFilterQuery materialTypeViewFilter = new HisMaterialTypeViewFilterQuery();
                        listMaterialTypes = new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager(param).GetView(materialTypeViewFilter);
                    }
                    // v_his_imp_mest_blood
                    #region
                    //skip = 0; 
                    //if (castFilter.IS_BLOOD)
                    //{
                    //    while (listImpMests.Count - skip > 0)
                    //    {
                    //        var listIDs = listImpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    //        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    //        var impMestBloodViewFilter = new HisImpMestBloodViewFilterQuery()
                    //        {
                    //            IMP_MEST_IDs = listIDs.Select(s => s.ID).ToList()
                    //        }; 
                    //        var listImpMestBlood = new MOS.MANAGER.HisImpMestBlood.HisImpMestBloodManager(param).GetView(impMestBloodViewFilter); 
                    //        listImpMestBloods.AddRange(listImpMestBlood); 
                    //    }

                    //    while (listImpMestBloods.Count - skip > 0)
                    //    {
                    //        var listIDs = listImpMestBloods.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    //        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    //        HisBloodViewFilterQuery bloodViewFilter = new HisBloodViewFilterQuery(); 
                    //        bloodViewFilter.IDs = listIDs.Select(s => s.BLOOD_ID).Distinct().ToList(); 
                    //        var listBlood = new MOS.MANAGER.HisBlood.HisBloodManager(param).GetView(bloodViewFilter); 
                    //        listBloods.AddRange(listBlood); 
                    //    }

                    //    HisBloodTypeViewFilterQuery bloodTypeViewFilter = new HisBloodTypeViewFilterQuery(); 
                    //    listBloodTypes = new MOS.MANAGER.HisBloodType.HisBloodTypeManager(param).GetView(bloodTypeViewFilter); 
                    //}
                    #endregion

                    // mediStock
                    skip = 0;
                    var listMediStocks = new List<HIS_MEDI_STOCK>();
                    while (castFilter.MEDI_STOCK_IDs.Count - skip > 0)
                    {
                        var listIDs = castFilter.MEDI_STOCK_IDs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisMediStockFilterQuery mediStockFilter = new HisMediStockFilterQuery();
                        mediStockFilter.IDs = listIDs;
                        var listMediStock = new MOS.MANAGER.HisMediStock.HisMediStockManager(param).Get(mediStockFilter);
                        listMediStocks.AddRange(listMediStock);
                    }
                    ListMediStock = String.Join(",", listMediStocks.Select(s => s.MEDI_STOCK_NAME).ToArray());
                }

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
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(listManuImpMests))
                {
                    CommonParam param = new CommonParam();
                    Dictionary<long, HIS_BID> dicBid = new HisBidManager(param).Get(new HisBidFilterQuery()).ToDictionary(o => o.ID);
                    foreach (var impMest in listManuImpMests)
                    {

                        var impMestMedicines = listImpMestMedicines.Where(s => s.IMP_MEST_ID == impMest.ID).ToList();
                        if (IsNotNullOrEmpty(impMestMedicines))
                        {
                            foreach (var impMestMedicine in impMestMedicines)
                            {
                                Mrs00356RDO rdo = new Mrs00356RDO();
                                rdo.GROUP_ID = GROUP_MEDI;
                                rdo.GROUP_NAME = "THUỐC";

                                var parent_id = impMestMedicine.MEDICINE_TYPE_ID;
                                var parent_name = impMestMedicine.MEDICINE_TYPE_NAME;
                                FindParent(parent_id, GROUP_MEDI, ref parent_id, ref parent_name);
                                rdo.PARENT_ID = parent_id;
                                rdo.PARENT_CODE = "TH" + parent_id;
                                rdo.PARENT_NAME = parent_name;

                                rdo.SERVICE_ID = impMestMedicine.MEDICINE_ID;
                                rdo.SERVICE_CODE = impMestMedicine.MEDICINE_TYPE_CODE;
                                rdo.SERVICE_NAME = impMestMedicine.MEDICINE_TYPE_NAME;

                                rdo.SERVICE_UNIT_NAME = impMestMedicine.SERVICE_UNIT_NAME;
                                rdo.NATIONAL_NAME = impMestMedicine.NATIONAL_NAME;
                                rdo.MANUFACTURER_NAME = impMestMedicine.MANUFACTURER_NAME;

                                rdo.DOCUMENT_NUMBER = impMest.DOCUMENT_NUMBER;
                                rdo.BID_NUMBER = dicBid.ContainsKey(impMestMedicine.BID_ID ?? 0) ? dicBid[impMestMedicine.BID_ID ?? 0].BID_NUMBER : "";

                                rdo.AMOUNT = impMestMedicine.AMOUNT;
                                rdo.PRICE = (impMestMedicine.PRICE ?? 0) * (1 + (impMestMedicine.VAT_RATIO??0));

                                rdo.PACKAGE_NUMBER = impMestMedicine.PACKAGE_NUMBER;
                                rdo.EXPIRED_DATE = impMestMedicine.EXPIRED_DATE ?? 10000101000000;

                                bool active = true;
                                if (IsNotNull(castFilter.IMP_SOURCE_ID))
                                {
                                    var medicine = listMedicines.Where(s => s.ID == impMestMedicine.MEDICINE_ID).First();
                                    if (medicine.IMP_SOURCE_ID != castFilter.IMP_SOURCE_ID)
                                        active = false;
                                }

                                if (active) ListRdo.Add(rdo);
                            }
                        }

                        var impMestMaterials = listImpMestMaterials.Where(s => s.IMP_MEST_ID == impMest.ID).ToList();
                        if (IsNotNullOrEmpty(impMestMaterials))
                        {
                            foreach (var impMestMaterial in impMestMaterials)
                            {
                                Mrs00356RDO rdo = new Mrs00356RDO();
                                rdo.GROUP_ID = GROUP_MATE;
                                rdo.GROUP_NAME = "VẬT TƯ";

                                var parent_id = impMestMaterial.MATERIAL_TYPE_ID;
                                var parent_name = impMestMaterial.MATERIAL_TYPE_NAME;
                                FindParent(parent_id, GROUP_MATE, ref parent_id, ref parent_name);
                                rdo.PARENT_ID = parent_id;
                                rdo.PARENT_CODE = "VT" + parent_id;
                                rdo.PARENT_NAME = parent_name;

                                rdo.SERVICE_ID = impMestMaterial.MATERIAL_ID;
                                rdo.SERVICE_CODE = impMestMaterial.MATERIAL_TYPE_CODE;
                                rdo.SERVICE_NAME = impMestMaterial.MATERIAL_TYPE_NAME;

                                rdo.SERVICE_UNIT_NAME = impMestMaterial.SERVICE_UNIT_NAME;
                                rdo.NATIONAL_NAME = impMestMaterial.NATIONAL_NAME;
                                rdo.MANUFACTURER_NAME = impMestMaterial.MANUFACTURER_NAME;

                                rdo.DOCUMENT_NUMBER = impMest.DOCUMENT_NUMBER;
                                rdo.BID_NUMBER = dicBid.ContainsKey(impMestMaterial.BID_ID ?? 0) ? dicBid[impMestMaterial.BID_ID ?? 0].BID_NUMBER : "";

                                rdo.AMOUNT = impMestMaterial.AMOUNT;
                                rdo.PRICE = (impMestMaterial.PRICE ?? 0) * (1 + (impMestMaterial.VAT_RATIO ?? 0));

                                rdo.PACKAGE_NUMBER = impMestMaterial.PACKAGE_NUMBER;
                                rdo.EXPIRED_DATE = impMestMaterial.EXPIRED_DATE ?? 10000101000000;

                                bool active = true;
                                if (IsNotNull(castFilter.IMP_SOURCE_ID))
                                {
                                    var material = listMaterials.Where(s => s.ID == impMestMaterial.MATERIAL_ID).First();
                                    if (material.IMP_SOURCE_ID != castFilter.IMP_SOURCE_ID)
                                        active = false;
                                }

                                if (active) ListRdo.Add(rdo);
                            }
                        }

                        #region
                        //var impMestBloods = listImpMestBloods.Where(s => s.IMP_MEST_ID == impMest.ID).ToList(); 
                        //if (IsNotNullOrEmpty(impMestBloods))
                        //{
                        //    foreach (var impMestBlood in impMestBloods)
                        //    {
                        //        Mrs00356RDO rdo = new Mrs00356RDO(); 
                        //        rdo.GROUP_ID = GROUP_BLOD; 
                        //        rdo.GROUP_NAME = "MÁU"; 

                        //        var parent_id = impMestBlood.BLOOD_TYPE_ID; 
                        //        var parent_name = impMestBlood.BLOOD_TYPE_NAME; 
                        //        FindParent(parent_id, GROUP_BLOD, ref parent_id, ref parent_name); 
                        //        rdo.PARENT_ID = parent_id; 
                        //        rdo.PARENT_CODE = "MA" + parent_id; 
                        //        rdo.PARENT_NAME = parent_name; 

                        //        rdo.SERVICE_ID = impMestBlood.BLOOD_ID; 
                        //        rdo.SERVICE_CODE = impMestBlood.BLOOD_TYPE_CODE; 
                        //        rdo.SERVICE_NAME = impMestBlood.BLOOD_TYPE_NAME; 

                        //        rdo.SERVICE_UNIT_NAME = impMestBlood.SERVICE_UNIT_NAME; 
                        //        //rdo.NATIONAL_NAME = impMestBlood.NATIONAL_NAME; 
                        //        //rdo.MANUFACTURER_NAME = impMestBlood.MANUFACTURER_NAME; 

                        //        rdo.DOCUMENT_NUMBER = manuImpMest.First().DOCUMENT_NUMBER; 
                        //        rdo.BID_NUMBER = manuImpMest.First().BID_NUMBER; 

                        //        //rdo.AMOUNT = impMestBlood.VOLUME; 
                        //        rdo.PRICE = impMestBlood.PRICE ?? 0; 

                        //        rdo.PACKAGE_NUMBER = impMestBlood.PACKAGE_NUMBER; 
                        //        rdo.EXPIRED_DATE = impMestBlood.EXPIRED_DATE ?? 10000101000000; 

                        //        bool active = true; 
                        //        if (IsNotNull(castFilter.IMP_SOURCE_ID))
                        //        {
                        //            var blood = listBloods.Where(s => s.ID == impMestBlood.BLOOD_ID).First(); 
                        //            if (blood.IMP_SOURCE_ID != castFilter.IMP_SOURCE_ID)
                        //                active = false; 
                        //        }

                        //        if (active) ListRdo.Add(rdo); 
                        //    }
                        //}
                        #endregion
                    }

                    var ListRdoGroupByGroupIds = ListRdo.GroupBy(s => s.GROUP_ID).OrderBy(o => o.First().GROUP_ID).ToList();
                    foreach (var ListRdoGroupByGroupId in ListRdoGroupByGroupIds)
                    {
                        int i = 0;
                        var ListRdoGroupByParentIds = ListRdoGroupByGroupId.GroupBy(g => g.PARENT_ID).OrderBy(o => o.First().PARENT_NAME).ToList();
                        foreach (var ListRdoGroupByParentId in ListRdoGroupByParentIds)
                        {
                            var ListRdoOrderByServiceNames = ListRdoGroupByParentId.OrderBy(s => s.SERVICE_NAME);
                            foreach (var rdo in ListRdoOrderByServiceNames)
                            {
                                i++;
                                rdo.NUMBER = i;
                            }
                        }
                    }

                    ListRdoGroup = ListRdo.GroupBy(g => g.GROUP_ID).Select(s => new Mrs00356RDO
                    {
                        GROUP_ID = s.First().GROUP_ID,
                        GROUP_NAME = s.First().GROUP_NAME
                    }).ToList();

                    ListRdoParent = ListRdo.GroupBy(g => g.PARENT_CODE).Select(s => new Mrs00356RDO
                    {
                        GROUP_ID = s.First().GROUP_ID,
                        PARENT_ID = s.First().PARENT_ID,
                        PARENT_CODE = s.First().PARENT_CODE,
                        PARENT_NAME = s.First().PARENT_NAME
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        public void FindParent(long serviceTypeId, long group_id, ref long parent_id, ref string parent_name)
        {
            if (group_id == GROUP_MEDI)
            {
                var parent = listMedicineTypes.Where(s => s.ID == serviceTypeId).ToList().First();
                while (IsNotNull(parent.PARENT_ID))
                {
                    parent = listMedicineTypes.Where(s => s.ID == parent.PARENT_ID).ToList().First();
                    parent_id = parent.ID;
                    parent_name = parent.MEDICINE_TYPE_NAME;
                }
            }
            else if (group_id == GROUP_MATE)
            {
                var parent = listMaterialTypes.Where(s => s.ID == serviceTypeId).ToList().First();
                while (IsNotNull(parent.PARENT_ID))
                {
                    parent = listMaterialTypes.Where(s => s.ID == parent.PARENT_ID).ToList().First();
                    parent_id = parent.ID;
                    parent_name = parent.MATERIAL_TYPE_NAME;
                }
            }
            else if (group_id == GROUP_BLOD)
            {
                var parent = listBloodTypes.Where(s => s.ID == serviceTypeId).ToList().First();
                while (IsNotNull(parent.PARENT_ID))
                {
                    parent = listBloodTypes.Where(s => s.ID == parent.PARENT_ID).ToList().First();
                    parent_id = parent.ID;
                    parent_name = parent.BLOOD_TYPE_NAME;
                }
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                dicSingleTag.Add("ListMediStock", ListMediStock);

                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Group", ListRdoGroup);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Parent", ListRdoParent.OrderBy(s => s.PARENT_NAME).ToList());
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group", "Parent", "GROUP_ID", "GROUP_ID");
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Rdo", ListRdo.OrderBy(s => s.SERVICE_NAME).ToList());
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Parent", "Rdo", "PARENT_CODE", "PARENT_CODE");
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
