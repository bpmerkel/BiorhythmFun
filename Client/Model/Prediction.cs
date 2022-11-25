namespace BiorhythmFun.Client.Model;

public class Prediction : ChartableBase
{
    public string MotherID { get; set; }
    public DateTime ConceptionDate { get; set; }

    public Prediction(string name, string motherID, DateTime conceptionDate)
    {
        Name = name;
        MotherID = motherID;
        ConceptionDate = conceptionDate;
    }
}
