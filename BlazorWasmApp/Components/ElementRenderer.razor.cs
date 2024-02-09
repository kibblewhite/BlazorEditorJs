using Microsoft.AspNetCore.Components;

namespace BlazorWasmApp.Components;

public partial class ElementRenderer : ComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected override async Task OnInitializedAsync()
    {
        ChildContent = new(RenderNewFragment);
        StateHasChanged();
        await base.OnInitializedAsync();
    }

    private static RenderFragment RenderNewFragment => builder =>
    {
        builder.OpenElement(0, "div");
        builder.AddContent(1, "This is a div element.");

        builder.OpenElement(2, "p");
        builder.AddContent(3, "This is a paragraph inside the div.");
        builder.CloseElement(); // Close the paragraph element

        builder.OpenElement(4, "p");
        builder.AddContent(5, "This is a second paragraph inside the div.");
        builder.CloseElement(); // Close the paragraph element

        builder.CloseElement(); // Close the div element
    };

}