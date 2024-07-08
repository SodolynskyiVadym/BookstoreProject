namespace BookstoreAPI.DapperRequests;

public class PayRequest
{
    public static readonly string GetAllPayments = "SELECT * FROM book_schema.Payments";

    public static readonly string GetPaymentById = "SELECT * FROM book_schema.Payments WHERE id=@Id";

    public static readonly string CreatePayment = "CALL book_schema.spOrder_Upsert(@BooksId, @OrderedQuantity, @Destination, @PhoneNumber, @UserId)";

    public static string DeletePayment(int id)
    {
        return $@"DELETE FROM book_schema.Payments WHERE id={id}";
    }
}