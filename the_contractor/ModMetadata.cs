using System;
using System.Collections.Generic;
using SPTarkov.Server.Core.Models.Spt.Mod;

// Disambiguate SemanticVersioning types from System.Version/System.Range
using SemVerVersion = SemanticVersioning.Version;
using SemVerRange = SemanticVersioning.Range;

namespace TheContractor
{
    public record ModMetadata : AbstractModMetadata
    {
        public override string ModGuid { get; init; } = "com.thecontractor.trader";
        public override string Name   { get; init; } = "Trader The Contractor";
        public override string Author { get; init; } = "ShaneeexD";
        public override List<string> Contributors { get; init; } = new();
        public override SemVerVersion Version { get; init; } = new SemVerVersion("1.0.0", false);
        public override SemVerRange SptVersion { get; init; } = new SemVerRange("~4.0.2", false);
        public override List<string> Incompatibilities { get; init; } = new();
        public override Dictionary<string, SemVerRange> ModDependencies { get; init; } = new();
        public override string? Url { get; init; } = string.Empty;
        public override bool? IsBundleMod { get; init; } = false;
        public override string? License { get; init; } = "MIT";

    }
}
