using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BiorhythmFun.Client.Pages;

public partial class Index
{
    [Inject] public ILocalStorageService? localStorage { get; set; }
    private Set ChartSet = new Set();
    private List<ChartableBase> Current { get; } = new List<ChartableBase>();

    private bool AddPersonDialogIsOpen = false;
    private bool ChangeChartdateDialogIsOpen = false;
    private bool EditPersonDialogIsOpen = false;
    private bool DeletePersonDialogIsOpen = false;
    private string AddName = string.Empty;
    private DateTime AddBirthdate = DateTime.Today;
    private Person? EditPerson { get; set; }
    private string? EditName = string.Empty;
    private DateTime EditBirthdate = DateTime.Today;
    private Person? DeletePerson { get; set; }
    private DateTime Startdate { get; set; }
    private DateTime Enddate { get; set; }
    private DateTime Chartdate { get; set; }
    private bool OldestFirst { get; set; }
    private bool FAQIsOpen { get; set; }

    public void ShowChart(ChartableBase chart)
    {
        if (chart is Group group)
        {
            Current.Clear();
            var ids = group.IDs
                .Select(id => ChartSet.GetPerson(id))
                .Where(p => p is not null)
                .Cast<Person>();
            Current.AddRange(ids);
        }
        else if (chart is Person person)
        {
            Current.Clear();
            Current.Add(person);
        }
        else if (chart is Compatibility compat)
        {
            Current.Clear();
            Current.Add(compat);
        }
        else if (chart is Prediction prediction)
        {
            Current.Clear();
            Current.Add(prediction);
        }
    }

    public void DoShowFAQ(MouseEventArgs e)
    {
        FAQIsOpen = !FAQIsOpen;
    }

    private void DoSortByAge(MouseEventArgs e)
    {
        // do an in-place ordering
        OldestFirst = !OldestFirst;
        SortPeople();
    }

    private void SortPeople()
    {
        ChartSet.People.Sort((x, y) =>
        {
            var diff = x.Birthdate.CompareTo(y.Birthdate);
            return OldestFirst ? -diff : diff;
        });
        ChartSet.Save();
    }

    private void DoAddPerson(MouseEventArgs e)
    {
        AddName = string.Empty;
        AddBirthdate = DateTime.Today;
        AddPersonDialogIsOpen = true;
    }

    private void DoAdd(MouseEventArgs e)
    {
        AddPersonDialogIsOpen = false;
        // save Name and Birthdate to localStorage
        if (!string.IsNullOrWhiteSpace(AddName))
        {
            ChartSet.People.Add(new Person { Name = AddName, Birthdate = AddBirthdate });
            SortPeople();
        }
    }

    private void DoPrevious(MouseEventArgs e)
    {
        Startdate = Startdate.AddMonths(-1);
    }

    private void DoNext(MouseEventArgs e)
    {
        Enddate = Enddate.AddMonths(1);
    }

    private void DoChangeChartDate(MouseEventArgs e)
    {
        Chartdate = Startdate;
        ChangeChartdateDialogIsOpen = true;
    }

    private void DoChange(MouseEventArgs e)
    {
        ChangeChartdateDialogIsOpen = false;
        Startdate = new DateTime(Chartdate.Year, Chartdate.Month, 1);
        Enddate = Startdate.AddMonths(1);
    }

    private void DoEditPerson(Person p)
    {
        if (p is not null)
        {
            EditPerson = p;
            EditName = p.Name;
            EditBirthdate = p.Birthdate;
            EditPersonDialogIsOpen = true;
        }
    }

    private void DoEdit()
    {
        if (EditPerson is not null)
        {
            EditPersonDialogIsOpen = false;
            if (!string.IsNullOrWhiteSpace(EditName)) EditPerson.Name = EditName;
            EditPerson.Birthdate = EditBirthdate;
            SortPeople();
        }
    }

    private void DoDeletePerson(Person p)
    {
        DeletePerson = p;
        DeletePersonDialogIsOpen = true;
    }

    private void DoDelete()
    {
        if (DeletePerson != null)
        {
            DeletePersonDialogIsOpen = false;
            ChartSet.People.Remove(DeletePerson);
            SortPeople();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        Startdate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0, DateTimeKind.Local);
        Enddate = Startdate.AddMonths(1);
        await ChartSet.Load(localStorage);
        ShowChart(ChartSet.Groups.First());
    }
}

public record ChartableBase
{
    public string ID { get; set; } = Guid.NewGuid().ToString();
    public string? Name { get; set; } = string.Empty;
}

public record Person : ChartableBase
{
    private DateTime _birthdate;
    public DateTime Birthdate
    {
        get { return _birthdate.ToLocalTime(); }
        set { _birthdate = DateTime.SpecifyKind(value, DateTimeKind.Local); }
    }
}
public record Group(List<string> IDs) : ChartableBase;
public record Compatibility(string ID1, string ID2) : ChartableBase;
public record Prediction(string MotherID, DateTime ConceptionDate) : ChartableBase;

public class Set
{
    public ILocalStorageService? localStorage { get; set; }

    public List<Person> People { get; set; } = new List<Person>();
    public List<Compatibility> CompatibilityCharts { get; set; } = new List<Compatibility>();
    public List<Prediction> PredictionCharts { get; set; } = new List<Prediction>();
    public List<Group> Groups { get; set; } = new List<Group>();

    public Person? GetPerson(string ID) => People.FirstOrDefault(p => p.ID == ID);

    public void AddGroup(string Name, List<string> IDs) => Groups.Add(new Group(IDs) { Name = Name });

    public void AddCompatibilityChart(string ID1, string ID2)
    {
        var p1 = GetPerson(ID1);
        var p2 = GetPerson(ID2);
        if (p1 is not null && p2 is not null)
        {
            CompatibilityCharts.Add(new Compatibility(ID1, ID2) { Name = $"{p1.Name} - {p2.Name}" });
        }
    }

    public void AddPredictionChart(string MotherID, DateTime ConceptionDate)
    {
        var p = GetPerson(MotherID);
        if (p is not null)
        {
            PredictionCharts.Add(new Prediction(MotherID, ConceptionDate) { Name = $"{p.Name} Prediction" });
        }
    }

    public async Task Load(ILocalStorageService localStorage)
    {
        this.localStorage = localStorage;
        try
        {
            var chartset = await localStorage.GetItemAsync<Set>("set") ?? new Set();

            if (!chartset.People.Any())
            {
                People.AddRange(new[]
                {
                new Person { Name = "Micky Mouse", Birthdate = DateTime.Parse("11/18/1928", null, System.Globalization.DateTimeStyles.AssumeLocal) },
                new Person { Name = "Donald Duck", Birthdate = DateTime.Parse("6/9/1934", null, System.Globalization.DateTimeStyles.AssumeLocal) },
                new Person { Name = "Minnie Mouse", Birthdate = DateTime.Parse("11/18/1928", null, System.Globalization.DateTimeStyles.AssumeLocal) },
            });
                Save();
            }
            else
            {
                People.AddRange(chartset.People);
            }

            if (!chartset.Groups.Any())
            {
                Groups.Add(new Group(People.Select(p => p.ID).ToList()) { Name = "Family" });
                Save();
            }
            else
            {
                Groups.AddRange(chartset.Groups);
            }

            if (!chartset.CompatibilityCharts.Any())
            {
                var top2 = chartset.People.Take(2).Select(p => p.ID).ToList();
                if (top2.Count == 2)
                {
                    AddCompatibilityChart(top2.First(), top2.Last());
                    Save();
                }
            }
            else
            {
                CompatibilityCharts.AddRange(chartset.CompatibilityCharts);
            }

            if (!chartset.PredictionCharts.Any())
            {
                AddPredictionChart(People.Select(p => p.ID).Last(), DateTime.Today);
                Save();
            }
            else
            {
                PredictionCharts.AddRange(chartset.PredictionCharts);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex}");
            await localStorage.ClearAsync();
        }
    }

    public async void Save() => await localStorage.SetItemAsync("set", this);
}