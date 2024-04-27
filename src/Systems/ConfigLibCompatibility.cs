using ConfigLib;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using VanillaVariants.Configuration;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace VanillaVariants;

public class ConfigLibCompatibility
{
    private const string metalsWithoutCopper = "vanvar:config/properties/metals-without-copper.json";
    private const string settingsAdvanced = "vanvar:Config.SettingsAdvanced";
    private const string settingsSimple = "vanvar:Config.SettingsSimple";
    private const string settingsBlockEntityItemFlow = "vanvar:Config.Settings.BlockEntityItemFlow";
    private const string settingsChuteFlowRates = "vanvar:Config.Settings.FlowRates";
    private const string settingsChuteQuantitySlots = "vanvar:Config.Settings.QuantitySlots";
    private const string settingsChuteCheckRateMs = "vanvar:Config.Settings.CheckRateMs";
    private const string settingsChuteCraftable = "vanvar:Config.Settings.CraftableChutes";
    private const string settingsQuantitySlotsChest = "vanvar:Config.Settings.ChestQuantitySlots";
    private const string vanvarChest = "vanvar:chest-east";
    private const string vanvarTrunk = "vanvar:trunk-east";
    private const string wtChest = "wildcrafttree:chest-east";
    private const string wtTrunk = "wildcrafttree:trunk-east";
    private const string settingsQuantitySlotsTrunk = "vanvar:Config.Settings.DoubleChestQuantitySlots";
    private const string settingPrefix = "vanvar:Config.Setting.";
    private const string textChutes = "Chutes";
    private const string textMechanics = "tabname-mechanics";
    private const string textBlocksAndItems = "Blocks and Items";
    private const string textQuern = "Quern";
    private const string textIssues = "Issues";
    private const string textCraftable = "Craftable";
    private const string textExperimental = "Experimental";
    private const string nameHopper = "hopper";

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
        if (buttons.Save) ModConfig.WriteConfig(api, Core.Config);
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
            config.ExperimentalOverlayTest = OnCheckBox(id, config.ExperimentalOverlayTest, nameof(config.ExperimentalOverlayTest));
            ImGui.NewLine();
            ImGui.TextWrapped(Lang.Get(textIssues));
            config.ResolveBarrelSounds = OnCheckBox(id, config.ResolveBarrelSounds, nameof(config.ResolveBarrelSounds));
            config.ResolveBasketTrapIssues = OnCheckBox(id, config.ResolveBasketTrapIssues, nameof(config.ResolveBasketTrapIssues));
            config.ResolveChestNames = OnCheckBox(id, config.ResolveChestNames, nameof(config.ResolveChestNames));
            config.ResolveHenboxImposter = OnCheckBox(id, config.ResolveHenboxImposter, nameof(config.ResolveHenboxImposter));
            config.ResolveMechanicalBlockIssues = OnCheckBox(id, config.ResolveMechanicalBlockIssues, nameof(config.ResolveMechanicalBlockIssues));
            config.ResolveQuernAndAxleRelationship = OnCheckBox(id, config.ResolveQuernAndAxleRelationship, nameof(config.ResolveQuernAndAxleRelationship));
            ImGui.NewLine();
            ImGui.TextWrapped(Lang.Get(textCraftable));
            config.CraftableCage = OnCheckBox(id, config.CraftableCage, nameof(config.CraftableCage));
            config.CraftableWagonWheels = OnCheckBox(id, config.CraftableWagonWheels, nameof(config.CraftableWagonWheels));
            config.CraftableWoodenRails = OnCheckBox(id, config.CraftableWoodenRails, nameof(config.CraftableWoodenRails));
            ImGui.NewLine();
            ImGui.TextWrapped(Lang.Get(textQuern));
            config.DecorativeQuern = OnCheckBox(id, config.DecorativeQuern, nameof(config.DecorativeQuern));
            config.FunctionalQuern = OnCheckBox(id, config.FunctionalQuern, nameof(config.FunctionalQuern));
            ImGui.NewLine();
            ImGui.TextWrapped(Lang.Get(textBlocksAndItems));
            config.ArmorStand = OnCheckBox(id, config.ArmorStand, nameof(config.ArmorStand));
            config.Barrel = OnCheckBox(id, config.Barrel, nameof(config.Barrel));
            config.Basket = OnCheckBox(id, config.Basket, nameof(config.Basket));
            config.Bed = OnCheckBox(id, config.Bed, nameof(config.Bed));
            config.Cage = OnCheckBox(id, config.Cage, nameof(config.Cage));
            config.Chair = OnCheckBox(id, config.Chair, nameof(config.Chair));
            config.Chest = OnCheckBox(id, config.Chest, nameof(config.Chest));
            config.CrudeDoor = OnCheckBox(id, config.CrudeDoor, nameof(config.CrudeDoor));
            config.DisplayCase = OnCheckBox(id, config.DisplayCase, nameof(config.DisplayCase));
            config.Firewood = OnCheckBox(id, config.Firewood, nameof(config.Firewood));
            config.Forge = OnCheckBox(id, config.Forge, nameof(config.Forge));
            config.FruitPress = OnCheckBox(id, config.FruitPress, nameof(config.FruitPress));
            config.HandBasket = OnCheckBox(id, config.HandBasket, nameof(config.HandBasket));
            config.Henbox = OnCheckBox(id, config.Henbox, nameof(config.Henbox));
            config.Ladder = OnCheckBox(id, config.Ladder, nameof(config.Ladder));
            config.Moldrack = OnCheckBox(id, config.Moldrack, nameof(config.Moldrack));
            config.OmokTabletop = OnCheckBox(id, config.OmokTabletop, nameof(config.OmokTabletop));
            config.Palisade = OnCheckBox(id, config.Palisade, nameof(config.Palisade));
            config.Shelf = OnCheckBox(id, config.Shelf, nameof(config.Shelf));
            config.Sieve = OnCheckBox(id, config.Sieve, nameof(config.Sieve));
            config.Sign = OnCheckBox(id, config.Sign, nameof(config.Sign));
            config.Signpost = OnCheckBox(id, config.Signpost, nameof(config.Signpost));
            config.SodRoofing = OnCheckBox(id, config.SodRoofing, nameof(config.SodRoofing));
            config.StonePath = OnCheckBox(id, config.StonePath, nameof(config.StonePath));
            config.SupportBeamMetal = OnCheckBox(id, config.SupportBeamMetal, nameof(config.SupportBeamMetal));
            config.SupportChain = OnCheckBox(id, config.SupportChain, nameof(config.SupportChain));
            config.Table = OnCheckBox(id, config.Table, nameof(config.Table));
            config.Toolrack = OnCheckBox(id, config.Toolrack, nameof(config.Toolrack));
            config.Trapdoor = OnCheckBox(id, config.Trapdoor, nameof(config.Trapdoor));
            config.TroughLarge = OnCheckBox(id, config.TroughLarge, nameof(config.TroughLarge));
            config.TroughSmall = OnCheckBox(id, config.TroughSmall, nameof(config.TroughSmall));
            config.WagonWheels = OnCheckBox(id, config.WagonWheels, nameof(config.WagonWheels));
            config.WoodBucket = OnCheckBox(id, config.WoodBucket, nameof(config.WoodBucket));
            config.WoodenPan = OnCheckBox(id, config.WoodenPan, nameof(config.WoodenPan));
            config.WoodenPath = OnCheckBox(id, config.WoodenPath, nameof(config.WoodenPath));
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

    private void BuildAdvancedSettings(ICoreAPI api, Configuration.Config config, string id)
    {
        if (ImGui.CollapsingHeader(Lang.Get(settingsAdvanced) + $"##settingsAdvanced-{id}"))
        {
            ImGui.Indent();
            if (ImGui.CollapsingHeader(Lang.Get(settingsBlockEntityItemFlow) + $"##settingsBlockEntityItemFlow-{id}"))
            {
                ImGui.Indent();
                if (ImGui.CollapsingHeader(Lang.Get(settingsChuteFlowRates) + $"##settingsChuteFlowRates-{id}"))
                {
                    ImGui.Indent();
                    foreach (string name in config.ChuteFlowRates.Keys.Where(name => ImGui.CollapsingHeader(name + $"##flowrates-{id}")))
                    {
                        DictionaryEditor(config.ChuteFlowRates[name], 1.0f, api.LoadTypesFromFile(metalsWithoutCopper));
                    }
                    ImGui.Unindent();
                }
                if (ImGui.CollapsingHeader(Lang.Get(settingsChuteQuantitySlots) + $"##settingsChuteQuantitySlots-{id}"))
                {
                    ImGui.Indent();
                    foreach (string name in config.ChuteQuantitySlots.Keys.Where(name => ImGui.CollapsingHeader(name + $"##chutequantityslots-{id}")))
                    {
                        DictionaryEditor(config.ChuteQuantitySlots[name], name == nameHopper ? 4 : 1, api.LoadTypesFromFile(metalsWithoutCopper));
                    }
                    ImGui.Unindent();
                }
                if (ImGui.CollapsingHeader(Lang.Get(settingsChuteCheckRateMs) + $"##settingsChuteCheckRateMs-{id}"))
                {
                    ImGui.Indent();
                    foreach (string name in config.ChuteCheckRateMs.Keys.Where(name => ImGui.CollapsingHeader(name + $"##checkratems-{id}")))
                    {
                        DictionaryEditor(config.ChuteCheckRateMs[name], 500, api.LoadTypesFromFile(metalsWithoutCopper));
                    }
                    ImGui.Unindent();
                }
                if (ImGui.CollapsingHeader(Lang.Get(settingsChuteCraftable) + $"##settingsChuteCraftable-{id}"))
                {
                    ImGui.Indent();
                    foreach (string name in config.ChuteCraftable.Keys.Where(name => ImGui.CollapsingHeader(name + $"##craftablechutes-{id}")))
                    {
                        DictionaryEditor(config.ChuteCraftable[name], true, api.LoadTypesFromFile(metalsWithoutCopper));
                    }
                    ImGui.Unindent();
                }
                ImGui.Unindent();
            }
            if (ImGui.CollapsingHeader(Lang.Get(settingsQuantitySlotsChest) + $"##settingsQuantitySlotsChest-{id}"))
            {
                config.OverrideChestQuantitySlots = OnCheckBox($"overrideChestQuantitySlots-{id}", config.OverrideChestQuantitySlots, nameof(config.OverrideChestQuantitySlots));
                ImGui.NewLine();
                DictionaryEditor(config.ChestQuantitySlots, 16, api.LoadTypesFromBlocks(api.World.GetBlock(AssetLocation.Create(vanvarChest)), api.World.GetBlock(AssetLocation.Create(wtChest))));
            }
            if (ImGui.CollapsingHeader(Lang.Get(settingsQuantitySlotsTrunk) + $"##settingsQuantitySlotsTrunk-{id}"))
            {
                config.OverrideDoubleChestQuantitySlots = OnCheckBox($"overrideDoubleChestQuantitySlots-{id}", config.OverrideDoubleChestQuantitySlots, nameof(config.OverrideDoubleChestQuantitySlots));
                ImGui.NewLine();
                DictionaryEditor(config.DoubleChestQuantitySlots, 36, api.LoadTypesFromBlocks(api.World.GetBlock(AssetLocation.Create(vanvarTrunk)), api.World.GetBlock(AssetLocation.Create(wtTrunk))));
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

    private void DictionaryEditor<T>(Dictionary<string, T> dict, T defaultValue = default, string[] possibleValues = null)
    {
        if (ImGui.BeginTable("dict", 3, ImGuiTableFlags.BordersOuter))
        {
            for (int row = 0; row < dict.Count; row++)
            {
                ImGui.TableNextRow();
                string key = dict.Keys.ElementAt(row);
                string prevKey = (string)key.Clone();
                T value = dict.Values.ElementAt(row);
                ImGui.TableNextColumn();
                ImGui.InputText($"##row-key-{row}", ref key, 300);
                if (prevKey != key)
                {
                    dict.Remove(prevKey);
                    dict.TryAdd(key, value);
                    value = dict.Values.ElementAt(row);
                }
                ImGui.TableNextColumn();
                if (typeof(T) == typeof(int))
                {
                    int intValue = Convert.ToInt32(value);
                    ImGui.InputInt($"##row-value-{row}", ref intValue);
                    value = (T)Convert.ChangeType(intValue, typeof(T));
                }
                else if (typeof(T) == typeof(float))
                {
                    float floatValue = Convert.ToSingle(value);
                    ImGui.InputFloat($"##row-value-{row}", ref floatValue);
                    value = (T)Convert.ChangeType(floatValue, typeof(T));
                }
                else if (typeof(T) == typeof(bool))
                {
                    bool boolValue = Convert.ToBoolean(value);
                    ImGui.Checkbox($"##row-value-{row}", ref boolValue);
                    value = (T)Convert.ChangeType(boolValue, typeof(T));
                }
                dict[key] = value;
                ImGui.TableNextColumn();
                if (ImGui.Button($"Remove##row-value-{row}"))
                {
                    dict.Remove(key);
                }
            }
            ImGui.TableNextRow();
            ImGui.TableNextColumn();
            if (ImGui.Button("Add"))
            {
                int id = dict.Count;
                string newKey = possibleValues?.FirstOrDefault(x => !dict.ContainsKey(x), null);
                if ((newKey != null || dict.Count == 0) && !dict.ContainsKey(newKey))
                {
                    dict.TryAdd(newKey, defaultValue);
                }
                else
                {
                    while (dict.ContainsKey($"row {id}")) id++;
                    dict.TryAdd($"row {id}", defaultValue);
                }
            }
            ImGui.TableNextColumn();
            ImGui.TableNextColumn();
            ImGui.EndTable();
        }
    }
}