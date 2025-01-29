using Stripe;
using Stripe.Checkout;
using E_commerce.Core.DTO;

namespace E_commerce.Services.Services.PaymentService
{
	public class PaymentService : IPaymentService
	{
		public PaymentService()
		{
			StripeConfiguration.ApiKey = "your_stripe_secret_key";  
		}

		public async Task<Session> CreateCheckoutSessionAsync(List<ProductQuantityDTO> products)
		{
			var lineItems = new List<SessionLineItemOptions>();

			foreach (var product in products)
			{
				lineItems.Add(new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = (long)(product.Price * 100),  
						Currency = "usd",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = product.ProductName,
						},
					},
					Quantity = product.Quantity,
				});
			}

			var options = new SessionCreateOptions
			{
				PaymentMethodTypes = new List<string> { "card" },
				LineItems = lineItems,
				Mode = "payment",
				SuccessUrl = "https://localhost:4200/success", 
				CancelUrl = "https://localhost:4200/cancel",   
			};

			var service = new SessionService();
			return await service.CreateAsync(options);
		}
	}
}