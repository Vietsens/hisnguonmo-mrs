using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisSereServ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;

using MOS.MANAGER.HisInvoice;
using MOS.MANAGER.HisInvoiceDetail;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisPatientTypeAlter;
using MRS.MANAGER.Config;
using FlexCel.Report;
using MOS.MANAGER.HisIcd;

namespace MRS.Processor.Mrs00348
{
    public class Mrs00348Processor : AbstractProcessor
    {
        Mrs00348Filter castFilter = null;
        private List<Mrs00348RDO> ListRdo = new List<Mrs00348RDO>();

        public Mrs00348Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        List<V_HIS_TREATMENT_4> ListTreatment = new List<V_HIS_TREATMENT_4>();
        List<V_HIS_SERE_SERV_3> ListSereServ3s = new List<V_HIS_SERE_SERV_3>();

        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>();
        Dictionary<long, List<V_HIS_SERE_SERV_3>> dicSereServ = new Dictionary<long, List<V_HIS_SERE_SERV_3>>();

        public override Type FilterType()
        {
            return typeof(Mrs00348Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = ((Mrs00348Filter)reportFilter);
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu bao cao Mrs00348: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => this.castFilter), this.castFilter));

                HisTreatmentView4FilterQuery treatmentFilter = new HisTreatmentView4FilterQuery();
                treatmentFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                treatmentFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                ListTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView4(treatmentFilter);

                if (castFilter.BRANCH_IDs != null)
                {
                    ListTreatment = ListTreatment.Where(o => castFilter.BRANCH_IDs.Contains(o.BRANCH_ID)).ToList();
                }

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu Mrs00348");
                }
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
                ListSereServ3s = new List<V_HIS_SERE_SERV_3>();
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    CommonParam paramGet = new CommonParam();
                    int start = 0;
                    int count = ListTreatment.Count;

                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var listSub = ListTreatment.Skip(start).Take(limit).ToList();

                        HisPatientTypeAlterViewFilterQuery patyAlterFilter = new HisPatientTypeAlterViewFilterQuery();
                        patyAlterFilter.TREATMENT_IDs = listSub.Select(s => s.ID).ToList();
                        patyAlterFilter.ORDER_DIRECTION = "DESC";
                        patyAlterFilter.ORDER_FIELD = "LOG_TIME";
                        patyAlterFilter.TREATMENT_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM;
                        var listPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(patyAlterFilter);

                        HisSereServView3FilterQuery sereServFilter = new HisSereServView3FilterQuery();
                        sereServFilter.TREATMENT_IDs = patyAlterFilter.TREATMENT_IDs;
                        var sereServ3s = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView3(sereServFilter);
                        //ListSereServ3s.AddRange(sereServ3s);
                        if (IsNotNullOrEmpty(sereServ3s))
                        {
                            foreach (var item in sereServ3s)
                            {
                                if (item.TDL_SERVICE_REQ_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH)
                                {
                                    continue;
                                }
                                if (!dicSereServ.ContainsKey(item.TDL_TREATMENT_ID ?? 0))
                                    dicSereServ[item.TDL_TREATMENT_ID ?? 0] = new List<V_HIS_SERE_SERV_3>();
                                dicSereServ[item.TDL_TREATMENT_ID ?? 0].Add(item);
                            }
                        }
                        if (paramGet.HasException)
                        {
                            throw new Exception("Co exception xay ra tai DAOGET trong qua trinh tong lay du lieu Mrs00348");
                        }

                        if (IsNotNullOrEmpty(listPatientTypeAlter))
                        {
                            foreach (var item in listPatientTypeAlter)
                            {
                                if (dicPatientTypeAlter.ContainsKey(item.TREATMENT_ID))
                                    continue;
                                dicPatientTypeAlter[item.TREATMENT_ID] = item;
                            }
                        }

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }

                    this.ProcessDataDetail();
                    this.ProcessListRdo();
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        void ProcessDataDetail()
        {
            foreach (var treatment in ListTreatment)
            {

                if (!dicPatientTypeAlter.ContainsKey(treatment.ID))
                {
                    Inventec.Common.Logging.LogSystem.Info("Ho So Dieu Tri khong co thong tin doi tuong: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => treatment.TREATMENT_CODE), treatment.TREATMENT_CODE));
                    continue;
                }

                var patyAlter = dicPatientTypeAlter[treatment.ID];
                if (patyAlter.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                {
                    continue;
                }

                if (!dicSereServ.ContainsKey(treatment.ID))
                {
                    continue;
                }

                List<string> executeRoom = new List<string>();
                var roomIds = dicSereServ[treatment.ID].Select(o => o.TDL_EXECUTE_ROOM_ID).ToList();
                if (IsNotNullOrEmpty(roomIds))
                {
                    foreach (var item in roomIds)
                    {
                        var room = MANAGER.Config.HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item);
                        if (IsNotNull(room))
                        {
                            executeRoom.Add(room.ROOM_NAME);
                        }
                    }
                }

                Mrs00348RDO rdo = new Mrs00348RDO(treatment);
                var time = dicSereServ[treatment.ID].OrderBy(o => o.TDL_INTRUCTION_TIME).FirstOrDefault().TDL_INTRUCTION_TIME;
                rdo.INTRUCTION_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(time);
                rdo.EXECUTE_ROOM = String.Join(";", executeRoom);

                if (patyAlter.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    rdo.IS_BHYT = "X";
                }

                ListRdo.Add(rdo);
            }
        }

        void ProcessListRdo()
        {
            try
            {
                if (IsNotNullOrEmpty(ListRdo))
                {
                    ListRdo = ListRdo.OrderBy(o => o.ID).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (((Mrs00348Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00348Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00348Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00348Filter)reportFilter).TIME_TO));
            }
            dicSingleTag.Add("COUNT_TREATMENT", ListRdo.Count);

            objectTag.AddObjectData(store, "Report", ListRdo);
            store.SetCommonFunctions();
        }
    }
}
