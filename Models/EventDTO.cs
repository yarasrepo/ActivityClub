namespace ActivityClubAPIs.Models
{
    public class EventDTO
    {
        public string? DateFrom { get; set; }

        public string? DateTo { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public double? Cost { get; set; }

        public string? Destination { get; set; }

        public string? Status { get; set; }

        public int? CategoryId { get; set; }
    }
}
