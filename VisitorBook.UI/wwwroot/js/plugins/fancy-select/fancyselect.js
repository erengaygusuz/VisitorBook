/*!
 * Copyright (c) 2021 Momo Bassit.
 * Licensed under the MIT License (MIT)
 * https://github.com/mdbassit/fancySelect
 */
!(function (a, m, e) {
    var r = null,
        l = "",
        s = null,
        h = 0;
    function t(e) {
        m.querySelectorAll((e = e || "select:not(.fsb-ignore)")).forEach(n);
    }
    function n(e, t) {
        if (!e.nextElementSibling || !e.nextElementSibling.classList.contains("fsb-select")) {
            var n = e.children,
                i = e.parentNode,
                a = m.createElement("span"),
                r = m.createElement("span"),
                l = m.createElement("button"),
                s = m.createElement("span"),
                o = m.createElement("span"),
                c = h++;
            e.classList.add("fsb-original-select"),
                (r.id = "fsb_" + c + "_label"),
                (r.className = "fsb-label"),
                (r.textContent = g(e, i)),
                (l.id = "fsb_" + c + "_button"),
                (l.className = "fsb-button"),
                (l.textContent = "&nbsp;"),
                l.setAttribute("type", "button"),
                l.setAttribute("aria-disabled", e.disabled),
                l.setAttribute("aria-haspopup", "listbox"),
                l.setAttribute("aria-expanded", "false"),
                l.setAttribute("aria-labelledby", "fsb_" + c + "_label fsb_" + c + "_button"),
                (s.className = "fsb-list"),
                s.setAttribute("role", "listbox"),
                s.setAttribute("tabindex", "-1"),
                s.setAttribute("aria-labelledby", "fsb_" + c + "_label");
            for (var u = 0, d = n.length; u < d; u++) {
                var f = E(n[u], t),
                    b = f.item,
                    p = f.selected,
                    f = f.itemLabel;
                s.appendChild(b), p && (l.innerHTML = f);
            }
            (a.className = "fsb-select"),
                a.appendChild(r),
                a.appendChild(l),
                a.appendChild(s),
                a.appendChild(o),
                (e.style.display = "none"),
                e.nextSibling ? i.insertBefore(a, e.nextSibling) : i.appendChild(a),
                s.firstElementChild && ((a = m.createElement("span")).setAttribute("style", "width: " + s.firstElementChild.offsetWidth + "px;"), (o.className = "fsb-resize"), o.appendChild(a));
        }
    }
    function i(e, t) {
        var n = e.children,
            i = e.parentNode,
            a = e.nextElementSibling;
        if (a && a.classList.contains("fsb-select")) {
            var r = a.firstElementChild,
                l = r.nextElementSibling,
                s = l.nextElementSibling,
                a = s.nextElementSibling,
                o = m.createDocumentFragment();
            (r.textContent = g(e, i)), l.setAttribute("aria-disabled", e.disabled);
            for (var c = 0, u = n.length; c < u; c++) {
                var d = E(n[c], t),
                    f = d.item,
                    b = d.selected,
                    d = d.itemLabel;
                o.appendChild(f), b && (l.innerHTML = d);
            }
            for (; s.firstChild;) s.removeChild(s.firstChild);
            s.appendChild(o), s.firstElementChild && a.firstElementChild.setAttribute("style", "width: " + s.firstElementChild.offsetWidth + "px;");
        }
    }
    function g(t, e) {
        var n,
            i = t.id;
        if (("LABEL" === e.nodeName ? (n = e) : void 0 !== i && (n = m.querySelector('label[for="' + i + '"]')), n)) {
            i = [].filter
                .call(n.childNodes, function (e) {
                    return 3 === e.nodeType;
                })
                .map(function (e) {
                    return e.textContent.replace(/\s+/g, " ").trim();
                })
                .filter(function (e) {
                    return "" !== e;
                })[0];
            if (i)
                return (
                    (n.onclick = function (e) {
                        t.nextElementSibling.querySelector("button").click(), e.preventDefault(), e.stopImmediatePropagation();
                    }),
                    i
                );
        }
        return "";
    }
    function E(e, t) {
        var n = m.createElement("span"),
            i = e.selected,
            t = (function (e, t) {
                if ("function" == typeof t) return t(e);
                (t = e.text), (e = e.getAttribute("data-icon")), (t = "" !== t ? t : "&nbsp;");
                (t = "<span>" + t + "</span>"), null !== e && (t = '<svg aria-hidden="true"><use href="' + e + '"></use></svg> ' + t);
                return t;
            })(e, t);
        return (n.className = "fsb-option"), (n.innerHTML = t), n.setAttribute("role", "option"), n.setAttribute("tabindex", "-1"), n.setAttribute("aria-selected", i), { item: n, selected: i, itemLabel: t };
    }
    function o(e) {
        var t = e.getBoundingClientRect(),
            n = e.nextElementSibling,
            i = (i = n.querySelector('[aria-selected="true"]')) || n.firstElementChild;
        (e.parentNode.className = "fsb-select"), e.setAttribute("aria-expanded", "true"), i.focus(), (r = e), t.y + t.height + n.offsetHeight > m.documentElement.clientHeight && (e.parentNode.className = "fsb-select fsb-top");
    }
    function c(e) {
        var t = m.querySelector('.fsb-button[aria-expanded="true"]');
        t && (t.setAttribute("aria-expanded", "false"), e && t.focus(), (l = ""), (s = null)), (r = null);
    }
    function u(e) {
        var t = e.parentNode,
            n = t.previousElementSibling,
            i = [].indexOf.call(t.children, e),
            a = t.querySelector('[aria-selected="true"]'),
            t = t.parentNode.previousElementSibling;
        a && a.setAttribute("aria-selected", "false"),
            e.setAttribute("aria-selected", "true"),
            (n.innerHTML = e.innerHTML),
            (t.selectedIndex = i),
            t.dispatchEvent(new Event("input", { bubbles: !0 })),
            t.dispatchEvent(new Event("change", { bubbles: !0 }));
    }
    function d(e) {
        e = (function (e, t) {
            var n,
                i = [].map.call(e.children, function (e) {
                    return e.textContent.trim().toLowerCase();
                }),
                a = f(i, t)[0];
            if (a) return e.children[i.indexOf(a)];
            if (
                (n = t.split("")).every(function (e) {
                    return e === n[0];
                })
            ) {
                (a = f(i, t[0])), (a = a[(t.length - 1) % a.length]);
                return e.children[i.indexOf(a)];
            }
            return null;
        })(e, l);
        e && e.focus();
    }
    function f(e, t) {
        return e.filter(function (e) {
            return 0 === e.indexOf(t.toLowerCase());
        });
    }
    function b(e) {
        var t = e.key,
            n = e.altKey,
            i = e.ctrlKey,
            e = e.metaKey;
        return (
            !(1 !== t.length || n || i || e) &&
            (s && a.clearTimeout(s),
                (s = a.setTimeout(function () {
                    l = "";
                }, 500)),
                (l += t),
                1)
        );
    }
    function p(e, t, n, i) {
        var a = Element.prototype.matches || Element.prototype.msMatchesSelector;
        "string" == typeof n
            ? e.addEventListener(t, function (e) {
                a.call(e.target, n) && i.call(e.target, e);
            })
            : ((i = n), e.addEventListener(t, i));
    }
    function v(e, t) {
        (t = void 0 !== t ? t : []),
            "loading" !== m.readyState
                ? e.apply(void 0, t)
                : m.addEventListener("DOMContentLoaded", function () {
                    e.apply(void 0, t);
                });
    }
    function y() {
        v(t);
    }
    p(m, "click", ".fsb-button", function (e) {
        var t = r === e.target;
        c(), t || o(e.target), e.preventDefault(), e.stopImmediatePropagation();
    }),
        p(m, "keydown", ".fsb-button", function (e) {
            var t = e.target,
                n = t.nextElementSibling,
                i = !0;
            switch (e.key) {
                case "ArrowUp":
                case "ArrowDown":
                case "Enter":
                case " ":
                    o(t);
                    break;
                default:
                    b(e) ? (o(t), d(n)) : (i = !1);
            }
            i && e.preventDefault();
        }),
        p(m.documentElement, "mousemove", ".fsb-option", function (e) {
            e.target.focus();
        }),
        p(m, "click", ".fsb-option", function (e) {
            u(e.target), c(!0);
        }),
        p(m, "keydown", ".fsb-option", function (e) {
            var t = e.target,
                n = t.parentNode,
                i = !0;
            switch (e.key) {
                case "ArrowUp":
                case "ArrowLeft":
                    t.previousElementSibling && t.previousElementSibling.focus();
                    break;
                case "ArrowDown":
                case "ArrowRight":
                    t.nextElementSibling && t.nextElementSibling.focus();
                    break;
                case "Home":
                    n.firstElementChild.focus();
                    break;
                case "End":
                    n.lastElementChild.focus();
                    break;
                case "PageUp":
                case "PageDown":
                    break;
                case "Tab":
                    u(t), c(), (i = !1);
                    break;
                case "Enter":
                case " ":
                    u(t);
                case "Escape":
                    c(!0);
                    break;
                default:
                    b(e) ? d(n) : (i = !1);
            }
            i && e.preventDefault();
        }),
        p(m, "click", function (e) {
            c();
        }),
        (a.FancySelect = ((y.init = t), (y.replace = n), (y.update = i), y)),
        e && v(t);
})(window, document, "undefined" == typeof FancySelectAutoInitialize || FancySelectAutoInitialize);
