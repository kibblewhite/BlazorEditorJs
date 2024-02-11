# editorjs-text

A plain text-element block for editorjs. You can capture the enter key to prevent new lines or line breaks from been created.

## NPM and Build

```
npm install vite@latest
npm install @codexteam/icons@latest
npm install vite-plugin-css-injected-by-js --save-dev
```

```
npm install
npm update
```

Build the distribution.

```
npm run build
```

Open `dist\index.html` in a local browser.

## Usage

Add a new Tool to the `tools` property of the CodeX Editor initial config.

```javascript
var editor = CodexEditor({
  ...
  
  tools: {
    ...
    textElement: TextElement,
  },
  
  ...
});
```

Or init Warning Tool with additional settings

```javascript
var editor = CodexEditor({
  ...
  
  tools: {
    ...
    textElement: {
        class: TextElement,
        inlineToolbar: true,
        config: {
            placeholder: '...',
            preserveBlank: false,
            allowEnterKeyDown: false,
            hidePopoverItem: true,
            hideToolbar: true
        }
    },
  },
  
  ...
});
```
#### Include as JS

```
https://cdn.jsdelivr.net/npm/editorjs-text/dist/bundle.min.js
```

## Config Params

| Field              | Type      | Description                       |
| ------------------ | --------- | ----------------------------------|
| placeholder        | `string`  | The placeholder. Will be shown only in the first text entry when the editor is empty.  |
| preserveBlank      | `boolean` | (default: `false`) Whether or not to keep blank paragraphs when saving editor data |
| allowEnterKeyDown  | `boolean` | (default: `false`) Whether or not to capture when the enter key or shift+enter key is pressed |
| hidePopoverItem    | `boolean` | (default: `false`) Whether or not to display the toolbar's text popover item within the edit toolbox (remember there is a difference between the toolbar and toolbox) |
| hideToolbar        | `boolean` | (default: `false`) Whether or not to display the text toolbar (note: this will affect all other toolbar items in that editor) |

## Output data

| Field  | Type     | Description      |
| ------ | -------- | ---------------- |
| text   | `string` | html text output without line breaks or new lines |


```json
{
    "type" : "text",
    "data" : {
        "text" : "Check out our projects on a <a href=\"https://github.com/codex-team\">GitHub page</a>.",
    }
}
```

## Issues

### Vite

If you get this during the `npm run build`:

```
'vite' is not recognized as an internal or external command, operable program or batch file.
```

Please ensure you have performed the npm install commands earlier in this document.

---

```
file://.../editorjs-text/node_modules/vite/bin/vite.js:7
    await import('source-map-support').then((r) => r.default.install())
    ^^^^^

SyntaxError: Unexpected reserved word
    at Loader.moduleStrategy (internal/modules/esm/translators.js:133:18)
    at async link (internal/modules/esm/module_job.js:42:21)
```

During the npm install, you may have an older version of node/npm/nvm.

Please go through the update process successfully and then try again.


### Console Errors in the Browser

```
Failed to load resource: net::ERR_FILE_NOT_FOUND
```

Or

```
Editor.js is not ready because of TypeError: Cannot set properties of undefined (setting 'focused')
```

Means you are running the wrong `index.html` please open the correct `dist\index.html` into your local browser from the local file system.

If the `dist\index.html` file is missing, you need to run the `npm run build` command in order to build the html.

### Known Issue

[https://github.com/codex-team/editor.js/discussions/1897#discussioncomment-5503544](https://github.com/codex-team/editor.js/discussions/1897#discussioncomment-5503544)

```
Paste handling for «paragraph» Tool hasn't been set up because of the error TypeError: this.constructable is not a constructor
```

Possible solution is to create a blank plug-in that can be used as drop-in replacement for the paragraph plug-in.
