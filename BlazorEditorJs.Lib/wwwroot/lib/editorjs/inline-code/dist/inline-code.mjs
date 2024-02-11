(function(){"use strict";try{if(typeof document<"u"){var e=document.createElement("style");e.appendChild(document.createTextNode(".inline-code{background:rgba(250,239,240,.78);color:#b44437;padding:3px 4px;border-radius:5px;margin:0 1px;font-family:inherit;font-size:.86em;font-weight:500;letter-spacing:.3px}")),document.head.appendChild(e)}}catch(n){console.error("vite-plugin-css-injected-by-js",n)}})();
const o = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M9.5 8L6.11524 11.8683C6.04926 11.9437 6.04926 12.0563 6.11524 12.1317L9.5 16"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M15 8L18.3848 11.8683C18.4507 11.9437 18.4507 12.0563 18.3848 12.1317L15 16"/></svg>';
class s {
  /**
   * Class name for term-tag
   *
   * @type {string}
   */
  static get CSS() {
    return "inline-code";
  }
  /**
   */
  constructor({ api: t }) {
    this.api = t, this.button = null, this.tag = "CODE", this.iconClasses = {
      base: this.api.styles.inlineToolButton,
      active: this.api.styles.inlineToolButtonActive
    };
  }
  /**
   * Specifies Tool as Inline Toolbar Tool
   *
   * @return {boolean}
   */
  static get isInline() {
    return !0;
  }
  /**
   * Create button element for Toolbar
   *
   * @return {HTMLElement}
   */
  render() {
    return this.button = document.createElement("button"), this.button.type = "button", this.button.classList.add(this.iconClasses.base), this.button.innerHTML = this.toolboxIcon, this.button;
  }
  /**
   * Wrap/Unwrap selected fragment
   *
   * @param {Range} range - selected fragment
   */
  surround(t) {
    if (!t)
      return;
    let e = this.api.selection.findParentTag(this.tag, s.CSS);
    e ? this.unwrap(e) : this.wrap(t);
  }
  /**
   * Wrap selection with term-tag
   *
   * @param {Range} range - selected fragment
   */
  wrap(t) {
    let e = document.createElement(this.tag);
    e.classList.add(s.CSS), e.appendChild(t.extractContents()), t.insertNode(e), this.api.selection.expandToTag(e);
  }
  /**
   * Unwrap term-tag
   *
   * @param {HTMLElement} termWrapper - term wrapper tag
   */
  unwrap(t) {
    this.api.selection.expandToTag(t);
    let e = window.getSelection(), n = e.getRangeAt(0), i = n.extractContents();
    t.parentNode.removeChild(t), n.insertNode(i), e.removeAllRanges(), e.addRange(n);
  }
  /**
   * Check and change Term's state for current selection
   */
  checkState() {
    const t = this.api.selection.findParentTag(this.tag, s.CSS);
    this.button.classList.toggle(this.iconClasses.active, !!t);
  }
  /**
   * Get Tool icon's SVG
   * @return {string}
   */
  get toolboxIcon() {
    return o;
  }
  /**
   * Sanitizer rule
   * @return {{span: {class: string}}}
   */
  static get sanitize() {
    return {
      code: {
        class: s.CSS
      }
    };
  }
}
export {
  s as default
};
