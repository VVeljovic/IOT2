namespace Sensor.Models;
using CsvHelper.Configuration.Attributes;

public class AirQualityData
{
    [Name("Date")]
    public string Date { get; set; }

    [Name("Time")]
    public string Time { get; set; }

    [Name("CO(GT)")]
    public float CO_GT { get; set; }

    [Name("PT08.S1(CO)")]
    public int PT08_S1_CO { get; set; }

    [Name("NMHC(GT)")]
    public float NMHC_GT { get; set; }

    [Name("C6H6(GT)")]
    public float C6H6_GT { get; set; }

    [Name("PT08.S2(NMHC)")]
    public float PT08_S2_NMHC { get; set; }

    [Name("NOx(GT)")]
    public float NOx_GT { get; set; }

    [Name("PT08.S3(NOx)")]
    public float PT08_S3_NOx { get; set; }

    [Name("NO2(GT)")]
    public float NO2_GT { get; set; }

    [Name("PT08.S4(NO2)")]
    public float PT08_S4_NO2 { get; set; }

    [Name("PT08.S5(O3)")]
    public float PT08_S5_O3 { get; set; }

    [Name("T")]
    public float T { get; set; }

    [Name("RH")]
    public float RH { get; set; }

    [Name("AH")]
    public float AH { get; set; }
}
