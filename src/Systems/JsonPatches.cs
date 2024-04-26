using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.ServerMods.NoObf;
using System;
using Vintagestory.API.Datastructures;
using Vintagestory.ServerMods;
using System.Linq;

namespace VanillaVariants;

public class JsonPatches : ModSystem
{
    public override double ExecuteOrder() => 0.06;

    public override void AssetsLoaded(ICoreAPI api) => ApplyJsonPatches(api);

    private static void ApplyJsonPatches(ICoreAPI api)
    {
        List<JsonPatch> newPatches = new();

        // spent 5 hours trying to make it work, gave up and decided to hardcode json patches
        // FillPlaceholderPatches(api, newPatches, file, types);

        foreach (string domain in new string[] { "vanvar", "wildcrafttree" })
        {
            AssetLocation chestNormal = AssetLocation.Create($"{domain}:blocktypes/wood/chest.json");
            AssetLocation chestDouble = AssetLocation.Create($"{domain}:blocktypes/wood/chest-trunk.json");
            AssetLocation chestLabeled = AssetLocation.Create($"{domain}:blocktypes/wood/chest-labeled.json");

            PatchCondition condition = new PatchCondition() { When = "VanillaVariants_Chest_Enabled", IsValue = "true" };

            TypeProperties chestProps = api.Assets.TryGet($"vanvar:config/properties/chests-{domain}.json").ToObject<TypeProperties>();
            string[] types = api.GetTypes(new RegistryObjectVariantGroup() { LoadFromProperties = chestProps.GetLoadFromProperties(), States = chestProps.States }, chestProps.SkipVariants);

            if (!api.ModLoader.IsModEnabled(domain))
            {
                continue;
            }

            for (int i = 0; i < types.Length; i++)
            {
                newPatches.Add(new JsonPatch()
                {
                    Op = EnumJsonPatchOp.AddMerge,
                    Value = JsonObject.FromJson($"{{ \"base\": \"game:block/wood/debarked/{types[i]}\", \"overlays\": [\"vanvar:block/chest/sides\" ] }}"),
                    Path = $"/textures/{types[i]}-sides2",
                    File = chestNormal,
                    Condition = condition
                });
                newPatches.Add(new JsonPatch()
                {
                    Op = EnumJsonPatchOp.AddMerge,
                    Value = JsonObject.FromJson($"{{ \"base\": \"game:block/wood/debarked/{types[i]}\" }}"),
                    Path = $"/textures/{types[i]}-lid",
                    File = chestNormal,
                    Condition = condition
                });
                newPatches.Add(new JsonPatch()
                {
                    Op = EnumJsonPatchOp.AddMerge,
                    Value = JsonObject.FromJson($"{{ \"base\": \"game:block/wood/debarked/{types[i]}\", \"overlays\": [\"vanvar:block/chest/accessories\" ] }}"),
                    Path = $"/textures/{types[i]}-accessories",
                    File = chestNormal,
                    Condition = condition
                });

                newPatches.Add(new JsonPatch()
                {
                    Op = EnumJsonPatchOp.AddMerge,
                    Value = JsonObject.FromJson($"{{ \"base\": \"game:block/wood/debarked/{types[i]}\", \"overlays\": [\"vanvar:block/trunk/right-side\" ] }}"),
                    Path = $"/textures/{types[i]}-right-side",
                    File = chestDouble,
                    Condition = condition
                });
                newPatches.Add(new JsonPatch()
                {
                    Op = EnumJsonPatchOp.AddMerge,
                    Value = JsonObject.FromJson($"{{ \"base\": \"game:block/wood/debarked/{types[i]}\", \"overlays\": [\"vanvar:block/chest/sides\" ] }}"),
                    Path = $"/textures/{types[i]}-sides2",
                    File = chestDouble,
                    Condition = condition
                });
                newPatches.Add(new JsonPatch()
                {
                    Op = EnumJsonPatchOp.AddMerge,
                    Value = JsonObject.FromJson($"{{ \"base\": \"game:block/wood/debarked/{types[i]}\" }}"),
                    Path = $"/textures/{types[i]}-lid",
                    File = chestDouble,
                    Condition = condition
                });
                newPatches.Add(new JsonPatch()
                {
                    Op = EnumJsonPatchOp.AddMerge,
                    Value = JsonObject.FromJson($"{{ \"base\": \"game:block/wood/debarked/{types[i]}\", \"overlays\": [\"vanvar:block/chest/accessories\" ] }}"),
                    Path = $"/textures/{types[i]}-accessories",
                    File = chestDouble,
                    Condition = condition
                });
                newPatches.Add(new JsonPatch()
                {
                    Op = EnumJsonPatchOp.AddMerge,
                    Value = JsonObject.FromJson($"{{ \"base\": \"game:block/wood/debarked/{types[i]}\", \"overlays\": [\"vanvar:block/trunk/left-side\" ] }}"),
                    Path = $"/textures/{types[i]}-left-side",
                    File = chestDouble,
                    Condition = condition
                });

                newPatches.Add(new JsonPatch()
                {
                    Op = EnumJsonPatchOp.Add,
                    Value = JsonObject.FromJson($"{{ \"base\": \"game:block/wood/debarked/{types[i]}\", \"overlays\": [\"vanvar:block/chest/sides\" ] }}"),
                    Path = $"/textures/{types[i]}-sides2",
                    File = chestLabeled,
                    Condition = condition
                });
                newPatches.Add(new JsonPatch()
                {
                    Op = EnumJsonPatchOp.Add,
                    Value = JsonObject.FromJson($"{{ \"base\": \"game:block/wood/debarked/{types[i]}\" }}"),
                    Path = $"/textures/{types[i]}-lid",
                    File = chestLabeled,
                    Condition = condition
                });
                newPatches.Add(new JsonPatch()
                {
                    Op = EnumJsonPatchOp.Add,
                    Value = JsonObject.FromJson($"{{ \"base\": \"game:block/wood/debarked/{types[i]}\", \"overlays\": [\"vanvar:block/chest/accessories\" ] }}"),
                    Path = $"/textures/{types[i]}-accessories",
                    File = chestLabeled,
                    Condition = condition
                });
                newPatches.Add(new JsonPatch()
                {
                    Op = EnumJsonPatchOp.Add,
                    Value = JsonObject.FromJson($"{{ \"base\": \"game:block/wood/chest/label\" }}"),
                    Path = $"/textures/{types[i]}-label",
                    File = chestLabeled,
                    Condition = condition
                });
            }
        }

        ModJsonPatchLoader loader = api.ModLoader.GetModSystem<ModJsonPatchLoader>();

        int appliedCount = 0;
        int notfoundCount = 0;
        int errorCount = 0;

        HashSet<string> loadedModIds = api.ModLoader.Mods.Select((Mod m) => m.Info.ModID).ToHashSet();
        for (int i = 0; newPatches != null && i < newPatches.Count; i++)
        {
            JsonPatch patch = newPatches[i];

            if (!patch.Enabled)
            {
                continue;
            }
            if (patch.Condition != null)
            {
                IAttribute attr = api.World.Config[patch.Condition.When];
                if (attr == null)
                {
                    continue;
                }
                if (patch.Condition.useValue)
                {
                    patch.Value = JsonObject.FromJson(attr.ToJsonToken());
                }
                else if (!patch.Condition.IsValue.Equals(attr.GetValue()?.ToString() ?? "", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }
            }
            if (patch.DependsOn != null)
            {
                bool enabled = true;
                PatchModDependence[] dependsOn = patch.DependsOn;
                foreach (PatchModDependence dependence in dependsOn)
                {
                    bool loaded = loadedModIds.Contains(dependence.modid);
                    enabled = enabled && (loaded ^ dependence.invert);
                }
                if (!enabled)
                {
                    continue;
                }
            }

            loader.ApplyPatch(i, null, patch, ref appliedCount, ref notfoundCount, ref errorCount);
        }
    }

    // private static void FillPlaceholderPatches(ICoreAPI api, List<JsonPatch> newPatches, VariantGroupsFromFile file, string[] woodTypes)
    // {
    //     foreach (string type in woodTypes)
    //     {
    //         foreach (JsonPatch patch in GetPlaceholderPatches(api, file))
    //         {
    //             string path = patch.Path.Replace(file.Code, type);
    //             JsonObject value = JsonObject.FromJson(patch.Value.ToString().Replace(file.Code, type));
    //             // JsonObject value = patch.Value;
    //             newPatches.Add(new JsonPatch()
    //             {
    //                 Op = patch.Op,
    //                 Value = value,
    //                 Path = path,
    //                 File = patch.File,
    //                 Condition = patch.Condition,
    //                 Side = patch.Side
    //             });
    //         }
    //     }
    // }

    // private static List<JsonPatch> GetPlaceholderPatches(ICoreAPI api, VariantGroupsFromFile file)
    // {
    //     IAsset asset = api.Assets.TryGet(file.PatchesFromFile);
    //     List<JsonPatch> placeholderPatches = new();

    //     try
    //     {
    //         JsonPatch[] assetPatches = asset.ToObject<JsonPatch[]>(null);
    //         if (assetPatches != null)
    //         {
    //             placeholderPatches.AddRange(assetPatches);
    //         }
    //     }
    //     catch (Exception e)
    //     {
    //         api.Logger.Error("[Vanilla Variants] Failed loading patches file {0}:", asset.Location);
    //         api.Logger.Error(e);
    //     }

    //     return placeholderPatches;
    // }
}
