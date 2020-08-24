using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement.Data.Models;

namespace Microsoft.FeatureManagement.Data
{
    public interface IFeatureManagementDbContext
    {
        DbSet<Feature> Features { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<GroupRollout> GroupRollouts { get; set; }
        DbSet<TimeWindow> TimeWindows { get; set; }
        DbSet<Audience> Audiences { get; set; }
        DbSet<BrowserRestriction> BrowserRestrictions { get; set; }
        DbSet<SupportedBrowser> SupportedBrowsers { get; set; }
    }
}