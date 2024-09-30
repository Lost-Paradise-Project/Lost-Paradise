using System.Text.RegularExpressions;
using Content.Server.Speech.Components;

namespace Content.Server.Speech.EntitySystems;

public sealed class ItalianAccentSystem : EntitySystem
{
    [Dependency] private readonly ReplacementAccentSystem _replacement = default!;
    
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ItalianAccentComponent, AccentGetEvent>(OnAccent);
    }

    private void OnAccent(EntityUid uid, ItalianAccentComponent component, AccentGetEvent args)
    {
        var message = args.Message;

        message = _replacement.ApplyReplacements(message, "italian");

        //They shoulda started runnin' an' hidin' from me!
        message = Regex.Replace(message, @"ing\b", "in'");
        message = Regex.Replace(message, @"\band\b", "an'");
        message = Regex.Replace(message, "d've", "da");
        args.Message = message;
    }
};