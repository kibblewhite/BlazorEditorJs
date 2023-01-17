const editorjs = {

    editorjs_elements: {},

    // This method handles the selection of multiple editorjs instances in the browser's DOM
    editorjs_element_selector(id, options = {}) {

        if (typeof editorjs.editorjs_elements[id] === 'undefined') {

            let editorjs_element = document.getElementById(id);
            if (editorjs_element === null) { return null; }

            options = Object.assign(options, { holder: id });
            editorjs.editorjs_elements[id] = new EditorJS(options);

        }

        return editorjs.editorjs_elements[id];

    },

    init(id, jsob, instance, callback) {

        let options = {
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

                code: CodeTool,

                inlineCode: {
                    class: InlineCode,
                    shortcut: 'CMD+SHIFT+M',
                },

                table: {
                    class: Table,
                    inlineToolbar: true,
                    shortcut: 'CMD+ALT+T'
                },

            },

            onChange: (api, event) => {
                editorjs_element_save_debounced_callback(api, event, editorjs_element, instance, callback);
            }
        };

        let editorjs_element = editorjs.editorjs_element_selector(id, options);

        const editorjs_element_save_debounced_callback = editorjs.debounce((api, event, editorjs_element, instance, callback) => {
            editorjs_element.save().then((output_data) => {
                console.debug('Saving', output_data, api, event);
                instance.invokeMethodAsync(callback, output_data);
            }).catch((error) => {
                console.error('Saving Failed', error)
            });
        });
    },

    render(id, jsob) {
        let editorjs_element = editorjs.editorjs_element_selector(id);
        console.debug("render", id, jsob, editorjs_element);
        editorjs_element.clear();
        editorjs_element.render(jsob);
    },

    debounce(callback, delay = 250) {
        let timeoutId
        return (...args) => {
            clearTimeout(timeoutId)
            timeoutId = setTimeout(() => {
                timeoutId = null
                callback(...args)
            }, delay)
        }
    }

};

export { editorjs };
