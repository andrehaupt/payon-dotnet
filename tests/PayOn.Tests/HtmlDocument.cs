using CsQuery;

namespace PayOn.Tests {
    public class HtmlDocument {
        private CQ _dom;

        public HtmlDocument(string html) {
            _dom = html;
        }

        public string GetValue(string selector) {
            return _dom[selector].Val();
        }

        public string GetAttribute(string selector, string attribute) {
            return _dom[selector].Attr(attribute);
        }
    }
}