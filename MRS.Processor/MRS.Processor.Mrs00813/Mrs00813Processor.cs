
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBaby;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00813
{
    public class Mrs00813Processor : AbstractProcessor
    {

        public List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();
        public List<V_HIS_BABY> listBaby = new List<V_HIS_BABY>();
        public Mrs00813Filter filter;
        public List<Mrs00813RDO> listRdo = new List<Mrs00813RDO>();
        public List<HIS_BIRTH_CERT_BOOK> listBirthCretBook = new List<HIS_BIRTH_CERT_BOOK>();
        public List<PrintLogUnique> listPrintLogUnique = new List<PrintLogUnique>();
        public CommonParam commonParam = new CommonParam();
        public Mrs00813Processor(CommonParam param, string reportName):base (param,reportName)
        {

        }
        public override Type FilterType()
        {
            return typeof(Mrs00813Filter);
        }

        protected override bool GetData()
        {
            filter = (Mrs00813Filter)this.reportFilter;
            bool result = false;
            try
            {
                HisTreatmentFilterQuery treatmentfilter = new HisTreatmentFilterQuery();
                if (filter.TIME_FROM!=null)
                {
                    treatmentfilter.OUT_TIME_FROM = filter.TIME_FROM;
                }
                if (filter.TIME_TO!=null)
                {
                    treatmentfilter.OUT_TIME_TO = filter.TIME_TO;
                }
                string query = "SELECT * FROM HIS_BIRTH_CERT_BOOK";
                listBirthCretBook = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_BIRTH_CERT_BOOK>(param, query);
                var birthCretBookIds = listBirthCretBook.Select(x=>x.ID).ToList();
                listTreatment = new HisTreatmentManager(new CommonParam()).Get(treatmentfilter);
                var treatmentIds = listTreatment.Select(o => o.ID).ToList();
                if (treatmentIds != null && treatmentIds.Count > 0)
                {
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var Ids = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisBabyViewFilterQuery HisBabyfilter = new HisBabyViewFilterQuery();
                        HisBabyfilter.TREATMENT_IDs = Ids;
                        HisBabyfilter.ORDER_DIRECTION = "ID"; 
                        HisBabyfilter.ORDER_FIELD = "ASC";
                        var listHisBabySub = new HisBabyManager(param).GetView(HisBabyfilter);
                        if (listHisBabySub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisBabySub GetView null");
                        else
                            listBaby.AddRange(listHisBabySub);
                    }
                }
                listBaby = listBaby.Where(x => x.BIRTH_CERT_BOOK_ID != null).ToList();
                //listBaby = listBaby.Where(x => x.BIRTH_CERT_BOOK_NAME.Contains("GCS")).ToList();
                listBaby.Where(x => birthCretBookIds.Contains(x.BIRTH_CERT_BOOK_ID??0)).ToList();
                listPrintLogUnique = new ManagerSql().GetPrintLog(filter) ?? new List<PrintLogUnique>();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                if (IsNotNullOrEmpty(listPrintLogUnique))
                {
                    foreach (var item in listPrintLogUnique)
                    {
                        if (!string.IsNullOrWhiteSpace(item.UNIQUE_CODE))
                        {
                            string[] uniques = item.UNIQUE_CODE.Split('_');
                            if (uniques.Length == 4)
                            {
                                item.TREATMENT_CODE = uniques[1];
                            }
                            else
                            {
                                int idTreatment = item.UNIQUE_CODE.IndexOf("TREATMENT_CODE:");
                                if (idTreatment >= 0)
                                {
                                    item.TREATMENT_CODE = item.UNIQUE_CODE.Substring(idTreatment + 15, 12);
                                }
                                //int idBirthCert = item.UNIQUE_CODE.IndexOf("BIRTH_CERT_BOOK_ID:");
                                //item.BIRTH_CERT_BOOK_ID = item.UNIQUE_CODE.Substring(idBirthCert + 19, 2);

                            }
                        }
                    }
                }
                foreach (var item in listBaby)
                {
                    Mrs00813RDO rdo = new Mrs00813RDO();
                    var trea = listTreatment.Where(x => x.ID == item.TREATMENT_ID).FirstOrDefault();
                    if (trea!= null)
                    {
                        rdo.TREATMENT_CODE = trea.TREATMENT_CODE;
                        rdo.PATIENT_NAME = trea.TDL_PATIENT_NAME;
                        rdo.PATIENT_CODE = trea.TDL_PATIENT_CODE;
                        rdo.PATIENT_DOB = trea.TDL_PATIENT_DOB.ToString().Substring(0,4);
                        rdo.PATIENT_ADDRESS = trea.TDL_PATIENT_ADDRESS;
                        rdo.IN_DATE = Inventec.Common.DateTime.Convert.TimeNumberToDateString(trea.IN_DATE);
                        rdo.OUT_DATE = Inventec.Common.DateTime.Convert.TimeNumberToDateString(trea.OUT_DATE??0);
                        var PrintLog = listPrintLogUnique.Where(x => x.TREATMENT_CODE == trea.TREATMENT_CODE).OrderByDescending(x=>x.NUM_ORDER).Take(1).FirstOrDefault();
                        if (PrintLog != null)
                        {
                            rdo.COUNT_PRINT = PrintLog.NUM_ORDER;
                        }
                        else
                        {
                            rdo.COUNT_PRINT = 1;
                        }
                    }
                    rdo.SERI_NUMBER = item.BIRTH_CERT_NUM??0;
                    rdo.BOOK_NUMBER = item.BIRTH_CERT_BOOK_NAME;
                    listRdo.Add(rdo);
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            objectTag.AddObjectData(store, "Report", listRdo.OrderBy(x=>x.SERI_NUMBER).ToList());
        }
    }
}
