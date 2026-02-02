using ConfigLib;
using ImGuiNET;
using System.Collections.Generic;
using System.Linq;
using VanillaVariants.Configuration;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace VanillaVariants;

public class ConfigLibCompatibility
{
    private const string metalsWithoutCopper = "vanvar:config/properties/metals-without-copper.json";
    private const string settingsAdvanced = "vanvar:Config.SettingsAdvanced";
    private const string settingsSimple = "vanvar:Config.SettingsSimple";
    private const string settingsChuteCraftable = "vanvar:Config.Settings.CraftableChutes";
    private const string settingPrefix = "vanvar:Config.Setting.";
    private const string textChutes = "Chutes";
    private const string textMechanics = "tabname-mechanics";
    private const string textBlocksAndItems = "Blocks and Items";
    private const string textIssues = "Issues";
    private const string textCraftable = "Craftable";
    private const string textExperimental = "Experimental";
    
    private const string settingOverride = "vanvar:Config.Setting.Override";

    public ConfigLibCompatibility(ICoreAPI api)
    {
        Init(api);
    }

    private void Init(ICoreAPI api)
    {
        api.ModLoader.GetModSystem<ConfigLibModSystem>().RegisterCustomConfig("vanvar", (id, buttons) => EditConfig(id, buttons, api));
    }

    private void EditConfig(string id, ControlButtons buttons, ICoreAPI api)
    {
        if (buttons.Save)
        {
            ModConfig.WriteConfig(api, Core.Config);
            if (api is ICoreClientAPI clientApi && !clientApi.IsSinglePlayer)
            {
                clientApi.Network.GetChannel("vanvar").SendPacket<VanillaVariants.Configuration.Config>(Core.Config);
            }
        }

        if (buttons.Restore) Core.Config = ModConfig.ReadConfig(api);
        if (buttons.Defaults) Core.Config = new();
        Edit(api, Core.Config, id);
    }

    private void Edit(ICoreAPI api, Configuration.Config config, string id)
    {
        BuildSimpleSettings(config, id);
        BuildAdvancedSettings(api, config, id);
    }

    private void BuildSimpleSettings(Configuration.Config config, string id)
    {
        if (ImGui.CollapsingHeader(Lang.Get(settingsSimple) + $"##settingsSimple-{id}"))
        {
            ImGui.TextWrapped(Lang.Get(textExperimental));
            ImGui.NewLine();
            ImGui.TextWrapped(Lang.Get(textIssues));
            config.ResolveChestNames = OnCheckBox(id, config.ResolveChestNames, nameof(config.ResolveChestNames));
            config.ResolveMechanicalBlockIssues = OnCheckBox(id, config.ResolveMechanicalBlockIssues, nameof(config.ResolveMechanicalBlockIssues));
            config.ResolveQuernAndAxleRelationship = OnCheckBox(id, config.ResolveQuernAndAxleRelationship, nameof(config.ResolveQuernAndAxleRelationship));
            ImGui.NewLine();
            ImGui.TextWrapped(Lang.Get(textCraftable));
            config.CraftableCage = OnCheckBox(id, config.CraftableCage, nameof(config.CraftableCage));
            config.CraftableWoodenRails = OnCheckBox(id, config.CraftableWoodenRails, nameof(config.CraftableWoodenRails));
            ImGui.NewLine();
            ImGui.TextWrapped(Lang.Get(textBlocksAndItems));
            config.ArmorStand = OnCheckBox(id, config.ArmorStand, nameof(config.ArmorStand));
            config.Barrel = OnCheckBox(id, config.Barrel, nameof(config.Barrel));
            config.Bed = OnCheckBox(id, config.Bed, nameof(config.Bed));
            config.Cage = OnCheckBox(id, config.Cage, nameof(config.Cage));
            config.Chair = OnCheckBox(id, config.Chair, nameof(config.Chair));
            config.Chandelier = OnCheckBox(id, config.Chandelier, nameof(config.Chandelier));
            config.Chest = OnCheckBox(id, config.Chest, nameof(config.Chest));
            config.CrudeDoor = OnCheckBox(id, config.CrudeDoor, nameof(config.CrudeDoor));
            config.DisplayCase = OnCheckBox(id, config.DisplayCase, nameof(config.DisplayCase));
            config.Forge = OnCheckBox(id, config.Forge, nameof(config.Forge));
            config.FruitPress = OnCheckBox(id, config.FruitPress, nameof(config.FruitPress));
            config.Henbox = OnCheckBox(id, config.Henbox, nameof(config.Henbox));
            config.Ladder = OnCheckBox(id, config.Ladder, nameof(config.Ladder));
            config.MetalDoor = OnCheckBox(id, config.MetalDoor, nameof(config.MetalDoor));
            config.Moldrack = OnCheckBox(id, config.Moldrack, nameof(config.Moldrack));
            config.OmokTabletop = OnCheckBox(id, config.OmokTabletop, nameof(config.OmokTabletop));
            config.Palisade = OnCheckBox(id, config.Palisade, nameof(config.Palisade));
            config.Quern = OnCheckBox(id, config.Quern, nameof(config.Quern));
            config.Shelf = OnCheckBox(id, config.Shelf, nameof(config.Shelf));
            config.Sieve = OnCheckBox(id, config.Sieve, nameof(config.Sieve));
            config.Sign = OnCheckBox(id, config.Sign, nameof(config.Sign));
            config.Signpost = OnCheckBox(id, config.Signpost, nameof(config.Signpost));
            config.StonePath = OnCheckBox(id, config.StonePath, nameof(config.StonePath));
            config.SupportBeamMetal = OnCheckBox(id, config.SupportBeamMetal, nameof(config.SupportBeamMetal));
            config.SupportChain = OnCheckBox(id, config.SupportChain, nameof(config.SupportChain));
            config.Table = OnCheckBox(id, config.Table, nameof(config.Table));
            config.Toolrack = OnCheckBox(id, config.Toolrack, nameof(config.Toolrack));
            config.TroughLarge = OnCheckBox(id, config.TroughLarge, nameof(config.TroughLarge));
            config.TroughSmall = OnCheckBox(id, config.TroughSmall, nameof(config.TroughSmall));
            config.WoodBucket = OnCheckBox(id, config.WoodBucket, nameof(config.WoodBucket));
            config.WoodenPan = OnCheckBox(id, config.WoodenPan, nameof(config.WoodenPan));
            config.WoodenRails = OnCheckBox(id, config.WoodenRails, nameof(config.WoodenRails));
            ImGui.NewLine();
            ImGui.TextWrapped(Lang.Get(textMechanics));
            config.MechanicalAngledGears = OnCheckBox(id, config.MechanicalAngledGears, nameof(config.MechanicalAngledGears));
            config.MechanicalAxle = OnCheckBox(id, config.MechanicalAxle, nameof(config.MechanicalAxle));
            config.MechanicalBrake = OnCheckBox(id, config.MechanicalBrake, nameof(config.MechanicalBrake));
            config.MechanicalClutch = OnCheckBox(id, config.MechanicalClutch, nameof(config.MechanicalClutch));
            config.MechanicalHelveHammerBase = OnCheckBox(id, config.MechanicalHelveHammerBase, nameof(config.MechanicalHelveHammerBase));
            config.MechanicalHelveHammerItem = OnCheckBox(id, config.MechanicalHelveHammerItem, nameof(config.MechanicalHelveHammerItem));
            config.MechanicalLargeGear = OnCheckBox(id, config.MechanicalLargeGear, nameof(config.MechanicalLargeGear));
            config.MechanicalLargeGearSectionItem = OnCheckBox(id, config.MechanicalLargeGearSectionItem, nameof(config.MechanicalLargeGearSectionItem));
            config.MechanicalPulverizer = OnCheckBox(id, config.MechanicalPulverizer, nameof(config.MechanicalPulverizer));
            config.MechanicalToggle = OnCheckBox(id, config.MechanicalToggle, nameof(config.MechanicalToggle));
            config.MechanicalTransmission = OnCheckBox(id, config.MechanicalTransmission, nameof(config.MechanicalTransmission));
            config.MechanicalWindmillRotor = OnCheckBox(id, config.MechanicalWindmillRotor, nameof(config.MechanicalWindmillRotor));
            ImGui.NewLine();
            ImGui.TextWrapped(Lang.Get(textChutes));
            config.ArchimedesScrew = OnCheckBox(id, config.ArchimedesScrew, nameof(config.ArchimedesScrew));
            config.Chute = OnCheckBox(id, config.Chute, nameof(config.Chute));
            config.ChuteSectionItem = OnCheckBox(id, config.ChuteSectionItem, nameof(config.ChuteSectionItem));
            config.Hopper = OnCheckBox(id, config.Hopper, nameof(config.Hopper));
        }
    }

    private string[] allMetals;

    private void BuildAdvancedSettings(ICoreAPI api, Configuration.Config config, string id)
    {
        allMetals ??= api.LoadTypesFromFile(metalsWithoutCopper);

        if (ImGui.CollapsingHeader(Lang.Get(settingsAdvanced) + $"##settingsAdvanced-{id}"))
        {
            ImGui.Indent();
            if (ImGui.CollapsingHeader(Lang.Get(settingsChuteCraftable) + $"##settingsChuteCraftable-{id}"))
            {
                Dictionary<string, bool> combinedDict =
                    config.ChuteCraftable
                    .ToDictionary(x => x, _ => true)
                    .Concat(allMetals.ToDictionary(x => x, _ => false))
                    .ToDictionary();

                foreach ((string key, bool val) in combinedDict)
                {
                    combinedDict[key] = OnCheckBox(id, val, key);
                }

                config.ChuteCraftable = [.. combinedDict.Where(x => x.Value).Select(x => x.Key)];
            }
            ImGui.Unindent();
        }
    }

    private bool OnCheckBox(string id, bool value, string name)
    {
        bool newValue = value;
        ImGui.Checkbox(Lang.Get(settingPrefix + name) + $"##{name}-{id}", ref newValue);
        return newValue;
    }
}