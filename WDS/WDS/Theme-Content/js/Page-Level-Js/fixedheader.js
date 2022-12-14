/*!
   Copyright 2009-2020 SpryMedia Ltd.

 This source file is free software, available under the following license:
   MIT license - http://datatables.net/license/mit

 This source file is distributed in the hope that it will be useful, but
 WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
 or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.

 For details please refer to: http://www.datatables.net
 FixedHeader 3.1.7
 ©2009-2020 SpryMedia Ltd - datatables.net/license
*/
var $jscomp = $jscomp || {}; $jscomp.scope = {}; $jscomp.findInternal = function (c, d, f) { c instanceof String && (c = String(c)); for (var h = c.length, g = 0; g < h; g++) { var m = c[g]; if (d.call(f, m, g, c)) return { i: g, v: m } } return { i: -1, v: void 0 } }; $jscomp.ASSUME_ES5 = !1; $jscomp.ASSUME_NO_NATIVE_MAP = !1; $jscomp.ASSUME_NO_NATIVE_SET = !1; $jscomp.SIMPLE_FROUND_POLYFILL = !1;
$jscomp.defineProperty = $jscomp.ASSUME_ES5 || "function" == typeof Object.defineProperties ? Object.defineProperty : function (c, d, f) { c != Array.prototype && c != Object.prototype && (c[d] = f.value) }; $jscomp.getGlobal = function (c) { c = ["object" == typeof window && window, "object" == typeof self && self, "object" == typeof global && global, c]; for (var d = 0; d < c.length; ++d) { var f = c[d]; if (f && f.Math == Math) return f } throw Error("Cannot find global object"); }; $jscomp.global = $jscomp.getGlobal(this);
$jscomp.polyfill = function (c, d, f, h) { if (d) { f = $jscomp.global; c = c.split("."); for (h = 0; h < c.length - 1; h++) { var g = c[h]; g in f || (f[g] = {}); f = f[g] } c = c[c.length - 1]; h = f[c]; d = d(h); d != h && null != d && $jscomp.defineProperty(f, c, { configurable: !0, writable: !0, value: d }) } }; $jscomp.polyfill("Array.prototype.find", function (c) { return c ? c : function (c, f) { return $jscomp.findInternal(this, c, f).v } }, "es6", "es3");
(function (c) { "function" === typeof define && define.amd ? define(["jquery", "datatables.net"], function (d) { return c(d, window, document) }) : "object" === typeof exports ? module.exports = function (d, f) { d || (d = window); f && f.fn.dataTable || (f = require("datatables.net")(d, f).$); return c(f, d, d.document) } : c(jQuery, window, document) })(function (c, d, f, h) {
    var g = c.fn.dataTable, m = 0, l = function (a, b) {
        if (!(this instanceof l)) throw "FixedHeader must be initialised with the 'new' keyword."; !0 === b && (b = {}); a = new g.Api(a); this.c = c.extend(!0,
        {}, l.defaults, b); this.s = { dt: a, position: { theadTop: 0, tbodyTop: 0, tfootTop: 0, tfootBottom: 0, width: 0, left: 0, tfootHeight: 0, theadHeight: 0, windowHeight: c(d).height(), visible: !0 }, headerMode: null, footerMode: null, autoWidth: a.settings()[0].oFeatures.bAutoWidth, namespace: ".dtfc" + m++, scrollLeft: { header: -1, footer: -1 }, enable: !0 }; this.dom = {
            floatingHeader: null, thead: c(a.table().header()), tbody: c(a.table().body()), tfoot: c(a.table().footer()), header: { host: null, floating: null, placeholder: null }, footer: {
                host: null, floating: null,
                placeholder: null
            }
        }; this.dom.header.host = this.dom.thead.parent(); this.dom.footer.host = this.dom.tfoot.parent(); a = a.settings()[0]; if (a._fixedHeader) throw "FixedHeader already initialised on table " + a.nTable.id; a._fixedHeader = this; this._constructor()
    }; c.extend(l.prototype, {
        destroy: function () { this.s.dt.off(".dtfc"); c(d).off(this.s.namespace); this.c.header && this._modeChange("in-place", "header", !0); this.c.footer && this.dom.tfoot.length && this._modeChange("in-place", "footer", !0) }, enable: function (a, b) {
            this.s.enable =
            a; if (b || b === h) this._positions(), this._scroll(!0)
        }, enabled: function () { return this.s.enable }, headerOffset: function (a) { a !== h && (this.c.headerOffset = a, this.update()); return this.c.headerOffset }, footerOffset: function (a) { a !== h && (this.c.footerOffset = a, this.update()); return this.c.footerOffset }, update: function () { var a = this.s.dt.table().node(); c(a).is(":visible") ? this.enable(!0, !1) : this.enable(!1, !1); this._positions(); this._scroll(!0) }, _constructor: function () {
            var a = this, b = this.s.dt; c(d).on("scroll" + this.s.namespace,
            function () { a._scroll() }).on("resize" + this.s.namespace, g.util.throttle(function () { a.s.position.windowHeight = c(d).height(); a.update() }, 50)); var k = c(".fh-fixedHeader"); !this.c.headerOffset && k.length && (this.c.headerOffset = k.outerHeight()); k = c(".fh-fixedFooter"); !this.c.footerOffset && k.length && (this.c.footerOffset = k.outerHeight()); b.on("column-reorder.dt.dtfc column-visibility.dt.dtfc draw.dt.dtfc column-sizing.dt.dtfc responsive-display.dt.dtfc", function () { a.update() }); b.on("destroy.dtfc", function () { a.destroy() });
            this._positions(); this._scroll()
        }, _clone: function (a, b) {
            var k = this.s.dt, e = this.dom[a], f = "header" === a ? this.dom.thead : this.dom.tfoot; !b && e.floating ? e.floating.removeClass("fixedHeader-floating fixedHeader-locked") : (e.floating && (e.placeholder.remove(), this._unsize(a), e.floating.children().detach(), e.floating.remove()), e.floating = c(k.table().node().cloneNode(!1)).css("table-layout", "fixed").attr("aria-hidden", "true").removeAttr("id").append(f).appendTo("body"), e.placeholder = f.clone(!1), e.placeholder.find("*[id]").removeAttr("id"),
            e.host.prepend(e.placeholder), this._matchWidths(e.placeholder, e.floating))
        }, _matchWidths: function (a, b) { var k = function (b) { return c(b, a).map(function () { return c(this).width() }).toArray() }, e = function (a, e) { c(a, b).each(function (a) { c(this).css({ width: e[a], minWidth: e[a] }) }) }, f = k("th"); k = k("td"); e("th", f); e("td", k) }, _unsize: function (a) { var b = this.dom[a].floating; b && ("footer" === a || "header" === a && !this.s.autoWidth) ? c("th, td", b).css({ width: "", minWidth: "" }) : b && "header" === a && c("th, td", b).css("min-width", "") },
        _horizontal: function (a, b) { var c = this.dom[a], e = this.s.position, f = this.s.scrollLeft; c.floating && f[a] !== b && (c.floating.css("left", e.left - b), f[a] = b) }, _modeChange: function (a, b, k) {
            var e = this.dom[b], d = this.s.position, g = function (a) { e.floating.attr("style", function (b, c) { return (c || "") + "width: " + a + "px !important;" }) }, h = this.dom["footer" === b ? "tfoot" : "thead"], l = c.contains(h[0], f.activeElement) ? f.activeElement : null; l && l.blur(); "in-place" === a ? (e.placeholder && (e.placeholder.remove(), e.placeholder = null), this._unsize(b),
            "header" === b ? e.host.prepend(h) : e.host.append(h), e.floating && (e.floating.remove(), e.floating = null)) : "in" === a ? (this._clone(b, k), e.floating.addClass("fixedHeader-floating").css("header" === b ? "top" : "bottom", this.c[b + "Offset"]).css("left", d.left + "px"), g(d.width), "footer" === b && e.floating.css("top", "")) : "below" === a ? (this._clone(b, k), e.floating.addClass("fixedHeader-locked").css("top", d.tfootTop - d.theadHeight).css("left", d.left + "px"), g(d.width)) : "above" === a && (this._clone(b, k), e.floating.addClass("fixedHeader-locked").css("top",
            d.tbodyTop).css("left", d.left + "px"), g(d.width)); l && l !== f.activeElement && setTimeout(function () { l.focus() }, 10); this.s.scrollLeft.header = -1; this.s.scrollLeft.footer = -1; this.s[b + "Mode"] = a
        }, _positions: function () {
            var a = this.s.dt.table(), b = this.s.position, f = this.dom; a = c(a.node()); var e = a.children("thead"), d = a.children("tfoot"); f = f.tbody; b.visible = a.is(":visible"); b.width = a.outerWidth(); b.left = a.offset().left; b.theadTop = e.offset().top; b.tbodyTop = f.offset().top; b.tbodyHeight = f.outerHeight(); b.theadHeight =
            b.tbodyTop - b.theadTop; d.length ? (b.tfootTop = d.offset().top, b.tfootBottom = b.tfootTop + d.outerHeight(), b.tfootHeight = b.tfootBottom - b.tfootTop) : (b.tfootTop = b.tbodyTop + f.outerHeight(), b.tfootBottom = b.tfootTop, b.tfootHeight = b.tfootTop)
        }, _scroll: function (a) {
            var b = c(f).scrollTop(), d = c(f).scrollLeft(), e = this.s.position; if (this.c.header) {
                var g = this.s.enable ? !e.visible || b <= e.theadTop - this.c.headerOffset ? "in-place" : b <= e.tfootTop - e.theadHeight - this.c.headerOffset ? "in" : "below" : "in-place"; (a || g !== this.s.headerMode) &&
                this._modeChange(g, "header", a); this._horizontal("header", d)
            } this.c.footer && this.dom.tfoot.length && (b = this.s.enable ? !e.visible || b + e.windowHeight >= e.tfootBottom + this.c.footerOffset ? "in-place" : e.windowHeight + b > e.tbodyTop + e.tfootHeight + this.c.footerOffset ? "in" : "above" : "in-place", (a || b !== this.s.footerMode) && this._modeChange(b, "footer", a), this._horizontal("footer", d))
        }
    }); l.version = "3.1.7"; l.defaults = { header: !0, footer: !1, headerOffset: 0, footerOffset: 0 }; c.fn.dataTable.FixedHeader = l; c.fn.DataTable.FixedHeader =
    l; c(f).on("init.dt.dtfh", function (a, b, d) { "dt" === a.namespace && (a = b.oInit.fixedHeader, d = g.defaults.fixedHeader, !a && !d || b._fixedHeader || (d = c.extend({}, d, a), !1 !== a && new l(b, d))) }); g.Api.register("fixedHeader()", function () { }); g.Api.register("fixedHeader.adjust()", function () { return this.iterator("table", function (a) { (a = a._fixedHeader) && a.update() }) }); g.Api.register("fixedHeader.enable()", function (a) { return this.iterator("table", function (b) { b = b._fixedHeader; a = a !== h ? a : !0; b && a !== b.enabled() && b.enable(a) }) });
    g.Api.register("fixedHeader.enabled()", function () { if (this.context.length) { var a = this.content[0]._fixedHeader; if (a) return a.enabled() } return !1 }); g.Api.register("fixedHeader.disable()", function () { return this.iterator("table", function (a) { (a = a._fixedHeader) && a.enabled() && a.enable(!1) }) }); c.each(["header", "footer"], function (a, b) {
        g.Api.register("fixedHeader." + b + "Offset()", function (a) {
            var c = this.context; return a === h ? c.length && c[0]._fixedHeader ? c[0]._fixedHeader[b + "Offset"]() : h : this.iterator("table", function (c) {
                if (c =
                c._fixedHeader) c[b + "Offset"](a)
            })
        })
    }); return l
});