using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

namespace BiorhythmFun.Client.Pages;

public partial class Index
{
    [Inject] public ILocalStorageService LocalStorage { get; set; }
    private readonly Set ChartSet = new();
    private ChartableBase Current;

    private bool FAQIsOpen = false;
    private bool AddPersonDialogIsOpen = false;
    private bool EditPersonDialogIsOpen = false;
    private bool DeletePersonDialogIsOpen = false;
    private string AddName = string.Empty;
    private DateTime AddBirthdate = DateTime.Today;
    private Person? DeletePerson;
    private Person? EditPerson;
    private string? EditName = string.Empty;
    private DateTime EditBirthdate = DateTime.Today;

    private bool AddGroupDialogIsOpen = false;
    private bool DeleteGroupDialogIsOpen = false;
    private bool EditGroupDialogIsOpen = false;
    private Group? DeleteGroup;
    private Group? EditGroup;
    // use AddName and EditName from above
    
    private bool AddCompatibilityChartDialogIsOpen = false;
    private bool EditCompatibilityDialogIsOpen = false;
    private bool DeleteCompatibilityDialogIsOpen = false;
    private Person? AddPerson1;
    private Person? AddPerson2;
    private Compatibility? DeleteCompatibility;
    private Compatibility? EditCompatibility;
    private Person? EditPerson1;
    private Person? EditPerson2;

    private bool AddPredictionChartDialogIsOpen = false;
    private bool EditPredictionDialogIsOpen = false;
    private bool DeletePredictionDialogIsOpen = false;
    private Person? AddMother;
    private DateTime AddConceptionDate = DateTime.Today;
    private Prediction? DeletePrediction;
    private Prediction? EditPrediction;
    private Person? EditMother;
    private DateTime EditConceptionDate = DateTime.Today;

    private bool ChangeChartdateDialogIsOpen = false;

    private DateTime Startdate;
    private DateTime Enddate;
    private DateTime Chartdate;

    public void DoShowFAQ(MouseEventArgs e) => FAQIsOpen = !FAQIsOpen;

    private void DoAddPerson(MouseEventArgs e)
    {
        AddName = string.Empty;
        AddBirthdate = DateTime.Today;
        AddPersonDialogIsOpen = true;
    }

    private void DoAddPersonObject(MouseEventArgs e)
    {
        AddPersonDialogIsOpen = false;
        // save Name and Birthdate to localStorage
        if (!string.IsNullOrWhiteSpace(AddName))
        {
            ChartSet.AddPerson(new Person { Name = AddName, Birthdate = AddBirthdate });
        }
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

    private void DoEditPersonObject()
    {
        if (EditPerson is not null)
        {
            EditPersonDialogIsOpen = false;
            if (!string.IsNullOrWhiteSpace(EditName)) EditPerson.Name = EditName;
            EditPerson.Birthdate = EditBirthdate;
            ChartSet.Save();
        }
    }

    private void DoDeletePerson(Person p)
    {
        DeletePerson = p;
        DeletePersonDialogIsOpen = true;
    }

    private void DoDeletePersonObject()
    {
        if (DeletePerson is not null)
        {
            DeletePersonDialogIsOpen = false;
            ChartSet.RemovePerson(DeletePerson);
        }
    }

    private void DoAddGroup(MouseEventArgs e)
    {
        AddName = string.Empty;
        AddGroupDialogIsOpen = true;
    }

    private void DoAddGroupObject(MouseEventArgs e)
    {
        AddGroupDialogIsOpen = false;
    }

    private void DoEditGroup(Group group)
    {
        if (group is not null)
        {
            EditGroup = group;
            EditName = group.Name;
            EditGroupDialogIsOpen = true;
        }
    }

    private void EditGroupObject()
    {
        if (EditGroup is not null)
        {
            EditGroupDialogIsOpen = false;
            if (!string.IsNullOrWhiteSpace(EditName)) EditGroup.Name = EditName;
            ChartSet.Save();
        }
    }

    private void DoDeleteGroup(Group group)
    {
        DeleteGroup = group;
        DeleteGroupDialogIsOpen = true;
    }

    private void DeleteGroupObject()
    {
        if (DeleteGroup is not null)
        {
            DeleteGroupDialogIsOpen = false;
            ChartSet.RemoveGroup(DeleteGroup);
        }
    }

    private void DoAddCompatibilityChart(MouseEventArgs e)
    {
        AddPerson1 = null;
        AddPerson2 = null;
        AddCompatibilityChartDialogIsOpen = true;
    }

    private void AddCompatibilityObject(MouseEventArgs e)
    {
        AddCompatibilityChartDialogIsOpen = false;
        if (AddPerson1 is not null && AddPerson2 is not null)
        {
            ChartSet.AddCompatibilityChart(AddPerson1.ID, AddPerson2.ID);
        }
    }

    private void DoEditCompatibility(Compatibility compat)
    {
        EditCompatibility = compat;
        EditPerson1 = ChartSet.GetPerson(compat.ID1);
        EditPerson2 = ChartSet.GetPerson(compat.ID2);
        EditCompatibilityDialogIsOpen = true;
    }

    private void EditCompatibilityObject()
    {
        if (EditCompatibility is not null && EditPerson1 is not null && EditPerson2 is not null)
        {
            //EditCompatibility.ID1 = EditPerson1.ID;
            //EditCompatibility.ID2 = EditPerson2.ID;
            //EditCompatibility.Name = $"{EditPerson1.Name} - {EditPerson2.Name}";
            EditCompatibilityDialogIsOpen = false;
            ChartSet.Save();
        }
    }

    private void DoDeleteCompatibility(Compatibility compat)
    {
        DeleteCompatibility = compat;
        DeleteCompatibilityDialogIsOpen = true;
    }

    private void DeleteCompatibilityObject(MouseEventArgs e)
    {
        if (DeleteCompatibility != null)
        {
            DeleteCompatibilityDialogIsOpen = false;
            ChartSet.RemoveCompatibility(DeleteCompatibility);
        }
    }

    private void DoAddPredictionChart(MouseEventArgs e)
    {
        AddMother = null;
        AddConceptionDate = DateTime.Today;
        AddPredictionChartDialogIsOpen = true;
    }

    private void AddPredictionObject(MouseEventArgs e)
    {
        AddPredictionChartDialogIsOpen = false;
        if (AddMother != null)
        {
            ChartSet.AddPredictionChart(AddMother.ID, AddConceptionDate);
        }
    }

    private void DoEditPrediction(Prediction prediction)
    {
        EditPrediction = prediction;
        EditConceptionDate = prediction.ConceptionDate;
        EditMother = ChartSet.GetPerson(prediction.MotherID);
        EditPredictionDialogIsOpen = true;
    }

    private void EditPredictionObject(MouseEventArgs e)
    {
        if (EditPrediction is not null && EditMother is not null)
        {
            //EditPrediction.MotherID = EditMother.ID;
            //EditPrediction.ConceptionDate = EditConceptionDate;
            //EditPrediction.Name = $"{EditMother.Name} Prediction";
            EditPredictionDialogIsOpen = false;
            ChartSet.Save();
        }
    }

    private void DoDeletePrediction(Prediction prediction)
    {
        DeletePrediction = prediction;
        DeletePredictionDialogIsOpen = true;
    }

    private void DeletePredictionObject(MouseEventArgs e)
    {
        if (DeletePrediction != null)
        {
            DeletePredictionDialogIsOpen = false;
            ChartSet.RemovePrediction(DeletePrediction);
        }
    }

    private void DoPrevious(MouseEventArgs e) => Startdate = Startdate.AddMonths(-1);

    private void DoNext(MouseEventArgs e) => Enddate = Enddate.AddMonths(1);

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

    protected override async Task OnInitializedAsync()
    {
        Startdate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0, DateTimeKind.Local);
        Enddate = Startdate.AddMonths(1);
        await ChartSet.Load(LocalStorage);
        if (ChartSet.Groups.Any()) Current = ChartSet.Groups.First();
    }
}

public record ChartableBase
{
    public string ID { get; init; } = Guid.NewGuid().ToString();
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
    private ILocalStorageService LocalStorage { get; set; }

    public List<Person> People { get; set; } = new List<Person>();
    public readonly BoolDictionary GroupPeople = new();

    public List<Compatibility> CompatibilityCharts { get; set; } = new List<Compatibility>();
    public List<Prediction> PredictionCharts { get; set; } = new List<Prediction>();
    public List<Group> Groups { get; set; } = new List<Group>();

    public Person? GetPerson(string ID) => People.FirstOrDefault(p => p.ID == ID);

    public void AddPerson(Person p)
    {
        People.Add(p);
        GroupPeople.Add(p.ID, false);
        Save();
    }

    public void RemovePerson(Person p)
    {
        People.Remove(p);
        Groups
            .ForEach(g =>
            {
                g.IDs.ForEach(id =>
                {
                    if (!has(id)) g.IDs.Remove(id);
                });
            });
        var compatToRemove = CompatibilityCharts
            .Where(c => !has(c.ID1) || !has(c.ID2))
            .ToList();
        compatToRemove.ForEach(c => CompatibilityCharts.Remove(c));
        var predToRemove = PredictionCharts
            .Where(pr => !has(pr.MotherID))
            .ToList();
        predToRemove.ForEach(pr => PredictionCharts.Remove(pr));
        GroupPeople.Remove(p.ID);

        Save();

        bool has(string id) => People.Any(pp => pp.ID == id);
    }

    public void AddGroup(string Name, List<string> IDs)
    {
        Groups.Add(new Group(IDs) { Name = Name });
        Save();
    }
    public void RemoveGroup(Group g)
    {
        Groups.Remove(g);
        Save();
    }

    public void AddCompatibilityChart(string ID1, string ID2)
    {
        var p1 = GetPerson(ID1);
        var p2 = GetPerson(ID2);
        if (p1 is not null && p2 is not null)
        {
            CompatibilityCharts.Add(new Compatibility(ID1, ID2) { Name = $"{p1.Name} - {p2.Name}" });
            Save();
        }
    }

    public void RemoveCompatibility(Compatibility c)
    {
        CompatibilityCharts.Remove(c);
        Save();
    }

    public void AddPredictionChart(string MotherID, DateTime ConceptionDate)
    {
        var p = GetPerson(MotherID);
        if (p is not null)
        {
            PredictionCharts.Add(new Prediction(MotherID, ConceptionDate) { Name = $"{p.Name} Prediction" });
            Save();
        }
    }

    public void RemovePrediction(Prediction p)
    {
        PredictionCharts.Remove(p);
        Save();
    }

    public async void Save() => await LocalStorage.SetItemAsync("set", this);

    public async Task Load(ILocalStorageService localStorage)
    {
        LocalStorage = localStorage;
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

            // update the GroupPeople dictionary
            GroupPeople.Clear();
            People.ForEach(p => GroupPeople.Add(p.ID, false));

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
            await LocalStorage.ClearAsync();
        }
    }

    public class BoolDictionary : Dictionary<string, bool>
    {
        public bool Contains(string key) => ContainsKey(key);
    }
}