using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
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

namespace MRS.Processor.Mrs00107
{
    public class Mrs00107Processor : AbstractProcessor
    {
        Mrs00107Filter castFilter = null;
        List<Mrs00107RDO> ListRdo = new List<Mrs00107RDO>();
        List<Department> ListDepartment = new List<Department>();
        List<V_HIS_SERVICE_REQ> ListServiceReq;

        decimal Total_Amount = 0;
        string Execute_Room_Name;

        public Mrs00107Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00107Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                this.castFilter = (Mrs00107Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu tu V_HIS_SERVICE_REQ, MRS00107 Filter." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                HisServiceReqViewFilterQuery sReqFilter = new HisServiceReqViewFilterQuery();
                sReqFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                sReqFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                sReqFilter.EXECUTE_ROOM_ID = castFilter.EXECUTE_ROOM_ID;
                sReqFilter.SERVICE_REQ_STT_IDs = new List<long>();
                sReqFilter.SERVICE_REQ_STT_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                //IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT); 
                ListServiceReq = new MOS.MANAGER.HisServiceReq.HisServiceReqManager(paramGet).GetView(sReqFilter);
                if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_SERVICE_REQ, MRS00107.");
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
                ProcessListServiceReq(ListServiceReq);
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListServiceReq(List<V_HIS_SERVICE_REQ> ListServiceReq)
        {
            try
            {
                if (IsNotNullOrEmpty(ListServiceReq))
                {
                    CommonParam paramGet = new CommonParam();
                    Execute_Room_Name = ListServiceReq.First().EXECUTE_ROOM_NAME;
                    var Groups = ListServiceReq.GroupBy(g => g.REQUEST_DEPARTMENT_ID).ToList();
                    foreach (var group in Groups)
                    {
                        var listSub = group.ToList<V_HIS_SERVICE_REQ>();
                        Department depart = new Department();
                        depart.ID = listSub.First().REQUEST_DEPARTMENT_ID;
                        depart.DEPARTMENT_CODE = listSub.First().REQUEST_DEPARTMENT_CODE;
                        depart.DEPARTMENT_NAME = listSub.First().REQUEST_DEPARTMENT_NAME;
                        ProcessDetailListServiceRed(paramGet, listSub);
                        depart.TOTAL_AMOUNT = Total_Amount;
                        ListDepartment.Add(depart);
                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00107.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void ProcessDetailListServiceRed(CommonParam paramGet, List<V_HIS_SERVICE_REQ> ListSub)
        {
            try
            {
                if (IsNotNullOrEmpty(ListSub))
                {
                    Total_Amount = 0;
                    List<Mrs00107RDO> listRdo = new List<Mrs00107RDO>();
                    int start = 0;
                    int count = ListSub.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        List<V_HIS_SERVICE_REQ> hisServiceReqs = ListSub.Skip(start).Take(limit).ToList();
                        HisSereServViewFilterQuery ss2Filter = new HisSereServViewFilterQuery();
                        ss2Filter.SERVICE_REQ_IDs = hisServiceReqs.Select(s => s.ID).ToList();
                        List<V_HIS_SERE_SERV> ListSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(ss2Filter);
                        if (!paramGet.HasException)
                        {
                            var Groups = ListSereServ.GroupBy(g => g.SERVICE_ID).ToList();
                            foreach (var group in Groups)
                            {
                                List<V_HIS_SERE_SERV> listSub = group.ToList<V_HIS_SERE_SERV>();
                                listRdo.Add(new Mrs00107RDO(listSub));
                            }
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    Total_Amount = listRdo.Sum(s => s.AMOUNT);
                    ListRdo.AddRange(listRdo.GroupBy(g => g.SERVICE_ID).Select(s => new Mrs00107RDO { SERVICE_ID = s.First().SERVICE_ID, DEPARTMENT_ID = s.First().DEPARTMENT_ID, SERVICE_CODE = s.First().SERVICE_CODE, SERVICE_NAME = s.First().SERVICE_NAME, AMOUNT = s.Sum(s1 => s1.AMOUNT) }).ToList());
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
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("INTRUCTION_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("INTRUCTION_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                dicSingleTag.Add("TOTAL_AMOUNT_ALL", ListDepartment.Sum(s => s.TOTAL_AMOUNT));
                dicSingleTag.Add("EXECUTE_ROOM_NAME", Execute_Room_Name);

                objectTag.AddObjectData(store, "Departments", ListDepartment);
                objectTag.AddObjectData(store, "Services", ListRdo);
                objectTag.AddRelationship(store, "Departments", "Services", "ID", "DEPARTMENT_ID");
                objectTag.SetUserFunction(store, "FuncRownumber", new RDOCustomerFuncManyRownumberData());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

    class Department
    {
        public long ID { get; set; }

        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }

        public decimal TOTAL_AMOUNT { get; set; }
    }

    class RDOCustomerFuncManyRownumberData : FlexCel.Report.TFlexCelUserFunction
    {
        private long Department_Id { get; set; }
        private long num_order { get; set; }
        public RDOCustomerFuncManyRownumberData()
        {
        }
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            //long result = 0; 
            try
            {
                long departmentId = Convert.ToInt64(parameters[0]);
                if (Department_Id == 0 || departmentId != Department_Id)
                {
                    Department_Id = departmentId;
                    num_order = 0;
                }
                if (departmentId == Department_Id)
                {
                    num_order = (num_order + 1);
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
