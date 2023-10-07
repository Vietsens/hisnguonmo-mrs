using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBid;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisServiceRetyCat;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00649
{
    class Mrs00649Processor : AbstractProcessor
    {
        Mrs00649Filter castFilter = null;
        List<Mrs00649RDO> ListRdo = new List<Mrs00649RDO>();

        List<V_HIS_IMP_MEST> listManuImpMests = new List<V_HIS_IMP_MEST>();
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterials = new List<V_HIS_IMP_MEST_MATERIAL>();

        List<V_HIS_MEDICINE_TYPE> listMedicineTypes = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MATERIAL_TYPE> listMaterialTypes = new List<V_HIS_MATERIAL_TYPE>();

        List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();

        public string ListMediStock = "";

        private long VAT_TU = 1;
        private long HOA_CHAT = 2;
        private long VI_THUOC = 3;
        private long GAY_NGHIEN = 4;
        private long HUONG_THAN = 5;
        private long KHAC = 6;
        bool takeMedicineData = true;
        bool takeMaterialData = true;
        bool takeChemicalSubstanceData = true;

        public Mrs00649Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00649Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                this.castFilter = (Mrs00649Filter)this.reportFilter;
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
                // v_his_imp_mest
                HisImpMestViewFilterQuery impMestViewFilter = new HisImpMestViewFilterQuery();
                impMestViewFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                impMestViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                impMestViewFilter.IMP_TIME_TO = castFilter.TIME_TO;
                impMestViewFilter.IMP_MEST_STT_IDs = new List<long> { IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT };
                impMestViewFilter.IMP_MEST_TYPE_IDs = castFilter.IMP_MEST_TYPE_IDs;
                listManuImpMests = new MOS.MANAGER.HisImpMest.HisImpMestManager(param).GetView(impMestViewFilter);

                if (IsNotNullOrEmpty(listManuImpMests))
                {
                    var skip = 0;

                    // v_his_imp_mét_medicine
                    // v_his_imp_mét_material
                    while (listManuImpMests.Count - skip > 0)
                    {
                        var listIDs = listManuImpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        if (takeMedicineData)
                        {
                            var impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery()
                            {
                                IMP_MEST_IDs = listIDs.Select(s => s.ID).ToList()
                            };
                            var listImpMestMedicine = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(param).GetView(impMestMedicineViewFilter);
                            listImpMestMedicines.AddRange(listImpMestMedicine);
                        }

                        if (takeChemicalSubstanceData || takeMaterialData)
                        {
                            var impMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery()
                            {
                                IMP_MEST_IDs = listIDs.Select(s => s.ID).ToList()
                            };
                            var listImpMestMaterial = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(param).GetView(impMestMaterialViewFilter);
                            listImpMestMaterials.AddRange(listImpMestMaterial);
                        }
                    }

                    if (takeMedicineData)
                    {
                        //lay medicine type phan loai dong thuoc "Vi thuoc Dong y"
                        HisMedicineTypeViewFilterQuery medicinetypeViewFilter = new HisMedicineTypeViewFilterQuery();
                        listMedicineTypes = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(param).GetView(medicinetypeViewFilter);
                    }

                    if (takeChemicalSubstanceData || takeMaterialData)
                    {
                        //lay material type phan loai hoat chat
                        HisMaterialTypeViewFilterQuery materialTypeViewFilter = new HisMaterialTypeViewFilterQuery();
                        listMaterialTypes = new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager(param).GetView(materialTypeViewFilter);
                    }

                    
                }

                // mediStock
                if (castFilter.MEDI_STOCK_IDs != null && castFilter.MEDI_STOCK_IDs.Count > 0)
                {
                    var listMediStocks = new List<HIS_MEDI_STOCK>();
                    var skip = 0;
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

                HisServiceRetyCatViewFilterQuery HisServiceRetyCatfilter = new HisServiceRetyCatViewFilterQuery()
                {
                    REPORT_TYPE_CODE__EXACT = this.ReportTypeCode
                };
                var retyCat = new HisServiceRetyCatManager().GetView(HisServiceRetyCatfilter);
                listHisServiceRetyCat.AddRange(retyCat);

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

                    List<long> viThuocIds = new List<long>();
                    if (listHisServiceRetyCat != null && listHisServiceRetyCat.Count > 0)
                    {
                        viThuocIds = listHisServiceRetyCat.Select(s => s.SERVICE_ID).Distinct().ToList();
                    }

                    //trong cau hinh co dich vu cha se lay het cac dich vu con.
                    if (IsNotNullOrEmpty(viThuocIds))
                    {
                        //danh sach cac thuoc duoc cau hinh
                        var medcine_types = listMedicineTypes.Where(o => viThuocIds.Contains(o.SERVICE_ID)).ToList();

                        if (IsNotNullOrEmpty(medcine_types))
                        {
                            var medcineTypeIds = medcine_types.Select(s => s.ID).ToList();
                            //danh sach cac thuoc co cha lay tu cau hinh
                            var childMedicine = listMedicineTypes.Where(o => medcineTypeIds.Contains(o.PARENT_ID ?? 0)).ToList();
                            if (IsNotNullOrEmpty(childMedicine))
                            {
                                viThuocIds.AddRange(childMedicine.Select(s => s.SERVICE_ID).ToList());
                                viThuocIds = viThuocIds.Distinct().ToList();
                            }
                        }
                    }

                    foreach (var impMest in listManuImpMests)
                    {

                        var impMestMedicines = listImpMestMedicines.Where(s => s.IMP_MEST_ID == impMest.ID).ToList();
                        if (IsNotNullOrEmpty(impMestMedicines))
                        {
                            foreach (var impMestMedicine in impMestMedicines)
                            {
                                var medicine_type = listMedicineTypes.FirstOrDefault(o => o.ID == impMestMedicine.MEDICINE_TYPE_ID);
                                if (medicine_type == null) continue;

                                Mrs00649RDO rdo = new Mrs00649RDO();
                                rdo.GROUP_ID = rdo.GROUP_6_ID = this.KHAC;
                                rdo.GROUP_NAME = rdo.GROUP_6_NAME = "THUỐC";

                                if (viThuocIds.Count > 0 && viThuocIds.Contains(medicine_type.SERVICE_ID))
                                {
                                    rdo.GROUP_ID = rdo.GROUP_6_ID = this.VI_THUOC;
                                    rdo.GROUP_NAME = rdo.GROUP_6_NAME = "VỊ THUỐC Y HỌC CỔ TRUYỀN";
                                }
                                else
                                {
                                    if (medicine_type.MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__GN)
                                    {
                                        rdo.GROUP_6_ID = this.GAY_NGHIEN;
                                        rdo.GROUP_6_NAME = "GÂY NGHIỆN";
                                    }
                                    else if (medicine_type.MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__HT)
                                    {
                                        rdo.GROUP_6_ID = this.HUONG_THAN;
                                        rdo.GROUP_6_NAME = "HƯỚNG THẦN";
                                    }
                                }

                                rdo.IMP_TIME = impMestMedicine.IMP_TIME;
                                rdo.REGISTER_NUMBER = impMestMedicine.MEDICINE_REGISTER_NUMBER;

                                rdo.SERVICE_ID = impMestMedicine.MEDICINE_ID;
                                rdo.SERVICE_CODE = impMestMedicine.MEDICINE_TYPE_CODE;
                                rdo.SERVICE_NAME = impMestMedicine.MEDICINE_TYPE_NAME;

                                rdo.SERVICE_UNIT_NAME = impMestMedicine.SERVICE_UNIT_NAME;
                                rdo.NATIONAL_NAME = impMestMedicine.NATIONAL_NAME;
                                rdo.MANUFACTURER_NAME = impMestMedicine.MANUFACTURER_NAME;
                                rdo.SUPPLIER_NAME = impMestMedicine.SUPPLIER_NAME;

                                rdo.DOCUMENT_NUMBER = impMest.DOCUMENT_NUMBER;

                                rdo.DOCUMENT_DATE = impMest.DOCUMENT_DATE;
                                
                                rdo.BID_NUMBER = dicBid.ContainsKey(impMestMedicine.BID_ID ?? 0) ? dicBid[impMestMedicine.BID_ID ?? 0].BID_NUMBER : "";

                                rdo.AMOUNT = impMestMedicine.AMOUNT;
                                rdo.PRICE = (impMestMedicine.PRICE ?? 0) * (1 + (impMestMedicine.VAT_RATIO ?? 0));
                                rdo.IMP_PRICE = impMestMedicine.IMP_PRICE;
                                rdo.IMP_VAT_RATIO = impMestMedicine.IMP_VAT_RATIO;
                                rdo.IMP_VAT_100 = impMestMedicine.IMP_VAT_RATIO * 100;

                                rdo.PACKAGE_NUMBER = impMestMedicine.PACKAGE_NUMBER;
                                rdo.EXPIRED_DATE = impMestMedicine.EXPIRED_DATE ?? 0;

                                ListRdo.Add(rdo);
                            }
                        }

                        var impMestMaterials = listImpMestMaterials.Where(s => s.IMP_MEST_ID == impMest.ID).ToList();
                        if (IsNotNullOrEmpty(impMestMaterials))
                        {
                            foreach (var impMestMaterial in impMestMaterials)
                            {
                                var material_type = listMaterialTypes.FirstOrDefault(o => o.ID == impMestMaterial.MATERIAL_TYPE_ID);
                                if (material_type == null) continue;

                                Mrs00649RDO rdo = new Mrs00649RDO();
                                rdo.GROUP_ID = rdo.GROUP_6_ID = this.VAT_TU;
                                rdo.GROUP_NAME = rdo.GROUP_6_NAME = "VẬT TƯ";

                                if (material_type.IS_CHEMICAL_SUBSTANCE == 1)
                                {
                                    rdo.GROUP_ID = rdo.GROUP_6_ID = this.HOA_CHAT;
                                    rdo.GROUP_NAME = rdo.GROUP_6_NAME = "HÓA CHẤT";
                                }
                                if (material_type.IS_CHEMICAL_SUBSTANCE != 1 && !this.takeMaterialData)
                                {
                                    continue;
                                }
                                if (material_type.IS_CHEMICAL_SUBSTANCE == 1 && !this.takeChemicalSubstanceData)
                                {
                                    continue;
                                }
                                rdo.IMP_TIME = impMestMaterial.IMP_TIME;

                                rdo.SERVICE_ID = impMestMaterial.MATERIAL_ID;
                                rdo.SERVICE_CODE = impMestMaterial.MATERIAL_TYPE_CODE;
                                rdo.SERVICE_NAME = impMestMaterial.MATERIAL_TYPE_NAME;

                                rdo.SERVICE_UNIT_NAME = impMestMaterial.SERVICE_UNIT_NAME;
                                rdo.NATIONAL_NAME = impMestMaterial.NATIONAL_NAME;
                                rdo.MANUFACTURER_NAME = impMestMaterial.MANUFACTURER_NAME;
                                rdo.SUPPLIER_NAME = impMestMaterial.SUPPLIER_NAME;

                                rdo.DOCUMENT_NUMBER = impMest.DOCUMENT_NUMBER;
                                rdo.DOCUMENT_DATE = impMest.DOCUMENT_DATE;
                                rdo.BID_NUMBER = dicBid.ContainsKey(impMestMaterial.BID_ID ?? 0) ? dicBid[impMestMaterial.BID_ID ?? 0].BID_NUMBER : "";

                                rdo.AMOUNT = impMestMaterial.AMOUNT;
                                rdo.PRICE = (impMestMaterial.PRICE ?? 0) * (1 + (impMestMaterial.VAT_RATIO ?? 0));
                                rdo.IMP_PRICE = impMestMaterial.IMP_PRICE;
                                rdo.IMP_VAT_RATIO = impMestMaterial.IMP_VAT_RATIO;
                                rdo.IMP_VAT_100 = impMestMaterial.IMP_VAT_RATIO * 100;

                                rdo.PACKAGE_NUMBER = impMestMaterial.PACKAGE_NUMBER;
                                rdo.EXPIRED_DATE = impMestMaterial.EXPIRED_DATE ?? 0;

                                ListRdo.Add(rdo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
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

                var ListRdoGroup4 = ListRdo.GroupBy(g => g.GROUP_ID).Select(s => new Mrs00649RDO { GROUP_ID = s.First().GROUP_ID, GROUP_NAME = s.First().GROUP_NAME }).ToList();
                var ListRdoGroup6 = ListRdo.GroupBy(g => g.GROUP_6_ID).Select(s => new Mrs00649RDO { GROUP_6_ID = s.First().GROUP_6_ID, GROUP_6_NAME = s.First().GROUP_6_NAME }).ToList();
                if (ListRdoGroup4 == null || ListRdoGroup4.Count == 0)
                {
                    ListRdoGroup4.Add(new Mrs00649RDO());
                }

                if (ListRdoGroup6 == null || ListRdoGroup6.Count == 0)
                {
                    ListRdoGroup6.Add(new Mrs00649RDO());
                }

                ListRdo = ListRdo.OrderBy(s => s.SERVICE_NAME).ToList();
                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Group_4", ListRdoGroup4.OrderBy(o => o.GROUP_NAME).ToList());
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Report_4", ListRdo);
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group_4", "Report_4", "GROUP_ID", "GROUP_ID");

                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Group_6", ListRdoGroup6.OrderBy(o => o.GROUP_6_NAME).ToList());
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Report_6", ListRdo);
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group_6", "Report_6", "GROUP_6_ID", "GROUP_6_ID");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
