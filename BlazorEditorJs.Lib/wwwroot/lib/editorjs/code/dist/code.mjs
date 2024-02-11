(function(){"use strict";try{if(typeof document<"u"){var e=document.createElement("style");e.appendChild(document.createTextNode(".ce-code__textarea{min-height:200px;font-family:Menlo,Monaco,Consolas,Courier New,monospace;color:#41314e;line-height:1.6em;font-size:12px;background:#f8f7fa;border:1px solid #f1f1f4;box-shadow:none;white-space:pre;word-wrap:normal;overflow-x:auto;resize:vertical}")),document.head.appendChild(e)}}catch(o){console.error("vite-plugin-css-injected-by-js",o)}})();
function l(c, t) {
  let r = "";
  for (; r !== `
` && t > 0; )
    t = t - 1, r = c.substr(t, 1);
  return r === `
` && (t += 1), t;
}
const h = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 8L5 12L9 16"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 8L19 12L15 16"/></svg>';
/**
 * CodeTool for Editor.js
 *
 * @author CodeX (team@ifmo.su)
 * @copyright CodeX 2018
 * @license MIT
 * @version 2.0.0
 */
class d {
  /**
   * Notify core that read-only mode is supported
   *
   * @returns {boolean}
   */
  static get isReadOnlySupported() {
    return !0;
  }
  /**
   * Allow to press Enter inside the CodeTool textarea
   *
   * @returns {boolean}
   * @public
   */
  static get enableLineBreaks() {
    return !0;
  }
  /**
   * @typedef {object} CodeData — plugin saved data
   * @property {string} code - previously saved plugin code
   */
  /**
   * Render plugin`s main Element and fill it with saved data
   *
   * @param {object} options - tool constricting options
   * @param {CodeData} options.data — previously saved plugin code
   * @param {object} options.config - user config for Tool
   * @param {object} options.api - Editor.js API
   * @param {boolean} options.readOnly - read only mode flag
   */
  constructor({ data: t, config: e, api: r, readOnly: a }) {
    this.api = r, this.readOnly = a, this.placeholder = this.api.i18n.t(e.placeholder || d.DEFAULT_PLACEHOLDER), this.CSS = {
      baseClass: this.api.styles.block,
      input: this.api.styles.input,
      wrapper: "ce-code",
      textarea: "ce-code__textarea"
    }, this.nodes = {
      holder: null,
      textarea: null
    }, this.data = {
      code: t.code || ""
    }, this.nodes.holder = this.drawView();
  }
  /**
   * Create Tool's view
   *
   * @returns {HTMLElement}
   * @private
   */
  drawView() {
    const t = document.createElement("div"), e = document.createElement("textarea");
    return t.classList.add(this.CSS.baseClass, this.CSS.wrapper), e.classList.add(this.CSS.textarea, this.CSS.input), e.textContent = this.data.code, e.placeholder = this.placeholder, this.readOnly && (e.disabled = !0), t.appendChild(e), e.addEventListener("keydown", (r) => {
      switch (r.code) {
        case "Tab":
          this.tabHandler(r);
          break;
      }
    }), this.nodes.textarea = e, t;
  }
  /**
   * Return Tool's view
   *
   * @returns {HTMLDivElement} this.nodes.holder - Code's wrapper
   * @public
   */
  render() {
    return this.nodes.holder;
  }
  /**
   * Extract Tool's data from the view
   *
   * @param {HTMLDivElement} codeWrapper - CodeTool's wrapper, containing textarea with code
   * @returns {CodeData} - saved plugin code
   * @public
   */
  save(t) {
    return {
      code: t.querySelector("textarea").value
    };
  }
  /**
   * onPaste callback fired from Editor`s core
   *
   * @param {PasteEvent} event - event with pasted content
   */
  onPaste(t) {
    const e = t.detail.data;
    this.data = {
      code: e.textContent
    };
  }
  /**
   * Returns Tool`s data from private property
   *
   * @returns {CodeData}
   */
  get data() {
    return this._data;
  }
  /**
   * Set Tool`s data to private property and update view
   *
   * @param {CodeData} data - saved tool data
   */
  set data(t) {
    this._data = t, this.nodes.textarea && (this.nodes.textarea.textContent = t.code);
  }
  /**
   * Get Tool toolbox settings
   * icon - Tool icon's SVG
   * title - title to show in toolbox
   *
   * @returns {{icon: string, title: string}}
   */
  static get toolbox() {
    return {
      icon: h,
      title: "Code"
    };
  }
  /**
   * Default placeholder for CodeTool's textarea
   *
   * @public
   * @returns {string}
   */
  static get DEFAULT_PLACEHOLDER() {
    return "Enter a code";
  }
  /**
   *  Used by Editor.js paste handling API.
   *  Provides configuration to handle CODE tag.
   *
   * @static
   * @returns {{tags: string[]}}
   */
  static get pasteConfig() {
    return {
      tags: ["pre"]
    };
  }
  /**
   * Automatic sanitize config
   *
   * @returns {{code: boolean}}
   */
  static get sanitize() {
    return {
      code: !0
      // Allow HTML tags
    };
  }
  /**
   * Handles Tab key pressing (adds/removes indentations)
   *
   * @private
   * @param {KeyboardEvent} event - keydown
   * @returns {void}
   */
  tabHandler(t) {
    t.stopPropagation(), t.preventDefault();
    const e = t.target, r = t.shiftKey, a = e.selectionStart, s = e.value, n = "  ";
    let i;
    if (!r)
      i = a + n.length, e.value = s.substring(0, a) + n + s.substring(a);
    else {
      const o = l(s, a);
      if (s.substr(o, n.length) !== n)
        return;
      e.value = s.substring(0, o) + s.substring(o + n.length), i = a - n.length;
    }
    e.setSelectionRange(i, i);
  }
}
export {
  d as default
};
