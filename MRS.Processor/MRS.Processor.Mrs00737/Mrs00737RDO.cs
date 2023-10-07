using System.Collections.Generic;
public class Mrs00737RDO
{
    public string JSON_PAY_FORM_CODE { get; set; }

    public Dictionary<string,decimal> DIC_PAY_FORM_CODE { get; set; }

    public string TREATMENT_CODE { get; set; }

    public string PATIENT_CODE { get; set; }

    public string HEIN_CARD_NUMBER { get; set; }

    public string PATIENT_NAME { get; set; }

    public string REP_TRANSACTION_CODE { get; set; }

    public string BILL_TIME_STR { get; set; }

    public decimal? TOTAL_REPAY_AMOUNT { get; set; }

    public decimal? BILL_AMOUNT_VP { get; set; }

    public decimal? BILL_AMOUNT_DV { get; set; }

    public decimal? BILL_AMOUNT_AN { get; set; }

    public decimal? DIFF { get; set; }

    public string IN_TIME_STR { get; set; }

    public string BILL_CASHIER_LOGINNAME { get; set; }

    public string BILL_CASHIER_USERNAME { get; set; }

    public string PAY_FORM_SERVICE_CODE { get; set; }

    public string PAY_FORM_NORMAL_CODE { get; set; }

    public string PAY_FORM_AN_CODE { get; set; }

    public string PAY_FORM_SERVICE_NAME { get; set; }

    public string PAY_FORM_NORMAL_NAME { get; set; }

    public string PAY_FORM_AN_NAME { get; set; }

    public long? TRANSACTION_DATE { get; set; }

    public long? IN_TIME { get; set; }

    public string DEPARTMENT_NAME { get; set; }

}

public class PAY_FORM_AGGREGATE
{
    public string PAY_FORM_CODE { get; set; }
    public string PAY_FORM_NAME { get; set; }

    public decimal? AMOUNT { get; set; }

    public bool IS_HAS_DIFF { get; set; }
}