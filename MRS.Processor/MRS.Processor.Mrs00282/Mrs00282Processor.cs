using AutoMapper;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpSource;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisSupplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisMedicinePaty;
using MOS.MANAGER.HisMaterialPaty;
using MOS.MANAGER.HisImpMestBlood;
using MOS.MANAGER.HisBlood;
using MOS.MANAGER.HisBloodType;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestBlood;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;

namespace MRS.Processor.Mrs00282
{
    class Mrs00282Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam();
        List<V_HIS_IMP_MEST> listImp = new List<V_HIS_IMP_MEST>();//get phieu nhap
        List<V_HIS_IMP_MEST> listManuImpMest = new List<V_HIS_IMP_MEST>();
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();//get
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();//get
        List<V_HIS_IMP_MEST_BLOOD> listImpMestBlood = new List<V_HIS_IMP_MEST_BLOOD>();

        List<V_HIS_EXP_MEST> listExp = new List<V_HIS_EXP_MEST>();//get phieu ntra
        List<V_HIS_EXP_MEST> listManuExpMest = new List<V_HIS_EXP_MEST>();
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();//get
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();//get
        List<V_HIS_EXP_MEST_BLOOD> listExpMestBlood = new List<V_HIS_EXP_MEST_BLOOD>();


        List<HIS_IMP_SOURCE> listImpSource = new List<HIS_IMP_SOURCE>();
        Dictionary<long, HIS_IMP_SOURCE> dicImpSource = new Dictionary<long, HIS_IMP_SOURCE>();
        List<V_HIS_BLOOD> listBlood = new List<V_HIS_BLOOD>();
        Dictionary<long, V_HIS_BLOOD> dicBlood = new Dictionary<long, V_HIS_BLOOD>();


        List<V_HIS_MEDICINE> listMedicine = new List<V_HIS_MEDICINE>();
        Dictionary<long, V_HIS_MEDICINE> dicMedicine = new Dictionary<long, V_HIS_MEDICINE>();
        Dictionary<long, V_HIS_MATERIAL> dicMaterial = new Dictionary<long, V_HIS_MATERIAL>();
        List<V_HIS_MATERIAL> listMaterial = new List<V_HIS_MATERIAL>();
        List<Mrs00282RDO> ListMedicineRdo = new List<Mrs00282RDO>();//get
        List<Mrs00282RDO> ListMaterialRdo = new List<Mrs00282RDO>();//get
        List<Mrs00282RDO> ListBloodRdo = new List<Mrs00282RDO>();//get
        List<HIS_SUPPLIER> listSupplier = new List<HIS_SUPPLIER>();//get
        List<HIS_MEDICINE_PATY> listMedicinePaty = new List<HIS_MEDICINE_PATY>();//get
        List<HIS_MATERIAL_PATY> listMaterialPaty = new List<HIS_MATERIAL_PATY>();//get
        List<HIS_MEDICAL_CONTRACT> listMedicalContract = new List<HIS_MEDICAL_CONTRACT>();
        List<V_HIS_MEDICINE_TYPE> listMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MATERIAL_TYPE> listMaterialType = new List<V_HIS_MATERIAL_TYPE>();
        List<V_HIS_BLOOD_TYPE> listBloodType = new List<V_HIS_BLOOD_TYPE>();

        Mrs00282Filter filter;//get
        public Mrs00282Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00282Filter);
        }

        protected override bool GetData()
        {
            filter = ((Mrs00282Filter)reportFilter);
            bool result = true;
            try
            {
                //get dữ liệu:
                //Nhập NCC
                HisSupplierFilterQuery supplierFiler = new HisSupplierFilterQuery();
                listSupplier = new HisSupplierManager(new CommonParam()).Get(supplierFiler);

                //Thuốc
                HisMedicinePatyFilterQuery MedicinePatyFiler = new HisMedicinePatyFilterQuery();
                listMedicinePaty = new HisMedicinePatyManager(new CommonParam()).Get(MedicinePatyFiler);
                //Vật liệu
                HisMaterialPatyFilterQuery MaterialPatyFiler = new HisMaterialPatyFilterQuery();
                listMaterialPaty = new HisMaterialPatyManager(new CommonParam()).Get(MaterialPatyFiler);

                if (filter.IS_IMP_EXP != false)
                {
                    GetImp();
                }
                else
                {
                    GetExp();
                }


                var listImpSource = new HisImpSourceManager().Get(new HisImpSourceFilterQuery());
                foreach (var i in listImpSource) if (!dicImpSource.ContainsKey(i.ID)) dicImpSource[i.ID] = i;

                string query = string.Format("select * from his_medical_contract");
                listMedicalContract = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_MEDICAL_CONTRACT>(query) ?? new List<HIS_MEDICAL_CONTRACT>();
                LogSystem.Info("query:" + query);
                //Loai thuoc, vat tu
                GetMetyMatyBlty();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetExp()
        {
            //filter lọc dữ liệu
            HisExpMestViewFilterQuery filterManu = new HisExpMestViewFilterQuery();
            filterManu.FINISH_TIME_FROM = filter.TIME_FROM??filter.DOCUMENT_DATE_FROM;
            filterManu.FINISH_TIME_TO = filter.TIME_TO??filter.DOCUMENT_DATE_TO;
            filterManu.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
            filterManu.SUPPLIER_ID = filter.SUPPLIER_ID;
            filterManu.MEDI_STOCK_ID = filter.MEDI_STOCK_ID;
            if (filter.MEDI_STOCK_IDs != null)
            {
                filterManu.MEDI_STOCK_IDs = filter.MEDI_STOCK_IDs;
            }
            filterManu.EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__TNCC;
            listManuExpMest = new HisExpMestManager(paramGet).GetView(filterManu);

            listExp = listManuExpMest;

            var listExpMestId = listManuExpMest.Select(o => o.ID).ToList();

            //Máu
            if (IsNotNullOrEmpty(listExpMestId))
            {
                var skip = 0;
                while (listExpMestId.Count - skip > 0)
                {
                    var listIds = listExpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisExpMestBloodViewFilterQuery filterExpMestBlood = new HisExpMestBloodViewFilterQuery
                    {
                        EXP_MEST_IDs = listIds,
                    };
                    var listExpMestBloodSub = new HisExpMestBloodManager(paramGet).GetView(filterExpMestBlood);
                    listExpMestBlood.AddRange(listExpMestBloodSub);
                }
            }

            if (IsNotNullOrEmpty(listExpMestBlood))
            {
                var listExpMestBloodId = listExpMestBlood.Select(o => o.BLOOD_ID).Distinct().ToList();
                var blood = GetBlood(listExpMestBloodId);
                if (blood != null)
                {
                    listBlood.AddRange(blood);
                }
                foreach (var me in listBlood) if (!dicBlood.ContainsKey(me.ID)) dicBlood[me.ID] = me;
                if (filter.IMP_SOURCE_IDs != null && filter.IMP_SOURCE_IDs.Count > 0)
                    listExpMestBlood = listExpMestBlood.Where(o => dicBlood.ContainsKey(o.BLOOD_ID) && filter.IMP_SOURCE_IDs.Contains(dicBlood[o.BLOOD_ID].IMP_SOURCE_ID ?? 0)).ToList();
                else if (filter.IMP_SOURCE_ID != null) listExpMestBlood = listExpMestBlood.Where(o => dicBlood.ContainsKey(o.BLOOD_ID) && filter.IMP_SOURCE_ID == (dicBlood[o.BLOOD_ID].IMP_SOURCE_ID ?? 0)).ToList();
            }

            //Thuốc
            if (IsNotNullOrEmpty(listExpMestId))
            {
                var skip = 0;
                while (listExpMestId.Count - skip > 0)
                {
                    var listIds = listExpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisExpMestMedicineViewFilterQuery filterExpMestMedicine = new HisExpMestMedicineViewFilterQuery
                    {
                        EXP_MEST_IDs = listIds,
                    };
                    var listExpMestMedicineSub = new HisExpMestMedicineManager(paramGet).GetView(filterExpMestMedicine);
                    listExpMestMedicine.AddRange(listExpMestMedicineSub);
                }
            }

            if (IsNotNullOrEmpty(listExpMestMedicine))
            {
                var listExpMestMedicineId = listExpMestMedicine.Select(o => o.MEDICINE_ID??0).Distinct().ToList();
                var medicine = GetMedicine(listExpMestMedicineId);
                if (medicine != null)
                {
                    listMedicine.AddRange(medicine);
                }
                foreach (var me in listMedicine) if (!dicMedicine.ContainsKey(me.ID)) dicMedicine[me.ID] = me;
                if (filter.IMP_SOURCE_IDs != null && filter.IMP_SOURCE_IDs.Count > 0)
                    listExpMestMedicine = listExpMestMedicine.Where(o => dicMedicine.ContainsKey(o.MEDICINE_ID??0) && filter.IMP_SOURCE_IDs.Contains(dicMedicine[o.MEDICINE_ID??0].IMP_SOURCE_ID ?? 0)).ToList();
                else if (filter.IMP_SOURCE_ID != null) listExpMestMedicine = listExpMestMedicine.Where(o => dicMedicine.ContainsKey(o.MEDICINE_ID??0) && filter.IMP_SOURCE_ID == (dicMedicine[o.MEDICINE_ID??0].IMP_SOURCE_ID ?? 0)).ToList();
            }

            //Vật tư:
            if (IsNotNullOrEmpty(listExpMestId))
            {
                var skip = 0;
                while (listExpMestId.Count - skip > 0)
                {
                    var listIds = listExpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisExpMestMaterialViewFilterQuery filterExpMestMaterial = new HisExpMestMaterialViewFilterQuery
                    {
                        EXP_MEST_IDs = listIds,
                    };
                    var listExpMestMaterialSub = new HisExpMestMaterialManager(paramGet).GetView(filterExpMestMaterial);
                    listExpMestMaterial.AddRange(listExpMestMaterialSub);
                }
            }

            if (IsNotNullOrEmpty(listExpMestMaterial))
            {
                var listExpMestMaterialId = listExpMestMaterial.Select(o => o.MATERIAL_ID??0).ToList();
                var material = GetMaterial(listExpMestMaterialId);
                if (material != null)
                {
                    listMaterial.AddRange(material);
                }

                foreach (var ma in listMaterial) if (!dicMaterial.ContainsKey(ma.ID)) dicMaterial[ma.ID] = ma;
                if (filter.IMP_SOURCE_IDs != null && filter.IMP_SOURCE_IDs.Count > 0)
                    listExpMestMaterial = listExpMestMaterial.Where(o => dicMaterial.ContainsKey(o.MATERIAL_ID??0) && filter.IMP_SOURCE_IDs.Contains(dicMaterial[o.MATERIAL_ID??0].IMP_SOURCE_ID ?? 0)).ToList();
                else if (filter.IMP_SOURCE_ID != null) listExpMestMaterial = listExpMestMaterial.Where(o => dicMaterial.ContainsKey(o.MATERIAL_ID??0) && filter.IMP_SOURCE_ID == (dicMaterial[o.MATERIAL_ID??0].IMP_SOURCE_ID ?? 0)).ToList();
            }
        }

        private void GetImp()
        {
            //filter lọc dữ liệu
            HisImpMestViewFilterQuery filterManu = new HisImpMestViewFilterQuery();
            filterManu.IMP_TIME_FROM = filter.TIME_FROM;
            filterManu.IMP_TIME_TO = filter.TIME_TO;
            filterManu.DOCUMENT_DATE_FROM = filter.DOCUMENT_DATE_FROM;
            filterManu.DOCUMENT_DATE_TO = filter.DOCUMENT_DATE_TO;
            filterManu.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
            filterManu.SUPPLIER_ID = filter.SUPPLIER_ID;
            filterManu.MEDI_STOCK_ID = filter.MEDI_STOCK_ID;
            if (filter.MEDI_STOCK_IDs != null)
            {
                filterManu.MEDI_STOCK_IDs = filter.MEDI_STOCK_IDs;
            }
            filterManu.IMP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC;
            listManuImpMest = new HisImpMestManager(paramGet).GetView(filterManu);

            listImp = listManuImpMest;

            var listImpMestId = listManuImpMest.Select(o => o.ID).ToList();

            //Máu
            if (IsNotNullOrEmpty(listImpMestId))
            {
                var skip = 0;
                while (listImpMestId.Count - skip > 0)
                {
                    var listIds = listImpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisImpMestBloodViewFilterQuery filterImpMestBlood = new HisImpMestBloodViewFilterQuery
                    {
                        IMP_MEST_IDs = listIds,
                    };
                    var listImpMestBloodSub = new HisImpMestBloodManager(paramGet).GetView(filterImpMestBlood);
                    listImpMestBlood.AddRange(listImpMestBloodSub);
                }
            }

            if (IsNotNullOrEmpty(listImpMestBlood))
            {
                var listImpMestBloodId = listImpMestBlood.Select(o => o.BLOOD_ID).Distinct().ToList();
                var blood = GetBlood(listImpMestBloodId);
                if (blood != null)
                {
                    listBlood.AddRange(blood);
                }
                foreach (var me in listBlood) if (!dicBlood.ContainsKey(me.ID)) dicBlood[me.ID] = me;
                if (filter.IMP_SOURCE_IDs != null && filter.IMP_SOURCE_IDs.Count > 0)
                    listImpMestBlood = listImpMestBlood.Where(o => dicBlood.ContainsKey(o.BLOOD_ID) && filter.IMP_SOURCE_IDs.Contains(dicBlood[o.BLOOD_ID].IMP_SOURCE_ID ?? 0)).ToList();
                else if (filter.IMP_SOURCE_ID != null) listImpMestBlood = listImpMestBlood.Where(o => dicBlood.ContainsKey(o.BLOOD_ID) && filter.IMP_SOURCE_ID == (dicBlood[o.BLOOD_ID].IMP_SOURCE_ID ?? 0)).ToList();
            }

            //Thuốc
            if (IsNotNullOrEmpty(listImpMestId))
            {
                var skip = 0;
                while (listImpMestId.Count - skip > 0)
                {
                    var listIds = listImpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisImpMestMedicineViewFilterQuery filterImpMestMedicine = new HisImpMestMedicineViewFilterQuery
                    {
                        IMP_MEST_IDs = listIds,
                    };
                    var listImpMestMedicineSub = new HisImpMestMedicineManager(paramGet).GetView(filterImpMestMedicine);
                    listImpMestMedicine.AddRange(listImpMestMedicineSub);
                }
            }

            if (IsNotNullOrEmpty(listImpMestMedicine))
            {
                var listImpMestMedicineId = listImpMestMedicine.Select(o => o.MEDICINE_ID).Distinct().ToList();
                var medicine = GetMedicine(listImpMestMedicineId);
                if (medicine != null)
                {
                    listMedicine.AddRange(medicine);
                }
                foreach (var me in listMedicine) if (!dicMedicine.ContainsKey(me.ID)) dicMedicine[me.ID] = me;
                if (filter.IMP_SOURCE_IDs != null && filter.IMP_SOURCE_IDs.Count > 0)
                    listImpMestMedicine = listImpMestMedicine.Where(o => dicMedicine.ContainsKey(o.MEDICINE_ID) && filter.IMP_SOURCE_IDs.Contains(dicMedicine[o.MEDICINE_ID].IMP_SOURCE_ID ?? 0)).ToList();
                else if (filter.IMP_SOURCE_ID != null) listImpMestMedicine = listImpMestMedicine.Where(o => dicMedicine.ContainsKey(o.MEDICINE_ID) && filter.IMP_SOURCE_ID == (dicMedicine[o.MEDICINE_ID].IMP_SOURCE_ID ?? 0)).ToList();
            }

            //Vật tư:
            if (IsNotNullOrEmpty(listImpMestId))
            {
                var skip = 0;
                while (listImpMestId.Count - skip > 0)
                {
                    var listIds = listImpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisImpMestMaterialViewFilterQuery filterImpMestMaterial = new HisImpMestMaterialViewFilterQuery
                    {
                        IMP_MEST_IDs = listIds,
                    };
                    var listImpMestMaterialSub = new HisImpMestMaterialManager(paramGet).GetView(filterImpMestMaterial);
                    listImpMestMaterial.AddRange(listImpMestMaterialSub);
                }
            }

            if (IsNotNullOrEmpty(listImpMestMaterial))
            {
                var listImpMestMaterialId = listImpMestMaterial.Select(o => o.MATERIAL_ID).ToList();
                var material = GetMaterial(listImpMestMaterialId);
                if (material != null)
                {
                    listMaterial.AddRange(material);
                }
               
                foreach (var ma in listMaterial) if (!dicMaterial.ContainsKey(ma.ID)) dicMaterial[ma.ID] = ma;
                if (filter.IMP_SOURCE_IDs != null && filter.IMP_SOURCE_IDs.Count > 0)
                    listImpMestMaterial = listImpMestMaterial.Where(o => dicMaterial.ContainsKey(o.MATERIAL_ID) && filter.IMP_SOURCE_IDs.Contains(dicMaterial[o.MATERIAL_ID].IMP_SOURCE_ID ?? 0)).ToList();
                else if (filter.IMP_SOURCE_ID != null) listImpMestMaterial = listImpMestMaterial.Where(o => dicMaterial.ContainsKey(o.MATERIAL_ID) && filter.IMP_SOURCE_ID == (dicMaterial[o.MATERIAL_ID].IMP_SOURCE_ID ?? 0)).ToList();
            }
        }


        private List<V_HIS_MEDICINE> GetMedicine(List<long> listImpMestMedicineId)
        {
            List<V_HIS_MEDICINE> result = new List<V_HIS_MEDICINE>();
            try
            {
                var skip = 0;
                while (listImpMestMedicineId.Count - skip > 0)
                {
                    var listIds = listImpMestMedicineId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisMedicineViewFilterQuery filterMedicine = new HisMedicineViewFilterQuery();
                    filterMedicine.IDs = listIds;
                    filter.MEDICINE_GROUP_IDs = filter.MEDICINE_GROUP_IDs;
                    var listMedicineSub = new HisMedicineManager(paramGet).GetView(filterMedicine);
                    result.AddRange(listMedicineSub);
                }

            }
            catch (Exception ex)
            {
                result = new List<V_HIS_MEDICINE>();
                LogSystem.Error(ex);
            }
            return result;
        }


        private List<V_HIS_MATERIAL> GetMaterial(List<long> listImpMestMaterialId)
        {
            List<V_HIS_MATERIAL> result = new List<V_HIS_MATERIAL>();
            try
            {
                var skip = 0;
                while (listImpMestMaterialId.Count - skip > 0)
                {
                    var listIds = listImpMestMaterialId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisMaterialViewFilterQuery filterMaterial = new HisMaterialViewFilterQuery();
                    filterMaterial.IDs = listIds;
                    var listMaterialSub = new HisMaterialManager(paramGet).GetView(filterMaterial);
                    result.AddRange(listMaterialSub);
                }
            }
            catch (Exception ex)
            {
                result = new List<V_HIS_MATERIAL>();
                LogSystem.Error(ex);
            }
            return result;
        }
        private List<V_HIS_BLOOD> GetBlood(List<long> listImpMestBloodId)
        {
            List<V_HIS_BLOOD> result = new List<V_HIS_BLOOD>();
            try
            {
                var skip = 0;
                while (listImpMestBloodId.Count - skip > 0)
                {
                    var listIds = listImpMestBloodId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisBloodViewFilterQuery filterBlood = new HisBloodViewFilterQuery();
                    filterBlood.IDs = listIds;
                    var listBloodSub = new HisBloodManager(paramGet).GetView(filterBlood);
                    result.AddRange(listBloodSub);
                }

            }
            catch (Exception ex)
            {
                result = new List<V_HIS_BLOOD>();
                LogSystem.Error(ex);
            }
            return result;
        }

        private void GetMetyMatyBlty()
        {
            CommonParam paramGet = new CommonParam();
            listMedicineType = new HisMedicineTypeManager(paramGet).GetView(new HisMedicineTypeViewFilterQuery());
            listMaterialType = new HisMaterialTypeManager(paramGet).GetView(new HisMaterialTypeViewFilterQuery());
            listBloodType = new HisBloodTypeManager(paramGet).GetView(new HisBloodTypeViewFilterQuery());

        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                ListMedicineRdo.Clear();
                ListMaterialRdo.Clear();
                ListBloodRdo.Clear();
                if (filter.IS_IMP_EXP != false)
                {
                    ProcessImp();
                }
                else 
                {
                    ProcessExp();
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessExp()
        {
            //Máu
            if (IsNotNullOrEmpty(listExpMestBlood))
            {
                foreach (var ExpMestBlood in listExpMestBlood)
                {
                    Mrs00282RDO rdo = new Mrs00282RDO();
                    rdo.VOLUME = ExpMestBlood.VOLUME;
                    var deliver = listManuExpMest.FirstOrDefault(o => o.ID == ExpMestBlood.EXP_MEST_ID) ?? new V_HIS_EXP_MEST();
                    //var medicinePatys = listMedicinePaty.Where(o => o.MEDICINE_ID == ExpMestBlood.MEDICINE_ID).ToList();
                    //rdo.EXP_PRICE = string.Join(",", (medicinePatys ?? new List<HIS_MEDICINE_PATY>()).Select(o => o.EXP_PRICE).ToList());

                    rdo.AMOUNT = 1;
                    rdo.BID_NUMBER = ExpMestBlood.BID_NUMBER;
                    //rdo.DELIVERER = deliver.DELIVERER;
                    rdo.EXPIRED_DATE = ExpMestBlood.EXPIRED_DATE != null ? Inventec.Common.DateTime.Convert.TimeNumberToDateString((long)ExpMestBlood.EXPIRED_DATE) : "";
                    rdo.IMP_MEST_CODE = ExpMestBlood.EXP_MEST_CODE;
                    //rdo.DOCUMENT_DATE = Inventec.Common.DateTime.Convert.TimeNumberToDateString(deliver.DOCUMENT_DATE ?? 0);
                    //rdo.DOCUMENT_NUMBER = deliver.DOCUMENT_NUMBER;
                    rdo.IMP_TIME = ExpMestBlood.EXP_TIME != null ? Inventec.Common.DateTime.Convert.TimeNumberToDateString((long)ExpMestBlood.EXP_TIME) : "";
                    rdo.TIME_IMP = ExpMestBlood.EXP_TIME;
                    rdo.MANUFACTURER_NAME = ExpMestBlood.BLOOD_ABO_CODE;
                    rdo.BLOOD_TYPE_CODE = ExpMestBlood.BLOOD_TYPE_CODE;
                    rdo.BLOOD_TYPE_NAME = ExpMestBlood.BLOOD_TYPE_NAME;
                    //rdo.RECORDING_TRANSACTION = ExpMestBlood.;
                    //rdo.NATIONAL_NAME = ExpMestBlood.BID_NAME;
                    var supplier = listSupplier.FirstOrDefault(o => o.ID == deliver.SUPPLIER_ID) ?? new HIS_SUPPLIER();
                    rdo.SUPPLIER_CODE = ExpMestBlood.SUPPLIER_CODE ?? supplier.SUPPLIER_CODE;
                    rdo.SUPPLIER_NAME = ExpMestBlood.SUPPLIER_NAME ?? supplier.SUPPLIER_NAME;
                    rdo.PACKAGE_NUMBER = ExpMestBlood.PACKAGE_NUMBER;
                    rdo.IMP_PRICE = ExpMestBlood.IMP_PRICE * (1 + (ExpMestBlood.IMP_VAT_RATIO));
                    rdo.IMP_VAT = ExpMestBlood.IMP_VAT_RATIO;
                    rdo.IMP_PRICE_BEFORE_VAT = ExpMestBlood.IMP_PRICE;
                    rdo.VIR_PRICE = ExpMestBlood.VIR_PRICE;
                    rdo.TOTAL_PRICE = Math.Round(rdo.IMP_PRICE * rdo.AMOUNT);
                    rdo.VIR_TOTAL_PRICE = ExpMestBlood.IMP_PRICE * rdo.AMOUNT;
                    //rdo.PRICE = string.Join(";", (listMedicinePaty ?? new List<HIS_MEDICINE_PATY>()).Where(o => o.MEDICINE_ID == ExpMestBlood.MEDICINE_ID).Distinct().Select(p => p.EXP_PRICE.ToString()).ToList());
                    rdo.VAT = ExpMestBlood.VAT_RATIO ?? 0;
                    rdo.SERVICE_UNIT_NAME = ExpMestBlood.SERVICE_UNIT_NAME;
                    rdo.IMP_SOURCE_NAME = dicBlood.ContainsKey(ExpMestBlood.BLOOD_ID)
                        && dicImpSource.ContainsKey(dicBlood[ExpMestBlood.BLOOD_ID].IMP_SOURCE_ID ?? 0) ? dicImpSource[dicBlood[ExpMestBlood.BLOOD_ID].IMP_SOURCE_ID ?? 0].IMP_SOURCE_NAME : "Khác";

                    //rdo.MEDI_CONTRACT_CODE = "Số hợp đồng: " + string.Join(",", checkContract);
                    //rdo.CONCENTRA = ExpMestBlood.CONCENTRA;
                    //rdo.REGISTER_NUMBER = ExpMestBlood.BID_NUMBER;
                    var bloodType = listBloodType.FirstOrDefault(o => o.ID == ExpMestBlood.BLOOD_TYPE_ID);
                    if (bloodType != null)
                    {
                        var parent = listBloodType.FirstOrDefault(o => o.ID == bloodType.PARENT_ID);
                        if (parent != null)
                        {
                            rdo.PARENT_ID = parent.ID;
                            rdo.PARENT_CODE = parent.BLOOD_TYPE_CODE;
                            rdo.PARENT_NAME = parent.BLOOD_TYPE_NAME;
                        }
                    }
                    ListBloodRdo.Add(rdo);
                }
            }
            List<long> expMestIds = new List<long>();
            //Thuốc
            if (IsNotNullOrEmpty(listExpMestMedicine))
            {
                foreach (var ExpMestMedicine in listExpMestMedicine)
                {
                    var medicine = listMedicine.FirstOrDefault(p => p.ID == ExpMestMedicine.MEDICINE_ID);
                    Mrs00282RDO rdo = new Mrs00282RDO();
                    var deliver = listManuExpMest.FirstOrDefault(o => o.ID == ExpMestMedicine.EXP_MEST_ID) ?? new V_HIS_EXP_MEST();

                    var checkContraindication = listMedicineType != null ? listMedicineType.FirstOrDefault(p => p.ID == ExpMestMedicine.MEDICINE_TYPE_ID) : null;
                    //var checkContract = imp != null && listMedicalContract != null ? listMedicalContract.FirstOrDefault(p => p.ID == imp.MEDICAL_CONTRACT_ID) : null;
                    //var checkContract = EXP_MEST != null && listMedicalContract != null ? listMedicalContract.FirstOrDefault(p => p.ID == EXP_MEST.MEDICAL_CONTRACT_ID) : null;

                    //tổng tiền
                    rdo.TOTAL_PRICE = (ExpMestMedicine.IMP_PRICE * (1 + ExpMestMedicine.IMP_VAT_RATIO) * ExpMestMedicine.AMOUNT);

                    var medicinePatys = listMedicinePaty.Where(o => o.MEDICINE_ID == ExpMestMedicine.MEDICINE_ID).ToList();
                    //số đấu thầu
                    rdo.BID_NUMBER = ExpMestMedicine.BID_NUMBER;
                    //người giao hàng
                    //rdo.DELIVERER = deliver.DELIVERER;
                    //ngày cấp tài liệu
                    //rdo.DOCUMENT_DATE = Inventec.Common.DateTime.Convert.TimeNumberToDateString(deliver.DOCUMENT_DATE ?? 0);
                    //số hiệu tài liệu
                    //rdo.DOCUMENT_NUMBER = deliver.DOCUMENT_NUMBER ?? "----";
                    //bản ghi giao dịch
                    rdo.RECORDING_TRANSACTION = ExpMestMedicine.RECORDING_TRANSACTION;
                    //mã nhà cung cấp
                    var supplier = listSupplier.FirstOrDefault(o => o.ID == deliver.SUPPLIER_ID) ?? new HIS_SUPPLIER();
                    rdo.SUPPLIER_CODE = ExpMestMedicine.SUPPLIER_CODE ?? supplier.SUPPLIER_CODE;
                    rdo.SUPPLIER_NAME = ExpMestMedicine.SUPPLIER_NAME ?? supplier.SUPPLIER_NAME;
                    //số gói
                    rdo.PACKAGE_NUMBER = ExpMestMedicine.PACKAGE_NUMBER;
                    //tên loại gói
                    if (checkContraindication != null)
                    {
                        rdo.PACKING_TYPE_NAME = checkContraindication.PACKING_TYPE_NAME;
                    }
                    //thuế vat
                    rdo.IMP_VAT = ExpMestMedicine.IMP_VAT_RATIO;
                    // giá nhập trước thuế
                    rdo.IMP_PRICE_BEFORE_VAT = ExpMestMedicine.IMP_PRICE;
                    //giá văn kiện
                    //rdo.DOCUMENT_PRICE = ExpMestMedicine.DOCUMENT_PRICE;
                    rdo.VIR_TOTAL_PRICE = ExpMestMedicine.IMP_PRICE * rdo.AMOUNT;
                    //Thuế
                    rdo.VAT = ExpMestMedicine.VAT_RATIO ?? 0;
                    //Tên nguốn nhập
                    rdo.IMP_SOURCE_NAME = dicMedicine.ContainsKey(ExpMestMedicine.MEDICINE_ID??0)
                        && dicImpSource.ContainsKey(dicMedicine[ExpMestMedicine.MEDICINE_ID??0].IMP_SOURCE_ID ?? 0) ? dicImpSource[dicMedicine[ExpMestMedicine.MEDICINE_ID??0].IMP_SOURCE_ID ?? 0].IMP_SOURCE_NAME : "Khác";

                    //số hiệu đăng ký
                    rdo.REGISTER_NUMBER = ExpMestMedicine.REGISTER_NUMBER;
                    //rdo.MEDI_CONTRACT_CODE = "Số hợp đồng: " + string.Join(",", checkContract);

                    //if (checkContract != null)
                    //{
                    //    rdo.MEDI_CONTRACT_CODE = "Số hợp đồng: " + checkContract.MEDICAL_CONTRACT_NAME;
                    //}
                    if (checkContraindication != null)
                    {
                        //CHỐNG CHỈ ĐỊNH
                        rdo.CONTRAINDICATION = checkContraindication.CONTRAINDICATION ?? "";
                        //là vắc xin
                        rdo.IS_VACCINE = checkContraindication.IS_VACCINE;
                        //là thực phẩm chức năng
                        rdo.IS_FUNCTIONAL_FOOD = checkContraindication.IS_FUNCTIONAL_FOOD;
                    }

                    //ngày nhập
                    rdo.IMP_TIME = ExpMestMedicine.EXP_TIME != null ? Inventec.Common.DateTime.Convert.TimeNumberToDateString((long)ExpMestMedicine.EXP_TIME) : "";
                    rdo.TIME_IMP = ExpMestMedicine.EXP_TIME;
                    //TÊN NHÀ SẢN XUẤT
                    //mã phiếu nhập
                    rdo.IMP_MEST_CODE = ExpMestMedicine.EXP_MEST_CODE;
                    rdo.MANUFACTURER_NAME = ExpMestMedicine.MANUFACTURER_NAME;
                    rdo.MEDICINE_TYPE_CODE = ExpMestMedicine.MEDICINE_TYPE_CODE;
                    rdo.MEDICINE_TYPE_NAME = ExpMestMedicine.MEDICINE_TYPE_NAME;
                    rdo.CONCENTRA = ExpMestMedicine.CONCENTRA;
                    rdo.ACTIVE_INGR_BHYT_NAME = ExpMestMedicine.ACTIVE_INGR_BHYT_NAME;
                    rdo.NATIONAL_NAME = ExpMestMedicine.NATIONAL_NAME;
                    rdo.SERVICE_UNIT_NAME = ExpMestMedicine.SERVICE_UNIT_NAME;
                    rdo.PACKAGE_NUMBER = ExpMestMedicine.PACKAGE_NUMBER;
                    rdo.EXPIRED_DATE = ExpMestMedicine.EXPIRED_DATE != null ? Inventec.Common.DateTime.Convert.TimeNumberToDateString((long)ExpMestMedicine.EXPIRED_DATE) : "";
                    rdo.IMP_PRICE = ExpMestMedicine.IMP_PRICE * (1 + (ExpMestMedicine.IMP_VAT_RATIO));
                    rdo.PRICE = string.Join(";", (listMedicinePaty ?? new List<HIS_MEDICINE_PATY>()).Where(o => o.MEDICINE_ID == ExpMestMedicine.MEDICINE_ID).Distinct().Select(p => p.EXP_PRICE.ToString()).ToList());
                    rdo.AMOUNT = ExpMestMedicine.AMOUNT;
                    rdo.EXP_PRICE = string.Join(",", (medicinePatys ?? new List<HIS_MEDICINE_PATY>()).Select(o => o.EXP_PRICE).ToList());

                    var medicineType = listMedicineType.FirstOrDefault(o => o.ID == ExpMestMedicine.MEDICINE_TYPE_ID);
                    if (medicineType != null)
                    {
                        var parent = listMedicineType.FirstOrDefault(o => o.ID == medicineType.PARENT_ID);
                        if (parent != null)
                        {
                            rdo.PARENT_ID = parent.ID;
                            rdo.PARENT_CODE = parent.MEDICINE_TYPE_CODE;
                            rdo.PARENT_NAME = parent.MEDICINE_TYPE_NAME;
                        }
                    }
                    ListMedicineRdo.Add(rdo);
                }
            }

            //Vật tư
            if (IsNotNullOrEmpty(listExpMestMaterial))
            {
                foreach (var ExpMestMaterial in listExpMestMaterial)
                {
                    var deliver = listManuExpMest.FirstOrDefault(o => o.ID == ExpMestMaterial.EXP_MEST_ID) ?? new V_HIS_EXP_MEST();
                    Mrs00282RDO rdo = new Mrs00282RDO();
                    //mã vât liệu
                    rdo.MATERIAL_TYPE_CODE = ExpMestMaterial.MATERIAL_TYPE_CODE;
                    //tên vật liệu
                    rdo.MATERIAL_TYPE_NAME = ExpMestMaterial.MATERIAL_TYPE_NAME;
                    //tên hoathj chất
                    rdo.ACTIVE_INGR_BHYT_NAME = rdo.ACTIVE_INGR_BHYT_NAME;
                    //quốc gia
                    rdo.NATIONAL_NAME = ExpMestMaterial.NATIONAL_NAME;
                    //đơn vị
                    rdo.SERVICE_UNIT_NAME = ExpMestMaterial.SERVICE_UNIT_NAME;
                    //giá nhập
                    rdo.IMP_PRICE = ExpMestMaterial.IMP_PRICE * (1 + (ExpMestMaterial.IMP_VAT_RATIO));
                    //số lượng
                    rdo.AMOUNT = ExpMestMaterial.AMOUNT;// số lượng

                    //tổng tiền
                    rdo.TOTAL_PRICE = (ExpMestMaterial.IMP_PRICE * (1 + ExpMestMaterial.IMP_VAT_RATIO) * ExpMestMaterial.AMOUNT);
                    rdo.VIR_TOTAL_PRICE = Math.Round(ExpMestMaterial.IMP_PRICE * rdo.AMOUNT);

                    rdo.BID_NUMBER = ExpMestMaterial.BID_NUMBER;
                    // người giao
                    //rdo.DELIVERER = deliver.DELIVERER;
                    //hạn sử dụng
                    rdo.EXPIRED_DATE = ExpMestMaterial.EXPIRED_DATE != null ? Inventec.Common.DateTime.Convert.TimeNumberToDateString(ExpMestMaterial.EXPIRED_DATE ?? 0) : "";
                    //hàm lượng
                    rdo.IMP_MEST_CODE = ExpMestMaterial.EXP_MEST_CODE;
                    //Ngay lap chung tu nha cung cap
                    //rdo.DOCUMENT_DATE = Inventec.Common.DateTime.Convert.TimeNumberToDateString(deliver.DOCUMENT_DATE ?? 0);

                    //rdo.DOCUMENT_TIME = deliver.DOCUMENT_DATE ?? 99999999999999;

                    //rdo.DOCUMENT_NUMBER = deliver.DOCUMENT_NUMBER ?? "----";

                    rdo.IMP_TIME = ExpMestMaterial.EXP_TIME != null ? Inventec.Common.DateTime.Convert.TimeNumberToTimeString(ExpMestMaterial.EXP_TIME ?? 0) : "";

                    rdo.TIME_IMP = ExpMestMaterial.EXP_TIME;

                    rdo.MANUFACTURER_NAME = ExpMestMaterial.MANUFACTURER_NAME;
                    //bản ghi
                    rdo.RECORDING_TRANSACTION = ExpMestMaterial.RECORDING_TRANSACTION;
                    //mã nhà cung cấp
                    var supplier = listSupplier.FirstOrDefault(o => o.ID == deliver.SUPPLIER_ID) ?? new HIS_SUPPLIER();
                    rdo.SUPPLIER_CODE = ExpMestMaterial.SUPPLIER_CODE ?? supplier.SUPPLIER_CODE;
                    rdo.SUPPLIER_NAME = ExpMestMaterial.SUPPLIER_NAME ?? supplier.SUPPLIER_NAME;

                    //lô sản suất
                    rdo.PACKAGE_NUMBER = ExpMestMaterial.PACKAGE_NUMBER;

                    var checkContraindication = listMaterialType != null ? listMaterialType.FirstOrDefault(p => p.ID == ExpMestMaterial.MATERIAL_TYPE_ID) : null;
                    if (checkContraindication != null)
                    {
                        rdo.PACKING_TYPE_NAME = checkContraindication.PACKING_TYPE_NAME;
                    }



                    rdo.IMP_VAT = ExpMestMaterial.IMP_VAT_RATIO;

                    rdo.IMP_PRICE_BEFORE_VAT = ExpMestMaterial.IMP_PRICE;

                    //rdo.DOCUMENT_PRICE = ExpMestMaterial.DOCUMENT_PRICE;

                    var materialPatys = listMaterialPaty.Where(o => o.MATERIAL_ID == ExpMestMaterial.MATERIAL_ID).ToList();
                    rdo.EXP_PRICE = string.Join(",", (materialPatys ?? new List<HIS_MATERIAL_PATY>()).Select(o => o.EXP_PRICE).ToList());//giá xuất
                    rdo.PRICE = string.Join(";", (listMaterialPaty ?? new List<HIS_MATERIAL_PATY>()).Where(o => o.MATERIAL_ID == ExpMestMaterial.MATERIAL_ID).Distinct().Select(p => p.EXP_PRICE.ToString()).ToList());
                    // thuế vat
                    rdo.VAT = ExpMestMaterial.VAT_RATIO ?? 0;



                    rdo.IMP_SOURCE_NAME = dicMaterial.ContainsKey(ExpMestMaterial.MATERIAL_ID??0)

                        && dicImpSource.ContainsKey(dicMaterial[ExpMestMaterial.MATERIAL_ID ?? 0].IMP_SOURCE_ID ?? 0) ? dicImpSource[dicMaterial[ExpMestMaterial.MATERIAL_ID ?? 0].IMP_SOURCE_ID ?? 0].IMP_SOURCE_NAME : "Khác";


                    var checkContract = listMedicalContract.Where(o => o.SUPPLIER_ID == ExpMestMaterial.SUPPLIER_ID).Select(P => P.MEDICAL_CONTRACT_CODE).ToList();
                    rdo.MEDI_CONTRACT_CODE = "Số hợp đồng: " + string.Join(",", checkContract);
                    var materialType = listMaterialType.FirstOrDefault(o => o.ID == ExpMestMaterial.MATERIAL_TYPE_ID);
                    if (materialType != null)
                    {
                        rdo.IS_CHEMICAL_SUBSTANCE = materialType.IS_CHEMICAL_SUBSTANCE ?? 0;
                        var parent = listMaterialType.FirstOrDefault(o => o.ID == materialType.PARENT_ID);
                        if (parent != null)
                        {
                            rdo.PARENT_ID = parent.ID;
                            rdo.PARENT_CODE = parent.MATERIAL_TYPE_CODE;
                            rdo.PARENT_NAME = parent.MATERIAL_TYPE_NAME;
                        }
                    }
                    ListMaterialRdo.Add(rdo);
                }
            }
        }

        private void ProcessImp()
        {
            //Máu
            if (IsNotNullOrEmpty(listImpMestBlood))
            {
                foreach (var impMestBlood in listImpMestBlood)
                {
                    Mrs00282RDO rdo = new Mrs00282RDO();
                    rdo.impMestBlood = impMestBlood;
                    rdo.VOLUME = impMestBlood.VOLUME;
                    var deliver = listManuImpMest.FirstOrDefault(o => o.ID == impMestBlood.IMP_MEST_ID) ?? new V_HIS_IMP_MEST();
                    //var medicinePatys = listMedicinePaty.Where(o => o.MEDICINE_ID == impMestBlood.MEDICINE_ID).ToList();
                    //rdo.EXP_PRICE = string.Join(",", (medicinePatys ?? new List<HIS_MEDICINE_PATY>()).Select(o => o.EXP_PRICE).ToList());

                    rdo.AMOUNT = 1;
                    rdo.BID_NUMBER = impMestBlood.BID_NUMBER;
                    rdo.DELIVERER = deliver.DELIVERER;
                    rdo.EXPIRED_DATE = impMestBlood.EXPIRED_DATE != null ? Inventec.Common.DateTime.Convert.TimeNumberToDateString((long)impMestBlood.EXPIRED_DATE) : "";
                    rdo.IMP_MEST_CODE = impMestBlood.IMP_MEST_CODE;
                    rdo.DOCUMENT_DATE = Inventec.Common.DateTime.Convert.TimeNumberToDateString(deliver.DOCUMENT_DATE ?? 0);
                    rdo.DOCUMENT_NUMBER = deliver.DOCUMENT_NUMBER;
                    rdo.IMP_TIME = impMestBlood.IMP_TIME != null ? Inventec.Common.DateTime.Convert.TimeNumberToDateString((long)impMestBlood.IMP_TIME) : "";
                    rdo.TIME_IMP = impMestBlood.IMP_TIME;
                    rdo.MANUFACTURER_NAME = impMestBlood.BLOOD_ABO_CODE;
                    rdo.BLOOD_TYPE_CODE = impMestBlood.BLOOD_TYPE_CODE;
                    rdo.BLOOD_TYPE_NAME = impMestBlood.BLOOD_TYPE_NAME;
                    //rdo.RECORDING_TRANSACTION = impMestBlood.;
                    //rdo.NATIONAL_NAME = impMestBlood.BID_NAME;
                    var supplier = listSupplier.FirstOrDefault(o => o.ID == deliver.SUPPLIER_ID) ?? new HIS_SUPPLIER();
                    rdo.SUPPLIER_CODE = impMestBlood.SUPPLIER_CODE ?? supplier.SUPPLIER_CODE;
                    rdo.SUPPLIER_NAME = impMestBlood.SUPPLIER_NAME ?? supplier.SUPPLIER_NAME;
                    rdo.PACKAGE_NUMBER = impMestBlood.PACKAGE_NUMBER ?? impMestBlood.BLOOD_CODE;
                    rdo.IMP_PRICE = impMestBlood.IMP_PRICE * (1 + (impMestBlood.IMP_VAT_RATIO));
                    rdo.IMP_VAT = impMestBlood.IMP_VAT_RATIO;
                    rdo.IMP_PRICE_BEFORE_VAT = impMestBlood.IMP_PRICE;
                    rdo.VIR_PRICE = impMestBlood.VIR_PRICE;
                    rdo.TOTAL_PRICE = Math.Round(rdo.IMP_PRICE * rdo.AMOUNT);
                    rdo.VIR_TOTAL_PRICE = impMestBlood.IMP_PRICE * rdo.AMOUNT;
                    //rdo.PRICE = string.Join(";", (listMedicinePaty ?? new List<HIS_MEDICINE_PATY>()).Where(o => o.MEDICINE_ID == impMestBlood.MEDICINE_ID).Distinct().Select(p => p.EXP_PRICE.ToString()).ToList());
                    rdo.VAT = impMestBlood.VAT_RATIO ?? 0;
                    rdo.SERVICE_UNIT_NAME = impMestBlood.SERVICE_UNIT_NAME;
                    rdo.IMP_SOURCE_NAME = dicBlood.ContainsKey(impMestBlood.BLOOD_ID)
                        && dicImpSource.ContainsKey(dicBlood[impMestBlood.BLOOD_ID].IMP_SOURCE_ID ?? 0) ? dicImpSource[dicBlood[impMestBlood.BLOOD_ID].IMP_SOURCE_ID ?? 0].IMP_SOURCE_NAME : "Khác";

                    //rdo.MEDI_CONTRACT_CODE = "Số hợp đồng: " + string.Join(",", checkContract);
                    //rdo.CONCENTRA = impMestBlood.CONCENTRA;
                    //rdo.REGISTER_NUMBER = impMestBlood.BID_NUMBER;
                    var bloodType = listBloodType.FirstOrDefault(o => o.ID == impMestBlood.BLOOD_TYPE_ID);
                    if (bloodType != null)
                    {
                        var parent = listBloodType.FirstOrDefault(o => o.ID == bloodType.PARENT_ID);
                        if (parent != null)
                        {
                            rdo.PARENT_ID = parent.ID;
                            rdo.PARENT_CODE = parent.BLOOD_TYPE_CODE;
                            rdo.PARENT_NAME = parent.BLOOD_TYPE_NAME;
                        }
                    }
                    ListBloodRdo.Add(rdo);
                }
            }
            List<long> expMestIds = new List<long>();
            //Thuốc
            if (IsNotNullOrEmpty(listImpMestMedicine))
            {
                foreach (var impMestMedicine in listImpMestMedicine)
                {
                    var medicine = listMedicine.FirstOrDefault(p => p.ID == impMestMedicine.MEDICINE_ID);
                    var checkContraindication = listMedicineType != null ? listMedicineType.FirstOrDefault(p => p.ID == impMestMedicine.MEDICINE_TYPE_ID) : null;
                    Mrs00282RDO rdo = new Mrs00282RDO();

                    rdo.impMestMedicine = impMestMedicine;
                    var deliver = listManuImpMest.FirstOrDefault(o => o.ID == impMestMedicine.IMP_MEST_ID) ?? new V_HIS_IMP_MEST();
                    //var checkContract = imp != null && listMedicalContract != null ? listMedicalContract.FirstOrDefault(p => p.ID == imp.MEDICAL_CONTRACT_ID) : null;
                    //var checkContract = imp_mest != null && listMedicalContract != null ? listMedicalContract.FirstOrDefault(p => p.ID == imp_mest.MEDICAL_CONTRACT_ID) : null;

                    //tổng tiền
                    if (!expMestIds.Contains(impMestMedicine.IMP_MEST_ID))
                    {
                        expMestIds.Add(impMestMedicine.IMP_MEST_ID);
                        rdo.TOTAL_PRICE = (deliver.DOCUMENT_PRICE ?? 0)
                            - listImpMestMedicine.Where(o => o.IMP_MEST_ID == impMestMedicine.IMP_MEST_ID).Sum(s => s.IMP_PRICE * (1 + s.IMP_VAT_RATIO) * s.AMOUNT)
                            - listImpMestMaterial.Where(o => o.IMP_MEST_ID == impMestMedicine.IMP_MEST_ID).Sum(s => s.IMP_PRICE * (1 + s.IMP_VAT_RATIO) * s.AMOUNT)
                            + (impMestMedicine.IMP_PRICE * (1 + impMestMedicine.IMP_VAT_RATIO) * impMestMedicine.AMOUNT);
                    }
                    else
                    {

                        rdo.TOTAL_PRICE = (impMestMedicine.IMP_PRICE * (1 + impMestMedicine.IMP_VAT_RATIO) * impMestMedicine.AMOUNT);
                    }
                    var medicinePatys = listMedicinePaty.Where(o => o.MEDICINE_ID == impMestMedicine.MEDICINE_ID).ToList();
                    //số đấu thầu
                    rdo.BID_NUMBER = impMestMedicine.BID_NUMBER;
                    //người giao hàng
                    rdo.DELIVERER = deliver.DELIVERER;
                    //ngày cấp tài liệu
                    rdo.DOCUMENT_DATE = Inventec.Common.DateTime.Convert.TimeNumberToDateString(deliver.DOCUMENT_DATE ?? 0);
                    //số hiệu tài liệu
                    rdo.DOCUMENT_NUMBER = deliver.DOCUMENT_NUMBER ?? "----";
                    //bản ghi giao dịch
                    rdo.RECORDING_TRANSACTION = impMestMedicine.RECORDING_TRANSACTION;
                    //mã nhà cung cấp
                    var supplier = listSupplier.FirstOrDefault(o => o.ID == deliver.SUPPLIER_ID) ?? new HIS_SUPPLIER();
                    rdo.SUPPLIER_CODE = impMestMedicine.SUPPLIER_CODE ?? supplier.SUPPLIER_CODE;
                    rdo.SUPPLIER_NAME = impMestMedicine.SUPPLIER_NAME ?? supplier.SUPPLIER_NAME;
                    //số gói
                    rdo.PACKAGE_NUMBER = impMestMedicine.PACKAGE_NUMBER;
                    //tên loại gói
                    rdo.PACKING_TYPE_NAME = impMestMedicine.PACKING_TYPE_NAME;
                    //thuế vat
                    rdo.IMP_VAT = impMestMedicine.IMP_VAT_RATIO;
                    // giá nhập trước thuế
                    rdo.IMP_PRICE_BEFORE_VAT = impMestMedicine.IMP_PRICE;
                    //giá văn kiện
                    rdo.DOCUMENT_PRICE = impMestMedicine.DOCUMENT_PRICE;
                    rdo.VIR_TOTAL_PRICE = impMestMedicine.IMP_PRICE * rdo.AMOUNT;
                    //Thuế
                    rdo.VAT = impMestMedicine.VAT_RATIO ?? 0;
                    //Tên nguốn nhập
                    rdo.IMP_SOURCE_NAME = dicMedicine.ContainsKey(impMestMedicine.MEDICINE_ID)
                        && dicImpSource.ContainsKey(dicMedicine[impMestMedicine.MEDICINE_ID].IMP_SOURCE_ID ?? 0) ? dicImpSource[dicMedicine[impMestMedicine.MEDICINE_ID].IMP_SOURCE_ID ?? 0].IMP_SOURCE_NAME : "Khác";

                    //số hiệu đăng ký
                    rdo.REGISTER_NUMBER = impMestMedicine.REGISTER_NUMBER;
                    //rdo.MEDI_CONTRACT_CODE = "Số hợp đồng: " + string.Join(",", checkContract);

                    //if (checkContract != null)
                    //{
                    //    rdo.MEDI_CONTRACT_CODE = "Số hợp đồng: " + checkContract.MEDICAL_CONTRACT_NAME;
                    //}
                    if (checkContraindication != null)
                    {
                        //CHỐNG CHỈ ĐỊNH
                        rdo.CONTRAINDICATION = checkContraindication.CONTRAINDICATION ?? "";
                        //là vắc xin
                        rdo.IS_VACCINE = checkContraindication.IS_VACCINE;
                        //là thực phẩm chức năng
                        rdo.IS_FUNCTIONAL_FOOD = checkContraindication.IS_FUNCTIONAL_FOOD;
                    }

                    //ngày nhập
                    rdo.IMP_TIME = impMestMedicine.IMP_TIME != null ? Inventec.Common.DateTime.Convert.TimeNumberToDateString((long)impMestMedicine.IMP_TIME) : "";
                    rdo.TIME_IMP = impMestMedicine.IMP_TIME;
                    //TÊN NHÀ SẢN XUẤT
                    //mã phiếu nhập
                    rdo.IMP_MEST_CODE = impMestMedicine.IMP_MEST_CODE;
                    rdo.MANUFACTURER_NAME = impMestMedicine.MANUFACTURER_NAME;
                    rdo.MEDICINE_TYPE_CODE = impMestMedicine.MEDICINE_TYPE_CODE;
                    rdo.MEDICINE_TYPE_NAME = impMestMedicine.MEDICINE_TYPE_NAME;
                    rdo.CONCENTRA = impMestMedicine.CONCENTRA;
                    rdo.ACTIVE_INGR_BHYT_NAME = impMestMedicine.ACTIVE_INGR_BHYT_NAME;
                    rdo.NATIONAL_NAME = impMestMedicine.NATIONAL_NAME;
                    rdo.SERVICE_UNIT_NAME = impMestMedicine.SERVICE_UNIT_NAME;
                    rdo.PACKAGE_NUMBER = impMestMedicine.PACKAGE_NUMBER;
                    rdo.EXPIRED_DATE = impMestMedicine.EXPIRED_DATE != null ? Inventec.Common.DateTime.Convert.TimeNumberToDateString((long)impMestMedicine.EXPIRED_DATE) : "";
                    rdo.IMP_PRICE = impMestMedicine.IMP_PRICE * (1 + (impMestMedicine.IMP_VAT_RATIO));
                    rdo.PRICE = string.Join(";", (listMedicinePaty ?? new List<HIS_MEDICINE_PATY>()).Where(o => o.MEDICINE_ID == impMestMedicine.MEDICINE_ID).Distinct().Select(p => p.EXP_PRICE.ToString()).ToList());
                    rdo.AMOUNT = impMestMedicine.AMOUNT;
                    rdo.EXP_PRICE = string.Join(",", (medicinePatys ?? new List<HIS_MEDICINE_PATY>()).Select(o => o.EXP_PRICE).ToList());

                    var medicineType = listMedicineType.FirstOrDefault(o => o.ID == impMestMedicine.MEDICINE_TYPE_ID);
                    if (medicineType != null)
                    {
                        var parent = listMedicineType.FirstOrDefault(o => o.ID == medicineType.PARENT_ID);
                        if (parent != null)
                        {
                            rdo.PARENT_ID = parent.ID;
                            rdo.PARENT_CODE = parent.MEDICINE_TYPE_CODE;
                            rdo.PARENT_NAME = parent.MEDICINE_TYPE_NAME;
                        }
                    }
                    ListMedicineRdo.Add(rdo);
                }
            }

            //Vật tư
            if (IsNotNullOrEmpty(listImpMestMaterial))
            {
                foreach (var impMestMaterial in listImpMestMaterial)
                {
                    var deliver = listManuImpMest.FirstOrDefault(o => o.ID == impMestMaterial.IMP_MEST_ID) ?? new V_HIS_IMP_MEST();
                    Mrs00282RDO rdo = new Mrs00282RDO();
                    rdo.impMestMaterial = impMestMaterial;
                    //mã vât liệu
                    rdo.MATERIAL_TYPE_CODE = impMestMaterial.MATERIAL_TYPE_CODE;
                    //tên vật liệu
                    rdo.MATERIAL_TYPE_NAME = impMestMaterial.MATERIAL_TYPE_NAME;
                    //tên hoathj chất
                    rdo.ACTIVE_INGR_BHYT_NAME = rdo.ACTIVE_INGR_BHYT_NAME;
                    //quốc gia
                    rdo.NATIONAL_NAME = impMestMaterial.NATIONAL_NAME;
                    //đơn vị
                    rdo.SERVICE_UNIT_NAME = impMestMaterial.SERVICE_UNIT_NAME;
                    //giá nhập
                    rdo.IMP_PRICE = impMestMaterial.IMP_PRICE * (1 + (impMestMaterial.IMP_VAT_RATIO));
                    //số lượng
                    rdo.AMOUNT = impMestMaterial.AMOUNT;// số lượng

                    //tổng tiền
                    if (!expMestIds.Contains(impMestMaterial.IMP_MEST_ID))
                    {
                        expMestIds.Add(impMestMaterial.IMP_MEST_ID);
                        rdo.TOTAL_PRICE = (deliver.DOCUMENT_PRICE ?? 0)
                            - listImpMestMedicine.Where(o => o.IMP_MEST_ID == impMestMaterial.IMP_MEST_ID).Sum(s => s.IMP_PRICE * (1 + s.IMP_VAT_RATIO) * s.AMOUNT)
                            - listImpMestMaterial.Where(o => o.IMP_MEST_ID == impMestMaterial.IMP_MEST_ID).Sum(s => s.IMP_PRICE * (1 + s.IMP_VAT_RATIO) * s.AMOUNT)
                            + (impMestMaterial.IMP_PRICE * (1 + impMestMaterial.IMP_VAT_RATIO) * impMestMaterial.AMOUNT);
                    }
                    else
                    {

                        rdo.TOTAL_PRICE = (impMestMaterial.IMP_PRICE * (1 + impMestMaterial.IMP_VAT_RATIO) * impMestMaterial.AMOUNT);
                    }
                    rdo.VIR_TOTAL_PRICE = Math.Round(impMestMaterial.IMP_PRICE * rdo.AMOUNT);

                    rdo.BID_NUMBER = impMestMaterial.BID_NUMBER;
                    // người giao
                    rdo.DELIVERER = deliver.DELIVERER;
                    //hạn sử dụng
                    rdo.EXPIRED_DATE = impMestMaterial.EXPIRED_DATE != null ? Inventec.Common.DateTime.Convert.TimeNumberToDateString(impMestMaterial.EXPIRED_DATE ?? 0) : "";
                    //hàm lượng
                    rdo.IMP_MEST_CODE = impMestMaterial.IMP_MEST_CODE;
                    //Ngay lap chung tu nha cung cap
                    rdo.DOCUMENT_DATE = Inventec.Common.DateTime.Convert.TimeNumberToDateString(deliver.DOCUMENT_DATE ?? 0);

                    rdo.DOCUMENT_TIME = deliver.DOCUMENT_DATE ?? 99999999999999;

                    rdo.DOCUMENT_NUMBER = deliver.DOCUMENT_NUMBER ?? "----";

                    rdo.IMP_TIME = impMestMaterial.IMP_TIME != null ? Inventec.Common.DateTime.Convert.TimeNumberToTimeString(impMestMaterial.IMP_TIME ?? 0) : "";

                    rdo.TIME_IMP = impMestMaterial.IMP_TIME;

                    rdo.MANUFACTURER_NAME = impMestMaterial.MANUFACTURER_NAME;
                    //bản ghi
                    rdo.RECORDING_TRANSACTION = impMestMaterial.RECORDING_TRANSACTION;
                    //mã nhà cung cấp
                    var supplier = listSupplier.FirstOrDefault(o => o.ID == deliver.SUPPLIER_ID) ?? new HIS_SUPPLIER();
                    rdo.SUPPLIER_CODE = impMestMaterial.SUPPLIER_CODE ?? supplier.SUPPLIER_CODE;
                    rdo.SUPPLIER_NAME = impMestMaterial.SUPPLIER_NAME ?? supplier.SUPPLIER_NAME;

                    //lô sản suất
                    rdo.PACKAGE_NUMBER = impMestMaterial.PACKAGE_NUMBER;

                    rdo.PACKING_TYPE_NAME = impMestMaterial.PACKING_TYPE_NAME;



                    rdo.IMP_VAT = impMestMaterial.IMP_VAT_RATIO;

                    rdo.IMP_PRICE_BEFORE_VAT = impMestMaterial.IMP_PRICE;

                    rdo.DOCUMENT_PRICE = impMestMaterial.DOCUMENT_PRICE;

                    var materialPatys = listMaterialPaty.Where(o => o.MATERIAL_ID == impMestMaterial.MATERIAL_ID).ToList();
                    rdo.EXP_PRICE = string.Join(",", (materialPatys ?? new List<HIS_MATERIAL_PATY>()).Select(o => o.EXP_PRICE).ToList());//giá xuất
                    rdo.PRICE = string.Join(";", (listMaterialPaty ?? new List<HIS_MATERIAL_PATY>()).Where(o => o.MATERIAL_ID == impMestMaterial.MATERIAL_ID).Distinct().Select(p => p.EXP_PRICE.ToString()).ToList());
                    // thuế vat
                    rdo.VAT = impMestMaterial.VAT_RATIO ?? 0;



                    rdo.IMP_SOURCE_NAME = dicMaterial.ContainsKey(impMestMaterial.MATERIAL_ID)

                        && dicImpSource.ContainsKey(dicMaterial[impMestMaterial.MATERIAL_ID].IMP_SOURCE_ID ?? 0) ? dicImpSource[dicMaterial[impMestMaterial.MATERIAL_ID].IMP_SOURCE_ID ?? 0].IMP_SOURCE_NAME : "Khác";


                    var checkContract = listMedicalContract.Where(o => o.SUPPLIER_ID == impMestMaterial.SUPPLIER_ID).Select(P => P.MEDICAL_CONTRACT_CODE).ToList();
                    rdo.MEDI_CONTRACT_CODE = "Số hợp đồng: " + string.Join(",", checkContract);
                    var materialType = listMaterialType.FirstOrDefault(o => o.ID == impMestMaterial.MATERIAL_TYPE_ID);
                    if (materialType != null)
                    {
                        rdo.IS_CHEMICAL_SUBSTANCE = materialType.IS_CHEMICAL_SUBSTANCE ?? 0;
                        var parent = listMaterialType.FirstOrDefault(o => o.ID == materialType.PARENT_ID);
                        if (parent != null)
                        {
                            rdo.PARENT_ID = parent.ID;
                            rdo.PARENT_CODE = parent.MATERIAL_TYPE_CODE;
                            rdo.PARENT_NAME = parent.MATERIAL_TYPE_NAME;
                        }
                    }
                    ListMaterialRdo.Add(rdo);
                }
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (((Mrs00282Filter)reportFilter).TIME_FROM.HasValue)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00282Filter)reportFilter).TIME_FROM.Value));
                //+ Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)((Mrs00282Filter)reportFilter).DOCUMENT_DATE_FROM));
            }

            if (((Mrs00282Filter)reportFilter).TIME_TO.HasValue)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00282Filter)reportFilter).TIME_TO.Value));
                //+ Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)((Mrs00282Filter)reportFilter).DOCUMENT_DATE_TO));
            }

            if (((Mrs00282Filter)reportFilter).DOCUMENT_DATE_FROM.HasValue)
            {
                dicSingleTag.Add("DOCUMENT_DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00282Filter)reportFilter).DOCUMENT_DATE_FROM.Value));
            }

            if (((Mrs00282Filter)reportFilter).DOCUMENT_DATE_TO.HasValue)
            {
                dicSingleTag.Add("DOCUMENT_DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00282Filter)reportFilter).DOCUMENT_DATE_TO.Value));
            }

            if (((Mrs00282Filter)reportFilter).MEDI_STOCK_ID.HasValue)
            {
                var x = new HisMediStockManager().Get(new HisMediStockFilterQuery()).Where(o => ((Mrs00282Filter)reportFilter).MEDI_STOCK_ID == o.ID).ToList();
                if (IsNotNullOrEmpty(x))
                    dicSingleTag.Add("MEDI_STOCK_NAME", x.First());
            }

            List<Mrs00282RDO> listRdo = new List<Mrs00282RDO>();
            listRdo.AddRange(ListMedicineRdo);
            listRdo.AddRange(ListMaterialRdo);
            listRdo = listRdo.GroupBy(o => o.IMP_SOURCE_NAME).Select(p => new Mrs00282RDO { IMP_SOURCE_NAME = p.First().IMP_SOURCE_NAME, TOTAL_PRICE = p.Sum(q => q.TOTAL_PRICE) }).ToList();


            objectTag.AddObjectData(store, "Medicine", ListMedicineRdo.OrderBy(o => o.DOCUMENT_TIME).ThenBy(p => p.DOCUMENT_NUMBER).ToList());
            objectTag.AddObjectData(store, "Material", ListMaterialRdo.OrderBy(o => o.DOCUMENT_TIME).ThenBy(p => p.DOCUMENT_NUMBER).ToList());
            objectTag.AddObjectData(store, "Blood", ListBloodRdo.OrderBy(o => o.DOCUMENT_TIME).ThenBy(p => p.DOCUMENT_NUMBER).ToList());
            objectTag.AddObjectData(store, "ImpSource", listRdo);

            List<Mrs00282RDO> listMedicineGoupImpTime = new List<Mrs00282RDO>();
            listMedicineGoupImpTime.AddRange(ListMedicineRdo);
            listMedicineGoupImpTime.AddRange(ListMaterialRdo);
            listMedicineGoupImpTime = listMedicineGoupImpTime.GroupBy(g => g.IMP_TIME).Select(p => new Mrs00282RDO { IMP_TIME = p.First().IMP_TIME, TOTAL_PRICE = p.Sum(q => q.TOTAL_PRICE), AMOUNT = p.Sum(q => q.AMOUNT), VIR_TOTAL_PRICE = p.Sum(q => q.VIR_TOTAL_PRICE), TIME_IMP = p.First().TIME_IMP }).ToList();
            objectTag.AddObjectData(store, "ImpTime", listMedicineGoupImpTime.OrderBy(o => o.TIME_IMP).ToList());
            objectTag.AddRelationship(store, "ImpTime", "Medicine", "IMP_TIME", "IMP_TIME");
            objectTag.AddRelationship(store, "ImpTime", "Material", "IMP_TIME", "IMP_TIME");
            objectTag.SetUserFunction(store, "FuncSameTitleColDocumentNumber", new CustomerFuncMergeSameData());
            objectTag.SetUserFunction(store, "FuncSameTitleColDocumentDate", new CustomerFuncMergeSameData());
            objectTag.SetUserFunction(store, "FuncSameTitleColImpDate", new CustomerFuncMergeSameData());


        }
    }

    class CustomerFuncMergeSameData : TFlexCelUserFunction
    {
        string SameType;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length <= 0)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            bool result = false;
            try
            {
                string ServiceId = Convert.ToString(parameters[0]);

                if (ServiceId != null && ServiceId != "")
                {
                    if (SameType == ServiceId)
                    {
                        return true;
                    }
                    else
                    {
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
}
