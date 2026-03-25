namespace EditorJs.Provisioning;

/// <summary>
/// Tracks the accumulated provisioning state of a single editor instance using bitwise flags.
/// </summary>
public sealed class ProvisioningTrackEntry
{
    /// <summary>
    /// Creates a new entry initialised with the specified state.
    /// </summary>
    /// <param name="state">The initial provisioning state.</param>
    public static ProvisioningTrackEntry Entry(ProvisioningState state) => new()
    {
        State = state
    };

    /// <summary>
    /// The combined provisioning state flags for this editor instance.
    /// </summary>
    public required ProvisioningState State { get; set; }

    /// <summary>
    /// Records an additional provisioning state using bitwise OR.
    /// </summary>
    /// <param name="state">The state flag to add.</param>
    /// <returns>This entry for chaining.</returns>
    public ProvisioningTrackEntry RecordState(ProvisioningState state)
    {
        State |= state;
        return this;
    }
}
