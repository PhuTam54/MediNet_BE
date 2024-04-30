namespace MediNet_BE.Dto.Payments.PayPal;

public class PaymentInformationModel
{
	public string OrderId { get; set; }
	public string OrderType { get; set; }
    public double Amount { get; set; }
    public string OrderDescription { get; set; }
    public string Name { get; set; }
}