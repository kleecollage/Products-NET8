namespace Web.Models;

public class WebpayRespuesta
{
  public string Vci { get; set; }
  public int Amount { get; set; }
  public string Status { get; set; }
  public string Buy_Order { get; set; }
  public string Session_Id { get; set; }
  public WebpayRespuestaCardDetail Card_Detail { get; set; }
  public string Accounting_Date { get; set; }
  public string Transaction_Date { get; set; }
  public string Authorization_Code { get; set; }
  public string Payment_Type_Code { get; set; }
  public string Response_Code { get; set; }
  public string Installments_Number { get; set; }
}

public class WebpayRespuestaCardDetail {
  public string Card_Number { get; set; }
}