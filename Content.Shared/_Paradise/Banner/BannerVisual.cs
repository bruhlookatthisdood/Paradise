using Robust.Shared.Serialization;

namespace Content.Shared._Paradise.Banner;

[Serializable, NetSerializable]
public enum BannerVisuals : byte
{
    State,
}

[Serializable, NetSerializable]
public enum BannerVisualState : byte
{
    Unrolled,
    Rolled,
    Burning,
}
