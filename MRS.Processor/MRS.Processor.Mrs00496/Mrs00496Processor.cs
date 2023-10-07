using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Common.Repository;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMestPeriodMate;
using MOS.MANAGER.HisMestPeriodMedi;
using MRS.Proccessor.Mrs00496;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MOS.MANAGER.HisBid;
using MOS.MANAGER.HisMedicineBean;
using MOS.MANAGER.HisMaterialBean;
using MOS.MANAGER.HisMedicinePaty;
using MOS.MANAGER.HisMaterialPaty;
using MOS.MANAGER.HisMediStockMaty;
using MOS.MANAGER.HisMediStockMety;
using MOS.MANAGER.HisBloodType;
using MOS.MANAGER.HisBlood;

namespace MRS.Processor.Mrs00496
{
    public class Mrs00496Processor : AbstractProcessor
    {
        Mrs00496Filter castFilter = null;
        private Dictionary<string, Mrs00496RDO> dicRdo = new Dictionary<string, Mrs00496RDO>();
        List<Mrs00496RDO> ListRdo = new List<Mrs00496RDO>();
        List<Mrs00496RDO> listRdoMedicine = new List<Mrs00496RDO>();
        List<Mrs00496RDO> listRdoMaterial = new List<Mrs00496RDO>();
        List<Mrs00496RDO> listRdoChemistry = new List<Mrs00496RDO>();
        List<Mrs00496RDO> listRdoBlood = new List<Mrs00496RDO>();
        CommonParam paramGet = new CommonParam();
        Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType = new Dictionary<long, V_HIS_MEDICINE_TYPE>();
        Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType = new Dictionary<long, V_HIS_MATERIAL_TYPE>();
        Dictionary<long, V_HIS_BLOOD_TYPE> dicBloodType = new Dictionary<long, V_HIS_BLOOD_TYPE>();

        List<Mrs00496RDO> hisImpMestMedicine = new List<Mrs00496RDO>();
        List<Mrs00496RDO> hisExpMestMedicine = new List<Mrs00496RDO>();
        List<Mrs00496RDO> hisImpMestMaterial = new List<Mrs00496RDO>();
        List<Mrs00496RDO> hisExpMestMaterial = new List<Mrs00496RDO>();
        List<Mrs00496RDO> hisImpMestBlood = new List<Mrs00496RDO>();
        List<Mrs00496RDO> hisExpMestBlood = new List<Mrs00496RDO>();

        List<Mrs00496RDO> listMestPeriodMedi = new List<Mrs00496RDO>(); // DS thuoc chot ki
        List<Mrs00496RDO> listMestPeriodMate = new List<Mrs00496RDO>(); // DS vat tu chot ki
        List<Mrs00496RDO> listMestPeriodBlood = new List<Mrs00496RDO>(); // DS mau chot ki

        List<V_HIS_MEDICINE> hisMedicine = new List<V_HIS_MEDICINE>();
        List<V_HIS_MATERIAL> hisMaterial = new List<V_HIS_MATERIAL>();
        List<V_HIS_BLOOD> hisBlood = new List<V_HIS_BLOOD>();
        List<HIS_MEDICINE_PATY> hisMedicinePaty = new List<HIS_MEDICINE_PATY>();
        List<HIS_MATERIAL_PATY> hisMaterialPaty = new List<HIS_MATERIAL_PATY>();
        List<Mrs00496RDO> ListMedicineBean = new List<Mrs00496RDO>();
        List<Mrs00496RDO> ListMaterialBean = new List<Mrs00496RDO>();
        List<HIS_BID> listHisBid = new List<HIS_BID>();

        private string MEDI_STOCK_CODE;
        private string MEDI_STOCK_NAME;
        string thisReportTypeCode = "";
        long? timeTo = 0;
        long? timeFrom = 0;
        bool takeMedicineData = true;
        bool takeMaterialData = true;
        bool takeChemicalSubstanceData = true;
        bool takeBloodData = true;
        Dictionary<long, HIS_MEDI_STOCK_MATY> DicMediStockMaty = new Dictionary<long, HIS_MEDI_STOCK_MATY>();
        Dictionary<long, HIS_MEDI_STOCK_METY> DicMediStockMety = new Dictionary<long, HIS_MEDI_STOCK_METY>();
        Dictionary<long, HIS_MEDI_STOCK_BLTY> DicMediStockBlty = new Dictionary<long, HIS_MEDI_STOCK_BLTY>();

        public Mrs00496Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00496Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            castFilter = (Mrs00496Filter)this.reportFilter;
            try
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

                if (castFilter.IS_BLOOD != true && (castFilter.IS_MEDICINE == true || castFilter.IS_MATERIAL == true || castFilter.IS_CHEMICAL_SUBSTANCE == true))
                {
                    takeBloodData = false;
                }

                listHisBid = new HisBidManager().Get(new HisBidFilterQuery());
                timeTo = castFilter.TIME_TO ?? castFilter.TIME ?? Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(System.DateTime.Now);
                timeFrom = castFilter.TIME_FROM ?? castFilter.TIME ?? Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(System.DateTime.Now);
                GetMetyMatyBlty();
                GetMedicineBeanMaterialBean();

                //Du lieu ton thuoc, vat tu 
                GetMediMateBloodPeriod();

                //Xuat thuoc, vat tu 
                GetExpMestMediMateBlood();

                //Nhap thuoc, vat tu 
                GetImpMestMediMateBlood();

                GetMediMateBlood();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetMediMateBlood()
        {
            try
            {
                if (takeMedicineData)
                {
                    List<long> medicineIds = new List<long>();
                    if (ListMedicineBean != null)
                    {
                        medicineIds.AddRange(ListMedicineBean.Select(o => o.MEDI_MATE_ID).ToList());
                    }
                    if (listMestPeriodMedi != null)
                    {
                        medicineIds.AddRange(listMestPeriodMedi.Select(o => o.MEDI_MATE_ID).ToList());
                    }
                    if (hisImpMestMedicine != null)
                    {
                        medicineIds.AddRange(hisImpMestMedicine.Select(o => o.MEDI_MATE_ID).ToList());
                    }
                    if (hisExpMestMedicine != null)
                    {
                        medicineIds.AddRange(hisExpMestMedicine.Select(o => o.MEDI_MATE_ID).ToList());
                    }

                    medicineIds = medicineIds.Distinct().ToList();
                    LogSystem.Info("medicineId" + medicineIds.Count);
                    if (medicineIds != null && medicineIds.Count > 0)
                    {
                        HisMedicineViewFilterQuery Medicinefilter = new HisMedicineViewFilterQuery();
                        Medicinefilter.IDs = medicineIds;
                        hisMedicine.AddRange(new ManagerSql().Get(Medicinefilter));
                        HisMedicinePatyFilterQuery MedicinePatyfilter = new HisMedicinePatyFilterQuery();
                        MedicinePatyfilter.MEDICINE_IDs = medicineIds;
                        MedicinePatyfilter.PATIENT_TYPE_ID = castFilter.PRICE_PATIENT_TYPE_ID;
                        hisMedicinePaty.AddRange(new ManagerSql().Get(MedicinePatyfilter));
                    }

                    LogSystem.Info("hisMedicine" + hisMedicine.Count);
                }

                if (takeChemicalSubstanceData || takeMaterialData)
                {
                    List<long> materialIds = new List<long>();
                    if (ListMaterialBean != null)
                    {
                        materialIds.AddRange(ListMaterialBean.Select(o => o.MEDI_MATE_ID).ToList());
                    }
                    if (listMestPeriodMate != null)
                    {
                        materialIds.AddRange(listMestPeriodMate.Select(o => o.MEDI_MATE_ID).ToList());
                    }
                    if (hisImpMestMaterial != null)
                    {
                        materialIds.AddRange(hisImpMestMaterial.Select(o => o.MEDI_MATE_ID).ToList());
                    }
                    if (hisExpMestMaterial != null)
                    {
                        materialIds.AddRange(hisExpMestMaterial.Select(o => o.MEDI_MATE_ID).ToList());
                    }

                    materialIds = materialIds.Distinct().ToList();

                    LogSystem.Info("materialId" + materialIds.Count);
                    if (materialIds != null && materialIds.Count > 0)
                    {
                        HisMaterialViewFilterQuery Materialfilter = new HisMaterialViewFilterQuery();
                        Materialfilter.IDs = materialIds;
                        hisMaterial.AddRange(new ManagerSql().Get(Materialfilter));
                        HisMaterialPatyFilterQuery MaterialPatyfilter = new HisMaterialPatyFilterQuery();
                        MaterialPatyfilter.MATERIAL_IDs = materialIds;
                        MaterialPatyfilter.PATIENT_TYPE_ID = castFilter.PRICE_PATIENT_TYPE_ID;
                        hisMaterialPaty.AddRange(new ManagerSql().Get(MaterialPatyfilter));
                    }
                    LogSystem.Info("hisMaterial" + hisMaterial.Count);
                }

                if (takeBloodData)
                {
                    List<long> bloodIds = new List<long>();
                    if (listMestPeriodBlood != null)
                    {
                        bloodIds.AddRange(listMestPeriodBlood.Select(o => o.MEDI_MATE_ID).ToList());
                    }
                    if (hisImpMestBlood != null)
                    {
                        bloodIds.AddRange(hisImpMestBlood.Select(o => o.MEDI_MATE_ID).ToList());
                    }
                    if (hisExpMestBlood != null)
                    {
                        bloodIds.AddRange(hisExpMestBlood.Select(o => o.MEDI_MATE_ID).ToList());
                    }

                    bloodIds = bloodIds.Distinct().ToList();

                    LogSystem.Info("bloodId" + bloodIds.Count);
                    if (bloodIds != null && bloodIds.Count > 0)
                    {
                        HisBloodViewFilterQuery Bloodfilter = new HisBloodViewFilterQuery();
                        Bloodfilter.IDs = bloodIds;
                        hisBlood.AddRange(new ManagerSql().Get(Bloodfilter));
                    }
                    LogSystem.Info("bloodId" + hisBlood.Count);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSession.Error(ex);
            }
        }

        private void GetMetyMatyBlty()
        {
            CommonParam paramGet = new CommonParam();
            var ListMedicineType = new List<V_HIS_MEDICINE_TYPE>();
            var ListMaterialType = new List<V_HIS_MATERIAL_TYPE>();
            var ListBloodType = new List<V_HIS_BLOOD_TYPE>();
            if (takeMedicineData)
            {
                ListMedicineType = new HisMedicineTypeManager(paramGet).GetView(new HisMedicineTypeViewFilterQuery());
            }

            if (takeChemicalSubstanceData || takeMaterialData)
            {
                HisMaterialTypeViewFilterQuery filterMate = new HisMaterialTypeViewFilterQuery();
                ListMaterialType = new HisMaterialTypeManager(paramGet).GetView(new HisMaterialTypeViewFilterQuery());
            }
            if (takeBloodData)
            {
                ListBloodType = new HisBloodTypeManager(paramGet).GetView(new HisBloodTypeViewFilterQuery());
            }

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

            if (IsNotNullOrEmpty(ListBloodType))
            {
                foreach (var item in ListBloodType)
                {
                    dicBloodType[item.ID] = item;
                }
            }

            var listMediStockMaty = new HisMediStockMatyManager().GetByMediStockId(castFilter.MEDI_STOCK_ID);
            if (IsNotNullOrEmpty(listMediStockMaty))
            {
                foreach (var item in listMediStockMaty)
                {
                    DicMediStockMaty[item.MATERIAL_TYPE_ID] = item;
                }
            }

            var listMediStockMety = new HisMediStockMetyManager().GetByMediStockId(castFilter.MEDI_STOCK_ID);
            if (IsNotNullOrEmpty(listMediStockMety))
            {
                foreach (var item in listMediStockMety)
                {
                    DicMediStockMety[item.MEDICINE_TYPE_ID] = item;
                }
            }

            var listMediStockBlty = this.GetByMediStockId(castFilter.MEDI_STOCK_ID);
            if (IsNotNullOrEmpty(listMediStockMety))
            {
                foreach (var item in listMediStockBlty)
                {
                    DicMediStockBlty[item.BLOOD_TYPE_ID] = item;
                }
            }
        }

        private List<HIS_MEDI_STOCK_BLTY> GetByMediStockId(long mediStockId)
        {
            return new MOS.DAO.Sql.SqlDAO().GetSql<HIS_MEDI_STOCK_BLTY>(string.Format("select * from his_medi_stock_blty where medi_stock_id={0}", mediStockId));
        }

        private void GetMedicineBeanMaterialBean()
        {
            try
            {
                if (takeMedicineData)
                {
                    ListMedicineBean = new ManagerSql().GetMediBean(new List<long>() { castFilter.MEDI_STOCK_ID });
                }
                if (takeChemicalSubstanceData || takeMaterialData)
                {
                    ListMaterialBean = new ManagerSql().GetMateBean(new List<long>() { castFilter.MEDI_STOCK_ID });
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void GetMediMateBloodPeriod()
        {
            if (takeMedicineData)
            {
                listMestPeriodMedi = new ManagerSql().GetMediPeriod(new List<long>() { castFilter.MEDI_STOCK_ID }, this.timeFrom ?? 0);
            }
            if (takeChemicalSubstanceData || takeMaterialData)
            {
                listMestPeriodMate = new ManagerSql().GetMatePeriod(new List<long>() { castFilter.MEDI_STOCK_ID }, this.timeFrom ?? 0);
            }
            if (takeBloodData)
            {
                listMestPeriodBlood = new ManagerSql().GetBloodPeriod(new List<long>() { castFilter.MEDI_STOCK_ID }, this.timeFrom ?? 0);
            }
        }

        private void GetExpMestMediMateBlood()
        {
            if (takeChemicalSubstanceData || takeMaterialData)
            {
                hisExpMestMaterial = new ManagerSql().GetMateExp(new List<long>() { castFilter.MEDI_STOCK_ID }, this.timeFrom ?? 0, this.timeTo ?? 0, castFilter);
            }
            if (takeMedicineData)
            {
                hisExpMestMedicine = new ManagerSql().GetMediExp(new List<long>() { castFilter.MEDI_STOCK_ID }, this.timeFrom ?? 0, this.timeTo ?? 0, castFilter);
            }
            if (takeBloodData)
            {
                hisExpMestBlood = new ManagerSql().GetBloodExp(new List<long>() { castFilter.MEDI_STOCK_ID }, this.timeFrom ?? 0, this.timeTo ?? 0, castFilter);
            }
        }

        private void GetImpMestMediMateBlood()
        {
            if (takeChemicalSubstanceData || takeMaterialData)
            {
                hisImpMestMaterial = new ManagerSql().GetMateImp(new List<long>() { castFilter.MEDI_STOCK_ID }, this.timeFrom ?? 0, this.timeTo ?? 0);
            }
            if (takeMedicineData)
            {
                hisImpMestMedicine = new ManagerSql().GetMediImp(new List<long>() { castFilter.MEDI_STOCK_ID }, this.timeFrom ?? 0, this.timeTo ?? 0);
            }
            if (takeBloodData)
            {
                hisImpMestBlood = new ManagerSql().GetBloodImp(new List<long>() { castFilter.MEDI_STOCK_ID }, this.timeFrom ?? 0, this.timeTo ?? 0);
            }
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                dicRdo.Clear();

                if (takeChemicalSubstanceData || takeMaterialData)
                {
                    // gan du lieu ton kho bean cua cac lo vat tu
                    foreach (var item in ListMaterialBean)
                    {
                        string key = string.Format("{0}_{1}", item.MEDI_MATE_ID, item.TYPE);
                        if (!dicRdo.ContainsKey(key))
                        {
                            var material = hisMaterial.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                            var materialType = dicMaterialType.ContainsKey(item.MATERIAL_TYPE_ID) ? dicMaterialType[item.MATERIAL_TYPE_ID] : null;
                            InfoMateToRdo(item, materialType, material);
                            dicRdo.Add(key, item);
                        }
                        else
                        {
                            dicRdo[key].BEAN_AMOUNT += item.BEAN_AMOUNT;
                            dicRdo[key].ENABLE_AMOUNT += item.ENABLE_AMOUNT;
                        }
                    }

                    ListMaterialBean.Clear();
                    ListMaterialBean = null;
                    // gan du lieu chot ky vat tu vao so luong ton dau cua cac lo vat tu
                    foreach (var item in listMestPeriodMate)
                    {
                        string key = string.Format("{0}_{1}", item.MEDI_MATE_ID, item.TYPE);
                        if (!dicRdo.ContainsKey(key))
                        {
                            var material = hisMaterial.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                            var materialType = dicMaterialType.ContainsKey(item.MATERIAL_TYPE_ID) ? dicMaterialType[item.MATERIAL_TYPE_ID] : null;
                            InfoMateToRdo(item, materialType, material);
                            dicRdo.Add(key, item);
                        }
                        else
                        {
                            dicRdo[key].AMOUNT += item.AMOUNT;
                            dicRdo[key].TOTAL_PRICE += item.AMOUNT * dicRdo[key].VIR_IMP_PRICE;
                        }
                    }

                    listMestPeriodMate.Clear();
                    listMestPeriodMate = null;
                    // gan du lieu xuat vat tu vao so luong ton dau, ton cuoi, cac loai xuat, cac khoa yeu cau, cac li do xuat khac cua cac lo vat tu
                    foreach (var item in hisExpMestMaterial)
                    {
                        string key = string.Format("{0}_{1}", item.MEDI_MATE_ID, item.TYPE);
                        if (!dicRdo.ContainsKey(key))
                        {
                            var material = hisMaterial.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                            var materialType = dicMaterialType.ContainsKey(item.MATERIAL_TYPE_ID) ? dicMaterialType[item.MATERIAL_TYPE_ID] : null;
                            //them thong tin lo
                            InfoMateToRdo(item, materialType, material);
                            dicRdo.Add(key, item);
                        }
                        else
                        {
                            dicRdo[key].AMOUNT += item.AMOUNT;
                            dicRdo[key].TOTAL_PRICE += item.AMOUNT * dicRdo[key].VIR_IMP_PRICE;
                            dicRdo[key].EXP_AMOUNT += item.EXP_AMOUNT;
                            dicRdo[key].EXP_TOTAL_PRICE += item.EXP_AMOUNT * dicRdo[key].VIR_IMP_PRICE;
                        }
                    }

                    hisExpMestMaterial.Clear();
                    hisExpMestMaterial = null;
                    // gan du lieu nhap vat tu vao so luong ton dau, ton cuoi, cac loai nhap cua cac lo vat tu
                    foreach (var item in hisImpMestMaterial)
                    {
                        string key = string.Format("{0}_{1}", item.MEDI_MATE_ID, item.TYPE);
                        if (!dicRdo.ContainsKey(key))
                        {
                            var material = hisMaterial.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                            var materialType = dicMaterialType.ContainsKey(item.MATERIAL_TYPE_ID) ? dicMaterialType[item.MATERIAL_TYPE_ID] : null;
                            //them thong tin lo
                            InfoMateToRdo(item, materialType, material);

                            dicRdo.Add(key, item);
                        }
                        else
                        {
                            dicRdo[key].AMOUNT += item.AMOUNT;
                            dicRdo[key].TOTAL_PRICE += item.AMOUNT * dicRdo[key].VIR_IMP_PRICE;
                            dicRdo[key].IMP_AMOUNT += item.IMP_AMOUNT;
                            dicRdo[key].IMP_TOTAL_PRICE += item.IMP_AMOUNT * dicRdo[key].VIR_IMP_PRICE;
                        }
                    }

                    hisImpMestMaterial.Clear();
                    hisImpMestMaterial = null;
                }
                if (takeMedicineData)
                {
                    // gan du lieu ton kho bean cua cac lo vat tu
                    foreach (var item in ListMedicineBean)
                    {
                        string key = string.Format("{0}_{1}", item.MEDI_MATE_ID, item.TYPE);
                        if (!dicRdo.ContainsKey(key))
                        {
                            var medicine = hisMedicine.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                            var medicineType = dicMedicineType.ContainsKey(item.MEDICINE_TYPE_ID) ? dicMedicineType[item.MEDICINE_TYPE_ID] : null;
                            InfoMediToRdo(item, medicineType, medicine);
                            dicRdo.Add(key, item);
                        }
                        else
                        {
                            dicRdo[key].BEAN_AMOUNT += item.BEAN_AMOUNT;
                            dicRdo[key].ENABLE_AMOUNT += item.ENABLE_AMOUNT;
                        }
                    }

                    ListMedicineBean.Clear();
                    ListMedicineBean = null;
                    // gan du lieu chot ky thuoc vao so luong ton dau cua cac lo thuoc
                    foreach (var item in listMestPeriodMedi)
                    {
                        string key = string.Format("{0}_{1}", item.MEDI_MATE_ID, item.TYPE);
                        if (!dicRdo.ContainsKey(key))
                        {
                            var medicine = hisMedicine.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                            var medicineType = dicMedicineType.ContainsKey(item.MEDICINE_TYPE_ID) ? dicMedicineType[item.MEDICINE_TYPE_ID] : null;
                            InfoMediToRdo(item, medicineType, medicine);
                            dicRdo.Add(key, item);
                        }
                        else
                        {
                            dicRdo[key].AMOUNT += item.AMOUNT;
                            dicRdo[key].TOTAL_PRICE += item.AMOUNT * dicRdo[key].VIR_IMP_PRICE;
                        }
                    }
                    listMestPeriodMedi.Clear();
                    listMestPeriodMedi = null;
                    // gan du lieu xuat thuoc vao so luong ton dau, ton cuoi, cac loai xuat, cac khoa yeu cau, cac li do xuat khac cua cac lo thuoc
                    foreach (var item in hisExpMestMedicine)
                    {
                        string key = string.Format("{0}_{1}", item.MEDI_MATE_ID, item.TYPE);
                        if (!dicRdo.ContainsKey(key))
                        {
                            var medicine = hisMedicine.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                            var medicineType = dicMedicineType.ContainsKey(item.MEDICINE_TYPE_ID) ? dicMedicineType[item.MEDICINE_TYPE_ID] : null;
                            //them thong tin lo
                            InfoMediToRdo(item, medicineType, medicine);
                            dicRdo.Add(key, item);
                        }
                        else
                        {
                            dicRdo[key].AMOUNT += item.AMOUNT;
                            dicRdo[key].TOTAL_PRICE += item.AMOUNT * dicRdo[key].VIR_IMP_PRICE;
                            dicRdo[key].EXP_AMOUNT += item.EXP_AMOUNT;
                            dicRdo[key].EXP_TOTAL_PRICE += item.EXP_AMOUNT * dicRdo[key].VIR_IMP_PRICE;
                        }
                    }

                    hisExpMestMedicine.Clear();
                    hisExpMestMedicine = null;
                    // gan du lieu nhap thuoc vao so luong ton dau, ton cuoi, cac loai nhap cua cac lo thuoc
                    foreach (var item in hisImpMestMedicine)
                    {
                        string key = string.Format("{0}_{1}", item.MEDI_MATE_ID, item.TYPE);
                        if (!dicRdo.ContainsKey(key))
                        {
                            var medicine = hisMedicine.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                            var medicineType = dicMedicineType.ContainsKey(item.MEDICINE_TYPE_ID) ? dicMedicineType[item.MEDICINE_TYPE_ID] : null;
                            //them thong tin lo
                            InfoMediToRdo(item, medicineType, medicine);
                            dicRdo.Add(key, item);
                        }
                        else
                        {
                            dicRdo[key].AMOUNT += item.AMOUNT;
                            dicRdo[key].TOTAL_PRICE += item.AMOUNT * dicRdo[key].VIR_IMP_PRICE;
                            dicRdo[key].IMP_AMOUNT += item.IMP_AMOUNT;
                            dicRdo[key].IMP_TOTAL_PRICE += item.IMP_AMOUNT * dicRdo[key].VIR_IMP_PRICE;
                        }
                    }
                    hisImpMestMedicine.Clear();
                    hisImpMestMedicine = null;
                }
                if (takeBloodData)
                {
                    // gan du lieu chot ky thuoc vao so luong ton dau cua cac lo mau
                    foreach (var item in listMestPeriodBlood)
                    {
                        string key = string.Format("{0}_{1}", item.MEDI_MATE_ID, item.TYPE);
                        if (!dicRdo.ContainsKey(key))
                        {
                            var blood = hisBlood.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                            var bloodType = dicBloodType.ContainsKey(item.BLOOD_TYPE_ID) ? dicBloodType[item.BLOOD_TYPE_ID] : null;
                            InfoBloodToRdo(item, bloodType, blood);
                            dicRdo.Add(key, item);
                        }
                        else
                        {
                            dicRdo[key].AMOUNT += item.AMOUNT;
                            dicRdo[key].TOTAL_PRICE += item.AMOUNT * dicRdo[key].VIR_IMP_PRICE;
                        }
                    }
                    listMestPeriodBlood.Clear();
                    listMestPeriodBlood = null;
                    // gan du lieu xuat thuoc vao so luong ton dau, ton cuoi, cac loai xuat, cac khoa yeu cau, cac li do xuat khac cua cac lo thuoc
                    foreach (var item in hisExpMestBlood)
                    {
                        string key = string.Format("{0}_{1}", item.MEDI_MATE_ID, item.TYPE);
                        if (!dicRdo.ContainsKey(key))
                        {
                            var blood = hisBlood.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                            var bloodType = dicBloodType.ContainsKey(item.BLOOD_TYPE_ID) ? dicBloodType[item.BLOOD_TYPE_ID] : null;
                            //them thong tin lo
                            InfoBloodToRdo(item, bloodType, blood);
                            dicRdo.Add(key, item);
                        }
                        else
                        {
                            dicRdo[key].AMOUNT += item.AMOUNT;
                            dicRdo[key].TOTAL_PRICE += item.AMOUNT * dicRdo[key].VIR_IMP_PRICE;
                            dicRdo[key].EXP_AMOUNT += item.EXP_AMOUNT;
                            dicRdo[key].EXP_TOTAL_PRICE += item.EXP_AMOUNT * dicRdo[key].VIR_IMP_PRICE;
                        }
                    }

                    hisExpMestBlood.Clear();
                    hisExpMestBlood = null;
                    // gan du lieu nhap thuoc vao so luong ton dau, ton cuoi, cac loai nhap cua cac lo thuoc
                    foreach (var item in hisImpMestBlood)
                    {
                        string key = string.Format("{0}_{1}", item.MEDI_MATE_ID, item.TYPE);
                        if (!dicRdo.ContainsKey(key))
                        {
                            var blood = hisBlood.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                            var bloodType = dicBloodType.ContainsKey(item.BLOOD_TYPE_ID) ? dicBloodType[item.BLOOD_TYPE_ID] : null;
                            //them thong tin lo
                            InfoBloodToRdo(item, bloodType, blood);
                            dicRdo.Add(key, item);
                        }
                        else
                        {
                            dicRdo[key].AMOUNT += item.AMOUNT;
                            dicRdo[key].TOTAL_PRICE += item.AMOUNT * dicRdo[key].VIR_IMP_PRICE;
                            dicRdo[key].IMP_AMOUNT += item.IMP_AMOUNT;
                            dicRdo[key].IMP_TOTAL_PRICE += item.IMP_AMOUNT * dicRdo[key].VIR_IMP_PRICE;
                        }
                    }
                    hisImpMestBlood.Clear();
                    hisImpMestBlood = null;
                }

                //gop theo dieu kien loc
                MergeByFilter();
                //gom rdo theo loai, ma, gia
                if (this.castFilter.IS_DETAIL_MEDICINE == true)
                {

                    GroupByMaterialTypeIdMedicineTypeIdPriceTypeAndExpiredDate();
                }
                else
                {
                    GroupByMediMateTypePrice();
                }
                if (ListRdo != null && ListRdo.Count > 0)
                    ListRdo = ListRdo.Where(o => o.AMOUNT != 0 || (o.EXP_AMOUNT != 0 && castFilter.TIME_FROM > 0)).ToList();

                if (castFilter.IS_MEDICINE == false && castFilter.IS_MATERIAL == false && castFilter.IS_CHEMICAL_SUBSTANCE == false && castFilter.IS_BLOOD == false)
                {
                    ListRdo = ListRdo.Where(o => o.AMOUNT != 0 || (o.EXP_AMOUNT != 0 && castFilter.TIME_FROM > 0)).ToList();
                }
                var listRdoNew = ListRdo.ToList();
                if (castFilter.IS_MEDICINE == true || castFilter.IS_MATERIAL == true || castFilter.IS_CHEMICAL_SUBSTANCE == true || castFilter.IS_BLOOD == true)
                {
                    ListRdo.Clear();
                    if (castFilter.IS_MEDICINE == true)
                    {
                        var medicine = listRdoNew.Where(o => o.TYPE == "THUOC").ToList();
                        ListRdo.AddRange(medicine);
                    }
                    if (castFilter.IS_MATERIAL == true)
                    {
                        var material = listRdoNew.Where(o => o.TYPE == "VATTU").ToList();
                        ListRdo.AddRange(material);
                    }
                    if (castFilter.IS_CHEMICAL_SUBSTANCE == true)
                    {
                        var chemical = listRdoNew.Where(o => o.TYPE == "HOACHAT").ToList();
                        ListRdo.AddRange(chemical);
                    }
                    if (castFilter.IS_BLOOD == true)
                    {
                        var blood = listRdoNew.Where(o => o.TYPE == "MAU").ToList();
                        ListRdo.AddRange(blood);
                    }
                }



                if (ListRdo != null && ListRdo.Count > 0)
                {
                    //Tach rdo ra thanh cac danh sach thuoc vat tu hoa chat
                    if (takeMedicineData)
                    {
                        listRdoMedicine = ListRdo.Where(o => o.TYPE == "THUOC").ToList();
                        AddInfoGroup(listRdoMedicine);
                    }

                    if (takeMaterialData)
                    {
                        listRdoMaterial = ListRdo.Where(o => o.TYPE == "VATTU").ToList();
                        AddInfoGroup(listRdoMaterial);
                    }

                    if (takeChemicalSubstanceData)
                    {
                        listRdoChemistry = ListRdo.Where(o => o.TYPE == "HOACHAT").ToList();
                        AddInfoGroup(listRdoChemistry);
                    }

                    if (takeBloodData)
                    {
                        listRdoBlood = ListRdo.Where(o => o.TYPE == "MAU").ToList();
                        AddInfoGroup(listRdoBlood);
                    }
                }

                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        private void InfoMediToRdo(Mrs00496RDO item, V_HIS_MEDICINE_TYPE medicineType, V_HIS_MEDICINE medicine)
        {
            if (medicine != null)
            {
                //giá bán
                if (medicine.IS_SALE_EQUAL_IMP_PRICE == 1)
                {
                    item.SALE_PRICE = Math.Round(medicine.IMP_PRICE * (1 + medicine.IMP_VAT_RATIO), 0, MidpointRounding.AwayFromZero);
                }
                else if (hisMedicinePaty != null && hisMedicinePaty.Count > 0)
                {
                    var Paty = hisMedicinePaty.FirstOrDefault(o => o.MEDICINE_ID == medicine.ID);
                    if (Paty != null && Paty.EXP_PRICE > 0)
                    {
                        item.SALE_PRICE = Paty.EXP_PRICE;
                        item.SALE_VAT_RATIO = Paty.EXP_VAT_RATIO;
                    }
                }
                /*
                 * bổ sung điều kiện lọc giá theo đối tượng, 
                 * khi chọn đối tượng nào thì sẽ thay đối cột giá theo đối tượng đó trong chính sách giá
                 */
                if (castFilter.PRICE_PATIENT_TYPE_ID != null)
                {
                    if (medicine.IS_SALE_EQUAL_IMP_PRICE == 1)
                    {
                        item.SALE_PRICE = Math.Round(medicine.IMP_PRICE * (1 + medicine.IMP_VAT_RATIO), 0, MidpointRounding.AwayFromZero);
                    }
                    else if (hisMedicinePaty != null && hisMedicinePaty.Count > 0)
                    {
                        var Paty = hisMedicinePaty.FirstOrDefault(o => o.MEDICINE_ID == medicine.ID && castFilter.PRICE_PATIENT_TYPE_ID == o.PATIENT_TYPE_ID);
                        if (Paty != null && Paty.EXP_PRICE > 0)
                        {
                            item.SALE_PRICE = Paty.EXP_PRICE;
                            item.SALE_VAT_RATIO = Paty.EXP_VAT_RATIO;
                            medicine.IMP_PRICE = Paty.EXP_PRICE;
                            medicine.IMP_VAT_RATIO = Paty.EXP_VAT_RATIO;
                        }
                    }
                }
                //giá báo cáo theo lựa chọn
                if (castFilter.INPUT_DATA_ID_PRICE_TYPE == 1)//lựa chọn giá bán trước vat
                {
                    medicine.IMP_PRICE = item.SALE_PRICE ?? 0;
                    medicine.IMP_VAT_RATIO = 0;
                }
                else if (castFilter.INPUT_DATA_ID_PRICE_TYPE == 2)//lựa chọn giá bán sau vat
                {

                    medicine.IMP_PRICE = (item.SALE_PRICE ?? 0) * (1 + item.SALE_VAT_RATIO);
                    medicine.IMP_VAT_RATIO = 0;
                }
                else if (castFilter.INPUT_DATA_ID_PRICE_TYPE == 3)//lựa chọn giá nhập trước vat
                {
                    medicine.IMP_PRICE = medicine.IMP_PRICE;
                    medicine.IMP_VAT_RATIO = 0;
                }
                else if (castFilter.INPUT_DATA_ID_PRICE_TYPE == 4)//lựa chọn giá nhập sau vat
                {

                    medicine.IMP_PRICE = medicine.IMP_PRICE * (1 + medicine.IMP_VAT_RATIO);
                    medicine.IMP_VAT_RATIO = 0;
                }
                item.BID_GROUP_CODE = medicine.TDL_BID_GROUP_CODE;
                item.BID_NUM_ORDER = medicine.TDL_BID_NUM_ORDER;
                item.TDL_BID_NUMBER = medicine.TDL_BID_NUMBER;
                item.PACKAGE_NUMBER = medicine.PACKAGE_NUMBER;
                item.IMP_VAT = medicine.IMP_VAT_RATIO;
                item.VIR_IMP_PRICE = medicine.IMP_PRICE * (1 + medicine.IMP_VAT_RATIO);
                item.PRICE = medicine.IMP_PRICE;
                item.PRICE_AFTER_VAT = Math.Round(medicine.IMP_PRICE * (1 + medicine.IMP_VAT_RATIO), 0, MidpointRounding.AwayFromZero);
                item.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(medicine.EXPIRED_DATE ?? 0);
                item.TOTAL_PRICE = item.AMOUNT * medicine.IMP_PRICE * (1 + medicine.IMP_VAT_RATIO);
                item.IMP_TOTAL_PRICE = item.IMP_AMOUNT * medicine.IMP_PRICE * (1 + medicine.IMP_VAT_RATIO);
                var bid = (this.listHisBid ?? new List<HIS_BID>()).FirstOrDefault(o => o.ID == medicine.BID_ID);
                if (bid != null)
                {
                    item.BID_NUMBER = bid != null ? bid.BID_NUMBER : (medicine != null ? medicine.TDL_BID_NUMBER : "");
                    //item.BID_GROUP_CODE = bid != null ? bid.GROUP_CODE : (medicine != null ? medicine.TDL_BID_GROUP_CODE : "");
                    item.BID_NAME = medicine != null ? medicine.TDL_BID_PACKAGE_CODE : "";
                    item.BID_NUM_ORDER = medicine != null ? medicine.TDL_BID_NUM_ORDER : "";
                    item.BID_YEAR = medicine != null ? medicine.TDL_BID_YEAR : "";
                }

                if (medicineType != null)
                {
                    item.MEDICINE_TYPE_CODE = medicineType.MEDICINE_TYPE_CODE;
                    item.MEDICINE_TYPE_NAME = medicineType.MEDICINE_TYPE_NAME;
                    item.SERVICE_UNIT_NAME = medicineType.SERVICE_UNIT_NAME;
                    item.CONCENTRA = medicineType.CONCENTRA;
                    item.REGISTER_NUMBER = medicine.MEDICINE_REGISTER_NUMBER ?? medicineType.REGISTER_NUMBER;
                    item.MANUFACTURER_NAME = medicineType.MANUFACTURER_NAME;
                    item.PACKING_TYPE_NAME = medicineType.PACKING_TYPE_NAME;
                    item.NATIONAL_NAME = medicineType.NATIONAL_NAME;
                    item.RECORDING_TRANSACTION = medicineType.RECORDING_TRANSACTION;
                    //item.MEDICINE_TYPE_PROPRIETARY_NAME = medicineType.MEDICINE_TYPE_PROPRIETARY_NAME;
                    //item.HEIN_SERVICE_CODE = medicineType.HEIN_SERVICE_BHYT_CODE;
                    //item.HEIN_SERVICE_NAME = medicineType.HEIN_SERVICE_BHYT_NAME;
                    item.ACTIVE_INGR_BHYT_CODE = medicine.ACTIVE_INGR_BHYT_CODE ?? medicineType.ACTIVE_INGR_BHYT_CODE;
                    item.ACTIVE_INGR_BHYT_NAME = medicine.ACTIVE_INGR_BHYT_NAME ?? medicineType.ACTIVE_INGR_BHYT_NAME;
                    //item.MEDICINE_USE_FORM_CODE = medicineType.MEDICINE_USE_FORM_CODE;
                    //item.MEDICINE_USE_FORM_NAME = medicineType.MEDICINE_USE_FORM_NAME;
                    item.BYT_NUM_ORDER = medicineType.BYT_NUM_ORDER;
                    item.TCY_NUM_ORDER = medicineType.TCY_NUM_ORDER;
                }

                //if (suppliers != null)
                //{
                //    var supplier = suppliers.FirstOrDefault(o => o.ID == medicine.SUPPLIER_ID);
                //    if (supplier != null)
                //    {
                //        item.SUPPLIER_CODE = supplier.SUPPLIER_CODE;
                //        item.SUPPLIER_NAME = supplier.SUPPLIER_NAME;
                //        item.SUPPLIER_ID = supplier.ID;
                //    }
                //}
            }

        }

        private void InfoMateToRdo(Mrs00496RDO item, V_HIS_MATERIAL_TYPE materialType, V_HIS_MATERIAL material)
        {
            if (material != null)
            {
                //giá bán
                if (material.IS_SALE_EQUAL_IMP_PRICE == 1)
                {
                    item.SALE_PRICE = Math.Round(material.IMP_PRICE * (1 + material.IMP_VAT_RATIO), 0, MidpointRounding.AwayFromZero);
                }
                else if (hisMaterialPaty != null && hisMaterialPaty.Count > 0)
                {
                    var Paty = hisMaterialPaty.FirstOrDefault(o => o.MATERIAL_ID == material.ID);
                    if (Paty != null && Paty.EXP_PRICE > 0)
                    {
                        item.SALE_PRICE = Paty.EXP_PRICE;
                        item.SALE_VAT_RATIO = Paty.EXP_VAT_RATIO;
                    }
                }
                /*
                * bổ sung điều kiện lọc giá theo đối tượng, 
                * khi chọn đối tượng nào thì sẽ thay đối cột giá theo đối tượng đó trong chính sách giá
                */
                if (castFilter.PRICE_PATIENT_TYPE_ID != null)
                {
                    if (material.IS_SALE_EQUAL_IMP_PRICE == 1)
                    {
                        item.SALE_PRICE = Math.Round(material.IMP_PRICE * (1 + material.IMP_VAT_RATIO), 0, MidpointRounding.AwayFromZero);
                    }
                    else if (hisMaterialPaty != null && hisMaterialPaty.Count > 0)
                    {
                        var Paty = hisMaterialPaty.FirstOrDefault(o => o.MATERIAL_ID == material.ID && castFilter.PRICE_PATIENT_TYPE_ID == o.PATIENT_TYPE_ID);
                        if (Paty != null && Paty.EXP_PRICE > 0)
                        {
                            item.SALE_PRICE = Paty.EXP_PRICE;
                            item.SALE_VAT_RATIO = Paty.EXP_VAT_RATIO;
                            material.IMP_PRICE = Paty.EXP_PRICE;
                            material.IMP_VAT_RATIO = Paty.EXP_VAT_RATIO;
                        }
                    }
                }
                //giá báo cáo theo lựa chọn
                if (castFilter.INPUT_DATA_ID_PRICE_TYPE == 1)//lựa chọn giá bán trước vat
                {
                    material.IMP_PRICE = item.SALE_PRICE ?? 0;
                    material.IMP_VAT_RATIO = 0;
                }
                else if (castFilter.INPUT_DATA_ID_PRICE_TYPE == 2)//lựa chọn giá bán sau vat
                {

                    material.IMP_PRICE = (item.SALE_PRICE ?? 0) * (1 + item.SALE_VAT_RATIO);
                    material.IMP_VAT_RATIO = 0;
                }
                else if (castFilter.INPUT_DATA_ID_PRICE_TYPE == 3)//lựa chọn giá nhập trước vat
                {
                    material.IMP_PRICE = material.IMP_PRICE;
                    material.IMP_VAT_RATIO = 0;
                }
                else if (castFilter.INPUT_DATA_ID_PRICE_TYPE == 4)//lựa chọn giá nhập sau vat
                {

                    material.IMP_PRICE = material.IMP_PRICE * (1 + material.IMP_VAT_RATIO);
                    material.IMP_VAT_RATIO = 0;
                }

                item.BID_GROUP_CODE = material.TDL_BID_GROUP_CODE;
                item.BID_NUM_ORDER = material.TDL_BID_NUM_ORDER;
                item.TDL_BID_NUMBER = material.TDL_BID_NUMBER;
                item.PACKAGE_NUMBER = material.PACKAGE_NUMBER;
                item.IMP_VAT = material.IMP_VAT_RATIO;
                item.VIR_IMP_PRICE = material.IMP_PRICE * (1 + material.IMP_VAT_RATIO);
                item.PRICE = material.IMP_PRICE;
                item.PRICE_AFTER_VAT = Math.Round(material.IMP_PRICE * (1 + material.IMP_VAT_RATIO), 0, MidpointRounding.AwayFromZero);
                item.TOTAL_PRICE = item.AMOUNT * material.IMP_PRICE * (1 + material.IMP_VAT_RATIO);
                item.IMP_TOTAL_PRICE = item.IMP_AMOUNT * material.IMP_PRICE * (1 + material.IMP_VAT_RATIO);
                item.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(material.EXPIRED_DATE ?? 0);
                var bid = (this.listHisBid ?? new List<HIS_BID>()).FirstOrDefault(o => o.ID == material.BID_ID);
                if (bid != null)
                {
                    item.BID_NUMBER = bid != null ? bid.BID_NUMBER : (material != null ? material.TDL_BID_NUMBER : "");
                    //item.BID_GROUP_CODE = bid != null ? bid.GROUP_CODE : (material != null ? material.TDL_BID_GROUP_CODE : "");
                    item.BID_NAME = material != null ? material.TDL_BID_PACKAGE_CODE : "";
                    item.BID_NUM_ORDER = material != null ? material.TDL_BID_NUM_ORDER : "";
                    item.BID_YEAR = material != null ? material.TDL_BID_YEAR : "";
                }
                if (materialType != null)
                {
                    item.MATERIAL_TYPE_CODE = materialType.MATERIAL_TYPE_CODE;
                    item.MATERIAL_TYPE_NAME = materialType.MATERIAL_TYPE_NAME;
                    item.SERVICE_UNIT_NAME = materialType.SERVICE_UNIT_NAME;
                    item.CONCENTRA = materialType.CONCENTRA;
                    item.MANUFACTURER_NAME = materialType.MANUFACTURER_NAME;
                    item.PACKING_TYPE_NAME = materialType.PACKING_TYPE_NAME;
                    item.NATIONAL_NAME = materialType.NATIONAL_NAME;
                    item.TYPE = materialType.IS_CHEMICAL_SUBSTANCE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE ? "HOACHAT" : "VATTU";
                    item.RECORDING_TRANSACTION = materialType.RECORDING_TRANSACTION;
                    //item.HEIN_SERVICE_CODE = materialType.HEIN_SERVICE_BHYT_CODE;
                    //item.HEIN_SERVICE_NAME = materialType.HEIN_SERVICE_BHYT_NAME;
                }

                //if (suppliers != null)
                //{
                //    var supplier = suppliers.FirstOrDefault(o => o.ID == material.SUPPLIER_ID);
                //    if (supplier != null)
                //    {
                //        item.SUPPLIER_CODE = supplier.SUPPLIER_CODE;
                //        item.SUPPLIER_NAME = supplier.SUPPLIER_NAME;
                //        item.SUPPLIER_ID = supplier.ID;
                //    }
                //}
            }

        }

        private void InfoBloodToRdo(Mrs00496RDO item, V_HIS_BLOOD_TYPE bloodType, V_HIS_BLOOD blood)
        {
            if (blood != null)
            {
                item.PACKAGE_NUMBER = blood.PACKAGE_NUMBER;
                item.IMP_VAT = blood.IMP_VAT_RATIO;
                item.VIR_IMP_PRICE = blood.IMP_PRICE * (1 + blood.IMP_VAT_RATIO);
                item.PRICE = blood.IMP_PRICE;
                item.PRICE_AFTER_VAT = Math.Round(blood.IMP_PRICE * (1 + blood.IMP_VAT_RATIO), 0, MidpointRounding.AwayFromZero);
                item.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(blood.EXPIRED_DATE ?? 0);
                item.TOTAL_PRICE = item.AMOUNT * blood.IMP_PRICE * (1 + blood.IMP_VAT_RATIO);
                item.IMP_TOTAL_PRICE = item.IMP_AMOUNT * blood.IMP_PRICE * (1 + blood.IMP_VAT_RATIO);
                var bid = (this.listHisBid ?? new List<HIS_BID>()).FirstOrDefault(o => o.ID == blood.BID_ID);
                if (bid != null)
                {
                    item.BID_NUMBER = bid.BID_NUMBER;
                    item.BID_YEAR = bid.BID_YEAR;
                }

                if (bloodType != null)
                {
                    item.BLOOD_TYPE_CODE = bloodType.BLOOD_TYPE_CODE;
                    item.BLOOD_TYPE_NAME = bloodType.BLOOD_TYPE_NAME;
                    item.SERVICE_UNIT_NAME = bloodType.SERVICE_UNIT_NAME;
                    item.PACKING_TYPE_NAME = bloodType.PACKING_TYPE_NAME;
                }

            }

        }

        private void AddInfoGroup(List<Mrs00496RDO> ListRdo)
        {
            foreach (var item in ListRdo)
            {
                if (item.TYPE == "THUOC")
                {
                    var medicineType = dicMedicineType.Values.FirstOrDefault(o => o.ID == item.MEDICINE_TYPE_ID);
                    if (medicineType != null && medicineType.MEDICINE_LINE_ID != null)
                    {
                        item.MEDICINE_LINE_ID = medicineType.MEDICINE_LINE_ID;
                        item.MEDICINE_LINE_CODE = medicineType.MEDICINE_LINE_CODE;
                        item.MEDICINE_LINE_NAME = medicineType.MEDICINE_LINE_NAME;
                        item.LINE_NUM_ORDER = medicineType.MEDICINE_LINE_NUM_ORDER ?? 1000000;
                    }
                    else
                    {
                        item.MEDICINE_LINE_ID = 0;
                        item.MEDICINE_LINE_CODE = "DTK";
                        item.MEDICINE_LINE_NAME = "Dòng thuốc khác";
                        item.LINE_NUM_ORDER = 1000000;
                    }

                    if (medicineType != null && medicineType.MEDICINE_GROUP_ID != null)
                    {
                        item.MEDICINE_GROUP_ID = medicineType.MEDICINE_GROUP_ID;
                        item.MEDICINE_GROUP_CODE = medicineType.MEDICINE_GROUP_CODE;
                        item.MEDICINE_GROUP_NAME = medicineType.MEDICINE_GROUP_NAME;
                    }
                    else
                    {
                        item.MEDICINE_GROUP_ID = 0;
                        item.MEDICINE_GROUP_CODE = "NTK";
                        item.MEDICINE_GROUP_NAME = "Nhóm thuốc khác";
                    }

                    if (medicineType != null && medicineType.PARENT_ID != null)
                    {
                        var parentMedicineType = dicMedicineType.Values.FirstOrDefault(o => o.ID == medicineType.PARENT_ID);
                        if (parentMedicineType != null)
                        {
                            item.PARENT_MEDICINE_TYPE_ID = parentMedicineType.ID;
                            item.PARENT_MEDICINE_TYPE_CODE = parentMedicineType.MEDICINE_TYPE_CODE;
                            item.PARENT_MEDICINE_TYPE_NAME = parentMedicineType.MEDICINE_TYPE_NAME;
                            item.PARENT_NUM_ORDER = parentMedicineType.NUM_ORDER ?? 1000000;
                        }
                        else
                        {
                            item.PARENT_MEDICINE_TYPE_ID = 0;
                            item.PARENT_MEDICINE_TYPE_CODE = "NTK";
                            item.PARENT_MEDICINE_TYPE_NAME = "Nhóm thuốc khác";
                            item.PARENT_NUM_ORDER = 1000000;
                        }
                    }
                    else
                    {
                        item.PARENT_MEDICINE_TYPE_ID = 0;
                        item.PARENT_MEDICINE_TYPE_CODE = "NTK";
                        item.PARENT_MEDICINE_TYPE_NAME = "Nhóm thuốc khác";
                        item.PARENT_NUM_ORDER = 1000000;
                    }

                    if (DicMediStockMety.ContainsKey(item.MEDICINE_TYPE_ID))
                    {
                        item.ALERT_MAX_IN_STOCK = DicMediStockMety[item.MEDICINE_TYPE_ID].ALERT_MAX_IN_STOCK;
                    }
                }

                if (item.TYPE == "VATTU")
                {
                    item.MEDICINE_LINE_CODE = "DVT";
                    item.MEDICINE_LINE_NAME = "Vật tư";
                    item.MEDICINE_GROUP_CODE = "DVT";
                    item.MEDICINE_GROUP_NAME = "Vật tư";
                    item.PARENT_MEDICINE_TYPE_CODE = "DVT";
                    item.PARENT_MEDICINE_TYPE_NAME = "Vật tư";
                    item.LINE_NUM_ORDER = 2000000;
                    item.PARENT_NUM_ORDER = 2000000;

                    var materialType = dicMaterialType.Values.FirstOrDefault(o => o.ID == item.MATERIAL_TYPE_ID);
                   
                    if (materialType != null && materialType.PARENT_ID != null)
                    {
                        var parentMaterialType = dicMaterialType.Values.FirstOrDefault(o => o.ID == materialType.PARENT_ID);
                        if (parentMaterialType != null)
                        {
                            item.PARENT_MEDICINE_TYPE_ID = parentMaterialType.ID;
                            item.PARENT_MEDICINE_TYPE_CODE = parentMaterialType.MATERIAL_TYPE_CODE;
                            item.PARENT_MEDICINE_TYPE_NAME = parentMaterialType.MATERIAL_TYPE_NAME;
                            item.PARENT_NUM_ORDER = parentMaterialType.NUM_ORDER ?? 2000000;
                        }
                    }
                   
                    if (DicMediStockMaty.ContainsKey(item.MATERIAL_TYPE_ID))
                    {
                        item.ALERT_MAX_IN_STOCK = DicMediStockMaty[item.MATERIAL_TYPE_ID].ALERT_MAX_IN_STOCK;
                    }
                }

                if (item.TYPE == "HOACHAT")
                {
                    item.MEDICINE_LINE_CODE = "DHC";
                    item.MEDICINE_LINE_NAME = "Hóa chất";
                    item.MEDICINE_GROUP_CODE = "DHC";
                    item.MEDICINE_GROUP_NAME = "Hóa chất";
                    item.PARENT_MEDICINE_TYPE_CODE = "DHC";
                    item.PARENT_MEDICINE_TYPE_NAME = "Hóa chất";
                    item.LINE_NUM_ORDER = 3000000;
                    item.PARENT_NUM_ORDER = 3000000;

                    var materialType = dicMaterialType.Values.FirstOrDefault(o => o.ID == item.MATERIAL_TYPE_ID);

                    if (materialType != null && materialType.PARENT_ID != null)
                    {
                        var parentMaterialType = dicMaterialType.Values.FirstOrDefault(o => o.ID == materialType.PARENT_ID);
                        if (parentMaterialType != null)
                        {
                            item.PARENT_MEDICINE_TYPE_ID = parentMaterialType.ID;
                            item.PARENT_MEDICINE_TYPE_CODE = parentMaterialType.MATERIAL_TYPE_CODE;
                            item.PARENT_MEDICINE_TYPE_NAME = parentMaterialType.MATERIAL_TYPE_NAME;
                            item.PARENT_NUM_ORDER = parentMaterialType.NUM_ORDER ?? 3000000;
                        }
                    }
                    if (DicMediStockMaty.ContainsKey(item.MATERIAL_TYPE_ID))
                    {
                        item.ALERT_MAX_IN_STOCK = DicMediStockMaty[item.MATERIAL_TYPE_ID].ALERT_MAX_IN_STOCK;
                    }
                }

                if (item.TYPE == "MAU")
                {
                    item.MEDICINE_LINE_CODE = "DM";
                    item.MEDICINE_LINE_NAME = "Máu";
                    item.MEDICINE_GROUP_CODE = "DM";
                    item.MEDICINE_GROUP_NAME = "Máu";
                    item.PARENT_MEDICINE_TYPE_CODE = "DM";
                    item.PARENT_MEDICINE_TYPE_NAME = "Máu";
                    item.LINE_NUM_ORDER = 4000000;
                    item.PARENT_NUM_ORDER = 4000000;

                    var bloodType = dicBloodType.Values.FirstOrDefault(o => o.ID == item.BLOOD_TYPE_ID);

                    if (bloodType != null && bloodType.PARENT_ID != null)
                    {
                        var parentBloodType = dicBloodType.Values.FirstOrDefault(o => o.ID == bloodType.PARENT_ID);
                        if (parentBloodType != null)
                        {
                            item.PARENT_MEDICINE_TYPE_ID = parentBloodType.ID;
                            item.PARENT_MEDICINE_TYPE_CODE = parentBloodType.BLOOD_TYPE_CODE;
                            item.PARENT_MEDICINE_TYPE_NAME = parentBloodType.BLOOD_TYPE_NAME;
                            item.PARENT_NUM_ORDER = parentBloodType.NUM_ORDER ?? 3000000;
                        }
                    }
                    if (DicMediStockBlty.ContainsKey(item.BLOOD_TYPE_ID))
                    {
                        item.ALERT_MAX_IN_STOCK = DicMediStockBlty[item.BLOOD_TYPE_ID].ALERT_MAX_IN_STOCK;
                    }
                }
            }
        }

        private void MergeByFilter()
        {
            try
            {
                ListRdo = dicRdo.Values.ToList();
                foreach (var item in ListRdo)
                {
                    if (castFilter.IS_MERGER_EXPIRED_DATE == true)
                    {
                        item.EXPIRED_DATE_STR = "";
                    }

                    if (castFilter.IS_MERGER_PRICE == true)
                    {
                        item.PRICE_AFTER_VAT = 0;
                    }

                    if (castFilter.IS_MERGER_PACKAGE_NUMBER == true)
                    {
                        item.PACKAGE_NUMBER = "";
                    }

                    if (castFilter.IS_MERGER_BID_NUMBER == true)
                    {
                        item.BID_NUMBER = "";
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void GroupByMediMateTypePrice()
        {
            string errorField = "";
            try
            {
                var group = ListRdo.GroupBy(o => new { o.MEDICINE_TYPE_ID, o.MATERIAL_TYPE_ID, o.TYPE }).ToList();
                ListRdo.Clear();

                Decimal sum = 0;
                Mrs00496RDO rdo;
                List<Mrs00496RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00496RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00496RDO();
                    listSub = item.ToList<Mrs00496RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("AMOUNT") || field.Name.Contains("TOTAL_PRICE"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }

                    if (!hide) ListRdo.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }

        private void GroupByMaterialTypeIdMedicineTypeIdPriceTypeAndExpiredDate()
        {
            string errorField = "";
            try
            {
                var group = ListRdo.GroupBy(o => new { o.MEDICINE_TYPE_ID, o.BID_NUMBER, o.PACKAGE_NUMBER, o.MATERIAL_TYPE_ID, o.PRICE_AFTER_VAT, o.TYPE, o.EXPIRED_DATE_STR }).ToList();
                ListRdo.Clear();

                Decimal sum = 0;
                Mrs00496RDO rdo;
                List<Mrs00496RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00496RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00496RDO();
                    listSub = item.ToList<Mrs00496RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("AMOUNT") || field.Name.Contains("TOTAL_PRICE"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }

                    if (!hide) ListRdo.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }

        private Mrs00496RDO IsMeaningful(List<Mrs00496RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00496RDO();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            var mediStock = new HisMediStockManager(paramGet).Get(new HisMediStockFilterQuery());
            if (castFilter.MEDI_STOCK_ID != null) mediStock = mediStock.Where(o => o.ID == castFilter.MEDI_STOCK_ID).ToList();
            if (IsNotNull(mediStock))
            {
                MEDI_STOCK_CODE = string.Join(", ", mediStock.Select(o => o.MEDI_STOCK_CODE).ToList());
                MEDI_STOCK_NAME = string.Join(", ", mediStock.Select(o => o.MEDI_STOCK_NAME).ToList());
            }

            dicSingleTag.Add("MEDI_STOCK_CODE_AND_NAME", MEDI_STOCK_CODE + " - " + MEDI_STOCK_NAME);
            dicSingleTag.Add("TIME", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? castFilter.TIME ?? (Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(System.DateTime.Now) ?? 0)));

            decimal totalPrice = 0;
            if (IsNotNullOrEmpty(listRdoMedicine))
            {
                totalPrice += listRdoMedicine.Sum(s => s.TOTAL_PRICE);
            }

            if (IsNotNullOrEmpty(listRdoMaterial))
            {
                totalPrice += listRdoMaterial.Sum(s => s.TOTAL_PRICE);
            }

            if (IsNotNullOrEmpty(listRdoChemistry))
            {
                totalPrice += listRdoChemistry.Sum(s => s.TOTAL_PRICE);
            }

            if (IsNotNullOrEmpty(listRdoBlood))
            {
                totalPrice += listRdoBlood.Sum(s => s.TOTAL_PRICE);
            }

            List<RoleUserADO> listRole = new List<RoleUserADO>();
            if (IsNotNullOrEmpty(castFilter.EXECUTE_ROLE_GROUP))
            {
                listRole.AddRange(castFilter.EXECUTE_ROLE_GROUP);
            }

            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO ?? 0));

            dicSingleTag.Add("TOTAL_PRICE", totalPrice);

            objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(o => o.MEDICINE_TYPE_NAME).ThenBy(p => p.MATERIAL_TYPE_NAME).ToList());
            listRdoMedicine = listRdoMedicine.OrderBy(t => t.MEDICINE_TYPE_NAME).ToList();
            listRdoMaterial = listRdoMaterial.OrderBy(t => t.MATERIAL_TYPE_NAME).ToList();
            listRdoChemistry = listRdoChemistry.OrderBy(t => t.MATERIAL_TYPE_NAME).ToList();
            listRdoBlood = listRdoBlood.OrderBy(t => t.BLOOD_TYPE_NAME).ToList();
            CreateParent(objectTag, store, "listRdoMedicine", listRdoMedicine);
            CreateParent(objectTag, store, "listRdoMaterial", listRdoMaterial);
            CreateParent(objectTag, store, "listRdoChemistry", listRdoChemistry);
            CreateParent(objectTag, store, "listRdoBlood", listRdoBlood);

            objectTag.AddObjectData(store, "UserRole", listRole);
        }

        private void CreateParent(ProcessObjectTag objectTag, Store store, string tableName, List<Mrs00496RDO> table)
        {
            objectTag.AddObjectData(store, tableName, table);
            objectTag.AddObjectData(store, "GrandParent" + tableName, table.OrderBy(q => q.PARENT_MEDICINE_TYPE_NAME).GroupBy(o => o.PARENT_MEDICINE_TYPE_ID).Select(p => p.First()).OrderBy(o => o.PARENT_NUM_ORDER).ToList());
            objectTag.AddObjectData(store, "Parent" + tableName, table.GroupBy(o => new { o.MEDICINE_LINE_ID, o.PARENT_MEDICINE_TYPE_ID }).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "GrandParent" + tableName, "Parent" + tableName, "PARENT_MEDICINE_TYPE_ID", "PARENT_MEDICINE_TYPE_ID");
            objectTag.AddRelationship(store, "Parent" + tableName, tableName, new string[] { "MEDICINE_LINE_ID", "PARENT_MEDICINE_TYPE_ID" }, new string[] { "MEDICINE_LINE_ID", "PARENT_MEDICINE_TYPE_ID" });
        }
    }
}
