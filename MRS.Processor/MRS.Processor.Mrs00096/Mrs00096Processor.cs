using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestStt;
using MOS.MANAGER.HisMedicineBean;
using MOS.MANAGER.HisMaterialBean;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMediStock; 

namespace MRS.Processor.Mrs00096
{
    public class Mrs00096Processor : AbstractProcessor
    {
        Mrs00096Filter castFilter = null; 
        List<Mrs00096RDO> ListRdoMedicine = new List<Mrs00096RDO>(); 
        List<Mrs00096RDO> ListRdoMaterial = new List<Mrs00096RDO>();
        List<Mrs00096RDO> ListRdo = new List<Mrs00096RDO>(); 
        List<V_HIS_MATERIAL_TYPE> hisMaterialTypes = new List<V_HIS_MATERIAL_TYPE>();
        List<V_HIS_MEDICINE_TYPE> hisMedicineTypes = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MATERIAL> hisMaterial = new List<V_HIS_MATERIAL>();
        List<V_HIS_MEDICINE> hisMedicine = new List<V_HIS_MEDICINE>();
        List<HIS_MEDI_STOCK> hisMediStock = new List<HIS_MEDI_STOCK>();
        DateTime CurrentTime;
        List<V_HIS_MEDICINE_BEAN> ListMedicineBean; 
        List<V_HIS_MATERIAL_BEAN> ListMaterialBean; 
        List<V_HIS_EXP_MEST> ListExpMest; 

        public Mrs00096Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00096Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                castFilter = ((Mrs00096Filter)this.reportFilter); 
                CommonParam paramGet = new CommonParam(); 

                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_MEDICINE_BEAN, V_HIS_MATERIAL_BEAN, V_HIS_EXP_MEST MRS00096. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));


                hisMaterialTypes = new HisMaterialTypeManager(paramGet).GetView(new HisMaterialTypeViewFilterQuery());
                hisMedicineTypes = new HisMedicineTypeManager(paramGet).GetView(new HisMedicineTypeViewFilterQuery()); 

                HisMedicineBeanViewFilterQuery mediBeanFilter = new HisMedicineBeanViewFilterQuery(); 
                mediBeanFilter.IN_STOCK = 0; 
                
                HisMaterialBeanViewFilterQuery mateBeanFilter = new HisMaterialBeanViewFilterQuery(); 
                mateBeanFilter.IN_STOCK = 0; 

                HisExpMestViewFilterQuery expMestFilter = new HisExpMestViewFilterQuery(); 
                expMestFilter.EXP_MEST_STT_IDs = new List<long>(); 
                //expMestFilter.EXP_MEST_TYPE_IDs = new List<long>(); 
                //expMestFilter.EXP_MEST_TYPE_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK); 
                expMestFilter.EXP_MEST_STT_IDs.AddRange(new List<long> {
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE,
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DRAFT,
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__REJECT,
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__REQUEST
                    //Config.HisExpMestSttCFG.EXP_MEST_STT_ID__APPROVED, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DRAFT, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__REJECT, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__REQUEST
                }); 

                CurrentTime = DateTime.Now; 

                ListMedicineBean = new MOS.MANAGER.HisMedicineBean.HisMedicineBeanManager(paramGet).GetView(mediBeanFilter);
                //ListMedicineBean = ListMedicineBean.Where(x => x.MEDI_STOCK_ID != null).ToList();
                ListMaterialBean = new MOS.MANAGER.HisMaterialBean.HisMaterialBeanManager(paramGet).GetView(mateBeanFilter);
                //ListMaterialBean = ListMaterialBean.Where(x => x.MEDI_STOCK_ID != null).ToList();
                var listMaterialIds = ListMaterialBean.Select(x => x.MATERIAL_ID).Distinct().ToList();
                var listMedicineIds = ListMedicineBean.Select(x => x.MEDICINE_ID).Distinct().ToList();
                HisMaterialViewFilterQuery materialFilter = new HisMaterialViewFilterQuery();
                materialFilter.IDs = listMaterialIds;
                hisMaterial = new HisMaterialManager().GetView(materialFilter);
                HisMedicineViewFilterQuery medicineFilter = new HisMedicineViewFilterQuery();
                medicineFilter.IDs = listMedicineIds;
                hisMedicine = new HisMedicineManager().GetView(medicineFilter);
                ListExpMest = new MOS.MANAGER.HisExpMest.HisExpMestManager(paramGet).GetView(expMestFilter);
                HisMediStockFilterQuery medistockFilter = new HisMediStockFilterQuery();
                hisMediStock = new HisMediStockManager().Get(medistockFilter);
                if (!paramGet.HasException)
                {
                    result = true; 
                }
                else
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu."); 
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
            bool result = false; 
            try
            {
                if (IsNotNullOrEmpty(ListExpMest))
                {
                    if (IsNotNullOrEmpty(ListMedicineBean))
                    {
                        ProcessListMedicineBean(ListMedicineBean, ListExpMest); 
                    }
                    if (IsNotNullOrEmpty(ListMaterialBean))
                    {
                        ProcessListMaterialBean(ListMaterialBean, ListExpMest); 
                    }
                    if (IsNotNullOrEmpty(ListRdo))
                    {
                 var group = ListRdo.GroupBy(g => new { g.MATY_METY_BEAN_ID, g.IMP_PRICE,g.TYPE }).ToList();
                 ListRdo.Clear();
                foreach (var item in group)
                {
                    Mrs00096RDO rdo = new Mrs00096RDO();
                    rdo.MATY_METY_BEAN_ID = item.First().MATY_METY_BEAN_ID;
                    rdo.MATY_METY_TYPE_CODE = item.First().MATY_METY_TYPE_CODE;
                    rdo.MATY_METY_TYPE_NAME = item.First().MATY_METY_TYPE_NAME;
                    rdo.ACTIVE_INGR_BHYT_CODE = item.First().ACTIVE_INGR_BHYT_CODE;
                    rdo.ACTIVE_INGR_BHYT_NAME = item.First().ACTIVE_INGR_BHYT_NAME;
                    rdo.AMOUNT = item.First().AMOUNT;
                    rdo.MANUFACTURER_NAME = item.First().MANUFACTURER_NAME;
                    rdo.NATIONAL_NAME = item.First().NATIONAL_NAME;
                    rdo.SERVICE_UNIT_NAME = item.First().SERVICE_UNIT_NAME;
                    rdo.SUPPLIER_NAME = item.First().SUPPLIER_NAME;
                    rdo.TDL_BID_GROUP_CODE = item.First().TDL_BID_GROUP_CODE;
                    rdo.TDL_BID_NUMBER = item.First().TDL_BID_NUMBER;
                    rdo.TDL_BID_PACKAGE_CODE = item.First().TDL_BID_PACKAGE_CODE;
                    rdo.TDL_BID_YEAR = item.First().TDL_BID_YEAR;
                    rdo.MEDICINE_REGISTER_NUMBER = item.First().MEDICINE_REGISTER_NUMBER;
                    rdo.HEIN_SERVICE_BHYT_NAME = item.First().HEIN_SERVICE_BHYT_NAME;
                    rdo.MEDI_STOCK_NAME = item.First().MEDI_STOCK_NAME;
                    rdo.dicMedistock = item.GroupBy(x=>x.MEDI_STOCK_CODE).ToDictionary(x=>x.Key,y=>y.Sum(x=>x.AMOUNT));
                    ListRdo.Add(rdo);
                }
                    }
                    result = true; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private void ProcessListMedicineBean(List<V_HIS_MEDICINE_BEAN> ListMedicineBean, List<V_HIS_EXP_MEST> ListExpMest)
        {
            try
            {
                CommonParam paramGet = new CommonParam();
                ListMedicineBean = ListMedicineBean.Where(x => x.MEDI_STOCK_ID != null).ToList();
                if (IsNotNullOrEmpty(ListMedicineBean))
                {
                    ListRdoMedicine = (from r in ListMedicineBean select new Mrs00096RDO(r, hisMedicineTypes,hisMedicine)).ToList();
                    ListRdo.AddRange(ListRdoMedicine);
                }

                if (IsNotNullOrEmpty(ListExpMest))
                {
                    ProcessListMedicineBeanExpMest(paramGet, ListExpMest); 
                }
                ListRdoMedicine = ListRdoMedicine.GroupBy(o => o.MEDICINE_BEAN_ID).Select(p => p.First()).ToList();
                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MEDICINE_BEAN, MRS00096."); 
                }

                ListRdoMedicine = ListRdoMedicine.GroupBy(g => new { g.MEDICINE_TYPE_ID, g.IMP_PRICE }).Select(s => new Mrs00096RDO { MEDICINE_TYPE_ID = s.First().MEDICINE_TYPE_ID, IMP_PRICE = s.First().IMP_PRICE, MEDICINE_TYPE_CODE = s.First().MEDICINE_TYPE_CODE, MEDICINE_TYPE_NAME = s.First().MEDICINE_TYPE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, NUM_ORDER = s.First().NUM_ORDER, CONCENTRA = s.First().CONCENTRA, AMOUNT = s.Sum(s1 => s1.AMOUNT) }).ToList();
               
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                ListRdoMedicine.Clear(); 
            }
        }

        private void ProcessListMedicineBeanExpMest(CommonParam paramGet, List<V_HIS_EXP_MEST> hisExpMests)
        {
            try
            {
                int start = 0; 
                int count = hisExpMests.Count; 
                while (count > 0)
                {
                    int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery(); 
                    expMediFilter.EXP_MEST_IDs = hisExpMests.Skip(start).Take(limit).Select(s => s.ID).ToList();
                    expMediFilter.IS_EXPORT = true;
                    List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(expMediFilter); 
                    if (IsNotNullOrEmpty(hisExpMestMedicine))
                    {
                        HisMedicineBeanViewFilterQuery mediBeanFilter = new HisMedicineBeanViewFilterQuery();
                        mediBeanFilter.MEDICINE_IDs = hisExpMestMedicine.Select(s => s.MEDICINE_ID ?? 0).Distinct().ToList(); //review
                        var hisMedicineBeans = new MOS.MANAGER.HisMedicineBean.HisMedicineBeanManager(paramGet).GetView(mediBeanFilter);
                        hisMedicineBeans = hisMedicineBeans.Where(x => x.MEDI_STOCK_ID != null).ToList();
                        if (IsNotNullOrEmpty(hisMedicineBeans))
                        {
                            ListRdoMedicine.AddRange((from r in hisMedicineBeans select new Mrs00096RDO(r, hisMedicineTypes,hisMedicine)).ToList()); 
                        }

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessListMaterialBean(List<V_HIS_MATERIAL_BEAN> ListMaterialBean, List<V_HIS_EXP_MEST> ListExpMest)
        {
            try
            {
                CommonParam paramGet = new CommonParam(); 
                ListMaterialBean = ListMaterialBean.Where(x=>x.MEDI_STOCK_ID!=null).ToList();
                if (IsNotNullOrEmpty(ListMaterialBean))
                {
                    ListRdoMaterial = (from r in ListMaterialBean select new Mrs00096RDO(r, hisMaterialTypes,hisMaterial)).ToList();
                    ListRdo.AddRange(ListRdoMaterial);
                }

                if (IsNotNullOrEmpty(ListExpMest))
                {
                    ProcessListMaterialBeanExpMest(paramGet, ListExpMest); 

                }
                ListRdoMaterial = ListRdoMaterial.GroupBy(o => o.MATERIAL_BEAN_ID).Select(p => p.First()).ToList();
                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MATERIAL_BEAN, MRS00096."); 
                }

                ListRdoMaterial = ListRdoMaterial.GroupBy(g => new { g.MATERIAL_TYPE_ID, g.IMP_PRICE }).Select(s => new Mrs00096RDO { MATERIAL_TYPE_ID = s.First().MATERIAL_TYPE_ID, IMP_PRICE = s.First().IMP_PRICE, MATERIAL_TYPE_CODE = s.First().MATERIAL_TYPE_CODE, MATERIAL_TYPE_NAME = s.First().MATERIAL_TYPE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, NUM_ORDER = s.First().NUM_ORDER, CONCENTRA = s.First().CONCENTRA, AMOUNT = s.Sum(s1 => s1.AMOUNT) }).ToList();
                

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                ListRdoMedicine.Clear(); 
            }
        }

        private void ProcessListMaterialBeanExpMest(CommonParam paramGet, List<V_HIS_EXP_MEST> hisExpMests)
        {
            try
            {
                int start = 0; 
                int count = hisExpMests.Count; 
                while (count > 0)
                {
                    int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                    expMateFilter.EXP_MEST_IDs = hisExpMests.Skip(start).Take(limit).Select(s => s.ID).ToList();
                    expMateFilter.IS_EXPORT = true;
                    List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(paramGet).GetView(expMateFilter); 
                    if (IsNotNullOrEmpty(hisExpMestMaterial))
                    {
                        HisMaterialBeanViewFilterQuery mateBeanFilter = new HisMaterialBeanViewFilterQuery();
                        mateBeanFilter.MATERIAL_IDs = hisExpMestMaterial.Select(s => s.MATERIAL_ID ?? 0).Distinct().ToList(); 
                        var hisMaterialBeans = new MOS.MANAGER.HisMaterialBean.HisMaterialBeanManager(paramGet).GetView(mateBeanFilter); 
                        hisMaterialBeans = hisMaterialBeans.Where(x=>x.MEDI_STOCK_ID!=null).ToList();
                        if (IsNotNullOrEmpty(hisMaterialBeans))
                        {
                            ListRdoMaterial.AddRange((from r in hisMaterialBeans select new Mrs00096RDO(r, hisMaterialTypes,hisMaterial)).ToList()); 
                        }

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (CurrentTime != null)
                {
                    dicSingleTag.Add("CREATE_DATE_SEPARATE_STR", Inventec.Common.DateTime.Convert.SystemDateTimeToDateSeparateString(CurrentTime)); 
                }

                objectTag.AddObjectData(store, "ReportMedicine", ListRdoMedicine); 
                objectTag.AddObjectData(store, "ReportMaterial", ListRdoMaterial);
                objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
