using Hikipuro.Text;

namespace JsonParser.Sample.Data {
	class GlossList {
		public GlossEntry GlossEntry = null;

		public override string ToString() {
			return ToStringBuilder.Build<GlossList>(this);
		}
	}
}
