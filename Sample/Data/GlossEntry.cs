using Hikipuro.Text;

namespace JsonParser.Sample.Data {
	class GlossEntry {
		public string ID = string.Empty;
		public string SortAs = string.Empty;
		public string GlossTerm = string.Empty;
		public string Acronym = string.Empty;
		public string Abbrev = string.Empty;
		public GlossDef GlossDef = null;
		public string GlossSee = string.Empty;

		public override string ToString() {
			return ToStringBuilder.Build<GlossEntry>(this);
		}
	}
}
