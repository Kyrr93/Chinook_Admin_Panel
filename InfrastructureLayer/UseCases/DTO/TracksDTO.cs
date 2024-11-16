namespace InfrastructureLayer.UseCases.DTO
{
    public class TracksDTO
    {
        public int TrackId { get; set; }
        public string Name { get; set; } = null!;
        public string AlbumTitle { get; set; } = null!;
        public string MediaTypeName { get; set; } = null!;
        public string? GenreName { get; set; }
        public string? Composer { get; set; }
        public string Duration { get; set; } = "00:00";
        public string SizeInMB { get; set; } = "0 MB";
        public decimal UnitPrice { get; set; }
    }

    public class FindTrackDTO
    {
        public int TrackId { get; set; }

        public string Name { get; set; }

        public int? AlbumId { get; set; }

        public int MediaTypeId { get; set; }

        public int? GenreId { get; set; }

        public string? Composer { get; set; }

        public int Milliseconds { get; set; }

        public int? Bytes { get; set; }

        public decimal UnitPrice { get; set; }
    }

    public class CreateTrackDTO
    {
        public string Name { get; set; }

        public int? AlbumId { get; set; }

        public int MediaTypeId { get; set; }

        public int? GenreId { get; set; }

        public string? Composer { get; set; }

        public int Milliseconds { get; set; }

        public int? Bytes { get; set; }

        public decimal UnitPrice { get; set; }
    }

    public class UpdateTrackDTO
    {
        public int TrackId { get; set; }

        public string Name { get; set; }

        public int? AlbumId { get; set; }

        public int MediaTypeId { get; set; }

        public int? GenreId { get; set; }

        public string? Composer { get; set; }

        public int Milliseconds { get; set; }

        public int? Bytes { get; set; }

        public decimal UnitPrice { get; set; }
    }

    public class DeleteTrackDTO
    {
        public int TrackId { get; set; }
    }
}
