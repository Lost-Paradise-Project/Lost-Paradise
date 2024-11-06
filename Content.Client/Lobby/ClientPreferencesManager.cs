using System;
using System.Collections.Generic;
using System.Linq;
using Content.Shared.Preferences;
using Robust.Client;
using Robust.Client.Player;
using Robust.Shared.Configuration;
using Robust.Shared.IoC;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;
#if LPP_Sponsors  // _LostParadise-Sponsors
using Content.Client._LostParadise.Sponsors;
#endif

namespace Content.Client.Lobby
{
    /// <summary>
    ///     Receives <see cref="PlayerPreferences" /> and <see cref="GameSettings" /> from the server during the initial
    ///     connection.
    ///     Stores preferences on the server through <see cref="SelectCharacter" /> and <see cref="UpdateCharacter" />.
    /// </summary>
    public sealed class ClientPreferencesManager : IClientPreferencesManager
    {
        [Dependency] private readonly IClientNetManager _netManager = default!;
        [Dependency] private readonly IBaseClient _baseClient = default!;
        [Dependency] private readonly IConfigurationManager _cfg = default!;
        [Dependency] private readonly IPrototypeManager _prototypes = default!;
        [Dependency] private readonly IPlayerManager _playerManager = default!;
#if LPP_Sponsors  // _LostParadise-Sponsors
        [Dependency] private readonly SponsorsManager _sponsorsManager = default!;
#endif

        public event Action? OnServerDataLoaded;

        public GameSettings Settings { get; private set; } = default!;
        public PlayerPreferences Preferences { get; private set; } = default!;

        public void Initialize()
        {
            _netManager.RegisterNetMessage<MsgPreferencesAndSettings>(HandlePreferencesAndSettings);
            _netManager.RegisterNetMessage<MsgUpdateCharacter>();
            _netManager.RegisterNetMessage<MsgSelectCharacter>();
            _netManager.RegisterNetMessage<MsgDeleteCharacter>();

            _baseClient.RunLevelChanged += BaseClientOnRunLevelChanged;
        }

        private void BaseClientOnRunLevelChanged(object? sender, RunLevelChangedEventArgs e)
        {
            if (e.NewLevel == ClientRunLevel.Initialize)
            {
                Settings = default!;
                Preferences = default!;
            }
        }

        public void SelectCharacter(ICharacterProfile profile)
        {
            SelectCharacter(Preferences.IndexOfCharacter(profile));
        }

        public void SelectCharacter(int slot)
        {
            Preferences = new PlayerPreferences(Preferences.Characters, slot, Preferences.AdminOOCColor);
            var msg = new MsgSelectCharacter
            {
                SelectedCharacterIndex = slot
            };
            _netManager.ClientSendMessage(msg);
        }

        public void UpdateCharacter(ICharacterProfile profile, int slot)
        {
            var collection = IoCManager.Instance!;

#if LPP_Sponsors  // _LostParadise-Sponsors
            var allowedMarkings = _sponsorsManager.TryGetInfo(out var sponsor) ? sponsor.AllowedMarkings : Array.Empty<string>();
            if (allowedMarkings is null) // Somehow
                allowedMarkings = Array.Empty<string>();
            if (sponsor != null)
            {
                var tier = sponsor.Tier > 5 ? 5 : sponsor.Tier;
                var sponsorMarkings = Loc.GetString($"sponsor-markings-tier-{tier}").Split(";", StringSplitOptions.RemoveEmptyEntries);
                if (sponsorMarkings is not null && sponsorMarkings.Count() > 0)
                    allowedMarkings = allowedMarkings.Concat(sponsorMarkings).ToArray();
            }
            var session = _playerManager.LocalSession!;
            profile.EnsureValid(session, collection, allowedMarkings);
#else
            profile.EnsureValid(_playerManager.LocalSession!, collection);
#endif
            var characters = new Dictionary<int, ICharacterProfile>(Preferences.Characters) { [slot] = profile };
            Preferences = new PlayerPreferences(characters, Preferences.SelectedCharacterIndex, Preferences.AdminOOCColor);
            var msg = new MsgUpdateCharacter
            {
                Profile = profile,
                Slot = slot
            };
            _netManager.ClientSendMessage(msg);
        }

        public void CreateCharacter(ICharacterProfile profile)
        {
            var characters = new Dictionary<int, ICharacterProfile>(Preferences.Characters);
            var lowest = Enumerable.Range(0, Settings.MaxCharacterSlots)
                .Except(characters.Keys)
                .FirstOrNull();

            if (lowest == null)
            {
                throw new InvalidOperationException("Out of character slots!");
            }

            var l = lowest.Value;
            characters.Add(l, profile);
            Preferences = new PlayerPreferences(characters, Preferences.SelectedCharacterIndex, Preferences.AdminOOCColor);

            UpdateCharacter(profile, l);
        }

        public void DeleteCharacter(ICharacterProfile profile)
        {
            DeleteCharacter(Preferences.IndexOfCharacter(profile));
        }

        public void DeleteCharacter(int slot)
        {
            var characters = Preferences.Characters.Where(p => p.Key != slot);
            Preferences = new PlayerPreferences(characters, Preferences.SelectedCharacterIndex, Preferences.AdminOOCColor);
            var msg = new MsgDeleteCharacter
            {
                Slot = slot
            };
            _netManager.ClientSendMessage(msg);
        }

        private void HandlePreferencesAndSettings(MsgPreferencesAndSettings message)
        {
            Preferences = message.Preferences;
            Settings = message.Settings;

            OnServerDataLoaded?.Invoke();
        }
    }
}
