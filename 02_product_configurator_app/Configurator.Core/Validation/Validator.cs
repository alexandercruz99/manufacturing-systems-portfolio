using Configurator.Core.Enums;
using Configurator.Core.Models;

namespace Configurator.Core.Validation;

public static class Validator
{
    private const decimal MinDimension = 6.0m;
    private const decimal MaxDimension = 120.0m;
    private const int MinQuantity = 1;
    private const int MaxQuantity = 1000;

    private static readonly HashSet<ConfigOption> ValidOptions = new()
    {
        ConfigOption.None,
        ConfigOption.Coating,
        ConfigOption.StainlessFasteners,
        ConfigOption.HighEfficiencyFins,
        ConfigOption.ExpressBuild
    };

    public static (bool IsValid, List<string> Errors) Validate(ConfiguratorRequest request)
    {
        var errors = new List<string>();

        if (request.WidthIn < MinDimension || request.WidthIn > MaxDimension)
        {
            errors.Add($"Width must be between {MinDimension} and {MaxDimension} inches.");
        }

        if (request.HeightIn < MinDimension || request.HeightIn > MaxDimension)
        {
            errors.Add($"Height must be between {MinDimension} and {MaxDimension} inches.");
        }

        if (request.DepthIn < MinDimension || request.DepthIn > MaxDimension)
        {
            errors.Add($"Depth must be between {MinDimension} and {MaxDimension} inches.");
        }

        if (request.Quantity < MinQuantity || request.Quantity > MaxQuantity)
        {
            errors.Add($"Quantity must be between {MinQuantity} and {MaxQuantity}.");
        }

        if (request.Options == null || request.Options.Count == 0)
        {
            errors.Add("At least one option must be specified (use None if no options).");
        }
        else
        {
            var invalidOptions = request.Options.Where(o => !ValidOptions.Contains(o)).ToList();
            if (invalidOptions.Any())
            {
                errors.Add($"Invalid option values: {string.Join(", ", invalidOptions)}. Valid options are: None, Coating, StainlessFasteners, HighEfficiencyFins, ExpressBuild");
            }
        }

        return (errors.Count == 0, errors);
    }
}

