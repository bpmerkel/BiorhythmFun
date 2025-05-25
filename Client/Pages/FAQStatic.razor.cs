namespace BiorhythmFun.Client.Pages;

public partial class FAQStatic
{
    protected string version;

    /// <summary>
    /// Gets the version of the application along with the build date.
    /// </summary>
    public string Version
    {
        get
        {
            if (version == null)
            {
                var build = Assembly.GetExecutingAssembly().GetName().Version;
                var buildDate = new DateTime(2000, 1, 1).AddDays(build.Build).AddSeconds(build.Revision * 2);
                version = $"{build}, built on {buildDate:ddd MM-dd-yyyy hh\\:mm tt}";
            }
            return version;
        }
    }
}