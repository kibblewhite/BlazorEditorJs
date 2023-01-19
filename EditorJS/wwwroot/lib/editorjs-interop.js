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

    init(id, jsob, load_plugins, instance, callback) {

        let tools = {};

        if (typeof window.Header !== 'undefined') {
            tools = Object.assign(tools, {
                header: {
                    class: Header,
                    inlineToolbar: ['marker', 'link'],
                    config: {
                        placeholder: 'Header'
                    },
                    shortcut: 'CMD+SHIFT+H'
                }
            });
        }

        if (typeof window.LinkTool !== 'undefined') {
            tools = Object.assign(tools, {
                linkTool: LinkTool
            });
        }

        if (typeof window.List !== 'undefined') {
            tools = Object.assign(tools, {
                list: {
                    class: List,
                    inlineToolbar: true,
                    shortcut: 'CMD+SHIFT+L'
                }
            });
        }

        if (typeof window.NestedList !== 'undefined') {
            tools = Object.assign(tools, {
                list: {
                    class: NestedList,
                    inlineToolbar: true,
                    config: {
                        defaultStyle: 'unordered'
                    },
                }
            });
        }

        if (typeof window.Marker !== 'undefined') {
            tools = Object.assign(tools, {
                marker: {
                    class: Marker,
                    shortcut: 'CMD+SHIFT+M'
                }
            });
        }

        if (typeof window.Warning !== 'undefined') {
            tools = Object.assign(tools, {
                warning: Warning
            });
        }

        if (typeof window.Checklist !== 'undefined') {
            tools = Object.assign(tools, {
                checklist: {
                    class: Checklist,
                    inlineToolbar: true
                }
            });
        }

        if (typeof window.CodeTool !== 'undefined') {
            tools = Object.assign(tools, {
                code: CodeTool
            });
        }

        if (typeof window.Delimiter !== 'undefined') {
            tools = Object.assign(tools, {
                delimiter: Delimiter
            });
        }

        if (typeof window.Embed !== 'undefined') {
            tools = Object.assign(tools, {
                embed: {
                    class: Embed,
                    config: {
                        services: {
                            youtube: true,
                            vimeo: true,
                            imgur: true,
                            twitter: true,
                            facebook: true,
                            instagram : true
                        }
                    }
                }
            });
        }

        if (typeof window.SimpleImage !== 'undefined') {
            tools = Object.assign(tools, {
                image: SimpleImage
            });
        }

        if (typeof window.InlineCode !== 'undefined') {
            tools = Object.assign(tools, {
                inlineCode: {
                    class: InlineCode,
                    shortcut: 'CMD+SHIFT+M',
                }
            });
        }

        if (typeof window.Quote !== 'undefined') {
            tools = Object.assign(tools, {
                quote: {
                    class: Quote,
                    inlineToolbar: true,
                    config: {
                        quotePlaceholder: 'Enter a quote',
                        captionPlaceholder: 'Quote\'s author',
                    },
                    shortcut: 'CMD+SHIFT+O'
                }
            });
        }

        if (typeof window.Table !== 'undefined') {
            tools = Object.assign(tools, {
                table: {
                    class: Table,
                    inlineToolbar: true,
                    shortcut: 'CMD+ALT+T'
                }
            });
        }

        console.log('load_plugins', load_plugins);
        console.log('tools', tools);

        let options = {
            data: jsob,
            readOnly: false,
            tools: tools,
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
