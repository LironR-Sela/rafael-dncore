using Grpc.Net.Client;
using Product;
using ShoppingCartGrpcService;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingCart.Client
{
    class Program
    {
        static ProductCartService.ProductCartServiceClient cartClient;
        static async Task Main(string[] args)
        {

            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //var greetingClient = new Greeter.GreeterClient(channel);

            //var reply = greetingClient.SayHello(new HelloRequest
            //{
            //    Name = "Liron"
            //});

            cartClient = new ProductCartService.ProductCartServiceClient(channel);
            //await AddToCartUnary();
            //await AddMultipleProductsToCart();
            //await  GetCart();
            await CalculateVAT();


            Console.ReadKey();
        }

        private static async Task CalculateVAT()
        {
            var stream = cartClient.CalcVATforProduct();


            //Listen
            var responseReader = Task.Run(async () =>
            {

                while (await stream.ResponseStream.MoveNext(CancellationToken.None))
                {
                    var result = stream.ResponseStream.Current.Product;
                    Console.WriteLine($"Id: {result.Title}, updated price: {result.Price}");
                }

            });


            //Send
            Product.Product[] products =
            {
                new Product.Product { Price = 10, Title = "Product" },
                new Product.Product { Price = 10, Title = "Product" },
                new Product.Product { Price = 10, Title = "Product" },
                new Product.Product { Price = 10, Title = "Product" },
                new Product.Product { Price = 10, Title = "Product" },
                new Product.Product { Price = 10, Title = "Product" },
                new Product.Product { Price = 10, Title = "Product" },
                new Product.Product { Price = 10, Title = "Product" },
                new Product.Product { Price = 10, Title = "Product" },
                new Product.Product { Price = 10, Title = "Product" },
                new Product.Product { Price = 10, Title = "Product" },
                new Product.Product { Price = 10, Title = "Product" },
                new Product.Product { Price = 10, Title = "Product" },
                new Product.Product { Price = 10, Title = "Product" },
                new Product.Product { Price = 10, Title = "Product" },
                new Product.Product { Price = 10, Title = "Product" },
                new Product.Product { Price = 10, Title = "Product" },
                new Product.Product { Price = 10, Title = "Product" },
                new Product.Product { Price = 10, Title = "Product" },
                new Product.Product { Price = 10, Title = "Product" },
            };

            foreach (var item in products)
            {
                Console.WriteLine("Client sending...");
                await stream.RequestStream.WriteAsync(new ClacVATforProductRequest
                {
                    Product = item
                });

                Thread.Sleep(1000);
            }

            await stream.RequestStream.CompleteAsync();
            await responseReader;
        }

        private static async Task GetCart()
        {
            var response = cartClient.GetCart(new GetCartRequest() { });
            while (await response.ResponseStream.MoveNext(CancellationToken.None))
            {
                Console.WriteLine(response.ResponseStream.Current.Product.Id);
            }
        }

        private static async Task AddMultipleProductsToCart()
        {
            var carttProduct = new Product.Product
            {
                Title = "Milk",
                Price = 10
            };


            var request = new AddMultipleProductsToCartRequest
            {
                Product = carttProduct
            };


            var stream = cartClient.AddMultipleProductsToCart();

            foreach (var item in Enumerable.Range(1, 10))
            {
                await stream.RequestStream.WriteAsync(request);
                Thread.Sleep(2000);
            }

            await stream.RequestStream.CompleteAsync();
            var response = await stream.ResponseAsync;
            Console.WriteLine($"Count: {response.Status}");
        }

        private static async Task AddToCartUnary()
        {
            try
            {
                var product = new Product.Product();
                product.Title = "Pizza";
                product.Price = 107;

                var response = await cartClient.AddToCartAsync(new AddToCartRequest
                {
                    Product = product
                });

                Console.WriteLine($"New id: {response.Product.Id.ToLower()}");

            }
            catch (Exception ex)
            {

            }
        }
    }
}
