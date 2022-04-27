
const debounce = (callback, delay = 250) => {
    let timeoutId
    return (...args) => {
        clearTimeout(timeoutId)
        timeoutId = setTimeout(() => {
            timeoutId = null
            callback(...args)
        }, delay)
    }
}

export function init(id, jsob, instance, callback) {
    let editor = new EditorJS({
        holder: id,
        data: jsob,
        readOnly: false,
        tools: {
            /**
             * Each Tool is a Plugin. Pass them via 'class' option with necessary settings {@link docs/tools.md}
             */
            header: {
                class: Header,
                inlineToolbar: ['marker', 'link'],
                config: {
                    placeholder: 'Header'
                },
                shortcut: 'CMD+SHIFT+H'
            },

            /**
             * Or pass class directly without any configuration
             */
            image: SimpleImage,

            list: {
                class: List,
                inlineToolbar: true,
                shortcut: 'CMD+SHIFT+L'
            },

            checklist: {
                class: Checklist,
                inlineToolbar: true,
            },

            quote: {
                class: Quote,
                inlineToolbar: true,
                config: {
                    quotePlaceholder: 'Enter a quote',
                    captionPlaceholder: 'Quote\'s author',
                },
                shortcut: 'CMD+SHIFT+O'
            },

            warning: Warning,

            marker: {
                class: Marker,
                shortcut: 'CMD+SHIFT+M'
            },

            delimiter: Delimiter,

            embed: Embed,

            table: {
                class: Table,
                inlineToolbar: true,
                shortcut: 'CMD+ALT+T'
            },

        },
        onChange: (api, event) => {
            // this is a lot of information getting send to the web server, let's debounce this please...
            editor.save().then((outputData) => {
                console.log('Now I know that Editor\'s content changed!', event);
                console.log(outputData);
                instance.invokeMethodAsync(callback, outputData);
            }).catch((error) => {
                console.log('Saving failed: ', error)
            });
        }
    });
}


export function render(id, jsob) {
    // How does one get the already instantiated instance of the Editor JS by its id value when there are two or more instances of editorjs running in the same browser?
    // Currently ```const editor = new EditorJS({ holder: "my-id"});``` I no longer have access to the const editor property, so I would like to use the id "my-id" to get hold of the instance. I'm struggling to find documentation to confirm or reject if this is possible. I do hope it's possible to do this?
    // https://github.com/codex-team/editor.js/issues/2032

    console.log("render", id, jsob);
    //let editor = new EditorJS({
    //    holder: id
    //});
    //editor.clear();
    //editor.render(jsob);
    // https://stackoverflow.com/questions/66334297/how-to-load-blocks-data-into-editorjs
    // https://editorjs.io/blocks#render
}