using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Reflection; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00123
{
    class Mrs00123RDO
    {
        public const int MAX_DEPARTMENT_NUM = 15; 
        public long PatientTypeId { get;  set;  }

        //So luong kham tuong ung voi tung khoa
        public int Department1ExamAmount { get;  set;  }
        public int Department2ExamAmount { get;  set;  }
        public int Department3ExamAmount { get;  set;  }
        public int Department4ExamAmount { get;  set;  }
        public int Department5ExamAmount { get;  set;  }
        public int Department6ExamAmount { get;  set;  }
        public int Department7ExamAmount { get;  set;  }
        public int Department8ExamAmount { get;  set;  }
        public int Department9ExamAmount { get;  set;  }
        public int Department10ExamAmount { get;  set;  }
        public int Department11ExamAmount { get;  set;  }
        public int Department12ExamAmount { get;  set;  }
        public int Department13ExamAmount { get;  set;  }
        public int Department14ExamAmount { get;  set;  }
        public int Department15ExamAmount { get;  set;  }

        //So luong dieu tri tuong ung voi tung khoa
        public int Department1TreatAmount { get;  set;  }
        public int Department2TreatAmount { get;  set;  }
        public int Department3TreatAmount { get;  set;  }
        public int Department4TreatAmount { get;  set;  }
        public int Department5TreatAmount { get;  set;  }
        public int Department6TreatAmount { get;  set;  }
        public int Department7TreatAmount { get;  set;  }
        public int Department8TreatAmount { get;  set;  }
        public int Department9TreatAmount { get;  set;  }
        public int Department10TreatAmount { get;  set;  }
        public int Department11TreatAmount { get;  set;  }
        public int Department12TreatAmount { get;  set;  }
        public int Department13TreatAmount { get;  set;  }
        public int Department14TreatAmount { get;  set;  }
        public int Department15TreatAmount { get;  set;  }

        public string PatientTypeName { get;  set;  }

        //Lay gia tri theo thu tu va loai cua trường dữ liệu
        public int GetValue(int order, Mrs00123RDOFieldType fieldType)
        {
            PropertyInfo p = this.GetProperty(order, fieldType); 
            if (p != null)
            {
                return (int)p.GetValue(this); 
            }
            return 0; 
        }

        public void SetValue(int order, Mrs00123RDOFieldType fieldType, int value)
        {
            PropertyInfo p = this.GetProperty(order, fieldType); 
            if (p != null)
            {
                p.SetValue(this, value); 
            }
        }

        private PropertyInfo GetProperty(int order, Mrs00123RDOFieldType fieldType)
        {
            string type = fieldType == Mrs00123RDOFieldType.EXAM ? "Exam" : "Treat"; 
            return typeof(Mrs00123RDO).GetProperty(string.Format("Department{0}{1}Amount", order, type)); 
        }
    }

    enum Mrs00123RDOFieldType
    {
        EXAM,
        TREAT
    }
}
