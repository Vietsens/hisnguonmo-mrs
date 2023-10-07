using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMestBlood;
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
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisInvoice;
using MOS.MANAGER.HisInvoiceDetail;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicineType;
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
using SAR.EFMODEL.DataModels;
using SAR.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00364
{
    public class Mrs00364Processor : AbstractProcessor
    {
        /// <summary>
        /// Khai bao
        CommonParam paramGet = new CommonParam();
        Mrs00364Filter castFilter = null;
        List<Mrs00364RDO> ListRdo = new List<Mrs00364RDO>();

        List<V_HIS_EXP_MEST> listExpMest = new List<V_HIS_EXP_MEST>();
        List<V_HIS_EXP_MEST> ListName = new List<V_HIS_EXP_MEST>();
        List<V_HIS_EXP_MEST_BLOOD> listExpMestBloodView = new List<V_HIS_EXP_MEST_BLOOD>();
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterialView = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicineView = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();
        List<HIS_MEDI_STOCK> lisMedistock = new List<HIS_MEDI_STOCK>();
        HIS_MEDI_STOCK medistock = new HIS_MEDI_STOCK();
        HIS_DEPARTMENT department = new HIS_DEPARTMENT();
        public Mrs00364Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00364Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                this.castFilter = (Mrs00364Filter)this.reportFilter;
                var paramGet = new CommonParam();
                //get dữ liệu:
                //if (castFilter.DEPARTMENT_ID > 0)
                //listDepartment = new HisDepartmentManager(paramGet).GetById(castFilter.DEPARTMENT_IDs);
                listDepartment = new HisDepartmentManager(paramGet).Get(new HisDepartmentFilterQuery());
                lisMedistock = new HisMediStockManager(paramGet).Get(new HisMediStockFilterQuery());
                if (castFilter.DEPARTMENT_IDs != null)
                    listDepartment = listDepartment.Where(x => castFilter.DEPARTMENT_IDs.Contains(x.ID)).ToList();
                if (castFilter.MEDI_STOCK_ID != null)
                    medistock = lisMedistock.Where(x => castFilter.MEDI_STOCK_ID == x.ID).First();
                HisExpMestViewFilterQuery expMestfilter = new HisExpMestViewFilterQuery();
                expMestfilter.FINISH_DATE_FROM = castFilter.TIME_FROM;
                expMestfilter.FINISH_DATE_TO = castFilter.TIME_TO;
                expMestfilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                expMestfilter.EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DM;
                expMestfilter.REQ_DEPARTMENT_IDs = castFilter.DEPARTMENT_IDs;
                expMestfilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                listExpMest = new HisExpMestManager(paramGet).GetView(expMestfilter);
                /////////////////////////////////////////////////////////////////
                var listExpMestIds = listExpMest.Select(s => s.ID).ToList();

                var skip = 0;
                while (listExpMestIds.Count - skip > 0)
                {
                    var listIds = listExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    var metyFilterExpMestBlood = new HisExpMestBloodViewFilterQuery
                    {
                        EXP_MEST_IDs = listIds,
                    };
                    var expMestBloodViews = new MOS.MANAGER.HisExpMestBlood.HisExpMestBloodManager(paramGet).GetView(metyFilterExpMestBlood);
                    listExpMestBloodView.AddRange(expMestBloodViews);
                }

                ListName = listExpMest.GroupBy(o => o.TDL_PATIENT_ID ?? 0).Select(p => p.First()).ToList();

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

                foreach (var expMest in listExpMest)
                {
                    var expMestBloodView = listExpMestBloodView.Where(s => s.EXP_MEST_ID == expMest.ID).ToList();
                    if (IsNotNullOrEmpty(expMestBloodView))
                    {
                        Mrs00364RDO rdo = new Mrs00364RDO
                        {
                            EXP_MEST_CODE = expMest.EXP_MEST_CODE,
                            BLOOD_ABO_NAME = expMestBloodView.First().BLOOD_ABO_CODE,
                            AMOUNT = expMestBloodView.First().VOLUME,
                            PRICE = expMestBloodView.First().PRICE ?? 0,
                            TOTAL_PRICE = expMestBloodView.First().PRICE ?? 0,
                            PATIENT_ID = (long)expMest.TDL_PATIENT_ID,
                            BLOOD_TYPE_NAME = expMestBloodView.First().BLOOD_TYPE_NAME
                        };

                        ListRdo.Add(rdo);
                    }
                }

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
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00364Filter)this.reportFilter).TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00364Filter)this.reportFilter).TIME_TO));
            //dicSingleTag.Add("DEPARTMENT_NAME", department.DEPARTMENT_NAME);
            dicSingleTag.Add("MEDI_STOCK_NAME", medistock.MEDI_STOCK_NAME);

            ListRdo = ListRdo.OrderBy(o => o.PATIENT_ID).ToList();
            objectTag.AddObjectData(store, "Name", ListName);

            objectTag.AddObjectData(store, "ExpMest", ListRdo);
            objectTag.AddRelationship(store, "Name", "ExpMest", "TDL_PATIENT_ID", "PATIENT_ID");
        }
    }
}