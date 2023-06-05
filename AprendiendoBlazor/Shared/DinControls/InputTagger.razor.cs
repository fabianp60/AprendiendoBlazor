using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Text.Json;

namespace AprendiendoBlazor.Shared.DinControls
{
    public partial class InputTagger
    {
        private string TagInputText = "";
        private double MinWidth = 5;
        private bool FirstMinWidth = true;
        private List<TagObj> _tags { get; set; } = new List<TagObj>();
        private ElementReference inputElement;
        private ElementReference textWidthElement;
        
        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public string Value { get; set; }

        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        private async Task OnValueChange(ChangeEventArgs e)
        {
            Value = e.Value.ToString();
            await ValueChanged.InvokeAsync(Value);
        }

        private async Task UpdateValue(string newValue)
        {
            Value = newValue;
            await ValueChanged.InvokeAsync(Value);
        }

        [Parameter]
        public string Placeholder { get; set; } = "";

        public async Task OnKeyUpDetected(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                _tags.Add(new TagObj { value = TagInputText, label = TagInputText });
                await UpdateValue(TagsToJson());
                TagInputText = "";
            }
            else if (e.Code == "Backspace")
            {
                if (TagInputText.Length == 0 && _tags.Count > 0)
                {
                    RemoveTag(_tags.Count - 1);
                }
            }
        }

        private async Task RemoveTag(int index)
        {
            _tags.RemoveAt(index);
            await UpdateValue(TagsToJson());
        }

        private async Task FocusInput()
        {
            await JS.InvokeVoidAsync("BlazorFocusElement", inputElement);
        }

        private async Task OnInputDetected(ChangeEventArgs e)
        {
            TagInputText = e.Value?.ToString() ?? string.Empty;
            await UpdateInputWidth();
        }

        private async Task UpdateInputWidth()
        {
            if (FirstMinWidth)
            {
                MinWidth = await JS.InvokeAsync<double>("BlazorMeasureTextWidth", Placeholder, textWidthElement);
                FirstMinWidth = false;
            }
            var width = await JS.InvokeAsync<double>("BlazorMeasureTextWidth", TagInputText, textWidthElement);
            await JS.InvokeVoidAsync("BlazorSetElementWidth", inputElement, (MinWidth > width ? MinWidth : width) + 10);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await UpdateInputWidth();
            }
        }

        private string TagsToJson()
        {
            return JsonSerializer.Serialize(_tags);
        }
    }

    public class TagObj
    {
        public string value { get; set; } = "";
        public string label { get; set; } = "";
    }
}
