using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
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
using Inventec.Common.DateTime;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMedicineTypeAcin;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisSupplier;
using MOS.MANAGER.HisMedicineBean;

namespace MRS.Processor.Mrs00717
{
    class Mrs00717Processor : AbstractProcessor
    {

        private Mrs00717Filter filter;
        List<ManagerSql.TREATMENT> Listdata = new List<ManagerSql.TREATMENT>();
      

        List<Mrs00717RDO> ListRdo = new List<Mrs00717RDO>();
        CommonParam paramGet = new CommonParam();
        const string checkThai = "thai";
        const string CATEGORY_CODE__HIV = "717HIV";
        const string CATEGORY_CODE_XNNUOVTIEU = "717XNNT";
        const string CATEGORY_CODE_XNMAU = "717XNMAU";
        const string CATEGORY_CODE_PHATHAI = "717PHATHAI";

        Dictionary<string, object> dicValue = new Dictionary<string, object>();

        public Mrs00717Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
        List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCat = null;
        
        public override Type FilterType()
        {
            return typeof(Mrs00717Filter);
        }
        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00717Filter)reportFilter;
            try
            {
                Listdata = new MRS.Processor.Mrs00717.ManagerSql().GetTreatment(filter) ?? new List<ManagerSql.TREATMENT>();
                listServiceRetyCat = new MRS.Processor.Mrs00717.ManagerSql().GetServiceRetyCat() ?? new List<V_HIS_SERVICE_RETY_CAT>();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private bool Less(long Dob, long IN_TIME, long Age)
        {
            return IN_TIME - Age * 10000000000 <= Dob;
        }
        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                if (IsNotNullOrEmpty(Listdata))
                {
                    //Mrs00717RDO rdo = new Mrs00717RDO();

                    List<long> serviceId_HIV = new List<long>();
                    List<long> serviceId_XNMAU = new List<long>();
                    List<long> serviceId_XNNT = new List<long>();
                    List<long> serviceId_PHATHAI = new List<long>();
                    if (IsNotNullOrEmpty(listServiceRetyCat))
                    {
                        foreach (var item in listServiceRetyCat)
                        {
                            if (item.CATEGORY_CODE == CATEGORY_CODE__HIV)
                            {
                                serviceId_HIV.Add(item.SERVICE_ID);
                            }
                            else if (item.CATEGORY_CODE == CATEGORY_CODE_XNMAU)
                            {
                                serviceId_XNMAU.Add(item.SERVICE_ID);
                            }
                            else if (item.CATEGORY_CODE == CATEGORY_CODE_XNNUOVTIEU)
                            {
                                serviceId_XNNT.Add(item.SERVICE_ID);
                            }
                            else if (item.CATEGORY_CODE == CATEGORY_CODE_PHATHAI)
                            {
                                serviceId_PHATHAI.Add(item.SERVICE_ID);
                            }
                        }
                    }

                    List<ManagerSql.TREATMENT> listDataNotThai = Listdata.Where(o => !IsNotNull(o.ICD_NAME) || !o.ICD_NAME.ToLower().Contains(checkThai)).ToList();
                    //lọc danh sách chỉ lấy các hồ sơ có chẩn đoán chữa chữ "thai"
                    List<ManagerSql.TREATMENT> listDataThai = Listdata.Where(o => IsNotNull(o.ICD_NAME) && o.ICD_NAME.ToLower().Contains(checkThai)).ToList();
                    if (IsNotNullOrEmpty(listDataThai))
                    {

                        int bn = listDataThai.Select(o => o.TDL_PATIENT_CODE).Distinct().Count();
                        dicValue.Add("COUNT_PATIENT_CODE_FETUS", bn);

                        int dt = listDataThai.Select(o => o.TREATMENT_CODE).Distinct().Count();
                        dicValue.Add("COUNT_TREATMENT_CODE_FETUS", dt);
                        var t19 = listDataThai.Where(o => Less(o.TDL_PATIENT_DOB, o.IN_TIME, 19)).ToList();
                        int bn19 = t19.Select(o => o.TDL_PATIENT_CODE).Distinct().Count();
                        dicValue.Add("COUNT_PATIENT_CODE_FETUS_LESS19", bn19);
                        var sieuam = listDataThai.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA).ToList();
                        int bnsieuam = sieuam.Select(o => o.TREATMENT_CODE).Distinct().Count();
                        dicValue.Add("COUNT_TREATMENT_CODE_FETUS_ULTRASOUND", bnsieuam);
                        // var xetnghiem = Listdata.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN && IsNotNull(o.ICD_NAME) && o.ICD_NAME.ToLower().Contains(checknuoctieu)).ToList();
                        // int bnxetnghiem = xetnghiem.Select(o => o.TREATMENT_CODE).Distinct().Count();

                        var hiv = listDataThai.Where(o => serviceId_HIV.Contains(o.SERVICE_ID)).ToList();
                        int xnhiv = hiv.Count();
                        dicValue.Add("COUNT_TREATMENT_CODE_FETUS_TEST_HIV", xnhiv);
                        var mau = listDataThai.Where(o => serviceId_XNMAU.Contains(o.SERVICE_ID)).ToList();
                        int xnmau = mau.Count();
                        dicValue.Add("COUNT_TREATMENT_CODE_TEST_FETUS_BLOOD", xnmau);
                        var nuoctieu = listDataThai.Where(o => serviceId_XNNT.Contains(o.SERVICE_ID)).ToList();
                        int xnnuoctieu = nuoctieu.Count();
                        dicValue.Add("COUNT_TREATMENT_CODE_FETUS_TEST_URINE", xnnuoctieu);
                        var phathai = listDataThai.Where(o => serviceId_PHATHAI.Contains(o.SERVICE_ID)).ToList();
                        int bnphathai = phathai.Count();
                        dicValue.Add("COUNT_TREATMENT_CODE_OBSTETRIC_ABORTION", bnphathai);
                    }

                    if (IsNotNullOrEmpty(listDataNotThai))
                    {
                        var lstTreatment = listDataNotThai.GroupBy(o => o.TREATMENT_CODE).Select(s => s.First()).ToList();
                        var group = lstTreatment.GroupBy(o => o.LAST_DEPARTMENT_ID).ToList();
                        foreach (var item in group)
                        {
                            dicValue.Add("COUNT_TREATMENT_CODE_EXAM_OBSTETRIC_" + item.First().LAST_DEPARTMENT_CODE, item.Count());
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM ?? 0));

            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO ?? 0));

            foreach (var item in dicValue)
            {
                if (!dicSingleTag.ContainsKey(item.Key))
                    dicSingleTag.Add(item.Key, item.Value);

                //dicSingleTag[item.Key] = item.Value;

            }

            //objectTag.AddObjectData(store, "Report", ListRdo);
        }
    }
}
