using System.Linq;

namespace Esfa.Vacancy.Register.Api.Models
{
    /// <summary>
    /// Versions of the application
    /// </summary>
    public class VersionInformation
    {
        /// <summary>
        /// The semantic version (ie: 1.2.3-rc.45)
        /// </summary>
        public string SemanticVersion { get; set; }

        /// <summary>
        /// The version you would see on nuget (ie: 1.2.3)
        /// </summary>
        public string PackageVersion { get; set; }
        /// <summary>
        /// The assembly version (ie: 1.2.3.45)
        /// </summary>
        public string AssemblyVersion { get; set; }
    }
}