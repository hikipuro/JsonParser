using Hikipuro.Text;
using System.Collections.Generic;

namespace JsonParser.Sample.Data {
	class GlossDef {
		public string para = string.Empty;
		public List<string> GlossSeeAlso = null;

		public override string ToString() {
			return ToStringBuilder.Build<GlossDef>(this);
		}
	}
}
