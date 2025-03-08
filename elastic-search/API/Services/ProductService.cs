using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Options;
using API.Settings;
using Elastic.Clients.Elasticsearch.QueryDsl;

namespace API.Services
{
    public class ProductService
    {
        private readonly ElasticsearchClient _client;

        private const string INDEX_NAME = "products";

        // Elasticsearch connection string is injected from appsettings.json
        public ProductService(IOptions<ElasticsearchSettings> options)
        {
            var settings = options.Value;
            _client = new ElasticsearchClient(new Uri(settings.Uri));
        }

        public async Task IndexProductAsync(Product product)
        {
            await _client.IndexAsync(product, idx => idx.Index(INDEX_NAME));
        }

        public async Task<List<Product>> GetProducts()
        {
            var response = await _client.SearchAsync<Product>(s => s
                .Index(INDEX_NAME)
                .Size(1000) // Adjust size as needed
            );

            return [.. response.Hits.Select(hit => hit.Source)];
        }

        public async Task<List<Product>> SearchProducts(string searchTerm)
        {

            var response = await _client.SearchAsync<Product>(s => s
                .Index(INDEX_NAME)
                .Query(q => q
                    .Bool(b => b
                        .Should(
                            bs => bs.Match(m => m.Field(f => f.Id.ToString()).Query(searchTerm)),
                            bs => bs.Match(m => m.Field(f => f.Name).Query(searchTerm))
                        )
                    )
                )
            );

            return response.IsValidResponse
            ? [.. response.Hits.Select(hit => hit.Source)]
            : [];
        }

        public async Task<List<Product>> SearchProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            var response = await _client.SearchAsync<Product>(s => s
                .Index(INDEX_NAME)
                .Query(q => q
                    .Range(range => range
                        .NumberRange(t => t
                            .Field(f => f.Price)
                            .Gte((double?)minPrice)
                            .Lte((double?)maxPrice)
                        )

                    )
                )
            );

            return response.IsValidResponse
            ? [.. response.Hits.Select(hit => hit.Source)]
            : [];
        }

        public async Task<List<Product>> SearchProductWithWildCard(string searchTerm)
        {
            var response = await _client.SearchAsync<Product>(s => s
                .Index(INDEX_NAME)
                .Query(q => q
                    .Wildcard(w => w
                        .Field(f => f.Name)
                        .Value($"*{searchTerm}*")
                    )
                )
            );

            return response.IsValidResponse
            ? [.. response.Hits.Select(hit => hit.Source)]
            : [];
        }

        public async Task<List<Product>> SearchProductWithFuzzy(string searchTerm)
        {
            var response = await _client.SearchAsync<Product>(s => s
                .Index(INDEX_NAME)
                .Query(q => q
                    .Fuzzy(f => f
                        .Field(f => f.Name)
                        .Value(searchTerm)
                        .Fuzziness(new Fuzziness(3)) // Specify the number of characters to be changed to fuzzy match
                    )
                )
            );

            return response.IsValidResponse
            ? [.. response.Hits.Select(hit => hit.Source)]
            : [];
        }

        public async Task<Boolean> DeleteProduct(string id)
        {
            var response = await _client.DeleteAsync<Product>(id, d => d.Index(INDEX_NAME));
            if (!response.IsValidResponse)
            {
                return false;
            }
            return true;
        }
    }
}