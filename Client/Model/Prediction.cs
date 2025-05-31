namespace BiorhythmFun.Client.Model;

/// <summary>
/// Represents a prediction as a chartable item.
/// </summary>
public class Prediction : ChartableBase
{
    /// <summary>
    /// Gets or sets the ID of the mother.
    /// </summary>
    public string MotherID { get; set; }

    /// <summary>
    /// Gets or sets the conception date.
    /// </summary>
    public DateTime ConceptionDate { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Prediction"/> class.
    /// </summary>
    /// <param name="name">The name of the prediction.</param>
    /// <param name="motherID">The ID of the mother.</param>
    /// <param name="conceptionDate">The conception date.</param>
    public Prediction(string name, string motherID, DateTime conceptionDate)
    {
        Name = name;
        MotherID = motherID;
        ConceptionDate = conceptionDate;
    }
}