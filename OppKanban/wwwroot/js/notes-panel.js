window.notesPanel = (() => {
    const allowedTags = new Set([
        "A", "B", "BLOCKQUOTE", "BR", "DIV", "EM", "H1", "H2", "H3",
        "I", "LI", "OL", "P", "STRONG", "U", "UL"
    ]);

    function safeLinkUrl(value) {
        try {
            const url = new URL(value, window.location.href);
            return ["http:", "https:", "mailto:"].includes(url.protocol) ? url.href : null;
        } catch {
            return null;
        }
    }

    function copySafeNodes(source, destination) {
        for (const node of source.childNodes) {
            if (node.nodeType === Node.TEXT_NODE) {
                destination.appendChild(document.createTextNode(node.textContent ?? ""));
                continue;
            }

            if (node.nodeType !== Node.ELEMENT_NODE) {
                continue;
            }

            const element = node;
            const childrenTarget = allowedTags.has(element.tagName)
                ? document.createElement(element.tagName.toLowerCase())
                : destination;
            if (element.tagName === "A") {
                const href = safeLinkUrl(element.getAttribute("href") ?? "");
                if (href) {
                    childrenTarget.setAttribute("href", href);
                    childrenTarget.setAttribute("target", "_blank");
                    childrenTarget.setAttribute("rel", "noopener noreferrer");
                }
            }
            copySafeNodes(element, childrenTarget);
            if (childrenTarget !== destination) {
                destination.appendChild(childrenTarget);
            }
        }
    }

    return {
        setHtml(id, html) {
            const editor = document.getElementById(id);
            if (!editor) {
                throw new Error("Notes editor was not found.");
            }

            const parsed = new DOMParser().parseFromString(html ?? "", "text/html");
            const safeContent = document.createDocumentFragment();
            copySafeNodes(parsed.body, safeContent);
            editor.replaceChildren(safeContent);
        },
        getHtml(id) {
            const editor = document.getElementById(id);
            if (!editor) {
                throw new Error("Notes editor was not found.");
            }

            return editor.innerHTML;
        },
        executeCommand(command, value) {
            const commandValue = command === "formatBlock" && value
                ? `<${value}>`
                : value ?? null;
            document.execCommand(command, false, commandValue);
        },
        createLink() {
            const value = window.prompt("Enter a link URL");
            const href = value ? safeLinkUrl(value) : null;
            if (href) {
                document.execCommand("createLink", false, href);
            }
        }
    };
})();
