namespace BiorhythmFun.Client.Model;

/// <summary>
/// Represents a person as a chartable item.
/// </summary>
public class Person : ChartableBase
{
    private DateTime _birthdate;

    /// <summary>
    /// Gets or sets the birthdate of the person.
    /// </summary>
    public DateTime Birthdate
    {
        get { return _birthdate.ToLocalTime(); }
        set { _birthdate = DateTime.SpecifyKind(value, DateTimeKind.Local); }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Person"/> class.
    /// </summary>
    /// <param name="name">The name of the person.</param>
    /// <param name="birthdate">The birthdate of the person.</param>
    public Person(string name, DateTime birthdate)
    {
        Name = name;
        Birthdate = birthdate;
    }
}
