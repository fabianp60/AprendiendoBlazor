using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FSoft.BlazorLib
{
    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.
    public class SharedJsInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public SharedJsInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/FSoft.BlazorLib/js/shared.js").AsTask());
        }

        public async ValueTask<string> Prompt(string message)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>("showPrompt", message);
        }

        public async ValueTask FocusInput(ElementReference element)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("FSoft_BlazorLib_FocusElement", element);
        }

        public async ValueTask<double> MeasureTextWidth(string text, ElementReference element)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<double>("FSoft_BlazorLib_MeasureTextWidth", text, element);
        }

        public async ValueTask SetElementWidth(ElementReference element, double width)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("FSoft_BlazorLib_SetElementWidth", element, width);
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
