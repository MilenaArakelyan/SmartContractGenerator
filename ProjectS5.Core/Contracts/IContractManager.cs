namespace ProjectS5.Core.Contracts;

public interface IContractManager<T> where T : class
{
    string GenerateCustomContract(T model);
}