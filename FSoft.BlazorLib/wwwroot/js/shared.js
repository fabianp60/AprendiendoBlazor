// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

export function showPrompt(message) {
    return prompt(message, 'Escribe algo aquí');
}

export function FSoft_BlazorLib_FocusElement(element) {
    element.focus();
}

export function FSoft_BlazorLib_MeasureTextWidth(text, element) {
    element.textContent = text;
    return element.offsetWidth;
}

export function FSoft_BlazorLib_SetElementWidth(element, width) {
    element.style.width = `${width}px`;
}