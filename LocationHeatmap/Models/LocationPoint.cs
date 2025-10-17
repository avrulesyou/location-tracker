using SQLite;
using System;

namespace LocationHeatmap.Models
{
    [Table("locations")]
    public class LocationPoint
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
