using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisExpMestType;
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
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisInvoice;
using MOS.MANAGER.HisInvoiceDetail;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMestPeriodMate;
using MOS.MANAGER.HisMestPeriodMedi;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatientTypeAlter;
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
using MOS.MANAGER.HisServiceReq;

namespace MRS.Processor.Mrs00235
{
    public class Mrs00235Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam();
        List<Mrs00235RDO> ListRdo = new List<Mrs00235RDO>();
        List<Mrs00235RDO> ListExpMestType = new List<Mrs00235RDO>();
        HIS_MEDI_STOCK MediStock = new HIS_MEDI_STOCK();
        List<HIS_DEPARTMENT> ListDepartment = new List<HIS_DEPARTMENT>();
        List<Mrs00235RDO> ListSumDepartment = new List<Mrs00235RDO>();
        Mrs00235Filter filter = null;
        public Mrs00235Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00235Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                //get dữ liệu:
                filter = ((Mrs00235Filter)this.reportFilter);
                HisMediStockFilterQuery stock = new HisMediStockFilterQuery();
                if (filter.MEDI_STOCK_ID != null)
                {
                    stock.ID = filter.MEDI_STOCK_ID;
                }

                MediStock = new HisMediStockManager(paramGet).Get(stock).First();
                HisDepartmentFilterQuery Departmentfilter = new HisDepartmentFilterQuery();
                if (filter.DEPARTMENT_IDs != null)
                {
                    Departmentfilter.IDs = filter.DEPARTMENT_IDs;
                }
                ListDepartment = new HisDepartmentManager(paramGet).Get(Departmentfilter);

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
                if (((Mrs00235Filter)reportFilter).IS_DETAIL == true)
                {
                    ListRdo = new ManagerSql().GetDetail((Mrs00235Filter)reportFilter);
                    if (IsNotNullOrEmpty(ListRdo))
                    {
                        foreach (var item in ListRdo)
                        {
                            item.EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.EXP_TIME ?? 0);
                            if (item.EXP_MEST_REASON_NAME==null)
                            {
                                item.EXP_MEST_REASON_NAME = item.EXP_MEST_TYPE_NAME;
                                if (item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK && item.CHMS_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__ADDITION)
                                {
                                    item.EXP_MEST_REASON_NAME = "Bổ sung cơ số";
                                }
                                if (item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK && item.CHMS_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__REDUCTION)
                                {
                                    item.EXP_MEST_REASON_NAME = "Hoàn trả cơ số";
                                }
                            }
                            //if (item.TT_PATIENT_TYPE_NAME!=null&&item.PATIENT_TYPE_NAME!=null)
                            //{
                            //    if (item.PATIENT_TYPE_NAME.ToLower() == "bhyt" && item.TT_PATIENT_TYPE_NAME.ToLower() == "viện phí")
                            //    {
                            //        item.EXP_MEST_REASON_NAME = "Xuất dịch vụ nội trú";
                            //    }
                                
                            //}
                            
                        }
                    }

                }
                else
                {
                    ListRdo = new ManagerSql().GetTotalExpMest((Mrs00235Filter)reportFilter);
                    if (IsNotNullOrEmpty(ListRdo))
                    {
                        foreach (var rdo in ListRdo)
                        {
                            rdo.EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)rdo.EXP_TIME);
                        }
                    }
                }

                ListSumDepartment = ListRdo.GroupBy(o => new { o.EXP_MEST_TYPE_ID, o.REQ_DEPARTMENT_ID }).Select(p => p.First()).ToList();
                ListExpMestType = ListSumDepartment.GroupBy(o => o.EXP_MEST_TYPE_ID).Select(p => p.First()).ToList();
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
            dicSingleTag.Add("MEDI_STOCK_NAME", MediStock.MEDI_STOCK_NAME);
            objectTag.AddObjectData(store, "ExpMestType", ListExpMestType);
            objectTag.AddObjectData(store, "Department", ListSumDepartment);
            objectTag.AddObjectData(store, "ExpMest", ListRdo);
            objectTag.AddRelationship(store, "ExpMestType", "Department", "EXP_MEST_TYPE_ID", "EXP_MEST_TYPE_ID");
            string[] ship = { "REQ_DEPARTMENT_ID", "EXP_MEST_TYPE_ID" };
            objectTag.AddRelationship(store, "Department", "ExpMest", ship, ship);
            objectTag.SetUserFunction(store, "FuncSameTitleCol", new CustomerFuncMergeSameData());
            if (filter.ORDER_BY == null) //theo phieu xuat
            {
                ListRdo = ListRdo.OrderBy(p => p.EXP_MEST_CODE).ThenBy(p => p.PATIENT_CODE).ThenBy(p => p.EXP_TIME).ThenBy(p => p.MEDI_MATE_CODE).ToList();
            }
            else if (filter.ORDER_BY == false)// theo ma benh nhan
            {
                ListRdo = ListRdo.OrderBy(p => p.PATIENT_CODE).ThenBy(p => p.EXP_MEST_CODE).ThenBy(p => p.EXP_TIME).ThenBy(p => p.MEDI_MATE_CODE).ToList();
            }
            else if (filter.ORDER_BY == true) //theo thoi gian xuat
            {
                ListRdo = ListRdo.OrderBy(p => p.EXP_TIME).ThenBy(p => p.EXP_MEST_CODE).ThenBy(p => p.PATIENT_CODE).ThenBy(p => p.EXP_TIME).ThenBy(p => p.MEDI_MATE_CODE).ToList();
            }
            objectTag.AddObjectData(store, "Report", ListRdo);
        }

        class CustomerFuncMergeSameData : TFlexCelUserFunction
        {
            long MediStockId;
            int SameType;

            public override object Evaluate(object[] parameters)
            {
                if (parameters == null || parameters.Length <= 0)
                    throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

                bool result = false;
                try
                {
                    long mediId = Convert.ToInt64(parameters[0]);
                    int ServiceId = Convert.ToInt32(parameters[1]);

                    if (mediId > 0 && ServiceId > 0)
                    {
                        if (SameType == ServiceId && MediStockId == mediId)
                        {
                            return true;
                        }
                        else
                        {
                            MediStockId = mediId;
                            SameType = ServiceId;
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
    }
}