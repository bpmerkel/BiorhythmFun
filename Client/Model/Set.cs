namespace BiorhythmFun.Client.Model;

/// <summary>
/// Handle saving/fecthing objects from local storage
/// </summary>
public class Set
{
    private ILocalStorageService LocalStorage { get; set; }
    /// <summary>
    /// The list of people.
    /// </summary>
    public List<Person> People { get; set; } = [];
    /// <summary>
    /// The list of groups.
    /// </summary>
    public List<Group> Groups { get; set; } = [];
    /// <summary>
    /// The dictionary of people in groups.
    /// </summary>
    public readonly BoolDictionary GroupPeople = [];
    /// <summary>
    /// The list of compatibility charts.
    /// </summary>
    public List<Compatibility> CompatibilityCharts { get; set; } = [];
    /// <summary>
    /// The list of prediction charts.
    /// </summary>
    public List<Prediction> PredictionCharts { get; set; } = [];
    /// <summary>
    /// Get the Person for the given ID.
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public Person GetPerson(string ID) => People.FirstOrDefault(p => p.ID == ID);

    /// <summary>
    /// Add a person and save it.
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Remove a person.
    /// </summary>
    /// <param name="p"></param>
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

    /// <summary>
    /// Add a group.
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="IDs"></param>
    /// <returns></returns>
    public Group AddGroup(string Name, List<string> IDs)
    {
        var g = new Group(Name, IDs);
        Groups.Add(g);
        Save();
        return g;
    }

    /// <summary>
    /// Remove a group.
    /// </summary>
    /// <param name="g"></param>
    public void RemoveGroup(Group g)
    {
        Groups.Remove(g);
        Save();
    }

    /// <summary>
    /// Add a compatibility chart.
    /// </summary>
    /// <param name="ID1"></param>
    /// <param name="ID2"></param>
    /// <returns></returns>
    public Compatibility AddCompatibilityChart(string ID1, string ID2)
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

    /// <summary>
    /// Remove a compatibility chart.
    /// </summary>
    /// <param name="c"></param>
    public void RemoveCompatibility(Compatibility c)
    {
        CompatibilityCharts.Remove(c);
        Save();
    }

    /// <summary>
    /// Add a prediction chart.
    /// </summary>
    /// <param name="MotherID"></param>
    /// <param name="ConceptionDate"></param>
    /// <returns></returns>
    public Prediction AddPredictionChart(string MotherID, DateTime ConceptionDate)
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

    /// <summary>
    /// Remove a prediction chart.
    /// </summary>
    /// <param name="p"></param>
    public void RemovePrediction(Prediction p)
    {
        PredictionCharts.Remove(p);
        Save();
    }

    /// <summary>
    /// Save to local storage.
    /// </summary>
    public async void Save()
    {
        if (LocalStorage != null)
        {
            await LocalStorage.SetItemAsync(nameof(Set), this);
        }
    }

    /// <summary>
    /// Load from local storage.
    /// </summary>
    /// <param name="localStorage"></param>
    /// <param name="qd"></param>
    /// <returns></returns>
    public async Task<ChartableBase> Load(ILocalStorageService localStorage, Dictionary<string, string> qd)
    {
        LocalStorage = localStorage;
        try
        {
            var chartset = await localStorage.GetItemAsync<Set>(nameof(Set));
            if (chartset?.People?.Any() ?? false)
            {
                People.AddRange(chartset.People);
                // update the GroupPeople dictionary
                GroupPeople.Clear();
                People.ForEach(p => GroupPeople.Add(p.ID, false));
                Groups.AddRange(chartset.Groups);
                CompatibilityCharts.AddRange(chartset.CompatibilityCharts);
                PredictionCharts.AddRange(chartset.PredictionCharts);
            }

            if (qd.TryGetValue("t", out string value))
            {
                switch (value)
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
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Loading local storage exception occurred: {ex}");
            await localStorage.ClearAsync();
        }

        return Groups.Any() ? Groups.First()
            : People.Any() ? People.First()
            : new Group("Family", []);
    }

    /// <summary>
    /// A simple dictionary of strings to bools.
    /// </summary>
    public class BoolDictionary : Dictionary<string, bool>
    {
        public bool Contains(string key) => ContainsKey(key);
    }
}
