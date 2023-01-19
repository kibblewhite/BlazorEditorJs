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

    _merge_tool_options(class_fn, default_options, tool_options) {
        tool_options = Object.assign(default_options, tool_options);
        tool_options.class = class_fn;
        return tool_options;
    },

    init(id, jsob, tool_options, instance, callback) {

        let tools = {};

        if (typeof window.Header !== 'undefined' && typeof tool_options.header !== 'undefined') {
            tools = Object.assign(tools, {
                header: editorjs._merge_tool_options(Header, {
                    inlineToolbar: ['marker', 'link'],
                    config: {
                        placeholder: 'Header'
                    },
                    shortcut: 'CMD+SHIFT+H'
                }, tool_options.header)
            });
        }

        if (typeof window.LinkTool !== 'undefined' && typeof tool_options.linkTool !== 'undefined') {
            tools = Object.assign(tools, {
                linkTool: editorjs._merge_tool_options(LinkTool, LinkTool, tool_options.linkTool)
            });
        }

        if (typeof window.List !== 'undefined' && typeof tool_options.list !== 'undefined') {
            tools = Object.assign(tools, {
                list: editorjs._merge_tool_options(List, {
                    inlineToolbar: true,
                    shortcut: 'CMD+SHIFT+L'
                }, tool_options.list)
            });
        }

        if (typeof window.NestedList !== 'undefined' && typeof tool_options.nestedList !== 'undefined') {
            tools = Object.assign(tools, {
                list: editorjs._merge_tool_options(NestedList, {
                    inlineToolbar: true,
                    config: {
                        defaultStyle: 'unordered'
                    }
                }, tool_options.list) 
            });
        }

        if (typeof window.Marker !== 'undefined' && typeof tool_options.marker !== 'undefined') {
            tools = Object.assign(tools, {
                marker: editorjs._merge_tool_options(Marker, {
                    shortcut: 'CMD+SHIFT+M'
                }, tool_options.marker)
            });
        }

        if (typeof window.Warning !== 'undefined' && typeof tool_options.warning !== 'undefined') {
            tools = Object.assign(tools, {
                warning: editorjs._merge_tool_options(Warning, Warning, tool_options.warning)
            });
        }

        if (typeof window.Checklist !== 'undefined' && typeof tool_options.checklist !== 'undefined') {
            tools = Object.assign(tools, {
                checklist: editorjs._merge_tool_options(Checklist, {
                    inlineToolbar: true
                }, tool_options.checklist)
            });
        }

        if (typeof window.CodeTool !== 'undefined' && typeof tool_options.code !== 'undefined') {
            tools = Object.assign(tools, {
                code: editorjs._merge_tool_options(CodeTool, CodeTool, tool_options.checklist)
            });
        }

        if (typeof window.Delimiter !== 'undefined' && typeof tool_options.delimiter !== 'undefined') {
            tools = Object.assign(tools, {
                delimiter: editorjs._merge_tool_options(Delimiter, Delimiter, tool_options.delimiter)
            });
        }

        if (typeof window.Embed !== 'undefined' && typeof tool_options.embed !== 'undefined') {
            tools = Object.assign(tools, {
                embed: editorjs._merge_tool_options(Embed, {
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
                }, tool_options.embed)
            });
        }

        if (typeof window.SimpleImage !== 'undefined' && typeof tool_options.simpleImage !== 'undefined') {
            tools = Object.assign(tools, {
                image: editorjs._merge_tool_options(SimpleImage, SimpleImage, tool_options.simpleImage)
            });
        }

        if (typeof window.InlineCode !== 'undefined' && typeof tool_options.inlineCode !== 'undefined') {
            tools = Object.assign(tools, {
                inlineCode: editorjs._merge_tool_options(InlineCode, {
                    shortcut: 'CMD+SHIFT+M',
                }, tool_options.inlineCode) 
            });
        }

        if (typeof window.Quote !== 'undefined' && typeof tool_options.quote !== 'undefined') {
            tools = Object.assign(tools, {
                quote: editorjs._merge_tool_options(Quote, {
                    inlineToolbar: true,
                    config: {
                        quotePlaceholder: 'Enter a quote',
                        captionPlaceholder: 'Quote\'s author',
                    },
                    shortcut: 'CMD+SHIFT+O'
                }, tool_options.quote)
            });
        }

        if (typeof window.Table !== 'undefined' && typeof tool_options.table !== 'undefined') {
            tools = Object.assign(tools, {
                table: editorjs._merge_tool_options(Table, {
                    inlineToolbar: true,
                    shortcut: 'CMD+ALT+T'
                }, tool_options.table)
            });
        }

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

    debounce(callback, delay = 320) {
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
