(function(){"use strict";try{if(typeof document<"u"){var t=document.createElement("style");t.appendChild(document.createTextNode(".cdx-quote-icon svg{transform:rotate(180deg)}.cdx-quote{margin:0}.cdx-quote__text{min-height:158px;margin-bottom:10px}.cdx-quote [contentEditable=true][data-placeholder]:before{position:absolute;content:attr(data-placeholder);color:#707684;font-weight:400;opacity:0}.cdx-quote [contentEditable=true][data-placeholder]:empty:before{opacity:1}.cdx-quote [contentEditable=true][data-placeholder]:empty:focus:before{opacity:0}.cdx-quote-settings{display:flex}.cdx-quote-settings .cdx-settings-button{width:50%}")),document.head.appendChild(t)}}catch(e){console.error("vite-plugin-css-injected-by-js",e)}})();
const a = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M18 7L6 7"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M18 17H6"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M16 12L8 12"/></svg>', c = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M17 7L5 7"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M17 17H5"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M13 12L5 12"/></svg>', l = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 10.8182L9 10.8182C8.80222 10.8182 8.60888 10.7649 8.44443 10.665C8.27998 10.5651 8.15181 10.4231 8.07612 10.257C8.00043 10.0909 7.98063 9.90808 8.01922 9.73174C8.0578 9.55539 8.15304 9.39341 8.29289 9.26627C8.43275 9.13913 8.61093 9.05255 8.80491 9.01747C8.99889 8.98239 9.19996 9.00039 9.38268 9.0692C9.56541 9.13801 9.72159 9.25453 9.83147 9.40403C9.94135 9.55353 10 9.72929 10 9.90909L10 12.1818C10 12.664 9.78929 13.1265 9.41421 13.4675C9.03914 13.8084 8.53043 14 8 14"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 10.8182L15 10.8182C14.8022 10.8182 14.6089 10.7649 14.4444 10.665C14.28 10.5651 14.1518 10.4231 14.0761 10.257C14.0004 10.0909 13.9806 9.90808 14.0192 9.73174C14.0578 9.55539 14.153 9.39341 14.2929 9.26627C14.4327 9.13913 14.6109 9.05255 14.8049 9.01747C14.9989 8.98239 15.2 9.00039 15.3827 9.0692C15.5654 9.13801 15.7216 9.25453 15.8315 9.40403C15.9414 9.55353 16 9.72929 16 9.90909L16 12.1818C16 12.664 15.7893 13.1265 15.4142 13.4675C15.0391 13.8084 14.5304 14 14 14"/></svg>';
class i {
  /**
   * Notify core that read-only mode is supported
   *
   * @returns {boolean}
   */
  static get isReadOnlySupported() {
    return !0;
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
      icon: l,
      title: "Quote"
    };
  }
  /**
   * Empty Quote is not empty Block
   *
   * @public
   * @returns {boolean}
   */
  static get contentless() {
    return !0;
  }
  /**
   * Allow to press Enter inside the Quote
   *
   * @public
   * @returns {boolean}
   */
  static get enableLineBreaks() {
    return !0;
  }
  /**
   * Default placeholder for quote text
   *
   * @public
   * @returns {string}
   */
  static get DEFAULT_QUOTE_PLACEHOLDER() {
    return "Enter a quote";
  }
  /**
   * Default placeholder for quote caption
   *
   * @public
   * @returns {string}
   */
  static get DEFAULT_CAPTION_PLACEHOLDER() {
    return "Enter a caption";
  }
  /**
   * Allowed quote alignments
   *
   * @public
   * @returns {{left: string, center: string}}
   */
  static get ALIGNMENTS() {
    return {
      left: "left",
      center: "center"
    };
  }
  /**
   * Default quote alignment
   *
   * @public
   * @returns {string}
   */
  static get DEFAULT_ALIGNMENT() {
    return i.ALIGNMENTS.left;
  }
  /**
   * Allow Quote to be converted to/from other blocks
   */
  static get conversionConfig() {
    return {
      /**
       * To create Quote data from string, simple fill 'text' property
       */
      import: "text",
      /**
       * To create string from Quote data, concatenate text and caption
       *
       * @param {QuoteData} quoteData
       * @returns {string}
       */
      export: function(t) {
        return t.caption ? `${t.text} — ${t.caption}` : t.text;
      }
    };
  }
  /**
   * Tool`s styles
   *
   * @returns {{baseClass: string, wrapper: string, quote: string, input: string, caption: string}}
   */
  get CSS() {
    return {
      baseClass: this.api.styles.block,
      wrapper: "cdx-quote",
      text: "cdx-quote__text",
      input: this.api.styles.input,
      caption: "cdx-quote__caption"
    };
  }
  /**
   * Tool`s settings properties
   *
   * @returns {*[]}
   */
  get settings() {
    return [
      {
        name: "left",
        icon: c
      },
      {
        name: "center",
        icon: a
      }
    ];
  }
  /**
   * Render plugin`s main Element and fill it with saved data
   *
   * @param {{data: QuoteData, config: QuoteConfig, api: object}}
   *   data — previously saved data
   *   config - user config for Tool
   *   api - Editor.js API
   *   readOnly - read-only mode flag
   */
  constructor({ data: t, config: e, api: n, readOnly: r }) {
    const { ALIGNMENTS: o, DEFAULT_ALIGNMENT: s } = i;
    this.api = n, this.readOnly = r, this.quotePlaceholder = e.quotePlaceholder || i.DEFAULT_QUOTE_PLACEHOLDER, this.captionPlaceholder = e.captionPlaceholder || i.DEFAULT_CAPTION_PLACEHOLDER, this.data = {
      text: t.text || "",
      caption: t.caption || "",
      alignment: Object.values(o).includes(t.alignment) && t.alignment || e.defaultAlignment || s
    };
  }
  /**
   * Create Quote Tool container with inputs
   *
   * @returns {Element}
   */
  render() {
    const t = this._make("blockquote", [this.CSS.baseClass, this.CSS.wrapper]), e = this._make("div", [this.CSS.input, this.CSS.text], {
      contentEditable: !this.readOnly,
      innerHTML: this.data.text
    }), n = this._make("div", [this.CSS.input, this.CSS.caption], {
      contentEditable: !this.readOnly,
      innerHTML: this.data.caption
    });
    return e.dataset.placeholder = this.quotePlaceholder, n.dataset.placeholder = this.captionPlaceholder, t.appendChild(e), t.appendChild(n), t;
  }
  /**
   * Extract Quote data from Quote Tool element
   *
   * @param {HTMLDivElement} quoteElement - element to save
   * @returns {QuoteData}
   */
  save(t) {
    const e = t.querySelector(`.${this.CSS.text}`), n = t.querySelector(`.${this.CSS.caption}`);
    return Object.assign(this.data, {
      text: e.innerHTML,
      caption: n.innerHTML
    });
  }
  /**
   * Sanitizer rules
   */
  static get sanitize() {
    return {
      text: {
        br: !0
      },
      caption: {
        br: !0
      },
      alignment: {}
    };
  }
  /**
   * Create wrapper for Tool`s settings buttons:
   * 1. Left alignment
   * 2. Center alignment
   *
   * @returns {TunesMenuConfig}
   *
   */
  renderSettings() {
    const t = (e) => e[0].toUpperCase() + e.substr(1);
    return this.settings.map((e) => ({
      icon: e.icon,
      label: this.api.i18n.t(`Align ${t(e.name)}`),
      onActivate: () => this._toggleTune(e.name),
      isActive: this.data.alignment === e.name,
      closeOnActivate: !0
    }));
  }
  /**
   * Toggle quote`s alignment
   *
   * @param {string} tune - alignment
   * @private
   */
  _toggleTune(t) {
    this.data.alignment = t;
  }
  /**
   * Helper for making Elements with attributes
   *
   * @param  {string} tagName           - new Element tag name
   * @param  {Array|string} classNames  - list or name of CSS classname(s)
   * @param  {object} attributes        - any attributes
   * @returns {Element}
   */
  _make(t, e = null, n = {}) {
    const r = document.createElement(t);
    Array.isArray(e) ? r.classList.add(...e) : e && r.classList.add(e);
    for (const o in n)
      r[o] = n[o];
    return r;
  }
}
export {
  i as default
};
