const editorjs = {

    editorjs_elements: {},
    options_naming_scheme_list: ["CamelCase", "PascalCase", "SnakeCase", "KebabCase"],
    supported_tools_configuration_options: {
        Header: {
            inlineToolbar: ['marker', 'link'],
            config: {
                placeholder: 'Header'
            },
            shortcut: 'CMD+SHIFT+H'
        },
        List: {
            inlineToolbar: true,
            shortcut: 'CMD+SHIFT+L'
        },
        LinkTool: {
            class: LinkTool
        },
        NestedList: {
            inlineToolbar: true,
            config: {
                defaultStyle: 'unordered'
            }
        },
        Marker: {
            shortcut: 'CMD+SHIFT+M'
        },
        Warning: {
            class: Warning
        },
        CheckList: {
            inlineToolbar: true
        },
        CodeTool: {
            class: CodeTool
        },
        Delimiter: {
            class: Delimiter
        },
        Embed: {
            inlineToolbar: true,
            config: {
                services: {
                    "youtube": false
                }
            }
        },
        SimpleImage: {
            class: SimpleImage
        },
        InlineCode: {
            shortcut: 'CMD+SHIFT+M',
        },
        Quote: {
            inlineToolbar: true,
            config: {
                quotePlaceholder: 'Enter a quote',
                captionPlaceholder: 'Quote\'s author',
            },
            shortcut: 'CMD+SHIFT+O'
        },
        Table: {
            inlineToolbar: true,
            shortcut: 'CMD+ALT+T'
        }
    },

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

    _format_string(str, load_actions) {

        const { OptionsNamingScheme, OverrideOptionsKey } = load_actions;

        if (typeof OverrideOptionsKey !== 'undefined') {
            return OverrideOptionsKey; 
        }

        // Replace all non-alphanumeric characters with spaces
        str = str.replace(/[^a-zA-Z0-9]/g, ' ');

        // Split the string into words
        let words = str.split(' ');

        switch (OptionsNamingScheme) {
            case "CamelCase":
                // Ensure that the first letter is lower cased
                words[0] = words[0].charAt(0).toLowerCase() + words[0].slice(1);
                // Iterate through the words, capitalizing the first letter of each word and joining them
                for (let i = 1; i < words.length; i++) {
                    words[i] = words[i].charAt(0).toUpperCase() + words[i].slice(1);
                }
                return words.join('');
            case "PascalCase":
                // Iterate through the words, capitalizing the first letter of each word and joining them
                for (let i = 0; i < words.length; i++) {
                    words[i] = words[i].charAt(0).toUpperCase() + words[i].slice(1);
                }
                return words.join('');
            case "SnakeCase":
                return words.join('_').toLowerCase();
            case "KebabCase":
                return words.join('-').toLowerCase();
        }

        throw new Error(`Invalid format provided. Choose one of ["${editorjs.options_naming_scheme_list.join('", "')}"]`);

    },

    // The `_is_plain_property` function is used to check if a given json object is a plain property.
    // A plain property is defined as one of the following:
    // - The json object is null
    // - The json object is undefined
    // - The json object is an empty object (i.e. it has no properties)
    _is_plain_property(json) {
        return json === null || typeof json === 'undefined' || (typeof json === 'object' && Object.keys(json).length === 0);
    },

    _merge_tool_options(id, key, tool_options, load_actions) {

        let default_options = editorjs.supported_tools_configuration_options[key] ?? {};
        tool_options = Object.assign(default_options, tool_options);

        if (typeof load_actions.LoadProviderClassFunctionDefault !== 'undefined' && typeof window[load_actions.LoadProviderClassFunctionDefault] !== 'undefined') {
            tool_options.class = window[load_actions.LoadProviderClassFunctionDefault];
        } else {
            tool_options.class = window[key];
        }

        return tool_options;
    },

    // Retrieves options and checks the presences of the `LoadActions.OptionsNamingScheme`
    _get_validated_load_options(tool_options) {

        // Destructuring assignment to extract LoadActions property
        const { LoadActions } = tool_options;

        // Check if LoadActions is undefined
        if (typeof LoadActions === 'undefined') {
            // Throws an error if LoadActions is not defined with reminder in the error
            throw new Error('LoadActions should be defined. Example: { "LinkTool":{"LoadActions":{ ... }} }');
        }

        // Check if LoadActions.OptionsNamingScheme is undefined
        if (typeof LoadActions.OptionsNamingScheme !== 'string' || editorjs.options_naming_scheme_list.includes(LoadActions.OptionsNamingScheme) != true) {
            // Throws an error if LoadActions.OptionsNamingScheme is not defined, chances are high that the developer has not read the documentation and has only reacted to the first error message
            throw new Error('LoadActions.OptionsNamingScheme is not defined correctly. Please read any available documentation first before using this component.');
        }

        // Return tool_options.options if no other conditions are met
        return tool_options.options;

    },

    init(id, jsob, tool_options, instance, callback) {

        let tools = {};

        for (let key of Object.keys(tool_options)) {

            if (typeof key !== 'string' || typeof window[key] === 'undefined') {
                continue;
            }

            let options = editorjs._get_validated_load_options(tool_options[key]);
            let tool_key = editorjs._format_string(key, tool_options[key].LoadActions);
            let output = {
                [tool_key]: editorjs._merge_tool_options(id, key, options, tool_options[key].LoadActions)
            };

            tools = Object.assign(tools, output);
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
