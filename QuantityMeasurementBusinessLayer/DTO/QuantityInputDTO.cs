using System.ComponentModel.DataAnnotations;
using QuantityMeasurementBusinessLayer.DTO;

namespace QuantityMeasurementWebAPI;

public class QuantityInputDTO
{
    public QuantityDTO? ThisQuantityDTO { get; set; }
    public QuantityDTO? ThatQuantityDTO { get; set; }
}