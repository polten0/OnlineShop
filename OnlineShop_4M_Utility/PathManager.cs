using System;

namespace OnlineShop_4M_Utility
{
	public static class PathManager
	{
		public static string ImagePath = @"/images/product/";
        public static string TemplatesPath = @"/templates/";
        public static string SessionCart = "ShoppingCartSession";
        public static string SessionInquiryId = "InquirySession";

        public const string AdminRole = "Admin";
		public const string CustomerRole = "Customer";

		public const string CategoryName = "Category";
		public const string CompanyName = "Company";

		public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusAtWork = "AtWork";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";
        public const string StatusPaid = "Paid";
    }
}

