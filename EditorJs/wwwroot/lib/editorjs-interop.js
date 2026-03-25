const editorjs = {

    editorjs_elements: {},
    options_naming_scheme_list: ['CamelCase', 'PascalCase', 'SnakeCase', 'KebabCase'],

    editorjs_element_selector(id, element_id, options = {}) {
        const identifier = editorjs._format_element_selector_key(id, element_id);
        const is_editor_type_defined = typeof editorjs.editorjs_elements[identifier];
        if (is_editor_type_defined === 'undefined') {
            const editorjs_element = document.getElementById(id);
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

        str = str.replace(/[^a-zA-Z0-9]/g, ' ');
        const words = str.split(' ');

        switch (OptionsNamingScheme) {
            case 'CamelCase':
                words[0] = words[0].charAt(0).toLowerCase() + words[0].slice(1);
                for (let i = 1; i < words.length; i++) {
                    words[i] = words[i].charAt(0).toUpperCase() + words[i].slice(1);
                }
                return words.join('');
            case 'PascalCase':
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

    _resolve_global(path) {
        return path.split('.').reduce((obj, key) => obj?.[key], window);
    },

    _merge_tool_options(key, tool_options, load_actions) {
        tool_options = tool_options ?? {};

        if (typeof load_actions.LoadProviderClassFunctionDefault === 'string') {
            const resolved = editorjs._resolve_global(load_actions.LoadProviderClassFunctionDefault);
            if (typeof resolved !== 'undefined') {
                tool_options.class = resolved;
            }
        } else if (typeof load_actions.LoadProviderClassFunctionDefault === 'boolean' && load_actions.LoadProviderClassFunctionDefault === false) {
            console.warn(`LoadProviderClassFunctionDefault: false is deprecated for tool "${key}". Use "TextElement.DisabledParagraph" instead.`);
            tool_options = false;
        } else {
            tool_options.class = window[key];
        }

        return tool_options;
    },

    _get_validated_load_options(tool_options) {
        const { LoadActions } = tool_options;

        if (typeof LoadActions === 'undefined') {
            throw new Error('LoadActions should be defined. Example: { "LinkTool":{"LoadActions":{ ... }} }');
        }

        if (typeof LoadActions.OptionsNamingScheme !== 'string' || editorjs.options_naming_scheme_list.includes(LoadActions.OptionsNamingScheme) !== true) {
            throw new Error('LoadActions.OptionsNamingScheme is not defined correctly. Please read any available documentation first before using this component.');
        }

        return tool_options.options;
    },

    init(id, element_id, jsob, tool_options, configurations, instance, callback, enterkeydown_callback) {
        let tools = {};
        tool_options = tool_options || {};
        configurations = configurations || { defaultBlock: 'text' };

        for (const key of Object.keys(tool_options)) {
            const has_provider = typeof tool_options[key]?.LoadActions?.LoadProviderClassFunctionDefault !== 'undefined';
            if (typeof key !== 'string' || (!has_provider && typeof window[key] === 'undefined')) {
                continue;
            }

            const tool_key_options = editorjs._get_validated_load_options(tool_options[key]);
            const tool_key = editorjs._format_string(key, tool_options[key].LoadActions);
            const output = {
                [tool_key]: editorjs._merge_tool_options(key, tool_key_options, tool_options[key].LoadActions)
            };

            tools = Object.assign(tools, output);
        }

        const editorjs_element_save_debounced_callback = editorjs.debounce((api, event, editorjs_element, instance, callback) => {
            editorjs_element.save().then((output_data) => {
                instance.invokeMethodAsync(callback, id, element_id, output_data);
            }).catch((error) => {
                console.error('Saving failed', error);
            });
        });

        let options = {
            data: jsob,
            readOnly: false,
            tools: tools,
            onChange: (api, event) => {
                editorjs_element_save_debounced_callback(api, event, editorjs_element, instance, callback);
            }
        };

        if (typeof configurations.DefaultBlock !== 'undefined') {
            options = Object.assign(options, { defaultBlock: configurations.DefaultBlock });
        }

        if (configurations?.Rtl === true) {
            options = Object.assign(options, { i18n: { direction: 'rtl' } });
            document.getElementById(id).style.direction = 'rtl';
        }

        const editorjs_element = editorjs.editorjs_element_selector(id, element_id, options);

        editorjs_element.isReady.then(() => {
            editorjs_element.events.on('block:enter', (data) => {
                editorjs_element.save().then((output_data) => {
                    instance.invokeMethodAsync(callback, id, element_id, output_data);
                }).catch((error) => {
                    console.error('Saving failed', error);
                }).then(() => {
                    instance.invokeMethodAsync(enterkeydown_callback, id, element_id);
                });
            });
        }).catch((error) => {
            console.error('Editor initialisation failed', error);
        });

        const codex_editor = document.querySelector('#' + id);
        const codex_editor_mutation_observer = new MutationObserver(() => {
            const codex_editor_redactor = codex_editor.querySelector('div.codex-editor__redactor');
            if (codex_editor_redactor !== null) {
                const codex_editor_redactor_style_config = configurations?.CodexEditorRedactor?.style ?? {};
                Object.assign(codex_editor_redactor.style, codex_editor_redactor_style_config);
                codex_editor_mutation_observer.disconnect();
            }
        });

        codex_editor_mutation_observer.observe(codex_editor, { childList: true, subtree: true, attributes: true });
    },

    render(id, element_id, jsob) {
        const editorjs_element = editorjs.editorjs_element_selector(id, element_id);
        editorjs_element.render(jsob);
    },

    clear(id, element_id) {
        const editorjs_element = editorjs.editorjs_element_selector(id, element_id);
        editorjs_element.blocks.clear();
    },

    focus(id, element_id, on_last) {
        const editorjs_element = editorjs.editorjs_element_selector(id, element_id);
        if (editorjs_element === null || typeof editorjs_element.focus === 'undefined') { return; }
        editorjs_element.focus(on_last);
    },

    toggle(id, element_id, state) {
        const editorjs_element = editorjs.editorjs_element_selector(id, element_id);
        if (editorjs_element === null) { return; }
        editorjs_element.readOnly.toggle(state);
    },

    debounce(callback, delay = 1) {
        let timeout_id;
        return (...args) => {
            clearTimeout(timeout_id);
            timeout_id = setTimeout(() => {
                timeout_id = null;
                callback(...args);
            }, delay);
        };
    }

};

export { editorjs };
