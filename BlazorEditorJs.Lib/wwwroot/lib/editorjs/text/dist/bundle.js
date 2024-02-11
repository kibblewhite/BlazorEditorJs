(function() {
  "use strict";
  try {
    if (typeof document != "undefined") {
      var elementStyle = document.createElement("style");
      elementStyle.appendChild(document.createTextNode('.ce-text {\n  line-height: 1.6em;\n  outline: none;\n}\n\n.ce-text[data-placeholder]:empty::before{\n  content: attr(data-placeholder);\n  color: #707684;\n  font-weight: normal;\n  opacity: 0;\n}\n\n/** Show placeholder at the first text if Editor is empty */\n.codex-editor--empty .ce-block:first-child .ce-text[data-placeholder]:empty::before {\n  opacity: 1;\n}\n\n.codex-editor--toolbox-opened .ce-block:first-child .ce-text[data-placeholder]:empty::before,\n.codex-editor--empty .ce-block:first-child .ce-text[data-placeholder]:empty:focus::before {\n  opacity: 0;\n}\n\n.ce-text p:first-of-type{\n  margin-top: 0;\n}\n\n.ce-text p:last-of-type{\n  margin-bottom: 0;\n}\n\n[contenteditable="true"].ce-text {\n  white-space: nowrap;\n  overflow: hidden;\n}\n\n[contenteditable="true"].ce-text br {\n  display: none;\n}\n\n[contenteditable="true"].ce-text * {\n  display: inline;\n  white-space: nowrap;\n}'));
      document.head.appendChild(elementStyle);
    }
  } catch (e) {
    console.error("vite-plugin-css-injected-by-js", e);
  }
})();
var TextElement = function() {
  "use strict";
  class t {
    static get DefaultPlaceHolder() {
      return "";
    }
    static get Version() {
      return "0.1.4";
    }
    static get DefaultWrapElement() {
      return "text";
    }
    static get SupportedWrapElementsArray() {
      return ["text", "custom", "title", "synopsis"];
    }
    _set_wrap_element(e) {
      this._data.wrap = t.SupportedWrapElementsArray.find((t2) => t2 === e) ?? t.DefaultWrapElement;
    }
    _instantiate_data(t2) {
      this._data = this.normalizeData(t2 || {});
    }
    constructor({ data: e, config: a, api: i, readOnly: r }) {
      this.readOnly = r, this.api = i, this.holder = this.api.ui.nodes.wrapper.parentElement, this._instantiate_data(e), this._set_wrap_element(a.wrapElement), this._CSS = { block: this.api.styles.block, wrapper: "ce-text" }, this.readOnly || (this.onKeyUp = this.onKeyUp.bind(this), this.onKeyDown = this.onKeyDown.bind(this)), this._element = null, this._placeholder = a.placeholder ? a.placeholder : t.DefaultPlaceHolder, this._preserveBlank = void 0 !== a.preserveBlank && a.preserveBlank, this._allowEnterKeyDown = void 0 !== a.allowEnterKeyDown && a.allowEnterKeyDown, this._hidePopoverItem = void 0 !== a.hidePopoverItem && a.hidePopoverItem, this._hideToolbar = void 0 !== a.hideToolbar && a.hideToolbar;
    }
    static get toolbox() {
      return true === this._hidePopoverItem ? [] : { icon: '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M8 9V7.2C8 7.08954 8.08954 7 8.2 7L12 7M16 9V7.2C16 7.08954 15.9105 7 15.8 7L12 7M12 7L12 17M12 17H10M12 17H14"/></svg>', title: "Text" };
    }
    _currentWrapElement() {
      return t.SupportedWrapElementsArray.find((t2) => t2.wrap === this._data.wrap).toString() ?? t.DefaultWrapElement;
    }
    normalizeData(e) {
      return { text: e && "string" == typeof e.text ? e.text : "", wrap: e && e.wrap ? e.wrap : t.DefaultWrapElement };
    }
    onKeyUp(t2) {
      if ("Backspace" !== t2.code && "Delete" !== t2.code)
        return;
      const { textContent: e } = this._element;
      "" === e && (this._element.innerHTML = "");
    }
    onKeyDown(t2) {
      if (false === this._allowEnterKeyDown && "Enter" === t2.key)
        return t2.stopPropagation(), t2.preventDefault(), false;
    }
    drawView() {
      const t2 = document.createElement("div");
      return t2.classList.add(this._CSS.wrapper, this._CSS.block), t2.contentEditable = false, t2.dataset.placeholder = this.api.i18n.t(this._placeholder), this._data.text && (t2.innerHTML = this._data.text), this.readOnly || (t2.contentEditable = true, t2.addEventListener("keyup", this.onKeyUp), t2.addEventListener("keydown", this.onKeyDown)), true === this._hidePopoverItem && this._hide_element_on_mutation('.ce-popover-item[data-item-name="text"]'), true === this._hideToolbar && this._hide_element_on_mutation(".ce-toolbar"), t2;
    }
    _hide_element_on_mutation(t2) {
      var e = new MutationObserver(() => {
        let a = this.holder.querySelector(t2);
        if (null != a) {
          "none" !== window.getComputedStyle(a).display && (a.style.display = "none", e.disconnect());
        }
      });
      e.observe(this.holder, { childList: true, subtree: true, attributes: true });
    }
    render() {
      return this._element = this.drawView(), this._element;
    }
    merge(t2) {
      let e = { text: this._data.text + t2.text, wrap: this._data.wrap };
      this._data = this.normalizeData(e);
    }
    validate(t2) {
      return !("" === t2.text.trim() && !this._preserveBlank);
    }
    save(t2) {
      return { text: t2.innerHTML, wrap: this._data.wrap };
    }
    onPaste(t2) {
      const e = { text: t2.detail.data.innerHTML };
      this._data = this.normalizeData(e);
    }
    static get sanitize() {
      return { text: { br: false } };
    }
    static get isReadOnlySupported() {
      return true;
    }
    get data() {
      if (null !== this._element && void 0 !== this._element) {
        const t2 = this._element.innerHTML;
        this._data.text = t2;
      }
      return this._data.wrap = this._currentWrapElement(), this.normalizeData(this._data);
    }
    set data(t2) {
      this._data = this.normalizeData(t2), null !== this._element && this.hydrate();
    }
    hydrate() {
      window.requestAnimationFrame(() => {
        this._element.innerHTML = this._data.text || "";
      });
    }
    static get pasteConfig() {
      return { tags: ["p"] };
    }
    static get enableLineBreaks() {
      return false;
    }
  }
  return t;
}();
