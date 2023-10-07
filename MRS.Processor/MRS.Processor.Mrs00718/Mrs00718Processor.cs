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
using FlexCel.Report;

namespace MRS.Processor.Mrs00718
{
    class Mrs00718Processor : AbstractProcessor
    {
        private Mrs00718Filter filter;
        List<ManagerSql.SERE_SERV> Listdata = new List<ManagerSql.SERE_SERV>();
        List<Mrs00718RDO> ListRdo = new List<Mrs00718RDO>();
        CommonParam paramGet = new CommonParam();

        public Mrs00718Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00718Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00718Filter)reportFilter;
            try
            {
                Listdata = new MRS.Processor.Mrs00718.ManagerSql().GetService(filter) ?? new List<ManagerSql.SERE_SERV>();
                
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
            var result = true;

            try
            {
                if (IsNotNullOrEmpty(Listdata))
                {
                    Dictionary<string, Mrs00718RDO> dicRdo = new Dictionary<string, Mrs00718RDO>();
                    foreach (var item in Listdata)
                    {
                            Mrs00718RDO rdo = new Mrs00718RDO();
                        if (!dicRdo.ContainsKey(item.MA_DICH_VU))
                        {
                            rdo.PR_SERVICE_NAME = item.DICH_VU_CHA;
                            rdo.SV_SERVICE_NAME = item.DICH_VU;
                            //rdo.PATIENT_TYPE_NAME = item.DOI_TUONG_THANH_TOAN;
                            rdo.PR_SERVICE_CODE = item.MA_DICH_VU_CHA;
                            rdo.SV_SERVICE_CODE = item.MA_DICH_VU;
                            //rdo.PATIENT_TYPE_CODE = item.MA_DOI_TUONG_THANH_TOAN;
                            rdo.COUNT_TOTAL = item.SO_LUONG;
                            rdo.DIC_PATIENT_TYPE = new Dictionary<string,int>();
                            rdo.DIC_TREATMENT_TYPE = new Dictionary<string, int>();
                            dicRdo[item.MA_DICH_VU] = rdo;
                            
                        }
                        else
                        {
                            dicRdo[item.MA_DICH_VU].COUNT_TOTAL += item.SO_LUONG;
                        }
                        if (!dicRdo[item.MA_DICH_VU].DIC_PATIENT_TYPE.ContainsKey(item.MA_DOI_TUONG_THANH_TOAN))
                        {
                            dicRdo[item.MA_DICH_VU].DIC_PATIENT_TYPE[item.MA_DOI_TUONG_THANH_TOAN] = item.SO_LUONG;
                        }
                        else
                        {
                            dicRdo[item.MA_DICH_VU].DIC_PATIENT_TYPE[item.MA_DOI_TUONG_THANH_TOAN] += item.SO_LUONG;
                        }
                        if (!dicRdo[item.MA_DICH_VU].DIC_TREATMENT_TYPE.ContainsKey(item.MA_DIEN_DIEU_TRI))
                        {
                            dicRdo[item.MA_DICH_VU].DIC_TREATMENT_TYPE[item.MA_DIEN_DIEU_TRI] = item.SO_LUONG;
                        }
                        else
                        {
                            dicRdo[item.MA_DICH_VU].DIC_TREATMENT_TYPE[item.MA_DIEN_DIEU_TRI] += item.SO_LUONG;
                        }
                    }
                    ListRdo = dicRdo.Values.ToList();
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
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM ?? 0));

            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO ?? 0));
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "Parent", ListRdo.GroupBy(o=>o.PR_SERVICE_CODE).Select(p=>p.First()).ToList());
            objectTag.AddRelationship(store, "Parent", "Report", "PR_SERVICE_CODE", "PR_SERVICE_CODE");
        }
    }
}
