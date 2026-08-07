namespace Content.Shared._Paradise.Banner;

[RegisterComponent]
public sealed partial class BannerComponent : Component
{
    [DataField]
    public BannerVisualsState State = BannerVisualsState.Unrolled;

    [DataField]
    public bool Burning = false;
}
