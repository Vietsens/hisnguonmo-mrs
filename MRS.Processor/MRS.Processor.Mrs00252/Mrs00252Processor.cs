using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using MRS.MANAGER.Core.MrsReport.RDO; 
using MOS.MANAGER.HisDepartment; 
using MOS.MANAGER.HisExpMest; 
using MOS.MANAGER.HisExpMestMaterial; 
using MOS.MANAGER.HisExpMestMedicine; 
using MOS.MANAGER.HisImpMest; 
using MOS.MANAGER.HisImpMestMaterial; 
using MOS.MANAGER.HisImpMestMedicine; 
using MOS.MANAGER.HisMediStock; 
using MOS.MANAGER.HisMediStockPeriod; 
using MOS.MANAGER.HisMestPeriodMety; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00252
{
    public class Mrs00252Processor : AbstractProcessor
    {
        List<long> listExpMestIds = new List<long>(); 
        List<V_HIS_IMP_MEST> listMobaImpMest = new List<V_HIS_IMP_MEST>(); 

        CommonParam paramGet = new CommonParam(); 
        List<Mrs00252RDO> ListRdo = new List<Mrs00252RDO>(); 
        List<Mrs00252RDO> Parent = new List<Mrs00252RDO>(); 
        List<V_HIS_EXP_MEST_MEDICINE> expMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>(); 
        List<V_HIS_EXP_MEST_MATERIAL> expMestMaterials = new List<V_HIS_EXP_MEST_MATERIAL>(); 
        List<HIS_IMP_MEST_MEDICINE> listImpMestMedicine = new List<HIS_IMP_MEST_MEDICINE>(); 
        List<HIS_IMP_MEST_MATERIAL> listImpMestMaterial = new List<HIS_IMP_MEST_MATERIAL>();

        HIS_DEPARTMENT hisDepartment = new HIS_DEPARTMENT();
        HIS_MEDI_STOCK hisMediStock = new HIS_MEDI_STOCK(); 
        private string a = ""; 
        public Mrs00252Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            a = reportTypeCode; 
        }

        public override Type FilterType()
        {
            return typeof(Mrs00252Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            try
            {
                var filter = ((Mrs00252Filter)reportFilter); 
                //get dữ liệu:
                if (filter.IS_MEDICINE == null || filter.IS_MEDICINE == true)
                {
                    HisExpMestMedicineViewFilterQuery expMestMedicineFilter = new HisExpMestMedicineViewFilterQuery(); 
                    expMestMedicineFilter.EXP_TIME_FROM = filter.TIME_FROM; 
                    expMestMedicineFilter.EXP_TIME_TO = filter.TIME_TO;
                    expMestMedicineFilter.REQ_DEPARTMENT_ID = filter.DEPARTMENT_ID;
                    expMestMedicineFilter.IS_EXPORT = true;
                    expMestMedicineFilter.EXP_MEST_TYPE_ID = filter.EXP_MEST_TYPE_ID;
                    expMestMedicineFilter.MEDI_STOCK_ID = filter.MEDI_STOCK_ID; 
                    var expMestMedicinesSub = new HisExpMestMedicineManager(paramGet).GetView(expMestMedicineFilter); 
                    expMestMedicines.AddRange(expMestMedicinesSub);
                    var listExpMestIdsSub = expMestMedicines.Select(o => o.EXP_MEST_ID ?? 0).ToList(); 
                    listExpMestIds.AddRange(listExpMestIdsSub); 

                }

                if (filter.IS_MATERIAL == null || filter.IS_MATERIAL == true)
                {
                    HisExpMestMaterialViewFilterQuery expMestMaterialFilter = new HisExpMestMaterialViewFilterQuery(); 
                    expMestMaterialFilter.EXP_TIME_FROM = filter.TIME_FROM; 
                    expMestMaterialFilter.EXP_TIME_TO = filter.TIME_TO;
                    expMestMaterialFilter.IS_EXPORT = true;
                    expMestMaterialFilter.EXP_MEST_TYPE_ID = filter.EXP_MEST_TYPE_ID;
                    expMestMaterialFilter.MEDI_STOCK_ID = filter.MEDI_STOCK_ID; 
                    var expMestMaterialsSub = new HisExpMestMaterialManager(paramGet).GetView(expMestMaterialFilter); 
                    if (filter.DEPARTMENT_ID != null) expMestMaterialsSub = expMestMaterialsSub.Where(o => o.REQ_DEPARTMENT_ID == filter.DEPARTMENT_ID).ToList(); 
                    expMestMaterials.AddRange(expMestMaterialsSub);
                    var listExpMestIdsSub = expMestMaterials.Select(o => o.EXP_MEST_ID ?? 0).ToList(); 
                    listExpMestIds.AddRange(listExpMestIdsSub); 
                }
                
                listExpMestIds = listExpMestIds.Distinct().ToList();
                if (listExpMestIds.Count > 0)
                {
                    //Lấy các phiếu nhập thu hồi từ các phiếu đã xuất
                    var skip = 0;
                    while (listExpMestIds.Count - skip > 0)
                    {
                        var listIDs = listExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisImpMestViewFilterQuery MobaImpMestFilter = new HisImpMestViewFilterQuery()
                        {
                            MOBA_EXP_MEST_IDs = listIDs
                        };
                        var listMobaImpMestSub = new HisImpMestManager(paramGet).GetView(MobaImpMestFilter);
                        listMobaImpMest.AddRange(listMobaImpMestSub);
                    }
                }
                if (listMobaImpMest.Count > 0)
                {
                    var skip = 0;
                    while (listMobaImpMest.Count - skip > 0)
                    {
                        var listIDs = listMobaImpMest.Select(o=>o.ID).Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisImpMestMedicineFilterQuery impMeFilter = new HisImpMestMedicineFilterQuery();
                        impMeFilter.IMP_MEST_IDs = listIDs;
                        var listImpMestMedicineSub = new HisImpMestMedicineManager().Get(impMeFilter);
                        listImpMestMedicine.AddRange(listImpMestMedicineSub);

                        HisImpMestMaterialFilterQuery impMaFilter = new HisImpMestMaterialFilterQuery();
                        impMaFilter.IMP_MEST_IDs = listIDs;
                        var listImpMestMaterialSub = new HisImpMestMaterialManager().Get(impMaFilter);
                        listImpMestMaterial.AddRange(listImpMestMaterialSub);
                    }
                    
                }
                hisMediStock = new HisMediStockManager(paramGet).GetById(filter.MEDI_STOCK_ID ?? 0);
                hisDepartment = new HisDepartmentManager(paramGet).GetById(filter.DEPARTMENT_ID ?? 0); 
                result = true; 
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
            bool result = false; 
            try
            {
                ListRdo.Clear(); 
                List<Mrs00252RDO> data = new List<Mrs00252RDO>(); 
                //Them du lieu thuoc vao danh sach xuat bao cao
                this.AddMedicine(expMestMedicines, ref data); 
                //Them du lieu vat tu vao danh sach xuat bao cao
                this.AddMaterial(expMestMaterials, ref data);  //Gom nhom theo lo thuoc/vat tu
                this.ListRdo = data.GroupBy(o => new { o.TYPE, o.ID, o.VAT_RATIO, o.EXP_PRICE })
                    .Select(t =>
                        new Mrs00252RDO
                        {
                            VAT_RATIO = t.Key.VAT_RATIO,
                            EXP_PRICE = t.Key.EXP_PRICE,
                            EXP_AMOUNT = t.Sum(x => x.EXP_AMOUNT),
                            MOBA_AMOUNT = t.Sum(x => x.MOBA_AMOUNT),
                            BID_NUMBER = t.First().BID_NUMBER,
                            EXPIRED_DATE_STR = t.First().EXPIRED_DATE_STR,
                            PACKAGE_NUMBER = t.First().PACKAGE_NUMBER,
                            SERVICE_UNIT_NAME = t.First().SERVICE_UNIT_NAME,
                            SUPPLIER_CODE = t.First().SUPPLIER_CODE,
                            SUPPLIER_NAME = t.First().SUPPLIER_NAME,
                            TYPE = t.Key.TYPE,
                            TYPE_CODE = t.First().TYPE_CODE,
                            TYPE_NAME = t.First().TYPE_NAME
                        }).ToList(); 
                ListRdo = ListRdo.OrderBy(o => o.TYPE).ThenBy(p => p.TYPE_CODE).ToList(); 
                Parent = ListRdo.GroupBy(o => o.TYPE).Select(p => p.First()).ToList(); 
                result = true; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                ListRdo.Clear(); 

            }
            return result; 
        }
        private void AddMedicine(List<V_HIS_EXP_MEST_MEDICINE> expMestMedicines, ref List<Mrs00252RDO> ListRdo)
        {
            if (IsNotNullOrEmpty(expMestMedicines))
            {
                expMestMedicines = expMestMedicines
                    .OrderByDescending(o => o.NUM_ORDER)
                    .ThenBy(o => o.MEDICINE_TYPE_ID)
                    .ToList(); 
                var medicines = expMestMedicines.Select(o => new Mrs00252RDO
                {
                    TYPE = Mrs00252RDO.MEDICINE,
                    EXP_AMOUNT = o.AMOUNT,
                    MOBA_AMOUNT = listMobaImpMest.Where(r => r.MOBA_EXP_MEST_ID == o.EXP_MEST_ID).ToList().Count > 0 ? listImpMestMedicine.Where(p => listMobaImpMest.Where(r => r.MOBA_EXP_MEST_ID == o.EXP_MEST_ID).Select(q => q.ID).ToList().Contains(p.IMP_MEST_ID) && p.MEDICINE_ID == o.MEDICINE_ID).Sum(s => s.AMOUNT) : 0,
                    MANUFACTURER_NAME = o.MANUFACTURER_NAME,
                    BID_NUMBER = o.BID_NUMBER,
                    EXPIRED_DATE_STR = o.EXPIRED_DATE.HasValue ? Inventec.Common.DateTime.Convert.TimeNumberToTimeString(o.EXPIRED_DATE.Value) : null,
                    EXP_PRICE = o.PRICE ?? o.IMP_PRICE,
                    EXP_TIME_STR = o.EXP_TIME.HasValue ? Inventec.Common.DateTime.Convert.TimeNumberToTimeString(o.EXP_TIME.Value) : null,
                    INTERNAL_PRICE = o.INTERNAL_PRICE ?? 0,
                    PACKAGE_NUMBER = o.PACKAGE_NUMBER,
                    SERVICE_UNIT_NAME = o.SERVICE_UNIT_NAME,
                    SUPPLIER_CODE = o.SUPPLIER_CODE,
                    SUPPLIER_NAME = o.SUPPLIER_NAME,
                    TYPE_NAME = o.MEDICINE_TYPE_NAME,
                    TYPE_CODE = o.MEDICINE_TYPE_CODE,
                    EXP_MEST_CODE = o.EXP_MEST_CODE,
                    VAT_RATIO = o.IMP_VAT_RATIO,
                    ID = o.MEDICINE_ID ?? 0
                }).ToList(); 

                if (ListRdo == null)
                {
                    ListRdo = new List<Mrs00252RDO>(); 
                }
                ListRdo.AddRange(medicines); 
            }
        }

        private void AddMaterial(List<V_HIS_EXP_MEST_MATERIAL> expMestMaterials, ref List<Mrs00252RDO> ListRdo)
        {
            if (IsNotNullOrEmpty(expMestMaterials))
            {
                expMestMaterials = expMestMaterials
                    .OrderByDescending(o => o.NUM_ORDER)
                    .ThenBy(o => o.MATERIAL_TYPE_ID).ToList(); 
                var materials = expMestMaterials.Select(o => new Mrs00252RDO
                {
                    TYPE = Mrs00252RDO.MATERIAL,
                    EXP_AMOUNT = o.AMOUNT,
                    MOBA_AMOUNT = listMobaImpMest.Where(r => r.MOBA_EXP_MEST_ID == o.EXP_MEST_ID).ToList().Count > 0 ? listImpMestMaterial.Where(p => listMobaImpMest.Where(r => r.MOBA_EXP_MEST_ID == o.EXP_MEST_ID).Select(q => q.ID).ToList().Contains(p.IMP_MEST_ID) && p.MATERIAL_ID == o.MATERIAL_ID).Sum(s => s.AMOUNT) : 0,
                    MANUFACTURER_NAME = o.MANUFACTURER_NAME,
                    BID_NUMBER = o.BID_NUMBER,
                    EXPIRED_DATE_STR = o.EXPIRED_DATE.HasValue ? Inventec.Common.DateTime.Convert.TimeNumberToTimeString(o.EXPIRED_DATE.Value) : null,
                    EXP_PRICE = o.PRICE ?? o.IMP_PRICE,
                    EXP_TIME_STR = o.EXP_TIME.HasValue ? Inventec.Common.DateTime.Convert.TimeNumberToTimeString(o.EXP_TIME.Value) : null,
                    INTERNAL_PRICE = o.INTERNAL_PRICE ?? 0,
                    PACKAGE_NUMBER = o.PACKAGE_NUMBER,
                    SERVICE_UNIT_NAME = o.SERVICE_UNIT_NAME,
                    SUPPLIER_CODE = o.SUPPLIER_CODE,
                    SUPPLIER_NAME = o.SUPPLIER_NAME,
                    TYPE_NAME = o.MATERIAL_TYPE_NAME,
                    TYPE_CODE = o.MATERIAL_TYPE_CODE,
                    EXP_MEST_CODE = o.EXP_MEST_CODE,
                    VAT_RATIO = o.IMP_VAT_RATIO,
                    ID = o.MATERIAL_ID ?? 0
                }).ToList(); 

                if (ListRdo == null)
                {
                    ListRdo = new List<Mrs00252RDO>(); 
                }
                ListRdo.AddRange(materials); 
            }
        }
      
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00252Filter)this.reportFilter).TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00252Filter)this.reportFilter).TIME_TO));
            if (hisDepartment != null) dicSingleTag.Add("DEPARTMENT_NAME", hisDepartment.DEPARTMENT_NAME);
            if (hisMediStock != null) dicSingleTag.Add("MEDI_STOCK_NAME", hisMediStock.MEDI_STOCK_NAME); 
            objectTag.AddObjectData(store, "Report", ListRdo); 
            objectTag.AddObjectData(store, "Parent", Parent); 
            
        }
    }
}
