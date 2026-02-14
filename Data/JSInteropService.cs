using Microsoft.JSInterop;

namespace WebsiteFirstDraft.Data
{
    // This service provides methods to interact with JavaScript functions from Blazor WebAssembly.
    // It includes methods for displaying confirmation dialogs and alert messages using JavaScript's built-in functions.
    // The IJSInteropService interface defines the contract for the service, while the JSInteropService class implements the interface using the IJSRuntime to invoke JavaScript functions.

    public interface IJSInteropService
    {
        Task<bool> ConfirmAsync(string message);
        Task AlertAsync(string message);
    }

    // The JSInteropService class implements the IJSInteropService interface and uses the IJSRuntime to call JavaScript functions.
    public class JSInteropService(IJSRuntime jsRuntime) : IJSInteropService
    {
        // The ConfirmAsync method displays a confirmation dialog with the specified message and returns a boolean indicating whether the user confirmed (true) or canceled (false).
        public async Task<bool> ConfirmAsync(string message)
        {
            return await jsRuntime.InvokeAsync<bool>("confirm", message);
        }

        // The AlertAsync method displays an alert dialog with the specified message.
        public async Task AlertAsync(string message)
        {
            await jsRuntime.InvokeVoidAsync("alert", message);
        }
    }
}
