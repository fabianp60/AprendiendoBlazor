using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

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
        public string TagsDataString { get; set; } = "";

        [Parameter]
        public string TagHtmlTemplate { get; set; } = "<span data-value='<%value%>' class='badge bg-primary'><%label%></span>";

        [Parameter]
        public string Placeholder { get; set; } = "";

        public void OnKeyDownDetected(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                _tags.Add(new TagObj { value = TagInputText, label = TagInputText });
                TagInputText = "";
            }
            else if (e.Code == "Backspace")
            {
                if (TagInputText.Length == 0 && _tags.Count > 0)
                {
                    _tags.RemoveAt(_tags.Count - 1);
                }
            }
        }

        private string GetTagHtml(TagObj curTag)
        {
            return TagHtmlTemplate.Replace("<%label%>", curTag.label).Replace("<%value%>", curTag.value) + "\r\n";
        }

        private async Task FocusInput()
        {
            await JS.InvokeVoidAsync("BlazorFocusElement", inputElement);
        }

        private async Task UpdateInputWidth(ChangeEventArgs e)
        {
            string inputText = e.Value?.ToString() ?? string.Empty;
            if (FirstMinWidth)
            {
                MinWidth = await JS.InvokeAsync<double>("BlazorMeasureTextWidth", Placeholder, textWidthElement);
                FirstMinWidth = false;
            }
            var width = await JS.InvokeAsync<double>("BlazorMeasureTextWidth", inputText, textWidthElement);
            await JS.InvokeVoidAsync("BlazorSetElementWidth", inputElement, (MinWidth > width ? MinWidth : width) + 10);
        }
    }

    public class TagObj
    {
        public string value { get; set; } = "";
        public string label { get; set; } = "";
    }
}
