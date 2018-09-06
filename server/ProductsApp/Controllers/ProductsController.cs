using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductStore.Controllers
{
    public class ProductsController : ApiController
    {
        private static readonly IProductRepository repository = new ProductRepository();

        [HttpGet]
        public IEnumerable<Product> Products()
        {
            return repository.GetAll();
        }

        [HttpGet]
        public Product Product(int id)
        {
            Product item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }

        [HttpPost]
        public HttpResponseMessage PostProduct(Product item)
        {
            item = repository.Add(item);
            var response = Request.CreateResponse<Product>(HttpStatusCode.Created, item);

            string uri = Url.Link("DefaultApi", new { id = item.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        [HttpPut]
        public void product(int id, Product product)
        {
            product.Id = id;
            if (!repository.Update(product))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpDelete]
        public void product(int id)
        {
            repository.Remove(id);
        }
    }
}