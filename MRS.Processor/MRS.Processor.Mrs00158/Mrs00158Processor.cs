using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
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
using MOS.MANAGER.HisExpMest; 
using MOS.MANAGER.HisExpMestMaterial; 
using MOS.MANAGER.HisExpMestMedicine; 
using MOS.MANAGER.HisImpMest; 
using MOS.MANAGER.HisImpMestMaterial; 
using MOS.MANAGER.HisImpMestMedicine; 
using MOS.MANAGER.HisMaterialType; 
using MOS.MANAGER.HisMedicineType; 
using MOS.MANAGER.HisMediStock; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00158
{
    public class Mrs00158Processor : AbstractProcessor
    {
        List<V_HIS_IMP_MEST> listMobaImpMest = new List<V_HIS_IMP_MEST>(); 
        Mrs00158Filter filter = new Mrs00158Filter(); 
        List<Mrs00158RDO> ListRdo = new List<Mrs00158RDO>(); 
        CommonParam paramGet = new CommonParam(); 

        List<V_HIS_IMP_MEST> ListHisMobaImpMest = new List<V_HIS_IMP_MEST>(); 

        List<V_HIS_EXP_MEST_MEDICINE> ListExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>(); 
        List<V_HIS_EXP_MEST_MATERIAL> ListExpMestMaterial = new List<V_HIS_EXP_MEST_MATERIAL>(); 
        List<V_HIS_IMP_MEST_MEDICINE> ListImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>(); 
        List<V_HIS_IMP_MEST_MATERIAL> ListImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>(); 

        List<V_HIS_MEDI_STOCK> listMediStockViews = new List<V_HIS_MEDI_STOCK>(); 
        public List<V_HIS_EXP_MEST> listChmsExpMest = new List<V_HIS_EXP_MEST>(); 
        string ImpMestStockName = ""; 
        public Mrs00158Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00158Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            try
            {
                filter = ((Mrs00158Filter)reportFilter); 
                if (filter.IS_MATERIAL == filter.IS_MEDICINE == null || filter.IS_MATERIAL == filter.IS_MEDICINE == false) filter.IS_MEDICINE = filter.IS_MATERIAL = true; 
                //get dữ liệu:
                //Danh sách kho
                listMediStockViews = new HisMediStockManager(paramGet).GetView(new HisMediStockViewFilterQuery()); 
                var listImpMestDepartment = listMediStockViews.Where(s => filter.IMP_MEDI_STOCK_IDs.Contains(s.ID)).ToList(); 
                ImpMestStockName = string.Join(", ", listImpMestDepartment.Select(s => s.DEPARTMENT_NAME)); 
                Inventec.Common.Logging.LogSystem.Info("ImpMestStockName" + ImpMestStockName); 
                List<long> listChmsExpMestTypeId = new List<long>()
                {
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK,
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS,
                    //IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HCS
                }; 
                //Xuất chuyển kho
                var metyFilterHisChmsExpMest = new HisExpMestViewFilterQuery
                {
                    FINISH_DATE_FROM = filter.EXP_TIME_FROM,
                    FINISH_DATE_TO = filter.EXP_TIME_TO,
                    MEDI_STOCK_IDs = filter.MEDI_STOCK_IDs,
                    //IMP_MEDI_STOCK_IDs = filter.IMP_MEDI_STOCK_IDs,
                    EXP_MEST_TYPE_IDs = listChmsExpMestTypeId
                }; 
                listChmsExpMest = new HisExpMestManager(paramGet).GetView(metyFilterHisChmsExpMest); 
                if (IsNotNullOrEmpty(filter.IMP_MEDI_STOCK_IDs))
                    listChmsExpMest = listChmsExpMest.Where(o => filter.IMP_MEDI_STOCK_IDs.Contains(o.IMP_MEDI_STOCK_ID??0)).ToList(); 
                Inventec.Common.Logging.LogSystem.Info("listChmsExpMest" + listChmsExpMest.Count.ToString()); 

                var listExpMestIDs = listChmsExpMest.Select(s => s.ID).ToList(); 
                //Nhập thu hồi
                if (IsNotNullOrEmpty(listExpMestIDs))
                {
                    var skip = 0; 
                    while (listExpMestIDs.Count - skip > 0)
                    {
                        var listIds = listExpMestIDs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        var listHisMobaImpMestfilter = new HisImpMestViewFilterQuery
                        {
                            MOBA_EXP_MEST_IDs = listIds,
                        }; 
                        var listHisMobaImpMestSub = new HisImpMestManager(paramGet).GetView(listHisMobaImpMestfilter); 
                        ListHisMobaImpMest.AddRange(listHisMobaImpMestSub); 
                    }
                }
                Inventec.Common.Logging.LogSystem.Info("ListHisMobaImpMest" + ListHisMobaImpMest.Count.ToString()); 

                var ListImpMestId = ListHisMobaImpMest.Select(o => o.ID).ToList(); 
                //if (filter.IS_MEDICINE??false)
                {
                    //Thuốc xuất
                    if (IsNotNullOrEmpty(listExpMestIDs))
                    {
                        var skip = 0; 
                        while (listExpMestIDs.Count - skip > 0)
                        {
                            var listIds = listExpMestIDs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                            var metyFilterExpMestMedicine = new HisExpMestMedicineViewFilterQuery
                            {
                                EXP_MEST_IDs = listIds,
                                IS_EXPORT = true
                            }; 
                            var expMestMedicineViews = new HisExpMestMedicineManager(paramGet).GetView(metyFilterExpMestMedicine); 
                            ListExpMestMedicine.AddRange(expMestMedicineViews); 
                        }
                    }

                    //Thuốc nhập
                    if (IsNotNullOrEmpty(ListImpMestId))
                    {
                        var skip = 0; 
                        while (ListImpMestId.Count - skip > 0)
                        {
                            var listIds = ListImpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                            var metyFilterImpMestMedicine = new HisImpMestMedicineViewFilterQuery
                            {
                                IMP_MEST_IDs = listIds,
                            }; 
                            var impMestMedicineViews = new HisImpMestMedicineManager(paramGet).GetView(metyFilterImpMestMedicine); 
                            ListImpMestMedicine.AddRange(impMestMedicineViews); 
                        }
                    }
                }
                //if (filter.IS_MATERIAL ?? false)
                {
                    //Vật tư xuất
                    if (IsNotNullOrEmpty(listExpMestIDs))
                    {
                        var skip = 0; 
                        while (listExpMestIDs.Count - skip > 0)
                        {
                            var listIds = listExpMestIDs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                            var metyFilterExpMestMaterial = new HisExpMestMaterialViewFilterQuery
                            {
                                EXP_MEST_IDs = listIds,
                                IS_EXPORT = true
                            }; 
                            var expMestMaterialViews = new HisExpMestMaterialManager(paramGet).GetView(metyFilterExpMestMaterial); 
                            ListExpMestMaterial.AddRange(expMestMaterialViews); 
                        }
                    }

                    //Vật tư nhập
                    if (IsNotNullOrEmpty(ListImpMestId))
                    {
                        var skip = 0; 
                        while (ListImpMestId.Count - skip > 0)
                        {
                            var listIds = ListImpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                            var metyFilterImpMestMaterial = new HisImpMestMaterialViewFilterQuery
                            {
                                IMP_MEST_IDs = listIds,
                            }; 
                            var impMestMaterialViews = new HisImpMestMaterialManager(paramGet).GetView(metyFilterImpMestMaterial); 
                            ListImpMestMaterial.AddRange(impMestMaterialViews); 
                        }
                    }
                }




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
                //DS thuốc xuất kho lẻ
               // if (filter.IS_MEDICINE ?? false)
                    ExpListMedicine(); 
                //DS vật tư xuất kho lẻ
              //  if (filter.IS_MATERIAL ?? false)
                    ExpListMaterial(); 


                result = true; 

            }


            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                ListRdo.Clear(); 

            }
            return result; 
        }



        private void ExpListMedicine()
        {
            try
            {
                foreach (var exp in ListExpMestMedicine)
                {
                    Inventec.Common.Logging.LogSystem.Info("ListExpMestMedicine" + ListExpMestMedicine.Count.ToString()); 

                    //thu hồi
                    List<long> listMobaImpMestSubId = listMobaImpMest.Where(o => o.MOBA_EXP_MEST_ID == exp.EXP_MEST_ID).Select(p => p.ID).ToList(); 
                    var listImpMestMedicineSub = IsNotNullOrEmpty(listMobaImpMestSubId) ? ListImpMestMedicine.Where(o => listMobaImpMestSubId.Contains(o.IMP_MEST_ID)).ToList() : null; 
                    var rdo = new Mrs00158RDO
                    {
                        IMP_MEST_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(exp.EXP_TIME.Value),

                        EXP_MEST_CODE = exp.EXP_MEST_CODE,
                        MEDICINE_TYPE_NAME = exp.MEDICINE_TYPE_NAME,
                        SERVICE_UNIT_NAME = exp.SERVICE_UNIT_NAME,
                        AMOUNT = exp.AMOUNT,
                        PRICE = exp.PRICE ?? exp.IMP_PRICE,
                        VAT = exp.IMP_VAT_RATIO,
                        MOBA = IsNotNullOrEmpty(listImpMestMedicineSub) ? listImpMestMedicineSub.Sum(s => s.AMOUNT) : 0,

                    }; 
                    ListRdo.Add(rdo); 
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
            }
        }
        private void ExpListMaterial()
        {
            try
            {
                foreach (var exp in ListExpMestMaterial)
                {
                    Inventec.Common.Logging.LogSystem.Info("ListExpMestMaterial" + ListExpMestMaterial.Count.ToString()); 

                    //thu hồi
                    List<long> listMobaImpMestSubId = listMobaImpMest.Where(o => o.MOBA_EXP_MEST_ID == exp.EXP_MEST_ID).Select(p => p.ID).ToList(); 
                    var listImpMestMaterialSub = IsNotNullOrEmpty(listMobaImpMestSubId) ? ListImpMestMaterial.Where(o => listMobaImpMestSubId.Contains(o.IMP_MEST_ID)).ToList() : null; 
                    var rdo = new Mrs00158RDO
                    {
                        IMP_MEST_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(exp.EXP_TIME.Value),

                        EXP_MEST_CODE = exp.EXP_MEST_CODE,
                        MEDICINE_TYPE_NAME = exp.MATERIAL_TYPE_NAME,
                        SERVICE_UNIT_NAME = exp.SERVICE_UNIT_NAME,
                        AMOUNT = exp.AMOUNT,
                        PRICE = exp.PRICE ?? exp.IMP_PRICE,
                        VAT = exp.IMP_VAT_RATIO,
                        MOBA = IsNotNullOrEmpty(listImpMestMaterialSub) ? listImpMestMaterialSub.Sum(s => s.AMOUNT) : 0,

                    }; 
                    ListRdo.Add(rdo); 
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
            }
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            filter = ((Mrs00158Filter)reportFilter); 
            if (filter.EXP_TIME_FROM > 0)
            {
                dicSingleTag.Add("EXP_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.EXP_TIME_FROM)); 
            }
            if (filter.EXP_TIME_TO > 0)
            {
                dicSingleTag.Add("EXP_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.EXP_TIME_TO)); 
            }

            Inventec.Common.Logging.LogSystem.Info("ListRdo" + ListRdo.Count.ToString()); 
            objectTag.AddObjectData(store, "Report", ListRdo); 


        }


    }
}