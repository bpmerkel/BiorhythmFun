namespace BiorhythmFun.Client.Model;

public class Person : ChartableBase
{
    private DateTime _birthdate;
    public DateTime Birthdate
    {
        get { return _birthdate.ToLocalTime(); }
        set { _birthdate = DateTime.SpecifyKind(value, DateTimeKind.Local); }
    }

    public Person(string name, DateTime birthdate)
    {
        Name = name;
        Birthdate = birthdate;
    }
}
