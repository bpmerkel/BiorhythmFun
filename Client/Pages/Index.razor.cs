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
            var p = new Person(AddName, AddBirthdate);
            ChartSet.AddPerson(p);
            Current = p;
        }
    }

    private void DoEditPerson(Person p)
    {
        if (p != null)
        {
            EditPerson = p;
            EditName = p.Name;
            EditBirthdate = p.Birthdate;
            EditPersonDialogIsOpen = true;
        }
    }

    private void DoEditPersonObject()
    {
        if (EditPerson != null)
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
        if (DeletePerson != null)
        {
            DeletePersonDialogIsOpen = false;
            ChartSet.RemovePerson(DeletePerson);
            Current = ChartSet.People.First();
        }
    }

    private void DoAddGroup(MouseEventArgs e)
    {
        AddName = string.Empty;
        AddGroupDialogIsOpen = true;
    }

    private void DoAddGroupObject(MouseEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(AddName))
        {
            AddGroupDialogIsOpen = false;
            ChartSet.AddGroup(AddName, ChartSet.GroupPeople.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToList());
        }
    }

    private void DoEditGroup(Group group)
    {
        if (group != null)
        {
            EditGroup = group;
            EditName = group.Name;
            foreach (var g in ChartSet.GroupPeople)
            {
                ChartSet.GroupPeople[g.Key] = false;
            }
            foreach (var g in group.IDs)
            {
                ChartSet.GroupPeople[g] = true;
            }
            EditGroupDialogIsOpen = true;
        }
    }

    private void EditGroupObject()
    {
        if (EditGroup != null)
        {
            EditGroupDialogIsOpen = false;
            if (!string.IsNullOrWhiteSpace(EditName)) EditGroup.Name = EditName;
            EditGroup.IDs.Clear();
            foreach (var kvp in ChartSet.GroupPeople)
            {
                if (kvp.Value) EditGroup.IDs.Add(kvp.Key);
            }
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
        if (DeleteGroup != null)
        {
            DeleteGroupDialogIsOpen = false;
            ChartSet.RemoveGroup(DeleteGroup);
            Current = ChartSet.People.First();
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
        if (AddPerson1 != null && AddPerson2 != null)
        {
            ChartSet.AddCompatibilityChart(AddPerson1.ID, AddPerson2.ID);
        }
    }

    private void DoEditCompatibility(Compatibility compat)
    {
        if (compat != null)
        {
            EditCompatibility = compat;
            EditPerson1 = ChartSet.GetPerson(compat.ID1);
            EditPerson2 = ChartSet.GetPerson(compat.ID2);
            EditCompatibilityDialogIsOpen = true;
        }
    }

    private void EditCompatibilityObject()
    {
        if (EditCompatibility != null && EditPerson1 != null && EditPerson2 != null)
        {
            EditCompatibility.ID1 = EditPerson1.ID;
            EditCompatibility.ID2 = EditPerson2.ID;
            EditCompatibility.Name = $"{EditPerson1.Name} - {EditPerson2.Name}";
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
            Current = ChartSet.People.First();
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
        if (prediction != null)
        {
            EditPrediction = prediction;
            EditConceptionDate = prediction.ConceptionDate;
            EditMother = ChartSet.GetPerson(prediction.MotherID);
            EditPredictionDialogIsOpen = true;
        }
    }

    private void EditPredictionObject(MouseEventArgs e)
    {
        if (EditPrediction != null && EditMother != null)
        {
            EditPrediction.MotherID = EditMother.ID;
            EditPrediction.ConceptionDate = EditConceptionDate;
            EditPrediction.Name = $"{EditMother.Name} Prediction";
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
            Current = ChartSet.People.First();
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
        if (ChartSet.CompatibilityCharts.Any()) Current = ChartSet.CompatibilityCharts.First();
        else if (ChartSet.Groups.Any()) Current = ChartSet.Groups.First();
        else if (ChartSet.People.Any()) Current = ChartSet.People.First();
    }
}

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
                var idtoremove = g.IDs.Where(id => !has(id)).ToList();
                idtoremove.ForEach(id => g.IDs.Remove(id));
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
        Groups.Add(new Group(Name, IDs));
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
            CompatibilityCharts.Add(new Compatibility($"{p1.Name} - {p2.Name}", ID1, ID2));
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
            PredictionCharts.Add(new Prediction($"{p.Name} Prediction", MotherID, ConceptionDate));
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
                    new Person("Micky Mouse", DateTime.Parse("11/18/1928", null, System.Globalization.DateTimeStyles.AssumeLocal) ),
                    new Person("Donald Duck", DateTime.Parse("6/9/1934", null, System.Globalization.DateTimeStyles.AssumeLocal) ),
                    new Person("Minnie Mouse", DateTime.Parse("11/18/1928", null, System.Globalization.DateTimeStyles.AssumeLocal) )
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
                Groups.Add(new Group("Family", People.Select(p => p.ID).ToList()));
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

public class ChartableBase
{
    public string ID { get; init; } = Guid.NewGuid().ToString();
    public string? Name { get; set; } = string.Empty;
}

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

public class Group : ChartableBase
{
    public List<string> IDs { get; set; }

    public Group(string name, List<string> ids)
    {
        Name = name;
        IDs = ids;
    }
}

public class Compatibility : ChartableBase
{
    public string ID1 { get; set; }
    public string ID2 { get; set; }

    public Compatibility(string name, string id1, string id2)
    {
        Name = name;
        ID1 = id1;
        ID2 = id2;
    }
}

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
