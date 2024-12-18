using Robust.Shared.Configuration;

namespace Content.Shared._LostParadise.CCVar;

[CVarDefs]
public sealed partial class AccVars
{
    public static readonly CVarDef<string> DiscordBanWebhook =
        CVarDef.Create("discord.ban_webhook", "", CVar.SERVERONLY);

    /// <summary>
    ///     URL of the sponsors server API.
    /// </summary>
    public static readonly CVarDef<string> SponsorsApiUrl =
    CVarDef.Create("sponsor.api_url", "", CVar.SERVERONLY);

    /// <summary>
    /// Cooldown for missions.
    /// </summary>
    public static readonly CVarDef<float>
        SalvageExpeditionCooldown = CVarDef.Create("lpsalvage.expedition_cooldown", 780f, CVar.REPLICATED);

    /// <summary>
    /// Cooldown for failed missions.
    /// </summary>
    public static readonly CVarDef<float> SalvageExpeditionFailedCooldown =
        CVarDef.Create("lpsalvage.expedition_failed_cooldown", 1200f, CVar.REPLICATED);

    /// <summary>
    /// The maximum number of shuttles able to go on expedition at once.
    /// </summary>
    public static readonly CVarDef<int> SalvageExpeditionMaxActive =
        CVarDef.Create("nf14.salvage.expedition_max_active", 15, CVar.REPLICATED);

}
