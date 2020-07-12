using Grpc.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;
using Product;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingCartGrpcService.Services
{
    public class CartDataService : ProductCartService.ProductCartServiceBase
    {
        private static MongoClient mongoClient = new MongoClient("connection string");
        private static IMongoDatabase mongoDatabase = mongoClient.GetDatabase("demo-shopping-site-db");
        private static IMongoCollection<BsonDocument> mongoCollection = mongoDatabase.GetCollection<BsonDocument>("cart");


        public override async Task CalcVATforProduct(IAsyncStreamReader<ClacVATforProductRequest> requestStream, IServerStreamWriter<ClacVATforProductResponse> responseStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                var updatePrice = requestStream.Current.Product.Price * 1.17;
                await responseStream.WriteAsync(new ClacVATforProductResponse
                {
                    Product = new Product.Product
                    {
                        Title = requestStream.Current.Product.Title,
                        Price = updatePrice
                    }
                });

                Console.WriteLine($"Serber is sending: {updatePrice}");

                Thread.Sleep(2000);
            }
        }

        public override async Task<AddToCartResponse> AddToCart(AddToCartRequest request, ServerCallContext context)
        {
            var product = request.Product;
            var newProduct = AddToCartDB(product);

            return new AddToCartResponse
            {
                Product = newProduct
            };
        }


        public override async Task<AddMultipleProductsToCartResponse> AddMultipleProductsToCart(IAsyncStreamReader<AddMultipleProductsToCartRequest> requestStream, ServerCallContext context)
        {
            int counter = 0;
            while (await requestStream.MoveNext())
            {
                var prosuct = AddToCartDB(requestStream.Current.Product);
                Console.WriteLine("product Added!");
                counter++;
            }

            return new AddMultipleProductsToCartResponse
            {
                Status = counter
            };
        }


        public override async Task GetCart(GetCartRequest request, IServerStreamWriter<GetCartResponse> responseStream, ServerCallContext context)
        {
            var filter = new FilterDefinitionBuilder<BsonDocument>().Empty;
            var result = mongoCollection.Find(filter);

            foreach (var item in result.ToList())
            {
                await responseStream.WriteAsync(new GetCartResponse()
                {
                    Product = new Product.Product
                    {
                        Id = item.GetValue("_id").ToString(),
                        Price = item.GetValue("price").AsDouble,
                        Title = item.GetValue("title").ToString()
                    }
                });

                Thread.Sleep(2000);
            }
        }

        public Product.Product AddToCartDB(Product.Product product)
        {
            try
            {
                BsonDocument doc = new BsonDocument("title", product.Title)
                    .Add("price", product.Price);

                mongoCollection.InsertOne(doc);

                var id = doc.GetValue("_id").ToString();
                product.Id = id;

            }
            catch (Exception ex)
            {
                return null;
            }

            return product;

        }
    }
}
