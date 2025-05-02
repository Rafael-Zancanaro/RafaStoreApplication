using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using RafaStore.Core.DomainObjects;

namespace RafaStore.WebApp.MVC.Extensions;

public class CpfAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        => Cpf.Validar(value.ToString()) ? ValidationResult.Success : new ValidationResult("CPF em formato inválido");
}

public class CpfAttributeAdapter(CpfAttribute attribute, IStringLocalizer stringLocalizer)
    : AttributeAdapterBase<CpfAttribute>(attribute, stringLocalizer)
{
    public override void AddValidation(ClientModelValidationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        MergeAttribute(context.Attributes, "data-val", "true");
        MergeAttribute(context.Attributes, "data-val-cpf", GetErrorMessage(context));
    }
    
    public override string GetErrorMessage(ModelValidationContextBase validationContext)
        => "CPF em formato inválido";
}

public class CpfValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
{
    private readonly IValidationAttributeAdapterProvider _provider = new ValidationAttributeAdapterProvider();

    public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        => attribute is CpfAttribute cpfAttribute
            ? new CpfAttributeAdapter(cpfAttribute, stringLocalizer)
            : _provider.GetAttributeAdapter(attribute, stringLocalizer);
}