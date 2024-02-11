(function(){"use strict";try{if(typeof document<"u"){var e=document.createElement("style");e.appendChild(document.createTextNode(".cdx-list{margin:0;padding-left:40px;outline:none}.cdx-list__item{padding:5.5px 0 5.5px 3px;line-height:1.6em}.cdx-list--unordered{list-style:disc}.cdx-list--ordered{list-style:decimal}.cdx-list-settings{display:flex}.cdx-list-settings .cdx-settings-button{width:50%}")),document.head.appendChild(e)}}catch(t){console.error("vite-plugin-css-injected-by-js",t)}})();
const a = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><line x1="9" x2="19" y1="7" y2="7" stroke="currentColor" stroke-linecap="round" stroke-width="2"/><line x1="9" x2="19" y1="12" y2="12" stroke="currentColor" stroke-linecap="round" stroke-width="2"/><line x1="9" x2="19" y1="17" y2="17" stroke="currentColor" stroke-linecap="round" stroke-width="2"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M5.00001 17H4.99002"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M5.00001 12H4.99002"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M5.00001 7H4.99002"/></svg>', o = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><line x1="12" x2="19" y1="7" y2="7" stroke="currentColor" stroke-linecap="round" stroke-width="2"/><line x1="12" x2="19" y1="12" y2="12" stroke="currentColor" stroke-linecap="round" stroke-width="2"/><line x1="12" x2="19" y1="17" y2="17" stroke="currentColor" stroke-linecap="round" stroke-width="2"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M7.79999 14L7.79999 7.2135C7.79999 7.12872 7.7011 7.0824 7.63597 7.13668L4.79999 9.5"/></svg>';
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
   * Allow to use native Enter behaviour
   *
   * @returns {boolean}
   * @public
   */
  static get enableLineBreaks() {
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
      icon: a,
      title: "List"
    };
  }
  /**
   * Render plugin`s main Element and fill it with saved data
   *
   * @param {object} params - tool constructor options
   * @param {ListData} params.data - previously saved data
   * @param {object} params.config - user config for Tool
   * @param {object} params.api - Editor.js API
   * @param {boolean} params.readOnly - read-only mode flag
   */
  constructor({ data: e, config: t, api: r, readOnly: s }) {
    this._elements = {
      wrapper: null
    }, this.api = r, this.readOnly = s, this.settings = [
      {
        name: "unordered",
        label: this.api.i18n.t("Unordered"),
        icon: a,
        default: t.defaultStyle === "unordered" || !1
      },
      {
        name: "ordered",
        label: this.api.i18n.t("Ordered"),
        icon: o,
        default: t.defaultStyle === "ordered" || !0
      }
    ], this._data = {
      style: this.settings.find((i) => i.default === !0).name,
      items: []
    }, this.data = e;
  }
  /**
   * Returns list tag with items
   *
   * @returns {Element}
   * @public
   */
  render() {
    return this._elements.wrapper = this.makeMainTag(this._data.style), this._data.items.length ? this._data.items.forEach((e) => {
      this._elements.wrapper.appendChild(this._make("li", this.CSS.item, {
        innerHTML: e
      }));
    }) : this._elements.wrapper.appendChild(this._make("li", this.CSS.item)), this.readOnly || this._elements.wrapper.addEventListener("keydown", (e) => {
      const [t, r] = [13, 8];
      switch (e.keyCode) {
        case t:
          this.getOutofList(e);
          break;
        case r:
          this.backspace(e);
          break;
      }
    }, !1), this._elements.wrapper;
  }
  /**
   * @returns {ListData}
   * @public
   */
  save() {
    return this.data;
  }
  /**
   * Allow List Tool to be converted to/from other block
   *
   * @returns {{export: Function, import: Function}}
   */
  static get conversionConfig() {
    return {
      /**
       * To create exported string from list, concatenate items by dot-symbol.
       *
       * @param {ListData} data - list data to create a string from thats
       * @returns {string}
       */
      export: (e) => e.items.join(". "),
      /**
       * To create a list from other block's string, just put it at the first item
       *
       * @param {string} string - string to create list tool data from that
       * @returns {ListData}
       */
      import: (e) => ({
        items: [e],
        style: "unordered"
      })
    };
  }
  /**
   * Sanitizer rules
   *
   * @returns {object}
   */
  static get sanitize() {
    return {
      style: {},
      items: {
        br: !0
      }
    };
  }
  /**
   * Settings
   *
   * @public
   * @returns {Array}
   */
  renderSettings() {
    return this.settings.map((e) => ({
      ...e,
      isActive: this._data.style === e.name,
      closeOnActivate: !0,
      onActivate: () => this.toggleTune(e.name)
    }));
  }
  /**
   * On paste callback that is fired from Editor
   *
   * @param {PasteEvent} event - event with pasted data
   */
  onPaste(e) {
    const t = e.detail.data;
    this.data = this.pasteHandler(t);
  }
  /**
   * List Tool on paste configuration
   *
   * @public
   */
  static get pasteConfig() {
    return {
      tags: ["OL", "UL", "LI"]
    };
  }
  /**
   * Creates main <ul> or <ol> tag depended on style
   *
   * @param {string} style - 'ordered' or 'unordered'
   * @returns {HTMLOListElement|HTMLUListElement}
   */
  makeMainTag(e) {
    const t = e === "ordered" ? this.CSS.wrapperOrdered : this.CSS.wrapperUnordered, r = e === "ordered" ? "ol" : "ul";
    return this._make(r, [this.CSS.baseBlock, this.CSS.wrapper, t], {
      contentEditable: !this.readOnly
    });
  }
  /**
   * Toggles List style
   *
   * @param {string} style - 'ordered'|'unordered'
   */
  toggleTune(e) {
    const t = this.makeMainTag(e);
    for (; this._elements.wrapper.hasChildNodes(); )
      t.appendChild(this._elements.wrapper.firstChild);
    this._elements.wrapper.replaceWith(t), this._elements.wrapper = t, this._data.style = e;
  }
  /**
   * Styles
   *
   * @private
   */
  get CSS() {
    return {
      baseBlock: this.api.styles.block,
      wrapper: "cdx-list",
      wrapperOrdered: "cdx-list--ordered",
      wrapperUnordered: "cdx-list--unordered",
      item: "cdx-list__item"
    };
  }
  /**
   * List data setter
   *
   * @param {ListData} listData
   */
  set data(e) {
    e || (e = {}), this._data.style = e.style || this.settings.find((r) => r.default === !0).name, this._data.items = e.items || [];
    const t = this._elements.wrapper;
    t && t.parentNode.replaceChild(this.render(), t);
  }
  /**
   * Return List data
   *
   * @returns {ListData}
   */
  get data() {
    this._data.items = [];
    const e = this._elements.wrapper.querySelectorAll(`.${this.CSS.item}`);
    for (let t = 0; t < e.length; t++)
      e[t].innerHTML.replace("<br>", " ").trim() && this._data.items.push(e[t].innerHTML);
    return this._data;
  }
  /**
   * Helper for making Elements with attributes
   *
   * @param  {string} tagName           - new Element tag name
   * @param  {Array|string} classNames  - list or name of CSS classname(s)
   * @param  {object} attributes        - any attributes
   * @returns {Element}
   */
  _make(e, t = null, r = {}) {
    const s = document.createElement(e);
    Array.isArray(t) ? s.classList.add(...t) : t && s.classList.add(t);
    for (const i in r)
      s[i] = r[i];
    return s;
  }
  /**
   * Returns current List item by the caret position
   *
   * @returns {Element}
   */
  get currentItem() {
    let e = window.getSelection().anchorNode;
    return e.nodeType !== Node.ELEMENT_NODE && (e = e.parentNode), e.closest(`.${this.CSS.item}`);
  }
  /**
   * Get out from List Tool
   * by Enter on the empty last item
   *
   * @param {KeyboardEvent} event
   */
  getOutofList(e) {
    const t = this._elements.wrapper.querySelectorAll("." + this.CSS.item);
    if (t.length < 2)
      return;
    const r = t[t.length - 1], s = this.currentItem;
    s === r && !r.textContent.trim().length && (s.parentElement.removeChild(s), this.api.blocks.insert(), this.api.caret.setToBlock(this.api.blocks.getCurrentBlockIndex()), e.preventDefault(), e.stopPropagation());
  }
  /**
   * Handle backspace
   *
   * @param {KeyboardEvent} event
   */
  backspace(e) {
    const t = this._elements.wrapper.querySelectorAll("." + this.CSS.item), r = t[0];
    r && t.length < 2 && !r.innerHTML.replace("<br>", " ").trim() && e.preventDefault();
  }
  /**
   * Select LI content by CMD+A
   *
   * @param {KeyboardEvent} event
   */
  selectItem(e) {
    e.preventDefault();
    const t = window.getSelection(), r = t.anchorNode.parentNode, s = r.closest("." + this.CSS.item), i = new Range();
    i.selectNodeContents(s), t.removeAllRanges(), t.addRange(i);
  }
  /**
   * Handle UL, OL and LI tags paste and returns List data
   *
   * @param {HTMLUListElement|HTMLOListElement|HTMLLIElement} element
   * @returns {ListData}
   */
  pasteHandler(e) {
    const { tagName: t } = e;
    let r;
    switch (t) {
      case "OL":
        r = "ordered";
        break;
      case "UL":
      case "LI":
        r = "unordered";
    }
    const s = {
      style: r,
      items: []
    };
    if (t === "LI")
      s.items = [e.innerHTML];
    else {
      const i = Array.from(e.querySelectorAll("LI"));
      s.items = i.map((n) => n.innerHTML).filter((n) => !!n.trim());
    }
    return s;
  }
}
export {
  d as default
};
