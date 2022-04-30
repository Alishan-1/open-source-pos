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

    class FNN_INV_MST_TRValidator : AbstractValidator<FNN_INV_MST_TR>
    {
        public FNN_INV_MST_TRValidator()
        {
            RuleFor(x => x.INV_DATE).NotEmpty();
            RuleFor(x => x.COMPANY_ID).NotEmpty().GreaterThan(0);
            RuleFor(x => x.SUB_COMPANY_ID).NotEmpty().GreaterThan(0);
            RuleFor(x => x.fiscal_year_id).NotEmpty().GreaterThan(0);
            RuleFor(x => x.INV_TYPE).NotEmpty();
            RuleFor(x => x.SALE_TYPE).NotEmpty();
        }
    }

    class FNN_INV_DTL_ITEM_TRValidator : AbstractValidator<FNN_INV_DTL_ITEM_TR>
    {
        public FNN_INV_DTL_ITEM_TRValidator()
        {
            RuleFor(x => x.INV_NO).NotEmpty();
            RuleFor(x => x.ARTICLE_NO).NotEmpty();
            RuleFor(x => x.SCH_CODE).NotEmpty();
            RuleFor(x => x.QTY).NotEmpty().GreaterThan(0);
            RuleFor(x => x.INV_RATE).NotEmpty().GreaterThan(0); 
            RuleFor(x => x.INV_AMOUNT).NotEmpty().GreaterThan(0);
            RuleFor(x => x.COMPANY_ID).NotEmpty().GreaterThan(0);
            RuleFor(x => x.SUB_COMPANY_ID).NotEmpty().GreaterThan(0);
            RuleFor(x => x.fiscal_year_id).NotEmpty().GreaterThan(0);
            RuleFor(x => x.INV_TYPE).NotEmpty();
            RuleFor(x => x.SALE_TYPE).NotEmpty();
        }
    }
}
