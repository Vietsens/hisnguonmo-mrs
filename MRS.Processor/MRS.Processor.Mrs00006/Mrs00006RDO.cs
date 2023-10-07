using Inventec.Common.Logging; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00006
{
    class Mrs00006RDO
    {
        public string NAME { get;  set;  }

        public string PACKING_TYPE_NAME { get; set; }

        public string SERVICE_UNIT_NAME { get;  set;  }
        public string DATE1 { get;  set;  }
        public string DATE2 { get;  set;  }
        public string DATE3 { get;  set;  }
        public string DATE4 { get;  set;  }
        public string DATE5 { get;  set;  }
        public string DATE6 { get;  set;  }
        public string DATE7 { get;  set;  }
        public string DATE8 { get;  set;  }
        public string DATE9 { get;  set;  }
        public string DATE10 { get;  set;  }
        public string DATE11 { get;  set;  }
        public string DATE12 { get;  set;  }
        public string DATE13 { get;  set;  }
        public string DATE14 { get;  set;  }
        public string DATE15 { get;  set;  }

        public decimal AMOUNT1 { get;  set;  }
        public decimal AMOUNT2 { get;  set;  }
        public decimal AMOUNT3 { get;  set;  }
        public decimal AMOUNT4 { get;  set;  }
        public decimal AMOUNT5 { get;  set;  }
        public decimal AMOUNT6 { get;  set;  }
        public decimal AMOUNT7 { get;  set;  }
        public decimal AMOUNT8 { get;  set;  }
        public decimal AMOUNT9 { get;  set;  }
        public decimal AMOUNT10 { get;  set;  }
        public decimal AMOUNT11 { get;  set;  }
        public decimal AMOUNT12 { get;  set;  }
        public decimal AMOUNT13 { get;  set;  }
        public decimal AMOUNT14 { get;  set;  }
        public decimal AMOUNT15 { get;  set;  }

        public Mrs00006RDO(string name, string unitName)
        {
            try
            {
                PACKING_TYPE_NAME = PACKING_TYPE_NAME;
                NAME = name; 
                SERVICE_UNIT_NAME = unitName; 
                SetExtendField(this); 
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
            }
        }

        private void SetExtendField(Mrs00006RDO data)
        {

        }

        public bool GenerateDate(System.DateTime? fromDate)
        {
            bool result = false; 
            try
            {
                if (fromDate.HasValue)
                {
                    DATE1 = Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromDate.Value); 
                    DATE2 = Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromDate.Value.AddDays(1)); 
                    DATE3 = Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromDate.Value.AddDays(2)); 
                    DATE4 = Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromDate.Value.AddDays(3)); 
                    DATE5 = Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromDate.Value.AddDays(4)); 
                    DATE6 = Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromDate.Value.AddDays(5)); 
                    DATE7 = Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromDate.Value.AddDays(6)); 
                    DATE8 = Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromDate.Value.AddDays(7)); 
                    DATE9 = Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromDate.Value.AddDays(8)); 
                    DATE10 = Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromDate.Value.AddDays(9)); 
                    DATE11 = Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromDate.Value.AddDays(10)); 
                    DATE12 = Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromDate.Value.AddDays(11)); 
                    DATE13 = Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromDate.Value.AddDays(12)); 
                    DATE14 = Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromDate.Value.AddDays(13)); 
                    DATE15 = Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromDate.Value.AddDays(14)); 
                    result = true; 
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
                result = false; 
            }

            return result; 
        }

        public bool Calculate(long? time, decimal amount)
        {
            bool result = false; 
            try
            {
                if (time.HasValue)
                {
                    string date = Inventec.Common.DateTime.Convert.TimeNumberToDateString(time.Value); 
                    if (!String.IsNullOrEmpty(date))
                    {
                        if (date == DATE1) AMOUNT1 += amount; 
                        else if (date == DATE2) AMOUNT2 += amount; 
                        else if (date == DATE3) AMOUNT3 += amount; 
                        else if (date == DATE4) AMOUNT4 += amount; 
                        else if (date == DATE5) AMOUNT5 += amount; 
                        else if (date == DATE6) AMOUNT6 += amount; 
                        else if (date == DATE7) AMOUNT7 += amount; 
                        else if (date == DATE8) AMOUNT8 += amount; 
                        else if (date == DATE9) AMOUNT9 += amount; 
                        else if (date == DATE10) AMOUNT10 += amount; 
                        else if (date == DATE11) AMOUNT11 += amount; 
                        else if (date == DATE12) AMOUNT12 += amount; 
                        else if (date == DATE13) AMOUNT13 += amount; 
                        else if (date == DATE14) AMOUNT14 += amount; 
                        else if (date == DATE15) AMOUNT15 += amount; 
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }
    }
}
