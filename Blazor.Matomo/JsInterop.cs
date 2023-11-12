using Microsoft.JSInterop;

using System;
using System.Threading.Tasks;

namespace Blazor.Matomo
{
  public class JsInterop : IAsyncDisposable
  {
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;

    public JsInterop(IJSRuntime jsRuntime)
    {
      moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
          "import", "./_content/Blazor.Matomo/Blazor.Matomo.js").AsTask());
    }

    public async ValueTask Init(string apiUrl, int siteId)
    {
      var module = await moduleTask.Value;
      await module.InvokeVoidAsync("init", apiUrl, siteId);
    }

    public async ValueTask TriggerPageView(string? relativeUrl, string? userId)
    {
      var module = await moduleTask.Value;
      await module.InvokeVoidAsync("triggerEvent", relativeUrl, userId);
    }

    public async ValueTask DisposeAsync()
    {
      if (moduleTask.IsValueCreated)
      {
        var module = await moduleTask.Value;
        await module.DisposeAsync();
      }
    }
  }
}
