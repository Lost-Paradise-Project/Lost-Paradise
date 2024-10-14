using System.Linq;
using Content.Shared.Preferences;
using Content.Shared.Roles;
using JetBrains.Annotations;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.Customization.Systems;


/// <summary>
///    Requires all of the requirements to be true
/// </summary>
[UsedImplicitly]
[Serializable, NetSerializable]
public sealed partial class CharacterLogicAndRequirement : CharacterRequirement
{
    [DataField]
    public List<CharacterRequirement> Requirements { get; private set; } = new();

#if LPP_Sponsors
    public override bool IsValid(JobPrototype job, HumanoidCharacterProfile profile,
        Dictionary<string, TimeSpan> playTimes, bool whitelisted, IPrototype prototype,
        IEntityManager entityManager, IPrototypeManager prototypeManager, IConfigurationManager configManager,
        out FormattedMessage? reason, int depth = 0) => IsValid(job, profile, playTimes, whitelisted, prototype, entityManager, prototypeManager, configManager, out reason, depth, 0);
#endif
    public override bool IsValid(JobPrototype job, HumanoidCharacterProfile profile,
        Dictionary<string, TimeSpan> playTimes, bool whitelisted, IPrototype prototype,
        IEntityManager entityManager, IPrototypeManager prototypeManager, IConfigurationManager configManager,
        out FormattedMessage? reason, int depth = 0
#if LPP_Sponsors
        , int sponsorTier = 0
#endif
        )
    {
        var succeeded = entityManager.EntitySysManager.GetEntitySystem<CharacterRequirementsSystem>()
            .CheckRequirementsValid(Requirements, job, profile, playTimes, whitelisted, prototype, entityManager,
                prototypeManager, configManager, out var reasons, depth + 1
#if LPP_Sponsors
            , sponsorTier = 0
#endif
                );

        if (reasons.Count == 0)
        {
            reason = null;
            return succeeded;
        }

        reason = new FormattedMessage();
        foreach (var message in reasons)
            reason.AddMessage(FormattedMessage.FromMarkup(
                Loc.GetString("character-logic-and-requirement-listprefix", ("indent", new string(' ', depth * 2))) + message.ToMarkup()));
        reason = FormattedMessage.FromMarkup(Loc.GetString("character-logic-and-requirement",
            ("inverted", Inverted), ("options", reason.ToMarkup())));

        return succeeded;
    }
}

/// <summary>
///     Requires any of the requirements to be true
/// </summary>
[UsedImplicitly]
[Serializable, NetSerializable]
public sealed partial class CharacterLogicOrRequirement : CharacterRequirement
{
    [DataField]
    public List<CharacterRequirement> Requirements { get; private set; } = new();

#if LPP_Sponsors
    public override bool IsValid(JobPrototype job, HumanoidCharacterProfile profile,
        Dictionary<string, TimeSpan> playTimes, bool whitelisted, IPrototype prototype,
        IEntityManager entityManager, IPrototypeManager prototypeManager, IConfigurationManager configManager,
        out FormattedMessage? reason, int depth = 0) => IsValid(job, profile, playTimes, whitelisted, prototype, entityManager, prototypeManager, configManager, out reason, depth, 0);
#endif

    public override bool IsValid(JobPrototype job, HumanoidCharacterProfile profile,
        Dictionary<string, TimeSpan> playTimes, bool whitelisted, IPrototype prototype,
        IEntityManager entityManager, IPrototypeManager prototypeManager, IConfigurationManager configManager,
        out FormattedMessage? reason, int depth = 0
#if LPP_Sponsors
        , int sponsorTier = 0
#endif
        )
    {
        var succeeded = false;
        var reasons = new List<FormattedMessage>();
        var characterRequirements = entityManager.EntitySysManager.GetEntitySystem<CharacterRequirementsSystem>();

        foreach (var requirement in Requirements)
        {
            var validation = false;
            FormattedMessage? raisin;
#if LPP_Sponsors
            if (requirement is CharacterDepartmentTimeRequirement ||
                requirement is CharacterOverallTimeRequirement ||
                requirement is CharacterPlaytimeRequirement ||
                requirement is CharacterSponsorRequirement
                )
                validation = characterRequirements.CheckRequirementValid(requirement, job, profile, playTimes, whitelisted, prototype,
                entityManager, prototypeManager, configManager, out raisin, depth + 1, sponsorTier);
            else
                validation = characterRequirements.CheckRequirementValid(requirement, job, profile, playTimes, whitelisted, prototype,
                entityManager, prototypeManager, configManager, out raisin, depth + 1);
#else
            validation = characterRequirements.CheckRequirementValid(requirement, job, profile, playTimes, whitelisted, prototype,
                entityManager, prototypeManager, configManager, out raisin, depth + 1);
#endif

            if (validation)
            {
                succeeded = true;
                break;
            }

            if (raisin != null)
                reasons.Add(raisin);
        }

        if (reasons.Count == 0)
        {
            reason = null;
            return succeeded;
        }

        reason = new FormattedMessage();
        foreach (var message in reasons)
            reason.AddMessage(FormattedMessage.FromMarkup(
                Loc.GetString("character-logic-or-requirement-listprefix", ("indent", new string(' ', depth * 2))) + message.ToMarkup()));
        reason = FormattedMessage.FromMarkup(Loc.GetString("character-logic-or-requirement",
            ("inverted", Inverted), ("options", reason.ToMarkup())));

        return succeeded;
    }
}

/// <summary>
///     Requires only one of the requirements to be true
/// </summary>
[UsedImplicitly]
[Serializable, NetSerializable]
public sealed partial class CharacterLogicXorRequirement : CharacterRequirement
{
    [DataField]
    public List<CharacterRequirement> Requirements { get; private set; } = new();

#if LPP_Sponsors
    public override bool IsValid(JobPrototype job, HumanoidCharacterProfile profile,
        Dictionary<string, TimeSpan> playTimes, bool whitelisted, IPrototype prototype,
        IEntityManager entityManager, IPrototypeManager prototypeManager, IConfigurationManager configManager,
        out FormattedMessage? reason, int depth = 0) => IsValid(job, profile, playTimes, whitelisted, prototype, entityManager, prototypeManager, configManager, out reason, depth, 0);
#endif
    public override bool IsValid(JobPrototype job, HumanoidCharacterProfile profile,
        Dictionary<string, TimeSpan> playTimes, bool whitelisted, IPrototype prototype,
        IEntityManager entityManager, IPrototypeManager prototypeManager, IConfigurationManager configManager,
        out FormattedMessage? reason, int depth = 0
#if LPP_Sponsors
        , int sponsorTier = 0
#endif
        )
    {
        var reasons = new List<FormattedMessage>();
        var succeeded = false;
        var characterRequirements = entityManager.EntitySysManager.GetEntitySystem<CharacterRequirementsSystem>();

        foreach (var requirement in Requirements)
        {
            var validation = false;
            FormattedMessage? raisin;
#if LPP_Sponsors
            if (requirement is CharacterDepartmentTimeRequirement ||
                requirement is CharacterOverallTimeRequirement ||
                requirement is CharacterPlaytimeRequirement ||
                requirement is CharacterSponsorRequirement
                )
                validation = characterRequirements.CheckRequirementValid(requirement, job, profile, playTimes, whitelisted, prototype,
                entityManager, prototypeManager, configManager, out raisin, depth + 1, sponsorTier);
            else
                validation = characterRequirements.CheckRequirementValid(requirement, job, profile, playTimes, whitelisted, prototype,
                entityManager, prototypeManager, configManager, out raisin, depth + 1);
#else
            validation = characterRequirements.CheckRequirementValid(requirement, job, profile, playTimes, whitelisted, prototype,
                entityManager, prototypeManager, configManager, out raisin, depth + 1);
#endif
            if (validation)
            {
                if (succeeded)
                {
                    succeeded = false;
                    break;
                }

                succeeded = true;
            }

            if (raisin != null)
                reasons.Add(raisin);
        }

        if (reasons.Count == 0)
        {
            reason = null;
            return succeeded;
        }

        reason = new FormattedMessage();
        foreach (var message in reasons)
            reason.AddMessage(FormattedMessage.FromMarkup(
                Loc.GetString("character-logic-xor-requirement-listprefix", ("indent", new string(' ', depth * 2))) + message.ToMarkup()));
        reason = FormattedMessage.FromMarkup(Loc.GetString("character-logic-xor-requirement",
            ("inverted", Inverted), ("options", reason.ToMarkup())));

        return succeeded;
    }
}
