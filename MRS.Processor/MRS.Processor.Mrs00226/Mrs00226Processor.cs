using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestType;
using MOS.MANAGER.HisExpMestStt;
using AutoMapper; 
using FlexCel.Report; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisActiveIngredient; 
using MOS.MANAGER.HisExpMestMaterial; 
using MOS.MANAGER.HisExpMestMedicine; 
using MOS.MANAGER.HisIcd; 
using MOS.MANAGER.HisImpMestMaterial; 
using MOS.MANAGER.HisImpMestMedicine; 
using MOS.MANAGER.HisInvoice; 
using MOS.MANAGER.HisInvoiceDetail; 
using MOS.MANAGER.HisMaterialType; 
using MOS.MANAGER.HisMedicineType; 
using MOS.MANAGER.HisMedicineTypeAcin; 
using MOS.MANAGER.HisMediStock; 
using MOS.MANAGER.HisMediStockPeriod; 
using MOS.MANAGER.HisMestPeriodMate; 
using MOS.MANAGER.HisMestPeriodMedi; 
using MOS.MANAGER.HisPatient; 
using MOS.MANAGER.HisPatientType; 
using MOS.MANAGER.HisReportTypeCat; 
using MOS.MANAGER.HisSereServ; 
using MOS.MANAGER.HisService; 
using MOS.MANAGER.HisServiceRetyCat; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00226
{
    public class Mrs00226Processor : AbstractProcessor
    {
        private List<Mrs00226RDO> listMrs00226RDOs = new List<Mrs00226RDO>(); 
        CommonParam paramGet = new CommonParam(); //CastFilter = (Mrs00157Filter)this.reportFilter; 
        List<Mrs00226RDO> ListRdo = new List<Mrs00226RDO>(); 

        List<Mrs00226RDO> listRdoMedicine = new List<Mrs00226RDO>(); 
        List<Mrs00226RDO> listRdoMaterial = new List<Mrs00226RDO>(); 

        List<V_HIS_MEDI_STOCK> ListMediStock = new List<V_HIS_MEDI_STOCK>(); 
        List<V_HIS_MEDICINE_TYPE_ACIN> ListMedicineTypeAcin = new List<V_HIS_MEDICINE_TYPE_ACIN>(); 

        V_HIS_MEDI_STOCK CurrentMediStock = new V_HIS_MEDI_STOCK(); 

        const string IMP_MANU = "MANU"; //Nhập từ nhà cung cấp
        const string IMP_CHMS = "CHMS"; //Nhập chuyển kho
        const string IMP_MOBA = "MOBA"; //Nhập thu hồi
        const string EXP_PRES = "PRES"; //Xuất đơn thuốc
        const string EXP_DEPA = "DEPA"; //Xuất khoa phòng
        const string EXP_CHMS = "CHMS"; //Xuất Chuyển kho
        const string EXP_MANU = "MANU"; //Xuất trả nhà cung cấp
        const string EXP_EXPE = "EXPE"; //Xuất hao phí
        const string EXP_LOST = "LOST"; //Xuất mất mát
        const string EXP_SALE = "SALE"; //Xuất bán
        const string EXP_LIQU = "LIQU"; //Xuất thanh lý
        const string EXP_OTHER = "OTHER"; //Xuất khác
        private string a = ""; 
        public Mrs00226Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            a = reportTypeCode; 
        }

        public override Type FilterType()
        {
            return typeof(Mrs00226Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            try
            {
                var paramGet = new CommonParam(); //CastFilter = (Mrs00157Filter)this.reportFilter; 
                //get dữ liệu
                HisMedicineTypeAcinViewFilterQuery AcinGrefilter = new HisMedicineTypeAcinViewFilterQuery(); 
                ListMedicineTypeAcin = new HisMedicineTypeAcinManager(paramGet).GetView(AcinGrefilter); 
                if (IsNotNullOrEmpty(((Mrs00226Filter)this.reportFilter).MEDI_STOCK_IDs))
                {
                    HisMediStockViewFilterQuery mediStockFilter = new HisMediStockViewFilterQuery(); 
                    mediStockFilter.IDs = ((Mrs00226Filter)this.reportFilter).MEDI_STOCK_IDs; 
                    ListMediStock = new HisMediStockManager(paramGet).GetView(mediStockFilter); 

                    ProcessData(); 
                }
                else
                {
                    throw new DataMisalignedException("Filter khong truyen len MEDI_STOCK ID"); 
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
                if (IsNotNullOrEmpty(ListMediStock))
                {
                    CommonParam paramGet = new CommonParam(); //CastFilter = (Mrs00157Filter)this.reportFilter; 
                    foreach (var medistock in ListMediStock)
                    {
                        CurrentMediStock = medistock; 
                        HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery(); 
                        impMediFilter.IMP_TIME_FROM = ((Mrs00226Filter)this.reportFilter).TIME_FROM; 
                        impMediFilter.IMP_TIME_TO = ((Mrs00226Filter)this.reportFilter).TIME_TO; 
                        impMediFilter.MEDI_STOCK_ID = medistock.ID; 
                        impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; 
                        List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new HisImpMestMedicineManager().GetView(impMediFilter); 

                        HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery(); 
                        expMediFilter.EXP_TIME_FROM = ((Mrs00226Filter)this.reportFilter).TIME_FROM; 
                        expMediFilter.EXP_TIME_TO = ((Mrs00226Filter)this.reportFilter).TIME_TO; 
                        expMediFilter.MEDI_STOCK_ID = medistock.ID; 
                        expMediFilter.IS_EXPORT = true; 
                        //HisExpMestSttCFG.EXP_MEST_STT_ID__EXPORTED; 
                        //expMediFilter.IN_EXECUTE = true; 
                        List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new HisExpMestMedicineManager().GetView(expMediFilter); 

                        HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery(); 
                        impMateFilter.IMP_TIME_FROM = ((Mrs00226Filter)this.reportFilter).TIME_FROM; 
                        impMateFilter.IMP_TIME_TO = ((Mrs00226Filter)this.reportFilter).TIME_TO; 
                        impMateFilter.MEDI_STOCK_ID = medistock.ID; 
                        impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; 
                        List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new HisImpMestMaterialManager().GetView(impMateFilter); 

                        HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery(); 
                        expMateFilter.EXP_TIME_FROM = ((Mrs00226Filter)this.reportFilter).TIME_FROM; 
                        expMateFilter.EXP_TIME_TO = ((Mrs00226Filter)this.reportFilter).TIME_TO; 
                        expMateFilter.MEDI_STOCK_ID = medistock.ID; 
                        expMateFilter.IS_EXPORT = true; 
                        //HisExpMestSttCFG.EXP_MEST_STT_ID__EXPORTED; 
                        //expMateFilter.IN_EXECUTE = true; 
                        List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new HisExpMestMaterialManager().GetView(expMateFilter); 
                        if (!paramGet.HasException)
                        {
                            ProcessAmountMedicine(hisImpMestMedicine, hisExpMestMedicine); 
                            ProcessAmountMaterial(hisImpMestMaterial, hisExpMestMaterial); 
                            ProcessGetPeriod(paramGet); 

                            #region listRdoMedicine
                            listRdoMedicine = listRdoMedicine.GroupBy(g => g.MEDI_MATE_ID).Select(s =>
                                new Mrs00226RDO
                                {
                                    ACTIVE_INGREDIENT_ID = s.First().ACTIVE_INGREDIENT_ID,
                                    ACTIVE_INGREDIENT_NAME = s.First().ACTIVE_INGREDIENT_NAME,
                                    SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                                    SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                                    MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                                    MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                                    MEDI_MATE_ID = s.First().MEDI_MATE_ID,
                                    MEDI_MATE_IM_ID = s.First().MEDI_MATE_IM_ID,
                                    REPORT_TYPE_CAT_ID = s.First().REPORT_TYPE_CAT_ID,
                                    SUPPLIER_ID = s.First().SUPPLIER_ID,
                                    SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                                    SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                                    IMP_PRICE = s.First().IMP_PRICE,
                                    SERVICE_ID = s.First().SERVICE_ID,
                                    SERVICE_CODE = s.First().SERVICE_CODE,
                                    SERVICE_NAME = s.First().SERVICE_NAME,
                                    SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                                    BEGIN_AMOUNT = s.Sum(s1 => s1.BEGIN_AMOUNT),
                                    IMP_MANU_AMOUNT = s.Sum(s2 => s2.IMP_MANU_AMOUNT),
                                    IMP_CHMS_AMOUNT = s.Sum(s3 => s3.IMP_CHMS_AMOUNT),
                                    IMP_MOBA_AMOUNT = s.Sum(s4 => s4.IMP_MOBA_AMOUNT),
                                    EXP_PRES_AMOUNT = s.Sum(s5 => s5.EXP_PRES_AMOUNT),
                                    EXP_DEPA_AMOUNT = s.Sum(s6 => s6.EXP_DEPA_AMOUNT),
                                    EXP_CHMS_AMOUNT = s.Sum(s7 => s7.EXP_CHMS_AMOUNT),
                                    EXP_MANU_AMOUNT = s.Sum(s8 => s8.EXP_MANU_AMOUNT),
                                    EXP_EXPE_AMOUNT = s.Sum(s9 => s9.EXP_EXPE_AMOUNT),
                                    EXP_LOST_AMOUNT = s.Sum(s10 => s10.EXP_LOST_AMOUNT),
                                    EXP_SALE_AMOUNT = s.Sum(s11 => s11.EXP_SALE_AMOUNT),
                                    EXP_LIQU_AMOUNT = s.Sum(s12 => s12.EXP_LIQU_AMOUNT),
                                    EXP_OTHER_AMOUNT = s.Sum(s13 => s13.EXP_OTHER_AMOUNT)
                                }).Where(o =>
                                    o.BEGIN_AMOUNT > 0 ||
                                    o.IMP_MANU_AMOUNT > 0 ||
                                    o.IMP_CHMS_AMOUNT > 0 ||
                                    o.IMP_MOBA_AMOUNT > 0 ||
                                    o.EXP_PRES_AMOUNT > 0 ||
                                    o.EXP_DEPA_AMOUNT > 0 ||
                                    o.EXP_CHMS_AMOUNT > 0 ||
                                    o.EXP_MANU_AMOUNT > 0 ||
                                    o.EXP_EXPE_AMOUNT > 0 ||
                                    o.EXP_LOST_AMOUNT > 0 ||
                                    o.EXP_SALE_AMOUNT > 0 ||
                                    o.EXP_LIQU_AMOUNT > 0 ||
                                    o.EXP_OTHER_AMOUNT > 0
                                    ).ToList(); 

                            listRdoMedicine = listRdoMedicine.GroupBy(g =>
                                new { g.SERVICE_ID, g.SUPPLIER_ID, g.IMP_PRICE }).Select(s =>
                                    new Mrs00226RDO
                                    {
                                        ACTIVE_INGREDIENT_ID = s.First().ACTIVE_INGREDIENT_ID,
                                        ACTIVE_INGREDIENT_NAME = s.First().ACTIVE_INGREDIENT_NAME,
                                        SUPPLIER_ID = s.First().SUPPLIER_ID,
                                        SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                                        SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                                        REPORT_TYPE_CAT_ID = s.First().REPORT_TYPE_CAT_ID,
                                        SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                                        SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                                        MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                                        MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                                        IMP_PRICE = s.First().IMP_PRICE,
                                        SERVICE_ID = s.First().SERVICE_ID,
                                        SERVICE_CODE = s.First().SERVICE_CODE,
                                        SERVICE_NAME = s.First().SERVICE_NAME,
                                        SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                                        BEGIN_AMOUNT = s.Sum(s1 => s1.BEGIN_AMOUNT),
                                        IMP_MANU_AMOUNT = s.Sum(s2 => s2.IMP_MANU_AMOUNT),
                                        IMP_CHMS_AMOUNT = s.Sum(s3 => s3.IMP_CHMS_AMOUNT),
                                        IMP_MOBA_AMOUNT = s.Sum(s4 => s4.IMP_MOBA_AMOUNT),
                                        EXP_PRES_AMOUNT = s.Sum(s5 => s5.EXP_PRES_AMOUNT),
                                        EXP_DEPA_AMOUNT = s.Sum(s6 => s6.EXP_DEPA_AMOUNT),
                                        EXP_CHMS_AMOUNT = s.Sum(s7 => s7.EXP_CHMS_AMOUNT),
                                        EXP_MANU_AMOUNT = s.Sum(s8 => s8.EXP_MANU_AMOUNT),
                                        EXP_EXPE_AMOUNT = s.Sum(s9 => s9.EXP_EXPE_AMOUNT),
                                        EXP_LOST_AMOUNT = s.Sum(s10 => s10.EXP_LOST_AMOUNT),
                                        EXP_SALE_AMOUNT = s.Sum(s11 => s11.EXP_SALE_AMOUNT),
                                        EXP_LIQU_AMOUNT = s.Sum(s12 => s12.EXP_LIQU_AMOUNT),
                                        EXP_OTHER_AMOUNT = s.Sum(s13 => s13.EXP_OTHER_AMOUNT),
                                        MEDI_MATE_IM_ID = s.First().MEDI_MATE_IM_ID
                                    }).ToList(); 
                            #endregion

                            #region listRdoMaterial
                            listRdoMaterial = listRdoMaterial.GroupBy(g => g.MEDI_MATE_ID).Select(s =>
                                new Mrs00226RDO
                                {
                                    ACTIVE_INGREDIENT_ID = s.First().ACTIVE_INGREDIENT_ID,
                                    ACTIVE_INGREDIENT_NAME = s.First().ACTIVE_INGREDIENT_NAME,
                                    MEDI_MATE_ID = s.First().MEDI_MATE_ID,
                                    MEDI_MATE_IM_ID = s.First().MEDI_MATE_IM_ID,
                                    REPORT_TYPE_CAT_ID = s.First().REPORT_TYPE_CAT_ID,
                                    SUPPLIER_ID = s.First().SUPPLIER_ID,
                                    SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                                    SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                                    SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                                    MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                                    MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                                    SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                                    IMP_PRICE = s.First().IMP_PRICE,
                                    SERVICE_ID = s.First().SERVICE_ID,
                                    SERVICE_CODE = s.First().SERVICE_CODE,
                                    SERVICE_NAME = s.First().SERVICE_NAME,
                                    SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                                    BEGIN_AMOUNT = s.Sum(s1 => s1.BEGIN_AMOUNT),
                                    IMP_MANU_AMOUNT = s.Sum(s2 => s2.IMP_MANU_AMOUNT),
                                    IMP_CHMS_AMOUNT = s.Sum(s3 => s3.IMP_CHMS_AMOUNT),
                                    IMP_MOBA_AMOUNT = s.Sum(s4 => s4.IMP_MOBA_AMOUNT),
                                    EXP_PRES_AMOUNT = s.Sum(s5 => s5.EXP_PRES_AMOUNT),
                                    EXP_DEPA_AMOUNT = s.Sum(s6 => s6.EXP_DEPA_AMOUNT),
                                    EXP_CHMS_AMOUNT = s.Sum(s7 => s7.EXP_CHMS_AMOUNT),
                                    EXP_MANU_AMOUNT = s.Sum(s8 => s8.EXP_MANU_AMOUNT),
                                    EXP_EXPE_AMOUNT = s.Sum(s9 => s9.EXP_EXPE_AMOUNT),
                                    EXP_LOST_AMOUNT = s.Sum(s10 => s10.EXP_LOST_AMOUNT),
                                    EXP_SALE_AMOUNT = s.Sum(s11 => s11.EXP_SALE_AMOUNT),
                                    EXP_LIQU_AMOUNT = s.Sum(s12 => s12.EXP_LIQU_AMOUNT),
                                    EXP_OTHER_AMOUNT = s.Sum(s13 => s13.EXP_OTHER_AMOUNT)
                                }).Where(o =>
                                    o.BEGIN_AMOUNT > 0 ||
                                    o.IMP_MANU_AMOUNT > 0 ||
                                    o.IMP_CHMS_AMOUNT > 0 ||
                                    o.IMP_MOBA_AMOUNT > 0 ||
                                    o.EXP_PRES_AMOUNT > 0 ||
                                    o.EXP_DEPA_AMOUNT > 0 ||
                                    o.EXP_CHMS_AMOUNT > 0 ||
                                    o.EXP_MANU_AMOUNT > 0 ||
                                    o.EXP_EXPE_AMOUNT > 0 ||
                                    o.EXP_LOST_AMOUNT > 0 ||
                                    o.EXP_SALE_AMOUNT > 0 ||
                                    o.EXP_LIQU_AMOUNT > 0 ||
                                    o.EXP_OTHER_AMOUNT > 0
                                    ).ToList(); 

                            listRdoMaterial = listRdoMaterial.GroupBy(g =>
                                new { g.SERVICE_ID, g.SUPPLIER_ID, g.IMP_PRICE }).Select(s =>
                                    new Mrs00226RDO
                                    {
                                        ACTIVE_INGREDIENT_ID = s.First().ACTIVE_INGREDIENT_ID,
                                        ACTIVE_INGREDIENT_NAME = s.First().ACTIVE_INGREDIENT_NAME,
                                        SERVICE_ID = s.First().SERVICE_ID,
                                        REPORT_TYPE_CAT_ID = s.First().REPORT_TYPE_CAT_ID,
                                        SUPPLIER_ID = s.First().SUPPLIER_ID,
                                        SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                                        SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                                        MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                                        MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                                        SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                                        IMP_PRICE = s.First().IMP_PRICE,
                                        SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                                        SERVICE_CODE = s.First().SERVICE_CODE,
                                        SERVICE_NAME = s.First().SERVICE_NAME,
                                        SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                                        BEGIN_AMOUNT = s.Sum(s1 => s1.BEGIN_AMOUNT),
                                        IMP_MANU_AMOUNT = s.Sum(s2 => s2.IMP_MANU_AMOUNT),
                                        IMP_CHMS_AMOUNT = s.Sum(s3 => s3.IMP_CHMS_AMOUNT),
                                        IMP_MOBA_AMOUNT = s.Sum(s4 => s4.IMP_MOBA_AMOUNT),
                                        EXP_PRES_AMOUNT = s.Sum(s5 => s5.EXP_PRES_AMOUNT),
                                        EXP_DEPA_AMOUNT = s.Sum(s6 => s6.EXP_DEPA_AMOUNT),
                                        EXP_CHMS_AMOUNT = s.Sum(s7 => s7.EXP_CHMS_AMOUNT),
                                        EXP_MANU_AMOUNT = s.Sum(s8 => s8.EXP_MANU_AMOUNT),
                                        EXP_EXPE_AMOUNT = s.Sum(s9 => s9.EXP_EXPE_AMOUNT),
                                        EXP_LOST_AMOUNT = s.Sum(s10 => s10.EXP_LOST_AMOUNT),
                                        EXP_SALE_AMOUNT = s.Sum(s11 => s11.EXP_SALE_AMOUNT),
                                        EXP_LIQU_AMOUNT = s.Sum(s12 => s12.EXP_LIQU_AMOUNT),
                                        EXP_OTHER_AMOUNT = s.Sum(s13 => s13.EXP_OTHER_AMOUNT),
                                        MEDI_MATE_IM_ID = s.First().MEDI_MATE_IM_ID
                                    }).ToList(); 
                            #endregion

                            ListRdo.AddRange(listRdoMedicine); 
                            ListRdo.AddRange(listRdoMaterial); 
                        }
                        else
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00226."); 
                        }
                        //ListRdo=ListRdo.GroupBy(o=>o.
                    }
                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00226."); 
                    }
                }
                result = true; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                ListRdo.Clear(); 
                ListMediStock.Clear(); 
            }
            return result; 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            ListRdo = ListRdo.GroupBy(o => o.MEDI_MATE_IM_ID).Select(p => p.First()).ToList(); 
            ListRdo = ListRdo.OrderBy(o => o.MEDI_STOCK_ID).ThenBy(t1 => t1.SERVICE_TYPE_ID).ThenBy(t4 => t4.ACTIVE_INGREDIENT_ID).ThenByDescending(t2 => t2.NUM_ORDER).ThenBy(t3 => t3.SERVICE_NAME).ToList(); 
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00226Filter)this.reportFilter).TIME_FROM)); 
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00226Filter)this.reportFilter).TIME_TO)); 
            List<Mrs00226RDO> ListActiveGredient = new List<Mrs00226RDO>(); 
            ListActiveGredient = ListRdo.GroupBy(o => new { MEDI_STOCK_ID = o.MEDI_STOCK_ID, ACTIVE_INGREDIENT_ID = o.ACTIVE_INGREDIENT_ID }).Select(p => p.First()).ToList(); 
            objectTag.AddObjectData(store, "MediStocks", ListMediStock); 
            objectTag.AddObjectData(store, "Services", ListRdo); 
            objectTag.AddRelationship(store, "MediStocks", "Services", "ID", "MEDI_STOCK_ID"); 
            objectTag.SetUserFunction(store, "FuncSameTitleCol1", new CustomerFuncMergeSameData1()); 
            objectTag.SetUserFunction(store, "FuncSameTitleCol2", new CustomerFuncMergeSameData2()); 
            objectTag.SetUserFunction(store, "FuncRownumber", new RDOCustomerFuncManyRownumberData()); 
        }

        //private void ProcessListMediStock()
        //{
        //    try
        //    {
        //        if (IsNotNullOrEmpty(ListMediStock))
        //        {
        //            CommonParam paramGet = new CommonParam(); //CastFilter = (Mrs00157Filter)this.reportFilter; 
        //            foreach (var medistock in ListMediStock)
        //            {
        //                CurrentMediStock = medistock; 
        //                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery(); 
        //                impMediFilter.IMP_TIME_FROM = ((Mrs00226Filter)this.reportFilter).TIME_FROM; 
        //                impMediFilter.IMP_TIME_TO = ((Mrs00226Filter)this.reportFilter).TIME_TO; 
        //                impMediFilter.MEDI_STOCK_ID = medistock.ID; 
        //                impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; 
        //                List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new HisImpMestMedicineManager().GetView(impMediFilter); 

        //                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery(); 
        //                expMediFilter.EXP_TIME_FROM = ((Mrs00226Filter)this.reportFilter).TIME_FROM; 
        //                expMediFilter.EXP_TIME_TO = ((Mrs00226Filter)this.reportFilter).TIME_TO; 
        //                expMediFilter.MEDI_STOCK_ID = medistock.ID; 
        //                expMediFilter.EXP_MEST_STT_ID = HisExpMestSttCFG.EXP_MEST_STT_ID__EXPORTED; 
        //                expMediFilter.IN_EXECUTE = true; 
        //                List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new HisExpMestMedicineManager().GetView(expMediFilter); 

        //                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery(); 
        //                impMateFilter.IMP_TIME_FROM = ((Mrs00226Filter)this.reportFilter).TIME_FROM; 
        //                impMateFilter.IMP_TIME_TO = ((Mrs00226Filter)this.reportFilter).TIME_TO; 
        //                impMateFilter.MEDI_STOCK_ID = medistock.ID; 
        //                impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; 
        //                List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new HisImpMestMaterialManager().GetView(impMateFilter); 

        //                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery(); 
        //                expMateFilter.EXP_TIME_FROM = ((Mrs00226Filter)this.reportFilter).TIME_FROM; 
        //                expMateFilter.EXP_TIME_TO = ((Mrs00226Filter)this.reportFilter).TIME_TO; 
        //                expMateFilter.MEDI_STOCK_ID = medistock.ID; 
        //                expMateFilter.EXP_MEST_STT_ID = HisExpMestSttCFG.EXP_MEST_STT_ID__EXPORTED; 
        //                expMateFilter.IN_EXECUTE = true; 
        //                List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new HisExpMestMaterialManager().GetView(expMateFilter); 
        //                if (!paramGet.HasException)
        //                {
        //                    ProcessAmountMedicine(hisImpMestMedicine, hisExpMestMedicine); 
        //                    ProcessAmountMaterial(hisImpMestMaterial, hisExpMestMaterial); 
        //                    ProcessGetPeriod(paramGet); 
        //                    listRdoMedicine = listRdoMedicine.GroupBy(g => g.MEDI_MATE_ID).Select(s => new Mrs00226RDO { ACTIVE_INGREDIENT_ID = s.First().ACTIVE_INGREDIENT_ID, ACTIVE_INGREDIENT_NAME = s.First().ACTIVE_INGREDIENT_NAME, SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME, SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID, MEDI_STOCK_ID = s.First().MEDI_STOCK_ID, MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME, MEDI_MATE_ID = s.First().MEDI_MATE_ID, MEDI_MATE_IM_ID = s.First().MEDI_MATE_IM_ID, REPORT_TYPE_CAT_ID = s.First().REPORT_TYPE_CAT_ID, SUPPLIER_ID = s.First().SUPPLIER_ID, SUPPLIER_CODE = s.First().SUPPLIER_CODE, SUPPLIER_NAME = s.First().SUPPLIER_NAME, IMP_PRICE = s.First().IMP_PRICE, SERVICE_ID = s.First().SERVICE_ID, SERVICE_CODE = s.First().SERVICE_CODE, SERVICE_NAME = s.First().SERVICE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, BEGIN_AMOUNT = s.Sum(s1 => s1.BEGIN_AMOUNT), IMP_MANU_AMOUNT = s.Sum(s2 => s2.IMP_MANU_AMOUNT), IMP_CHMS_AMOUNT = s.Sum(s3 => s3.IMP_CHMS_AMOUNT), IMP_MOBA_AMOUNT = s.Sum(s4 => s4.IMP_MOBA_AMOUNT), EXP_PRES_AMOUNT = s.Sum(s5 => s5.EXP_PRES_AMOUNT), EXP_DEPA_AMOUNT = s.Sum(s6 => s6.EXP_DEPA_AMOUNT), EXP_CHMS_AMOUNT = s.Sum(s7 => s7.EXP_CHMS_AMOUNT), EXP_MANU_AMOUNT = s.Sum(s8 => s8.EXP_MANU_AMOUNT), EXP_EXPE_AMOUNT = s.Sum(s9 => s9.EXP_EXPE_AMOUNT), EXP_LOST_AMOUNT = s.Sum(s10 => s10.EXP_LOST_AMOUNT), EXP_SALE_AMOUNT = s.Sum(s11 => s11.EXP_SALE_AMOUNT), EXP_LIQU_AMOUNT = s.Sum(s12 => s12.EXP_LIQU_AMOUNT) }).Where(o => o.BEGIN_AMOUNT > 0 || o.IMP_MANU_AMOUNT > 0 || o.IMP_CHMS_AMOUNT > 0 || o.IMP_MOBA_AMOUNT > 0 || o.EXP_PRES_AMOUNT > 0 || o.EXP_DEPA_AMOUNT > 0 || o.EXP_CHMS_AMOUNT > 0 || o.EXP_MANU_AMOUNT > 0 || o.EXP_EXPE_AMOUNT > 0 || o.EXP_LOST_AMOUNT > 0 || o.EXP_SALE_AMOUNT > 0 || o.EXP_LIQU_AMOUNT > 0).ToList(); 

        //                    listRdoMedicine = listRdoMedicine.GroupBy(g => new { g.SERVICE_ID, g.SUPPLIER_ID, g.IMP_PRICE }).Select(s => new Mrs00226RDO { ACTIVE_INGREDIENT_ID = s.First().ACTIVE_INGREDIENT_ID, ACTIVE_INGREDIENT_NAME = s.First().ACTIVE_INGREDIENT_NAME, SUPPLIER_ID = s.First().SUPPLIER_ID, REPORT_TYPE_CAT_ID = s.First().REPORT_TYPE_CAT_ID, SUPPLIER_CODE = s.First().SUPPLIER_CODE, SUPPLIER_NAME = s.First().SUPPLIER_NAME, SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME, SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID, MEDI_STOCK_ID = s.First().MEDI_STOCK_ID, MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME, IMP_PRICE = s.First().IMP_PRICE, SERVICE_ID = s.First().SERVICE_ID, SERVICE_CODE = s.First().SERVICE_CODE, SERVICE_NAME = s.First().SERVICE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, BEGIN_AMOUNT = s.Sum(s1 => s1.BEGIN_AMOUNT), IMP_MANU_AMOUNT = s.Sum(s2 => s2.IMP_MANU_AMOUNT), IMP_CHMS_AMOUNT = s.Sum(s3 => s3.IMP_CHMS_AMOUNT), IMP_MOBA_AMOUNT = s.Sum(s4 => s4.IMP_MOBA_AMOUNT), EXP_PRES_AMOUNT = s.Sum(s5 => s5.EXP_PRES_AMOUNT), EXP_DEPA_AMOUNT = s.Sum(s6 => s6.EXP_DEPA_AMOUNT), EXP_CHMS_AMOUNT = s.Sum(s7 => s7.EXP_CHMS_AMOUNT), EXP_MANU_AMOUNT = s.Sum(s8 => s8.EXP_MANU_AMOUNT), EXP_EXPE_AMOUNT = s.Sum(s9 => s9.EXP_EXPE_AMOUNT), EXP_LOST_AMOUNT = s.Sum(s10 => s10.EXP_LOST_AMOUNT), EXP_SALE_AMOUNT = s.Sum(s11 => s11.EXP_SALE_AMOUNT), EXP_LIQU_AMOUNT = s.Sum(s12 => s12.EXP_LIQU_AMOUNT) }).ToList(); 

        //                    listRdoMaterial = listRdoMaterial.GroupBy(g => g.MEDI_MATE_ID).Select(s => new Mrs00226RDO { ACTIVE_INGREDIENT_ID = s.First().ACTIVE_INGREDIENT_ID, ACTIVE_INGREDIENT_NAME = s.First().ACTIVE_INGREDIENT_NAME, MEDI_MATE_ID = s.First().MEDI_MATE_ID, MEDI_MATE_IM_ID = s.First().MEDI_MATE_IM_ID, REPORT_TYPE_CAT_ID = s.First().REPORT_TYPE_CAT_ID, SUPPLIER_ID = s.First().SUPPLIER_ID, SUPPLIER_CODE = s.First().SUPPLIER_CODE, SUPPLIER_NAME = s.First().SUPPLIER_NAME, SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME, MEDI_STOCK_ID = s.First().MEDI_STOCK_ID, MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME, SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID, IMP_PRICE = s.First().IMP_PRICE, SERVICE_ID = s.First().SERVICE_ID, SERVICE_CODE = s.First().SERVICE_CODE, SERVICE_NAME = s.First().SERVICE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, BEGIN_AMOUNT = s.Sum(s1 => s1.BEGIN_AMOUNT), IMP_MANU_AMOUNT = s.Sum(s2 => s2.IMP_MANU_AMOUNT), IMP_CHMS_AMOUNT = s.Sum(s3 => s3.IMP_CHMS_AMOUNT), IMP_MOBA_AMOUNT = s.Sum(s4 => s4.IMP_MOBA_AMOUNT), EXP_PRES_AMOUNT = s.Sum(s5 => s5.EXP_PRES_AMOUNT), EXP_DEPA_AMOUNT = s.Sum(s6 => s6.EXP_DEPA_AMOUNT), EXP_CHMS_AMOUNT = s.Sum(s7 => s7.EXP_CHMS_AMOUNT), EXP_MANU_AMOUNT = s.Sum(s8 => s8.EXP_MANU_AMOUNT), EXP_EXPE_AMOUNT = s.Sum(s9 => s9.EXP_EXPE_AMOUNT), EXP_LOST_AMOUNT = s.Sum(s10 => s10.EXP_LOST_AMOUNT), EXP_SALE_AMOUNT = s.Sum(s11 => s11.EXP_SALE_AMOUNT), EXP_LIQU_AMOUNT = s.Sum(s12 => s12.EXP_LIQU_AMOUNT) }).Where(o => o.BEGIN_AMOUNT > 0 || o.IMP_MANU_AMOUNT > 0 || o.IMP_CHMS_AMOUNT > 0 || o.IMP_MOBA_AMOUNT > 0 || o.EXP_PRES_AMOUNT > 0 || o.EXP_DEPA_AMOUNT > 0 || o.EXP_CHMS_AMOUNT > 0 || o.EXP_MANU_AMOUNT > 0 || o.EXP_EXPE_AMOUNT > 0 || o.EXP_LOST_AMOUNT > 0 || o.EXP_SALE_AMOUNT > 0 || o.EXP_LIQU_AMOUNT > 0).ToList(); 

        //                    listRdoMaterial = listRdoMaterial.GroupBy(g => new { g.SERVICE_ID, g.SUPPLIER_ID, g.IMP_PRICE }).Select(s => new Mrs00226RDO { ACTIVE_INGREDIENT_ID = s.First().ACTIVE_INGREDIENT_ID, ACTIVE_INGREDIENT_NAME = s.First().ACTIVE_INGREDIENT_NAME, SERVICE_ID = s.First().SERVICE_ID, REPORT_TYPE_CAT_ID = s.First().REPORT_TYPE_CAT_ID, SUPPLIER_ID = s.First().SUPPLIER_ID, SUPPLIER_CODE = s.First().SUPPLIER_CODE, SUPPLIER_NAME = s.First().SUPPLIER_NAME, MEDI_STOCK_ID = s.First().MEDI_STOCK_ID, MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME, SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME, IMP_PRICE = s.First().IMP_PRICE, SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID, SERVICE_CODE = s.First().SERVICE_CODE, SERVICE_NAME = s.First().SERVICE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, BEGIN_AMOUNT = s.Sum(s1 => s1.BEGIN_AMOUNT), IMP_MANU_AMOUNT = s.Sum(s2 => s2.IMP_MANU_AMOUNT), IMP_CHMS_AMOUNT = s.Sum(s3 => s3.IMP_CHMS_AMOUNT), IMP_MOBA_AMOUNT = s.Sum(s4 => s4.IMP_MOBA_AMOUNT), EXP_PRES_AMOUNT = s.Sum(s5 => s5.EXP_PRES_AMOUNT), EXP_DEPA_AMOUNT = s.Sum(s6 => s6.EXP_DEPA_AMOUNT), EXP_CHMS_AMOUNT = s.Sum(s7 => s7.EXP_CHMS_AMOUNT), EXP_MANU_AMOUNT = s.Sum(s8 => s8.EXP_MANU_AMOUNT), EXP_EXPE_AMOUNT = s.Sum(s9 => s9.EXP_EXPE_AMOUNT), EXP_LOST_AMOUNT = s.Sum(s10 => s10.EXP_LOST_AMOUNT), EXP_SALE_AMOUNT = s.Sum(s11 => s11.EXP_SALE_AMOUNT), EXP_LIQU_AMOUNT = s.Sum(s12 => s12.EXP_LIQU_AMOUNT) }).ToList(); 
        //                    ListRdo.AddRange(listRdoMedicine); 
        //                    ListRdo.AddRange(listRdoMaterial); 
        //                }
        //                else
        //                {
        //                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00226."); 
        //                }
        //            }
        //            if (paramGet.HasException)
        //            {
        //                throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00226."); 
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex); 
        //        ListRdo.Clear(); 
        //        ListMediStock.Clear(); 
        //    }
        //}

        //Tính số lượng nhập và xuất thuốc
        private void ProcessAmountMedicine(List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine, List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine)
        {
            try
            {
                listRdoMedicine.Clear(); 
                ProcessImpAmountMedicine(hisImpMestMedicine); 
                ProcessExpAmountMedicine(hisExpMestMedicine); 
                #region listRdoMedicine
                listRdoMedicine = listRdoMedicine.GroupBy(g => g.MEDI_MATE_ID).Select(s =>
                    new Mrs00226RDO
                    {
                        ACTIVE_INGREDIENT_ID = s.First().ACTIVE_INGREDIENT_ID,
                        ACTIVE_INGREDIENT_NAME = s.First().ACTIVE_INGREDIENT_NAME,
                        SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                        SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                        MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                        MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                        MEDI_MATE_ID = s.First().MEDI_MATE_ID,
                        MEDI_MATE_IM_ID = s.First().MEDI_MATE_IM_ID,
                        REPORT_TYPE_CAT_ID = s.First().REPORT_TYPE_CAT_ID,
                        SUPPLIER_ID = s.First().SUPPLIER_ID,
                        SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                        SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                        IMP_PRICE = s.First().IMP_PRICE,
                        SERVICE_ID = s.First().SERVICE_ID,
                        SERVICE_CODE = s.First().SERVICE_CODE,
                        SERVICE_NAME = s.First().SERVICE_NAME,
                        SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                        NUM_ORDER = s.First().NUM_ORDER,
                        BEGIN_AMOUNT = s.Sum(s1 => s1.BEGIN_AMOUNT),
                        IMP_MANU_AMOUNT = s.Sum(s2 => s2.IMP_MANU_AMOUNT),
                        IMP_CHMS_AMOUNT = s.Sum(s3 => s3.IMP_CHMS_AMOUNT),
                        IMP_MOBA_AMOUNT = s.Sum(s4 => s4.IMP_MOBA_AMOUNT),
                        EXP_PRES_AMOUNT = s.Sum(s5 => s5.EXP_PRES_AMOUNT),
                        EXP_DEPA_AMOUNT = s.Sum(s6 => s6.EXP_DEPA_AMOUNT),
                        EXP_CHMS_AMOUNT = s.Sum(s7 => s7.EXP_CHMS_AMOUNT),
                        EXP_MANU_AMOUNT = s.Sum(s8 => s8.EXP_MANU_AMOUNT),
                        EXP_EXPE_AMOUNT = s.Sum(s9 => s9.EXP_EXPE_AMOUNT),
                        EXP_LOST_AMOUNT = s.Sum(s10 => s10.EXP_LOST_AMOUNT),
                        EXP_SALE_AMOUNT = s.Sum(s11 => s11.EXP_SALE_AMOUNT),
                        EXP_LIQU_AMOUNT = s.Sum(s12 => s12.EXP_LIQU_AMOUNT),
                        EXP_OTHER_AMOUNT = s.Sum(s13 => s13.EXP_OTHER_AMOUNT)
                    }).ToList(); 
                #endregion
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                listRdoMedicine.Clear(); 
            }
        }

        private void ProcessImpAmountMedicine(List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine)
        {
            try
            {
                if (IsNotNullOrEmpty(hisImpMestMedicine))
                {
                    //Nhập từ nhà cung cấp
                    var listImpMestMediMANU = hisImpMestMedicine.Where(o => o.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC).ToList(); 
                    if (IsNotNullOrEmpty(listImpMestMediMANU))
                    {
                        ProcessImpAmountMedicineByImpMestType(listImpMestMediMANU, IMP_MANU); 
                    }

                    //Nhập chuyển kho
                    var listImpMestMediCHMS = hisImpMestMedicine.Where(o => o.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK).ToList(); 
                    if (IsNotNullOrEmpty(listImpMestMediCHMS))
                    {
                        ProcessImpAmountMedicineByImpMestType(listImpMestMediCHMS, IMP_CHMS); 
                    }

                    //Nhập thu hồi
                    var listImpMestMediMOBA = hisImpMestMedicine.Where(o => o.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH).ToList(); 
                    if (IsNotNullOrEmpty(listImpMestMediMOBA))
                    {
                        ProcessImpAmountMedicineByImpMestType(listImpMestMediMOBA, IMP_MOBA); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessExpAmountMedicine(List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine)
        {
            try
            {
                if (IsNotNullOrEmpty(hisExpMestMedicine))
                {
                    //Xuất đơn thuốc
                    var listExpMestMediPRES = hisExpMestMedicine.Where(o => (o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK )).ToList(); 
                    if (IsNotNullOrEmpty(listExpMestMediPRES))
                    {
                        ProcessExpAmountMedicineByExpMestType(listExpMestMediPRES, EXP_PRES); 
                    }

                    //Xuất cho khoa phòng
                    var listExpMestMediDEPA = hisExpMestMedicine.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP).ToList(); 
                    if (IsNotNullOrEmpty(listExpMestMediDEPA))
                    {
                        ProcessExpAmountMedicineByExpMestType(listExpMestMediDEPA, EXP_DEPA); 
                    }

                    //Xuất chuyển kho
                    var listExpMestMediCHMS = hisExpMestMedicine.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK).ToList(); 
                    if (IsNotNullOrEmpty(listExpMestMediCHMS))
                    {
                        ProcessExpAmountMedicineByExpMestType(listExpMestMediCHMS, EXP_CHMS); 
                    }

                    //Xuất trả nhà cung cấp
                    var listExpMestMediMANU = hisExpMestMedicine.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__TNCC).ToList(); 
                    if (IsNotNullOrEmpty(listExpMestMediMANU))
                    {
                        ProcessExpAmountMedicineByExpMestType(listExpMestMediMANU, EXP_MANU); 
                    }

                    ////Xuất hao phí
                    //var listExpMestMediEXPE = hisExpMestMedicine.Where(o => o.EXP_MEST_TYPE_ID == HisExpMestTypeCFG.EXP_MEST_TYPE_ID__EXPE).ToList(); 
                    //if (IsNotNullOrEmpty(listExpMestMediEXPE))
                    //{
                    //    ProcessExpAmountMedicineByExpMestType(listExpMestMediEXPE, EXP_EXPE); 
                    //}

                    ////Xuất mất mát
                    //var listExpMestMediLOST = hisExpMestMedicine.Where(o => o.EXP_MEST_TYPE_ID == HisExpMestTypeCFG.EXP_MEST_TYPE_ID__LOST).ToList(); 
                    //if (IsNotNullOrEmpty(listExpMestMediLOST))
                    //{
                    //    ProcessExpAmountMedicineByExpMestType(listExpMestMediLOST, EXP_LOST); 
                    //}

                    //Xuất bán
                    var listExpMestMediSALE = hisExpMestMedicine.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN).ToList(); 
                    if (IsNotNullOrEmpty(listExpMestMediSALE))
                    {
                        ProcessExpAmountMedicineByExpMestType(listExpMestMediSALE, EXP_SALE); 
                    }

                    ////Xuất thanh lý
                    //var listExpMestMediLIQU = hisExpMestMedicine.Where(o => o.EXP_MEST_TYPE_ID == HisExpMestTypeCFG.EXP_MEST_TYPE_ID__LIQU).ToList(); 
                    //if (IsNotNullOrEmpty(listExpMestMediLIQU))
                    //{
                    //    ProcessExpAmountMedicineByExpMestType(listExpMestMediLIQU, EXP_LIQU); 
                    //}

                    //Xuất Khác
                    var listExpMestMediOTHER = hisExpMestMedicine.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__KHAC).ToList(); 
                    if (IsNotNullOrEmpty(listExpMestMediOTHER))
                    {
                        ProcessExpAmountMedicineByExpMestType(listExpMestMediOTHER, EXP_OTHER); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessImpAmountMedicineByImpMestType(List<V_HIS_IMP_MEST_MEDICINE> listImpMestMediSub, string code)
        {
            try
            {
                if (IsNotNullOrEmpty(listImpMestMediSub))
                {
                    var GroupImps = listImpMestMediSub.GroupBy(g => g.MEDICINE_ID).ToList(); 
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>(); 
                        Mrs00226RDO rdo = new Mrs00226RDO(); 
                        rdo.SERVICE_TYPE_NAME = "THUỐC"; 
                        rdo.SERVICE_TYPE_ID = 1; 
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID; 
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME; 
                        rdo.ACTIVE_INGREDIENT_ID = ListMedicineTypeAcin.Where(p => p.MEDICINE_TYPE_ID == listmediSub.First().MEDICINE_TYPE_ID).Select(o => o.ACTIVE_INGREDIENT_ID).FirstOrDefault(); 
                        rdo.ACTIVE_INGREDIENT_NAME = ListMedicineTypeAcin.Where(p => p.MEDICINE_TYPE_ID == listmediSub.First().MEDICINE_TYPE_ID).Select(o => o.ACTIVE_INGREDIENT_NAME).FirstOrDefault(); 
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID; 
                        rdo.MEDI_MATE_IM_ID = listmediSub.First().ID; 
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID; 
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE; 
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME; 
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME; 
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID; 
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE; 
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME; 
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER; 
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE; 
                        switch (code)
                        {
                            case IMP_MANU: rdo.IMP_MANU_AMOUNT = listmediSub.Sum(s => s.AMOUNT);  break; 
                            case IMP_CHMS: rdo.IMP_CHMS_AMOUNT = listmediSub.Sum(s => s.AMOUNT);  break; 
                            case IMP_MOBA: rdo.IMP_MOBA_AMOUNT = listmediSub.Sum(s => s.AMOUNT);  break; 
                            default:
                                break; 
                        }
                        listRdoMedicine.Add(rdo); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessExpAmountMedicineByExpMestType(List<V_HIS_EXP_MEST_MEDICINE> listExpMestMediSub, string code)
        {
            try
            {
                if (IsNotNullOrEmpty(listExpMestMediSub))
                {
                    var GroupImps = listExpMestMediSub.GroupBy(g => g.MEDICINE_ID).ToList(); 
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>(); 
                        Mrs00226RDO rdo = new Mrs00226RDO(); 
                        rdo.SERVICE_TYPE_NAME = "THUỐC"; 
                        rdo.SERVICE_TYPE_ID = 1; 
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID; 
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME; 
                        rdo.ACTIVE_INGREDIENT_ID = ListMedicineTypeAcin.Where(p => p.MEDICINE_TYPE_ID == listmediSub.First().MEDICINE_TYPE_ID).Select(o => o.ACTIVE_INGREDIENT_ID).FirstOrDefault(); 
                        rdo.ACTIVE_INGREDIENT_NAME = ListMedicineTypeAcin.Where(p => p.MEDICINE_TYPE_ID == listmediSub.First().MEDICINE_TYPE_ID).Select(o => o.ACTIVE_INGREDIENT_NAME).FirstOrDefault();

                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID ?? 0; 
                        rdo.MEDI_MATE_IM_ID = listmediSub.First().ID; 
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID; 
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE; 
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME; 
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME; 
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID; 
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE; 
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME; 
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER; 
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE; 
                        switch (code)
                        {
                            case EXP_PRES: rdo.EXP_PRES_AMOUNT = listmediSub.Sum(s => s.AMOUNT);  break; 
                            case EXP_DEPA: rdo.EXP_DEPA_AMOUNT = listmediSub.Sum(s => s.AMOUNT);  break; 
                            case EXP_CHMS: rdo.EXP_CHMS_AMOUNT = listmediSub.Sum(s => s.AMOUNT);  break; 
                            case EXP_MANU: rdo.EXP_MANU_AMOUNT = listmediSub.Sum(s => s.AMOUNT);  break; 
                            case EXP_EXPE: rdo.EXP_EXPE_AMOUNT = listmediSub.Sum(s => s.AMOUNT);  break; 
                            case EXP_LOST: rdo.EXP_LOST_AMOUNT = listmediSub.Sum(s => s.AMOUNT);  break; 
                            case EXP_SALE: rdo.EXP_SALE_AMOUNT = listmediSub.Sum(s => s.AMOUNT);  break; 
                            case EXP_LIQU: rdo.EXP_LIQU_AMOUNT = listmediSub.Sum(s => s.AMOUNT);  break; 
                            case EXP_OTHER: rdo.EXP_OTHER_AMOUNT = listmediSub.Sum(s => s.AMOUNT);  break; 
                            default:
                                break; 
                        }
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
        private void ProcessAmountMaterial(List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial, List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial)
        {
            try
            {
                listRdoMaterial.Clear(); 
                ProcessImpAmountMaterial(hisImpMestMaterial); 
                ProcessExpAmountMaterial(hisExpMestMaterial); 
                #region listRdoMaterial
                listRdoMaterial = listRdoMaterial.GroupBy(g => g.MEDI_MATE_ID).Select(s =>
                    new Mrs00226RDO
                    {
                        ACTIVE_INGREDIENT_ID = s.First().ACTIVE_INGREDIENT_ID,
                        ACTIVE_INGREDIENT_NAME = s.First().ACTIVE_INGREDIENT_NAME,
                        SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                        SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                        MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                        MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                        MEDI_MATE_ID = s.First().MEDI_MATE_ID,
                        MEDI_MATE_IM_ID = s.First().MEDI_MATE_IM_ID,
                        REPORT_TYPE_CAT_ID = s.First().REPORT_TYPE_CAT_ID,
                        SUPPLIER_ID = s.First().SUPPLIER_ID,
                        SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                        SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                        IMP_PRICE = s.First().IMP_PRICE,
                        SERVICE_ID = s.First().SERVICE_ID,
                        SERVICE_CODE = s.First().SERVICE_CODE,
                        SERVICE_NAME = s.First().SERVICE_NAME,
                        SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                        NUM_ORDER = s.First().NUM_ORDER,
                        BEGIN_AMOUNT = s.Sum(s1 => s1.BEGIN_AMOUNT),
                        IMP_MANU_AMOUNT = s.Sum(s2 => s2.IMP_MANU_AMOUNT),
                        IMP_CHMS_AMOUNT = s.Sum(s3 => s3.IMP_CHMS_AMOUNT),
                        IMP_MOBA_AMOUNT = s.Sum(s4 => s4.IMP_MOBA_AMOUNT),
                        EXP_PRES_AMOUNT = s.Sum(s5 => s5.EXP_PRES_AMOUNT),
                        EXP_DEPA_AMOUNT = s.Sum(s6 => s6.EXP_DEPA_AMOUNT),
                        EXP_CHMS_AMOUNT = s.Sum(s7 => s7.EXP_CHMS_AMOUNT),
                        EXP_MANU_AMOUNT = s.Sum(s8 => s8.EXP_MANU_AMOUNT),
                        EXP_EXPE_AMOUNT = s.Sum(s9 => s9.EXP_EXPE_AMOUNT),
                        EXP_LOST_AMOUNT = s.Sum(s10 => s10.EXP_LOST_AMOUNT),
                        EXP_SALE_AMOUNT = s.Sum(s11 => s11.EXP_SALE_AMOUNT),
                        EXP_LIQU_AMOUNT = s.Sum(s12 => s12.EXP_LIQU_AMOUNT),
                        EXP_OTHER_AMOUNT = s.Sum(s13 => s13.EXP_OTHER_AMOUNT)
                    }).ToList(); 
                #endregion
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                listRdoMaterial.Clear(); 
            }
        }

        private void ProcessImpAmountMaterial(List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial)
        {
            try
            {
                if (IsNotNullOrEmpty(hisImpMestMaterial))
                {
                    //Nhập từ nhà cung cấp
                    var listImpMestMateMANU = hisImpMestMaterial.Where(o => o.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC).ToList(); 
                    if (IsNotNullOrEmpty(listImpMestMateMANU))
                    {
                        ProcessImpAmountMateByImpMestType(listImpMestMateMANU, IMP_MANU); 
                    }

                    //Nhập chuyển kho
                    var listImpMestMateCHMS = hisImpMestMaterial.Where(o => o.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK).ToList(); 
                    if (IsNotNullOrEmpty(listImpMestMateCHMS))
                    {
                        ProcessImpAmountMateByImpMestType(listImpMestMateCHMS, IMP_CHMS); 
                    }

                    //Nhập thu hồi
                    var listImpMestMateMOBA = hisImpMestMaterial.Where(o => o.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH).ToList(); 
                    if (IsNotNullOrEmpty(listImpMestMateMOBA))
                    {
                        ProcessImpAmountMateByImpMestType(listImpMestMateMOBA, IMP_MOBA); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessExpAmountMaterial(List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial)
        {
            try
            {
                if (IsNotNullOrEmpty(hisExpMestMaterial))
                {
                    //Xuất đơn thuốc
                    var listExpMestMatePRES = hisExpMestMaterial.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK).ToList(); 
                    if (IsNotNullOrEmpty(listExpMestMatePRES))
                    {
                        ProcessExpAmountMateByExpMestType(listExpMestMatePRES, EXP_PRES); 
                    }

                    //Xuất cho khoa phòng
                    var listExpMestMateDEPA = hisExpMestMaterial.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP).ToList(); 
                    if (IsNotNullOrEmpty(listExpMestMateDEPA))
                    {
                        ProcessExpAmountMateByExpMestType(listExpMestMateDEPA, EXP_DEPA); 
                    }

                    //Xuất chuyển kho
                    var listExpMestMateCHMS = hisExpMestMaterial.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK).ToList(); 
                    if (IsNotNullOrEmpty(listExpMestMateCHMS))
                    {
                        ProcessExpAmountMateByExpMestType(listExpMestMateCHMS, EXP_CHMS); 
                    }

                    //Xuất trả nhà cung cấp
                    var listExpMestMateMANU = hisExpMestMaterial.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__TNCC).ToList(); 
                    if (IsNotNullOrEmpty(listExpMestMateMANU))
                    {
                        ProcessExpAmountMateByExpMestType(listExpMestMateMANU, EXP_MANU); 
                    }

                    ////Xuất hao phí
                    //var listExpMestMateEXPE = hisExpMestMaterial.Where(o => o.EXP_MEST_TYPE_ID == HisExpMestTypeCFG.EXP_MEST_TYPE_ID__EXPE).ToList(); 
                    //if (IsNotNullOrEmpty(listExpMestMateEXPE))
                    //{
                    //    ProcessExpAmountMateByExpMestType(listExpMestMateEXPE, EXP_EXPE); 
                    //}

                    ////Xuất mất mát
                    //var listExpMestMateLOST = hisExpMestMaterial.Where(o => o.EXP_MEST_TYPE_ID == HisExpMestTypeCFG.EXP_MEST_TYPE_ID__LOST).ToList(); 
                    //if (IsNotNullOrEmpty(listExpMestMateLOST))
                    //{
                    //    ProcessExpAmountMateByExpMestType(listExpMestMateLOST, EXP_LOST); 
                    //}

                    //Xuất bán
                    var listExpMestMateSALE = hisExpMestMaterial.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN).ToList(); 
                    if (IsNotNullOrEmpty(listExpMestMateSALE))
                    {
                        ProcessExpAmountMateByExpMestType(listExpMestMateSALE, EXP_SALE); 
                    }

                    ////Xuất thanh lý
                    //var listExpMestMateLIQU = hisExpMestMaterial.Where(o => o.EXP_MEST_TYPE_ID == HisExpMestTypeCFG.EXP_MEST_TYPE_ID__LIQU).ToList(); 
                    //if (IsNotNullOrEmpty(listExpMestMateLIQU))
                    //{
                    //    ProcessExpAmountMateByExpMestType(listExpMestMateLIQU, EXP_LIQU); 
                    //}

                    //Xuất thanh lý
                    var listExpMestMateOTHER = hisExpMestMaterial.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__KHAC).ToList(); 
                    if (IsNotNullOrEmpty(listExpMestMateOTHER))
                    {
                        ProcessExpAmountMateByExpMestType(listExpMestMateOTHER, EXP_OTHER); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessImpAmountMateByImpMestType(List<V_HIS_IMP_MEST_MATERIAL> listImpMestMateSub, string code)
        {
            try
            {
                if (IsNotNullOrEmpty(listImpMestMateSub))
                {
                    var GroupImps = listImpMestMateSub.GroupBy(g => g.MATERIAL_ID).ToList(); 
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>(); 
                        Mrs00226RDO rdo = new Mrs00226RDO(); 
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ"; 
                        rdo.SERVICE_TYPE_ID = 2; 
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID; 
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME; 
                        rdo.ACTIVE_INGREDIENT_ID = 0; 
                        rdo.ACTIVE_INGREDIENT_NAME = ""; 
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID; 
                        rdo.MEDI_MATE_IM_ID = listmateSub.First().ID; 
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID; 
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE; 
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME; 
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME; 
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID; 
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE; 
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME; 
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER; 
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE; 
                        switch (code)
                        {
                            case IMP_MANU: rdo.IMP_MANU_AMOUNT = listmateSub.Sum(s => s.AMOUNT);  break; 
                            case IMP_CHMS: rdo.IMP_CHMS_AMOUNT = listmateSub.Sum(s => s.AMOUNT);  break; 
                            case IMP_MOBA: rdo.IMP_MOBA_AMOUNT = listmateSub.Sum(s => s.AMOUNT);  break; 
                            default:
                                break; 
                        }
                        listRdoMaterial.Add(rdo); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessExpAmountMateByExpMestType(List<V_HIS_EXP_MEST_MATERIAL> listExpMestMateSub, string code)
        {
            try
            {
                if (IsNotNullOrEmpty(listExpMestMateSub))
                {
                    var GroupImps = listExpMestMateSub.GroupBy(g => g.MATERIAL_ID).ToList(); 
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>(); 
                        Mrs00226RDO rdo = new Mrs00226RDO(); 
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ"; 
                        rdo.SERVICE_TYPE_ID = 2; 
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID; 
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME; 
                        rdo.ACTIVE_INGREDIENT_ID = 0; 
                        rdo.ACTIVE_INGREDIENT_NAME = "";
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID ?? 0; 
                        rdo.MEDI_MATE_IM_ID = listmateSub.First().ID; 
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID; 
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE; 
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME; 
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME; 
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID; 
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE; 
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME; 
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER; 
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE; 
                        switch (code)
                        {
                            case EXP_PRES: rdo.EXP_PRES_AMOUNT = listmateSub.Sum(s => s.AMOUNT);  break; 
                            case EXP_DEPA: rdo.EXP_DEPA_AMOUNT = listmateSub.Sum(s => s.AMOUNT);  break; 
                            case EXP_CHMS: rdo.EXP_CHMS_AMOUNT = listmateSub.Sum(s => s.AMOUNT);  break; 
                            case EXP_MANU: rdo.EXP_MANU_AMOUNT = listmateSub.Sum(s => s.AMOUNT);  break; 
                            case EXP_EXPE: rdo.EXP_EXPE_AMOUNT = listmateSub.Sum(s => s.AMOUNT);  break; 
                            case EXP_LOST: rdo.EXP_LOST_AMOUNT = listmateSub.Sum(s => s.AMOUNT);  break; 
                            case EXP_SALE: rdo.EXP_SALE_AMOUNT = listmateSub.Sum(s => s.AMOUNT);  break; 
                            case EXP_LIQU: rdo.EXP_LIQU_AMOUNT = listmateSub.Sum(s => s.AMOUNT);  break; 
                            case EXP_OTHER: rdo.EXP_OTHER_AMOUNT = listmateSub.Sum(s => s.AMOUNT);  break; 
                            default:
                                break; 
                        }
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
        private void ProcessGetPeriod(CommonParam paramGet)
        {
            try
            {
                HisMediStockPeriodViewFilterQuery periodFilter = new HisMediStockPeriodViewFilterQuery(); 
                periodFilter.CREATE_TIME_TO = ((Mrs00226Filter)this.reportFilter).TIME_FROM; 
                periodFilter.MEDI_STOCK_ID = CurrentMediStock.ID; 
                List<V_HIS_MEDI_STOCK_PERIOD> hisMediStockPeriod = new HisMediStockPeriodManager(paramGet).GetView(periodFilter); 
                if (!paramGet.HasException)
                {
                    if (IsNotNullOrEmpty(hisMediStockPeriod))
                    {
                        //Trường hợp có kỳ được chốt gần nhất
                        V_HIS_MEDI_STOCK_PERIOD neighborPeriod = hisMediStockPeriod.OrderByDescending(d => d.CREATE_TIME).ToList()[0]; 
                        ProcessBeinAmountMedicineByMediStockPeriod(paramGet, neighborPeriod); 
                        processBeinAmountMaterialByMediStockPeriod(paramGet, neighborPeriod); 
                    }
                    else
                    {
                        // Trường hợp không có kỳ chốt gần nhât - chưa được chốt kỳ nào
                        ProcessBeinAmountMedicineNotMediStockPriod(paramGet); 
                        ProcessBeinAmountMaterialNotMediStockPriod(paramGet); 
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
        private void ProcessBeinAmountMedicineByMediStockPeriod(CommonParam paramGet, V_HIS_MEDI_STOCK_PERIOD neighborPeriod)
        {
            try
            {
                List<Mrs00226RDO> listrdo = new List<Mrs00226RDO>(); 
                HisMestPeriodMediViewFilterQuery periodMediFilter = new HisMestPeriodMediViewFilterQuery(); 
                periodMediFilter.MEDI_STOCK_PERIOD_ID = neighborPeriod.ID; 
                List<V_HIS_MEST_PERIOD_MEDI> hisMestPeriodMedi = new HisMestPeriodMediManager(paramGet).GetView(periodMediFilter); 
                if (IsNotNullOrEmpty(hisMestPeriodMedi))
                {
                    foreach (var item in hisMestPeriodMedi)
                    {
                        Mrs00226RDO rdo = new Mrs00226RDO(); 
                        rdo.SERVICE_TYPE_NAME = "THUỐC"; 
                        rdo.SERVICE_TYPE_ID = 1; 
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID; 
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME; 
                        rdo.ACTIVE_INGREDIENT_ID = ListMedicineTypeAcin.Where(p => p.MEDICINE_TYPE_ID == item.MEDICINE_TYPE_ID).Select(o => o.ACTIVE_INGREDIENT_ID).FirstOrDefault(); 
                        rdo.ACTIVE_INGREDIENT_NAME = ListMedicineTypeAcin.Where(p => p.MEDICINE_TYPE_ID == item.MEDICINE_TYPE_ID).Select(o => o.ACTIVE_INGREDIENT_NAME).FirstOrDefault(); 
                        rdo.MEDI_MATE_ID = item.MEDICINE_ID; 
                        rdo.MEDI_MATE_IM_ID = item.ID; 
                        rdo.SERVICE_ID = item.SERVICE_ID; 
                        rdo.SERVICE_CODE = item.MEDICINE_TYPE_CODE; 
                        rdo.SERVICE_NAME = item.MEDICINE_TYPE_NAME; 
                        rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME; 
                        rdo.SUPPLIER_ID = item.SUPPLIER_ID; 
                        rdo.SUPPLIER_CODE = item.SUPPLIER_CODE; 
                        rdo.SUPPLIER_NAME = item.SUPPLIER_NAME; 
                        rdo.NUM_ORDER = item.NUM_ORDER; 
                        rdo.IMP_PRICE = item.IMP_PRICE; 
                        rdo.BEGIN_AMOUNT = item.AMOUNT; 
                        if (rdo.ACTIVE_INGREDIENT_ID != 0) listrdo.Add(rdo); 
                    }
                }
                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery(); 
                impMediFilter.IMP_TIME_FROM = neighborPeriod.CREATE_TIME + 1; 
                impMediFilter.IMP_TIME_TO = ((Mrs00226Filter)this.reportFilter).TIME_FROM - 1; 
                impMediFilter.MEDI_STOCK_ID = CurrentMediStock.ID; 
                impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; 
                List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new HisImpMestMedicineManager(paramGet).GetView(impMediFilter); 
                if (IsNotNullOrEmpty(hisImpMestMedicine))
                {
                    var GroupImps = hisImpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList(); 
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>(); 
                        Mrs00226RDO rdo = new Mrs00226RDO(); 
                        rdo.SERVICE_TYPE_NAME = "THUỐC"; 
                        rdo.SERVICE_TYPE_ID = 1; 
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID; 
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME; 
                        rdo.ACTIVE_INGREDIENT_ID = ListMedicineTypeAcin.Where(p => p.MEDICINE_TYPE_ID == listmediSub.First().MEDICINE_TYPE_ID).Select(o => o.ACTIVE_INGREDIENT_ID).FirstOrDefault(); 
                        rdo.ACTIVE_INGREDIENT_NAME = ListMedicineTypeAcin.Where(p => p.MEDICINE_TYPE_ID == listmediSub.First().MEDICINE_TYPE_ID).Select(o => o.ACTIVE_INGREDIENT_NAME).FirstOrDefault(); 
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID; 
                        rdo.MEDI_MATE_IM_ID = listmediSub.First().ID; 
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID; 
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE; 
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME; 
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME; 
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID; 
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE; 
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME; 
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER; 
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE; 
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => s.AMOUNT); 
                        if (rdo.ACTIVE_INGREDIENT_ID != 0) listrdo.Add(rdo); 
                    }
                }

                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery(); 
                expMediFilter.EXP_TIME_FROM = neighborPeriod.CREATE_TIME + 1; 
                expMediFilter.EXP_TIME_TO = ((Mrs00226Filter)this.reportFilter).TIME_FROM - 1; 
                expMediFilter.MEDI_STOCK_ID = CurrentMediStock.ID; 
                expMediFilter.IS_EXPORT = true; 
                //HisExpMestSttCFG.EXP_MEST_STT_ID__EXPORTED; 
                //expMediFilter.IN_EXECUTE = true; 
                List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new HisExpMestMedicineManager(paramGet).GetView(expMediFilter); 
                if (IsNotNullOrEmpty(hisExpMestMedicine))
                {
                    var GroupExps = hisExpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList(); 
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>(); 
                        Mrs00226RDO rdo = new Mrs00226RDO(); 
                        rdo.SERVICE_TYPE_NAME = "THUỐC"; 
                        rdo.SERVICE_TYPE_ID = 1; 
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID; 
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME; 
                        rdo.ACTIVE_INGREDIENT_ID = ListMedicineTypeAcin.Where(p => p.MEDICINE_TYPE_ID == listmediSub.First().MEDICINE_TYPE_ID).Select(o => o.ACTIVE_INGREDIENT_ID).FirstOrDefault(); 
                        rdo.ACTIVE_INGREDIENT_NAME = ListMedicineTypeAcin.Where(p => p.MEDICINE_TYPE_ID == listmediSub.First().MEDICINE_TYPE_ID).Select(o => o.ACTIVE_INGREDIENT_NAME).FirstOrDefault();
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID ?? 0; 
                        rdo.MEDI_MATE_IM_ID = listmediSub.First().ID; 
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID; 
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE; 
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME; 
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME; 
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID; 
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE; 
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME; 
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER; 
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE; 
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => -(s.AMOUNT)); 
                        if (rdo.ACTIVE_INGREDIENT_ID != 0) listrdo.Add(rdo); 
                    }
                }

                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s =>
                    new Mrs00226RDO
                    {
                        ACTIVE_INGREDIENT_ID = s.First().ACTIVE_INGREDIENT_ID,
                        ACTIVE_INGREDIENT_NAME = s.First().ACTIVE_INGREDIENT_NAME,
                        SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                        SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                        MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                        MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                        MEDI_MATE_ID = s.First().MEDI_MATE_ID,
                        MEDI_MATE_IM_ID = s.First().MEDI_MATE_IM_ID,
                        REPORT_TYPE_CAT_ID = s.First().REPORT_TYPE_CAT_ID,
                        SUPPLIER_ID = s.First().SUPPLIER_ID,
                        SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                        SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                        IMP_PRICE = s.First().IMP_PRICE,
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

        //Tính số lượng tồn đầu nếu có chốt kỳ gần nhất của vật tư
        private void processBeinAmountMaterialByMediStockPeriod(CommonParam paramGet, V_HIS_MEDI_STOCK_PERIOD neighborPeriod)
        {
            try
            {
                List<Mrs00226RDO> listrdo = new List<Mrs00226RDO>(); 
                HisMestPeriodMateViewFilterQuery periodMateFilter = new HisMestPeriodMateViewFilterQuery(); 
                periodMateFilter.MEDI_STOCK_PERIOD_ID = neighborPeriod.ID; 
                List<V_HIS_MEST_PERIOD_MATE> hisMestPeriodMate = new HisMestPeriodMateManager(paramGet).GetView(periodMateFilter); 
                if (IsNotNullOrEmpty(hisMestPeriodMate))
                {
                    foreach (var item in hisMestPeriodMate)
                    {
                        Mrs00226RDO rdo = new Mrs00226RDO(); 
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ"; 
                        rdo.SERVICE_TYPE_ID = 2; 
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID; 
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME; 
                        rdo.ACTIVE_INGREDIENT_ID = 0; 
                        rdo.ACTIVE_INGREDIENT_NAME = ""; 
                        rdo.MEDI_MATE_ID = item.MATERIAL_ID; 
                        rdo.MEDI_MATE_IM_ID = item.ID; 
                        rdo.SERVICE_ID = item.SERVICE_ID; 
                        rdo.SERVICE_CODE = item.MATERIAL_TYPE_CODE; 
                        rdo.SERVICE_NAME = item.MATERIAL_TYPE_NAME; 
                        rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME; 
                        rdo.SUPPLIER_ID = item.SUPPLIER_ID; 
                        rdo.SUPPLIER_CODE = item.SUPPLIER_CODE; 
                        rdo.SUPPLIER_NAME = item.SUPPLIER_NAME; 
                        rdo.NUM_ORDER = item.NUM_ORDER; 
                        rdo.IMP_PRICE = item.IMP_PRICE; 
                        rdo.BEGIN_AMOUNT = item.AMOUNT; 
                        if (rdo.ACTIVE_INGREDIENT_ID != 0) listrdo.Add(rdo); 
                    }
                }
                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery(); 
                impMateFilter.IMP_TIME_FROM = neighborPeriod.CREATE_TIME + 1; 
                impMateFilter.IMP_TIME_TO = ((Mrs00226Filter)this.reportFilter).TIME_FROM - 1; 
                impMateFilter.MEDI_STOCK_ID = CurrentMediStock.ID; 
                impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; 
                List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new HisImpMestMaterialManager(paramGet).GetView(impMateFilter); 
                if (IsNotNullOrEmpty(hisImpMestMaterial))
                {
                    var GroupImps = hisImpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList(); 
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>(); 
                        Mrs00226RDO rdo = new Mrs00226RDO(); 
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ"; 
                        rdo.SERVICE_TYPE_ID = 2; 
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID; 
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME; 
                        rdo.ACTIVE_INGREDIENT_ID = 0; 
                        rdo.ACTIVE_INGREDIENT_NAME = ""; 
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID; 
                        rdo.MEDI_MATE_IM_ID = listmateSub.First().ID; 
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID; 
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE; 
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME; 
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME; 
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID; 
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE; 
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME; 
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER; 
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE; 
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => s.AMOUNT); 
                        if (rdo.ACTIVE_INGREDIENT_ID != 0) listrdo.Add(rdo); 
                    }
                }

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery(); 
                expMateFilter.EXP_TIME_FROM = neighborPeriod.CREATE_TIME + 1; 
                expMateFilter.EXP_TIME_TO = ((Mrs00226Filter)this.reportFilter).TIME_FROM - 1; 
                expMateFilter.MEDI_STOCK_ID = CurrentMediStock.ID; 
                expMateFilter.IS_EXPORT = true; 
                //HisExpMestSttCFG.EXP_MEST_STT_ID__EXPORTED; 
                //expMateFilter.IN_EXECUTE = true; 
                List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new HisExpMestMaterialManager(paramGet).GetView(expMateFilter); 
                if (IsNotNullOrEmpty(hisExpMestMaterial))
                {
                    var GroupExps = hisExpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList(); 
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>(); 
                        Mrs00226RDO rdo = new Mrs00226RDO(); 
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ"; 
                        rdo.SERVICE_TYPE_ID = 2; 
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID; 
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME; 
                        rdo.ACTIVE_INGREDIENT_ID = 0; 
                        rdo.ACTIVE_INGREDIENT_NAME = "";
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID ?? 0; 
                        rdo.MEDI_MATE_IM_ID = listmateSub.First().ID; 
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID; 
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE; 
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME; 
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME; 
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID; 
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE; 
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME; 
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER; 
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE; 
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => -(s.AMOUNT)); 
                        if (rdo.ACTIVE_INGREDIENT_ID != 0) listrdo.Add(rdo); 
                    }
                }

                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s =>
                    new Mrs00226RDO
                    {
                        ACTIVE_INGREDIENT_ID = s.First().ACTIVE_INGREDIENT_ID,
                        ACTIVE_INGREDIENT_NAME = s.First().ACTIVE_INGREDIENT_NAME,
                        SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                        SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                        MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                        MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                        MEDI_MATE_ID = s.First().MEDI_MATE_ID,
                        MEDI_MATE_IM_ID = s.First().MEDI_MATE_IM_ID,
                        REPORT_TYPE_CAT_ID = s.First().REPORT_TYPE_CAT_ID,
                        SUPPLIER_ID = s.First().SUPPLIER_ID,
                        SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                        SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                        IMP_PRICE = s.First().IMP_PRICE,
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
        private void ProcessBeinAmountMedicineNotMediStockPriod(CommonParam paramGet)
        {
            try
            {
                List<Mrs00226RDO> listrdo = new List<Mrs00226RDO>(); 

                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery(); 
                impMediFilter.IMP_TIME_TO = ((Mrs00226Filter)this.reportFilter).TIME_FROM - 1; 
                impMediFilter.MEDI_STOCK_ID = CurrentMediStock.ID; 
                impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; 
                List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new HisImpMestMedicineManager().GetView(impMediFilter); 
                if (IsNotNullOrEmpty(hisImpMestMedicine))
                {
                    var GroupImps = hisImpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList(); 
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>(); 
                        Mrs00226RDO rdo = new Mrs00226RDO(); 
                        rdo.SERVICE_TYPE_NAME = "THUỐC"; 
                        rdo.SERVICE_TYPE_ID = 1; 
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID; 
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME; 
                        rdo.ACTIVE_INGREDIENT_ID = ListMedicineTypeAcin.Where(p => p.MEDICINE_TYPE_ID == listmediSub.First().MEDICINE_TYPE_ID).Select(o => o.ACTIVE_INGREDIENT_ID).FirstOrDefault(); 
                        rdo.ACTIVE_INGREDIENT_NAME = ListMedicineTypeAcin.Where(p => p.MEDICINE_TYPE_ID == listmediSub.First().MEDICINE_TYPE_ID).Select(o => o.ACTIVE_INGREDIENT_NAME).FirstOrDefault(); 
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID; 
                        rdo.MEDI_MATE_IM_ID = listmediSub.First().ID; 
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID; 
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE; 
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME; 
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME; 
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID; 
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE; 
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME; 
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER; 
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE; 
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => s.AMOUNT); 
                        if (rdo.ACTIVE_INGREDIENT_ID != 0) listrdo.Add(rdo); 
                    }
                }

                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery(); 
                expMediFilter.EXP_TIME_TO = ((Mrs00226Filter)this.reportFilter).TIME_FROM - 1; 
                expMediFilter.MEDI_STOCK_ID = CurrentMediStock.ID; 
                expMediFilter.IS_EXPORT = true; 
                //HisExpMestSttCFG.EXP_MEST_STT_ID__EXPORTED; 
                //expMediFilter.IN_EXECUTE = true; 
                List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new HisExpMestMedicineManager().GetView(expMediFilter); 
                if (IsNotNullOrEmpty(hisExpMestMedicine))
                {
                    var GroupExps = hisExpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList(); 
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>(); 
                        Mrs00226RDO rdo = new Mrs00226RDO(); 
                        rdo.SERVICE_TYPE_NAME = "THUỐC"; 
                        rdo.SERVICE_TYPE_ID = 1; 
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID; 
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME; 
                        rdo.ACTIVE_INGREDIENT_ID = ListMedicineTypeAcin.Where(p => p.MEDICINE_TYPE_ID == listmediSub.First().MEDICINE_TYPE_ID).Select(o => o.ACTIVE_INGREDIENT_ID).FirstOrDefault(); 
                        rdo.ACTIVE_INGREDIENT_NAME = ListMedicineTypeAcin.Where(p => p.MEDICINE_TYPE_ID == listmediSub.First().MEDICINE_TYPE_ID).Select(o => o.ACTIVE_INGREDIENT_NAME).FirstOrDefault();
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID ?? 0; 
                        rdo.MEDI_MATE_IM_ID = listmediSub.First().ID; 
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID; 
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE; 
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME; 
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME; 
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID; 
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE; 
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME; 
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER; 
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE; 
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => -(s.AMOUNT)); 
                        if (rdo.ACTIVE_INGREDIENT_ID != 0) listrdo.Add(rdo); 
                    }
                }

                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s =>
                    new Mrs00226RDO
                    {
                        ACTIVE_INGREDIENT_ID = s.First().ACTIVE_INGREDIENT_ID,
                        ACTIVE_INGREDIENT_NAME = s.First().ACTIVE_INGREDIENT_NAME,
                        SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                        SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                        MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                        MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                        MEDI_MATE_ID = s.First().MEDI_MATE_ID,
                        MEDI_MATE_IM_ID = s.First().MEDI_MATE_IM_ID,
                        REPORT_TYPE_CAT_ID = s.First().REPORT_TYPE_CAT_ID,
                        SUPPLIER_ID = s.First().SUPPLIER_ID,
                        SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                        SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                        IMP_PRICE = s.First().IMP_PRICE,
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

        // Tính số lượng tồn đầu nếu không có chốt kỳ gần nhất của vật tư
        private void ProcessBeinAmountMaterialNotMediStockPriod(CommonParam paramGet)
        {
            try
            {
                List<Mrs00226RDO> listrdo = new List<Mrs00226RDO>(); 

                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery(); 
                impMateFilter.IMP_TIME_TO = ((Mrs00226Filter)this.reportFilter).TIME_FROM - 1; 
                impMateFilter.MEDI_STOCK_ID = CurrentMediStock.ID; 
                impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; 
                List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new HisImpMestMaterialManager(paramGet).GetView(impMateFilter); 
                if (IsNotNullOrEmpty(hisImpMestMaterial))
                {
                    var GroupImps = hisImpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList(); 
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>(); 
                        Mrs00226RDO rdo = new Mrs00226RDO(); 
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ"; 
                        rdo.SERVICE_TYPE_ID = 2; 
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID; 
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME; 

                        rdo.ACTIVE_INGREDIENT_ID = 0; 
                        rdo.ACTIVE_INGREDIENT_NAME = ""; 
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID; 
                        rdo.MEDI_MATE_IM_ID = listmateSub.First().ID; 
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID; 
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE; 
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME; 
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME; 
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID; 
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE; 
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME; 
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER; 
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE; 
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => s.AMOUNT); 
                        if (rdo.ACTIVE_INGREDIENT_ID != 0) listrdo.Add(rdo); 
                    }
                }

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery(); 
                expMateFilter.EXP_TIME_TO = ((Mrs00226Filter)this.reportFilter).TIME_FROM - 1; 
                expMateFilter.MEDI_STOCK_ID = CurrentMediStock.ID; 
                expMateFilter.IS_EXPORT = true; 
                //HisExpMestSttCFG.EXP_MEST_STT_ID__EXPORTED; 
                //expMateFilter.IN_EXECUTE = true; 
                List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new HisExpMestMaterialManager(paramGet).GetView(expMateFilter); 
                if (IsNotNullOrEmpty(hisExpMestMaterial))
                {
                    var GroupExps = hisExpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList(); 
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>(); 
                        Mrs00226RDO rdo = new Mrs00226RDO(); 
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ"; 
                        rdo.SERVICE_TYPE_ID = 2; 
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID; 
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME; 
                        rdo.ACTIVE_INGREDIENT_ID = 0; 
                        rdo.ACTIVE_INGREDIENT_NAME = "";
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID ?? 0; 
                        rdo.MEDI_MATE_IM_ID = listmateSub.First().ID; 
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID; 
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE; 
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME; 
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME; 
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID; 
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE; 
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME; 
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER; 
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE; 
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => -(s.AMOUNT)); 
                        if (rdo.ACTIVE_INGREDIENT_ID != 0) listrdo.Add(rdo); 
                    }
                }

                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s =>
                    new Mrs00226RDO
                    {
                        ACTIVE_INGREDIENT_ID = s.First().ACTIVE_INGREDIENT_ID,
                        ACTIVE_INGREDIENT_NAME = s.First().ACTIVE_INGREDIENT_NAME,
                        SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                        SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                        MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                        MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                        MEDI_MATE_ID = s.First().MEDI_MATE_ID,
                        MEDI_MATE_IM_ID = s.First().MEDI_MATE_IM_ID,
                        REPORT_TYPE_CAT_ID = s.First().REPORT_TYPE_CAT_ID,
                        SUPPLIER_ID = s.First().SUPPLIER_ID,
                        SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                        SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                        IMP_PRICE = s.First().IMP_PRICE,
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

        private Mrs00226Filter ProcessCastFilterQuery(Mrs00226Filter Mrs00226Filter)
        {
            Mrs00226Filter castFilter = null; 
            try
            {
                if (Mrs00226Filter == null) throw new ArgumentNullException("Input param Mrs00226Filter is null"); 

                Mapper.CreateMap<Mrs00226Filter, Mrs00226Filter>(); 
                castFilter = Mapper.Map<Mrs00226Filter, Mrs00226Filter>(Mrs00226Filter); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
            return castFilter; 
        }
    }

    class CustomerFuncMergeSameData1 : TFlexCelUserFunction
    {
        long MediStockId1; 
        int SameType1; 

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length <= 0)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function"); 

            bool result = false; 
            try
            {
                long mediId1 = Convert.ToInt64(parameters[0]); 
                int ServiceId1 = Convert.ToInt32(parameters[1]); 

                if (mediId1 > 0 && ServiceId1 > 0)
                {
                    if (SameType1 == ServiceId1 && MediStockId1 == mediId1)
                    {
                        return true; 
                    }
                    else
                    {
                        MediStockId1 = mediId1; 
                        SameType1 = ServiceId1; 
                        return false; 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
            return result; 
        }
    }

    class CustomerFuncMergeSameData2 : TFlexCelUserFunction
    {
        long MediStockId2; 
        int SameType2; 

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length <= 0)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function"); 

            bool result = false; 
            try
            {
                long mediId2 = Convert.ToInt64(parameters[0]); 
                int ServiceId2 = Convert.ToInt32(parameters[1]); 

                if (mediId2 > 0 && ServiceId2 > 0)
                {
                    if (SameType2 == ServiceId2 && MediStockId2 == mediId2)
                    {
                        return true; 
                    }
                    else
                    {
                        MediStockId2 = mediId2; 
                        SameType2 = ServiceId2; 
                        return false; 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
            return result; 
        }
    }

    class RDOCustomerFuncManyRownumberData : TFlexCelUserFunction
    {
        long Medi_Stock_Id; 
        int Service_Type_Id; 
        long num_order = 0; 

        public RDOCustomerFuncManyRownumberData() { }

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
