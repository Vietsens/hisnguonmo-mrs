using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisDepartment;
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
using MOS.MANAGER.HisWorkPlace;
using Inventec.Common.Logging;

namespace MRS.Processor.Mrs00074
{
    public class Mrs00074Processor : AbstractProcessor
    {
        Mrs00074Filter castFilter = null;
        List<Mrs00074RDO> ListRdo = new List<Mrs00074RDO>();
        Dictionary<long, Mrs00074RDO> dicTreatmentRdo = new Dictionary<long, Mrs00074RDO>();
        string Department_Name;
        List<V_HIS_TREATMENT_FEE> ListTreatmentFee;
        List<HIS_WORK_PLACE> listWorkPlace = new List<HIS_WORK_PLACE>();
        Dictionary<long, string> dicTreatmentMainSurg = new Dictionary<long, string>();
        Dictionary<long, string> dicTreatmentEinvoiceNumOrder = new Dictionary<long, string>();

        public Mrs00074Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00074Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00074Filter)this.reportFilter);

                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_TREATMENT. MRS00074. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                HisTreatmentFeeViewFilterQuery treatmentFilter = new HisTreatmentFeeViewFilterQuery();
                treatmentFilter.OUT_TIME_FROM = castFilter.TIME_FROM;
                treatmentFilter.OUT_TIME_TO = castFilter.TIME_TO;
                ListTreatmentFee = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetFeeView(treatmentFilter);
                if (castFilter.PATIENT_CLASSIFY_IDs != null)
                {
                    ListTreatmentFee = ListTreatmentFee.Where(o => castFilter.PATIENT_CLASSIFY_IDs.Contains(o.TDL_PATIENT_CLASSIFY_ID ?? 0)).ToList();
                }
                if (castFilter.PATIENT_TYPE_IDs != null)
                {
                    ListTreatmentFee = ListTreatmentFee.Where(o => castFilter.PATIENT_TYPE_IDs.Contains(o.TDL_PATIENT_TYPE_ID ?? 0)).ToList();
                }
                //thêm thông tin nơi làm việc
                GetWorkPlace();

                //thêm thông tin bác sĩ phẫu thuật chính
                GetMainSurg(castFilter);

                //thêm thông tin số hóa đơn điện tử
                GetEinvoiceNumOrder(castFilter);

                FilterWorkPlace();
               

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetMainSurg(Mrs00074Filter filter)
        {
            try
            {
                string query = "";
               query += string.Format("select\n");
                query += string.Format("trea.id treatment_id,\n");
                query += string.Format("sse.INSTRUCTION_NOTE username\n");
                query += string.Format("from his_treatment trea\n");
                query += string.Format("join his_sere_serv ss on ss.tdl_treatment_id=trea.id\n");
                query += string.Format("join his_service_req sr on sr.id=ss.service_req_id\n");
                query += string.Format("join his_sere_serv_pttt ssp on ss.id=ssp.sere_serv_id\n");
                query += string.Format("join his_sere_serv_ext sse on ss.id=sse.sere_serv_id\n");
                query += string.Format("where 1=1 and trea.out_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
                if (castFilter.PATIENT_CLASSIFY_IDs != null)
                {
                    query += string.Format("and trea.tdl_patient_classify_id in ({0})\n", string.Join(", ", castFilter.PATIENT_CLASSIFY_IDs));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var listMainSurg = new MOS.DAO.Sql.SqlDAO().GetSql<MAIN_SURG>(query);
                if (listMainSurg != null && listMainSurg.Count > 0)
                {
                    dicTreatmentMainSurg = listMainSurg.GroupBy(o => o.TREATMENT_ID).ToDictionary(p => p.Key, q => string.Join(", ", q.Select(o => o.USERNAME).Distinct().ToList()));
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void GetEinvoiceNumOrder(Mrs00074Filter filter)
        {
            try
            {
                string query = "";
                  query += string.Format("select\n");
                query += string.Format("trea.id treatment_id,\n");
                query += string.Format("tran.einvoice_num_order\n");
                query += string.Format("from his_treatment trea\n");
                query += string.Format("join his_transaction tran on tran.treatment_id=trea.id and tran.einvoice_num_order is not null\n");
                query += string.Format("where 1=1 and trea.out_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var listEinvoiceNumOrder = new MOS.DAO.Sql.SqlDAO().GetSql<EINVOICE_INF>(query);
                if (listEinvoiceNumOrder != null && listEinvoiceNumOrder.Count > 0)
                {
                    dicTreatmentEinvoiceNumOrder = listEinvoiceNumOrder.GroupBy(o => o.TREATMENT_ID).ToDictionary(p => p.Key, q => string.Join(", ", q.Select(o => o.EINVOICE_NUM_ORDER).Distinct().ToList()));
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void GetWorkPlace()
        {
            listWorkPlace = new HisWorkPlaceManager().Get(new HisWorkPlaceFilterQuery());
        }

        private void FilterWorkPlace()
        {
            if (castFilter.WORK_PLACE_IDs != null)
            {
                ListTreatmentFee = ListTreatmentFee.Where(o => castFilter.WORK_PLACE_IDs.Contains(o.TDL_PATIENT_WORK_PLACE_ID ?? 0)).ToList();
            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(ListTreatmentFee))
                {
                    ProcessListTreatment(ListTreatmentFee);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListTreatment(List<V_HIS_TREATMENT_FEE> ListTreatment)
        {
            try
            {
                ListTreatment = ListTreatment.Where(o => o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.IS_ACTIVE == ((this.castFilter.IS_FEE_LOCK != true) ? IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE : IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE)).ToList();
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    CommonParam paramGet = new CommonParam();

                    ListTreatment = ListTreatment.Where(o => CheckTreatmentOnDepartment(paramGet, o)).ToList();

                    if (!paramGet.HasException)
                    {
                        int start = 0;
                        int count = dicTreatmentRdo.Count;
                        while (count > 0)
                        {
                            int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var dicSub = dicTreatmentRdo.Skip(start).Take(limit).ToDictionary(k => k.Key, k => k.Value);

                            ProcessDicTreatment(paramGet, dicSub);
                            start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        }

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00074.");
                        }
                    }
                    else
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00074.");
                    }
                    ListRdo = ListRdo.OrderBy(o => o.TREATMENT_CODE).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void ProcessDicTreatment(CommonParam paramGet, Dictionary<long, Mrs00074RDO> dicSub)
        {
            try
            {
                if (IsNotNullOrEmpty(dicSub))
                {
                    foreach (var dic in dicSub)
                    {
                        dic.Value.ProcessTotalPrice(dic.Value, ref paramGet);
                        ListRdo.Add(dic.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private bool CheckTreatmentOnDepartment(CommonParam paramGet, V_HIS_TREATMENT_FEE treatment)
        {
            bool result = false;
            try
            {
                Mrs00074RDO rdo = new Mrs00074RDO(treatment);

                rdo.DATE_IN_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.IN_TIME);

                if (treatment.OUT_TIME.HasValue)
                {
                    rdo.DATE_OUT_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.OUT_TIME.Value);
                }
                if (dicTreatmentMainSurg.ContainsKey(treatment.ID))
                {
                    rdo.MAIN_SURG_USERNAME = dicTreatmentMainSurg[treatment.ID];
                }
                if (dicTreatmentEinvoiceNumOrder.ContainsKey(treatment.ID))
                {
                    rdo.EINVOICE_NUM_ORDER = dicTreatmentEinvoiceNumOrder[treatment.ID];
                }
                if (castFilter.DEPARTMENT_ID.HasValue)
                {
                    if (treatment.END_DEPARTMENT_ID.HasValue && treatment.END_DEPARTMENT_ID == castFilter.DEPARTMENT_ID.Value)
                    {
                        result = true;
                        dicTreatmentRdo.Add(treatment.ID, rdo);
                    }
                }
                else
                {
                    result = true;
                    dicTreatmentRdo.Add(treatment.ID, rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetDepartment()
        {
            try
            {
                if (castFilter.DEPARTMENT_ID.HasValue)
                {
                    var department = new MOS.MANAGER.HisDepartment.HisDepartmentManager().GetById(castFilter.DEPARTMENT_ID.Value);
                    if (IsNotNull(department))
                    {
                        Department_Name = department.DEPARTMENT_NAME;
                    }
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
                    dicSingleTag.Add("CREATE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                GetDepartment();
                dicSingleTag.Add("DEPARTMENT_NAME", Department_Name);

                objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
