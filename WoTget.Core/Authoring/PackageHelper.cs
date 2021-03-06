﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WoTget.Core.Authoring
{
    public static class PackageHelper
    {
        public static string FileName(this IPackage package)
        {
            return $"{package.Name}.{package.SemanticVersion().ToNormalizedString()}{Constants.PackageExtension}";
        }

        public static SemanticVersion SemanticVersion(this IPackage package)
        {
            return new SemanticVersion(package.Version);
        }

        public static string ToManifestString(this IPackage package)
        {
            string xml = string.Empty;
            using (var memoryStream = new MemoryStream())
            {
                ManifestHelper.Save(package, memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                xml = new StreamReader(memoryStream).ReadToEnd();
            }
            return xml;
        }

        public static IEnumerable<IPackage> FindByName(this IEnumerable<IPackage> packages, string name)
        {
            return packages.Where(p => p.Name.ToLower() == name.ToLower());
        }

        public static IPackage FindByNameAndVersion(this IEnumerable<IPackage> packages, IPackage packageToSearch)
        {
            return FindByNameAndVersion(packages,packageToSearch.Name,packageToSearch.Version);
        }

        public static IPackage FindByNameAndVersion(this IEnumerable<IPackage> packages, string name, string version)
        {
            return packages.SingleOrDefault(p => p.Name.ToLower() == name.ToLower() && p.SemanticVersion() == new SemanticVersion(version));
        }

        public static bool ExistsByName(this IEnumerable<IPackage> packages, string name)
        {
            return packages.Any(p => p.Name.ToLower() == name.ToLower());
        }

        public static bool ExistsByName(this IEnumerable<IPackage> packages, IPackage packageToSearch)
        {
            return ExistsByName(packages,packageToSearch.Name);
        }

        public static bool ExistsByNameAndVersion(this IEnumerable<IPackage> packages, string name,string version)
        {
            return packages.Any(p => p.Name.ToLower() == name.ToLower() && p.SemanticVersion()==new SemanticVersion(version) );
        }

        public static bool ExistsByNameAndVersion(this IEnumerable<IPackage> packages, IPackage packageToSearch)
        {
            return ExistsByNameAndVersion(packages,packageToSearch.Name,packageToSearch.Version);
        }

        public static IEnumerable<IPackage> OnlyLatestVersion(this IEnumerable<IPackage> packages)
        {
            return packages.Where(p => packages.Where(p1 => p1.Name == p.Name).Select(p2 => new SemanticVersion(p2.Version)).Max().ToNormalizedString() == p.SemanticVersion().ToNormalizedString());
        }
    }
}
