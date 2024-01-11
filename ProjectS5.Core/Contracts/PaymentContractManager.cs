using ProjectS5.Core.Contracts.Models;

namespace ProjectS5.Core.Contracts;

public class PaymentContractManager : IContractManager<PaymentContractModel>
{
    public string GenerateCustomContract(PaymentContractModel model)
    {
        return string.Empty;
    }
}
