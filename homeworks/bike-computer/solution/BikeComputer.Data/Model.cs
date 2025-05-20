namespace BikeComputer.Data;

public class Bike
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string SerialNumberBikeComputer { get; set; }
    public required int Circumference_mm { get; set; }

    public DateTimeOffset ViewedAt { get; set; }

    public List<Ride> Rides { get; set; } = [];
}

public class Ride
{
    public int Id { get; set; }

    public int BikeId { get; set; }
    public Bike? Bike { get; set; }

    public required string Title { get; set; }

    public DateTimeOffset UploadedAt { get; set; }

    public float RideDuration_s { get; set; }
    public float RideDistance_m { get; set; }
    public float AvgSpeed_kmh { get; set; }
    public int NumberOfStops { get; set; }
    public float TotalStopTime_s { get; set; }
}