using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;

namespace eTicaretUygulamasi.Mvc.App.Data.Entities
{
    public class ProductEntity
    {
    
        public int Id { get; set; }

       
        public int SellerId { get; set; }
        public UserEntity Seller { get; set; } 

     
        public int CategoryId { get; set; }
        public CategoryEntity Category { get; set; } = null!;

   
        public string DDName { get; set; }


        public decimal Price { get; set; }

   
        public string Details { get; set; }


        public byte StockAmount { get; set; }

  
        public DateTime CreatedAt { get; set; }

        
        public bool Enabled { get; set; } = true;
        
        public virtual ICollection<ProductImageEntity> Images { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string ImageUrl
        {
            get
            {
                // Eğer veritabanından geçerli bir resim gelmişse (sahte/kırık yol değilse) onu kullan
                if (Images != null && Images.Any())
                {
                    var url = Images.First().Url;
                    if (!url.StartsWith("/images/products/") && !url.StartsWith("wwwroot/"))
                    {
                        return url;
                    }
                }

                // Yoksa kategoriye göre webden (Unsplash) alakalı gerçek bir görsel getir
                switch (CategoryId)
                {
                    case 1: return "https://images.unsplash.com/photo-1498049794561-7780e7231661?w=600&q=80"; // Electronics
                    case 2: return "https://images.unsplash.com/photo-1441984904996-e0b6ba687e04?w=600&q=80"; // Clothing
                    case 3: return "https://images.unsplash.com/photo-1513694203232-719a280e022f?w=600&q=80"; // Home
                    case 4: return "https://images.unsplash.com/photo-1544947950-fa07a98d237f?w=600&q=80"; // Books
                    case 5: return "https://images.unsplash.com/photo-1505751172876-fa1923c5c528?w=600&q=80"; // Health
                    case 6: return "https://images.unsplash.com/photo-1517836357463-d25dfeac3438?w=600&q=80"; // Sports
                    case 7: return "https://images.unsplash.com/photo-1566576912321-d58ddd7a6088?w=600&q=80"; // Toys
                    case 8: return "https://images.unsplash.com/photo-1492144534655-ae79c964c9d7?w=600&q=80"; // Automotive
                    case 9: return "https://images.unsplash.com/photo-1555041469-a586c61ea9bc?w=600&q=80"; // Furniture
                    case 10: return "https://images.unsplash.com/photo-1488459716781-31db52582fe9?w=600&q=80"; // Food
                    default: return "https://images.unsplash.com/photo-1472851294608-062f824d29cc?w=600&q=80"; // E-commerce generic
                }
            }
        }
        
    }
    internal class ProductEntityConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.SellerId).IsRequired();
            builder.Property(e => e.CategoryId).IsRequired();
            builder.Property(e => e.DDName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(e => e.Details).HasMaxLength(1000);
            builder.Property(e => e.StockAmount).IsRequired();
            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.Enabled).IsRequired().HasDefaultValue(true);

            builder.HasOne(d => d.Seller)
                .WithMany()
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(d => d.Category)
                .WithMany()
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
