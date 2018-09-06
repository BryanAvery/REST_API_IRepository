using System.Collections.Generic;

namespace ProductStore.Models
{
    internal interface IProductRepository
    {
        IEnumerable<Product> GetAll();

        Product Get(int id);

        Product Add(Product item);

        void Remove(int id);

        bool Update(Product item);
    }
}