using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Database.Implementations.Entities;
using Jellyfin.Database.Implementations.Enums;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Model.Configuration;
using MediaBrowser.Model.MediaSegments;

namespace MediaBrowser.Controller.MediaSegments;

/// <summary>
/// Defines methods for interacting with media segments.
/// </summary>
public interface IMediaSegmentManager
{
    /// <summary>
    /// Uses all segment providers enabled for the <see cref="BaseItem"/>'s library to get the Media Segments.
    /// </summary>
    /// <param name="baseItem">The Item to evaluate.</param>
    /// <param name="libraryOptions">The library options.</param>
    /// <param name="forceOverwrite">If set, will force to remove existing segments and replace it with new ones otherwise will check for existing segments and if found any that should not be deleted, stops.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that indicates the Operation is finished.</returns>
    Task RunSegmentPluginProviders(BaseItem baseItem, LibraryOptions libraryOptions, bool forceOverwrite, CancellationToken cancellationToken);

    /// <summary>
    /// Returns if this item supports media segments.
    /// </summary>
    /// <param name="baseItem">The base Item to check.</param>
    /// <returns>True if supported otherwise false.</returns>
    bool IsTypeSupported(BaseItem baseItem);

    /// <summary>
    /// Creates a new Media Segment associated with an Item.
    /// </summary>
    /// <param name="mediaSegment">The segment to create.</param>
    /// <param name="segmentProviderId">The id of the Provider who created this segment.</param>
    /// <returns>The created Segment entity.</returns>
    Task<MediaSegmentDto> CreateSegmentAsync(MediaSegmentDto mediaSegment, string segmentProviderId);

    /// <summary>
    /// Deletes a single media segment.
    /// </summary>
    /// <param name="segmentId">The <see cref="MediaSegment.Id"/> to delete.</param>
    /// <returns>a task.</returns>
    Task DeleteSegmentAsync(Guid segmentId);

    /// <summary>
    /// Deletes all media segments of an item.
    /// </summary>
    /// <param name="itemId">The <see cref="BaseItem.Id"/> to delete all segments for.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>a task.</returns>
    Task DeleteSegmentsAsync(Guid itemId, CancellationToken cancellationToken);

    /// <summary>
    /// Obtains all segments associated with the itemId.
    /// </summary>
    /// <param name="item">The <see cref="BaseItem"/>.</param>
    /// <param name="typeFilter">filters all media segments of the given type to be included. If null all types are included.</param>
    /// <param name="libraryOptions">The library options.</param>
    /// <param name="filterByProvider">When set filters the segments to only return those that which providers are currently enabled on their library.</param>
    /// <returns>An enumerator of <see cref="MediaSegmentDto"/>'s.</returns>
    Task<IEnumerable<MediaSegmentDto>> GetSegmentsAsync(BaseItem item, IEnumerable<MediaSegmentType>? typeFilter, LibraryOptions libraryOptions, bool filterByProvider = true);

    /// <summary>
    /// Gets information about any media segments stored for the given itemId.
    /// </summary>
    /// <param name="itemId">The id of the <see cref="BaseItem"/>.</param>
    /// <returns>True if there are any segments stored for the item, otherwise false.</returns>
    /// TODO: this should be async but as the only caller BaseItem.GetVersionInfo isn't async, this is also not. Venson.
    bool HasSegments(Guid itemId);

    /// <summary>
    /// Gets a list of all registered Segment Providers and their IDs.
    /// </summary>
    /// <param name="item">The media item that should be tested for providers.</param>
    /// <returns>A list of all providers for the tested item.</returns>
    IEnumerable<(string Name, string Id)> GetSupportedProviders(BaseItem item);
}
