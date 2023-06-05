window.BlazorFocusElement = (element) => {
    element.focus();
};

window.BlazorMeasureTextWidth = (text, element) => {
    element.textContent = text;
    return element.offsetWidth;
};

window.BlazorSetElementWidth = (element, width) => {
    element.style.width = `${width}px`;
};
