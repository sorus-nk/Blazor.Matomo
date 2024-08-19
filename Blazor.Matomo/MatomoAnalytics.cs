using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Hosting;
using Microsoft.JSInterop;

using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blazor.Matomo;

public partial class MatomoAnalytics : ComponentBase, IDisposable
{
  [Parameter]
  public int SiteId { get; set; }

  [Parameter]
  public string ApiUrl { get; set; } = string.Empty;

  [Inject] private NavigationManager NavigationManager { get; set; } = default!;
  [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
  [Inject] protected IJSRuntime JSRuntime { get; set; } = default!;
  [Inject] private IHostEnvironment Environment { get; set; } = default!;
  private JsInterop jsInterop = default!;
  private string? _userName;
  private bool _isDevelopment;
  protected override async Task OnInitializedAsync()
  {
    await base.OnInitializedAsync();
    NavigationManager.LocationChanged -= OnLocationChanged;
    NavigationManager.LocationChanged += OnLocationChanged;
    jsInterop = new(JSRuntime);
    _userName = await GetUserName();
    _isDevelopment = Environment.IsDevelopment();
    //_isDevelopment = false;
  }

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    await base.OnAfterRenderAsync(firstRender);
    if (!_isDevelopment)
    {
      if (firstRender)
      {
        await jsInterop.Init(ApiUrl, SiteId);
      }

      await jsInterop.TriggerPageView(NavigationManager.Uri, _userName);
    }
  }

  public void Dispose()
  {
    NavigationManager.LocationChanged -= OnLocationChanged;
  }

  private async void OnLocationChanged(object? sender, LocationChangedEventArgs args)
  {
    if (!_isDevelopment)
    {
      await jsInterop.TriggerPageView(args.Location, _userName);
    }
  }

  private async Task<string?> GetUserName()
  {
    AuthenticationState b = await AuthenticationStateProvider.GetAuthenticationStateAsync();
    Claim? uid = b.User.FindFirst(ClaimTypes.NameIdentifier) ?? b.User.FindFirst(ClaimTypes.Name);
    return uid?.Subject?.Name;
  }
}