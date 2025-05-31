namespace BiorhythmFun.Client.Pages;

/// <summary>
/// Behavior for the main page of the application.
/// </summary>
public partial class Index
{
    /// <summary>
    /// Gets or sets the local storage service.
    /// </summary>
    [Inject] public ILocalStorageService LocalStorage { get; set; }
    /// <summary>
    /// Gets or sets the JavaScript runtime.
    /// </summary>
    [Inject] public IJSRuntime JsRuntime { get; set; }
    /// <summary>
    /// Gets or sets the navigation manager.
    /// </summary>
    [Inject] public NavigationManager NavManager { get; set; }
    /// <summary>
    /// Gets or sets the snackbar.
    /// </summary>
    [Inject] public ISnackbar Snackbar { get; set; }

    /// <summary>
    /// Gets the chart set.
    /// </summary>
    public Set ChartSet { get; init; } = new();

    private ChartableBase Current = default!;
    private int Size = 0;

    private bool FAQIsVisible = false;
    private bool AddPersonDialogIsVisible = false;
    private bool EditPersonDialogIsVisible = false;
    private bool DeletePersonDialogIsVisible = false;
    private string AddName = string.Empty;
    private DateTime? AddBirthdate = DateTime.Today;
    private Person DeletePerson;
    private Person EditPerson;
    private string EditName = string.Empty;
    private DateTime? EditBirthdate = DateTime.Today;

    private bool AddGroupDialogIsVisible = false;
    private bool DeleteGroupDialogIsVisible = false;
    private bool EditGroupDialogIsVisible = false;
    private Group DeleteGroup;
    private Group EditGroup;

    private bool AddCompatibilityChartDialogIsVisible = false;
    private bool EditCompatibilityDialogIsVisible = false;
    private bool DeleteCompatibilityDialogIsVisible = false;
    private Person AddPerson1;
    private Person AddPerson2;
    private Compatibility DeleteCompatibility;
    private Compatibility EditCompatibility;
    private Person EditPerson1;
    private Person EditPerson2;

    private bool AddPredictionChartDialogIsVisible = false;
    private bool EditPredictionDialogIsVisible = false;
    private bool DeletePredictionDialogIsVisible = false;
    private Person AddMother;
    private Prediction DeletePrediction;
    private Prediction EditPrediction;
    private Person EditMother;
    private DateTime? AddConceptionDate = DateTime.Today;
    private DateTime? EditConceptionDate = DateTime.Today;
    private DateTime? AddConceptionBirthDate = DateTime.Today;
    private DateTime? EditConceptionBirthDate = DateTime.Today;
    private DateTime? Chartdate;

    private bool ChangeChartdateDialogIsVisible = false;

    private DateTime Startdate;
    private DateTime Enddate;
    private bool ShowInfo = false;
    private ChartClickEventArgs cycledata;
    private readonly DialogOptions FAQDialogOptions = new() { FullWidth = true, MaxWidth = MaxWidth.Medium };

    public void DoShowFAQ(MouseEventArgs e) => FAQIsVisible = !FAQIsVisible;

    /// <summary>
    /// Shows the cycle information.
    /// </summary>
    /// <param name="args"></param>
    public void ShowCycleInfo(ChartClickEventArgs args)
    {
        cycledata = args;
        ShowInfo = true;
        StateHasChanged();
    }

    /// <summary>
    /// Adds a person to the chart set.
    /// </summary>
    /// <param name="e"></param>
    private void DoAddPerson(MouseEventArgs e)
    {
        AddName = string.Empty;
        AddBirthdate = DateTime.Today;
        AddPersonDialogIsVisible = true;
    }

    /// <summary>
    /// Adds a person object to the chart set.
    /// </summary>
    /// <param name="e"></param>
    private void DoAddPersonObject(MouseEventArgs e)
    {
        AddPersonDialogIsVisible = false;
        // save Name and Birthdate to localStorage
        if (!string.IsNullOrWhiteSpace(AddName))
        {
            var p = new Person(AddName, AddBirthdate.Value);
            ChartSet.AddPerson(p);
            Current = p;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Edit a person.
    /// </summary>
    /// <param name="p"></param>
    private void DoEditPerson(Person p)
    {
        if (p != null)
        {
            EditPerson = p;
            EditName = p.Name;
            EditBirthdate = p.Birthdate;
            EditPersonDialogIsVisible = true;
        }
    }

    /// <summary>
    /// Edit a person object.
    /// </summary>
    private void DoEditPersonObject()
    {
        if (EditPerson != null)
        {
            EditPersonDialogIsVisible = false;
            if (!string.IsNullOrWhiteSpace(EditName)) EditPerson.Name = EditName;
            EditPerson.Birthdate = EditBirthdate.Value;
            ChartSet.Save();
            StateHasChanged();
        }
    }

    // Adjust the size of the chart
    private void DoAdjustSize(int amount)
    {
        Size += amount;
        StateHasChanged();
    }

    /// <summary>
    /// Delete a person.
    /// </summary>
    /// <param name="p"></param>
    private void DoDeletePerson(Person p)
    {
        DeletePerson = p;
        DeletePersonDialogIsVisible = true;
    }

    /// <summary>
    /// Delete a person object.
    /// </summary>

    private void DoDeletePersonObject()
    {
        if (DeletePerson != null)
        {
            DeletePersonDialogIsVisible = false;
            ChartSet.RemovePerson(DeletePerson);
            Current = ChartSet.People.First();
        }
    }

    /// <summary>
    /// Adds a group to the chart set.
    /// </summary>
    /// <param name="e"></param>
    private void DoAddGroup(MouseEventArgs e)
    {
        AddName = string.Empty;
        AddGroupDialogIsVisible = true;
    }

    /// <summary>
    /// Adds a group object to the chart set.
    /// </summary>
    /// <param name="e"></param>
    private void DoAddGroupObject(MouseEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(AddName))
        {
            AddGroupDialogIsVisible = false;
            ChartSet.AddGroup(AddName, ChartSet.GroupPeople.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToList());
            StateHasChanged();
        }
    }

    /// <summary>
    /// Edit a group.
    /// </summary>
    /// <param name="group"></param>
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
            EditGroupDialogIsVisible = true;
        }
    }

    /// <summary>
    /// Edit a group object.
    /// </summary>
    private void EditGroupObject()
    {
        if (EditGroup != null)
        {
            EditGroupDialogIsVisible = false;
            if (!string.IsNullOrWhiteSpace(EditName)) EditGroup.Name = EditName;
            EditGroup.IDs.Clear();
            foreach (var kvp in ChartSet.GroupPeople)
            {
                if (kvp.Value) EditGroup.IDs.Add(kvp.Key);
            }
            ChartSet.Save();
            StateHasChanged();
        }
    }

    /// <summary>
    /// Delete a group.
    /// </summary>
    /// <param name="group"></param>
    private void DoDeleteGroup(Group group)
    {
        DeleteGroup = group;
        DeleteGroupDialogIsVisible = true;
    }

    /// <summary>
    /// Delete a group object.
    /// </summary>
    private void DeleteGroupObject()
    {
        if (DeleteGroup != null)
        {
            DeleteGroupDialogIsVisible = false;
            ChartSet.RemoveGroup(DeleteGroup);
            Current = ChartSet.People.First();
            StateHasChanged();
        }
    }

    /// <summary>
    /// Adds a compatibility chart to the chart set.
    /// </summary>
    /// <param name="e"></param>
    private void DoAddCompatibilityChart(MouseEventArgs e)
    {
        AddPerson1 = null;
        AddPerson2 = null;
        AddCompatibilityChartDialogIsVisible = true;
    }

    /// <summary>
    /// Adds a compatibility chart object to the chart set.
    /// </summary>
    /// <param name="e"></param>
    private void AddCompatibilityObject(MouseEventArgs e)
    {
        AddCompatibilityChartDialogIsVisible = false;
        if (AddPerson1 != null && AddPerson2 != null)
        {
            ChartSet.AddCompatibilityChart(AddPerson1.ID, AddPerson2.ID);
            StateHasChanged();
        }
    }

    /// <summary>
    /// Edit a compatibility chart.
    /// </summary>
    /// <param name="compat"></param>
    private void DoEditCompatibility(Compatibility compat)
    {
        if (compat != null)
        {
            EditCompatibility = compat;
            EditPerson1 = ChartSet.GetPerson(compat.ID1);
            EditPerson2 = ChartSet.GetPerson(compat.ID2);
            EditCompatibilityDialogIsVisible = true;
        }
    }

    /// <summary>
    /// Edit a compatibility chart object.
    /// </summary>
    private void EditCompatibilityObject()
    {
        if (EditCompatibility != null && EditPerson1 != null && EditPerson2 != null)
        {
            EditCompatibility.ID1 = EditPerson1.ID;
            EditCompatibility.ID2 = EditPerson2.ID;
            EditCompatibility.Name = $"{EditPerson1.Name} - {EditPerson2.Name}";
            EditCompatibilityDialogIsVisible = false;
            ChartSet.Save();
            StateHasChanged();
        }
    }

    /// <summary>
    /// Delete a compatibility chart.
    /// </summary>
    /// <param name="compat"></param>
    private void DoDeleteCompatibility(Compatibility compat)
    {
        DeleteCompatibility = compat;
        DeleteCompatibilityDialogIsVisible = true;
    }

    /// <summary>
    /// Delete a compatibility chart object.
    /// </summary>
    /// <param name="e"></param>
    private void DeleteCompatibilityObject(MouseEventArgs e)
    {
        if (DeleteCompatibility != null)
        {
            DeleteCompatibilityDialogIsVisible = false;
            ChartSet.RemoveCompatibility(DeleteCompatibility);
            Current = ChartSet.People.First();
            StateHasChanged();
        }
    }

    /// <summary>
    /// Handle the change of the conception date for a prediction.
    /// </summary>
    /// <param name="e"></param>
    private void PredictionConceptionDateChanged(DateTime? e)
    {
        AddConceptionDate = EditConceptionDate = e.Value;
        AddConceptionBirthDate = EditConceptionBirthDate = e.Value.AddDays(280);    // birth date is 280 days after conception
        StateHasChanged();
    }

    /// <summary>
    /// Handle the change of the birth date for a prediction.
    /// </summary>
    /// <param name="e"></param>
    private void PredictionConceptionBirthDateChanged(DateTime? e)
    {
        AddConceptionBirthDate = EditConceptionBirthDate = e.Value;
        AddConceptionDate = EditConceptionDate = e.Value.AddDays(-280);    // conception date is 280 days before birth
        StateHasChanged();
    }

    /// <summary>
    /// Adds a prediction chart to the chart set.
    /// </summary>
    /// <param name="e"></param>
    private void DoAddPredictionChart(MouseEventArgs e)
    {
        AddMother = null;
        AddConceptionDate = DateTime.Today;
        AddConceptionBirthDate = DateTime.Today.AddDays(280);
        AddPredictionChartDialogIsVisible = true;
    }

    /// <summary>
    /// Adds a prediction chart object to the chart set.
    /// </summary>
    /// <param name="e"></param>
    private void AddPredictionObject(MouseEventArgs e)
    {
        AddPredictionChartDialogIsVisible = false;
        if (AddMother != null)
        {
            ChartSet.AddPredictionChart(AddMother.ID, AddConceptionDate.Value);
            StateHasChanged();
        }
    }

    /// <summary>
    /// Edit a prediction chart.
    /// </summary>
    /// <param name="prediction"></param>
    private void DoEditPrediction(Prediction prediction)
    {
        if (prediction != null)
        {
            EditPrediction = prediction;
            EditConceptionDate = prediction.ConceptionDate;
            EditConceptionBirthDate = prediction.ConceptionDate.AddDays(280);
            EditMother = ChartSet.GetPerson(prediction.MotherID);
            EditPredictionDialogIsVisible = true;
        }
    }

    /// <summary>
    /// Edit a prediction chart object.
    /// </summary>
    /// <param name="e"></param>
    private void EditPredictionObject(MouseEventArgs e)
    {
        if (EditPrediction != null && EditMother != null)
        {
            EditPrediction.MotherID = EditMother.ID;
            EditPrediction.ConceptionDate = EditConceptionDate.Value;
            EditPrediction.Name = $"{EditMother.Name} Prediction";
            EditPredictionDialogIsVisible = false;
            ChartSet.Save();
            StateHasChanged();
        }
    }

    /// <summary>
    /// Delete a prediction chart.
    /// </summary>
    /// <param name="prediction"></param>
    private void DoDeletePrediction(Prediction prediction)
    {
        DeletePrediction = prediction;
        DeletePredictionDialogIsVisible = true;
    }

    /// <summary>
    /// Delete a prediction chart object.
    /// </summary>
    /// <param name="e"></param>
    private void DeletePredictionObject(MouseEventArgs e)
    {
        if (DeletePrediction != null)
        {
            DeletePredictionDialogIsVisible = false;
            ChartSet.RemovePrediction(DeletePrediction);
            Current = ChartSet.People.First();
            StateHasChanged();
        }
    }

    /// <summary>
    /// Share the chart link to the clipboard.
    /// </summary>
    /// <param name="chart"></param>
    private async void DoShare(ChartableBase chart)
    {
        // copy the link to the chart to the clipboard
        var URL = $"{NavManager.Uri}?";

        switch (chart)
        {
            case Person p:
                {
                    URL += $"t=p&n={p.Name}&b={p.Birthdate:yyyy-MM-dd}";
                    break;
                }
            case Group g:
                {
                    URL += $"t=g&n={g.Name}&s={g.IDs.Count}&";
                    // add each member Person
                    URL += string.Join("&", g.IDs
                        .Select((id, i) =>
                        {
                            var p = ChartSet.GetPerson(id);
                            return $"p{i + 1}={p.Name}&b{i + 1}={p.Birthdate:yyyy-MM-dd}";
                        }));
                    break;
                }
            case Compatibility c:
                {
                    var p1 = ChartSet.GetPerson(c.ID1);
                    var p2 = ChartSet.GetPerson(c.ID2);
                    URL += $"t=c&p1={p1.Name}&p2={p2.Name}&b1={p1.Birthdate:yyyy-MM-dd}&b2={p2.Birthdate:yyyy-MM-dd}";
                    break;
                }
            case Prediction m:
                {
                    var mother = ChartSet.GetPerson(m.MotherID);
                    URL += $"t=m&c={m.ConceptionDate:yyyy-MM-dd}&m={mother.Name}&b={mother.Birthdate:yyyy-MM-dd}";
                    break;
                }
        }

        await CopyTextToClipboard(URL);
        Snackbar.Add("Address copied to clipboard", Severity.Normal);
    }

    /// <summary>
    /// Copies the specified text to the clipboard.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private async Task CopyTextToClipboard(string text) => await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);

    /// <summary>
    /// Go to the previous month.
    /// </summary>
    /// <param name="e"></param>
    private void DoPrevious(MouseEventArgs e) => Startdate = Startdate.AddMonths(-1);

    /// <summary>
    /// Go to the next month.
    /// </summary>
    /// <param name="e"></param>
    private void DoNext(MouseEventArgs e) => Enddate = Enddate.AddMonths(1);

    /// <summary>
    /// Handle the change of the chart date.
    /// </summary>
    /// <param name="e"></param>
    private void DoChangeChartDate(MouseEventArgs e)
    {
        Chartdate = Startdate;
        ChangeChartdateDialogIsVisible = true;
    }

    /// <summary>
    /// Handle the change of the chart date.
    /// </summary>
    /// <param name="e"></param>
    private void DoChange(MouseEventArgs e)
    {
        ChangeChartdateDialogIsVisible = false;
        Startdate = new DateTime(Chartdate.Value.Year, Chartdate.Value.Month, 1);
        Enddate = Startdate.AddMonths(1);
        StateHasChanged();
    }

    /// <summary>
    /// Initializes the component asynchronously.
    /// </summary>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {
        Startdate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0, DateTimeKind.Local);
        Enddate = Startdate.AddMonths(1);
        var nvc = HttpUtility.ParseQueryString(new Uri(NavManager.Uri).Query);
        var qd = nvc.AllKeys.ToDictionary(k => k, k => nvc[k]);
        Current = await ChartSet.Load(LocalStorage, qd);
    }
}