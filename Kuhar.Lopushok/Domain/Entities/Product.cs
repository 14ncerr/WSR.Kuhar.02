using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Kuhar.Lopushok.Domain.Entities
{
    public partial class Product
    {
        public Product()
        {
            ProductCostHistories = new HashSet<ProductCostHistory>();
            ProductMaterials = new HashSet<ProductMaterial>();
            ProductSales = new HashSet<ProductSale>();
        }

        public string? _image;

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int? ProductTypeId { get; set; }
        public string ArticleNumber { get; set; } = null!;
        public string? Description { get; set; }
        public string? Image 
        {
            get => (_image == string.Empty) || (_image == null)
                ? $"..\\Resources\\picture.png"
                : $"..\\Resources{_image}";

            set => _image = value;
        }
        public int? ProductionPersonCount { get; set; }
        public int? ProductionWorkshopNumber { get; set; }
        public decimal MinCostForAgent { get; set; }


        public virtual ProductType? ProductType { get; set; }
        public virtual ICollection<ProductCostHistory> ProductCostHistories { get; set; }
        public virtual ICollection<ProductMaterial> ProductMaterials { get; set; }
        public virtual ICollection<ProductSale> ProductSales { get; set; }

        public decimal TotalCost 
        { 
            get
            {
                if (ProductMaterials.Count == 0)
                    return MinCostForAgent;

                var totalCost = 0M;

                foreach (var item in ProductMaterials)
                {
                    totalCost += Math.Ceiling((decimal)item.Count) * item.Material.Cost;
                }

                return totalCost;
            }
        }

        public string MaterialStringRepresentation 
        { 
            get
            {
                if (ProductMaterials.Count == 0) 
                    return "Нет материалов";

                StringBuilder stringBuilder = new();

                stringBuilder.Append("Материалы: ");

                foreach (var item in ProductMaterials)
                {
                    stringBuilder.Append(item.Material.Title);
                    stringBuilder.Append(", ");
                }

                stringBuilder.Remove(stringBuilder.Length - 2, 2);
                return stringBuilder.ToString();
            }
          
        }

    }
}
