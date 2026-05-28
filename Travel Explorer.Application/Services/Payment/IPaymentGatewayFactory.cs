namespace Travel_Explorer.Application.Services.Payment
{
    public interface IPaymentGatewayFactory
    {
        IPaymentGateway GetGateway(string providerName);
    }
}
