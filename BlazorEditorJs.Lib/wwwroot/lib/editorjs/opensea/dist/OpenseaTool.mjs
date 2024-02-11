(function(){"use strict";try{if(typeof document!="undefined"){var e=document.createElement("style");e.appendChild(document.createTextNode("._opensea-tool_76yf4_1{margin:1em auto}")),document.head.appendChild(e)}}catch(t){console.error("vite-plugin-css-injected-by-js",t)}})();
var i = Object.defineProperty;
var o = (a, t, e) => t in a ? i(a, t, { enumerable: !0, configurable: !0, writable: !0, value: e }) : a[t] = e;
var r = (a, t, e) => (o(a, typeof t != "symbol" ? t + "" : t, e), e);
const p = {
  "opensea-tool": "_opensea-tool_76yf4_1"
};
class n {
  constructor({ data: t, config: e, api: s, readOnly: d }) {
    r(this, "api");
    r(this, "readOnly");
    r(this, "data");
    r(this, "config");
    r(this, "nodes");
    this.data = t, this.config = e, this.api = s, this.readOnly = d, this.nodes = {
      wrapper: null,
      nftCard: null
    };
  }
  static prepare() {
    const t = "https://unpkg.com/embeddable-nfts/dist/nft-card.min.js";
    if (!document.querySelector(`script[src="${t}"]`)) {
      const s = document.createElement("script");
      s.src = t, document.head.appendChild(s);
    }
  }
  render() {
    return this.nodes.wrapper = document.createElement("div"), this.nodes.wrapper.classList.add(p["opensea-tool"]), this.data.contractAddress && this.data.tokenId && this.renderNftCard(), this.nodes.wrapper;
  }
  save() {
    return this.data;
  }
  onPaste(t) {
    const { data: e } = t.detail, s = e.match(n.regexp);
    if (!s || s.length < 4) {
      this.api.notifier.show({
        message: "Bad opensea item URL",
        style: "error"
      });
      return;
    }
    this.data = {
      coin: s[1],
      contractAddress: s[2],
      tokenId: s[3]
    }, this.renderNftCard();
  }
  static get pasteConfig() {
    return {
      patterns: {
        opensea: n.regexp
      }
    };
  }
  static get isReadOnlySupported() {
    return !0;
  }
  static get regexp() {
    return /^https:\/\/opensea\.io\/assets\/([a-zA-Z]+)\/([A-Za-z0-9]+)\/([A-Za-z0-9]+)$/i;
  }
  renderNftCard() {
    this.nodes.nftCard = document.createElement("nft-card"), this.nodes.nftCard.setAttribute("contractAddress", this.data.contractAddress), this.nodes.nftCard.setAttribute("tokenId", this.data.tokenId), this.nodes.wrapper ? (this.nodes.wrapper.innerHTML = "", this.nodes.wrapper.appendChild(this.nodes.nftCard)) : this.api.notifier.show({
      message: "Wrapper is not initialized",
      style: "error"
    });
  }
}
export {
  n as default
};
