using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sensor.Models
{
    [Table("air_quality")]
    public class AirQualityData
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public float CO_GT { get; set; }
        public float PT08_S1_CO { get; set; }
        public float NMHC_GT { get; set; }
        public float C6H6_GT { get; set; }
        public float PT08_S2_NMHC { get; set; }
        public float NOx_GT { get; set; }
        public float PT08_S3_NOx { get; set; }
        public float NO2_GT { get; set; }
        public float PT08_S4_NO2 { get; set; }
        public float PT08_S5_O3 { get; set; }
        public float T { get; set; }
        public float RH { get; set; }
        public float AH { get; set; }
    }
}
