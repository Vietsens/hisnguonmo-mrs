using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceType;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisSeseDepoRepay;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisSereServDeposit;
using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisBranch;
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
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServBill;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00295
{
    class Mrs00295Processor : AbstractProcessor
    {
        Mrs00295Filter castFilter = null;

        public Mrs00295Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        Dictionary<long, Mrs00295RDO> dicRdo = new Dictionary<long, Mrs00295RDO>();
        List<Mrs00295RDO> listRdo = new List<Mrs00295RDO>();
        List<Mrs00295RDO> listDeposit = new List<Mrs00295RDO>();
        List<Mrs00295RDO> listRepay = new List<Mrs00295RDO>();
        List<Mrs00295RDO> listBill = new List<Mrs00295RDO>();

        List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();

        string Branch_Name = "";

        public override Type FilterType()
        {
            return typeof(Mrs00295Filter);
        }

        protected override bool GetData()///
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00295Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu bao cao MRS00295, V_HIS_DEPOSIT_2,V_HIS_REPAY_1 ,V_HIS_CASHIER_ROOM: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                if (castFilter.SERVICE_TYPE_ID == null)
                {
                    throw new Exception("Nguoi dung truyen len ServiceTypeId khong chinh xac: " + castFilter.SERVICE_TYPE_ID);
                }
                //BÁO CÁO NÀY CHỈ LẤY DỊCH VỤ CỦA BỆNH NHÂN NGOẠI TRÚ THÔI.
                listDeposit = new Mrs00295RDOManager().GetDeposit(this.castFilter);
                listRepay = new Mrs00295RDOManager().GetRepay(this.castFilter);
                listBill = new Mrs00295RDOManager().GetBill(this.castFilter);
              
               
                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_DEPOSIT_1,V_HIS_CASHIER_ROOM, MRS00293");
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
                ProcessBranchById();
                ProcessDetailData(listDeposit, true);
                ProcessDetailData(listRepay, false);
                ProcessDetailData(listBill, true);
                listRdo = dicRdo.Values.ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessDetailData(List<Mrs00295RDO> list,bool IsDepBil)
        {
            try
            {
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {

                        Mrs00295RDO rdo = new Mrs00295RDO();
                        rdo.SERVICE_CODE = item.SERVICE_CODE;
                        rdo.SERVICE_ID = item.SERVICE_ID;
                        rdo.SERVICE_NAME = item.SERVICE_NAME;
                        rdo.TDL_SERVICE_TYPE_ID = item.TDL_SERVICE_TYPE_ID;
                        if (IsDepBil)
                        {
                            rdo.PRICE = item.PRICE;
                            rdo.AMOUNT_DEPOSIT = item.AMOUNT_DEPOSIT;
                            rdo.TOTAL_DEPOSIT_PRICE = item.TOTAL_DEPOSIT_PRICE;
                        }
                        else
                        {
                            rdo.AMOUNT_REPAY = item.AMOUNT_REPAY;
                            rdo.TOTAL_REPAY_PRICE = item.TOTAL_REPAY_PRICE;
                        }
                         
                        if (!dicRdo.ContainsKey(rdo.SERVICE_ID))
                        {
                            dicRdo[rdo.SERVICE_ID] = rdo;
                        }
                        else
                        {
                            dicRdo[rdo.SERVICE_ID].AMOUNT_DEPOSIT += rdo.AMOUNT_DEPOSIT;
                            dicRdo[rdo.SERVICE_ID].TOTAL_DEPOSIT_PRICE += rdo.TOTAL_DEPOSIT_PRICE;
                            dicRdo[rdo.SERVICE_ID].AMOUNT_REPAY += rdo.AMOUNT_REPAY;
                            dicRdo[rdo.SERVICE_ID].TOTAL_REPAY_PRICE += rdo.TOTAL_REPAY_PRICE;
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
               Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        private void ProcessBranchById()
        {
            try
            {
                var branch = MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == castFilter.BRANCH_ID);
                if (branch != null)
                {
                    this.Branch_Name = branch.BRANCH_NAME;
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
                listRdo = listRdo.OrderBy(o => o.SERVICE_CODE).ToList();
                dicSingleTag.Add("CREATE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM??0));
                dicSingleTag.Add("CREATE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));
                var ParentService = new HisServiceManager().GetById(castFilter.SERVICE_ID ?? 0);
                var ServiceType = HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == castFilter.SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE();
                if (ParentService != null)
                {
                    dicSingleTag.Add("SERVICE_CODE", ParentService.SERVICE_CODE);
                    dicSingleTag.Add("SERVICE_NAME", ParentService.SERVICE_NAME);
                    dicSingleTag.Add("SERVICE_TYPE_NAME", ServiceType.SERVICE_TYPE_NAME);
                }
                else
                {
                    dicSingleTag.Add("SERVICE_TYPE_NAME", ServiceType.SERVICE_TYPE_NAME);
                }

                dicSingleTag.Add("BRANCH_NAME", this.Branch_Name);
                objectTag.AddObjectData(store, "Report", listRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
