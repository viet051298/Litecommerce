using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface IProductAttributeDAL
    {
        List<ProductAttribute> List(int page, int pagesize, string searchValue, int productID);

        int Add(ProductAttribute data);

        int Count(string searchValue, int productID);

        bool Update(ProductAttribute data);

        int Delete(int[] AttributeIDs);

        ProductAttribute Get(int productAttributeId);
    }
}
