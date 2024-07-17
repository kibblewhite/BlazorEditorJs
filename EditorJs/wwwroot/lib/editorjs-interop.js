const editorjs = {

    editorjs_elements: {},
    options_naming_scheme_list: [ 'CamelCase', 'PascalCase', 'SnakeCase', 'KebabCase' ],
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
    editorjs_element_selector(id, element_id, options = {}) {

        let identifier = editorjs._format_element_selector_key(id, element_id);
        if (typeof editorjs.editorjs_elements[identifier] === 'undefined') {

            let editorjs_element = document.getElementById(id);
            if (editorjs_element === null) { return null; }

            options = Object.assign(options, { holder: id });
            editorjs.editorjs_elements[identifier] = new EditorJS(options);

        }

        return editorjs.editorjs_elements[identifier];

    },

    _format_element_selector_key(id, element_id) {
        return id + '.' + element_id;
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
            case 'CamelCase':
                // Ensure that the first letter is lower cased
                words[0] = words[0].charAt(0).toLowerCase() + words[0].slice(1);
                // Iterate through the words, capitalizing the first letter of each word and joining them
                for (let i = 1; i < words.length; i++) {
                    words[i] = words[i].charAt(0).toUpperCase() + words[i].slice(1);
                }
                return words.join('');
            case 'PascalCase':
                // Iterate through the words, capitalizing the first letter of each word and joining them
                for (let i = 0; i < words.length; i++) {
                    words[i] = words[i].charAt(0).toUpperCase() + words[i].slice(1);
                }
                return words.join('');
            case 'SnakeCase':
                return words.join('_').toLowerCase();
            case 'KebabCase':
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

    _merge_tool_options(key, tool_options, load_actions) {

        let default_options = editorjs.supported_tools_configuration_options[key] ?? {};
        tool_options = Object.assign(default_options, tool_options);

        if (typeof load_actions.LoadProviderClassFunctionDefault !== 'undefined' && typeof window[load_actions.LoadProviderClassFunctionDefault] !== 'undefined') {
            tool_options.class = window[load_actions.LoadProviderClassFunctionDefault];
        } else if (typeof load_actions.LoadProviderClassFunctionDefault === 'boolean' && load_actions.LoadProviderClassFunctionDefault === false) {
            tool_options = false;
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

    init(id, element_id, jsob, tool_options, configurations, instance, callback) {

        let tools = {};

        for (let key of Object.keys(tool_options)) {

            if (typeof key !== 'string' || typeof window[key] === 'undefined') {
                continue;
            }

            let tool_key_options = editorjs._get_validated_load_options(tool_options[key]);
            let tool_key = editorjs._format_string(key, tool_options[key].LoadActions);
            let output = {
                [tool_key]: editorjs._merge_tool_options(key, tool_key_options, tool_options[key].LoadActions)
            };

            tools = Object.assign(tools, output);
        }

        let options = {
            data: jsob,
            readOnly: false,
            tools: tools,
            onChange: (api, event) => {

                // let block_count_limit = typeof configurations.BlockCountLimit === 'undefined' ? 0 : configurations.BlockCountLimit;
                //if (block_count_limit !== 0) {
                //    let block_count = api.blocks.getBlocksCount();
                //    if (block_count > block_count_limit) {
                //        let block_limit_index = block_count - 2;
                //        let current_block_index = api.blocks.getCurrentBlockIndex();
                //        api.blocks.delete(current_block_index);
                //        api.caret.setToBlock('end', block_limit_index);
                //        for (let i = block_count - 1; i >= block_limit_index; i--) {
                //            if (i <= block_limit_index || i == current_block_index) { continue; }
                //            api.blocks.delete(i);
                //        }
                //    }
                //}

                // <div class="ce-popover-item" data-item-name="text" style="display: none;"></div>

                editorjs_element_save_debounced_callback(api, event, editorjs_element, instance, callback);
            }
        };

        if (typeof configurations.DefaultBlock !== 'undefined') {
            let default_block_option = { defaultBlock: configurations.DefaultBlock };
            options = Object.assign(options, default_block_option);
        }

        let editorjs_element = editorjs.editorjs_element_selector(id, element_id, options);

        //////

        const codex_editor = document.querySelector('#' + id); // api.ui.nodes.wrapper.parentElement
        const codex_editor_mutation_observer = new MutationObserver(() => {
            let codex_editor_redactor = codex_editor.querySelector('div.codex-editor__redactor');
            if (typeof codex_editor_redactor !== 'undefined') {
                // Use optional chaining and nullish coalescing to handle potential undefined/null properties
                let codex_editor_redactor_style_config = configurations?.CodexEditorRedactor?.style ?? {};
                // Merge configurations.CodexEditorRedactor.style into codex_editor_redactor.style
                Object.assign(codex_editor_redactor.style, codex_editor_redactor_style_config);
                codex_editor_mutation_observer.disconnect();
            }
        });

        // Configure and start the codex_editor_mutation_observer
        codex_editor_mutation_observer.observe(codex_editor, { childList: true, subtree: true, attributes: true });

        //////

        const editorjs_element_save_debounced_callback = editorjs.debounce((api, event, editorjs_element, instance, callback) => {
            editorjs_element.save().then((output_data) => {
                instance.invokeMethodAsync(callback, id, element_id, output_data);
            }).catch((error) => {
                console.error('Saving Failed', error, api, event);
                // todo (2023-11-19|kibble): Invoke (using instance.invokeMethodAsync) a failure endpoint to log error and record incident to a backend
            });
        });

    },

    render(id, element_id, jsob) {
        let editorjs_element = editorjs.editorjs_element_selector(id, element_id);
        console.debug('render', id, element_id, jsob, editorjs_element);
        editorjs_element.blocks.clear();
        editorjs_element.render(jsob);
    },

    debounce(callback, delay = 60) {
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
