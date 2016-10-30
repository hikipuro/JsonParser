using Hikipuro.Text;

namespace JsonParser.Sample.Data {
	class GlossDiv {
		public string title = string.Empty;
		public GlossList GlossList = null;

		public override string ToString() {
			return ToStringBuilder.Build<GlossDiv>(this);
		}
	}
}
