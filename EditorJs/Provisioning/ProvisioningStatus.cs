namespace EditorJs.Provisioning;

/// <summary>
/// Carries a provisioning state update from an editor instance to the <see cref="ProvisioningMonitor"/>.
/// </summary>
public sealed class ProvisioningStatus
{
    /// <summary>
    /// The unique identifier of the editor instance reporting its state.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// The provisioning state being reported.
    /// </summary>
    public required ProvisioningState State { get; set; }
}
