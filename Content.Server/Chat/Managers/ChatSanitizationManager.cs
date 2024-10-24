using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;

namespace Content.Server.Chat.Managers;

public sealed class ChatSanitizationManager : IChatSanitizationManager
{
    [Dependency] private readonly IConfigurationManager _configurationManager = default!;

    private static readonly Dictionary<string, string> SmileyToEmote = new()
    {
        // Corvax-Localization-Start
        { "хд", "chatsan-laughs" },
        { "хд.", "chatsan-laughs" },
        { "о-о", "chatsan-wide-eyed" }, // cyrillic о нет
        { "о.о", "chatsan-wide-eyed" }, // cyrillic о нет
        { "0_о", "chatsan-wide-eyed" }, // cyrillic о нет
        { "о/", "chatsan-waves" }, // cyrillic о
        { "о7", "chatsan-salutes" }, // cyrillic о
        { "0_o", "chatsan-wide-eyed" }, // нет
        { "лмао", "chatsan-laughs" },
        { "лмао.", "chatsan-laughs" },
        { "рофл", "chatsan-laughs" },
        { "рофл.", "chatsan-laughs" },
        { "яхз", "chatsan-shrugs" },
        { "яхз.", "chatsan-shrugs" },
        { ":0", "chatsan-surprised" },
        { ":р", "chatsan-stick-out-tongue" }, // cyrillic р . в канал гарнитуры
        { "кек", "chatsan-laughs" },
        { "кек.", "chatsan-laughs" },
        { "T_T", "chatsan-cries" }, // нет
        { "Т_Т", "chatsan-cries" }, // cyrillic T нет
        { "=_(", "chatsan-cries" },
        { "!с", "chatsan-laughs" },
        { "!с.", "chatsan-laughs" },
        { "!в", "chatsan-sighs" },
        { "!в.", "chatsan-sighs" },
        { "!х", "chatsan-claps" },
        { "!х.", "chatsan-claps" },
        { "!щ", "chatsan-snaps" },
        { "!щ.", "chatsan-snaps" },
        { "))", "chatsan-smiles-widely" },
        { ")", "chatsan-smiles" },
        { "((", "chatsan-frowns-deeply" },
        { "(", "chatsan-frowns" },
        // Corvax-Localization-End
		// Lost-Paradise-Localization-Start
		{ "орууу", "chatsan-laughs" },
        { "орууу.", "chatsan-laughs" },
		{ "хз", "chatsan-shrugs" },
        { "хз.", "chatsan-shrugs" },
		{ "хсс", "chatsan-shrugs" },
        { "хсс.", "chatsan-shrugs" },
		{ "хссс", "chatsan-shrugs" },
        { "хссс.", "chatsan-shrugs" },
		{ "шшш", "chatsan-hiss" },
        { "шшш.", "chatsan-hiss" },
		{ "авууу", "chatsan-awoo" },
        { "авууу.", "chatsan-awoo" },
		{ "няяя", "chatsan-nyaaa" },
        { "няяя.", "chatsan-nyaaa" },
		// Lost-Paradise-Localization-End
        // I could've done this with regex, but felt it wasn't the right idea.
        { ":)", "chatsan-smiles" },
        { ":]", "chatsan-smiles" },
        { "=)", "chatsan-smiles" }, // = + улыбается
        { "=]", "chatsan-smiles" },
        { "(:", "chatsan-smiles" },
        { "[:", "chatsan-smiles" }, // : в ООС
        { "(=", "chatsan-smiles" },
        { "[=", "chatsan-smiles" },
        { "^^", "chatsan-smiles" },
        { "^-^", "chatsan-smiles" },
        { ":(", "chatsan-frowns" },
        { ":[", "chatsan-frowns" },
        { "=(", "chatsan-frowns" }, // = + хмурится
        { "=[", "chatsan-frowns" },
        { "):", "chatsan-frowns" },
        { ")=", "chatsan-frowns" },
        { "]:", "chatsan-frowns" }, // : в админ
        { "]=", "chatsan-frowns" },
        { ":D", "chatsan-smiles-widely" }, //нет канала с ключём , . в шёпот
        { "D:", "chatsan-frowns-deeply" },
        { ":O", "chatsan-surprised" }, // . в безопасность
        { ":3", "chatsan-smiles" }, //nope
        { ":S", "chatsan-uncertain" }, //нет канала с ключём, . в шёпот
        { ":>", "chatsan-grins" },
        { ":<", "chatsan-pouts" },
        { "xD", "chatsan-laughs" }, // нет
        { ":'(", "chatsan-cries" },
        { ":'[", "chatsan-cries" },
        { "='(", "chatsan-cries" }, // =' + хмурится
        { "='[", "chatsan-cries" },
        { ")':", "chatsan-cries" },
        { "]':", "chatsan-cries" }, // ': в админ чат
        { ")'=", "chatsan-cries" },
        { "]'=", "chatsan-cries" }, // '= в админ чат
        { ";-;", "chatsan-cries" },
        { ";_;", "chatsan-cries" },
        { "qwq", "chatsan-cries" }, // нет
        { ":u", "chatsan-smiles-smugly" }, // нет канала с ключём, . в шёпот
        { ":v", "chatsan-smiles-smugly" }, // нет канала с ключём, . в шёпот
        { ">:i", "chatsan-annoyed" }, // :i в чат
        { ":i", "chatsan-sighs" }, // нет канала с ключём, . в шёпот
        { ":|", "chatsan-sighs" },
        { ":p", "chatsan-stick-out-tongue" }, // нет канала с ключём, . в шёпот
        { ";p", "chatsan-stick-out-tongue" }, // Р в общий
        { ":b", "chatsan-stick-out-tongue" }, // нет канала с ключём, . в шёпот
        { "0-0", "chatsan-wide-eyed" }, // нет
        { "o-o", "chatsan-wide-eyed" }, // нет
        { "o.o", "chatsan-wide-eyed" }, // нет
        { "._.", "chatsan-surprised" }, 
        { ".-.", "chatsan-confused" },
        { "-_-", "chatsan-unimpressed" },
        { "smh", "chatsan-unimpressed" }, // нет
        { "o/", "chatsan-waves" },
        { "^^/", "chatsan-waves" },
        { ":/", "chatsan-uncertain" },
        { ":\\", "chatsan-uncertain" }, // нет канала с \
        { "lmao", "chatsan-laughs" },
        { "lmao.", "chatsan-laughs" },
        { "lol", "chatsan-laughs" },
        { "lol.", "chatsan-laughs" },
        { "lel", "chatsan-laughs" },
        { "lel.", "chatsan-laughs" },
        { "kek", "chatsan-laughs" },
        { "kek.", "chatsan-laughs" },
        { "rofl", "chatsan-laughs" }, // нет
        { "o7", "chatsan-salutes" },
        { ";_;7", "chatsan-tearfully-salutes"},
        { "idk", "chatsan-shrugs" },
        { "idk.", "chatsan-shrugs" },
        { ";)", "chatsan-winks" },
        { ";]", "chatsan-winks" },
        { "(;", "chatsan-winks" },
        { "[;", "chatsan-winks" }, // ; в OOC
        { ":')", "chatsan-tearfully-smiles" },
        { ":']", "chatsan-tearfully-smiles" },
        { "=')", "chatsan-tearfully-smiles" }, // =' + улыбается
        { "=']", "chatsan-tearfully-smiles" },
        { "(':", "chatsan-tearfully-smiles" },
        { "[':", "chatsan-tearfully-smiles" }, // ': в OOC
        { "('=", "chatsan-tearfully-smiles" },
        { "['=", "chatsan-tearfully-smiles" }, // '= в админ чат
    };

    private bool _doSanitize;

    public void Initialize()
    {
        _configurationManager.OnValueChanged(CCVars.ChatSanitizerEnabled, x => _doSanitize = x, true);
    }

    public bool TrySanitizeOutSmilies(string input, EntityUid speaker, out string sanitized, [NotNullWhen(true)] out string? emote)
    {
        if (!_doSanitize)
        {
            sanitized = input;
            emote = null;
            return false;
        }

        input = input.TrimEnd();

        foreach (var (smiley, replacement) in SmileyToEmote)
        {
            if (input.EndsWith(smiley, true, CultureInfo.InvariantCulture))
            {
                sanitized = input.Remove(input.Length - smiley.Length).TrimEnd();
                emote = Loc.GetString(replacement, ("ent", speaker));
                return true;
            }
        }

        sanitized = input;
        emote = null;
        return false;
    }
}
