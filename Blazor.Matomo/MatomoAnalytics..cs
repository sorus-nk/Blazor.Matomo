using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blazor.Matomo
{
  public partial class MatomoAnalytics : ComponentBase, IDisposable
  {
    [Parameter]
    public int SiteId { get; set; }

    [Parameter]
    public string ApiUrl { get; set; } = String.Empty;

    [Inject] NavigationManager NavigationManager { get; set; }
    [Inject] AuthenticationStateProvider? AuthenticationStateProvider { get; set; }
    [Inject]
    protected IJSRuntime? JSRuntime { get; set; }
    private JsInterop jsInterop;
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      NavigationManager.LocationChanged -= OnLocationChanged;
      NavigationManager.LocationChanged += OnLocationChanged;
      jsInterop = new(JSRuntime!);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      await base.OnAfterRenderAsync(firstRender);
      if (firstRender)
      {
        await jsInterop.Init(ApiUrl, SiteId);
      }
    }

    public void Dispose()
    {
      this.NavigationManager.LocationChanged -= OnLocationChanged;
    }

    private async void OnLocationChanged(object? sender, LocationChangedEventArgs args)
    {
      var b = await AuthenticationStateProvider.GetAuthenticationStateAsync();
      var uid = b.User.FindFirst(ClaimTypes.NameIdentifier) ?? b.User.FindFirst(ClaimTypes.Name);
      await jsInterop.TriggerPageView(args.Location, uid?.Subject?.Name);
    }
  }
}