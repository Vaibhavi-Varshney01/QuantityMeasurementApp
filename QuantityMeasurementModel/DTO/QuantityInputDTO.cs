using System.ComponentModel.DataAnnotations;
using QuantityMeasurementModel.DTO;

namespace QuantityMeasurementModel.DTO;

public class QuantityInputDTO
{
    public QuantityDTO? ThisQuantityDTO { get; set; }
    public QuantityDTO? ThatQuantityDTO { get; set; }
}