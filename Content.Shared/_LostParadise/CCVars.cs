using Robust.Shared.Configuration;

namespace Content.Shared._LostParadise.CCVar;

[CVarDefs]
public sealed partial class AccVars
{
    public static readonly CVarDef<string> DiscordBanWebhook =
        CVarDef.Create("discord.ban_webhook", "", CVar.SERVERONLY);
}
