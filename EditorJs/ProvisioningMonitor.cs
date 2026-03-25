namespace EditorJs;

/// <summary>
/// Tracks the provisioning lifecycle of all editor instances on a page.
/// Registered as a transient service via <see cref="Extensions.AddScopedEditorJsInterop"/>.
/// </summary>
public sealed class ProvisioningMonitor
{
    /// <summary>
    /// Dictionary mapping editor IDs to their accumulated provisioning state.
    /// </summary>
    public Dictionary<string, ProvisioningTrackEntry> Tracker { get; } = [];

    /// <summary>
    /// Combined state representing a fully provisioned editor: Constructed, Initialised, and Rendered.
    /// </summary>
    public static readonly ProvisioningState FullState = ProvisioningState.Constructed | ProvisioningState.Initialised | ProvisioningState.Rendered;

    /// <summary>
    /// Combined state representing an editor that is constructed and initialised (ready for interaction, but not yet rendered with data).
    /// </summary>
    public static readonly ProvisioningState InitialisedState = ProvisioningState.Constructed | ProvisioningState.Initialised;

    /// <summary>
    /// Used for the <c>ProvisioningCallbacks</c> of an editor to track its loading and active states.
    /// </summary>
    /// <param name="value">The provisioning status update from an editor instance.</param>
    /// <returns>A completed task.</returns>
    public Task EventsCallbacksHandlerAsync(ProvisioningStatus value)
    {
        if (Tracker.TryGetValue(value.Id, out ProvisioningTrackEntry? existing_entry) is true)
        {
            existing_entry.RecordState(value.State);
        }
        else
        {
            Tracker[value.Id] = ProvisioningTrackEntry.Entry(value.State);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Checks that all tracked editors have reached the specified provisioning state(s).
    /// </summary>
    /// <param name="state">The required provisioning state flags to check for.</param>
    /// <returns><c>true</c> if all tracked editors have the specified state; <c>false</c> if any are missing it or no editors are tracked.</returns>
    public bool CheckEditorsProvisioningStates(ProvisioningState state)
    {
        foreach (ProvisioningTrackEntry entry in Tracker.Values)
        {
            if ((entry.State & state) != state)
            {
                return false;
            }
        }

        return Tracker.Values.Count is not 0;
    }

    /// <summary>
    /// Checks if all entries in the Tracker have their ProvisioningState fully applied.
    /// </summary>
    public bool AreAllEditorsFullyProvisioned()
        => CheckEditorsProvisioningStates(FullState);
}
