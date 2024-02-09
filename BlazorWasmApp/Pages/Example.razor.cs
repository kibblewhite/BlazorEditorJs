using BlazorWasmApp.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorWasmApp.Pages;

public partial class Example : IComponent
{
    [Inject]
    public required IServiceProvider service_provider { get; set; }

    [Inject]
    public required ILoggerFactory logger_factory { get; set; }

    [Parameter]
    public string? RawElementHtml { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await using HtmlRenderer html_renderer = new(service_provider, logger_factory);
        RawElementHtml = await html_renderer.Dispatcher.InvokeAsync(async () =>
        {
            Dictionary<string, object?> dictionary = new()
            {
                { nameof(ElementRenderer.ChildContent), null }
            };

            ParameterView parameters = ParameterView.FromDictionary(dictionary);
            Microsoft.AspNetCore.Components.Web.HtmlRendering.HtmlRootComponent output = await html_renderer.RenderComponentAsync<ElementRenderer>(parameters);

            return output.ToHtmlString();
        });
        StateHasChanged();
        await base.OnAfterRenderAsync(firstRender);
    }
}