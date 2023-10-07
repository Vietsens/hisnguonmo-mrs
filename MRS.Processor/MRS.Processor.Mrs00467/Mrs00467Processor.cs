using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMestPeriodMedi;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisExpMestMedicine;
using AutoMapper; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
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
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00467
{
    class Mrs00467Processor : AbstractProcessor
    {
        Mrs00467Filter castFilter = null; 
        List<Mrs00467RDO> ListRdo = new List<Mrs00467RDO>(); 
        List<Mrs00467RDO> ListRdoGroup = new List<Mrs00467RDO>(); 
        List<Mrs00467RDO> ListRdoResult = new List<Mrs00467RDO>(); 
        List<Mrs00467RDO> ListRdoGroupResult = new List<Mrs00467RDO>(); 

        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines = new List<V_HIS_IMP_MEST_MEDICINE>(); 
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>(); 

        List<V_HIS_MEDI_STOCK_PERIOD> listMediStockPeriods = new List<V_HIS_MEDI_STOCK_PERIOD>(); 
        List<V_HIS_MEDICINE> listMedicines = new List<V_HIS_MEDICINE>(); 
        List<V_HIS_MEDICINE_TYPE> listMedicineTypes = new List<V_HIS_MEDICINE_TYPE>(); 

        public string MEDI_STOCK_NAME = ""; 
        public string MEDI_STOCK_CODE = ""; 



        public Mrs00467Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
        public override Type FilterType()
        {
            return typeof(Mrs00467Filter); 
        }


        protected override bool GetData()
        {
            CommonParam paramGet = new CommonParam(); 
            this.castFilter = ((Mrs00467Filter)reportFilter); 
            bool result = true; 
            try
            {
                HisMediStockViewFilterQuery mediStockFilter = new HisMediStockViewFilterQuery(); 
                mediStockFilter.ID = this.castFilter.MEDI_STOCK_ID; 
                var listMediStocks = new MOS.MANAGER.HisMediStock.HisMediStockManager(paramGet).GetView(mediStockFilter); 
                if (IsNotNullOrEmpty(listMediStocks))
                {
                    MEDI_STOCK_NAME = listMediStocks.First().MEDI_STOCK_NAME; 
                    MEDI_STOCK_CODE = listMediStocks.First().MEDI_STOCK_CODE; 
                }
                // Phieu nhap thuoc trong ky
                listImpMestMedicines = getImpMestMedicine(this.castFilter.TIME_FROM, this.castFilter.TIME_TO,this.castFilter.MEDI_STOCK_ID); 

                // Phieu xuat trong ky
                listExpMestMedicines = getExpMestMedicine(this.castFilter.TIME_FROM, this.castFilter.TIME_TO, this.castFilter.MEDI_STOCK_ID); 

                // Kiem ke gan nhat
                HisMediStockPeriodViewFilterQuery mediStockPeriodFilter = new HisMediStockPeriodViewFilterQuery(); 
                mediStockPeriodFilter.MEDI_STOCK_ID = this.castFilter.MEDI_STOCK_ID; 
                listMediStockPeriods = new MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodManager(paramGet).GetView(mediStockPeriodFilter); 
                listMediStockPeriods = listMediStockPeriods.Where(w => w.CREATE_TIME < this.castFilter.TIME_FROM).OrderByDescending(o => o.CREATE_TIME).ToList(); 

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
            CommonParam paramGet = new CommonParam(); 
            this.castFilter = ((Mrs00467Filter)reportFilter); 
            bool result = true; 
            try
            {
                //ListRdo nhap trong ky
                var listRdoImp = getRdoImp(listImpMestMedicines); 
                //ListRdo xuat trong ky
                var listRdoExp = getRdoExp(listExpMestMedicines); 
                //Ton dau ky
                var listRdoBegin = getMedicineBegin(listMediStockPeriods); 

                ListRdo.AddRange(listRdoImp); 
                ListRdo.AddRange(listRdoExp); 
                ListRdo.AddRange(listRdoBegin); 
                ListRdoGroup = ListRdo.GroupBy(gr => new { gr.MEDICINE_ID, gr.IMP_PRICE }).Select(s => new Mrs00467RDO
                {
                    MEDICINE_ID = s.First().MEDICINE_ID,
                    IMP_PRICE = s.First().IMP_PRICE,
                    BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT),
                    DELETE_AMOUNT = s.Sum(su => su.DELETE_AMOUNT),
                    EXP_AMOUNT = s.Sum(su => su.EXP_AMOUNT),
                    FINISH_AMOUNT = s.Sum(su => su.FINISH_AMOUNT),
                    IMP_AMOUNT = s.Sum(su => su.IMP_AMOUNT)
                }).ToList(); 
                var listMedicineIds = ListRdoGroup.Select(s => s.MEDICINE_ID).Distinct().ToList(); 
                var skip = 0; 
                while(listMedicineIds.Count - skip > 0)
                {
                    var listIds = listMedicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisMedicineViewFilterQuery filter = new HisMedicineViewFilterQuery(); 
                    filter.IDs = listIds; 
                    var medicines = new MOS.MANAGER.HisMedicine.HisMedicineManager(paramGet).GetView(filter); 
                    listMedicines.AddRange(medicines); 
                }
                var listMedicineTypeIds = listMedicines.Where(w=>w.MEDICINE_TYPE_ID!=null).Select(s=>s.MEDICINE_TYPE_ID).Distinct().ToList(); 
                skip = 0; 
                while(listMedicineTypeIds.Count - skip >0)
                {
                    var listIds = listMedicineTypeIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisMedicineTypeViewFilterQuery filter = new HisMedicineTypeViewFilterQuery(); 
                    filter.IDs = listIds; 
                    var medicineTypes = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(paramGet).GetView(filter); 
                    listMedicineTypes.AddRange(medicineTypes); 
                }
                ListRdoResult = (from Item1 in ListRdoGroup
                                join Item2 in listMedicines
                                on Item1.MEDICINE_ID equals Item2.ID 
                                join Item3 in listMedicineTypes
                                on Item2.MEDICINE_TYPE_ID equals Item3.ID
                                select new Mrs00467RDO {
                                    MEDICINE_TYPE_ID = Item3.ID,
                                    MEDICINE_TYPE_CODE=Item3.MEDICINE_TYPE_CODE,
                                    MEDICINE_TYPE_NAME=Item3.MEDICINE_TYPE_NAME,
                                    SERVICE_UNIT_NAME = Item2.SERVICE_UNIT_NAME,
                                    BEGIN_AMOUNT=Item1.BEGIN_AMOUNT,
                                    DELETE_AMOUNT=Item1.DELETE_AMOUNT,
                                    EXP_AMOUNT = Item1.EXP_AMOUNT,
                                    FINISH_AMOUNT = Item1.FINISH_AMOUNT,
                                    IMP_AMOUNT = Item1.IMP_AMOUNT,
                                    IMP_PRICE = Item1.IMP_PRICE,
                                    MANUFACTURER_NAME = Item3.MANUFACTURER_NAME,
                                    
                                }).ToList(); 
                ListRdoGroupResult = ListRdoResult.GroupBy(gr => new
                {
                    gr.MEDICINE_TYPE_ID,
                    gr.MEDICINE_TYPE_CODE,
                    gr.MEDICINE_TYPE_NAME,
                    gr.SERVICE_UNIT_NAME,
                    gr.MANUFACTURER_NAME,
                    gr.IMP_PRICE
                }).Select(s => new Mrs00467RDO
                {
                    MEDICINE_TYPE_CODE = s.First().MEDICINE_TYPE_CODE,
                    MEDICINE_TYPE_NAME = s.First().MEDICINE_TYPE_NAME,
                    SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                    MANUFACTURER_NAME = s.First().MANUFACTURER_NAME,
                    IMP_PRICE = s.First().IMP_PRICE,
                    BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT),
                    DELETE_AMOUNT = s.Sum(su => su.DELETE_AMOUNT),
                    EXP_AMOUNT = s.Sum(su => su.EXP_AMOUNT),
                    FINISH_AMOUNT = s.Sum(su => su.FINISH_AMOUNT),
                    IMP_AMOUNT = s.Sum(su => su.IMP_AMOUNT)
                }).ToList(); 
                result = true; 
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
                if (((Mrs00467Filter)reportFilter).TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00467Filter)reportFilter).TIME_FROM)); 
                }
                if (((Mrs00467Filter)reportFilter).TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00467Filter)reportFilter).TIME_TO)); 
                }

                dicSingleTag.Add("DEPARTMENT_NAME", MEDI_STOCK_NAME); 
                dicSingleTag.Add("DEPARTMENT_CODE", MEDI_STOCK_CODE); 

                bool exportSuccess = true; 
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Rdo", ListRdoGroupResult.OrderBy(o => o.MEDICINE_TYPE_NAME).ToList()); 
                //exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Group", ListRdoGroup.OrderBy(s => s.DEPARTMENT_NAME).ToList()); 
                //exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group", "Rdo", "DEPARTMENT_ID", "DEPARTMENT_ID"); 
                exportSuccess = exportSuccess && store.SetCommonFunctions(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
        //Ton dau ky 
        private List<Mrs00467RDO> getMedicineBegin(List<V_HIS_MEDI_STOCK_PERIOD> listMediStockPeriods)
        {
            try
            {
                CommonParam paramGet = new CommonParam(); 
                this.castFilter = ((Mrs00467Filter)reportFilter); 
                List<Mrs00467RDO> listRdoBegin = new List<Mrs00467RDO>(); 
                List<Mrs00467RDO> listResult = new List<Mrs00467RDO>(); 

                if (IsNotNullOrEmpty(listMediStockPeriods))
                {
                    //Kiem ke thuoc
                    HisMestPeriodMediViewFilterQuery mestPeriodMediFilter = new HisMestPeriodMediViewFilterQuery(); 
                    mestPeriodMediFilter.MEDI_STOCK_PERIOD_ID = listMediStockPeriods.Select(s => s.ID).First(); 
                    var listMestPeriodMedis = new MOS.MANAGER.HisMestPeriodMedi.HisMestPeriodMediManager(paramGet).GetView(mestPeriodMediFilter); 
                    if (IsNotNullOrEmpty(listMestPeriodMedis))
                    {
                        // Ton dau ky
                        foreach (var mestPeriodMedi in listMestPeriodMedis)
                        {
                            Mrs00467RDO rdo = new Mrs00467RDO(); 
                            rdo.IMP_AMOUNT = 0 ; 
                            rdo.EXP_AMOUNT = 0 ; 
                            rdo.BEGIN_AMOUNT = mestPeriodMedi.AMOUNT; 
                            rdo.MEDICINE_ID = mestPeriodMedi.MEDICINE_TYPE_ID; 

                            listRdoBegin.Add(rdo); 
                        }

                    }

                    //Nhap truoc ky
                    var listImpMestMedicineBegin = getImpMestMedicine(listMediStockPeriods.Select(s => s.CREATE_TIME).First() ?? 0, this.castFilter.TIME_FROM - 1, this.castFilter.MEDI_STOCK_ID); 
                    listRdoBegin.AddRange(getRdoImp(listImpMestMedicineBegin)); 
                    //Xuat truoc ky
                    var listExpMestMedicineBegin = getExpMestMedicine(listMediStockPeriods.Select(s => s.CREATE_TIME).First() ?? 0, this.castFilter.TIME_FROM - 1, this.castFilter.MEDI_STOCK_ID); 
                    listRdoBegin.AddRange(getRdoExp(listExpMestMedicineBegin)); 

                    var listRdoBegin_ = listRdoBegin.GroupBy(gr => new { gr.MEDICINE_ID,gr.IMP_PRICE}).Select(s => new Mrs00467RDO
                    {
                        MEDICINE_ID = s.First().MEDICINE_ID,
                        BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT),
                        IMP_AMOUNT = s.Sum(su => su.IMP_AMOUNT),
                        EXP_AMOUNT = s.Sum(su => su.EXP_AMOUNT),
                        IMP_PRICE = s.First().IMP_PRICE
                    }).ToList(); 
                    
                    foreach (var rdoBegin in listRdoBegin_)
                    {
                        Mrs00467RDO rdo = new Mrs00467RDO(); 
                        rdo.BEGIN_AMOUNT = rdoBegin.BEGIN_AMOUNT + rdoBegin.IMP_AMOUNT - rdoBegin.EXP_AMOUNT; 
                        rdo.DELETE_AMOUNT = 0; 
                        rdo.EXP_AMOUNT = 0; 
                        rdo.FINISH_AMOUNT = 0; 
                        rdo.IMP_AMOUNT = 0; 
                        rdo.IMP_PRICE = rdoBegin.IMP_PRICE; 
                        rdo.MEDICINE_ID = rdoBegin.MEDICINE_ID; 
                        listResult.Add(rdo); 
                    }
                   
                }
                else 
                {
                    //Nhap truoc ky
                    var listImpMestMedicineBegin = getImpMestMedicine(0, this.castFilter.TIME_FROM - 1, this.castFilter.MEDI_STOCK_ID); 
                    listRdoBegin.AddRange(getRdoImp(listImpMestMedicineBegin)); 
                    //Xuat truoc ky
                    var listExpMestMedicineBegin = getExpMestMedicine(0, this.castFilter.TIME_FROM - 1, this.castFilter.MEDI_STOCK_ID); 
                    listRdoBegin.AddRange(getRdoExp(listExpMestMedicineBegin)); 

                    var listRdoBegin_ = listRdoBegin.GroupBy(gr => new { gr.MEDICINE_ID,gr.IMP_PRICE}).Select(s => new Mrs00467RDO
                    {
                        MEDICINE_ID = s.First().MEDICINE_ID,
                        IMP_AMOUNT = s.Sum(su => su.IMP_AMOUNT),
                        EXP_AMOUNT = s.Sum(su => su.EXP_AMOUNT),
                        IMP_PRICE = s.First().IMP_PRICE
                    }).ToList(); 
                    foreach (var rdoBegin in listRdoBegin_)
                    {
                        Mrs00467RDO rdo = new Mrs00467RDO(); 
                        rdo.BEGIN_AMOUNT = rdoBegin.IMP_AMOUNT - rdoBegin.EXP_AMOUNT; 
                        rdo.DELETE_AMOUNT = 0; 
                        rdo.EXP_AMOUNT = 0; 
                        rdo.FINISH_AMOUNT = 0; 
                        rdo.IMP_AMOUNT = 0; 
                        rdo.IMP_PRICE = rdoBegin.IMP_PRICE; 
                        rdo.MEDICINE_ID = rdoBegin.MEDICINE_ID; 
                        listResult.Add(rdo); 
                    }
                }
                return listResult; 

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
            return null; 
        }

        //Nhap
        private List<V_HIS_IMP_MEST_MEDICINE> getImpMestMedicine(long time_from, long time_to, long medi_stock_id )
        {
            try
            {
                CommonParam paramGet = new CommonParam(); 

                HisImpMestMedicineViewFilterQuery impMestMedicineFilter = new HisImpMestMedicineViewFilterQuery(); 
                impMestMedicineFilter.IMP_TIME_FROM = time_from; 
                impMestMedicineFilter.IMP_TIME_TO = time_to; 
                impMestMedicineFilter.MEDI_STOCK_ID = medi_stock_id; 
                impMestMedicineFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE; 

                var impMestMedicines = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(impMestMedicineFilter); 
                return impMestMedicines; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
            return null; 
        }

        //Xuat
        private List<V_HIS_EXP_MEST_MEDICINE> getExpMestMedicine(long time_from, long time_to, long medi_stock_id)
        {
            try
            {
                CommonParam paramGet = new CommonParam(); 

                HisExpMestMedicineViewFilterQuery expMestMedicineFilter = new HisExpMestMedicineViewFilterQuery(); 
                expMestMedicineFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE; 
                expMestMedicineFilter.EXP_TIME_FROM = time_from; 
                expMestMedicineFilter.EXP_TIME_TO = time_to;
                expMestMedicineFilter.MEDI_STOCK_ID = medi_stock_id;
                expMestMedicineFilter.IS_EXPORT = true;
                var expMestMedicines = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(expMestMedicineFilter); 
                return expMestMedicines; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
            return null; 
        }

        //ListRdo nhap
        private List<Mrs00467RDO> getRdoImp(List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines)
        {
            try 
            {
                List<Mrs00467RDO> listRdoImp = new List<Mrs00467RDO>(); 
                List<Mrs00467RDO> listRdoImp_ = new List<Mrs00467RDO>(); 
                if (IsNotNullOrEmpty(listImpMestMedicines))
                {
                    foreach (var impMest in listImpMestMedicines)
                    {
                        Mrs00467RDO rdo = new Mrs00467RDO(); 
                        rdo.IMP_AMOUNT = impMest.AMOUNT; 
                        rdo.IMP_PRICE = impMest.IMP_PRICE; 
                        rdo.MEDICINE_ID = impMest.MEDICINE_ID; 
                        listRdoImp.Add(rdo); 
                    }
                    listRdoImp_ = listRdoImp.GroupBy(gr => new { gr.MEDICINE_ID, gr.IMP_PRICE }).Select(s => new Mrs00467RDO
                    {
                        MEDICINE_ID = s.First().MEDICINE_ID,
                        BEGIN_AMOUNT = 0,
                        DELETE_AMOUNT = 0,
                        EXP_AMOUNT = 0,
                        FINISH_AMOUNT = 0,
                        IMP_AMOUNT = s.Sum(su => su.IMP_AMOUNT),
                        IMP_PRICE = s.First().IMP_PRICE
                    }).ToList(); 
                }
                return listRdoImp_; 
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
            }
            return null; 
        }

        //ListRdo xuat
        private List<Mrs00467RDO> getRdoExp(List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines)
        {
            try
            {
                List<Mrs00467RDO> listRdoExp = new List<Mrs00467RDO>(); 
                List<Mrs00467RDO> listRdoExp_ = new List<Mrs00467RDO>(); 
                if (IsNotNullOrEmpty(listExpMestMedicines))
                {
                    foreach (var expMest in listExpMestMedicines)
                    {
                        Mrs00467RDO rdo = new Mrs00467RDO(); 
                        rdo.EXP_AMOUNT = expMest.AMOUNT;
                        rdo.MEDICINE_ID = expMest.MEDICINE_ID ?? 0; 
                        rdo.IMP_PRICE = expMest.IMP_PRICE; 

                        listRdoExp.Add(rdo); 
                    }
                    listRdoExp_ = listRdoExp.GroupBy(gr => new { gr.MEDICINE_ID, gr.IMP_PRICE }).Select(s => new Mrs00467RDO
                    {
                        MEDICINE_ID = s.First().MEDICINE_ID,
                        BEGIN_AMOUNT = 0,
                        DELETE_AMOUNT = 0,
                        EXP_AMOUNT = s.Sum(su=>su.EXP_AMOUNT),
                        FINISH_AMOUNT = 0,
                        IMP_AMOUNT = 0,
                        IMP_PRICE =s.First().IMP_PRICE

                    }).ToList(); 
                }
                return listRdoExp_; 
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
            }
            return null; 
        }

    }

}
