using Microsoft.JSInterop;

using System;
using System.Threading.Tasks;

namespace Blazor.Matomo
{
    public class JsInterop : IAsyncDisposable
    {
      private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

      public JsInterop(IJSRuntime jsRuntime)
      {
        _moduleTask = new Lazy<Task<IJSObjectReference>>(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/Blazor.Matomo/Blazor.Matomo.js")
            .AsTask());
      }

      public async ValueTask Init(string apiUrl, int siteId)
      {
        IJSObjectReference module = await _moduleTask.Value;
        await module.InvokeVoidAsync("init", apiUrl, siteId);
      }

      public async ValueTask TriggerPageView(string? relativeUrl, string? userId)
      {
        IJSObjectReference module = await _moduleTask.Value;
        await module.InvokeVoidAsync("triggerEvent", relativeUrl, userId);
      }

      public async ValueTask DisposeAsync()
      {
        if (_moduleTask.IsValueCreated)
        {
          IJSObjectReference module = await _moduleTask.Value;
          await module.DisposeAsync();
        }
      }
    }
}
