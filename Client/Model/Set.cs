using Blazored.LocalStorage;

namespace BiorhythmFun.Client.Model;

public class Set
{
    private ILocalStorageService LocalStorage { get; set; } = default!;

    public List<Person> People { get; set; } = new List<Person>();
    public readonly BoolDictionary GroupPeople = new();

    public List<Compatibility> CompatibilityCharts { get; set; } = new List<Compatibility>();
    public List<Prediction> PredictionCharts { get; set; } = new List<Prediction>();
    public List<Group> Groups { get; set; } = new List<Group>();

    public Person? GetPerson(string ID) => People.FirstOrDefault(p => p.ID == ID);

    public Person AddPerson(Person p)
    {
        if (!People.Any(pp => pp.Name == p.Name && pp.Birthdate == p.Birthdate))
        {
            People.Add(p);
            GroupPeople.Add(p.ID, false);
            Save();
            return p;
        }
        else
        {
            return People.First(pp => pp.Name == p.Name && pp.Birthdate == p.Birthdate);
        }
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

    public Group AddGroup(string Name, List<string> IDs)
    {
        var g = new Group(Name, IDs);
        Groups.Add(g);
        Save();
        return g;
    }

    public void RemoveGroup(Group g)
    {
        Groups.Remove(g);
        Save();
    }

    public Compatibility? AddCompatibilityChart(string ID1, string ID2)
    {
        var p1 = GetPerson(ID1);
        var p2 = GetPerson(ID2);
        if (p1 is not null && p2 is not null)
        {
            var c = new Compatibility($"{p1.Name} - {p2.Name}", ID1, ID2);
            CompatibilityCharts.Add(c);
            Save();
            return c;
        }
        return null;
    }

    public void RemoveCompatibility(Compatibility c)
    {
        CompatibilityCharts.Remove(c);
        Save();
    }

    public Prediction? AddPredictionChart(string MotherID, DateTime ConceptionDate)
    {
        var p = GetPerson(MotherID);
        if (p is not null)
        {
            if (!PredictionCharts.Any(pp => pp.Name.StartsWith(p.Name) && pp.ConceptionDate == ConceptionDate))
            {
                var m = new Prediction($"{p.Name} Prediction", MotherID, ConceptionDate);
                PredictionCharts.Add(m);
                Save();
                return m;
            }
        }
        return null;
    }

    public void RemovePrediction(Prediction p)
    {
        PredictionCharts.Remove(p);
        Save();
    }

    public async void Save() => await LocalStorage.SetItemAsync("set", this);

    public async Task<ChartableBase?> Load(ILocalStorageService localStorage, Dictionary<string, string> qd)
    {
        LocalStorage = localStorage;
        try
        {
            var chartset = await localStorage.GetItemAsync<Set>("set") ?? new Set();

            if (chartset.People.Any())
            {
                People.AddRange(chartset.People);
                // update the GroupPeople dictionary
                GroupPeople.Clear();
                People.ForEach(p => GroupPeople.Add(p.ID, false));
                Groups.AddRange(chartset.Groups);
                CompatibilityCharts.AddRange(chartset.CompatibilityCharts);
                PredictionCharts.AddRange(chartset.PredictionCharts);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex}");
            await LocalStorage.ClearAsync();
        }

        if (qd.Any())
        {
            switch (qd["t"])
            {
                case "p":
                    // n = name
                    // b = birthdate
                    var p = AddPerson(new Person(qd["n"], DateTime.Parse(qd["b"])));
                    Save();
                    return p;
                case "g":
                    // n = name
                    // s = size of group
                    var size = Convert.ToInt32(qd["s"]);
                    var ids = Enumerable.Range(1, size)
                        .Select(i =>
                        {
                            var p = AddPerson(new Person(qd[$"p{i}"], DateTime.Parse(qd[$"b{i}"])));
                            return p.ID;
                        })
                        .ToList();
                    var g = AddGroup(qd["n"], ids);
                    Save();
                    return g;
                case "c":
                    // p1 = name of 1st person
                    // p2 = name of 2nd person
                    // b1 = birthdate of 1st person
                    // b2 = birthdate of 2nd person
                    var two = Enumerable.Range(1, 2)
                        .Select(i =>
                        {
                            var p = AddPerson(new Person(qd[$"p{i}"], DateTime.Parse(qd[$"b{i}"])));
                            return p.ID;
                        })
                        .ToList();
                    var c = AddCompatibilityChart(two.First(), two.Last());
                    Save();
                    return c;
                case "m":
                    // b = mother's birthdate
                    // c = conception date
                    var mother = AddPerson(new Person(qd["m"], DateTime.Parse(qd["b"])));
                    var m = AddPredictionChart(mother.ID, DateTime.Parse(qd["c"]));
                    Save();
                    return m;
            }
        }

        return Groups.Any() ? Groups.First() : People.First();
    }

    public class BoolDictionary : Dictionary<string, bool>
    {
        public bool Contains(string key) => ContainsKey(key);
    }
}
