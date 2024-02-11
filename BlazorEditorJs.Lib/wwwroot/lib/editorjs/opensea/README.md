# OpenSea Tool for Editor.js

Parses the pasted link to OpenSea item and inserts the embed block.

![Opensea Tool example card](assets/demo.png)

## Installation

Use your package manager to install the `@editorjs/opensea` package.

```
npm install -D @editorjs/opensea

yarn add -D @editorjs/opensea
```

## Usage

Import and add the Tool to Editor.js tools config.

```javascript
import OpenseaTool from '@editorjs/opensea';

const editor = new EditorJS({
  tools: {
    opensea: {
      class: OpenseaTool
    },
  },

  // ...
});
```

Check out the [example page](./index.html).

## Output Data

Check the `OpenseaToolData` interface in [src/types/index.ts](./src/types/index.ts) file with types.

## Render Template

Use the following HTML to render the block.

```html
<nft-card contractAddress="{ data.contractAddress }" tokenId="{ data.tokenId }"></nft-card>

<script src="https://unpkg.com/embeddable-nfts/dist/nft-card.min.js"></script>
```

## Development

This tool uses [Vite](https://vitejs.dev/) as builder.

`npm run dev` — run development environment with hot reload

`npm run build` — build the tool for production to the `dist` folder

## Links

[Editor.js](https://editorjs.io) • [Create Tool](https://github.com/editor-js/create-tool)

## About CodeX

<img align="right" width="120" height="120" src="https://codex.so/public/app/img/codex-logo.svg" hspace="50">

CodeX is a team of digital specialists around the world interested in building high-quality open source products on a global market. We are [open](https://codex.so/join) for young people who want to constantly improve their skills and grow professionally with experiments in cutting-edge technologies.

| 🌐 | Join  👋  | Twitter | Instagram |
| -- | -- | -- | -- |
| [codex.so](https://codex.so) | [codex.so/join](https://codex.so/join) |[@codex_team](http://twitter.com/codex_team) | [@codex_team](http://instagram.com/codex_team/) |