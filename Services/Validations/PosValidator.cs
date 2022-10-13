using System;
using System.Collections.Generic;
using System.Text;
using Models;
using FluentValidation;

namespace Services.Validations
{
    class PosValidator : AbstractValidator<POS>
    {
        public PosValidator()
        {
            RuleFor(x => x.Task).NotEmpty();

        }
    }

    class InvoiceMasterValidator : AbstractValidator<InvoiceMaster>
    {
        public InvoiceMasterValidator()
        {
            RuleFor(x => x.InvoiceDate).NotEmpty();
            RuleFor(x => x.CompanyID).NotEmpty().GreaterThan(0);
            RuleFor(x => x.FiscalYearID).NotEmpty().GreaterThan(0);
            RuleFor(x => x.InvoiceType).NotEmpty();
        }
    }

    class InvoiceDetailItemsValidator : AbstractValidator<InvoiceDetailItems>
    {
        public InvoiceDetailItemsValidator()
        {
            RuleFor(x => x.InvoiceNo).NotEmpty();
            RuleFor(x => x.ItemDescription).NotEmpty();
            RuleFor(x => x.ItemCode).NotEmpty();
            RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0);
            RuleFor(x => x.InvoiceRate).NotEmpty().GreaterThan(0); 
            RuleFor(x => x.InvoiceValue).NotEmpty().GreaterThan(0);
            RuleFor(x => x.CompanyID).NotEmpty().GreaterThan(0);
            RuleFor(x => x.FiscalYearID).NotEmpty().GreaterThan(0);
            RuleFor(x => x.InvoiceType).NotEmpty();
        }
    }

    class PosItemValidator : AbstractValidator<PosItem>
    {
        public PosItemValidator()
        {
            RuleFor(x => x.SalePrice).NotEmpty().GreaterThan(0);
            RuleFor(x => x.CompanyID).NotEmpty().GreaterThan(0);
            RuleFor(x => x.CreateUser).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Description).NotEmpty().MinimumLength(4);
            RuleFor(x => x.CustomCode).NotEmpty().MinimumLength(4);
        }
    }
}
