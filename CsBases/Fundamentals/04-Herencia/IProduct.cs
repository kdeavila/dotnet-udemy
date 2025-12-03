namespace CsBases.Fundamentals;

interface IProduct
{
  void ApplyDiscount(decimal percentage);
  string GetDescription();
}