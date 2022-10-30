using Kuhar.Lopushok.Domain.Entities;
using Kuhar.Lopushok.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Kuhar.Lopushok.Presentation.ViewModels
{
    public class AddNewProductViewModel : ViewModelBase
    {
        public List<ProductType> ProductTypeList { get; }
        public Product NewProduct { get; set; }

        private void AddNewProduct()
        {
            using (ApplicationDbContext context = new())
            {
                if (!context.Products.Any(pr => pr.Title == NewProduct.Title))
                {               
                    Product product = new()
                    {
                        Title = NewProduct.Title,
                        ProductTypeId = NewProduct.ProductTypeId,
                        ArticleNumber = NewProduct.ArticleNumber,
                        Description = NewProduct.Description,
                        Image = NewProduct.Image,
                        ProductionPersonCount = NewProduct.ProductionPersonCount,
                        ProductionWorkshopNumber = NewProduct.ProductionWorkshopNumber,
                        MinCostForAgent = NewProduct.MinCostForAgent
                    };
                    context.Products.Add(product);
                    context.SaveChanges();
                    MessageBox.Show($"Продукт {NewProduct.Title} добавлен!");
                }
                else
                    MessageBox.Show($"Продукт {NewProduct.Title} уже существует!");
            }
        }
    }
}
