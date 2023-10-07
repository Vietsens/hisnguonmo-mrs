using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMestPeriodMedi;
using MOS.MANAGER.HisMestPeriodMate;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using AutoMapper; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Common.Repository; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisBranch; 
using MOS.MANAGER.HisDepartment; 
using MOS.MANAGER.HisExecuteRoom; 
using MOS.MANAGER.HisPatientTypeAlter; 
using MOS.MANAGER.HisReportTypeCat; 
using MOS.MANAGER.HisRoom; 
using MOS.MANAGER.HisSereServ; 
using MOS.MANAGER.HisServiceReq; 
using MOS.MANAGER.HisServiceRetyCat; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Reflection; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00427
{
    class Mrs00427Processor : AbstractProcessor
    {
        Mrs00427Filter castFilter = null; 
        List<Mrs00427RDO> ListRdo = new List<Mrs00427RDO>(); 

        List<V_HIS_MEDICINE> listMedicines = new List<V_HIS_MEDICINE>(); 
        List<V_HIS_MATERIAL> listMaterials = new List<V_HIS_MATERIAL>(); 

        List<V_HIS_IMP_MEST> listImpMests = new List<V_HIS_IMP_MEST>(); 
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines = new List<V_HIS_IMP_MEST_MEDICINE>(); 
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterials = new List<V_HIS_IMP_MEST_MATERIAL>(); 

        List<V_HIS_EXP_MEST> listExpMests = new List<V_HIS_EXP_MEST>(); 
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>(); 
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterials = new List<V_HIS_EXP_MEST_MATERIAL>(); 

        List<V_HIS_MEDI_STOCK_PERIOD> listMediStockPeriods = new List<V_HIS_MEDI_STOCK_PERIOD>(); 
        List<V_HIS_MEST_PERIOD_MEDI> listMestPeriodMedis = new List<V_HIS_MEST_PERIOD_MEDI>(); 
        List<V_HIS_MEST_PERIOD_MATE> listMestPeriodMates = new List<V_HIS_MEST_PERIOD_MATE>(); 

        List<V_HIS_EXP_MEST> listPrescriptions = new List<V_HIS_EXP_MEST>(); 

        Dictionary<long, PropertyInfo> dicExpAmountType = new Dictionary<long, PropertyInfo>(); 
        Dictionary<long, PropertyInfo> dicImpAmountType = new Dictionary<long, PropertyInfo>(); 


        public string SERVICE_TYPE_NAME { get;  set;  }
        public string SERVICE_TYPE_CODE { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }

        public string MEDI_STOCK_NAME = ""; 
        public string MEDI_STOCK_CODE = ""; 



        public Mrs00427Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
        public override Type FilterType()
        {
            return typeof(Mrs00427Filter); 
        }


        protected override bool GetData()
        {
            CommonParam paramGet = new CommonParam(); 
            this.castFilter = ((Mrs00427Filter)reportFilter); 
            bool result = true; 
            try
            {
                var skip = 0; 
                HisMediStockViewFilterQuery mediStockFilter = new HisMediStockViewFilterQuery(); 
                mediStockFilter.ID = this.castFilter.MEDI_STOCK_ID; 
                var listMediStocks = new MOS.MANAGER.HisMediStock.HisMediStockManager(paramGet).GetView(mediStockFilter); 
                if (IsNotNullOrEmpty(listMediStocks))
                {
                    MEDI_STOCK_NAME = listMediStocks.First().MEDI_STOCK_NAME; 
                    MEDI_STOCK_CODE = listMediStocks.First().MEDI_STOCK_CODE; 
                }
                // Lay phieu nhap
                HisImpMestViewFilterQuery impMestFilter = new HisImpMestViewFilterQuery(); 
                impMestFilter.IMP_TIME_FROM = this.castFilter.TIME_FROM; 
                impMestFilter.IMP_TIME_TO = this.castFilter.TIME_TO; 
                impMestFilter.MEDI_STOCK_ID = this.castFilter.MEDI_STOCK_ID; 
                impMestFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE; 
                listImpMests = new MOS.MANAGER.HisImpMest.HisImpMestManager(paramGet).GetView(impMestFilter); 
                var listImpMestIds = listImpMests.Select(s => s.ID).ToList(); 
                #region thuoc trong phieu
                //phieu nhap thuoc
                if (IsNotNull(this.castFilter.MEDICINE_TYPE_ID))
                {
                    while (listImpMestIds.Count - skip > 0)
                    {
                        var listImpMestId = listImpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        HisImpMestMedicineViewFilterQuery impMestMedicineFilter = new HisImpMestMedicineViewFilterQuery(); 
                        impMestMedicineFilter.IMP_MEST_IDs = listImpMestId; 
                        impMestMedicineFilter.MEDICINE_TYPE_ID = this.castFilter.MEDICINE_TYPE_ID; 
                        impMestMedicineFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE; 
                        if (IsNotNull(this.castFilter.MEDICINE_ID))
                        {
                            impMestMedicineFilter.MEDICINE_ID = this.castFilter.MEDICINE_ID; 
                        }
                        var listImpMestMedicine = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(impMestMedicineFilter); 

                        listImpMestMedicines.AddRange(listImpMestMedicine); 
                    }
                }

                //lay thong tin thuoc nhap
                var listImpMedicineIds = listImpMestMedicines.Where(w => w.MEDICINE_ID != null).Select(s => s.MEDICINE_ID).Distinct().ToList(); 
                skip = 0; 
                while (listImpMedicineIds.Count - skip > 0)
                {
                    var listMedicineId = listImpMedicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisMedicineViewFilterQuery medicineFilter = new HisMedicineViewFilterQuery(); 
                    medicineFilter.IDs = listMedicineId; 
                    var listMedicine = new MOS.MANAGER.HisMedicine.HisMedicineManager(paramGet).GetView(medicineFilter); 
                    listMedicines.AddRange(listMedicine); 
                }
                #endregion
                #region vat tu trong phieu
                if (IsNotNull(this.castFilter.MATERIAL_TYPE_ID))
                {
                    skip = 0; 
                    while (listImpMestIds.Count - skip > 0)
                    {
                        var listImpMestId = listImpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        HisImpMestMaterialViewFilterQuery impMestMaterialFilter = new HisImpMestMaterialViewFilterQuery(); 
                        impMestMaterialFilter.IMP_MEST_IDs = listImpMestId; 
                        impMestMaterialFilter.MATERIAL_TYPE_ID = this.castFilter.MATERIAL_TYPE_ID; 
                        impMestMaterialFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE; 
                        if (IsNotNull(this.castFilter.MATERIAL_ID))
                        {
                            impMestMaterialFilter.MATERIAL_ID = this.castFilter.MATERIAL_ID; 
                        }
                        var listImpMestMaterial = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(impMestMaterialFilter); 

                        listImpMestMaterials.AddRange(listImpMestMaterial); 
                    }
                }
                //lay thong tin vat tu nhap
                var listImpMaterialIds = listImpMestMaterials.Where(w => w.MATERIAL_ID != null).Select(s => s.MATERIAL_ID).ToList(); 
                skip = 0; 
                while (listImpMaterialIds.Count - skip > 0)
                {
                    var listMaterialId = listImpMaterialIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisMaterialViewFilterQuery materialFilter = new HisMaterialViewFilterQuery(); 
                    materialFilter.IDs = listMaterialId; 
                    var listMaterial = new MOS.MANAGER.HisMaterial.HisMaterialManager(paramGet).GetView(materialFilter); 
                    listMaterials.AddRange(listMaterial); 
                }
                #endregion
                // Lay phieu xuat
                HisExpMestViewFilterQuery expMestFilter = new HisExpMestViewFilterQuery(); 
                expMestFilter.FINISH_DATE_FROM = this.castFilter.TIME_FROM; 
                expMestFilter.FINISH_DATE_TO = this.castFilter.TIME_TO; 
                expMestFilter.MEDI_STOCK_ID = this.castFilter.MEDI_STOCK_ID; 
                expMestFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE; 
                listExpMests = new MOS.MANAGER.HisExpMest.HisExpMestManager(paramGet).GetView(expMestFilter); 
                #region thuoc trong phieu
                //lay phieu xuat thuoc
                var listExpMestIds = listExpMests.Select(s => s.ID).ToList(); 
                if (IsNotNull(this.castFilter.MEDICINE_TYPE_ID))
                {
                    skip = 0; 
                    while (listExpMestIds.Count - skip > 0)
                    {
                        var listExpMestId = listExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        HisExpMestMedicineViewFilterQuery expMestMedicineFilter = new HisExpMestMedicineViewFilterQuery(); 
                        expMestMedicineFilter.MEDICINE_TYPE_ID = this.castFilter.MEDICINE_TYPE_ID; 
                        expMestMedicineFilter.EXP_MEST_IDs = listExpMestId; 
                        if (IsNotNull(this.castFilter.MEDICINE_ID))
                        {
                            expMestMedicineFilter.MEDICINE_ID = this.castFilter.MEDICINE_ID; 
                        }
                        expMestMedicineFilter.IS_EXPORT = true;
                        var listExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(expMestMedicineFilter); 

                        listExpMestMedicines.AddRange(listExpMestMedicine); 
                    }
                }
                //lay thong tin thuoc xuat
                var listExpMedicineIds = listExpMestMedicines.Where(w => w.MEDICINE_ID != null).Select(s => s.MEDICINE_ID ?? 0).ToList(); 
                skip = 0; 
                while (listExpMedicineIds.Count - skip > 0)
                {
                    var listMedicineId = listExpMedicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisMedicineViewFilterQuery medicineFilter = new HisMedicineViewFilterQuery(); 
                    medicineFilter.IDs = listMedicineId; 
                    var listMedicine = new MOS.MANAGER.HisMedicine.HisMedicineManager(paramGet).GetView(medicineFilter); 
                    listMedicines.AddRange(listMedicine); 

                }
                listMedicines = listMedicines.Distinct().ToList(); 
                #endregion
                #region vat tu trong phieu
                //lay phieu xuat vat tu
                if (IsNotNull(this.castFilter.MATERIAL_TYPE_ID))
                {
                    skip = 0; 
                    while (listExpMestIds.Count - skip > 0)
                    {
                        var listExpMestId = listExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        HisExpMestMaterialViewFilterQuery expMestMAterialFilter = new HisExpMestMaterialViewFilterQuery(); 
                        expMestMAterialFilter.MATERIAL_TYPE_ID = this.castFilter.MATERIAL_TYPE_ID; 
                        expMestMAterialFilter.EXP_MEST_IDs = listExpMestId; 
                        if (IsNotNull(this.castFilter.MATERIAL_ID))
                        {
                            expMestMAterialFilter.MATERIAL_ID = this.castFilter.MATERIAL_ID;
                            expMestMAterialFilter.IS_EXPORT = true;
                        }
                        var listExpMestMaterial = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(paramGet).GetView(expMestMAterialFilter); 

                        listExpMestMaterials.AddRange(listExpMestMaterial); 
                    }
                }
                //lay thong tin vat tu xuat
                var listExpMaterialIds = listExpMestMaterials.Where(w => w.MATERIAL_ID != null).Select(s => s.MATERIAL_ID ?? 0).ToList(); 
                skip = 0; 
                while (listExpMaterialIds.Count - skip > 0)
                {
                    var listMaterialId = listExpMaterialIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisMaterialViewFilterQuery materialFilter = new HisMaterialViewFilterQuery(); 
                    materialFilter.IDs = listMaterialId; 
                    var listMaterial = new MOS.MANAGER.HisMaterial.HisMaterialManager(paramGet).GetView(materialFilter); 
                    listMaterials.AddRange(listMaterial); 

                }
                listMaterials = listMaterials.Distinct().ToList(); 
                #endregion
                #region thong tin in
                // Lay thong tin loai thuoc
                if (IsNotNull(this.castFilter.MEDICINE_TYPE_ID))
                {
                    HisMedicineTypeViewFilterQuery medicineTypeViewFilter = new HisMedicineTypeViewFilterQuery(); 
                    medicineTypeViewFilter.ID = this.castFilter.MEDICINE_TYPE_ID; 
                    var listMedicineTypes = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(param).GetView(medicineTypeViewFilter); 
                    if (IsNotNullOrEmpty(listMedicineTypes))
                    {
                        SERVICE_TYPE_NAME = listMedicineTypes.First().MEDICINE_TYPE_NAME; 
                        SERVICE_TYPE_CODE = listMedicineTypes.First().MEDICINE_TYPE_CODE; 
                        SERVICE_UNIT_NAME = listMedicineTypes.First().SERVICE_UNIT_NAME; 
                    }
                }
                // Lay thong tin vat tu
                if (IsNotNull(this.castFilter.MATERIAL_TYPE_ID))
                {
                    HisMaterialTypeViewFilterQuery materialTypeViewFilter = new HisMaterialTypeViewFilterQuery(); 
                    materialTypeViewFilter.ID = this.castFilter.MATERIAL_TYPE_ID; 
                    var listMaterialTypes = new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager(param).GetView(materialTypeViewFilter); 

                    if (IsNotNullOrEmpty(listMaterialTypes))
                    {
                        SERVICE_TYPE_NAME = listMaterialTypes.First().MATERIAL_TYPE_NAME; 
                        SERVICE_TYPE_CODE = listMaterialTypes.First().MATERIAL_TYPE_CODE; 
                        SERVICE_UNIT_NAME = listMaterialTypes.First().SERVICE_UNIT_NAME; 
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

        private void makeRdo()
        {
            //Danh sach loai nhap, loai xuat
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE typeExp = new IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE(); 
            PropertyInfo[] piExpAmount = Properties.Get<IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE>(); 
            Dictionary<string, long> dicExpMestType = piExpAmount.ToDictionary(o => string.Format("{0}_EXP_AMOUNT", o.Name), o => (long)o.GetValue(typeExp));  

            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE typeImp = new IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE(); 
            PropertyInfo[] piImp = Properties.Get<IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE>();
            Dictionary<string, long> dicImpMestType = piImp.ToDictionary(o => string.Format("{0}_IMP_AMOUNT", o.Name), o => (long)o.GetValue(typeImp));  

            //Danh sach loai SL nhap, loai SL xuat
            PropertyInfo[] piAmount = Properties.Get<Mrs00427RDO>(); 

            foreach (var item in piAmount)
            {
                if (dicImpMestType.ContainsKey(item.Name))
                {
                    if (!dicImpAmountType.ContainsKey(dicImpMestType[item.Name])) dicImpAmountType[dicImpMestType[item.Name]] = item; 
                }
                else if (dicExpMestType.ContainsKey(item.Name))
                {
                    if (!dicExpAmountType.ContainsKey(dicExpMestType[item.Name])) dicExpAmountType[dicExpMestType[item.Name]] = item; 
                }
            }

        }


        protected override bool ProcessData()
        {
            CommonParam paramGet = new CommonParam(); 
            this.castFilter = ((Mrs00427Filter)reportFilter); 
            bool result = true; 
            try
            {
                // Lay thong tin theo phieu nhap
                if (IsNotNullOrEmpty(listImpMests))
                {
                    foreach (var impMest in listImpMests)
                    {
                      
                        #region Thuốc
                        var impMestMedicines = listImpMestMedicines.Where(w => w.IMP_MEST_ID == impMest.ID).ToList(); 
                        if (IsNotNullOrEmpty(impMestMedicines))
                        {
                            foreach (var impMestMedicine in impMestMedicines)
                            {
                                var medicine = listMedicines.Where(w => w.ID == impMestMedicine.MEDICINE_ID).ToList(); 
                                Mrs00427RDO rdo = new Mrs00427RDO(); 
                                rdo.IMP_EXP_TIME = impMest.IMP_TIME; 
                                rdo.IMP_MEST_CODE = impMest.IMP_MEST_CODE; 
                                if (IsNotNullOrEmpty(medicine))
                                {
                                    rdo.PACKAGE_NUMBER = medicine.FirstOrDefault().PACKAGE_NUMBER; 
                                    rdo.EXPIRED_DATE = medicine.FirstOrDefault().EXPIRED_DATE; 
                                    rdo.BEGIN_AMOUNT = tinhTonDauKy(impMest.IMP_TIME - 1 ?? 0, this.castFilter.MEDI_STOCK_ID, medicine.FirstOrDefault().ID, -1); 
                                }

                                rdo.DESCRIPTION = impMest.IMP_MEST_TYPE_NAME + " từ "  + impMest.REQ_DEPARTMENT_NAME; 
                               
                                rdo.IMP_AMOUNT = impMestMedicine.AMOUNT; 

                                ListRdo.Add(rdo); 
                            }
                        }

                        #endregion

                        #region vật tư
                        var impMestMaterials = listImpMestMaterials.Where(w => w.IMP_MEST_ID == impMest.ID).ToList(); 
                        if (IsNotNullOrEmpty(impMestMaterials))
                        {
                            foreach (var impMestMaterial in impMestMaterials)
                            {
                                var material = listMaterials.Where(w => w.ID == impMestMaterial.MATERIAL_ID).ToList(); 
                                Mrs00427RDO rdo = new Mrs00427RDO(); 
                                rdo.IMP_EXP_TIME = impMest.IMP_TIME; 
                                rdo.IMP_MEST_CODE = impMest.IMP_MEST_CODE; 
                                    rdo.PACKAGE_NUMBER = material.FirstOrDefault().PACKAGE_NUMBER; 
                                    rdo.EXPIRED_DATE = material.FirstOrDefault().EXPIRED_DATE; 
                                    rdo.BEGIN_AMOUNT = tinhTonDauKy(impMest.IMP_TIME - 1 ?? 0, this.castFilter.MEDI_STOCK_ID, -1, material.FirstOrDefault().ID); 

                                    rdo.DESCRIPTION = impMest.IMP_MEST_TYPE_NAME + " từ " + impMest.REQ_DEPARTMENT_NAME; 
                                
                                rdo.IMP_AMOUNT = impMestMaterial.AMOUNT; 

                                ListRdo.Add(rdo); 
                            }
                        }
                        #endregion
                    }
                }

                // lay thong tin phieu xuat
                if (IsNotNullOrEmpty(listExpMests))
                {
                    foreach (var expMest in listExpMests)
                    {
                        //var prescription = listPrescriptions.Where(w => w.EXP_MEST_ID == expMest.ID && w.EXP_MEST_TYPE_ID == expMest.EXP_MEST_TYPE_ID).ToList(); 
                        //var depaExpMest = listDepaExpMests.Where(w => w.EXP_MEST_ID == expMest.ID && w.EXP_MEST_TYPE_ID == expMest.EXP_MEST_TYPE_ID).ToList(); 
                        //var chsmExpMest = listChmsExpMests.Where(w => w.EXP_MEST_ID == expMest.ID && w.EXP_MEST_TYPE_ID == expMest.EXP_MEST_TYPE_ID).ToList(); 
                        //var manuExpMest = listManuExpMests.Where(w => w.EXP_MEST_ID == expMest.ID && w.EXP_MEST_TYPE_ID == expMest.EXP_MEST_TYPE_ID).ToList(); 
                        //var expeExpMest = listExpeExpMests.Where(w => w.EXP_MEST_ID == expMest.ID && w.EXP_MEST_TYPE_ID == expMest.EXP_MEST_TYPE_ID).ToList(); 
                        //var lostExpMest = listLostExpMests.Where(w => w.EXP_MEST_ID == expMest.ID && w.EXP_MEST_TYPE_ID == expMest.EXP_MEST_TYPE_ID).ToList(); 
                        //var saleExpMest = listSaleExpMests.Where(w => w.EXP_MEST_ID == expMest.ID && w.EXP_MEST_TYPE_ID == expMest.EXP_MEST_TYPE_ID).ToList(); 
                        //var liquExpMest = listLiquExpMests.Where(w => w.EXP_MEST_ID == expMest.ID && w.EXP_MEST_TYPE_ID == expMest.EXP_MEST_TYPE_ID).ToList(); 
                        //var otherExpMest = listOtherExpMests.Where(w => w.EXP_MEST_ID == expMest.ID && w.EXP_MEST_TYPE_ID == expMest.EXP_MEST_TYPE_ID).ToList(); 
                        #region thuoc
                        var expMestMedicines = listExpMestMedicines.Where(w => w.EXP_MEST_ID == expMest.ID).ToList(); 
                        if (IsNotNullOrEmpty(expMestMedicines))
                        {
                            foreach (var expMestMedicine in expMestMedicines)
                            {
                                var medicine = listMedicines.Where(w => w.ID == expMestMedicine.MEDICINE_ID).ToList(); 

                                Mrs00427RDO rdo = new Mrs00427RDO(); 
                                rdo.IMP_EXP_TIME = expMest.FINISH_TIME; 
                                rdo.EXP_MEST_CODE = expMest.EXP_MEST_CODE; 
                                if (IsNotNullOrEmpty(medicine))
                                {
                                    rdo.PACKAGE_NUMBER = medicine.FirstOrDefault().PACKAGE_NUMBER; 
                                    rdo.EXPIRED_DATE = medicine.FirstOrDefault().EXPIRED_DATE; 
                                    rdo.BEGIN_AMOUNT = tinhTonDauKy(expMest.FINISH_TIME - 1 ?? 0, this.castFilter.MEDI_STOCK_ID, medicine.FirstOrDefault().ID, -1); 
                                }

                                rdo.DESCRIPTION = "Xuất cho " + expMest.REQ_ROOM_NAME + "," + expMest.REQ_DEPARTMENT_NAME; 

                                rdo.EXP_AMOUNT = expMestMedicine.AMOUNT; 

                                ListRdo.Add(rdo); 
                            }
                        }
                        #endregion
                        #region vat tu
                        var expMestMaterials = listExpMestMaterials.Where(w => w.EXP_MEST_ID == expMest.ID).ToList(); 
                        if (IsNotNullOrEmpty(expMestMaterials))
                        {
                            foreach (var expMestMaterial in expMestMaterials)
                            {
                                var material = listMaterials.Where(w => w.ID == expMestMaterial.MATERIAL_ID).ToList(); 

                                Mrs00427RDO rdo = new Mrs00427RDO(); 
                                rdo.IMP_EXP_TIME = expMest.FINISH_TIME; 
                                rdo.EXP_MEST_CODE = expMest.EXP_MEST_CODE; 
                                if (IsNotNullOrEmpty(material))
                                {
                                    rdo.PACKAGE_NUMBER = material.FirstOrDefault().PACKAGE_NUMBER; 
                                    rdo.EXPIRED_DATE = material.FirstOrDefault().EXPIRED_DATE; 
                                    rdo.BEGIN_AMOUNT = tinhTonDauKy(expMest.FINISH_TIME - 1 ?? 0, this.castFilter.MEDI_STOCK_ID, material.FirstOrDefault().ID, -1); 
                                }
                                rdo.DESCRIPTION = "Xuất cho " + expMest.REQ_ROOM_NAME + "," + expMest.REQ_DEPARTMENT_NAME; 
                                rdo.EXP_AMOUNT = expMestMaterial.AMOUNT; 

                                ListRdo.Add(rdo); 
                            }
                        }
                        #endregion
                    }
                }
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
                if (((Mrs00427Filter)reportFilter).TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00427Filter)reportFilter).TIME_FROM)); 
                }
                if (((Mrs00427Filter)reportFilter).TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00427Filter)reportFilter).TIME_TO)); 
                }

                dicSingleTag.Add("SERVICE_TYPE_NAME", SERVICE_TYPE_NAME); 
                dicSingleTag.Add("SERVICE_TYPE_CODE", SERVICE_TYPE_CODE); 
                dicSingleTag.Add("SERVICE_UNIT_NAME", SERVICE_UNIT_NAME); 
                dicSingleTag.Add("DEPARTMENT_NAME", MEDI_STOCK_NAME); 
                dicSingleTag.Add("DEPARTMENT_CODE", MEDI_STOCK_CODE); 

                bool exportSuccess = true; 
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Rdo", ListRdo.OrderBy(o => o.IMP_EXP_TIME).ToList()); 
                //exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Group", ListRdoGroup.OrderBy(s => s.DEPARTMENT_NAME).ToList()); 
                //exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group", "Rdo", "DEPARTMENT_ID", "DEPARTMENT_ID"); 
                exportSuccess = exportSuccess && store.SetCommonFunctions(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        //Tinh ton dau ky thuoc
        private decimal tinhTonDauKy(long ImpExpTime, long medi_stock_id, long medicine_id, long material_id)
        {
            CommonParam paramGet = new CommonParam(); 
            // Kiem ke 
            HisMediStockPeriodViewFilterQuery mediStockPeriodFilter = new HisMediStockPeriodViewFilterQuery(); 
            mediStockPeriodFilter.MEDI_STOCK_ID = this.castFilter.MEDI_STOCK_ID; 
            listMediStockPeriods = new MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodManager(paramGet).GetView(mediStockPeriodFilter); 
            listMediStockPeriods = listMediStockPeriods.Where(w => w.CREATE_TIME < ImpExpTime).OrderByDescending(o => o.CREATE_TIME).ToList(); 
            if (IsNotNullOrEmpty(listMediStockPeriods))
            {
                List<V_HIS_MEST_PERIOD_MEDI> listMestPeriodMedi = null; 
                List<V_HIS_MEST_PERIOD_MATE> listMestPeriodMate = null; 
                decimal result = 0; 
                if (medicine_id > 0)
                {
                    HisMestPeriodMediViewFilterQuery mestPeriodMediFilter = new HisMestPeriodMediViewFilterQuery(); 
                    mestPeriodMediFilter.MEDI_STOCK_PERIOD_ID = listMediStockPeriods.FirstOrDefault().ID; 
                    listMestPeriodMedi = new MOS.MANAGER.HisMestPeriodMedi.HisMestPeriodMediManager(paramGet).GetView(mestPeriodMediFilter); 
                    listMestPeriodMedi = listMestPeriodMedi.Where(w => w.MEDICINE_ID == medicine_id).ToList(); 
                    if (IsNotNullOrEmpty(listMestPeriodMedi))
                    {
                        var slTon = listMestPeriodMedi.FirstOrDefault().AMOUNT; 
                        var slNhap = tinhSLNhap(listMediStockPeriods.FirstOrDefault().CREATE_TIME ?? 0, ImpExpTime, medi_stock_id, medicine_id, -1); 
                        var slXuat = tinhSLXuat(listMediStockPeriods.FirstOrDefault().CREATE_TIME ?? 0, ImpExpTime, medi_stock_id, medicine_id, -1); 

                        result = slTon + slNhap - slXuat; 
                    }
                    else
                    {
                        var slNhap = tinhSLNhap(0, ImpExpTime, medi_stock_id, medicine_id, material_id); 
                        var slXuat = tinhSLXuat(0, ImpExpTime, medi_stock_id, medicine_id, material_id); 
                        result = slNhap - slXuat; 
                    }
                }
                if (material_id > 0)
                {
                    HisMestPeriodMateViewFilterQuery mestPeriodMateFilter = new HisMestPeriodMateViewFilterQuery(); 
                    mestPeriodMateFilter.MEDI_STOCK_PERIOD_ID = listMediStockPeriods.FirstOrDefault().ID; 
                    listMestPeriodMate = new MOS.MANAGER.HisMestPeriodMate.HisMestPeriodMateManager(paramGet).GetView(mestPeriodMateFilter); 
                    listMestPeriodMate = listMestPeriodMate.Where(w => w.MATERIAL_ID == material_id).ToList(); 
                    if (IsNotNullOrEmpty(listMestPeriodMate))
                    {
                        var slTon = listMestPeriodMate.FirstOrDefault().AMOUNT; 
                        var slNhap = tinhSLNhap(listMestPeriodMate.FirstOrDefault().CREATE_TIME ?? 0, ImpExpTime, medi_stock_id, medicine_id, material_id); 
                        var slXuat = tinhSLXuat(listMestPeriodMate.FirstOrDefault().CREATE_TIME ?? 0, ImpExpTime, medi_stock_id, medicine_id, material_id); 

                        result = slTon + slNhap - slXuat; 
                    }
                    else
                    {
                        var slNhap = tinhSLNhap(0, ImpExpTime, medi_stock_id, medicine_id, material_id); 
                        var slXuat = tinhSLXuat(0, ImpExpTime, medi_stock_id, medicine_id, material_id); 
                        result = slNhap - slXuat; 
                    }
                }

                return result; 

            }
            else
            {
                var slNhap = tinhSLNhap(0, ImpExpTime, medi_stock_id, medicine_id, material_id); 
                var slXuat = tinhSLXuat(0, ImpExpTime, medi_stock_id, medicine_id, material_id); 
                var result = slNhap - slXuat; 
                return result; 
            }
            return Convert.ToDecimal(0); 
        }
        //Tinh so luong nhap thuoc
        private decimal tinhSLNhap(long timeFrom, long timeTo, long medi_stock_id, long medicine_id, long material_id)
        {
            CommonParam paramGet = new CommonParam(); 
            decimal result = 0; 
            if (medicine_id > 0)
            {
                HisImpMestMedicineViewFilterQuery impMestMedicineFilter = new HisImpMestMedicineViewFilterQuery(); 
                impMestMedicineFilter.IMP_TIME_FROM = timeFrom; 
                impMestMedicineFilter.IMP_TIME_TO = timeTo; 
                impMestMedicineFilter.MEDI_STOCK_ID = medi_stock_id; 
                impMestMedicineFilter.MEDICINE_ID = medicine_id; 
                impMestMedicineFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE; 
                var listImpMestMedicine = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(impMestMedicineFilter); 
                result = listImpMestMedicine.Sum(su => su.AMOUNT); 
            }
            if (material_id > 0)
            {
                HisImpMestMaterialViewFilterQuery impMestMaterialFilter = new HisImpMestMaterialViewFilterQuery(); 
                impMestMaterialFilter.IMP_TIME_FROM = timeFrom; 
                impMestMaterialFilter.IMP_TIME_TO = timeTo; 
                impMestMaterialFilter.MEDI_STOCK_ID = medi_stock_id; 
                impMestMaterialFilter.MATERIAL_ID = material_id; 
                impMestMaterialFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE; 
                var listImpMestMaterial = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(impMestMaterialFilter); 
                result = listImpMestMaterial.Sum(su => su.AMOUNT); 
            }
            return result; 
        }
        // Tinh so luong xuat thuoc
        private decimal tinhSLXuat(long timeFrom, long timeTo, long medi_stock_id, long medicine_id, long material_id)
        {
            CommonParam paramGet = new CommonParam(); 
            decimal result = 0; 
            if (medicine_id > 0)
            {
                HisExpMestMedicineViewFilterQuery expMestMedicineFilter = new HisExpMestMedicineViewFilterQuery(); 
                expMestMedicineFilter.EXP_TIME_FROM = timeFrom; 
                expMestMedicineFilter.EXP_TIME_TO = timeTo; 
                expMestMedicineFilter.MEDI_STOCK_ID = medi_stock_id; 
                expMestMedicineFilter.MEDICINE_ID = medicine_id;
                expMestMedicineFilter.IS_EXPORT = true;
                var listExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(expMestMedicineFilter); 
                result = listExpMestMedicine.Sum(su => su.AMOUNT); 
            }
            if (material_id > 0)
            {
                HisExpMestMaterialViewFilterQuery expMestMaterialFilter = new HisExpMestMaterialViewFilterQuery(); 
                expMestMaterialFilter.EXP_TIME_FROM = timeFrom; 
                expMestMaterialFilter.EXP_TIME_TO = timeTo; 
                expMestMaterialFilter.MEDI_STOCK_ID = medi_stock_id;
                expMestMaterialFilter.MATERIAL_ID = material_id;
                expMestMaterialFilter.IS_EXPORT = true;
                var listExpMestMaterial = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(paramGet).GetView(expMestMaterialFilter); 
                result = listExpMestMaterial.Sum(su => su.AMOUNT); 
            }

            return result; 
        }
    }

}
