using Content.Shared.Dataset;
using Content.Shared.Bed.Sleep;
using Content.Server.Chat.Managers;
using Robust.Shared.Random;
using Robust.Shared.Prototypes;
using Robust.Shared.Player;
using System.Threading.Tasks;
using Content.Server.Mood;
using Robust.Server.Player;

namespace Content.Server.Psionics.Dreams
{
    public sealed class DreamsSystem : EntitySystem // Redacted by Silly Elf Farrellka~ <3
    {
        [Dependency] private readonly IRobustRandom _random = default!;
        [Dependency] private readonly IChatManager _chatManager = default!;
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

        private float _accumulator = 0f;
        private float _updateRate = 15f;

        public readonly IReadOnlyList<string> DreamSetPrototypes = new[]
        {
            "dreamadjectives",
            "dreamadverbs",
            "dreamingverbs",
            "dreamstrings",
            "dreamverbs",
            "nightmare"
        };

        public override void Update(float frameTime)
        {
            _accumulator += frameTime;
            if (_accumulator < _updateRate)
                return;

            _accumulator -= _updateRate;
            _updateRate = _random.NextFloat(30f, 60f);

            foreach (var sleeper in EntityQuery<SleepingComponent>())
            {
                if (!TryComp<ActorComponent>(sleeper.Owner, out var actor) || !TryComp<MoodComponent>(sleeper.Owner, out var mood))
                    continue;

                var dreamFragments = GenerateDreamOrNightmare(mood);
                ShowDreamFragmentsAsync(dreamFragments, sleeper.Owner, actor, mood);
            }
        }

        private List<string> GenerateDreamOrNightmare(MoodComponent mood)
        {
            var dreamFragments = new List<string>();

            if (mood.CurrentMoodLevel <= 40 && _random.Prob(0.5f))
            {
                dreamFragments.Add(Loc.GetString("chat-manager-dream-see"));

                dreamFragments.Add(ProcessAdjective(GetRandomFragment("nightmare")));
                dreamFragments.Add(_random.Prob(0.5f)
                    ? (_random.Prob(0.35f) ? GetRandomFragment("dreamadverbs") + " " : "") + GetRandomFragment("dreamingverbs")
                    : Loc.GetString("chat-manager-dream-willbe") + " " + GetRandomFragment("dreamverbs"));

                dreamFragments.Add(ProcessAdjective(GetRandomFragment("nightmare")));
            }
            else
            {
                dreamFragments.Add(Loc.GetString("chat-manager-dream-see"));
                dreamFragments.Add(ProcessAdjective(GetRandomFragment("dreamstrings")));
                dreamFragments.Add(_random.Prob(0.5f)
                    ? (_random.Prob(0.35f) ? GetRandomFragment("dreamadverbs") + " " : "") + GetRandomFragment("dreamingverbs")
                    : Loc.GetString("chat-manager-dream-willbe") + " " + GetRandomFragment("dreamverbs"));

                dreamFragments.Add(ProcessAdjective(GetRandomFragment("dreamstrings")));
            }

            return dreamFragments;
        }

        private string GetRandomFragment(string setName) =>
            _prototypeManager.TryIndex<DatasetPrototype>(setName, out var set) ? _random.Pick(set.Values) : "";

        private string ProcessAdjective(string fragment)
        {
            if (_random.Prob(0.5f))
            {
                var adjective = GetRandomFragment("dreamadjectives");
                fragment = fragment.Replace("%ADJECTIVE%", adjective);
            }
            else
            {
                fragment = fragment.Replace("%ADJECTIVE% ", "");
            }
            return fragment.Replace("%A% ", "\a ");
        }

        private async void ShowDreamFragmentsAsync(List<string> fragments, EntityUid sleeper, ActorComponent actor, MoodComponent mood)
        {
            foreach (var fragment in fragments)
            {
                if (!EntityManager.HasComponent<SleepingComponent>(sleeper))
                    break;

                var messageColor = mood.CurrentMoodLevel <= 40 ? Color.DarkRed : Color.BlueViolet;
                _chatManager.ChatMessageToOne(Shared.Chat.ChatChannel.Telepathic, fragment, Loc.GetString("chat-manager-send-dream-chat-wrap-message", ("message", fragment)), sleeper, false, actor.PlayerSession.Channel, messageColor);

                await Task.Delay((int)_random.NextFloat(5000f, 10000f));
            }
        }
    }
}
