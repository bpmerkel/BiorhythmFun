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
    /// Gets the chart set from DI.
    /// </summary>
    [Inject] public Set ChartSet { get; set; }
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
    public async Task DoShowFAQ(MouseEventArgs e) => FAQIsVisible = !FAQIsVisible;

    /// <summary>
    /// Shows the cycle information.
    /// </summary>
    /// <param name="args"></param>
    public async Task ShowCycleInfo(ChartClickEventArgs args)
    {
        cycledata = args;
        ShowInfo = true;
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Adds a person to the chart set.
    /// </summary>
    /// <param name="e"></param>
    private async Task DoAddPerson(MouseEventArgs e)
    {
        AddName = string.Empty;
        AddBirthdate = DateTime.Today;
        AddPersonDialogIsVisible = true;
    }

    /// <summary>
    /// Adds a person object to the chart set.
    /// </summary>
    /// <param name="e"></param>
    private async Task DoAddPersonObject(MouseEventArgs e)
    {
        AddPersonDialogIsVisible = false;
        // save Name and Birthdate to localStorage
        if (!string.IsNullOrWhiteSpace(AddName))
        {
            var p = new Person(AddName, AddBirthdate.Value);
            var np = await ChartSet.AddPersonAsync(p);
            Current = np;
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Edit a person.
    /// </summary>
    /// <param name="p"></param>
    private async Task DoEditPerson(Person p)
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
    private async Task DoEditPersonObject()
    {
        if (EditPerson != null)
        {
            EditPersonDialogIsVisible = false;
            if (!string.IsNullOrWhiteSpace(EditName)) EditPerson.Name = EditName;
            EditPerson.Birthdate = EditBirthdate.Value;
            await ChartSet.SaveAsync();
            await InvokeAsync(StateHasChanged);
        }
    }

    // Adjust the size of the chart
    private async Task DoAdjustSize(int amount)
    {
        Size += amount;
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Delete a person.
    /// </summary>
    /// <param name="p"></param>
    private async Task DoDeletePerson(Person p)
    {
        DeletePerson = p;
        DeletePersonDialogIsVisible = true;
    }

    /// <summary>
    /// Delete a person object.
    /// </summary>

    private async Task DoDeletePersonObject()
    {
        if (DeletePerson != null)
        {
            DeletePersonDialogIsVisible = false;
            await ChartSet.RemovePersonAsync(DeletePerson);
            Current = ChartSet.People.First();
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Adds a group to the chart set.
    /// </summary>
    /// <param name="e"></param>
    private async Task DoAddGroup(MouseEventArgs e)
    {
        AddName = string.Empty;
        AddGroupDialogIsVisible = true;
    }

    /// <summary>
    /// Adds a group object to the chart set.
    /// </summary>
    /// <param name="e"></param>
    private async Task DoAddGroupObject(MouseEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(AddName))
        {
            AddGroupDialogIsVisible = false;
            await ChartSet.AddGroupAsync(AddName, [.. ChartSet.GroupPeople
                .Where(kvp => kvp.Value)
                .Select(kvp => kvp.Key)]);
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Edit a group.
    /// </summary>
    /// <param name="group"></param>
    private async Task DoEditGroup(Group group)
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
    private async Task EditGroupObject()
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
            await ChartSet.SaveAsync();
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Delete a group.
    /// </summary>
    /// <param name="group"></param>
    private async Task DoDeleteGroup(Group group)
    {
        DeleteGroup = group;
        DeleteGroupDialogIsVisible = true;
    }

    /// <summary>
    /// Delete a group object.
    /// </summary>
    private async Task DeleteGroupObject()
    {
        if (DeleteGroup != null)
        {
            DeleteGroupDialogIsVisible = false;
            await ChartSet.RemoveGroupAsync(DeleteGroup);
            Current = ChartSet.People.First();
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Adds a compatibility chart to the chart set.
    /// </summary>
    /// <param name="e"></param>
    private async Task DoAddCompatibilityChart(MouseEventArgs e)
    {
        AddPerson1 = null;
        AddPerson2 = null;
        AddCompatibilityChartDialogIsVisible = true;
    }

    /// <summary>
    /// Adds a compatibility chart object to the chart set.
    /// </summary>
    /// <param name="e"></param>
    private async Task AddCompatibilityObject(MouseEventArgs e)
    {
        AddCompatibilityChartDialogIsVisible = false;
        if (AddPerson1 != null && AddPerson2 != null)
        {
            await ChartSet.AddCompatibilityChartAsync(AddPerson1.ID, AddPerson2.ID);
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Edit a compatibility chart.
    /// </summary>
    /// <param name="compat"></param>
    private async Task DoEditCompatibility(Compatibility compat)
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
    private async Task EditCompatibilityObject()
    {
        if (EditCompatibility != null && EditPerson1 != null && EditPerson2 != null)
        {
            EditCompatibility.ID1 = EditPerson1.ID;
            EditCompatibility.ID2 = EditPerson2.ID;
            EditCompatibility.Name = $"{EditPerson1.Name} - {EditPerson2.Name}";
            EditCompatibilityDialogIsVisible = false;
            await ChartSet.SaveAsync();
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Delete a compatibility chart.
    /// </summary>
    /// <param name="compat"></param>
    private async Task DoDeleteCompatibility(Compatibility compat)
    {
        DeleteCompatibility = compat;
        DeleteCompatibilityDialogIsVisible = true;
    }

    /// <summary>
    /// Delete a compatibility chart object.
    /// </summary>
    /// <param name="e"></param>
    private async Task DeleteCompatibilityObject(MouseEventArgs e)
    {
        if (DeleteCompatibility != null)
        {
            DeleteCompatibilityDialogIsVisible = false;
            await ChartSet.RemoveCompatibilityAsync(DeleteCompatibility);
            Current = ChartSet.People.First();
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Handle the change of the conception date for a prediction.
    /// </summary>
    /// <param name="e"></param>
    private async Task PredictionConceptionDateChanged(DateTime? e)
    {
        AddConceptionDate = EditConceptionDate = e.Value;
        AddConceptionBirthDate = EditConceptionBirthDate = e.Value.AddDays(280);    // birth date is 280 days after conception
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Handle the change of the birth date for a prediction.
    /// </summary>
    /// <param name="e"></param>
    private async Task PredictionConceptionBirthDateChanged(DateTime? e)
    {
        AddConceptionBirthDate = EditConceptionBirthDate = e.Value;
        AddConceptionDate = EditConceptionDate = e.Value.AddDays(-280);    // conception date is 280 days before birth
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Adds a prediction chart to the chart set.
    /// </summary>
    /// <param name="e"></param>
    private async Task DoAddPredictionChart(MouseEventArgs e)
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
    private async Task AddPredictionObject(MouseEventArgs e)
    {
        AddPredictionChartDialogIsVisible = false;
        if (AddMother != null)
        {
            await ChartSet.AddPredictionChartAsync(AddMother.ID, AddConceptionDate.Value);
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Edit a prediction chart.
    /// </summary>
    /// <param name="prediction"></param>
    private async Task DoEditPrediction(Prediction prediction)
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
    private async Task EditPredictionObject(MouseEventArgs e)
    {
        if (EditPrediction != null && EditMother != null)
        {
            EditPrediction.MotherID = EditMother.ID;
            EditPrediction.ConceptionDate = EditConceptionDate.Value;
            EditPrediction.Name = $"{EditMother.Name} Prediction";
            EditPredictionDialogIsVisible = false;
            await ChartSet.SaveAsync();
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Delete a prediction chart.
    /// </summary>
    /// <param name="prediction"></param>
    private async Task DoDeletePrediction(Prediction prediction)
    {
        DeletePrediction = prediction;
        DeletePredictionDialogIsVisible = true;
    }

    /// <summary>
    /// Delete a prediction chart object.
    /// </summary>
    /// <param name="e"></param>
    private async Task DeletePredictionObject(MouseEventArgs e)
    {
        if (DeletePrediction != null)
        {
            DeletePredictionDialogIsVisible = false;
            await ChartSet.RemovePredictionAsync(DeletePrediction);
            Current = ChartSet.People.First();
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Share the chart link to the clipboard.
    /// </summary>
    /// <param name="chart"></param>
    private async Task DoShare(ChartableBase chart)
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
    private async Task DoPrevious(MouseEventArgs e) => Startdate = Startdate.AddMonths(-1);

    /// <summary>
    /// Go to the next month.
    /// </summary>
    /// <param name="e"></param>
    private async Task DoNext(MouseEventArgs e) => Enddate = Enddate.AddMonths(1);

    /// <summary>
    /// Handle the change of the chart date.
    /// </summary>
    /// <param name="e"></param>
    private async Task DoChangeChartDate(MouseEventArgs e)
    {
        Chartdate = Startdate;
        ChangeChartdateDialogIsVisible = true;
    }

    /// <summary>
    /// Handle the change of the chart date.
    /// </summary>
    /// <param name="e"></param>
    private async Task DoChange(MouseEventArgs e)
    {
        ChangeChartdateDialogIsVisible = false;
        Startdate = new DateTime(Chartdate.Value.Year, Chartdate.Value.Month, 1);
        Enddate = Startdate.AddMonths(1);
        await InvokeAsync(StateHasChanged);
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
        Current = await ChartSet.LoadAsync(LocalStorage, qd);
    }
}