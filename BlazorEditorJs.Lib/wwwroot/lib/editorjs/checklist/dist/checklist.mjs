(function(){"use strict";try{if(typeof document<"u"){var e=document.createElement("style");e.appendChild(document.createTextNode('.cdx-checklist{gap:6px;display:flex;flex-direction:column}.cdx-checklist__item{display:flex;box-sizing:content-box;align-items:flex-start}.cdx-checklist__item-text{outline:none;flex-grow:1;line-height:1.57em}.cdx-checklist__item-checkbox{width:22px;height:22px;display:flex;align-items:center;margin-right:8px;margin-top:calc(.785em - 11px);cursor:pointer}.cdx-checklist__item-checkbox svg{opacity:0;height:20px;width:20px;position:absolute;left:-1px;top:-1px;max-height:20px}@media (hover: hover){.cdx-checklist__item-checkbox:not(.cdx-checklist__item-checkbox--no-hover):hover .cdx-checklist__item-checkbox-check svg{opacity:1}}.cdx-checklist__item-checkbox-check{cursor:pointer;display:inline-block;flex-shrink:0;position:relative;width:20px;height:20px;box-sizing:border-box;margin-left:0;border-radius:5px;border:1px solid #C9C9C9;background:#fff}.cdx-checklist__item-checkbox-check:before{content:"";position:absolute;top:0;right:0;bottom:0;left:0;border-radius:100%;background-color:#369fff;visibility:hidden;pointer-events:none;transform:scale(1);transition:transform .4s ease-out,opacity .4s}@media (hover: hover){.cdx-checklist__item--checked .cdx-checklist__item-checkbox:not(.cdx-checklist__item--checked .cdx-checklist__item-checkbox--no-hover):hover .cdx-checklist__item-checkbox-check{background:#0059AB;border-color:#0059ab}}.cdx-checklist__item--checked .cdx-checklist__item-checkbox-check{background:#369FFF;border-color:#369fff}.cdx-checklist__item--checked .cdx-checklist__item-checkbox-check svg{opacity:1}.cdx-checklist__item--checked .cdx-checklist__item-checkbox-check svg path{stroke:#fff}.cdx-checklist__item--checked .cdx-checklist__item-checkbox-check:before{opacity:0;visibility:visible;transform:scale(2.5)}')),document.head.appendChild(e)}}catch(c){console.error("vite-plugin-css-injected-by-js",c)}})();
const k = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M7 12L10.4884 15.8372C10.5677 15.9245 10.705 15.9245 10.7844 15.8372L17 9"/></svg>', g = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M9.2 12L11.0586 13.8586C11.1367 13.9367 11.2633 13.9367 11.3414 13.8586L14.7 10.5"/><rect width="14" height="14" x="5" y="5" stroke="currentColor" stroke-width="2" rx="4"/></svg>';
function d() {
  const s = document.activeElement, t = window.getSelection().getRangeAt(0), n = t.cloneRange();
  return n.selectNodeContents(s), n.setStart(t.endContainer, t.endOffset), n.extractContents();
}
function C(s) {
  const e = document.createElement("div");
  return e.appendChild(s), e.innerHTML;
}
function c(s, e = null, t = {}) {
  const n = document.createElement(s);
  Array.isArray(e) ? n.classList.add(...e) : e && n.classList.add(e);
  for (const i in t)
    n[i] = t[i];
  return n;
}
function m(s) {
  return s.innerHTML.replace("<br>", " ").trim();
}
function p(s, e = !1, t = void 0) {
  const n = document.createRange(), i = window.getSelection();
  n.selectNodeContents(s), t !== void 0 && (n.setStart(s, t), n.setEnd(s, t)), n.collapse(e), i.removeAllRanges(), i.addRange(n);
}
Element.prototype.matches || (Element.prototype.matches = Element.prototype.msMatchesSelector || Element.prototype.webkitMatchesSelector);
Element.prototype.closest || (Element.prototype.closest = function(s) {
  let e = this;
  if (!document.documentElement.contains(e))
    return null;
  do {
    if (e.matches(s))
      return e;
    e = e.parentElement || e.parentNode;
  } while (e !== null && e.nodeType === 1);
  return null;
});
class f {
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
      icon: g,
      title: "Checklist"
    };
  }
  /**
   * Allow Checkbox Tool to be converted to/from other block
   *
   * @returns {{export: Function, import: Function}}
   */
  static get conversionConfig() {
    return {
      /**
       * To create exported string from the checkbox, concatenate items by dot-symbol.
       *
       * @param {ChecklistData} data - checklist data to create a string from that
       * @returns {string}
       */
      export: (e) => e.items.map(({ text: t }) => t).join(". "),
      /**
       * To create a checklist from other block's string, just put it at the first item
       *
       * @param {string} string - string to create list tool data from that
       * @returns {ChecklistData}
       */
      import: (e) => ({
        items: [
          {
            text: e,
            checked: !1
          }
        ]
      })
    };
  }
  /**
   * Render plugin`s main Element and fill it with saved data
   *
   * @param {object} options - block constructor options
   * @param {ChecklistData} options.data - previously saved data
   * @param {object} options.config - user config for Tool
   * @param {object} options.api - Editor.js API
   * @param {boolean} options.readOnly - read only mode flag
   */
  constructor({ data: e, config: t, api: n, readOnly: i }) {
    this._elements = {
      wrapper: null,
      items: []
    }, this.readOnly = i, this.api = n, this.data = e || {};
  }
  /**
   * Returns checklist tag with items
   *
   * @returns {Element}
   */
  render() {
    return this._elements.wrapper = c("div", [this.CSS.baseBlock, this.CSS.wrapper]), this.data.items || (this.data.items = [
      {
        text: "",
        checked: !1
      }
    ]), this.data.items.forEach((e) => {
      const t = this.createChecklistItem(e);
      this._elements.wrapper.appendChild(t);
    }), this.readOnly ? this._elements.wrapper : (this._elements.wrapper.addEventListener("keydown", (e) => {
      const [t, n] = [13, 8];
      switch (e.keyCode) {
        case t:
          this.enterPressed(e);
          break;
        case n:
          this.backspace(e);
          break;
      }
    }, !1), this._elements.wrapper.addEventListener("click", (e) => {
      this.toggleCheckbox(e);
    }), this._elements.wrapper);
  }
  /**
   * Return Checklist data
   *
   * @returns {ChecklistData}
   */
  save() {
    let e = this.items.map((t) => {
      const n = this.getItemInput(t);
      return {
        text: m(n),
        checked: t.classList.contains(this.CSS.itemChecked)
      };
    });
    return e = e.filter((t) => t.text.trim().length !== 0), {
      items: e
    };
  }
  /**
   * Validate data: check if Checklist has items
   *
   * @param {ChecklistData} savedData — data received after saving
   * @returns {boolean} false if saved data is not correct, otherwise true
   * @public
   */
  validate(e) {
    return !!e.items.length;
  }
  /**
   * Toggle checklist item state
   *
   * @param {MouseEvent} event - click
   * @returns {void}
   */
  toggleCheckbox(e) {
    const t = e.target.closest(`.${this.CSS.item}`), n = t.querySelector(`.${this.CSS.checkboxContainer}`);
    n.contains(e.target) && (t.classList.toggle(this.CSS.itemChecked), n.classList.add(this.CSS.noHover), n.addEventListener("mouseleave", () => this.removeSpecialHoverBehavior(n), { once: !0 }));
  }
  /**
   * Create Checklist items
   *
   * @param {ChecklistItem} item - data.item
   * @returns {Element} checkListItem - new element of checklist
   */
  createChecklistItem(e = {}) {
    const t = c("div", this.CSS.item), n = c("span", this.CSS.checkbox), i = c("div", this.CSS.checkboxContainer), o = c("div", this.CSS.textField, {
      innerHTML: e.text ? e.text : "",
      contentEditable: !this.readOnly
    });
    return e.checked && t.classList.add(this.CSS.itemChecked), n.innerHTML = k, i.appendChild(n), t.appendChild(i), t.appendChild(o), t;
  }
  /**
   * Append new elements to the list by pressing Enter
   *
   * @param {KeyboardEvent} event - keyboard event
   */
  enterPressed(e) {
    e.preventDefault();
    const t = this.items, n = document.activeElement.closest(`.${this.CSS.item}`);
    if (t.indexOf(n) === t.length - 1 && m(this.getItemInput(n)).length === 0) {
      const u = this.api.blocks.getCurrentBlockIndex();
      n.remove(), this.api.blocks.insert(), this.api.caret.setToBlock(u + 1);
      return;
    }
    const a = d(), l = C(a), r = this.createChecklistItem({
      text: l,
      checked: !1
    });
    this._elements.wrapper.insertBefore(r, n.nextSibling), p(this.getItemInput(r), !0);
  }
  /**
   * Handle backspace
   *
   * @param {KeyboardEvent} event - keyboard event
   */
  backspace(e) {
    const t = e.target.closest(`.${this.CSS.item}`), n = this.items.indexOf(t), i = this.items[n - 1];
    if (!i || !(window.getSelection().focusOffset === 0))
      return;
    e.preventDefault();
    const l = d(), r = this.getItemInput(i), h = r.childNodes.length;
    r.appendChild(l), p(r, void 0, h), t.remove();
  }
  /**
   * Styles
   *
   * @private
   * @returns {object<string>}
   */
  get CSS() {
    return {
      baseBlock: this.api.styles.block,
      wrapper: "cdx-checklist",
      item: "cdx-checklist__item",
      itemChecked: "cdx-checklist__item--checked",
      noHover: "cdx-checklist__item-checkbox--no-hover",
      checkbox: "cdx-checklist__item-checkbox-check",
      textField: "cdx-checklist__item-text",
      checkboxContainer: "cdx-checklist__item-checkbox"
    };
  }
  /**
   * Return all items elements
   *
   * @returns {Element[]}
   */
  get items() {
    return Array.from(this._elements.wrapper.querySelectorAll(`.${this.CSS.item}`));
  }
  /**
   * Removes class responsible for special hover behavior on an item
   * 
   * @private
   * @param {Element} el - item wrapper
   * @returns {Element}
   */
  removeSpecialHoverBehavior(e) {
    e.classList.remove(this.CSS.noHover);
  }
  /**
   * Find and return item's content editable element
   *
   * @private
   * @param {Element} el - item wrapper
   * @returns {Element}
   */
  getItemInput(e) {
    return e.querySelector(`.${this.CSS.textField}`);
  }
}
export {
  f as default
};
