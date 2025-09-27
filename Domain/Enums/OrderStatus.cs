using System.Text.Json.Serialization;

namespace Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus
    {
        PendingPayment, // Aguardando pagamento
        PaymentFailed,  // Pagamento falhou
        Paid,           // Pagamento realizado
        Processing,     // Pedido em processamento
        Released,       // Pedido liberado para download ou uso
        Cancelled,      // Pedido cancelado pelo usuário ou sistema
        Refunded        // Pedido reembolsado
    }
}
