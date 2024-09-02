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
}
