using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBidBloodType;
using MOS.MANAGER.HisBidMaterialType;
using MOS.MANAGER.HisBidMedicineType;
using MOS.MANAGER.HisBlood;
using MOS.MANAGER.HisBloodType;
using MOS.MANAGER.HisImpMestBlood;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpSource;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMedicineUseForm;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MRS.Processor.Mrs00553
{
    class Mrs00553Processor : AbstractProcessor
    {
        Mrs00553Filter castFilter;
        List<Mrs00553RDO> ListSup = new List<Mrs00553RDO>();
        List<V_IMP_MEST> ListImpMest = new List<V_IMP_MEST>();
        List<Mrs00553RDO> ListRdo = new List<Mrs00553RDO>();

        List<MOS.EFMODEL.DataModels.V_HIS_IMP_MEST_MEDICINE> ListImpMestMedicine = new List<MOS.EFMODEL.DataModels.V_HIS_IMP_MEST_MEDICINE>();
        List<MOS.EFMODEL.DataModels.V_HIS_IMP_MEST_MATERIAL> ListImpMestMaterial = new List<MOS.EFMODEL.DataModels.V_HIS_IMP_MEST_MATERIAL>();
        List<V_HIS_IMP_MEST_MATERIAL> ListImpMestChemicalSubStance = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<V_HIS_IMP_MEST_BLOOD> ListImpMestBlood = new List<V_HIS_IMP_MEST_BLOOD>();
        Dictionary<long, MOS.EFMODEL.DataModels.HIS_SUPPLIER> DicSupplier = new Dictionary<long, MOS.EFMODEL.DataModels.HIS_SUPPLIER>();
        List<MOS.EFMODEL.DataModels.V_HIS_MEDICINE_TYPE> ListMedicineType = new List<MOS.EFMODEL.DataModels.V_HIS_MEDICINE_TYPE>();
        List<MOS.EFMODEL.DataModels.V_HIS_MATERIAL_TYPE> ListMaterialType = new List<MOS.EFMODEL.DataModels.V_HIS_MATERIAL_TYPE>();
        List<MOS.EFMODEL.DataModels.V_HIS_BLOOD_TYPE> ListBloodType = new List<MOS.EFMODEL.DataModels.V_HIS_BLOOD_TYPE>();
        List<MOS.EFMODEL.DataModels.HIS_MEDICINE> ListMedicine = new List<MOS.EFMODEL.DataModels.HIS_MEDICINE>();
        List<MOS.EFMODEL.DataModels.HIS_MATERIAL> ListMaterial = new List<MOS.EFMODEL.DataModels.HIS_MATERIAL>();
        List<MOS.EFMODEL.DataModels.HIS_BLOOD> ListBlood = new List<MOS.EFMODEL.DataModels.HIS_BLOOD>();
        List<HIS_MEDICINE_USE_FORM> listMedicineUseForm = new List<HIS_MEDICINE_USE_FORM>();
        List<HIS_IMP_SOURCE> listImpSource = new List<HIS_IMP_SOURCE>();
        List<Mrs00553RDO> ListDocument = new List<Mrs00553RDO>();
        bool takeMedicineData = true;
        bool takeMaterialData = true;
        bool takeChemicalSubstanceData = true;
        bool takeBloodData = true;
        long medicineServiceTypeId = 6;
        long materialServiceTypeId = 7;
        long bloodServiceTypeId = 14;

        List<MOS.EFMODEL.DataModels.V_HIS_BID_MEDICINE_TYPE> ListBidMedicineType = new List<MOS.EFMODEL.DataModels.V_HIS_BID_MEDICINE_TYPE>();
        List<MOS.EFMODEL.DataModels.V_HIS_BID_MATERIAL_TYPE> ListBidMaterialType = new List<MOS.EFMODEL.DataModels.V_HIS_BID_MATERIAL_TYPE>();
        List<MOS.EFMODEL.DataModels.V_HIS_BID_BLOOD_TYPE> ListBidBloodType = new List<MOS.EFMODEL.DataModels.V_HIS_BID_BLOOD_TYPE>();

        Dictionary<string, decimal> dicBidAmount = new Dictionary<string, decimal>();

        public Mrs00553Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00553Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = (Mrs00553Filter)reportFilter;
                if (castFilter.SERVICE_TYPE_IDs != null)
                {
                    if (castFilter.SERVICE_TYPE_IDs.Count == 1 && castFilter.SERVICE_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC))
                    {
                        takeMedicineData = true;
                        takeMaterialData = false;
                        takeChemicalSubstanceData = false;
                        takeBloodData = false;
                    }
                    else if (castFilter.SERVICE_TYPE_IDs.Count == 1 && castFilter.SERVICE_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT))
                    {
                        takeMaterialData = true;
                        takeChemicalSubstanceData = true;
                        takeMedicineData = false;
                        takeBloodData = false;
                    }
                    else if (castFilter.SERVICE_TYPE_IDs.Count == 1 && castFilter.SERVICE_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU))
                    {
                        takeMaterialData = false;
                        takeChemicalSubstanceData = false;
                        takeMedicineData = false;
                        takeBloodData = true;
                    }
                }
                else
                {
                    if (castFilter.IS_MEDICINE != true && (castFilter.IS_MATERIAL == true || castFilter.IS_CHEMICAL_SUBSTANCE == true || castFilter.IS_BLOOD == true))
                    {
                        takeMedicineData = false;
                    }
                    if (castFilter.IS_MATERIAL != true && (castFilter.IS_MEDICINE == true || castFilter.IS_CHEMICAL_SUBSTANCE == true || castFilter.IS_BLOOD == true))
                    {
                        takeMaterialData = false;
                    }

                    if (castFilter.IS_CHEMICAL_SUBSTANCE != true && (castFilter.IS_MEDICINE == true || castFilter.IS_MATERIAL == true || castFilter.IS_BLOOD == true))
                    {
                        takeChemicalSubstanceData = false;
                    }
                    if (castFilter.IS_BLOOD != true && (castFilter.IS_MEDICINE == true || castFilter.IS_MATERIAL == true || castFilter.IS_MEDICINE == true))
                    {
                        takeBloodData = false;
                    }
                    if (castFilter.IS_BLOOD != true && castFilter.IS_MATERIAL != true && castFilter.IS_CHEMICAL_SUBSTANCE != true && castFilter.IS_MEDICINE != true)
                    {
                        takeMedicineData = true;
                        takeMaterialData = true;
                        takeChemicalSubstanceData = true;
                        takeBloodData = true;
                    }
                }
                

                CommonParam commonParam = new CommonParam();

                //if (!IsNotNullOrEmpty(castFilter.MEDI_STOCK_IDs))
                //{
                //    throw new Exception("Khong co kho nhap");
                //}

                var supplier = new MOS.MANAGER.HisSupplier.HisSupplierManager(commonParam).Get(new MOS.MANAGER.HisSupplier.HisSupplierFilterQuery());
                if (IsNotNullOrEmpty(supplier))
                {
                    DicSupplier = supplier.ToDictionary(o => o.ID);
                }

                //foreach (var item in castFilter.MEDI_STOCK_IDs)
                //{
                MOS.MANAGER.HisImpMest.HisImpMestViewFilterQuery impFilter = new MOS.MANAGER.HisImpMest.HisImpMestViewFilterQuery();
                impFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                impFilter.IMP_TIME_TO = castFilter.TIME_TO;
                impFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                impFilter.IMP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC;
                var impMest = new MOS.MANAGER.HisImpMest.HisImpMestManager(commonParam).GetView(impFilter);
                if (IsNotNullOrEmpty(impMest))
                {

                    var keyOrderImp = "{0}_{1}_{2}";
                    //khi có điều kiện lọc từ template thì đổi sang key từ template
                    if (this.dicDataFilter.ContainsKey("KEY_ORDER_IMP") && this.dicDataFilter["KEY_ORDER_IMP"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_ORDER_IMP"].ToString()))
                    {
                        keyOrderImp = this.dicDataFilter["KEY_ORDER_IMP"].ToString();
                    }

                    ListImpMest = impMest.Select(o => new V_IMP_MEST(o, keyOrderImp)).ToList();
                }
                //}

                if (IsNotNullOrEmpty(ListImpMest))
                {
                    if (takeMedicineData)
                    {
                        ListMedicineType = new HisMedicineTypeManager(commonParam).GetView(new HisMedicineTypeViewFilterQuery());
                        ListImpMestMedicine = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(commonParam).GetViewByImpMestIds(ListImpMest.Select(s => s.ID).ToList());
                        if (castFilter.SERVICE_IDs != null)
                        {
                            ListImpMestMedicine = ListImpMestMedicine.Where(p => castFilter.SERVICE_IDs.Contains(p.SERVICE_ID)).ToList();
                        }
                        if (castFilter.SUPPLIER_IDs != null)
                        {
                            ListImpMestMedicine = ListImpMestMedicine.Where(p => castFilter.SUPPLIER_IDs.Contains(p.SUPPLIER_ID ?? 0)).ToList();
                        }
                        if (castFilter.BID_ID != null)
                        {
                            ListImpMestMedicine = ListImpMestMedicine.Where(o => o.BID_ID == castFilter.BID_ID).ToList();

                        }
                    }

                    if (takeChemicalSubstanceData || takeMaterialData)
                    {
                        ListMaterialType = new HisMaterialTypeManager(commonParam).GetView(new HisMaterialTypeViewFilterQuery());
                        var ListMaterialTypeAndChemialSubStance = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(commonParam).GetViewByImpMestIds(ListImpMest.Select(s => s.ID).ToList()) ?? new List<V_HIS_IMP_MEST_MATERIAL>();
                        if (takeMaterialData)
                        {
                            ListImpMestMaterial = ListMaterialTypeAndChemialSubStance.Where(o => !ListMaterialType.Exists(p => p.ID == o.MATERIAL_TYPE_ID && p.IS_CHEMICAL_SUBSTANCE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)).ToList();
                            if (castFilter.SERVICE_IDs != null)
                            {
                                ListImpMestMaterial = ListImpMestMaterial.Where(p => castFilter.SERVICE_IDs.Contains(p.SERVICE_ID)).ToList();
                            }
                            if (castFilter.SUPPLIER_IDs != null)
                            {
                                ListImpMestMaterial = ListImpMestMaterial.Where(p => castFilter.SUPPLIER_IDs.Contains(p.SUPPLIER_ID ?? 0)).ToList();
                            }
                            if (castFilter.BID_ID != null)
                            {
                                ListImpMestMaterial = ListImpMestMaterial.Where(o => o.BID_ID == castFilter.BID_ID).ToList();

                            }
                        }

                        if (takeChemicalSubstanceData)
                        {
                            ListImpMestChemicalSubStance = ListMaterialTypeAndChemialSubStance.Where(o => ListMaterialType.Exists(p => p.ID == o.MATERIAL_TYPE_ID && p.IS_CHEMICAL_SUBSTANCE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)).ToList();
                            if (castFilter.SERVICE_IDs != null)
                            {
                                ListImpMestChemicalSubStance = ListImpMestChemicalSubStance.Where(p => castFilter.SERVICE_IDs.Contains(p.SERVICE_ID)).ToList();
                            }
                            if (castFilter.SUPPLIER_IDs != null)
                            {
                                ListImpMestChemicalSubStance = ListImpMestChemicalSubStance.Where(p => castFilter.SUPPLIER_IDs.Contains(p.SUPPLIER_ID ?? 0)).ToList();
                            }
                            if (castFilter.BID_ID != null)
                            {
                                ListImpMestChemicalSubStance = ListImpMestChemicalSubStance.Where(o => o.BID_ID == castFilter.BID_ID).ToList();

                            }
                            //if (castFilter.CHEMICAL_SUBSTANCE_TYPE_IDs != null)
                            //{
                            //    ListImpMestChemicalSubStance = ListImpMestChemicalSubStance.Where(p => castFilter.CHEMICAL_SUBSTANCE_TYPE_IDs.Contains(p.SERVICE_ID)).ToList();
                            //}
                        }
                    }

                    if (takeBloodData)
                    {
                        ListBloodType = new HisBloodTypeManager(commonParam).GetView(new HisBloodTypeViewFilterQuery());
                        var ListImpMestIds = ListImpMest.Select(o => o.ID).ToList();

                        int skip = 0;
                        while (ListImpMestIds.Count - skip > 0)
                        {
                            var listId = ListImpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisImpMestBloodViewFilterQuery immmFilter = new HisImpMestBloodViewFilterQuery();
                            immmFilter.IMP_MEST_IDs = listId;
                            var immm = new HisImpMestBloodManager().GetView(immmFilter);


                            if (IsNotNullOrEmpty(immm))
                            {
                                ListImpMestBlood.AddRange(immm);
                            }
                        }
                        if (castFilter.SERVICE_IDs != null)
                        {
                            ListImpMestBlood = ListImpMestBlood.Where(p => castFilter.SERVICE_IDs.Contains(p.SERVICE_ID)).ToList();
                        }
                        if (castFilter.SUPPLIER_IDs != null)
                        {
                            ListImpMestBlood = ListImpMestBlood.Where(p => castFilter.SUPPLIER_IDs.Contains(p.SUPPLIER_ID ?? 0)).ToList();
                        }
                        if (castFilter.BID_ID != null)
                        {
                            ListImpMestBlood = ListImpMestBlood.Where(o => o.BID_ID == castFilter.BID_ID).ToList();

                        }
                    }
                }
                //get nguồn nhập
                GetImpSource();

                //get lô thuốc vật tư
                GetMediMate();

                if (ListMedicine != null)
                {
                    ListImpMestMedicine = ListImpMestMedicine.Where(o => ListMedicine.Exists(p => p.ID == o.MEDICINE_ID)).ToList();
                    foreach (var item in ListImpMestMedicine)
                    {
                        item.BID_NUMBER = (ListMedicine.FirstOrDefault(o => o.ID == item.MEDICINE_ID) ?? new HIS_MEDICINE()).TDL_BID_NUMBER;
                    }
                }
                if (ListMaterial != null)
                {
                    ListImpMestMaterial = ListImpMestMaterial.Where(o => ListMaterial.Exists(p => p.ID == o.MATERIAL_ID)).ToList();
                    ListImpMestChemicalSubStance = ListImpMestChemicalSubStance.Where(o => ListMaterial.Exists(p => p.ID == o.MATERIAL_ID)).ToList();
                    foreach (var item in ListImpMestMaterial)
                    {
                        item.BID_NUMBER = (ListMaterial.FirstOrDefault(o => o.ID == item.MATERIAL_ID) ?? new HIS_MATERIAL()).TDL_BID_NUMBER;
                    }
                    foreach (var item in ListImpMestChemicalSubStance)
                    {
                        item.BID_NUMBER = (ListMaterial.FirstOrDefault(o => o.ID == item.MATERIAL_ID) ?? new HIS_MATERIAL()).TDL_BID_NUMBER;
                    }
                }
                //if (castFilter.BID_ID != null)
                //{
                //    ListImpMestMedicine = ListImpMestMedicine.Where(o => o.BID_ID == castFilter.BID_ID).ToList();
                                       
                //}

                HisMedicineUseFormFilterQuery useMedicineFilter = new HisMedicineUseFormFilterQuery();
                listMedicineUseForm = new HisMedicineUseFormManager(commonParam).Get(useMedicineFilter);

                //get thông tin số lượng thầu
                GetBidAmount();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetBidAmount()
        {
            HisBidMedicineTypeViewFilterQuery bidMetyFilter = new HisBidMedicineTypeViewFilterQuery();
            ListBidMedicineType = new HisBidMedicineTypeManager().GetView(bidMetyFilter);
            foreach (var item in ListBidMedicineType)
            {
                string key = string.Format("{0}_{1}_{2}","THUOC",item.BID_NUMBER,item.MEDICINE_TYPE_CODE);
                if (!dicBidAmount.ContainsKey(key))
                {
                    dicBidAmount.Add(key, item.AMOUNT);
                }
            }
            HisBidMaterialTypeViewFilterQuery bidMatyFilter = new HisBidMaterialTypeViewFilterQuery();
            ListBidMaterialType = new HisBidMaterialTypeManager().GetView(bidMatyFilter);
            foreach (var item in ListBidMaterialType)
            {
                var chemicalSubstance = ListMaterialType.FirstOrDefault(p => p.ID == item.MATERIAL_TYPE_ID && p.IS_CHEMICAL_SUBSTANCE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                string key = string.Format("{0}_{1}_{2}", chemicalSubstance != null ? "HOACHAT" : "VATTU", item.BID_NUMBER, item.MATERIAL_TYPE_CODE);
                if (!dicBidAmount.ContainsKey(key))
                {
                    dicBidAmount.Add(key, item.AMOUNT);
                }
            }
            HisBidBloodTypeViewFilterQuery bidBltyFilter = new HisBidBloodTypeViewFilterQuery();
            ListBidBloodType = new HisBidBloodTypeManager().GetView(bidBltyFilter);
            foreach (var item in ListBidBloodType)
            {
                var chemicalSubstance = ListBloodType.FirstOrDefault(p => p.ID == item.BLOOD_TYPE_ID);
                string key = string.Format("{0}_{1}_{2}","MAU", item.BID_NUMBER, item.BLOOD_TYPE_CODE);
                if (!dicBidAmount.ContainsKey(key))
                {
                    dicBidAmount.Add(key, item.AMOUNT);
                }
            }
        }

        private void GetImpSource()
        {
            this.listImpSource = new HisImpSourceManager().Get(new HisImpSourceFilterQuery());
        }


        private void GetMediMate()
        {
            try
            {
                List<long> medicineIds = new List<long>();

                if (ListImpMestMedicine != null)
                {
                    medicineIds.AddRange(ListImpMestMedicine.Select(o => o.MEDICINE_ID).ToList());
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
                        ListMedicine.AddRange(MedicineSub);
                    }
                }
                if (castFilter.IMP_SOURCE_IDs != null)
                {
                    ListMedicine = ListMedicine.Where(o => castFilter.IMP_SOURCE_IDs.Contains(o.IMP_SOURCE_ID ?? 0)).ToList();
                    listImpSource = listImpSource.Where(x => castFilter.IMP_SOURCE_IDs.Contains(x.ID)).ToList();
                }

                List<long> bloodIds = new List<long>();

                if (ListImpMestBlood != null)
                {
                    bloodIds.AddRange(ListImpMestBlood.Select(o => o.BLOOD_ID).ToList());
                }


                bloodIds = bloodIds.Distinct().ToList();

                if (bloodIds != null && bloodIds.Count > 0)
                {
                    var skip = 0;
                    while (bloodIds.Count - skip > 0)
                    {
                        var limit = bloodIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisBloodFilterQuery Bloodfilter = new HisBloodFilterQuery();
                        Bloodfilter.IDs = limit;

                        var BloodSub = new HisBloodManager().Get(Bloodfilter);
                        ListBlood.AddRange(BloodSub);
                    }
                }
                if (castFilter.IMP_SOURCE_IDs != null)
                {
                    ListBlood = ListBlood.Where(o => castFilter.IMP_SOURCE_IDs.Contains(o.IMP_SOURCE_ID ?? 0)).ToList();
                    listImpSource = listImpSource.Where(x => castFilter.IMP_SOURCE_IDs.Contains(x.ID)).ToList();
                }

                List<long> materialIds = new List<long>();

                if (ListImpMestMaterial != null)
                {
                    materialIds.AddRange(ListImpMestMaterial.Select(o => o.MATERIAL_ID).ToList());
                }
                if (ListImpMestChemicalSubStance != null)
                {
                    materialIds.AddRange(ListImpMestChemicalSubStance.Select(o => o.MATERIAL_ID).ToList());
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
                        ListMaterial.AddRange(MaterialSub);
                    }
                }
                if (castFilter.IMP_SOURCE_IDs != null)
                {
                    ListMaterial = ListMaterial.Where(o => castFilter.IMP_SOURCE_IDs.Contains(o.IMP_SOURCE_ID ?? 0)).ToList();
                }
                var medidineIds = ListMedicine.Select(x => x.MEDICINE_USE_FORM_ID).ToList();
                listMedicineUseForm = listMedicineUseForm.Where(x => medidineIds.Contains(x.ID)).ToList();
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
                if (IsNotNullOrEmpty(ListImpMest))
                {
                    foreach (var item in ListImpMest)
                    {
                        if (!item.SUPPLIER_ID.HasValue) continue;
                        Mrs00553RDO rdo = new Mrs00553RDO(item, DicSupplier);
                        if (!ListSup.Exists(o => o.SUPPLIER_ID == item.SUPPLIER_ID))
                            ListSup.Add(rdo);
                    }

                    string KeyGroupExp = "{0}_{1}_{2}_{3}";
                    if (this.dicDataFilter.ContainsKey("KEY_GROUP_EXP") && this.dicDataFilter["KEY_GROUP_EXP"] != null)
                    {
                        KeyGroupExp = this.dicDataFilter["KEY_GROUP_EXP"].ToString();
                    }
                    if (IsNotNullOrEmpty(ListImpMestMedicine) || IsNotNullOrEmpty(ListImpMestMaterial) || IsNotNullOrEmpty(ListImpMestChemicalSubStance) || IsNotNullOrEmpty(ListImpMestBlood))
                    {

                        if (takeMedicineData)
                        {
                            if (IsNotNullOrEmpty(ListImpMestMedicine))
                            {
                                var groups = ListImpMestMedicine.GroupBy(o => string.Format(KeyGroupExp, o.SUPPLIER_ID, o.PRICE, o.VAT_RATIO, o.MEDICINE_TYPE_ID, o.ID, o.IMP_MEST_ID));
                                foreach (var group in groups)
                                {
                                    var medicine = new Mrs00553RDO(group.First());
                                    var impMest = ListImpMest.FirstOrDefault(o => o.ID == group.First().IMP_MEST_ID) ?? new V_HIS_IMP_MEST();
                                    if (impMest != null)
                                    {
                                        System.Reflection.PropertyInfo[] pis = Inventec.Common.Repository.Properties.Get<MOS.EFMODEL.DataModels.V_HIS_IMP_MEST>();
                                        foreach (var item in pis)
                                        {
                                            item.SetValue(medicine, item.GetValue(impMest));
                                        }

                                        if (DicSupplier.ContainsKey(impMest.SUPPLIER_ID ?? 0))
                                        {
                                            medicine.SUPPLIER_NAME = DicSupplier[impMest.SUPPLIER_ID ?? 0].SUPPLIER_NAME;
                                        }

                                    }

                                    medicine.AMOUNT = group.Sum(o => o.AMOUNT);
                                    //medicine.DOCUMENT_PRICE = group.GroupBy(o => o.ID).Select(p => p.First()).Sum(o => o.DOCUMENT_PRICE ?? 0);
                                    medicine.MEDI_MATE_CHEMI = "1.THUOC";
                                    medicine.MEDICINE_ID = group.First().MEDICINE_ID;
                                    medicine.MEDICINE_TYPE_ID = group.First().MEDICINE_TYPE_ID;
                                    medicine.MEDICINE_TYPE_CODE = group.First().MEDICINE_TYPE_CODE;
                                    medicine.MEDICINE_TYPE_NAME = group.First().MEDICINE_TYPE_NAME;
                                    medicine.MEDICINE_NUM_ORDER = group.First().MEDICINE_NUM_ORDER;
                                    medicine.TH_EXP_MEST_MEDICINE_ID = group.First().TH_EXP_MEST_MEDICINE_ID;
                                    medicine.HIS_MEDICINE = ListMedicine.Where(x => x.ID == group.First().MEDICINE_ID).FirstOrDefault();
                                    var medi = ListMedicine.FirstOrDefault(x => x.ID == group.First().MEDICINE_ID);
                                    if (medi != null)
                                    {
                                        var medicineUseForm = listMedicineUseForm.FirstOrDefault(x => x.ID == medi.MEDICINE_USE_FORM_ID);
                                        medicine.HIS_MEDICINE_USE_FORM = medicineUseForm;
                                        if (medicineUseForm != null)
                                        {
                                            medicine.MEDICINE_USE_FORM_CODE = medicineUseForm.MEDICINE_USE_FORM_CODE;
                                            medicine.MEDICINE_USE_FORM_NAME = medicineUseForm.MEDICINE_USE_FORM_NAME;
                                        }
                                        medicine.MEDICINE_REGISTER_NUMBER = medi.MEDICINE_REGISTER_NUMBER;
                                        var impSource = listImpSource.FirstOrDefault(x => x.ID == medi.IMP_SOURCE_ID);
                                        if (impSource != null)
                                        {
                                            medicine.IMP_SOURCE_NAME = impSource.IMP_SOURCE_NAME;
                                            medicine.IMP_SOURCE_CODE = impSource.IMP_SOURCE_CODE;
                                        }
                                    }
                                    var mety = ListMedicineType.FirstOrDefault(x => x.ID == group.First().MEDICINE_TYPE_ID);
                                    medicine.PARENT_NUM_ORDER = 10000;
                                    if (mety != null)
                                    {
                                        medicine.HEIN_SERVICE_BHYT_NAME = mety.HEIN_SERVICE_BHYT_NAME;
                                        medicine.MEDICINE_TYPE_PROPRIETARY_NAME = mety.MEDICINE_TYPE_PROPRIETARY_NAME;
                                        var par = ListMedicineType.FirstOrDefault(x => x.ID == mety.PARENT_ID);
                                        if (par != null)
                                        {
                                            medicine.PARENT_NUM_ORDER = par.NUM_ORDER ?? 10000;
                                            medicine.PARENT_SERVICE_CODE = mety.PARENT_CODE;
                                            medicine.PARENT_SERVICE_NAME = mety.PARENT_NAME;
                                        }
                                    }
                                    ListRdo.Add(medicine);
                                }
                            }
                        }


                        if (takeBloodData)
                        {
                            if (IsNotNullOrEmpty(ListImpMestBlood))
                            {
                                var groups = ListImpMestBlood.GroupBy(o => string.Format(KeyGroupExp, o.SUPPLIER_ID, o.PRICE, o.VAT_RATIO, o.BLOOD_TYPE_ID, o.ID, o.IMP_MEST_ID));
                                foreach (var group in groups)
                                {
                                    var medicine = new Mrs00553RDO(group.First());
                                    var impMest = ListImpMest.FirstOrDefault(o => o.ID == group.First().IMP_MEST_ID) ?? new V_HIS_IMP_MEST();
                                    if (impMest != null)
                                    {
                                        System.Reflection.PropertyInfo[] pis = Inventec.Common.Repository.Properties.Get<MOS.EFMODEL.DataModels.V_HIS_IMP_MEST>();
                                        foreach (var item in pis)
                                        {
                                            item.SetValue(medicine, item.GetValue(impMest));
                                        }

                                        if (DicSupplier.ContainsKey(impMest.SUPPLIER_ID ?? 0))
                                        {
                                            medicine.SUPPLIER_NAME = DicSupplier[impMest.SUPPLIER_ID ?? 0].SUPPLIER_NAME;
                                        }

                                    }

                                    medicine.AMOUNT = group.Sum(o => 1);
                                    //medicine.DOCUMENT_PRICE = group.GroupBy(o => o.ID).Select(p => p.First()).Sum(o => o.DOCUMENT_PRICE ?? 0);
                                    medicine.MEDI_MATE_CHEMI = "4.MAU";
                                    medicine.MEDICINE_ID = group.First().BLOOD_ID;
                                    medicine.MEDICINE_TYPE_ID = group.First().BLOOD_TYPE_ID;
                                    medicine.MEDICINE_TYPE_CODE = group.First().BLOOD_TYPE_CODE;
                                    medicine.MEDICINE_TYPE_NAME = group.First().BLOOD_TYPE_NAME;
                                    //medicine.MEDICINE_NUM_ORDER = group.First().BLOOD_NUM_ORDER;
                                    //medicine.TH_EXP_MEST_MEDICINE_ID = group.First().TH_EXP_MEST_BLOOD_ID;
                                    //medicine.HIS_MEDICINE = ListBlood.Where(x => x.ID == group.First().BLOOD_ID).FirstOrDefault();
                                    var medi = ListBlood.FirstOrDefault(x => x.ID == group.First().BLOOD_ID);
                                    if (medi != null)
                                    {
                                        //var medicineUseForm = listBloodUseForm.FirstOrDefault(x => x.ID == medi.BLOOD_USE_FORM_ID);
                                        //medicine.HIS_BLOOD_USE_FORM = medicineUseForm;
                                        //if (medicineUseForm != null)
                                        //{
                                        //    medicine.BLOOD_USE_FORM_CODE = medicineUseForm.BLOOD_USE_FORM_CODE;
                                        //    medicine.BLOOD_USE_FORM_NAME = medicineUseForm.BLOOD_USE_FORM_NAME;
                                        //}
                                        //medicine.BLOOD_REGISTER_NUMBER = medi.BLOOD_REGISTER_NUMBER;
                                        var impSource = listImpSource.FirstOrDefault(x => x.ID == medi.IMP_SOURCE_ID);
                                        if (impSource != null)
                                        {
                                            medicine.IMP_SOURCE_NAME = impSource.IMP_SOURCE_NAME;
                                            medicine.IMP_SOURCE_CODE = impSource.IMP_SOURCE_CODE;
                                        }
                                    }
                                    var mety = ListBloodType.FirstOrDefault(x => x.ID == group.First().BLOOD_TYPE_ID);
                                    medicine.PARENT_NUM_ORDER = 10000;
                                    if (mety != null)
                                    {
                                        medicine.HEIN_SERVICE_BHYT_NAME = mety.HEIN_SERVICE_BHYT_NAME;
                                        //medicine.BLOOD_TYPE_PROPRIETARY_NAME = mety.BLOOD_TYPE_PROPRIETARY_NAME;
                                        var par = ListBloodType.FirstOrDefault(x => x.ID == mety.PARENT_ID);
                                        if (par != null)
                                        {
                                            medicine.PARENT_NUM_ORDER = par.NUM_ORDER ?? 10000;
                                            medicine.PARENT_SERVICE_CODE = par.BLOOD_TYPE_CODE;
                                            medicine.PARENT_SERVICE_NAME = par.BLOOD_TYPE_NAME;
                                        }
                                    }
                                    ListRdo.Add(medicine);
                                }
                            }
                        }


                        if (takeMaterialData)
                        {
                            if (IsNotNullOrEmpty(ListImpMestMaterial))
                            {

                                var groups = ListImpMestMaterial.GroupBy(o => string.Format(KeyGroupExp, o.SUPPLIER_ID, o.PRICE, o.VAT_RATIO, o.MATERIAL_TYPE_ID, o.ID, o.IMP_MEST_ID));
                                foreach (var group in groups)
                                {
                                    var medicine = new Mrs00553RDO(group.First());
                                    var impMest = ListImpMest.FirstOrDefault(o => o.ID == group.First().IMP_MEST_ID) ?? new V_HIS_IMP_MEST();
                                    if (impMest != null)
                                    {
                                        System.Reflection.PropertyInfo[] pis = Inventec.Common.Repository.Properties.Get<MOS.EFMODEL.DataModels.V_HIS_IMP_MEST>();
                                        foreach (var item in pis)
                                        {
                                            item.SetValue(medicine, item.GetValue(impMest));
                                        }

                                        if (DicSupplier.ContainsKey(impMest.SUPPLIER_ID ?? 0))
                                        {
                                            medicine.SUPPLIER_NAME = DicSupplier[impMest.SUPPLIER_ID ?? 0].SUPPLIER_NAME;
                                        }

                                    }

                                    medicine.AMOUNT = group.Sum(o => o.AMOUNT);
                                    //medicine.DOCUMENT_PRICE = group.GroupBy(o => o.ID).Select(p => p.First()).Sum(o => o.DOCUMENT_PRICE ?? 0);
                                    medicine.MEDI_MATE_CHEMI = "2.VATTU";
                                    medicine.MEDICINE_ID = group.First().MATERIAL_ID;
                                    medicine.MEDICINE_TYPE_ID = group.First().MATERIAL_TYPE_ID;
                                    medicine.MEDICINE_TYPE_CODE = group.First().MATERIAL_TYPE_CODE;
                                    medicine.MEDICINE_TYPE_NAME = group.First().MATERIAL_TYPE_NAME;
                                    medicine.MEDICINE_NUM_ORDER = group.First().MATERIAL_NUM_ORDER;
                                    medicine.TH_EXP_MEST_MEDICINE_ID = group.First().TH_EXP_MEST_MATERIAL_ID;

                                    var medi = ListMaterial.FirstOrDefault(x => x.ID == group.First().MATERIAL_ID);
                                    if (medi != null)
                                    {
                                        //var medicineUseForm = listMedicineUseForm.FirstOrDefault(x => x.ID == medi.MEDICINE_USE_FORM_ID);
                                        //if (medicineUseForm != null)
                                        //{
                                        //    medicine.MEDICINE_USE_FORM_CODE = medicineUseForm.MEDICINE_USE_FORM_CODE;
                                        //    medicine.MEDICINE_USE_FORM_NAME = medicineUseForm.MEDICINE_USE_FORM_NAME;
                                        //}
                                        medicine.MEDICINE_REGISTER_NUMBER = medi.MATERIAL_REGISTER_NUMBER;
                                        var impSource = listImpSource.FirstOrDefault(x => x.ID == medi.IMP_SOURCE_ID);
                                        if (impSource != null)
                                        {
                                            medicine.IMP_SOURCE_NAME = impSource.IMP_SOURCE_NAME;
                                            medicine.IMP_SOURCE_CODE = impSource.IMP_SOURCE_CODE;
                                        }
                                    }
                                    var mety = ListMaterialType.FirstOrDefault(x => x.ID == group.First().MATERIAL_TYPE_ID);
                                    medicine.PARENT_NUM_ORDER = 10000;
                                    if (mety != null)
                                    {
                                        medicine.HEIN_SERVICE_BHYT_NAME = mety.HEIN_SERVICE_BHYT_NAME;
                                        var par = ListMaterialType.FirstOrDefault(x => x.ID == mety.PARENT_ID);
                                        if (par != null)
                                        {
                                            medicine.PARENT_NUM_ORDER = par.NUM_ORDER??10000;
                                            medicine.PARENT_SERVICE_CODE = mety.PARENT_CODE;
                                            medicine.PARENT_SERVICE_NAME = mety.PARENT_NAME;
                                        }
                                    }

                                    ListRdo.Add(medicine);
                                }
                            }
                        }

                        if (takeChemicalSubstanceData)
                        {
                            if (IsNotNullOrEmpty(ListImpMestChemicalSubStance))
                            {

                                var groups = ListImpMestChemicalSubStance.GroupBy(o => string.Format(KeyGroupExp, o.SUPPLIER_ID, o.PRICE, o.VAT_RATIO, o.MATERIAL_TYPE_ID, o.ID, o.IMP_MEST_ID));
                                foreach (var group in groups)
                                {
                                    var medicine = new Mrs00553RDO(group.First());
                                    var impMest = ListImpMest.FirstOrDefault(o => o.ID == group.First().IMP_MEST_ID) ?? new V_HIS_IMP_MEST();
                                    if (impMest != null)
                                    {
                                        System.Reflection.PropertyInfo[] pis = Inventec.Common.Repository.Properties.Get<MOS.EFMODEL.DataModels.V_HIS_IMP_MEST>();
                                        foreach (var item in pis)
                                        {
                                            item.SetValue(medicine, item.GetValue(impMest));
                                        }

                                        if (DicSupplier.ContainsKey(impMest.SUPPLIER_ID ?? 0))
                                        {
                                            medicine.SUPPLIER_NAME = DicSupplier[impMest.SUPPLIER_ID ?? 0].SUPPLIER_NAME;
                                        }

                                    }

                                    medicine.AMOUNT = group.Sum(o => o.AMOUNT);
                                    //medicine.DOCUMENT_PRICE = group.GroupBy(o => o.ID).Select(p => p.First()).Sum(o => o.DOCUMENT_PRICE ?? 0);
                                    medicine.MEDI_MATE_CHEMI = "3.HOACHAT";
                                    medicine.MEDICINE_ID = group.First().MATERIAL_ID;
                                    medicine.MEDICINE_TYPE_ID = group.First().MATERIAL_TYPE_ID;
                                    medicine.MEDICINE_TYPE_CODE = group.First().MATERIAL_TYPE_CODE;
                                    medicine.MEDICINE_TYPE_NAME = group.First().MATERIAL_TYPE_NAME;
                                    medicine.MEDICINE_NUM_ORDER = group.First().MATERIAL_NUM_ORDER;
                                    medicine.TH_EXP_MEST_MEDICINE_ID = group.First().TH_EXP_MEST_MATERIAL_ID;
                                    var medi = ListMaterial.FirstOrDefault(x => x.ID == group.First().MATERIAL_ID);
                                    if (medi != null)
                                    {
                                        //var medicineUseForm = listMedicineUseForm.FirstOrDefault(x => x.ID == medi.MEDICINE_USE_FORM_ID);
                                        //if (medicineUseForm != null)
                                        //{
                                        //    medicine.MEDICINE_USE_FORM_CODE = medicineUseForm.MEDICINE_USE_FORM_CODE;
                                        //    medicine.MEDICINE_USE_FORM_NAME = medicineUseForm.MEDICINE_USE_FORM_NAME;
                                        //}

                                        medicine.MEDICINE_REGISTER_NUMBER = medi.MATERIAL_REGISTER_NUMBER;
                                        var impSource = listImpSource.FirstOrDefault(x => x.ID == medi.IMP_SOURCE_ID);
                                        if (impSource != null)
                                        {
                                            medicine.IMP_SOURCE_NAME = impSource.IMP_SOURCE_NAME;
                                            medicine.IMP_SOURCE_CODE = impSource.IMP_SOURCE_CODE;
                                        }
                                    }
                                    var mety = ListMaterialType.FirstOrDefault(x => x.ID == group.First().MATERIAL_TYPE_ID);
                                    medicine.PARENT_NUM_ORDER = 10000;
                                    if (mety != null)
                                    {
                                        medicine.HEIN_SERVICE_BHYT_NAME = mety.HEIN_SERVICE_BHYT_NAME;
                                        var par = ListMaterialType.FirstOrDefault(x => x.ID == mety.PARENT_ID);
                                        if (par != null)
                                        {
                                            medicine.PARENT_NUM_ORDER = par.NUM_ORDER??10000;
                                            medicine.PARENT_SERVICE_CODE = mety.PARENT_CODE;
                                            medicine.PARENT_SERVICE_NAME = mety.PARENT_NAME;
                                        }
                                    }

                                    ListRdo.Add(medicine);
                                }
                            }
                        }
                    }
                    var groupByDocument = ListImpMest.GroupBy(o => new { o.DOCUMENT_NUMBER, o.SUPPLIER_ID, o.DOCUMENT_DATE });
                    foreach (var item in groupByDocument)
                    {
                        Mrs00553RDO rdo = new Mrs00553RDO();
                        if (DicSupplier.ContainsKey(item.First().SUPPLIER_ID ?? 0))
                        {
                            rdo.SUPPLIER_NAME = DicSupplier[item.First().SUPPLIER_ID ?? 0].SUPPLIER_NAME;
                        }
                        rdo.SUPPLIER_ID = item.First().SUPPLIER_ID;
                        rdo.DOCUMENT_NUMBER = item.First().DOCUMENT_NUMBER;
                        rdo.DOCUMENT_DATE = item.First().DOCUMENT_DATE;
                        rdo.IMP_MEST_CODE = string.Join(", ", item.Select(o => o.IMP_MEST_CODE).Distinct().ToList());
                        rdo.DOCUMENT_PRICE = item.Sum(s => s.DOCUMENT_PRICE);
                        rdo.VAT_RATIO = (ListRdo.FirstOrDefault(o => o.DOCUMENT_NUMBER == item.First().DOCUMENT_NUMBER) ?? new Mrs00553RDO()).VAT_RATIO;
                        ListDocument.Add(rdo);

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
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                dicSingleTag.Add("DIC_BID_AMOUNT", dicBidAmount);

                MOS.MANAGER.HisMediStock.HisMediStockFilterQuery filter = new MOS.MANAGER.HisMediStock.HisMediStockFilterQuery();
                filter.IDs = castFilter.MEDI_STOCK_IDs;
                var mediStock = new MOS.MANAGER.HisMediStock.HisMediStockManager(new CommonParam()).Get(filter);
                if (IsNotNullOrEmpty(mediStock))
                {
                    string name = string.Join(";", mediStock.Select(s => s.MEDI_STOCK_NAME).ToArray());
                    dicSingleTag.Add("MEDI_STOCK_NAME", name);
                }
                string Choice = "";
                if (this.takeMedicineData == true)
                {
                    Choice += "Thuốc;";
                }
                if (this.takeChemicalSubstanceData == true)
                {
                    Choice += "Hóa chất;";
                }
                if (this.takeMaterialData == true)
                {
                    Choice += "Vật tư;";
                }
                if (this.takeBloodData == true)
                {
                    Choice += "Máu;";
                }
                dicSingleTag.Add("SERVICE_GROUP_NAME_CHOICE", Choice);
                //dữ liệu tổng hợp
                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "ImpMest", ListSup);
                objectTag.AddRelationship(store, "ImpMest", "Report", "SUPPLIER_ID", "SUPPLIER_ID");
                objectTag.AddObjectData(store, "GrandParent", DicSupplier.Values.Where(o => ListImpMest.Exists(p => p.SUPPLIER_ID == o.ID)).ToList());

                //dữ liệu gốc
                ListImpMest = ListImpMest.Where(o => ListImpMestMedicine.Exists(p => p.IMP_MEST_ID == o.ID) || ListImpMestMaterial.Exists(p => p.IMP_MEST_ID == o.ID) || ListImpMestChemicalSubStance.Exists(p => p.IMP_MEST_ID == o.ID) || ListImpMestBlood.Exists(p => p.IMP_MEST_ID == o.ID)).ToList();
                objectTag.AddObjectData(store, "Parent", ListImpMest.OrderBy(o => o.KEY_ORDER).ToList());
                objectTag.AddObjectData(store, "ImpMestMedicine", ListImpMestMedicine);
                objectTag.AddObjectData(store, "ImpMestMaterial", ListImpMestMaterial);
                objectTag.AddObjectData(store, "ImpMestChemicalSubStance", ListImpMestChemicalSubStance);
                objectTag.AddObjectData(store, "ImpMestBlood", ListImpMestBlood);
                //objectTag.AddObjectData(store, "ImpMestMedicine", listImpMestMedicine);
                //objectTag.AddObjectData(store, "ImpMestMaterial", listImpMestMaterial);
                //objectTag.AddObjectData(store, "ImpMestChemicalSubStance", listImpMestChemicalSubStance) ;
                objectTag.AddRelationship(store, "GrandParent", "Parent", "ID", "SUPPLIER_ID");
                objectTag.AddRelationship(store, "Parent", "ImpMestMedicine", "ID", "IMP_MEST_ID");
                objectTag.AddRelationship(store, "Parent", "ImpMestMaterial", "ID", "IMP_MEST_ID");
                objectTag.AddRelationship(store, "Parent", "ImpMestChemicalSubStance", "ID", "IMP_MEST_ID");
                objectTag.AddRelationship(store, "Parent", "ImpMestBlood", "ID", "IMP_MEST_ID");

                //dữ liệu hóa đơn chứng từ
                objectTag.AddObjectData(store, "Document", ListDocument);
                objectTag.AddRelationship(store, "GrandParent", "Document", "ID", "SUPPLIER_ID");

                //dữ liệu chi tiết thầu
                objectTag.AddObjectData(store, "BidMety", ListBidMedicineType);
                objectTag.AddObjectData(store, "BidMaty", ListBidMaterialType);
                objectTag.AddObjectData(store, "BidBlty", ListBidBloodType);

                objectTag.SetUserFunction(store, "IsSameAboveRow1", new MRS.MANAGER.Core.MrsReport.RDO.RDOCustomerFuncMergeSameData());
                objectTag.SetUserFunction(store, "IsSameAboveRow2", new MRS.MANAGER.Core.MrsReport.RDO.RDOCustomerFuncMergeSameData());
                objectTag.SetUserFunction(store, "IsSameAboveRow3", new MRS.MANAGER.Core.MrsReport.RDO.RDOCustomerFuncMergeSameData());
                objectTag.SetUserFunction(store, "IsSameAboveRow4", new MRS.MANAGER.Core.MrsReport.RDO.RDOCustomerFuncMergeSameData());
                objectTag.SetUserFunction(store, "IsSameAboveRow5", new MRS.MANAGER.Core.MrsReport.RDO.RDOCustomerFuncMergeSameData());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }


    }
}
