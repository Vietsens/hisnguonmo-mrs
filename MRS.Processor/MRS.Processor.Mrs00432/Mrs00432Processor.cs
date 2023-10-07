using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisBlood;
using MOS.MANAGER.HisImpMestType;
using MOS.MANAGER.HisMestPeriodBlood;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisImpMestBlood;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisBloodType;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestBlood;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisSupplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00432
{
    class Mrs00432Processor : AbstractProcessor
    {
        Mrs00432Filter castFilter = null;
        List<Mrs00432RDO> listRdo = new List<Mrs00432RDO>();

        V_HIS_BLOOD_TYPE BLOOD_TYPE = new V_HIS_BLOOD_TYPE();
        public decimal BEGIN_AMOUNT = 0;
        public decimal END_AMOUNT = 0;

        List<V_HIS_IMP_MEST> listImpMests = new List<V_HIS_IMP_MEST>();
        List<V_HIS_IMP_MEST_BLOOD> listImpMestBloods = new List<V_HIS_IMP_MEST_BLOOD>();
        List<V_HIS_IMP_MEST> listChmsImpMests = new List<V_HIS_IMP_MEST>();
        List<V_HIS_IMP_MEST> listManuImpMests = new List<V_HIS_IMP_MEST>();
        List<V_HIS_IMP_MEST> listMobaImpMests = new List<V_HIS_IMP_MEST>();

        List<V_HIS_EXP_MEST> listExpMests = new List<V_HIS_EXP_MEST>();
        List<V_HIS_EXP_MEST_BLOOD> listExpMestBloods = new List<V_HIS_EXP_MEST_BLOOD>();
        List<V_HIS_EXP_MEST> listChmsExpMests = new List<V_HIS_EXP_MEST>();
        List<V_HIS_EXP_MEST> listSaleExpMests = new List<V_HIS_EXP_MEST>();
        List<V_HIS_EXP_MEST> listPrescriptions = new List<V_HIS_EXP_MEST>();
        List<V_HIS_EXP_MEST> listDepaExpMests = new List<V_HIS_EXP_MEST>();

        List<long> CHMS_EXP_MEST_TYPE_IDs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS
        };

        List<long> PRES_EXP_MEST_TYPE_IDs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DM
        };

        List<long> CHMS_IMP_MEST_TYPE_IDs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS
        };

        List<long> MOBA_IMP_MEST_TYPE_IDs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL
        };

        public string MEDI_STOCK_NAME = "";
        public string HOSPITAL_NAME = "";
        public string DEPARTMENT_OF_HEATH = "";

        public Mrs00432Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00432Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00432Filter)this.reportFilter;

                #region bla...bla...
                BLOOD_TYPE = null;

                HisMediStockViewFilterQuery mediStockFilter = new HisMediStockViewFilterQuery();
                mediStockFilter.ID = castFilter.MEDI_STOCK_ID;
                var listMediStocks = new HisMediStockManager(param).GetView(mediStockFilter);

                if (IsNotNullOrEmpty(listMediStocks))
                {
                    MEDI_STOCK_NAME = listMediStocks.First().MEDI_STOCK_NAME;

                    try
                    {
                        HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery();
                        departmentFilter.IDs = listMediStocks.Select(s => s.DEPARTMENT_ID).ToList();
                        var listDepartments = new MOS.MANAGER.HisDepartment.HisDepartmentManager(param).Get(departmentFilter);

                        if (IsNotNullOrEmpty(listDepartments))
                        {
                            HisBranchFilterQuery branchFilter = new HisBranchFilterQuery();
                            branchFilter.IDs = listDepartments.Select(s => s.BRANCH_ID).ToList();
                            var listBranchs = new MOS.MANAGER.HisBranch.HisBranchManager(param).Get(branchFilter);

                            if (IsNotNullOrEmpty(listBranchs))
                            {
                                HOSPITAL_NAME = listBranchs.First().BRANCH_NAME;
                                DEPARTMENT_OF_HEATH = listBranchs.First().PARENT_ORGANIZATION_NAME;
                            }
                        }
                    }
                    catch { }
                }

                HisBloodTypeViewFilterQuery bloodTypeViewfilter = new HisBloodTypeViewFilterQuery();
                bloodTypeViewfilter.ID = castFilter.BLOOD_TYPE_ID;
                var listBloodTypes = new MOS.MANAGER.HisBloodType.HisBloodTypeManager(param).GetView(bloodTypeViewfilter);

                if (IsNotNullOrEmpty(listBloodTypes))
                    BLOOD_TYPE = listBloodTypes.First();
                #endregion

                HisImpMestViewFilterQuery impMestFilter = new HisImpMestViewFilterQuery();
                impMestFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMestFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                impMestFilter.IMP_TIME_TO = castFilter.TIME_TO;
                impMestFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                listImpMests = new MOS.MANAGER.HisImpMest.HisImpMestManager(param).GetView(impMestFilter);

                var skip = 0;
                while (listImpMests.Count - skip > 0)
                {
                    var listIds = listImpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisImpMestBloodViewFilterQuery impMestBloodViewFilter = new HisImpMestBloodViewFilterQuery();
                    impMestBloodViewFilter.IMP_MEST_IDs = listIds.Select(s => s.ID).ToList();
                    listImpMestBloods.AddRange(new MOS.MANAGER.HisImpMestBlood.HisImpMestBloodManager(param).GetView(impMestBloodViewFilter));
                }

                listImpMestBloods = listImpMestBloods.Where(w => w.BLOOD_TYPE_ID == castFilter.BLOOD_TYPE_ID).ToList();

                if (IsNotNullOrEmpty(listImpMestBloods))
                {
                    listChmsImpMests = listImpMests.Where(o => this.CHMS_IMP_MEST_TYPE_IDs.Contains(o.IMP_MEST_TYPE_ID) && listImpMestBloods.Select(s => s.IMP_MEST_ID).ToList().Contains(o.ID)).ToList();

                    listMobaImpMests = listImpMests.Where(o => this.MOBA_IMP_MEST_TYPE_IDs.Contains(o.IMP_MEST_TYPE_ID) && listImpMestBloods.Select(s => s.IMP_MEST_ID).ToList().Contains(o.ID)).ToList();

                    listManuImpMests = listImpMests.Where(o => (o.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC) && listImpMestBloods.Select(s => s.IMP_MEST_ID).ToList().Contains(o.ID)).ToList();
                }

                HisExpMestBloodViewFilterQuery expMestBloodViewFilter = new HisExpMestBloodViewFilterQuery();
                expMestBloodViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMestBloodViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                expMestBloodViewFilter.EXP_TIME_TO = castFilter.TIME_TO;
                expMestBloodViewFilter.IS_EXPORT = true;
                listExpMestBloods.AddRange(new HisExpMestBloodManager(param).GetView(expMestBloodViewFilter));
                var listExpMestId = listExpMestBloods.Select(o => o.EXP_MEST_ID).Distinct().ToList();
                skip = 0;
                while (listExpMestId.Count - skip > 0)
                {
                    var listIds = listExpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisExpMestViewFilterQuery ExpMestViewFilter = new HisExpMestViewFilterQuery();
                    ExpMestViewFilter.IDs = listIds;
                    listExpMests.AddRange(new HisExpMestManager(param).GetView(ExpMestViewFilter));
                }
                listExpMestBloods = listExpMestBloods.Where(s => s.BLOOD_TYPE_ID == castFilter.BLOOD_TYPE_ID).ToList();

                if (IsNotNullOrEmpty(listExpMestBloods))
                {
                    listChmsExpMests = listExpMests.Where(o => this.CHMS_EXP_MEST_TYPE_IDs.Contains(o.EXP_MEST_TYPE_ID) && listExpMestBloods.Select(s => s.EXP_MEST_ID).ToList().Contains(o.ID)).ToList();

                    listSaleExpMests = listExpMests.Where(o => (o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN) && listExpMestBloods.Select(s => s.EXP_MEST_ID).ToList().Contains(o.ID)).ToList();

                    listPrescriptions = listExpMests.Where(o => this.PRES_EXP_MEST_TYPE_IDs.Contains(o.EXP_MEST_TYPE_ID) && listExpMestBloods.Select(s => s.EXP_MEST_ID).ToList().Contains(o.ID)).ToList();

                    listDepaExpMests = listExpMests.Where(o => (o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP) && listExpMestBloods.Select(s => s.EXP_MEST_ID).ToList().Contains(o.ID)).ToList();
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
                CommonParam paramGet = new CommonParam();
                decimal beginAmount = ProcessBeginAmount();
                BEGIN_AMOUNT = beginAmount;

                if (IsNotNullOrEmpty(listImpMestBloods))
                {
                    Dictionary<long, HIS_SUPPLIER> dicSupplier = new HisSupplierManager().Get(new HisSupplierFilterQuery()).ToDictionary(o => o.ID);

                    foreach (var blood in listImpMestBloods)
                    {
                        var rdo = new Mrs00432RDO();
                        rdo.BLOOD_TYPE = this.BLOOD_TYPE;
                        rdo.IMP_EXP_TIME = blood.IMP_TIME;
                        rdo.IMP_MEST_CODE = blood.IMP_MEST_CODE;
                        rdo.BLOOD_CODE = blood.BLOOD_CODE;
                        rdo.BLOOD_ABO_CODE = blood.BLOOD_ABO_CODE;

                        if (blood.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC)
                        {
                            var manu = listManuImpMests.Where(w => w.ID == blood.IMP_MEST_ID).ToList();
                            if (IsNotNullOrEmpty(manu))
                                rdo.NOTE = dicSupplier.ContainsKey(manu.First().SUPPLIER_ID.Value) ? "Nhập từ " + dicSupplier[manu.First().SUPPLIER_ID.Value].SUPPLIER_NAME : "";
                        }
                        else if (this.CHMS_IMP_MEST_TYPE_IDs.Contains(blood.IMP_MEST_TYPE_ID))
                        {
                            var chms = listChmsImpMests.Where(w => w.ID == blood.IMP_MEST_ID).ToList();
                            //if (IsNotNullOrEmpty(chms))
                            //rdo.NOTE = "Kho xuất " + chms.First().EXP_MEDI_STOCK_NAME; 
                        }
                        else if (blood.AGGR_IMP_MEST_ID != null)
                            rdo.NOTE = "Nhập tổng hợp";
                        //else if (blood.IMP_MEST_TYPE_ID == HisImpMestTypeCFG.IMP_MEST_TYPE_ID__INIT)
                        //    rdo.NOTE = "Nhập đầu kỳ"; 
                        //else if (blood.IMP_MEST_TYPE_ID == HisImpMestTypeCFG.IMP_MEST_TYPE_ID__INVE)
                        //    rdo.NOTE = "Nhập kiểm kê"; 
                        else if (blood.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__THT)
                        {
                            rdo.NOTE = "Nhập thu hồi";
                        }
                        else if (blood.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__KHAC)
                            rdo.NOTE = "Nhập khác";

                        rdo.IMP_AMOUNT = 1;

                        listRdo.Add(rdo);
                    }
                }

                if (IsNotNullOrEmpty(listExpMestBloods))
                {
                    Dictionary<long, HIS_MEDI_STOCK> dicMediStock = new HisMediStockManager().Get(new HisMediStockFilterQuery()).ToDictionary(o => o.ID);
                    foreach (var blood in listExpMestBloods)
                    {
                        var rdo = new Mrs00432RDO();
                        rdo.BLOOD_TYPE = this.BLOOD_TYPE;
                        rdo.IMP_EXP_TIME = blood.EXP_TIME;
                        rdo.EXP_MEST_CODE = blood.EXP_MEST_CODE;
                        rdo.BLOOD_CODE = blood.BLOOD_CODE;
                        rdo.BLOOD_ABO_CODE = blood.BLOOD_ABO_CODE;

                        if (this.CHMS_EXP_MEST_TYPE_IDs.Contains(blood.EXP_MEST_TYPE_ID))
                        {
                            var chms = listChmsExpMests.Where(w => w.ID == blood.EXP_MEST_ID).ToList();
                            if (IsNotNullOrEmpty(chms))
                                rdo.NOTE = dicMediStock.ContainsKey(chms.First().IMP_MEDI_STOCK_ID.Value) ? "Kho nhập " + dicMediStock[chms.First().IMP_MEDI_STOCK_ID.Value].MEDI_STOCK_NAME : "";
                        }
                        else if (blood.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN)
                        {
                            var sale = listSaleExpMests.Where(w => w.ID == blood.EXP_MEST_ID).ToList();
                            if (IsNotNullOrEmpty(sale))
                                rdo.NOTE = sale.First().TDL_PATIENT_NAME;
                        }
                        else if (this.PRES_EXP_MEST_TYPE_IDs.Contains(blood.EXP_MEST_TYPE_ID))
                        {
                            var pres = listPrescriptions.Where(w => w.ID == blood.EXP_MEST_ID).ToList();
                            if (IsNotNullOrEmpty(pres))
                                rdo.NOTE = pres.First().TDL_TREATMENT_CODE + " : " + pres.First().TDL_PATIENT_NAME;
                        }
                        else if (blood.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP)
                        {
                            var depa = listDepaExpMests.Where(w => w.ID == blood.EXP_MEST_ID).ToList();
                            if (IsNotNullOrEmpty(depa))
                                rdo.NOTE = depa.First().REQ_DEPARTMENT_NAME;
                        }
                        else if (blood.AGGR_EXP_MEST_ID != null)
                            rdo.NOTE = "Xuất tổng hợp";

                        else if (blood.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__TNCC)
                            rdo.NOTE = "Xuất trả nhà cung cấp";
                        else if (blood.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__KHAC)
                            rdo.NOTE = "Xuất khác";

                        rdo.EXP_AMOUNT = 1;

                        listRdo.Add(rdo);
                    }
                }

                listRdo = listRdo.OrderBy(o => o.IMP_EXP_TIME).ToList();

                foreach (var rdo in listRdo)
                {
                    rdo.BEGIN_AMOUNT = beginAmount;
                    if (IsNotNull(rdo.IMP_MEST_CODE) && rdo.IMP_MEST_CODE.Length > 0)
                    {
                        rdo.IMP_EXP_MEST_CODE = rdo.IMP_MEST_CODE;
                        rdo.END_AMOUNT = beginAmount + rdo.IMP_AMOUNT;
                        beginAmount = beginAmount + rdo.IMP_AMOUNT;
                    }
                    else if (IsNotNull(rdo.EXP_MEST_CODE) && rdo.EXP_MEST_CODE.Length > 0)
                    {
                        rdo.IMP_EXP_MEST_CODE = rdo.EXP_MEST_CODE;
                        rdo.END_AMOUNT = beginAmount - rdo.EXP_AMOUNT;
                        beginAmount = beginAmount - rdo.EXP_AMOUNT;
                    }
                }

                END_AMOUNT = beginAmount;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        public decimal ProcessBeginAmount()
        {
            decimal beginAmount = 0;
            #region thiếu API
            HisMediStockPeriodFilterQuery mediStockPeriodFilter = new HisMediStockPeriodFilterQuery();
            mediStockPeriodFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
            mediStockPeriodFilter.CREATE_TIME_TO = castFilter.TIME_FROM - 1;
            var listMediStockPeriods = new MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodManager(param).Get(mediStockPeriodFilter);

            if (IsNotNullOrEmpty(listMediStockPeriods))
            {
                var period = listMediStockPeriods.OrderByDescending(o => o.CREATE_TIME).First();

                HisMestPeriodBloodFilterQuery mestPeriodBloodFilter = new HisMestPeriodBloodFilterQuery();
                mestPeriodBloodFilter.MEDI_STOCK_PERIOD_ID = period.ID;
                var listMestPeriods = new MOS.MANAGER.HisMestPeriodBlood.HisMestPeriodBloodManager(param).Get(mestPeriodBloodFilter);

                var skip = 0;
                var listBloods = new List<HIS_BLOOD>();
                while (listMestPeriods.Count - skip > 0)
                {
                    var listIds = listMestPeriods.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisBloodFilterQuery bloodFilter = new HisBloodFilterQuery();
                    bloodFilter.IDs = listIds.Select(s => s.BLOOD_ID).ToList();
                    bloodFilter.BLOOD_TYPE_ID = castFilter.BLOOD_TYPE_ID;
                    listBloods.AddRange(new MOS.MANAGER.HisBlood.HisBloodManager(param).Get(bloodFilter));
                }

                beginAmount = listMestPeriods.Where(w => listBloods.Select(s => s.ID).Contains(w.BLOOD_ID)).Count();

                HisImpMestBloodViewFilterQuery impMestBloodFilter = new HisImpMestBloodViewFilterQuery();
                impMestBloodFilter.IMP_TIME_FROM = period.CREATE_TIME;
                impMestBloodFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                impMestBloodFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                var listImpMestBloodOutTime = new MOS.MANAGER.HisImpMestBlood.HisImpMestBloodManager(param).GetView(impMestBloodFilter);

                if (IsNotNullOrEmpty(listImpMestBloodOutTime))
                    beginAmount = beginAmount + listImpMestBloodOutTime.Where(s => s.BLOOD_TYPE_ID == castFilter.BLOOD_TYPE_ID).Count();

                HisExpMestBloodViewFilterQuery expMestBloodViewFilter = new HisExpMestBloodViewFilterQuery();
                expMestBloodViewFilter.EXP_TIME_FROM = period.CREATE_TIME;
                expMestBloodViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMestBloodViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                var listExpMestBloodOutTime = new MOS.MANAGER.HisExpMestBlood.HisExpMestBloodManager(param).GetView(expMestBloodViewFilter);

                if (IsNotNullOrEmpty(listExpMestBloodOutTime))
                    beginAmount = beginAmount - listExpMestBloodOutTime.Where(s => s.BLOOD_TYPE_ID == castFilter.BLOOD_TYPE_ID).Count();
            }
            else
            #endregion
            {
                HisImpMestViewFilterQuery impMestFilter = new HisImpMestViewFilterQuery();
                impMestFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMestFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                impMestFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                listImpMests = new MOS.MANAGER.HisImpMest.HisImpMestManager(param).GetView(impMestFilter);
                var skip = 0;
                var listImpMestBloodOutTime = new List<V_HIS_IMP_MEST_BLOOD>();
                while (listImpMests.Count - skip > 0)
                {
                    var listIds = listImpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisImpMestBloodViewFilterQuery impMestBloodViewFilter = new HisImpMestBloodViewFilterQuery();
                    impMestBloodViewFilter.IMP_MEST_IDs = listIds.Select(s => s.ID).ToList();
                    listImpMestBloodOutTime.AddRange(new MOS.MANAGER.HisImpMestBlood.HisImpMestBloodManager(param).GetView(impMestBloodViewFilter));
                }

                listImpMestBloodOutTime = listImpMestBloodOutTime.Where(w => w.BLOOD_TYPE_ID == castFilter.BLOOD_TYPE_ID).ToList();

                if (IsNotNullOrEmpty(listImpMestBloodOutTime))
                    beginAmount = beginAmount + listImpMestBloodOutTime.Count();
                var listExpMestBloodOutTime = new List<V_HIS_EXP_MEST_BLOOD>();
                HisExpMestBloodViewFilterQuery expMestBloodViewFilter = new HisExpMestBloodViewFilterQuery();
                expMestBloodViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMestBloodViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMestBloodViewFilter.IS_EXPORT = true;
                listExpMestBloodOutTime.AddRange(new HisExpMestBloodManager(param).GetView(expMestBloodViewFilter));

                listExpMestBloodOutTime = listExpMestBloodOutTime.Where(w => w.BLOOD_TYPE_ID == castFilter.BLOOD_TYPE_ID).ToList();

                if (IsNotNullOrEmpty(listExpMestBloodOutTime))
                    beginAmount = beginAmount - listExpMestBloodOutTime.Count();
            }

            return beginAmount;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", castFilter.TIME_FROM);
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", castFilter.TIME_TO);
                }

                dicSingleTag.Add("MEDI_STOCK_NAME", MEDI_STOCK_NAME);
                dicSingleTag.Add("BEGIN_AMOUNT", BEGIN_AMOUNT);
                dicSingleTag.Add("END_AMOUNT", END_AMOUNT);
                dicSingleTag.Add("BLOOD_TYPE_NAME", BLOOD_TYPE.BLOOD_TYPE_NAME);
                dicSingleTag.Add("SERVICE_UNIT_NAME", BLOOD_TYPE.SERVICE_UNIT_NAME);
                dicSingleTag.Add("HOSPITAL_NAME", HOSPITAL_NAME);
                dicSingleTag.Add("DEPARTMENT_OF_HEATH", DEPARTMENT_OF_HEATH);

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Rdo", listRdo);

                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
