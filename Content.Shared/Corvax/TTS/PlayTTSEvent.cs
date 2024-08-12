using Robust.Shared.Serialization;
using Content.Shared.Language;

namespace Content.Shared.Corvax.TTS;

[Serializable, NetSerializable]
// ReSharper disable once InconsistentNaming
public sealed class PlayTTSEvent : EntityEventArgs
{
    public byte[] Data { get; }
    public NetEntity? SourceUid { get; }
    public bool IsWhisper { get; }

    public byte[] LanguageData { get; } // Languages TTS support
    public string LanguageProtoId { get; } // Languages TTS support

    public PlayTTSEvent(byte[] data, byte[] languageData, LanguagePrototype language, NetEntity? sourceUid = null, bool isWhisper = false)
    {
        Data = data;
        SourceUid = sourceUid;
        IsWhisper = isWhisper;
        LanguageProtoId = language.ID;  // Languages TTS support
        LanguageData = languageData;    // Languages TTS support
    }
}
