namespace MiniStyleWebService.Infrastructure.Utils
{
    public static class Enums
    {
        public enum Role
        {
            Admin = 1
        }

        public enum PaymentStatus 
        {
            Pending = 1,
            Completed = 2,
            Failed = 3,
            Refunded = 4,
            Partially_Refunded = 5,
            Cancelled = 6,
            Authorized = 7,
            Captured = 8,
            Charge_back = 9,
            On_Hold = 10
        }

        public enum ShippingStatus
        {
            Not_Shipped = 1,
            Processing = 2,
            Shipped = 3,
            In_Transit = 4,
            Out_for_Delivery = 5,
            Delivered = 6,
            Returned = 7,
            Cancelled = 8,
            Failed_Delivery = 9,
            Awaiting_Pickup = 10
        }
    }
}
