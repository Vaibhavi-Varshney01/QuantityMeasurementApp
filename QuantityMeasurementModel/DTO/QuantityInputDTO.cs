using System.ComponentModel.DataAnnotations;
using QuantityMeasurementModel.DTO;

namespace QuantityMeasurementWebAPI;

public class QuantityInputDTO
{
    public QuantityDTO? ThisQuantityDTO { get; set; }
    public QuantityDTO? ThatQuantityDTO { get; set; }
}