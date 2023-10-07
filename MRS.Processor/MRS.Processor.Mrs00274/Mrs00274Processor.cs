using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using ACS.EFMODEL.DataModels;
using ACS.Filter;
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
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMediStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACS.MANAGER.Manager;
using ACS.MANAGER.Core.AcsUser.Get;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisService;

namespace MRS.Processor.Mrs00274
{
    class Mrs00274Processor : AbstractProcessor
    {
        List<Mrs00274RDO> listRdo = new List<Mrs00274RDO>();
        List<Mrs00274RDO> ListExpRdo = new List<Mrs00274RDO>();
        List<Mrs00274RDO> ListExpRdo1 = new List<Mrs00274RDO>();
        List<Mrs00274RDO> ListExpRdo2 = new List<Mrs00274RDO>();
        List<V_HIS_EXP_MEST_MEDICINE> listMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> listMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<V_HIS_EXP_MEST> listSaleExpMest = new List<V_HIS_EXP_MEST>();
        List<HIS_SERVICE_REQ> listServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_TRANSACTION> ListTransaction = new List<HIS_TRANSACTION>();
        List<V_HIS_MEDICINE_TYPE> listMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        List<PrintLogUnique> listPrintLogUnique = new List<PrintLogUnique>();
        Dictionary<string, List<PrintLogUnique>> dicPrintLogUnique = new Dictionary<string, List<PrintLogUnique>>();
        //them danh sach sach ban tra lai
        List<Mrs00274RDO> listImpRdo = new List<Mrs00274RDO>();
        List<V_HIS_IMP_MEST_MEDICINE> listImpMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> listImpMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<V_HIS_IMP_MEST> listSaleImpMest = new List<V_HIS_IMP_MEST>();

        Dictionary<long, decimal> dicThExpMestMedicinePrice = new Dictionary<long, decimal>();

        Dictionary<long, decimal> dicThExpMestMedicineVat = new Dictionary<long, decimal>();

        List<V_HIS_EXP_MEST> listMobaExpMest = new List<V_HIS_EXP_MEST>();

        Dictionary<long, decimal> dicThExpMestMaterialPrice = new Dictionary<long, decimal>();
        Dictionary<long, decimal> dicThExpMestMaterialVat = new Dictionary<long, decimal>();

        List<HIS_SERVICE> services = new List<HIS_SERVICE>();

        Mrs00274Filter filter;
        CommonParam paramGet = new CommonParam();
        public Mrs00274Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00274Filter);
        }

        protected override bool GetData()
        {
            filter = ((Mrs00274Filter)reportFilter);
            bool result = true;
            try
            {
                //xử lý bỏ nhóm cha và loại dịch vụ nếu đã lọc theo thuốc vật tư cụ thể
                if (filter.MEDICINE_TYPE_IDs != null || filter.MATERIAL_TYPE_IDs != null)
                {
                    filter.EXACT_PARENT_SERVICE_IDs = null;
                    filter.SERVICE_TYPE_IDs = null;
                }


                if (filter.MEDICINE_TYPE_IDs != null && filter.MATERIAL_TYPE_IDs == null)
                {
                    filter.MATERIAL_TYPE_IDs = new List<long>();
                }
                if (filter.MATERIAL_TYPE_IDs != null && filter.MEDICINE_TYPE_IDs == null)
                {
                    filter.MEDICINE_TYPE_IDs = new List<long>();
                }
                //xuất
                List<long> expMesstIds = new List<long>();
                HisExpMestMedicineViewFilterQuery medicinefilter = new HisExpMestMedicineViewFilterQuery();
                medicinefilter.MEDI_STOCK_IDs = filter.MEDI_STOCK_BUSINESS_IDs;
                medicinefilter.EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN;
                if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    medicinefilter.EXP_TIME_FROM = filter.TIME_FROM;
                    medicinefilter.EXP_TIME_TO = filter.TIME_TO;
                    medicinefilter.IS_EXPORT = true;
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    medicinefilter.CREATE_TIME_FROM = filter.TIME_FROM;
                    medicinefilter.CREATE_TIME_TO = filter.TIME_TO;
                }
                else
                {
                    medicinefilter.EXP_TIME_FROM = filter.TIME_FROM;
                    medicinefilter.EXP_TIME_TO = filter.TIME_TO;
                    medicinefilter.IS_EXPORT = true;
                }

                medicinefilter.TDL_MEDICINE_TYPE_IDs = filter.MEDICINE_TYPE_IDs;
                listMedicine = new HisExpMestMedicineManager(paramGet).GetView(medicinefilter);//
                FilterParentService(ref listMedicine);
                if (listMedicine != null)
                {
                    expMesstIds.AddRange(listMedicine.Where(p => p.EXP_MEST_ID.HasValue).Select(o => o.EXP_MEST_ID.Value).ToList());
                }
                HisExpMestMaterialViewFilterQuery materialfilter = new HisExpMestMaterialViewFilterQuery();
                materialfilter.MEDI_STOCK_IDs = filter.MEDI_STOCK_BUSINESS_IDs;
                materialfilter.EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN;
                if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    materialfilter.EXP_TIME_FROM = filter.TIME_FROM;
                    materialfilter.EXP_TIME_TO = filter.TIME_TO;
                    materialfilter.IS_EXPORT = true;
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    materialfilter.CREATE_TIME_FROM = filter.TIME_FROM;
                    materialfilter.CREATE_TIME_TO = filter.TIME_TO;
                }
                else
                {
                    materialfilter.EXP_TIME_FROM = filter.TIME_FROM;
                    materialfilter.EXP_TIME_TO = filter.TIME_TO;
                    materialfilter.IS_EXPORT = true;
                }

                materialfilter.TDL_MATERIAL_TYPE_IDs = filter.MATERIAL_TYPE_IDs;

                listMaterial = new HisExpMestMaterialManager(paramGet).GetView(materialfilter);
                FilterParentService(ref listMaterial);

                if (listMaterial != null)
                {
                    expMesstIds.AddRange(listMaterial.Where(p => p.EXP_MEST_ID.HasValue).Select(o => o.EXP_MEST_ID.Value).ToList());
                }
                expMesstIds = expMesstIds.Distinct().ToList();

                if (IsNotNullOrEmpty(expMesstIds))
                {
                    var skip = 0;
                    while (expMesstIds.Count - skip > 0)
                    {
                        var lists = expMesstIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisExpMestViewFilterQuery ExpFilter = new HisExpMestViewFilterQuery();
                        ExpFilter.IDs = lists;

                        var listSaleExpMestSub = new HisExpMestManager(paramGet).GetView(ExpFilter);
                        if (!string.IsNullOrWhiteSpace(filter.LOGINNAME_SALE)) //theo nguoi ban
                        {
                            listSaleExpMestSub = listSaleExpMestSub.Where(p => p.REQ_LOGINNAME == filter.LOGINNAME_SALE).ToList();
                        }

                        if (!string.IsNullOrWhiteSpace(filter.REQUEST_LOGINNAME)) //theo nguoi yêu cầu
                        {
                            listSaleExpMestSub = listSaleExpMestSub.Where(p => p.TDL_PRESCRIPTION_REQ_LOGINNAME == filter.REQUEST_LOGINNAME).ToList();
                        }

                        if (listSaleExpMestSub != null)
                        {
                            listSaleExpMest.AddRange(listSaleExpMestSub);
                        }
                    }
                }
                if (IsNotNullOrEmpty(listSaleExpMest))
                {
                    if (!string.IsNullOrWhiteSpace(filter.LOGINNAME_SALE))
                    {
                        listSaleExpMest = listSaleExpMest.Where(o => o.REQ_LOGINNAME == filter.LOGINNAME_SALE).ToList();
                    }

                    if (!string.IsNullOrWhiteSpace(filter.REQUEST_LOGINNAME))
                    {
                        listSaleExpMest = listSaleExpMest.Where(o => o.TDL_PRESCRIPTION_REQ_LOGINNAME == filter.REQUEST_LOGINNAME).ToList();
                    }

                    if (!string.IsNullOrWhiteSpace(filter.CASHIER_LOGINNAME))
                    {
                        listSaleExpMest = listSaleExpMest.Where(o => o.CASHIER_LOGINNAME == filter.CASHIER_LOGINNAME).ToList();
                    }

                    if (filter.CASHIER_LOGINNAMEs != null)
                    {
                        listSaleExpMest = listSaleExpMest.Where(o => filter.CASHIER_LOGINNAMEs.Contains(o.CASHIER_LOGINNAME ?? "")).ToList();
                    }

                    if (filter.NO_BILL_PAY_FORM_IDs != null)
                    {
                        listSaleExpMest = listSaleExpMest.Where(p => filter.NO_BILL_PAY_FORM_IDs.Contains(p.PAY_FORM_ID ?? 0)).ToList();
                    }
                    if (filter.PAY_FORM_IDs != null)
                    {
                        listSaleExpMest = listSaleExpMest.Where(p => filter.PAY_FORM_IDs.Contains(p.PAY_FORM_ID ?? 0)).ToList();
                    }
                    var expMestIds = listSaleExpMest.Select(o => o.ID).ToList() ?? new List<long>();

                    listMedicine = listMedicine.Where(p => expMestIds.Contains(p.EXP_MEST_ID ?? 0)).ToList();
                    listMaterial = listMaterial.Where(p => expMestIds.Contains(p.EXP_MEST_ID ?? 0)).ToList();
                    var prescriptionIds = listSaleExpMest.Where(o => o.PRESCRIPTION_ID.HasValue).Select(p => p.PRESCRIPTION_ID.Value).ToList();
                    var skip = 0;
                    while (prescriptionIds.Count - skip > 0)
                    {
                        var lists = prescriptionIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisServiceReqFilterQuery srFilter = new HisServiceReqFilterQuery();
                        srFilter.IDs = lists;
                        var listServiceReqSub = new HisServiceReqManager(paramGet).Get(srFilter);

                        if (listServiceReqSub != null)
                        {
                            listServiceReq.AddRange(listServiceReqSub);
                        }
                    }

                    var billIds = listSaleExpMest.Select(s => s.BILL_ID ?? 0).Distinct().ToList();
                    skip = 0;
                    while (billIds.Count - skip > 0)
                    {
                        var listID = billIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisTransactionFilterQuery tranFilter = new HisTransactionFilterQuery();
                        tranFilter.IDs = listID;

                        var listTranSub = new HisTransactionManager(paramGet).Get(tranFilter);
                        //if (filter.NO_BILL_PAY_FORM_IDs != null)
                        //{
                        //    listTranSub = listTranSub.Where(p => filter.NO_BILL_PAY_FORM_IDs.Contains(p.PAY_FORM_ID) || p.BILL_ID == null).ToList();
                        //}
                        if (filter.PAY_FORM_IDs != null)
                        {
                            listTranSub = listTranSub.Where(p => filter.PAY_FORM_IDs.Contains(p.PAY_FORM_ID)).ToList();
                        }
                        if (listTranSub != null)
                        {
                            ListTransaction.AddRange(listTranSub);
                        }
                    }
                }
                //xuất
                if (filter.PAY_FORM_IDs != null)
                {
                    var transactionIds = ListTransaction.Select(o => o.ID).ToList();
                    var expMestIds = listSaleExpMest.Where(p => transactionIds.Contains(p.BILL_ID ?? 0)).Select(o => o.ID).ToList() ?? new List<long>();
                    listMedicine = listMedicine.Where(p => expMestIds.Contains(p.EXP_MEST_ID ?? 0)).ToList();

                    listMaterial = listMaterial.Where(p => expMestIds.Contains(p.EXP_MEST_ID ?? 0)).ToList();

                }

                if (filter.NO_BILL_PAY_FORM_IDs != null)
                {
                    //var transactionIds = ListTransaction.Select(o => o.ID).ToList();
                    var expMestIds = listSaleExpMest/*.Where(p => transactionIds.Contains(p.BILL_ID ?? 0))*/.Select(o => o.ID).ToList() ?? new List<long>();

                    listMedicine = listMedicine.Where(p => expMestIds.Contains(p.EXP_MEST_ID ?? 0)).ToList();

                    listMaterial = listMaterial.Where(p => expMestIds.Contains(p.EXP_MEST_ID ?? 0)).ToList();
                }
                //nhập
                List<long> impMestIds = new List<long>();
                HisImpMestMedicineViewFilterQuery impmedicinefilter = new HisImpMestMedicineViewFilterQuery();
                impmedicinefilter.MEDI_STOCK_IDs = filter.MEDI_STOCK_BUSINESS_IDs;
                impmedicinefilter.IMP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BTL;
                impmedicinefilter.IMP_TIME_FROM = filter.TIME_FROM;
                impmedicinefilter.IMP_TIME_TO = filter.TIME_TO;
                impmedicinefilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                impmedicinefilter.MEDICINE_TYPE_IDs = filter.MEDICINE_TYPE_IDs;
                listImpMedicine = new HisImpMestMedicineManager(paramGet).GetView(impmedicinefilter);//

                FilterParentService(ref listImpMedicine);

                if (listImpMedicine != null)
                {
                    impMestIds.AddRange(listImpMedicine.Where(p => p.IMP_MEST_ID > 0).Select(o => o.IMP_MEST_ID).ToList());
                }
                HisImpMestMaterialViewFilterQuery impmaterialfilter = new HisImpMestMaterialViewFilterQuery();
                impmaterialfilter.MEDI_STOCK_IDs = filter.MEDI_STOCK_BUSINESS_IDs;
                impmaterialfilter.IMP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BTL;
                impmaterialfilter.IMP_TIME_FROM = filter.TIME_FROM;
                impmaterialfilter.IMP_TIME_TO = filter.TIME_TO;
                impmaterialfilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                impmaterialfilter.MATERIAL_TYPE_IDs = filter.MATERIAL_TYPE_IDs;
                listImpMaterial = new HisImpMestMaterialManager(paramGet).GetView(impmaterialfilter);

                FilterParentService(ref listImpMaterial);

                if (listImpMaterial != null)
                {
                    impMestIds.AddRange(listImpMaterial.Where(p => p.IMP_MEST_ID > 0).Select(o => o.IMP_MEST_ID).ToList());
                }
                impMestIds = impMestIds.Distinct().ToList();

                if (IsNotNullOrEmpty(impMestIds))
                {
                    var skip = 0;
                    while (impMestIds.Count - skip > 0)
                    {
                        var lists = impMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisImpMestViewFilterQuery ImpFilter = new HisImpMestViewFilterQuery();
                        ImpFilter.IDs = lists;

                        var listSaleImpMestSub = new HisImpMestManager(paramGet).GetView(ImpFilter);
                        if (!string.IsNullOrWhiteSpace(filter.LOGINNAME_SALE)) //theo nguoi ban
                        {
                            listSaleImpMestSub = listSaleImpMestSub.Where(p => p.REQ_LOGINNAME == filter.LOGINNAME_SALE).ToList();
                        }
                        //if (!string.IsNullOrWhiteSpace(filter.REQUEST_LOGINNAME)) //theo bac si chi dinh
                        //{
                        //    listSaleImpMestSub = listSaleImpMestSub.Where(p => p.TDL_PRESCRIPTION_REQ_LOGINNAME == filter.REQUEST_LOGINNAME).ToList();
                        //}
                        if (listSaleImpMestSub != null)
                        {
                            listSaleImpMest.AddRange(listSaleImpMestSub);
                        }
                    }
                }
                //đi tìm giá bán
                List<long> thExpMestMedicineIds = listImpMedicine.Select(o => o.TH_EXP_MEST_MEDICINE_ID ?? 0).Distinct().ToList();

                if (IsNotNullOrEmpty(thExpMestMedicineIds))
                {
                    var skip = 0;
                    while (thExpMestMedicineIds.Count - skip > 0)
                    {
                        var lists = thExpMestMedicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisExpMestMedicineViewFilterQuery ExpMestMedicineFilter = new HisExpMestMedicineViewFilterQuery();
                        ExpMestMedicineFilter.IDs = lists;

                        var listThExpMestMedicineSub = new HisExpMestMedicineManager(paramGet).GetView(ExpMestMedicineFilter);
                        if (this.IsNotNullOrEmpty(listThExpMestMedicineSub)) //theo nguoi ban
                        {
                            foreach (var item in listThExpMestMedicineSub)
                            {
                                if (!dicThExpMestMedicinePrice.ContainsKey(item.ID))
                                {
                                    dicThExpMestMedicinePrice[item.ID] = (item.PRICE ?? 0);
                                }
                                if (!dicThExpMestMedicineVat.ContainsKey(item.ID))
                                {
                                    dicThExpMestMedicineVat[item.ID] = (item.VAT_RATIO ?? 0);
                                }
                            }
                        }

                    }
                }
                List<long> thExpMestMaterialIds = listImpMaterial.Select(o => o.TH_EXP_MEST_MATERIAL_ID ?? 0).Distinct().ToList();

                if (IsNotNullOrEmpty(thExpMestMaterialIds))
                {
                    var skip = 0;
                    while (thExpMestMaterialIds.Count - skip > 0)
                    {
                        var lists = thExpMestMaterialIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisExpMestMaterialViewFilterQuery ExpMestMaterialFilter = new HisExpMestMaterialViewFilterQuery();
                        ExpMestMaterialFilter.IDs = lists;

                        var listThExpMestMaterialSub = new HisExpMestMaterialManager(paramGet).GetView(ExpMestMaterialFilter);
                        if (this.IsNotNullOrEmpty(listThExpMestMaterialSub)) //theo nguoi ban
                        {
                            foreach (var item in listThExpMestMaterialSub)
                            {
                                if (!dicThExpMestMaterialPrice.ContainsKey(item.ID))
                                {
                                    dicThExpMestMaterialPrice[item.ID] = (item.PRICE ?? 0);
                                }
                                if (!dicThExpMestMaterialVat.ContainsKey(item.ID))
                                {
                                    dicThExpMestMaterialVat[item.ID] = (item.VAT_RATIO ?? 0);
                                }
                            }
                        }

                    }
                }
                //Khoa kê đơn
                List<long> mobaExpMestIds = listSaleImpMest.Select(o => o.MOBA_EXP_MEST_ID ?? 0).Distinct().ToList();

                if (IsNotNullOrEmpty(mobaExpMestIds))
                {
                    var skip = 0;
                    while (mobaExpMestIds.Count - skip > 0)
                    {
                        var lists = mobaExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisExpMestViewFilterQuery ExpMestFilter = new HisExpMestViewFilterQuery();
                        ExpMestFilter.IDs = lists;

                        var listMobaExpMestSub = new HisExpMestManager(paramGet).GetView(ExpMestFilter);
                        if (this.IsNotNullOrEmpty(listMobaExpMestSub)) //theo nguoi ban
                        {
                            listMobaExpMest.AddRange(listMobaExpMestSub);
                        }

                    }
                    var prescriptionIds = listMobaExpMest.Where(o => o.PRESCRIPTION_ID.HasValue).Select(p => p.PRESCRIPTION_ID.Value).ToList();
                    skip = 0;
                    while (prescriptionIds.Count - skip > 0)
                    {
                        var lists = prescriptionIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisServiceReqFilterQuery srFilter = new HisServiceReqFilterQuery();
                        srFilter.IDs = lists;
                        var listServiceReqSub = new HisServiceReqManager(paramGet).Get(srFilter);

                        if (listServiceReqSub != null)
                        {
                            listServiceReq.AddRange(listServiceReqSub);
                        }
                    }
                }
                var minCreateExpMest = listSaleExpMest.Min(o => o.CREATE_TIME ?? o.FINISH_TIME);
                listPrintLogUnique = new ManagerSql().GetPrintLog(this.filter, minCreateExpMest) ?? new List<PrintLogUnique>();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void FilterParentService(ref List<V_HIS_EXP_MEST_MEDICINE> _listMedicine)
        {
            if (filter.SERVICE_TYPE_IDs != null)
            {
                if (!filter.SERVICE_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC))
                {
                    _listMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
                }
            }
            if (services == null || services.Count == 0)
            {
                services = new HisServiceManager().Get(new HisServiceFilterQuery());
            }
            if (filter.EXACT_PARENT_SERVICE_IDs != null)
            {
                services = services.Where(o => filter.EXACT_PARENT_SERVICE_IDs.Contains(o.PARENT_ID ?? 0)).ToList();
            }
            var svIds = services.Select(o => o.ID).ToList();
            _listMedicine = _listMedicine.Where(o => svIds.Contains(o.SERVICE_ID)).ToList();

        }

        private void FilterParentService(ref List<V_HIS_EXP_MEST_MATERIAL> _listMaterial)
        {

            if (filter.SERVICE_TYPE_IDs != null)
            {
                if (!filter.SERVICE_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT))
                {
                    _listMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();
                }
            }
            if (services == null || services.Count == 0)
            {
                services = new HisServiceManager().Get(new HisServiceFilterQuery());
            }
            if (filter.EXACT_PARENT_SERVICE_IDs != null)
            {
                services = services.Where(o => filter.EXACT_PARENT_SERVICE_IDs.Contains(o.PARENT_ID ?? 0)).ToList();
            }
            var svIds = services.Select(o => o.ID).ToList();
            _listMaterial = _listMaterial.Where(o => svIds.Contains(o.SERVICE_ID)).ToList();

        }

        private void FilterParentService(ref List<V_HIS_IMP_MEST_MEDICINE> _listMedicine)
        {
            if (filter.SERVICE_TYPE_IDs != null)
            {
                if (!filter.SERVICE_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC))
                {
                    _listMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
                }
            }
            if (services == null || services.Count == 0)
            {
                services = new HisServiceManager().Get(new HisServiceFilterQuery());
            }
            if (filter.EXACT_PARENT_SERVICE_IDs != null)
            {
                services = services.Where(o => filter.EXACT_PARENT_SERVICE_IDs.Contains(o.PARENT_ID ?? 0)).ToList();
            }
            var svIds = services.Select(o => o.ID).ToList();
            _listMedicine = _listMedicine.Where(o => svIds.Contains(o.SERVICE_ID)).ToList();

        }

        private void FilterParentService(ref List<V_HIS_IMP_MEST_MATERIAL> _listMaterial)
        {

            if (filter.SERVICE_TYPE_IDs != null)
            {
                if (!filter.SERVICE_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT))
                {
                    _listMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();
                }
            }
            if (services == null || services.Count == 0)
            {
                services = new HisServiceManager().Get(new HisServiceFilterQuery());
            }
            if (filter.EXACT_PARENT_SERVICE_IDs != null)
            {
                services = services.Where(o => filter.EXACT_PARENT_SERVICE_IDs.Contains(o.PARENT_ID ?? 0)).ToList();
            }
            var svIds = services.Select(o => o.ID).ToList();
            _listMaterial = _listMaterial.Where(o => svIds.Contains(o.SERVICE_ID)).ToList();

        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                listRdo.Clear();
                ListExpRdo.Clear();


                if (IsNotNullOrEmpty(listPrintLogUnique))
                {
                    foreach (var item in listPrintLogUnique)
                    {
                        if (!string.IsNullOrWhiteSpace(item.UNIQUE_CODE))
                        {
                            string[] uniques = item.UNIQUE_CODE.Split('_');
                            if (uniques.Length == 4)
                            {
                                item.TRANSACTION_CODE = uniques[2];
                            }
                            else
                            {
                                int id = item.UNIQUE_CODE.IndexOf("TRANSACTION_CODE:");
                                if (id >= 0)
                                    item.TRANSACTION_CODE = item.UNIQUE_CODE.Substring(id + 17, 12);
                            }
                        }
                    }
                    dicPrintLogUnique = listPrintLogUnique.GroupBy(o => o.TRANSACTION_CODE).ToDictionary(p => p.Key, q => q.ToList());
                }
                if (IsNotNullOrEmpty(listMedicine))
                {
                    LogSystem.Info("count:" + listMedicine.Count());
                    foreach (var Medicine in listMedicine)
                    {
                        V_HIS_EXP_MEST sale = listSaleExpMest.Where(o => o.ID == Medicine.EXP_MEST_ID).FirstOrDefault() ?? new V_HIS_EXP_MEST();
                        //var impMedi = listImpMedicine.FirstOrDefault(o => o.TH_EXP_MEST_MEDICINE_ID == Medicine.EXP_MEST_ID);
                        var serviceReq = listServiceReq.FirstOrDefault(o => o.ID == sale.PRESCRIPTION_ID);
                        //if (!string.IsNullOrWhiteSpace(filter.REQUEST_LOGINNAME))
                        //{
                        //    if (sale.TDL_PRESCRIPTION_REQ_LOGINNAME != filter.REQUEST_LOGINNAME && (serviceReq == null || serviceReq.REQUEST_LOGINNAME != filter.LOGINNAME_SALE))
                        //    {
                        //        continue;
                        //    }
                        //}
                        Mrs00274RDO rdo = new Mrs00274RDO(sale);
                        rdo.THUOC_VATTU = "THUOC";
                        rdo.MAME_TYPE_NAME = Medicine.MEDICINE_TYPE_NAME;
                        rdo.MAME_TYPE_CODE = Medicine.MEDICINE_TYPE_CODE;
                        if (!string.IsNullOrWhiteSpace(filter.BIOLOGY_PRODUCTs))
                        {
                            string[] biologyProducts = filter.BIOLOGY_PRODUCTs.Split(',');
                            if (biologyProducts.Contains(Medicine.MEDICINE_TYPE_CODE))
                            {
                                rdo.IS_BIOLOGY_PRODUCT = 1;
                            }
                        }
                        rdo.NATIONAL_NAME = Medicine.NATIONAL_NAME;
                        rdo.SERVICE_UNIT_NAME = Medicine.SERVICE_UNIT_NAME;
                        rdo.PRICE = Medicine.PRICE.HasValue ? Medicine.PRICE.Value * (1 + (Medicine.VAT_RATIO ?? 0)) : Medicine.IMP_PRICE * (1 + Medicine.IMP_VAT_RATIO);
                        rdo.IMP_PRICE = Medicine.IMP_PRICE * (1 + Medicine.IMP_VAT_RATIO);
                        rdo.AMOUNT = Medicine.AMOUNT;
                        rdo.TOTAL_PRICE = Medicine.PRICE.HasValue ? Medicine.PRICE.Value * (1 + (Medicine.VAT_RATIO ?? 0)) * Medicine.AMOUNT : Medicine.IMP_PRICE * (1 + Medicine.IMP_VAT_RATIO) * Medicine.AMOUNT;
                        rdo.FUND = Medicine.IMP_PRICE * (1 + Medicine.IMP_VAT_RATIO) * Medicine.AMOUNT;
                        rdo.TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)(Medicine.EXP_TIME ?? Medicine.CREATE_TIME));
                        rdo.TDL_PATIENT_NAME = sale.TDL_PATIENT_NAME;

                        rdo.TREATMENT_CODE = sale.TDL_TREATMENT_CODE;
                        rdo.EXP_MEST_CODE = sale.EXP_MEST_CODE;
                        rdo.PATIENT_TYPE_NAME = sale.PATIENT_TYPE_NAME;

                        rdo.EXPIRED_DATE = Medicine.EXPIRED_DATE;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(Medicine.EXPIRED_DATE ?? 0);
                        rdo.PACKAGE_NUMBER = Medicine.PACKAGE_NUMBER;

                        if (serviceReq != null)
                        {
                            rdo.REQUEST_LOGINNAME = serviceReq.REQUEST_LOGINNAME;
                            rdo.REQUEST_USERNAME = serviceReq.REQUEST_USERNAME;
                            rdo.ICD_CODE = serviceReq.ICD_CODE;
                            rdo.ICD_NAME = serviceReq.ICD_NAME;
                            rdo.SERVICE_REQ_CODE = serviceReq.SERVICE_REQ_CODE ?? "";
                            rdo.REQUEST_DEPARTMENT_ID = serviceReq.REQUEST_DEPARTMENT_ID;
                            var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == serviceReq.REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT();
                            rdo.REQUEST_DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                            rdo.REQUEST_DEPARTMENT_NAME = department.DEPARTMENT_NAME;

                        }
                        else
                        {
                            rdo.REQUEST_LOGINNAME = sale.TDL_PRESCRIPTION_REQ_LOGINNAME;
                            rdo.REQUEST_USERNAME = sale.TDL_PRESCRIPTION_REQ_USERNAME;
                        }

                        if (sale.BILL_ID.HasValue)
                        {
                            rdo.HAS_BILL = (short)1;
                            var tran = ListTransaction.FirstOrDefault(o => o.ID == sale.BILL_ID);
                            if (tran != null)
                            {
                                rdo.TRANSACTION_CODE = tran.TRANSACTION_CODE;
                                rdo.TRANSACTION_NUM_ORDER = tran.NUM_ORDER;
                                rdo.EINVOICE_NUM_ORDER = tran.EINVOICE_NUM_ORDER;
                                rdo.EINVOICE_TIME = tran.EINVOICE_TIME ?? 0;
                                rdo.IS_PRINTED = this.IsPrinted(tran) ? (short)1 : (short)0;
                            }
                        }
                        //if (impMedi != null)
                        //{
                        //    rdo.TH_AMOUNT = impMedi.AMOUNT;
                        //    rdo.TH_PRICE = impMedi.AMOUNT * impMedi.IMP_PRICE;
                        //    rdo.REAL_PRICE = rdo.TOTAL_PRICE - rdo.TH_PRICE;
                        //}

                        rdo.MEDICINE_HOATCHAT_NAME = Medicine.ACTIVE_INGR_BHYT_NAME;
                        rdo.MEDICINE_CODE_DMBYT = Medicine.ACTIVE_INGR_BHYT_CODE;
                        rdo.MEDICINE_UNIT_NAME = Medicine.SERVICE_UNIT_NAME;
                        rdo.MEDICINE_DUONGDUNG_NAME = Medicine.MEDICINE_USE_FORM_NAME;
                        rdo.MEDICINE_DUONGDUNG_CODE = Medicine.MEDICINE_USE_FORM_CODE;
                        rdo.MANUFACTURER_NAME = Medicine.MANUFACTURER_NAME;
                        rdo.CONCENTRA = Medicine.CONCENTRA;

                        listRdo.Add(rdo);
                    }
                }

                if (IsNotNullOrEmpty(listMaterial))
                {

                    foreach (var Material in listMaterial)
                    {
                        V_HIS_EXP_MEST sale = listSaleExpMest.Where(o => o.ID == Material.EXP_MEST_ID).FirstOrDefault() ?? new V_HIS_EXP_MEST();
                        ////var impMate = listImpMaterial.FirstOrDefault(o => o.TH_EXP_MEST_MATERIAL_ID == Material.EXP_MEST_ID);
                        var serviceReq = listServiceReq.FirstOrDefault(o => o.ID == sale.PRESCRIPTION_ID);
                        //if (!string.IsNullOrWhiteSpace(filter.REQUEST_LOGINNAME))
                        //{
                        //    if (sale.TDL_PRESCRIPTION_REQ_LOGINNAME != filter.REQUEST_LOGINNAME && (serviceReq == null || serviceReq.REQUEST_LOGINNAME != filter.LOGINNAME_SALE))
                        //    {
                        //        continue;
                        //    }
                        //}

                        Mrs00274RDO rdo = new Mrs00274RDO(sale);
                        rdo.THUOC_VATTU = "VATTU";
                        rdo.MAME_TYPE_NAME = Material.MATERIAL_TYPE_NAME;
                        rdo.MAME_TYPE_CODE = Material.MATERIAL_TYPE_CODE;
                        if (!string.IsNullOrWhiteSpace(filter.BIOLOGY_PRODUCTs))
                        {
                            string[] biologyProducts = filter.BIOLOGY_PRODUCTs.Split(',');
                            if (biologyProducts.Contains(Material.MATERIAL_TYPE_CODE))
                            {
                                rdo.IS_BIOLOGY_PRODUCT = 1;
                            }
                        }
                        rdo.NATIONAL_NAME = Material.NATIONAL_NAME;
                        rdo.SERVICE_UNIT_NAME = Material.SERVICE_UNIT_NAME;
                        rdo.IMP_PRICE = Material.IMP_PRICE * (1 + Material.IMP_VAT_RATIO);
                        rdo.PRICE = Material.PRICE.HasValue ? Material.PRICE.Value * (1 + (Material.VAT_RATIO ?? 0)) : Material.IMP_PRICE * (1 + Material.IMP_VAT_RATIO);
                        rdo.AMOUNT = Material.AMOUNT;
                        rdo.TOTAL_PRICE = Material.PRICE.HasValue ? Material.PRICE.Value * (1 + (Material.VAT_RATIO ?? 0)) * Material.AMOUNT : Material.IMP_PRICE * (1 + Material.IMP_VAT_RATIO) * Material.AMOUNT;
                        rdo.TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)(Material.EXP_TIME ?? Material.CREATE_TIME));
                        rdo.TDL_PATIENT_NAME = sale.TDL_PATIENT_NAME;


                        rdo.TREATMENT_CODE = sale.TDL_TREATMENT_CODE;
                        rdo.EXP_MEST_CODE = sale.EXP_MEST_CODE;
                        rdo.EXPIRED_DATE = Material.EXPIRED_DATE;

                        rdo.PACKAGE_NUMBER = Material.PACKAGE_NUMBER;
                        if (serviceReq != null)
                        {
                            rdo.REQUEST_LOGINNAME = serviceReq.REQUEST_LOGINNAME;
                            rdo.REQUEST_USERNAME = serviceReq.REQUEST_USERNAME;
                            rdo.ICD_CODE = serviceReq.ICD_CODE;
                            rdo.ICD_NAME = serviceReq.ICD_NAME;
                            var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == serviceReq.REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT();
                            rdo.REQUEST_DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                            rdo.REQUEST_DEPARTMENT_NAME = department.DEPARTMENT_NAME;

                        }
                        else
                        {
                            rdo.REQUEST_LOGINNAME = sale.TDL_PRESCRIPTION_REQ_LOGINNAME;
                            rdo.REQUEST_USERNAME = sale.TDL_PRESCRIPTION_REQ_USERNAME;
                        }
                        if (sale.BILL_ID.HasValue)
                        {
                            var tran = ListTransaction.FirstOrDefault(o => o.ID == sale.BILL_ID);
                            if (tran != null)
                            {
                                rdo.HAS_BILL = (short)1;
                                rdo.TRANSACTION_CODE = tran.TRANSACTION_CODE;
                                rdo.TRANSACTION_NUM_ORDER = tran.NUM_ORDER;
                                rdo.EINVOICE_NUM_ORDER = tran.EINVOICE_NUM_ORDER;
                                rdo.EINVOICE_TIME = tran.EINVOICE_TIME??0;
                                rdo.IS_PRINTED = this.IsPrinted(tran) ? (short)1 : (short)0;
                            }
                        }
                        //if (impMate != null)
                        //{
                        //    rdo.TH_AMOUNT = impMate.AMOUNT;
                        //    rdo.TH_PRICE = impMate.AMOUNT * impMate.IMP_PRICE;
                        //    rdo.REAL_PRICE = rdo.TOTAL_PRICE - rdo.TH_PRICE;
                        //}

                        listRdo.Add(rdo);
                    }
                }
                //nhập
                if (IsNotNullOrEmpty(listImpMedicine))
                {
                    LogSystem.Info("count:" + listImpMedicine.Count());
                    foreach (var Medicine in listImpMedicine)
                    {
                        V_HIS_IMP_MEST sale = listSaleImpMest.Where(o => o.ID == Medicine.IMP_MEST_ID).FirstOrDefault() ?? new V_HIS_IMP_MEST();
                        V_HIS_EXP_MEST_MEDICINE exp = listMedicine.Where(o => o.MEDICINE_TYPE_ID == Medicine.MEDICINE_TYPE_ID).FirstOrDefault() ?? new V_HIS_EXP_MEST_MEDICINE();

                        if (!string.IsNullOrWhiteSpace(filter.REQUEST_LOGINNAME))
                        {
                            continue;
                        }
                        var mobaExpMest = listMobaExpMest.FirstOrDefault(o => o.ID == sale.MOBA_EXP_MEST_ID) ?? new V_HIS_EXP_MEST();

                        Mrs00274RDO rdo = new Mrs00274RDO(mobaExpMest);
                        rdo.THUOC_VATTU = "THUOC";
                        rdo.MAME_TYPE_NAME = Medicine.MEDICINE_TYPE_NAME;
                        rdo.MAME_TYPE_CODE = Medicine.MEDICINE_TYPE_CODE;
                        if (!string.IsNullOrWhiteSpace(filter.BIOLOGY_PRODUCTs))
                        {
                            string[] biologyProducts = filter.BIOLOGY_PRODUCTs.Split(',');
                            if (biologyProducts.Contains(Medicine.MEDICINE_TYPE_CODE))
                            {
                                rdo.IS_BIOLOGY_PRODUCT = 1;
                            }
                        }
                        rdo.NATIONAL_NAME = Medicine.NATIONAL_NAME;
                        rdo.SERVICE_UNIT_NAME = Medicine.SERVICE_UNIT_NAME;
                        rdo.PRICE = Medicine.PRICE.HasValue ? Medicine.PRICE.Value * (1 + (Medicine.VAT_RATIO ?? 0)) : Medicine.IMP_PRICE * (1 + Medicine.IMP_VAT_RATIO);
                        rdo.PRICE_1 = Medicine.TH_EXP_MEST_MEDICINE_ID != null && dicThExpMestMedicinePrice.ContainsKey(Medicine.TH_EXP_MEST_MEDICINE_ID ?? 0)
                            ? dicThExpMestMedicinePrice[Medicine.TH_EXP_MEST_MEDICINE_ID ?? 0]
                            : (Medicine.PRICE ?? 0) * (1 + (Medicine.VAT_RATIO ?? 0));
                        rdo.VAT_RATIO_1 = Medicine.TH_EXP_MEST_MEDICINE_ID != null && dicThExpMestMedicineVat.ContainsKey(Medicine.TH_EXP_MEST_MEDICINE_ID ?? 0)
                            ? dicThExpMestMedicineVat[Medicine.TH_EXP_MEST_MEDICINE_ID ?? 0]
                            : 0;
                        var serviceReq = listServiceReq.FirstOrDefault(o => o.ID == mobaExpMest.PRESCRIPTION_ID);
                        if (serviceReq != null)
                        {
                            rdo.REQUEST_LOGINNAME = serviceReq.REQUEST_LOGINNAME;
                            rdo.REQUEST_USERNAME = serviceReq.REQUEST_USERNAME;
                            rdo.ICD_CODE = serviceReq.ICD_CODE;
                            rdo.ICD_NAME = serviceReq.ICD_NAME;
                            var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == serviceReq.REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT();
                            rdo.REQUEST_DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                            rdo.REQUEST_DEPARTMENT_NAME = department.DEPARTMENT_NAME;


                        }
                        else
                        {
                            rdo.REQUEST_LOGINNAME = mobaExpMest.TDL_PRESCRIPTION_REQ_LOGINNAME;
                            rdo.REQUEST_USERNAME = mobaExpMest.TDL_PRESCRIPTION_REQ_USERNAME;
                        }
                        rdo.IMP_PRICE = Medicine.IMP_PRICE * (1 + Medicine.IMP_VAT_RATIO);
                        rdo.EXP_PRICE = exp.PRICE * (1 + exp.VAT_RATIO);
                        rdo.AMOUNT = Medicine.AMOUNT;
                        rdo.TOTAL_PRICE = rdo.PRICE * rdo.AMOUNT;
                        rdo.EXP_TOTAL_PRICE = rdo.EXP_PRICE * rdo.AMOUNT;
                        rdo.FUND = rdo.IMP_PRICE * rdo.AMOUNT;
                        //rdo.REQ_LOGINNAME = sale.REQ_LOGINNAME;
                        //rdo.REQ_USERNAME = sale.REQ_USERNAME;
                        rdo.TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)Medicine.IMP_TIME);
                        rdo.TDL_PATIENT_NAME = sale.TDL_PATIENT_NAME;

                        rdo.TREATMENT_CODE = sale.TDL_TREATMENT_CODE;
                        rdo.IMP_MEST_CODE = sale.IMP_MEST_CODE;
                        //rdo.PATIENT_TYPE_NAME = sale.PATIENT_TYPE_NAME;

                        rdo.EXPIRED_DATE = Medicine.EXPIRED_DATE;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(Medicine.EXPIRED_DATE ?? 0);
                        rdo.PACKAGE_NUMBER = Medicine.PACKAGE_NUMBER;
                        rdo.EXP_MEST_CODE = mobaExpMest.EXP_MEST_CODE;
                        //if (serviceReq != null)
                        //{
                        //    rdo.REQUEST_LOGINNAME = serviceReq.REQUEST_LOGINNAME;
                        //    rdo.REQUEST_USERNAME = serviceReq.REQUEST_USERNAME;
                        //    rdo.ICD_CODE = serviceReq.ICD_CODE;
                        //    rdo.ICD_NAME = serviceReq.ICD_NAME;
                        //    rdo.SERVICE_REQ_CODE = serviceReq.SERVICE_REQ_CODE ?? "";

                        //}
                        //else
                        //{
                        //    rdo.REQUEST_LOGINNAME = sale.TDL_PRESCRIPTION_REQ_LOGINNAME;
                        //    rdo.REQUEST_USERNAME = sale.TDL_PRESCRIPTION_REQ_USERNAME;
                        //}

                        //if (sale.BILL_ID.HasValue)
                        //{
                        //    var tran = ListTransaction.FirstOrDefault(o => o.ID == sale.BILL_ID);
                        //    if (tran != null)
                        //    {
                        //        rdo.TRANSACTION_CODE = tran.TRANSACTION_CODE;
                        //        rdo.TRANSACTION_NUM_ORDER = tran.NUM_ORDER;
                        //        rdo.IS_PRINTED = this.IsPrinted(tran) ? (short)1 : (short)0;
                        //    }
                        //}

                        rdo.MEDICINE_HOATCHAT_NAME = Medicine.ACTIVE_INGR_BHYT_NAME;
                        rdo.MEDICINE_CODE_DMBYT = Medicine.ACTIVE_INGR_BHYT_CODE;
                        rdo.MEDICINE_UNIT_NAME = Medicine.SERVICE_UNIT_NAME;
                        //rdo.MEDICINE_DUONGDUNG_NAME = Medicine.MEDICINE_USE_FORM_NAME;
                        //rdo.MEDICINE_DUONGDUNG_CODE = Medicine.MEDICINE_USE_FORM_CODE;
                        rdo.MANUFACTURER_NAME = Medicine.MANUFACTURER_NAME;
                        rdo.CONCENTRA = Medicine.CONCENTRA;

                        listImpRdo.Add(rdo);
                    }
                }
                if (IsNotNullOrEmpty(listImpMaterial))
                {

                    foreach (var Material in listImpMaterial)
                    {
                        V_HIS_IMP_MEST sale = listSaleImpMest.Where(o => o.ID == Material.IMP_MEST_ID).FirstOrDefault() ?? new V_HIS_IMP_MEST();
                        V_HIS_EXP_MEST_MATERIAL exp = listMaterial.Where(o => o.MATERIAL_TYPE_ID == Material.MATERIAL_TYPE_ID).FirstOrDefault() ?? new V_HIS_EXP_MEST_MATERIAL();

                        if (!string.IsNullOrWhiteSpace(filter.REQUEST_LOGINNAME))
                        {
                            continue;
                        }

                        var mobaExpMest = listMobaExpMest.FirstOrDefault(o => o.ID == sale.MOBA_EXP_MEST_ID) ?? new V_HIS_EXP_MEST();
                        Mrs00274RDO rdo = new Mrs00274RDO(mobaExpMest);
                        rdo.THUOC_VATTU = "VATTU";
                        rdo.MAME_TYPE_NAME = Material.MATERIAL_TYPE_NAME;
                        rdo.MAME_TYPE_CODE = Material.MATERIAL_TYPE_CODE;
                        if (!string.IsNullOrWhiteSpace(filter.BIOLOGY_PRODUCTs))
                        {
                            string[] biologyProducts = filter.BIOLOGY_PRODUCTs.Split(',');
                            if (biologyProducts.Contains(Material.MATERIAL_TYPE_CODE))
                            {
                                rdo.IS_BIOLOGY_PRODUCT = 1;
                            }
                        }
                        rdo.NATIONAL_NAME = Material.NATIONAL_NAME;
                        rdo.SERVICE_UNIT_NAME = Material.SERVICE_UNIT_NAME;
                        rdo.PRICE = Material.PRICE.HasValue ? Material.PRICE.Value * (1 + (Material.VAT_RATIO ?? 0)) : Material.IMP_PRICE * (1 + Material.IMP_VAT_RATIO);
                        rdo.PRICE_1 = Material.TH_EXP_MEST_MATERIAL_ID != null && dicThExpMestMaterialPrice.ContainsKey(Material.TH_EXP_MEST_MATERIAL_ID ?? 0)
                            ? dicThExpMestMaterialPrice[Material.TH_EXP_MEST_MATERIAL_ID ?? 0]
                            : (Material.PRICE ?? 0) * (1 + (Material.VAT_RATIO ?? 0));
                        rdo.VAT_RATIO_1 = Material.TH_EXP_MEST_MATERIAL_ID != null && dicThExpMestMaterialVat.ContainsKey(Material.TH_EXP_MEST_MATERIAL_ID ?? 0)
                            ? dicThExpMestMaterialVat[Material.TH_EXP_MEST_MATERIAL_ID ?? 0]
                            : 0;
                        var serviceReq = listServiceReq.FirstOrDefault(o => o.ID == mobaExpMest.PRESCRIPTION_ID);
                        if (serviceReq != null)
                        {
                            rdo.REQUEST_LOGINNAME = serviceReq.REQUEST_LOGINNAME;
                            rdo.REQUEST_USERNAME = serviceReq.REQUEST_USERNAME;
                            rdo.ICD_CODE = serviceReq.ICD_CODE;
                            rdo.ICD_NAME = serviceReq.ICD_NAME;
                            var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == serviceReq.REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT();
                            rdo.REQUEST_DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                            rdo.REQUEST_DEPARTMENT_NAME = department.DEPARTMENT_NAME;

                        }
                        else
                        {
                            rdo.REQUEST_LOGINNAME = mobaExpMest.TDL_PRESCRIPTION_REQ_LOGINNAME;
                            rdo.REQUEST_USERNAME = mobaExpMest.TDL_PRESCRIPTION_REQ_USERNAME;
                        }
                        rdo.AMOUNT = Material.AMOUNT;
                        rdo.TOTAL_PRICE = rdo.PRICE * rdo.AMOUNT;
                        rdo.TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)Material.IMP_TIME);
                        //rdo.REQ_LOGINNAME = sale.REQ_LOGINNAME;
                        //rdo.REQ_USERNAME = sale.REQ_USERNAME;
                        rdo.TDL_PATIENT_NAME = sale.TDL_PATIENT_NAME;

                        rdo.EXP_PRICE = exp.PRICE * (1 + exp.VAT_RATIO);
                        rdo.EXP_TOTAL_PRICE = rdo.EXP_PRICE * rdo.AMOUNT;
                        rdo.EXP_MEST_CODE = mobaExpMest.EXP_MEST_CODE;
                        rdo.TREATMENT_CODE = sale.TDL_TREATMENT_CODE;
                        rdo.IMP_MEST_CODE = sale.IMP_MEST_CODE;
                        rdo.EXPIRED_DATE = Material.EXPIRED_DATE;

                        rdo.PACKAGE_NUMBER = Material.PACKAGE_NUMBER;
                        //if (serviceReq != null)
                        //{
                        //    rdo.REQUEST_LOGINNAME = serviceReq.REQUEST_LOGINNAME;
                        //    rdo.REQUEST_USERNAME = serviceReq.REQUEST_USERNAME;
                        //    rdo.ICD_CODE = serviceReq.ICD_CODE;
                        //    rdo.ICD_NAME = serviceReq.ICD_NAME;
                        //}
                        //else
                        //{
                        //    rdo.REQUEST_LOGINNAME = sale.TDL_PRESCRIPTION_REQ_LOGINNAME;
                        //    rdo.REQUEST_USERNAME = sale.TDL_PRESCRIPTION_REQ_USERNAME;
                        //}
                        //if (sale.BILL_ID.HasValue)
                        //{
                        //    var tran = ListTransaction.FirstOrDefault(o => o.ID == sale.BILL_ID);
                        //    if (tran != null)
                        //    {
                        //        rdo.TRANSACTION_CODE = tran.TRANSACTION_CODE;
                        //        rdo.TRANSACTION_NUM_ORDER = tran.NUM_ORDER;
                        //        rdo.IS_PRINTED = this.IsPrinted(tran) ? (short)1 : (short)0;
                        //    }
                        //}

                        listImpRdo.Add(rdo);
                    }
                }
                listRdo = listRdo.OrderBy(o => o.TIME).ToList();
                var grExp = listRdo.OrderBy(o => o.EXP_MEST_CODE).GroupBy(o => o.EXP_MEST_CODE).ToList();
                for (int i = 0; i < grExp.Count; i++)
                {
                    var exp = new Mrs00274RDO(grExp[i].First());
                    exp.TOTAL_PRICE = grExp[i].Sum(s => s.TOTAL_PRICE);
                    exp.EXP_TOTAL_PRICE = grExp[i].Sum(s => s.EXP_TOTAL_PRICE);
                    exp.AMOUNT = grExp[i].Sum(s => s.AMOUNT);
                    exp.TH_AMOUNT = grExp[i].Sum(s => s.TH_AMOUNT);
                    exp.TH_PRICE = grExp[i].Sum(s => s.TH_PRICE);
                    exp.REAL_PRICE = grExp[i].Sum(s => s.REAL_PRICE);

                    ListExpRdo.Add(exp);
                }

                var expMestSale = ManagerSql.GetLisExpMest((Mrs00274Filter)reportFilter);
                if (IsNotNullOrEmpty(expMestSale))
                {
                    for (int i = 0; i < expMestSale.Count; i++)
                    {
                        var exp = new Mrs00274RDO(expMestSale[i]);
                        exp.TOTAL_PRICE = expMestSale[i].TDL_TOTAL_PRICE ?? 0;
                        if (i % 2 == 1)
                        {
                            ListExpRdo2.Add(exp);
                        }
                        else
                        {
                            ListExpRdo1.Add(exp);
                        }
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

        private bool IsPrinted(HIS_TRANSACTION transaction)
        {
            return this.dicPrintLogUnique.ContainsKey(transaction.TRANSACTION_CODE ?? "");

        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            var acsUser = new AcsUserManager(paramGet).Get<List<ACS_USER>>(new AcsUserFilterQuery());
            if (((Mrs00274Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00274Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00274Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00274Filter)reportFilter).TIME_TO));
            }
            if (((Mrs00274Filter)reportFilter).LOGINNAME_SALE != null)
            {
                try
                {
                    dicSingleTag.Add("SALE_USERNAME", acsUser.Where(o => o.LOGINNAME == ((Mrs00274Filter)reportFilter).LOGINNAME_SALE).ToList().First().USERNAME);
                }
                catch (Exception)
                {
                    Inventec.Common.Logging.LogSystem.Debug("SALE_USERNAME is null");

                }

            }
            if (IsNotNullOrEmpty(((Mrs00274Filter)reportFilter).MEDI_STOCK_BUSINESS_IDs))
            {
                dicSingleTag.Add("MEDI_STOCK_NAME", String.Join(", ", new HisMediStockManager().Get(new HisMediStockFilterQuery()).Where(o => ((Mrs00274Filter)reportFilter).MEDI_STOCK_BUSINESS_IDs.Contains(o.ID)).Select(p => p.MEDI_STOCK_NAME).ToArray()));
            }

            objectTag.AddObjectData(store, "MaMe", listRdo.OrderBy(o => o.IS_PRINTED).ToList());

            objectTag.AddObjectData(store, "MaMeIm", listImpRdo);
            objectTag.AddObjectData(store, "Medicine", listRdo.Where(p => p.REQUEST_LOGINNAME != null && p.THUOC_VATTU == "THUOC").OrderBy(p => p.TIME).ThenBy(p => p.REQUEST_LOGINNAME).ToList());
            objectTag.AddObjectData(store, "ExpMest", ListExpRdo.OrderBy(o => o.IS_PRINTED).ToList());
            objectTag.AddRelationship(store, "ExpMest", "MaMe", "EXP_MEST_CODE", "EXP_MEST_CODE");
            objectTag.AddObjectData(store, "Sale", ListExpRdo.GroupBy(o => o.CREATOR).Select(p => new SALE() { CREATOR = p.Key, USERNAME = (acsUser.FirstOrDefault(o => o.LOGINNAME == p.Key) ?? new ACS_USER()).USERNAME }).OrderBy(r => r.USERNAME).ToList());
            objectTag.AddRelationship(store, "Sale", "ExpMest", "CREATOR", "CREATOR");

            objectTag.AddObjectData(store, "ExpMest1", ListExpRdo1);
            objectTag.AddObjectData(store, "ExpMest2", ListExpRdo2);

            decimal totalPrice = ListExpRdo1.Sum(s => s.TOTAL_PRICE) + ListExpRdo2.Sum(s => s.TOTAL_PRICE);
            dicSingleTag.Add("TOTAL_PRICE", totalPrice);

            string sumText = String.Format("{0:0.####}", Inventec.Common.Number.Convert.NumberToNumberRoundMax4(totalPrice));
            dicSingleTag.Add("TOTAL_PRICE_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(sumText));
            dicSingleTag.Add("TOTAL_PRICE_TEXT_ROUND", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(totalPrice).ToString()));


            objectTag.AddObjectData(store, "Doctor", listRdo.GroupBy(o => o.REQUEST_LOGINNAME).Select(p => p.FirstOrDefault()).OrderBy(p => p.MAME_TYPE_NAME).ToList());
            objectTag.AddObjectData(store, "DoctorMty", listRdo.GroupBy(o => o.REQUEST_LOGINNAME).Select(p => new Mrs00274RDO() { REQUEST_LOGINNAME = p.First().REQUEST_LOGINNAME, REQUEST_USERNAME = p.First().REQUEST_USERNAME, DIC_MTY_AMOUNT = p.GroupBy(o => o.MAME_TYPE_CODE ?? "NONE").ToDictionary(l => l.Key, m => m.Sum(s => s.AMOUNT)) }).OrderBy(p => string.IsNullOrWhiteSpace(p.REQUEST_LOGINNAME)).ToList());
            objectTag.AddObjectData(store, "Service", listRdo.GroupBy(o => o.MAME_TYPE_CODE).Select(p => p.FirstOrDefault()).OrderBy(p => p.MAME_TYPE_NAME).ToList());
            //objectTag.AddRelationship(store, "Doctor", "DoctorService", new string[] { "REQ_LOGINNAME", "MAME_TYPE_CODE" }, new string[] { "REQ_LOGINNAME", "MAME_TYPE_CODE" });

        }

    }
}
