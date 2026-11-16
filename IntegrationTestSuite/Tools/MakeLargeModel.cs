// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MakeLargeModel.cs" company="Starion Group S.A.">
//
//   Copyright 2016-2026 Starion Group S.A.
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

// Builds LargeModel.zip, the Annex C.3 archive used by LargeEngineeringModelSetupTestFixture to verify that an
// EngineeringModel with many things of one ClassKind can be seeded and copied.
//
// This is a .NET file-based app. Run it from the root of the repository:
//
//     dotnet run IntegrationTestSuite/Tools/MakeLargeModel.cs -- Data IntegrationTestSuite/WebservicesIntegrationTests/Tests/SiteDirectory/EngineeringModelSetup/LargeModel/LargeModel.zip
//
// The archive is the regular seed data of the Data folder, which is only read, plus ElementCount extra
// ElementDefinitions in its iteration. That is more rows of one ClassKind than fit in one SQL statement on the
// server: PostgreSQL allows 65535 bind parameters per statement, which is 21845 rows for the cache tables
// (3 parameters per row) and 16383 rows for the revision tables (4 parameters per row).
//
// The iids and the timestamps of the zip entries are fixed, so the archive only changes when the Data folder or
// this file changes. Rebuild it when the Data folder changes.

using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;

const int ElementCount = 25000;
const string IterationEntry = "EngineeringModels/9ec982e4-ef72-4953-aa85-b158a95d8d56/Iterations/e163c5ad-f32b-4387-b805-f4b34600bc2c.json";
const string TemplateElementIid = "fe9295c5-af99-494e-86ff-e715837806ae";

var iidNamespace = Guid.Parse("5b0f3e7c-3c84-4a5e-9f3e-0a3a6f2b8384");
var entryTimestamp = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: dotnet run MakeLargeModel.cs -- <Data folder> <target LargeModel.zip>");
    return 1;
}

var dataFolder = Path.GetFullPath(args[0]);
var target = Path.GetFullPath(args[1]);

// Annex C.3 entries must use forward slashes; sort them so the archive does not depend on the file system order
var entries = Directory.GetFiles(dataFolder, "*", SearchOption.AllDirectories)
    .Select(file => Path.GetRelativePath(dataFolder, file).Replace(Path.DirectorySeparatorChar, '/'))
    .OrderBy(entry => entry, StringComparer.Ordinal)
    .ToList();

if (!entries.Contains(IterationEntry))
{
    Console.Error.WriteLine($"{IterationEntry} not found in {dataFolder}");
    return 1;
}

// write to a temporary file first, so that a failed run does not leave a broken archive behind
var temporaryTarget = target + ".tmp";

using (var stream = File.Create(temporaryTarget))
using (var archive = new ZipArchive(stream, ZipArchiveMode.Create))
{
    foreach (var entry in entries)
    {
        var data = File.ReadAllBytes(Path.Combine(dataFolder, entry));

        if (entry == IterationEntry)
        {
            data = EnlargeIteration(data);
        }

        var zipEntry = archive.CreateEntry(entry, CompressionLevel.Optimal);
        zipEntry.LastWriteTime = entryTimestamp;

        using var entryStream = zipEntry.Open();
        entryStream.Write(data);
    }
}

File.Move(temporaryTarget, target, overwrite: true);

Console.WriteLine($"wrote {target} with {ElementCount} extra ElementDefinitions");
return 0;

byte[] EnlargeIteration(byte[] data)
{
    var things = JsonNode.Parse(data)!.AsArray();
    var iteration = things.Single(thing => (string)thing!["classKind"]! == "Iteration")!;
    var template = things.Single(thing => (string)thing!["iid"]! == TemplateElementIid)!;
    var iterationElements = iteration["element"]!.AsArray();

    for (var index = 0; index < ElementCount; index++)
    {
        var iid = CreateNameBasedGuid(iidNamespace, $"ElementDefinition-{index}").ToString();

        var element = template.DeepClone();
        element["iid"] = iid;
        element["name"] = $"Large Model Element {index}";
        element["shortName"] = $"LargeModelElement{index}";

        foreach (var collection in new[] { "containedElement", "parameter", "parameterGroup", "referencedElement", "category", "alias", "definition", "hyperLink" })
        {
            element[collection] = new JsonArray();
        }

        things.Add(element);
        // the cast selects Add(JsonNode) instead of the generic Add<T>, which needs reflection that file-based apps do not have
        iterationElements.Add((JsonNode)iid);
    }

    return Encoding.UTF8.GetBytes(things.ToJsonString());
}

// a name-based (version 5, SHA-1) UUID as defined by RFC 9562, so the iids are the same on every run
static Guid CreateNameBasedGuid(Guid namespaceIid, string name)
{
    var namespaceBytes = namespaceIid.ToByteArray(bigEndian: true);
    var nameBytes = Encoding.UTF8.GetBytes(name);

    var hash = SHA1.HashData([.. namespaceBytes, .. nameBytes]);

    var bytes = hash[..16];
    bytes[6] = (byte)((bytes[6] & 0x0F) | 0x50);
    bytes[8] = (byte)((bytes[8] & 0x3F) | 0x80);

    return new Guid(bytes, bigEndian: true);
}
