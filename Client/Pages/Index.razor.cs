using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MatBlazor;
using System.Web;
using BiorhythmFun.Client.Model;

namespace BiorhythmFun.Client.Pages;

public partial class Index
{
    [Inject] public ILocalStorageService LocalStorage { get; set; } = default!;
    [Inject] public IJSRuntime JsRuntime { get; set; } = default!;
    [Inject] public NavigationManager NavManager { get; set; } = default!;
    [Inject] public IMatToaster Toaster { get; set; } = default!;

    private readonly Set ChartSet = new();
    private ChartableBase Current = default!;

    private bool FAQIsOpen = false;
    private bool AddPersonDialogIsOpen = false;
    private bool EditPersonDialogIsOpen = false;
    private bool DeletePersonDialogIsOpen = false;
    private string AddName = string.Empty;
    private DateTime AddBirthdate = DateTime.Today;
    private Person DeletePerson;
    private Person EditPerson;
    private string EditName = string.Empty;
    private DateTime EditBirthdate = DateTime.Today;

    private bool AddGroupDialogIsOpen = false;
    private bool DeleteGroupDialogIsOpen = false;
    private bool EditGroupDialogIsOpen = false;
    private Group DeleteGroup;
    private Group EditGroup;
    // use AddName and EditName from above

    private bool AddCompatibilityChartDialogIsOpen = false;
    private bool EditCompatibilityDialogIsOpen = false;
    private bool DeleteCompatibilityDialogIsOpen = false;
    private Person AddPerson1;
    private Person AddPerson2;
    private Compatibility DeleteCompatibility;
    private Compatibility EditCompatibility;
    private Person EditPerson1;
    private Person EditPerson2;

    private bool AddPredictionChartDialogIsOpen = false;
    private bool EditPredictionDialogIsOpen = false;
    private bool DeletePredictionDialogIsOpen = false;
    private Person AddMother;
    private DateTime AddConceptionDate = DateTime.Today;
    private Prediction DeletePrediction;
    private Prediction EditPrediction;
    private Person EditMother;
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

        Toaster.Add("Address copied to clipboard", MatToastType.Info, "Share", "share");
    }

    private async Task CopyTextToClipboard(string text)
    {
        await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
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

        //var qs = new Uri(NavManager.Uri).GetComponents(UriComponents.Query, UriFormat.Unescaped);
        var nvc = HttpUtility.ParseQueryString(new Uri(NavManager.Uri).Query);
        var qd = nvc.AllKeys.ToDictionary(k => k, k => nvc[k]);

        Current = await ChartSet.Load(LocalStorage, qd);
    }
}