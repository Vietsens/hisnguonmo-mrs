using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisBlood;
using MOS.MANAGER.HisSupplier;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestBlood;
using MOS.MANAGER.HisBloodType;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Core.MrsReport; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;
using MOS.MANAGER.HisMediStock; 

namespace MRS.Processor.Mrs00438
{
    class Mrs00438Processor : AbstractProcessor
    {
        Mrs00438Filter castFilter = null; 
        List<Mrs00438RDO> listRdo = new List<Mrs00438RDO>(); 
        List<Mrs00438RDO> listRdoGroup = new List<Mrs00438RDO>(); 


        public Mrs00438Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }
        List<V_HIS_IMP_MEST> listImpMests = new List<V_HIS_IMP_MEST>(); 
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines = new List<V_HIS_IMP_MEST_MEDICINE>(); 
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterials = new List<V_HIS_IMP_MEST_MATERIAL>(); 

        List<V_HIS_IMP_MEST_BLOOD> listImpMestBloods = new List<V_HIS_IMP_MEST_BLOOD>(); 
        List<V_HIS_BLOOD_TYPE> listBloodTypes = new List<V_HIS_BLOOD_TYPE>();

        List<HIS_MEDI_STOCK> listMediStock = new List<HIS_MEDI_STOCK>();
        List<HIS_SUPPLIER> supplier = new List<HIS_SUPPLIER>(); 

        public override Type FilterType()
        {
            return typeof(Mrs00438Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 
                this.castFilter = (Mrs00438Filter)this.reportFilter; 
                var skip = 0; 
               //lay nha cung cap
                HisSupplierFilterQuery supplierFilter = new HisSupplierFilterQuery(); 
                supplierFilter.ID = this.castFilter.SUPPLIER_ID; 
                supplier = new MOS.MANAGER.HisSupplier.HisSupplierManager(paramGet).Get(supplierFilter); 
                //lấy kho
                HisMediStockFilterQuery mediStockFilter= new HisMediStockFilterQuery();
                mediStockFilter.IDs = castFilter.MEDI_STOCK_IDs;
                listMediStock = new HisMediStockManager(paramGet).Get(mediStockFilter);
                //lay thuoc nhap NCC
                HisImpMestMedicineViewFilterQuery mediFilter = new HisImpMestMedicineViewFilterQuery();
                mediFilter.IMP_TIME_FROM = this.castFilter.TIME_FROM;
                mediFilter.IMP_TIME_TO = this.castFilter.TIME_TO;
                mediFilter.MEDI_STOCK_IDs = this.castFilter.MEDI_STOCK_IDs; 
                mediFilter.IMP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC; 
                listImpMestMedicines = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(mediFilter); 
                listImpMestMedicines = listImpMestMedicines.Where(w => w.SUPPLIER_ID == this.castFilter.SUPPLIER_ID).ToList(); 
                //lay vat tu, hoa chat NCC
                HisImpMestMaterialViewFilterQuery mateFilter = new HisImpMestMaterialViewFilterQuery(); 
                mateFilter.IMP_TIME_FROM = this.castFilter.TIME_FROM;
                mateFilter.IMP_TIME_TO = this.castFilter.TIME_TO;
                mateFilter.MEDI_STOCK_IDs = this.castFilter.MEDI_STOCK_IDs; 
                mateFilter.IMP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC; 
                listImpMestMaterials = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(mateFilter); 
                listImpMestMaterials = listImpMestMaterials.Where(w => w.SUPPLIER_ID == this.castFilter.SUPPLIER_ID).ToList(); 
                //lay mau NCC
                HisImpMestViewFilterQuery impMestFilter = new HisImpMestViewFilterQuery(); 
                impMestFilter.IMP_TIME_FROM = this.castFilter.TIME_FROM; 
                impMestFilter.IMP_TIME_TO = this.castFilter.TIME_TO; 
                listImpMests = new MOS.MANAGER.HisImpMest.HisImpMestManager(paramGet).GetView(impMestFilter); 
                var listImpMestIds = listImpMests.Select(s => s.ID).ToList(); 
                skip = 0; 
                while(listImpMestIds.Count - skip > 0)
                {
                    var listIds = listImpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisImpMestBloodViewFilterQuery bloodFilter = new HisImpMestBloodViewFilterQuery(); 
                    bloodFilter.IMP_MEST_IDs = listIds; 
                    var impMestBloods = new MOS.MANAGER.HisImpMestBlood.HisImpMestBloodManager(paramGet).GetView(bloodFilter); 
                    listImpMestBloods.AddRange(impMestBloods); 
                }

                listImpMestBloods = listImpMestBloods.Where(w => w.SUPPLIER_ID == this.castFilter.SUPPLIER_ID && w.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC).ToList(); 
                var listBloodTypeIds = listImpMestBloods.Where(w => w.BLOOD_TYPE_ID != null).Select(s => s.BLOOD_TYPE_ID).Distinct().ToList(); 
                skip = 0; 
                while(listBloodTypeIds.Count - skip >0)
                {
                    var listIds = listBloodTypeIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisBloodTypeViewFilterQuery filter = new HisBloodTypeViewFilterQuery(); 
                    filter.IDs = listIds; 
                    var bloodTypes = new MOS.MANAGER.HisBloodType.HisBloodTypeManager(paramGet).GetView(filter); 
                    listBloodTypes.AddRange(bloodTypes); 
                }
                
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
                if(IsNotNullOrEmpty(listImpMestMedicines))
                {
                    foreach(var medicine in listImpMestMedicines)
                    {
                        Mrs00438RDO rdo = new Mrs00438RDO(); 
                        rdo.IMP_MEST_CODE = medicine.IMP_MEST_CODE; 
                        rdo.SERVICE_TYPE_NAME = medicine.MEDICINE_TYPE_NAME; 
                        rdo.PACKING_TYPE_NAME = medicine.PACKING_TYPE_NAME;
                        rdo.MANU_NAME = medicine.MANUFACTURER_NAME;
                        rdo.NATIONAL_NAM = medicine.NATIONAL_NAME;
                        var mediStock = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == medicine.MEDI_STOCK_ID);
                        if (mediStock != null)
                        {
                            rdo.MEDI_STOCK_NAME = mediStock.MEDI_STOCK_NAME; 
                        }
                        rdo.DOCUMENT_NUMBER= medicine.DOCUMENT_NUMBER;
                        rdo.SERVICE_UNIT_NAME = medicine.SERVICE_UNIT_NAME; 
                        rdo.PACKAGE_NUMBER = medicine.PACKAGE_NUMBER; 
                        rdo.EXPIRED_DATE = medicine.EXPIRED_DATE; 
                        rdo.IMP_AMOUNT = medicine.AMOUNT; 
                        rdo.PRICE = medicine.IMP_PRICE;
                        rdo.VAT_RATIO = medicine.VAT_RATIO;
                        listRdo.Add(rdo); 
                    }
                }
                if (IsNotNullOrEmpty(listImpMestMaterials))
                {
                    foreach (var material in listImpMestMaterials)
                    {
                        Mrs00438RDO rdo = new Mrs00438RDO(); 
                        rdo.IMP_MEST_CODE = material.IMP_MEST_CODE; 
                        rdo.SERVICE_TYPE_NAME = material.MATERIAL_TYPE_NAME; 
                        rdo.PACKING_TYPE_NAME = material.PACKING_TYPE_NAME; 
                        rdo.MANU_NAME = material.MANUFACTURER_NAME;
                        rdo.NATIONAL_NAM = material.NATIONAL_NAME;
                        var mediStock = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == material.MEDI_STOCK_ID);
                        if (mediStock != null)
                        {
                            rdo.MEDI_STOCK_NAME = mediStock.MEDI_STOCK_NAME;
                        }
                        rdo.DOCUMENT_NUMBER = material.DOCUMENT_NUMBER;
                        rdo.SERVICE_UNIT_NAME = material.SERVICE_UNIT_NAME; 
                        rdo.PACKAGE_NUMBER = material.PACKAGE_NUMBER; 
                        rdo.EXPIRED_DATE = material.EXPIRED_DATE; 
                        rdo.IMP_AMOUNT = material.AMOUNT; 
                        rdo.PRICE = material.IMP_PRICE;
                        rdo.VAT_RATIO = material.VAT_RATIO;
                        listRdo.Add(rdo); 
                    }
                }

                if (IsNotNullOrEmpty(listImpMestBloods))
                {
                    foreach (var blood in listImpMestBloods)
                    {
                        var bloodType = listBloodTypes.Where(w => w.ID == blood.BLOOD_TYPE_ID).ToList(); 
                        Mrs00438RDO rdo = new Mrs00438RDO(); 
                        rdo.IMP_MEST_CODE = blood.IMP_MEST_CODE; 
                        rdo.SERVICE_TYPE_NAME = blood.BLOOD_TYPE_NAME; 
                        if(IsNotNullOrEmpty(bloodType))
                        {
                            rdo.PACKING_TYPE_NAME = bloodType.First().PACKING_TYPE_NAME; 
                        }
                        rdo.DOCUMENT_NUMBER = "";
                        rdo.MANU_NAME = "";
                        rdo.NATIONAL_NAM = "";
                        var mediStock = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == blood.MEDI_STOCK_ID);
                        if (mediStock != null)
                        {
                            rdo.MEDI_STOCK_NAME = mediStock.MEDI_STOCK_NAME;
                        }
                        rdo.SERVICE_UNIT_NAME = blood.SERVICE_UNIT_NAME; 
                        rdo.PACKAGE_NUMBER = blood.PACKAGE_NUMBER; 
                        rdo.EXPIRED_DATE = blood.EXPIRED_DATE; 
                        rdo.IMP_AMOUNT = blood.VOLUME; 
                        rdo.PRICE = blood.IMP_PRICE;
                        rdo.VAT_RATIO = blood.VAT_RATIO;
                        listRdo.Add(rdo); 
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
                if (castFilter.DOCUMENT_NUMBER!=null)
                {
                    listRdo = listRdo.Where(x=>x.DOCUMENT_NUMBER.Equals(castFilter.DOCUMENT_NUMBER)).ToList();
                    dicSingleTag.Add("INVOICE_NUMBER",castFilter.DOCUMENT_NUMBER);
                }
                else
                {
                    dicSingleTag.Add("INVOICE_NUMBER", "NOTHING");
                }
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                dicSingleTag.Add("SUPPLIER_NAME", supplier.First().SUPPLIER_NAME); 
                objectTag.AddObjectData(store, "Report", listRdo.OrderBy(o=>o.IMP_MEST_CODE).ToList()); 
                dicSingleTag.Add("MEDI_STOCK_NAMEs", string.Join(",",listMediStock.Select(x=>x.MEDI_STOCK_NAME).ToList()));
                dicSingleTag.Add("IMP_MEST_CODEs", string.Join(",", listRdo.Select(x => x.IMP_MEST_CODE).Distinct().ToList()));
                //objectTag.AddObjectData(store, "Group", listRdoGroup.OrderBy(s => s.GROUP_DATE).ToList()); 
                //bool exportSuccess = true; 
                //exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group", "Report", "GROUP_DATE", "GROUP_DATE"); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

    }
}
