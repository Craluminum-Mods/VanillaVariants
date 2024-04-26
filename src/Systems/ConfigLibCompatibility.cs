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
        #region Simple Settings
        if (ImGui.CollapsingHeader(Lang.Get("vanvar:Config.SettingsSimple") + $"##{id}"))
        {
            ImGui.TextWrapped(Lang.Get("Experimental"));
            config.ExperimentalOverlayTest = OnCheckBox(id, config.ExperimentalOverlayTest, nameof(config.ExperimentalOverlayTest));
            ImGui.NewLine();
            ImGui.TextWrapped(Lang.Get("Issues"));
            config.ResolveBarrelSounds = OnCheckBox(id, config.ResolveBarrelSounds, nameof(config.ResolveBarrelSounds));
            config.ResolveChestNames = OnCheckBox(id, config.ResolveChestNames, nameof(config.ResolveChestNames));
            config.ResolveHenboxImposter = OnCheckBox(id, config.ResolveHenboxImposter, nameof(config.ResolveHenboxImposter));
            config.ResolveMechanicalBlockIssues = OnCheckBox(id, config.ResolveMechanicalBlockIssues, nameof(config.ResolveMechanicalBlockIssues));
            config.ResolveQuernAndAxleRelationship = OnCheckBox(id, config.ResolveQuernAndAxleRelationship, nameof(config.ResolveQuernAndAxleRelationship));
            config.CraftableCage = OnCheckBox(id, config.CraftableCage, nameof(config.CraftableCage));
            config.CraftableWagonWheels = OnCheckBox(id, config.CraftableWagonWheels, nameof(config.CraftableWagonWheels));
            config.CraftableWoodenRails = OnCheckBox(id, config.CraftableWoodenRails, nameof(config.CraftableWoodenRails));
            ImGui.NewLine();
            ImGui.TextWrapped(Lang.Get("Blocks and Items"));
            config.Cage = OnCheckBox(id, config.Cage, nameof(config.Cage));
            config.ArmorStand = OnCheckBox(id, config.ArmorStand, nameof(config.ArmorStand));
            config.Barrel = OnCheckBox(id, config.Barrel, nameof(config.Barrel));
            config.Bed = OnCheckBox(id, config.Bed, nameof(config.Bed));
            config.Chair = OnCheckBox(id, config.Chair, nameof(config.Chair));
            config.Chest = OnCheckBox(id, config.Chest, nameof(config.Chest));
            config.CrudeDoor = OnCheckBox(id, config.CrudeDoor, nameof(config.CrudeDoor));
            config.DecorativeQuern = OnCheckBox(id, config.DecorativeQuern, nameof(config.DecorativeQuern));
            config.DisplayCase = OnCheckBox(id, config.DisplayCase, nameof(config.DisplayCase));
            config.Firewood = OnCheckBox(id, config.Firewood, nameof(config.Firewood));
            config.Forge = OnCheckBox(id, config.Forge, nameof(config.Forge));
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
            #region Mechanics
            ImGui.NewLine();
            ImGui.TextWrapped(Lang.Get("tabname-mechanics"));
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
            ImGui.TextWrapped(Lang.Get("Chutes"));
            config.ArchimedesScrew = OnCheckBox(id, config.ArchimedesScrew, nameof(config.ArchimedesScrew));
            config.Chute = OnCheckBox(id, config.Chute, nameof(config.Chute));
            config.ChuteSectionItem = OnCheckBox(id, config.ChuteSectionItem, nameof(config.ChuteSectionItem));
            config.Hopper = OnCheckBox(id, config.Hopper, nameof(config.Hopper));
            #endregion
        }
        #endregion

        #region Advanced Settings
        if (ImGui.CollapsingHeader(Lang.Get("vanvar:Config.SettingsAdvanced") + $"##{id}"))
        {
            ImGui.Indent();
            #region BlockEntityItemFlow Settings
            if (ImGui.CollapsingHeader(Lang.Get("vanvar:Config.Settings.BlockEntityItemFlow") + $"##{id}"))
            {
                ImGui.Indent();
                #region Flow Rates
                if (ImGui.CollapsingHeader(Lang.Get("vanvar:Config.Settings.FlowRates") + $"##{id}"))
                {
                    ImGui.Indent();
                    foreach (string name in config.FlowRates.Keys)
                    {
                        if (ImGui.CollapsingHeader(name + $"##flowrates-{id}"))
                        {
                            DictionaryEditor(config.FlowRates[name]);
                        }
                    }
                    ImGui.Unindent();
                }
                #endregion
                #region Quantity Slots
                if (ImGui.CollapsingHeader(Lang.Get("vanvar:Config.Settings.QuantitySlots") + $"##{id}"))
                {
                    ImGui.Indent();
                    foreach (string name in config.QuantitySlots.Keys)
                    {
                        if (ImGui.CollapsingHeader(name + $"##quantityslots-{id}"))
                        {
                            DictionaryEditor(config.QuantitySlots[name]);
                        }
                    }
                    ImGui.Unindent();
                }
                #endregion
                #region Check Rate Ms
                if (ImGui.CollapsingHeader(Lang.Get("vanvar:Config.Settings.CheckRateMs") + $"##{id}"))
                {
                    ImGui.Indent();
                    foreach (string name in config.CheckRateMs.Keys)
                    {
                        if (ImGui.CollapsingHeader(name + $"##checkratems-{id}"))
                        {
                            DictionaryEditor(config.CheckRateMs[name]);
                        }
                    }
                    ImGui.Unindent();
                }
                #endregion
                ImGui.Unindent();
            }
            #endregion
            #region Craftable Chutes
            if (ImGui.CollapsingHeader(Lang.Get("vanvar:Config.Settings.CraftableChutes") + $"##{id}"))
            {
                ImGui.Indent();
                foreach (string name in config.CraftableChutes.Keys)
                {
                    if (ImGui.CollapsingHeader(name + $"##craftablechutes-{id}"))
                    {
                        DictionaryEditor(config.CraftableChutes[name]);
                    }
                }
                ImGui.Unindent();
            }
            #endregion
            ImGui.Unindent();
        }
        #endregion
    }

    private bool OnCheckBox(string id, bool value, string name)
    {
        bool newValue = value;
        ImGui.Checkbox(Lang.Get("vanvar:Config.Setting." + name) + $"##{id}", ref newValue);
        return newValue;
    }

    private void DictionaryEditor<T>(Dictionary<string, T> dict)
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
                while (dict.ContainsKey($"row {id}")) id++;
                dict.TryAdd($"row {id}", default);
            }
            ImGui.TableNextColumn();
            ImGui.TableNextColumn();
            ImGui.EndTable();
        }
    }
}
