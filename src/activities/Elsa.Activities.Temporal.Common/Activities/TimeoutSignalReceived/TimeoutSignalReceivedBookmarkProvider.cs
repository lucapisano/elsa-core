using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Services;
using Elsa.Services.Bookmarks;

// ReSharper disable once CheckNamespace
namespace Elsa.Activities.Signaling
{

    public class TimeoutSignalReceivedBookmarkProvider : BookmarkProvider<SignalReceivedBookmark, TimeoutSignalReceived>
    {
        public override async ValueTask<IEnumerable<BookmarkResult>> GetBookmarksAsync(BookmarkProviderContext<TimeoutSignalReceived> context, CancellationToken cancellationToken) => await GetBookmarksInternalAsync(context, cancellationToken).ToListAsync(cancellationToken);

        private async IAsyncEnumerable<BookmarkResult> GetBookmarksInternalAsync(BookmarkProviderContext<TimeoutSignalReceived> context, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var signalName = (await context.ReadActivityPropertyAsync(x => x.Signal, cancellationToken))?.ToLowerInvariant().Trim();
            
            // Can't do anything with an empty signal name.
            if(string.IsNullOrEmpty(signalName))
                yield break;
            
            yield return Result(new SignalReceivedBookmark
            {
                Signal = signalName
            });
        }
    }
}