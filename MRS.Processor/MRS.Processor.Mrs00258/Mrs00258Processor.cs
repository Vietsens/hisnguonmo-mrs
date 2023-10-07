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
using MOS.MANAGER.HisMestPeriodBlood;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestBlood;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestBlood;
using MOS.MANAGER.HisBloodType;
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisInvoice; 
using MOS.MANAGER.HisInvoiceDetail; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using Inventec.Common.DateTime; 
using MRS.MANAGER.Config; 
//using MOS.MANAGER.HisDeath; 
using MOS.MANAGER.HisIcd; 
using MOS.MANAGER.HisIcdGroup; 
using MOS.MANAGER.HisPatientTypeAlter; 
using MOS.MANAGER.HisTreatment; 
//using MOS.MANAGER.HisExamServiceReq; 
using MOS.MANAGER.HisSereServ; 
using MOS.MANAGER.HisMaterialBean; 
using FlexCel.Report; 
using MOS.MANAGER.HisMaterialType; 
using MOS.MANAGER.HisMedicineBean; 
using MOS.MANAGER.HisBlood; 
using MOS.MANAGER.HisServiceType; 

namespace MRS.Processor.Mrs00258
{
    public class Mrs00258Processor : AbstractProcessor
    {
        private Mrs00258Filter filter; 

        public List<Mrs00258RDO> listRdo = new List<Mrs00258RDO>(); 
        public List<Mrs00258RDO> listRdoGroup = new List<Mrs00258RDO>(); 

        public List<Mrs00258RDO> listRdoMedicine = new List<Mrs00258RDO>(); 
        public List<Mrs00258RDO> listRdoMaterial = new List<Mrs00258RDO>(); 
        public List<Mrs00258RDO> listRdoChemical = new List<Mrs00258RDO>(); 
        public List<Mrs00258RDO> listRdoBlood = new List<Mrs00258RDO>(); 

        public List<V_HIS_MEDI_STOCK> listMediStocks = new List<V_HIS_MEDI_STOCK>(); 

        public bool IS_CHEMICAL = false; 
        public bool IS_MATERIAL = false; 
        public bool IS_MEDICINE = false; 
        public bool IS_BLOOD = false; 

        public const int MEDICINE_GROUP_ID = 1; 
        public const int MATERIAL_GROUP_ID = 2; 
        public const int CHEMICAL_GROUP_ID = 3; 
        public const int BLOOD_GROUP_ID = 4; 
        private short IS_TRUE = 1; 

        CommonParam paramGet = new CommonParam(); 
        public Mrs00258Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00258Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            filter = (Mrs00258Filter)reportFilter; 
            try
            {
                HisMediStockViewFilterQuery mediStockFilter = new HisMediStockViewFilterQuery(); 
                mediStockFilter.IDs = filter.MEDI_STOCK_IDs; 
                listMediStocks = new MOS.MANAGER.HisMediStock.HisMediStockManager(param).GetView(mediStockFilter); 

                if (filter.IS_BLOOD) this.IS_BLOOD = true; 
                if (filter.IS_MATERIAL) this.IS_MATERIAL = true; 
                if (filter.IS_CHEMICAL) this.IS_CHEMICAL = true; 
                if (filter.IS_MEDICINE) this.IS_MEDICINE = true; 

                if ((filter.IS_BLOOD && filter.IS_CHEMICAL && filter.IS_MATERIAL && filter.IS_MEDICINE) || (!filter.IS_BLOOD && !filter.IS_CHEMICAL && !filter.IS_MATERIAL && !filter.IS_MEDICINE))
                {
                    this.IS_BLOOD = true; 
                    this.IS_MATERIAL = true; 
                    this.IS_CHEMICAL = true; 
                    this.IS_MEDICINE = true; 
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 

                result = false; 
            }
            return result; 
        }

        protected override bool ProcessData()
        {
            var result = true; 
            try
            {
                if (IsNotNullOrEmpty(listMediStocks))
                {
                    foreach (var mediStock in listMediStocks)
                    {
                        HisMediStockPeriodViewFilterQuery mediStockPeriodViewFilter = new HisMediStockPeriodViewFilterQuery(); 
                        mediStockPeriodViewFilter.MEDI_STOCK_ID = mediStock.ID; 
                        mediStockPeriodViewFilter.CREATE_TIME_TO = filter.CREATE_TIME - 1; 
                        var listMediStockPeriods = new MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodManager(param).GetView(mediStockPeriodViewFilter); 
                        if (IsNotNullOrEmpty(listMediStockPeriods))
                        {
                            // có kỳ kiểm kê
                            var period = listMediStockPeriods.OrderByDescending(o => o.CREATE_TIME).First(); 
                            ProcessBeginAmountWithPeriod(period, mediStock); 
                        }
                        else
                        {
                            ProcessBeginAmountWithNoPeriod(mediStock); 
                        }

                        ProcessOptimizeLisst(ref listRdoBlood); 
                        ProcessOptimizeLisst(ref listRdoChemical); 
                        ProcessOptimizeLisst(ref listRdoMaterial); 
                        ProcessOptimizeLisst(ref listRdoMedicine); 
                    }

                    if (this.IS_BLOOD) listRdo.AddRange(listRdoBlood); 
                    if (this.IS_CHEMICAL) listRdo.AddRange(listRdoChemical); 
                    if (this.IS_MATERIAL) listRdo.AddRange(listRdoMaterial); 
                    if (this.IS_MEDICINE) listRdo.AddRange(listRdoMedicine); 

                    listRdo = listRdo.OrderBy(o => o.SERVICE_TYPE_NAME).ToList(); 

                    listRdoGroup = listRdo.GroupBy(g => g.SERVICE_GROUP_ID).Select(s => new Mrs00258RDO
                    {
                        SERVICE_GROUP_ID = s.First().SERVICE_GROUP_ID,
                        SERVICE_GROUP_NAME = s.First().SERVICE_GROUP_NAME
                    }).ToList(); 
                }
            }
            catch (Exception ex)
            {
                result = false; 
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
            return result; 
        }

        protected void ProcessOptimizeLisst(ref List<Mrs00258RDO> listRdo)
        {
            listRdo = listRdo.GroupBy(g => new { g.MEDI_STOCK, g.SERVICE_GROUP_ID, g.SERVICE_TYPE_ID, g.IMP_PRICE }).Select(s => new Mrs00258RDO
            {
                MEDI_STOCK = s.First().MEDI_STOCK,

                SERVICE_GROUP_ID = s.First().SERVICE_GROUP_ID,
                SERVICE_GROUP_NAME = s.First().SERVICE_GROUP_NAME,

                SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                SERVICE_TYPE_CODE = s.First().SERVICE_TYPE_CODE,
                SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,

                SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,

                //PACKAGE_NUMBER = s.First().PACKAGE_NUMBER,

                SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                MANUFACTURER_NAME = s.First().MANUFACTURER_NAME,

                IMP_PRICE = s.First().IMP_PRICE,

                AMOUNT = s.Sum(su => su.AMOUNT)

            }).ToList(); 
        }

        protected void ProcessBeginAmountWithNoPeriod(V_HIS_MEDI_STOCK mediStock)
        {
            HisImpMestFilterQuery impMEstFilter = new HisImpMestFilterQuery(); 
            impMEstFilter.MEDI_STOCK_ID = mediStock.ID; 
            impMEstFilter.IMP_TIME_TO = filter.CREATE_TIME; 
            impMEstFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; 
            var listImpMests = new MOS.MANAGER.HisImpMest.HisImpMestManager(param).Get(impMEstFilter); 

            HisExpMestFilterQuery expMestFilter = new HisExpMestFilterQuery(); 
            expMestFilter.MEDI_STOCK_ID = mediStock.ID; 
            expMestFilter.FINISH_DATE_TO = filter.CREATE_TIME; 
            expMestFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE; 
            var listExpMests = new MOS.MANAGER.HisExpMest.HisExpMestManager(param).Get(expMestFilter); 

            #region blood
            if (IS_BLOOD)
            {
                var listBloodIds = new List<long>(); 

                var skip = 0; 
                var listImpMestBloods = new List<V_HIS_IMP_MEST_BLOOD>(); 
                if (IsNotNullOrEmpty(listImpMests))
                {
                    while (listImpMests.Count - skip > 0)
                    {
                        var listIds = listImpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                        HisImpMestBloodViewFilterQuery impMestBloodViewFilter = new HisImpMestBloodViewFilterQuery(); 
                        impMestBloodViewFilter.IMP_MEST_IDs = listIds.Select(s => s.ID).ToList(); 
                        listImpMestBloods.AddRange(new MOS.MANAGER.HisImpMestBlood.HisImpMestBloodManager(param).GetView(impMestBloodViewFilter));  
                    }
                    listBloodIds.AddRange(listImpMestBloods.Select(s => s.BLOOD_ID).ToList());
                }
                #region MyRegion
                /*skip = 0; 
                var listExpMestBloods = new List<V_HIS_EXP_MEST_BLOOD>(); 
                if (IsNotNullOrEmpty(listExpMests))
                {
                    while (listExpMests.Count - skip > 0)
                    {
                        var listIds = listExpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                        HisExpMestBloodViewFilterQuery expMestBloodViewFilter = new HisExpMestBloodViewFilterQuery(); 
                        expMestBloodViewFilter.EXP_MEST_IDs = listIds.Select(s => s.ID).ToList(); 
                        listExpMestBloods.AddRange(new MOS.MANAGER.HisExpMestBlood.HisExpMestBloodManager(param).GetView(expMestBloodViewFilter));  
                    }
                    listBloodIds.AddRange(listExpMestBloods.Select(s => s.BLOOD_ID).ToList()); 
                }
*/
                #endregion
                var listExpMestBloods = new List<V_HIS_EXP_MEST_BLOOD>();
                HisExpMestBloodViewFilterQuery expMestBloodViewFilter = new HisExpMestBloodViewFilterQuery();
                expMestBloodViewFilter.MEDI_STOCK_ID = mediStock.ID;
                expMestBloodViewFilter.EXP_TIME_TO = filter.CREATE_TIME;
                expMestBloodViewFilter.IS_EXPORT = true;
                listExpMestBloods.AddRange(new MOS.MANAGER.HisExpMestBlood.HisExpMestBloodManager(param).GetView(expMestBloodViewFilter));
                listBloodIds.AddRange(listExpMestBloods.Select(s => s.BLOOD_ID).ToList());
                HisBloodViewFilterQuery bloodViewFilter = new HisBloodViewFilterQuery(); 
                bloodViewFilter.IDs = listBloodIds; 
                var listBloods = new MOS.MANAGER.HisBlood.HisBloodManager(param).GetView(bloodViewFilter); 

                skip = 0; 
                var listBloodTypes = new List<V_HIS_BLOOD_TYPE>(); 
                while (listBloods.Count - skip > 0)
                {
                    var listIds = listBloods.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisBloodTypeViewFilterQuery bloodTypeViewFilter = new HisBloodTypeViewFilterQuery(); 
                    bloodTypeViewFilter.IDs = listIds.Select(s => s.BLOOD_TYPE_ID).ToList(); 
                    listBloodTypes.AddRange(new MOS.MANAGER.HisBloodType.HisBloodTypeManager(param).GetView(bloodTypeViewFilter));  
                }

                foreach (var blood in listImpMestBloods)
                {
                    var bloods = listBloods.Where(s => s.ID == blood.ID).ToList(); 
                    if (IsNotNullOrEmpty(bloods))
                    {
                        var rdo = new Mrs00258RDO(); 
                        rdo.MEDI_STOCK = mediStock; 
                        rdo.SERVICE_GROUP_ID = BLOOD_GROUP_ID; 
                        rdo.SERVICE_GROUP_NAME = "MÁU"; 

                        rdo.SERVICE_ID = bloods.First().ID; 

                        //rdo.PACKAGE_NUMBER = bloods.First().PACKAGE_NUMBER ?? 0; 

                        rdo.SERVICE_TYPE_ID = bloods.First().BLOOD_TYPE_ID; 
                        rdo.SERVICE_TYPE_CODE = bloods.First().BLOOD_TYPE_CODE; 
                        rdo.SERVICE_TYPE_NAME = bloods.First().BLOOD_TYPE_NAME; 

                        var bloodType = listBloodTypes.Where(w => w.ID == bloods.First().BLOOD_TYPE_ID).ToList(); 
                        if (IsNotNullOrEmpty(bloodType))
                            rdo.SERVICE_UNIT_NAME = bloodType.First().SERVICE_UNIT_NAME; 

                        rdo.SUPPLIER_NAME = bloods.First().SUPPLIER_NAME; 

                        rdo.IMP_PRICE = bloods.First().IMP_PRICE; 

                        rdo.AMOUNT = 1; 

                        listRdoBlood.Add(rdo); 
                    }
                }

                foreach (var blood in listExpMestBloods)
                {
                    var bloods = listBloods.Where(s => s.ID == blood.ID).ToList(); 
                    if (IsNotNullOrEmpty(bloods))
                    {
                        var rdo = new Mrs00258RDO(); 
                        rdo.MEDI_STOCK = mediStock; 
                        rdo.SERVICE_GROUP_ID = BLOOD_GROUP_ID; 
                        rdo.SERVICE_GROUP_NAME = "MÁU"; 

                        rdo.SERVICE_ID = bloods.First().ID; 

                        //rdo.BID_ID = bloods.First().BID_ID ?? 0; 

                        rdo.SERVICE_TYPE_ID = bloods.First().BLOOD_TYPE_ID; 
                        rdo.SERVICE_TYPE_CODE = bloods.First().BLOOD_TYPE_CODE; 
                        rdo.SERVICE_TYPE_NAME = bloods.First().BLOOD_TYPE_NAME; 

                        var bloodType = listBloodTypes.Where(w => w.ID == bloods.First().BLOOD_TYPE_ID).ToList(); 
                        if (IsNotNullOrEmpty(bloodType))
                            rdo.SERVICE_UNIT_NAME = bloodType.First().SERVICE_UNIT_NAME; 

                        rdo.SUPPLIER_NAME = bloods.First().SUPPLIER_NAME; 

                        rdo.IMP_PRICE = bloods.First().IMP_PRICE; 

                        rdo.AMOUNT = -1; 

                        listRdoBlood.Add(rdo); 
                    }
                }
            }
            #endregion

            #region medicine
            if (this.IS_MEDICINE)
            {
                var skip = 0; 
                var listImpMestMedicines = new List<V_HIS_IMP_MEST_MEDICINE>(); 
                if (IsNotNullOrEmpty(listImpMests))
                {
                    while (listImpMests.Count - skip > 0)
                    {
                        var listIds = listImpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                        HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery(); 
                        impMestMedicineViewFilter.IMP_MEST_IDs = listIds.Select(s => s.ID).ToList(); 
                        listImpMestMedicines.AddRange(new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(param).GetView(impMestMedicineViewFilter));  
                    }
                }

                foreach (var medi in listImpMestMedicines)
                {
                    var rdo = new Mrs00258RDO(); 
                    rdo.MEDI_STOCK = mediStock; 
                    rdo.SERVICE_GROUP_ID = MEDICINE_GROUP_ID; 
                    rdo.SERVICE_GROUP_NAME = "THUỐC"; 

                    rdo.SERVICE_ID = medi.MEDICINE_ID; 
                    rdo.SERVICE_TYPE_ID = medi.MEDICINE_TYPE_ID; 
                    rdo.SERVICE_TYPE_CODE = medi.MEDICINE_TYPE_CODE; 
                    rdo.SERVICE_TYPE_NAME = medi.MEDICINE_TYPE_NAME; 

                    //rdo.BID_ID = medi.BID_ID ?? 0; 

                    rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME; 

                    rdo.SUPPLIER_NAME = medi.SUPPLIER_NAME; 

                    rdo.MANUFACTURER_NAME = medi.MANUFACTURER_NAME; 

                    rdo.IMP_PRICE = medi.IMP_PRICE; 

                    rdo.AMOUNT = medi.AMOUNT; 

                    listRdoMedicine.Add(rdo);
                }
                #region MyRegion
                /* skip = 0; 
                var listExpMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>(); 
                if (IsNotNullOrEmpty(listExpMests))
                {
                    while (listExpMests.Count - skip > 0)
                    {
                        var listIds = listExpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                        HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery(); 
                        expMestMedicineViewFilter.EXP_MEST_IDs = listIds.Select(s => s.ID).ToList(); 
                        //expMestMedicineViewFilter.IN_EXECUTE = true; 
                        listExpMestMedicines.AddRange(new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineViewFilter));  
                    }
                }*/
                #endregion

                var listExpMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>();
                HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
                expMestMedicineViewFilter.MEDI_STOCK_ID = mediStock.ID;
                expMestMedicineViewFilter.EXP_TIME_TO = filter.CREATE_TIME;
                expMestMedicineViewFilter.IS_EXPORT = true;
                listExpMestMedicines.AddRange(new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineViewFilter));  
                foreach (var medi in listExpMestMedicines)
                {
                    var rdo = new Mrs00258RDO(); 
                    rdo.MEDI_STOCK = mediStock; 
                    rdo.SERVICE_GROUP_ID = MEDICINE_GROUP_ID; 
                    rdo.SERVICE_GROUP_NAME = "THUỐC";

                    rdo.SERVICE_ID = medi.MEDICINE_ID ?? 0; 
                    rdo.SERVICE_TYPE_ID = medi.MEDICINE_TYPE_ID; 
                    rdo.SERVICE_TYPE_CODE = medi.MEDICINE_TYPE_CODE; 
                    rdo.SERVICE_TYPE_NAME = medi.MEDICINE_TYPE_NAME; 

                    //rdo.BID_ID = medi.BID_ID ?? 0; 

                    rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME; 

                    rdo.SUPPLIER_NAME = medi.SUPPLIER_NAME; 

                    rdo.MANUFACTURER_NAME = medi.MANUFACTURER_NAME; 

                    rdo.IMP_PRICE = medi.IMP_PRICE; 

                    rdo.AMOUNT = -(medi.AMOUNT); 

                    listRdoMedicine.Add(rdo); 
                }
            }
            #endregion

            #region material and chemical
            if (this.IS_CHEMICAL || this.IS_MATERIAL)
            {
                List<long> listMaterialTypeIds = new List<long>(); 

                var skip = 0; 
                var listImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>(); 
                if (IsNotNullOrEmpty(listImpMests))
                {
                    while (listImpMests.Count - skip > 0)
                    {
                        var listIds = listImpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                        HisImpMestMaterialViewFilterQuery impMEstMaterialViewFilter = new HisImpMestMaterialViewFilterQuery(); 
                        impMEstMaterialViewFilter.IMP_MEST_IDs = listIds.Select(s => s.ID).ToList(); 
                        listImpMestMaterial.AddRange(new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(param).GetView(impMEstMaterialViewFilter));  
                    }
                    listMaterialTypeIds.AddRange(listImpMestMaterial.Select(s => s.MATERIAL_TYPE_ID).ToList());
                }
                #region MyRegion
                /*skip = 0; 
                var listExpMestMaterial = new List<V_HIS_EXP_MEST_MATERIAL>(); 
                if (IsNotNullOrEmpty(listExpMests))
                {
                    while (listExpMests.Count - skip > 0)
                    {
                        var listIds = listExpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                        HisExpMestMaterialViewFilterQuery expMEstMaterialViewFilter = new HisExpMestMaterialViewFilterQuery(); 
                        expMEstMaterialViewFilter.EXP_MEST_IDs = listIds.Select(s => s.ID).ToList(); 
                        //expMEstMaterialViewFilter.IN_EXECUTE = true; 
                        listExpMestMaterial.AddRange(new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(param).GetView(expMEstMaterialViewFilter));  
                    }
                    listMaterialTypeIds.AddRange(listExpMestMaterial.Select(s => s.MATERIAL_TYPE_ID).ToList()); 
                }*/
                #endregion

                var listExpMestMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();
                HisExpMestMaterialViewFilterQuery expMEstMaterialViewFilter = new HisExpMestMaterialViewFilterQuery();
                expMEstMaterialViewFilter.MEDI_STOCK_ID = mediStock.ID;
                expMEstMaterialViewFilter.EXP_TIME_TO = filter.CREATE_TIME;
                expMEstMaterialViewFilter.IS_EXPORT = true;
                listExpMestMaterial.AddRange(new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(param).GetView(expMEstMaterialViewFilter));
                listMaterialTypeIds.AddRange(listExpMestMaterial.Select(s => s.MATERIAL_TYPE_ID).ToList()); 
                HisMaterialTypeViewFilterQuery materialTypeFilter = new HisMaterialTypeViewFilterQuery(); 
                materialTypeFilter.IDs = listMaterialTypeIds; 
                var listMaterialTypes = new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager(param).GetView(materialTypeFilter); 

                foreach (var mate in listImpMestMaterial)
                {
                    var material = listMaterialTypes.Where(w => w.ID == mate.MATERIAL_TYPE_ID).ToList(); 
                    if (IsNotNullOrEmpty(material))
                    {
                        if ((this.IS_CHEMICAL && material.First().IS_CHEMICAL_SUBSTANCE == IS_TRUE)
                            || (this.IS_MATERIAL && material.First().IS_CHEMICAL_SUBSTANCE != IS_TRUE))
                        {
                            var rdo = new Mrs00258RDO(); 
                            rdo.MEDI_STOCK = mediStock; 

                            rdo.SERVICE_ID = mate.MATERIAL_ID; 

                            rdo.SERVICE_TYPE_ID = mate.MATERIAL_TYPE_ID; 
                            rdo.SERVICE_TYPE_CODE = mate.MATERIAL_TYPE_CODE; 
                            rdo.SERVICE_TYPE_NAME = mate.MATERIAL_TYPE_NAME; 

                            //rdo.BID_ID = mate.BID_ID ?? 0; 

                            rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME; 
                            rdo.SUPPLIER_NAME = mate.SUPPLIER_NAME; 
                            rdo.MANUFACTURER_NAME = mate.MANUFACTURER_NAME; 

                            rdo.IMP_PRICE = mate.IMP_PRICE; 
                            rdo.AMOUNT = mate.AMOUNT; 

                            if (material.First().IS_CHEMICAL_SUBSTANCE == IS_TRUE)
                            {
                                rdo.SERVICE_GROUP_ID = CHEMICAL_GROUP_ID; 
                                rdo.SERVICE_GROUP_NAME = "HÓA CHẤT"; 
                                listRdoChemical.Add(rdo); 
                            }
                            else
                            {
                                rdo.SERVICE_GROUP_ID = MATERIAL_GROUP_ID; 
                                rdo.SERVICE_GROUP_NAME = "VẬT TƯ"; 
                                listRdoMaterial.Add(rdo); 
                            }
                        }
                    }
                }

                foreach (var mate in listExpMestMaterial)
                {
                    var material = listMaterialTypes.Where(w => w.ID == mate.MATERIAL_TYPE_ID).ToList(); 
                    if (IsNotNullOrEmpty(material))
                    {
                        if ((this.IS_CHEMICAL && material.First().IS_CHEMICAL_SUBSTANCE == IS_TRUE)
                            || (this.IS_MATERIAL && material.First().IS_CHEMICAL_SUBSTANCE != IS_TRUE))
                        {
                            var rdo = new Mrs00258RDO(); 
                            rdo.MEDI_STOCK = mediStock;

                            rdo.SERVICE_ID = mate.MATERIAL_ID ?? 0; 

                            rdo.SERVICE_TYPE_ID = mate.MATERIAL_TYPE_ID; 
                            rdo.SERVICE_TYPE_CODE = mate.MATERIAL_TYPE_CODE; 
                            rdo.SERVICE_TYPE_NAME = mate.MATERIAL_TYPE_NAME; 

                            //rdo.BID_ID = mate.BID_ID ?? 0; 

                            rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME; 
                            rdo.SUPPLIER_NAME = mate.SUPPLIER_NAME; 
                            rdo.MANUFACTURER_NAME = mate.MANUFACTURER_NAME; 

                            rdo.IMP_PRICE = mate.IMP_PRICE; 
                            rdo.AMOUNT = -(mate.AMOUNT); 

                            if (material.First().IS_CHEMICAL_SUBSTANCE == IS_TRUE)
                            {
                                rdo.SERVICE_GROUP_ID = CHEMICAL_GROUP_ID; 
                                rdo.SERVICE_GROUP_NAME = "HÓA CHẤT"; 
                                listRdoChemical.Add(rdo); 
                            }
                            else
                            {
                                rdo.SERVICE_GROUP_ID = MATERIAL_GROUP_ID; 
                                rdo.SERVICE_GROUP_NAME = "VẬT TƯ"; 
                                listRdoMaterial.Add(rdo); 
                            }
                        }
                    }
                }
            }
            #endregion
        }

        protected void ProcessBeginAmountWithPeriod(V_HIS_MEDI_STOCK_PERIOD period, V_HIS_MEDI_STOCK mediStock)
        {
            HisImpMestFilterQuery impMestFilter = new HisImpMestFilterQuery(); 
            impMestFilter.MEDI_STOCK_ID = mediStock.ID; 
            impMestFilter.IMP_TIME_FROM = period.CREATE_TIME; 
            impMestFilter.IMP_TIME_TO = filter.CREATE_TIME; 
            impMestFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; 
            var listImpMests = new MOS.MANAGER.HisImpMest.HisImpMestManager(param).Get(impMestFilter); 

            HisExpMestFilterQuery expMestFilter = new HisExpMestFilterQuery(); 
            expMestFilter.MEDI_STOCK_ID = mediStock.ID; 
            expMestFilter.FINISH_DATE_FROM = period.CREATE_TIME; 
            expMestFilter.FINISH_DATE_TO = filter.CREATE_TIME; 
            expMestFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE; 
            var listExpMests = new MOS.MANAGER.HisExpMest.HisExpMestManager(param).Get(expMestFilter); 

            #region blood
            if (this.IS_BLOOD)
            {
                var listBloodIds = new List<long>(); 

                HisMestPeriodBloodFilterQuery mestPeriodBloodFilter = new HisMestPeriodBloodFilterQuery(); 
                mestPeriodBloodFilter.MEDI_STOCK_PERIOD_ID = mediStock.ID; 
                var listMestPeriodBloods = new MOS.MANAGER.HisMestPeriodBlood.HisMestPeriodBloodManager(param).Get(mestPeriodBloodFilter); 
                listBloodIds.AddRange(listMestPeriodBloods.Select(s => s.BLOOD_ID).ToList());
                var skip = 0; 
                var listImpMestBloods = new List<V_HIS_IMP_MEST_BLOOD>(); 
                if (IsNotNullOrEmpty(listImpMests))
                {
                    while (listImpMests.Count - skip > 0)
                    {
                        var listIds = listImpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                        HisImpMestBloodViewFilterQuery impMestBloodViewFilter = new HisImpMestBloodViewFilterQuery(); 
                        impMestBloodViewFilter.IMP_MEST_IDs = listIds.Select(s => s.ID).ToList(); 
                        listImpMestBloods.AddRange(new MOS.MANAGER.HisImpMestBlood.HisImpMestBloodManager(param).GetView(impMestBloodViewFilter));  
                    }
                    listBloodIds.AddRange(listImpMestBloods.Select(s => s.BLOOD_ID).ToList());
                }
                #region MyRegion
                /*var listExpMestBloods = new List<V_HIS_EXP_MEST_BLOOD>(); 
                if (IsNotNullOrEmpty(listExpMests))
                {
                    skip = 0; 
                    while (listExpMests.Count - skip > 0)
                    {
                        var listIds = listExpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                        HisExpMestBloodViewFilterQuery expMestBloodViewFilter = new HisExpMestBloodViewFilterQuery(); 
                        expMestBloodViewFilter.EXP_MEST_IDs = listIds.Select(s => s.ID).ToList(); 
                        listExpMestBloods.AddRange(new MOS.MANAGER.HisExpMestBlood.HisExpMestBloodManager(param).GetView(expMestBloodViewFilter));  
                    }

                    //listExpMestBloods = listExpMestBloods.Where(w => w.IN_EXECUTE == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_MEDICINE.IN_EXECUTE__TRUE).ToList(); 

                    listBloodIds.AddRange(listExpMestBloods.Select(s => s.BLOOD_ID).ToList()); 
                }
*/
                #endregion
                var listExpMestBloods = new List<V_HIS_EXP_MEST_BLOOD>();
                HisExpMestBloodViewFilterQuery expMestBloodViewFilter = new HisExpMestBloodViewFilterQuery();
                expMestBloodViewFilter.MEDI_STOCK_ID = mediStock.ID;
                expMestBloodViewFilter.EXP_TIME_TO = filter.CREATE_TIME;
                expMestBloodViewFilter.IS_EXPORT = true;
                listExpMestBloods.AddRange(new MOS.MANAGER.HisExpMestBlood.HisExpMestBloodManager(param).GetView(expMestBloodViewFilter));
                listBloodIds.AddRange(listExpMestBloods.Select(s => s.BLOOD_ID).ToList());
                
                skip = 0; 
                var listBloods = new List<V_HIS_BLOOD>(); 
                while (listBloodIds.Count - skip > 0)
                {
                    var listIds = listBloodIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisBloodViewFilterQuery bloodViewFilter = new HisBloodViewFilterQuery(); 
                    bloodViewFilter.IDs = listIds; 
                    listBloods.AddRange(new MOS.MANAGER.HisBlood.HisBloodManager(param).GetView(bloodViewFilter));  
                }

                skip = 0; 
                var listBloodTypes = new List<V_HIS_BLOOD_TYPE>(); 
                while (listBloods.Count - skip > 0)
                {
                    var listIds = listBloods.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisBloodTypeViewFilterQuery bloodTypeViewFilter = new HisBloodTypeViewFilterQuery(); 
                    bloodTypeViewFilter.IDs = listIds.Select(s => s.BLOOD_TYPE_ID).ToList(); 
                    listBloodTypes.AddRange(new MOS.MANAGER.HisBloodType.HisBloodTypeManager(param).GetView(bloodTypeViewFilter));  
                }

                foreach (var blood in listMestPeriodBloods)
                {
                    var bloods = listBloods.Where(s => s.ID == blood.ID).ToList(); 
                    if (IsNotNullOrEmpty(bloods))
                    {
                        var bloodType = listBloodTypes.Where(w => w.ID == bloods.First().BLOOD_TYPE_ID).ToList(); 
                        if (IsNotNullOrEmpty(bloodType))
                        {
                            var rdo = new Mrs00258RDO(); 
                            rdo.MEDI_STOCK = mediStock; 
                            rdo.SERVICE_GROUP_ID = BLOOD_GROUP_ID; 
                            rdo.SERVICE_GROUP_NAME = "MÁU"; 

                            rdo.SERVICE_ID = bloods.First().ID; 

                            //rdo.BID_ID = bloods.First().BID_ID ?? 0; 

                            rdo.SERVICE_TYPE_ID = bloods.First().BLOOD_TYPE_ID; 
                            rdo.SERVICE_TYPE_CODE = bloods.First().BLOOD_TYPE_CODE; 
                            rdo.SERVICE_TYPE_NAME = bloods.First().BLOOD_TYPE_NAME; 


                            rdo.SERVICE_UNIT_NAME = bloodType.First().SERVICE_UNIT_NAME; 

                            rdo.SUPPLIER_NAME = bloods.First().SUPPLIER_NAME; 

                            rdo.IMP_PRICE = bloods.First().IMP_PRICE; 

                            rdo.AMOUNT = 1; 

                            listRdoBlood.Add(rdo); 
                        }
                    }
                }

                foreach (var blood in listImpMestBloods)
                {
                    var bloods = listBloods.Where(s => s.ID == blood.ID).ToList(); 
                    if (IsNotNullOrEmpty(bloods))
                    {
                        var bloodType = listBloodTypes.Where(w => w.ID == bloods.First().BLOOD_TYPE_ID).ToList(); 
                        if (IsNotNullOrEmpty(bloodType))
                        {
                            var rdo = new Mrs00258RDO(); 
                            rdo.MEDI_STOCK = mediStock; 
                            rdo.SERVICE_GROUP_ID = BLOOD_GROUP_ID; 
                            rdo.SERVICE_GROUP_NAME = "MÁU"; 

                            rdo.SERVICE_ID = bloods.First().ID; 

                            //rdo.BID_ID = bloods.First().BID_ID ?? 0; 

                            rdo.SERVICE_TYPE_ID = bloods.First().BLOOD_TYPE_ID; 
                            rdo.SERVICE_TYPE_CODE = bloods.First().BLOOD_TYPE_CODE; 
                            rdo.SERVICE_TYPE_NAME = bloods.First().BLOOD_TYPE_NAME; 


                            rdo.SERVICE_UNIT_NAME = bloodType.First().SERVICE_UNIT_NAME; 

                            rdo.SUPPLIER_NAME = bloods.First().SUPPLIER_NAME; 

                            rdo.IMP_PRICE = bloods.First().IMP_PRICE; 

                            rdo.AMOUNT = 1; 

                            listRdoBlood.Add(rdo); 
                        }
                    }
                }

                foreach (var blood in listExpMestBloods)
                {
                    var bloods = listBloods.Where(s => s.ID == blood.ID).ToList(); 
                    if (IsNotNullOrEmpty(bloods))
                    {
                        var bloodType = listBloodTypes.Where(w => w.ID == bloods.First().BLOOD_TYPE_ID).ToList(); 
                        if (IsNotNullOrEmpty(bloodType))
                        {
                            var rdo = new Mrs00258RDO(); 
                            rdo.MEDI_STOCK = mediStock; 
                            rdo.SERVICE_GROUP_ID = BLOOD_GROUP_ID; 
                            rdo.SERVICE_GROUP_NAME = "MÁU"; 

                            rdo.SERVICE_ID = bloods.First().ID; 

                            //rdo.BID_ID = bloods.First().BID_ID ?? 0; 

                            rdo.SERVICE_TYPE_ID = bloods.First().BLOOD_TYPE_ID; 
                            rdo.SERVICE_TYPE_CODE = bloods.First().BLOOD_TYPE_CODE; 
                            rdo.SERVICE_TYPE_NAME = bloods.First().BLOOD_TYPE_NAME; 

                            rdo.SERVICE_UNIT_NAME = bloodType.First().SERVICE_UNIT_NAME; 

                            rdo.SUPPLIER_NAME = bloods.First().SUPPLIER_NAME; 

                            rdo.IMP_PRICE = bloods.First().IMP_PRICE; 

                            rdo.AMOUNT = -1; 

                            listRdoBlood.Add(rdo); 
                        }
                    }
                }
            }
            #endregion

            #region medicine
            if (this.IS_MEDICINE)
            {
                HisMestPeriodMediViewFilterQuery mestPeriodMediViewFilter = new HisMestPeriodMediViewFilterQuery(); 
                mestPeriodMediViewFilter.MEDI_STOCK_PERIOD_ID = period.ID; 
                var listMestPeriodMedis = new MOS.MANAGER.HisMestPeriodMedi.HisMestPeriodMediManager(param).GetView(mestPeriodMediViewFilter); 

                var skip = 0; 
                var listMedicineTypeInMediStocks = new List<V_HIS_MEDICINE_TYPE>(); 
                while (listMestPeriodMedis.Count - skip > 0)
                {
                    var listIds = listMestPeriodMedis.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisMedicineTypeViewFilterQuery medicineTypeViewFilter = new HisMedicineTypeViewFilterQuery(); 
                    medicineTypeViewFilter.IDs = listIds.Select(s => s.MEDICINE_TYPE_ID).ToList(); 
                    listMedicineTypeInMediStocks.AddRange(new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(param).GetView(medicineTypeViewFilter));  
                }
                foreach (var medi in listMestPeriodMedis)
                {
                    var rdo = new Mrs00258RDO(); 
                    rdo.MEDI_STOCK = mediStock; 
                    rdo.SERVICE_GROUP_ID = MEDICINE_GROUP_ID; 
                    rdo.SERVICE_GROUP_NAME = "THUỐC"; 

                    rdo.SERVICE_ID = medi.MEDICINE_ID; 
                    rdo.SERVICE_TYPE_ID = medi.MEDICINE_TYPE_ID; 
                    rdo.SERVICE_TYPE_CODE = medi.MEDICINE_TYPE_CODE; 
                    rdo.SERVICE_TYPE_NAME = medi.MEDICINE_TYPE_NAME; 

                    //rdo.BID_ID = medi.BID_ID ?? 0; 

                    rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME; 

                    rdo.SUPPLIER_NAME = medi.SUPPLIER_NAME; 

                    var medicineType = listMedicineTypeInMediStocks.Where(s => s.ID == medi.MEDICINE_TYPE_ID).ToList(); 
                    if (IsNotNullOrEmpty(medicineType))
                    {
                        rdo.MANUFACTURER_NAME = medicineType.First().MANUFACTURER_NAME; 
                    }

                    rdo.IMP_PRICE = medi.IMP_PRICE; 

                    rdo.AMOUNT = medi.AMOUNT; 

                    listRdoMedicine.Add(rdo); 
                }

                skip = 0; 
                var listImpMestMedicines = new List<V_HIS_IMP_MEST_MEDICINE>(); 
                while (listImpMests.Count - skip > 0)
                {
                    var listIds = listImpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery(); 
                    impMestMedicineViewFilter.IMP_MEST_IDs = listIds.Select(s => s.ID).ToList(); 
                    listImpMestMedicines.AddRange(new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(param).GetView(impMestMedicineViewFilter));  
                }

                foreach (var medi in listImpMestMedicines)
                {
                    var rdo = new Mrs00258RDO(); 
                    rdo.MEDI_STOCK = mediStock; 
                    rdo.SERVICE_GROUP_ID = MEDICINE_GROUP_ID; 
                    rdo.SERVICE_GROUP_NAME = "THUỐC"; 

                    rdo.SERVICE_ID = medi.MEDICINE_ID; 
                    rdo.SERVICE_TYPE_ID = medi.MEDICINE_TYPE_ID; 
                    rdo.SERVICE_TYPE_CODE = medi.MEDICINE_TYPE_CODE; 
                    rdo.SERVICE_TYPE_NAME = medi.MEDICINE_TYPE_NAME; 

                    //rdo.BID_ID = medi.BID_ID ?? 0; 

                    rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME; 

                    rdo.SUPPLIER_NAME = medi.SUPPLIER_NAME; 

                    rdo.MANUFACTURER_NAME = medi.MANUFACTURER_NAME; 

                    rdo.IMP_PRICE = medi.IMP_PRICE; 

                    rdo.AMOUNT = medi.AMOUNT; 

                    listRdoMedicine.Add(rdo);
                }
                #region MyRegion
                /*skip = 0; 
                var listExpMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>(); 
                while (listExpMests.Count - skip > 0)
                {
                    var listIds = listExpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery(); 
                    expMestMedicineViewFilter.EXP_MEST_IDs = listIds.Select(s => s.ID).ToList(); 
                    //expMestMedicineViewFilter.IN_EXECUTE = true; 
                    listExpMestMedicines.AddRange(new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineViewFilter));  
                }*/
                #endregion
                var listExpMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>();
                HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
                expMestMedicineViewFilter.MEDI_STOCK_ID = mediStock.ID;
                expMestMedicineViewFilter.EXP_TIME_TO = filter.CREATE_TIME;
                expMestMedicineViewFilter.IS_EXPORT = true;
                listExpMestMedicines.AddRange(new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineViewFilter));  

                foreach (var medi in listExpMestMedicines)
                {
                    var rdo = new Mrs00258RDO(); 
                    rdo.MEDI_STOCK = mediStock; 
                    rdo.SERVICE_GROUP_ID = MEDICINE_GROUP_ID; 
                    rdo.SERVICE_GROUP_NAME = "THUỐC";

                    rdo.SERVICE_ID = medi.MEDICINE_ID ?? 0; 
                    rdo.SERVICE_TYPE_ID = medi.MEDICINE_TYPE_ID; 
                    rdo.SERVICE_TYPE_CODE = medi.MEDICINE_TYPE_CODE; 
                    rdo.SERVICE_TYPE_NAME = medi.MEDICINE_TYPE_NAME; 

                    //rdo.BID_ID = medi.BID_ID ?? 0; 

                    rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME; 

                    rdo.SUPPLIER_NAME = medi.SUPPLIER_NAME; 

                    rdo.MANUFACTURER_NAME = medi.MANUFACTURER_NAME; 

                    rdo.IMP_PRICE = medi.IMP_PRICE; 

                    rdo.AMOUNT = -(medi.AMOUNT); 

                    listRdoMedicine.Add(rdo); 
                }
            }
            #endregion

            #region material and chemical
            if (this.IS_CHEMICAL || this.IS_MATERIAL)
            {
                List<long> listMaterialTypeIds = new List<long>(); 

                HisMestPeriodMateViewFilterQuery mestPeriodViewFilter = new HisMestPeriodMateViewFilterQuery(); 
                mestPeriodViewFilter.MEDI_STOCK_PERIOD_ID = period.ID; 
                var listMestPeriodMate = new MOS.MANAGER.HisMestPeriodMate.HisMestPeriodMateManager(param).GetView(mestPeriodViewFilter); 
                listMaterialTypeIds.AddRange(listMestPeriodMate.Select(s => s.MATERIAL_TYPE_ID).ToList()); 

                var skip = 0; 
                var listImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>(); 
                while (listImpMests.Count - skip > 0)
                {
                    var listIds = listImpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisImpMestMaterialViewFilterQuery impMEstMaterialViewFilter = new HisImpMestMaterialViewFilterQuery(); 
                    impMEstMaterialViewFilter.IMP_MEST_IDs = listIds.Select(s => s.ID).ToList(); 
                    listImpMestMaterial.AddRange(new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(param).GetView(impMEstMaterialViewFilter));  
                }
                listMaterialTypeIds.AddRange(listImpMestMaterial.Select(s => s.MATERIAL_TYPE_ID).ToList());
                #region MyRegion
                /*skip = 0; 
                var listExpMestMaterial = new List<V_HIS_EXP_MEST_MATERIAL>(); 
                while (listExpMests.Count - skip > 0)
                {
                    var listIds = listExpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisExpMestMaterialViewFilterQuery expMEstMaterialViewFilter = new HisExpMestMaterialViewFilterQuery(); 
                    expMEstMaterialViewFilter.EXP_MEST_IDs = listIds.Select(s => s.ID).ToList(); 
                    //expMEstMaterialViewFilter.IN_EXECUTE = true; 
                    listExpMestMaterial.AddRange(new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(param).GetView(expMEstMaterialViewFilter));  
                }
                listMaterialTypeIds.AddRange(listExpMestMaterial.Select(s => s.MATERIAL_TYPE_ID).ToList());*/
                #endregion
                
                var listExpMestMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();
                HisExpMestMaterialViewFilterQuery expMEstMaterialViewFilter = new HisExpMestMaterialViewFilterQuery();
                expMEstMaterialViewFilter.MEDI_STOCK_ID = mediStock.ID;
                expMEstMaterialViewFilter.EXP_TIME_TO = filter.CREATE_TIME;
                expMEstMaterialViewFilter.IS_EXPORT = true;
                listExpMestMaterial.AddRange(new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(param).GetView(expMEstMaterialViewFilter));
                listMaterialTypeIds.AddRange(listExpMestMaterial.Select(s => s.MATERIAL_TYPE_ID).ToList()); 
                HisMaterialTypeViewFilterQuery materialTypeFilter = new HisMaterialTypeViewFilterQuery(); 
                materialTypeFilter.IDs = listMaterialTypeIds; 
                var listMaterialTypes = new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager(param).GetView(materialTypeFilter); 

                foreach (var mate in listMestPeriodMate)
                {
                    var material = listMaterialTypes.Where(w => w.ID == mate.MATERIAL_TYPE_ID).ToList(); 
                    if (IsNotNullOrEmpty(material))
                    {
                        if ((this.IS_CHEMICAL && material.First().IS_CHEMICAL_SUBSTANCE == IS_TRUE)
                            || (this.IS_MATERIAL && material.First().IS_CHEMICAL_SUBSTANCE != IS_TRUE))
                        {
                            var rdo = new Mrs00258RDO(); 
                            rdo.MEDI_STOCK = mediStock; 

                            rdo.SERVICE_ID = mate.MATERIAL_ID; 

                            rdo.SERVICE_TYPE_ID = mate.MATERIAL_TYPE_ID; 
                            rdo.SERVICE_TYPE_CODE = mate.MATERIAL_TYPE_CODE; 
                            rdo.SERVICE_TYPE_NAME = mate.MATERIAL_TYPE_NAME; 

                            //rdo.BID_ID = mate.BID_ID ?? 0; 

                            rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME; 
                            rdo.SUPPLIER_NAME = mate.SUPPLIER_NAME; 
                            rdo.MANUFACTURER_NAME = material.First().MANUFACTURER_NAME; 

                            rdo.IMP_PRICE = mate.IMP_PRICE; 
                            rdo.AMOUNT = mate.AMOUNT; 

                            if (material.First().IS_CHEMICAL_SUBSTANCE == IS_TRUE)
                            {
                                rdo.SERVICE_GROUP_ID = CHEMICAL_GROUP_ID; 
                                rdo.SERVICE_GROUP_NAME = "HÓA CHẤT"; 
                                listRdoChemical.Add(rdo); 
                            }
                            else
                            {
                                rdo.SERVICE_GROUP_ID = MATERIAL_GROUP_ID; 
                                rdo.SERVICE_GROUP_NAME = "VẬT TƯ"; 
                                listRdoMaterial.Add(rdo); 
                            }
                        }
                    }
                }

                foreach (var mate in listImpMestMaterial)
                {
                    var material = listMaterialTypes.Where(w => w.ID == mate.MATERIAL_TYPE_ID).ToList(); 
                    if (IsNotNullOrEmpty(material))
                    {
                        if ((this.IS_CHEMICAL && material.First().IS_CHEMICAL_SUBSTANCE == IS_TRUE)
                            || (this.IS_MATERIAL && material.First().IS_CHEMICAL_SUBSTANCE != IS_TRUE))
                        {
                            var rdo = new Mrs00258RDO(); 
                            rdo.MEDI_STOCK = mediStock; 

                            rdo.SERVICE_ID = mate.MATERIAL_ID; 

                            rdo.SERVICE_TYPE_ID = mate.MATERIAL_TYPE_ID; 
                            rdo.SERVICE_TYPE_CODE = mate.MATERIAL_TYPE_CODE; 
                            rdo.SERVICE_TYPE_NAME = mate.MATERIAL_TYPE_NAME; 

                            //rdo.BID_ID = mate.BID_ID ?? 0; 

                            rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME; 
                            rdo.SUPPLIER_NAME = mate.SUPPLIER_NAME; 
                            rdo.MANUFACTURER_NAME = mate.MANUFACTURER_NAME; 

                            rdo.IMP_PRICE = mate.IMP_PRICE; 
                            rdo.AMOUNT = mate.AMOUNT; 

                            if (material.First().IS_CHEMICAL_SUBSTANCE == IS_TRUE)
                            {
                                rdo.SERVICE_GROUP_ID = CHEMICAL_GROUP_ID; 
                                rdo.SERVICE_GROUP_NAME = "HÓA CHẤT"; 
                                listRdoChemical.Add(rdo); 
                            }
                            else
                            {
                                rdo.SERVICE_GROUP_ID = MATERIAL_GROUP_ID; 
                                rdo.SERVICE_GROUP_NAME = "VẬT TƯ"; 
                                listRdoMaterial.Add(rdo); 
                            }
                        }
                    }
                }

                foreach (var mate in listExpMestMaterial)
                {
                    var material = listMaterialTypes.Where(w => w.ID == mate.MATERIAL_TYPE_ID).ToList(); 
                    if (IsNotNullOrEmpty(material))
                    {
                        if ((this.IS_CHEMICAL && material.First().IS_CHEMICAL_SUBSTANCE == IS_TRUE)
                            || (this.IS_MATERIAL && material.First().IS_CHEMICAL_SUBSTANCE != IS_TRUE))
                        {
                            var rdo = new Mrs00258RDO(); 
                            rdo.MEDI_STOCK = mediStock;

                            rdo.SERVICE_ID = mate.MATERIAL_ID ?? 0; 

                            rdo.SERVICE_TYPE_ID = mate.MATERIAL_TYPE_ID; 
                            rdo.SERVICE_TYPE_CODE = mate.MATERIAL_TYPE_CODE; 
                            rdo.SERVICE_TYPE_NAME = mate.MATERIAL_TYPE_NAME; 

                            //rdo.BID_ID = mate.BID_ID ?? 0; 

                            rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME; 
                            rdo.SUPPLIER_NAME = mate.SUPPLIER_NAME; 
                            rdo.MANUFACTURER_NAME = mate.MANUFACTURER_NAME; 

                            rdo.IMP_PRICE = mate.IMP_PRICE; 
                            rdo.AMOUNT = -(mate.AMOUNT); 

                            if (material.First().IS_CHEMICAL_SUBSTANCE == IS_TRUE)
                            {
                                rdo.SERVICE_GROUP_ID = CHEMICAL_GROUP_ID; 
                                rdo.SERVICE_GROUP_NAME = "HÓA CHẤT"; 
                                listRdoChemical.Add(rdo); 
                            }
                            else
                            {
                                rdo.SERVICE_GROUP_ID = MATERIAL_GROUP_ID; 
                                rdo.SERVICE_GROUP_NAME = "VẬT TƯ"; 
                                listRdoMaterial.Add(rdo); 
                            }
                        }
                    }
                }
            }
            #endregion
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                dicSingleTag.Add("CREATE_TIME", filter.CREATE_TIME); 

                bool exportSuccess = true; 
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Rdo", listRdo); 
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "RdoGroup", listRdoGroup); 
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "RdoGroup", "Rdo", "SERVICE_GROUP_ID", "SERVICE_GROUP_ID"); 
                exportSuccess = exportSuccess && store.SetCommonFunctions(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
