namespace Domain.DataTransferObjects;

public class OrderReportDto
{
    public int LunchSetUnits { get; set; }
    public string UserName { get; set; }
    public string LunchSetPrice { get; set; }
    public string OptionsPrice { get; set; }
    public decimal Summary { get; set; }
    public bool Payment { get; set; }
}