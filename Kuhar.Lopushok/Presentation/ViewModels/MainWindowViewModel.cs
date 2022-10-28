using Kuhar.Lopushok.Domain.Entities;
using Kuhar.Lopushok.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kuhar.Lopushok.Presentation.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public List<Product> Products { get; set; }

        public MainWindowViewModel()
        {
            using (ApplicationDbContext context = new())
            {
                Products = context.Products
                    .Include(t => t.ProductType)
                    .Include(pb => pb.ProductMaterials)
                    .ThenInclude(m => m.Material)
                    .ToList();
            }
        }

    }
}
