namespace TradeSuite.Application.Suppliers.Dtos.Responses
{
    public class SupplierResponseDto
    {
        public Guid Id { get; set; }

        public bool Active { get; set; }

        public required string Address { get; set; }

        public required string Email { get; set; }

        public bool IsDeleted { get; set; }

        public required string Name { get; set; }

        public required string Phone { get; set; }
    }
}