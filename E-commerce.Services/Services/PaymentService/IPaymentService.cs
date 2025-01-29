using E_commerce.Core.DTO;
using Stripe.Checkout;

namespace E_commerce.Services.Services.PaymentService
{
	public interface IPaymentService
	{
		Task<Session> CreateCheckoutSessionAsync(List<ProductQuantityDTO> products);
	}
}