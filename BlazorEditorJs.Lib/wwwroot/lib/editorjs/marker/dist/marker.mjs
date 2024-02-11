(function(){"use strict";try{if(typeof document<"u"){var e=document.createElement("style");e.appendChild(document.createTextNode(".cdx-marker{background:rgba(245,235,111,.29);padding:3px 0}")),document.head.appendChild(e)}}catch(d){console.error("vite-plugin-css-injected-by-js",d)}})();
const o = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-width="2" d="M11.3536 9.31802L12.7678 7.90381C13.5488 7.12276 14.8151 7.12276 15.5962 7.90381C16.3772 8.68486 16.3772 9.95119 15.5962 10.7322L14.182 12.1464M11.3536 9.31802L7.96729 12.7043C7.40889 13.2627 7.02827 13.9739 6.8734 14.7482L6.69798 15.6253C6.55804 16.325 7.17496 16.942 7.87468 16.802L8.75176 16.6266C9.52612 16.4717 10.2373 16.0911 10.7957 15.5327L14.182 12.1464M11.3536 9.31802L14.182 12.1464"/><line x1="15" x2="19" y1="17" y2="17" stroke="currentColor" stroke-linecap="round" stroke-width="2"/></svg>';
class s {
  /**
   * Class name for term-tag
   *
   * @type {string}
   */
  static get CSS() {
    return "cdx-marker";
  }
  /**
   * @param {{api: object}}  - Editor.js API
   */
  constructor({ api: t }) {
    this.api = t, this.button = null, this.tag = "MARK", this.iconClasses = {
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
   * @return {{mark: {class: string}}}
   */
  static get sanitize() {
    return {
      mark: {
        class: s.CSS
      }
    };
  }
}
export {
  s as default
};
