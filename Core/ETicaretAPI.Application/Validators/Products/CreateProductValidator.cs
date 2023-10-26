using ETicaretAPI.Application.ViewModels.Product;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(p=>p.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Ürün adını boş geçmeyiniz")
                .MaximumLength(150)
                .MinimumLength(3)
                .WithMessage("Ürün adını 3 ile 150 karakter arasında giriniz");

            RuleFor(p => p.Stock)
                .NotNull()
                .NotEmpty()
                .WithMessage("Stok bilgisini boş geçmeyiniz")
                .Must(s => s >= 0)
                .WithMessage("Stok bilgisi negatif olamaz");

            RuleFor(p => p.Price)
                .NotNull()
                .NotEmpty()
                .WithMessage("Fiyat bilgisini boş geçmeyiniz")
                .Must(s => s >= 0)
                .WithMessage("Fiyat bilgisi negatif olamaz");

        }
    }
}
