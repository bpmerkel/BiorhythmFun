namespace BiorhythmFun.Client.Model;

/// <summary>
/// Handle saving/fetching objects from local storage
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
    public readonly Dictionary<string, bool> GroupPeople = [];

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
    /// Add a person and save it asynchronously (AddPersonAsync).
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    public async Task<Person> AddPersonAsync(Person p)
    {
        if (!People.Any(pp => pp.Name == p.Name && pp.Birthdate == p.Birthdate))
        {
            People.Add(p);
            GroupPeople.Add(p.ID, false);
            await SaveAsync();
            return p;
        }
        else
        {
            return People.First(pp => pp.Name == p.Name && pp.Birthdate == p.Birthdate);
        }
    }

    /// <summary>
    /// Remove a person and save changes asynchronously (RemovePersonAsync).
    /// </summary>
    /// <param name="p"></param>
    public async Task RemovePersonAsync(Person p)
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

        await SaveAsync();

        bool has(string id) => People.Any(pp => pp.ID == id);
    }

    /// <summary>
    /// Add a group and save it asynchronously (AddGroupAsync).
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="IDs"></param>
    /// <returns></returns>
    public async Task<Group> AddGroupAsync(string Name, List<string> IDs)
    {
        var g = new Group(Name, IDs);
        Groups.Add(g);
        await SaveAsync();
        return g;
    }

    /// <summary>
    /// Remove a group and save changes asynchronously (RemoveGroupAsync).
    /// </summary>
    /// <param name="g"></param>
    public async Task RemoveGroupAsync(Group g)
    {
        Groups.Remove(g);
        await SaveAsync();
    }

    /// <summary>
    /// Add a compatibility chart and save it asynchronously (AddCompatibilityChartAsync).
    /// </summary>
    /// <param name="ID1"></param>
    /// <param name="ID2"></param>
    /// <returns></returns>
    public async Task<Compatibility> AddCompatibilityChartAsync(string ID1, string ID2)
    {
        var p1 = GetPerson(ID1);
        var p2 = GetPerson(ID2);
        if (p1 is not null && p2 is not null)
        {
            var c = new Compatibility($"{p1.Name} - {p2.Name}", ID1, ID2);
            CompatibilityCharts.Add(c);
            await SaveAsync();
            return c;
        }
        return null;
    }

    /// <summary>
    /// Remove a compatibility chart and save changes asynchronously (RemoveCompatibilityAsync).
    /// </summary>
    /// <param name="c"></param>
    public async Task RemoveCompatibilityAsync(Compatibility c)
    {
        CompatibilityCharts.Remove(c);
        await SaveAsync();
    }

    /// <summary>
    /// Add a prediction chart and save it asynchronously (AddPredictionChartAsync).
    /// </summary>
    /// <param name="MotherID"></param>
    /// <param name="ConceptionDate"></param>
    /// <returns></returns>
    public async Task<Prediction> AddPredictionChartAsync(string MotherID, DateTime ConceptionDate)
    {
        var p = GetPerson(MotherID);
        if (p is not null)
        {
            if (!PredictionCharts.Any(pp => pp.Name.StartsWith(p.Name) && pp.ConceptionDate == ConceptionDate))
            {
                var m = new Prediction($"{p.Name} Prediction", MotherID, ConceptionDate);
                PredictionCharts.Add(m);
                await SaveAsync();
                return m;
            }
        }
        return null;
    }

    /// <summary>
    /// Remove a prediction chart and save changes asynchronously (RemovePredictionAsync).
    /// </summary>
    /// <param name="p"></param>
    public async Task RemovePredictionAsync(Prediction p)
    {
        PredictionCharts.Remove(p);
        await SaveAsync();
    }

    /// <summary>
    /// Save to local storage asynchronously (SaveAsync).
    /// </summary>
    public async Task SaveAsync()
    {
        if (LocalStorage != null)
        {
            await LocalStorage.SetItemAsync(nameof(Set), this);
        }
    }

    /// <summary>
    /// Load from local storage asynchronously and apply any query-data actions (LoadAsync).
    /// </summary>
    /// <param name="qd">A dictionary of query data used to create charts from URL parameters.</param>
    /// <returns>The initial ChartableBase to display.</returns>
    public async Task<ChartableBase> LoadAsync(ILocalStorageService localStorage, Dictionary<string, string> qd)
    {
        LocalStorage = localStorage;
        try
        {
            var chartset = await LocalStorage.GetItemAsync<Set>(nameof(Set));
            if (chartset?.People?.Count > 0)
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
                        var p = await AddPersonAsync(new Person(qd["n"], DateTime.Parse(qd["b"])));
                        return p;
                    case "g":
                        // n = name
                        // s = size of group
                        var size = Convert.ToInt32(qd["s"]);
                        var ids = new List<string>();
                        for (var i = 1; i <= size; i++)
                        {
                            var np = await AddPersonAsync(new Person(qd[$"p{i}"], DateTime.Parse(qd[$"b{i}"])));
                            ids.Add(np.ID);
                        }
                        var g = await AddGroupAsync(qd["n"], ids);
                        return g;
                    case "c":
                        // p1 = name of 1st person
                        // p2 = name of 2nd person
                        // b1 = birthdate of 1st person
                        // b2 = birthdate of 2nd person
                        var two = new List<string>();
                        for (var i = 1; i <= 2; i++)
                        {
                            var np = await AddPersonAsync(new Person(qd[$"p{i}"], DateTime.Parse(qd[$"b{i}"])));
                            two.Add(np.ID);
                        }
                        var c = await AddCompatibilityChartAsync(two.First(), two.Last());
                        return c;
                    case "m":
                        // b = mother's birthdate
                        // c = conception date
                        var mother = await AddPersonAsync(new Person(qd["m"], DateTime.Parse(qd["b"])));
                        var m = await AddPredictionChartAsync(mother.ID, DateTime.Parse(qd["c"]));
                        return m;
                }
            }
        }
        catch
        {
            await LocalStorage.ClearAsync();
        }

        return Groups.Count != 0 ? Groups.First()
            : People.Count != 0 ? People.First()
            : new Group("Family", []);
    }

    ///// <summary>
    ///// A simple dictionary of strings to bools.
    ///// </summary>
    //public class BoolDictionary : Dictionary<string, bool>
    //{
    //    public bool Contains(string key) => ContainsKey(key);
    //}
}