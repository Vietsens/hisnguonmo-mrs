using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMest;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Repository;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMestPeriodMate;
using MOS.MANAGER.HisMestPeriodMedi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType;
using MOS.MANAGER.HisMedicinePaty;
using MOS.MANAGER.HisMaterialPaty;
using Inventec.Common.Logging;
using MRS.MANAGER.Base;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisImpSource;
using MOS.MANAGER.HisImpMestBlood;
using MOS.MANAGER.HisExpMestBlood;
using MOS.MANAGER.HisBlood;

namespace MRS.Processor.Mrs00133
{
    class Mrs00133Processor : AbstractProcessor
    {
        Mrs00133Filter castFilter = null;

        List<Mrs00133RDO> ListRdo = new List<Mrs00133RDO>();

        List<Mrs00133RDO> listRdoMedicine = new List<Mrs00133RDO>();
        List<Mrs00133RDO> listRdoMaterial = new List<Mrs00133RDO>();
        List<Mrs00133RDO> listRdoBlood = new List<Mrs00133RDO>();

        List<V_HIS_MEDI_STOCK> ListMediStock = new List<V_HIS_MEDI_STOCK>();
        V_HIS_MEDI_STOCK CurrentMediStock = new V_HIS_MEDI_STOCK();
        List<HIS_MEDICINE_PATY> listMedicinePaty = new List<HIS_MEDICINE_PATY>();
        List<HIS_MATERIAL_PATY> listMaterialPaty = new List<HIS_MATERIAL_PATY>();
        
        Dictionary<string, long> dicExpMestType = new Dictionary<string, long>();
        Dictionary<string, long> dicImpMestType = new Dictionary<string, long>();
        List<V_HIS_MEDICINE_TYPE> hisMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MATERIAL_TYPE> hisMaterialType = new List<V_HIS_MATERIAL_TYPE>();
        List<V_HIS_BLOOD_TYPE> hisBloodType = new List<V_HIS_BLOOD_TYPE>();
        List<HIS_MEDICINE> Medicines = new List<HIS_MEDICINE>();
        List<HIS_MATERIAL> Materials = new List<HIS_MATERIAL>();
        List<HIS_BLOOD> Bloods = new List<HIS_BLOOD>();
        List<HIS_IMP_SOURCE> ImpSources = new List<HIS_IMP_SOURCE>();

        int Thuoc = 10;
        int VatTu = 20;
        int HoaChat = 30;
        int Mau = 40;

        int DichTruyen = 11;
        int HuongThan = 12;
        int GayNghien = 13;
        int ThuocThuong = 14;
        List<long> listChmsExpMestId = new List<long>() { 
        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK, 
        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS, 
        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL
        };
        List<long> listChmsImpMestId = new List<long>() { 
        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK, 
        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS, 
        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BL
        };

        public Mrs00133Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00133Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                castFilter = ((Mrs00133Filter)reportFilter);
                //DS kho
                HisMediStockViewFilterQuery mediStockFilter = new HisMediStockViewFilterQuery();
                mediStockFilter.IDs = castFilter.MEDI_STOCK_IDs;
                ListMediStock = new HisMediStockManager(paramGet).GetView(mediStockFilter);
                result = true;
                //Tao loai nhap xuat
                RDOImpExpMestTypeContext.Define(ref dicImpMestType, ref dicExpMestType);
                listMedicinePaty = new HisMedicinePatyManager().Get(new HisMedicinePatyFilterQuery() { IS_ACTIVE = 1, PATIENT_TYPE_ID = castFilter.PATIENT_TYPE_ID });
                listMaterialPaty = new HisMaterialPatyManager().Get(new HisMaterialPatyFilterQuery() { IS_ACTIVE = 1, PATIENT_TYPE_ID = castFilter.PATIENT_TYPE_ID });

                hisMedicineType = new HisMedicineTypeManager().GetView(new HisMedicineTypeViewFilterQuery() { IS_ACTIVE = 1 });
                hisMaterialType = new HisMaterialTypeManager().GetView(new HisMaterialTypeViewFilterQuery() { IS_ACTIVE = 1 });
                ImpSources = new HisImpSourceManager().Get(new HisImpSourceFilterQuery() { IS_ACTIVE = 1 });
               
              
               
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
                if (IsNotNullOrEmpty(ListMediStock))
                {
                    CommonParam paramGet = new CommonParam();
                    foreach (var medistock in ListMediStock)
                    {
                        CurrentMediStock = medistock;
                        HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                        impMediFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                        impMediFilter.IMP_TIME_TO = castFilter.TIME_TO;
                        impMediFilter.MEDI_STOCK_ID = medistock.ID;
                        impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                        List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new HisImpMestMedicineManager().GetView(impMediFilter);

                        HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                        expMediFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                        expMediFilter.EXP_TIME_TO = castFilter.TIME_TO;
                        expMediFilter.MEDI_STOCK_ID = medistock.ID;
                        expMediFilter.IS_EXPORT = true;
                        //expMediFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                        List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new HisExpMestMedicineManager().GetView(expMediFilter);
                        HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                        impMateFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                        impMateFilter.IMP_TIME_TO = castFilter.TIME_TO;
                        impMateFilter.MEDI_STOCK_ID = medistock.ID; ////
                        impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                        List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new HisImpMestMaterialManager().GetView(impMateFilter);

                        HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                        expMateFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                        expMateFilter.EXP_TIME_TO = castFilter.TIME_TO;
                        expMateFilter.MEDI_STOCK_ID = medistock.ID;
                        expMateFilter.IS_EXPORT = true;
                        //expMateFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                        List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new HisExpMestMaterialManager().GetView(expMateFilter);

                        HisImpMestBloodViewFilterQuery impBloodFilter = new HisImpMestBloodViewFilterQuery();
                        impBloodFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                        impBloodFilter.IMP_TIME_TO = castFilter.TIME_TO;
                        impBloodFilter.MEDI_STOCK_ID = medistock.ID;
                        impBloodFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                        List<V_HIS_IMP_MEST_BLOOD> hisImpMestBlood = new HisImpMestBloodManager().GetView(impBloodFilter);

                        HisExpMestBloodViewFilterQuery expBloodFilter = new HisExpMestBloodViewFilterQuery();
                        expBloodFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                        expBloodFilter.EXP_TIME_TO = castFilter.TIME_TO;
                        expBloodFilter.MEDI_STOCK_ID = medistock.ID;
                        expBloodFilter.IS_EXPORT = true;
                        //expMateFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                        List<V_HIS_EXP_MEST_BLOOD> hisExpMestBlood = new HisExpMestBloodManager().GetView(expBloodFilter);
                        if (!paramGet.HasException)
                        {
                            if (castFilter.IS_MERGE_STOCK_THROW_CHMS == true)
                            {
                                hisImpMestMedicine = hisImpMestMedicine.Where(o => !this.listChmsImpMestId.Contains(o.IMP_MEST_TYPE_ID)).ToList();
                                hisImpMestMaterial = hisImpMestMaterial.Where(o => !this.listChmsImpMestId.Contains(o.IMP_MEST_TYPE_ID)).ToList();
                                hisImpMestBlood = hisImpMestBlood.Where(o => !this.listChmsImpMestId.Contains(o.IMP_MEST_ID)).ToList();
                                hisExpMestMedicine = hisExpMestMedicine.Where(o => !this.listChmsExpMestId.Contains(o.EXP_MEST_TYPE_ID)).ToList();
                                hisExpMestMaterial = hisExpMestMaterial.Where(o => !this.listChmsExpMestId.Contains(o.EXP_MEST_TYPE_ID)).ToList();
                                hisExpMestBlood = hisExpMestBlood.Where(o => !this.listChmsExpMestId.Contains(o.EXP_MEST_TYPE_ID)).ToList();
                            }
                            LogSystem.Info("id: " + CurrentMediStock.ID);
                            ProcessAmountMedicine(hisImpMestMedicine, hisExpMestMedicine, medistock);
                            ProcessAmountMaterial(hisImpMestMaterial, hisExpMestMaterial, medistock);
                            ProcessAmountBlood(hisImpMestBlood, hisExpMestBlood, medistock);
                            ProcessGetPeriod(paramGet, medistock);

                            listRdoMedicine = groupById(listRdoMedicine);
                            GetMediMate(listRdoMedicine);
                            listRdoMedicine = groupByServiceAndPriceAndSupplier(listRdoMedicine);

                            listRdoMaterial = groupById(listRdoMaterial);
                            GetMediMate(listRdoMaterial);
                            listRdoMaterial = groupByServiceAndPriceAndSupplier(listRdoMaterial);

                            listRdoBlood = groupById(listRdoBlood);
                            GetMediMate(listRdoBlood);
                            listRdoBlood = groupByServiceAndPriceAndSupplier(listRdoBlood);

                            CountEndAmount(listRdoMedicine);
                            CountEndAmount(listRdoMaterial);
                            CountEndAmount(listRdoBlood);

                            AddMedicinePrice(listRdoMedicine);
                            AddMaterialPrice(listRdoMaterial);
                            AddBloodPrice(listRdoBlood);

                            ListRdo.AddRange(listRdoMedicine);
                            ListRdo.AddRange(listRdoMaterial);
                            ListRdo.AddRange(listRdoBlood);
                            AddInfoMedicine(ListRdo);
                        }
                        else
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00133.");
                        }
                    }

                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00133.");
                    }
                }

                if (castFilter.NATIONAL_NAME.HasValue)
                {
                    if (castFilter.NATIONAL_NAME.Value == 0)
                    {
                        ListRdo = ListRdo.Where(x => x.NATIONAL_NAME != null).ToList();
                        ListRdo = ListRdo.Where(x => x.NATIONAL_NAME.Contains("Việt Nam")).ToList();
                    }
                    else if (castFilter.NATIONAL_NAME.Value == 1)
                    {
                        ListRdo = ListRdo.Where(x => x.NATIONAL_NAME != null).ToList();
                        ListRdo = ListRdo.Where(x => x.NATIONAL_NAME != "Việt Nam").ToList();
                    }
                }
                else
                {
                   ListRdo=  ListRdo.ToList();
                }

                AddInfoGroup(ListRdo);
                if (castFilter.MEDICINE_GROUP_IDs!=null)
                {
                    ListRdo = ListRdo.Where(x => x.MEDICINE_GROUP_ID != null).ToList();
                    ListRdo = ListRdo.Where(x => castFilter.MEDICINE_GROUP_IDs.Contains((long)x.MEDICINE_GROUP_ID)).ToList();
                }
                if (castFilter.IS_MERGE != null && castFilter.IS_MERGE != false)
                {
                    #region gop thuoc, vat tu
                    ListRdo = groupByServiceAndPrice(ListRdo);
                    #endregion
                }
                else if (castFilter.IS_MERGE_CODE.HasValue && castFilter.IS_MERGE_CODE.Value)
                {
                    #region gop thuoc, vat tu
                    ListRdo = groupByCode(ListRdo);
                    #endregion
                }

                //MergeMediStock();
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return false;
            }
            return result;
        }

        private void AddInfoMedicine(List<Mrs00133RDO> ListRdo)
        {
            foreach (var item in ListRdo)
            {
                if (item.SERVICE_TYPE_ID == Thuoc)
                {
                    var medicineType = hisMedicineType.FirstOrDefault(o => o.SERVICE_ID == item.SERVICE_ID);
                    if (medicineType != null)
                    {
                        item.REGISTER_NUMBER = medicineType.REGISTER_NUMBER;//Số đăng ký
                        item.HEIN_SERVICE_BHYT_CODE = medicineType.HEIN_SERVICE_BHYT_CODE;//Mã BHYT
                        item.HEIN_SERVICE_BHYT_NAME = medicineType.HEIN_SERVICE_BHYT_NAME;//Tên BHYT
                        item.ATC_CODES = medicineType.ATC_CODES;//Tên BHYT
                    }

                    var medicine = Medicines.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                    if (medicine != null)
                    {
                        item.PACKAGE_NUMBER = medicine.PACKAGE_NUMBER;//Số lô
                        item.BID_GROUP_CODE = medicine.TDL_BID_GROUP_CODE;//Mã nhóm thầu
                        item.BID_NUM_ORDER = medicine.TDL_BID_NUM_ORDER;//Số thứ tự thầu
                        item.BID_NUMBER = medicine.TDL_BID_NUMBER;//Mã nhóm thầu
                        item.BID_YEAR = medicine.TDL_BID_YEAR;//năm thầu
                        item.BID_PACKAGE_CODE = medicine.TDL_BID_PACKAGE_CODE;//năm thầu
                        item.NATIONAL_NAME = medicine.NATIONAL_NAME;

                        var source = ImpSources.FirstOrDefault(o => o.ID == medicine.IMP_SOURCE_ID);
                        if (source != null)
                        {
                            item.IMP_SOURCE_NAME = source.IMP_SOURCE_NAME;//Mã nhóm thầu
                        }
                    }
                }
                else if (item.SERVICE_TYPE_ID == VatTu)
                {

                    var materialType = hisMaterialType.FirstOrDefault(o => o.SERVICE_ID == item.SERVICE_ID);
                    if (materialType != null)
                    {
                        item.HEIN_SERVICE_BHYT_CODE = materialType.HEIN_SERVICE_BHYT_CODE;//Mã BHYT
                        item.HEIN_SERVICE_BHYT_NAME = materialType.HEIN_SERVICE_BHYT_NAME;//Tên BHYT
                        if (materialType.IS_CHEMICAL_SUBSTANCE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                        {
                            item.SERVICE_TYPE_ID = HoaChat;
                            item.SERVICE_TYPE_NAME = "HÓA CHẤT";
                        }
                    }
                    var material = Materials.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                    if (material != null)
                    {
                        item.PACKAGE_NUMBER = material.PACKAGE_NUMBER;//Số lô
                        item.BID_GROUP_CODE = material.TDL_BID_GROUP_CODE;//Mã nhóm thầu
                        item.BID_NUM_ORDER = material.TDL_BID_NUM_ORDER;//Số thứ tự thầu
                        item.BID_NUMBER = material.TDL_BID_NUMBER;//Mã nhóm thầu
                        item.BID_YEAR = material.TDL_BID_YEAR;//năm thầu
                        item.BID_PACKAGE_CODE = material.TDL_BID_PACKAGE_CODE;//năm thầu
                    }
                }
                else if (item.SERVICE_TYPE_ID == Mau)
                {

                    var bloodType = hisBloodType.FirstOrDefault(o => o.SERVICE_ID == item.SERVICE_ID);
                    if (hisBloodType != null)
                    {
                        //item.HEIN_SERVICE_BHYT_CODE = hisBloodType.HEIN_SERVICE_BHYT_CODE;//Mã BHYT
                        //item.HEIN_SERVICE_BHYT_NAME = hisBloodType.HEIN_SERVICE_BHYT_NAME;//Tên BHYT
                        
                    }
                    var blood = Bloods.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                    if (blood != null)
                    {
                        item.PACKAGE_NUMBER = blood.PACKAGE_NUMBER;//Số lô
                        //item.BID_GROUP_CODE = blood.TDL_BID_GROUP_CODE;//Mã nhóm thầu
                        //item.BID_NUM_ORDER = blood.TDL_BID_NUM_ORDER;//Số thứ tự thầu
                        //item.BID_NUMBER = blood.TDL_BID_NUMBER;//Mã nhóm thầu
                        //item.BID_YEAR = blood.TDL_BID_YEAR;//năm thầu
                        //item.BID_PACKAGE_CODE = blood.TDL_BID_PACKAGE_CODE;//năm thầu
                    }
                }
            }
        }

        private void GetMediMate(List<Mrs00133RDO> list)
        {
            try
            {
                List<long> medicineIds = new List<long>();
                List<long> materialIds = new List<long>();
                List<long> bloodIds = new List<long>();
                if (list != null)
                {
                    medicineIds.AddRange(list.Where(p => p.SERVICE_TYPE_ID == Thuoc).Select(o => o.MEDI_MATE_ID).ToList());
                    medicineIds = medicineIds.Distinct().ToList();
                    materialIds.AddRange(list.Where(p => p.SERVICE_TYPE_ID == VatTu).Select(o => o.MEDI_MATE_ID).ToList());
                    materialIds = materialIds.Distinct().ToList();
                    bloodIds.AddRange(list.Where(p => p.SERVICE_TYPE_ID == Mau).Select(o => o.MEDI_MATE_ID).ToList());
                    bloodIds = bloodIds.Distinct().ToList();
                    if (medicineIds.Count > 0)
                    {
                        var skip = 0;
                        while (medicineIds.Count - skip > 0)
                        {
                            var limit = medicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisMedicineFilterQuery Medicinefilter = new HisMedicineFilterQuery();
                            Medicinefilter.IDs = limit;
                            var MedicineSub = new HisMedicineManager().Get(Medicinefilter);
                            Medicines.AddRange(MedicineSub);
                        }
                    }
                    if (materialIds.Count > 0)
                    {
                        var skip = 0;
                        while (materialIds.Count - skip > 0)
                        {
                            var limit = materialIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisMaterialFilterQuery Materialfilter = new HisMaterialFilterQuery();
                            Materialfilter.IDs = limit;
                            var MaterialSub = new HisMaterialManager().Get(Materialfilter);
                            Materials.AddRange(MaterialSub);
                        }
                    }
                    if (bloodIds.Count > 0)
                    {
                        var skip = 0;
                        while (bloodIds.Count - skip > 0)
                        {
                            var limit = bloodIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisBloodFilterQuery Bloodfilter = new HisBloodFilterQuery();
                            Bloodfilter.IDs = limit;
                            var BloodSub = new HisBloodManager().Get(Bloodfilter);
                            Bloods.AddRange(BloodSub);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

            }
        }

        private void MergeMediStock()
        {
            string errorField = "";
            try
            {
                if (!(castFilter.IS_MERGE_STOCK ?? false))
                {
                    return;
                }
                var group = ListRdo.GroupBy(o => new { o.MEDI_MATE_ID, o.SERVICE_TYPE_ID }).ToList();
                ListRdo.Clear();

                Decimal sum = 0;
                Mrs00133RDO rdo;
                List<Mrs00133RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00133RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00133RDO();
                    listSub = item.ToList<Mrs00133RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("_AMOUNT"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s))) ?? new Mrs00133RDO()));
                        }
                    }
                    rdo.MEDI_STOCK_ID = 9999999;
                    if (!hide) ListRdo.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }

        private void AddBloodPrice(List<Mrs00133RDO> listRdoBlood)
        {
            try
            {
                foreach (var item in listRdoBlood)
                {
                    item.EXP_VAT = (listMaterialPaty.FirstOrDefault(o => o.MATERIAL_ID == item.MEDI_MATE_ID) ?? new HIS_MATERIAL_PATY()).EXP_VAT_RATIO;
                    item.EXP_PRICE = (listMaterialPaty.FirstOrDefault(o => o.MATERIAL_ID == item.MEDI_MATE_ID) ?? new HIS_MATERIAL_PATY()).EXP_PRICE;

                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void AddMaterialPrice(List<Mrs00133RDO> listRdoMaterial)
        {
            try
            {
                foreach (var item in listRdoMaterial)
                {
                    item.EXP_VAT = (listMaterialPaty.FirstOrDefault(o => o.MATERIAL_ID == item.MEDI_MATE_ID) ?? new HIS_MATERIAL_PATY()).EXP_VAT_RATIO;
                    item.EXP_PRICE = (listMaterialPaty.FirstOrDefault(o => o.MATERIAL_ID == item.MEDI_MATE_ID) ?? new HIS_MATERIAL_PATY()).EXP_PRICE;
                    
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void AddMedicinePrice(List<Mrs00133RDO> listRdoMedicine)
        {
            try
            {
                foreach (var item in listRdoMedicine)
                {

                    item.EXP_VAT = (listMedicinePaty.FirstOrDefault(o => o.MEDICINE_ID == item.MEDI_MATE_ID) ?? new HIS_MEDICINE_PATY()).EXP_VAT_RATIO;
                    item.EXP_PRICE = (listMedicinePaty.FirstOrDefault(o => o.MEDICINE_ID == item.MEDI_MATE_ID) ?? new HIS_MEDICINE_PATY()).EXP_PRICE;
                    
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void CountEndAmount(List<Mrs00133RDO> listRdoMedicine)
        {
            try
            {
                foreach (var item in listRdoMedicine)
                {
                    decimal ImpAmount = 0;
                    foreach (var r in dicImpMestType.Keys)
                    {
                        
                        PropertyInfo p = typeof(Mrs00133RDO).GetProperty(string.Format(r));
                        ImpAmount += (decimal)p.GetValue(item);
                        LogSystem.Info("IMP_AMOUNT:" + (decimal)p.GetValue(item));
                        
                    }
                    decimal ExpAmount = 0;
                    foreach (var r in dicExpMestType.Keys)
                    {
                        PropertyInfo p = typeof(Mrs00133RDO).GetProperty(string.Format(r));
                        ExpAmount += (decimal)p.GetValue(item);
                        LogSystem.Info("EXP_AMOUNT:" + (decimal)p.GetValue(item));
                    }
                    item.END_AMOUNT = item.BEGIN_AMOUNT + ImpAmount - ExpAmount;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }

        }

        //Gop theo id
        private List<Mrs00133RDO> groupById(List<Mrs00133RDO> listRdoMedicine)
        {
            List<Mrs00133RDO> result = new List<Mrs00133RDO>();
            try
            {
                var group = listRdoMedicine.GroupBy(g => g.MEDI_MATE_ID).ToList();
                Decimal sum = 0;
                Mrs00133RDO rdo;
                List<Mrs00133RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00133RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00133RDO();
                    listSub = item.ToList<Mrs00133RDO>();
                    bool hide = true;
                    foreach (var field in pi)
                    {
                        if (field.Name.Contains("_AMOUNT"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s))) ?? new Mrs00133RDO()));
                        }
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<Mrs00133RDO>();
            }
            return result;
        }

        //Gop theo gia, theo dich vu, theo nha cung cap
        private List<Mrs00133RDO> groupByServiceAndPriceAndSupplier(List<Mrs00133RDO> listRdoMedicine)
        {
            List<Mrs00133RDO> result = new List<Mrs00133RDO>();
            try
            {
                var group = listRdoMedicine.GroupBy(g => new { g.SERVICE_ID, g.SUPPLIER_ID, g.IMP_PRICE }).ToList();
                Decimal sum = 0;
                Mrs00133RDO rdo;
                List<Mrs00133RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00133RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00133RDO();
                    listSub = item.ToList<Mrs00133RDO>();
                    bool hide = true;
                    foreach (var field in pi)
                    {
                        if (field.Name.Contains("_AMOUNT"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s))) ?? new Mrs00133RDO()));
                        }
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<Mrs00133RDO>();
            }
            return result;
        }

        //Gop theo gia, theo dich vu
        private List<Mrs00133RDO> groupByServiceAndPrice(List<Mrs00133RDO> listRdoMedicine)
        {
            List<Mrs00133RDO> result = new List<Mrs00133RDO>();
            try
            {
                var group = listRdoMedicine.GroupBy(g => new { g.MEDI_STOCK_ID, g.SERVICE_TYPE_ID, g.SERVICE_ID, g.IMP_PRICE }).ToList();
                Decimal sum = 0;
                Mrs00133RDO rdo;
                List<Mrs00133RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00133RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00133RDO();
                    listSub = item.ToList<Mrs00133RDO>();
                    bool hide = true;
                    foreach (var field in pi)
                    {
                        if (field.Name.Contains("_AMOUNT"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s))) ?? new Mrs00133RDO()));
                        }
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<Mrs00133RDO>();
            }
            return result;
        }

        //Gop theo ma dich vu
        private List<Mrs00133RDO> groupByCode(List<Mrs00133RDO> listRdoMedicine)
        {
            List<Mrs00133RDO> result = new List<Mrs00133RDO>();
            try
            {
                var group = listRdoMedicine.GroupBy(g => new { g.MEDI_STOCK_ID, g.SERVICE_TYPE_ID, g.SERVICE_ID }).ToList();
                Decimal sum = 0;
                Mrs00133RDO rdo;
                List<Mrs00133RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00133RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00133RDO();
                    listSub = item.ToList<Mrs00133RDO>();
                    bool hide = true;
                    foreach (var field in pi)
                    {
                        if (field.Name.Contains("_AMOUNT"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s))) ?? new Mrs00133RDO()));
                        }
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<Mrs00133RDO>();
            }
            return result;
        }

        private bool IsMeaningful(object p)
        {
            return (IsNotNull(p) && p.ToString() != "0" && p.ToString() != "");
        }

        //Tính số lượng nhập và xuất thuốc
        private void ProcessAmountMedicine(List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine, List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine, V_HIS_MEDI_STOCK medistock)
        {
            try
            {
                listRdoMedicine.Clear();
                ProcessImpAmountMedicine(hisImpMestMedicine, medistock);
                ProcessExpAmountMedicine(hisExpMestMedicine, medistock);
                listRdoMedicine = groupById(listRdoMedicine);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listRdoMedicine.Clear();
            }
        }

        private void ProcessImpAmountMedicine(List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine, V_HIS_MEDI_STOCK medistock)
        {
            try
            {
                if (IsNotNullOrEmpty(hisImpMestMedicine))
                {
                    var listImpMestMediSub = new List<V_HIS_IMP_MEST_MEDICINE>();
                    foreach (var item in dicImpMestType.Keys)
                    {
                        listImpMestMediSub = hisImpMestMedicine.Where(o => o.IMP_MEST_TYPE_ID == dicImpMestType[item]).ToList();
                        if (!IsNotNullOrEmpty(listImpMestMediSub)) continue;

                        ProcessImpAmountMedicineByImpMestType(listImpMestMediSub, item, medistock);
                    }

                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessExpAmountMedicine(List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine, V_HIS_MEDI_STOCK medistock)
        {
            try
            {
                if (IsNotNullOrEmpty(hisExpMestMedicine))
                {
                    //Inventec.Common.Logging.LogSystem.Info("1:" + hisExpMestMedicine.Where(o => o.MEDICINE_TYPE_CODE == "ACE001").Sum(p => p.AMOUNT));
                    var listExpMestMediSub = new List<V_HIS_EXP_MEST_MEDICINE>();
                    //Inventec.Common.Logging.LogSystem.Info("1:" + dicExpMestType.Count);
                    foreach (var item in dicExpMestType.Keys)
                    {
                        listExpMestMediSub = hisExpMestMedicine.Where(o => o.EXP_MEST_TYPE_ID == dicExpMestType[item]).ToList();
                        if (!IsNotNullOrEmpty(listExpMestMediSub)) continue;
                        //Inventec.Common.Logging.LogSystem.Info("1:" + item + listExpMestMediSub.Where(o => o.MEDICINE_TYPE_CODE == "ACE001").Sum(p => p.AMOUNT));
                        ProcessExpAmountMedicineByExpMestType(listExpMestMediSub, item, medistock);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessImpAmountMedicineByImpMestType(List<V_HIS_IMP_MEST_MEDICINE> listImpMestMediSub, string fieldName, V_HIS_MEDI_STOCK medistock)
        {
            try
            {
                if (IsNotNullOrEmpty(listImpMestMediSub))
                {
                    PropertyInfo p = typeof(Mrs00133RDO).GetProperty(fieldName);
                    if (!IsNotNull(p)) return;

                    var GroupImps = listImpMestMediSub.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        Mrs00133RDO rdo = new Mrs00133RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = Thuoc;
                        
                        rdo.MEDI_STOCK_ID = medistock.ID;
                        rdo.MEDI_STOCK_NAME = medistock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID;
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.CONCENTRA = listmediSub.First().CONCENTRA;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.MANUFACTURER_NAME = listmediSub.First().MANUFACTURER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE * (1 + listmediSub.First().IMP_VAT_RATIO);
                        rdo.ACTIVE_INGR_BHYT_CODE = listmediSub.First().ACTIVE_INGR_BHYT_CODE;
                        rdo.ACTIVE_INGR_BHYT_NAME = listmediSub.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        rdo.IMP_VAT = listmediSub.First().IMP_VAT_RATIO;
                        p.SetValue(rdo, listmediSub.Sum(s => s.AMOUNT));
                        rdo.IMP_TOTAL_AMOUNT = listmediSub.Sum(s => s.AMOUNT);
                        listRdoMedicine.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessExpAmountMedicineByExpMestType(List<V_HIS_EXP_MEST_MEDICINE> listExpMestMediSub, string fieldName, V_HIS_MEDI_STOCK medistock)
        {
            try
            {
                if (IsNotNullOrEmpty(listExpMestMediSub))
                {
                    PropertyInfo p = typeof(Mrs00133RDO).GetProperty(fieldName);
                    if (!IsNotNull(p)) return;
                    var GroupImps = listExpMestMediSub.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        Mrs00133RDO rdo = new Mrs00133RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = Thuoc;
                        
                        rdo.MEDI_STOCK_ID = medistock.ID;
                        rdo.MEDI_STOCK_NAME = medistock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID ?? 0;
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.CONCENTRA = listmediSub.First().CONCENTRA;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.MANUFACTURER_NAME = listmediSub.First().MANUFACTURER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE * (1 + listmediSub.First().IMP_VAT_RATIO);
                        rdo.ACTIVE_INGR_BHYT_CODE = listmediSub.First().ACTIVE_INGR_BHYT_CODE;
                        rdo.ACTIVE_INGR_BHYT_NAME = listmediSub.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        rdo.IMP_VAT = listmediSub.First().IMP_VAT_RATIO;
                        rdo.PRICE = listmediSub.First().PRICE * (1 + listmediSub.First().VAT_RATIO);
                        p.SetValue(rdo, listmediSub.Sum(s => s.AMOUNT));
                        rdo.EXP_TOTAL_AMOUNT = listmediSub.Sum(s => s.AMOUNT);
                        //if (rdo.SERVICE_CODE == "ACE001")
                        //Inventec.Common.Logging.LogSystem.Info("sss:" + listmediSub.Sum(x => x.AMOUNT));
                        listRdoMedicine.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        // Tính số lượng nhập và xuất vật tư
        private void ProcessAmountMaterial(List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial, List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial, V_HIS_MEDI_STOCK medistock)
        {
            try
            {
                listRdoMaterial.Clear();
                ProcessImpAmountMaterial(hisImpMestMaterial, medistock);
                ProcessExpAmountMaterial(hisExpMestMaterial, medistock);
                listRdoMaterial = groupById(listRdoMaterial);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listRdoMaterial.Clear();
            }
        }

        private void ProcessAmountBlood(List<V_HIS_IMP_MEST_BLOOD> hisImpMestBlood, List<V_HIS_EXP_MEST_BLOOD> hisExpMestBlood, V_HIS_MEDI_STOCK medistock)
        {
            try
            {
                listRdoBlood.Clear();
                ProcessImpAmountBlood(hisImpMestBlood, medistock);
                ProcessExpAmountBlood(hisExpMestBlood, medistock);
                listRdoBlood = groupById(listRdoBlood);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listRdoMaterial.Clear();
            }
        }

        private void ProcessImpAmountMaterial(List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial, V_HIS_MEDI_STOCK medistock)
        {
            try
            {
                if (IsNotNullOrEmpty(hisImpMestMaterial))
                {
                    var listImpMestMateSub = new List<V_HIS_IMP_MEST_MATERIAL>();
                    foreach (var item in dicImpMestType.Keys)
                    {
                        listImpMestMateSub = hisImpMestMaterial.Where(o => o.IMP_MEST_TYPE_ID == (long)dicImpMestType[item]).ToList();
                        if (!IsNotNullOrEmpty(listImpMestMateSub)) continue;
                        ProcessImpAmountMateByImpMestType(listImpMestMateSub, item, medistock);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessImpAmountBlood(List<V_HIS_IMP_MEST_BLOOD> hisImpMestBlood, V_HIS_MEDI_STOCK medistock)
        {
            try
            {
                if (IsNotNullOrEmpty(hisImpMestBlood))
                {
                    var listImpMestBloodSub = new List<V_HIS_IMP_MEST_BLOOD>();
                    foreach (var item in dicImpMestType.Keys)
                    {
                        listImpMestBloodSub = hisImpMestBlood.Where(o => o.IMP_MEST_TYPE_ID == (long)dicImpMestType[item]).ToList();
                        if (!IsNotNullOrEmpty(listImpMestBloodSub)) continue;
                        ProcessImpAmountBloodByImpMestType(listImpMestBloodSub, item, medistock);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessExpAmountMaterial(List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial, V_HIS_MEDI_STOCK medistock)
        {
            try
            {
                if (IsNotNullOrEmpty(hisExpMestMaterial))
                {
                    if (IsNotNullOrEmpty(hisExpMestMaterial))
                    {
                        var listExpMestMateSub = new List<V_HIS_EXP_MEST_MATERIAL>();
                        foreach (var item in dicExpMestType.Keys)
                        {
                            listExpMestMateSub = hisExpMestMaterial.Where(o => o.EXP_MEST_TYPE_ID == dicExpMestType[item]).ToList();
                            if (!IsNotNullOrEmpty(listExpMestMateSub)) continue;
                            ProcessExpAmountMateByExpMestType(listExpMestMateSub, item, medistock);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessExpAmountBlood(List<V_HIS_EXP_MEST_BLOOD> hisExpMestBlood, V_HIS_MEDI_STOCK medistock)
        {
            try
            {
                if (IsNotNullOrEmpty(hisExpMestBlood))
                {
                    if (IsNotNullOrEmpty(hisExpMestBlood))
                    {
                        var listExpMestBloodSub = new List<V_HIS_EXP_MEST_BLOOD>();
                        foreach (var item in dicExpMestType.Keys)
                        {
                            listExpMestBloodSub = hisExpMestBlood.Where(o => o.EXP_MEST_TYPE_ID == dicExpMestType[item]).ToList();
                            if (!IsNotNullOrEmpty(listExpMestBloodSub)) continue;
                            ProcessExpAmountBloodByExpMestType(listExpMestBloodSub, item, medistock);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessImpAmountMateByImpMestType(List<V_HIS_IMP_MEST_MATERIAL> listImpMestMateSub, string fieldName, V_HIS_MEDI_STOCK medistock)
        {
            try
            {
                if (IsNotNullOrEmpty(listImpMestMateSub))
                {
                    PropertyInfo p = typeof(Mrs00133RDO).GetProperty(string.Format(fieldName));
                    if (!IsNotNull(p)) return;
                    var GroupImps = listImpMestMateSub.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        Mrs00133RDO rdo = new Mrs00133RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = VatTu;
                        
                        rdo.MEDI_STOCK_ID = medistock.ID;
                        rdo.MEDI_STOCK_NAME = medistock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID;
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        // rdo.CONCENTRA = listmateSub.First().CONCENTRA;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.MANUFACTURER_NAME = listmateSub.First().MANUFACTURER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE * (1 + listmateSub.First().IMP_VAT_RATIO);
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmateSub.First().EXPIRED_DATE ?? 0);
                        rdo.IMP_VAT = listmateSub.First().IMP_VAT_RATIO;
                        p.SetValue(rdo, listmateSub.Sum(o => o.AMOUNT));
                        rdo.IMP_TOTAL_AMOUNT = listmateSub.Sum(s => s.AMOUNT);
                        listRdoMaterial.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessImpAmountBloodByImpMestType(List<V_HIS_IMP_MEST_BLOOD> listImpMestBloodSub, string fieldName, V_HIS_MEDI_STOCK medistock)
        {
            try
            {
                if (IsNotNullOrEmpty(listImpMestBloodSub))
                {
                    PropertyInfo p = typeof(Mrs00133RDO).GetProperty(string.Format(fieldName));
                    if (!IsNotNull(p)) return;
                    var GroupImps = listImpMestBloodSub.GroupBy(g => g.BLOOD_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_BLOOD> listBloodSub = group.ToList<V_HIS_IMP_MEST_BLOOD>();
                        Mrs00133RDO rdo = new Mrs00133RDO();
                        rdo.SERVICE_TYPE_NAME = "MÁU";
                        rdo.SERVICE_TYPE_ID = Mau;

                        rdo.MEDI_STOCK_ID = medistock.ID;
                        rdo.MEDI_STOCK_NAME = medistock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listBloodSub.First().BLOOD_ID;
                        rdo.SERVICE_ID = listBloodSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listBloodSub.First().BLOOD_TYPE_CODE;
                        rdo.SERVICE_NAME = listBloodSub.First().BLOOD_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listBloodSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listBloodSub.First().SUPPLIER_ID;
                        // rdo.CONCENTRA = listmateSub.First().CONCENTRA;
                        rdo.SUPPLIER_CODE = listBloodSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listBloodSub.First().SUPPLIER_NAME;
                        //rdo.MANUFACTURER_NAME = listBloodSub.First().MANUFACTURER_NAME;
                        rdo.NUM_ORDER = listBloodSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listBloodSub.First().IMP_PRICE * (1 + listBloodSub.First().IMP_VAT_RATIO);
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listBloodSub.First().EXPIRED_DATE ?? 0);
                        rdo.IMP_VAT = listBloodSub.First().IMP_VAT_RATIO;
                        p.SetValue(rdo, listBloodSub.Count());
                        rdo.IMP_TOTAL_AMOUNT = listBloodSub.Count();
                        listRdoMaterial.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessExpAmountMateByExpMestType(List<V_HIS_EXP_MEST_MATERIAL> listExpMestMateSub, string fieldName, V_HIS_MEDI_STOCK medistock)
        {
            try
            {
                if (IsNotNullOrEmpty(listExpMestMateSub))
                {
                    PropertyInfo p = typeof(Mrs00133RDO).GetProperty(string.Format(fieldName));
                    if (!IsNotNull(p)) return;
                    var GroupImps = listExpMestMateSub.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        Mrs00133RDO rdo = new Mrs00133RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = VatTu;
                        
                        rdo.MEDI_STOCK_ID = medistock.ID;
                        rdo.MEDI_STOCK_NAME = medistock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID ?? 0;
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        //rdo.CONCENTRA = listmateSub.First().CONCENTRA;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.MANUFACTURER_NAME = listmateSub.First().MANUFACTURER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE * (1 + listmateSub.First().IMP_VAT_RATIO);
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmateSub.First().EXPIRED_DATE ?? 0);
                        rdo.IMP_VAT = listmateSub.First().IMP_VAT_RATIO;
                        rdo.PRICE = listmateSub.First().PRICE * (1 + listmateSub.First().VAT_RATIO);
                        p.SetValue(rdo, listmateSub.Sum(o => o.AMOUNT));
                        rdo.EXP_TOTAL_AMOUNT = listmateSub.Sum(s => s.AMOUNT);
                        listRdoMaterial.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessExpAmountBloodByExpMestType(List<V_HIS_EXP_MEST_BLOOD> listExpMestBloodSub, string fieldName, V_HIS_MEDI_STOCK medistock)
        {
            try
            {
                if (IsNotNullOrEmpty(listExpMestBloodSub))
                {
                    PropertyInfo p = typeof(Mrs00133RDO).GetProperty(string.Format(fieldName));
                    if (!IsNotNull(p)) return;
                    var GroupImps = listExpMestBloodSub.GroupBy(g => g.BLOOD_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_EXP_MEST_BLOOD> listBloodSub = group.ToList<V_HIS_EXP_MEST_BLOOD>();
                        Mrs00133RDO rdo = new Mrs00133RDO();
                        rdo.SERVICE_TYPE_NAME = "MÁU";
                        rdo.SERVICE_TYPE_ID = Mau;

                        rdo.MEDI_STOCK_ID = medistock.ID;
                        rdo.MEDI_STOCK_NAME = medistock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listBloodSub.First().BLOOD_ID;
                        rdo.SERVICE_ID = listBloodSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listBloodSub.First().BLOOD_TYPE_CODE;
                        rdo.SERVICE_NAME = listBloodSub.First().BLOOD_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listBloodSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listBloodSub.First().SUPPLIER_ID;
                        //rdo.CONCENTRA = listmateSub.First().CONCENTRA;
                        rdo.SUPPLIER_CODE = listBloodSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listBloodSub.First().SUPPLIER_NAME;
                        //rdo.MANUFACTURER_NAME = listBloodSub.First().SUP;
                        rdo.NUM_ORDER = listBloodSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listBloodSub.First().IMP_PRICE * (1 + listBloodSub.First().IMP_VAT_RATIO);
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listBloodSub.First().EXPIRED_DATE ?? 0);
                        rdo.IMP_VAT = listBloodSub.First().IMP_VAT_RATIO;
                        rdo.PRICE = listBloodSub.First().PRICE * (1 + listBloodSub.First().VAT_RATIO);
                        p.SetValue(rdo, listBloodSub.Count());
                        rdo.EXP_TOTAL_AMOUNT = listBloodSub.Count();
                        listRdoMaterial.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //Lay ky du liệu cũ và gần timeFrom nhất
        private void ProcessGetPeriod(CommonParam paramGet, V_HIS_MEDI_STOCK medistock)
        {
            try
            {
                HisMediStockPeriodViewFilterQuery periodFilter = new HisMediStockPeriodViewFilterQuery();
                periodFilter.MEDI_STOCK_ID = medistock.ID;//CurrentMediStock.ID;
                List<V_HIS_MEDI_STOCK_PERIOD> hisMediStockPeriod = new HisMediStockPeriodManager(paramGet).GetView(periodFilter);
                hisMediStockPeriod = hisMediStockPeriod.Where(x => x.TO_TIME >= castFilter.TIME_FROM).ToList();
                if (!paramGet.HasException)
                {
                    if (IsNotNullOrEmpty(hisMediStockPeriod))
                    {
                        //Trường hợp có kỳ được chốt gần nhất
                        V_HIS_MEDI_STOCK_PERIOD neighborPeriod = hisMediStockPeriod.OrderByDescending(d => d.TO_TIME).ToList()[0];
                        ProcessBeinAmountMedicineByMediStockPeriod(paramGet, neighborPeriod, medistock);
                        processBeinAmountMaterialByMediStockPeriod(paramGet, neighborPeriod, medistock);
                        ProcessBeinAmountBloodByMediStockPeriod(paramGet, neighborPeriod, medistock);
                    }
                    else
                    {
                        // Trường hợp không có kỳ chốt gần nhât - chưa được chốt kỳ nào
                        ProcessBeinAmountMedicineNotMediStockPriod(paramGet, medistock);
                        ProcessBeinAmountMaterialNotMediStockPriod(paramGet, medistock);
                        ProcessBeinAmountBloodNotMediStockPriod(paramGet, medistock);
                    }

                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu.");
                    }
                }
                else
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu.");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listRdoMaterial.Clear();
                listRdoMedicine.Clear();
            }
        }

        //Tinh so luong ton dau neu co chot ky gan nhat của thuốc
        private void ProcessBeinAmountMedicineByMediStockPeriod(CommonParam paramGet, V_HIS_MEDI_STOCK_PERIOD neighborPeriod, V_HIS_MEDI_STOCK medistock)
        {
            try
            {
                List<Mrs00133RDO> listrdo = new List<Mrs00133RDO>();
                HisMestPeriodMediViewFilterQuery periodMediFilter = new HisMestPeriodMediViewFilterQuery();
                periodMediFilter.MEDI_STOCK_PERIOD_ID = neighborPeriod.ID;
                List<V_HIS_MEST_PERIOD_MEDI> hisMestPeriodMedi = new HisMestPeriodMediManager(paramGet).GetView(periodMediFilter);
                if (IsNotNullOrEmpty(hisMestPeriodMedi))
                {
                    foreach (var item in hisMestPeriodMedi)
                    {
                        Mrs00133RDO rdo = new Mrs00133RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = Thuoc;
                        
                        rdo.MEDI_STOCK_ID = medistock.ID;
                        rdo.MEDI_STOCK_NAME = medistock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = item.MEDICINE_ID;
                        rdo.SERVICE_ID = item.SERVICE_ID;
                        rdo.SERVICE_CODE = item.MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = item.MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = item.SUPPLIER_ID;
                        //rdo.CONCENTRA = item.CONCENTRA;
                        rdo.SUPPLIER_CODE = item.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = item.SUPPLIER_NAME;
                        rdo.NUM_ORDER = item.NUM_ORDER;
                        rdo.IMP_PRICE = item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                        rdo.IMP_VAT = item.IMP_VAT_RATIO;
                        //rdo.BEGIN_AMOUNT = item.AMOUNT;
                        rdo.BEGIN_AMOUNT = item.BEGIN_AMOUNT??0;
                        listrdo.Add(rdo);
                    }
                }

                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
               DateTime impTimeFrom = new DateTime(int.Parse(neighborPeriod.TO_TIME.ToString().Substring(0, 4)), int.Parse(neighborPeriod.TO_TIME.ToString().Substring(4, 2)), int.Parse(neighborPeriod.TO_TIME.ToString().Substring(6, 2)),23,59,59);
               DateTime impTimeTo = new DateTime(int.Parse(castFilter.TIME_FROM.ToString().Substring(0, 4)), int.Parse(castFilter.TIME_FROM.ToString().Substring(4, 2)), int.Parse(castFilter.TIME_FROM.ToString().Substring(6, 2)), 23, 59, 59);
               impMediFilter.IMP_TIME_FROM = (long.Parse(impTimeFrom.AddDays(1).AddMonths(-1).ToString("yyyyMMdd"))* 1000000);
               impMediFilter.IMP_TIME_TO = (long.Parse(impTimeTo.AddDays(-1).ToString("yyyyMMdd")) * 1000000)+235959;
               impMediFilter.MEDI_STOCK_ID = medistock.ID;//CurrentMediStock.ID;
                impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new HisImpMestMedicineManager(paramGet).GetView(impMediFilter);
                if (castFilter.IS_MERGE_STOCK_THROW_CHMS == true)
                {
                    hisImpMestMedicine = hisImpMestMedicine.Where(o => !this.listChmsImpMestId.Contains(o.IMP_MEST_TYPE_ID)).ToList();
                }
                if (IsNotNullOrEmpty(hisImpMestMedicine))
                {
                    var GroupImps = hisImpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        Mrs00133RDO rdo = new Mrs00133RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = Thuoc;
                        
                        rdo.MEDI_STOCK_ID = medistock.ID;
                        rdo.MEDI_STOCK_NAME = medistock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID;
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.CONCENTRA = listmediSub.First().CONCENTRA;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.MANUFACTURER_NAME = listmediSub.First().MANUFACTURER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE * (1 + listmediSub.First().IMP_VAT_RATIO);
                        rdo.ACTIVE_INGR_BHYT_CODE = listmediSub.First().ACTIVE_INGR_BHYT_CODE;
                        rdo.ACTIVE_INGR_BHYT_NAME = listmediSub.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        rdo.IMP_VAT = listmediSub.First().IMP_VAT_RATIO;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => s.AMOUNT);
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                DateTime expTimeFrom = new DateTime(int.Parse(neighborPeriod.TO_TIME.ToString().Substring(0, 4)), int.Parse(neighborPeriod.TO_TIME.ToString().Substring(4, 2)), int.Parse(neighborPeriod.TO_TIME.ToString().Substring(6, 2)), 23, 59, 59);
                DateTime expTimeTo = new DateTime(int.Parse(castFilter.TIME_FROM.ToString().Substring(0, 4)), int.Parse(castFilter.TIME_FROM.ToString().Substring(4, 2)), int.Parse(castFilter.TIME_FROM.ToString().Substring(6, 2)), 23, 59, 59);
                expMediFilter.EXP_TIME_FROM = (long.Parse(expTimeFrom.AddDays(1).AddMonths(-1).ToString("yyyyMMdd")) * 1000000); //neighborPeriod.TO_TIME + 1;
                expMediFilter.EXP_TIME_TO = (long.Parse(expTimeTo.AddDays(-1).ToString("yyyyMMdd")) * 1000000) + 235959;//castFilter.TIME_FROM - 1;
                expMediFilter.MEDI_STOCK_ID = medistock.ID;
                expMediFilter.IS_EXPORT = true;
                //expMediFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new HisExpMestMedicineManager(paramGet).GetView(expMediFilter);
                if (castFilter.IS_MERGE_STOCK_THROW_CHMS == true)
                {
                    hisExpMestMedicine = hisExpMestMedicine.Where(o => !this.listChmsExpMestId.Contains(o.EXP_MEST_TYPE_ID)).ToList();
                }
                if (IsNotNullOrEmpty(hisExpMestMedicine))
                {
                    var GroupExps = hisExpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        Mrs00133RDO rdo = new Mrs00133RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = Thuoc;
                        
                        rdo.MEDI_STOCK_ID = medistock.ID;
                        rdo.MEDI_STOCK_NAME = medistock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID ?? 0;
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.CONCENTRA = listmediSub.First().CONCENTRA;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.MANUFACTURER_NAME = listmediSub.First().MANUFACTURER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE * (1 + listmediSub.First().IMP_VAT_RATIO);
                        rdo.ACTIVE_INGR_BHYT_CODE = listmediSub.First().ACTIVE_INGR_BHYT_CODE;
                        rdo.ACTIVE_INGR_BHYT_NAME = listmediSub.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        rdo.IMP_VAT = listmediSub.First().IMP_VAT_RATIO;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => -(s.AMOUNT));
                        listrdo.Add(rdo);
                    }
                }

                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s => new Mrs00133RDO
                {
                    SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                    SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                    MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                    MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                    MEDI_MATE_ID = s.First().MEDI_MATE_ID,
                    SUPPLIER_ID = s.First().SUPPLIER_ID,
                    CONCENTRA = s.First().CONCENTRA,
                    SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                    SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                    MANUFACTURER_NAME = s.First().MANUFACTURER_NAME,
                    IMP_PRICE = s.First().IMP_PRICE,
                    EXPIRED_DATE_STR = s.First().EXPIRED_DATE_STR,
                    ACTIVE_INGR_BHYT_CODE = s.First().ACTIVE_INGR_BHYT_CODE,
                    ACTIVE_INGR_BHYT_NAME = s.First().ACTIVE_INGR_BHYT_NAME,
                    IMP_VAT = s.First().IMP_VAT,
                    SERVICE_ID = s.First().SERVICE_ID,
                    SERVICE_CODE = s.First().SERVICE_CODE,
                    SERVICE_NAME = s.First().SERVICE_NAME,
                    SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                    NUM_ORDER = s.First().NUM_ORDER,
                    BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT)
                }).ToList();
                listRdoMedicine.AddRange(listrdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //Tinh so luong ton dau neu co chot ky gan nhat của máu
        private void ProcessBeinAmountBloodByMediStockPeriod(CommonParam paramGet, V_HIS_MEDI_STOCK_PERIOD neighborPeriod, V_HIS_MEDI_STOCK medistock)
        {
            try
            {
                List<Mrs00133RDO> listrdo = new List<Mrs00133RDO>();
                string query = string.Format("select * from v_his_mest_period_blty where medi_stock_period_id = {0}", neighborPeriod.ID);

                List<V_HIS_MEST_PERIOD_BLTY> hisMestPeriodMedi = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_MEST_PERIOD_BLTY>(query);
                if (IsNotNullOrEmpty(hisMestPeriodMedi))
                {
                    foreach (var item in hisMestPeriodMedi)
                    {
                        var bloodType = hisBloodType.FirstOrDefault(p => p.ID == item.BLOOD_TYPE_ID);
                        var blood = Bloods.FirstOrDefault(p => p.BLOOD_TYPE_ID == item.BLOOD_TYPE_ID);
                        Mrs00133RDO rdo = new Mrs00133RDO();
                        rdo.SERVICE_TYPE_NAME = "MÁU";
                        rdo.SERVICE_TYPE_ID = Mau;

                        rdo.MEDI_STOCK_ID = medistock.ID;
                        rdo.MEDI_STOCK_NAME = medistock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = blood.ID;
                        rdo.SERVICE_ID = bloodType.SERVICE_ID;
                        rdo.SERVICE_CODE = bloodType.BLOOD_TYPE_CODE;
                        rdo.SERVICE_NAME = bloodType.BLOOD_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = blood.SUPPLIER_ID;
                        //rdo.CONCENTRA = item.CONCENTRA;
                        rdo.SUPPLIER_CODE = blood.SUPPLIER_ID != null ? blood.HIS_SUPPLIER.SUPPLIER_CODE : "";
                        rdo.SUPPLIER_NAME = blood.SUPPLIER_ID != null ? blood.HIS_SUPPLIER.SUPPLIER_NAME : "";
                        rdo.NUM_ORDER = bloodType.NUM_ORDER;
                        rdo.IMP_PRICE = bloodType.IMP_PRICE ?? 0 * (1 + (bloodType.IMP_VAT_RATIO ?? 0));
                        rdo.IMP_VAT = bloodType.IMP_VAT_RATIO ?? 0;
                        //rdo.BEGIN_AMOUNT = item.AMOUNT;
                        rdo.BEGIN_AMOUNT = item.BEGIN_AMOUNT;
                        listrdo.Add(rdo);
                    }
                }

                HisImpMestBloodViewFilterQuery impBloodFilter = new HisImpMestBloodViewFilterQuery();
                DateTime impTimeFrom = new DateTime(int.Parse(neighborPeriod.TO_TIME.ToString().Substring(0, 4)), int.Parse(neighborPeriod.TO_TIME.ToString().Substring(4, 2)), int.Parse(neighborPeriod.TO_TIME.ToString().Substring(6, 2)), 23, 59, 59);
                DateTime impTimeTo = new DateTime(int.Parse(castFilter.TIME_FROM.ToString().Substring(0, 4)), int.Parse(castFilter.TIME_FROM.ToString().Substring(4, 2)), int.Parse(castFilter.TIME_FROM.ToString().Substring(6, 2)), 23, 59, 59);
                impBloodFilter.IMP_TIME_FROM = (long.Parse(impTimeFrom.AddDays(1).AddMonths(-1).ToString("yyyyMMdd")) * 1000000);
                impBloodFilter.IMP_TIME_TO = (long.Parse(impTimeTo.AddDays(-1).ToString("yyyyMMdd")) * 1000000) + 235959;
                impBloodFilter.MEDI_STOCK_ID = medistock.ID;//CurrentMediStock.ID;
                impBloodFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_BLOOD> hisImpMestBlood = new HisImpMestBloodManager(paramGet).GetView(impBloodFilter);
                if (castFilter.IS_MERGE_STOCK_THROW_CHMS == true)
                {
                    hisImpMestBlood = hisImpMestBlood.Where(o => !this.listChmsImpMestId.Contains(o.IMP_MEST_TYPE_ID)).ToList();
                }
                if (IsNotNullOrEmpty(hisImpMestBlood))
                {
                    var GroupImps = hisImpMestBlood.GroupBy(g => g.BLOOD_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_BLOOD> listBloodSub = group.ToList<V_HIS_IMP_MEST_BLOOD>();
                        Mrs00133RDO rdo = new Mrs00133RDO();
                        rdo.SERVICE_TYPE_NAME = "MÁU";
                        rdo.SERVICE_TYPE_ID = Mau;

                        rdo.MEDI_STOCK_ID = medistock.ID;
                        rdo.MEDI_STOCK_NAME = medistock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listBloodSub.First().BLOOD_ID;
                        rdo.SERVICE_ID = listBloodSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listBloodSub.First().BLOOD_TYPE_CODE;
                        rdo.SERVICE_NAME = listBloodSub.First().BLOOD_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listBloodSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listBloodSub.First().SUPPLIER_ID;
                        //rdo.CONCENTRA = listBloodSub.First().CONCENTRA;
                        rdo.SUPPLIER_CODE = listBloodSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listBloodSub.First().SUPPLIER_NAME;
                        //rdo.MANUFACTURER_NAME = listBloodSub.First().MANUFACTURER_NAME;
                        rdo.NUM_ORDER = listBloodSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listBloodSub.First().IMP_PRICE * (1 + listBloodSub.First().IMP_VAT_RATIO);
                        //rdo.ACTIVE_INGR_BHYT_CODE = listBloodSub.First().ACTIVE_INGR_BHYT_CODE;
                        //rdo.ACTIVE_INGR_BHYT_NAME = listBloodSub.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listBloodSub.First().EXPIRED_DATE ?? 0);
                        rdo.IMP_VAT = listBloodSub.First().IMP_VAT_RATIO;
                        rdo.BEGIN_AMOUNT = listBloodSub.Count();
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestBloodViewFilterQuery expBloodFilter = new HisExpMestBloodViewFilterQuery();
                DateTime expTimeFrom = new DateTime(int.Parse(neighborPeriod.TO_TIME.ToString().Substring(0, 4)), int.Parse(neighborPeriod.TO_TIME.ToString().Substring(4, 2)), int.Parse(neighborPeriod.TO_TIME.ToString().Substring(6, 2)), 23, 59, 59);
                DateTime expTimeTo = new DateTime(int.Parse(castFilter.TIME_FROM.ToString().Substring(0, 4)), int.Parse(castFilter.TIME_FROM.ToString().Substring(4, 2)), int.Parse(castFilter.TIME_FROM.ToString().Substring(6, 2)), 23, 59, 59);
                expBloodFilter.EXP_TIME_FROM = (long.Parse(expTimeFrom.AddDays(1).AddMonths(-1).ToString("yyyyMMdd")) * 1000000); //neighborPeriod.TO_TIME + 1;
                expBloodFilter.EXP_TIME_TO = (long.Parse(expTimeTo.AddDays(-1).ToString("yyyyMMdd")) * 1000000) + 235959;//castFilter.TIME_FROM - 1;
                expBloodFilter.MEDI_STOCK_ID = medistock.ID;
                expBloodFilter.IS_EXPORT = true;
                //expMediFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                List<V_HIS_EXP_MEST_BLOOD> hisExpMestBlood = new HisExpMestBloodManager(paramGet).GetView(expBloodFilter);
                if (castFilter.IS_MERGE_STOCK_THROW_CHMS == true)
                {
                    hisExpMestBlood = hisExpMestBlood.Where(o => !this.listChmsExpMestId.Contains(o.EXP_MEST_TYPE_ID)).ToList();
                }
                if (IsNotNullOrEmpty(hisExpMestBlood))
                {
                    var GroupExps = hisExpMestBlood.GroupBy(g => g.BLOOD_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_BLOOD> listBloodSub = group.ToList<V_HIS_EXP_MEST_BLOOD>();
                        Mrs00133RDO rdo = new Mrs00133RDO();
                        rdo.SERVICE_TYPE_NAME = "MÁU";
                        rdo.SERVICE_TYPE_ID = Mau;

                        rdo.MEDI_STOCK_ID = medistock.ID;
                        rdo.MEDI_STOCK_NAME = medistock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listBloodSub.First().BLOOD_ID;
                        rdo.SERVICE_ID = listBloodSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listBloodSub.First().BLOOD_TYPE_CODE;
                        rdo.SERVICE_NAME = listBloodSub.First().BLOOD_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listBloodSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listBloodSub.First().SUPPLIER_ID;
                        //rdo.CONCENTRA = listBloodSub.First().CONCENTRA;
                        rdo.SUPPLIER_CODE = listBloodSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listBloodSub.First().SUPPLIER_NAME;
                        //rdo.MANUFACTURER_NAME = listBloodSub.First().MANUFACTURER_NAME;
                        rdo.NUM_ORDER = listBloodSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listBloodSub.First().IMP_PRICE * (1 + listBloodSub.First().IMP_VAT_RATIO);
                        //rdo.ACTIVE_INGR_BHYT_CODE = listBloodSub.First().ACTIVE_INGR_BHYT_CODE;
                        //rdo.ACTIVE_INGR_BHYT_NAME = listBloodSub.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listBloodSub.First().EXPIRED_DATE ?? 0);
                        rdo.IMP_VAT = listBloodSub.First().IMP_VAT_RATIO;
                        rdo.BEGIN_AMOUNT = listBloodSub.Count();
                        listrdo.Add(rdo);
                    }
                }

                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s => new Mrs00133RDO
                {
                    SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                    SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                    MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                    MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                    MEDI_MATE_ID = s.First().MEDI_MATE_ID,
                    SUPPLIER_ID = s.First().SUPPLIER_ID,
                    //CONCENTRA = s.First().CONCENTRA,
                    SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                    SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                    //MANUFACTURER_NAME = s.First().MANUFACTURER_NAME,
                    IMP_PRICE = s.First().IMP_PRICE,
                    EXPIRED_DATE_STR = s.First().EXPIRED_DATE_STR,
                    //ACTIVE_INGR_BHYT_CODE = s.First().ACTIVE_INGR_BHYT_CODE,
                    //ACTIVE_INGR_BHYT_NAME = s.First().ACTIVE_INGR_BHYT_NAME,
                    IMP_VAT = s.First().IMP_VAT,
                    SERVICE_ID = s.First().SERVICE_ID,
                    SERVICE_CODE = s.First().SERVICE_CODE,
                    SERVICE_NAME = s.First().SERVICE_NAME,
                    SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                    NUM_ORDER = s.First().NUM_ORDER,
                    BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT)
                }).ToList();
                listRdoBlood.AddRange(listrdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //Tính số lượng tồn đầu nếu có chốt kỳ gần nhất của vật tư
        private void processBeinAmountMaterialByMediStockPeriod(CommonParam paramGet, V_HIS_MEDI_STOCK_PERIOD neighborPeriod, V_HIS_MEDI_STOCK medistock)
        {
            try
            {
                List<Mrs00133RDO> listrdo = new List<Mrs00133RDO>();
                HisMestPeriodMateViewFilterQuery periodMateFilter = new HisMestPeriodMateViewFilterQuery();
                periodMateFilter.MEDI_STOCK_PERIOD_ID = neighborPeriod.ID;
                List<V_HIS_MEST_PERIOD_MATE> hisMestPeriodMate = new HisMestPeriodMateManager(paramGet).GetView(periodMateFilter);
                if (IsNotNullOrEmpty(hisMestPeriodMate))
                {
                    foreach (var item in hisMestPeriodMate)
                    {
                        Mrs00133RDO rdo = new Mrs00133RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = VatTu;
                        
                        rdo.MEDI_STOCK_ID = medistock.ID;
                        rdo.MEDI_STOCK_NAME = medistock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = item.MATERIAL_ID;
                        rdo.SERVICE_ID = item.SERVICE_ID;
                        rdo.SERVICE_CODE = item.MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = item.MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = item.SUPPLIER_ID;
                        //rdo.CONCENTRA = item.CONCENTRA;
                        rdo.SUPPLIER_CODE = item.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = item.SUPPLIER_NAME;
                        rdo.NUM_ORDER = item.NUM_ORDER;
                        rdo.IMP_PRICE = item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                        rdo.IMP_VAT = item.IMP_VAT_RATIO;
                        rdo.BEGIN_AMOUNT = item.AMOUNT;
                        //rdo.BEGIN_AMOUNT = item.BEGIN_AMOUNT ?? 0;
                        listrdo.Add(rdo);
                    }
                }

                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                DateTime impTimeFrom = new DateTime(int.Parse(neighborPeriod.TO_TIME.ToString().Substring(0, 4)), int.Parse(neighborPeriod.TO_TIME.ToString().Substring(4, 2)), int.Parse(neighborPeriod.TO_TIME.ToString().Substring(6, 2)), 23, 59, 59);
                DateTime impTimeTo = new DateTime(int.Parse(castFilter.TIME_FROM.ToString().Substring(0, 4)), int.Parse(castFilter.TIME_FROM.ToString().Substring(4, 2)), int.Parse(castFilter.TIME_FROM.ToString().Substring(6, 2)), 23, 59, 59);
                impMateFilter.IMP_TIME_FROM = (long.Parse(impTimeFrom.AddDays(1).AddMonths(-1).ToString("yyyyMMdd")) * 1000000);//neighborPeriod.TO_TIME + 1;
                impMateFilter.IMP_TIME_TO = (long.Parse(impTimeTo.AddDays(-1).ToString("yyyyMMdd")) * 1000000) + 235959;//castFilter.TIME_FROM - 1;
                impMateFilter.MEDI_STOCK_ID = medistock.ID;
                impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; ;
                List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new HisImpMestMaterialManager(paramGet).GetView(impMateFilter);
                if (castFilter.IS_MERGE_STOCK_THROW_CHMS == true)
                {
                    hisImpMestMaterial = hisImpMestMaterial.Where(o => !this.listChmsImpMestId.Contains(o.IMP_MEST_TYPE_ID)).ToList();
                }
                if (IsNotNullOrEmpty(hisImpMestMaterial))
                {
                    var GroupImps = hisImpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        Mrs00133RDO rdo = new Mrs00133RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = VatTu;
                        
                        rdo.MEDI_STOCK_ID = medistock.ID;
                        rdo.MEDI_STOCK_NAME = medistock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID;
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        //rdo.CONCENTRA = listmateSub.First().CONCENTRA;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.MANUFACTURER_NAME = listmateSub.First().MANUFACTURER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE * (1 + listmateSub.First().IMP_VAT_RATIO);
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmateSub.First().EXPIRED_DATE ?? 0);
                        rdo.IMP_VAT = listmateSub.First().IMP_VAT_RATIO;
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => s.AMOUNT);
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                DateTime expTimeFrom = new DateTime(int.Parse(neighborPeriod.TO_TIME.ToString().Substring(0, 4)), int.Parse(neighborPeriod.TO_TIME.ToString().Substring(4, 2)), int.Parse(neighborPeriod.TO_TIME.ToString().Substring(6, 2)), 23, 59, 59);
                DateTime expTimeTo = new DateTime(int.Parse(castFilter.TIME_FROM.ToString().Substring(0, 4)), int.Parse(castFilter.TIME_FROM.ToString().Substring(4, 2)), int.Parse(castFilter.TIME_FROM.ToString().Substring(6, 2)), 23, 59, 59);
                expMateFilter.EXP_TIME_FROM = (long.Parse(expTimeFrom.AddDays(1).AddMonths(-1).ToString("yyyyMMdd")) * 1000000); //neighborPeriod.TO_TIME + 1;
                expMateFilter.EXP_TIME_TO = (long.Parse(expTimeTo.AddDays(-1).ToString("yyyyMMdd")) * 1000000) + 235959;//castFilter.TIME_FROM - 1;
                //expMateFilter.EXP_TIME_FROM = neighborPeriod.TO_TIME + 1;
                //expMateFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMateFilter.MEDI_STOCK_ID = medistock.ID;
                expMateFilter.IS_EXPORT = true;
                //expMateFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new HisExpMestMaterialManager(paramGet).GetView(expMateFilter);
                if (castFilter.IS_MERGE_STOCK_THROW_CHMS == true)
                {
                    hisExpMestMaterial = hisExpMestMaterial.Where(o => !this.listChmsExpMestId.Contains(o.EXP_MEST_TYPE_ID)).ToList();
                }
                if (IsNotNullOrEmpty(hisExpMestMaterial))
                {
                    var GroupExps = hisExpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        Mrs00133RDO rdo = new Mrs00133RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = VatTu;
                        
                        rdo.MEDI_STOCK_ID = medistock.ID;
                        rdo.MEDI_STOCK_NAME = medistock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID ?? 0;
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        //rdo.CONCENTRA = listmateSub.First().CONCENTRA;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.MANUFACTURER_NAME = listmateSub.First().MANUFACTURER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE * (1 + listmateSub.First().IMP_VAT_RATIO);
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmateSub.First().EXPIRED_DATE ?? 0);
                        rdo.IMP_VAT = listmateSub.First().IMP_VAT_RATIO;
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => -(s.AMOUNT));
                        listrdo.Add(rdo);
                    }
                }

                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s => new Mrs00133RDO
                {
                    SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                    SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                    MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                    MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                    MEDI_MATE_ID = s.First().MEDI_MATE_ID,
                    SUPPLIER_ID = s.First().SUPPLIER_ID,
                    CONCENTRA = s.First().CONCENTRA,
                    SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                    SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                    MANUFACTURER_NAME = s.First().MANUFACTURER_NAME,
                    IMP_PRICE = s.First().IMP_PRICE,
                    EXPIRED_DATE_STR = s.First().EXPIRED_DATE_STR,
                    ACTIVE_INGR_BHYT_CODE = s.First().ACTIVE_INGR_BHYT_CODE,
                    ACTIVE_INGR_BHYT_NAME = s.First().ACTIVE_INGR_BHYT_NAME,
                    IMP_VAT = s.First().IMP_VAT,
                    SERVICE_ID = s.First().SERVICE_ID,
                    SERVICE_CODE = s.First().SERVICE_CODE,
                    SERVICE_NAME = s.First().SERVICE_NAME,
                    SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                    NUM_ORDER = s.First().NUM_ORDER,
                    BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT)
                }).ToList();
                listRdoMaterial.AddRange(listrdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //Tính số lượng tồn đầu nều không có chốt kỳ gần nhất của thuốc
        private void ProcessBeinAmountMedicineNotMediStockPriod(CommonParam paramGet, V_HIS_MEDI_STOCK medistock)
        {
            try
            {
                List<Mrs00133RDO> listrdo = new List<Mrs00133RDO>();
                DateTime impTimeTo = new DateTime(int.Parse(castFilter.TIME_FROM.ToString().Substring(0, 4)), int.Parse(castFilter.TIME_FROM.ToString().Substring(4, 2)), int.Parse(castFilter.TIME_FROM.ToString().Substring(6, 2)), 23, 59, 59);
                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                impMediFilter.IMP_TIME_TO = (long.Parse(impTimeTo.AddDays(-1).ToString("yyyyMMdd")) * 1000000) + 235959;//castFilter.TIME_FROM - 1;
                impMediFilter.MEDI_STOCK_ID = medistock.ID;
                impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new HisImpMestMedicineManager().GetView(impMediFilter);
                if (castFilter.IS_MERGE_STOCK_THROW_CHMS == true)
                {
                    hisImpMestMedicine = hisImpMestMedicine.Where(o => !this.listChmsImpMestId.Contains(o.IMP_MEST_TYPE_ID)).ToList();
                }
                if (IsNotNullOrEmpty(hisImpMestMedicine))
                {
                    var GroupImps = hisImpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        Mrs00133RDO rdo = new Mrs00133RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = Thuoc;
                        
                        rdo.MEDI_STOCK_ID = medistock.ID;
                        rdo.MEDI_STOCK_NAME = medistock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID;
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.CONCENTRA = listmediSub.First().CONCENTRA;
                        rdo.ACTIVE_INGR_BHYT_CODE = listmediSub.First().ACTIVE_INGR_BHYT_CODE;
                        rdo.ACTIVE_INGR_BHYT_NAME = listmediSub.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.MANUFACTURER_NAME = listmediSub.First().MANUFACTURER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.ACTIVE_INGR_BHYT_CODE = listmediSub.First().ACTIVE_INGR_BHYT_CODE;
                        rdo.ACTIVE_INGR_BHYT_NAME = listmediSub.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE * (1 + listmediSub.First().IMP_VAT_RATIO);
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        rdo.IMP_VAT = listmediSub.First().IMP_VAT_RATIO;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => s.AMOUNT);
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                DateTime expTimeTo = new DateTime(int.Parse(castFilter.TIME_FROM.ToString().Substring(0, 4)), int.Parse(castFilter.TIME_FROM.ToString().Substring(4, 2)), int.Parse(castFilter.TIME_FROM.ToString().Substring(6, 2)), 23, 59, 59);
                expMediFilter.EXP_TIME_TO = (long.Parse(impTimeTo.AddDays(-1).ToString("yyyyMMdd")) * 1000000) + 235959; //castFilter.TIME_FROM - 1;
                expMediFilter.MEDI_STOCK_ID = medistock.ID;
                expMediFilter.IS_EXPORT = true;
                //expMediFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new HisExpMestMedicineManager().GetView(expMediFilter);
                if (castFilter.IS_MERGE_STOCK_THROW_CHMS == true)
                {
                    hisExpMestMedicine = hisExpMestMedicine.Where(o => !this.listChmsExpMestId.Contains(o.EXP_MEST_TYPE_ID)).ToList();
                }
                if (IsNotNullOrEmpty(hisExpMestMedicine))
                {
                    var GroupExps = hisExpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        Mrs00133RDO rdo = new Mrs00133RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = Thuoc;
                        
                        rdo.MEDI_STOCK_ID = medistock.ID;
                        rdo.MEDI_STOCK_NAME = medistock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID ?? 0;
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.CONCENTRA = listmediSub.First().CONCENTRA;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.MANUFACTURER_NAME = listmediSub.First().MANUFACTURER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.ACTIVE_INGR_BHYT_CODE = listmediSub.First().ACTIVE_INGR_BHYT_CODE;
                        rdo.ACTIVE_INGR_BHYT_NAME = listmediSub.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE * (1 + listmediSub.First().IMP_VAT_RATIO);
                        rdo.IMP_VAT = listmediSub.First().IMP_VAT_RATIO;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => -(s.AMOUNT));
                        listrdo.Add(rdo);
                    }
                }

                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s =>
                    new Mrs00133RDO
                    {
                        SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                        SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                        MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                        MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                        CONCENTRA = s.First().CONCENTRA,
                        EXPIRED_DATE_STR = s.First().EXPIRED_DATE_STR,
                        ACTIVE_INGR_BHYT_CODE = s.First().ACTIVE_INGR_BHYT_CODE,
                        ACTIVE_INGR_BHYT_NAME = s.First().ACTIVE_INGR_BHYT_NAME,
                        MEDI_MATE_ID = s.First().MEDI_MATE_ID,
                        SUPPLIER_ID = s.First().SUPPLIER_ID,
                        SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                        SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                        MANUFACTURER_NAME = s.First().MANUFACTURER_NAME,
                        IMP_PRICE = s.First().IMP_PRICE,
                        IMP_VAT = s.First().IMP_VAT,
                        SERVICE_ID = s.First().SERVICE_ID,
                        SERVICE_CODE = s.First().SERVICE_CODE,
                        SERVICE_NAME = s.First().SERVICE_NAME,
                        SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                        NUM_ORDER = s.First().NUM_ORDER,
                        BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT)
                    }).ToList();
                listRdoMedicine.AddRange(listrdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //Tính số lượng tồn đầu nếu không có chốt kỳ gần nhất của máu
        private void ProcessBeinAmountBloodNotMediStockPriod(CommonParam paramGet, V_HIS_MEDI_STOCK medistock)
        {
            try
            {
                List<Mrs00133RDO> listrdo = new List<Mrs00133RDO>();
                DateTime impTimeTo = new DateTime(int.Parse(castFilter.TIME_FROM.ToString().Substring(0, 4)), int.Parse(castFilter.TIME_FROM.ToString().Substring(4, 2)), int.Parse(castFilter.TIME_FROM.ToString().Substring(6, 2)), 23, 59, 59);
                HisImpMestBloodViewFilterQuery impBloodFilter = new HisImpMestBloodViewFilterQuery();
                impBloodFilter.IMP_TIME_TO = (long.Parse(impTimeTo.AddDays(-1).ToString("yyyyMMdd")) * 1000000) + 235959;//castFilter.TIME_FROM - 1;
                impBloodFilter.MEDI_STOCK_ID = medistock.ID;
                impBloodFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_BLOOD> hisImpMestBlood = new HisImpMestBloodManager().GetView(impBloodFilter);
                if (castFilter.IS_MERGE_STOCK_THROW_CHMS == true)
                {
                    hisImpMestBlood = hisImpMestBlood.Where(o => !this.listChmsImpMestId.Contains(o.IMP_MEST_TYPE_ID)).ToList();
                }
                if (IsNotNullOrEmpty(hisImpMestBlood))
                {
                    var GroupImps = hisImpMestBlood.GroupBy(g => g.BLOOD_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_BLOOD> listBloodSub = group.ToList<V_HIS_IMP_MEST_BLOOD>();
                        Mrs00133RDO rdo = new Mrs00133RDO();
                        rdo.SERVICE_TYPE_NAME = "MÁU";
                        rdo.SERVICE_TYPE_ID = Mau;

                        rdo.MEDI_STOCK_ID = medistock.ID;
                        rdo.MEDI_STOCK_NAME = medistock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listBloodSub.First().BLOOD_ID;
                        rdo.SERVICE_ID = listBloodSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listBloodSub.First().BLOOD_TYPE_CODE;
                        rdo.SERVICE_NAME = listBloodSub.First().BLOOD_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listBloodSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listBloodSub.First().SUPPLIER_ID;
                        //rdo.CONCENTRA = listBloodSub.First().CONCENTRA;
                        rdo.SUPPLIER_CODE = listBloodSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listBloodSub.First().SUPPLIER_NAME;
                        //rdo.MANUFACTURER_NAME = listBloodSub.First().MANUFACTURER_NAME;
                        rdo.NUM_ORDER = listBloodSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listBloodSub.First().IMP_PRICE * (1 + listBloodSub.First().IMP_VAT_RATIO);
                        //rdo.ACTIVE_INGR_BHYT_CODE = listBloodSub.First().ACTIVE_INGR_BHYT_CODE;
                        //rdo.ACTIVE_INGR_BHYT_NAME = listBloodSub.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listBloodSub.First().EXPIRED_DATE ?? 0);
                        rdo.IMP_VAT = listBloodSub.First().IMP_VAT_RATIO;
                        rdo.BEGIN_AMOUNT = listBloodSub.Count();
                        
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestBloodViewFilterQuery expBloodFilter = new HisExpMestBloodViewFilterQuery();
                DateTime expTimeTo = new DateTime(int.Parse(castFilter.TIME_FROM.ToString().Substring(0, 4)), int.Parse(castFilter.TIME_FROM.ToString().Substring(4, 2)), int.Parse(castFilter.TIME_FROM.ToString().Substring(6, 2)), 23, 59, 59);
                expBloodFilter.EXP_TIME_TO = (long.Parse(impTimeTo.AddDays(-1).ToString("yyyyMMdd")) * 1000000) + 235959; //castFilter.TIME_FROM - 1;
                expBloodFilter.MEDI_STOCK_ID = medistock.ID;
                expBloodFilter.IS_EXPORT = true;
                //expMediFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                List<V_HIS_EXP_MEST_BLOOD> hisExpMestBlood = new HisExpMestBloodManager().GetView(expBloodFilter);
                if (castFilter.IS_MERGE_STOCK_THROW_CHMS == true)
                {
                    hisExpMestBlood = hisExpMestBlood.Where(o => !this.listChmsExpMestId.Contains(o.EXP_MEST_TYPE_ID)).ToList();
                }
                if (IsNotNullOrEmpty(hisExpMestBlood))
                {
                    var GroupExps = hisExpMestBlood.GroupBy(g => g.BLOOD_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_BLOOD> listBloodSub = group.ToList<V_HIS_EXP_MEST_BLOOD>();
                        Mrs00133RDO rdo = new Mrs00133RDO();
                        rdo.SERVICE_TYPE_NAME = "MÁU";
                        rdo.SERVICE_TYPE_ID = Mau;

                        rdo.MEDI_STOCK_ID = medistock.ID;
                        rdo.MEDI_STOCK_NAME = medistock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listBloodSub.First().BLOOD_ID;
                        rdo.SERVICE_ID = listBloodSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listBloodSub.First().BLOOD_TYPE_CODE;
                        rdo.SERVICE_NAME = listBloodSub.First().BLOOD_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listBloodSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listBloodSub.First().SUPPLIER_ID;
                        //rdo.CONCENTRA = listBloodSub.First().CONCENTRA;
                        rdo.SUPPLIER_CODE = listBloodSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listBloodSub.First().SUPPLIER_NAME;
                        //rdo.MANUFACTURER_NAME = listBloodSub.First().MANUFACTURER_NAME;
                        rdo.NUM_ORDER = listBloodSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listBloodSub.First().IMP_PRICE * (1 + listBloodSub.First().IMP_VAT_RATIO);
                        //rdo.ACTIVE_INGR_BHYT_CODE = listBloodSub.First().ACTIVE_INGR_BHYT_CODE;
                        //rdo.ACTIVE_INGR_BHYT_NAME = listBloodSub.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listBloodSub.First().EXPIRED_DATE ?? 0);
                        rdo.IMP_VAT = listBloodSub.First().IMP_VAT_RATIO;
                        rdo.BEGIN_AMOUNT = listBloodSub.Count();

                        listrdo.Add(rdo);
                    }
                }

                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s =>
                    new Mrs00133RDO
                    {
                        SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                        SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                        MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                        MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                        //CONCENTRA = s.First().CONCENTRA,
                        EXPIRED_DATE_STR = s.First().EXPIRED_DATE_STR,
                        //ACTIVE_INGR_BHYT_CODE = s.First().ACTIVE_INGR_BHYT_CODE,
                        //ACTIVE_INGR_BHYT_NAME = s.First().ACTIVE_INGR_BHYT_NAME,
                        MEDI_MATE_ID = s.First().MEDI_MATE_ID,
                        SUPPLIER_ID = s.First().SUPPLIER_ID,
                        SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                        SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                        //MANUFACTURER_NAME = s.First().MANUFACTURER_NAME,
                        IMP_PRICE = s.First().IMP_PRICE,
                        IMP_VAT = s.First().IMP_VAT,
                        SERVICE_ID = s.First().SERVICE_ID,
                        SERVICE_CODE = s.First().SERVICE_CODE,
                        SERVICE_NAME = s.First().SERVICE_NAME,
                        SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                        NUM_ORDER = s.First().NUM_ORDER,
                        BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT)
                    }).ToList();
                listRdoBlood.AddRange(listrdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        // Tính số lượng tồn đầu nếu không có chốt kỳ gần nhất của vật tư
        private void ProcessBeinAmountMaterialNotMediStockPriod(CommonParam paramGet, V_HIS_MEDI_STOCK medistock)
        {
            try
            {
                List<Mrs00133RDO> listrdo = new List<Mrs00133RDO>();

                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                //impMateFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                DateTime impTimeTo = new DateTime(int.Parse(castFilter.TIME_FROM.ToString().Substring(0, 4)), int.Parse(castFilter.TIME_FROM.ToString().Substring(4, 2)), int.Parse(castFilter.TIME_FROM.ToString().Substring(6, 2)), 23, 59, 59);
                impMateFilter.IMP_TIME_TO = (long.Parse(impTimeTo.AddDays(-1).ToString("yyyyMMdd")) * 1000000) + 235959;
                impMateFilter.MEDI_STOCK_ID = medistock.ID;
                impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; ;
                List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new HisImpMestMaterialManager(paramGet).GetView(impMateFilter);
                if (castFilter.IS_MERGE_STOCK_THROW_CHMS == true)
                {
                    hisImpMestMaterial = hisImpMestMaterial.Where(o => !this.listChmsImpMestId.Contains(o.IMP_MEST_TYPE_ID)).ToList();
                }
                if (IsNotNullOrEmpty(hisImpMestMaterial))
                {
                    var GroupImps = hisImpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        Mrs00133RDO rdo = new Mrs00133RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = VatTu;
                        
                        rdo.MEDI_STOCK_ID = medistock.ID;
                        rdo.MEDI_STOCK_NAME = medistock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID;
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        //rdo.CONCENTRA = listmateSub.First().CONCENTRA;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.MANUFACTURER_NAME = listmateSub.First().MANUFACTURER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE * (1 + listmateSub.First().IMP_VAT_RATIO);
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmateSub.First().EXPIRED_DATE ?? 0);
                        rdo.IMP_VAT = listmateSub.First().IMP_VAT_RATIO;
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => s.AMOUNT);
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                DateTime expTimeTo = new DateTime(int.Parse(castFilter.TIME_FROM.ToString().Substring(0, 4)), int.Parse(castFilter.TIME_FROM.ToString().Substring(4, 2)), int.Parse(castFilter.TIME_FROM.ToString().Substring(6, 2)), 23, 59, 59);
                expMateFilter.EXP_TIME_TO = (long.Parse(impTimeTo.AddDays(-1).ToString("yyyyMMdd")) * 1000000) + 235959;
                //expMateFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMateFilter.MEDI_STOCK_ID = medistock.ID;
                expMateFilter.IS_EXPORT = true;
                //expMateFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new HisExpMestMaterialManager(paramGet).GetView(expMateFilter);
                if (castFilter.IS_MERGE_STOCK_THROW_CHMS == true)
                {
                    hisExpMestMaterial = hisExpMestMaterial.Where(o => !this.listChmsExpMestId.Contains(o.EXP_MEST_TYPE_ID)).ToList();
                }
                if (IsNotNullOrEmpty(hisExpMestMaterial))
                {
                    var GroupExps = hisExpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        Mrs00133RDO rdo = new Mrs00133RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = VatTu;
                        
                        rdo.MEDI_STOCK_ID = medistock.ID;
                        rdo.MEDI_STOCK_NAME = medistock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID ?? 0;
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        //rdo.CONCENTRA = listmateSub.First().CONCENTRA;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.MANUFACTURER_NAME = listmateSub.First().MANUFACTURER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE * (1 + listmateSub.First().IMP_VAT_RATIO);
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmateSub.First().EXPIRED_DATE ?? 0);
                        rdo.IMP_VAT = listmateSub.First().IMP_VAT_RATIO;
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => -(s.AMOUNT));
                        listrdo.Add(rdo);
                    }
                }

                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s =>
                    new Mrs00133RDO
                    {
                        SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                        SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                        MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                        MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                        MEDI_MATE_ID = s.First().MEDI_MATE_ID,
                        SUPPLIER_ID = s.First().SUPPLIER_ID,
                        CONCENTRA = s.First().CONCENTRA,
                        SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                        SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                        MANUFACTURER_NAME = s.First().MANUFACTURER_NAME,
                        IMP_PRICE = s.First().IMP_PRICE,
                        EXPIRED_DATE_STR = s.First().EXPIRED_DATE_STR,
                        ACTIVE_INGR_BHYT_CODE = s.First().ACTIVE_INGR_BHYT_CODE,
                        ACTIVE_INGR_BHYT_NAME = s.First().ACTIVE_INGR_BHYT_NAME,
                        IMP_VAT = s.First().IMP_VAT,
                        SERVICE_ID = s.First().SERVICE_ID,
                        SERVICE_CODE = s.First().SERVICE_CODE,
                        SERVICE_NAME = s.First().SERVICE_NAME,
                        SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                        NUM_ORDER = s.First().NUM_ORDER,
                        BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT)
                    }).ToList();
                listRdoMaterial.AddRange(listrdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        private void AddInfoGroup(List<Mrs00133RDO> ListRdo)
        {
            foreach (var item in ListRdo)
            {
                if (item.SERVICE_TYPE_ID == Thuoc)
                {
                    var medicineType = hisMedicineType.FirstOrDefault(o => o.SERVICE_ID == item.SERVICE_ID);

                    if (medicineType != null && medicineType.MEDICINE_LINE_ID != null)
                    {
                        item.MEDICINE_LINE_ID = medicineType.MEDICINE_LINE_ID;
                        item.MEDICINE_LINE_CODE = medicineType.MEDICINE_LINE_CODE;
                        item.MEDICINE_LINE_NAME = medicineType.MEDICINE_LINE_NAME;
                    }
                    else
                    {
                        item.MEDICINE_LINE_ID = 0;
                        item.MEDICINE_LINE_CODE = "DTK";
                        item.MEDICINE_LINE_NAME = "Dòng thuốc khác";
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
                        var parentMedicineType = hisMedicineType.FirstOrDefault(o => o.ID == medicineType.PARENT_ID);
                        if (parentMedicineType != null)
                        {
                            item.PARENT_MEDICINE_TYPE_ID = parentMedicineType.ID;
                            item.PARENT_MEDICINE_TYPE_CODE = parentMedicineType.MEDICINE_TYPE_CODE;
                            item.PARENT_MEDICINE_TYPE_NAME = parentMedicineType.MEDICINE_TYPE_NAME;
                        }
                        else
                        {
                            item.PARENT_MEDICINE_TYPE_ID = 0;
                            item.PARENT_MEDICINE_TYPE_CODE = "NTK";
                            item.PARENT_MEDICINE_TYPE_NAME = "Nhóm thuốc khác";
                        }
                    }
                    else
                    {
                        item.PARENT_MEDICINE_TYPE_ID = 0;
                        item.PARENT_MEDICINE_TYPE_CODE = "NTK";
                        item.PARENT_MEDICINE_TYPE_NAME = "Nhóm thuốc khác";
                    }

                    //neu gop theo nhom thuoc thi thay the SERVICE_TYPE_ID bang nhom thuoc

                    if (castFilter.IS_MEDICINE_GROUP.HasValue && castFilter.IS_MEDICINE_GROUP.Value)
                    {
                        item.SERVICE_TYPE_ID = ThuocThuong;
                        item.SERVICE_TYPE_NAME = "Thuốc Thường";

                        if (item.MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__GN)
                        {
                            item.SERVICE_TYPE_ID = GayNghien;
                            item.SERVICE_TYPE_NAME = item.MEDICINE_GROUP_NAME;
                        }
                        else if (item.MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__HT)
                        {
                            item.SERVICE_TYPE_ID = HuongThan;
                            item.SERVICE_TYPE_NAME = item.MEDICINE_GROUP_NAME;
                        }
                        else if (item.MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__DICH_TRUYEN)
                        {
                            item.SERVICE_TYPE_ID = DichTruyen;
                            item.SERVICE_TYPE_NAME = item.MEDICINE_GROUP_NAME;
                        }
                    }
                }
                else if (item.SERVICE_TYPE_ID == VatTu)
                {
                    var materialType = hisMaterialType.FirstOrDefault(o => o.SERVICE_ID == item.SERVICE_ID);

                    if (materialType != null && materialType.PARENT_ID != null)
                    {
                        var parentMaterialType = hisMaterialType.FirstOrDefault(o => o.ID == materialType.PARENT_ID);
                        if (parentMaterialType != null)
                        {
                            item.PARENT_MEDICINE_TYPE_ID = parentMaterialType.ID;
                            item.PARENT_MEDICINE_TYPE_CODE = parentMaterialType.MATERIAL_TYPE_CODE;
                            item.PARENT_MEDICINE_TYPE_NAME = parentMaterialType.MATERIAL_TYPE_NAME;
                        }
                        else
                        {
                            item.PARENT_MEDICINE_TYPE_ID = 0;
                            item.PARENT_MEDICINE_TYPE_CODE = "NVTK";
                            item.PARENT_MEDICINE_TYPE_NAME = "Nhóm vật tư khác";
                        }
                    }
                    else
                    {
                        item.PARENT_MEDICINE_TYPE_ID = 0;
                        item.PARENT_MEDICINE_TYPE_CODE = "NVTK";
                        item.PARENT_MEDICINE_TYPE_NAME = "Nhóm vật tư khác";
                    }

                    item.MEDICINE_LINE_CODE = "DVT";
                    item.MEDICINE_LINE_NAME = "Vật tư";
                    item.MEDICINE_GROUP_CODE = "DVT";
                    item.MEDICINE_GROUP_NAME = "Vật tư";
                    //item.PARENT_MEDICINE_TYPE_CODE = "DVT";
                    //item.PARENT_MEDICINE_TYPE_NAME = "Vật tư";
                }
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            #region Cac the Single
            if (castFilter.TIME_FROM > 0)
            {
                dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
            }
            if (castFilter.TIME_TO > 0)
            {
                dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
            }
            if (castFilter.MEDI_STOCK_IDs != null)
            {
                dicSingleTag.Add("MEDI_STOCK_NAMEs", string.Join(",", HisMediStockCFG.HisMediStocks.Where(o => castFilter.MEDI_STOCK_IDs.Contains(o.ID)).Select(p => p.MEDI_STOCK_NAME).ToList()));
            }
            #endregion

            if (castFilter.IS_MEDICINE == true || castFilter.IS_MATERIAL == true || castFilter.IS_CHEMICAL_SUBSTANCE == true)
            {
                if (castFilter.IS_MEDICINE != true)
                {
                    ListRdo = ListRdo.Where(o => o.SERVICE_TYPE_ID != Thuoc).ToList();
                }
                if (castFilter.IS_MATERIAL != true)
                {
                    ListRdo = ListRdo.Where(o => o.SERVICE_TYPE_ID != VatTu).ToList();
                }
                if (castFilter.IS_CHEMICAL_SUBSTANCE != true)
                {
                    ListRdo = ListRdo.Where(o => o.SERVICE_TYPE_ID != HoaChat).ToList();
                }
            }
            
            ListRdo = ListRdo.OrderBy(o => o.MEDI_STOCK_ID).ThenBy(t1 => t1.SERVICE_TYPE_ID).ThenBy(t3 => t3.SERVICE_NAME).ThenByDescending(t2 => t2.NUM_ORDER).ToList();
            if (ListRdo != null && ListRdo.Count > 0 && ListMediStock != null && ListMediStock.Count > 0)
            {
                var mediStockId = ListRdo.Select(o => o.MEDI_STOCK_ID).Distinct().ToList();
                ListMediStock = ListMediStock.Where(o => mediStockId.Contains(o.ID)).ToList();
            }

            V_HIS_MEDI_STOCK mediStockDefault = new V_HIS_MEDI_STOCK();
            if (IsNotNullOrEmpty(ListRdo))
            {
                mediStockDefault = new V_HIS_MEDI_STOCK() { ID = ListRdo.First().MEDI_STOCK_ID, MEDI_STOCK_NAME = "" };
            }

            objectTag.AddObjectData(store, "MediStocks", (castFilter.IS_MERGE_STOCK != true) ? new List<V_HIS_MEDI_STOCK>() { mediStockDefault } : ListMediStock);
            objectTag.AddObjectData(store, "Services", ListRdo.OrderBy(p => p.SERVICE_TYPE_ID).ThenBy(p => p.SERVICE_NAME).ToList());

            if (IsNotNullOrEmpty(ListRdo))
            {
                if (castFilter.IS_MERGE_STOCK == true)
                {

                    objectTag.AddObjectData(store, "Parent", ListRdo.GroupBy(o => new { o.MEDICINE_LINE_ID, o.PARENT_MEDICINE_TYPE_ID, o.SERVICE_TYPE_ID, o.MEDI_STOCK_ID }).Select(p => p.First()).ToList());

                    objectTag.AddObjectData(store, "GrandParent", ListRdo.OrderBy(q => q.PARENT_MEDICINE_TYPE_NAME).GroupBy(o => new { o.PARENT_MEDICINE_TYPE_ID, o.SERVICE_TYPE_ID, o.MEDI_STOCK_ID }).Select(p => p.First()).ToList());

                    objectTag.AddObjectData(store, "ServicesType", ListRdo.GroupBy(o => new { o.SERVICE_TYPE_ID, o.MEDI_STOCK_ID }).Select(s => s.First()).OrderBy(p => p.SERVICE_TYPE_ID).ToList());

                    objectTag.AddRelationship(store, "Parent", "Services", new string[] { "MEDICINE_LINE_ID", "PARENT_MEDICINE_TYPE_ID", "SERVICE_TYPE_ID", "MEDI_STOCK_ID" }, new string[] { "MEDICINE_LINE_ID", "PARENT_MEDICINE_TYPE_ID", "SERVICE_TYPE_ID", "MEDI_STOCK_ID" });

                    objectTag.AddRelationship(store, "GrandParent", "Services", new string[] { "PARENT_MEDICINE_TYPE_ID", "SERVICE_TYPE_ID", "MEDI_STOCK_ID" }, new string[] { "PARENT_MEDICINE_TYPE_ID", "SERVICE_TYPE_ID", "MEDI_STOCK_ID" });

                    objectTag.AddRelationship(store, "GrandParent", "Parent", new string[] { "PARENT_MEDICINE_TYPE_ID", "SERVICE_TYPE_ID", "MEDI_STOCK_ID" }, new string[] { "PARENT_MEDICINE_TYPE_ID", "SERVICE_TYPE_ID", "MEDI_STOCK_ID" });

                    objectTag.AddRelationship(store, "ServicesType", "Services", new string[] { "SERVICE_TYPE_ID", "MEDI_STOCK_ID" }, new string[] { "SERVICE_TYPE_ID", "MEDI_STOCK_ID" });

                    objectTag.AddRelationship(store, "ServicesType", "Parent", new string[] { "SERVICE_TYPE_ID", "MEDI_STOCK_ID" }, new string[] { "SERVICE_TYPE_ID", "MEDI_STOCK_ID" });

                    objectTag.AddRelationship(store, "ServicesType", "GrandParent", new string[] { "SERVICE_TYPE_ID", "MEDI_STOCK_ID" }, new string[] { "SERVICE_TYPE_ID", "MEDI_STOCK_ID" });

                    objectTag.AddRelationship(store, "MediStocks", "Services", "ID", "MEDI_STOCK_ID");

                    objectTag.AddRelationship(store, "MediStocks", "Parent", "ID", "MEDI_STOCK_ID");

                    objectTag.AddRelationship(store, "MediStocks", "GrandParent", "ID", "MEDI_STOCK_ID");

                    objectTag.AddRelationship(store, "MediStocks", "ServicesType", "ID", "MEDI_STOCK_ID");
                }
                else
                {
                    objectTag.AddObjectData(store, "Parent", ListRdo.GroupBy(o => new { o.MEDICINE_LINE_ID, o.PARENT_MEDICINE_TYPE_ID, o.SERVICE_TYPE_ID }).Select(p => p.First()).ToList());

                    objectTag.AddObjectData(store, "GrandParent", ListRdo.OrderBy(q => q.PARENT_MEDICINE_TYPE_NAME).GroupBy(o => new { o.PARENT_MEDICINE_TYPE_ID, o.SERVICE_TYPE_ID }).Select(p => p.First()).ToList());

                    objectTag.AddObjectData(store, "ServicesType", ListRdo.GroupBy(o => o.SERVICE_TYPE_ID).Select(s => s.First()).OrderBy(p => p.SERVICE_TYPE_ID).ToList());

                    objectTag.AddRelationship(store, "Parent", "Services", new string[] { "MEDICINE_LINE_ID", "PARENT_MEDICINE_TYPE_ID", "SERVICE_TYPE_ID" }, new string[] { "MEDICINE_LINE_ID", "PARENT_MEDICINE_TYPE_ID", "SERVICE_TYPE_ID" });

                    objectTag.AddRelationship(store, "GrandParent", "Services", new string[] { "PARENT_MEDICINE_TYPE_ID", "SERVICE_TYPE_ID" }, new string[] { "PARENT_MEDICINE_TYPE_ID", "SERVICE_TYPE_ID" });

                    objectTag.AddRelationship(store, "GrandParent", "Parent", new string[] { "PARENT_MEDICINE_TYPE_ID", "SERVICE_TYPE_ID" }, new string[] { "PARENT_MEDICINE_TYPE_ID", "SERVICE_TYPE_ID" });

                    objectTag.AddRelationship(store, "ServicesType", "Services", "SERVICE_TYPE_ID", "SERVICE_TYPE_ID");

                    objectTag.AddRelationship(store, "ServicesType", "Parent", "SERVICE_TYPE_ID", "SERVICE_TYPE_ID");

                    objectTag.AddRelationship(store, "ServicesType", "GrandParent", "SERVICE_TYPE_ID", "SERVICE_TYPE_ID");
                }
            }

            objectTag.SetUserFunction(store, "FuncSameTitleCol", new CustomerFuncMergeSameData());
            objectTag.SetUserFunction(store, "FuncRownumber", new RDOCustomerFuncManyRownumberData());
        }
    }

    class CustomerFuncMergeSameData : TFlexCelUserFunction
    {
        long MediStockId;
        int SameType;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length <= 0)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            bool result = false;
            try
            {
                long mediId = Convert.ToInt64(parameters[0]);
                int ServiceId = Convert.ToInt32(parameters[1]);

                if (mediId > 0 && ServiceId > 0)
                {
                    if (SameType == ServiceId && MediStockId == mediId)
                    {
                        return true;
                    }
                    else
                    {
                        MediStockId = mediId;
                        SameType = ServiceId;
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return result;
        }
    }

    class RDOCustomerFuncManyRownumberData : TFlexCelUserFunction
    {
        long Medi_Stock_Id;
        int Service_Type_Id;
        long num_order = 0;
        public RDOCustomerFuncManyRownumberData()
        {
        }
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            try
            {
                long mediId = Convert.ToInt64(parameters[0]);
                int ServiceId = Convert.ToInt32(parameters[1]);

                if (mediId > 0 && ServiceId > 0)
                {
                    if (Service_Type_Id == ServiceId && Medi_Stock_Id == mediId)
                    {
                        num_order = num_order + 1;
                    }
                    else
                    {
                        Medi_Stock_Id = mediId;
                        Service_Type_Id = ServiceId;
                        num_order = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Debug(ex);
            }

            return num_order;
        }
    }
}
