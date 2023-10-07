using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 
using System.Reflection; 

namespace MRS.Processor.Mrs00285
{
 enum Mrs00285RDOFieldType
 {
  EXAM,
  TREAT
 }
class Mrs00285RDO
    {
     public const int MAX_EXAM_SERVICE_TYPE_NUM = 50; 
     public long PatientTypeId { get;  set;  }

     //So luong kham tuong ung voi tung khoa
     public int ExamServiceType1ExamAmount { get;  set;  }
     public int ExamServiceType2ExamAmount { get;  set;  }
     public int ExamServiceType3ExamAmount { get;  set;  }
     public int ExamServiceType4ExamAmount { get;  set;  }
     public int ExamServiceType5ExamAmount { get;  set;  }
     public int ExamServiceType6ExamAmount { get;  set;  }
     public int ExamServiceType7ExamAmount { get;  set;  }
     public int ExamServiceType8ExamAmount { get;  set;  }
     public int ExamServiceType9ExamAmount { get;  set;  }
     public int ExamServiceType10ExamAmount { get;  set;  }
     public int ExamServiceType11ExamAmount { get;  set;  }
     public int ExamServiceType12ExamAmount { get;  set;  }
     public int ExamServiceType13ExamAmount { get;  set;  }
     public int ExamServiceType14ExamAmount { get;  set;  }
     public int ExamServiceType15ExamAmount { get;  set;  }
     public int ExamServiceType16ExamAmount { get;  set;  }
     public int ExamServiceType17ExamAmount { get;  set;  }
     public int ExamServiceType18ExamAmount { get;  set;  }
     public int ExamServiceType19ExamAmount { get;  set;  }
     public int ExamServiceType20ExamAmount { get;  set;  }
     public int ExamServiceType21ExamAmount { get;  set;  }
     public int ExamServiceType22ExamAmount { get;  set;  }
     public int ExamServiceType23ExamAmount { get;  set;  }
     public int ExamServiceType24ExamAmount { get;  set;  }
     public int ExamServiceType25ExamAmount { get;  set;  }
     public int ExamServiceType26ExamAmount { get;  set;  }
     public int ExamServiceType27ExamAmount { get;  set;  }
     public int ExamServiceType28ExamAmount { get;  set;  }
     public int ExamServiceType29ExamAmount { get;  set;  }
     public int ExamServiceType30ExamAmount { get;  set;  }
     public int ExamServiceType31ExamAmount { get;  set;  }
     public int ExamServiceType32ExamAmount { get;  set;  }
     public int ExamServiceType33ExamAmount { get;  set;  }
     public int ExamServiceType34ExamAmount { get;  set;  }
     public int ExamServiceType35ExamAmount { get;  set;  }
     public int ExamServiceType36ExamAmount { get;  set;  }
     public int ExamServiceType37ExamAmount { get;  set;  }
     public int ExamServiceType38ExamAmount { get;  set;  }
     public int ExamServiceType39ExamAmount { get;  set;  }
     public int ExamServiceType40ExamAmount { get;  set;  }
     public int ExamServiceType41ExamAmount { get;  set;  }
     public int ExamServiceType42ExamAmount { get;  set;  }
     public int ExamServiceType43ExamAmount { get;  set;  }
     public int ExamServiceType44ExamAmount { get;  set;  }
     public int ExamServiceType45ExamAmount { get;  set;  }
     public int ExamServiceType46ExamAmount { get;  set;  }
     public int ExamServiceType47ExamAmount { get;  set;  }
     public int ExamServiceType48ExamAmount { get;  set;  }
     public int ExamServiceType49ExamAmount { get;  set;  }
     public int ExamServiceType50ExamAmount { get;  set;  }
     //So luong dieu tri tuong ung voi tung khoa
     public int ExamServiceType1TreatAmount { get;  set;  }
     public int ExamServiceType2TreatAmount { get;  set;  }
     public int ExamServiceType3TreatAmount { get;  set;  }
     public int ExamServiceType4TreatAmount { get;  set;  }
     public int ExamServiceType5TreatAmount { get;  set;  }
     public int ExamServiceType6TreatAmount { get;  set;  }
     public int ExamServiceType7TreatAmount { get;  set;  }
     public int ExamServiceType8TreatAmount { get;  set;  }
     public int ExamServiceType9TreatAmount { get;  set;  }
     public int ExamServiceType10TreatAmount { get;  set;  }
     public int ExamServiceType11TreatAmount { get;  set;  }
     public int ExamServiceType12TreatAmount { get;  set;  }
     public int ExamServiceType13TreatAmount { get;  set;  }
     public int ExamServiceType14TreatAmount { get;  set;  }
     public int ExamServiceType15TreatAmount { get;  set;  }
     public int ExamServiceType16TreatAmount { get;  set;  }
     public int ExamServiceType17TreatAmount { get;  set;  }
     public int ExamServiceType18TreatAmount { get;  set;  }
     public int ExamServiceType19TreatAmount { get;  set;  }
     public int ExamServiceType20TreatAmount { get;  set;  }
     public int ExamServiceType21TreatAmount { get;  set;  }
     public int ExamServiceType22TreatAmount { get;  set;  }
     public int ExamServiceType23TreatAmount { get;  set;  }
     public int ExamServiceType24TreatAmount { get;  set;  }
     public int ExamServiceType25TreatAmount { get;  set;  }
     public int ExamServiceType26TreatAmount { get;  set;  }
     public int ExamServiceType27TreatAmount { get;  set;  }
     public int ExamServiceType28TreatAmount { get;  set;  }
     public int ExamServiceType29TreatAmount { get;  set;  }
     public int ExamServiceType30TreatAmount { get;  set;  }
     public int ExamServiceType31TreatAmount { get;  set;  }
     public int ExamServiceType32TreatAmount { get;  set;  }
     public int ExamServiceType33TreatAmount { get;  set;  }
     public int ExamServiceType34TreatAmount { get;  set;  }
     public int ExamServiceType35TreatAmount { get;  set;  }
     public int ExamServiceType36TreatAmount { get;  set;  }
     public int ExamServiceType37TreatAmount { get;  set;  }
     public int ExamServiceType38TreatAmount { get;  set;  }
     public int ExamServiceType39TreatAmount { get;  set;  }
     public int ExamServiceType40TreatAmount { get;  set;  }
     public int ExamServiceType41TreatAmount { get;  set;  }
     public int ExamServiceType42TreatAmount { get;  set;  }
     public int ExamServiceType43TreatAmount { get;  set;  }
     public int ExamServiceType44TreatAmount { get;  set;  }
     public int ExamServiceType45TreatAmount { get;  set;  }
     public int ExamServiceType46TreatAmount { get;  set;  }
     public int ExamServiceType47TreatAmount { get;  set;  }
     public int ExamServiceType48TreatAmount { get;  set;  }
     public int ExamServiceType49TreatAmount { get;  set;  }
     public int ExamServiceType50TreatAmount { get;  set;  }

     public string PatientTypeName { get; set; }
     public string TREATMENT_CODE { get; set; }
     
     //Lay gia tri theo thu tu va loai cua trường dữ liệu
     public int GetValue(int order, Mrs00285RDOFieldType fieldType)
     {
      PropertyInfo p = this.GetProperty(order, fieldType); 
      if (p != null)
      {
          return (int)p.GetValue(this); 
      }
      return 0; 
     }

     public void SetValue(int order, Mrs00285RDOFieldType fieldType, int value)
     {
      PropertyInfo p = this.GetProperty(order, fieldType); 
      if (p != null)
      {
       p.SetValue(this, value); 
      }
     }

     private PropertyInfo GetProperty(int order, Mrs00285RDOFieldType fieldType)
     {
      string type = fieldType == Mrs00285RDOFieldType.EXAM ? "Exam" : "Treat"; 
      return typeof(Mrs00285RDO).GetProperty(string.Format("ExamServiceType{0}{1}Amount", order, type)); 
     }
    }
}
