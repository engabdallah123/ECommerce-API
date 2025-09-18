// DTO بعد التعديل
public class OrderItemCreateDTO
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

 
    public decimal Total => Quantity * UnitPrice;


}
